import ExcelJS from 'exceljs';
import { saveAs } from 'file-saver';

export const Export_Excel = async (data, headers, title, properties) => {
    // Crear un nuevo libro de trabajo
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Datos');
    
    // Añadir filas vacías
    for (let i = 0; i < 6; i++) {
        worksheet.addRow([]);
    }

    // Añadir los encabezados
    const headerRow = worksheet.addRow(headers);
    headerRow.eachCell((cell, number) => {
        cell.font = { color: { argb: '000' }, bold: true  }; 
        cell.fill = {  // Esto agregará un color de fondo a la celda
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFCA53' }  // Puedes cambiar este código de color ARGB según tus necesidades
        };
    });
    
    // Añadir los datos
    data.forEach(item => {
        const rowData = properties.map(prop => item[prop]);
        const row = worksheet.addRow(rowData);
        row.eachCell(cell => {
            cell.alignment = { 
                vertical: 'middle',
                readingOrder: 'leftToRight',
                textRotation: 'horizontal' 
            };
        });
    });

    // Personalizar el ancho de las columnas
    worksheet.columns.forEach(column => {
        column.width = 30;  // Ancho por defecto para todas las columnas
    });

    // Insertar una columna vacía al principio
    for (let i = 0; i < 3; i++) {
        worksheet.spliceColumns(1, 0, []);
    }
    

    // Añadir el título en la celda B6 y combinar las celdas hasta la última columna con datos
    const lastColumnLetter = worksheet.lastColumn.letter;  // Obtener la letra de la última columna
    const titleCell = worksheet.getCell('D6');
    titleCell.font = { size: 14, bold: true };
    titleCell.alignment = { horizontal: 'center' };
    titleCell.value = `LISTADO DE ${title}`;
    worksheet.mergeCells(`D6:${lastColumnLetter}6`);
  
    // Generar el archivo Excel
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    saveAs(blob, `${title}_${Date.now()}.xlsx`);
};
