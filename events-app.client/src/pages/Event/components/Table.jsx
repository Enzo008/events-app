import {
    useReactTable, 
    getCoreRowModel, 
    getPaginationRowModel,
    getSortedRowModel,
    flexRender,
} from '@tanstack/react-table';
import { useState } from 'react';
import SortIcon from '../../../icons/SortIcon';

const smallPageSizes = [10, 20, 30, 50];
const largePageSizes = [100, 200, 300, 500];

const Table = ({data = [], columns = []}) => {
    const [sorting, setSorting] = useState([]);
    
    const table = useReactTable({
        data,
        columns,
        getCoreRowModel: getCoreRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        getSortedRowModel: getSortedRowModel(),
        state: {
            sorting
        },
        onSortingChange: setSorting,
        columnResizeMode: "onChange"
    })

    return (
        <>
            <div className='flex-grow-1 overflow-auto'>
                <table className='Large_12 f_75'>
                    {
                        <thead className=''>
                            {
                                table.getHeaderGroups().map(headerGroup => (
                                    <tr key={headerGroup.id} className=''>
                                        {
                                            headerGroup.headers.map((header, index, array) => (
                                                <th
                                                    style={{whiteSpace: 'nowrap'}}
                                                    key={header.id} 
                                                    onClick={header.column.getToggleSortingHandler()}
                                                >
                                                    <div className='flex ai-center jc-space-between pointer'>
                                                        {
                                                            <span className='bold flex-grow-1'>
                                                                {flexRender(header.column.columnDef.header, header.getContext())}
                                                            </span>
                                                        }
                                                        <div className='flex ai-center jc-center'>
                                                            {header.column.getIsSorted() === 'asc' && !header.column.columnDef.disableSorting ? 
                                                                <span className="sort-icon active">
                                                                    <SortIcon type='asc' />
                                                                </span> :
                                                                header.column.getIsSorted() === 'desc' && !header.column.columnDef.disableSorting ? 
                                                                <span className="sort-icon active">
                                                                    <SortIcon type='desc' />
                                                                </span> :
                                                                !header.column.columnDef.disableSorting &&
                                                                <>
                                                                    <span className="sort-icon active">
                                                                        <SortIcon />
                                                                    </span>
                                                                </>
                                                            }
                                                        </div>
                                                    </div>
                                                </th>
                                            ))
                                        }
                                    </tr>
                                ))
                            }
                        </thead>
                    }
                    <tbody>
                        {
                            table.getRowModel().rows.length > 0 ?
                                <>
                                    {table.getRowModel().rows.map(row => (
                                        <tr key={row.id}>
                                            {row.getVisibleCells().map(cell => (
                                                <td style={{ width:  cell.column.getSize(), whiteSpace: 'nowrap'}} key={cell.id}>
                                                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                                                </td>
                                            ))}
                                        </tr>
                                    ))}
                                </>
                            :    
                            <tr className='' style={{height: '100%'}}>
                                <td colSpan={20} className='Large-p1 center '>
                                    <br /><br />
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
            <div className="pagination Large_12 flex column jc-space-between ai-center Large-p_25">
                <div className="todo">
                    <button onClick={() => table.setPageIndex(0)} disabled={!table.getCanPreviousPage()}>{"<<"}</button>
                    <button onClick={() => table.previousPage()} disabled={!table.getCanPreviousPage()}>{"<"}</button>
                    <button onClick={() => table.nextPage()} disabled={!table.getCanNextPage()}>{">"}</button> 
                    <button onClick={() => table.setPageIndex(table.getPageCount() -1)} disabled={!table.getCanNextPage()}>{">>"}</button>
                </div>
                <div>
                    <select 
                        value={table.options.state.pagination.pageSize} 
                        onChange={(e) => table.setPageSize(e.target.value)}
                        className="p_25"
                    > 
                        {smallPageSizes.map(pageSize => {
                            return  <option key={pageSize} value={pageSize}> 
                                        {pageSize} 
                                    </option>;
                        })}
                    </select>
                </div>
                <div>
                    <p className=''>
                        Mostrando {table.options.state.pagination.pageIndex * table.options.state.pagination.pageSize + 1} al {Math.min((table.options.state.pagination.pageIndex + 1) * table.options.state.pagination.pageSize, table.options.data.length)} de {table.options.data.length} registros
                    </p>
                </div>
            </div>
        </>
    )
}

export default Table