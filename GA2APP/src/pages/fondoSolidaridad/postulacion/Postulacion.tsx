import React, { useEffect, useState } from 'react';
import { Grid, makeStyles, createStyles, Button } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller } from 'react-hook-form';
import { toast } from 'react-toastify';
import { startGetAllDepartments, startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import { AppState } from '../../../store/rootReducer';
import CiudadPreferenciaParaRecibirViviendaPostulacion from './CiudadPreferenciaParaRecibirViviendaPostulacion';
import FirmaSolicitudPostulacion from './FirmaSolicitudPostulacion';
import InformacionSolicitudPostulacion from './InformacionSolicitudPostulacion';
import ManifestacionesAutorizacionesPostulacion from './ManifestacionesAutorizacionesPostulacion';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import ApiCatalog from '../../../services/api-catalog.service';
import NotificationService from '../../../services/notification.service';
import { CITYDTO } from '../../../types/catalog.type';
import CHPageTitleV2 from '../../../components/ch-page-title/CHPageTitleV2';
import { SolicitudDto } from '../../../models/requests/solicitud-tarea.model';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { ACTIVIDAD_POSTULACION, ESTADO_POSTULACION, ID_ACTIVIDAD_POSTULACION, TIPO_SUBMODALIDAD } from '../constants/fondoSolidaridad.constants';
import { LoadingComponent } from '../../../components/loading/loading.component';
import InformacionBeneficiariosPostulantes from './InformacionBeneficiariosPostulantes';
import { Postulante } from '../constants/fondosolidaridadform.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        buttonAdd: {
            width: '200px',
            marginRight: '15px',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        buttonInconsistencia: {
            width: '220px',
            marginLeft: '.5rem',
        },
    }),
);

const Postulacion = (props: propsType) => {
    const classes = useStyles();
    const apiFondoSolidaridad  = ApiFondoSolidaridad();
    const apiCatalog = ApiCatalog();
    const [ciudadPreferencia1, setCiudadPreferencia1] = useState<CITYDTO[]>([]);
    const [ciudadPreferencia2, setCiudadPreferencia2] = useState<CITYDTO[]>([]);
    const [ciudadPreferencia3, setCiudadPreferencia3] = useState<CITYDTO[]>([]);
    const [loading, setLoading] = useState(false);
   
	const { loginState, 
            catalogState, 
            tipoSolicitud, 
            tipoModalidad, 
            formularioSolicitud,
            formularioRetiro,
            formularioPostulacion,
            datosSolicitudPostulacion,
            selectedData,
            cliente, 
            numeroRadicado,
            existePostulacion,
            camposSolicitudPostulacionSoloLectura,
            beneficiariosPostulantesPostulacion,
            setSelectedData,
            setActividadSolicitud,
            setEstadoSolicitud,
            consultarConvocatorias,
            consultarSolicitudes,
            setAgregarNuevaPostulacion, 
            startGetAllIdentificationTypes, 
            startGetAllDepartments,
            setDatosSolicitudPostulacion,
            setAgregarSolicitudPostulacion,
            actualizarPostulacion,
            setBeneficiariosPostulantesPostulacion,
            actualizarPostulacionAfiliadoFallecido,
            consultarBenficiariosPostulacion
        } = props;

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);
        startGetAllDepartments();
	}, []);

    useEffect(() => {

        if(datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA){
            if(datosSolicitudPostulacion?.idSolicitud !== undefined){
                consultarBenficiariosPostulacion(datosSolicitudPostulacion?.idSolicitud);
            }
        }

        // Si la solicitud esta creada pero no tiene los datos completos del primer formulario
        if( datosSolicitudPostulacion.departamento1 === 0
            && datosSolicitudPostulacion.departameno2 === 0
            && datosSolicitudPostulacion.deparmento3 === 0){
             formularioPostulacion.setValue('idSolicitud',datosSolicitudPostulacion.idSolicitud);
             formularioPostulacion.setValue('tipoDocumentoId',datosSolicitudPostulacion.tipoDocumentoId);  
             formularioPostulacion.setValue('numeroDocumento',datosSolicitudPostulacion.numeroDocumeto);
             formularioPostulacion.setValue('idTipoSolicitud',datosSolicitudPostulacion.idTipoSolicitud);
             formularioPostulacion.setValue('idModalidad',datosSolicitudPostulacion.idModalidad); 
             formularioPostulacion.setValue('idSubModalidad',datosSolicitudPostulacion.idSubModalidad); 
             formularioPostulacion.setValue('numeroConvocatoria',datosSolicitudPostulacion.numeroConvocatoria);
             formularioPostulacion.setValue('disfrutePension',datosSolicitudPostulacion.spoDistruePension  || '');
             formularioPostulacion.setValue('manifestacionAutorizacion', false);
             formularioPostulacion.setValue('departamento1','');
             formularioPostulacion.setValue('ciudad1','');
             formularioPostulacion.setValue('departamento2','');
             formularioPostulacion.setValue('ciudad2','');
             formularioPostulacion.setValue('departamento3','');
             formularioPostulacion.setValue('ciudad3','');
             formularioPostulacion.setValue('firmaSolicitud','');
             formularioPostulacion.setValue('numeroDocumentoFirma','');
             formularioPostulacion.setValue('numeroCelular','');
             formularioPostulacion.setValue('observacion', '');
             formularioPostulacion.setValue('nombreFirma','');
             formularioPostulacion.setValue('apellidoFirma','');
             formularioPostulacion.setValue('representanteLegal',false);
         }

        // Se crea solicitud por primera vez
        if(existePostulacion === false && datosSolicitudPostulacion){
            formularioPostulacion.setValue('tipoDocumentoId',datosSolicitudPostulacion.tipoDocumentoId);  
            formularioPostulacion.setValue('numeroDocumento',datosSolicitudPostulacion.numeroDocumeto);
            formularioPostulacion.setValue('idTipoSolicitud',datosSolicitudPostulacion.idTipoSolicitud);
            formularioPostulacion.setValue('idModalidad',datosSolicitudPostulacion.idModalidad); 
            formularioPostulacion.setValue('idSubModalidad',datosSolicitudPostulacion.idSubModalidad); 
            formularioPostulacion.setValue('numeroConvocatoria',datosSolicitudPostulacion.numeroConvocatoria);
            formularioPostulacion.setValue('disfrutePension',datosSolicitudPostulacion.spoDistruePension || '');
        } 

        // Si existen datos de solicitud
        if(existePostulacion === true 
            && datosSolicitudPostulacion.departamento1 !== 0
            && datosSolicitudPostulacion.departameno2 !== 0
            && datosSolicitudPostulacion.deparmento3 !== 0){
            obtenerCiudadesPreferencia1(Number(datosSolicitudPostulacion.departamento1));
            obtenerCiudadesPreferencia2(Number(datosSolicitudPostulacion.departameno2));
            obtenerCiudadesPreferencia3(Number(datosSolicitudPostulacion.deparmento3));
            formularioPostulacion.setValue('idSolicitud',datosSolicitudPostulacion.idSolicitud);
            formularioPostulacion.setValue('numeroDocumento',datosSolicitudPostulacion.numeroDocumeto);
            formularioPostulacion.setValue('idTipoSolicitud',datosSolicitudPostulacion.idTipoSolicitud);
            formularioPostulacion.setValue('idModalidad',datosSolicitudPostulacion.idModalidad); 
            formularioPostulacion.setValue('idSubModalidad',datosSolicitudPostulacion.idSubModalidad); 
            formularioPostulacion.setValue('disfrutePension',datosSolicitudPostulacion.spoDistruePension  || '');
            formularioPostulacion.setValue('manifestacionAutorizacion',datosSolicitudPostulacion.manifestacionAutorizacion === 1);
            formularioPostulacion.setValue('departamento1',datosSolicitudPostulacion.departamento1);
            formularioPostulacion.setValue('ciudad1',datosSolicitudPostulacion.ciuadad1);
            formularioPostulacion.setValue('departamento2',datosSolicitudPostulacion.departameno2);
            formularioPostulacion.setValue('ciudad2',datosSolicitudPostulacion.ciudad2);
            formularioPostulacion.setValue('departamento3',datosSolicitudPostulacion.deparmento3);
            formularioPostulacion.setValue('ciudad3',datosSolicitudPostulacion.ciudad3);
            formularioPostulacion.setValue('firmaSolicitud',datosSolicitudPostulacion.firmaSolicitud);
            formularioPostulacion.setValue('numeroDocumentoFirma',datosSolicitudPostulacion.numeroDocumentoFirma);
            formularioPostulacion.setValue('numeroCelular',datosSolicitudPostulacion.numeroCelular);
            formularioPostulacion.setValue('numeroConvocatoria',datosSolicitudPostulacion.numeroConvocatoria);
            formularioPostulacion.setValue('observacion',datosSolicitudPostulacion.observacion);
            formularioPostulacion.setValue('nombreFirma',datosSolicitudPostulacion.nombreFirma);
            formularioPostulacion.setValue('apellidoFirma',datosSolicitudPostulacion.apellidoFirma);
            formularioPostulacion.setValue('representanteLegal',datosSolicitudPostulacion.representanteLegal);
            formularioPostulacion.setValue('tipoDocumentoId',datosSolicitudPostulacion.tipoDocumento);  
        }
	}, [datosSolicitudPostulacion]);

    useEffect(() => {
        formularioPostulacion.setValue('nombreAfiliado',`${cliente?.primerNombre} ${cliente?.segundoNombre} ${cliente?.primerApellido} ${cliente?.segundoApellido}`);
	}, [cliente]);

    const actualizar = (data: any) => {

        const informacionPostulacion = {
            idSolicitud: data.idSolicitud,
            tipoDocumentoId: data.tipoDocumentoId,
            numeroDocumeto: Number(data.numeroDocumento),
            idTipoSolicitud: Number(data.idTipoSolicitud),
            idModalidad: Number(data.idModalidad),
            idSubModalidad: Number(data.idSubModalidad),
            manifestacionAutorizacion: data.manifestacionAutorizacion === true? 1 : 0,
            departamento1: Number(data.departamento1),
            ciuadad1: Number(data.ciudad1),
            departameno2: Number(data.departamento2),
            ciudad2: Number(data.ciudad2),
            deparmento3: Number(data.departamento3),
            ciudad3: Number(data.ciudad3),
            firmaSolicitud: Number(data.firmaSolicitud),
            numeroDocumentoFirma:  Number(data.numeroDocumentoFirma),
            numeroCelular:  Number(data.numeroCelular),
            numeroConvocatoria: Number(data.numeroConvocatoria),
            observacion: data.observacion,
            nombreFirma: data.nombreFirma,
            apeliidoFirma: data.apellidoFirma,
            representanteLegal: formularioPostulacion.watch('idSubModalidad') === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA? data.representanteLegal : false,
            spoDistruePension: data.disfrutePension,
            spoUnidadEjecutora: data.unidadEjecutora,
            spoCategoria: data.categoria,
            nombreCompletoAfiliado: data.nombreAfiliado,
            mismoAfiliado: data.mismoAfiliado,
        }

        
        if(datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA){
            actualizarPostulacionAfiliadoFallecido(informacionPostulacion);
        }else{
            actualizarPostulacion(informacionPostulacion);
        }
       
    }

    const aprobar = async(data: any) => {
        setLoading(true);
        await apiFondoSolidaridad
        .postValidarExistePostulacionPorId(String(numeroRadicado || datosSolicitudPostulacion?.noRadicado))
        .then(async (responseExisteSolicitud: any) => {
            setLoading(false);
            if(responseExisteSolicitud?.data?.departamento1 !== 0 &&
               responseExisteSolicitud?.data?.departameno2 !== 0 &&
               responseExisteSolicitud?.data?.deparmento3 !== 0){
                const confirmAlert = await NotificationService.confirmAlert(
                    `¿Está seguro que desea realizar esta acción ?`,
                );
                if(confirmAlert){
                    setLoading(true);
                    await apiFondoSolidaridad
                    .postAprobarSolicitudPostulante(data.idSolicitud)
                    .then(async response => {
                        if(response.data){
                            setLoading(false);
                            await NotificationService.genericAlert(
                                'Exitoso!',
                                'Se realizó la petición con éxito.',
                            );
                            setDatosSolicitudPostulacion(response.data);
                            setActividadSolicitud(response.data?.actividad);
                            setEstadoSolicitud(response.data?.estadoProceso);
                            formularioPostulacion.setValue('afectacionesCuenta',response?.data?.afectacionCuenta);
                            formularioPostulacion.setValue('valorAfectaciones',response?.data?.valorAfectacion);
                            formularioPostulacion.setValue('numeroRadicado',response?.data?.noRadicado);
                        }else if(response.data === null && response.returnMessage){
                            await NotificationService.genericAlert(
                                'Información',
                                `${response.returnMessage}`,
                                'warning'
                            );
                        }
                    })
                    .catch(() => {
                        setLoading(false);
                        toast('Error al aprobar solicitud de postulación', { type: toast.TYPE.ERROR });
                    });
                }
            }else{
                setLoading(false);
                await NotificationService.genericAlert(
                    'Información',
                    `Se debe ingresar la información requerida`,
                    'warning'
                );
            }
        }).catch(() => {
            setLoading(false);
            toast('Error validación existe solicitud en aprobar postulación', { type: toast.TYPE.ERROR });
        });
    }

    const rechazar = async() => {
      if(formularioPostulacion.getValues('observacion') === '' || 
         formularioPostulacion.getValues('observacion') === null){
            NotificationService.genericAlert(
                'Información',
                `Se debe ingresar la observación`,
                'warning'
            );
          return;
        } 

        const confirmAlert = await NotificationService.confirmAlert(
            `¿Está seguro que desea realizar esta acción?`,
        );
        if(confirmAlert){
            const request ={
                idSolicitud: formularioPostulacion.getValues('idSolicitud'),
                etapa: ID_ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD,
                observacion: formularioPostulacion.getValues('observacion'),
            }
            anularPostulacion(request);
        }
    }

    const anularPostulacion = async (request: any) => {
        setLoading(true);
        await apiFondoSolidaridad
            .postAnularEtapa(request)
            .then(async response => {
                if(response.returnMessage === 'OK'){
                    setLoading(false);
                    await NotificationService.genericAlert(
                        'Exitoso!',
                        'Se realizó la petición con éxito.',
                    );
                    handleOnclickRegresar();
                }
            })
            .catch(() => {
                setLoading(false);
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerCiudadesPreferencia1 = async (idDepto: number) => {
		const value = idDepto.toString();
		const response = await apiCatalog.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
			setCiudadPreferencia1(datos);
		}
	};

    const obtenerCiudadesPreferencia2 = async (idDepto: number) => {
		const value = idDepto.toString();
		const response = await apiCatalog.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
			setCiudadPreferencia2(datos);
		}
	};

    const obtenerCiudadesPreferencia3 = async (idDepto: number) => {
		const value = idDepto.toString();
		const response = await apiCatalog.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
			setCiudadPreferencia3(datos);
		}
	};

    const handleOnclickRegresar = () => {
        formularioSolicitud.reset();
        formularioRetiro.reset();
        formularioPostulacion.reset();
        setAgregarNuevaPostulacion(false);
        setActividadSolicitud('');
        setEstadoSolicitud('');
        const busqueda = {
            numeroConvocatoria: '',
            estado: 1,
            fechaDesde: '',
            fechaHasta: '',
            fechaPublicacion: '',
        }
        consultarConvocatorias(busqueda);
        const request = {
            radicado: '',
			idEtapa: '',
			identificacion: '',
			tipoModalidad: '',
            numeroConvocatoria: '',
		}
        consultarSolicitudes(request);
        setAgregarSolicitudPostulacion(false);
        setSelectedData(null);
    }

    if (loading) {
		return 	<LoadingComponent loading={loading} />
	}

    return (
    <>
        <CHPageTitleV2
            topText="Gestión de postulaciones"
            title1="Solicitud Postulación"
            title2='Fondo Solidaridad'
            icono="fact_check"
		/>
        <InformacionSolicitudPostulacion 
            cliente={cliente} 
            catalogState={catalogState} 
            tipoSolicitud={tipoSolicitud !== 0 && tipoSolicitud !== undefined? Number(tipoSolicitud) : Number(selectedData?.idTipoSolicitud)} 
            tipoModalidad={tipoModalidad !== 0 && tipoSolicitud !== undefined? Number(tipoModalidad) : Number(selectedData?.idModalidad)} 
            tipoSubModalidad={datosSolicitudPostulacion.idSubModalidad? datosSolicitudPostulacion.idSubModalidad : Number(selectedData?.idSubModalidad)}
            formularioPostulacion = {formularioPostulacion}
        />
        <ManifestacionesAutorizacionesPostulacion 
            formularioPostulacion = {formularioPostulacion}
            camposSolicitudPostulacionSoloLectura = {camposSolicitudPostulacionSoloLectura}
        />
        <CiudadPreferenciaParaRecibirViviendaPostulacion 
            catalogState={catalogState}
            formularioPostulacion = {formularioPostulacion}
            ciudadPreferencia1={ciudadPreferencia1}
            ciudadPreferencia2={ciudadPreferencia2}
            ciudadPreferencia3={ciudadPreferencia3}
            camposSolicitudPostulacionSoloLectura={camposSolicitudPostulacionSoloLectura}
            obtenerCiudadesPreferencia1={obtenerCiudadesPreferencia1}
            obtenerCiudadesPreferencia2={obtenerCiudadesPreferencia2}
            obtenerCiudadesPreferencia3={obtenerCiudadesPreferencia3}
        />
        {
            datosSolicitudPostulacion?.idSubModalidad && datosSolicitudPostulacion?.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA && (
                <InformacionBeneficiariosPostulantes
                   catalogState={catalogState}
                   setBeneficiariosPostulantesPostulacion={setBeneficiariosPostulantesPostulacion}
                   beneficiariosPostulantesPostulacion={beneficiariosPostulantesPostulacion}
                   camposSolicitudPostulacionSoloLectura={camposSolicitudPostulacionSoloLectura}
                />
            )
        }
        <FirmaSolicitudPostulacion 
            catalogState={catalogState} 
            cliente={cliente}
            formularioPostulacion = {formularioPostulacion}
            tipoSubModalidad={datosSolicitudPostulacion.idSubModalidad? datosSolicitudPostulacion.idSubModalidad : Number(selectedData?.idSubModalidad)}
            camposSolicitudPostulacionSoloLectura={camposSolicitudPostulacionSoloLectura}
        />
       <CHPaper title="Observaciones">
          <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="observacion"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={camposSolicitudPostulacionSoloLectura}
                                    label="Observaciones Adicionales *"
                                    value={value}
                                    onChange={e => {
                                        if(e.target.value.length <= 350){
                                            onChange(e);
                                        }
                                    }}
                                    multiline
                                    rows={7}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
             <Grid container>
                <Grid
                    className={classes.columnButtonAdd}
                    item
                    xs={12}
                    md={12}
                >
            {((existePostulacion === false) ||
            (datosSolicitudPostulacion?.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD && 
            datosSolicitudPostulacion?.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion?.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD && 
            datosSolicitudPostulacion?.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) && (
           
                 <Button
                 className={classes.button}
                 variant="outlined"
                 onClick={formularioPostulacion.handleSubmit(actualizar)}
                 >
                    ACTUALIZAR
                </Button>
            )}

            {((existePostulacion === false) ||
            (datosSolicitudPostulacion?.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD && 
            datosSolicitudPostulacion?.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion?.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD && 
            datosSolicitudPostulacion?.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) && (
             <Button
                 className={classes.button}
                 variant="outlined"
                 color="default"
                 onClick={formularioPostulacion.handleSubmit(aprobar)}
             >
                 APROBAR
             </Button>
              )}

            {((existePostulacion === false) ||
            (datosSolicitudPostulacion?.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD && 
             datosSolicitudPostulacion?.estadoProceso === ESTADO_POSTULACION.PENDIENTE) ||
            (datosSolicitudPostulacion?.actividad === ACTIVIDAD_POSTULACION.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD && 
             datosSolicitudPostulacion?.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE)) && (
             <Button
                 className={classes.button}
                 variant="outlined"
                 color="default"
                 onClick={rechazar}
             >
                ANULAR
             </Button>
              )}
                <Button
                    className={classes.button}
                    variant="outlined"
                    color="default"
                    onClick={handleOnclickRegresar}
                >
                    REGRESAR
                </Button>
            </Grid>
        </Grid>
    </>
    )
}

interface LinkMapStateToProps {
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog
});

interface LinkMapDispatchToProps {
    loginState: LoginState;
    tipoSolicitud: number;
    tipoModalidad: number;
    formularioSolicitud: any;
    formularioRetiro: any;
    formularioPostulacion: any;
    datosSolicitudPostulacion: any | null;
    selectedData: SolicitudDto | null;
    cliente: any;
    existePostulacion: boolean;
    numeroRadicado: string;
    camposSolicitudPostulacionSoloLectura: boolean;
    beneficiariosPostulantesPostulacion: Postulante[];
    setSelectedData: (value: SolicitudDto | null) => void;
    consultarConvocatorias: (busqueda: any) => void;
    consultarSolicitudes: (request: any) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetAllDepartments: () => void;
    setAgregarNuevaPostulacion: (value: boolean) => void;
    setActividadSolicitud: (value: string) => void;
    setDatosSolicitudPostulacion: (value: any) => void;
    setEstadoSolicitud: (value: any) => void;
    setAgregarSolicitudPostulacion: (value: boolean) => void;
    actualizarPostulacion: (datosPostulacion: any) => void;
    setBeneficiariosPostulantesPostulacion: (value: Postulante[]) => void;
    actualizarPostulacionAfiliadoFallecido: (datosPostulacion: any) => void;
    consultarBenficiariosPostulacion: (idSolicitud: string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetAllDepartments
};

type propsType = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Postulacion);