import { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const PublicRoute = ({ children }) => {
    const { authInfo } = useContext(AuthContext);
    const { userLogged } = authInfo;

    return (
        <>
            {
                userLogged ? <Navigate to='/' /> : children
            }
        </>
    );
};

export default PublicRoute;
