import React, { useEffect, useState } from "react";
import { makeStyles, createStyles, Tooltip, CircularProgress, IconButton } from "@material-ui/core";
import MUIDataTable from "mui-datatables";
import { toast } from "react-toastify";
import { CloudDownload } from "@material-ui/icons";
import UtilService from "../../../../../utils/util.service";
import { ColumnFormatType } from "../../../../../enums/ColumnFormatType";
import handleExportToExcel from "../../../../Reports/helpers/handleExportToExcel";
import { ReporteEstadoCuentaPlanPago } from "../types/ReporteEstadoCuentaPlanPago.type";
import ReporteEstadoCuentaAfiliado from "../types/ReporteEstadoCuentaAfiliado.type";
import CatalogApi from "../../../../../services/api-catalog.service";

interface tableProps {
    data: ReporteEstadoCuentaPlanPago[],
    estadoCuenta: ReporteEstadoCuentaAfiliado,
    isLoading?: boolean
}

const useStyles = makeStyles(() =>
    createStyles({
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        columnDate: {
            minWidth: '110px',
        },

        tablaCierre: {
            overflow: 'auto !important'
        },
        buttonsecundario: {
            marginLeft: '1rem',
            marginBottom: '0.5rem',
            background: '#43b5f1',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
        },
        nombreCuentaContable: {
            minHeight: '200px'
        },
        errorMessage: {
            color: 'red'
        },
        summaryCell: {
            textAlign: 'right',
            fontWeight: 'bold',
        },
        summaryRow: {
            backgroundColor: '#f1f1f1'
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr auto',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        },
        totales: {
            textAlign: 'right'
        }
    }),
);

const TablaReportePlanPago = (props: tableProps) => {

    const { isLoading, data, estadoCuenta } = props;
    const classes = useStyles();
    const [downloading, setDownloading] = useState(false);

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

    const columns = [
        {
            name: 'numeroCuota',
            label: 'Num. Cuota',
            options: defaultOptions,
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'fechaPago',
            label: 'Fecha Cuota',
            options: defaultOptionsDate,
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'capital',
            label: 'Capital',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'capitalPorPagar',
            label: 'Capital Remanente',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'intereses',
            label: 'Intereses',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'interesesPorPagar',
            label: 'Interés Remanente',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'seguroHogar',
            label: 'Seguro Hogar',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'seguroVida',
            label: 'Seguro Vida',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'valorAPagar',
            label: 'Valor a Pagar',
            options: defaultOptionsCurrency,
            formatType: ColumnFormatType.Decimal,
        },
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
        const dataCliente = `${estadoCuenta.tipoCredito} - ${estadoCuenta.numeroCredito}`;
        const fileName = `Reporte plan pago ${dataCliente}.xlsx`;
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

    return (
        <div className='MuiTableCell-stickyHeader' >  
            <MUIDataTable
                    title=""
                    data={data}
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
                        rowsPerPageOptions: [10, 20, 50, 120, 180],
                        textLabels: {
                            body: {
                                noMatch: isLoading
                                    ? <CircularProgress />
                                    : 'No se encontraron resultados',
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
        </div>
    );
}

export default TablaReportePlanPago;