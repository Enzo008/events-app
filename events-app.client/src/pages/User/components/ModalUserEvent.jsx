import Notiflix from 'notiflix';
import { useEffect, useState } from 'react';
import Modal from 'react-modal';
import ModalEvent from './ModalEvent';

const ModalUserEvent = ({modalOpen, closeModal, record }) => {
    console.log(record)
    const [ events, setEvents ] = useState([])

    const [ dataSelected, setDataSelected ] = useState(null)
    const [ modalTablaOpen, setModalTablaOpen ] = useState(false)
    const [ refresh, setRefresh ] = useState(false)
    const openModal = (record) => {
        setDataSelected(record);
        setModalTablaOpen(true);
    };
    const closeModalTabla = () => {
        setModalTablaOpen(false);
        setDataSelected(null);
    }

    useEffect(() => {
        if (modalOpen && record) {
            const {usuAno,usuCod} = record;
            const fetchData = async () => {
                try {
                    // Valores del storage
                    const token = localStorage.getItem('token');
                    // Obtenemos los datos
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Usuario/evento/${usuAno}/${usuCod}`, {
                        headers: {
                            'Authorization': `Bearer ${token}`
                        }
                    });
                    console.log(response)
                    if (!response.ok) {
                        Notiflix.Notify.failure("Ocurrió un error inesperado");
                        return navigate('/error');
                    }

                    const data = await response.json();
                    console.log(data);
                    setEvents(data);
                } catch (error) {
                    console.error('Error:', error);
                } finally {
                }
            };
            
            fetchData();
        }
    }, [modalOpen, refresh])
    

    return (
        <Modal
            isOpen={ modalOpen }
            onRequestClose={ closeModal }
            style={{content: {
                padding: '2rem',
                width: '50%',
                height: '50%',
                display: 'flex',
                flexDirection: 'column'
            }}}
            closeTimeoutMS={ 200 }
            ariaHideApp={false}
            className='modal'
            overlayClassName='modal-fondo'
        >
            <h4 className='center'>EVENTOS PARA:</h4>
            <h4 className='center'>{record && record.usuNom} {record && record.usuApe}</h4>
            <div className='p1 center'>
                <button
                    className='button-primary Phone_6 p_25 f1 '
                    onClick={() => openModal(record)}
                >
                    Agregar
                </button>
            </div>
            <div className='table-content overflow-auto flex-grow-1' style={{maxHeight: '200px'}}>
                <table className='Large_12 f_75'>
                    <thead>
                        <tr>
                            <th>Nombre</th>
                            <th>Ubicación</th>
                            <th>Fecha</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            events.length > 0 ?
                            events.map((item, index) => (
                                <tr key={index}>
                                    <td className='center'>{item.eveNom}</td>
                                    <td>{item.ubiNom}</td>
                                    <td>{item.eveFec}</td>
                                </tr>
                            ))
                            :    
                            <tr className=''>
                                <td colSpan={3} className='Large-p1 center '>
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
            <ModalEvent 
                modalOpen={modalTablaOpen}
                closeModal={() => closeModalTabla() }
                setRefresh={setRefresh}
                record={dataSelected}
            />
        </Modal>
    )
}

export default ModalUserEvent