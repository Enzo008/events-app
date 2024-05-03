import { useForm } from 'react-hook-form';
import Notiflix from "notiflix";
import { AuthContext } from '../../context/AuthContext';
import { useContext } from 'react';

const Login = () => {
    // Variables State AuthContext 
    const { authActions } = useContext(AuthContext);
    const { setUserLogged } = authActions;

    // Propiedades Form Principal
    const { 
        register, 
        watch, 
        handleSubmit, 
        formState: { errors, dirtyFields, isSubmitted }, 
        reset, 
        setValue, 
        trigger,
        setFocus
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
            
            setUserLogged(dataRes.user);
        } catch (error) {
            console.error(`Error al hacer la solicitud: ${error}`);
        } finally {
            Notiflix.Loading.remove();
        }
    }

    return (
        <div className="login-container Phone_12 flex flex-row todos">
            <section className='Phone_6'>
                {/* Imagen o algo */}
            </section>
            <section className='Phone_6 flex ai-center jc-center'>
                <form
                    className='Phone_8 p1 flex flex-column gap-1 jc-center ai-center'
                    onSubmit={handleSubmit(onSubmit)}
                >
                    <div>
                        <label htmlFor="usuCorEle" className="">
                            Correo Electrónico:
                        </label>
                        <input 
                            type="text" 
                            id="usuCorEle"
                            className={`input-${dirtyFields.usuCorEle || isSubmitted ? (errors.usuCorEle ? 'invalid' : 'valid') : ''}`} 
                            autoComplete='off'
                            maxLength={50}
                            {...register('usuCorEle', { 
                                required: 'El campo es requerido.',
                            })} 
                        />
                        {errors.usuCorEle ? (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.usuCorEle.message}</p>
                        ) : (
                            <p style={{ visibility: "hidden" }}>
                                Espacio reservado para el mensaje de error
                            </p>
                        )}
                    </div>
                    <div>
                        <label htmlFor="usuPas" className="">
                            Contraseña:
                        </label>
                        <input 
                            type="text" 
                            id="usuPas"
                            className={`input-${dirtyFields.usuPas || isSubmitted ? (errors.usuPas ? 'invalid' : 'valid') : ''}`} 
                            autoComplete='off'
                            maxLength={50}
                            {...register('usuPas', { 
                                required: 'El campo es requerido.',
                            })} 
                        />
                        {errors.usuPas ? (
                            <p className="Large-f_75 Medium-f1 f_75 PowerMas_Message_Invalid">{errors.usuPas.message}</p>
                        ) : (
                            <p className="Large-f_75 Medium-f1 f_75 PowerMas_Message_Invalid" style={{ visibility: "hidden" }}>
                                Espacio reservado para el mensaje de error
                            </p>
                        )}
                    </div>
                    <button
                        className='Phone_6'
                        type='submit'
                    >
                        Ingresar
                    </button>
                </form>
            </section>
        </div>
    )
}

export default Login