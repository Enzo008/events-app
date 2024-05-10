import { useContext, useEffect, useState } from 'react';
import { AuthContext } from '../../../context/AuthContext';
import EventIcon from '../../../icons/EventIcon';
import Notiflix from 'notiflix';
import MenuItem from './MenuItem';
import { Link, NavLink, useMatch, useNavigate } from 'react-router-dom';
import HomeIcon from '../../../icons/HomeIcon';
import masculino from '../../../img/avatar_masculino.svg';
import femenino from '../../../img/avatar_femenino.svg';
import ArrowIcon from '../../../icons/ArrowIcon';

const groupByParent = (menuData) => {
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
    const navigate = useNavigate();
    // Estados del AuthContext
    const { authActions, authInfo } = useContext(AuthContext);
    const { setUserLogged, setMenuData } = authActions;
    const { userLogged, menuData  } = authInfo;
    // Estados local - useState
    const [ menuGroup, setMenuGroup ] = useState([])

    useEffect(() => {
        const fetchMenuData = async () => {
            if (userLogged) {
                Notiflix.Loading.pulse('Cargando...');
                // Storage
                const token = localStorage.getItem('token');
    
                try {
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Menu`, {
                        headers: {
                            'Authorization': `Bearer ${token}`
                        }
                    });
                    if (!response.ok) {
                        const data = await response.json();
                        if (data.result) {
                            setUserLogged(null);
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

    const [isOpen, setIsOpen] = useState(false);

    const toggleActive = (event) => {
        event.stopPropagation();
        setIsOpen(!isOpen);
    };

    useEffect(() => {
        // Función para manejar los clics en el documento
        const handleDocumentClick = (event) => {
            // Comprueba si el clic fue dentro del menú desplegable
            const clickedInsideMenu = event.target.closest('.dropdown_menu');
            if (!clickedInsideMenu) {
                // Si el clic fue fuera del menú desplegable, cierra el menú
                setIsOpen(false);
            }
        };
    
        // Añade el detector de clics al documento
        document.addEventListener('click', handleDocumentClick);
    
        // Limpia el detector de clics cuando el componente se desmonta
        return () => {
            document.removeEventListener('click', handleDocumentClick);
        };
    }, []);

    const CerrarSesion = () => {
        setUserLogged(null)
        localStorage.removeItem('token');
    }
    
    
    return (
        <div 
            className='sidebar-container Medium_2 flex flex-column'
            data-active={isOpen ? 'true' : 'false'}
        >
            <div className='flex ai-center jc-center f2_5 gap-1 p1'>
                <EventIcon /> 
                <h1 className='f1_5'>
                    EVENT GLOW
                </h1>
            </div>
            <div className='sidebar-menus flex-grow-1'>
                {menuGroup.map((menuItem, index) => (
                    <MenuItem key={index} menu={menuItem} level={0} />
                ))}
            </div>
            <div className="sidebar_profile flex ai-center p_5 gap_5">
                <div className="profile_picture">
                    <img 
                        src={userLogged && (userLogged.usuAva ? `data:image/jpeg;base64,${userLogged.usuAva}` : (userLogged.usuSex == 'M' ? masculino : femenino ))} 
                        alt="Descripción de la imagen" 
                    />
                </div>
                <div className="flex flex-column flex-grow-1">
                    <span style={{textTransform: 'capitalize', borderBottom: '1px solid var(--palet-c)', marginBottom: '0.25rem', paddingBottom: '0.25rem'}} className="PowerMas_Username Large-f1 Medium-f1 Small-f_75">{userLogged  ? `${userLogged.usuNom.toLowerCase()} ${userLogged.usuApe.toLowerCase()}` : ''}</span>
                    <span style={{textTransform: 'capitalize'}}  className="PowerMas_UserRole Large-f_75 Medium-f_75 Small-f_5">{userLogged  ? userLogged.rolNom.toLowerCase() : ''}</span>
                </div>
                <span 
                    className="arrow_profile f1_5 flex ai-center jc-center"
                    onClick={toggleActive}
                > 
                    <ArrowIcon />
                </span>
                {
                    isOpen &&
                    <div className='dropdown_menu'>
                        <a href="#">Perfil</a>
                        <hr className='m0' />
                        <Link to='/login' onClick={CerrarSesion}>Cerrar sesión</Link>
                    </div>
                }
            </div>
        </div>
    )
}

export default Sidebar