import React, { useEffect, useState } from 'react';
import moment from 'moment';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import UtilService from '../../../../../utils/util.service';
import { ColumnFormatType } from '../../../../../enums/ColumnFormatType';
import DownloadToolbar from '../componentes/DownloadToolbar';
import CatalogApi from '../../../../../services/api-catalog.service';
import handleReporteGarantia from '../../../../Reports/helpers/handleReporteGarantia';

const TablaReporteGeneralGarantia = ({data, periodo}: { data: any[], periodo?: Date | null }) => {

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
            name: 'direccionInmuble',
            label: 'Dirección inmueble',
            options: defaultCellOptions,
        },
        {
            name: 'ciudadInmueble',
            label: 'Ciudad inmueble',
            options: defaultCellOptions,
        },
        {
            name: 'matriculaInmueble',
            label: 'Matrícula inmueble',
            options: defaultCellOptions,
        },
        {
            name: 'numeroEscrituraInmueble',
            label: 'Escritura inmueble',
            options: defaultCellOptions,
        },
        {
            name: 'porcentajeGarantias',
            label: 'Porcentaje de la Garantía',
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
        const fileName = `Reporte general de garantía - Periodo ${format}.xlsx`;
        await handleReporteGarantia(data, columns, fileName, publicIpAddress, 'Reporte General de Garantía');
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

export default TablaReporteGeneralGarantia;