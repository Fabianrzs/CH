import React, { useEffect, useState } from 'react';
import moment from 'moment';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import UtilService from '../../../../../utils/util.service';
import { ColumnFormatType } from '../../../../../enums/ColumnFormatType';
import handleExportToExcel from '../../../../Reports/helpers/handleExportToExcel';
import DownloadToolbar from '../componentes/DownloadToolbar';
import CatalogApi from '../../../../../services/api-catalog.service';
import handleReporteAPNRCAR from '../../../../Reports/helpers/handleReporteAPNRCAR';

const TablaReporteAPNRCAR = ({data, periodo}: { data: any[], periodo?: Date | null }) => {

    const defaultCellOptions = {
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
                textAlign: 'center',
            },
        }),
        setCellProps: () => ({
            style: {
                textAlign: 'center',
            },
        }),
    };

    const defaultCellDateOptions = {
        customBodyRender: (value: Date) => UtilService.getDate(value),
        setCellHeaderProps: () => ({
            style: {
                backgroundColor: '#FEC917',
                color: '#676767',
                minWidth: '150px',
                textAlign: 'center',
            },
        }),
        setCellProps: () => ({
            style: {
                textAlign: 'center',
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
            name: 'consecutivoNumeroDeContrato',
            label: 'CONSECUTIVO NÚMERO DE CONTRATO',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'creditoInversionInterbancario',
            label: 'CRÉDITO INVERSIÓN INTERBANCARIO',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'claseDeCreditoOInversion',
            label: 'CLASE DE CRÉDITO O INVERSIÓN',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'clasificacionYPonderacion',
            label: 'CLASIFICACIÓN Y PONDERACIÓN',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'nit',
            label: 'NIT',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'riesgoCortoPlazo',
            label: 'RIESGO CORTO PLAZO',
            options: defaultCellOptions,
        },
        {
            name: 'riesgoLargoPlazo',
            label: 'RIESGO LARGO PLAZO',
            options: defaultCellOptions,
        },
        {
            name: 'saldoCredito',
            label: 'SALDO CRÉDITO',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'inversionSubordinada',
            label: 'INVERSIÓN SUBORDINADA',
            options: defaultCellOptions,
        },
        {
            name: 'diasResidual',
            label: 'DÍAS RESIDUAL',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'efectivo',
            label: 'EFECTIVO',
            options: defaultCellOptions,
        },
        {
            name: 'garantizadosPorOEmitidoPor',
            label: 'GARANTIZADOS POR O EMITIDO POR',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'valorDeLaGarantiaDelCredito',
            label: 'VALOR DE LA GARANTÍA DEL CRÉDITO',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'porcentajeDeGarantia',
            label: 'PORCENTAJE DE GARANTÍA',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'saldoCuentaPorCobrar',
            label: 'SALDO CUENTA POR COBRAR',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'destinoDelCredito',
            label: 'DESTINO DEL CRÉDITO',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'desembolso',
            label: 'DESEMBOLSO',
            options: defaultCellOptions,
        },
        {
            name: 'destinoDeOtorgamiento',
            label: 'DESTINO DE OTORGAMIENTO',
            options: defaultCellOptions,
        },
        {
            name: 'revocable',
            label: 'REVOCABLE',
            options: defaultCellOptions,
        },
        {
            name: 'procesosDeTitularizacion',
            label: 'PROCESOS DE TITULARIZACIÓN',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'provision',
            label: 'PROVISIÓN',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'coberturaDeLaGarantiaValoracion',
            label: 'COBERTURA DE LA GARANTÍA VALORACIÓN',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'claseDeGarantia',
            label: 'CLASE DE GARANTÍA',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'garantiaVivienda',
            label: 'GARANTÍA VIVIENDA',
            options: defaultCellOptions,
        },
        {
            name: 'diasDeIncumplimiento',
            label: 'DÍAS DE INCUMPLIMIENTO',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'tipoDeCartera',
            label: 'TIPO DE CARTERA',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'monedaLocal',
            label: 'MONEDA LOCAL',
            options: defaultCellOptions,
        },
        {
            name: 'activoEnIncumplimiento',
            label: 'ACTIVO EN INCUMPLIMIENTO',
            options: defaultCellOptions,
        },
        {
            name: 'fechaVencimiento',
            label: 'FECHA VENCIMIENTO',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaInicial',
            label: 'FECHA INICIAL',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'plazoInicial',
            label: 'PLAZO INICIAL',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'fechaCorte',
            label: 'FECHA CORTE',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'codigoGarantia',
            label: 'CÓDIGO GARANTÍA',
            options: defaultCellOptions,
        },
    ];

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi = CatalogApi();

    const isValidIPv4 = (ip: string) =>
        /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(ip);

    const isValidIPv6 = (ip: string) =>
        /([a-f0-9:]+:+)+[a-f0-9]+/i.test(ip);

    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0];
        await catalogApi
            .getObtenerIpLocal(convertIp)
            .then((response: any) => {
                if (response?.isSuccess) {
                    setPublicIpAddress(response.data);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    useEffect(() => {
        const peerConnection = new RTCPeerConnection({ iceServers: [] });
        peerConnection.createDataChannel('');
        peerConnection.createOffer()
            .then((offer) => peerConnection.setLocalDescription(offer))
            .catch((error) => console.error('Error creating offer:', error));

        peerConnection.onicecandidate = (event) => {
            if (event.candidate) {
                const ip: any = event.candidate.address;
                if (isValidIPv4(ip) || isValidIPv6(ip)) {
                    enviarIp(ip);
                } else {
                    enviarIp(ip);
                }
            }
        };

        return () => {
            peerConnection.close();
        };
    }, []);

    const handleDownload = async () => {
        const format = periodo ? ` - Periodo ${moment(periodo).format('YYYY-MM')}` : '';
        const fileName = `Reporte APNRCAR${format}.xlsx`;
        await handleReporteAPNRCAR(data, columns, fileName, publicIpAddress);
    }

    return (
        <div style={{width: '100%', 'display': 'table', 'tableLayout': 'fixed'}}>
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
                        customToolbar: () => <DownloadToolbar  downloadAction={handleDownload} />
                    }}
                />
        </div>
    );
}

export default TablaReporteAPNRCAR;