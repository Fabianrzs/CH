import React, { useEffect, useState } from "react";
import { makeStyles, createStyles, Tooltip, CircularProgress, IconButton, Button } from "@material-ui/core";
import { toast } from "react-toastify";
import MUIDataTable, { MUIDataTableColumn } from "mui-datatables";
import { CloudDownload } from "@material-ui/icons";
import UtilService from "../../../../../utils/util.service";
import { ColumnFormatType } from "../../../../../enums/ColumnFormatType";
import handleDownloadEstadoCuentas from "../../../../Reports/helpers/handleDownloadEstadoCuentas";
import ReporteEstadoCuentaAfiliado from "../types/ReporteEstadoCuentaAfiliado.type";
import { ModalReporteDetalles } from "./modal/ModalReporteDetalles";
import AuthService from "../../../../../services/auth.service";
import { Auditoria } from "../../../../../types/Auditoria.model.type";
import CatalogApi from "../../../../../services/api-catalog.service";
import AuditoriaApi from '../../../../../services/api-auditorias.servise';


type Columns = MUIDataTableColumn & {formatType?: ColumnFormatType, label: string, download?: boolean };

interface tableProps {
    tableData: ReporteEstadoCuentaAfiliado[]
}

const useStyles = makeStyles(() =>
    createStyles({
        columnDate: {
            minWidth: '110px',
        },
        buttonsecundario: {
            marginLeft: '1rem',
            marginBottom: '0.5rem',
            background: '#43b5f1',
        },
    }),
);

const TablaReporteCreditos = (props: tableProps) => {

    const { tableData } = props;
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);
    const [openModalDetalles, setOpenModalDetalles] = useState(false);
    const [selectedReport, setSelectedReporte] = useState<ReporteEstadoCuentaAfiliado>();

    const defaultOptions = {
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
    };
    
    const defaultOptionsDate = {
        setCellProps: () => ({
            className: classes.columnDate,
            style: {
                textAlign: 'left',
            },
            
        }),
        customBodyRender: (value: Date) => UtilService.getDate(value),
        setCellHeaderProps: () => ({
            className: 'filas'
        }),
       
    };

    const defaultOptionsPercent = {
        customBodyRender: (value: number) => UtilService.formatPercent(value),
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
            className: 'filas'
        }),
    };

    const columns: Columns[] = [
        {
            name: 'estado',
            label: 'Estado',
            options: defaultOptions
        },
        {
            name: 'numeroCredito',
            label: 'Radicación',
            options: defaultOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'lineaCredito',
            label: 'Línea Crédito',
            options: defaultOptions
        },
        {
            name: 'numeroIdentificacion',
            label: 'Identificación',
            options: defaultOptions
        },
        {
            name: 'nombreAfiliado',
            label: 'Afiliado',
            options: {
                ...defaultOptions,
                setCellProps: () => ({
                    style: {
                        minWidth: '200px',
                    },
                }),
            }
        },
        {
            name: 'unidadEjecutora',
            label: 'Pagaduría',
            options: defaultOptions
        },
        {
            name: 'fechaDesembolso',
            label: 'Solicitud',
            options: defaultOptionsDate,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'moneda',
            label: 'Moneda',
            options: defaultOptions,
        },
        {
            name: 'valorDesembolso',
            label: 'Monto Solicitado',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'saldo',
            label: 'Saldo',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorCuota',
            label: 'Cuota',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'plazoTotal',
            label: 'Plazo',
            options: defaultOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'formatoPlazo',
            label: 'Formato Plazo',
            options: defaultOptions,
        },
        {
            name: 'formatoPago',
            label: 'Formato Pago',
            options: defaultOptions,
        },
        {
            name: 'fechaUltimoPago',
            label: 'Ultimo Pago',
            options: defaultOptionsDate,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaProximoPago',
            label: 'Próximo Pago',
            options: defaultOptionsDate,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'tasaCreditoEA',
            label: 'Tasa Efectiva',
            options: defaultOptionsPercent,
            formatType: ColumnFormatType.Porcentaje,
        },
        {
            name: 'tasaCreditoNOM',
            label: 'Tasa Nominal',
            options: defaultOptionsPercent,
            formatType: ColumnFormatType.Porcentaje,
        },
        {
            name: 'modalidad',
            label: 'Modalidad',
            options: defaultOptions,
        },
        {
            name: 'idCredito',
            label: 'Opciones',
            download: false,
            options: {...defaultOptions, 
                customBodyRender(value, tableMeta, updateValue) {
                    return (
                        <>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={() => verDetalles(value)}
                            >
                                Detalles
                            </Button>
                        </>
                    );
                },
            },
            
        }
    ];

    const verDetalles = (idCredito: string) => {
        const data = tableData.find(x => x.idCredito === idCredito);
        setSelectedReporte(data);
        setOpenModalDetalles(true);
    };

    const handleDownload = async () => {
        const fileName = `Reporte estado de cuentas afiliados.xlsx`;
        await handleDownloadEstadoCuentas(tableData, columns, fileName);
    }

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
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

    const handleClick = async () => {
        const nav = (window.navigator as any);

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const usrAg = nav.userAgentData?.brands?.filter((x: { brand: string; }) =>
            x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0]?.brand ?? "Desconocido";
    
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Estado de cuenta y movimientos',
                hostname: document.location.host
            } as Auditoria
    
        try {
            setDownloading(true);
            await handleDownload();
    
       
            requestAuditoria.tipoAccion = 'CONSULTAS';
            requestAuditoria.idTipoAccion = 3;
            requestAuditoria.descripcion = 'Consulta exitosa del reporte estado de cuentas afiliados.';
            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error) {

            requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion = 8;
            requestAuditoria.descripcion = `Error al descargar el reporte estado de cuentas afiliados.\nError: ${error}`;
            auditoriaApi.InsertAuditoria(requestAuditoria);
        } finally {
            setDownloading(false);
        }
    };
    

    const handleCloseModal = () => {
        setOpenModalDetalles(false);
        setSelectedReporte(undefined);
    }

    return (
        <div className='MuiTableCell-stickyHeader; jss86'>  
            <MUIDataTable
                    title=""
                    data={tableData}
                    columns={columns}
                    options={{
                        selectableRows: 'none',
                        filter: false,
                        print: false,
                        elevation: 2,
                        filterType: 'dropdown',
                        responsive: 'standard',
                        fixedHeader: true,
                        fixedSelectColumn: true,
                        tableBodyHeight: '600px',
                        rowsPerPageOptions: [20, 100, 200, 500 ],
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
                        )
                    }}
            />
            {
                selectedReport &&
                <ModalReporteDetalles open={openModalDetalles} data={selectedReport} handleClose={handleCloseModal}/>
            }
        </div>
    );
}

export default TablaReporteCreditos;