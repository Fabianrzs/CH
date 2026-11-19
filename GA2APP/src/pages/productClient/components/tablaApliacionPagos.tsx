
import { Grid, Table, TableRow, TableHead, TableCell, TableBody, makeStyles, createStyles, Button, Tooltip, IconButton } from "@material-ui/core";
import MUIDataTable from "mui-datatables";
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import convert from 'convert-string-bytes';
import saveAs from 'file-saver';
import React, { useEffect, useState } from "react";
import moment from "moment";
import { CancelRounded, CancelScheduleSendSharp, CancelSharp, CloudDownload } from "@material-ui/icons";
import { connect } from "react-redux";
import { AplicacionPago, ProductoDto } from "../../../types/producto.model.type";
import ApiproductoClients from '../../../services/api-producto.service'
import UtilService from "../../../utils/util.service";
import { dataExcel } from "../../Reports/helpers/HelperExcel";
import NotificationService from '../../../services/notification.service';
import ApiReportes from "../../../services/api-reportes.service";
import { startGetAllAplicacionPago, startGetProductoByIdentification } from "../../../actions/producto.action";
import { LoginState } from "../../../reducers/login.reducer";
import { AppState } from "../../../store/rootReducer";
import { ProductState } from "../../../reducers/producto.reducer";
import { ColumnFormatType } from "../../../enums/ColumnFormatType";
import AuthService from "../../../services/auth.service";
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from "../../../types/Auditoria.model.type";
import CatalogApi from "../../../services/api-catalog.service";

const useStyles = makeStyles(() =>
    createStyles({
        columnDate: {
            minWidth: '100px',
        },

        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
        },
    }),
);



const PayAplicationTable = (props: any) => {

    const {
        producto,
        pagos,
        startGetAllAplicacionPago,
        startGetProductoByIdentification,
        productoState,
        productosSelec,
        catalogState

    } = props;




    const apiReportes = ApiReportes();
    const api = ApiproductoClients()
    const classes = useStyles();
    const columns = [
        {

            name: 'fechaCreacion',
            label: 'Fecha de Creacion',
            formatType: ColumnFormatType.Fecha,
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },

                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },
        {

            name: 'fechaPago',
            label: 'Fecha de pago',
            formatType: ColumnFormatType.Fecha,
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },

                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },
        {

            name: 'fechaAplicacion',
            label: 'Fecha de Cuota',
            formatType: ColumnFormatType.Fecha,
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },

                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },

        {

            name: 'comprobanteInterno',
            label: 'Comprobante Interno',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                    className: classes.columnDate,
                }),
                customBodyRender: (value: string) => value || 'N/A',
            },
        },
        {

            name: 'comprobanteSeven',
            label: 'Comprobante Seven',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                    className: classes.columnDate,
                }),
                customBodyRender: (value: any) => value === 0 ? 'N/A' : value,
            },
        },
        {

            name: 'tipoPago',
            label: 'Tipo Pago',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                    className: classes.columnDate,



                }),
                customBodyRender: (value: string) => (value),
            },
        },
        {

            name: 'modalidadPago',
            label: 'Modalidad Pago',
            options: {
                width: 1,
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },


                }),
                customBodyRender: (value: string) => (value),

            },
        },
        {
            name: 'valorPago',
            label: 'Valor Pagado',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'abonoCapital',
            label: 'Capital',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'abonoInteresCorriente',
            label: 'Interes Corriente',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'abonoCapitalMora',
            label: 'Capital Mora',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'abonoInteresMora',
            label: 'Interes Mora',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'abonoSeguroVida',
            label: 'Seguro de Vida',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'abonoSeguroHogar',
            label: 'Seguro de Hogar',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'capitalRemanente',
            label: 'Capital Remanente',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'interesRemanente',
            label: 'Interés Remanente',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: any) =>
                    UtilService.formatCurrencyTres(Number(value) || 0),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {

            name: 'tipoRecurso',
            label: 'Origen de Pago',
            options: {
                width: 1,
                setCellProps: () => ({
                    style: {textAlign: 'left',},
                }),
                customBodyRender: (value: string) => {

                    if(value === 'RECURSOS_PROPIOS'){
                        return 'Recursos Propios'
                    }
                    if(value === 'CUENTA_INDIVIDUAL'){
                        return 'Cuenta Individual'
                    }
                    return  'N/A'
                },

            },
        },
        {
            name: "Accion",
            label: 'ACCIÓN',
            options: {
                customBodyRender: (value: any, tableMeta: any, rowData: any) => {
                    const pago = pagos[tableMeta.currentTableData[tableMeta.rowIndex].index]
                    if (pago.esPagoReversible) {
                        return (
                            <>
                                <Tooltip title="Reversar">
                                    <IconButton
                                        color="secondary"
                                        onClick={() => handleReversarPago(pago, "Reversar")}
                                    >
                                        <CancelRounded />
                                    </IconButton>
                                </Tooltip>
                                {/* <Button
                            onClick={() => CrearMovContableSeven(pago)}
                            variant="contained"
                            color="primary"
                          >
                            SEVEN
                          </Button> */}
                            </>
                        );
                    }
                    return null;
                },
            },
        },

    ]

    const productos = productoState.productos.find((objeto: any) => objeto.idCredito === productosSelec.idCredito);
    const opcionesPago = catalogState.Pay


    const CrearMovContableSeven = async (data: any) => {
        const validaProductoCliente: any = productos
        const codigoPago = opcionesPago.find((index: any) => index.nombre === data.modalidadPago);
        const request = {
            aplicaSimulacion: false,
            numeroCredito: validaProductoCliente.numeroCredito,
            tipoIdentificacion: Number(validaProductoCliente.tipoIdentificacion),
            numeroDocumentoCliente: validaProductoCliente.identificacion,
            cpgid: data.idAplicacionPago,
            codigo: codigoPago.codigo,
            nCuota: 1,
            tipoCredito: validaProductoCliente.idTipoCredito,
        }
        await api
            .postCreaSCnMcontCrearCuotaCartera(request)
            .then(async response => {
                const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                const errorSeven = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);

                if (retornoExitoso[1] === '0') {
                    toast('Pago por ventanilla a Seven EXITOSO', { type: toast.TYPE.SUCCESS });
                } else {
                    toast(`Pago ventanilla FALLO: ${errorSeven[1]}`, { type: toast.TYPE.ERROR });
                }

            })
            .catch((error) => {
                if (error.response && error.response.data) {
                    toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR, autoClose: 10000 });
                } else {
                    toast("Error del sistema", { type: toast.TYPE.ERROR });
                }
            });
    };

    const anularCuotaCartera = (rowData: any) => {
        const request = {
            numeroCredito: Number(producto.numeroCredito),
            cpgId: String(rowData.idAplicacionPago)
        }
        api
            .postAnularCuotaCartera(request)
            .then(response => {
                if (response.isSuccess) {
                    const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                    const errorSeven = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);

                    if (retornoExitoso[1] === "0") {
                        toast('Anulacion de movimiento exitosa', { type: toast.TYPE.SUCCESS });

                    } else {
                        // toast(`Error:${errorSeven}`, { type: toast.TYPE.ERROR });
                        toast(`Anulacion de movimiento FALLO: ${errorSeven[1]}`, { type: toast.TYPE.ERROR, autoClose: 10000 });

                    }
                }
            })
            .catch((error) => {
                if (error.response && error.response.data) {
                    toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR, autoClose: 10000 });
                } else {
                    toast("Error del sistema", { type: toast.TYPE.ERROR });
                }
            });
    }

    const handleReversarPago = async (rowData: any, buttonText: string) => {

        const requestReversionPago = {
            IdPago: rowData.idAplicacionPago,
            Estado_Id: rowData.estadoComprobante,
            Comprobante_Id: rowData.comprobanteInterno
        }
        swal({
            title: 'Reversar',
            text: '¿Está seguro de reversar el pago?',
            icon: 'warning',
            buttons: ['No', 'Sí'],
        }).then(respuesta => {
            if (respuesta) {
                api.reversarPago(requestReversionPago)
                    .then(async (response) => {
                        if (response) {
                            if (response.isSuccess) {
                                // anularCuotaCartera(rowData)
                                startGetAllAplicacionPago(producto.idCredito);
                                startGetProductoByIdentification(producto.tipoIdentificacion
                                    , producto.identificacion)

                                await NotificationService.genericAlert(
                                    'Reversión',
                                    'La reversión se realizó correctamente',
                                );

                            } else {
                                NotificationService.genericAlert(
                                    'Error en el proceso de reversión',
                                    `${response.returnMessage}`,
                                    'warning',
                                )
                            }
                        }
                    }).catch(() => {
                        toast("Error del sistema", { type: toast.TYPE.ERROR });
                    });
            }
        });
    };

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

    const handleExportToExcel = async () => {

        const columnsReport = columns.filter(x => x.name !== "Accion");

        const columnDataList = pagos?.map((row: any) => ({
            data: columnsReport.map(x => {

                if (x.formatType === ColumnFormatType.Fecha && row[x.name]) {
                    const parseDate = moment(row[x.name]);
                    return parseDate.isValid()
                        ? parseDate.format("YYYY-MM-DD")
                        : '';
                }

                return row[x.name]?.toString() ?? ''
            })
        }));

        if (columnDataList) {

            const request = {
                columnData: columnDataList.map((item: any) => item.data),
                columnsInfo: columnsReport.map(x => ({
                    name: x.label,
                    formatType: x.formatType
                }))
            };

            const response = await apiReportes.GenerarExcelInvariantCulture(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Historial Pago - ${producto.nombreCliente} - Fecha reporte ${moment().format('YYYY-MM-DD HH_mm_ss')}.xlsx`;
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(link);
            const user = AuthService.getUserInfo();
            const auditoriaApi = AuditoriaApi();
            const nav = (window.navigator as any);
            const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de cartera',
                hostname: document.location.host
            } as Auditoria;
            requestAuditoria.tipoAccion = 'DESCARGAS';
            requestAuditoria.idTipoAccion = 5;
            requestAuditoria.descripcion = `Descarga de reporte historial de pagos en formato excel:
                         \n archivo: ${link.download}`;
            await auditoriaApi.InsertAuditoria(requestAuditoria);
        }
    }

    return (

        <>

            <div
                style={{
                    display: 'table',
                    tableLayout: 'fixed',
                    width: '100%',

                }}>



                <MUIDataTable

                    title="HISTORIAL DE PAGOS"
                    data={pagos}
                    columns={columns}

                    options={{

                        selectableRows: 'none',
                        filter: false,
                        print: false,
                        elevation: 2,
                        filterType: 'dropdown',
                        responsive: 'vertical',
                        fixedHeader: true,
                        fixedSelectColumn: true,
                        tableBodyHeight: '430px',
                        rowsPerPageOptions: [5, 10, 15, 20, 100, 200],
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
                            <>
                                <Tooltip title="Descargar Excel">
                                    <IconButton onClick={() => handleExportToExcel()} >
                                        <CloudDownload />
                                    </IconButton>
                                </Tooltip>
                            </>
                        )
                    }}
                />
            </div>

        </>
    );
}


// producto: any;
// pagos: any;
// startGetAllAplicacionPago: (idCredito: string) => void;
// startGetProductoByIdentification: (identificationType: number, idNumber: string) => void;
interface LinkMapStateToProps {
    loginState: LoginState;
    productoState: ProductState;
}

const mapStateToProps = (state: AppState) => ({
    productoState: state.producto,
    loginState: state.login,
    catalogState: state.catalog
});

interface LinkMapDispatchToProps {
    loginState: LoginState;
    producto: ProductoDto;
    startGetProductoByIdentification: (identificationType: number, idNumber: string) => void;
    startGetAllAplicacionPago: (idCredito: string) => void;

}

const mapDispatchToProps = {
    startGetAllAplicacionPago,
    startGetProductoByIdentification,
};

type props = LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(PayAplicationTable);

