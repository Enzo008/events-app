import { useNavigate, useParams } from "react-router-dom";
import CryptoJS from 'crypto-js';
import { useContext, useEffect, useMemo, useState } from "react";
import Notiflix from "notiflix";
import moment from "moment";
import SearchInput from "./SearchInput";
import PlusIcon from "../../../icons/PlusIcon";
import { getMaterialColumns, getSupplierColumns, getTaskColumns } from "./columns";
import Table from "./Table";
import ModalTask from "./ModalTask";
import ModalMaterial from "./ModalMaterial";
import ModalSupplier from "./ModalSupplier";
import { AuthContext } from "../../../context/AuthContext";
import ExcelIcon from "../../../icons/ExcelIcon";
import { Export_Excel } from "../../../helpers/export";

const formatterBudget = new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 2, 
    maximumFractionDigits: 2
}); 

const EventDetails = () => {
    const { authInfo } = useContext(AuthContext);
    const { userLogged  } = authInfo;

    console.log(userLogged)

    const navigate = useNavigate();
    let id = '';
    const { id: safeCiphertext } = useParams();
    if (safeCiphertext) {
        try {
            const ciphertext = atob(safeCiphertext);
            // Desencripta el ID
            const bytes  = CryptoJS.AES.decrypt(ciphertext, 'secret key 123');
            id = bytes.toString(CryptoJS.enc.Utf8);
        } catch (error) {
            Notiflix.Notify.failure('Ocurrió un error.')
            // Aquí puedes manejar el error como mejor te parezca
            // Por ejemplo, podrías redirigir al usuario a una página de error
            return navigate('/error');
        }
    }

    const [supplierCost, setSupplierCost] = useState(0);
    const [materialCost, setMaterialCost] = useState(0);


    const [ taskSelected, setTaskSelected ] = useState(null)
    const [ modalTask, setModalTask ] = useState(false)
    const [ refreshTask, setRefreshTask ] = useState(false)
    const openModalTask = (record) => {
        setTaskSelected(record);
        setModalTask(true);
    };

    const [ materialSelected, setMaterialSelected ] = useState(null)
    const [ modalMaterial, setModalMaterial ] = useState(false)
    const [ refreshMaterial, setRefreshMaterial ] = useState(false)
    const openModalMaterial = (record) => {
        setMaterialSelected(record);
        setModalMaterial(true);
    };

    const [ supplierSelected, setSupplierSelected ] = useState(null)
    const [ modalSupplier, setModalSupplier ] = useState(false)
    const [ refreshSupplier, setRefreshSupplier ] = useState(false)
    const openModalSupplier = (record) => {
        setSupplierSelected(record);
        setModalSupplier(true);
    };


    const closeModal = (setModal, setRecord) => {
        setModal(false);
        setRecord(null);
    }

    const supplierColumns = useMemo(() => getSupplierColumns(openModalSupplier, setRefreshSupplier, userLogged), [openModalSupplier, setRefreshSupplier, userLogged]);
    const materialColumns = useMemo(() => getMaterialColumns(openModalMaterial, setRefreshMaterial, userLogged), [openModalMaterial, setRefreshMaterial, userLogged]);
    const taskColumns = useMemo(() => getTaskColumns(openModalTask, setRefreshTask, userLogged), [openModalTask, setRefreshTask, userLogged]);

    const [ event, setEvent ] = useState(null)
    const [ tasks, setTasks ] = useState([])
    const [ materials, setMaterials ] = useState([])
    const [ suppliers, setSuppliers ] = useState([])

    useEffect(() => {
        if (id.length === 10) {
            const eveAno = id.slice(0, 4);
            const eveCod = id.slice(4);

            const fetchData = async () => {
                try {
                    if (document.querySelector('.block-event')) {
                        Notiflix.Block.pulse('.block-event', {
                            svgSize: '100px',
                            svgColor: '#F87C56',
                        });
                    }
                    // Valores del storage
                    const token = localStorage.getItem('token');
                    // Obtenemos los datos
                    const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Evento/${eveAno}/${eveCod}`, {
                        headers: {
                            'Authorization': `Bearer ${token}`
                        }
                    });
                    console.log(response)
                    if (!response.ok) {
                        Notiflix.Notify.failure("Ocurrió un error inesperado");
                        return navigate('/error');
                    }

                    if (response.status === 204) {
                        Notiflix.Notify.failure('Evento no encontrado.');
                        return navigate('/error');
                    }

                    const data = await response.json();
                    setEvent(data);
                } catch (error) {
                    console.error('Error:', error);
                } finally {
                    Notiflix.Block.remove('.block-event');
                }
            };
            
            fetchData();
            
        } else {
            navigate('/error');
            Notiflix.Notify.failure('Ocurrió un error al descifrar la url.')
        }
    }, [id])
    
    useEffect(() => {
        const eveAno = id.slice(0, 4);
        const eveCod = id.slice(4);
        const fetchDataTasks = async () => {
            try {
                
                if (document.querySelector('.block-tareas')) {
                    Notiflix.Block.pulse('.block-tareas', {
                        svgSize: '100px',
                        svgColor: '#F87C56',
                    });
                }
                
                // Valores del storage
                const token = localStorage.getItem('token');
                // Obtenemos los datos
                const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Tarea/${eveAno}/${eveCod}`, {
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
                setTasks(data);
                console.log(data);
            } catch (error) {
                console.error('Error:', error);
            } finally {
                Notiflix.Block.remove('.block-tareas');
            }
        };
        fetchDataTasks();
    }, [refreshTask])

    useEffect(() => {
        const eveAno = id.slice(0, 4);
        const eveCod = id.slice(4);
        const fetchDataSupplier = async () => {
            try {
                if (document.querySelector('.block-supplier')) {
                    Notiflix.Block.pulse('.block-supplier', {
                        svgSize: '100px',
                        svgColor: '#F87C56',
                    });
                }
                // Valores del storage
                const token = localStorage.getItem('token');
                // Obtenemos los datos
                const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Proveedor/evento/${eveAno}/${eveCod}`, {
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
                setSuppliers(data);

                // Calcular la suma de los costos de los servicios de los proveedores
                const supplierCostSum = data.reduce((sum, supplier) => sum + Number(supplier.proSerCos), 0);

                // Actualizar el costo de los proveedores
                setSupplierCost(supplierCostSum);
            } catch (error) {
                console.error('Error:', error);
            } finally {
                Notiflix.Block.remove('.block-supplier');
            }
        };
        fetchDataSupplier();
    }, [refreshSupplier])

    useEffect(() => {
        const eveAno = id.slice(0, 4);
        const eveCod = id.slice(4);
        const fetchDataMaterial = async () => {
            try {
                if (document.querySelector('.block-material')) {
                    Notiflix.Block.pulse('.block-material', {
                        svgSize: '100px',
                        svgColor: '#F87C56',
                    });
                }
                // Valores del storage
                const token = localStorage.getItem('token');
                // Obtenemos los datos
                const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Material/evento/${eveAno}/${eveCod}`, {
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
                setMaterials(data);

                // Calcular la suma de los costos de los materiales
                const materialCostSum = data.reduce((sum, material) => sum + (Number(material.matPre) * Number(material.eveMatCan)), 0);

                // Actualizar el costo de los materiales
                setMaterialCost(materialCostSum);
            } catch (error) {
                console.error('Error:', error);
            } finally {
                Notiflix.Block.remove('.block-material');
            }
        };
        fetchDataMaterial();
    }, [refreshMaterial])
    

    const [searchFilterSupplier, setSearchFilterSupplier] = useState('');
    const filteredDataSupplier = useMemo(() => 
        suppliers.filter(item => 
            (item.proNom ? item.proNom.toUpperCase().includes(searchFilterSupplier.toUpperCase()) : false) ||
            (item.serCan ? item.serCan.toUpperCase().includes(searchFilterSupplier.toUpperCase()) : false) ||
            (item.proSerCos ? item.proSerCos.toUpperCase().includes(searchFilterSupplier.toUpperCase()) : false) ||
            (item.proApe ? item.proApe.toUpperCase().includes(searchFilterSupplier.toUpperCase()) : false)
        ), [suppliers, searchFilterSupplier]
    );
    const headersSupplier = ['NOMBRE','APELLIDO','COSTO','SERVICIOS'];
    const propertiesSuppler = ['proNom','proApe','proSerCos','serCan'];

    const [searchFilter, setSearchFilter] = useState('');
    const filteredData = useMemo(() => 
        tasks.filter(item => 
            (item.tarNom ? item.tarNom.toUpperCase().includes(searchFilter.toUpperCase()) : false) ||
            (item.tarFecIniPla ? item.tarFecIniPla.toUpperCase().includes(searchFilter.toUpperCase()) : false) ||
            (item.tarFecFinPla ? item.tarFecFinPla.toUpperCase().includes(searchFilter.toUpperCase()) : false) ||
            (item.tarDes ? item.tarDes.toUpperCase().includes(searchFilter.toUpperCase()) : false)
        ), [tasks, searchFilter]
    );
    const headersTask = ['NOMBRE','DESCRIPCION','FECHA INICIO','FECHA FIN','RESPONSABLE'];
    const propertiesTask = ['tarNom','tarDes','tarFecIniPla','tarFecFinPla','tarRes'];

    const [searchFilterMaterial, setSearchFilterMaterial] = useState('');
    const filteredDataMaterial = useMemo(() => 
        materials.filter(item => 
            (item.matNom ? item.matNom.toUpperCase().includes(searchFilterMaterial.toUpperCase()) : false) ||
            (item.matPre ? item.matPre.toUpperCase().includes(searchFilterMaterial.toUpperCase()) : false) ||
            (item.eveMatCan ? item.eveMatCan.toUpperCase().includes(searchFilterMaterial.toUpperCase()) : false) ||
            (item.matDes ? item.matDes.toUpperCase().includes(searchFilterMaterial.toUpperCase()) : false)
        ), [materials, searchFilterMaterial]
    );
    const headersMaterial = ['NOMBRE','DESCRIPCION','PRECIO UNITARIO','CANTIDAD'];
    const propertiesMaterial = ['matNom','matDes','matPre','eveMatCan'];

    const executedBudget = supplierCost + materialCost;

    return (
        <>
            <header className="header-event-info flex flex-column p_5">
                <h2 className="center">Detalles del Evento</h2>
                <hr className="m0" />
                <div className="flex ai-center flex-row">
                    <div className="left Phone_6">
                        <p>{event && event.eveNom}</p>
                        <p className="f_75">{event && event.eveDes}</p>
                    </div>
                    <div className="flex jc-center flex-row Phone_4 center gap-1">
                        <div className="">
                            <p>Presupuesto Planificado</p>
                            <p className="flex flex-column">
                                S/. {event && formatterBudget.format(event.evePrePla)}
                            </p>
                        </div>
                        <div className="">
                            <p>Presupuesto Ejecutado</p>
                            <p className="flex flex-column">
                                S/. {formatterBudget.format(executedBudget)}
                            </p>
                        </div>
                        <div className="">
                            <p>Tenemos un Saldo de</p>
                            <p className="flex flex-column">
                                S/. {event && formatterBudget.format(event.evePrePla - executedBudget)}
                            </p>
                        </div>
                    </div>
                    <div className="right Phone_6">
                        <p>{event && moment(event.eveFec).format('DD MMM YYYY')}</p>
                        <p className="f_75">{event && event.eveHor} hrs.</p>
                    </div>
                </div>
            </header>
            <div className="content-event-details flex flex-row flex-grow-1 overflow-auto p1 gap-1">
                <section className="Large_6">
                    <div className="table-content p_5 flex-grow-1 overflow-auto block-tareas">
                        <h4>Listado de Tareas</h4>
                        <div className="flex gap-1">
                            <SearchInput 
                                value={searchFilter} 
                                onChange={e => setSearchFilter(e.target.value)} 
                            />
                            {
                                userLogged.rolCod != '02' &&
                                <button 
                                    className='button-primary flex jc-space-between Large_3 ai-center gap_5'
                                    onClick={() => setModalTask(true)} 
                                >
                                    Añadir 
                                    <span className='flex f1_25'>
                                        <PlusIcon />
                                    </span>
                                </button>
                            }
                            <button 
                                className='button-primary flex jc-space-between Large_3 ai-center gap_5'
                                onClick={() => Export_Excel(filteredData,headersTask,'TAREAS',propertiesTask)} 
                            >
                                Exportar 
                                <span className='flex f1_25'>
                                    <ExcelIcon />
                                </span>
                            </button>
                        </div>
                        <Table data={filteredData} columns={taskColumns} />
                    </div>
                </section>
                <section className="Large_6">
                    <div className="table-content p_5 block-supplier" style={{height: '50%'}}>
                        <h4>Listado de Proveedores</h4>
                        <div className="flex gap_5">
                            <SearchInput 
                                value={searchFilterSupplier} 
                                onChange={e => setSearchFilterSupplier(e.target.value)} 
                            />
                            {
                                userLogged.rolCod != '02' &&
                                <button 
                                    className='button-primary flex jc-space-between Large_3 ai-center gap_5'
                                    onClick={() => setModalSupplier(true)} 
                                >
                                    Añadir 
                                    <span className='flex f1_25'>
                                        <PlusIcon />
                                    </span>
                                </button>
                            }
                             <button 
                                className='button-primary flex jc-space-between Large_3 ai-center gap_5'
                                onClick={() => Export_Excel(filteredDataSupplier,headersSupplier,'PROVEEDORES',propertiesSuppler)} 
                            >
                                Exportar 
                                <span className='flex f1_25'>
                                    <ExcelIcon />
                                </span>
                            </button>
                        </div>
                        <Table 
                            data={filteredDataSupplier} 
                            columns={supplierColumns} 
                        />
                    </div>
                    <div className="table-content p_5  block-material" style={{height: '50%'}}>
                        <h4>Listado de Materiales</h4>
                        <div className="flex gap_5">
                            <SearchInput 
                                value={searchFilterMaterial} 
                                onChange={e => setSearchFilterMaterial(e.target.value)} 
                            />
                            {
                                userLogged.rolCod != '02' &&
                                <button 
                                    className='button-primary flex jc-space-between Large_3 ai-center gap_5'
                                    onClick={() => setModalMaterial(true)} 
                                >
                                    Añadir 
                                    <span className='flex f1_25'>
                                        <PlusIcon />
                                    </span>
                                </button>
                            }
                             <button 
                                className='button-primary flex jc-space-between Large_3 ai-center gap_5'
                                onClick={() => Export_Excel(filteredDataMaterial,headersMaterial,'MATERIALES',propertiesMaterial)} 
                            >
                                Exportar 
                                <span className='flex f1_25'>
                                    <ExcelIcon />
                                </span>
                            </button>
                        </div>
                        <Table 
                            data={filteredDataMaterial} 
                            columns={materialColumns} 
                        />
                    </div>
                </section>
            </div>
            <footer className="footer-event-info p_5 flex jc-center gap-1">
                <button onClick={() => navigate('/')} className="p_5 button-detail Phone_1">Atrás</button>
                <button className="p_5 button-delete Phone_1">Eliminar</button>
            </footer>
            <ModalTask 
                modalOpen={modalTask}
                closeModal={() => closeModal(setModalTask, setTaskSelected) }
                setRefresh={setRefreshTask}
                record={taskSelected}
                event={event}
            />
            <ModalMaterial 
                modalOpen={modalMaterial}
                closeModal={() => closeModal(setModalMaterial, setMaterialSelected) }
                setRefresh={setRefreshMaterial}
                record={materialSelected}
                event={event}
            />
            <ModalSupplier 
                modalOpen={modalSupplier}
                closeModal={() => closeModal(setModalSupplier, setSupplierSelected) }
                setRefresh={setRefreshSupplier}
                record={supplierSelected}
                event={event}
            />
        </>
    )
}

export default EventDetails