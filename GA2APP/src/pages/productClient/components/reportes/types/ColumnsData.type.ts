import { ColumnFormatType } from "../../../../../enums/ColumnFormatType";

export type ColumnData = {
    name: string,
    label: string,
    formatType?: ColumnFormatType,
    download?: boolean,
}