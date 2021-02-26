import PropTypes from 'prop-types';
import styled from 'styled-components';

const ListaContainer = styled.div`
    padding:10px;
    border-right:1px solid silver;
    display:flex;
    flex-direction:column;
    align-items:center;
    width:100%
`;

const ListaUL = styled.ul`
    width:100%;
    display:flex;
    justify-content:space-evenly;
    flex-wrap:wrap;
`;

const ListaTitle = styled.h3`
    padding:10px;
    border-bottom: 1px solid silver;
    width:100%;
    text-align:center;
    margin-bottom:5px;
`;

const Lista = ({ titulo, children }) => {

    return (
        <ListaContainer>
            <ListaTitle>{titulo}</ListaTitle>
            <ListaUL>
                {children}
            </ListaUL>
        </ListaContainer>
    )

}

Lista.propTypes = {
    titulo: PropTypes.string.isRequired,
};

export default Lista;