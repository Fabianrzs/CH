import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    Checkbox,
    makeStyles,
    createStyles,
    CircularProgress,
} from '@material-ui/core';
import { useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';
import NotificationService from '../../services/notification.service';
import ApiSchedule from '../../services/api-cronograma.service';
import AuthService from '../../services/auth.service';
import history from '../../utils/history.router';
import AuditoriaApi from '../../services/api-auditorias.servise';
import { Auditoria } from '../../types/Auditoria.model.type';
import CatalogApi from '../../services/api-catalog.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {

            width: 'fit-content',
            marginLeft: '0',
            marginTop: '1.5rem',
            backgroundColor: 'transparent',
            color: '#1C3F8B',
            border: 'none',
            cursor: 'pointer',
        },
        tableContainer: {
            marginTop: '1rem',
        },
        backButton: {
            position: 'fixed',
            bottom: '1rem',
            left: '6rem',
        },
    })
);

const ConsultaNovedades = () => {
    const api = ApiSchedule();
    const user = AuthService.getUserInfo();
    const otroFormato = true;
    const classes = useStyles();
    const location = useLocation<{ nameUnidadEjecutora: any, archivoName: any, periodoNovedad: any, estadoArchivoNov: any, idCodigoUsuario: any, nombreConceptoNovedad: any, idUnidadEjecutora: any, idConceptoNovedad: any, fechaInicio: any, fechaFin: any, archivoId: any, codigoConcepto: any, codigoNovedad: any, descargado: any }>();
    const { nameUnidadEjecutora, archivoName, periodoNovedad, idCodigoUsuario, nombreConceptoNovedad, idUnidadEjecutora, idConceptoNovedad, fechaInicio, fechaFin, codigoConcepto, codigoNovedad, descargado } = location.state;

    const [archivosDescarga, setArchivosDescarga] = useState<any[]>([]);
    const [archivoSeleccionado, setArchivoSeleccionado] = useState(null);
    const [autorizado, setAutorizado] = useState(false);
    const [estadoNovedad, setEstadoNovedad] = useState();
    const [descargaArchivosIds, setDescargaArchivosIds] = useState();
    const [idUser, setIdUser] = useState();
    const fechaActual = new Date();
    const [siglas, setSiglas] = useState();
    const [idDeUnidadEjecutora, setIdDeUnidadEjecutora] = useState(0);
    const catalogApi = CatalogApi();
    


    const insertDescargaDefinitiva = async () => {
        try {
            const response = await api.InsertarDescargaDefinitiva({
                UnidadEjecutoraId: idUnidadEjecutora,
                ConceptoNovedadId: idConceptoNovedad,
                DescargaArchivoId: descargaArchivosIds,
                NombreArchivo: archivoName,
                NombreUnidadEjecutora: nameUnidadEjecutora,
                NombreConceptoNovedad: nombreConceptoNovedad,
                Periodo: periodoNovedad,
                Estado: estadoNovedad,
                Usuario: idUser === undefined ? user.USR_ID : idUser,
                SiglasUnidadEjecutora: siglas,
                Fecha: fechaActual,
                CodigoConcepto: codigoNovedad,
                // Descargado: descargado
            });

            const estadoArchivo = response.data[0]?.estado;
            setEstadoNovedad(estadoArchivo);
            const descargaArchivoIds = response.data[0]?.descargaArchivoId;
            setDescargaArchivosIds(descargaArchivoIds);

            return { response, estadoArchivo };
        } catch (error) {
            toast.error(`Error al enviar datos descarga definitiva: ${error}`);
            return null;
        }
    };

    const getArchivosDescargaDefinitiva = async () => {
        try {
            const response = await api.ObtenerArchivosDescargaDefinitiva({
                UnidadEjecutoraId: idUnidadEjecutora,
                ConceptoNovedadId: idConceptoNovedad,
                DescargaArchivoId: descargaArchivosIds,
                NombreArchivo: archivoName,
                NombreUnidadEjecutora: nameUnidadEjecutora,
                NombreConceptoNovedad: nombreConceptoNovedad,
                Periodo: periodoNovedad,
                Estado: estadoNovedad,
                Usuario: idUser === undefined ? user.USR_ID : idUser,
                SiglasUnidadEjecutora: siglas,
                Fecha: fechaActual,
                CodigoConcepto: codigoNovedad,
                Descargado: descargado
            });
    
            const sortedArchivos = (response.data || []).sort((a, b) => new Date(b.fecha).getTime() - new Date(a.fecha).getTime());
    
            const updatedArchivos = sortedArchivos.map((archivo: { unidadEjecutoraId: number; mostrarBotonDescargar: boolean; estado: string; descargado: any; }) => {
                const esIdBloqueado = idsBloqueados.includes(archivo.unidadEjecutoraId);
                archivo.mostrarBotonDescargar = archivo.estado === 'AUTORIZADA' && (!esIdBloqueado || !archivo.descargado);
                return archivo;
            });
    
            setArchivosDescarga(updatedArchivos);
            const idUEJ = updatedArchivos[0]?.unidadEjecutoraId;
            setIdDeUnidadEjecutora(idUEJ);
            return response;
        } catch (error) {
            console.error('Error al obtener datos de la API:', error);
            return null;
        }
    };
    
    const ActualizarEstadoDescargaDefinitiva = async (row: any, nuevoEstado: any, descargado: any) => {
        try {
            const response = await api.ActualizarEstadoDescargaDefinitiva({
                UnidadEjecutoraId: idUnidadEjecutora,
                ConceptoNovedadId: idConceptoNovedad,
                DescargaArchivoId: row.descargaArchivoId,
                NombreArchivo: archivoName,
                NombreUnidadEjecutora: nameUnidadEjecutora,
                NombreConceptoNovedad: nombreConceptoNovedad,
                Periodo: periodoNovedad,
                Estado: nuevoEstado,
                Usuario: idUser === undefined ? user.USR_ID : idUser,
                SiglasUnidadEjecutora: siglas,
                Fecha: fechaActual,
                CodigoConcepto: codigoNovedad,
                Descargado: descargado
            });
            if (!response.isSuccess) {
                NotificationService.genericAlert(
                    'Error',
                    'Los datos del archivo no están disponibles.',
                    'error',
                );
            }
            return response;
        } catch (error) {
            NotificationService.genericAlert(
                'Error',
                'No se pudo iniciar la descarga del archivo.',
                'error',
            );
            throw error;
        }
    };


    useEffect(() => {
        const fetchData = async () => {
            await insertDescargaDefinitiva();
            await getArchivosDescargaDefinitiva();
        };

        fetchData();
    }, []);


    const handleCheckBoxChange = (index: any) => {
        const archivoSeleccionado = archivosDescarga[index];
        const esIdBloqueado = idsBloqueados.includes(archivoSeleccionado.unidadEjecutoraId);
    
        if (archivoSeleccionado.estado === 'AUTORIZADA' && esIdBloqueado && archivoSeleccionado.descargado) {
            NotificationService.genericAlert(
                'Acción No Permitida',
                'Este archivo ya ha sido descargado y no se puede descargar de nuevo.',
                'warning'
            );
            return;
        }
    
        if (archivoSeleccionado === index) {
            setArchivoSeleccionado(null);
            setAutorizado(false);
        } else {
            setArchivoSeleccionado(index);
            setAutorizado(false);
        }
    };
    

    const handleRechazarClick = async (index: number) => {
        try {
            await ActualizarEstadoDescargaDefinitiva(archivosDescarga[index], 'RECHAZADA');
            const updatedArchivosDescarga = [...archivosDescarga];
            updatedArchivosDescarga[index].estado = 'RECHAZADA';
            setArchivosDescarga(updatedArchivosDescarga);

            NotificationService.genericAlert('Descarga rechazada', 'El archivo no será descargado.', 'info');

            if (archivoSeleccionado === index) {
                setArchivoSeleccionado(null);
                setAutorizado(false);
            }
        } catch (error: any) {
            toast.error(`Error al rechazar: ${error.message}`, { type: toast.TYPE.ERROR });
        }
    };

    const handleAutorizarClick = async (index: number) => {
        try {
            await ActualizarEstadoDescargaDefinitiva(archivosDescarga[index], 'AUTORIZADA', descargado);
            getArchivosDescargaDefinitiva();
        } catch (error: any) {
            toast.error(`Error al actualizar: ${error.message}`, { type: toast.TYPE.ERROR });
        }
    };

    const idsBloqueados = [1, 7, 11, 18];

    const handleDescargarClick = async (index: number) => {
        const archivoSeleccionado = archivosDescarga[index];
        const esIdBloqueado = idsBloqueados.includes(archivoSeleccionado.unidadEjecutoraId);
        
        if (esIdBloqueado && archivoSeleccionado.descargado) {
            NotificationService.genericAlert(
                'Descarga No Permitida',
                'Este archivo ya ha sido descargado y no se puede descargar de nuevo.',
                'warning'
            );
            return;
        }
    
        if (archivoSeleccionado.estado === 'AUTORIZADA') {
            NotificationService.genericAlert('Descarga Iniciada', 'El archivo se descargará', 'success');
            await onClickGenerar(archivoSeleccionado, fechaInicio, fechaFin, idCodigoUsuario, otroFormato);
    
            // Actualizar estado a descargado
            archivoSeleccionado.descargado = true;
            await ActualizarEstadoDescargaDefinitiva(archivoSeleccionado, 'AUTORIZADA', true);
    
            setArchivoSeleccionado(null);
            setAutorizado(false);
        } else {
            NotificationService.genericAlert(
                'Descarga No Permitida',
                'Este archivo no está autorizado para descarga.',
                'warning'
            );
        }
    };

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);

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

    const onClickGenerar = async (row: any, fechaInicio: Date, fechaFin: Date, idUser?: any, otroFormato?: any) => {
        const nav = window.navigator as any;
                const usrAg = nav.userAgentData?.brands?.find(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )?.brand;
                const auditoriaApi = AuditoriaApi();

        try {
            const response = await api.GenerarArchivo({
                idUnidadEjecutora: row.unidadEjecutoraId,
                idConceptoNovedad: row.conceptoNovedadId,
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                esAlta: true,
                codigo: row.codigoConcepto,
                idCodigoUsuario: idUser === undefined ? user.USR_ID : idUser,
                dHorizontal: false,
                otroFormato: otroFormato,
                nombreArchivo: row.nombreArchivo
            });

            const a = document.createElement('a');
            a.href = `data:image/png;base64,${response.data.archivoB64}`;

            const serverFileExtension = response.data.nameFile.split('.').pop();
            a.download = response.data.nameFile.replace(
                `.${serverFileExtension}`,
                `.${serverFileExtension}`
            );

            a.click();

            markArchivoDescargado(row);

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de novedades Activas',
                hostname: document.location.host,
                tipoAccion: 'DESCARGAS',
                idTipoAccion: 5,
                descripcion: `Descarga de archivo definitivo para unidad ejecutora ${archivosDescarga[0]?.nombreUnidadEjecutora}`
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error en descarga de archivo de la unidad ${archivosDescarga[0]?.nombreUnidadEjecutora}.
                    - Error: ${error?.message || 'Error desconocido'}
                    - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                    - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                    const requestAuditoriaError: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData?.platform,
                        nombreUsuario: user?.USR_USERNAME,
                        usurioId: user?.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Consulta de novedades Activas',
                        hostname: document.location.host,
                        tipoAccion: 'ERRORES Y EXCEPCIONES',
                        idTipoAccion: 8,
                        descripcion: descripcionError
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoriaError);
                    
            NotificationService.genericAlert(
                'Error',
                'No se pudo generar el archivo.',
                'error',
            );
        }
    };

    const markArchivoDescargado = (row: any) => {
        const updatedArchivosDescarga = archivosDescarga.map(archivo => {
            if (archivo.descargaArchivoId === row.descargaArchivoId) {
                return { ...archivo, descargado: true };
            }
            return archivo;
        });

        setArchivosDescarga(updatedArchivosDescarga);
    };

    const handleReturnConsulta = () => {
        history.push('/Consulta');
    };

    return (
        <Grid container>
            <>
                <Grid item xs={12}>
                    <table className="table">
                        <thead>
                            <tr>
                                <th style={{ textAlign: 'center' }}>Archivo</th>
                                <th style={{ textAlign: 'center' }}>Periodo</th>
                                <th style={{ textAlign: 'center' }}>Unidad Ejecutora</th>
                                <th style={{ textAlign: 'center' }}>Concepto</th>
                                <th style={{ textAlign: 'center' }}>Fecha Solicitud Descarga</th>
                                <th style={{ textAlign: 'center' }}>Estado</th>
                                <th style={{ textAlign: 'center' }}>Acciones</th>
                                <th style={{ textAlign: 'center' }}>Descarga</th>
                            </tr>
                        </thead>

                        <tbody>

                            {archivosDescarga.map((archivo, index) => (
                                <tr key={index}>
                                    <td>{archivo.nombreArchivo || archivoName}</td>
                                    <td>{archivo.periodo}</td>
                                    <td>{archivo.nombreUnidadEjecutora}</td>
                                    <td>{archivo.nombreConceptoNovedad}</td>
                                    <td>{new Date(archivo.fecha).toLocaleDateString()}</td>
                                    <td>{archivo.estado}</td>
                                    <td>
                                        <Checkbox
                                            checked={archivoSeleccionado === index}
                                            onChange={() => handleCheckBoxChange(index)}
                                        />
                                    </td>
                                    <td>
                                        {archivoSeleccionado === index && (
                                            <div style={{ display: 'flex', alignItems: 'center' }}>
                                                {archivo.estado === 'EVALUACIÓN' ? (
                                                    <>
                                                        <Button
                                                            variant="contained"
                                                            className={classes.button}
                                                            onClick={() => handleRechazarClick(index)}
                                                        >
                                                            RECHAZAR
                                                        </Button>
                                                        <Button
                                                            variant="contained"
                                                            className={classes.button}
                                                            onClick={() => handleAutorizarClick(index)}
                                                        >
                                                            AUTORIZAR
                                                        </Button>
                                                    </>
                                                ) : (
                                                    archivo.mostrarBotonDescargar && (
                                                        <button
                                                            onClick={() => handleDescargarClick(index)}
                                                            style={{
                                                                backgroundColor: 'blue',
                                                                padding: '5px',
                                                                color: 'white',
                                                                border: 'none',
                                                                marginRight: '10px',
                                                                cursor: 'pointer',
                                                                borderRadius: '5px',
                                                            }}
                                                        >
                                                            DESCARGAR ARCHIVO
                                                        </button>
                                                    )
                                                )}
                                            </div>
                                        )}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </Grid>

                <Grid item xs={12}>
                    <Button onClick={handleReturnConsulta} style={{ position: 'fixed', bottom: '1rem', left: '6rem' }}>
                        REGRESAR
                    </Button>
                </Grid>
            </>

        </Grid>
    );
};

export default ConsultaNovedades;
