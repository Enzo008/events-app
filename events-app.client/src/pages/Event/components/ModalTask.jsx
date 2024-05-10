import moment from 'moment';
import Notiflix from 'notiflix';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import Modal from 'react-modal';

const ModalTask = ({modalOpen, closeModal, setRefresh, record, event}) => {
    const ini = moment().add(1, 'days').format('YYYY-MM-DD');
    const fin = moment().add(2, 'days').format('YYYY-MM-DD');

    const initEvent = {
        tarNom: '',
        tarDes: '',
        tarFecIniPla: ini,
        tarFecFinPla: fin
    }

    const { 
        register, 
        handleSubmit, 
        setValue,
        reset,
        formState: { errors, dirtyFields, isSubmitted }, 
    } = useForm({ mode: "onChange", defaultValues: initEvent});

    const closeModalAndReset = () => {
        closeModal();
        reset(initEvent);
    }

    useEffect(() => {
        if (modalOpen) {
            if (record) {
                // Si estás editando un registro existente, inicializa el formulario con los datos del registro
                setValue('tarAno', record.tarAno);
                setValue('tarCod', record.tarCod);
                setValue('tarNom', record.tarNom);
                setValue('tarDes', record.tarDes);
                setValue('tarFecIniPla', moment(record.tarFecIniPla).format('YYYY-MM-DD'));
                setValue('tarFecFinPla', moment(record.tarFecFinPla).format('YYYY-MM-DD'));
            } else {
                // Si estás creando un nuevo registro, inicializa el formulario con valores predeterminados
                reset(initEvent);
            }
        }
    }, [modalOpen]);

    const onSubmit = async(data) => {
        data.eveAno = event.eveAno;
        data.eveCod = event.eveCod;
        console.log(data)

        try {
            Notiflix.Loading.pulse();
            const token = localStorage.getItem('token');
            const method = record ? 'PUT' : 'POST';
            console.log(method)
            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Tarea`, {
                method,
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
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

    return (
        <Modal
            isOpen={ modalOpen }
            onRequestClose={ closeModalAndReset }
            style={{content: {
                padding: '2rem',
            }}}
            closeTimeoutMS={ 200 }
            ariaHideApp={false}
            className='modal'
            overlayClassName='modal-fondo'
        >
            <h3 className="center" style={{textTransform: 'capitalize'}}>
                {record ? 'Editar' : 'Nueva'} Tarea
            </h3>
            <form
                className='p1 flex flex-column gap-1 jc-center ai-center'
                onSubmit={handleSubmit(onSubmit)}
            >
                <div>
                    <label htmlFor="tarNom">Nombre:</label>
                    <input
                        type="text" 
                        id="tarNom"
                        className={`input-${dirtyFields.tarNom || isSubmitted ? (errors.tarNom ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={50}
                        placeholder='Ingresa un nombre para el evento'
                        {...register('tarNom', { 
                            required: 'El campo es obligatorio.',
                        })} 
                    />
                    {errors.tarNom ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.tarNom.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="tarDes">Descripcion:</label>
                    <textarea
                        rows="3" cols="50"
                        id="tarDes"
                        className={`input-${dirtyFields.tarDes || isSubmitted ? (errors.tarDes ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={100}
                        placeholder='Ingresa una Descripcion para la tarea'
                        {...register('tarDes', { 
                            required: 'El campo es obligatorio.',
                        })} 
                    />
                    {errors.tarDes ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.tarDes.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div className='Phone_12 flex flex-row gap-1'>
                    <div className='Phone_6 flex flex-column'>
                        <label htmlFor="tarFecIniPla">Fecha Inicio:</label>
                        <input
                            type="date" 
                            id="tarFecIniPla"
                            className={`flex input-${dirtyFields.tarFecIniPla || isSubmitted ? (errors.tarFecIniPla ? 'invalid' : 'valid') : ''}`} 
                            autoComplete='off'
                            {...register('tarFecIniPla', { 
                                required: 'El campo es obligatorio.',
                            })} 
                        />
                        {errors.tarFecIniPla ? (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.tarFecIniPla.message}</p>
                        ) : (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                                Espacio reservado para el mensaje de error
                            </p>
                        )}
                    </div>
                    <div className='Phone_6 flex flex-column'>
                        <label htmlFor="tarFecFinPla">Fecha Fin:</label>
                        <input
                            type="date" 
                            id="tarFecFinPla"
                            className={`flex input-${dirtyFields.tarFecFinPla || isSubmitted ? (errors.tarFecFinPla ? 'invalid' : 'valid') : ''}`} 
                            autoComplete='off'
                            {...register('tarFecFinPla', { 
                                required: 'El campo es obligatorio.',
                            })} 
                        />
                        {errors.tarFecFinPla ? (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.tarFecFinPla.message}</p>
                        ) : (
                            <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                                Espacio reservado para el mensaje de error
                            </p>
                        )}
                    </div>
                </div>
                <button
                    className='button-primary Phone_6 p_25 f1'
                    type='submit'
                >
                    Guardar
                </button>
            </form>
        </Modal>
    )
}

export default ModalTask