import Notiflix from 'notiflix';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import Modal from 'react-modal';

const ModalService = ({modalOpen, closeModal, setRefresh, record }) => {

    const init = {
        serNom: '',
        serDes: '',
    }

    const { 
        register, 
        handleSubmit, 
        setValue,
        reset,
        formState: { errors, dirtyFields, isSubmitted }, 
    } = useForm({ mode: "onChange", defaultValues: init});

    const closeModalAndReset = () => {
        closeModal();
        reset(init);
    }

    useEffect(() => {
        if (record) {
            console.log(record)
            // Si estás editando un registro existente, inicializa el formulario con los datos del registro
            setValue('serCod', record.serCod);
            setValue('serNom', record.serNom);
            setValue('serDes', record.serDes);
        } else {
            // Si estás creando un nuevo registro, inicializa el formulario con valores predeterminados
            reset(init);
        }
    }, [record, setValue]);

    const onSubmit = async(data) => {
        console.log(data);
        try {
            Notiflix.Loading.pulse();
            const token = localStorage.getItem('token');
            const method = record ? 'PUT' : 'POST';
            console.log(method)
            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Servicio`, {
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
                {record ? 'Editar' : 'Nuevo'} Servicio
            </h3>
            <form
                className='p1 flex flex-column gap-1 jc-center ai-center'
                onSubmit={handleSubmit(onSubmit)}
            >
                <div>
                    <label htmlFor="serNom">Nombre:</label>
                    <input
                        id="serNom"
                        className={`input-${dirtyFields.serNom || isSubmitted ? (errors.serNom ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={10}
                        placeholder='Ingresa un Nombre para el material'
                        {...register('serNom', { 
                            required: 'El campo es obligatorio.',
                            minLength: {
                                value: 100,
                                message: 'El campo debe tener minimo 100 caracteres'
                            }
                        })} 
                    />
                    {errors.serNom ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.serNom.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="serDes">Descripcion:</label>
                    <input
                        id="serDes"
                        className={`input-${dirtyFields.serDes || isSubmitted ? (errors.serDes ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={10}
                        placeholder='Ingresa una descripcion para el material'
                        {...register('serDes', { 
                            required: 'El campo es obligatorio.',
                            minLength: {
                                value: 100,
                                message: 'El campo debe tener minimo 100 caracteres'
                            }
                        })} 
                    />
                    {errors.serDes ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.serDes.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
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

export default ModalService