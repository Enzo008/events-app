import Notiflix from 'notiflix';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import Modal from 'react-modal';

const ModalEvent = ({modalOpen, closeModal, setRefresh, record }) => {

    const init = {
        eveCod: '0',
    }

    const [events, setEvents] = useState([])

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
        if (modalOpen) {
            const fetchData = async () => {
                try {
                    Notiflix.Loading.pulse('Cargando...');
                    // Valores del storage
                    const token = localStorage.getItem('token');
                    // Obtenemos los datos
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Evento`, {
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
                    console.log(data);
                    setEvents(data);
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


    const onSubmit = async(data) => {
        const { eveAno, eveCod } = JSON.parse(data.eveCod);

        data.usuAno = record.usuAno;
        data.usuCod = record.usuCod;
        data.eveAno = eveAno;
        data.eveCod = eveCod;

        console.log(data)
        try {
            Notiflix.Loading.pulse();
            const token = localStorage.getItem('token');
            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Usuario/evento`, {
                method: 'POST',
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
                Asignar Evento
            </h3>
            <form
                className='p1 flex flex-column gap-1 jc-center ai-center'
                onSubmit={handleSubmit(onSubmit)}
            >
                <div>
                    <label htmlFor="eveCod">Proveedores Disponibles:</label>
                    <select 
                        id='eveCod'
                        className={`input-${dirtyFields.eveCod || isSubmitted ? (errors.eveCod ? 'invalid' : 'valid') : ''}`} 
                        {...register('eveCod', { 
                            validate: {
                                required: value => value !== '0' || 'El campo es requerido',
                            }
                        })}
                    >
                        <option value="0">--Seleccione Proveedor--</option>
                        {events.map((item, index) => (
                            <option 
                                key={index} 
                                value={JSON.stringify({ eveAno: item.eveAno, eveCod: item.eveCod })}
                            > 
                                {item.eveNom} | {item.ubiNom}
                            </option>
                        ))}
                    </select>
                    {errors.eveCod ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.eveCod.message}</p>
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

export default ModalEvent