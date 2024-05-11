import Notiflix from 'notiflix';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import Modal from 'react-modal';

const ModalMaterial = ({modalOpen, closeModal, setRefresh, record }) => {

    const init = {
        matNom: '',
        matDes: '',
        matPre: '',
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
            setValue('matCod', record.matCod);
            setValue('matNom', record.matNom);
            setValue('matDes', record.matDes);
            setValue('matPre', record.matPre);
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
            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Material`, {
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
                {record ? 'Editar' : 'Nuevo'} Material
            </h3>
            <form
                className='p1 flex flex-column gap-1 jc-center ai-center'
                onSubmit={handleSubmit(onSubmit)}
            >
                <div>
                    <label htmlFor="matNom">Nombre:</label>
                    <input
                        id="matNom"
                        className={`input-${dirtyFields.matNom || isSubmitted ? (errors.matNom ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={10}
                        placeholder='Ingresa un Nombre para el material'
                        {...register('matNom', { 
                            required: 'El campo es obligatorio.',
                        })} 
                    />
                    {errors.matNom ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.matNom.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="matDes">Descripcion:</label>
                    <input
                        id="matDes"
                        className={`input-${dirtyFields.matDes || isSubmitted ? (errors.matDes ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={10}
                        placeholder='Ingresa una descripcion para el material'
                        {...register('matDes', { 
                            required: 'El campo es obligatorio.',
                        })} 
                    />
                    {errors.matDes ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.matDes.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div>
                    <label htmlFor="matPre">Precio:</label>
                    <input
                        id="matPre"
                        className={`input-${dirtyFields.matPre || isSubmitted ? (errors.matPre ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={10}
                        onInput={(e) => {
                            e.target.value = e.target.value.replace(/[^0-9]/g, '');
                        }}
                        placeholder='Ingresa un precio para el material'
                        {...register('matPre', { 
                            required: 'El campo es obligatorio.',
                            pattern: {
                                value: /^(?:[1-9]\d*|)$/,
                                message: 'Valor no válido',
                            },
                        })} 
                    />
                    {errors.matPre ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.matPre.message}</p>
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

export default ModalMaterial