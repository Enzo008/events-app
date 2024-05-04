import { useContext, useEffect, useState } from 'react';
import { AuthContext } from '../../../context/AuthContext';
import EventIcon from '../../../icons/EventIcon';
import Notiflix from 'notiflix';
import MenuItem from './MenuItem';
import { NavLink, useMatch } from 'react-router-dom';
import HomeIcon from '../../../icons/HomeIcon';

const groupByParent = (menuData) => {
    console.log(menuData)
    // Primero, creamos un objeto donde las claves son los códigos de los menús (menCod)
    // y los valores son los elementos del menú correspondientes.
    const menuMap = menuData.reduce((map, item) => {
        map[item.menCod] = { ...item, subMenus: [] };
        return map;
    }, {});

    // Luego, recorremos menuData de nuevo para asignar cada menú a su menú padre.
    menuData.forEach((item) => {
        if (item.menCodPad) {
            const parent = menuMap[item.menCodPad];
            if (parent) {
                parent.subMenus.push(menuMap[item.menCod]);
            }
        }
    });

    // Finalmente, filtramos menuData para obtener solo los menús principales.
    const rootMenus = menuData.filter(item => item.menAnoPad === null).map(item => menuMap[item.menCod]);

    return rootMenus;
};

const Sidebar = () => {
    // Estados del AuthContext
    const { authActions, authInfo } = useContext(AuthContext);
    const { setIsLoggedIn, setMenuData } = authActions;
    const { userLogged, menuData  } = authInfo;
    // Estados local - useState
    const [ menuGroup, setMenuGroup ] = useState([])
    

    useEffect(() => {
        const fetchMenuData = async () => {
            if (userLogged) {
                Notiflix.Loading.pulse('Cargando...');
                // Storage
                const token = localStorage.getItem('token');
                const usuAno = userLogged.usuAno;
                const usuCod = userLogged.usuCod;
    
                try {
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Menu`, {
                        headers: {
                            'Authorization': `Bearer ${token}`
                        }
                    });
                    if (!response.ok) {
                        const data = await response.json();
                        if (data.result) {
                            setIsLoggedIn(false);
                        }
            
                        Notiflix.Notify.failure(data.message);
                        Notiflix.Loading.remove();
                        return;
                    }
        
                    const data = await response.json();
                    console.log(data)
                    setMenuData(data);
                } catch (error) {
                    console.error(error);
                    Notiflix.Loading.remove();
                }
            }
            Notiflix.Loading.remove();
        };
    
        fetchMenuData();
    }, [userLogged]);

    useEffect(() => {
        if (menuData.length > 0) {
            const groupData = groupByParent(menuData);
            setMenuGroup(groupData);
        }
    }, [menuData]);
    

    const match = useMatch('/'); // Comprueba si la ruta actual es la página de inicio
    const isHome = match ? 'active_menu' : '';

    return (
        <div className='sidebar-container Medium_2 flex flex-column'>
            <div className='flex ai-center jc-center f2_5 gap-1 p1'>
                <EventIcon /> 
                <h1 className='f1_75'>
                    EVENT GLOW
                </h1>
            </div>
            <div className='sidebar-menus flex-grow-1'>
                <NavLink className={`${isHome} flex ai-center gap-1 p_5`} to='/'>
                    <span className='f1_25 flex ai-center jc-center'>
                        <HomeIcon />
                    </span>
                    Home 
                </NavLink>
                {menuGroup.map((menuItem, index) => (
                    <MenuItem key={index} menu={menuItem} level={0} />
                ))}
            </div>
        </div>
    )
}

export default Sidebar