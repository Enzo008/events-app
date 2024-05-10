import Sidebar from './components/Sidebar'
import { Tooltip } from 'react-tooltip';

const Layout = ({children}) => {
    return (
        <div className='layout-container h-100 flex flex-row'>
            <Sidebar />
            <div className='Medium_10 overflow-auto flex flex-column'>
                {children}
            </div> 
            <Tooltip 
                    id="error-tooltip"
                    effect="solid"
                    place='bottom-start'
                    className="PowerMas_Tooltip_Info"
            />
            <Tooltip 
                    id="info-tooltip"
                    effect="solid"
                    place='bottom-start'
                    className="PowerMas_Tooltip_Info"
            />
            <Tooltip 
                id="edit-tooltip"
                effect="solid"
                place='top-end'
                className="PowerMas_Tooltip_Info"
            />
            <Tooltip 
                id="delete-tooltip" 
                effect="solid"
                place='top-start'
                className="PowerMas_Tooltip_Info"
            />
            <Tooltip 
                id="select-tooltip" 
                effect="solid"
                place='top-start'
                className="PowerMas_Tooltip_Info"
            />
            <Tooltip 
                id="error-total" 
                effect="solid"
                place='top-start'
                className="PowerMas_Tooltip_Error"
            />
        </div>
    )
}

export default Layout