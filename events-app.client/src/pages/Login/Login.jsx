import { useLocation } from 'react-router-dom';
import portada from '../../img/portada-login2.webp';
import LoginForm from "./components/LoginForm";
import RegisterForm from './components/RegisterForm';
import ForgotPasswordForm from './components/ForgotPasswordForm';

const Login = () => {
    const location = useLocation();
    let form;

    switch (location.pathname) {
        case '/register':
            form = <RegisterForm />;
            break;
        case '/forgot-password':
            form = <ForgotPasswordForm />;
            break;
        case '/login':
        default:
            form = <LoginForm />;
            break;
    }

    return (
        <div className="login-container h-100 Phone_12 flex flex-row">
            <section className='portada-login Phone_6'>
                <img src={portada} alt="Portada Login" />
            </section>
            <section className='Phone_6 flex ai-center jc-center flex-column'>
                {form}
            </section>
        </div>
    )
}

export default Login