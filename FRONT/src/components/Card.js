import { useState } from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { FaEdit, FaTrash, FaChevronCircleLeft, FaChevronCircleRight, FaSave, FaBan } from "react-icons/fa";
import marked from 'marked';
import DOMPurify from 'dompurify';

const DISPLAY_MODE = 'display';
const EDIT_MODE = 'edit';

const parseMarkdown = conteudo => DOMPurify.sanitize(marked(conteudo));


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

const CardTitle = styled.h4`
    flex-grow:1;
    text-align:center;
    padding:5px;
`;


const CardContent = styled.div`
    margin-top:10px;
    margin-bottom:10px;
    min-height: 100px;
`;

const CardButtonsContainer = styled.div`
    display: flex;
    justify-content:space-between;
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

const Card = ({ titulo, conteudo, sendBack, sendForward, update, remove }) => {

    const [mode, setMode] = useState(DISPLAY_MODE);
    const [editedTitle, setEditedTitle] = useState(titulo);
    const [editedContent, setEditedContent] = useState(conteudo);

    const renderForDisplay = (titulo, conteudo) => {
        return (
            <CardContainer>
                <CardHeader>
                    <CardTitle>{titulo}</CardTitle>
                    <span style={{ cursor: 'pointer' }} onClick={() => setMode(EDIT_MODE)}><FaEdit size={20} /></span>
                </CardHeader>
                <CardContent dangerouslySetInnerHTML={{ __html: parseMarkdown(conteudo) }}></CardContent>
                <CardButtonsContainer>
                    <span style={{ cursor: 'pointer' }} onClick={sendBack}><FaChevronCircleLeft size={20} /></span>
                    <span style={{ cursor: 'pointer' }} onClick={remove}><FaTrash size={20} /></span>
                    <span style={{ cursor: 'pointer' }} onClick={sendForward}><FaChevronCircleRight size={20} /></span>
                </CardButtonsContainer>
            </CardContainer>
        );
    };

    const renderForEdition = (titulo, conteudo) => {

        const cancelar = () => {
            setEditedContent(conteudo);
            setEditedTitle(titulo);
            setMode(DISPLAY_MODE);
        }

        const salvar = () => {
            update(editedTitle, editedContent);
            setMode(DISPLAY_MODE);
        }

        return (
            <CardContainer>
                <CardHeader>
                    <CardInput type='text' onChange={evt => setEditedTitle(evt.target.value)} value={editedTitle} />
                </CardHeader>
                <CardEditContent>
                    <CardTextArea onChange={evt => setEditedContent(evt.target.value)} value={editedContent}></CardTextArea>
                </CardEditContent>
                <CardButtonsContainer>
                    <span style={{ cursor: 'pointer' }} onClick={cancelar}><FaBan size={20} /></span>
                    <span style={{ cursor: 'pointer' }} onClick={salvar}><FaSave size={20} /></span>
                </CardButtonsContainer>
            </CardContainer >
        );
    };

    return (
        <>
            {mode === DISPLAY_MODE ? renderForDisplay(titulo, conteudo) : renderForEdition(titulo, conteudo)}
        </>
    );
}

Card.propTypes = {
    titulo: PropTypes.string.isRequired,
    conteudo: PropTypes.string.isRequired,
    update: PropTypes.func.isRequired,
    remove: PropTypes.func.isRequired,
    sendBack: PropTypes.func,
    sendForward: PropTypes.func,
}

Card.defaultProps = {
    sendForward: () => { },
    sendBack: () => { },
}

export default Card;