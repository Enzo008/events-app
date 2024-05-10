import { useEffect, useMemo, useState } from "react";
import SearchInput from "../Event/components/SearchInput";
import PlusIcon from "../../icons/PlusIcon";
import { getColumnsMaterial } from "./columns";
import Table from "../Event/components/Table";
import Notiflix from "notiflix";
import ModalMaterial from "./components/ModalMaterial";

const Material = () => {
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

    const columns = useMemo(() => getColumnsMaterial(openModal, setRefresh), [openModal, setRefresh]);

    useEffect(() => {
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


    const [searchFilterSupplier, setSearchFilterSupplier] = useState('');
    const filteredData = useMemo(() => 
        data.filter(item => 
            (item.matNom ? item.matNom.toUpperCase().includes(searchFilterSupplier.toUpperCase()) : false) ||
            (item.matDes ? item.matDes.toUpperCase().includes(searchFilterSupplier.toUpperCase()) : false)
        ), [data, searchFilterSupplier]
    );

    return (
        <>
            <div className="table-content p2 flex-grow-1">
                <h2>Listado de Materiales</h2>
                <div className="flex gap_5">
                    <SearchInput 
                        value={searchFilterSupplier} 
                        onChange={e => setSearchFilterSupplier(e.target.value)} 
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
                </div>
                <Table 
                    data={filteredData} 
                    columns={columns} 
                />
            </div>
            <ModalMaterial 
                modalOpen={modalOpen}
                closeModal={() => closeModal() }
                setRefresh={setRefresh}
                record={dataSelected}
            />
        </>
    )
}

export default Material