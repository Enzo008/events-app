import Notiflix from 'notiflix';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import Modal from 'react-modal';

const formatterBudget = new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 2, 
    maximumFractionDigits: 2
}); 

const ModalSupplier = ({modalOpen, closeModal, setRefresh, record, event }) => {
    const [ suppliers, setSuppliers ] = useState([])
    const [ services, setServices ] = useState([])
    const [ serviceCost, setServiceCost ] = useState('0')

    const initEvent = {
        proCod: '0',
    }

    const { 
        register, 
        watch,
        handleSubmit, 
        setValue,
        reset,
        formState: { errors, dirtyFields, isSubmitted }, 
    } = useForm({ mode: "onChange", defaultValues: initEvent});

    const closeModalAndReset = () => {
        closeModal();
        reset(initEvent);
        setServiceCost('');
    }

    useEffect(() => {
        if (modalOpen) {
            const fetchData = async () => {
                try {
                    Notiflix.Loading.pulse('Cargando...');
                    // Valores del storage
                    const token = localStorage.getItem('token');
                    // Obtenemos los datos
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Proveedor`, {
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
                    setSuppliers(data);
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
        if (suppliers.length > 0) {
            if (record) {
                // Si estás editando un registro existente, inicializa el formulario con los datos del registro
                setValue('proCod', JSON.stringify({ proAno: record.proAno, proCod: record.proCod }))
            } else {
                // Si estás creando un nuevo registro, inicializa el formulario con valores predeterminados
                reset(initEvent);
            }
        }
    }, [suppliers, record, setValue]);

    useEffect(() => {
        const proveedor = watch('proCod');
        if (proveedor && proveedor != '0') {
            console.log(proveedor)
            const { proAno, proCod } = JSON.parse(proveedor);

            const selectedSupplier = suppliers.find(supplier => supplier.proAno === proAno && supplier.proCod === proCod);
            if (selectedSupplier) {
                setServiceCost(selectedSupplier.proPre);
            }

            const fetchData = async () => {
                try {
                    Notiflix.Loading.pulse('Cargando...');
                    // Valores del storage
                    const token = localStorage.getItem('token');
                    // Obtenemos los datos
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Servicio/proveedor/${proAno}/${proCod}`, {
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
                    setServices(data);
                } catch (error) {
                    console.error('Error:', error);
                } finally {
                    Notiflix.Loading.remove();
                }
            };
        
            // Llama a fetchData cuando se monta el componente
            fetchData();
        } else {
            setServices([]);
        }
    }, [watch('proCod')]);

    const onSubmit = async(data) => {
        const { proAno, proCod } = JSON.parse(data.proCod);

        data.eveAno = event.eveAno;
        data.eveCod = event.eveCod;
        data.proAno = proAno;
        data.proCod = proCod;

        console.log(data)
        try {
            Notiflix.Loading.pulse();
            const token = localStorage.getItem('token');
            const method = record ? 'PUT' : 'POST';
            console.log(method)
            const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Proveedor/evento`, {
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
                {record ? 'Editar' : 'Nuevo'} Proveedor
            </h3>
            <form
                className='p1 flex flex-column gap-1 jc-center ai-center'
                onSubmit={handleSubmit(onSubmit)}
            >
                <div>
                    <label htmlFor="proCod">Proveedores Disponibles:</label>
                    <select 
                        id='proCod'
                        className={`input-${dirtyFields.proCod || isSubmitted ? (errors.proCod ? 'invalid' : 'valid') : ''}`} 
                        {...register('proCod', { 
                            validate: {
                                required: value => value !== '0' || 'El campo es requerido',
                            }
                        })}
                    >
                        <option value="0">--Seleccione Proveedor--</option>
                        {suppliers.map((item, index) => (
                            <option 
                                key={index} 
                                value={JSON.stringify({ proAno: item.proAno, proCod: item.proCod })}
                            > 
                                {item.proNom}
                            </option>
                        ))}
                    </select>
                    {errors.proCod ? (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid">{errors.proCod.message}</p>
                    ) : (
                        <p className="Large-f_75 Medium-f1 f_75 message-invalid" style={{ visibility: "hidden" }}>
                            Espacio reservado para el mensaje de error
                        </p>
                    )}
                </div>
                <div className='flex flex-row jc-space-between'>
                    <p>Servicios Asociados:</p>
                    <p className='bold'>Costo: S/. {formatterBudget.format(serviceCost)}</p>
                </div>
                <div className='table-content overflow-auto' style={{maxHeight: '200px'}}>
                    <table className='Large_12 f_75'>
                        <thead>
                            <tr>
                                <th>Nombre</th>
                                <th>Descripción</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                services.length > 0 ?
                                services.map(item => (
                                    <tr key={item.serCod}>
                                        <td className='center'>{item.serNom}</td>
                                        <td>{item.serDes}</td>
                                    </tr>
                                ))
                                :    
                                <tr className=''>
                                    <td colSpan={2} className='Large-p1 center '>
                                        <div className='Phone_12 flex flex-column ai-center jc-center'>
                                            {/* <img src={TableEmpty} alt="TableEmpty" className='Medium_4 Phone_12' /> */}
                                            <p className=''>No se encontraron datos.</p>
                                        </div>
                                    </td>
                                </tr>
                            }
                        </tbody>
                    </table>
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

export default ModalSupplier