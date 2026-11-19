import React, { ChangeEvent, useEffect, useState } from 'react';
import {
    Grid, IconButton,
} from '@material-ui/core';
import { Visibility } from '@material-ui/icons';
import moment from 'moment';
import MUIDataTable, { MUIDataTableColumn } from 'mui-datatables';
import ContainerTitle from '../../../components/containe-title/container-title';
import { Auditoria, LogAuditorias, PaginationTable } from '../../../types/Auditoria.model.type';
import UtilService from '../../../utils/util.service';
import TablePagination from '../../../components/table-pagination/TablePagination'
import { UserState } from '../../../reducers/user.reducer';
import { ColumnFormatType } from '../../../enums/ColumnFormatType';
import DatosAuditoria from './DatosAuditoria';

type Columns = MUIDataTableColumn & { formatType?: ColumnFormatType, label: string, download?: boolean };

function TableAuditoria({
    log,
    setDetalle,
    changePagination,
    usersState
}: {
    log: LogAuditorias;
    setDetalle: (detalle: Auditoria) => void;
    changePagination: (data: PaginationTable) => void;
    usersState: UserState;
}) {


    const [items, setItems] = useState<Auditoria[]>();

    const [numeroPaginas, setNumeroPaginas] = useState<number>();
    const [numeroActual, setNumeroPaginaActual] = useState<number>();
    
    const defaultOptionsDate = {
        setCellProps: () => ({ style: { textAlign: "left" } }),
        customBodyRender: (value: string) =>
            value ? moment(value).format("DD/MM/YYYY HH:mm") : "",
        setCellHeaderProps: () => ({ className: "filas" }),
    };


    const defaultOptions = {
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
    };


    useEffect(() => {
        setItems(log.auditorias);
        setNumeroPaginaActual(1);
        setNumeroPaginas(log.numeroPaginas);
    }, [log.auditorias]);

    const [open, setOpen] = useState(false);
    const [selectedRow, setSelectedRow] = useState<any>(null);
    const [selectedData, setSelectedData] = useState<any[]>([]);

    const handleOpen = (rowData: any) => {
        setSelectedRow(rowData);
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const columns: Columns[] = [
        {
            name: 'id',
            label: 'Id Evento',
            options: defaultOptions
        },
        {
            name: 'fechaRegistro',
            label: 'Fecha Registro',
            options: {
                ...defaultOptionsDate,
                sort: true,
                sortCompare: (order) => (a, b) => {
                    const timeA = new Date(a.data).getTime();
                    const timeB = new Date(b.data).getTime();

                    if (timeA < timeB) return order === "asc" ? -1 : 1;
                    if (timeA > timeB) return order === "asc" ? 1 : -1;
                    return 0;
                },
            },
        },
        {
            name: 'tipoAccion',
            label: 'Tipo de acción',
            options: defaultOptions
        },
        {
            name: 'descripcion',
            label: 'Descripción',
            options: {
                setCellProps: () => ({
                    style: {
                        minWidth: '200px',
                    },
                }),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            }
        },
        {
            name: 'modulo',
            label: 'Módulo',
            options: defaultOptions
        },
        // {
        //     name: 'nombreColumna',
        //     label: 'Campo',
        //     options: defaultOptions
        // },
        // {
        //     name: 'valorAnterior',
        //     label: 'Valor anterior',
        //     options: defaultOptions
        // },
        // {
        //     name: 'valorNuevo',
        //     label: 'Valor nuevo',
        //     options: defaultOptions
        // },
        {
            name: 'nombreUsuario',
            label: 'Usuario',
            options: defaultOptions
        },
        {
            name: 'hostname',
            label: 'Hostname',
            options: defaultOptions
        },
        {
            name: 'ip',
            label: 'Ip',
            options: defaultOptions
        },
        {
            name: 'navegador',
            label: 'Navegador',
            options: defaultOptions
        },
        {
            name: 'sistemaOperativo',
            label: 'Sistema operativo',
            options: defaultOptions
        },
        {
            name: 'acciones',
            label: 'Acciones',
            options: {
                ...defaultOptions,
                customBodyRender: (value, tableMeta) => {
                    const { rowIndex } = tableMeta;
                    const rowData = items?.[rowIndex];
                    if (!rowData || !['MODIFICACIONES', 'GUARDAR', 'ELIMINAR'].includes(rowData.tipoAccion)) return null;


                    return (
                        <IconButton
                            onClick={() => {
                                setSelectedData(rowData);
                                handleOpen(rowData);
                            }}
                            title="Ver detalle"
                        >
                            <Visibility />
                        </IconButton>
                    );
                },
            },
        }
    ];

    return (
        <div className='MuiTableCell-stickyHeader; jss86'>
            <MUIDataTable
                title="Log Auditoria"
                data={items as Auditoria[]}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    filter: false,
                    print: false,
                    elevation: 2,
                    filterType: 'dropdown',
                    responsive: 'standard',
                    fixedHeader: true,
                    fixedSelectColumn: true,
                    tableBodyHeight: '600px',
                    rowsPerPageOptions: [20, 100, 200, 500],
                    textLabels: {
                        body: {
                            noMatch: 'No se encontraron resultados',
                        },
                        pagination: {
                            rowsPerPage: 'Filas por página',
                            jumpToPage: '',
                        },
                    },
                    download: false, search: 'false', viewColumns: 'false',
                    sortOrder: {
                        name: "fechaRegistro", // columna inicial de orden
                        direction: "desc",     // descendente (más reciente arriba)
                      },
                }}
            />
            <DatosAuditoria
                open={open}
                handleClose={handleClose}
                selectedData={selectedData}
            />
        </div>
    );
}


export default TableAuditoria;