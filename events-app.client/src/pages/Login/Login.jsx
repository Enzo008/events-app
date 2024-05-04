import { Link } from "react-router-dom";
import { useForm } from 'react-hook-form';
import Notiflix from "notiflix";
import { AuthContext } from '../../context/AuthContext';
import { useContext } from 'react';
import portada from '../../img/portada-login2.webp';
import EventIcon from '../../icons/EventIcon';

const Login = () => {
    // Variables State AuthContext 
    const { authActions } = useContext(AuthContext);
    const { setUserLogged } = authActions;

    // Propiedades Form Principal
    const { 
        register, 
        handleSubmit, 
        formState: { errors, dirtyFields, isSubmitted }, 
    } = useForm({ mode: "onChange" });

    const onSubmit = async(data) => {
        console.log(data)
        try {
            Notiflix.Loading.pulse();

            // Obtén la dirección IP del cliente
            const ipResponse = await fetch('https://api.ipify.org?format=json');
            const ipData = await ipResponse.json();
            data.ip = ipData.ip;

            console.log(data)

            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Usuario/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
        
            const dataRes = await response.json();

            if (!response.ok) {
                return;
            }
            
            localStorage.setItem('token', dataRes.result);
            setUserLogged(dataRes.user);
        } catch (error) {
            console.error(`Error al hacer la solicitud: ${error}`);
        } finally {
            Notiflix.Loading.remove();
        }
    }

    return (
        <div className="login-container h-100 Phone_12 flex flex-row">
            <section className='portada-login Phone_6'>
                <img src={portada} alt="Portada Login" />
            </section>
            <section className='Phone_6 flex ai-center jc-center flex-column'>
                <span className='flex ai-center jc-center f5 gap-1'>
                    <EventIcon /> 
                    <h1 className='f3'>
                        EVENT GLOW
                    </h1>
                </span>
                <form
                    className='Phone_8 p2 flex flex-column gap-1 jc-center ai-center'
                    onSubmit={handleSubmit(onSubmit)}
                >
                    <div>
                        <input
                            type="text" 
                            id="usuCorEle"
                            className={`input-${dirtyFields.usuCorEle || isSubmitted ? (errors.usuCorEle ? 'invalid' : 'valid') : ''}`} 
                            autoComplete='off'
                            maxLength={50}
                            placeholder='Ingresa tu Correo Electrónico'
                            {...register('usuCorEle', { 
                                required: 'El campo es obligatorio.',
                                pattern: {
                                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i,
                                    message: 'Dirección de correo electrónico inválido.',
                                },
                            })} 
                        />
                        {errors.usuCorEle ? (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.usuCorEle.message}</p>
                        ) : (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                                Espacio reservado para el mensaje de error
                            </p>
                        )}
                    </div>
                    <div>
                        <input 
                            type="password" 
                            id="usuPas"
                            className={`input-${dirtyFields.usuPas || isSubmitted ? (errors.usuPas ? 'invalid' : 'valid') : ''}`} 
                            autoComplete='off'
                            maxLength={50}
                            placeholder='Ingresa tu Contraseña'
                            {...register('usuPas', { 
                                required: 'El campo es obligatorio.',
                            })} 
                        />
                        {errors.usuPas ? (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.usuPas.message}</p>
                        ) : (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                                Espacio reservado para el mensaje de error
                            </p>
                        )}
                    </div>
                    <div className="login-links flex flex-row jc-space-between">
                        <Link to='/register'>¿Olvidaste tu contraseña?</Link>
                        <Link to='/register'>¿No tienes una cuenta? Registrate</Link>
                    </div>
                    <br />
                    <button
                        className='Phone_6 p_5 bold f1_25'
                        type='submit'
                    >
                        Iniciar Sesión
                    </button>
                </form>
            </section>
        </div>
    )
}

export default Login