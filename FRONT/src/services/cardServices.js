const AUTH_URL = 'http://localhost:5000/login';
const CARD_URL = 'http://localhost:5000/cards';

const CREDENTIAL = { login: 'letscode', senha: 'lets@123' };

const DEFAULT_HEADERS = {
    'Accept': 'application/json',
    'Content-Type': 'application/json'
};

const toJson = resp => resp.json();

const useCardService = () => {

    const authenticate = async () => {
        return await fetch(AUTH_URL, {
            method: 'POST',
            mode: 'cors',
            body: JSON.stringify(CREDENTIAL),
            headers: DEFAULT_HEADERS,
        })
            .then(toJson)
            .then(token => `Bearer ${token}`)
            .then(token => ({ Authorization: token }))
            .catch(console.error);
    };

    let authHeader = undefined;

    const getToken = async () => {
        authHeader = await authenticate();
    }

    const getCards = async () => {
        if (!authHeader) await getToken();

        return await fetch(CARD_URL, { headers: authHeader })
            .then(res => {
                if (res.status === 200) return res.json();
                else if (res.status === 401) getToken();
                else throw new Error('unexpected status code');
                return getCards();
            })
            .catch(console.error);
    }

    const updateCard = async (card) => {
        if (!authHeader) await getToken();

        return await fetch(`${CARD_URL}/${card.id}`, {
            headers: { ...authHeader, ...DEFAULT_HEADERS },
            method: 'PUT',
            body: JSON.stringify(card)
        })
            .then(res => {
                if (res.status === 200) return res.json();
                else if (res.status === 401) getToken();
                else throw new Error('unexpected status code');
                return updateCard(card);
            })
            .catch(console.error);
    }

    const addCard = async (card) => {
        if (!authHeader) await getToken();

        return await fetch(`${CARD_URL}`, {
            headers: { ...authHeader, ...DEFAULT_HEADERS },
            method: 'POST',
            body: JSON.stringify(card)
        })
            .then(res => {
                if (res.status === 201) return res.json();
                else if (res.status === 401) getToken();
                else throw new Error('unexpected status code');
                return updateCard(card);
            })
            .catch(console.error);
    }

    const removeCard = async (id) => {
        if (!authHeader) await getToken();

        return await fetch(`${CARD_URL}/${id}`, {
            headers: authHeader,
            method: 'DELETE'
        })
            .then(res => {
                if (res.status === 200) return res.json();
                else if (res.status === 401) getToken();
                else throw new Error('unexpected status code');
                return removeCard(id);
            })
            .catch(console.error);
    }

    return {
        addCard,
        getCards,
        updateCard,
        removeCard
    };
}

export default useCardService;