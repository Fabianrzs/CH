import { createStyles, 
         makeStyles, 
         Button,
         Table,
         TableCell,
         TableHead,
         TableRow,
         FormLabel,
         TableBody,
         Grid,
         } from '@material-ui/core';
import React, {  useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import CheckIcon from '@material-ui/icons/Check';
import { Clear } from '@material-ui/icons';
import {  ACTIVIDAD_POSTULACION, ESTADO_POSTULACION } from '../constants/fondoSolidaridad.constants';
import { DocumentosPostulacionModel } from '../constants/fondosolidaridadform.model.type';
import ObservacionesPostulacion from './ObservacionesPostulacion';
import ObservacionVerificacionPostulacion from './ObservacionVerDocumentosPostulacion';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ApiSolicitud from '../../../services/api-solicitud.service';

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
            color:'#FFFFFF',
            margin: '30px',
            '&:hover': {
				background: '#FFFFFF ',
			},
        }
    }),
);

const CargueDocumentosPostulacion = (props: propsType) => {
   
    const {
           formularioObservacionCargueArchivos, 
           datosSolicitudPostulacion, 
           documentosAgregados,
           camposVerificacionNovedadesSoloLectura,
           camposVerificacionEmbargosSoloLectura,
           camposVerificacionCuentaIndividualSoloLectura,
           camposCargueDocumentosSoloLectura,
           mostrarVerificarNovedades,
           mostrarVerificarEmbargos,
           mostrarVerificarCuentaIndividual
           } = props;
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const apiSolicitud = ApiSolicitud();
    const classes = useStyles();
    const [listaDocumentos, setListadocumentos] = useState<any>([]);
    const [listaDocumentosDodo, setListadocumentosDodo] = useState<any>([]);
    const [loading, setLoading] = useState(false);
    const [mostrarVerificar, setMostrarVerificar] = useState(true);

    useEffect(() => {

      if(datosSolicitudPostulacion){

      if(camposCargueDocumentosSoloLectura){
         listarDocumentosCargados();
      }

      if(camposCargueDocumentosSoloLectura === false){
            const request = {
                idSolicitud: datosSolicitudPostulacion.idSolicitud,
                idModalidad: datosSolicitudPostulacion.idModalidad,
            }
            obtenerListaDocumentosPorModalidad(request);
            consultarDocumentosDodo();
        }
      }
        
    },[datosSolicitudPostulacion]);

    const eliminarDuplicados = (array: any[]) => {
        const arraySinDuplicados: any[] = [array[0]];

        for(let i=0; i < array.length; i+=1){
                let contador = 0;
                for(let j=0; j < arraySinDuplicados.length; j+=1){
                    if(array[i].descripcion === arraySinDuplicados[j].descripcion){
                        contador += 1;
                    }
            }
            if(contador === 0){
                arraySinDuplicados.push(array[i]);
            }
           
        }
        return arraySinDuplicados;
    }

    const listarDocumentosCargados = async() => {
        const request = {
            idSolicitud: datosSolicitudPostulacion.idSolicitud,
            idModalidad: datosSolicitudPostulacion.idSubModalidad,
        }
        await apiFondoSolidaridad
        .postObtenerListaArchivosPorModalidad(request)
        .then(async responseListaArchivos => {
            if(responseListaArchivos.data){
                const radicado = datosSolicitudPostulacion?.noRadicado;
                await apiSolicitud
                    .GetConsultarDocumentosPorSubModalidadDodo(String(radicado))
                    .then(async respuestaListaArchivosDodo => {
                        if(respuestaListaArchivosDodo?.data?.fileProperties !== null){
                            const listaDocumentosActualizada: any = [];
                            for(let i=0; i<responseListaArchivos.data.length; i+=1){
                                let encontrado = false;
                                for(let j=0; j<respuestaListaArchivosDodo.data.fileProperties.length; j+=1){
                                
                                    if(responseListaArchivos.data[i].nombre.toUpperCase()  === respuestaListaArchivosDodo.data.fileProperties[j].description.toUpperCase()){
                                        listaDocumentosActualizada.push({...responseListaArchivos.data[i],verificado: true});
                                        encontrado = true;
                                    }
                                }
                                if(encontrado === false){
                                   listaDocumentosActualizada.push({...responseListaArchivos.data[i],verificado: false});
                                }
                            }
                            // validacion temporal documentos
                            setListadocumentos(eliminarDuplicados(listaDocumentosActualizada));
                            setMostrarVerificar(false);
                        }else if(respuestaListaArchivosDodo?.data?.fileProperties === null &&
                                 respuestaListaArchivosDodo?.data?.success === false){
                                 toast(`El radicado no existe en dodo`, { type: toast.TYPE.ERROR });
                                 setLoading(false);

                        }
                        setLoading(false);
                    })
                    .catch((e) => {
                        toast(`Error al consultar documentos dodo en listarDocumentosCargados ${e}`, { type: toast.TYPE.ERROR });
                        setLoading(false);
                    });
            }
        })
        .catch(() => {
            setLoading(false);
            toast('Error al cargar documentos en listarDocumentosCargados', { type: toast.TYPE.ERROR });
        });
    }

    const obtenerListaDocumentosPorModalidad = async (request: any) => {
        setLoading(true);
        await apiFondoSolidaridad
            .postObtenerListaArchivosPorModalidad(request)
            .then(async response => {
                if(response.data){
                const array = await response.data.map((item:DocumentosPostulacionModel) => {
                    if (Number(item.numeroFolio) === 0) {
                        const newItem = {
                            ...item,
                            numeroFolio: '',
                        };
                        return newItem;
                    }
                    return item;
                });
                    setListadocumentos(array);
                    setLoading(false);
                }
            })
            .catch(() => {
                setLoading(false);
                toast('Error al obtener documentos por modalidad', { type: toast.TYPE.ERROR });
            });
    };

    const consultarDocumentosDodo = async () => {
        setLoading(true);
        const radicado = datosSolicitudPostulacion?.noRadicado;
        await apiSolicitud
            .GetConsultarDocumentosPorSubModalidadDodo(String(radicado))
            .then(async respuesta => {
                setListadocumentosDodo(respuesta.data);
                setLoading(false);
            })
            .catch((e) => {
                toast(`Error al consultar documentos dodo ${e}`, { type: toast.TYPE.ERROR });
                setLoading(false);
            });
    };

    const verificar = (documento: any) => {

        if(listaDocumentosDodo?.fileProperties === null || listaDocumentosDodo?.fileProperties === undefined || listaDocumentosDodo?.fileProperties.length === 0){
            toast(`No hay ningun documento cargado`, { type: toast.TYPE.ERROR });
            return;
        }

        const encontrado = listaDocumentosDodo?.fileProperties.some(
            (element: any) => element.description.toUpperCase() === documento.nombre.toUpperCase()
        );

       const documentosActualizados = listaDocumentos.map((option: any) => {

            if (option.nombre.toUpperCase() === documento.nombre.toUpperCase()) {
                if (encontrado) {
                    documentosAgregados((prevDocumentos: any) => [...prevDocumentos, documento]);
                } else {
                    toast('Archivo no encontrado en Dodo-Doc', { type: toast.TYPE.ERROR });
                }
                return {
                    ...option,
                    verificado: encontrado,
                };
            }
           return option;
       });

        documentosAgregados(documentosActualizados);
        setListadocumentos(documentosActualizados);
    };

    return (
        <>
            <LoadingComponent loading={loading} />
            <CHPaper title="Información Cargue Documentos">
                {mostrarVerificar && (
                     <Button 
                        className={classes.btnRefrescar} 
                        onClick={()=> consultarDocumentosDodo()}
                     >Refrescar Listado DODO</Button>
                )}
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell width="60%">Nombre Archivo</TableCell>
                          
                                    <TableCell width="10%" align="center">
                                        Es requerido
                                    </TableCell>
                                    {mostrarVerificar && (
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
                        {listaDocumentos.map(
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
                                            {option.nombre}
                                        </FormLabel>
                                    </TableCell>
                                  
                                        <TableCell align="center">
                                            {option.requerido === 1 ? 'Si' : 'No'}
                                        </TableCell>
                                        {mostrarVerificar && (
                                            <TableCell
                                                width="30%" align="center"
                                            >
                                                <Button 
                                                onClick={() => verificar(option)}
                                                >
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
                    <TableCell width="15%" align="center">
                        <Grid item xs={4} md={4} >
                        <Controller
                            name="numeroFolios"
                            control={formularioObservacionCargueArchivos.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número de Folios *"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={camposCargueDocumentosSoloLectura}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                        </Grid>
                    </TableCell>
                </Table>
            </CHPaper>
            < ObservacionesPostulacion 
                formulario={formularioObservacionCargueArchivos}
                camposCargueDocumentosSoloLectura={camposCargueDocumentosSoloLectura}
            />
        {
            ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) || 
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_NOVEDADES
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            mostrarVerificarNovedades) && (
                <ObservacionVerificacionPostulacion
                    formulario={formularioObservacionCargueArchivos}
                    nombreCampo='observacionVerificacionNovedades'
                    titulo='Observaciones Adicionales Verificación Novedades'
                    camposVerificacionSoloLectura={camposVerificacionNovedadesSoloLectura}
                />
            )
        }
        {
           ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_DE_EMBARGOS
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            mostrarVerificarEmbargos) && (
                <ObservacionVerificacionPostulacion
                    formulario={formularioObservacionCargueArchivos}
                    nombreCampo='observacionVerificacionEmbargos'
                    titulo='Observaciones Adicionales Verificación Embargos'
                    camposVerificacionSoloLectura={camposVerificacionEmbargosSoloLectura}
                />
            )
        }
        {
           ((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.BLOQUEADO) ||
            (datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.VERIFICACION_CUENTA_INDIVIDUAL
            && datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            mostrarVerificarCuentaIndividual) && (
                <ObservacionVerificacionPostulacion
                    formulario={formularioObservacionCargueArchivos}
                    nombreCampo='observacionVerificacionCuentaIndividual'
                    titulo='Observaciones Adicionales Verificación Cuenta Individual'
                    camposVerificacionSoloLectura={camposVerificacionCuentaIndividualSoloLectura}
                />
            )
        }
       
        </>
    )
}

type propsType = {
    formularioObservacionCargueArchivos: UseFormReturn<any>;
    documentosAgregados: (documentos: any) => void;
    datosSolicitudPostulacion: any;
    camposCargueDocumentosSoloLectura: boolean;
    camposVerificacionNovedadesSoloLectura: boolean;
    camposVerificacionEmbargosSoloLectura: boolean;
    camposVerificacionCuentaIndividualSoloLectura: boolean;
    mostrarVerificarNovedades: boolean;
    mostrarVerificarEmbargos: boolean;
    mostrarVerificarCuentaIndividual: boolean;
 };

export default CargueDocumentosPostulacion;