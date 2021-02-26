import { useState } from 'react';
import styled from 'styled-components';
import { FaPlusCircle } from 'react-icons/fa';

const CardContainer = styled.li`
    width:200px;
    padding:10px;
    list-style:none;
    margin:5px;
    background-color:white;
    box-shadow: 0 0 5px rgba(0,0,0,.2);
    height:200px;
`;

const CardHeader = styled.div`
    width:100%;
    display:flex;
    justify-content:space-between;
    border-bottom: 1px solid silver;
`;

const CardButtonsContainer = styled.div`
    display: flex;
    justify-content:center;
    margin-top:10px;
`;

const CardInput = styled.input`
    width:100%;
    text-align:center;
    padding:5px;
`;

const CardEditContent = styled.div`
    width:100%;
    display:flex;
    flex-direction:column;
    align-items:center;
`;

const CardTextArea = styled.textarea`
    width:100%;
    padding:5px;
    min-height: 100px;
`;

const NewCard = ({ addCard }) => {

    const [titulo, setTitulo] = useState('');
    const [conteudo, setConteudo] = useState('');

    const adicionar = () => {
        addCard(titulo, conteudo);
        setTitulo('');
        setConteudo('');
    }

    return (
        <CardContainer>
            <CardHeader>
                <CardInput value={titulo} placeholder="Título" onChange={evt => setTitulo(evt.target.value)} />
            </CardHeader>
            <CardEditContent>
                <CardTextArea value={conteudo} placeholder="Conteúdo" onChange={evt => setConteudo(evt.target.value)}></CardTextArea>
            </CardEditContent>
            <CardButtonsContainer>
                <span style={{ cursor: 'pointer' }} onClick={adicionar}><FaPlusCircle size={20} /></span>
            </CardButtonsContainer>
        </CardContainer>
    )

}

export default NewCard;