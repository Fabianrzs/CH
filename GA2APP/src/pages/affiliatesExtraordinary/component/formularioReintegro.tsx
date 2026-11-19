
import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Table, TableBody, TableCell, TableHead, TableRow, } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import moment from 'moment';
import { toast } from 'react-toastify';
import { Edit } from '@material-ui/icons';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHPaperNew from '../../../components/ch-paper/CHPaperV2';
import CHPageTitle2 from '../../../components/ch-page-title/CHPageTitleV2';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import { AfiliacionesExtraordinariasDTO, AfiliacionesExtraordinariasDTOInicial } from '../../../types/typeAffiliate.type';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { CAMPOCALIDADDTO, CATEGORIES, ESTADOAFILIACIONDTO, EXECUTINGUNITDTO, FORCEDTO, IDENTIFICATIONTYPEDTO, MODALIDADAFILIACIONVETERANODTO, PORCENTAJEDESCUENTODTO, TIPOAFILIACIONDTO, TIPOAFILIACIONPRODECENTEDTO } from '../../../types/catalog.type';
import { startGetAffiliateState, startGetAffiliateTypeProcedure, startGetAllExecutingUnits, startGetAllForces, startGetAllGrades, startGetCategory, startGetDiscountPercentage, startGetModalidadAfiliacionVeterano, startGetQualityField, startGetTypeAffiliate } from '../../../actions/catalog.action';
import { AffiliateExtraordinaryState } from '../../../reducers/affiliateExtraordinary.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import history from '../../../utils/history.router';
import { startPostUpdateExtraordinaryAffiliate } from '../../../actions/affiliatesExtraordinary.action';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ContainerTitle from '../../../components/containe-title/container-title';
import DetalleBeneficiarios from './detalleBeneficiarios';
import DetalleAportes from './detalleAportes';
import { AportesCategoriaAfiliaciones, DependienteCategoria } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import { TIPOS_AFILIACION_EXTRAORDINARIA } from '../../../constants/constants';
import ApiReportes from '../../../services/api-reportes.service';
import ApiTypeAffiliate from '../../../services/api-typeAffiliate.service';
import NotificationService from '../../../services/notification.service';
import CatalogApi from '../../../services/api-catalog.service';
import TramiteDesafiliaciones from './tramiteDesafiliaciones';



const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        button2: {
            width: '120px',
            marginLeft: '51rem',
            background: '#1c3b90',
            color: '#FFFFFF ',
            textAlign: 'center',
            borderRadius: '5%',
        },

        button3: {
            width: '120px',
            marginLeft: '1rem',
            background: '#42b7ef',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
        },

        icon: {
            padding: '10px',
            textAlign: 'inherit',
            color: '#1C3F8B',
        },

        title: {
            color: '#42b7ef',
            textAlign: 'center',
        },

        iconT: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#42b7ef',
            margin: '10px'
        },
        button6: {
            width: '20%',
            marginLeft: '80%',
            marginTop: '2%',
            background: '#1c3b90',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
        },
    }),
);

const apiAffiliate = ApiTypeAffiliate();
const apiReportes = ApiReportes();
const Oficios = [{
    id: 1, descripcion: 'TRANSFERENCIA CESANTÍAS'
},
{
    id: 2, descripcion: 'PONAL TRANSFERENCIA CESANTÍAS'
}]

const consignacionBancaria = [{
    id: 1, descripcion: 'CONSIGNACIÓN BANCARIA POR VENTANILLA'
},
{
    id: 2, descripcion: 'PAGO ELECTRÓNICO PSE'
}]

const apiCatalog = CatalogApi()

function formularioReintegro({
    catalogState,
    loginState,
    startGetAffiliateState,
    startGetQualityField,
    startGetAffiliateTypeProcedure,
    startGetAllForces,
    startGetCategory,
    startGetAllExecutingUnits,
    startGetTypeAffiliate,
    affiliateExtraordinaryState,
    startGetDiscountPercentage,
    startPostUpdateExtraordinaryAffiliate,
    startGetAllGrades,
    startGetModalidadAfiliacionVeterano
}:
    {
        catalogState: CatalogState;
        loginState: LoginState;
        startGetAffiliateState: () => void;
        startGetQualityField: () => void;
        startGetAffiliateTypeProcedure: () => void;
        startGetAllForces: (props: string) => void;
        startGetCategory: () => void;
        startGetAllExecutingUnits: (props: string) => void;
        startGetTypeAffiliate: () => void;
        affiliateExtraordinaryState: AffiliateExtraordinaryState;
        startGetDiscountPercentage: () => void;
        startPostUpdateExtraordinaryAffiliate: (data: AfiliacionesExtraordinariasDTO) => void;
        startGetAllGrades: (props: string) => void;
        startGetModalidadAfiliacionVeterano: () => void;
    }): any {
    const classes = useStyles();
    const { control, setValue, handleSubmit, watch } = useForm<AfiliacionesExtraordinariasDTO>({
        defaultValues: AfiliacionesExtraordinariasDTOInicial
    })

    const [loading, setLoading] = useState(false);
    const idTipofiliacionExtraordinaria = watch('idTipoAfiliacionExtraordinaria')

    const submitActualizarAfiliacion = async (data: AfiliacionesExtraordinariasDTO) => {
        try {
            setLoading(true);

            data.beneficiarios = beneficiarios as DependienteCategoria[];
            data.aportes = aportes as AportesCategoriaAfiliaciones[];
            data.tramitesDesafiliacion = listaDesafiliaciones;
            data.asignacionBasicaMensual = data.asignacionBasicaMensual.toString().includes('$')
                ? +data.asignacionBasicaMensual
                    .toLocaleString()
                    .replaceAll('$', '')
                    .replaceAll(',', '')
                : data.asignacionBasicaMensual;
            data.asignacionBasicaMensual = data.asignacionBasicaMensual != null ? data.asignacionBasicaMensual : 0;

            data.beneficiarios.forEach(element => {
                element.porcentajeReconocimientoPensionalDistribucionAportes =
                    +element.porcentajeReconocimientoPensionalDistribucionAportes.toString().replace('%', '');
                element.porcentajeReconocimientoPensionalDistribucionSubsidio =
                    +element.porcentajeReconocimientoPensionalDistribucionSubsidio.toString().replace('%', '');
                element.porcentajeReconocimientoPrestacionesSocialesDistribucionCesantias =
                    +element.porcentajeReconocimientoPrestacionesSocialesDistribucionCesantias.toString().replace('%', '');
            });

            const response = await apiAffiliate.PostUpdateExtraordinaryAffiliate(data);

            if (!response?.isSuccess) {
                toast('Error: respuesta errada del servidor');
            } else {
                await NotificationService.genericAlert(
                    'Guardado con éxito',
                    'Se realizó con éxito la inserción del registro.'
                );

                if (response.data.solicitudTrasladoCesantias || response.data.campoCesantias) {
                    const confirmAlert = await NotificationService.confirmAlert(
                        '¿Desea generar oficio de solicitud?'
                    );
                    if (confirmAlert && data.oficioSolicitudCesantias === 1) {
                        try {
                            const responseReporte = await apiReportes.oficioSolicitudCesantias(response?.data.idAfiliacion);
                            const mimeType = responseReporte.type;
                            if (mimeType) {
                                const file = new Blob([responseReporte], { type: mimeType });
                                const fileURL = URL.createObjectURL(file);
                                window.open(fileURL, '_blank');
                            }
                        } catch {
                            toast('Error al generar el oficio de respuesta', { type: toast.TYPE.ERROR });
                        }
                    } else if (confirmAlert && data.oficioSolicitudCesantias === 2) {
                        try {
                            const responseReporte = await apiReportes.oficioSolicitudCesantiasPONAL(response?.data.idAfiliacion);
                            const mimeType = responseReporte.type;
                            if (mimeType) {
                                const file = new Blob([responseReporte], { type: mimeType });
                                const fileURL = URL.createObjectURL(file);
                                window.open(fileURL, '_blank');
                            }
                        } catch {
                            toast('Error al generar el oficio de respuesta', { type: toast.TYPE.ERROR });
                        }
                    }
                }
                history.push('/afiliaciones');
            }
        } catch (error) {
            console.error('Error en submitActualizarAfiliacion:', error);
            toast('Error: algo salió mal', { type: toast.TYPE.ERROR });
        } finally {
            setLoading(false);
        }
    };


    useEffect(() => {
        startGetAffiliateState()
        startGetQualityField()
        startGetAffiliateTypeProcedure()
        startGetAllForces(loginState.claims)
        startGetAllExecutingUnits(loginState.claims)
        startGetTypeAffiliate()
        startGetCategory()
        startGetDiscountPercentage()
        startGetAllGrades(loginState.claims)
        startGetModalidadAfiliacionVeterano()
    }, [])

    const affiliate = affiliateExtraordinaryState.detalleAfiliacion;
    const [isOpenAportes, setIsOpenAportes] = useState<boolean>(false);
    const [isOpenBeneficiarios, setIsOpenBeneficiarios] = useState<boolean>(false);
    const [aportes, setAportes] = useState<AportesCategoriaAfiliaciones[]>([]);
    const [dataAportes, setDataAportes] = useState();
    const [dataBeneficiarions, setDataBeneficiarions] = useState<DependienteCategoria | null>();
    const [addBeneficiarions, setAddBeneficiarions] = useState(false);
    const [index, setIndex] = useState(0);
    const [hasLoadedFields, setHasLoadedFields] = useState(false);
    const [numeroCuotasCheck, setnumeroCuotasCheck] = useState(0);
    const [beneficiarios, setBeneficiarios] = useState<DependienteCategoria[]>();
    const [listaDesafiliaciones, setListaDesafiliaciones] = useState<any>();
    const [isOpenDesafiliaciones, setIsOpenDesafiliaciones] = useState<boolean>(false);

    const updateBeneficiarios = (data: any) => {
        setBeneficiarios(beneficiarios?.map(x => x.idDependiente === data.idDependiente ? data : x))
    }

    if (!hasLoadedFields && affiliate != null) {
        setHasLoadedFields(true);
        const fields = Object.keys(affiliate);
        fields.forEach(field => {
            const value = affiliate[field as keyof AfiliacionesExtraordinariasDTO];
            setValue(field as any, value);
        });
        setAportes(affiliate?.aportes)
        let sumatoriaNumeroCuotas = 0;
        affiliate?.aportes.forEach((aporte: AportesCategoriaAfiliaciones) => {
            sumatoriaNumeroCuotas += aporte.apC_CUOTAS_ACUMULADAS
        });
        setBeneficiarios(affiliate?.beneficiarios)
        setnumeroCuotasCheck(sumatoriaNumeroCuotas)

        affiliate?.tramitesDesafiliacion.forEach((element: any) => {
            apiCatalog.ObtenerTipoSolicitudDesafiliacion()
                .then((result: any) => {
                    element.descripcionSolicitud = result.data?.find((x: any) => x.idTipoSolicitud === element.idSolicitud)?.descripcionTipoSolicitud as string;
                    apiCatalog.getAllTypeModality(element.idSolicitud)
                        .then(res => {
                            element.descripcionModalidad = res.data?.find((x: any) => x.idTipoModalidad === element.idModalidad)?.descripcionTipoModalidad as string;
                        })
                })
        });
        setListaDesafiliaciones(affiliate?.tramitesDesafiliacion)
    }

    const validateFechaAlta = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (date > moment()) {
            message = "La fecha debe ser menor a la fecha actual"
        } else if (date > moment(watch('fechaPrimerAporte'))) {
            message = "La fecha de alta debe ser menor a la fecha de primer aporte"
        } else if (date > moment(watch('fechaUltimoAporte'))) {
            message = "La fecha de alta debe ser menor a la fecha de ultimo aporte"
        }
        return message !== '' ? message : true;
    }

    const validateFechaPrimerAporte = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (date > moment()) {
            message = "La fecha del primer aporte debe ser menor a la fecha actual"
        } else if (date > moment(watch('fechaPrimerAporte'))) {
            message = "La fecha del primer aporte debe ser menor a la fecha de primer aporte"
        } else if (date > moment(watch('fechaUltimoAporte'))) {
            message = "La fecha del primer aporte debe ser menor a la fecha de último aporte"
        }
        return message !== '' ? message : true;
    }

    const validateDatePeriodo = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (date > moment()) {
            message = "La fecha debe ser menor a la fecha actual"
        } else if (date > moment(watch('periodoAportesHasta'))) {
            message = "La fecha de período aportes desde debe ser mayor a fecha período aportes hasta"
        }

        return message !== '' ? message : true;
    }

    const validateDatePeriodoHasta = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (date > moment()) {
            message = "La fecha debe ser menor a la fecha actual"
        } else if (date < moment(watch('periodoAportesDesde'))) {
            message = "La fecha de período aportes desde debe ser mayor a fecha período aportes hasta"
        }

        return message !== '' ? message : true;
    }

    const validateDate = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (data > new Date()) {
            message = "La fecha debe ser menor a la fecha actual"
        }

        return message !== '' ? message : true;
    }

    const validateDateCreate = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (date > moment()) {
            message = "La fecha de elaboración debe ser menor a la fecha actual"
        }

        return message !== '' ? message : true;
    }

    const onClickItemAportes = (data: any) => (event: React.MouseEvent) => {
        setIsOpenAportes(true);
        setDataAportes(data)
        event.preventDefault();
    };

    const onCloseBeneficiarios = (value: AfiliacionesExtraordinariasDTO) => {
        setIsOpenBeneficiarios(false);
        console.log(value);
    };

    const onCloseAportes = () => {
        setIsOpenAportes(false);
    };

    const onClickItemBeneficiarios = (data: DependienteCategoria, indexBeneficiario: number) => (event: React.MouseEvent) => {
        setIsOpenBeneficiarios(true);
        setDataBeneficiarions(data);
        setIndex(indexBeneficiario + 1)
        event.preventDefault();
    };

    const openAddBeneficiario = () => {
        setIsOpenBeneficiarios(true);
        setDataBeneficiarions(null);
        setAddBeneficiarions(true)
    }

    const updateAportes = (data: any) => {
        setAportes(aportes?.map(x => x.ctG_ID === data.ctG_ID ? data : x))
        setIsOpenAportes(false)
        toast('Registro actualizado correctamente')
    }

    const onCloseDesafiliaciones = () => {
        setIsOpenDesafiliaciones(false)
    }

    const onSubmitDesafiliaciones = (data: any) => {
        data.valor = data.valor.replaceAll('$', '').replaceAll(',', '')
        setListaDesafiliaciones((listado: any) => [...listado, data])
        setIsOpenDesafiliaciones(false)
    }

    // Define los tipos de afiliación para los que deben mostrarse los campos de descuento
    const tiposAfiliacionConDescuento = [
        TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE,
        // TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS,
        TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE,
    ];

    // Condición de visibilidad: el checkbox está marcado Y el tipo de afiliación es el correcto
    const shouldShowDiscountFields = watch("activacionDescuento") && tiposAfiliacionConDescuento.includes(idTipofiliacionExtraordinaria);

    return (
        <CHPrivateContainer>
            {/* <LoadingComponent key="loader" loading={affiliateExtraordinaryState.loading} /> */}
            <CHPageTitle2
                topText='Gestión de afiliados'
                topText2='Afiliaciones Extraordinarias'
                title1="Afiliaciones"
                title2='Extraordinarias'
                icono='remember_me'
            />
            <CHPaperNew>
                <Grid container>
                    <LoadingComponent key="loader" loading={loading} />
                    {/* FORMULARIO AFILIACIONES PROCEDENTES */}
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="idTipoAfiliacionExtraordinaria"
                                rules={{
                                    validate: () => watch('idTipoAfiliacionExtraordinaria') < 1 || watch('idTipoAfiliacionExtraordinaria') == null ? 'El tipo de afiliación es obligatorio' : true
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        error={!!error}
                                        label="Tipo Afiliación Extraordinaria*"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}

                                    >
                                        {catalogState.tipoAfiliacion?.map(
                                            (option: TIPOAFILIACIONDTO) => (
                                                <MenuItem key={option.idTipoAfiliacion} value={option.idTipoAfiliacion}>
                                                    {option.descripcionTipoAfiliacion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="numeroRadicado"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Número Radicado*"
                                        value={value}
                                        onChange={onChange}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="fechaRadicado"
                                rules={{
                                    validate: validateDate
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePicker
                                        label="Fecha Radicado*"
                                        onChange={onChange}
                                        openTo="year"
                                        value={value}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="fechaElaboracion"
                                rules={{
                                    validate: validateDate
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePicker
                                        label="Fecha Elaboración*"
                                        onChange={onChange}
                                        value={value}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="tipoIdentificacion"
                                rules={{
                                    validate: () => watch('tipoIdentificacion') < 1 || watch('tipoIdentificacion') == null ? 'Este campo es obligatorio' : true
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        error={!!error}
                                        label="Tipo Identificación Afiliado*"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}

                                    >
                                        {catalogState.IdentificationType?.map(
                                            (option: IDENTIFICATIONTYPEDTO) => (
                                                <MenuItem key={option.id} value={option.id}>
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="cedula"
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido',
                                    },
                                    minLength: {
                                        value: 4,
                                        message:
                                            'La longitud mínima del campo es de 4 caracteres',
                                    },
                                    maxLength: {
                                        value: 12,
                                        message:
                                            'La longitud máxima del campo es de 12 caracteres',
                                    },
                                    min: {
                                        value: 1,
                                        message: 'No se aceptan valores negativos'
                                    },
                                    pattern: {
                                        value: /^[\d]*$/g,
                                        message: "No se permiten decimales"
                                    }
                                }}
                                control={control}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        type='number'
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Número De Identificación Afiliado*"
                                        onChange={onChange}
                                        value={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="cedula"
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido',
                                    },
                                    minLength: {
                                        value: 4,
                                        message:
                                            'La longitud mínima del campo es de 4 caracteres',
                                    },
                                    maxLength: {
                                        value: 12,
                                        message:
                                            'La longitud máxima del campo es de 12 caracteres',
                                    },
                                    min: {
                                        value: 1,
                                        message: 'No se aceptan valores negativos'
                                    },
                                    pattern: {
                                        value: /^[\d]*$/g,
                                        message: "No se permiten decimales"
                                    }
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Número Cédula Fallecido*"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="primerNombre"
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido.',
                                    },
                                    pattern: {
                                        value: /^[ A-zÀ-ú ']+$/g,
                                        message: 'El nombre no debe contener caracteres especiales'
                                    }
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Primer Nombre Fallecido*"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="segundoNombre"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Segundo Nombre Fallecido"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="primerApellido"
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido.',
                                    },
                                    pattern: {
                                        value: /^[ A-zÀ-ú ']+$/g,
                                        message: 'El nombre no debe contener caracteres especiales'
                                    }
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Primer Apellido Fallecido*"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="segundoApellido"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Segundo Apellido Fallecido"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="primerNombre"
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido.',
                                    },
                                    pattern: {
                                        value: /^[ A-zÀ-ú ']+$/g,
                                        message: 'El nombre no debe contener caracteres especiales'
                                    }
                                }}
                                control={control}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        isReadOnly
                                        label="Primer Nombre Afiliado*"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="segundoNombre"
                                rules={{
                                    pattern: {
                                        value: /^[ A-zÀ-ú ']+$/g,
                                        message: 'El nombre no debe contener caracteres especiales'
                                    }
                                }}
                                control={control}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Segundo Nombre Afiliado"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="primerApellido"
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido.',
                                    },
                                    pattern: {
                                        value: /^[ A-zÀ-ú ']+$/g,
                                        message: 'El nombre no debe contener caracteres especiales'
                                    }
                                }}
                                control={control}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Primer Apellido Afiliado*"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="segundoApellido"
                                rules={{
                                    pattern: {
                                        value: /^[ A-zÀ-ú ']+$/g,
                                        message: 'El nombre no debe contener caracteres especiales'
                                    }
                                }}
                                control={control}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Segundo Apellido Afiliado"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="fechaNacimiento"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        label="Fecha De Nacimiento"
                                        isReadOnly
                                        onChange={onChange}
                                        value={UtilService.getDate(value)}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE ?
                        (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <CHTextField
                                    isReadOnly
                                    label="Unidad Ejecutora"
                                    value={catalogState?.ExecutingUnit?.find(x => x.id === watch('idUnidadEjecutora'))?.nombre}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                />
                            </Grid>
                        </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE ?
                        (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <CHTextField
                                    isReadOnly
                                    label="Fuerza"
                                    value={catalogState?.Force?.find(x => x.id === watch('idFuerza'))?.descripcion}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                />
                            </Grid>
                        </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE ?
                        (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <CHTextField
                                    isReadOnly
                                    label="Grado"
                                    value={catalogState?.Grade?.find(x => x.id === watch('idGrado'))?.descripcion}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                />
                            </Grid>
                        </Grid>) : null}{idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE ?
                            (<Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <CHTextField
                                        isReadOnly
                                        label="Categoría"
                                        value={catalogState?.categories?.find(x => x.id === watch('idCategoria'))?.descripcion}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                    />
                                </Grid>
                            </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ? (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="idModalidadAfiliacionVeterano"
                                    rules={{
                                        validate: () => watch('idModalidadAfiliacionVeterano') < 1 || watch('idModalidadAfiliacionVeterano') == null ? 'Este campo es obligatorio' : true
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHSelect
                                            defaultValue=""
                                            label="Modalidad Afiliación Veterano*"
                                            error={!!error}
                                            onChange={onChange}
                                            value={value}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        >
                                            {catalogState.ModalidadAfiliacionVeterano?.map(
                                                (option: MODALIDADAFILIACIONVETERANODTO) => (
                                                    <MenuItem
                                                        key={option.idModalidad}
                                                        value={option.idModalidad}
                                                    >
                                                        {option.descripcionModalidad}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="campoCalidad"
                                    rules={{
                                        validate: () => watch('campoCalidad') < 1 || watch('campoCalidad') == null ? 'Este campo es obligatorio' : true
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHSelect
                                            defaultValue=""
                                            label="Calidad*"
                                            onChange={onChange}
                                            value={value}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        >
                                            {catalogState.campoCalidad?.map(
                                                (option: CAMPOCALIDADDTO) => (
                                                    <MenuItem
                                                        key={option.idCampoCalidad}
                                                        value={option.idCampoCalidad}
                                                    >
                                                        {option.descripcionCampoCalidad}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="idTipoAfiliacionProcedente"
                                    rules={{
                                        validate: () => watch('idTipoAfiliacionProcedente') < 1 || watch('idTipoAfiliacionProcedente') == null ? 'Este campo es obligatorio' : true
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHSelect
                                            defaultValue=""
                                            label="Tipo Afiliación*"
                                            onChange={onChange}
                                            value={value}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        >
                                            {catalogState.tipoAfiliacionProcedente?.map(
                                                (option: TIPOAFILIACIONPRODECENTEDTO) => (
                                                    <MenuItem
                                                        key={option.idTipoAfiliacionProcedente}
                                                        value={option.idTipoAfiliacionProcedente}
                                                    >
                                                        {option.descripcionTipoAfiliacionProcedente}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="inconsistencias"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="Inconsistencias*"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>


                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            {watch('inconsistencias') && (
                                <Controller
                                    name="observacionesInconsistencias"
                                    control={control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            label="Observaciones Inconsistencias*"
                                            onChange={onChange}
                                            value={value}
                                            multiline
                                            maxRows={5}
                                            inputProps={{
                                                style: {
                                                    textTransform: 'uppercase',
                                                },
                                            }}
                                        />
                                    )}
                                />
                            )}
                        </Grid>
                    </Grid>
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="tramiteAfiliacion"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHCheckbox
                                            label="¿Afiliado Tramitó Desafiliación?"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    {watch("tramiteAfiliacion") ? (
                        <Grid container style={{ margin: '40px', width: '95%' }}>
                            <CHPaperNew>
                                <Grid>
                                    <ContainerTitle title="Trámites De Desafiliaciones" />
                                </Grid>
                                <Grid>
                                    <Table className="client-table" style={{ margin: '20px', width: '95%' }}>
                                        <TableHead>
                                            <TableRow>
                                                <TableCell style={{ textAlign: 'center' }}>Tipo De Solicitud</TableCell>
                                                <TableCell style={{ textAlign: 'center' }}>Tipo De Submodalidad</TableCell>
                                                <TableCell style={{ textAlign: 'center' }}>Valor Desafiliación</TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {listaDesafiliaciones?.map((row: any, index: any) => (
                                                <TableRow key={`table${index}`}>
                                                    <TableCell>{row.descripcionSolicitud}</TableCell>
                                                    <TableCell>{row.descripcionModalidad}</TableCell>
                                                    <TableCell>{row.valor}</TableCell>
                                                </TableRow>
                                            ))}
                                        </TableBody>
                                    </Table>
                                    <Grid>
                                        <Button
                                            variant="contained"
                                            color="primary"
                                            onClick={() => setIsOpenDesafiliaciones(true)}
                                        >
                                            AGREGAR DESAFILIACIONES
                                        </Button>
                                    </Grid>
                                </Grid>
                            </CHPaperNew>
                        </Grid>
                    ) : null}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? (
                        <Grid container style={{ margin: '40px', width: '95%' }}>
                            <CHPaperNew>
                                <Grid>
                                    <ContainerTitle
                                        title="Beneficiarios"
                                    />
                                </Grid>
                                <Grid>
                                    <Table className="client-table" style={{ margin: '20px', width: '95%' }}>
                                        <TableHead>
                                            <TableRow>
                                                <TableCell style={{ textAlign: 'center' }}>Tipo De Identificación</TableCell>
                                                <TableCell style={{ textAlign: 'center' }}>Número De Identificación</TableCell>
                                                <TableCell style={{ textAlign: 'center' }}>Nombres y Apellidos Beneficiarios</TableCell>
                                                <TableCell style={{ textAlign: 'center' }}>Participación</TableCell>
                                                <TableCell style={{ textAlign: 'center' }}>Acciones</TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {beneficiarios?.map((row, index) => (
                                                <TableRow key={`table${index}`}>
                                                    <TableCell>{row.tipoIdentificacion}</TableCell>
                                                    <TableCell>{row.identificacion}</TableCell>
                                                    <TableCell>{row.nombre}</TableCell>
                                                    <TableCell>{row.participacion != null ? `${row.participacion}%` : `${row.porcentajeReconocimientoPensionalDistribucionAportes.toString().replaceAll('%', '')}%`}</TableCell>
                                                    <TableCell>
                                                        <IconButton aria-label="Editar" color="primary" onClick={onClickItemBeneficiarios(row, index)}>
                                                            <Edit />
                                                        </IconButton>
                                                    </TableCell>
                                                </TableRow>
                                            ))}
                                        </TableBody>
                                    </Table>
                                </Grid>
                            </CHPaperNew>
                        </Grid>
                    ) : null}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="fechaAlta"
                                    rules={{
                                        validate: validateFechaAlta
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Fecha De Alta"
                                            onChange={onChange}
                                            value={UtilService.getDate(value)}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="fechaPrimerAporte"
                                    rules={{
                                        validate: validateFechaPrimerAporte
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Fecha Primer Aporte"
                                            onChange={onChange}
                                            value={aportes.length > 0 ? UtilService.getDate(aportes[0].apC_FECHA_PRIMERA_CUOTA) : ''}

                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="fechaUltimoAporte"
                                    rules={{
                                        validate: validateFechaPrimerAporte
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHTextField
                                            isReadOnly
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Fecha Último Aporte"
                                            onChange={onChange}
                                            value={aportes.length > 0 ? UtilService.getDate(aportes[0].apC_FECHA_ULTIMA_COUTA) : ''}

                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <CHTextField
                                    label="Número De Cuotas"
                                    isReadOnly
                                    value={numeroCuotasCheck}
                                />
                            </Grid>
                        </Grid>)}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="conservaAntiguedad"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHCheckbox
                                            label="¿Conserva Antigüedad?"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    {watch("conservaAntiguedad") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <CHTextField
                                label="Número De Cuotas Asociadas"
                                isReadOnly
                                value={numeroCuotasCheck}
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="numeroActoAdministrativo"
                                    rules={{
                                        required: 'Este campo es requerido',
                                        min: {
                                            value: 1,
                                            message: 'No se aceptan valores negativos'
                                        }
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHTextField
                                            label="Número De Acto Administrativo*"
                                            onChange={onChange}
                                            error={!!error}
                                            value={value}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            numberMask
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="fechaNotificacion"
                                    rules={{
                                        validate: validateDate
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHDatePicker
                                            label="Fecha De Notificación / Ejecutora*"
                                            openTo="year"
                                            onChange={onChange}
                                            value={value}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="campoCesantias"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHCheckbox
                                            label="Cesantías"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    {/* {watch("campoCesantias") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="oficioSolicitudCesantias"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        label="Oficios De Solicitud"
                                        onChange={onChange}
                                        value={value}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    >
                                        {Oficios?.map(
                                            (option: any) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>) : null} */}
                    {idTipofiliacionExtraordinaria !== 19 ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="solicitudTrasladoCesantias"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="Solicitud Traslado Cesantías"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="activacionDescuento"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHCheckbox
                                            label="¿Activación De Descuento?"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    {shouldShowDiscountFields ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="mesDeInicioActivacionDescuento"

                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePicker
                                        views={['year', 'month']}
                                        openTo="year"
                                        label="Mes Activación"
                                        onChange={onChange}
                                        value={moment(value).endOf('month')}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        format="MM/yyyy"
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}

                    {watch('activacionDescuento') === true &&
                        (idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE) ?
                        (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="idPorcentajeDescuento"

                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHSelect
                                            defaultValue=""
                                            label="Porcentaje Descuento"
                                            onChange={onChange}
                                            value={value}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        >
                                            {catalogState.porcentajeDescuento?.map(
                                                (option: PORCENTAJEDESCUENTODTO) => (
                                                    <MenuItem
                                                        key={option.idPorcentajeDescuento}
                                                        value={option.idPorcentajeDescuento}
                                                    >
                                                        {option.descripcionPorcentajeDescuento}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="asignacionBasicaMensual"
                                    rules={{

                                        validate: () => +watch('asignacionBasicaMensual').toString().replace('$', '').replaceAll(',', '') <= 0 ? 'El valor de la asignación básica mensual debe ser mayor a 0' : true
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHTextField
                                            label="Valor Asignación Básica Mensual*"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                            inputProps={{ style: { textAlign: 'right' } }}
                                            value={value}
                                            currencyMask
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}

                    {idTipofiliacionExtraordinaria !== 17 ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="ahorroConsignacionBancaria"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="¿Realizar Ahorro Por Consignación Bancaria?"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {watch('ahorroConsignacionBancaria') && idTipofiliacionExtraordinaria === 17 ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="ahorroConsignacionBancariaOpcion"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        label="Opciones Consignación Bancaria"
                                        onChange={onChange}
                                        value={value}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    >
                                        {consignacionBancaria?.map(
                                            (option: any) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="fechaAlta"
                                rules={{
                                    validate: validateFechaAlta
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        label="Fecha De Alta Fallecido"
                                        isReadOnly
                                        onChange={onChange}
                                        value={UtilService.getDate(value)}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="documentacionAsociadaTramite"
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextField
                                            label="Documentación Asociada Al Trámite"
                                            onChange={onChange}
                                            isReadOnly
                                        />
                                    )}
                                />

                            </Grid>
                        </Grid>
                    )}
                    <Grid container md={12}>
                        <Grid className="item-containerModal" md={12}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="campoObservacionesConceptualizacion"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            label="Observaciones (Procedencia De La Afiliación)"
                                            onChange={onChange}
                                            value={value}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            multiline
                                            maxRows={5}
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
                    </Grid>

                    <DetalleBeneficiarios
                        isOpen={isOpenBeneficiarios}
                        onClose={onCloseBeneficiarios}
                        catalogState={catalogState}
                        data={dataBeneficiarions as DependienteCategoria}
                        setData={updateBeneficiarios}
                        numeroCuotasFallecido={numeroCuotasCheck}
                        calidad={+watch("campoCalidad")}
                        asignacionBasicaMensual={+watch("asignacionBasicaMensual")}
                        porcentaje={watch('campoCalidad') === 1 ? 0.08 : 0.045}
                        agregarBeneficiario={addBeneficiarions}
                        index={index}
                        porcentajeAcumulado={0}
                    />
                    <DetalleAportes
                        isOpen={isOpenAportes}
                        onClose={onCloseAportes}
                        data={dataAportes}
                        setData={updateAportes}
                        asignacionBasicaMensual={watch('asignacionBasicaMensual') != null ? +watch('asignacionBasicaMensual') : 0}
                        calidad={watch('campoCalidad')}
                        porcentaje={watch('campoCalidad') === 1 ? '8%' : '4.5%'}
                        idTipoAfiliacionExtraordinaria={watch('idTipoAfiliacionExtraordinaria')}
                    />
                    <Grid>
                        {/* <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            style={{ width: '200px' }}
                            onClick={handleSubmit(submitActualizarAfiliacion)}
                        >
                            PROCESAR
                        </Button> */}
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="default"
                            style={{ width: '200px' }}
                            onClick={() => history.push('/afiliaciones')}
                        >
                            REGRESAR
                        </Button>
                    </Grid>
                </Grid>
            </CHPaperNew>

            <TramiteDesafiliaciones
                isOpen={isOpenDesafiliaciones}
                onClose={onCloseDesafiliaciones}
                onSubmit={onSubmitDesafiliaciones}
            />
        </CHPrivateContainer >
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    affiliateExtraordinaryState: state.affiliateExtraordinary
})

const mapDispatchToProps = {
    startGetAffiliateState,
    startGetQualityField,
    startGetAffiliateTypeProcedure,
    startGetAllForces,
    startGetCategory,
    startGetAllExecutingUnits,
    startGetTypeAffiliate,
    startPostUpdateExtraordinaryAffiliate,
    startGetDiscountPercentage,
    startGetModalidadAfiliacionVeterano,
    startGetAllGrades
}


export default connect(mapStateToProps, mapDispatchToProps)(formularioReintegro);


