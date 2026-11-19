import React, { useEffect, useState } from 'react';
import { Grid, IconButton, TextField, Tooltip } from '@material-ui/core';
import MUIDataTable, { SelectableRows } from 'mui-datatables';
import { toast } from 'react-toastify';
import { AnnouncementOutlined, Delete } from '@material-ui/icons';
import UtilService from '../../../utils/util.service';
import './iconButtonStyle.css';
import handleExportExcelListas from '../../Reports/helpers/handleExportExcelListas';
import DownloadToolbar from './reportes/componentes/DownloadToolbar';
import { ColumnFormatType } from '../../../enums/ColumnFormatType';
import CatalogApi from '../../../services/api-catalog.service';

const TableMassivePayAplicationList = (props: props) => {
    const {
        listState,
        setListState,
        selectableRows,
        onRowsDelete,
        rowsSelected,
        onRowSelectionChange,
        listInfo,
        onDeletePago
    } = props;

    const [page, setPage] = useState<number>(0);
    const [rowPerPage, setRowPerPage] = useState<number>(10);
    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);

    const catalogApi = CatalogApi();

    const handleChangeValorCuota = (rowIndex: number, value: string) => {
        const nuevoValor = Number(value);

        if (Number.isNaN(nuevoValor) || nuevoValor < 0) {
            return;
        }

        const nuevaLista = [...listState];

        nuevaLista[rowIndex] = {
            ...nuevaLista[rowIndex],
            valorCuota: nuevoValor
        };

        setListState(nuevaLista);
    };

    const columns = [
        {
            name: 'fuerzaNombre',
            label: 'Fuerza',
            options: {
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        borderTopLeftRadius: '12px',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'tipoNomina',
            label: 'Nómina',
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
            name: 'codConcepto',
            label: 'Cod Concepto',
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
            name: 'concepto',
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
            name: 'funcionario',
            label: 'Funcionario',
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
            name: 'nroDocumento',
            label: 'Documento',
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
            name: 'codigoMilitar',
            label: 'Codigo Militar',
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
            name: 'valorCuota',
            label: 'Valor Cuota',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRenderLite: (dataIndex: number) => {
        
                    const row = listState[dataIndex];
        
                    const valorActual =
                        row?.valorCuota ??
                        row?.valor_Cuota ??
                        row?.VALOR_CUOTA ??
                        0;
        
                    return (
                        <TextField
                            variant="outlined"
                            size="small"
                            value={UtilService.formatCurrency(valorActual)}
                            inputProps={{
                                style: {
                                    textAlign: 'right',
                                    width: '140px',
                                },
                            }}
                            onMouseDown={(e) => e.stopPropagation()}
                            onClick={(e) => e.stopPropagation()}
                            onKeyDown={(e) => e.stopPropagation()}
                            onChange={(e) => {
        
                                // QUITA TODO LO QUE NO SEA NÚMERO
                                const onlyNumbers = e.target.value.replace(/\D/g, '');
        
                                const numericValue = Number(onlyNumbers || 0);
        
                                const nuevaLista = listState.map((item, index) => {
        
                                    if (index !== dataIndex) {
                                        return item;
                                    }
        
                                    return {
                                        ...item,
        
                                        // VALOR REAL QUE SE ENVÍA
                                        valorCuota: numericValue,
        
                                        // por compatibilidad
                                        valor_Cuota: numericValue,
                                        VALOR_CUOTA: numericValue,
                                    };
                                });
        
                                setListState(nuevaLista);
                            }}
                        />
                    );
                },
        
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
        
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaDesde',
            label: 'Fecha Desde',
            formatType: ColumnFormatType.Fecha,
            options: {
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'fechaHasta',
            label: 'Fecha Hasta',
            formatType: ColumnFormatType.Fecha,
            options: {
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
            },
        },
        {
            name: 'cpgId',
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
                customBodyRender: (value: any) => (
                    <Tooltip title="Eliminar pago de lista">
                        <IconButton
                            onClick={(e) => {
                                e?.stopPropagation();
                                onDeletePago(value);
                            }}
                        >
                            <Delete />
                        </IconButton>
                    </Tooltip>
                )
            },
            download: false,
        },
        {
            name: 'observaciones',
            label: 'Observaciones',
            options: {
                customBodyRender: (value: string | null) => (
                    value &&
                    <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
                        <AnnouncementOutlined />
                        <span>{value}</span>
                    </div>
                ),
                setCellHeaderProps: () => ({
                    style: {
                        backgroundColor: '#FEC917',
                        color: '#676767',
                    },
                }),
                setCellProps: () => ({
                    style: {
                        fontWeight: 'bold',
                        textAlign: 'justify'
                    }
                }),
                download: false,
                display: listState.some(x => x.observaciones)
            },
            download: false,
        },
    ];

    const indexObserv = columns.findIndex(x => x.name === 'observaciones');

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
        const details = [
            listInfo?.unidadEjecutora,
            listInfo?.tipoCredito,
            listInfo?.periodo
        ].filter(x => x != null);

        const format = details.join(' - ');
        const space = details.length ? ' - ' : '';
        const fileName = `REPORTE LISTA${space}${format}.xlsx`;

        await handleExportExcelListas(listState, columns, fileName, publicIpAddress);
    };

    const validateMaxPage = () => {
        const maxPage = Math.max(Math.ceil(listState.length / rowPerPage) - 1, 0);

        if (page > maxPage) {
            setPage(maxPage);
        }
    };

    validateMaxPage();

    return (
        <>
            {listState ? (
                <Grid container>
                    <Grid item xs={12}>
                        <MUIDataTable
                            title=""
                            data={listState}
                            columns={columns}
                            options={{
                                selectableRows: selectableRows,
                                responsive: 'vertical',
                                filter: false,
                                print: false,
                                elevation: 0,
                                fixedHeader: true,
                                fixedSelectColumn: true,
                                tableBodyHeight: '420px',
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
                                        text: 'registro(s) seleccionado(s) para remover del listado'
                                    }
                                },
                                setRowProps: (data) => {
                                    let propsRow = {};

                                    if (data[indexObserv] != null) {
                                        propsRow = {
                                            style: {
                                                background: '#fff7da'
                                            }
                                        };
                                    }

                                    return propsRow;
                                },
                                onRowsDelete: onRowsDelete,
                                onChangePage(currentPage) {
                                    setPage((currentPage < 0) ? 0 : currentPage);
                                },
                                page: page,
                                rowsPerPage: rowPerPage,
                                onChangeRowsPerPage(numberOfRows) {
                                    setRowPerPage(numberOfRows);
                                    validateMaxPage();
                                },
                                rowsSelected: rowsSelected,
                                onRowSelectionChange: onRowSelectionChange,
                                download: false,
                                customToolbar: () => <DownloadToolbar downloadAction={handleDownload} />
                            }}
                        />
                    </Grid>
                </Grid>
            ) : null}
        </>
    );
};

interface LinkMapDispatchToProps {
    listState: any[];
    setListState: React.Dispatch<React.SetStateAction<any[]>>;
    selectableRows?: SelectableRows;
    onRowsDelete?: (
        rowsDeleted: {
            lookup: { [dataIndex: number]: boolean };
            data: Array<{ index: number; dataIndex: number }>;
        },
        newTableData: any[],
    ) => void | false;
    rowsSelected?: any[];
    onRowSelectionChange?: (rowsSelectedData: any, allRows: any, rowsSelected: any) => void;
    listInfo?: {
        unidadEjecutora?: string;
        tipoCredito?: string;
        periodo?: string;
    } | null;
    onDeletePago: (data: any) => void;
}

type props = LinkMapDispatchToProps;

export default TableMassivePayAplicationList;