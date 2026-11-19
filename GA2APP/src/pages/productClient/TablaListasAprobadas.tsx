import React, { useEffect, useState } from 'react';
import MUIDataTable, { SelectableRows } from 'mui-datatables';
import { toast } from 'react-toastify';
import { IconButton, Tooltip } from '@material-ui/core';
import { Delete } from '@material-ui/icons';
import UtilService from "../../utils/util.service";
import { ColumnFormatType } from '../../enums/ColumnFormatType';
import DownloadToolbar from './components/reportes/componentes/DownloadToolbar';
import handleExportToExcel from '../Reports/helpers/handleExportToExcel';
import handleListaAprobada from '../Reports/helpers/handleListaAprobada';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuthService from '../../services/auth.service';
import CatalogApi from '../../services/api-catalog.service';
import AuditoriaApi from '../../services/api-auditorias.servise';

const TablaListasAprobadas = (props: any) => {

    const { rowsSelected, onRowSelectionChange, listState, onDeleteProceso } = props;

    const columns = [
        {
            name: 'fecha_Pago_Cuota',
            label: 'Fecha Aplicación',
            formatType: ColumnFormatType.Fecha,
            options: {
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                        borderTopLeftRadius: '12px',
                    },
                }),
            },
        },
        {
            name: 'estado',
            label: 'Estado',
            options: {
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'periodo',
            label: 'Periodo',
            formatType: ColumnFormatType.Numerico,
            options: {
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'unidadEjecutora',
            label: 'Unidad Ejecutora',
            options: {
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
            label: 'Concepto',
            options: {
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'total',
            label: 'Total',
            formatType: ColumnFormatType.Decimal,
            options: {
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
            },
        },
        {
            name: 'idProceso',
            label: 'Acciones',
            options: {
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                        borderTopRightRadius: '12px',
                    },
                }),
                download: false,
                filter: false,
                empty: true,
                customBodyRender: (value: any, tableMeta: any, updateValue: any) => 
                    <Tooltip title="Eliminar lista Aprobada">
                        <IconButton onClick={(e) => {
                            e?.stopPropagation();
                            onDeleteProceso(value);
                        }} >
                            <Delete/>
                        </IconButton>
                    </Tooltip>
            },
            download: false,
        },
    ];

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
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
        const fileName = `LISTA APROBADAS.xlsx`;
        await handleListaAprobada(listState, columns, fileName, publicIpAddress);

    }

    return <MUIDataTable
            title=""
            data={listState}
            columns={columns}
            options={{
                // selectableRows: selectableRows,
                selectableRows: 'single',
                responsive: 'vertical',
                selectToolbarPlacement: 'none',
                selectableRowsHideCheckboxes: true,
                selectableRowsOnClick: true,
                filter: false,
                print: false,
                elevation: 0,
                fixedHeader: true,
                fixedSelectColumn: true,
                tableBodyHeight: '420px',
                rowsPerPage: 5,
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
                    selectedRows: {
                        text: ""
                    }
                },
                rowsSelected: rowsSelected,
                onRowSelectionChange: onRowSelectionChange,
                download: false,
                customToolbar: () => <DownloadToolbar  downloadAction={handleDownload} />
            }}
        />
}


export default TablaListasAprobadas;