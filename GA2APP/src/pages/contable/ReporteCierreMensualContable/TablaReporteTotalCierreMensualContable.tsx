import React, { useEffect, useState } from 'react';
import { CircularProgress, IconButton, Tooltip } from '@material-ui/core';
import { toast } from 'react-toastify';
import MUIDataTable from 'mui-datatables';
import moment from 'moment';
import { CloudDownload } from '@material-ui/icons';
import { TotalReporteCierreMensualContable } from '../types/TotalReporteCierreMensualContable';
import { ColumnFormatType } from '../../../enums/ColumnFormatType';
import handleExportToExcel from '../../Reports/helpers/handleExportToExcel';
import UtilService from '../../../utils/util.service';
import CatalogApi from '../../../services/api-catalog.service';

interface tableProps {
    data: TotalReporteCierreMensualContable[],
    formData?: {
        periodo: Date,
        tipoCredito: number
    },
}

const TablaReporteTotalCierreMensualContable = (props: tableProps) => {

    const { data, formData } = props;
    const [downloading, setDownloading] = useState(false);

    const defaultOptions = {
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
                className: 'filas',
            }),
    };

    const columns = [
        {
            name: 'tipoOperacion',
            label: 'Tipo Operación',
            options: defaultOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'descripcionOperacion',
            label: 'Descripción Operación',
            options: defaultOptions
        },
        {
            name: 'debito',
            label: 'Débito',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'credito',
            label: 'Crédito',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        }
    ];

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
        const formatFecha = moment(formData?.periodo).format('YYYY-MM');
        const fileName = `Reporte total cierre contable - Fecha ${formatFecha}.xlsx`;
        await handleExportToExcel(data, columns, fileName, publicIpAddress);
    }

    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleDownload();
        } finally {
            setDownloading(false);
        }
    }
    return (<div className='MuiTableCell-stickyHeader; jss86'>
            <MUIDataTable
                    title="Total Cierres"
                    data={data}
                    columns={columns}
                    options={{
                        selectableRows: 'none',
                        filter: false,
                        print: false,
                        elevation: 1,
                        filterType: 'dropdown',
                        responsive: 'vertical',
                        fixedHeader: true,
                        fixedSelectColumn: true,
                        tableBodyHeight: '400px',
                        rowsPerPageOptions: [10, 50, 100],
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
                        customToolbar: () => (
                            <Tooltip title="Descargar Excel">
                                {
                                    downloading
                                        ? <CircularProgress size={20} style={{'verticalAlign': 'middle', 'marginLeft': '14px', 'marginRight': '14px'}}/>
                                        :  <IconButton onClick={handleClick} >
                                                <CloudDownload/>
                                            </IconButton>
                                }
                            </Tooltip>
                        ),
                    }}
                />
        </div>);
}

export default TablaReporteTotalCierreMensualContable;