import AddServiceIcon from '../../icons/AddServiceIcon';
import DeleteIcon from '../../icons/DeleteIcon';
import EditIcon from '../../icons/EditIcon';
import EyeIcon from '../../icons/EyeIcon';
import { handleDelete } from '../Event/components/columns';

export const getColumnsUser= (openModal, setRefresh, openModalTable) => {
    let baseColumns = [
        {
            header: "Nombre",
            accessorKey: "usuNom"
        },
        {
            header: "Apellido",
            accessorKey: "usuApe"
        },
        {
            header: "Correo",
            accessorKey: "usuCorEle"
        },
        {
            header: "Número de documento",
            accessorKey: "usuNumDoc"
        },
        {
            header: "Teléfono",
            accessorKey: "usuTel"
        },
        {
            header: "Rol",
            accessorKey: "rolNom"
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
                            data-tooltip-content="Eventos asignados" 
                            className='flex f1_25 p_25'
                            onClick={() => openModalTable(row.original)} 
                        >
                            <AddServiceIcon />
                        </span>
                    }
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
                            onClick={() => handleDelete('Usuario',row.original, setRefresh)} 
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