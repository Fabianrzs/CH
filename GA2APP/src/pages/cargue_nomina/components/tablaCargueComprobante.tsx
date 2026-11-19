import { createStyles, IconButton, makeStyles, Tooltip } from '@material-ui/core';
import { CloudDownload } from '@material-ui/icons';
import moment from 'moment';
import MUIDataTable from 'mui-datatables';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { ColumnFormatType } from '../../../enums/ColumnFormatType';
import ApiproductoClients from '../../../services/api-producto.service';
import ApiReportes from '../../../services/api-reportes.service';
import NotificationService from '../../../services/notification.service';
import UtilService from '../../../utils/util.service';


const useStyles = makeStyles(() =>
    createStyles({
        columnDate: {
            minWidth: '70px',
        },
        column: {
            padding: '10px',
            textAlign: 'left',
            alignItems: 'flex-end',
            justifyContent: 'left',
            display: 'flex',
        },
    }),
);

function TablaCargueComprobante (props: any) {
    const {
        producto
    } = props;

    const apiReportes = ApiReportes();
    const api = ApiproductoClients()

    const [datosHipotecario, setDatosHipotecario] = useState<any[]>([]);


    const classes = useStyles();
    const columns = [

        {
            name: 'cuota',
            label: 'Area de Negocio',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Estado de pago',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Numero de Comprobante',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Vigencia',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Clase de Aporte',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Producto',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Tipo Operacion',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Monto',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Acciones',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        },
      ];




  return (

    <MUIDataTable
        title=""
        data={[]}
        columns={columns}
        options={{
            selectableRows: 'none',
            filter: false,
            print: false,
            elevation: 0,
            fixedHeader: true,
            // fixedSelectColumn: true,
            tableBodyHeight: '300px',
            textLabels: {
                body: {
                    noMatch: 'No se encontraron resultados',
                },
                pagination: {
                    displayRows: 'de',
                    next: 'Siguiente',
                    previous: 'Anterior',
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

        }}
    />

  );
}

export default TablaCargueComprobante;
