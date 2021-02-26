import { useState, useEffect } from 'react';
import cardService from '../services/cardServices';
import Lista from './Lista';
import Card from './Card';
import styled from 'styled-components';
import NewCard from './NewCard';

const { getCards, updateCard, removeCard, addCard } = cardService();

const BoardContainer = styled.div`
    display:grid;
    grid-template-columns: 1fr repeat(3, 2fr);
    background-color:#f1f1f1;
    height:100vh;
`;

const Board = () => {

    const [cards, setCards] = useState([]);

    const changeListHandler = (list, id) => async () => {
        const card = cards.find(c => c.id === id);
        if (card) {
            const newCard = { ...card, lista: list };
            const savedCard = await updateCard(newCard);
            const newCards = cards.reduce((res, c) => c.id === savedCard.id ? [...res, savedCard] : [...res, c], []);
            setCards(newCards);
        }
    }

    const updateCardHandler = id => async (titulo, conteudo) => {
        const card = cards.find(c => c.id === id);
        if (!card) return;
        const newCard = { ...card, titulo, conteudo };
        const savedCard = await updateCard(newCard);
        const newCards = cards.reduce((res, c) => c.id === savedCard.id ? [...res, savedCard] : [...res, c], []);
        setCards(newCards);
    }

    const addCardHandler = lista => async (titulo, conteudo) => {
        const newCard = { lista, titulo, conteudo };
        const savedCard = await addCard(newCard);
        setCards([savedCard, ...cards]);
    }

    const removeCardHandler = id => async () => {
        const card = cards.find(c => c.id === id);
        if (!card) return;
        const remainingCards = await removeCard(id);
        setCards(remainingCards);
    }

    useEffect(() => {
        (async () => {
            const cs = await getCards();
            setCards(cs);
        })();
    }, []);

    return (
        <BoardContainer>
            <Lista titulo={"Novo"}>
                {
                    <NewCard addCard={addCardHandler('ToDo')} />
                }
            </Lista>
            <Lista titulo={"To Do"}>
                {
                    cards.filter(c => c.lista === 'ToDo').map(c =>
                        <Card
                            key={c.id}
                            titulo={c.titulo}
                            conteudo={c.conteudo}
                            sendForward={changeListHandler('Doing', c.id)}
                            update={updateCardHandler(c.id)}
                            remove={removeCardHandler(c.id)}
                        />
                    )
                }
            </Lista>
            <Lista titulo={"Doing"}>
                {
                    cards.filter(c => c.lista === 'Doing').map(c =>
                        <Card
                            key={c.id}
                            titulo={c.titulo}
                            conteudo={c.conteudo}
                            sendBack={changeListHandler('ToDo', c.id)}
                            sendForward={changeListHandler('Done', c.id)}
                            update={updateCardHandler(c.id)}
                            remove={removeCardHandler(c.id)}
                        />
                    )
                }
            </Lista>
            <Lista titulo={"Done"}>
                {
                    cards.filter(c => c.lista === 'Done').map(c =>
                        <Card
                            key={c.id}
                            titulo={c.titulo}
                            conteudo={c.conteudo}
                            sendBack={changeListHandler('Doing', c.id)}
                            update={updateCardHandler(c.id)}
                            remove={removeCardHandler(c.id)}
                        />
                    )
                }
            </Lista>
        </BoardContainer>
    )

}

export default Board;