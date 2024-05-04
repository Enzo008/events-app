import { useEffect, useState } from "react";
import { AuthContext } from "./AuthContext";

const AuthState = ({ children }) => {
    // Estados
    const [ userLogged, setUserLogged ] = useState(null)
    const [ userPermissions, setUserPermissions ] = useState([])
    const [ cargando, setCargando ] = useState(true)
    const [ isLoading, setIsLoading ] = useState(true);
    const [ menuData, setMenuData ] = useState([]);

    useEffect(() => {
        validarUsuario().finally(() => setIsLoading(false));
    }, []);


    const validarUsuario = async () => {
        try {
            const token = localStorage.getItem('token');
            if(!token) return;
            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Usuario/perfil`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });
            if (!response.ok) {
                    const data = await response.json();
                    setUserLogged(null)
                    localStorage.removeItem('token');
                    return;
            }

            const data = await response.json();
            console.log(data)
            setUserLogged(data.result)
        } catch (error) {
            console.error(`Error al hacer la solicitud: ${error}`);
        } finally {
            setCargando(false);
        }
    };

    // const verificarPermisos = async () => {
    //     const token = localStorage.getItem('token');
    //     if(!token) return;
    //     // Obtenemos los permisos del usuario
    //     const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Permiso/${userLogged.usuAno}/${userLogged.usuCod}`, {
    //         headers: {
    //             'Authorization': `Bearer ${token}`
    //         }
    //     });
    //     const userPermissions = await response.json();
    //     setUserPermissions(userPermissions);
    // }

    // useEffect( () => {
    //     validarUsuario();
    // }, [userLogged]);
    
    // useEffect( () => {
    //     if (Object.keys(userLogged).length !== 0) {
    //         verificarPermisos();
    //     }
    // }, [userLogged]);
    

    // Agrupar en un solo objeto
    const authInfo = {
        userLogged,
        userPermissions,
        cargando,
        isLoading,
        menuData
    };

    const authActions = {
        setUserLogged,
        setUserPermissions,
        validarUsuario,
        setMenuData
    };

    return (
        <AuthContext.Provider value={{ authInfo, authActions }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthState;
