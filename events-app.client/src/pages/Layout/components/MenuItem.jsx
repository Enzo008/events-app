import { useState } from "react";
import { NavLink, useMatch } from "react-router-dom";
import EventsIcon from "../../../icons/EventsIcon";
import SquareIcon from "../../../icons/SquareIcon";
import ArrowIcon from "../../../icons/ArrowIcon";
import UserIcon from "../../../icons/UserIcon";
import ToolsIcon from "../../../icons/ToolsIcon";

const componentMap = {
    'EventsIcon': EventsIcon,
    'SquareIcon': SquareIcon,
    'UserIcon': UserIcon,
    'ToolsIcon': ToolsIcon,
};

const MenuItem = ({ menu, level }) => {
    const Icono  = componentMap[menu.menIco.trim()];
    
    const match = useMatch(menu.menRef === '#' ? '#' : `/${menu.menRef}`);
    const isActive = match ? 'active_menu' : '';
    const [isOpen, setIsOpen] = useState(false);

    const toggleActive = (event) => {
        event.stopPropagation();
        setIsOpen(!isOpen);
    };
    
    return (
        <div
            style={{borderBottom: '1px solid var(--palet-c)'}}
            onClick={toggleActive} 
            data-active={isOpen ? 'true' : 'false'} // Añade un atributo de datos personalizado
            key={menu.menCod}
        >
            <div className="flex ai-center">
                <NavLink
                    className={`menu-item flex ai-center flex-grow-1 gap-1 p_5 ${isActive}`} 
                    to={menu.menRef === '#' ? '#' : `${menu.menRef}`}
                >
                    <span className='flex ai-center jc-center'
                        style={{fontSize: `${level == 0 ? '1.25rem': '1rem'}`}}
                    >
                        {Icono && <Icono />}
                    </span>
                    <span className={`Medium-f_75 Small-f_75`} style={{fontSize: `${level == 0 ? '1rem': '14px'}`}}> {menu.menNom} </span>
                </NavLink>
                {menu.subMenus.length > 0 && <span className="arrow f1_5 flex ai-center jc-center"> 
                    <ArrowIcon />
                </span>}
            </div>
            <div className="sub-menu">
                {menu.subMenus.map((subMenu, index) => (
                    <MenuItem key={index} menu={subMenu} level={level + 1} />
                ))}
            </div>
        </div>
    );
};
export default MenuItem;
