import React, { useEffect, useState, useMemo } from 'react';
import {
    Button, createStyles, Dialog, DialogTitle, Grid, IconButton, makeStyles, MenuItem,
    Table, TableBody, TableCell, TableHead, TableRow, TextField, Checkbox
} from '@material-ui/core';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
import { Edit } from '@material-ui/icons';
import { Controller, useForm, useWatch } from 'react-hook-form';
import moment from 'moment';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import AttachFileIcon from '@material-ui/icons/AttachFile';
import CHPaperNew from '../../../components/ch-paper/CHPaperV2';
import CHPageTitle2 from '../../../components/ch-page-title/CHPageTitleV2';
import UtilService from '../../../utils/util.service';
import ContainerTitle from '../../../components/containe-title/container-title';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import { AfiliacionesExtraordinariasDTO, AfiliacionesExtraordinariasDTOInicial } from '../../../types/typeAffiliate.type';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import {
    CAMPOCALIDADDTO, CATEGORIES, EXECUTINGUNITDTO, FORCEDTO, IDENTIFICATIONTYPEDTO,
    MODALIDADAFILIACIONVETERANODTO, PORCENTAJEDESCUENTODTO, TIPOAFILIACIONDTO,
    TIPOAFILIACIONPRODECENTEDTO, CITYDTO, DEPARTAMENTODTO, ADDRESSTYPE, GRADEDTO, FUERZADTO, PAISDTO, SEXODTO, CATEGORIADTO,
} from '../../../types/catalog.type';
import {
    startGetAffiliateState, startGetAffiliateTypeProcedure, startGetAllExecutingUnits,
    startGetAllForces, startGetAllGrades, startGetCategory, startGetDiscountPercentage,
    startGetQualityField, startGetTypeAffiliate
} from '../../../actions/catalog.action';
import { LoginState } from '../../../reducers/login.reducer';
import history from '../../../utils/history.router';
import { startCheckAffiliateExists, startPostNewExtraordinaryAffiliate } from '../../../actions/affiliatesExtraordinary.action';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { AffiliateExtraordinaryState } from '../../../reducers/affiliateExtraordinary.reducer';
import ApiClients from '../../../services/api-clients.service';
import CHPaper from '../../../components/ch-paper/CHPaper';
import DetalleBeneficiarios from './detalleBeneficiarios';
import TramiteDesafiliaciones from './tramiteDesafiliaciones';
import DetalleAportes from './detalleAportes';
import AportesCategoriaPorcliente from '../../AdministracionCuentas/components/aportes-por-categoria-cliente/aportes-por-categoria-cliente.component';
import {
    AporteCategoria, AportesCategoriaAfiliaciones, ClienteDto, Dependiente, DependienteCategoria
} from '../../../types/client.model.type';
import { BeneficiarioDto } from '../../../types/embargo.model.type';
import { TIPOS_AFILIACION_EXTRAORDINARIA } from '../../../constants/constants';
import ApiTypeAffiliate from '../../../services/api-typeAffiliate.service';
import ApiReportes from '../../../services/api-reportes.service';
import ApiCatalog from '../../../services/api-catalog.service'
import NotificationService from '../../../services/notification.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import { ModalArchivoRespuesta } from './ModalArchivoRespuesta';


const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        button2: {
            width: '12%',
            marginLeft: '71%',
            background: '#1c3b90',
            color: '#FFFFFF ',
            textAlign: 'center',
            borderRadius: '5%',
        },

        button3: {
            width: '12%',
            marginLeft: '2%',
            background: '#42b7ef',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
        },
        button4: {
            width: '16%',
            marginLeft: '67%',
            marginTop: '1%',
            background: '#1c3b90',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
        },
        button5: {
            width: '15%',
            marginLeft: '2%',
            marginTop: '1%',
            background: '#42b7ef',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
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

        icon: {
            padding: '10px',
            textAlign: 'inherit',
            color: '#1C3F8B',
        },
        iconT: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#42b7ef',
            margin: '10px'
        },

        title: {
            color: '#42b7ef',
            textAlign: 'center',
        }

    }),
);
const apiAffiliate = ApiTypeAffiliate();
const apiReportes = ApiReportes();
const apiCatalogo = ApiCatalog();


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

function InsertarAfiliacion({
    data,
    catalogState,
    loginState,
    clientModel,
    affiliateExtraordinaryState,
    startGetAffiliateState,
    startGetQualityField,
    startGetAffiliateTypeProcedure,
    startGetAllForces,
    startGetCategory,
    startGetAllExecutingUnits,
    startGetTypeAffiliate,
    startGetDiscountPercentage,
    startGetAllGrades,
    startPostNewExtraordinaryAffiliate,
    tipoDireccion,
    tipoTelefono,
    calidad,

}:
    {
        data: AfiliacionesExtraordinariasDTO;
        catalogState: CatalogState;
        loginState: LoginState;
        clientModel: ClienteDto;
        affiliateExtraordinaryState: AffiliateExtraordinaryState;
        startGetAffiliateState: () => void;
        startGetQualityField: () => void;
        startGetAffiliateTypeProcedure: () => void;
        startGetAllForces: (props: string) => void;
        startGetCategory: () => void;
        startGetAllExecutingUnits: (props: string) => void;
        startGetAllGrades: (props: string) => void;
        startGetTypeAffiliate: () => void;
        startGetDiscountPercentage: () => void;
        startPostNewExtraordinaryAffiliate: (data: AfiliacionesExtraordinariasDTO) => void;
        tipoDireccion: boolean;
        tipoTelefono: any;
        calidad: number;

    }): any {


    const classes = useStyles();
    const { control, handleSubmit, watch, setError, clearErrors, setValue, register, getValues, reset } = useForm<AfiliacionesExtraordinariasDTO>({
        defaultValues: AfiliacionesExtraordinariasDTOInicial
    })

    const idTipofiliacionExtraordinaria = watch('idTipoAfiliacionExtraordinaria');



    const [descripcionPorcentaje, setDescripcionPorcentaje] = useState('');
    const [producto, setProducto] = useState('');


    const insertDiscountPercentage = async (discountPercentage: any) => {
        try {
            const response = await apiCatalogo.postInsertDiscountPercentage(discountPercentage);
            if (response.isSuccess === true) {
                setDescripcionPorcentaje('');
                setProducto('');
                await startGetDiscountPercentage();
            }
        } catch (error: any) {
            const errorMessage = error.message || 'Error desconocido';
            toast(`No se guardó el nuevo porcentaje. Error: ${errorMessage}`);

        }
    };

    const handleGuardar = async () => {
        try {
            const discountPercentage = {
                descripcionPorcentajeDescuento: getValues('descripcionPorcentaje'),
                productoDescuento: getValues('producto'),
            };
            await insertDiscountPercentage(discountPercentage);
            handleCerrarModal();
            toast('Porcentaje añadido exitosamente', { type: toast.TYPE.SUCCESS });
        } catch (error: any) {
            const errorMessage = error.message || 'Error desconocido';
            toast(`No se guardó el nuevo porcentaje. Error: ${errorMessage}`);

        }

    };

    const handleAgregarPorcentaje = async () => {
        await handleGuardar();
    };

    const [isOpenAportes, setIsOpenAportes] = useState<boolean>(false);
    const [isOpenBeneficiarios, setIsOpenBeneficiarios] = useState<boolean>(false);
    const [isOpenDesafiliaciones, setIsOpenDesafiliaciones] = useState<boolean>(false);
    const [dataAportes, setDataAportes] = useState();
    const [dataBeneficiarions, setDataBeneficiarions] = useState<DependienteCategoria | null>();
    const [addBeneficiarions, setAddBeneficiarions] = useState(false);
    const [index, setIndex] = useState(0);
    const [listaDesafiliaciones, setListaDesafiliaciones] = useState<any>([]);
    const [unidadEjec, setUnidadEjecutora] = useState<any[]>([]);
    const identificationType = watch('tipoIdentificacion');
    const identificacionAfiliado = watch('cedula');
    const [linkDodo, setLinkDodo] = useState('')

    const [openModal, setOpenModal] = useState(false);
    const [nuevoPorcentaje, setNuevoPorcentaje] = useState('');
    const [productoPorcentaje, setProductoPorcentaje] = useState('');
    const [porcentajes, setPorcentajes] = useState<PORCENTAJEDESCUENTODTO[]>(catalogState.porcentajeDescuento);
    const [descuentosAplicados, setDescuentosAplicados] = useState<PORCENTAJEDESCUENTODTO[]>([]);

    useEffect(() => {
        obtenerUrlDodo();
    }, [watch('numeroRadicado')]);

    const handleAgregarNuevoPorcentaje = () => {
        setOpenModal(true);
    };

    const handleCerrarModal = () => {
        setOpenModal(false);
        setNuevoPorcentaje('');
        setProductoPorcentaje('');
    };

    const obtenerUrlDodo = async () => {

        const radicado = getValues('numeroRadicado');
        const enlaceCompleto = `http://cpvmp-112p:91/LinkFormulario/FormularioWM/Index?columna=Cod_Barras&valorColumna=${radicado}&idFormulario=20058`;
        setLinkDodo(enlaceCompleto);
    }


    const openModalAndAwait = async (setModalOpen: (open: boolean) => void, setResolver: (resolver: (value: boolean | null) => void) => void): Promise<void> => {
        await new Promise<boolean | null>((resolve) => {
            setResolver(() => resolve);
            setModalOpen(true);
        });
    };

    const submitActualizarAfiliacion = async (data: AfiliacionesExtraordinariasDTO) => {
        try {
            setLoading(true);

            // Procesamiento de datos
            data.beneficiarios = beneficiarios as DependienteCategoria[];
            data.aportes = aportes as AportesCategoriaAfiliaciones[];
            data.tramitesDesafiliacion = listaDesafiliaciones;
            data.tramitesDesafiliacion?.forEach(x => {
                x.valor = x.valor.replaceAll('$', '').replaceAll(',', '');
            });
            data.asignacionBasicaMensual = data.asignacionBasicaMensual.toString().includes('$')
                ? +data.asignacionBasicaMensual.toString().replaceAll('$', '').replaceAll(',', '')
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

            // Enviar datos al servidor
            const response = await apiAffiliate.PostNewExtraordinaryAffiliate(data);

            if (!response?.isSuccess) {
                toast('Error: respuesta errada del servidor');
            } else {
                await NotificationService.genericAlert(
                    'Guardado con éxito',
                    'Se realizó con éxito la inserción del registro.'
                );

                // Generar documentos según el tipo de solicitud
                const handleDocumentGeneration = async (apiMethod: any, afilId: string, type: string) => {
                    try {
                        const responseReporte = await apiMethod(afilId);
                        const mimeType = responseReporte.type;
                        if (mimeType) {
                            const file = new Blob([responseReporte], { type: mimeType });
                            const fileURL = URL.createObjectURL(file);
                            setDocumentoURL(fileURL);
                            await openModalAndAwait(setModalOpen, setResolver);
                        }
                    } catch {
                        toast(`Error al generar el oficio de ${type}`, { type: toast.TYPE.ERROR });
                    }
                };

                const afilId = response.data.idAfiliacion.toString();
                const procedures = [
                    { check: data.oficioSolicitudCesantias === 1, apiMethod: apiReportes.oficioSolicitudCesantias, type: 'oficio de solicitud' },
                    { check: data.oficioSolicitudCesantias === 2, apiMethod: apiReportes.oficioSolicitudCesantiasPONAL, type: 'oficio de solicitud' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaReintegroProcedente, type: 'Reintegro Procedente', extraParam: 12 },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaReincorporacionProcedente, type: 'Reincorporación Procedente' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaReconocimientoPensionProcedente, type: 'Reconocimiento de Pensión Procedente' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaNuevaVinculacionProcedente, type: 'Nueva Vinculación Laboral Procedente' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaAfiliacionVoluntariaProcedente, type: 'Afiliación Voluntaria Procedente' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaAfiliacionExtemporaneaProcedente, type: 'Afiliación Extemporánea Procedente' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaRecuperacionCalidadAfiliado, type: 'Recuperación Calidad de Afiliado' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaBeneficiarios, type: 'Beneficiarios' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS && response.data.idModalidadAfiliacionVeterano === 2, apiMethod: apiReportes.oficioRespuestaAfiliacionVoluntariaVeteranosCredito, type: 'Afiliación Voluntaria Veteranos (Crédito)' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS && response.data.idModalidadAfiliacionVeterano === 1 && response.data.activacionDescuento, apiMethod: apiReportes.oficioRespuestaAfiliacionVoluntariaVeteranosProcedenteAD, type: 'Afiliación Voluntaria Veteranos (Descuento)' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS && response.data.idModalidadAfiliacionVeterano === 1 && response.data.ahorroConsignacionBancaria, apiMethod: apiReportes.oficioRespuestaAfiliacionVoluntariaVeteranosProcedenteCB, type: 'Afiliación Voluntaria Veteranos (Consignación Bancaria)' },
                    { check: response.data.idTipoAfiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE, apiMethod: apiReportes.oficioRespuestaAfiliacionAdminCesantiasProcedente, type: 'Afiliación para Administración de Cesantías' }
                ];

                const procedurePromises = procedures
                    .filter(procedure => procedure.check)
                    .map(async procedure => {
                        const confirmAlert = await NotificationService.confirmAlert(procedure.alertMsg || '¿Desea generar el oficio de solicitud?');
                        if (confirmAlert) {
                            await handleDocumentGeneration(procedure.apiMethod.bind(null, afilId, procedure.extraParam), afilId, procedure.type);
                        }
                    });

                await Promise.all(procedurePromises);

                history.push('/afiliaciones');
            }
        } catch {
            toast('Error inesperado en el servidor', { type: toast.TYPE.ERROR });
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
        startGetAllGrades(loginState.claims)
        startGetDiscountPercentage()
    }, [])

    const [responseClient, setresponseClient] = useState<ClienteDto>();
    const [aportes, setAportes] = useState<AportesCategoriaAfiliaciones[]>([]);
    const [beneficiarios, setBeneficiarios] = useState<DependienteCategoria[]>();
    const [numeroCuotasCheck, setnumeroCuotasCheck] = useState(0);
    const [asignacionBasicaMensual, setAsignacionBasicaMensual] = useState(0);
    const [porcentajeAcumulado, setPorcentajeAcumulado] = useState(0);

    const api = ApiClients();
    const [loading, setLoading] = useState(false);
    const [selectedRows, setSelectedRows] = useState<number[]>([]);



    const tiposValidos = [
        "DESAFILIACION",
        "DESAFILIACION CI SOLUCION VIVIENDA SOLO APORTES (C",
        "DESAFILIACION PARCIAL DE CESANTIAS",
        "DESAFILIACION SOLO APORTES",
        "DESAFILIACION CON CESANTIAS",
        "DESAFILIACIONES",
    ];

    const solicitudesFiltradas = responseClient?.solicitudes?.filter(
        (solicitud) =>
            tiposValidos.includes(solicitud?.tipoSolicitud)
    ) || [];

    const datosCombinados = [
        ...(listaDesafiliaciones ?? []),
        ...(solicitudesFiltradas ?? []),
    ];

    const handleToggleRow = (index: number) => {
        setSelectedRows((prev) =>
            prev.includes(index)
                ? prev.filter((i) => i !== index)
                : [...prev, index]
        );
    };

    const obtenerValorNumerico = (item: any): number => {
        if (item?.valorPagado) {
            return Number(item.valorPagado) || 0;
        }
        if (item?.valor) {

            const limpio = item.valor.toString().replace(/[^0-9.-]+/g, "");
            return Number(limpio) || 0;
        }
        return 0;
    };


    const totalDesafiliacion = useMemo(
        () =>
            selectedRows.reduce((acc, idx) => {
                const item = datosCombinados[idx];
                return acc + obtenerValorNumerico(item);
            }, 0),
        [selectedRows, datosCombinados]
    );


    const DataValorDesafiliacion = useMemo(() =>
        responseClient?.solicitudes?.find(
            (solicitud: any) =>
                solicitud?.estadoSolicitud === "PROCESADA" &&
                tiposValidos.includes(solicitud?.tipoSolicitud)
        ), [responseClient]
    );

    const [afiliadoEncontrado, setAfiliadoEncontrado] = useState(false);


    const onBlurCedula = async (identification: string) => {
        setLoading(true);
        try {
            if (!identification) return;

            const idType = watch('tipoIdentificacion');
            const response = await api.getClientByIdentificationTypeIdentificationNumberRequest(
                idType,
                identification,
            );

            const idsPermitidos = [9, 10, 11, 12, 13, 14, 17, 18];
            const data = response?.data ?? null;


            if (!data) {
                setValue("primerNombre", "");
                setValue("segundoNombre", "");
                setValue("primerApellido", "");
                setValue("segundoApellido", "");
                setValue("fechaAlta", null);
                setValue("idFuerza", 0);
                setValue("idCategoria", 0);
                setValue("idUnidadEjecutora", 0);
                setValue("idGrado", 0);
                setValue("idCliente", 0);
                setValue("fechaNacimiento", "");
                setValue("fechaPrimerAporte", null);
                setValue("fechaUltimoAporte", null);
                setAportes([]);
                setnumeroCuotasCheck(0);

                if (idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE) {
                    toast("No es permitido. El documento ingresado no existe en el sistema.");
                    setError('cedula', { type: "manual", message: "El documento no existe en el sistema" });
                    setAfiliadoEncontrado(false);
                    return;
                }

                if (idsPermitidos.includes(idTipofiliacionExtraordinaria)) {
                    clearErrors("cedula");
                    return;
                }

                setError('cedula', { type: "focus", message: "El afiliado no fue encontrado" }, { shouldFocus: true });
                setAfiliadoEncontrado(false);
                return;
            }


            const {
                primerNombre = "",
                segundoNombre = "",
                primerApellido = "",
                segundoApellido = "",
                fechaAlta = null,
                idFuerza = 0,
                idCategoria = 0,
                idUnidadEjecutora = 0,
                idGrado = 0,
                idCliente = 0,
                fechaNacimiento = "",
                fechaPrimeraCuota = null,
                fechaUltimaCuota = null,
                ingresoMensual = 0,
                regimen = "",
                aportesCategoria: aportesCategoriaApi = [],
                dependientes: dependientesApi = [],
            } = data;

            setresponseClient(data);
            clearErrors("cedula");


            setValue("primerNombre", primerNombre);
            setValue("segundoNombre", segundoNombre);
            setValue("primerApellido", primerApellido);
            setValue("segundoApellido", segundoApellido);
            setValue("fechaAlta", fechaAlta);
            setValue("idFuerza", idFuerza);
            setValue("idCategoria", idCategoria);
            setValue("idUnidadEjecutora", idUnidadEjecutora);
            setValue("idGrado", idGrado);
            setValue("idCliente", idCliente);
            setValue("fechaNacimiento", fechaNacimiento);

            setValue("fechaAltaFallecido", fechaAlta);
            setAfiliadoEncontrado(true);


            if (aportesCategoriaApi.length > 0) {
                const [primerAporte] = aportesCategoriaApi;
                const primerAporteDate = new Date(primerAporte.apC_FECHA_PRIMERA_CUOTA);
                const ultimoAporteDate = new Date(primerAporte.apC_FECHA_ULTIMA_COUTA);

                setValue("fechaPrimerAporte", primerAporteDate);
                setValue("fechaUltimoAporte", ultimoAporteDate);
            } else {
                setValue("fechaPrimerAporte", null);
                setValue("fechaUltimoAporte", null);
            }

            setValue("fechaPrimerAporteFallecido", fechaPrimeraCuota ?? null);
            setValue("fechaUltimoAporteFallecido", fechaUltimaCuota ?? null);

            // 🧮 Construir aportes
            const aportesCategoria: AportesCategoriaAfiliaciones[] = [];
            let sumatoriaNumeroCuotas = 0;

            aportesCategoriaApi.forEach((element: AporteCategoria) => {
                const {
                    apC_CUOTAS_ACUMULADAS = 0,
                    apC_FECHA_ULTIMA_COUTA = null,
                    apC_FECHA_PRIMERA_CUOTA = null,
                    ctG_ID = 0,
                } = element;

                const aporte: AportesCategoriaAfiliaciones = {
                    apC_CUOTAS_ACUMULADAS,
                    apC_FECHA_ULTIMA_COUTA,
                    clI_ID: idCliente,
                    apC_FECHA_PRIMERA_CUOTA,
                    ctG_DESCRIPCION: catalogState?.categories?.find(x => x.id === ctG_ID)?.descripcion ?? "",
                    ctG_ID,
                    derechoLiquidacionCuotas: false,
                    liquidacionCuotaFondoSolidaridad: false,
                    periodoAportesDesde: new Date(),
                    periodoAportesHasta: new Date(),
                    ratificacionCuotas: 0,
                    reintegroAportes: false,
                    idAportesCategoriaAfiliaciones: 0,
                    valorDerechoLiquidacionCuotas: 0,
                    numeroCuotasLiquidacion: 0,
                    regimen,
                };

                aportesCategoria.push(aporte);
                sumatoriaNumeroCuotas += apC_CUOTAS_ACUMULADAS;
            });

            setnumeroCuotasCheck(sumatoriaNumeroCuotas);
            setAsignacionBasicaMensual(ingresoMensual);
            setAportes(aportesCategoria);


            const dependientesAportes: DependienteCategoria[] = [];
            let participacionPorcentaje = 0;

            dependientesApi.forEach((element: Dependiente) => {
                const {
                    idDependiente = 0,
                    idCliente = 0,
                    identificacion = "",
                    nombre = "",
                    tipoIdentificacion = 0,
                    participacion = 0,
                } = element;

                const parts = identificacion.split('-');
                const tipoIdentificacionDesc =
                    parts.length > 1
                        ? catalogState.IdentificationType.find(x => x.codigo === parts[0])?.descripcion
                        : catalogState.IdentificationType.find(x => x.id === +tipoIdentificacion)?.descripcion;

                const dependiente: DependienteCategoria = {
                    idDependiente,
                    idCliente,
                    identificacion: parts.length > 1 ? parts[1] : identificacion,
                    nombre,
                    tipoIdentificacion: tipoIdentificacionDesc ?? "",
                    participacion,
                    idTipoIdentificacion: catalogState.IdentificationType.find(x => x.codigo === parts[0])?.id ?? 0,
                    direccion: '',
                    correo: '',
                    telefono: '',
                    conservaAntiguedad: false,
                    numeroActoAdministrativoResolucionesReconocimientoPensional: 0,
                    fechaNotificacionResolucionesReconocimientoPensional: new Date(),
                    porcentajeReconocimientoPensionalDistribucionAportes: 0,
                    numeroActoAdministrativoResolucionesReconocimientoPrestacionesSociales: 0,
                    fechaNotificacionResolucionesReconocimientoPrestacionesSociales: new Date(),
                    porcentajeReconocimientoPrestacionesSocialesDistribucionCesantias: 0,
                    derechoLiquidacionCuotasBeneficiarios: false,
                    porcentajeReconocimientoPensionalDistribucionSubsidio: 0,
                    periodoAportesDesdeBeneficiarios: new Date(),
                    periodoAportesHastaBeneficiarios: new Date(),
                    valorDerechoLiquidacionCuotasBeneficiarios: 0,
                    numeroCuotasderechoLiquidacionCuotasBeneficiarios: 0,
                    index: 0,
                };

                participacionPorcentaje += participacion;
                dependientesAportes.push(dependiente);
            });

            setPorcentajeAcumulado(participacionPorcentaje);
            setBeneficiarios(dependientesAportes);

        } catch (error) {
            console.error("❌ Error en onBlurCedula:", error);
            setError(
                'cedula',
                { type: "focus", message: "Error al realizar la consulta. Intente nuevamente." },
                { shouldFocus: true }
            );
            setAfiliadoEncontrado(false);
        } finally {
            setLoading(false);
        }
    };




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

    const onClickItemBeneficiarios = (data: DependienteCategoria, indexBeneficiario: number) => (event: React.MouseEvent) => {
        setIsOpenBeneficiarios(true);
        setDataBeneficiarions(data);
        setAddBeneficiarions(false)
        setIndex(indexBeneficiario)
        event.preventDefault();
    };

    const openAddBeneficiario = () => {
        setIsOpenBeneficiarios(true);
        setDataBeneficiarions(null);
        setAddBeneficiarions(true)
    }

    const onClickItemAportes = (data: any) => (event: React.MouseEvent) => {
        setIsOpenAportes(true);
        setDataAportes(data)
        setAsignacionBasicaMensual(asignacionBasicaMensual)
        event.preventDefault();
    };

    const onCloseBeneficiarios = (value: AfiliacionesExtraordinariasDTO) => {
        setIsOpenBeneficiarios(false);
    };

    const onCloseAportes = () => {
        setIsOpenAportes(false);
    };

    const updateAportes = (data: any) => {
        setAportes(aportes?.map(x => x.ctG_ID === data.ctG_ID ? data : x))
        setIsOpenAportes(false)
        toast('Registro actualizado correctamente')
    }



    const updateBeneficiarios = (data: Partial<DependienteCategoria>) => {
        if (!data || !data.idTipoIdentificacion || !data.identificacion || !data.nombre) {
            toast.error('Datos incompletos, no se puede actualizar o agregar el beneficiario');
            return;
        }

        if (!addBeneficiarions && index !== null) {
            setBeneficiarios((beneficiarios) =>
                beneficiarios?.map((x, i) =>
                    i === index ? { ...x, ...data } as DependienteCategoria : x
                ) ?? []
            );
            toast.success('Registro actualizado correctamente');
        } else {
            const newData = {
                ...data,
                tipoIdentificacion:
                    catalogState?.IdentificationType?.find(
                        (x) => x.id === data.idTipoIdentificacion
                    )?.descripcion || '',
                participacion: data.participacion ?? 0,
                idCliente: watch('idCliente'),
                fechaNotificacionResolucionesReconocimientoPensional: watch(
                    'fechaNotificacionResolucionesReconocimientoPensional'
                ),
                fechaNotificacionResolucionesReconocimientoPrestacionesSociales: watch(
                    'fechaNotificacionResolucionesReconocimientoPrestacionesSociales'
                ),
            } as DependienteCategoria;

            setBeneficiarios((beneficiarios) => [...(beneficiarios ?? []), newData]);
            toast.success('Beneficiario agregado correctamente');
        }
    };



    const onCloseDesafiliaciones = () => {
        setIsOpenDesafiliaciones(false)
    }

    const onSubmitDesafiliaciones = (data: any) => {
        setListaDesafiliaciones((listado: any) => [...listado, data])
        setIsOpenDesafiliaciones(false)
    }

    const [modalOpen, setModalOpen] = useState(false);
    const [documentoURL, setDocumentoURL] = useState('');
    const [resolver, setResolver] = useState<((value: boolean | null) => void) | null>(null);

    const handleCloseModalRespuesta = async () => {
        setModalOpen(false);
        setDocumentoURL('');
        setLoading(false);
    };

    const onCloseModalRespuesta = async () => {
        setModalOpen(false);
        setDocumentoURL('');
        setLoading(false);
    };

    const shouldResetCheckbox = (value: any) => [
        TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE,
        TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE,
    ].includes(value);

    const isEditableNewDocument = () => {
        if (idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE ||
            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE ||
            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE ||
            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE) {
            return true;
        }

        return false;
    };

    const parseCurrency = (value: string | number): number => {
        if (typeof value === 'number') return value;
        return parseFloat(value.replace(/[^0-9.-]+/g, '')) || 0;
    };


    const asignacion = watch('asignacionBasicaMensual');
    const cuotas = watch('numeroCuotasLiquidacion');


    useEffect(() => {
        const asignacionNum = parseCurrency(asignacion);
        const cuotasNum = +cuotas || 0;

        if (asignacionNum > 0 && cuotasNum > 0) {
            const cuota = asignacionNum / cuotasNum;
            setValue('cuotaLiquidacionFondo', parseFloat(cuota.toFixed(2)));
        } else {
            setValue('cuotaLiquidacionFondo', 0);
        }
    }, [asignacion, cuotas]);

    const periodoDesde = watch('periodoAportesDesde');
    const periodoHasta = watch('periodoAportesHasta');

    useEffect(() => {
        if (periodoDesde && periodoHasta) {
            const cuotas = Math.round(moment(periodoHasta).diff(periodoDesde, 'months', true));
            setValue('numeroCuotasLiquidacion', cuotas);
        }
    }, [periodoDesde, periodoHasta]);


    const obtenerValor = (item: any): string => {
        if (typeof item.valorPagado === 'number') {
            return item.valorPagado.toLocaleString('es-CO', { style: 'currency', currency: 'COP' });
        }
        if (typeof item.valor === 'string') {
            return item.valor;
        }
        if (typeof item.valor === 'number') {
            return item.valor.toLocaleString('es-CO', { style: 'currency', currency: 'COP' });
        }
        return '-';
    };

    const [listaPorcentajes, setListaPorcentajes] = useState<PORCENTAJEDESCUENTODTO[]>([]);

    useEffect(() => {
        if (catalogState?.porcentajeDescuento) {
            setListaPorcentajes(catalogState.porcentajeDescuento);
        }
    }, [catalogState]);


    const handleEliminarPorcentaje = () => {
        const idSeleccionado = getValues("idPorcentajeDescuento");

        if (!idSeleccionado) {
            toast.error("Selecciona un porcentaje primero");
            return;
        }


        setListaPorcentajes((prev) =>
            prev.filter((p) => p.idPorcentajeDescuento !== idSeleccionado)
        );


        setValue("idPorcentajeDescuento", 0);
        toast.success("Porcentaje eliminado de la lista");
    };



    return (
        <CHPrivateContainer>
            <LoadingComponent key="loader" loading={affiliateExtraordinaryState.loading || loading} />
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
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="idTipoAfiliacionExtraordinaria"
                                rules={{
                                    validate: () => watch('idTipoAfiliacionExtraordinaria') < 1 || watch('idTipoAfiliacionExtraordinaria') == null ? 'El tipo de afiliación es requerido' : true
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
                                        onChange={(e) => {
                                            onChange(e);
                                            if (shouldResetCheckbox(e.target.value)) {
                                                setValue("campoCesantias", false);
                                            }
                                        }}

                                    >
                                        {catalogState.tipoAfiliacion?.map(
                                            (option: TIPOAFILIACIONDTO) => (
                                                <MenuItem
                                                    key={option.idTipoAfiliacion}
                                                    value={option.idTipoAfiliacion}>
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
                                    <div style={{ display: 'flex', alignItems: 'center' }}>
                                        <CHTextField
                                            error={!!error}
                                            label="Número Radicado*"
                                            value={value}
                                            onChange={onChange}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{
                                                style: {
                                                    textTransform: 'uppercase',
                                                },
                                            }}
                                        />
                                        <a href={linkDodo} target="_blank" rel="noopener noreferrer">
                                            <AttachFileIcon />
                                        </a>
                                    </div>

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
                                        label="Fecha Radicado"
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
                                    validate: validateDate,
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePicker
                                        label="Fecha Elaboración"
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
                                name="tipoIdentificacion"
                                rules={{
                                    validate: () => watch('tipoIdentificacion') < 1 || watch('tipoIdentificacion') == null ? 'Este campo es requerido' : true
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
                            {
                                identificationType === 9 ? (
                                    <Controller
                                        name="cedula"
                                        rules={{
                                            pattern: {
                                                value: /^[0-9]+$/g,
                                                message: 'Este campo es requerido'
                                            },
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
                                                message: 'Número de cédula inválido'
                                            },
                                        }}
                                        control={control}
                                        defaultValue=""
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                label="Número De Identificación Afiliado*"
                                                onChange={onChange}
                                                value={value}
                                                onBlur={() => {
                                                    onBlurCedula(identificacionAfiliado)
                                                }}
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}
                                            />
                                        )}
                                    />
                                ) : (
                                    <Controller
                                        name="cedula"
                                        rules={{
                                            pattern: {
                                                value: /^[0-9]+$/g,
                                                message: 'Este campo es requerido'
                                            },
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
                                                message: 'Número de cédula inválido'
                                            },
                                        }}
                                        control={control}
                                        defaultValue=""
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                label="Número De Identificación Afiliado*"
                                                onChange={onChange}
                                                value={value}
                                                onBlur={() => {
                                                    onBlurCedula(identificacionAfiliado)
                                                }}
                                                numberMask
                                            />
                                        )}
                                    />
                                )
                            }

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
                                        error={!!error}
                                        label="Número Identificación Fallecido*"
                                        onChange={onChange}
                                        value={value}
                                        numberMask
                                        onBlur={() => onBlurCedula(value)}
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
                                control={control}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        isReadOnly={!isEditableNewDocument()}
                                        label="Primer Nombre Afiliado"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
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
                                        isReadOnly={!isEditableNewDocument()}
                                        error={!!error}
                                        label="Segundo Nombre Afiliado"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="primerApellido"
                                control={control}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly={!isEditableNewDocument()}
                                        error={!!error}
                                        label="Primer Apellido Afiliado"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
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
                                        isReadOnly={!isEditableNewDocument()}
                                        error={!!error}
                                        label="Segundo Apellido Afiliado"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
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



                    {/* {
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                            ? null : (
                                <Grid className="item-containerModal" md={5}>
                                    <Grid className='subcontainerModal'>
                                        <Controller
                                            name="idGrado"
                                            control={control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error }
                                            }) => (
                                                <CHSelect
                                                    label="Grado"
                                                    value={watch('idGrado')}
                                                    onChange={onChange}
                                                >
                                                    {catalogState?.Grade?.map(unit => (
                                                        <MenuItem key={unit.id} value={unit.id}>
                                                            {unit.descripcion}
                                                        </MenuItem>
                                                    ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                </Grid>
                            )} */}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ? (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="idModalidadAfiliacionVeterano"

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

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ? (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="solicitudTrasladoCesantias"

                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHCheckbox
                                            label="Solicitud Traslado Cesantías"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>) : null}

                    {watch('solicitudTrasladoCesantias') &&
                        (idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE) && (
                            <Grid className='subcontainerModal'>
                                <Button
                                    className={classes.button}
                                    variant="contained"
                                    color="default"
                                    style={{ width: '200px' }}
                                // onClick={}
                                >
                                    GENERAR OFICIO
                                </Button>
                            </Grid>
                        )}


                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
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
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
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

                    {/* <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="inconsistencias"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="Inconsistencias"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid> */}


                    {/* <Grid className="item-containerModal" md={5}>
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
                    </Grid> */}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="tramiteAfiliacion"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHCheckbox
                                            label="¿Afiliado tramitó desafiliación?"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}

                    {
                        watch("tramiteAfiliacion") ? (
                            <Grid container style={{ margin: '40px', width: '95%' }}>
                                <CHPaperNew>
                                    <Grid>
                                        <ContainerTitle title="Trámites De Desafiliaciones" />
                                    </Grid>
                                    <Grid>
                                        <Table className="client-table" style={{ margin: '20px', width: '95%' }}>
                                            <TableHead>
                                                <TableRow>
                                                    <TableCell padding="checkbox">
                                                        <Checkbox disabled />
                                                    </TableCell>
                                                    <TableCell style={{ textAlign: 'center' }}>Tipo De Solicitud</TableCell>
                                                    <TableCell style={{ textAlign: 'center' }}>Tipo De Submodalidad</TableCell>
                                                    <TableCell style={{ textAlign: 'center' }}>Valor Desafiliación</TableCell>
                                                </TableRow>
                                            </TableHead>

                                            <TableBody>
                                                {datosCombinados.map((item: any, index: any) => (
                                                    <TableRow key={index}>
                                                        <TableCell padding="checkbox">
                                                            <Checkbox
                                                                checked={selectedRows.includes(index)}
                                                                onChange={() => handleToggleRow(index)}
                                                            />
                                                        </TableCell>
                                                        <TableCell style={{ textAlign: 'center' }}>
                                                            {item.descripcionSolicitud ?? item.tipoSolicitud ?? '-'}
                                                        </TableCell>
                                                        <TableCell style={{ textAlign: 'center' }}>
                                                            {item.descripcionModalidad ?? item.modalidad ?? '-'}
                                                        </TableCell>
                                                        <TableCell style={{ textAlign: 'center' }}>
                                                            {obtenerValorNumerico(item).toLocaleString("es-CO", {
                                                                style: "currency",
                                                                currency: "COP",
                                                            })}
                                                        </TableCell>

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
                        ) : null
                    }
                    {
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ? (
                            <Grid container style={{ margin: '40px', width: '95%' }}>
                                <CHPaperNew>
                                    <Grid>
                                        <ContainerTitle title="Beneficiarios" />
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
                                                        <TableCell>{row.participacion != null ? `${row.participacion}` : `${row.porcentajeReconocimientoPensionalDistribucionAportes.toString().replaceAll('%', '')}%`}</TableCell>
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
                                    <Grid>
                                        <Button
                                            variant="contained"
                                            color="primary"
                                            onClick={openAddBeneficiario}
                                            style={{ marginRight: '10px' }}
                                        >
                                            AGREGAR BENEFICIARIO
                                        </Button>
                                        {/* <Button
                                            variant="contained"
                                            color="default"
                                        >
                                            AGREGAR CUENTA
                                        </Button> */}
                                    </Grid>
                                </CHPaperNew>
                            </Grid>

                        ) : null
                    }
                    {/* <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="tramiteAfiliacion"
                                // rules={{
                                //     validate: () => watch('campoCalidad') < 1 || watch('campoCalidad') == null ? 'Este campo es obligatorio' : true
                                // }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        label="¿Afiliado tramitó desafiliación?"
                                        onChange={onChange}
                                        value={value}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    >
                                        <MenuItem value="SI">SÍ</MenuItem>
                                        <MenuItem value="NO">NO</MenuItem>
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid> */}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
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
                                        <CHDatePicker
                                            disabled={!isEditableNewDocument()}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Fecha De Alta"
                                            onChange={onChange}
                                            value={value}
                                            InputLabelProps={{ shrink: true }}
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
                                        <CHDatePicker
                                            disabled={!isEditableNewDocument()}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Fecha Primer Aporte"
                                            onChange={onChange}
                                            value={value}
                                            InputLabelProps={{ shrink: true }}
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
                                        <CHDatePicker
                                            disabled={!isEditableNewDocument()}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Fecha Último Aporte"
                                            onChange={onChange}
                                            value={value}
                                            InputLabelProps={{ shrink: true }}
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

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="conservaAntiguedad"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHCheckbox
                                            label="¿Conserva antigüedad?"
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

                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="ratificacionCuotas"
                                rules={{
                                    min: {
                                        value: 0,
                                        message: 'El valor de número de cuotas mínimo es 0'
                                    },
                                    max: {
                                        value: 168,
                                        message: 'El valor de número de cuotas máximo es 168'
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
                                        label="Ratificación De Cuotas"
                                        onChange={onChange}
                                        value={value === 0 ? null : value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        numberMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    {watch("ratificacionCuotas") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="observacionesRatCuotas"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        label="Observaciones (Ratificación cuotas)*"
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
                    </Grid>) : null}



                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="fechaAltaFallecido"
                                        rules={{
                                            validate: validateDate
                                        }}
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHDatePicker
                                                label="Fecha de Alta del Afiliado fallecido"
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
                        )}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="fechaPrimerAporteFallecido"
                                        rules={{
                                            validate: validateDate
                                        }}
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHDatePicker
                                                label="Fecha de primer aporte del Afiliado fallecido"
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
                        )}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="fechaUltimoAporteFallecido"
                                        rules={{
                                            validate: validateDate
                                        }}
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHDatePicker
                                                label="Fecha del último aporte del Afiliado fallecido"
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
                        )}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="numeroCuotasFallecido"

                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHTextField
                                                label="Número de cuotas del Afiliado fallecido"
                                                onChange={onChange}
                                                value={value === 0 ? null : value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                numberMask
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        )}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="resolucionesReconocimientoPensional"

                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHTextField
                                                label="Resoluciones de reconocimiento pensional"
                                                onChange={onChange}
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }

                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        )}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="porcentajeReconocimientoPensionalAportes"

                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHTextField
                                                label="Porcentaje reconocimiento pensional (Para distribución de aportes)"
                                                onChange={onChange}
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                percentMask

                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        )}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="porcentajeReconocimientoPensionalSubsidio"

                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHTextField
                                                label="Porcentaje reconocimiento pensional (Para distribución de subsidio)"
                                                onChange={onChange}
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                percentMask

                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        )}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="resolucionesReconocimientoPrestacionesSociales"

                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHTextField
                                                label="Resoluciones de reconocimiento prestaciones sociales"
                                                onChange={onChange}
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }

                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        )}
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE
                        ? null : (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="porcentajeReconocimientoPrestacionesSociales"

                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHTextField
                                                label="Porcentaje reconocimiento prestaciones sociales (Para distribución de cesantías)"
                                                onChange={onChange}
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                percentMask

                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        )}


                    {/* {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="observaciones"
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHTextField
                                            label="Observaciones*"
                                            onChange={onChange}
                                            error={!!error}
                                            value={value}
                                            multiline
                                            maxRows={5}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{
                                                style: {
                                                    textTransform: 'uppercase',
                                                },
                                            }}

                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)} */}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="numeroActoAdministrativo"
                                    rules={{
                                        required: 'Este campo es requerido',
                                        min: {
                                            value: 1,
                                            message: 'No se aceptan valores negativos'
                                        },

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
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
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
                                            label="Fecha De Notificación / Ejecutoria*"
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
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
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
                    {/* {watch("campoCesantias") || watch('solicitudTrasladoCesantias') && (idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        ) && (<Grid className="item-containerModal" md={5}>
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
                    </Grid>)} */}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="activacionDescuento"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHCheckbox
                                            label="¿Activación de descuento?"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}
                    {watch("activacionDescuento") ? (<Grid className="item-containerModal" md={5}>
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
                    {/* {
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                            ? null :
                            (<Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="idPorcentajeDescuento"
                                        rules={{
                                            required: {
                                                value: true,
                                                message: 'Este campo es requerido',
                                            }
                                        }}
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
                                <Grid item xs={12} container spacing={2}>
                                    <Grid item>
                                        <Button variant="contained" color="primary" onClick={handleAgregarNuevoPorcentaje}>
                                            Agregar Nuevo Porcentaje
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Grid>)} */}

                    {
                        // Condición para mostrar el componente completo
                        (
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        ) && watch("activacionDescuento")
                            ? (
                                <Grid className="item-containerModal" md={5}>
                                    <Grid className='subcontainerModal'>
                                        <Controller
                                            name="idPorcentajeDescuento"
                                            rules={{
                                                required: {
                                                    value: true,
                                                    message: 'Este campo es requerido',
                                                }
                                            }}
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
                                    <Grid item xs={12} container spacing={2}>
                                        <Grid item>
                                            <Button variant="contained" color="primary" onClick={handleAgregarNuevoPorcentaje}>
                                                Agregar Nuevo Porcentaje
                                            </Button>
                                        </Grid>
                                        <Grid item>
                                            <Button
                                                variant="outlined"
                                                color="secondary"
                                                onClick={handleEliminarPorcentaje}
                                            >
                                                Eliminar Porcentaje
                                            </Button>
                                        </Grid>
                                    </Grid>
                                </Grid>
                            ) : null
                    }

                    <Dialog open={openModal} onClose={handleCerrarModal}>
                        <DialogTitle>Agregar Nuevo Porcentaje</DialogTitle>
                        <DialogContent>
                            <Controller
                                name="descripcionPorcentaje"
                                control={control}
                                defaultValue=""
                                rules={{
                                    required: 'Este campo es requerido',
                                    pattern: {
                                        value: /^\d+%$/,
                                        message: 'Solo se permiten números seguidos de %'
                                    }
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <TextField
                                        label="Descripción del Porcentaje"
                                        type="text"
                                        fullWidth
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                    />
                                )}
                            />
                            <Controller
                                name="producto"
                                control={control}
                                defaultValue=""
                                rules={{
                                    required: 'Este campo es requerido',
                                    pattern: {
                                        value: /^\d+%$/,
                                        message: 'Solo se permiten números seguidos de %'
                                    }
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <TextField
                                        label="Producto"
                                        type="text"
                                        fullWidth
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                    />
                                )}
                            />
                        </DialogContent>
                        <DialogActions>
                            <Button onClick={handleCerrarModal} color="primary">
                                Cancelar
                            </Button>
                            <Button onClick={handleSubmit(handleAgregarPorcentaje)} color="primary">
                                Guardar
                            </Button>
                        </DialogActions>
                    </Dialog>
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        ? (
                            <Grid className="item-containerModal" md={5}>
                                <Grid className='subcontainerModal'>
                                    <Controller
                                        name="idFuerza"
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error }
                                        }) => (
                                            <CHSelect
                                                label="Fuerza*"
                                                value={watch('idFuerza')}
                                                onChange={onChange}
                                            >
                                                {catalogState?.Force?.map(unit => (
                                                    <MenuItem key={unit.id} value={unit.id}>
                                                        {unit.descripcion}
                                                    </MenuItem>
                                                ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        ) : null}

                    {
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                            idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_PROCEDENTE
                            ? (
                                <Grid className="item-containerModal" md={5}>
                                    <Grid className='subcontainerModal'>
                                        <Controller
                                            name="idUnidadEjecutora"
                                            rules={{
                                                required: 'Este campo es requerido',
                                            }}
                                            control={control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error }
                                            }) => (
                                                <CHSelect
                                                    label="Unidad Ejecutora*"
                                                    value={watch('idUnidadEjecutora')}
                                                    onChange={onChange}
                                                >
                                                    {catalogState?.ExecutingUnit?.map(unit => (
                                                        <MenuItem key={unit.id} value={unit.id}>
                                                            {unit.nombre}
                                                        </MenuItem>
                                                    ))}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                </Grid>
                            ) : null
                    }
                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE ? (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="idCategoria"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHSelect
                                            label="Categoría"
                                            value={watch('idCategoria')}
                                            onChange={onChange}
                                        >
                                            {catalogState?.categories?.map(unit => (
                                                <MenuItem key={unit.id} value={unit.id}>
                                                    {unit.descripcion}
                                                </MenuItem>
                                            ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        </Grid>
                    ) : null}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>

                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="liquidarCuotaFondo"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHCheckbox
                                            label="¿Liquidar cuota de fondo de solidaridad?"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    )}

                    {watch("liquidarCuotaFondo") ? (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="mesLiquidacionCuotaFondo"
                                    control={control}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHDatePicker
                                            views={['year', 'month']}
                                            openTo="year"
                                            label="Mes Liquidación"
                                            onChange={onChange}
                                            value={moment(value).endOf('month')}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            format="MM/yyyy"
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    ) : null}

                    {watch("liquidarCuotaFondo") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="cuotaLiquidacionFondo"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido.'
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                        label="Cuota Liquidación"
                                        isReadOnly
                                        value={value}
                                        currencyMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_PROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ||
                        idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_PROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="reintegroDeAportes"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHCheckbox
                                            label="¿Reintegro de aportes?"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>

                    )}

                    {watch("reintegroDeAportes") ? (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="valorDesafiliacion"
                                    control={control}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (

                                        <CHTextField
                                            label="Valor Desafiliación"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                            // value={DataValorDesafiliacion?.valorPagado || 0}
                                            value={totalDesafiliacion}
                                            inputProps={{ style: { textAlign: 'right' } }}
                                            currencyMask
                                            defaultValue=""
                                            isReadOnly
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    ) : null}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_PROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>

                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="periodoDeAportes"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHCheckbox
                                            label="Periodo de Aportes"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>

                    )}

                    {watch("periodoDeAportes") ? (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="periodoAportesDesde"
                                    control={control}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (

                                        <CHDatePicker
                                            label="Periodo de Aportes (Desde)"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{ style: { textAlign: 'right' } }}
                                            views={['year', 'month']}
                                            openTo="year"
                                            onChange={onChange}
                                            value={moment(value).endOf('month')}
                                            format="MM/yyyy"
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    ) : null}

                    {watch("periodoDeAportes") ? (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="periodoAportesHasta"
                                    control={control}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (

                                        <CHDatePicker
                                            label="Periodo de Aportes (Hasta)"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{ style: { textAlign: 'right' } }}
                                            views={['year', 'month']}
                                            openTo="year"
                                            onChange={onChange}
                                            value={moment(value).endOf('month')}
                                            format="MM/yyyy"
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    ) : null}

                    {idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINTEGRO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.REINCORPORACION_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECONOCIMIENTO_DE_PENSION_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.NUEVA_VINCULACION_LABORAL_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_EXTEMPORANEA_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.RECUPERACION_CALIDAD_AFILIADO_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_IMPROCEDENTE || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_VOLUNTARIA_VETERANOS_IMPROCEDENTE
                        || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.AFILIACION_PARA_ADMINISTRACION_DE_CESANTÍAS_IMPROCEDENTE ? null : (<Grid className="item-containerModal" md={5}>
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
                                            value={value === 0 ? null : value}
                                            inputProps={{ style: { textAlign: 'right' } }}
                                            currencyMask
                                            defaultValue=""

                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>)}

                    {(idTipofiliacionExtraordinaria === 1 || idTipofiliacionExtraordinaria === 5 || idTipofiliacionExtraordinaria === TIPOS_AFILIACION_EXTRAORDINARIA.BENEFICIARIOS_PROCEDENTE) && (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="liquidarCuota"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHCheckbox
                                            label="¿Tiene derecho a la liquidación de cuotas?"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    )}

                    {watch("liquidarCuota") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="numeroCuotasLiquidacion"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Número De Cuotas (Meses) A Liquidar"
                                        value={value}
                                        onChange={onChange}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {watch("liquidarCuota") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="mesesLiquidacion"
                                control={control}
                                render={({
                                    field: { onChange },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField

                                        error={!!error}
                                        label="Número De Meses A Liquidar"
                                        value={Math.round(moment(watch('periodoAportesHasta')).diff(watch('periodoAportesDesde'), 'months', true))}
                                        onChange={onChange}
                                        helperText={error ? error.message : null}

                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}

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
                                        isReadOnly
                                        label="Fecha De Alta Fallecido"
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
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            label="Observaciones (Procedencia De La Afiliación)*"
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
                        asignacionBasicaMensual={
                            +(watch('asignacionBasicaMensual') || '0')
                                .toString()
                                .replaceAll('$', '')
                                .replaceAll(',', '')
                        }
                        porcentaje={watch('campoCalidad') === 1 ? 0.08 : 0.045}
                        agregarBeneficiario={addBeneficiarions}
                        index={index}
                        porcentajeAcumulado={porcentajeAcumulado}
                    />
                    <DetalleAportes
                        isOpen={isOpenAportes}
                        onClose={onCloseAportes}
                        data={dataAportes}
                        setData={updateAportes}
                        asignacionBasicaMensual={
                            +(watch('asignacionBasicaMensual') || '0')
                                .toString()
                                .replaceAll('$', '')
                                .replaceAll(',', '')
                        }

                        calidad={watch('campoCalidad')}
                        porcentaje={watch('campoCalidad') === 1 ? '8%' : '4.5%'}
                        idTipoAfiliacionExtraordinaria={watch('idTipoAfiliacionExtraordinaria')}
                    />
                </Grid>
                <Grid>
                    <Button
                        className={classes.button}
                        variant="contained"
                        color="primary"
                        style={{ width: '200px' }}
                        onClick={handleSubmit(submitActualizarAfiliacion)}
                    >
                        PROCESAR
                    </Button>
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
            </CHPaperNew>



            <TramiteDesafiliaciones
                isOpen={isOpenDesafiliaciones}
                onClose={onCloseDesafiliaciones}
                onSubmit={onSubmitDesafiliaciones}
            />
            <ModalArchivoRespuesta
                open={modalOpen}
                documentoURL={documentoURL}
                handleClose={handleCloseModalRespuesta}
                onClose={onCloseModalRespuesta} s
                resolver={resolver}
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
    startPostNewExtraordinaryAffiliate,
    startCheckAffiliateExists,
    startGetDiscountPercentage,
    startGetAllGrades
}


export default connect(mapStateToProps, mapDispatchToProps)(InsertarAfiliacion);

