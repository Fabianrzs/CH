import React, { useEffect, useState } from 'react';
import {
    Button,
    createStyles,
    FormLabel,
    Grid,
    makeStyles,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import CheckIcon from '@material-ui/icons/Check';
import { Clear } from '@material-ui/icons';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import SolicitudApi from '../../../../services/api-solicitud.service';
import { ConsultarDocumentosSubModalidadTareaModel } from '../../../../models/requests/consultar-documentos-submodalidad-tarea.model';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';

const useStyles = makeStyles(() =>
    createStyles({
        aliniarIcono: {
            marginTop: '1rem',
        },
        aliniarTexto: {
            color: 'Black',
            padding: '10px',
        },
        columnaUpload: {
            paddingLeft: '15%',
        },

        btnRefrescar: {
            backgroundColor: '#1C3F8B',
            padding: '15px',
            color: '#FFFFFF',
            margin: '30px',
            '&:hover': {
                background: '#FFFFFF ',
            },
        }


    }),
);

function CargueDocumentosSolicitud({
    responseSolicitud,
    documentosAgregados,
    siEsAprobadaSolcitud,
    soloLecturaCargueDocumentos,

}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    documentosAgregados: (documentos: any) => void;
    siEsAprobadaSolcitud: any;
    soloLecturaCargueDocumentos?: boolean;
}) {
    const api = SolicitudApi();


    const [listaDocumentos, setListadocumentos] = useState<any[]>([]);
    const [listaDocumentosDodo, setListadocumentosDodo] = useState<{ fileProperties: any[] } | null>(null);
    const [ocultarOpciones, setOcultarOpciones] = useState(false)
    const [isLoading, setIsLoading] = useState(false)

    const normalizarTexto = (texto: any) =>
        String(texto || '')
            .trim()
            .toLowerCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '');



    useEffect(() => {
        if (responseSolicitud?.solicitudId) {
            validarMostrarCamposSoloLectura(responseSolicitud.solicitudId);
        }
    }, [
        responseSolicitud?.solicitudId,
        responseSolicitud?.solicitudTareaId,
        soloLecturaCargueDocumentos,
    ]);


    const listadoDodo = () => {
        if (soloLecturaCargueDocumentos || ocultarOpciones) {
            return;
        }

        consultarDocumentosDodo();
    };

    const classes = useStyles();


    const validarMostrarCamposSoloLectura = async (solicitudId: string) => {
        if (soloLecturaCargueDocumentos) {
            setOcultarOpciones(true);
            await obtenerDocumentosVerificadosDodo();
            return;
        }

        await api.obtenerTrazabilidadSolicitud(solicitudId)
            .then(async (response) => {
                const trazabilidad = Array.isArray(response?.data)
                    ? response.data
                    : [];

                const actividadActual = trazabilidad.find((item: any) =>
                    String(item?.idTareaSolicitud || '').toLowerCase() ===
                    String(responseSolicitud?.solicitudTareaId || '').toLowerCase()
                );

                const tareaActual = normalizarTexto(
                    actividadActual?.descripcionTarea || responseSolicitud?.nombreTarea
                );

                const estadoActual = normalizarTexto(
                    actividadActual?.descripcionEstadoSolicitud
                );

                const esCargueInconsistente =
                    tareaActual.includes('cargue') &&
                    estadoActual === 'inconsistente';

                if (esCargueInconsistente) {
                    setOcultarOpciones(true);
                    consultarDocumentosGa2();
                    return;
                }

                const existeCarguePendiente = trazabilidad.some((item: any) => {
                    const tarea = normalizarTexto(item?.descripcionTarea);
                    const estado = normalizarTexto(item?.descripcionEstadoSolicitud);

                    return (
                        (
                            tarea.includes('cargue') ||
                            tarea.includes('verificacion en tiempo')
                        ) &&
                        estado === 'pendiente'
                    );
                });

                // if (existeCarguePendiente) {
                //     const vieneDeRechazo =
                //         responseSolicitud?.soL_RECHAZADA === '1';

                //     setOcultarOpciones(false);

                //     await consultarDocumentosDodo();

                //     if (vieneDeRechazo) {
                //         await consultarDocumentosConMarcacionAnterior(
                //             trazabilidad,
                //         );
                //     } else {
                //         await consultarDocumentosGa2();
                //     }

                //     return;
                // }

                if (existeCarguePendiente) {
                    const vieneDeRechazo =
                        responseSolicitud?.soL_RECHAZADA === '1';

                    setOcultarOpciones(false);

                    await consultarDocumentosDodo();

                    if (vieneDeRechazo) {
                        await consultarDocumentosConMarcacionAnterior(
                            trazabilidad,
                        );
                    } else {
                        await obtenerDocumentosVerificadosDodo();
                    }

                    return;
                }

                setOcultarOpciones(true);
                obtenerDocumentosVerificadosDodo();
            });
    };

    // const obtenerDocumentosVerificadosDodo = async () => {
    //     const modelConsultaDocumentos: ConsultarDocumentosSubModalidadTareaModel = {
    //         idSolicitudtarea: responseSolicitud?.ultimoSolicitudTareaId as string,
    //         idSubModalidad: responseSolicitud?.tipoSubModalidadId as string,
    //     };
    //     try {
    //         const respuestaDocumentosDodo = await api.GerObtenerDocumentosVerifiacadosDodo(responseSolicitud?.solicitudId);
    //         const respuestaDocumentosPorSubmodalidad = await api.GetConsultarDocumentosPorSubModalidad(modelConsultaDocumentos);
    //         if (respuestaDocumentosDodo.data && respuestaDocumentosPorSubmodalidad.data) {
    //             const documentosVerificados = respuestaDocumentosDodo.data.map((element: any) => {
    //                 const yaVerificado = respuestaDocumentosPorSubmodalidad.data.some(
    //                     (doc: any) => doc.nombreArchivo === element.asC_NOMBRE_ARCHIVO
    //                 );
    //                 return {
    //                     ...element,
    //                     verificado: yaVerificado,
    //                 };
    //             });
    //             setListadocumentos(documentosVerificados);
    //         }
    //     } catch (e) {
    //         toast(`Error del sistema al cargar lista de archivos verificada ${e}`, { type: toast.TYPE.ERROR });
    //     }
    // };

    const obtenerDocumentosVerificadosDodo = async () => {
        const modelConsultaDocumentos: ConsultarDocumentosSubModalidadTareaModel = {
            idSolicitudtarea: responseSolicitud?.ultimoSolicitudTareaId as string,
            idSubModalidad: responseSolicitud?.tipoSubModalidadId as string,
        };

        try {
            const [
                respuestaDocumentosDodo,
                respuestaDocumentosPorSubmodalidad,
            ] = await Promise.all([
                api.GerObtenerDocumentosVerifiacadosDodo(responseSolicitud?.solicitudId),
                api.GetConsultarDocumentosPorSubModalidad(modelConsultaDocumentos),
            ]);

            const documentosDodo = Array.isArray(respuestaDocumentosDodo?.data)
                ? respuestaDocumentosDodo.data
                : [];

            const documentosSubmodalidad = Array.isArray(respuestaDocumentosPorSubmodalidad?.data)
                ? respuestaDocumentosPorSubmodalidad.data
                : [];

            const nombresVerificadosDodo = new Set<string>(
                documentosDodo
                    .map(obtenerNombreDocumento)
                    .filter((nombre: string) => nombre !== '')
            );

            const documentosVerificados = documentosSubmodalidad.map((doc: any) => {
                const nombreDocumento = obtenerNombreDocumento(doc);

                const estaVerificadoEnDodo =
                    nombresVerificadosDodo.has(nombreDocumento) ||
                    doc?.verificado === true;

                return {
                    ...doc,
                    verificado: estaVerificadoEnDodo,
                    esRequerido: esDocumentoRequerido(doc),
                };
            });

            setListadocumentos(documentosVerificados);
            documentosAgregados(documentosVerificados);
        } catch (e) {
            toast(`Error del sistema al cargar lista de archivos verificada ${e}`, {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const consultarDocumentosGa2 = async () => {
        const modelConsultaDocumentos: ConsultarDocumentosSubModalidadTareaModel =
        {
            idSolicitudtarea:
                responseSolicitud?.ultimoSolicitudTareaId as string,
            idSubModalidad: responseSolicitud?.tipoSubModalidadId as string,
        };

        await api
            .GetConsultarDocumentosPorSubModalidad(modelConsultaDocumentos)
            .then(async respuesta => {
                setListadocumentos(respuesta.data);
                documentosAgregados(respuesta.data);
            })
            .catch((e) => {
                toast(`Error del sistema ${e}`, { type: toast.TYPE.ERROR });
            });
    };

    const obtenerNombreDocumento = (documento: any) =>
        normalizarTexto(
            documento?.nombreArchivo ||
            documento?.asC_NOMBRE_ARCHIVO ||
            documento?.description ||
            '',
        );

    const esDocumentoRequerido = (documento: any): boolean => {
        const valor = documento?.esRequerido;

        return (
            valor === true ||
            valor === 1 ||
            ['true', 'si', 'sí', '1'].includes(
                normalizarTexto(valor)
            )
        );
    };

    const consultarDocumentosConMarcacionAnterior = async () => {
        try {
            const modelConsultaDocumentos: ConsultarDocumentosSubModalidadTareaModel = {
                idSolicitudtarea:
                    responseSolicitud?.ultimoSolicitudTareaId as string,
                idSubModalidad:
                    responseSolicitud?.tipoSubModalidadId as string,
            };

            const [
                respuestaDocumentosActuales,
                respuestaDocumentosVerificados,
            ] = await Promise.all([
                api.GetConsultarDocumentosPorSubModalidad(
                    modelConsultaDocumentos,
                ),
                api.GerObtenerDocumentosVerifiacadosDodo(
                    responseSolicitud?.solicitudId,
                ),
            ]);

            const documentosActuales = Array.isArray(
                respuestaDocumentosActuales?.data,
            )
                ? respuestaDocumentosActuales.data
                : [];

            const documentosVerificadosAntes = Array.isArray(
                respuestaDocumentosVerificados?.data,
            )
                ? respuestaDocumentosVerificados.data
                : [];

            const nombresVerificados = new Set<string>(
                documentosVerificadosAntes
                    .map(obtenerNombreDocumento)
                    .filter((nombre: string) => nombre !== ''),
            );

            const documentosFusionados = documentosActuales.map(
                (documento: any) => ({
                    ...documento,
                    verificado:
                        documento?.verificado === true ||
                        nombresVerificados.has(
                            obtenerNombreDocumento(documento),
                        ),
                    esRequerido: esDocumentoRequerido(documento),
                }),
            );

            setListadocumentos(documentosFusionados);
            documentosAgregados(documentosFusionados);
        } catch (error) {
            console.error(
                'Error recuperando la marcación anterior de documentos:',
                error,
            );

            await consultarDocumentosGa2();
        }
    };

    const consultarDocumentosDodo = async () => {
        setIsLoading(true)
        const api = SolicitudApi();
        const radicado = responseSolicitud?.numeroRadicadoSG
        await api
            .GetConsultarDocumentosPorSubModalidadDodo(String(radicado))
            .then(async respuesta => {
                setListadocumentosDodo(respuesta.data);
                setIsLoading(false)
            })
            .catch((e) => {
                toast(`Error del sistema ${e}`, { type: toast.TYPE.ERROR });
                setIsLoading(false)
            });
    };


    const verificar = (documento: any) => {
        if (soloLecturaCargueDocumentos || ocultarOpciones) {
            toast(
                'No es posible verificar documentos en Dodo porque la actividad se encuentra en estado inconsistente.',
                { type: toast.TYPE.WARNING }
            );
            return;
        }

        // if (!esDocumentoRequerido(documento)) {
        //     toast(
        //         'Este documento no es requerido y no necesita validación en Dodo.',
        //         { type: toast.TYPE.INFO }
        //     );
        //     return;
        // }

        const nombreDocumento = normalizarTexto(
            documento?.nombreArchivo ||
            documento?.asC_NOMBRE_ARCHIVO ||
            documento?.description ||
            ''
        );

        const encontrado = listaDocumentosDodo?.fileProperties?.some(
            (element: any) =>
                normalizarTexto(element?.description) === nombreDocumento
        );

        // PRUEBAS / PRODUCCIÓN: valida contra Dodo
        const verificacionExitosa = encontrado;

        // LOCAL: descomenta esta y comenta la anterior
        // const verificacionExitosa = true;


        if (!verificacionExitosa) {
            toast('Archivo no encontrado en Dodo-Doc', {
                type: toast.TYPE.WARNING,
            });
            return;
        }

        const documentosActualizados = listaDocumentos.map((option: any) => {
            const nombreOpcion = normalizarTexto(
                option?.nombreArchivo ||
                option?.asC_NOMBRE_ARCHIVO ||
                option?.description ||
                '',
            );

            if (nombreOpcion === nombreDocumento) {
                return {
                    ...option,
                    verificado: verificacionExitosa,
                };
            }

            return option;
        });

        documentosAgregados(documentosActualizados);
        setListadocumentos(documentosActualizados);
    };

    if (isLoading) {
        return <CHLoadingProcess />;
    }

    const folioOculto = responseSolicitud?.descripcionModalidad;


    const accionesCargueDeshabilitadas =
        !!soloLecturaCargueDocumentos || ocultarOpciones;

    const mostrarAccionesDodo = !accionesCargueDeshabilitadas;

    const listaDocumentosParaMostrar = accionesCargueDeshabilitadas
        ? listaDocumentos.filter((documento: any) => {
            const requerido = esDocumentoRequerido(documento);
            const cargadoEnDodo = documento?.verificado === true;

            return requerido || cargadoEnDodo;
        })
        : listaDocumentos;

    return (
        <>

            <CHPaper title="Cargue de documentos" >
                <Grid container>
                    {mostrarAccionesDodo && (
                        <Button
                            className={classes.btnRefrescar}
                            onClick={() => listadoDodo()}
                        >
                            Refrescar Listado DODO
                        </Button>
                    )}

                    <Table size="small">
                        <TableHead>
                            <TableRow>
                                <TableCell width="40%">Nombre Archivo</TableCell>
                                <TableCell width="10%" align="center">
                                    Es requerido
                                </TableCell>

                                {mostrarAccionesDodo && (
                                    <TableCell width="30%" align="center">
                                        Verificar Dodo
                                    </TableCell>
                                )}
                                <TableCell width="30%" align="center">
                                    Cargado en dodo
                                </TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {listaDocumentosParaMostrar.map(
                                (option: any) => (
                                    <TableRow key={option?.code}>
                                        <TableCell>
                                            <React.Fragment
                                                key={option?.code || option?.asC_ID
                                                }
                                            >
                                                {option.description}
                                            </React.Fragment>
                                            <FormLabel
                                                className={classes.aliniarTexto}
                                            >
                                                {option.nombreArchivo === undefined || option.nombreArchivo === '' ? option.asC_NOMBRE_ARCHIVO : option.nombreArchivo}
                                            </FormLabel>
                                        </TableCell>
                                        <TableCell align="center">
                                            {esDocumentoRequerido(option) ? 'Si  ' : 'No  '}
                                        </TableCell>

                                        {mostrarAccionesDodo && (
                                            <TableCell width="30%" align="center">
                                                <Button onClick={() => verificar(option)}>
                                                    Verificar Dodo
                                                </Button>
                                            </TableCell>
                                        )}
                                        <TableCell width="30%" align="center">
                                            {option.verificado ? (
                                                <CheckIcon style={{ color: 'green' }} />
                                            ) : (
                                                <Clear style={{ color: 'red' }} />
                                            )}
                                        </TableCell>
                                    </TableRow>
                                ),
                            )}
                        </TableBody>
                        {
                            folioOculto !== 'ACREDITACION V8' && (
                                <TableCell width="15%" align="center">
                                    <Grid item xs={12} md={12} lg={8}>
                                        <CHTextField
                                            isReadOnly
                                            label='Números de Folios'
                                            numberMask
                                            InputLabelProps={{ shrink: true, }}
                                            value={responseSolicitud?.numero_Folio}
                                        />
                                    </Grid>
                                </TableCell>
                            )
                        }

                    </Table>
                </Grid>
            </CHPaper>


        </>
    );
}
export default CargueDocumentosSolicitud;
