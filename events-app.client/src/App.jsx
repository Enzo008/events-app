import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import AuthState from './context/AuthState';
import Login from './pages/Login/Login';
import PublicRoute from './router/PublicRoute';
import PrivateRoute from './router/PrivateRoute';
import Home from './pages/Home/Home';
import Layout from './pages/Layout/Layout';
import EventForm from './pages/Event/EventForm';

const App = () => {
    return (
        <AuthState>
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
                    <Route path='*' element={
                        <PrivateRoute>
                            {
                                <Layout>
                                    <Routes>
                                        <Route index element={<Home />} />
                                        <Route path='/event' element={<EventForm />} />
                                        <Route path='/event-form/:id?' element={<EventForm />} />
                                        <Route path="*" element={<Navigate to='/' />} />
                                    </Routes>
                                </Layout>
                            }
                        </PrivateRoute>
                    } />
                </Routes>
            </Router>
        </AuthState>
    )
}

export default App;
