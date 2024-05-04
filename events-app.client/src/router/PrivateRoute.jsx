import { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

const PrivateRoute = ({ children }) => {
    const { authInfo } = useContext(AuthContext);
    const { userLogged, cargando } = authInfo;

    if (cargando) return <p> Cargando... </p>;

    return (
        <>
            {
                userLogged ? children : <Navigate to='/login' />
            }
        </>
    );
};

export default PrivateRoute;
