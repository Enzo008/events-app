import { Calendar, momentLocalizer } from 'react-big-calendar';
import moment from 'moment';
import 'moment/dist/locale/es';
import 'react-big-calendar/lib/css/react-big-calendar.css';
import Notiflix from 'notiflix';
import { useEffect, useState } from 'react';
import ModalCalendar from './ModalCalendar';
import CryptoJS from 'crypto-js';
import { useNavigate } from 'react-router-dom';

moment.locale('es');
const localizer = momentLocalizer(moment);

const CalendarContent = () => {
    const navigate = useNavigate();
    // Estado para el evento seleccionado
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [lastView, setLastView] = useState(localStorage.getItem('lastView') || 'month');
    const [events, setEvents] = useState([]);

    const [isModalOpen, setIsModalOpen] = useState(false)
    const [refresh, setRefresh] = useState(false)
    
    useEffect(() => {
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
                console.log(response)
                const data = await response.json();

                if (!response.ok) {
                    Notiflix.Notify.failure(data.message);
                    return;
                }
                console.log(data)
                const eventosCalendario = data.map(eventoApi => ({
                    id: eventoApi.eveAno + eventoApi.eveCod,
                    start: moment(eventoApi.eveFec + 'T' + eventoApi.eveHor, 'YYYY-MM-DDTHH:mm').toDate(),
                    end: moment(eventoApi.eveFec + 'T' + eventoApi.eveHor, 'YYYY-MM-DDTHH:mm').toDate(),
                    title: eventoApi.eveNom,
                    desc: eventoApi.eveDes,
                    ubiCod: eventoApi.ubiCod,
                    evePrePla: eventoApi.evePrePla,
                    evePreEje: eventoApi.evePreEje,
                }));

                setEvents(eventosCalendario);
            } catch (error) {
                console.error('Error:', error);
            } finally {
                Notiflix.Loading.remove();
            }
        };
        fetchData();
    }, [refresh])
    

    const onDoubleClick = (event) => {
        // Abre el modal cuando se hace doble clic en un evento
        setSelectedEvent(event);
        setIsModalOpen(true);
    }

    const onSelectEvent = (event) => {
        // Actualiza el evento seleccionado cuando se hace clic en un evento
        setSelectedEvent(event);
    }
      
    const onViewChange = (view) => {
        setLastView(view);
        localStorage.setItem('lastView', view);
    }

    const onSelectSlot = (slotInfo) => {
        // Borra la selección cuando se hace clic en un espacio vacío
        setSelectedEvent(null);
    }

    // Asegúrate de definir 'messages' y 'eventStyleGetter'
    const messages = {
        allDay: 'Todo el día',
        previous: '<',
        next: '>',
        today: 'Hoy',
        month: 'Mes',
        week: 'Semana',
        day: 'Día',
        agenda: 'Agenda',
        date: 'Fecha',
        time: 'Hora',
        event: 'Evento',
        noEventsInRange: 'No hay eventos en este rango',
        showMore: total => `+ Ver más (${total})`
    };

    const eventStyleGetter = (event, start, end, isSelected) => {
        // Aquí puedes personalizar el estilo de los eventos
    }

    const closeModal = () => {
        setIsModalOpen(false);
    }

    const handleDelete = () => {
        if (selectedEvent) {

            Notiflix.Confirm.show(
                'Eliminar Evento',
                '¿Está seguro de eliminar este Evento?',
                'Si',
                'No',
                async() => {
                    const formattedData = {
                        eveAno: selectedEvent.id.substring(0, 4),
                        eveCod: selectedEvent.id.substring(4)
                    }
            
                    try {
                        Notiflix.Loading.pulse();
                        const token = localStorage.getItem('token');
                        const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Evento`, {
                            method: 'DELETE',
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
                        
                        setRefresh(prevRefresh => !prevRefresh);
                        setSelectedEvent(null);
                        Notiflix.Notify.success(dataRes.message);
                    } catch (error) {
                        console.error(`Error al hacer la solicitud: ${error}`);
                    } finally {
                        Notiflix.Loading.remove();
                    }
                },
                () => {

                },
            );
        }
    }

    const handleDetails = () => {
        const ciphertext = CryptoJS.AES.encrypt(selectedEvent.id, 'secret key 123').toString();
        // Codifica la cadena cifrada para que pueda ser incluida de manera segura en una URL
        const safeCiphertext = btoa(ciphertext).replace('+', '-').replace('/', '_').replace(/=+$/, '');
        navigate(`/event-details/${safeCiphertext}`);

    }
    
    return (
        <div className='flex flex-grow-1'>
            <Calendar
                localizer={localizer}
                events={events}
                startAccessor="start"
                endAccessor="end"
                messages={messages}
                onDoubleClickEvent={onDoubleClick}
                onSelectEvent={onSelectEvent}
                eventPropGetter={eventStyleGetter} 
                onView={onViewChange}
                onSelectSlot={onSelectSlot}
                selectable={true}
                view={lastView}
                style={{ flexGrow: '1', display: 'flex', height: '100vh'}}
            />
            {
                selectedEvent 
                ?
                <div 
                    className='flex gap-1 p1' 
                    style={{
                        position: 'absolute',
                        bottom: '0',
                        right: '0'
                    }}
                >
                    {/* <button 
                        onClick={handleDelete} 
                        className='button-delete p_5'
                    >
                        Eliminar
                    </button> */}
                    <button
                        onClick={handleDetails}
                        className='button-detail p_5'
                    >
                        Ver Detalles
                    </button>
                </div>
                : 
                <button
                    className='button-add flex m1 p_5'
                    onClick={() => setIsModalOpen(true)} 
                    style={{position: 'absolute', bottom: '0', right: '0'}}
                >
                    Nuevo
                </button>
            }
            <ModalCalendar 
                isModalOpen={isModalOpen}
                closeModal={closeModal}
                selectedEvent={selectedEvent}
                setRefresh={setRefresh}
            />
        </div>
    ) 
}

export default CalendarContent 