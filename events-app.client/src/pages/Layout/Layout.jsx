import Sidebar from './components/Sidebar'

const Layout = ({children}) => {
    return (
        <div className='layout-container h-100 flex flex-row'>
            <Sidebar />
            <div className='Medium_10 overflow-auto flex'>
                {children}
            </div>  
        </div>
    )
}

export default Layout