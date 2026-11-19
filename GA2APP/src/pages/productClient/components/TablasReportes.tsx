import { IconButton, Tooltip, CircularProgress } from '@material-ui/core';
import { CloudDownload } from '@material-ui/icons';
import moment from 'moment';
import { toast } from 'react-toastify';
import MUIDataTable from 'mui-datatables';
import React, { useEffect, useState } from 'react'
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import { ColumnFormatType } from '../../../enums/ColumnFormatType';
import ApiReportes from '../../../services/api-reportes.service';
import UtilService from '../../../utils/util.service';
import CatalogApi from '../../../services/api-catalog.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise';

function TablasReportes(
    {
        data
    }: {
        data: any[]
    }) {


    const columns = [
        {
            name: 'documento',
            label: 'Número Documento',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'fechaAnalisis',
            label: 'Fecha de Análisis',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRadicacion',
            label: 'Fecha de Radicación',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRecepcionAnalista',
            label: 'Fecha de Recepción  Analista',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaVencimiento',
            label: 'Fecha de Vencimiento',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'numeroRadicado',
            label: 'Número de Radicado',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'proceso',
            label: 'Proceso',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'puntoRadicacion',
            label: 'Punto de Radicación',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'valorSolicitud',
            label: 'Valor Solicitud',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right'

                    },
                }),
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="Seguradora"
            />
            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Cierre-Diario-Cartera.csv',
                        separator: ',',
                    },
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
                }}
            />
        </>

    );
}

export function ReportesAvaluos(
    {
        data
    }: {
        data: any[]
    }) {


    const columns = [
        {
            name: 'areaInmueble',
            label: 'Area del Inmueble',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'ciudad',
            label: 'Ciudad',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'deposito',
            label: 'Depósito',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'diferencia',
            label: 'Diferencia',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'documento',
            label: 'Documento',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'estadoInmueble',
            label: 'Estado del Inmueble',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaInicioProceso',
            label: 'Fecha Inicio de Proceso',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRevisionAvaluo',
            label: 'Fecha Revision de Avalúo',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'firmaAvaluo',
            label: 'Firma Avalúo',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },

        {
            name: 'nombreAfiliado',
            label: 'Nombre de Afiliado',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'nombresVendedor',
            label: 'Nombre Vendedor',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },

        {
            name: 'nroRadicado',
            label: 'Número Radicado',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'nrofolio',
            label: 'Número Folio',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'parqueadero',
            label: 'Parqueadero',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'puntoRadicacion',
            label: 'Punto Radicación',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'responsable',
            label: 'Responsable',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'resultadoAvaluo',
            label: 'Resultado Avalúo',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'resultadoConsulta',
            label: 'Resultado Consulta',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoVivienda',
            label: 'Tipo Vivienda',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'valorComercial',
            label: 'Valor Comercial',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },


        {
            name: 'valorVenta',
            label: 'valor Venta',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right'

                    },
                }),
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="Avalúo"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Cierre-Diario-Cartera.csv',
                        separator: ',',
                    },
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
                }}
            />
        </>
    );

}

export function ReporteCargaAnalista(
    {
        data
    }: {
        data: any[]
    }) {


    const columns = [
        {
            name: 'documento',
            label: 'Número Documento',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'fechaAnalisis',
            label: 'Fecha de Análisis',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRadicacion',
            label: 'Fecha de Radicación',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRecepcionAnalista',
            label: 'Fecha de Recepción  Analista',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaVencimiento',
            label: 'Fecha de Vencimiento',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'numeroRadicado',
            label: 'Número de Radicado',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'proceso',
            label: 'Proceso',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'puntoRadicacion',
            label: 'Punto de Radicación',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'valorSolicitud',
            label: 'Valor Solicitud',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right'

                    },
                }),
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="Carga Analista"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Cierre-Diario-Cartera.csv',
                        separator: ',',
                    },
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
                }}
            />
        </>
    );

}

export function ReporteGeneralDeCartera({ data, periodo }: { data: any[], periodo?: Date | null }) {

    const [downloading, setDownloading] = useState(false);

    const defaultCellOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
            },
        }),
    };

    const defaultCellDateOptions = {
        customBodyRender: (value: Date) => UtilService.getDate(value),
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
            },
        }),
    };

    const defaultCellCurrencyOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
                textAlign: 'right',
            },
        }),
        customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value ?? 0),
        setCellProps: () => ({
            style: {
                textAlign: 'right',
            },
        }),
    };


    const columns = [
        {
            name: 'numeroCredito',
            label: 'Radicación',
            options: defaultCellOptions,
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: defaultCellOptions,
        },
        {
            name: 'identificacion',
            label: 'Identificación',
            options: defaultCellOptions,
        },
        {
            name: 'nombreCompleto',
            label: 'Apellidos y Nombres',
            options: defaultCellOptions,
        },
        {
            name: 'fuerza',
            label: 'Fuerza',
            options: defaultCellOptions,
        },
        {
            name: 'categoria',
            label: 'Categoría',
            options: defaultCellOptions,
        },
        {
            name: 'unidadejecutora',
            label: 'Nombre Pagaduría',
            options: defaultCellOptions,
        },
        {
            name: 'fechaDesembolso',
            label: 'Apertura',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaFinalizaPagos',
            label: 'Terminación',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'lineaCredito',
            label: 'Línea de Descuento',
            options: defaultCellOptions,
        },
        {
            name: 'tasaCredito',
            label: 'Tasa de Linea',
            options: {
                ...defaultCellCurrencyOptions,
                customBodyRender: (value: number | bigint) => UtilService.formatPercent(value ?? 0)
            },
            formatType: ColumnFormatType.Porcentaje,
        },
        {
            name: 'valorDesembolso',
            label: 'Monto',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'saldoCapital',
            label: 'Saldo',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorCuota',
            label: 'Cuota',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'fechaUltimoPago',
            label: 'Ultimo Pago',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaProximoPago',
            label: 'Proximo Pago',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'valorMoraAnterior',
            label: 'Mora Anterior',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorIntereses',
            label: 'Interés',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorSeguros',
            label: 'Seguros',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorCapital',
            label: 'Capital',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorTotalPagar',
            label: 'Total Pagar',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorVivienda',
            label: 'Valor del Inmueble',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'plazoTotal',
            label: 'Plazo de la   Obligación',
            options: defaultCellOptions,
        },
        {
            name: 'numeroCuotasFaltantes',
            label: 'Numero de cuotas faltantes',
            options: defaultCellOptions,
        },
        {
            name: 'calificacion',
            label: 'Calificación',
            options: defaultCellOptions,
        },
        {
            name: 'valorOpcionCompra',
            label: 'Valor opción de compra',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorDeudaRemanente',
            label: 'Interés Remanente',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'ciudadInmueble',
            label: 'Ciudad Inmueble',
            options: defaultCellOptions,
        },
        {
            name: 'tipoVivienda',
            label: 'Tipo Vivienda',
            options: defaultCellOptions,
        },
        {
            name: 'puntaje',
            label: 'Puntaje Scoring',
            options: defaultCellOptions,
        },
    ];

    return (
        <div style={{ 'display': 'block' }}>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="General de Cartera"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    rowsPerPage: 10,
                    tableBodyHeight: '500px',
                    selectableRows: 'none',
                    responsive: 'standard',
                    filter: false,
                    print: false,
                    elevation: 2,
                    filterType: 'dropdown',
                    fixedHeader: true,
                    fixedSelectColumn: true,
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
                    customToolbar: () => (
                        <>
                            <Tooltip title="Descargar Excel">
                                {
                                    downloading
                                        ? <CircularProgress size={20} style={{ 'verticalAlign': 'middle', 'marginLeft': '14px', 'marginRight': '14px' }} />
                                        : <IconButton onClick={() => {
                                            setDownloading(true);
                                            const fileName = `Reporte general de cartera - Periodo ${periodo ? moment(periodo).format('YYYY-MM') : 'Actual'}.xlsx`;
                                            handleExportToExcel(data, columns, fileName)
                                                .finally(() => setDownloading(false));
                                        }} >
                                            <CloudDownload />
                                        </IconButton>
                                }
                            </Tooltip>
                        </>
                    )
                }}
            />
        </div>
    );
}

export function ReporteGeneralDeGarantia({ data, periodo }: { data: any[], periodo?: Date | null }) {

    const [downloading, setDownloading] = useState(false);

    const defaultCellOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
            },
        }),
    };

    const defaultCellDateOptions = {
        customBodyRender: (value: Date) => UtilService.getDate(value),
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
            },
        }),
    };

    const defaultCellCurrencyOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
                textAlign: 'right',
            },
        }),
        customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value ?? 0),
        setCellProps: () => ({
            style: {
                textAlign: 'right',
            },
        }),
    };

    const columns = [
        {
            name: 'numeroRadicacion',
            label: 'Radicación',
            options: defaultCellOptions,
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: defaultCellOptions,
        },
        {
            name: 'tipoIdentificacion',
            label: 'Tipo documento',
            options: defaultCellOptions,
        },
        {
            name: 'identificacion',
            label: 'Número documento',
            options: defaultCellOptions,
        },
        {
            name: 'nombres',
            label: 'Nombres',
            options: defaultCellOptions,
        },
        {
            name: 'apellidos',
            label: 'Apellidos',
            options: defaultCellOptions,
        },
        {
            name: 'ciudadInmueble',
            label: 'Ciudad inmueble',
            options: defaultCellOptions,
        },
        {
            name: 'fechaApertura',
            label: 'Fecha Apertura',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaTerminacion',
            label: 'Fecha Terminación',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'valorInmueble',
            label: 'Valor Inmueble',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'ajusteGarantia',
            label: 'Ajuste Garantía',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorAjustado',
            label: 'Valor Ajustado',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
    ];

    return (
        <div style={{ 'display': 'block' }}>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="General de garantía"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    rowsPerPage: 10,
                    tableBodyHeight: '500px',
                    selectableRows: 'none',
                    responsive: 'standard',
                    filter: false,
                    print: false,
                    elevation: 2,
                    filterType: 'dropdown',
                    fixedHeader: true,
                    fixedSelectColumn: true,
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
                    customToolbar: () => (
                        <>
                            <Tooltip title="Descargar Excel">
                                {
                                    downloading
                                        ? <CircularProgress size={20} style={{ 'verticalAlign': 'middle', 'marginLeft': '14px', 'marginRight': '14px' }} />
                                        : <IconButton onClick={() => {
                                            setDownloading(true);
                                            const fileName = `Reporte general de garantía - Periodo ${periodo ? moment(periodo).format('YYYY-MM') : 'Actual'}.xlsx`;
                                            handleExportToExcel(data, columns, fileName)
                                                .finally(() => setDownloading(false));
                                        }} >
                                            <CloudDownload />
                                        </IconButton>
                                }
                            </Tooltip>
                        </>
                    )
                }}
            />
        </div>
    );
}

export function ReporteGeneralDeMora({ data, periodo }: { data: any[], periodo?: Date | null }) {

    const [downloading, setDownloading] = useState(false);
    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);

    const defaultCellOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
            },
        }),
    };

    const defaultCellDateOptions = {
        customBodyRender: (value: Date) => UtilService.getDate(value),
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
            },
        }),
    };

    const defaultCellCurrencyOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
                textAlign: 'right',
            },
        }),
        customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value ?? 0),
        setCellProps: () => ({
            style: {
                textAlign: 'right',
            },
        }),
    };

    
    const catalogApi = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0]

        const hexadecimal = convertIp
        await catalogApi
            .getObtenerIpLocal(hexadecimal)
            .then(async (response: any) => {

                if (response) {
                    if (response.isSuccess) {
                        setPublicIpAddress(response.data);
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }

    useEffect(() => {
        const fetchLocalIPAddress = () => {
            const peerConnection = new RTCPeerConnection({ iceServers: [] });
            peerConnection.createDataChannel('');
            peerConnection.createOffer()
                .then((offer) => peerConnection.setLocalDescription(offer))
                .catch((error) => console.error('Error creating offer:', error));

            peerConnection.onicecandidate = (event) => {
                if (event.candidate) {
                    const ip: any = event.candidate.address;
                    if (isValidIPv4(ip) || isValidIPv6(ip)) {
                        setLocalIpAddress(ip);
                        enviarIp(ip);
                    } else {
                        setLocalIpAddress(ip);
                        enviarIp(ip);
                    }
                }
            };

            return () => {
                peerConnection.close();
            };
        };
        fetchLocalIPAddress();

    }, []);


    const columns = [
        {
            name: 'numeroCredito',
            label: 'Número de Radicación',
            options: defaultCellOptions,
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: defaultCellOptions,
        },
        {
            name: 'unidadEjecutora',
            label: 'Unidad Ejecutora',
            options: defaultCellOptions,
        },
        {
            name: 'numeroIdentificacion',
            label: 'Identificación',
            options: defaultCellOptions,
        },
        {
            name: 'nombreCompleto',
            label: 'Apellidos y Nombres',
            options: defaultCellOptions,
        },
        {
            name: 'saldoCapital',
            label: 'Saldo',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'fechaUltimoPago',
            label: 'Fecha Ultimo Pago',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'valorUltimoPago',
            label: 'Valor Ultimo Pago',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'fechaProximoPago',
            label: 'Fecha Proximo Pago',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'valorCuota',
            label: 'Cuota',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
    ];

    return (
        <div style={{ 'display': 'block' }}>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="General de Mora"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    rowsPerPage: 10,
                    tableBodyHeight: '500px',
                    selectableRows: 'none',
                    responsive: 'standard',
                    filter: false,
                    print: false,
                    elevation: 2,
                    filterType: 'dropdown',
                    fixedHeader: true,
                    fixedSelectColumn: true,
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
                    customToolbar: () => (
                        <>
                            <Tooltip title="Descargar Excel">
                                {
                                    downloading
                                        ? <CircularProgress size={20} style={{ 'verticalAlign': 'middle', 'marginLeft': '14px', 'marginRight': '14px' }} />
                                        : <IconButton onClick={() => {
                                            setDownloading(true);
                                            const fileName = `Reporte general de mora - Periodo ${moment(periodo).format('YYYY-MM')}.xlsx`;
                                            handleExportToExcel(data, columns, fileName, publicIpAddress, setPublicIpAddress, setLocalIpAddress)
                                                .finally(() => setDownloading(false));
                                        }} >
                                            <CloudDownload />
                                        </IconButton>
                                }
                            </Tooltip>
                        </>
                    )
                }}
            />
        </div>
    );
}

export function ReporteGeneralDeGarantias(
    {
        data
    }: {
        data: any[]
    }) {


    const columns = [
        {
            name: 'documento',
            label: 'Número Documento',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'fechaAnalisis',
            label: 'Fecha de Análisis',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRadicacion',
            label: 'Fecha de Radicación',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRecepcionAnalista',
            label: 'Fecha de Recepción  Analista',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaVencimiento',
            label: 'Fecha de Vencimiento',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'numeroRadicado',
            label: 'Número de Radicado',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'proceso',
            label: 'Proceso',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'puntoRadicacion',
            label: 'Punto de Radicación',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'valorSolicitud',
            label: 'Valor Solicitud',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right'

                    },
                }),
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="general de garantías"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Cierre-Diario-Cartera.csv',
                        separator: ',',
                    },
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
                }}
            />
        </>
    );

}

export function ReportePreliminarDeCartera(
    {
        data
    }: {
        data: any[]
    }) {


    const columns = [
        {
            name: 'documento',
            label: 'Número Documento',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'fechaAnalisis',
            label: 'Fecha de Análisis',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRadicacion',
            label: 'Fecha de Radicación',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRecepcionAnalista',
            label: 'Fecha de Recepción  Analista',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaVencimiento',
            label: 'Fecha de Vencimiento',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'numeroRadicado',
            label: 'Número de Radicado',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'proceso',
            label: 'Proceso',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'puntoRadicacion',
            label: 'Punto de Radicación',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'valorSolicitud',
            label: 'Valor Solicitud',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right'

                    },
                }),
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="general de garantías"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Cierre-Diario-Cartera.csv',
                        separator: ',',
                    },
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
                }}
            />
        </>
    );

}

export function ReporteValorDeVenta(
    {
        data
    }: {
        data: any[]
    }) {


    const columns = [
        {
            name: 'documento',
            label: 'Número Documento',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'fechaAnalisis',
            label: 'Fecha de Análisis',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRadicacion',
            label: 'Fecha de Radicación',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRecepcionAnalista',
            label: 'Fecha de Recepción  Analista',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaVencimiento',
            label: 'Fecha de Vencimiento',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'numeroRadicado',
            label: 'Número de Radicado',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'proceso',
            label: 'Proceso',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'puntoRadicacion',
            label: 'Punto de Radicación',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'valorSolicitud',
            label: 'Valor Solicitud',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right'

                    },
                }),
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="Valor Venta"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Cierre-Diario-Cartera.csv',
                        separator: ',',
                    },
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
                }}
            />
        </>
    );

}

export function ReportesAnalistasDeCredito(
    {
        data
    }: {
        data: any[]
    }) {


    const columns = [
        {
            name: 'documento',
            label: 'Número Documento',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'fechaAnalisis',
            label: 'Fecha de Análisis',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRadicacion',
            label: 'Fecha de Radicación',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRecepcionAnalista',
            label: 'Fecha de Recepción  Analista',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaVencimiento',
            label: 'Fecha de Vencimiento',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'numeroRadicado',
            label: 'Número de Radicado',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'proceso',
            label: 'Proceso',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'puntoRadicacion',
            label: 'Punto de Radicación',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'valorSolicitud',
            label: 'Valor Solicitud',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right'

                    },
                }),
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="Analista de Crédito"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Cierre-Diario-Cartera.csv',
                        separator: ',',
                    },
                    textLabels: {
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
                }}
            />
        </>
    );

}


export function ReporteConsultasRestrictivas(
    {
        data
    }: {
        data: any[]
    }) {


    const columns = [
        {
            name: 'documento',
            label: 'Número Documento',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        borderTopRightRadius: '0px',
                        color: '#676767',
                    },
                }),
                setRowProps: (row: any, dataIndex: any, rowIndex: any) => ({
                    style:
                        rowIndex % 2 === 0
                            ? { color: '#C4C4C4', background: '#C4C4C4' }
                            : { color: '#C4C4C4', background: '#f1f2f6' },
                }),
            },
        },
        {
            name: 'fechaAnalisis',
            label: 'Fecha de Análisis',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRadicacion',
            label: 'Fecha de Radicación',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaRecepcionAnalista',
            label: 'Fecha de Recepción  Analista',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaVencimiento',
            label: 'Fecha de Vencimiento',
            options: {
                customBodyRender: (value: string) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'numeroRadicado',
            label: 'Número de Radicado',
            options: {
                customBodyRender: (value: any) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'proceso',
            label: 'Proceso',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'puntoRadicacion',
            label: 'Punto de Radicación',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                customBodyRender: (value: string) => value,
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'valorSolicitud',
            label: 'Valor Solicitud',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right'

                    },
                }),
                setCellHeaderProps: () => ({
                    style: {
                        borderTopLeftRadius: '0px',
                        borderTopRightRadius: '12px',
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
    ];

    return (
        <>
            <CHPageTitle
                topText="Consulta Reportes"
                subText=""
                title1="Reporte "
                title2="Consultas Restrictivas"
            />

            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    downloadOptions: {
                        filename: 'Cierre-Diario-Cartera.csv',
                        separator: ',',
                    },
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
                }}
            />
        </>
    );

}




const handleExportToExcel = async (data: any[], columns: { name: string, label: string, formatType?: ColumnFormatType }[], nameFile: string, publicIpAddress: string | null, setPublicIpAddress: (value: string | null) => void,
setLocalIpAddress: (value: string | null) => void) => {

    const apiReportes = ApiReportes();

    const columnDataList = data.map((row: any) => ({
        data: columns.map(x => {

            if (x.formatType === ColumnFormatType.Fecha && row[x.name]) {
                const parseDate = moment(row[x.name]);
                return parseDate.isValid()
                    ? parseDate.format("YYYY-MM-DD")
                    : '';
            }

            return row[x.name]?.toString() ?? ''
        })
    }));

    if (columnDataList) {
        const request = {
            columnData: columnDataList.map(item => item.data),
            columnsInfo: columns.map(x => ({
                name: x.label,
                formatType: x.formatType
            }))
        };
        const response = await apiReportes.GenerarExcelInvariantCulture(request);
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = nameFile;
        document.body.appendChild(link);
        link.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(link);

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Productos clientes / Reportes / Otros Reportes',
            hostname: document.location.host,
            tipoAccion: 'DESCARGAS',
            idTipoAccion: 5,
            descripcion: 'Descarga de Reporte General de Mora'

        } as Auditoria;

        auditoriaApi.InsertAuditoria(requestAuditoria);
    }
}


export default TablasReportes
