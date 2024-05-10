import Notiflix from 'notiflix';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import Modal from 'react-modal';

const ModalMaterial = ({modalOpen, closeModal, setRefresh, record, event }) => {
    const [ materials, setMaterials ] = useState([])

    const initEvent = {
        matCod: '0',
        eveMatCan: '',
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
            const fetchData = async () => {
                try {
                    Notiflix.Loading.pulse('Cargando...');
                    // Valores del storage
                    const token = localStorage.getItem('token');
                    // Obtenemos los datos
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Material`, {
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
                    setMaterials(data);
                } catch (error) {
                    console.error('Error:', error);
                } finally {
                    Notiflix.Loading.remove();
                }
            };
        
            // Llama a fetchData cuando se monta el componente
            fetchData();
        }
    }, [modalOpen]);

    useEffect(() => {
        if (materials.length > 0) {
            if (record) {
                console.log(record)
                // Si estás editando un registro existente, inicializa el formulario con los datos del registro
                setValue('matCod', record.matCod);
                setValue('eveMatCan', record.eveMatCan);
            } else {
                // Si estás creando un nuevo registro, inicializa el formulario con valores predeterminados
                reset(initEvent);
            }
        }
    }, [materials, record, setValue]);

    const onSubmit = async(data) => {
        console.log(data);
        data.eveAno = event.eveAno;
        data.eveCod = event.eveCod;

        console.log(data)
        try {
            Notiflix.Loading.pulse();
            const token = localStorage.getItem('token');
            const method = record ? 'PUT' : 'POST';
            console.log(method)
            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Material/evento`, {
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
                    <label htmlFor="matCod">{record ? 'Material' : 'Materiales Disponibles'}</label>
                    {
                        record ?
                        <>
                            <input 
                                type="text"
                                defaultValue={record.matNom}
                                disabled={true}
                            />
                        </>
                        :
                        <>
                            <select 
                                id='matCod'
                                className={`input-${dirtyFields.matCod || isSubmitted ? (errors.matCod ? 'invalid' : 'valid') : ''}`} 
                                {...register('matCod', { 
                                    validate: {
                                        required: value => value !== '0' || 'El campo es requerido',
                                    }
                                })}
                                disabled={record}
                            >
                                <option value="0">--Seleccione Material--</option>
                                {materials.map((item, index) => (
                                    <option 
                                        key={index} 
                                        value={item.matCod}
                                    > 
                                        {item.matNom}
                                    </option>
                                ))}
                            </select>
                            {errors.matCod ? (
                                <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.matCod.message}</p>
                            ) : (
                                <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                                    Espacio reservado para el mensaje de error
                                </p>
                            )}
                        </>
                    }
                </div>
                <div>
                    <label htmlFor="tarDes">Cantidad:</label>
                    <input
                        id="eveMatCan"
                        className={`input-${dirtyFields.eveMatCan || isSubmitted ? (errors.eveMatCan ? 'invalid' : 'valid') : ''}`} 
                        autoComplete='off'
                        maxLength={10}
                        placeholder='Ingresa una cantidad para el material'
                        {...register('eveMatCan', { 
                            required: 'El campo es obligatorio.',
                        })} 
                    />
                    {errors.eveMatCan ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.eveMatCan.message}</p>
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