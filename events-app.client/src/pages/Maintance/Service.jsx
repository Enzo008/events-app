import { useEffect, useMemo, useState } from "react";
import SearchInput from "../Event/components/SearchInput";
import PlusIcon from "../../icons/PlusIcon";
import { getColumnsService } from "./columns";
import Table from "../Event/components/Table";
import Notiflix from "notiflix";
import { useNavigate } from "react-router-dom";
import ExcelIcon from "../../icons/ExcelIcon";
import ModalService from "./components/ModalService";

const Service = () => {
    const navigate = useNavigate();
    const [ data, setData ] = useState([])
    const [ dataSelected, setDataSelected ] = useState(null)
    const [ modalOpen, setModalOpen ] = useState(false)
    const [ refresh, setRefresh ] = useState(false)
    const openModal = (record) => {
        setDataSelected(record);
        setModalOpen(true);
    };

    const closeModal = () => {
        setModalOpen(false);
        setDataSelected(null);
    }

    const columns = useMemo(() => getColumnsService(openModal, setRefresh), [openModal, setRefresh]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                Notiflix.Loading.pulse('Cargando...');
                // Valores del storage
                const token = localStorage.getItem('token');
                // Obtenemos los datos
                const response = await fetch(`${import.meta.env.VITE_APP_API_URL}/api/Servicio`, {
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
                setData(data);
            } catch (error) {
                console.error('Error:', error);
            } finally {
                Notiflix.Loading.remove();
            }
        };
        fetchData();
    }, [refresh])


    const [searchFilter, setSearchFilter] = useState('');
    const filteredData = useMemo(() => 
        data.filter(item => 
            (item.serNom ? item.serNom.toUpperCase().includes(searchFilter.toUpperCase()) : false) ||
            (item.setDes ? item.setDes.toUpperCase().includes(searchFilter.toUpperCase()) : false)
        ), [data, searchFilter]
    );

    return (
        <>
            <div className="table-content p2 flex-grow-1">
                <h2>Listado de Servicios</h2>
                <div className="flex gap_5">
                    <SearchInput 
                        value={searchFilter} 
                        onChange={e => setSearchFilter(e.target.value)} 
                    />
                    <button 
                        className='button-primary flex jc-space-between Large_3 ai-center gap_5'
                        onClick={() => setModalOpen(true)} 
                    >
                        Añadir 
                        <span className='flex f1_25'>
                            <PlusIcon />
                        </span>
                    </button>
                    <button 
                        className='button-primary flex jc-space-between Large_3 ai-center gap_5'
                        // onClick={() => Export_Excel(data,headers,'PROVEEDORES',properties)} 
                    >
                        Exportar 
                        <span className='flex f1_25'>
                            <ExcelIcon />
                        </span>
                    </button>
                </div>
                <Table 
                    data={filteredData} 
                    columns={columns} 
                />
            </div>
            <ModalService 
                modalOpen={modalOpen}
                closeModal={() => closeModal() }
                setRefresh={setRefresh}
                record={dataSelected}
            />
        </>
    )
}

export default Service