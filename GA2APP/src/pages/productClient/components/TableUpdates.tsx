import React, { useEffect, useState } from 'react';
import moment from 'moment';
import MUIDataTable, { MUIDataTableColumnDef } from 'mui-datatables';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { toast } from 'react-toastify';
import { Add, AttachMoney, InfoOutlined } from '@material-ui/icons';
import { connect } from 'react-redux';
import { ModalNovedad } from './modalProductUpdates/ModalNovedad';
import { ProductoDto } from '../../../types/producto.model.type';
import ApiproductoClients from '../../../services/api-producto.service';
import UtilService from '../../../utils/util.service';
import NotificationService from '../../../services/notification.service';
import { ModalPagoNovedad } from './modalProductUpdates/ModalPagoNovedad';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import CatalogApi from '../../../services/api-catalog.service';
import { startGetProductoByIdentification, startGetProductosByIdentification } from '../../../actions/producto.action';


const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        columnDate: {
            minWidth: '110px',
            textAlign: 'left'
        },
    }),
);


const TableUpdates = (props: props) => {

    const cartera = ApiproductoClients();

    const { producto,
        idCredito,
        estadoCredito,
        idEstadoCredito,
        productoData,
        clienteRequest,
        startGetProductoByIdentification,
        startGetProductosByIdentification } = props;
    const [modalAgregar, setModalAgregar] = useState(false);
    const [modalPagar, setModalPagar] = useState(false);
    const [selectedNovedad, setSelectedNovedad] = useState<any>(null);
    const [dataNovedadesCartera, setDataNovedadesCartera] = useState<any[]>([]);

    const [tipoNovedades, setDataTipoNovedades] = useState<any[]>([]);
    const [tiposCausales, setTiposCausales] = useState<any[]>([]);
    const [estadosNovedades, setEstadosNovedades] = useState<any[]>([]);

    useEffect(() => {
        obtenerTipoNovedades();
        obtenerEstadoNovedad();
        obtenerTiposCausales();
        obtenerNovedades();
    }, []);


    const classes = useStyles();

    const columns: MUIDataTableColumnDef[] = [
        {
            name: 'tipoNovedad',
            label: 'Tipo de novedad',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'tipoCausal',
            label: 'Causal',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                })
            },
        },
        {
            name: 'fechaNovedad',
            label: 'Fecha de novedad',
            options: {
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas',
                }),
            },
        },
        {
            name: 'fechaFinalizacion',
            label: 'Fecha de finalización',
            options: {
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas',
                }),
            },
        },
        {
            name: 'totalDeuda',
            label: 'Total Deuda',
            options: {
                customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'valorPagado',
            label: 'Valor Pagado',
            options: {
                customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'estadoNovedad',
            label: 'Estado',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: "accion",
            label: 'Acción',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas',
                    style: { textAlign: 'center' }
                }),
                customBodyRender: (_: any, tableMeta: any) => {

                    const novedad = { ...dataNovedadesCartera[tableMeta.rowIndex] };
                    const estado = novedad.codigoEstadoNovedad;

                    return (
                        <div style={{ display: 'flex', gap: '1rem' }}>
                            {
                                estado === 'VIGENTE' &&
                                <Button onClick={() => handleOpenModalPagar(novedad)} variant="contained" startIcon={<AttachMoney />} disabled={novedad.idTipoNovedad === 5}>
                                    Pagar
                                </Button>
                            }
                            <Button onClick={() => handleOpenModalNovedad(novedad)} variant="contained" startIcon={<InfoOutlined />}>
                                Detalle
                            </Button>
                        </div>
                    );
                },
                download: false
            },
        },
    ];

    const obtenerNovedades = () => {
        const request = producto.idCredito;
        cartera
            .getObtenerNovedadesCartera(request)
            .then(async (response) => {
                if (response) {
                    if (response.isSuccess) {
                        setDataNovedadesCartera(response.data);
                    }
                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
    }

    const obtenerTipoNovedades = () => {
        cartera
            .getTipoNovedades()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setDataTipoNovedades(response.data);
                    }
                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
    }

    const obtenerEstadoNovedad = () => {
        cartera
            .getEstadosNovedadCartera()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setEstadosNovedades(response.data);
                    }
                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
    }

    const obtenerTiposCausales = () => {
        cartera
            .getTiposCausalesAceleracionCartera()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setTiposCausales(response.data);
                    }
                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
    }

    const handleOpenModalNovedad = (novedad: any) => {
        setSelectedNovedad(novedad);
        setModalAgregar(true);
    };
    const handleCloseModalNovedad = () => setModalAgregar(false);

    const handleOpenModalPagar = (novedad: any) => {
        setSelectedNovedad(novedad);
        setModalPagar(true);
    };

    const handleCloseModalPagar = () => {
        obtenerNovedades();
        setModalPagar(false);
    };

    const obtenerPlanPago = async () => {
        const requestPlanDePago = {
            nro_Credito: producto.numeroCredito,
            tcr_Id: producto.idTipoCredito,
        };

        try {
            const response = await cartera.getPlanDePagoLeasing(requestPlanDePago);

            if (response && response.isSuccess) {
                return response.data.planPago;
            }
            NotificationService.genericAlert(
                'Error en actualización',
                `${response?.returnMessage || 'Error desconocido'}`,
                'error',
            );
            return null;

        } catch (error) {
            toast("Error del sistema", { type: toast.TYPE.ERROR });
            return null;
        }
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


    const handleAgregarNovedad = async (data: any) => {

        const TIPO_NOVEDAD_OTROS = 6;
        const planPago = await obtenerPlanPago();

        const totalCapitalReducido = planPago
            .filter((cuota: any) => cuota.estadoCuota === "PAGADA")
            .reduce((acc: number, cuota: any) => acc + (cuota.capitalReducido || 0), 0);

        const totalDeudaInteres = planPago
            .filter((cuota: any) => cuota.estadoCuota === "PAGADA")
            .reduce((acc: number, cuota: any) => acc + (cuota.interesPorPagar || 0), 0);

        const idTipoCausal = data.tipoNovedad === 5 ? 4 : data.tipoCausal;
        const valorPagadoCanonInicial = producto.valorVivienda - producto.valorDesembolso - producto.valorCanonInicial
        const valorPagado = producto.valorVivienda - producto.valorDesembolso + totalCapitalReducido
        const valorRestante = valorPagadoCanonInicial + producto.valorCanonInicial + totalCapitalReducido - totalDeudaInteres

        const idTipoCausalFinal = data.tipoNovedad === 2 ? 5 : idTipoCausal;

        const request = {
            SeguroVida: productoData.acumuladoSeguroVida,
            SeguroHogar: productoData.acumuladoSeguroHogar,
            InteresCorriente: productoData.acumuladoInteresCorriente,
            InteresMora: productoData.acumuladoInteresMora,
            InteresRemanente: Number(data.interesRemanente),
            Capital: productoData.saldoCapital,
            CapitalRemanente: productoData.capitalRemanenteCorte,
            totalDeuda: productoData.saldoTotalCorte,
            idCredito: producto.idCredito,
            idTipoNovedad: data.tipoNovedad,
            valorPagado: Number(data.valorPagado?.replace(/[^\d.-]/g, '')),
            ValorRestante: valorRestante,
            fechaNovedad: moment(data.fechaNovedad).startOf('day').format(),
            fechaFinalizacion: moment(data.fechaFinalizacion).startOf('day').format(),
            idTipoCausal: idTipoCausalFinal,
            cuotaCredito: data.cuotaCredito,
            fechaProximoPago: data.tipoNovedad === TIPO_NOVEDAD_OTROS ? data.mesAfectar : null,
            valorNuevaTasa: data.tipoNovedad === TIPO_NOVEDAD_OTROS ? Number(data.nuevaTasa) : null,
            // numeroCuotasAmpliar: data.cuotaCredito,
            porcentajeSeguroVida: Number(data.porcentajeSeguroVida?.replace('%', '')),
            porcentajeSeguroHogar: Number(data.porcentajeSeguroHogar?.replace('%', '')),
            porcentajeTasaNominalMensual: Number(data.porcentajeTasaNominalMensual?.replace('%', '')),

        };


        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string }) =>
            x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )[0]?.brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Novedades de cartera',
            hostname: document.location.host
        } as Auditoria;

        cartera
            .postCrearNovedadCartera(request)
            .then(async (response) => {
                if (response) {
                    if (response.isSuccess) {
                        await NotificationService.genericAlert(
                            'Guardado con éxito',
                            'Se creó novedad',
                        );
                        handleCloseModalNovedad();
                        obtenerNovedades();
                        requestAuditoria.tipoAccion = 'TRANSACCIONES';
                        requestAuditoria.idTipoAccion = 2;
                        requestAuditoria.descripcion = `Agregar novedad parámetros:
                            \n Tipo novedad: ${tipoNovedades.find(x => x.id === request.idTipoNovedad)?.descripcion}
                            \n Fecha novedad: ${moment(request.fechaNovedad).format("DD-MM-YYYY HH:mm")}
                            \n Fecha finalización: ${moment(request.fechaFinalizacion).format("DD-MM-YYYY HH:mm")}
                            \n Tipo causal: ${tiposCausales.find(x => x.id === request.idTipoCausal)?.nombre}`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    } else {
                        await NotificationService.genericAlertError(
                            'Error al crear la novedad',
                            response?.returnMessage ?? "Ha ocurrido un error",
                        );
                        const error = 'Error al crear la novedad';
                        requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                        requestAuditoria.idTipoAccion = 5;
                        requestAuditoria.descripcion = `Agregar novedad parámetros:
                            \n Tipo novedad: ${tipoNovedades.find(x => x.id === request.idTipoNovedad)?.descripcion}
                            \n Fecha novedad: ${moment(request.fechaNovedad).format("DD-MM-YYYY HH:mm")}
                            \n Fecha finalización: ${moment(request.fechaFinalizacion).format("DD-MM-YYYY HH:mm")}
                            \n Tipo causal: ${tiposCausales.find(x => x.id === request.idTipoCausal)?.nombre}
                            \n Error: ${error}
                            \n returnApiMessage: ${response.returnMessage}`;
                        auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                }
            }).catch((error: any) => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion = 5;
                requestAuditoria.descripcion = `Agregar novedad parámetros:
                    \n Tipo novedad: ${tipoNovedades.find(x => x.id === request.idTipoNovedad)?.descripcion}
                    \n Fecha novedad: ${moment(request.fechaNovedad).format("DD-MM-YYYY HH:mm")}
                    \n Fecha finalización: ${moment(request.fechaFinalizacion).format("DD-MM-YYYY HH:mm")}
                    \n Tipo causal: ${tiposCausales.find(x => x.id === request.idTipoCausal)?.nombre}
                    \n Error: ${error}
                    \n returnApiMessage: ${error?.response?.data?.returnMessage}`;
                auditoriaApi.InsertAuditoria(requestAuditoria);
            });
    };

    return (
        <>
            <Grid className={classes.column} item xs={12}>
                <Grid className={classes.column} item xs={1}>
                    <Button
                        size="small"
                        variant="contained"
                        color="primary"
                        className={classes.button}
                        onClick={() => handleOpenModalNovedad(null)}
                    >
                        <Add style={{ marginRight: '0.5rem' }} /> Agregar
                    </Button>
                </Grid>
                <Grid style={{ marginTop: '1rem' }}>
                    <MUIDataTable
                        title=""
                        data={dataNovedadesCartera}
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
                            tableBodyHeight: '470px',
                            rowsPerPageOptions: [5, 15, 20, 100, 200, 500],
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
                        }}
                    />
                </Grid>

            </Grid>
            <ModalNovedad
                open={modalAgregar}
                handleClose={handleCloseModalNovedad}
                handleSubmit={handleAgregarNovedad}
                novedad={selectedNovedad}
                readOnly={!!selectedNovedad}
                {...{ producto, tipoNovedades, tiposCausales, estadosNovedades, idCredito, estadoCredito, idEstadoCredito, productoData }}
            />

            <ModalPagoNovedad
                open={modalPagar}
                handleClose={handleCloseModalPagar}
                novedad={selectedNovedad}
                clienteRequest={clienteRequest}
                idCredito={idCredito}
                startGetProductoByIdentification={startGetProductoByIdentification}
                startGetProductosByIdentification={startGetProductosByIdentification}
                {...{ producto, tipoNovedades, tiposCausales, estadosNovedades, productoData }}
            />
        </>
    );
};
interface LinkMapDispatchToProps {

    producto: ProductoDto;
    idCredito: any;
    estadoCredito: any;
    idEstadoCredito: any;
    productoData: any;
    clienteRequest: any;
}

interface LinkMapDispatchToProps {
    startGetProductoByIdentification: (identificationType: number, idNumber: string, idProducto: any) => void;
    startGetProductosByIdentification: (identificationType: number, idNumber: string) => void;
}

const mapDispatchToProps = {
    startGetProductoByIdentification,
    startGetProductosByIdentification
};

type props = LinkMapDispatchToProps & LinkMapDispatchToProps;

export default connect(null, mapDispatchToProps)(TableUpdates);