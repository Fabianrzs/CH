import React, { useEffect, useState } from 'react';
import moment from 'moment';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import UtilService from '../../../../../utils/util.service';
import { ColumnFormatType } from '../../../../../enums/ColumnFormatType';
import handleExportToExcel from '../../../../Reports/helpers/handleExportToExcel';
import DownloadToolbar from '../componentes/DownloadToolbar';
import { ReporteMensualSeguros } from '../types/ReporteMensualSeguros.type';

import CatalogApi from '../../../../../services/api-catalog.service';
import handleReporteMensualSeguros from '../../../../Reports/helpers/handleReporteMensualSeguros';

const TablaReporteMensualSeguros = ({data, periodo}: { data: ReporteMensualSeguros[], periodo?: Date | null }) => {

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
        customBodyRender: (value: number | bigint) => value != null ? UtilService.formatCurrency(value) : '',
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
            formatType: ColumnFormatType.Numerico,
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
            options: {...defaultCellOptions,
                setCellProps: () => ({
                    style: {
                        minWidth: '180px',
                    },
                }),
            },
        },
        {
            name: 'apellidos',
            label: 'Apellidos',
            options: {...defaultCellOptions,
                setCellProps: () => ({
                    style: {
                        minWidth: '180px',
                    },
                }),
            },
        },
        {
            name: 'fechaExpedicionDocumento',
            label: 'Fecha Expedición Documento',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaNacimiento',
            label: 'Fecha Nacimiento',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'ciudadInmueble',
            label: 'Ciudad inmueble',
            options: defaultCellOptions,
        },
        {
            name: 'direccionInmueble',
            label: 'Dirección inmueble',
            options: {...defaultCellOptions,
                setCellProps: () => ({
                    style: {
                        minWidth: '320px',
                    },
                }),
            },
        },
        {
            name: 'matriculaInmueble',
            label: 'Matricula inmueble',
            options: {...defaultCellOptions,
                setCellProps: () => ({
                    style: {
                        minWidth: '200px',
                    },
                }),
            },
        },
        {
            name: 'valorInmueble',
            label: 'Valor Inmueble',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorDesembolso',
            label: 'Valor Crédito',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'seguroHogar',
            label: 'Seguro Hogar',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'seguroVida',
            label: 'Seguro Vida',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorAvaluoComercial',
            label: 'Valor avaluó comercial',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'tipoVivienda',
            label: 'Tipo de vivienda',
            options: defaultCellOptions,
        },
        {
            name: 'grado',
            label: 'Grado',
            options: defaultCellOptions,
        },
        {
            name: 'categoria',
            label: 'Categoría',
            options: defaultCellOptions,
        },
        {
            name: 'correo',
            label: 'Correo electrónico',
            options: defaultCellOptions,
        },
        {
            name: 'telefono',
            label: 'Teléfono Afiliado',
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
        const format = periodo ? moment(periodo).format('YYYY-MM'): 'Actual';
        const fileName = `Reporte mensual de seguros - Periodo ${format}.xlsx`;
        await handleReporteMensualSeguros(data, columns, fileName, publicIpAddress);
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

export default TablaReporteMensualSeguros;