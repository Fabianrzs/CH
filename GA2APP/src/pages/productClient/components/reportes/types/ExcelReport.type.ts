import { ColumnFormatType } from "../../../../../enums/ColumnFormatType";

export interface ExcelReport
{
    filename: string;
    sheets: ExcelSheet[]
}

export interface ExcelSheet {
    sheetName: string;
    columnsInfo: ColumnHeaderInfo[]; 
    columnData: string[][] 
}

export interface ColumnHeaderInfo {
    name: string;
    formatType?: ColumnFormatType;
}