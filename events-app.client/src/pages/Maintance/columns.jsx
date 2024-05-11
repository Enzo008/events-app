import { formatter, formatterBudget } from '../../helpers/format';
import DeleteIcon from '../../icons/DeleteIcon';
import EditIcon from '../../icons/EditIcon';
import { handleDelete } from '../Event/components/columns';

export const getColumnsService = (openModal, setRefresh) => {
    let baseColumns = [
        {
            header: "Nombre",
            accessorKey: "serNom"
        },
        {
            header: "Descripcion",
            accessorKey: "serDes"
        },
    ];

    if (true || true) {
        baseColumns.push({
            header: () => <div style={{textAlign: 'center', flexGrow: '1'}}>Acciones</div>,
            accessorKey: "acciones",
            disableSorting: true,
            stickyRight: 0,
            cell: ({row}) => (
                <div className='icons-action flex jc-center ai-center'>
                     {true &&
                        <span
                            data-tooltip-id="edit-tooltip" 
                            data-tooltip-content="Editar" 
                            className='flex f1_25 p_25' 
                            onClick={() => openModal(row.original)} 
                        >
                            <EditIcon />
                        </span>
                    }
                    {true &&
                        <span
                            data-tooltip-id="delete-tooltip" 
                            data-tooltip-content="Eliminar" 
                            className='flex f1_25 p_25'
                            onClick={() => handleDelete('Servicio',row.original, setRefresh)} 
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

export const getColumnsMaterial = (openModal, setRefresh) => {
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
    ];

    if (true || true) {
        baseColumns.push({
            header: () => <div style={{textAlign: 'center', flexGrow: '1'}}>Acciones</div>,
            accessorKey: "acciones",
            disableSorting: true,
            stickyRight: 0,
            cell: ({row}) => (
                <div className='icons-action flex jc-center ai-center'>
                     {true &&
                        <span
                            data-tooltip-id="edit-tooltip" 
                            data-tooltip-content="Editar" 
                            className='flex f1_25 p_25' 
                            onClick={() => openModal(row.original)} 
                        >
                            <EditIcon />
                        </span>
                    }
                    {true &&
                        <span
                            data-tooltip-id="delete-tooltip" 
                            data-tooltip-content="Eliminar" 
                            className='flex f1_25 p_25'
                            onClick={() => handleDelete('Material',row.original, setRefresh)} 
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

export const getColumnsSupplier = (openModal, setRefresh) => {
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
            header: "Telefno",
            accessorKey: "proTel"
        },
        {
            header: "Costo",
            accessorKey: "proSerCos",
            cell: ({row}) => (
                <span className='flex jc-center'>
                    S/. { formatterBudget.format(row.original.proSerCos) }
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

    if (true || true) {
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
                            className='flex f1_25 p_25' 
                            onClick={() => openModal(row.original)} 
                        >
                            <EditIcon />
                        </span>
                    }
                    {true &&
                        <span
                            data-tooltip-id="delete-tooltip" 
                            data-tooltip-content="Eliminar" 
                            className='flex f1_25 p_25'
                            onClick={() => handleDelete('Proveedor',row.original, setRefresh)} 
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