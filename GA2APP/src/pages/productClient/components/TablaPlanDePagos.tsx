import React from "react";
import moment from "moment";
import MUIDataTable from "mui-datatables";
import PdfToolbar from "./reportes/componentes/PdfToolbar";
import handleExportToExcel from "../../Reports/helpers/handleExportToExcel";
import DownloadToolbar from "./reportes/componentes/DownloadToolbar";
import { ColumnFormatType } from "../../../enums/ColumnFormatType";
import ApiReportes from "../../../services/api-reportes.service";

const TablaPlanDePagos = ({ reportRequest }: { reportRequest: any }) => {

    if (!reportRequest) return null;
    const apiReportes = ApiReportes();
    const planPago = reportRequest.planPago ?? [];

    const fecha = new Date()
        .toISOString()
        .replace("T", " ")
        .substring(0, 19)
        .replace(/:/g, "_");

    let lineaCredito = "";
    let esLeasing = false;

    const idTipoCredito = Number(reportRequest.idTipoCredito);
    const idLineaCredito = Number(reportRequest.lineaCredito);

    if (idTipoCredito === 1) {
        switch (idLineaCredito) {
            case 1:
                lineaCredito = "LEASING 24 - 107";
                esLeasing = true;
                break;

            case 2:
                lineaCredito = "LEASING 108";
                esLeasing = true;
                break;

            case 3:
                lineaCredito = "LEASING VETERANOS";
                esLeasing = true;
                break;

            case 4:
                lineaCredito = "LEASING CESANTIAS";
                esLeasing = true;
                break;

            default:
                lineaCredito = String(reportRequest.lineaCredito);
                break;
        }
    } else if (idTipoCredito === 2 && idLineaCredito === 5) {
        lineaCredito = "HIPOTECARIO";
    }

    const plazoTotalFormulario = Number(reportRequest.plazoTotal) || 0;

    const planPagoConNumeroCredito = planPago.map((item: any) => {
        if (esLeasing) {
            return {
                ...item,
                numeroCredito: reportRequest.numeroCredito,
                plnSaldoInicial: item.saldoLeasing,
                plnCapital: item.capitalLeasing,
                plnCapitalPorPagar: item.capitalpagar,
                plnInteres: item.interesLeasing,
                plnInteresPorPagar: item.interesPorPagar,
                plnSeguroVida: item.seguroVidaLeasing,
                plnSeguroHogar: item.seguroHogar,
                plnAPagar: item.cuotaUnidadPagadora,
                plnPlazo: plazoTotalFormulario,
                plnValorDeudaRemanente: item.valorDeudaRemanente,
            };
        }

        return {
            ...item,
            numeroCredito: reportRequest.numeroCredito,
            plnSaldoInicial: item.saldo,
            plnCapital: item.capital,
            plnCapitalPorPagar: item.capitalPorPagar,
            plnInteres: item.intereses,
            plnInteresPorPagar: item.interesesPorPagar,
            plnSeguroVida: item.seguroVida,
            plnSeguroHogar: item.seguroHogar,
            plnAPagar: item.cuotaUnidadPagadora,
            plnPlazo: plazoTotalFormulario,
            plnValorDeudaRemanente: item.valorDeudaRemanente,
        };
    });

    const nombreArchivo = `Plan pagos ${lineaCredito} - ${reportRequest.numeroCredito} - ${reportRequest.nombreCliente} - ${fecha}`;

    // Formatea números en estilo colombiano: punto para miles, coma para decimales. Ej: 1.000,00
    const formatoDecimal = new Intl.NumberFormat('es-CO', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });

    const formatearDecimal = (value: any) => {
        const numero = Number(value);
        return formatoDecimal.format(Number.isNaN(numero) ? 0 : numero);
    };

    const formatearFecha = (value: any) => {
        if (!value) return '';
        const fechaMoment = moment(value);
        return fechaMoment.isValid() ? fechaMoment.format('DD/MM/YYYY') : value;
    };

    const columns = [
        {
            name: 'numeroCredito',
            label: 'Número Crédito',
            formatType: ColumnFormatType.Numerico,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },

        },
        {
            name: 'cuota',
            label: 'Cuotas',
            formatType: ColumnFormatType.Numerico,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },

        },
        {
            name: 'fechaPago',
            label: 'Fecha Pago',
            formatType: ColumnFormatType.Fecha,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearFecha(value)
            },

        },
        {
            name: 'plnSaldoInicial',
            label: 'Saldo Inicial',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        {
            name: 'plnCapital',
            label: 'Capital',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        {
            name: 'plnCapitalPorPagar',
            label: 'Capital Por Pagar',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        {
            name: 'plnInteres',
            label: 'Interes',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        {
            name: 'plnInteresPorPagar',
            label: 'Interes Por Pagar',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        {
            name: 'plnSeguroVida',
            label: 'Seguro Vida',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        {
            name: 'plnSeguroHogar',
            label: 'Seguro Hogar',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        {
            name: 'plnAPagar',
            label: 'A Pagar',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        {
            name: 'plnPlazo',
            label: 'Plazo',
            formatType: ColumnFormatType.Numerico,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },

        },
        {
            name: 'plnValorDeudaRemanente',
            label: 'Deuda Remanente',
            formatType: ColumnFormatType.Decimal,
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatearDecimal(value)
            },

        },
        // {
        //     name: 'plnF14',
        //     label: 'F14',
        //     options: {
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //         customBodyRender: (value: any) => (value !== null ? value : 0)
        //     },

        // },
    ];

    const generarPDF = async () => {
        if (!planPago.length) return;

        const fechaFinal =
            planPago.length > 0
                ? planPago[planPago.length - 1].plnFechaPago
                : null;

        const indiceCanonInicial = esLeasing ? 0 : 1;
        const valorCanonInicial =
            planPago.length > indiceCanonInicial
                ? planPago[indiceCanonInicial].plnAPagar
                : null;

        const requestReport = {
            ...reportRequest,
            lineaCredito: lineaCredito,
            valorCuota: valorCanonInicial,
            fechaFinPagos: fechaFinal,
            planPago: reportRequest.planPago.map((item: any) => ({
                Fecha: item.plnFechaPago,
                Cuota: item.plnCuota,
                SeguroVida: item.plnSeguroVida,
                SeguroHogar: item.plnSeguroHogar,

                CuotaTradicional: item.plnAPagar,
                CuotaUnidadPagadora: item.plnAPagar,

                CapitalTradicional: 0,
                InteresTradicional: 0,
                SaldoTradicional: 0,

                CapitalReducido: 0,
                InteresReducido: 0,
                SaldoReducido: 0,

                CapitalPorPagar: item.plnCapitalPorPagar,
                SaldoCapital: item.plnSaldoIncial,
                InteresPorPagar: item.plnInteresPorPagar,

                SaldoTotal: 0,
                Saldo: item.plnSaldoIncial,

                Interes: item.plnInteres,
                Capital: item.plnCapital,

                SaldoInicial: item.plnSaldoIncial,
                DeudaRemanente: item.plnValorDeudaRemanente,

                EstadoCuota: "",
                AbonosCuota: 0
            }))
        };

        const response = await apiReportes.reporteInformacionPlanPago(requestReport);

        const binary = atob(response.data);

        const bytes = new Uint8Array(binary.length);

        for (let i = 0; i < binary.length; i += 1) {
            bytes[i] = binary.charCodeAt(i);
        }

        const pdfBlob = new Blob([bytes], {
            type: "application/pdf"
        });

        const url = URL.createObjectURL(pdfBlob);

        const link = document.createElement("a");

        link.href = url;

        link.download = `${nombreArchivo}.pdf`;

        link.click();

        URL.revokeObjectURL(url);
    };

    const handleDownload = async () => {
        if (!planPago?.length) return;

        await handleExportToExcel(
            planPagoConNumeroCredito,
            columns,
            `${nombreArchivo}.xlsx`,
            null
        );
    };


    return (

        <>
            <MUIDataTable
                title="Plan de pagos"
                data={planPagoConNumeroCredito}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    download: false,

                    customToolbar: () => (
                        <>
                            <DownloadToolbar downloadAction={handleDownload} />
                            <PdfToolbar pdfAction={generarPDF} />
                        </>
                    ),

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
                            downloadCsv: 'Descargar Excel',
                            search: 'Buscar',
                            viewColumns: 'Mostrar Columnas',
                        },
                        viewColumns: {
                            title: 'Mostrar Columnas',
                        },
                    },
                } as any}
            />
        </>
    )
}


export default TablaPlanDePagos;