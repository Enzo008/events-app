import { useContext } from "react";
import { AuthContext } from "../../../context/AuthContext";
import { useForm } from "react-hook-form";
import Notiflix from "notiflix";
import { Link } from "react-router-dom";
import EventIcon from '../../../icons/EventIcon';
import { validateNoLeadingSpaces } from "../../../helpers/formValidation";

const RegisterForm = () => {
    // Variables State AuthContext 
    const { authActions } = useContext(AuthContext);
    const { setUserLogged } = authActions;

    // Propiedades Form Principal
    const { 
        register, 
        handleSubmit, 
        getValues,
        formState: { errors, dirtyFields, isSubmitted }, 
    } = useForm({ mode: "onChange" });

    const onSubmit = async(data) => {
        try {
            Notiflix.Loading.pulse();

            // Obtén la dirección IP del cliente
            const ipResponse = await fetch('https://api.ipify.org?format=json');
            const ipData = await ipResponse.json();
            data.usuIp = ipData.ip;

            console.log(data)

            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Usuario`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });
        
            const dataRes = await response.json();

            if (!response.ok) {
                Notiflix.Notify.failure(dataRes.message)
                return;
            }
            
            localStorage.setItem('token', dataRes.result);
            setUserLogged(dataRes.user);
            Notiflix.Notify.success(dataRes.message);
        } catch (error) {
            console.error(`Error al hacer la solicitud: ${error}`);
        } finally {
            Notiflix.Loading.remove();
        }
    }

    return (
        <>
            <span className='flex ai-center jc-center f5 gap-1'>
                <EventIcon /> 
                <h1 className='f3'>
                    REGISTRATE
                </h1>
            </span>
            <form
                className='Phone_8 p2 flex flex-column gap_25 jc-center ai-center'
                onSubmit={handleSubmit(onSubmit)}
            >
                <div>
                    <label htmlFor="usuNom">Nombre:</label>
                    <input
                        type="text" 
                        id="usuNom"
                        className={`input-${dirtyFields.usuNom || isSubmitted ? (errors.usuNom ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={50}
                        placeholder='Ingresa tu Nombre'
                        {...register('usuNom', { 
                            required: 'El campo es requerido',
                            minLength: { value: 3, message: 'El campo debe tener minimo 3 digitos' },
                            pattern: {
                                value: /^[A-Za-zñÑáéíóúÁÉÍÓÚ\s]+$/,
                                message: 'Por favor, introduce solo letras y espacios',
                            },
                            validate: validateNoLeadingSpaces,
                        })} 
                    />
                    {errors.usuNom ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.usuNom.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="usuApe">Apellido:</label>
                    <input
                        type="text" 
                        id="usuApe"
                        className={`input-${dirtyFields.usuApe || isSubmitted ? (errors.usuApe ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={50}
                        placeholder='Ingresa tu Apellido'
                        {...register('usuApe', { 
                            required: 'El campo es requerido',
                            minLength: { value: 3, message: 'El campo debe tener minimo 3 digitos' },
                            pattern: {
                                value: /^[A-Za-zñÑáéíóúÁÉÍÓÚ\s]+$/,
                                message: 'Por favor, introduce solo letras y espacios',
                            },
                            validate: validateNoLeadingSpaces,
                        })} 
                    />
                    {errors.usuApe ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.usuApe.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div className="">
                    <label htmlFor="usuNumDoc">Número de Documento:</label>
                    <input
                        id="usuNumDoc"
                        className={`input-${dirtyFields.usuNumDoc || isSubmitted ? (errors.usuNumDoc ? 'invalid' : 'valid') : ''}`} 
                        type="text" 
                        placeholder="Ingresa tu Número de Documento"
                        maxLength={8}
                        autoComplete='off'
                        onInput={(event) => {
                            // Reemplaza cualquier carácter que no sea un número por una cadena vacía
                            event.target.value = event.target.value.replace(/[^0-9]/g, '');
                        }}
                        {...register('usuNumDoc', { 
                            required: 'El campo es requerido',
                            minLength: {
                                value: 8,
                                message: 'El campo debe tener al menos 8 dígitos'
                            },
                            maxLength: {
                                value: 8,
                                message: 'El campo no debe tener más de 8 dígitos'
                            },
                            pattern: {
                                value: /^[0-9]*$/,
                                message: 'El campo solo debe contener números'
                            }
                        })}
                    />
                    {errors.usuNumDoc ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.usuNumDoc.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                        Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div className="">
                    <label htmlFor="usuTel">Teléfono:</label>
                    <input
                        id="usuTel"
                        className={`input-${dirtyFields.usuTel || isSubmitted ? (errors.usuTel ? 'invalid' : 'valid') : ''}`} 
                        type="text" 
                        placeholder="Ingresa tu Número de Teléfono"
                        maxLength={9}
                        autoComplete='off'
                        onInput={(event) => {
                            // Reemplaza cualquier carácter que no sea un número por una cadena vacía
                            event.target.value = event.target.value.replace(/[^0-9]/g, '');
                        }}
                        {...register('usuTel', { 
                            required: 'El campo es requerido',
                            minLength: {
                                value: 9,
                                message: 'El campo debe tener al menos 9 dígitos'
                            },
                            maxLength: {
                                value: 9,
                                message: 'El campo no debe tener más de 9 dígitos'
                            },
                            pattern: {
                                value: /^[0-9]*$/,
                                message: 'El campo solo debe contener números'
                            }
                        })}
                    />
                    {errors.usuTel ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.usuTel.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                        Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div className="Large_12 flex flex-row gap-1 ai-center" style={{padding: '0.25rem 0'}}>
                    <label htmlFor="">Sexo: </label>
                    <div className="flex gap-1">
                        <div className="flex gap_5">
                            <input 
                                type="radio" 
                                id="masculino" 
                                name="usuSex" 
                                value="M"
                                {...register('usuSex', { required: 'Por favor, selecciona una opción' })}
                            />
                            <label htmlFor="masculino">Masculino</label>
                        </div>
                        <div className="flex gap_5">
                            <input 
                                type="radio" 
                                id="femenino" 
                                name="usuSex" 
                                value="F" 
                                {...register('usuSex', { required: 'Por favor, selecciona una opción' })}
                            />
                            <label htmlFor="femenino">Femenino</label>
                        </div>
                    </div>
                    {errors.usuSex ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.usuSex.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
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
                <div>
                    <label htmlFor="usuPas">Contraseña:</label>
                    <input 
                        type="password" 
                        id="usuPas"
                        className={`input-${dirtyFields.usuPas || isSubmitted ? (errors.usuPas ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={50}
                        placeholder='Ingresa tu Contraseña'
                        {...register('usuPas', { 
                            required: 'El campo es requerido',
                            maxLength: { value: 50, message: 'El campo no puede tener más de 50 caracteres' },
                            minLength:  { value: 8, message: 'El campo no puede tener menos de 8 caracteres' },
                            pattern: {
                                value: /^[A-Za-z0-9ñÑ\s@#.$%^&*()_+\-=\[\]{};':"\\|,<>\/?]+$/,
                                message: 'Por favor, introduce solo letras, números y caracteres especiales permitidos',
                            },
                            validate: validateNoLeadingSpaces,
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
                <div>
                    <label htmlFor="confirmPas">Confirmar Contraseña:</label>
                    <input 
                        type="password" 
                        id="confirmPas"
                        className={`input-${dirtyFields.confirmPas || isSubmitted ? (errors.confirmPas ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={50}
                        placeholder='Confirma tu Contraseña'
                        {...register('confirmPas', { 
                            required: 'El campo es requerido',
                            validate: {
                                noLeadingSpaces: validateNoLeadingSpaces,
                                matchesPassword: (value) => {
                                    const { usuPas } = getValues();
                                    return usuPas === value || "Las contraseñas deben ser iguales";
                                }
                            },
                        })} 
                    />
                    {errors.confirmPas ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.confirmPas.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>

                <div className="login-links flex flex-row jc-space-between">
                    <Link to='/forgot-password'>¿Olvidaste tu contraseña?</Link>
                    <Link to='/login'>¿Ya tienes una cuenta? Inicia Sesión</Link>
                </div>
                <br />
                <button
                    className='Phone_6 p_5 bold f1_25'
                    type='submit'
                >
                    Registrarme
                </button>
            </form>
        </>
    )
}

export default RegisterForm