import React, { useEffect, useState } from 'react';
import { CircularProgress, createStyles, IconButton, makeStyles, TablePagination, Tooltip } from '@material-ui/core';
import MUIDataTable, { MUIDataTableColumnDef, MUIDataTableTextLabels } from 'mui-datatables';
import { CloudDownload } from '@material-ui/icons';
import UtilService from '../../../utils/util.service';

interface tableProps {
    data: any[],
    totalRegistros: number,
    handleExportToExcel: () => Promise<void>,
    changePage: (page: number, rowsPerPage: number) => Promise<void>,
    isProccessing: boolean
}

const useStyles = makeStyles(() =>
    createStyles({
        columnDate: {
            minWidth: '110px',
        },
        summaryCell: {
            textAlign: 'right',
            fontWeight: 'bold',
        },
        summaryRow: {
            backgroundColor: '#f1f1f1'
        },
        progress: {
            verticalAlign: 'middle',
            marginLeft: '14px',
            marginRight: '14px'
        }
    }),
);


const TablaReporteCierreMensualContable = (props: tableProps) => {

    const { data, handleExportToExcel, changePage, totalRegistros, isProccessing } = props;
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);
    const [rowsPerPage, setRowsPerPage] = React.useState(500);
    const disableProps = {disabled: true};

    const defaultOptions = {
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
    };

    const defaultOptionsDate = {
        setCellProps: () => ({
            className: classes.columnDate,
            style: {
                textAlign: 'left',
            },
            
        }),
        customBodyRender: (value: Date) => UtilService.getDate(value),
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
       
    };

    const defaultOptionsCurrency = {
        customBodyRender: (value: number | bigint) =>
            UtilService.formatCurrency(value),
            setCellProps: () => ({
                style: {
                    textAlign: 'right'
                   
                },
            }),
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
    };

    const columns: MUIDataTableColumnDef[] = [
        {
            name: 'tipoOperacion',
            label: 'Tipo Operación',
            options: defaultOptions
        },
        {
            name: 'descripcionOperacion',
            label: 'Descripción Operación',
            options: defaultOptions
        },
        {
            name: 'fechaOperacion',
            label: 'Fecha Operación',
            options: defaultOptionsDate,
        
        },
        {
            name: 'numeroIdentificacion',
            label: 'Número Identificación',
            options: defaultOptions,
        },
        {
            name: 'numeroCredito',
            label: 'Número Crédito',
            options: defaultOptions,
        },
        {
            name: 'cuentaNif',
            label: 'Cuenta NIF',
            options: defaultOptions
        },
        {
            name: 'descripcionCuenta',
            label: 'Descripción Cuenta',
            options: defaultOptions
        },
        {
            name: 'debito',
            label: 'Débito',
            options: defaultOptionsCurrency
        },
        {
            name: 'credito',
            label: 'Crédito',
            options: defaultOptionsCurrency
        }
    ];

    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleExportToExcel();
        } finally {
            setDownloading(false);
        }
    }

    return (
        <div className='MuiTableCell-stickyHeader; jss86'>
            <MUIDataTable
                title="Detalles Cierres"
                data={data ?? []}
                columns={columns}
                options={{
                    onChangePage(currentPage) {
                        changePage(currentPage + 1, rowsPerPage);
                    },
                    selectableRows: 'none',
                    filter: false,
                    print: false,
                    elevation: 1,
                    filterType: 'dropdown',
                    responsive: 'vertical',
                    fixedHeader: true,
                    fixedSelectColumn: true,
                    tableBodyHeight: '500px',
                    textLabels: {
                        body: {
                            noMatch: 'No se encontraron resultados',
                        },
                        pagination: {
                            rowsPerPage: 'Filas por página',
                            jumpToPage: '',
                        },
                        toolbar: {
                            downloadCsv: 'Descargar CSV',
                            search: 'Buscar',
                            viewColumns: 'Mostrar Columnas',
                        },
                        viewColumns: {
                            title: 'Mostrar Columnas',
                        },
                    },
                    download: false,
                    search: false,
                    customToolbar: () => (
                        <Tooltip title="Descargar Excel">
                            {
                                downloading
                                    ? <CircularProgress size={20} className={classes.progress}/>
                                    :  <IconButton onClick={handleClick} >
                                            <CloudDownload/>
                                        </IconButton>
                            }
                        </Tooltip>
                    ),
                    customFooter: ( rowCount: number,
                        page: number,
                        rowsPerPage: number,
                        changeRowsPerPage: (page: string | number) => void,
                        changePage: (newPage: number) => void,
                        textLabels: Partial<MUIDataTableTextLabels> ) => {
                            useEffect(() => changeRowsPerPage(500), []);
                            return (
                                <>
                                    { isProccessing && 
                                        <>
                                            <CircularProgress size={25} className={classes.progress}/> 
                                            <b>Cargando ...</b>
                                        </>
                                    }
                                    <TablePagination
                                        rowsPerPageOptions={[100, 300, 500]}
                                        colSpan={3}
                                        count={totalRegistros}
                                        rowsPerPage={rowsPerPage}
                                        page={page}
                                        labelRowsPerPage='Filas por página'
                                        onPageChange={(_: any, page: number) => changePage(page)}
                                        onRowsPerPageChange={(event) => {
                                            const value = parseInt(event.target.value, 10);
                                            changeRowsPerPage(value);
                                            setRowsPerPage(value);
                                            changePage(0);
                                        }}
                                        backIconButtonProps={isProccessing ? disableProps : undefined}
                                        nextIconButtonProps={isProccessing ? disableProps : undefined}
                                        SelectProps={isProccessing ? disableProps : undefined}
                                    />
                                </>
                            );
                    }
                }}
            />
        </div>
    );
}

export default TablaReporteCierreMensualContable;