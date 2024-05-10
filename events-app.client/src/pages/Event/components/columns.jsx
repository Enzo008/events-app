import EditIcon from '../../../icons/EditIcon';
import DeleteIcon from '../../../icons/DeleteIcon';
import moment from 'moment';
import Notiflix from 'notiflix';
import EyeIcon from '../../../icons/EyeIcon';

const formatterBudget = new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 2, 
    maximumFractionDigits: 2
}); 

export const formatter = new Intl.NumberFormat("en-US");

export const getSupplierColumns = (openModal, setRefresh, userLogged) => {
    let baseColumns = [
        {
            header: "Nombre",
            accessorKey: "proNom"
        },
        {
            header: "Apellido",
            accessorKey: "proApe"
        },
        {
            header: "Costo",
            accessorKey: "proPre",
            cell: ({row}) => (
                <span className='flex jc-center'>
                    S/. { formatterBudget.format(row.original.proPre) }
                </span>
            )
        },
        {
            header: "Servicios",
            accessorKey: "serCan",
            cell: ({row}) => (
                <span className='flex jc-center'>
                    { formatter.format(row.original.serCan) }
                </span>
            )
        },
    ];

    if (userLogged.rolCod !== '02') {
        baseColumns.push({
            header: () => <div style={{textAlign: 'center', flexGrow: '1'}}>Acciones</div>,
            accessorKey: "acciones",
            disableSorting: true,
            stickyRight: 0,
            cell: ({row}) => (
                <div className='icons-action flex gap_25 jc-center ai-center'>
                    <span
                        data-tooltip-id="edit-tooltip" 
                        data-tooltip-content="Ver Servicios" 
                        className='flex f1_25' 
                        // onClick={() => openModal(row.original)} 
                    >
                        <EyeIcon />
                    </span>
                    <span
                        data-tooltip-id="delete-tooltip" 
                        data-tooltip-content="Eliminar" 
                        className='flex f1_25'
                        onClick={() => handleDelete('Proveedor/evento',row.original, setRefresh)} 
                    >
                        <DeleteIcon />
                    </span>
                </div>
            ),
        });
    }

    return baseColumns;
};


export const getMaterialColumns = (openModal, setRefresh, userLogged) => {
    console.log(userLogged)
    let baseColumns = [
        {
            header: "Nombre",
            accessorKey: "matNom"
        },
        {
            header: "Descripcion",
            accessorKey: "matDes"
        },
        {
            header: "Precio Unitario",
            accessorKey: "matPre",
            cell: ({row}) => (
                <span className='flex jc-center'>
                    S/. { formatterBudget.format(row.original.matPre) }
                </span>
            )
        },
        {
            header: "Cantidad",
            accessorKey: "eveMatCan",
            cell: ({row}) => (
                <span className='flex jc-center'>
                    { formatter.format(row.original.eveMatCan) }
                </span>
            )
        },
        {
            header: "Total",
            accessorKey: "total",
            cell: ({row}) => {
                const precioUnitario = row.original.matPre;
                const cantidad = row.original.eveMatCan;
                return (
                    <span className='flex jc-center'>
                        S/. { formatterBudget.format(precioUnitario*cantidad) }
                    </span>
                )
            },
        },
    ];

    if (userLogged.rolCod !== '02') {
        baseColumns.push({
            header: () => <div style={{textAlign: 'center', flexGrow: '1'}}>Acciones</div>,
            accessorKey: "acciones",
            disableSorting: true,
            stickyRight: 0,
            cell: ({row}) => (
                <div className='icons-action flex gap_25 jc-center ai-center'>
                     {true &&
                        <span
                            data-tooltip-id="edit-tooltip" 
                            data-tooltip-content="Editar" 
                            className='flex f1_25 ' 
                            onClick={() => openModal(row.original)} 
                        >
                            <EditIcon />
                        </span>
                    }
                    {true &&
                        <span
                            data-tooltip-id="delete-tooltip" 
                            data-tooltip-content="Eliminar" 
                            className='flex f1_25 '
                            onClick={() => handleDelete('Material/evento',row.original, setRefresh)} 
                        >
                            <DeleteIcon />
                        </span>
                    }
                </div>
            ),
        });
    }

    return baseColumns;
};

export const getTaskColumns = (openModal, setRefresh, userLogged) => {
    let baseColumns = [
        {
            header: "Nombre",
            accessorKey: "tarNom"
        },
        {
            header: "Descripcion",
            accessorKey: "tarDes"
        },
        {
            header: "Fecha Inicio",
            accessorKey: "tarFecIniPla",
            cell: ({row}) => {
                const fecha = moment(row.original.tarFecIniPla).format('DD MMM YYYY');
                return(
                    <span className='flex jc-center'>
                        {fecha}
                    </span>
                )                    
            },
        },
        {
            header: "Fecha Fin",
            accessorKey: "tarFecFinPla",
            cell: ({row}) => {
                const fecha = moment(row.original.tarFecFinPla).format('DD MMM YYYY');
                return(
                    <span className='flex jc-center'>
                        {fecha}
                    </span>
                )                    
            },
        },
        {
            header: "Estado",
            accessorKey: "tarEst",
            cell: ({row}) => {
                return(
                    <span className='flex jc-center'>
                        {row.original.tarEst === 'I' ? 'Incompleto' : 'Completo'}
                    </span>
                )                    
            },
        },
    ];

    if (userLogged.rolCod !== '02') {
        baseColumns.push({
            header: () => <div style={{textAlign: 'center', flexGrow: '1'}}>Acciones</div>,
            accessorKey: "acciones",
            disableSorting: true,
            stickyRight: 0,
            cell: ({row}) => (
                <div className='icons-action flex gap_25 jc-center ai-center'>
                     {true &&
                        <span
                            data-tooltip-id="edit-tooltip" 
                            data-tooltip-content="Editar" 
                            className='flex f1_25' 
                            onClick={() => openModal(row.original)} 
                        >
                            <EditIcon />
                        </span>
                    }
                    {true &&
                        <span
                            data-tooltip-id="delete-tooltip" 
                            data-tooltip-content="Eliminar" 
                            className='flex f1_25'
                            onClick={() => handleDelete('Tarea',row.original, setRefresh)} 
                        >
                            <DeleteIcon />
                        </span>
                    }
                </div>
            ),
        });
    }

    return baseColumns;
};



export const handleDelete = async (controller, obj, setRefresh) => {
    console.log(obj)
    Notiflix.Confirm.show(
        'Eliminar Registro',
        '¿Está seguro que quiere eliminar este registro?',
        'Sí',
        'No',
        async () => {
            const url = `${import.meta.env.VITE_APP_API_URL}/api/${controller}`;

            
            try {
                Notiflix.Loading.pulse();
                const token = localStorage.getItem('token');
                const response = await fetch(url, {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                    body: JSON.stringify(obj),
                });
                const data = await response.json();
                if (!response.ok) {
                    Notiflix.Notify.failure(data.message);
                    return;
                }
                
                // Actualiza los datos después de eliminar un registro
                setRefresh(prevRefresh => !prevRefresh);
                Notiflix.Notify.success(data.message);
            } catch (error) {
                console.error('Error:', error);
            } finally {
                Notiflix.Loading.remove();
            }
        },
        () => {
            // El usuario ha cancelado la operación de eliminación
        }
    );
};