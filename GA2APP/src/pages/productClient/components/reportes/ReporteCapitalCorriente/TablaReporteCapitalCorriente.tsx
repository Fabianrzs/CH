import React, { useEffect, useState } from 'react';
import moment from 'moment';
import { toast } from 'react-toastify';
import MUIDataTable from 'mui-datatables';
import UtilService from '../../../../../utils/util.service';
import { ColumnFormatType } from '../../../../../enums/ColumnFormatType';
import handleExportToExcel from '../../../../Reports/helpers/handleExportToExcel';
import { DatosReporteCapitalCorriente } from '../types/DatosReporteCapitalCorriente';

import CatalogApi from '../../../../../services/api-catalog.service';


export const ObtenerColumnasCapitalCorriente = (periodo: Date) => {
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
        customBodyRender: (value: number | bigint) =>
            value != null ? UtilService.formatCurrency(value) : '',
        setCellProps: () => ({
            style: {
                textAlign: 'right',
            },
        }),
    };

    // Toma el mes siguiente al período recibido
    const mesInicial = (periodo.getMonth() + 1) % 12;

    const meses = [
        'ENERO',
        'FEBRERO',
        'MARZO',
        'ABRIL',
        'MAYO',
        'JUNIO',
        'JULIO',
        'AGOSTO',
        'SEPTIEMBRE',
        'OCTUBRE',
        'NOVIEMBRE',
        'DICIEMBRE',
    ];

    const ordenMeses = meses.slice(mesInicial).concat(meses.slice(0, mesInicial));

    const columns = [
        {
            name: 'numeroCredito',
            label: 'CONTRATO',
            options: defaultCellOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'tipoCredito',
            label: 'TIPO CRÉDITO',
            options: defaultCellOptions,
        },
        {
            name: 'numeroIdentificacion',
            label: 'IDENTIFICACIÓN',
            options: defaultCellOptions,
        },
        {
            name: 'fechaApertura',
            label: 'FECHA APERTURA',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaFinalizacion',
            label: 'FECHA FINALIZACIÓN',
            options: defaultCellDateOptions,
            formatType: ColumnFormatType.Fecha,
        },
        ...ordenMeses.map((mes) => ({
            name: mes,
            label: mes,
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        })),
        {
            name: 'totalCapital',
            label: 'TOTAL',
            options: defaultCellCurrencyOptions,
            formatType: ColumnFormatType.Decimal,
        },
    ];

    return columns;
};

const TablaReporteCapitalCorriente = ({data, periodo}: { data: DatosReporteCapitalCorriente[], periodo: Date }) => {

    const columns = ObtenerColumnasCapitalCorriente(periodo);

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi  = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip:any) => {
        const  convertIp = ip.split('.')[0]
       const  hexadecimal = convertIp
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
                        enviarIp(ip);
                    } else {
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

    const handleDownload = async () => {
        const format = periodo ? moment(periodo).format('YYYY-MM'): 'Actual';
        const fileName = `Reporte capital corriente - Periodo ${format}.xlsx`;
        await handleExportToExcel(data, columns, fileName, publicIpAddress);
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
                        // customToolbar: () => <DownloadToolbar  downloadAction={handleDownload} />
                    }}
                />
        </div>
    );
}

export default TablaReporteCapitalCorriente;