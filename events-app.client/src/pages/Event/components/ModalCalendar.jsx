import moment from "moment";
import Notiflix from "notiflix";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import Modal from 'react-modal';
import { validateNoLeadingSpaces } from "../../../helpers/formValidation";

moment.locale('es');

const ModalCalendar = ({isModalOpen, closeModal, selectedEvent, setRefresh}) => {
    const [locations, setLocations] = useState([]);

    const now = moment().minutes(0).seconds(0).add(1, 'hours').format('YYYY-MM-DDTHH:mm');
    const nowPlus1 = moment(now).add(1, 'hours').format('YYYY-MM-DDTHH:mm');

    const initEvent = {
        title: '',
        desc: '',
        start: now,
        end: nowPlus1,
        evePrePla: '',
        ubiCod: '0'
    }

    // Propiedades Form Principal
    const { 
        register, 
        handleSubmit, 
        setValue,
        reset,
        formState: { errors, dirtyFields, isSubmitted }, 
    } = useForm({ mode: "onChange", defaultValues: initEvent });

    useEffect(() => {
        if (isModalOpen) {
            const fetchData = async () => {
                try {
                    Notiflix.Loading.pulse('Cargando...');
                    // Valores del storage
                    const token = localStorage.getItem('token');
                    // Obtenemos los datos
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Ubicacion`, {
                        headers: {
                            'Authorization': `Bearer ${token}`
                        }
                    });
                    const data = await response.json();
        
                    if (!response.ok) {
                        Notiflix.Notify.failure(data.message);
                        return;
                    }
        
                    // Guarda las ubicaciones en el estado
                    setLocations(data);
        
                } catch (error) {
                    console.error('Error:', error);
                } finally {
                    Notiflix.Loading.remove();
                }
            };
        
            // Llama a fetchData cuando se monta el componente
            fetchData();
        }
    }, [isModalOpen]); // Dependencias del useEffect

    useEffect(() => {
        if (locations.length > 0 && selectedEvent) {
            for (const key in selectedEvent) {
                if (key === 'start' || key === 'end') {
                    setValue(key, moment(selectedEvent[key]).format('YYYY-MM-DDTHH:mm'));
                } else {
                    setValue(key, selectedEvent[key]);
                }
            }
        } else {
            reset(initEvent);
        }
    }, [locations, selectedEvent, setValue]);
    

    const onSubmit = async(data) => {
        // Formatea los datos del formulario
        const formattedData = {
            eveNom: data.title,
            eveDes: data.desc,
            eveFec: moment(data.start).format('YYYY-MM-DD'),
            eveHor: moment(data.start).format('HH:mm'),
            evePrePla: data.evePrePla,
            ubiCod: data.ubiCod
        };
    
        // Si estás editando un evento, divide el id en eveAno y eveCod
        if (selectedEvent) {
            formattedData.eveAno = data.id.substring(0, 4);
            formattedData.eveCod = data.id.substring(4);
            formattedData.evePreEje = data.evePreEje;
        }
    
        try {
            Notiflix.Loading.pulse();
            const token = localStorage.getItem('token');
            const method = selectedEvent ? 'PUT' : 'POST';
            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Evento`, {
                method,
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formattedData)
            });
        
            const dataRes = await response.json();
    
            if (!response.ok) {
                console.log(dataRes)
                Notiflix.Notify.failure(dataRes.message);
                return;
            }
            
            Notiflix.Notify.success(dataRes.message);
            closeModalAndReset();
            setRefresh(prevRefresh => !prevRefresh);
        } catch (error) {
            console.error(`Error al hacer la solicitud: ${error}`);
        } finally {
            Notiflix.Loading.remove();
        }
    }

    const closeModalAndReset = () => {
        closeModal();
        reset(initEvent);
    }

    return (
        <Modal
            isOpen={ isModalOpen }
            onRequestClose={ closeModalAndReset }
            style={{content: {
                padding: '2rem',
            }}}
            closeTimeoutMS={ 200 }
            ariaHideApp={false}
            className='modal'
            overlayClassName='modal-fondo'
        >
            <h2 className="center" style={{textTransform: 'capitalize'}}>
                {selectedEvent ? 'Editar' : 'Nuevo'} Evento
            </h2>
            <form
                className='p1 flex flex-column gap-1 jc-center ai-center'
                onSubmit={handleSubmit(onSubmit)}
            >
                <div>
                    <label htmlFor="title">Nombre:</label>
                    <input
                        type="text" 
                        id="title"
                        className={`input-${dirtyFields.title || isSubmitted ? (errors.title ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={50}
                        placeholder='Ingresa un nombre para el evento'
                        {...register('title', { 
                            required: 'El campo es obligatorio.',
                            maxLength: {
                                value: 50,
                                message: 'El campo no puede tener más de 50 carácteres.'
                            },
                            minLength: {
                                value: 3,
                                message: 'El campo debe tener minímo 3 carácteres.'
                            },
                            validate: validateNoLeadingSpaces, 
                        })} 
                    />
                    {errors.title ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.title.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="desc">Descripcion:</label>
                    <textarea
                        rows="2" cols="50"
                        id="desc"
                        className={`input-${dirtyFields.desc || isSubmitted ? (errors.desc ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={100}
                        placeholder='Ingresa una descripción para el evento'
                        {...register('desc', { 
                            required: 'El campo es obligatorio.',
                            maxLength: {
                                value: 100,
                                message: 'El campo no puede tener más de 100 carácteres.'
                            },
                            minLength: {
                                value: 3,
                                message: 'El campo debe tener minímo 3 carácteres.'
                            },
                            validate: validateNoLeadingSpaces,
                        })} 
                    />
                    {errors.desc ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.desc.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="start">Fecha y Hora:</label>
                    <input
                        type="datetime-local" 
                        id="start"
                        className={`input-${dirtyFields.start || isSubmitted ? (errors.start ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        {...register('start', { 
                            required: 'El campo es obligatorio.',
                            validate: {
                                isFuture: value => moment(value).isAfter(moment().add(2, 'days')) || 'La fecha del evento debe ser con 2 dóas de anticipación.'
                            }
                        })} 
                    />
                    {errors.start ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.start.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="ubiCod">Ubicaciones Disponibles:</label>
                    <select 
                        id='ubiCod'
                        className={`input-${dirtyFields.ubiCod || isSubmitted ? (errors.ubiCod ? 'invalid' : 'valid') : ''}`} 
                        {...register('ubiCod', { 
                            validate: {
                                required: value => value !== '0' || 'El campo es requerido',
                            }
                        })}
                    >
                        <option value="0">--Seleccione Ubicación--</option>
                        {locations.map((item, index) => (
                            <option 
                                key={index} 
                                value={item.ubiCod}
                            > 
                                {item.ubiNom} - {item.ubiCapPer} PERSONAS
                            </option>
                        ))}
                    </select>
                    {errors.ubiCod ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.ubiCod.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="evePrePla">Presupuesto:</label>
                    <input
                        type="text" 
                        id="evePrePla"
                        className={`input-${dirtyFields.evePrePla || isSubmitted ? (errors.evePrePla ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={10}
                        placeholder='Ingresa tu presupuesto'
                        onInput={(event) => {
                            // Reemplaza cualquier carácter que no sea un número por una cadena vacía
                            event.target.value = event.target.value.replace(/[^0-9]/g, '');
                        }}
                        {...register('evePrePla', { 
                            required: 'El campo es obligatorio.',
                            pattern: {
                                value: /^[0-9]*$/,
                                message: 'El campo solo debe contener números'
                            },
                            maxLength: {
                                value: 10,
                                message: 'El campo no puede tener más de 10 carácteres.'
                            },
                        })} 
                    />
                    {errors.evePrePla ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.evePrePla.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <button
                    className='button-primary Phone_6 p_5 f1_25'
                    type='submit'
                >
                    Guardar
                </button>
            </form>
        </Modal>
    )
}

export default ModalCalendar