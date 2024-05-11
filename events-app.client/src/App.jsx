import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login/Login';
import PublicRoute from './router/PublicRoute';
import PrivateRoute from './router/PrivateRoute';
import Layout from './pages/Layout/Layout';
import Events from './pages/Event/Events';
import EventDetails from './pages/Event/components/EventDetails';
import Material from './pages/Maintance/Material';
import Supplier from './pages/Maintance/Supplier';
import { AuthContext } from './context/AuthContext';
import { useContext, useEffect, useState } from 'react';
import Notiflix from 'notiflix';
import Service from './pages/Maintance/Service';
import User from './pages/User/User';

const App = () => {
    const { authActions, authInfo } = useContext(AuthContext);
    const { setMenuData } = authActions;
    const { userLogged, menuData  } = authInfo;

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

    const componentMap = {
        'material': Material,
        'supplier': Supplier,
        'service': Service,
        'user': User,
    };

    return (
            <Router>
                <Routes>
                    <Route path='/login' element={
                        <PublicRoute>
                            <Login />
                        </PublicRoute>
                    }/>
                    <Route path='/register' element={
                        <PublicRoute>
                            <Login />
                        </PublicRoute>
                    }/>
                    <Route path='/forgot-password' element={
                        <PublicRoute>
                            <Login />
                        </PublicRoute>
                    }/>
                    <Route path='*' element={
                        // <PrivateRoute>
                        //     {
                        //         <Layout>
                        //             <Routes>
                        //                 <Route index element={<Events />} />
                        //                 <Route path='/event-details/:id' element={<EventDetails />} />
                        //                 <Route path='/material' element={<Material />} />
                        //                 <Route path='/supplier' element={<Supplier />} />
                        //                 <Route path="*" element={<Navigate to='/' />} />
                        //             </Routes>
                        //         </Layout>
                        //     }
                        // </PrivateRoute>
                        <PrivateRoute>
                        {
                            userLogged &&
                            <Layout>
                                    <Routes>
                                        <Route index element={<Events />} />
                                        <Route path='/event-details/:id' element={<EventDetails />} />
                                        {menuData.map((menu, index) => {
                                            const Component = componentMap[menu.menRef];
                                            return Component ? <Route path={`${menu.menRef}`} element={<Component />} key={index} /> : null;
                                        })}
                                        {/* {menuData.some(menu => menu.menRef === 'user') && (
                                            <>
                                                <Route path="form-user/:id?" element={<FormUser />} />
                                                <Route path="menu-user/:id" element={<MenuUser />} />
                                                <Route path="permiso-user/:id" element={<PermissionUser />} />
                                            </>
                                        )} */}
                                        <Route path="*" element={<Navigate to='/' />} />
                                    </Routes>
                            </Layout>
                        }
                    </PrivateRoute>
                    } />
                </Routes>
            </Router>
    )
}

export default App;
