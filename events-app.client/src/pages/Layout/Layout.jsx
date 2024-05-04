import Sidebar from './components/Sidebar'

const Layout = ({children}) => {
    return (
        <div className='layout-container h-100 todos flex flex-row'>
            <Sidebar />
            <div className='Medium_10 overflow-auto'>
                {children}
            </div>  
        </div>
    )
}

export default Layout