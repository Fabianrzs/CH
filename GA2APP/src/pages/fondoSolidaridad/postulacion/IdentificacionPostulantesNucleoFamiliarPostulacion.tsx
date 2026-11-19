import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    MenuItem,
    Table,
    TableCell,
    TableHead,
    TableRow,
    Typography,
    createStyles,
    TableBody,
    IconButton,
} from '@material-ui/core';
import { Controller, UseFormReturn } from 'react-hook-form';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { sortBy } from 'lodash';
import { Delete, Edit, Visibility } from '@material-ui/icons';
import moment from 'moment';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import { startGetAllIdentificationTypes,startGetAllProfessions, startGetAllDepartments } from '../../../actions/catalog.action';
import ApiCatalog from '../../../services/api-catalog.service';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import { CITYDTO, DEPARTAMENTODTO, ESTADOCIVILDTO, IDENTIFICATIONTYPEDTO, PARENTESCO, SEXODTO } from '../../../types/catalog.type';
import { ACTIVIDAD_POSTULACION, CATALOG, ESTADO_POSTULACION, TIPO_SUBMODALIDAD } from '../constants/fondoSolidaridad.constants';
import { Postulante } from '../constants/fondosolidaridadform.model.type';
import UtilService from '../../../utils/util.service';
import NotificationService from '../../../services/notification.service';
import { LoadingComponent } from '../../../components/loading/loading.component';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
    }),
);

function IdentificacionPostulantesNucleoFamiliarPostulacion(props: Props) {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const { loginState, 
            datosSolicitudPostulacion,
            catalogState, 
            datosEntrevista,
            cliente,
            listaTiposIntegrantes,
            listaTiposDiscapacidad,
            listaTiposAyudaTecnica,
            camposEntrevistaSoloLectura,
            postulantesControl,
            setListaPostulantes,
            consultarTiposIntegrantes, 
            consultarTiposDiscapcidad,
            consultarTiposAyudaTecnica,
            startGetAllDepartments,
            startGetAllIdentificationTypes,
            startGetAllProfessions,
         } = props;
   
    const [ciudades, setCiudades] = useState<CITYDTO[]>([]);
    const [listaParentesco, setListaParentesco] = useState<PARENTESCO[]>([]);
    const [listaGenero, setListaGenero] = useState<SEXODTO[]>([]);
    const [listaNivelesAcademicos, setListaNivelesAcademicos] = useState<any[]>([]);
    const [mostrarListaAyudaTecnica, setMostrarListaAyudaTecnica] = useState(false);
    const [loading, setLoading] = useState(false);
    const [estadoBoton, setEstadoBoton] = useState<string>(
        'AGREGAR POSTULANTE',
    );

    useEffect(() => {
            startGetAllIdentificationTypes(loginState.claims);
            startGetAllDepartments(loginState.claims);
            consultarCatalogoListaGenero();
            consultarCatalogoListaParentesco();
            consultarCatalogoListaNivelesAcademicos(CATALOG.NIVELES_ACADEMICOS);
            startGetAllProfessions(loginState.claims);
            consultarTiposIntegrantes();
            consultarTiposDiscapcidad();
            consultarTiposAyudaTecnica();
            if((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION && 
                datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)){
                    setEstadoBoton('EDITAR POSTULANTE');
            }
    }, []);

    useEffect(() => {
        setLoading(true);
        if(cliente){
            setLoading(false);
        }
    }, [cliente]);

   const [postulantesModel, setPostulantesModel] = useState<Postulante[]>(datosEntrevista.postulantes !== null? datosEntrevista.postulantes : []);

    const [postulanteAnterior, setPostulanteAnterior] =
        useState<number>(0);
    const [idAnterior, setIdAnterior] = useState<number>(0);

    const obtenerCiudades = async (idDepto: number) => {
        const value = String(idDepto);
        const response = await apiCatalog.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCiudades(datos);
        }
    };

    const consultarCatalogoListaParentesco = async () => {
        await apiFondoSolidaridad
            .getObtenerTipoParentesco()
            .then(async (response: any) => {
                if (response.data) {
                        const datosOrdenados = sortBy(
                            response.data,
                            'descripcion',
                        );
                        
                        setListaParentesco(datosOrdenados);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const consultarCatalogoListaNivelesAcademicos = async (idCatalogo: number) => {
        const SIN_ESTUDIOS = "S";
        const ordenNivelesAcademicos = [
            "PRE",  // PREESCOLAR
            "P",    // PRIMARIA
            "B",    // BACHILLERATO
            "T",    // TÉCNICO
            "TEC",  // TECNÓLOGO
            "R",    // PROFESIONAL
            "E",    // POSGRADO
            "F"     // FORMACIÓN PARA EL TRABAJO
        ];
        await apiCatalog
            .getObtenerValoresCatalogosPorIdCatalogo(idCatalogo)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        const array = response?.data
                        .filter((item: any) => item?.codigo !== SIN_ESTUDIOS)
                        .sort((a: any, b: any) =>
                            ordenNivelesAcademicos.indexOf(a.codigo) - ordenNivelesAcademicos.indexOf(b.codigo)
                        );
                        setListaNivelesAcademicos(array);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const consultarCatalogoListaGenero = async () => {
        await apiCatalog
            .getAllGeneros()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setListaGenero(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const agregarPostulantes = async (data: any) => {
       
        const datoEncontrado = postulantesModel.findIndex(
            item =>
                item.numeroDocumento === data.numeroDocumento &&
                item.numeroDocumento !== postulanteAnterior,
        );

        if (datoEncontrado === -1) {
            await agregarRegistroPostulante(data);
        }else {
            toast('El postulante ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const agregarRegistroPostulante = async (data: any) => {
        const JEFE_DE_HOGAR = 'JDH';
        if(moment(data.fechaNacimiento).format('YYYY-MM-DD') === '0001-01-01'){
            toast('Fecha de nacimiento no válida', {
                type: toast.TYPE.ERROR,
            });
            return;
        }

        if(postulantesModel.some(item => item.tipoParentescoId === JEFE_DE_HOGAR) && data.parentesco === JEFE_DE_HOGAR){
            toast('Ya existe un postulante como Jefe de Hogar', {
                        type: toast.TYPE.ERROR,
                });
            return;
        }
      
        const postulanteNuevo: Postulante = {
            nombre: String(data.nombre).toLocaleUpperCase(),
            apellido: String(data.apellido).toLocaleUpperCase(),
            tipoDocumentoId: Number(data.tipoDocumentoId),
            numeroDocumento: data.numeroDocumento,
            departamentoExpedicion: data.departamentoExpedicion,
            ciudadExpedicion: data.ciudadExpedicion,
            parentesco: '0',
            sexo: data.sexo,
            fechaNacimiento: moment(data.fechaNacimiento).format('YYYY-MM-DD'),
            estadoCivil: data.estadoCivil,
            ocupacion: data.ocupacion,
            nivelEducativo: data.nivelEducativo,
            afectionFisica: String(data.afectionFisica).toLocaleUpperCase(),
            afectacionSensorial: String(data.afectacionSensorial).toLocaleUpperCase(),
            enfermedadEspecifica: String(data.enfermedadEspecifica).toLocaleUpperCase(),
            tratamientoMedico: String(data.tratamientoMedico).toLocaleUpperCase(),
            dificultadEscaleras: String(data.dificultadEscaleras).toLocaleUpperCase(),
            medicoPermanente: String(data.medicoPermanente).toLocaleUpperCase(),
            ayudaTecnica: String(data.ayudaTecnica).toLocaleUpperCase(),
            tipoIntegranteId: data.tipoIntegrante,
            tipoParentescoId: data.parentesco,
            tipAyudaTecnicaId: data.tipoAyudaTecnica,
            tipoDiscapacidadId: data.discapacidad,
            condicionSalud: String(data.condicionSalud).toLocaleUpperCase(),
            porcentajePostulacion: Number(data.porcentajePostulacion.replace('%','')),
            porcentajeAportes: Number(data.porcentajeAportes.replace('%','')),
            porcentajeCesantias: Number(data.porcentajeCesantias.replace('%','')),
        };

        let postulantes: Postulante[] = [];
        if (estadoBoton === 'AGREGAR POSTULANTE') {

            setPostulantesModel(oldArray => [
                ...oldArray,
                postulanteNuevo,
            ]);
            postulantes = [...postulantesModel];
            postulantes.push(postulanteNuevo);
        } else {
            const temp = [...postulantesModel];
            const posicionActual = temp.findIndex(
                item => item.numeroDocumento === postulanteAnterior,
            );

            postulanteNuevo.numeroDocumento = temp[posicionActual].numeroDocumento;

            if (
                Number(postulanteNuevo.numeroDocumento) !==
                temp[posicionActual].numeroDocumento
            ) {
                temp[posicionActual].numeroDocumento =
                    postulanteNuevo.numeroDocumento;
            } else {
                postulanteNuevo.numeroDocumento = idAnterior;
            }

            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, postulanteNuevo);
            postulantes = temp;
            setPostulantesModel(temp);
            await NotificationService.genericAlert(
                'Actualizado con éxito',
                'Se realizó con éxito la actualización del registro.',
            );
        }
        setListaPostulantes(postulantes);
        setPostulanteAnterior(0);
        resetPostulanteNuevo();
    };

    const seleccionarRegistro = async (item: Postulante) => {
        setPostulanteAnterior(item.numeroDocumento);
        setIdAnterior(item.numeroDocumento);
        if(item.ayudaTecnica === 'SI'){
            setMostrarListaAyudaTecnica(true);
        }else{
            setMostrarListaAyudaTecnica(false);
        }
        postulantesControl.setValue('id', item.id || '');
        postulantesControl.setValue(
            'nombre',
            item.nombre,
        );
        postulantesControl.setValue(
            'apellido',
            item.apellido,
        );
        obtenerCiudades(item.departamentoExpedicion || 0);
        if(camposEntrevistaSoloLectura){
            const tipoIntegrante = listaTiposIntegrantes.find((tipo: any) => tipo.codigo ===  item.tipoIntegranteId);
            postulantesControl.setValue('tipoIntegrante', tipoIntegrante?.descripcion);
        }else{
            postulantesControl.setValue('tipoIntegrante', item.tipoIntegranteId);
        }
        postulantesControl.setValue('tipoDocumentoId', String(item.tipoDocumentoId));
        postulantesControl.setValue('numeroDocumento', String(item.numeroDocumento));
        postulantesControl.setValue('departamentoExpedicion', String(item.departamentoExpedicion));
        postulantesControl.setValue('ciudadExpedicion', String(item.ciudadExpedicion));
        postulantesControl.setValue('parentesco', String(item.parentesco));
        postulantesControl.setValue('sexo', item.sexo);
        postulantesControl.setValue('fechaNacimiento',item.fechaNacimiento? new Date(item.fechaNacimiento) : new Date());
        postulantesControl.setValue('estadoCivil', item.estadoCivil);
        postulantesControl.setValue('ocupacion', String(item.ocupacion));
        postulantesControl.setValue('nivelEducativo', String(item.nivelEducativo));
        postulantesControl.setValue('afectionFisica', item.afectionFisica);
        postulantesControl.setValue('afectacionSensorial', item.afectacionSensorial);
        postulantesControl.setValue('enfermedadEspecifica', item.enfermedadEspecifica);
        postulantesControl.setValue('tratamientoMedico', item.tratamientoMedico);
        postulantesControl.setValue('dificultadEscaleras', item.dificultadEscaleras);
        postulantesControl.setValue('medicoPermanente', item.medicoPermanente);
        postulantesControl.setValue('ayudaTecnica', item.ayudaTecnica);
        postulantesControl.setValue('condicionSalud', item.condicionSalud);
        postulantesControl.setValue('parentesco', item.tipoParentescoId);
        postulantesControl.setValue('tipoAyudaTecnica', String(item.tipAyudaTecnicaId));
        postulantesControl.setValue('discapacidad', item.tipoDiscapacidadId);
        postulantesControl.setValue('porcentajePostulacion', item.porcentajePostulacion === 0? '' : String(item.porcentajePostulacion));
        postulantesControl.setValue('porcentajeAportes', item.porcentajeAportes === 0? '' : String(item.porcentajeAportes));
        postulantesControl.setValue('porcentajeCesantias', item.porcentajeCesantias === 0? '' : String(item.porcentajeCesantias));
        setEstadoBoton('EDITAR POSTULANTE');
    };

    const resetPostulanteNuevo = async () => {
        if((datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION && 
            datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) && 
           ((postulantesControl.watch('tipoIntegrante') === 'MAFP' || 
             postulantesControl.watch('tipoIntegrante') === 'BP'))){
                setEstadoBoton('EDITAR POSTULANTE');
        }else{
            setEstadoBoton('AGREGAR POSTULANTE');
            postulantesControl.setValue('id', '');
            postulantesControl.setValue('nombre', '');
            postulantesControl.setValue('apellido', '');
            postulantesControl.setValue('tipoDocumentoId', '');
            postulantesControl.setValue('numeroDocumento', '');
            postulantesControl.setValue('departamentoExpedicion', '');
            postulantesControl.setValue('ciudadExpedicion', '');
            postulantesControl.setValue('parentesco', '');
            postulantesControl.setValue('sexo', '');
            postulantesControl.setValue('fechaNacimiento', new Date());
            postulantesControl.setValue('estadoCivil', '');
            postulantesControl.setValue('ocupacion', '');
            postulantesControl.setValue('nivelEducativo', '');
            postulantesControl.setValue('afectionFisica', '');
            postulantesControl.setValue('afectacionSensorial', '');
            postulantesControl.setValue('enfermedadEspecifica', '');
            postulantesControl.setValue('tratamientoMedico', '');
            postulantesControl.setValue('dificultadEscaleras', '');
            postulantesControl.setValue('medicoPermanente', '');
            postulantesControl.setValue('ayudaTecnica', '');
            postulantesControl.setValue('condicionSalud', '');
            postulantesControl.setValue('tipoIntegrante', '');
            postulantesControl.setValue('parentesco', '');
            postulantesControl.setValue('tipoAyudaTecnica', '');
            postulantesControl.setValue('discapacidad', '');
        }
    };

    const eliminarRegistro = async (numero: number) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...postulantesModel];
            temp.splice(
                temp.findIndex(item => item.numeroDocumento === numero),
                1,
            );
            setPostulantesModel(temp);
            setListaPostulantes(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
            resetPostulanteNuevo();
        }
    };

    const mostrarTipoIntegrante = (integrante: string) => {
        const tipoIntegrante = listaTiposIntegrantes.find((item: any) => item.codigo === integrante);
        return tipoIntegrante?.descripcion;
    }

    const mostrarParentesco = (parentesco: string) => {
        const tipoParentesco = listaParentesco.find(item => item.codigo === parentesco);
        return tipoParentesco?.descripcion;
    }

    const mostrarTipoIdentificacion = (numeroTipoDocumento: number) => {
        const tipoDocumento = catalogState.IdentificationType.find(item => item.id === numeroTipoDocumento);
        return tipoDocumento?.descripcion;
    }

    const mostrarGenero = (codigo: string) => {
        const genero = listaGenero.find(item => item.codigo === codigo);
        return genero?.descripcion;
    }

    const mostrarEstadoCivil = (id: string) => {
        let descripcionEstadoCivil = '';
        if(id !== ''){
            const estadoCivil = catalogState?.civilstates.find(item => item.id === id);
            descripcionEstadoCivil = estadoCivil?.descripcion === 'UNIÓN LIBRE'? estadoCivil?.descripcion : `${estadoCivil?.descripcion} (A)`;
        }
        return descripcionEstadoCivil;
    }

    return (
        <>
            <LoadingComponent loading={loading} />
            <CHPaper title="Identificación de los postulantes y su núcleo familiar">
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                {camposEntrevistaSoloLectura === false && (
                    <Controller
                        name="tipoIntegrante"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={postulantesControl.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                readOnly={camposEntrevistaSoloLectura}
                                label="Tipo Integrante *"
                                value={value}
                                onChange={e => {
                                    if(e.target.value === "MAFP"){
                                        postulantesControl.setValue('nombre', `${cliente?.primerNombre} ${cliente?.segundoNombre}`);
                                        postulantesControl.setValue('apellido', `${cliente?.primerApellido} ${cliente?.segundoApellido}`);
                                        postulantesControl.setValue('tipoDocumentoId', cliente?.idTipoIdentificacion);
                                        postulantesControl.setValue('numeroDocumento', cliente?.identificacion);
                                        postulantesControl.setValue('departamentoExpedicion', cliente?.idDepartamentoExpedicionDocumento);
                                        obtenerCiudades( cliente?.idDepartamentoExpedicionDocumento);
                                        postulantesControl.setValue('ciudadExpedicion', cliente?.idCiudadExpedicionDocumento);
                                        postulantesControl.setValue('parentesco', 'N');
                                        postulantesControl.setValue('sexo', cliente?.sexo);
                                        postulantesControl.setValue('fechaNacimiento', cliente?.fechaNacimiento);
                                        postulantesControl.setValue('estadoCivil', cliente?.estadoCivil);
                                        postulantesControl.setValue('ocupacion', cliente?.idProfesion);
                                    }
                                    onChange(e);
                                }}
                                error={!!error}
                                helperText={error ? error.message : null}
                                required
                            >
                              {(datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA ||
                                datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.MECANISMOS_ESPECIALES) && (
                                    listaTiposIntegrantes?.map((option: any) => {
                                    // No se muestra BENEFICIARIO/POSTULANTE
                                    if(option.codigo !== "BP"){
                                        return (
                                            <MenuItem
                                            key={option.codigo}
                                            value={option.codigo}
                                        >
                                            {option.descripcion}
                                        </MenuItem> 
                                        );
                                    }
                                    return null;
                                })
                                )} 

                              {datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA && (
                                    listaTiposIntegrantes?.map((option: any) => {
                                        // No se muestra MISMO AFILIADO/POSTULANTE
                                        if(option.codigo !== "MAFP"){
                                            return (
                                                <MenuItem
                                                key={option.codigo}
                                                value={option.codigo}
                                            >
                                                {option.descripcion}
                                            </MenuItem> 
                                            );
                                        }
                                        return null;
                                    })
                                )}
                            </CHSelect>
                        )}
                    />
                )}
                {camposEntrevistaSoloLectura && (
                     <Controller
                        name="tipoIntegrante"
                        control={postulantesControl.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                isReadOnly
                                error={!!error}
                                helperText={error ? error.message : null}
                                label="Tipo Integrante *"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                inputProps={{
                                    style: {
                                        textTransform: 'uppercase',
                                    },
                                }}
                                onChange={onChange}
                            />
                     )}
                 /> 
                )}
                </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombre"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombres *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 25){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                    '')
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="apellido"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Apellidos *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 25){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                   '',),
                                           );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoDocumentoId"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={postulantesControl.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposEntrevistaSoloLectura}
                                    label="Tipo documento *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                >
                                    {catalogState?.IdentificationType.map(
                                        (option: IDENTIFICATIONTYPEDTO) => {
                                            if(option.descripcion !== "CÓDIGO JUZGADO" &&
                                               option.descripcion !== "CODIGO MILITAR" &&
                                               option.descripcion !== "NÚMERO DE IDENTIFICACIÓN TRIBUTARIA"){
                                                return (
                                                    <MenuItem
                                                        key={option.id}
                                                        value={option.id}
                                                    >
                                                     {option.descripcion}
                                                    </MenuItem>
                                                )
                                            }
                                            return null;
                                    })}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroDocumento"
                            rules={{
                                required: 'Este campo es requerido.',
                                maxLength: {
                                    value: 15,
                                    message:
                                        'La longitud máxima del campo es de 15 digito',
                                },
                            }}
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    isReadOnly={camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Documento *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
										if(e.target.value.length <= 12){
											onChange(e);
										}
									}}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamentoExpedicion"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposEntrevistaSoloLectura}
                                    label="Departamento expedición *"
                                    value={value}
                                    onChange={e => {
                                        obtenerCiudades(Number(e.target.value));
                                        onChange(e);
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    required
                                >
                                    {catalogState.departments?.map((option: DEPARTAMENTODTO) => {
                                        if (option.id !== 0) {
                                            return (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.nombre}
                                                </MenuItem>
                                            );
                                        }
                                        return null;
                                    })}

                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudadExpedicion"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposEntrevistaSoloLectura}
                                    label="Ciudad expedición *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {ciudades?.map((option: CITYDTO) => {
                                        if (option.id !== 0) {
                                            return (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.codigo}
                                                >
                                                    {option.nombre}
                                                </MenuItem>
                                            );
                                        }
                                        return null;
                                    })}

                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="parentesco"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposEntrevistaSoloLectura}
                                    label="Parentesco *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {listaParentesco?.map((option: PARENTESCO) => (
                                        <MenuItem
                                            key={option.codigo}
                                            value={option.codigo}
                                        >
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    </Grid>
                    <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="sexo"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposEntrevistaSoloLectura}
                                    label="Sexo *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {listaGenero?.map((option: SEXODTO) => (
                                        <MenuItem
                                            key={option.id}
                                            value={option.codigo}
                                        >
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaNacimiento"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Nacimiento"
                                    InputLabelProps={{ shrink: true}}
                                    inputProps={{ readOnly: camposEntrevistaSoloLectura }}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    disableFuture
                                    autoOk
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="estadoCivil"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={postulantesControl.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    label="Estado civil *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {catalogState?.civilstates.map((option: ESTADOCIVILDTO) =>(
                                          
                                        <MenuItem
                                            key={option.id}
                                            value={option.id}
                                            >
                                        {option.descripcion === 'UNIÓN LIBRE'? option.descripcion:`${option.descripcion} (A)`}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Consulta Social
                </Typography>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ocupacion"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    label="Ocupación *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {catalogState?.professions.map((option: any) => {
                                        if(option.descripcion !== 'Trabajador Independiente')
                                        {
                                            return (
                                                <MenuItem
                                                  key={option.id}
                                                  value={option.id}
                                                >
                                                  {String(option.descripcion).toUpperCase()}
                                                </MenuItem>
                                            )
                                        }
                                        return null;
                                       
                                    })}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nivelEducativo"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    label="Nivel educativo *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {listaNivelesAcademicos.map((option: any) => (
                                        <MenuItem
                                            key={option.id}
                                            value={option.idValor}
                                        >
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Condición de salud
                </Typography>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="afectionFisica"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Afectación Física (partes de cuerpo afectadas) *"
                                    value={value}
                                    onChange={onChange}
                                >
                                <MenuItem key={1} value='SI'>
                                    SI
                                </MenuItem>
                                <MenuItem key={2} value='NO'>
                                    NO
                                </MenuItem>
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="afectacionSensorial"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Afectacion Sensorial *"
                                    value={value}
                                    onChange={onChange}
                                >
                                   <MenuItem key={1} value='SI'>
                                        SI
                                    </MenuItem>
                                    <MenuItem key={2} value='NO'>
                                         NO
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="enfermedadEspecifica"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Enfermedad Específica *"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={1} value='SI'>
                                        SI
                                    </MenuItem>
                                    <MenuItem key={2} value='NO'>
                                         NO
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="tratamientoMedico"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Recibe tratamiento médico permanente *"
                                    value={value}
                                    onChange={onChange}
                                >
                                <MenuItem key={1} value='SI'>
                                        SI
                                </MenuItem>
                                <MenuItem key={2} value='NO'>
                                         NO
                                </MenuItem>
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="dificultadEscaleras"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Presenta dificultad para subir y bajar escaleras  *"
                                    value={value}
                                    onChange={onChange}
                                >
                                <MenuItem key={1} value='SI'>
                                        SI
                                </MenuItem>
                                <MenuItem key={2} value='NO'>
                                         NO
                                </MenuItem>
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="medicoPermanente"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Requiere medicamento permanente *"
                                    value={value}
                                    onChange={onChange}
                                >
                                <MenuItem key={1} value='SI'>
                                        SI
                                </MenuItem>
                                <MenuItem key={2} value='NO'>
                                         NO
                                </MenuItem>
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="discapacidad"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Diagnóstico *"
                                    value={value}
                                    onChange={onChange}
                                >
                               {listaTiposDiscapacidad.map(
                                    (option: any) => (
                                        <MenuItem
                                            key={option.codigo}
                                            value={option.codigo}
                                        >
                                            {option.descripcion}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="ayudaTecnica"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Ayuda técnica para la movilidad o ayuda sensorial *"
                                    value={value}
                                    onChange={ e => {
                                        onChange(e);
                                        if(e.target.value === "SI"){
                                            setMostrarListaAyudaTecnica(true);
                                        }else{
                                            setMostrarListaAyudaTecnica(false);
                                        }
                                    }}
                                >
                                <MenuItem key={1} value='SI'>
                                        SI
                                </MenuItem>
                                <MenuItem key={2} value='NO'>
                                         NO
                                </MenuItem>
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    { mostrarListaAyudaTecnica && (
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                            name="tipoAyudaTecnica"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly= {camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Ayuda técnica *"
                                    value={value}
                                    onChange={onChange}
                                >
                                {listaTiposAyudaTecnica.map((option: any) => {
                                        if (option.id !== 0) {
                                            return (
                                                <MenuItem
                                                    key={option.codigo}
                                                    value={option.codigo}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            );
                                        }
                                        return null;
                                    })}
                            </CHSelect>
                            )}
                            />
                        </Grid>
                    ) }
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="condicionSalud"
                            control={postulantesControl.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Condición de Salud *"
                                    value={value}
                                    onChange={onChange}
                                    multiline
                                    rows={5}
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
                <Grid container>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        {!camposEntrevistaSoloLectura && (
                            <Button
                              className={classes.buttonAdd}
                              variant="outlined"
                              style={{ marginRight: '10px' }}
                              onClick={postulantesControl.handleSubmit(
                                  agregarPostulantes,
                              )}
                            >
                              {estadoBoton}
                            </Button>
                        )}

                        {(datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION && 
                          datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE) && 
                         ((postulantesControl.watch('tipoIntegrante') === 'MAFP' || 
                           postulantesControl.watch('tipoIntegrante') === 'BP')) && (
                            <Button
                              className={classes.buttonAdd}
                              variant="outlined"
                              style={{ marginRight: '10px' }}
                              onClick={postulantesControl.handleSubmit(
                                  agregarPostulantes,
                              )}
                            >
                              {estadoBoton}
                            </Button>
                        )}

                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetPostulanteNuevo}
                        >
                            CANCELAR
                        </Button>
                    </Grid>
                </Grid>

                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Lista de postulantes
                </Typography>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Tipo Integrante</TableCell>
                            <TableCell>Tipo de Identificación</TableCell>
                            <TableCell>Número de Identificación</TableCell>
                            <TableCell>Nombres</TableCell>
                            <TableCell>Apellidos</TableCell>
                            <TableCell>Parentesco</TableCell>
                            <TableCell>Sexo</TableCell>
                            <TableCell>Fecha de Nacimiento</TableCell>
                            <TableCell>Estado Civil</TableCell>
                            <TableCell align="center">
                                Acciones
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {postulantesModel.map(
                            (option: Postulante) => (
                                <TableRow key={option.id}>
                                    <TableCell>
                                        {mostrarTipoIntegrante(option.tipoIntegranteId || '')}
                                    </TableCell>
                                    <TableCell>
                                        {mostrarTipoIdentificacion(option.tipoDocumentoId)}
                                    </TableCell>
                                    <TableCell>{option.numeroDocumento}</TableCell>
                                    <TableCell>
                                        {option.nombre}
                                    </TableCell>
                                    <TableCell>
                                        {option.apellido}
                                    </TableCell>
                                    <TableCell>
                                        {mostrarParentesco(option.tipoParentescoId || '')}
                                    </TableCell>
                                    <TableCell>
                                        {mostrarGenero(option.sexo || '')}
                                    </TableCell>
                                    <TableCell>
                                        {UtilService.getDate(option.fechaNacimiento) === '01-01-0001'? null : UtilService.getDate(option.fechaNacimiento)}
                                    </TableCell>
                                    <TableCell>
                                        {mostrarEstadoCivil(option.estadoCivil || '')}
                                    </TableCell>
                                    <TableCell align="center" style={{ width: '200px' }}>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            size="small"
                                            onClick={() => {
                                                seleccionarRegistro(
                                                    option,
                                                );
                                            }}
                                        >
                                        {camposEntrevistaSoloLectura === true? 
                                            <Visibility/> : <Edit />
                                        }
                                        </IconButton>
                                        {!camposEntrevistaSoloLectura && (
                                               <IconButton
                                               aria-label="Eliminar"
                                               color="primary"
                                               size="small"
                                               onClick={() => {
                                                   eliminarRegistro(
                                                       option.numeroDocumento,
                                                   );
                                               }}
                                           >
                                              <Delete />
                                            </IconButton>
                                        )}
                                    </TableCell>
                                </TableRow>
                            )
                        )}
                    </TableBody>
                </Table>
            </CHPaper>
        </>
    );
}

interface LinkMapStateToProps {
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    productoState: state.producto,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    datosSolicitudPostulacion: any;
    catalogState: CatalogState;
    datosEntrevista: any | null;
    cliente: any;
    listaTiposIntegrantes: any;
    listaTiposDiscapacidad: any;
    listaTiposAyudaTecnica: any;
    camposEntrevistaSoloLectura: boolean;
    postulantesControl: UseFormReturn<any>;
    setListaPostulantes: (lista: Postulante[]) => void;
    consultarTiposIntegrantes: () => void;
    consultarTiposDiscapcidad: () => void;
    consultarTiposAyudaTecnica: () => void;
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetAllProfessions: (claims:string) => void;
    startGetAllDepartments: (claims:string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetAllProfessions,
    startGetAllDepartments,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(IdentificacionPostulantesNucleoFamiliarPostulacion);