import React, { useEffect, useState } from 'react';
import {
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
    Paper,
    Grid,
    Button,
    Checkbox,
    makeStyles,
    Theme
} from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { toast } from 'react-toastify';
import UtilService from '../../utils/util.service';
import Prospeccion from '../../services/api-prospeccion.service';
import AuthService from '../../services/auth.service';
import ApiReportes from '../../services/api-reportes.service';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import ModalPazSalvo from './ModalPazSalvo';

import CreditApi from '../../services/api-credit.service';



const useStyles = makeStyles((theme: Theme) => ({
    root: {
        width: '100%',
        '& .MuiAccordion-root': {
            marginBottom: 0,
        },
    },
    firstAccordionTab: {
        color: '#ffffff',
        backgroundColor: '#1C3F8B',
        fontSize: '100',
        flexDirection: 'row-reverse',
        '&:hover': {
            backgroundColor: '#fec917',
        }
    },
    fiveAccordionTab: {
        color: '#ffffff',
        backgroundColor: '#1C3F8B',
        flexDirection: 'row-reverse',
        '&:hover': {
            backgroundColor: '#fec917',
        },
    },
    heading: {
        fontSize: theme.typography.pxToRem(15),
    },
    scoringContainer: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
    },
    scoringText: {
        marginLeft: 4,
        fontWeight: 600,
    },
}));


const apicredito = CreditApi()
const AcordionProspeccionTablas = ({
    data,
    data1,
    datosCartera,
    volverEnviarBusqueda,
    mostrarPdf,
    listaDatosProspeccion
}: {
    data: any;
    data1: any;
    datosCartera: any;
    volverEnviarBusqueda: any;
    mostrarPdf: any;
    listaDatosProspeccion: any
}) => {
    const apiProspeccion = Prospeccion();
    const apiReportes = ApiReportes();
    const [arregloSeguros, setArregloSeguros] = useState<any[]>([])
    // const [nuevaData1, setNuevaData1] = useState<any[]>(data1)
    const [loading, setLoading] = useState(false)
    const [scoringValidado, setScoringValidado] = useState(false)
    const [cargandoScoring, setCargandoScoring] = useState(false)
    const [guardandoScoring, setGuardandoScoring] = useState(false)
    const [modalPazSalvoAbierto, setModalPazSalvoAbierto] = useState(false)
    const [obligacionSeleccionada, setObligacionSeleccionada] = useState<any>(null)
    const [cargandoPazSalvo, setCargandoPazSalvo] = useState(false)

    const user = AuthService.getUserInfo();

    useEffect(() => {
        const newData = [...datosCartera];
        const newDataWithColumn = newData.map((item) => {
            const arregloDatos = {
                ...item,
                columnaFactorPeriodico: (item.seguro / 12).toFixed(6),
                columnaFactorIva: ((item.seguro / 12) * 1.19).toFixed(6)
            };
            return arregloDatos;
        });
        setArregloSeguros(newDataWithColumn)
    }, [datosCartera]);


    const obtenerNumeroDocumento = () => {
        if (data?.[3]?.numeroDocumento) {
            return String(data[3].numeroDocumento);
        }

        if (data?.[3]?.idNumber) {
            return String(data[3].idNumber);
        }

        if (data?.[0]?.numerO_DOCUMENTO) {
            return String(data[0].numerO_DOCUMENTO);
        }

        if (data?.[1]?.numeroDocumento) {
            return String(data[1].numeroDocumento);
        }

        return '';
    };

  
    const buscarIdSolicitudCredito = (objeto: any): string => {
        if (!objeto || typeof objeto !== 'object') {
            return '';
        }
    
        const posiblesCampos = [
            'idSolicitudCredito',
            'idSolicitud',
            'solicitudCreditoId',
            'id_solicitud_credito',
            'iD_SOLICITUD_CREDITO',
            'idSolicitudCred',
            'solicitudId',
        ];
    
        const campoEncontrado = posiblesCampos.find(
            (campo) =>
                objeto[campo] !== undefined &&
                objeto[campo] !== null &&
                String(objeto[campo]).trim() !== '',
        );
    
        if (campoEncontrado) {
            return String(objeto[campoEncontrado]);
        }
    
        const valores = Object.values(objeto);
    
        const resultado = valores
            .map((valor) => buscarIdSolicitudCredito(valor))
            .find((valor) => Boolean(valor));
    
        return resultado || '';
    };
    
    const obtenerIdSolicitudCredito = () => {
        const idDesdeObligacion = buscarIdSolicitudCredito(
            obligacionSeleccionada,
        );
    
        if (idDesdeObligacion) {
            return idDesdeObligacion;
        }
    
        const idDesdeProspeccion = buscarIdSolicitudCredito(
            listaDatosProspeccion,
        );
    
        if (idDesdeProspeccion) {
            return idDesdeProspeccion;
        }
    
        const idDesdeData = buscarIdSolicitudCredito(data);
    
        if (idDesdeData) {
            return idDesdeData;
        }
    
        return '';
    };

    const generarFechaCreacion = () => {
        const fecha = new Date();
        const offset = fecha.getTimezoneOffset();
        const signo = offset > 0 ? '-' : '+';
        const offsetAbsoluto = Math.abs(offset);
        const horasOffset = String(Math.floor(offsetAbsoluto / 60)).padStart(2, '0');
        const minutosOffset = String(offsetAbsoluto % 60).padStart(2, '0');
        const fechaLocal = new Date(fecha.getTime() - offset * 60 * 1000)
            .toISOString()
            .slice(0, 19);

        return `${fechaLocal}${signo}${horasOffset}:${minutosOffset}`;
    };

    const consultarValidacionCreditoProspeccion = async () => {
        const numeroDocumento = obtenerNumeroDocumento();
    
        if (!numeroDocumento) {
            setScoringValidado(false);
            return;
        }
    
        setCargandoScoring(true);
    
        try {
            const response =
                await apicredito
                    .getConsultarValidacionCreditoProspeccion(
                        numeroDocumento,
                    );
    
            if (response?.isSuccess && response?.data) {
                setScoringValidado(
                    Boolean(response.data.estado),
                );
    
                return;
            }
    
            setScoringValidado(false);
        } catch (error) {
            setScoringValidado(false);
    
            toast(
                'No se pudo consultar la validación del scoring',
                {
                    type: toast.TYPE.ERROR,
                },
            );
        } finally {
            setCargandoScoring(false);
        }
    };

    useEffect(() => {
        consultarValidacionCreditoProspeccion();
    }, [data]);

    const cambiarEstadoScoring = async (
        event: React.ChangeEvent<HTMLInputElement>,
    ) => {
        const numeroDocumento = obtenerNumeroDocumento();
        const nuevoEstado = event.target.checked;
        const estadoAnterior = scoringValidado;
    
        if (!numeroDocumento) {
            toast('No se encontró el número de documento del afiliado', {
                type: toast.TYPE.WARNING,
            });
    
            return;
        }
    
        setScoringValidado(nuevoEstado);
        setGuardandoScoring(true);
    
        try {
            const payload = {
                numeroDocumento,
                estado: nuevoEstado,
            };
    
            const response =
                await apicredito
                    .postInsertarActualizarValidacionCreditoProspeccion(
                        payload,
                    );
    
            if (response?.isSuccess) {
                setScoringValidado(
                    Boolean(response?.data?.estado),
                );
    
                toast(
                    response.returnMessage ||
                        'Validación actualizada correctamente',
                    {
                        type: toast.TYPE.SUCCESS,
                    },
                );
    
                return;
            }
    
            setScoringValidado(estadoAnterior);
    
            toast(
                response?.returnMessage ||
                    'No se pudo actualizar la validación del scoring',
                {
                    type: toast.TYPE.WARNING,
                },
            );
        } catch (error) {
            setScoringValidado(estadoAnterior);
    
            toast(
                'Error al actualizar la validación del scoring',
                {
                    type: toast.TYPE.ERROR,
                },
            );
        } finally {
            setGuardandoScoring(false);
        }
    };


  

    const classes = useStyles();
    const generarPdf = async () => {
        setLoading(true)
        const tiposDocumento = data.filter((x: any) => x.numeroDocumento && x.tipoDocumento)
        const request = {
            tipoDocumento: Number(tiposDocumento[0].tipoDocumento),
            numeroDocumento: tiposDocumento[0].numeroDocumento,
            USR_ID: user.USR_ID,
        }

        await apiReportes
            .postReporteProspeccionReclasificacion(request)
            .then(async response => {
                if (response?.type === "application/pdf") {
                    const url = URL.createObjectURL(response);
                    window.open(url);
                    setLoading(false)
                }
            })
            .catch(async (e) => {
                toast('No se pudo generar el reporte', { type: toast.TYPE.ERROR });
                setLoading(false)
            })
    }

    const racalcular = async (documento: any, tipoDato: any) => {
        const request = {
            olN_ID: documento.olN_ID,
            olN_ESTADO_RECALCULAR: tipoDato !== 'Recalcular'
        };

        try {
            const response = await apiProspeccion
                .postRestaurarLineasCreditoObligacion(request);

            if (!response) {
                toast('No se recibió respuesta del servicio', {
                    type: toast.TYPE.ERROR,
                });

                return false;
            }

            if (!response.isSuccess) {
                toast(`Error: ${response.returnMessage || 'No se pudo actualizar la obligación'}`, {
                    type: toast.TYPE.ERROR,
                });

                return false;
            }

            volverEnviarBusqueda(response.data);
            return true;
        } catch (error) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
            return false;
        }
    };

    const abrirModalPazSalvo = (obligacion: any) => {
        setObligacionSeleccionada(obligacion);
        setModalPazSalvoAbierto(true);
    };

    const cerrarModalPazSalvo = () => {
        if (cargandoPazSalvo) {
            return;
        }

        setModalPazSalvoAbierto(false);
        setObligacionSeleccionada(null);
    };

    const cargarPazSalvoYRecalcular = async (archivo: File) => {
        if (!archivo) {
            toast('Debe seleccionar el documento de paz y salvo', {
                type: toast.TYPE.WARNING,
            });
    
            return;
        }
    
        if (!obligacionSeleccionada) {
            toast('No se encontró la obligación que desea recalcular', {
                type: toast.TYPE.WARNING,
            });
    
            return;
        }
    
        const idSolicitudCredito = obtenerIdSolicitudCredito();
        const numeroDocumento = obtenerNumeroDocumento();
    
      
    
        if (!idSolicitudCredito) {
            toast(
                'No se encontró el identificador de la solicitud de crédito',
                {
                    type: toast.TYPE.WARNING,
                },
            );
    
            return;
        }
    
        if (!numeroDocumento) {
            toast(
                'No se encontró el número de documento del afiliado',
                {
                    type: toast.TYPE.WARNING,
                },
            );
    
            return;
        }
    
        if (!user?.USR_ID) {
            toast('No se encontró el usuario autenticado', {
                type: toast.TYPE.WARNING,
            });
    
            return;
        }
    
        setCargandoPazSalvo(true);
    
        try {
            const formData = new FormData();
    
            formData.append('request', archivo);
    
            const respuestaCarga =
                await apicredito.postCreditUploadDocument(
                    formData,
                );
    
            if (
                !respuestaCarga?.isSuccess ||
                !respuestaCarga?.data
            ) {
                toast(
                    respuestaCarga?.returnMessage ||
                        'No se pudo cargar el documento de paz y salvo',
                    {
                        type: toast.TYPE.ERROR,
                    },
                );
    
                return;
            }
    
            const payloadDocumento = {
                creadoPor: user.USR_ID,
                idSolicitudCredito: "519219B1-447C-48D0-9652-95CD9696E9A4",
                idTipoDocumento: 166,
                rutaImagen: respuestaCarga.data,
                fechaCreacion: generarFechaCreacion(),
                numero_documento: numeroDocumento,
            };
    
            console.log(
                'Payload crear documento:',
                payloadDocumento,
            );
    
            const respuestaCreacion =
                await apicredito.postCreditCreateDocument(
                    payloadDocumento,
                );
    
            if (!respuestaCreacion?.isSuccess) {
                toast(
                    respuestaCreacion?.returnMessage ||
                        'El archivo fue cargado, pero no se pudo registrar el documento',
                    {
                        type: toast.TYPE.ERROR,
                    },
                );
    
                return;
            }
    
            const recalculado = await racalcular(
                obligacionSeleccionada,
                'Recalcular',
            );
    
            if (!recalculado) {
                return;
            }
    
            toast(
                'Documento cargado y obligación recalculada correctamente',
                {
                    type: toast.TYPE.SUCCESS,
                },
            );
    
            setModalPazSalvoAbierto(false);
            setObligacionSeleccionada(null);
        } catch (error) {
            console.error(
                'Error cargando documento de paz y salvo:',
                error,
            );
    
            toast(
                'Ocurrió un error al cargar el documento de paz y salvo',
                {
                    type: toast.TYPE.ERROR,
                },
            );
        } finally {
            setCargandoPazSalvo(false);
        }
    };

    let textoScoring = 'No validado';

    if (cargandoScoring) {
        textoScoring = 'Consultando...';
    } else if (scoringValidado) {
        textoScoring = 'Validado';
    }
    if (loading) {
        return <CHLoadingProcess />
    }



    return (
        <div className={classes.root} >
            <br />
            <br />
            <Grid>
                <Typography style={{ color: 'black' }}>Afiliado : <strong>{`${data[1].primerNombre} ${data[1].segundoNombre} ${data[1].primerApellido} ${data[1].segundoApellido} `}</strong></Typography>
                <Typography style={{ color: 'black' }}>Estado Afiliado : <strong>{`${data[1].estadoCliente} `}</strong></Typography>
            </Grid>
            <br />
            <br />
            <Accordion >
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.fiveAccordionTab}
                >
                    <Typography>PARAMETRIZACIÓN SEGUROS POR REGIÓN</Typography>
                </AccordionSummary>
                <AccordionDetails >
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead style={{ border: '1px solid #fec917' }}>
                                <TableRow>
                                    <TableCell>Región</TableCell>
                                    <TableCell>Factor Tarifa (%)</TableCell>
                                    <TableCell>Factor Periódico (%)</TableCell>
                                    <TableCell>Factor IVA (%)</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {arregloSeguros?.map((seguro) => (
                                    <TableRow key={seguro.id}>
                                        <TableCell>{seguro.nombre}</TableCell>
                                        <TableCell>{seguro.seguro}</TableCell>
                                        <TableCell>{seguro.columnaFactorPeriodico}</TableCell>
                                        <TableCell>{seguro.columnaFactorIva}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </AccordionDetails>
            </Accordion >
            <Accordion>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.firstAccordionTab}
                >
                    <Typography className={classes.heading}>INFORMACIÓN DEL INMUEBLE</Typography>
                </AccordionSummary>
                <AccordionDetails >
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead style={{ border: '1px solid #fec917' }}>
                                <TableRow>
                                    <TableCell>Valor Avaluo</TableCell>
                                    <TableCell>Valor Inmueble</TableCell>
                                    <TableCell>Tipo Vivienda</TableCell>
                                    <TableCell>Departamento</TableCell>
                                    <TableCell>Ciudad</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                <TableRow >
                                    <TableCell>{UtilService.formatCurrency(data[2].valoR_AVALUO)}</TableCell>
                                    <TableCell>{UtilService.formatCurrency(data[2].valoR_INMUEBLE)}</TableCell>
                                    <TableCell>{data[2].tipO_VIVIENDA}</TableCell>
                                    <TableCell>{data[2].departamentO_INMUBLE}</TableCell>
                                    <TableCell>{data[2].ciudaD_INMUEBLE}</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>
                </AccordionDetails>
            </Accordion>
            <Accordion>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.firstAccordionTab}
                >
                    <Typography className={classes.heading}>INFORMACIÓN DEL AFILIADO</Typography>
                </AccordionSummary>
                <AccordionDetails >
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead style={{ border: '1px solid #fec917' }}>
                                <TableRow>
                                    <TableCell>Tipo de Cuenta</TableCell>
                                    <TableCell>Estado de Cuenta</TableCell>
                                    <TableCell>Saldo</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {data[1].cuentas?.map((cuenta: any) => (
                                    <TableRow key={cuenta.id}>
                                        <TableCell>{cuenta.tipoCuenta}</TableCell>
                                        <TableCell>{cuenta.estadoCuenta}</TableCell>
                                        <TableCell>{UtilService.formatCurrency(cuenta.saldo)}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </AccordionDetails>
            </Accordion>
            <Accordion >
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.firstAccordionTab}
                >
                    <Typography className={classes.heading}>PARÁMETROS GENERALES</Typography>
                </AccordionSummary>
                <AccordionDetails >
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead style={{ border: '1px solid #fec917' }}>
                                <TableRow>
                                    <TableCell>Tasa crédito</TableCell>
                                    <TableCell>Tasa Nominal</TableCell>
                                    <TableCell>Seguro vida</TableCell>
                                    <TableCell>Seguro vida con extraprima</TableCell>
                                    <TableCell>Seguro hogar</TableCell>
                                    <TableCell>Tasa mixta</TableCell>
                                    <TableCell>Tasa mixta con extraprima</TableCell>
                                    <TableCell>Factor con extraprima</TableCell>
                                    <TableCell>Factor</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                <TableRow>
                                    <TableCell>{data[0].tasA_CREDITO}</TableCell>
                                    <TableCell>{data[0].tasA_NOMINAL}</TableCell>
                                    <TableCell>{data[0].segurO_VIDA}</TableCell>
                                    <TableCell>{data[0].tasA_MIXTA_EXTRAPRIMA}</TableCell>
                                    <TableCell>{data[0].segurO_HOGAR}</TableCell>
                                    <TableCell>{data[0].tasA_MIXTA}</TableCell>
                                    <TableCell>{data[0].tasA_MIXTA_EXTRAPRIMA}</TableCell>
                                    <TableCell>{data[0].factoR_EXTRA_PRIMA}</TableCell>
                                    <TableCell>{data[0].factor}</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>
                </AccordionDetails>
            </Accordion>
            <Accordion >
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.firstAccordionTab}
                >
                    <Typography className={classes.heading}>VALORES DEL CRÉDITO</Typography>
                </AccordionSummary>
                <AccordionDetails >
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead style={{ border: '1px solid #fec917' }}>
                                <TableRow>
                                    <TableCell>Valor préstamo</TableCell>
                                    <TableCell>Plazo</TableCell>
                                    <TableCell>Factor</TableCell>
                                    <TableCell>Factor con extraprima</TableCell>
                                    <TableCell>Cuota sin seguro hogar</TableCell>
                                    <TableCell>Cuota sin seguro hogar con extraprima</TableCell>
                                    <TableCell>Cuota con seguro hogar</TableCell>
                                    <TableCell>Cuota con seguro hogar y extraprima</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                <TableCell>{UtilService.formatCurrency(data[0].valoR_PRESTAMO)}</TableCell>
                                <TableCell>{data[0].plazo}</TableCell>
                                <TableCell>{data[0].factor}</TableCell>
                                <TableCell>{data[0].factoR_EXTRA_PRIMA}</TableCell>
                                <TableCell>{UtilService.formatCurrency(data[0].cuotA_SIN_SEGURO_HOGAR)}</TableCell>
                                <TableCell>{UtilService.formatCurrency(data[0].cuotA_SIN_SEGURO_HOGAR_EXTRAPRIMA)}</TableCell>
                                <TableCell>{UtilService.formatCurrency(data[0].cuotA_SIN_SEGURO_HOGAR)}</TableCell>
                                <TableCell>{UtilService.formatCurrency(data[0].cuotA_SIN_SEGURO_HOGAR_EXTRAPRIMA)}</TableCell>
                            </TableBody>
                        </Table>
                    </TableContainer>
                </AccordionDetails>
            </Accordion>
            <Accordion >
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.firstAccordionTab}
                >
                    <Typography className={classes.heading}> PLAN DE PAGO VALOR CUOTA</Typography>
                </AccordionSummary>
                <AccordionDetails >
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead style={{ border: '1px solid #fec917' }}>
                                <TableRow>
                                    <TableCell>Valor cuota con seguro</TableCell>
                                    <TableCell>Plazo</TableCell>
                                    <TableCell>Factor</TableCell>
                                    <TableCell>Valor préstamo</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                <TableCell>{UtilService.formatCurrency(data[0].capacidaD_PAGO)}</TableCell>
                                <TableCell>{data[0].plazo}</TableCell>
                                <TableCell>{data[0].factor}</TableCell>
                                <TableCell>{UtilService.formatCurrency(data[0].valoR_PRESTAMO)}</TableCell>
                            </TableBody>
                        </Table>
                    </TableContainer>
                </AccordionDetails>
            </Accordion>
            <Accordion >
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                    className={classes.firstAccordionTab}
                >
                    <Typography className={classes.heading}> LINEA DE CREDITO DE LA OBLIGACION</Typography>
                </AccordionSummary>
                <AccordionDetails >
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead style={{ border: '1px solid #fec917' }}>
                                <TableRow>
                                    <TableCell>Linea de credito</TableCell>
                                    <TableCell>Calidad</TableCell>
                                    <TableCell>Saldo</TableCell>
                                    <TableCell>Valor cuota</TableCell>
                                    <TableCell>Estado</TableCell>
                                    <TableCell>Cuotas pactadas</TableCell>
                                    <TableCell>Cuotas canceladas</TableCell>
                                    <TableCell>Cuotas restantes</TableCell>
                                    <TableCell>Acciones</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {data1?.map((datosLineas: any) => (
                                    <TableRow key={datosLineas.olN_ID}>
                                        <TableCell>{datosLineas.olN_LINEA_CREDITO}</TableCell>
                                        <TableCell>{datosLineas.olN_CALIDAD}</TableCell>
                                        <TableCell>{UtilService.formatCurrency(datosLineas.olN_SALDO_OBLIGACION)}</TableCell>
                                        <TableCell>{UtilService.formatCurrency(datosLineas.olN_VALOR_CUOTA)}</TableCell>
                                        <TableCell>{datosLineas.olN_ESTADO}</TableCell>
                                        <TableCell>{datosLineas.olN_CUOTAS_PACTADAS}</TableCell>
                                        <TableCell>{datosLineas.olN_CUOTAS_CANCELADAS}</TableCell>
                                        <TableCell>{datosLineas.olN_CUOTAS_RESTANTES}</TableCell>
                                        <TableCell>
                                            {
                                                datosLineas.olN_ESTADO_RECALCULAR ? (
                                                    <Button
                                                        style={{ backgroundColor: '#1C3F8B', color: 'white', width: "100%" }}
                                                        onClick={() => abrirModalPazSalvo(datosLineas)}
                                                    >
                                                        Recalcular
                                                    </Button>
                                                ) : (
                                                    <Button
                                                        style={{ backgroundColor: 'red', color: 'white', width: "100%" }}
                                                        onClick={() => racalcular(datosLineas, 'Regresar')}
                                                    >
                                                        Regresar
                                                    </Button>
                                                )
                                            }
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                        {
                            mostrarPdf ?
                                <div style={{ display: 'flex', justifyContent: 'flex-end', padding: '10px' }} >
                                    <Button onClick={() => generarPdf()} style={{ backgroundColor: '#1C3F8B', color: 'white' }} >
                                        Pdf
                                    </Button>
                                </div> : null
                        }
                    </TableContainer>
                </AccordionDetails>
            </Accordion>
            {
                data1.length === 0 || data1.length !== 0 ? <>

                    <Accordion >
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                            aria-controls="panel1a-content"
                            id="panel1a-header"
                            className={classes.firstAccordionTab}
                        >
                            <Typography className={classes.heading}>CONTROL ENDEUDAMIENTO NOMINA </Typography>
                        </AccordionSummary>
                        <AccordionDetails >
                            <TableContainer component={Paper}>
                                <Table>
                                    <TableHead style={{ border: '1px solid #fec917' }}>
                                        <TableRow>
                                            <TableCell>Canon Mensual</TableCell>
                                            <TableCell>Salario Total Último Mes</TableCell>
                                            <TableCell>Deducciones Último Mes Nómina</TableCell>

                                            <TableCell>Salario Neto Último Mes (SNM)</TableCell>
                                            <TableCell>Capacidad de Pago</TableCell>
                                            <TableCell>Disponible Libranza</TableCell>
                                            <TableCell>Control Capacidad Pago</TableCell>
                                            <TableCell>SCORING:</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        <TableRow >
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_CANON_MENSUAL)}</TableCell>
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_SALARIO_TOTAL)}</TableCell>
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_DEDUCCIONES)}</TableCell>
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_SALARIO_NETO)}</TableCell>
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_CAPACIDAD_PAGO)}</TableCell>
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_DISPONIBLE_LIBRANZA)}</TableCell>
                                            <TableCell>{listaDatosProspeccion.edP_CONTROL_NIVEL_ENDUEDAMIENTO_CUOTA_PORC}</TableCell>
                                            <TableCell>
                                                <div className={classes.scoringContainer}>
                                                    <span className={classes.scoringText}>
                                                        {listaDatosProspeccion.edP_SCORE}
                                                    </span>

                                                    <Checkbox
                                                        checked={scoringValidado}
                                                        disabled={
                                                            cargandoScoring ||
                                                            guardandoScoring
                                                        }
                                                        color="primary"
                                                        onChange={cambiarEstadoScoring}
                                                        inputProps={{
                                                            'aria-label': 'Validar scoring',
                                                        }}
                                                    />
                                                </div>
                                            </TableCell>
                                        </TableRow>
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </AccordionDetails>
                    </Accordion>
                    <Accordion >
                        <AccordionSummary
                            expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
                            aria-controls="panel1a-content"
                            id="panel1a-header"
                            className={classes.firstAccordionTab}
                        >
                            <Typography className={classes.heading}>CONTROL  ENDEUDAMIENTO GLOBAL</Typography>
                        </AccordionSummary>
                        <AccordionDetails >
                            <TableContainer component={Paper}>
                                <Table>
                                    <TableHead style={{ border: '1px solid #fec917' }}>
                                        <TableRow>
                                            <TableCell>Sector Financiero/Solidario/Asegurador</TableCell>
                                            <TableCell>Sector Real (Informe CIFIN)</TableCell>
                                            <TableCell>Total obligaciones + Canon mensual</TableCell>
                                            <TableCell>Porcentaje Endeudamiento Global Cuota</TableCell>
                                            <TableCell>Control Nivel Endeudamiento Global Cuota</TableCell>
                                            <TableCell>Porcentaje Endeudamiento Global Sin Cuota</TableCell>
                                            <TableCell>Control Nivel Endeudamiento Global Sin Cuota</TableCell>

                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        <TableRow >
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_SECTOR_FINANCIERO)}</TableCell>
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_SECTOR_REAL)}</TableCell>
                                            <TableCell>{UtilService.formatCurrency(listaDatosProspeccion.edP_TOTAL_CANON_DEDUCCIONES)}</TableCell>
                                            <TableCell>{listaDatosProspeccion.edP_ENDUEDAMIENTO_ACTUAL_CUOTA_PORC}</TableCell>
                                            <TableCell>{listaDatosProspeccion.edP_CONTROL_NIVEL_ENDUEDAMIENTO_CUOTA_PORC}</TableCell>
                                            <TableCell>{listaDatosProspeccion.edP_ENDUEDAMIENTO_ACTUAL_SIN_CUOTA_PORC}</TableCell>
                                            <TableCell>{listaDatosProspeccion.edP_CONTROL_NIVEL_ENDUEDAMIENTO_SIN_CUOTA_PORC}</TableCell>
                                        </TableRow>
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </AccordionDetails>
                    </Accordion>
                </> : null
            }


            <ModalPazSalvo
                open={modalPazSalvoAbierto}
                loading={cargandoPazSalvo}
                onClose={cerrarModalPazSalvo}
                onCargar={cargarPazSalvoYRecalcular}
            />
        </div>
    );
}
export default AcordionProspeccionTablas;