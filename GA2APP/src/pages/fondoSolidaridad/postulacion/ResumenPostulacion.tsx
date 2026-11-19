import { createStyles, 
         Grid, 
         makeStyles, 
         Typography } from '@material-ui/core';
import moment from 'moment';
import React, {useState, useEffect} from 'react';
import { Controller } from 'react-hook-form';
import { toast } from 'react-toastify';
import AttachFileIcon from '@material-ui/icons/AttachFile';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHTextFieldIcon from '../../../components/ch-text-field-icon/CHTextFieldIcon';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { TIPO_SOLICITUD, TIPO_SUBMODALIDAD } from '../constants/fondoSolidaridad.constants';
import ResumenCiudadesPreferencia from './ResumenCiudadesPreferencia';
import ResumenTramite from './ResumenTramite';
import ApiSolicitud from '../../../services/api-solicitud.service';
import InformacionBeneficiariosPostulantes from './InformacionBeneficiariosPostulantes';
import { Postulante } from '../constants/fondosolidaridadform.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        text: {
            fontSize: '.5rem',
        },
    }),
);

const ResumenPostulacion = (props: propsType) => {
    const classes = useStyles();
    const apiSolicitud = ApiSolicitud();
    const {cliente, 
           tipoSolicitud,
           formularioResumenInformacionSolicitud,
           formularioObservacionCargueArchivos,
           datosSolicitudPostulacion, 
           beneficiariosPostulantesPostulacion,
           catalogState,
           consultarBenficiariosPostulacion,
           setBeneficiariosPostulantesPostulacion} = props;
    const [linkDodo, setLinkDodo] = useState('');

    useEffect(() => {
            obtenerUrlDodo();

            if(datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA){
                if(datosSolicitudPostulacion?.idSolicitud !== undefined){
                    consultarBenficiariosPostulacion(datosSolicitudPostulacion?.idSolicitud);
                }
            }

            formularioResumenInformacionSolicitud.setValue('nombres',`${cliente?.primerNombre || ""} ${cliente?.segundoNombre || ""} `);
            formularioResumenInformacionSolicitud.setValue('apellidos',`${cliente?.primerApellido || ""} ${cliente?.segundoApellido || ""}`);
            formularioResumenInformacionSolicitud.setValue('numeroDocumento',cliente?.identificacion);
            formularioResumenInformacionSolicitud.setValue('celular',cliente?.telefono);
            formularioResumenInformacionSolicitud.setValue('email',cliente?.correo);
            formularioResumenInformacionSolicitud.setValue('tipoSolicitud',datosSolicitudPostulacion?.tipoSolicitudNombre);
            formularioResumenInformacionSolicitud.setValue('modalidad',datosSolicitudPostulacion?.tipoModalidadNombre);
            formularioResumenInformacionSolicitud.setValue('submodalidad',datosSolicitudPostulacion?.tipoSubModalidadNombre);
            formularioResumenInformacionSolicitud.setValue('fechaInicio',moment(datosSolicitudPostulacion?.fechaCreacion).format('YYYY-MM-DD h:mm A'));
            formularioResumenInformacionSolicitud.setValue('totalFolios',formularioObservacionCargueArchivos.getValues('numeroFolios') || '');
            formularioResumenInformacionSolicitud.setValue('modeloSolucionVivienda',datosSolicitudPostulacion?.modalidadSolucionVivienda);
            formularioResumenInformacionSolicitud.setValue('numeroConvocatoria',datosSolicitudPostulacion?.numeroConvocatoria);
            formularioResumenInformacionSolicitud.setValue('numeroResolucionApertura',datosSolicitudPostulacion?.numeroResolucion);
            formularioResumenInformacionSolicitud.setValue('fechaResolucionApertura',datosSolicitudPostulacion?.fechaApertura !== null? moment(datosSolicitudPostulacion?.fechaApertura).format('YYYY-MM-DD h:mm A') : '');
            formularioResumenInformacionSolicitud.setValue('numeroResolucionCierre',datosSolicitudPostulacion?.numeroResolucionCierre);
            formularioResumenInformacionSolicitud.setValue('observacion',String(datosSolicitudPostulacion?.observacion).toUpperCase());
    }, [cliente, datosSolicitudPostulacion]);

    const obtenerUrlDodo = async() => {
        const radicado = datosSolicitudPostulacion?.noRadicado;
        if(radicado){
            await apiSolicitud
            .GetBoteneEnlaceDodo(String(radicado))
            .then(async respuesta => {
                setLinkDodo(respuesta.data);
            })
            .catch(() => {
                toast('Error del sistema al obtener url de Dodo', { type: toast.TYPE.ERROR });
            });
        }
	}

    return (
         <>
            <CHPrivateContainer>
                <CHPaper title="Información Solicitud">
                    <Grid container>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        />
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="nombres"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Nombres"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                               <Controller
                                    name="apellidos"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Apellidos"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="numeroDocumento"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Número Documento"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="tipoSolicitud"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Tipo Solicitud"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={6}>
                                <Controller
                                    name="modalidad"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Modalidad (Modelo)"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={6}>
                                <Controller
                                    name="submodalidad"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Sub-Modalidad (Calidad Postulación)"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="telefono"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Teléfono"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="celular"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Celular"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                               <Controller
                                    name="email"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Correo Electrónico"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="fechaInicio"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Fecha Inicio"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="fechaFinal"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Fecha Final"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="fechaFinal"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={() => (
                                        <CHTextFieldIcon
                                            label="Número Radicado"
                                            icon={(
                                                <a  href={linkDodo} target="_blank" rel="noopener noreferrer">
                                                    <AttachFileIcon />
                                                </a>
                                            )}
                                            value={datosSolicitudPostulacion?.noRadicado || ''}
                                            InputLabelProps={{ shrink: true }}
                                            isReadOnly
                                        />
                                    )}
                                />
                             
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="totalFolios"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Total Folios"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="modeloSolucionVivienda"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Modelo de Solución de Vivienda"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="numeroConvocatoria"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Número de Convocatoria"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="numeroResolucionApertura"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Número de Resolución de Apertura"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="fechaResolucionApertura"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Fecha de Resolución de Apertura"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                               <Controller
                                    name="numeroResolucionCierre"
                                    control={formularioResumenInformacionSolicitud.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            label="Número de Resolución de Cierre"
                                            value={value}
                                            onChange={onChange}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={12}>
                            <Controller
                                name="observacion"
                                control={formularioResumenInformacionSolicitud.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        label="Observaciones"
                                        value={value}
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                </CHPaper>
                {tipoSolicitud !== TIPO_SOLICITUD.PERDIDA_BENEFICIO &&(
                    <ResumenCiudadesPreferencia
                     catalogState={catalogState}
                     datosSolicitudPostulacion={datosSolicitudPostulacion}
                    />
                )}
                {
                    datosSolicitudPostulacion && 
                    datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA 
                    && (
                        <InformacionBeneficiariosPostulantes 
                           catalogState={catalogState} 
                           setBeneficiariosPostulantesPostulacion={setBeneficiariosPostulantesPostulacion} 
                           beneficiariosPostulantesPostulacion={beneficiariosPostulantesPostulacion} 
                           camposSolicitudPostulacionSoloLectura                      
                        />
                    )
                }
               {tipoSolicitud !== TIPO_SOLICITUD.PERDIDA_BENEFICIO &&(
                    <ResumenTramite
                     catalogState={catalogState}
                     datosSolicitudPostulacion={datosSolicitudPostulacion}
                    />
                )}
            </CHPrivateContainer>
        </>
    )
}

type propsType = {
   cliente: any;
   catalogState: CatalogState;
   tipoSolicitud: number;
   formularioResumenInformacionSolicitud: any;
   formularioObservacionCargueArchivos: any;
   datosSolicitudPostulacion: any;
   beneficiariosPostulantesPostulacion: Postulante[];
   consultarBenficiariosPostulacion: (idSolicitud: string) => void;
   setBeneficiariosPostulantesPostulacion: (value: Postulante[]) => void;
};

export default ResumenPostulacion;
