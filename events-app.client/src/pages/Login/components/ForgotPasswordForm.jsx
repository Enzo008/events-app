import { useContext } from "react";
import { AuthContext } from "../../../context/AuthContext";
import { useForm } from "react-hook-form";
import Notiflix from "notiflix";
import { Link } from "react-router-dom";
import EventIcon from '../../../icons/EventIcon';

const ForgotPasswordForm = () => {
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
        // try {
        //     Notiflix.Loading.pulse();

        //     // Obtén la dirección IP del cliente
        //     const ipResponse = await fetch('https://api.ipify.org?format=json');
        //     const ipData = await ipResponse.json();
        //     data.ip = ipData.ip;

        //     console.log(data)

        //     const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Usuario/login`, {
        //         method: 'POST',
        //         headers: {
        //             'Content-Type': 'application/json'
        //         },
        //         body: JSON.stringify(data)
        //     });
        
        //     const dataRes = await response.json();

        //     if (!response.ok) {
        //         return;
        //     }
            
        //     localStorage.setItem('token', dataRes.result);
        //     setUserLogged(dataRes.user);
        // } catch (error) {
        //     console.error(`Error al hacer la solicitud: ${error}`);
        // } finally {
        //     Notiflix.Loading.remove();
        // }
    }

    return (
        <>
            <span className='flex ai-center jc-center f5 gap-1'>
                <EventIcon /> 
                <h1 className='f3'>
                    RECUPERA TU CUENTA
                </h1>
            </span>
            <form
                className='Phone_8 p2 flex flex-column gap-1 jc-center ai-center'
                onSubmit={handleSubmit(onSubmit)}
            >
                <div>
                    <label htmlFor="usuCorEle">Correo Electrónico:</label>
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
                <div className="login-links flex flex-row jc-space-between">
                    <Link to='/login'>¿Ya tienes una cuenta? Inicia Sesión</Link>
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
        </>
    )
}

export default ForgotPasswordForm