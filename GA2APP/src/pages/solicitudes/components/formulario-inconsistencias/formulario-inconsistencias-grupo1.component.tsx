import React, { useEffect, useRef, useState } from 'react';
import { Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller } from 'react-hook-form';
import { toast } from 'react-toastify';
import { Edit, Delete } from '@material-ui/icons';
import moment from 'moment';
import { AppState } from '../../../../store/rootReducer';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHSelect from '../../../../components/ch-select/CHSelect';
import {
    startGetAllGroupsInconsistencies,
    startGetAllPointsAttention,
    startGetAllToolsInconsistencies,
} from '../../../../actions/catalog.action';
import { startLoadUsers } from '../../../../actions/user.action';
import { CatalogState } from '../../../../reducers/catalog.reducer';
import { LoginState } from '../../../../reducers/login.reducer';
import { UserState } from '../../../../reducers/user.reducer';
import { FormularioInconsistenciasDto } from '../../../../models/requests/solicitud-tarea.model';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import SolicitudApi from '../../../../services/api-solicitud.service';
import NotificationService from '../../../../services/notification.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnLabel: {
            padding: '10px',
        },
        buttonSubmit: {
            marginLeft: '.5rem',
            background: '#1C3F8B',
            color: '#FFFFFF',
        },
    }),
);

function FormularioInconsistenciasGrupo1({
    catalogState,
    loginState,
    userState,
    control,
    idSolicitud,
    grupo1,
    entidadBloqueadaControlPreventivo,
    validandoBloqueoEntidad,
    handleSubmit,
    reset,
    setValue,
    getValues,
    watch,
    obtenerGrupos,
    obtenerHerramientas,
    obtenerPuntosAtencion,
    obtenerUsuarios,
    setEstadoSubsanar,
    listadoInconsistencias,
    addInconsistencia,
    updateInconsistencias,
    deleteInconsistencias,
    responseSolicitud,
    inconsistenciasSeleccionadas,
    deshablitarNinguna,
    desHabilitarNingunaGrupo1,
    setDeshabilitarNinguna,
    setCausalYNovedades,
    causalYNovedades,
    siEsAprobadaSolcitud,
    quitarTiposNovedades,
    setQuitarTiposNovedades,
    setTraerIdAntrior,
    caulsaSeleccionadas,
    setResetNovedad,
    resetNovedad,
    setTraerTodaData,
    estadoSubsanar,
    mostrarCheck,
    llamarNovedades,
    setLlamarNovedades,
    setMostrarCheck,
    deshativarBtnEliminarGp2,
    trazabilidad,
    refresh,
    setOmitirAlertaCausalTemporalmente = () => undefined,
    matriculaBloqueadaActiva
}: {
    catalogState: CatalogState;
    loginState: LoginState;
    userState: UserState;
    control: any;
    idSolicitud: string;
    grupo1: boolean;
    entidadBloqueadaControlPreventivo: boolean;
    validandoBloqueoEntidad: boolean;
    handleSubmit: any;
    reset: any
    setValue: (name: string, value: any) => void;
    getValues: any;
    watch: any;
    obtenerGrupos: () => void;
    obtenerHerramientas: () => void;
    obtenerPuntosAtencion: () => void;
    obtenerUsuarios: (token: string) => void;
    setEstadoSubsanar: (value: boolean) => void;
    listadoInconsistencias: FormularioInconsistenciasDto[];
    addInconsistencia: any;
    updateInconsistencias: any;
    deleteInconsistencias: any;
    responseSolicitud: any;
    inconsistenciasSeleccionadas: any;
    deshablitarNinguna: any;
    desHabilitarNingunaGrupo1: boolean;
    setDeshabilitarNinguna: (novedad: boolean) => void;
    setCausalYNovedades: (causalNovedades: boolean) => void;
    causalYNovedades: any;
    siEsAprobadaSolcitud: any;
    quitarTiposNovedades: any;
    setQuitarTiposNovedades: (valu: any) => void;
    setTraerIdAntrior: (idAnterior: any) => void;
    caulsaSeleccionadas: any;
    setResetNovedad: (reset: any) => void;
    resetNovedad: any;
    setTraerTodaData: (data: any) => void;
    estadoSubsanar: any;
    mostrarCheck: any;
    llamarNovedades: any
    setLlamarNovedades: any
    setMostrarCheck: any
    deshativarBtnEliminarGp2: any;
    trazabilidad: any[];
    refresh: boolean;
    setOmitirAlertaCausalTemporalmente: (
        value: boolean,
    ) => void;
    matriculaBloqueadaActiva: boolean;
}) {
    const classes = useStyles();
    const apiSolicitud = SolicitudApi();
    const [editButton, setEditButton] = useState(false);
    const [novedades, setNovedades] = useState<any[]>([])
    const [tiposNovedades, setTiposNovedades] = useState<any[]>([])
    const [tiposCausales, setTiposCausales] = useState<any[]>([])
    const [tiposPagosSucesivos, setTiposPagosSucesivos] = useState<any[]>([])
    const [tiposInformeInconsistencia, setTiposInformeInconsistencia] = useState<any[]>([])
    const [mostarListadoTipoNovedad, setMostarListadoTipoNovedad] = useState(false);
    const [mostarListadoCausales, setMostarListadoCausales] = useState(false);
    const [mostrarListadoTipoNovedad414, setMostrarListadoTipoNovedad414] = useState(false);
    const [fechaSubsanar, setFechaSubsanar] = useState(Date);
    const [ninguna, setNinguna] = useState(false);
    const [desHabilitarNingunaGr2, setdesHabilitarNingunaGr2] = useState(false);

    const [listadoInconsistenciasVista, setListadoInconsistenciasVista] = useState<FormularioInconsistenciasDto[]>([]);
    const alertaPagosSucesivosMostradaRef = useRef<string | null>(null);
    const fechasLocalesNovedadesRef = useRef<Record<string, string>>({});
    const alertaControlPreventivoRef =
        useRef<boolean>(false);

    const normalizarTexto = (valor: any) =>
        String(valor ?? '')
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .trim()
            .replace(/\s+/g, ' ')
            .toUpperCase();

    const normalizarValor = (valor: any) =>
        String(valor ?? '').trim();

    const esTareaSeguridadDocumentalV8 = (nombreTarea: any) => {
        const tarea = normalizarTexto(nombreTarea);

        return /^SEGURIDAD DOCUMENTAL GRUPO [12]\s*V\s*8$/.test(tarea);
    };

    const consultarPagosSucesivosAfiliado = async () => {
        const numeroRadicadoSG = normalizarValor(
            responseSolicitud?.numeroRadicadoSG,
        );

        const nombreTarea = responseSolicitud?.nombreTarea;


        if (!numeroRadicadoSG || !esTareaSeguridadDocumentalV8(nombreTarea)) {
            return;
        }


        const beneficiariosActuales = Array.isArray(
            responseSolicitud?.solicitudTerceroBeneficiarioDto,
        )
            ? responseSolicitud.solicitudTerceroBeneficiarioDto
            : [];

        const beneficiariosValidos = beneficiariosActuales
            .map((beneficiario: any) => {
                const tipoDocumento =
                    beneficiario?.tipoDoc ??
                    responseSolicitud?.tipoIdentificacion;

                const numeroDocumento = normalizarValor(
                    beneficiario?.numDoc,
                );

                return {
                    tipoDocumento,
                    numeroDocumento,
                    llave: `${tipoDocumento}-${numeroDocumento}`,
                };
            })
            .filter((beneficiario: any) =>
                Boolean(beneficiario?.numeroDocumento),
            )
            .filter(
                (beneficiario: any, index: number, arreglo: any[]) =>
                    arreglo.findIndex(
                        (item: any) => item?.llave === beneficiario?.llave,
                    ) === index,
            );

        if (beneficiariosValidos.length === 0) {
            return;
        }

        try {
            const resultados = await Promise.all(
                beneficiariosValidos.map(async (beneficiario: any) => {
                    const request = {
                        tipoDocumento: beneficiario?.tipoDocumento,
                        numeroDocumento: beneficiario?.numeroDocumento,
                    };

                    try {
                        const response =
                            await apiSolicitud.PostListaTramitesIntervinientes(
                                request,
                            );

                        const tramites = Array.isArray(response?.data)
                            ? response.data
                            : [];

                        const radicadosProcesadosUnicos = Array.from(
                            new Set(
                                tramites
                                    .filter(
                                        (tramite: any) =>
                                            normalizarTexto(
                                                tramite?.itA_NOMBRE,
                                            ) === 'BENEFICIARIO DE PAGO' &&
                                            normalizarTexto(
                                                tramite?.esS_DESCRIPCION,
                                            ) === 'PROCESADA',
                                    )
                                    .map((tramite: any) =>
                                        normalizarValor(
                                            tramite?.soL_RADICADO_GD,
                                        ),
                                    )
                                    .filter(Boolean),
                            ),
                        );

                        return {
                            success: true,
                            numeroDocumento: beneficiario?.numeroDocumento,
                            radicadosProcesadosUnicos,
                        };
                    } catch (error) {
                        console.error(
                            '[PAGOS SUCESIVOS] Error consultando el historial del beneficiario:',
                            {
                                numeroDocumento:
                                    beneficiario?.numeroDocumento,
                                error,
                            },
                        );

                        return {
                            success: false,
                            numeroDocumento: beneficiario?.numeroDocumento,
                            radicadosProcesadosUnicos: [],
                        };
                    }
                }),
            );

            const debeMostrarAlerta = resultados.some(
                (resultado: any) =>
                    resultado?.success &&
                    resultado?.radicadosProcesadosUnicos?.length >= 2,
            );


            const claveAlerta = `${numeroRadicadoSG}-${normalizarTexto(
                nombreTarea,
            )}`;

            if (
                debeMostrarAlerta &&
                alertaPagosSucesivosMostradaRef.current !== claveAlerta
            ) {
                alertaPagosSucesivosMostradaRef.current = claveAlerta;

                NotificationService.genericAlert(
                    'Alerta',
                    'Beneficiario de pago presenta pagos sucesivos.',
                    'warning',
                );
            }
        } catch (error) {
            console.error(
                '[PAGOS SUCESIVOS] Error general validando pagos sucesivos:',
                error,
            );
        }
    };

    const llaveBeneficiariosActuales = Array.isArray(
        responseSolicitud?.solicitudTerceroBeneficiarioDto,
    )
        ? responseSolicitud.solicitudTerceroBeneficiarioDto
            .map(
                (beneficiario: any) =>
                    `${beneficiario?.tipoDoc ?? ''}-${beneficiario?.numDoc ?? ''}`,
            )
            .join('|')
        : '';

    useEffect(() => {
        consultarPagosSucesivosAfiliado();
    }, [
        responseSolicitud?.numeroRadicadoSG,
        responseSolicitud?.nombreTarea,
        llaveBeneficiariosActuales,
        refresh,
    ]);

    useEffect(() => {
        const nombreTarea = normalizarTexto(
            responseSolicitud?.nombreTarea,
        );

        const estaEnGrupo2 = nombreTarea.includes(
            'SEGURIDAD DOCUMENTAL GRUPO 2',
        );

        if (estaEnGrupo2) {
            recargarInconsistenciasVista();
            return;
        }

        setListadoInconsistenciasVista(
            aplicarFechasLocales(
                Array.isArray(listadoInconsistencias)
                    ? listadoInconsistencias
                    : [],
            ),
        );
    }, [
        listadoInconsistencias,
        responseSolicitud?.nombreTarea,
        responseSolicitud?.solicitudId,
    ]);

    const recargarInconsistenciasVista = async () => {
        try {
            const request = responseSolicitud?.solicitudId || idSolicitud;

            if (!request) return;

            const result = await apiSolicitud.GetConsultarInconsistencia(request);


            let data: any[] = [];


            if (Array.isArray(result?.data)) {
                data = result.data;
            } else if (Array.isArray(result)) {
                data = result;
            }


            setListadoInconsistenciasVista(
                aplicarFechasLocales(data),
            );

            setListadoInconsistenciasVista(aplicarFechasLocales(data));
        } catch (error) {
            toast('Error cargando inconsistencias', { type: toast.TYPE.ERROR });
        }
    };

    useEffect(() => {
        obtenerGrupos();
        obtenerHerramientas();
        obtenerUsuarios(loginState.claims);
        obtenerPuntosAtencion();
    }, []);

    useEffect(() => {
        const nombreTarea = String(responseSolicitud?.nombreTarea || '');

        if (
            idSolicitud &&
            (
                nombreTarea.includes('Seguridad documental grupo 1') ||
                nombreTarea.includes('Seguridad documental grupo 2')
            )
        ) {
            recargarInconsistenciasVista();
        }
    }, [
        idSolicitud,
        responseSolicitud?.solicitudId,
        responseSolicitud?.nombreTarea,
        refresh
    ]);

    const nombreTareaActual = String(
        responseSolicitud?.nombreTarea || '',
    )
        .trim()
        .toLowerCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '');

    const esGrupo1 =
        nombreTareaActual.includes(
            'seguridad documental',
        ) &&
        nombreTareaActual.includes(
            'grupo 1',
        );

    const esGrupo2 =
        nombreTareaActual.includes(
            'seguridad documental',
        ) &&
        nombreTareaActual.includes(
            'grupo 2',
        );


    useEffect(() => {
        if (
            esGrupo2 &&
            entidadBloqueadaControlPreventivo &&
            !alertaControlPreventivoRef.current
        ) {
            alertaControlPreventivoRef.current = true;

            NotificationService.genericAlwarning(
                'Alerta',
                'Presenta bloqueo por control preventivo. Se requiere la revisión en el módulo de Entidades Educativas.',
                'warning',
            );

            return;
        }

        if (
            !entidadBloqueadaControlPreventivo
        ) {
            alertaControlPreventivoRef.current =
                false;
        }
    }, [
        esGrupo2,
        entidadBloqueadaControlPreventivo,
    ]);

    const bloqueoControlPreventivoActivo =
        esGrupo2 &&
        (
            entidadBloqueadaControlPreventivo ||
            validandoBloqueoEntidad
        );

    const listadoTabla = listadoInconsistenciasVista.filter((item: any) => {
        const grupoInconsistencia = Number(
            item?.grupoInconsistencia,
        );

        if (esGrupo1) {
            return grupoInconsistencia === 1;
        }

        if (esGrupo2) {
            return (
                grupoInconsistencia === 1 ||
                grupoInconsistencia === 2
            );
        }

        return true;
    });


    const normalizarId = (value: any) => String(value ?? '');

    const indexButton = Number(watch('index') || 0);

    const itemEditando = indexButton > 0 ? listadoTabla[indexButton - 1] : null;

    const mismoIdEnEdicion = (campo: string, id: any) =>
        itemEditando && normalizarId(itemEditando?.[campo]) === normalizarId(id);

    const normalizarNovedad = (valor: any) =>
        String(valor ?? '').trim().toUpperCase();

    const obtenerClaveOrdenNovedad = (
        item: any,
    ) => {
        const detalle =
            item?.listaInconsistenciaId ||
            item?.causalInterrupcionId ||
            item?.informe ||
            item?.pagoSucesivo ||
            '';

        return [
            Number(item?.grupoInconsistencia),
            normalizarNovedad(item?.novedad),
            normalizarId(detalle),
        ].join('|');
    };

    const obtenerGrupoActivoActual = () => {
        const nombreTareaActual = String(responseSolicitud?.nombreTarea || '');

        if (nombreTareaActual.includes('Seguridad documental grupo 1')) {
            return 1;
        }

        if (nombreTareaActual.includes('Seguridad documental grupo 2')) {
            return 2;
        }

        return null;
    };

    const grupoActivoActual = obtenerGrupoActivoActual();

    const listadoTablaOrdenado = [
        ...listadoTabla,
    ].sort((itemA: any, itemB: any) => {
        const grupoA = Number(
            itemA?.grupoInconsistencia,
        );

        const grupoB = Number(
            itemB?.grupoInconsistencia,
        );

        const prioridadGrupoA =
            grupoA === grupoActivoActual
                ? 0
                : 1;

        const prioridadGrupoB =
            grupoB === grupoActivoActual
                ? 0
                : 1;

        if (
            prioridadGrupoA !==
            prioridadGrupoB
        ) {
            return (
                prioridadGrupoA -
                prioridadGrupoB
            )
        }

        const claveA =
            obtenerClaveOrdenNovedad(itemA);

        const claveB =
            obtenerClaveOrdenNovedad(itemB);

        if (claveA !== claveB) {
            return claveA.localeCompare(claveB);
        }

        const subsanadoA =
            itemA?.subsanado === true ||
            String(itemA?.subsanado)
                .toLowerCase() === 'true';

        const subsanadoB =
            itemB?.subsanado === true ||
            String(itemB?.subsanado)
                .toLowerCase() === 'true';

        if (subsanadoA !== subsanadoB) {
            return subsanadoA ? -1 : 1;
        }

        return 0;
    });

    const existeNingunaGrupo1 = listadoInconsistenciasVista.some(
        (item: any) =>
            Number(item?.grupoInconsistencia) === 1 &&
            normalizarNovedad(item?.novedad) === 'NINGUNA',
    );

    const existeNingunaGrupoActual =
        listadoInconsistenciasVista.some(
            (item: any) =>
                Number(item?.grupoInconsistencia) ===
                grupoActivoActual &&
                normalizarNovedad(item?.novedad) ===
                'NINGUNA',
        );

    const estaEditandoLaNingunaGrupo1 =
        Number(itemEditando?.grupoInconsistencia) === 1 &&
        normalizarNovedad(itemEditando?.novedad) === 'NINGUNA';

    const bloquearNingunaGrupo1 =
        esGrupo1 &&
        existeNingunaGrupo1 &&
        !estaEditandoLaNingunaGrupo1;


    const idsTiposNovedadUsados = listadoTabla
        .filter((x: any) =>
            x.novedad === 'NOVEDAD SUBSANABLE' &&
            normalizarId(x.listaInconsistenciaId) !== ''
        )
        .map((x: any) => normalizarId(x.listaInconsistenciaId));

    const idsCausalesUsados = listadoTabla
        .filter((x: any) =>
            x.novedad === 'CAUSAL DE INTERRUPCIÓN' &&
            normalizarId(x.causalInterrupcionId) !== ''
        )
        .map((x: any) => normalizarId(x.causalInterrupcionId));

    const idsInformesUsados = listadoTabla
        .filter((x: any) =>
            x.novedad === 'INFORME 114 - INCONSISTENCIA' &&
            normalizarId(x.informe) !== ''
        )
        .map((x: any) => normalizarId(x.informe));

    const tipoNovedadYaUsado = (id: any) =>
        idsTiposNovedadUsados.includes(normalizarId(id)) &&
        !mismoIdEnEdicion('listaInconsistenciaId', id);

    const causalYaUsada = (id: any) =>
        idsCausalesUsados.includes(normalizarId(id)) &&
        !mismoIdEnEdicion('causalInterrupcionId', id);


    const informeYaUsado = (id: any) =>
        idsInformesUsados.includes(normalizarId(id)) &&
        !mismoIdEnEdicion('informe', id);


    const obtenerFechaActual = () =>
        moment().format('YYYY-MM-DDTHH:mm:ss');

    const normalizarLlaveFecha = (valor: any) =>
        String(valor ?? '')
            .trim()
            .toUpperCase();

    const normalizarSubsanadoFecha = (valor: any) =>
        valor === true || String(valor).toLowerCase() === 'true'
            ? 'SI'
            : 'NO';

    const esFechaVaciaODefecto = (fecha: any) => {
        if (!fecha) return true;

        const fechaTexto = String(fecha).trim();

        return (
            fechaTexto === '' ||
            fechaTexto.startsWith('0001-01-01') ||
            fechaTexto.startsWith('01/01/0001')
        );
    };

    const obtenerFechaValida = (...fechas: any[]) =>
        fechas.find((fecha: any) => !esFechaVaciaODefecto(fecha)) || '';

    const obtenerTipoDetalleNovedad = (item: any) =>
        normalizarLlaveFecha(
            item?.listaInconsistenciaId ||
            item?.causalInterrupcionId ||
            item?.informe ||
            item?.pagoSucesivo ||
            '',
        );

    const obtenerLlaveFechaNovedadExacta = (item: any) =>
        [
            item?.solicitudId || responseSolicitud?.solicitudId || idSolicitud,
            item?.grupoInconsistencia,
            item?.novedad,
            item?.listaInconsistenciaId,
            item?.causalInterrupcionId,
            item?.informe,
            item?.pagoSucesivo,
            normalizarSubsanadoFecha(item?.subsanado),
        ]
            .map(normalizarLlaveFecha)
            .join('|');

    const obtenerLlaveFechaNovedadFlexible = (item: any) =>
        [
            item?.solicitudId || responseSolicitud?.solicitudId || idSolicitud,
            item?.grupoInconsistencia,
            item?.novedad,
            obtenerTipoDetalleNovedad(item),
            normalizarSubsanadoFecha(item?.subsanado),
        ]
            .map(normalizarLlaveFecha)
            .join('|');

    const consultarInconsistenciasServidor = async () => {
        const request = responseSolicitud?.solicitudId || idSolicitud;

        if (!request) {
            return [];
        }

        const result = await apiSolicitud.GetConsultarInconsistencia(request);


        return Array.isArray(result)
            ? result
            : result?.data || [];
    };

    const obtenerDetalleNovedad = (item: any) =>
        normalizarId(
            item?.listaInconsistenciaId ||
            item?.causalInterrupcionId ||
            item?.informe ||
            item?.pagoSucesivo ||
            '',
        ).toUpperCase();

    const esRegistroRelacionadoParaBorrar = (item: any, referencia: any) => {
        const mismaSolicitud =
            normalizarId(item?.solicitudId || responseSolicitud?.solicitudId || idSolicitud) ===
            normalizarId(referencia?.solicitudId || responseSolicitud?.solicitudId || idSolicitud);

        const mismoGrupo =
            normalizarId(item?.grupoInconsistencia) ===
            normalizarId(referencia?.grupoInconsistencia);

        const mismaNovedad =
            normalizarNovedad(item?.novedad) ===
            normalizarNovedad(referencia?.novedad);

        const mismoDetalle =
            obtenerDetalleNovedad(item) === obtenerDetalleNovedad(referencia);

        return (
            mismaSolicitud &&
            mismoGrupo &&
            mismaNovedad &&
            mismoDetalle
        );
    };

    const obtenerRegistrosUnicosPorId = (registros: any[]) => {
        const ids = new Set<string>();

        return registros.filter((registro: any) => {
            const id = normalizarId(obtenerIdRegistro(registro));

            if (!id || ids.has(id)) {
                return false;
            }

            ids.add(id);
            return true;
        });
    };

    const existeRegistroSubsanadoEnLista = (lista: any[], registro: any) =>
        lista.some((item: any) =>
            obtenerLlaveFechaNovedadFlexible(item) ===
            obtenerLlaveFechaNovedadFlexible(registro),
        );

    const registrarFechaLocalNovedad = (item: any, fecha: string) => {
        const llaveExacta = obtenerLlaveFechaNovedadExacta(item);
        const llaveFlexible = obtenerLlaveFechaNovedadFlexible(item);

        fechasLocalesNovedadesRef.current[llaveExacta] = fecha;
        fechasLocalesNovedadesRef.current[llaveFlexible] = fecha;


    };

    const obtenerFechaLocalNovedad = (item: any) => {
        const llaveExacta = obtenerLlaveFechaNovedadExacta(item);
        const llaveFlexible = obtenerLlaveFechaNovedadFlexible(item);

        return (
            fechasLocalesNovedadesRef.current[llaveExacta] ||
            fechasLocalesNovedadesRef.current[llaveFlexible] ||
            ''
        );
    };

    const aplicarFechasLocales = (data: any[]) =>
        data.map((item: any) => {
            const fechaBackend = obtenerFechaValida(
                item?.fecha_Novedad,
                item?.fechaNovedad,
                item?.fechaCreacionNovedad,
                item?.fecha_Creacion,
                item?.fechaCreacion,
                item?.fecha_Subsanacion,
                item?.fechaRegistro
            );

            if (fechaBackend) {
                return item;
            }

            const fechaLocal = obtenerFechaLocalNovedad(item);


            return {
                ...item,
                fecha_Novedad: fechaLocal,
                fechaNovedad: fechaLocal,
                fechaCreacionNovedad: fechaLocal,
                fecha_Subsanacion: fechaLocal,
                fechaRegistro: fechaLocal,
            };
        });

    const obtenerFechaFila = (option: any) => {
        const fecha = obtenerFechaValida(
            option?.fecha_Novedad,
            option?.fechaNovedad,
            option?.fechaCreacionNovedad,
            option?.fecha_Creacion,
            option?.fechaCreacion,
            option?.fecha_Subsanacion,
            option?.fechaRegistro
        );

        if (!fecha) {
            return '';
        }

        const fechaMoment = moment(
            fecha,
            [
                moment.ISO_8601,
                'YYYY-MM-DDTHH:mm:ss',
                'YYYY-MM-DD HH:mm:ss',
                'DD/MM/YYYY HH:mm:ss',
                'DD/MM/YYYY:HH:mm',
            ],
            true,
        );

        return fechaMoment.isValid()
            ? fechaMoment.format('DD/MM/YYYY hh:mm:ss A')
            : String(fecha);
    };

    const obtenerIdRegistro = (item: any) =>
        item?.formularioInconsistenciaId ||
        item?.inconsistenciaId ||
        item?.id ||
        '';

    const crearIdVinculoNovedad = () =>
        `${responseSolicitud?.solicitudId || idSolicitud}-${grupoActivoActual || 'NA'}-${Date.now()}-${Math.random()
            .toString(36)
            .substring(2, 8)}`;

    const obtenerIdVinculoNovedad = (item: any) =>
        normalizarId(
            item?.grupoVinculoNovedadId ||
            item?.idGrupoNovedad ||
            item?.novedadVinculoId ||
            '',
        );

    const obtenerLlaveLogicaNovedad = (item: any) =>
        [
            item?.grupoInconsistencia,
            item?.novedad,
            item?.listaInconsistenciaId,
            item?.causalInterrupcionId,
            item?.informe,
            item?.pagoSucesivo,
        ].map(x => normalizarId(x)).join('|');




    const obtenerRegistrosVinculados = (option: any) =>
        listadoInconsistenciasVista.filter((item: any) =>
            esRegistroRelacionadoParaBorrar(item, option),
        );

    const estaSubsanado = (item: any) =>
        item?.subsanado === true ||
        String(item?.subsanado).toLowerCase() === 'true';

    const mostrarAccionesPorVinculo = (option: any) => {
        const registrosVinculados = obtenerRegistrosVinculados(option);

        const existeRegistroSubsanado = registrosVinculados.some((item: any) =>
            estaSubsanado(item),
        );

        // Si todavía no existe un SI, se muestran acciones en el NO.
        if (!existeRegistroSubsanado) {
            return true;
        }

        // Si ya existe SI y NO vinculados, solo se muestran acciones en el SI.
        return estaSubsanado(option);
    };

    const obtenerDetalleBorradoNovedad = (item: any) =>
        normalizarLlaveFecha(
            item?.listaInconsistenciaId ||
            item?.causalInterrupcionId ||
            item?.informe ||
            item?.pagoSucesivo ||
            '',
        );


    const camposEditablesNovedad = (data: any) => ({
        grupoInconsistencia: data?.grupoInconsistencia,
        novedad: data?.novedad,
        listaInconsistenciaId: data?.listaInconsistenciaId,
        causalInterrupcionId: data?.causalInterrupcionId,
        informe: data?.informe,
        pagoSucesivo: data?.pagoSucesivo,
    });


    useEffect(() => {
        setFechaSubsanar(moment(new Date()).format('DD/MM/YYYY:HH:mm'))
    }, [fechaSubsanar]);

    useEffect(() => {
        obtenerNovedades();

        if (listadoInconsistenciasVista.length === 0) return;

        if (listadoInconsistenciasVista.some((x: any) => x.novedad === 'CAUSAL DE INTERRUPCIÓN')) {
            tiposNovedadPorId('CAUSAL DE INTERRUPCIÓN');
        }

        if (listadoInconsistenciasVista.some((x: any) => x.novedad === 'NOVEDAD SUBSANABLE')) {
            tiposNovedadPorId('NOVEDAD SUBSANABLE');
        }

        if (listadoInconsistenciasVista.some((x: any) => x.novedad === 'INFORME 114 - INCONSISTENCIA')) {
            tiposNovedadPorId('INFORME 114 - INCONSISTENCIA');
        }
    }, [
        causalYNovedades,
        deshablitarNinguna,
        listadoInconsistenciasVista,
        responseSolicitud?.nombreTarea
    ]);

    useEffect(() => {
        setMostarListadoCausales(false)
        setMostarListadoTipoNovedad(false)

        setMostrarListadoTipoNovedad414(false)
        setResetNovedad(false)

    }, [resetNovedad])

    useEffect(() => {
        setEditButton(indexButton > 0);
    }, [indexButton]);


    const obtenerNovedades = async () => {

        try {
            const response = await apiSolicitud.GetObtenerNovedades();
            if (response) {
                const { data } = response;
                const nombreTarea = String(responseSolicitud?.nombreTarea || '').trim();

                const encontrarNovedadGrupo1 = listadoInconsistenciasVista.some((x: any) => x.grupoInconsistencia === 1);
                const encontrarNovedadGrupo2 = listadoInconsistenciasVista.some((x: any) => x.grupoInconsistencia === 2);

                if (nombreTarea.includes('Seguridad documental grupo 1 ')) {
                    if (encontrarNovedadGrupo1 && !encontrarNovedadGrupo2) {
                        let contador = 0;
                        let contadorNinguna = 0;
                        listadoInconsistenciasVista.forEach((element: any) => {

                            if (element.novedad !== 'NINGUNA' && element.grupoInconsistencia === 1) {
                                contador += 1;

                            }
                            if (element.novedad === 'NINGUNA') {
                                contadorNinguna += 1;
                            }

                        });
                        if (contador === listadoInconsistencias.length) {
                            setdesHabilitarNingunaGr2(true)
                        }
                        if (contadorNinguna === listadoInconsistencias.length) {
                            setNinguna(true)
                            setDeshabilitarNinguna(true)
                        }


                    } else {
                        setdesHabilitarNingunaGr2(false)
                        setNinguna(false)
                        setDeshabilitarNinguna(false)
                    }

                } else if (nombreTarea.includes('Seguridad documental grupo 2')) {
                    if (encontrarNovedadGrupo2) {
                        let contador = 0;
                        let cantidadItenGrupo2 = 0;
                        listadoInconsistenciasVista.forEach((element: any) => {
                            if (element.grupoInconsistencia === 2) {
                                cantidadItenGrupo2 += 1
                                if (element.novedad !== 'NINGUNA') {
                                    contador += 1;
                                    setNinguna(false)
                                } else {
                                    setNinguna(true)
                                }
                            }

                        });
                        if (contador === cantidadItenGrupo2) {
                            setdesHabilitarNingunaGr2(true)
                            setNinguna(false)
                        }

                    } else {
                        setdesHabilitarNingunaGr2(false)
                        setNinguna(false)
                    }
                }
                setMostarListadoCausales(false)
                setMostarListadoTipoNovedad(false)

                // setMostrarListadoTipoNovedad414(false)

                let filteredNovedades;

                if (nombreTarea.includes('Seguridad documental grupo 1 ')) {
                    filteredNovedades = data.filter((lista: any) => lista.noV_GRUPO === 1);
                } else if (nombreTarea.includes('Seguridad documental grupo 2 ')) {
                    filteredNovedades = data.filter((lista: any) => lista.noV_GRUPO === 2 || lista.noV_GRUPO === 1);
                } else {
                    filteredNovedades = data;
                }
                setNovedades([]);
                setNovedades(filteredNovedades);
            }
        } catch (error) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        }
    };

    const tiposNovedadPorId = async (idNovedad: any) => {

        try {

            if (idNovedad === "NOVEDAD SUBSANABLE") {
                const response = await apiSolicitud.PostObtenerTiposNovedadPorId(idNovedad);

                if (response) {
                    const nuevosTiposNovedades = response.data;

                    const { nombreTarea } = responseSolicitud || {};
                    let filteridNovedades;
                    if (nombreTarea.includes('Seguridad documental grupo 1')) {
                        filteridNovedades = nuevosTiposNovedades.filter((lista: any) => lista.tpO_NOV_GRUPO === 0 || lista.tpO_NOV_GRUPO === 1);
                    } else if (nombreTarea.includes('Seguridad documental grupo 2')) {
                        filteridNovedades = nuevosTiposNovedades.filter((lista: any) => lista.tpO_NOV_GRUPO === 0 || lista.tpO_NOV_GRUPO === 2 || lista.tpO_NOV_GRUPO === 1);
                    }
                    setTiposNovedades(filteridNovedades)

                }

            } else if (idNovedad === "CAUSAL DE INTERRUPCIÓN") {
                const response = await apiSolicitud.PostObtenerTiposCausalId(idNovedad);

                if (response) {
                    const nuevosTiposNovedades = response.data;
                    setTiposCausales(nuevosTiposNovedades)
                }
            } else if (idNovedad === "PAGOS SUCESIVOS") {
                const response = await apiSolicitud.PostObtenerTiposPagosSucesivos(idNovedad);

                if (response) {
                    const nuevosTiposNovedades = response.data;
                    setTiposPagosSucesivos(nuevosTiposNovedades)
                }
            } else if (idNovedad === "INFORME 114 - INCONSISTENCIA") {

                const response = await apiSolicitud.PostObtenerTiposInformeInconsistenciasSolicitud(idNovedad);

                if (response) {
                    const nuevosTiposNovedades = response.data;
                    setTiposInformeInconsistencia(nuevosTiposNovedades)
                }
            }
        } catch (error) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        }

    };

    const loadEditInconsistencia = (data: any, index: any) => {

        setNinguna(false)
        setMostrarCheck(true)
        setTraerTodaData(data)
        if (data.listaInconsistenciaId !== '' && data.novedad === 'NOVEDAD SUBSANABLE') {
            setTraerIdAntrior(data.listaInconsistenciaId)
            setNinguna(true)
        } else if (data.causalInterrupcionId !== '' && data.novedad === 'CAUSAL DE INTERRUPCIÓN') {
            setTraerIdAntrior(data.causalInterrupcionId)
            setNinguna(true)
        } else if (data.informe !== '' && data.novedad === 'INFORME 114 - INCONSISTENCIA') {
            setTraerIdAntrior(data.informe)
            setNinguna(true)
        }

        if (data.causalInterrupcionId !== null && data.causalInterrupcionId !== '') {
            setMostarListadoCausales(true)
            setMostarListadoTipoNovedad(false)
            setMostrarListadoTipoNovedad414(false)

        } else if (data.pagoSucesivo !== null && data.pagoSucesivo !== undefined && data.pagoSucesivo !== 0 && data.pagoSucesivo !== '') {
            setMostarListadoCausales(false)
            setMostarListadoTipoNovedad(false)
            setMostrarListadoTipoNovedad414(false)

        } else if (data.informe !== null && data.informe !== 0 && data.informe !== undefined && data.informe !== '') {
            setMostarListadoCausales(false)
            setMostarListadoTipoNovedad(false)
            setMostrarListadoTipoNovedad414(true)
        } else if (data.listaInconsistenciaId !== null && data.listaInconsistenciaId !== 0 && data.listaInconsistenciaId !== undefined && data.listaInconsistenciaId !== '') {
            setMostarListadoCausales(false)
            setMostarListadoTipoNovedad(true)
            setMostrarListadoTipoNovedad414(false)
        }
        if (data.novedad === "NINGUNA") {
            setDeshabilitarNinguna(false)
            setMostarListadoCausales(false)
            setMostarListadoTipoNovedad(false)
            setNinguna(true)
            setMostrarListadoTipoNovedad414(false)
            setMostrarCheck(false)
        }
        if (data.novedad === "INFORME 114 - INCONSISTENCIA") {
            setDeshabilitarNinguna(false)
            setMostarListadoCausales(false)
            setMostarListadoTipoNovedad(false)
            setNinguna(false)
            setMostrarListadoTipoNovedad414(true)
            setMostrarCheck(false)
        }
        if (data.novedad === "ANALISIS DESBLOQUEO MATRICULA") {
            setDeshabilitarNinguna(false)
            setMostarListadoCausales(false)
            setMostarListadoTipoNovedad(false)

            setMostrarListadoTipoNovedad414(false)
        }
        if (data.novedad === "Seleccione") {
            setDeshabilitarNinguna(false)
            setMostarListadoCausales(false)
            setMostarListadoTipoNovedad(false)
            setMostrarListadoTipoNovedad414(false)
        }

        const fields = Object.keys(data as FormularioInconsistenciasDto);
        fields.forEach(field => {
            const value = data[field as keyof FormularioInconsistenciasDto];
            setValue(field as any, value);
        })

        setValue('index', index + 1)
    }

    // const novedadCausalInterrupcion = (descripcionOpcion: string) => {

    //     let causalInterrupcion = false;
    //     if (listadoInconsistencias && listadoInconsistencias.length > 0) {
    //         if (listadoInconsistencias.some((option: any) => String(option?.novedad) === 'CAUSAL DE INTERRUPCIÓN' && option?.subsanado === false) &&
    //             descripcionOpcion !== "CAUSAL DE INTERRUPCIÓN") {
    //             causalInterrupcion = true;
    //         } else {
    //             causalInterrupcion = false;
    //         }
    //     }
    //     return causalInterrupcion;
    // }

    const NOVEDAD_INFORME_114 =
        'INFORME 114 - INCONSISTENCIA';

    const NOVEDAD_SUBSANABLE =
        'NOVEDAD SUBSANABLE';

    const NOVEDAD_CAUSAL =
        'CAUSAL DE INTERRUPCIÓN';

    const NOVEDAD_NINGUNA =
        'NINGUNA';

    const novedadesGrupoActualParaReglas =
        listadoInconsistenciasVista.filter(
            (item: any) =>
                Number(item?.grupoInconsistencia) ===
                Number(grupoActivoActual),
        );

    const existeInforme114GrupoActual =
        novedadesGrupoActualParaReglas.some(
            (item: any) =>
                normalizarNovedad(item?.novedad) ===
                NOVEDAD_INFORME_114,
        );

    const existeNingunaParaReglas =
        novedadesGrupoActualParaReglas.some(
            (item: any) =>
                normalizarNovedad(item?.novedad) ===
                NOVEDAD_NINGUNA,
        );

    const existeNovedadDistintaNinguna =
        novedadesGrupoActualParaReglas.some(
            (item: any) => {
                const novedad =
                    normalizarNovedad(
                        item?.novedad,
                    );

                return (
                    Boolean(novedad) &&
                    novedad !== 'SELECCIONE' &&
                    novedad !== NOVEDAD_NINGUNA
                );
            },
        );

    const causalesGrupoActual =
        novedadesGrupoActualParaReglas.filter(
            (item: any) =>
                normalizarNovedad(item?.novedad) ===
                NOVEDAD_CAUSAL,
        );

    const obtenerLlaveCausal = (
        item: any,
    ): string => {
        const idVinculo =
            obtenerIdVinculoNovedad(item);

        if (idVinculo) {
            return normalizarId(idVinculo);
        }

        return normalizarId(
            item?.causalInterrupcionId,
        );
    };

    const llavesCausalesGrupoActual =
        Array.from(
            new Set(
                causalesGrupoActual
                    .map(obtenerLlaveCausal)
                    .filter(Boolean),
            ),
        );

    const hayCausalInterrupcionPendiente =
        llavesCausalesGrupoActual.some(
            (llave: string) => {
                const existeRegistroSubsanado =
                    causalesGrupoActual.some(
                        (item: any) =>
                            obtenerLlaveCausal(item) ===
                            llave &&
                            estaSubsanado(item),
                    );

                return !existeRegistroSubsanado;
            },
        );

    const [deshabilitarCampos, setDeshabilitarCampos] = useState(false);

    useEffect(() => {
        const validacionVerificacion = responseSolicitud?.nombreTarea;
        const tareasDeshabilitadas = ["Trámite de pago retiro parcial", "Trámite de pago V 8", "Trámite de pago V 14", "Verificación cuenta individual"];

        setDeshabilitarCampos(tareasDeshabilitadas.includes(validacionVerificacion));
    }, [responseSolicitud]);



    const bloquearAccionesPorGrupoActual = (option: any) => {
        if (!grupoActivoActual) {
            return true;
        }

        return option?.grupoInconsistencia !== grupoActivoActual;
    };

    const limpiarFormularioNovedad = () => {
        setValue('index', 0);
        setValue('novedad', 'Seleccione');
        setValue('listaInconsistenciaId', '');
        setValue('causalInterrupcionId', '');
        setValue('informe', '');
        setValue('pagoSucesivo', '');
        setValue('subsanado', false);

        setMostarListadoCausales(false);
        setMostarListadoTipoNovedad(false);
        setMostrarListadoTipoNovedad414(false);
        setMostrarCheck(false);
        setNinguna(false);

        setTraerIdAntrior('');
        setTraerTodaData(null);
        setEstadoSubsanar(false);
    };

    const esMismaFila = (a: any, b: any) =>
        normalizarId(a.grupoInconsistencia) === normalizarId(b.grupoInconsistencia) &&
        normalizarId(a.novedad) === normalizarId(b.novedad) &&
        normalizarId(a.listaInconsistenciaId) === normalizarId(b.listaInconsistenciaId) &&
        normalizarId(a.causalInterrupcionId) === normalizarId(b.causalInterrupcionId) &&
        normalizarId(a.informe) === normalizarId(b.informe) &&
        normalizarId(a.pagoSucesivo) === normalizarId(b.pagoSucesivo);


    const handleAgregarNovedad = handleSubmit(async (data: any) => {
        const novedadSeleccionada = normalizarNovedad(data?.novedad);

        if (
            deshabilitarOpcionNovedad(
                novedadSeleccionada,
            )
        ) {
            toast(
                'La novedad seleccionada no está permitida de acuerdo con las novedades actualmente registradas.',
                { type: toast.TYPE.WARNING },
            );
            return;
        }

        if (
            novedadSeleccionada ===
            'NOVEDAD SUBSANABLE' &&
            existeNingunaGrupoActual
        ) {
            toast(
                'No puede agregar una novedad subsanable porque ya existe una novedad NINGUNA.',
                { type: toast.TYPE.WARNING },
            );
            return;
        }

        if (
            esGrupo1 &&
            novedadSeleccionada === 'NINGUNA' &&
            existeNingunaGrupo1
        ) {
            toast(
                'Ya existe una novedad NINGUNA registrada en Seguridad documental grupo 1.',
                { type: toast.TYPE.WARNING },
            );
            return;
        }

        const idVinculo = crearIdVinculoNovedad();

        const fechaActual = obtenerFechaActual();

        const payloadAgregar = {
            ...data,
            grupoInconsistencia: data?.grupoInconsistencia || grupoActivoActual,
            grupoVinculoNovedadId: idVinculo,
            fecha_Novedad: fechaActual,
            fechaNovedad: fechaActual,
            fechaCreacionNovedad: fechaActual,
            fecha_Subsanacion: fechaActual,
            fechaRegistro: fechaActual,
            subsanado: false,
        };

        registrarFechaLocalNovedad(payloadAgregar, fechaActual);

        setListadoInconsistenciasVista(prev =>
            aplicarFechasLocales([...prev, payloadAgregar]),
        );

        await addInconsistencia(payloadAgregar);
        limpiarFormularioNovedad();
        await recargarInconsistenciasVista();
    });

    // const handleEditarNovedad = handleSubmit(async (data: any) => {
    //     const registrosVinculados = itemEditando
    //         ? obtenerRegistrosVinculados(itemEditando)
    //         : [];

    //     const idVinculoExistente =
    //         obtenerIdVinculoNovedad(itemEditando) ||
    //         crearIdVinculoNovedad();

    //     const vaASubsanar =
    //         data?.subsanado === true ||
    //         String(data?.subsanado).toLowerCase() === 'true';


    //     const yaTieneRegistroSubsanado = registrosVinculados.some(
    //         (item: any) => item?.subsanado === true,
    //     );

    //     const camposEditables = camposEditablesNovedad(data);

    //     const registroOriginal =
    //         itemEditando || data;

    //     await updateInconsistencias({
    //         ...registroOriginal,
    //         ...camposEditables,

    //         grupoVinculoNovedadId:
    //             obtenerIdVinculoNovedad(
    //                 registroOriginal,
    //             ) || idVinculoExistente,

    //         subsanado:
    //             registroOriginal?.subsanado,

    //         fecha_Novedad:
    //             registroOriginal?.fecha_Novedad ||
    //             registroOriginal?.fechaNovedad ||
    //             registroOriginal?.fechaCreacionNovedad ||
    //             registroOriginal?.fecha_Creacion ||
    //             registroOriginal?.fechaCreacion ||
    //             registroOriginal?.fechaRegistro ||
    //             obtenerFechaActual(),
    //     });

    //     if (vaASubsanar && !yaTieneRegistroSubsanado) {
    //         const {
    //             formularioInconsistenciaId,
    //             inconsistenciaId,
    //             id,
    //             index,
    //             fecha_Novedad,
    //             fechaNovedad,
    //             fechaCreacionNovedad,
    //             fecha_Creacion,
    //             fechaCreacion,
    //             fecha_Subsanacion,
    //             fechaRegistro,
    //             ...dataSinIds
    //         } = data;

    //         const fechaActual = obtenerFechaActual();

    //         const payloadSubsanado = {
    //             ...dataSinIds,
    //             ...camposEditables,
    //             inconsistenciaId: null,
    //             formularioInconsistenciaId: undefined,
    //             id: undefined,
    //             grupoInconsistencia:
    //                 data?.grupoInconsistencia ||
    //                 itemEditando?.grupoInconsistencia ||
    //                 grupoActivoActual,
    //             grupoVinculoNovedadId: idVinculoExistente,
    //             subsanado: true,
    //             fecha_Novedad: fechaActual,
    //             fechaNovedad: fechaActual,
    //             fechaCreacionNovedad: fechaActual,
    //             fecha_Subsanacion: fechaActual,
    //             fechaRegistro: fechaActual,
    //         };

    //         registrarFechaLocalNovedad(payloadSubsanado, fechaActual);

    //         await addInconsistencia(payloadSubsanado);

    //         await recargarInconsistenciasVista();

    //         setListadoInconsistenciasVista(prev => {
    //             const listaConFechas = aplicarFechasLocales(prev);

    //             if (existeRegistroSubsanadoEnLista(listaConFechas, payloadSubsanado)) {
    //                 return listaConFechas;
    //             }

    //             return aplicarFechasLocales([
    //                 ...listaConFechas,
    //                 payloadSubsanado,
    //             ]);
    //         });

    //         limpiarFormularioNovedad();

    //     }

    // });

    // const handleEditarNovedad = handleSubmit(
    //     async (data: any) => {
    //         const novedadActual = normalizarNovedad(
    //             data?.novedad ||
    //             itemEditando?.novedad,
    //         );

    //         const generaHistorialSubsanacion =
    //             novedadActual ===
    //             'NOVEDAD SUBSANABLE' ||
    //             novedadActual ===
    //             'CAUSAL DE INTERRUPCIÓN';

    //         const registrosVinculados =
    //             itemEditando
    //                 ? obtenerRegistrosVinculados(
    //                     itemEditando,
    //                 )
    //                 : [];

    //         const idVinculoExistente =
    //             obtenerIdVinculoNovedad(
    //                 itemEditando,
    //             ) ||
    //             crearIdVinculoNovedad();

    //         const vaASubsanar =
    //             data?.subsanado === true ||
    //             String(data?.subsanado)
    //                 .toLowerCase() === 'true';

    //         const yaTieneRegistroSubsanado =
    //             registrosVinculados.some(
    //                 (item: any) =>
    //                     item?.subsanado === true ||
    //                     String(item?.subsanado)
    //                         .toLowerCase() === 'true',
    //             );

    //         const debeCrearRegistroSubsanado =
    //             generaHistorialSubsanacion &&
    //             vaASubsanar &&
    //             !yaTieneRegistroSubsanado;

    //         if (debeCrearRegistroSubsanado) {
    //             setOmitirAlertaCausalTemporalmente?.(
    //                 true,
    //             );
    //         }

    //         try {
    //             const camposEditables =
    //                 camposEditablesNovedad(data);

    //             const registroOriginal =
    //                 itemEditando || data;

    //             /*
    //              * Se actualizan únicamente los campos
    //              * editables del registro original.
    //              *
    //              * IMPORTANTE:
    //              * El registro original conserva su valor
    //              * anterior de subsanado, normalmente false.
    //              */
    //             await updateInconsistencias({
    //                 ...registroOriginal,
    //                 ...camposEditables,

    //                 grupoVinculoNovedadId:
    //                     obtenerIdVinculoNovedad(
    //                         registroOriginal,
    //                     ) ||
    //                     idVinculoExistente,

    //                 subsanado:
    //                     registroOriginal?.subsanado,

    //                 fecha_Novedad:
    //                     registroOriginal
    //                         ?.fecha_Novedad ||
    //                     registroOriginal
    //                         ?.fechaNovedad ||
    //                     registroOriginal
    //                         ?.fechaCreacionNovedad ||
    //                     registroOriginal
    //                         ?.fecha_Creacion ||
    //                     registroOriginal
    //                         ?.fechaCreacion ||
    //                     registroOriginal
    //                         ?.fechaRegistro ||
    //                     obtenerFechaActual(),
    //             });

    //             /*
    //              * Para NOVEDAD SUBSANABLE y
    //              * CAUSAL DE INTERRUPCIÓN se crea
    //              * un nuevo registro con subsanado true.
    //              */
    //             if (debeCrearRegistroSubsanado) {
    //                 const {
    //                     formularioInconsistenciaId,
    //                     inconsistenciaId,
    //                     id,
    //                     index,
    //                     fecha_Novedad,
    //                     fechaNovedad,
    //                     fechaCreacionNovedad,
    //                     fecha_Creacion,
    //                     fechaCreacion,
    //                     fecha_Subsanacion,
    //                     fechaRegistro,
    //                     ...dataSinIds
    //                 } = data;

    //                 const fechaActual =
    //                     obtenerFechaActual();

    //                 const payloadSubsanado = {
    //                     ...dataSinIds,
    //                     ...camposEditables,

    //                     /*
    //                      * Se eliminan los identificadores
    //                      * para obligar al backend a insertar
    //                      * una nueva fila.
    //                      */
    //                     inconsistenciaId: null,
    //                     formularioInconsistenciaId:
    //                         undefined,
    //                     id: undefined,

    //                     grupoInconsistencia:
    //                         data?.grupoInconsistencia ||
    //                         itemEditando
    //                             ?.grupoInconsistencia ||
    //                         grupoActivoActual,

    //                     /*
    //                      * El mismo vínculo permite relacionar
    //                      * el registro NO con el registro SI.
    //                      */
    //                     grupoVinculoNovedadId:
    //                         idVinculoExistente,

    //                     subsanado: true,

    //                     fecha_Novedad:
    //                         fechaActual,
    //                     fechaNovedad:
    //                         fechaActual,
    //                     fechaCreacionNovedad:
    //                         fechaActual,
    //                     fecha_Subsanacion:
    //                         fechaActual,
    //                     fechaRegistro:
    //                         fechaActual,
    //                 };

    //                 registrarFechaLocalNovedad(
    //                     payloadSubsanado,
    //                     fechaActual,
    //                 );

    //                 await addInconsistencia(
    //                     payloadSubsanado,
    //                 );

    //                 /*
    //                  * Primero se consulta nuevamente al
    //                  * servidor para obtener ambos registros.
    //                  */
    //                 await recargarInconsistenciasVista();

    //                 /*
    //                  * Respaldo local mientras el backend
    //                  * termina de reflejar el nuevo registro.
    //                  */
    //                 setListadoInconsistenciasVista(
    //                     (prev) => {
    //                         const listaConFechas =
    //                             aplicarFechasLocales(
    //                                 prev,
    //                             );

    //                         const yaExisteSubsanado =
    //                             existeRegistroSubsanadoEnLista(
    //                                 listaConFechas,
    //                                 payloadSubsanado,
    //                             );

    //                         if (yaExisteSubsanado) {
    //                             return listaConFechas;
    //                         }

    //                         return aplicarFechasLocales([
    //                             ...listaConFechas,
    //                             payloadSubsanado,
    //                         ]);
    //                     },
    //                 );
    //             }

    //             limpiarFormularioNovedad();
    //         } catch (error) {
    //             console.error(
    //                 'Error editando la novedad:',
    //                 error,
    //             );

    //             toast(
    //                 'No fue posible actualizar la novedad.',
    //                 {
    //                     type: toast.TYPE.ERROR,
    //                 },
    //             );
    //         } finally {
    //             if (debeCrearRegistroSubsanado) {
    //                 setOmitirAlertaCausalTemporalmente?.(
    //                     false,
    //                 );
    //             }
    //         }
    //     },
    // );

    const handleEditarNovedad = handleSubmit(
        async (data: any) => {
            if (!itemEditando) {
                toast(
                    'No se encontró el registro que se desea editar.',
                    {
                        type: toast.TYPE.ERROR,
                    },
                );
    
                return;
            }
    
            const estaSubsanando =
                data?.subsanado === true ||
                String(data?.subsanado)
                    .toLowerCase() === 'true';
    
            const fechaActual =
                obtenerFechaActual();
    
            const camposEditables =
                camposEditablesNovedad(data);
    
            const payloadActualizar = {
                ...itemEditando,
                ...camposEditables,
    
                /*
                 * Se conserva el identificador original
                 * para que el backend actualice la misma fila.
                 */
                formularioInconsistenciaId:
                    itemEditando?.formularioInconsistenciaId,
    
                inconsistenciaId:
                    itemEditando?.inconsistenciaId,
    
                id:
                    itemEditando?.id,
    
                grupoInconsistencia:
                    itemEditando?.grupoInconsistencia ||
                    data?.grupoInconsistencia ||
                    grupoActivoActual,
    
                grupoVinculoNovedadId:
                    obtenerIdVinculoNovedad(
                        itemEditando,
                    ),
    
                /*
                 * Ahora sí se envía el nuevo estado
                 * seleccionado durante la edición.
                 */
                subsanado:
                    estaSubsanando,
    
                /*
                 * Se conserva la fecha original
                 * de creación de la novedad.
                 */
                fecha_Novedad:
                    itemEditando?.fecha_Novedad ||
                    itemEditando?.fechaNovedad ||
                    itemEditando?.fechaCreacionNovedad ||
                    itemEditando?.fecha_Creacion ||
                    itemEditando?.fechaCreacion ||
                    itemEditando?.fechaRegistro ||
                    fechaActual,
    
                /*
                 * La fecha de subsanación solamente
                 * cambia cuando se marca subsanado.
                 */
                fecha_Subsanacion:
                    estaSubsanando
                        ? fechaActual
                        : (
                            itemEditando?.fecha_Subsanacion ||
                            ''
                        ),
    
                solicitudId:
                    itemEditando?.solicitudId ||
                    responseSolicitud?.solicitudId ||
                    idSolicitud,
    
                solicitudTareaId:
                    itemEditando?.solicitudTareaId ||
                    responseSolicitud?.solicitudTareaId,
            };
    
            console.log(
                '[EDITAR NOVEDAD - SOLO ACTUALIZAR]',
                payloadActualizar,
            );
    
            if (
                normalizarNovedad(
                    payloadActualizar?.novedad,
                ) === 'CAUSAL DE INTERRUPCIÓN' &&
                estaSubsanando
            ) {
                setOmitirAlertaCausalTemporalmente?.(
                    true,
                );
            }
    
            try {
                await updateInconsistencias(
                    payloadActualizar,
                );
    
                await recargarInconsistenciasVista();
    
                limpiarFormularioNovedad();
            } catch (error) {
                console.error(
                    'Error actualizando la novedad:',
                    error,
                );
    
                toast(
                    'No fue posible actualizar la novedad.',
                    {
                        type: toast.TYPE.ERROR,
                    },
                );
            } finally {
                setOmitirAlertaCausalTemporalmente?.(
                    false,
                );
            }
        },
    );

    const handleEliminarNovedad = async (
        option: any,
    ) => {
        try {
            const confirmAlert =
                await NotificationService.confirmAlert(
                    '¿Desea eliminar la novedad y sus registros vinculados?',
                );

            if (!confirmAlert) {
                return;
            }

            setOmitirAlertaCausalTemporalmente(true);

            const registrosServidorActuales =
                await consultarInconsistenciasServidor();

            const registrosRelacionadosServidor =
                registrosServidorActuales.filter(
                    (item: any) =>
                        esRegistroRelacionadoParaBorrar(
                            item,
                            option,
                        ),
                );

            const registrosRelacionadosLocales =
                listadoInconsistenciasVista.filter(
                    (item: any) =>
                        esRegistroRelacionadoParaBorrar(
                            item,
                            option,
                        ),
                );

            const registrosAEliminar =
                obtenerRegistrosUnicosPorId([
                    ...registrosRelacionadosServidor,
                    ...registrosRelacionadosLocales,
                    option,
                ]);

            if (registrosAEliminar.length === 0) {
                toast(
                    'No se encontraron registros para eliminar.',
                    {
                        type: toast.TYPE.WARNING,
                    },
                );

                return;
            }

            const respuestasEliminacion: any[] = [];

            await registrosAEliminar.reduce(
                (
                    promesaAnterior: Promise<void>,
                    registro: any,
                ) =>
                    promesaAnterior.then(async () => {
                        const respuesta =
                            await deleteInconsistencias(
                                registro,
                                {
                                    mostrarAlerta: false,
                                    mostrarToast: false,
                                },
                            );

                        respuestasEliminacion.push({
                            inconsistenciaId:
                                registro
                                    ?.inconsistenciaId,
                            subsanado:
                                registro?.subsanado,
                            respuesta,
                        });
                    }),
                Promise.resolve(),
            );

            const dataActualizada =
                await consultarInconsistenciasServidor();

            const registrosQueQuedaron =
                dataActualizada.filter(
                    (item: any) =>
                        esRegistroRelacionadoParaBorrar(
                            item,
                            option,
                        ),
                );

            setListadoInconsistenciasVista(
                aplicarFechasLocales(
                    dataActualizada,
                ),
            );

            if (registrosQueQuedaron.length > 0) {
                toast(
                    `No se eliminaron todos los registros. Quedaron ${registrosQueQuedaron.length} registro(s) en servidor.`,
                    {
                        type: toast.TYPE.WARNING,
                    },
                );
            } else {
                toast(
                    'Novedad eliminada correctamente.',
                    {
                        type: toast.TYPE.SUCCESS,
                    },
                );
            }

            limpiarFormularioNovedad();
        } catch (error) {
            console.error(
                'Error eliminando novedades vinculadas:',
                error,
            );

            toast(
                'Error eliminando novedades vinculadas',
                {
                    type: toast.TYPE.ERROR,
                },
            );
        } finally {
            setOmitirAlertaCausalTemporalmente(false);
        }
    };


    const descripcionTipoNovedad = (option: any) =>
        tiposNovedades?.find(
            (x: any) => normalizarId(x?.tpO_NOV_ID) === normalizarId(option?.listaInconsistenciaId)
        )?.tpO_NOV_DESCRIPCION
        || option?.tpO_NOV_DESCRIPCION
        || option?.tipoNovedad
        || option?.tipoNovedadDescripcion
        || '';


    const descripcionCausal = (option: any) =>
        tiposCausales?.find(
            (x: any) => normalizarId(x?.caU_ID) === normalizarId(option?.causalInterrupcionId)
        )?.caU_DESCRIPCION
        || option?.caU_DESCRIPCION
        || option?.causalInterrupcion
        || option?.causalInterrupcionDescripcion
        || '';


    const descripcionInforme = (option: any) =>
        tiposInformeInconsistencia?.find(
            (x: any) => normalizarId(x?.inF_ID) === normalizarId(option?.informe)
        )?.inF_DESCRIPCION
        || option?.inF_DESCRIPCION
        || option?.informeDescripcion
        || '';


    const obtenerKeyFila = (option: any, index: number) =>
        [
            option?.formularioInconsistenciaId,
            option?.inconsistenciaId,
            option?.id,
            option?.grupoVinculoNovedadId,
            option?.idGrupoNovedad,
            option?.novedadVinculoId,
            option?.grupoInconsistencia,
            option?.novedad,
            option?.listaInconsistenciaId,
            option?.causalInterrupcionId,
            option?.informe,
            option?.pagoSucesivo,
            option?.fecha_Novedad,
            option?.fecha_Subsanacion,
            index,
        ].map(x => normalizarId(x)).join('-');


    const deshabilitarOpcionNovedad = (
        descripcion: string,
    ) => {
        const novedadNormalizada =
            normalizarNovedad(descripcion);

        const esNinguna =
            novedadNormalizada ===
            NOVEDAD_NINGUNA;

        const esNovedadSubsanable =
            novedadNormalizada ===
            NOVEDAD_SUBSANABLE;

        const esCausalInterrupcion =
            novedadNormalizada ===
            NOVEDAD_CAUSAL;

        /*
         * Si existe INFORME 114 en el grupo actual,
         * no se permite agregar otra novedad.
         */
        if (existeInforme114GrupoActual) {
            return true;
        }

        /*
         * Si existe NINGUNA en el grupo actual,
         * no se permite agregar otra novedad.
         */
        if (
            existeNingunaParaReglas &&
            !esNinguna
        ) {
            return true;
        }

        /*
         * Evita registrar NINGUNA más de una vez
         * en el mismo grupo.
         */
        if (
            esNinguna &&
            existeNingunaGrupoActual
        ) {
            return true;
        }

        /*
         * Una causal pendiente solo permite
         * seleccionar otra causal.
         */
        if (
            hayCausalInterrupcionPendiente &&
            !esCausalInterrupcion
        ) {
            return true;
        }

        /*
         * En Grupo 1, NINGUNA continúa siendo
         * excluyente respecto a otras novedades.
         *
         * En Grupo 2 sí se permite seleccionar
         * NINGUNA aunque ya existan otras novedades.
         */
        if (
            esGrupo1 &&
            esNinguna &&
            existeNovedadDistintaNinguna
        ) {
            return true;
        }

        /*
         * Una novedad subsanable no se puede agregar
         * si ya existe NINGUNA en el grupo actual.
         */
        if (
            esNovedadSubsanable &&
            existeNingunaGrupoActual
        ) {
            return true;
        }

        /*
         * Restricciones exclusivas de Grupo 1.
         */
        if (
            esGrupo1 &&
            esNinguna &&
            bloquearNingunaGrupo1
        ) {
            return true;
        }

        if (
            esGrupo1 &&
            esNinguna &&
            desHabilitarNingunaGrupo1
        ) {
            return true;
        }

        return false;
    };


    return (
        <>
            <CHPaper title={`Seguridad documental ${grupo1 ? 'Grupo 1' : 'Grupo 2'}`}>
                <Grid container>
                    {!(String(responseSolicitud?.nombreTarea).includes('Verificación cuenta individual')) && (
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="novedad"
                                control={control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido.',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => {
                                    const bloquearOpcionesPorMatricula =
                                        esGrupo2 &&
                                        matriculaBloqueadaActiva;

                                    return (
                                        <CHSelect
                                            error={!!error}
                                            label="Novedad"
                                            value={value}
                                            helperText={
                                                error
                                                    ? error.message
                                                    : null
                                            }
                                            /*
                                             * El select continúa desplegándose cuando
                                             * existe bloqueo de matrícula.
                                             *
                                             * El bloqueo se aplica individualmente
                                             * sobre las opciones.
                                             */
                                            readOnly={
                                                siEsAprobadaSolcitud ||
                                                deshabilitarCampos ||
                                                bloqueoControlPreventivoActivo
                                            }
                                            onChange={(e: any) => {
                                                const valor =
                                                    e.target.value;

                                                tiposNovedadPorId(valor);

                                                if (
                                                    valor === 'NINGUNA' &&
                                                    desHabilitarNingunaGr2
                                                ) {
                                                    setNinguna(true);
                                                    setMostarListadoCausales(
                                                        false,
                                                    );
                                                    setMostarListadoTipoNovedad(
                                                        false,
                                                    );
                                                    setMostrarListadoTipoNovedad414(
                                                        false,
                                                    );

                                                    setValue(
                                                        'listaInconsistenciaId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'causalInterrupcionId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'informe',
                                                        '',
                                                    );
                                                    setValue(
                                                        'pagoSucesivo',
                                                        '',
                                                    );

                                                    setMostrarCheck(false);
                                                } else if (
                                                    valor ===
                                                    'NOVEDAD SUBSANABLE'
                                                ) {
                                                    setMostarListadoCausales(
                                                        false,
                                                    );
                                                    setMostarListadoTipoNovedad(
                                                        true,
                                                    );
                                                    setMostrarListadoTipoNovedad414(
                                                        false,
                                                    );

                                                    setValue(
                                                        'listaInconsistenciaId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'causalInterrupcionId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'informe',
                                                        '',
                                                    );
                                                    setValue(
                                                        'pagoSucesivo',
                                                        '',
                                                    );

                                                    setMostrarCheck(false);
                                                } else if (
                                                    valor ===
                                                    'PAGOS SUCESIVOS'
                                                ) {
                                                    setMostarListadoCausales(
                                                        false,
                                                    );
                                                    setMostarListadoTipoNovedad(
                                                        false,
                                                    );
                                                    setMostrarListadoTipoNovedad414(
                                                        false,
                                                    );

                                                    setValue(
                                                        'listaInconsistenciaId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'causalInterrupcionId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'informe',
                                                        '',
                                                    );
                                                    setValue(
                                                        'pagoSucesivo',
                                                        '',
                                                    );
                                                } else if (
                                                    valor ===
                                                    'CAUSAL DE INTERRUPCIÓN'
                                                ) {
                                                    setMostarListadoCausales(
                                                        true,
                                                    );
                                                    setMostarListadoTipoNovedad(
                                                        false,
                                                    );
                                                    setMostrarListadoTipoNovedad414(
                                                        false,
                                                    );

                                                    setValue(
                                                        'listaInconsistenciaId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'causalInterrupcionId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'informe',
                                                        '',
                                                    );
                                                    setValue(
                                                        'pagoSucesivo',
                                                        '',
                                                    );

                                                    setMostrarCheck(false);
                                                } else if (
                                                    valor ===
                                                    'INFORME 114 - INCONSISTENCIA'
                                                ) {
                                                    setMostarListadoCausales(
                                                        false,
                                                    );
                                                    setMostarListadoTipoNovedad(
                                                        false,
                                                    );
                                                    setMostrarListadoTipoNovedad414(
                                                        true,
                                                    );

                                                    setValue(
                                                        'listaInconsistenciaId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'causalInterrupcionId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'informe',
                                                        '',
                                                    );
                                                    setValue(
                                                        'pagoSucesivo',
                                                        '',
                                                    );

                                                    setMostrarCheck(false);
                                                } else {
                                                    setMostarListadoCausales(
                                                        false,
                                                    );
                                                    setMostarListadoTipoNovedad(
                                                        false,
                                                    );
                                                    setMostrarListadoTipoNovedad414(
                                                        false,
                                                    );

                                                    setValue(
                                                        'listaInconsistenciaId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'causalInterrupcionId',
                                                        '',
                                                    );
                                                    setValue(
                                                        'informe',
                                                        '',
                                                    );
                                                    setValue(
                                                        'pagoSucesivo',
                                                        '',
                                                    );

                                                    setMostrarCheck(false);
                                                }

                                                onChange(e);
                                            }}
                                            defaultValue="Seleccione"
                                        >
                                            <MenuItem
                                                key="Seleccione"
                                                value="Seleccione"
                                                disabled={
                                                    mostrarCheck ||
                                                    bloquearOpcionesPorMatricula
                                                }
                                            >
                                                --SELECCIONE--
                                            </MenuItem>

                                            {novedades?.map(
                                                (option: any) => (
                                                    <MenuItem
                                                        key={option.noV_ID}
                                                        value={
                                                            option.noV_DESCRIPCION
                                                        }
                                                        disabled={
                                                            bloquearOpcionesPorMatricula ||
                                                            deshabilitarOpcionNovedad(
                                                                option
                                                                    .noV_DESCRIPCION,
                                                            )
                                                        }
                                                    >
                                                        {
                                                            option
                                                                .noV_DESCRIPCION
                                                        }
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    );
                                }}
                            />
                        </Grid>
                    )}
                    {mostarListadoTipoNovedad ? (
                        <>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="listaInconsistenciaId"
                                    control={control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido.',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo de Inconsistencia*"
                                            value={value}
                                            helperText={error ? error.message : null}
                                            onChange={(e: any) => {
                                                onChange(e);
                                            }}

                                        >

                                            {tiposNovedades?.map((option: any) => (
                                                <MenuItem
                                                    key={option?.tpO_NOV_ID}
                                                    disabled={
                                                        bloqueoControlPreventivoActivo ||
                                                        tipoNovedadYaUsado(
                                                            option?.tpO_NOV_ID,
                                                        )
                                                    }
                                                    value={option?.tpO_NOV_ID}
                                                >
                                                    {option.tpO_NOV_DESCRIPCION}
                                                </MenuItem>
                                            ))}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>

                        </>
                    ) : null}

                    {mostarListadoCausales ? (
                        <>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="causalInterrupcionId"
                                    control={control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido.',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Causal de Interrupción*"
                                            value={value}
                                            helperText={error ? error.message : null}
                                            onChange={(e: any) => {
                                                onChange(e);
                                            }}
                                        >

                                            {tiposCausales?.map((option: any) => (
                                                <MenuItem
                                                    key={option.caU_ID
                                                    }
                                                    value={option.caU_ID
                                                    }
                                                    disabled={causalYaUsada(option?.caU_ID)}
                                                >
                                                    {option.caU_DESCRIPCION}
                                                </MenuItem>
                                            ))}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>

                        </>
                    ) : null}

                    {mostrarListadoTipoNovedad414 ? (
                        <>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="informe"
                                    control={control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido.',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Informe 114 - Inconsistencia"
                                            value={value}
                                            helperText={error ? error.message : null}
                                            onChange={(e: any) => {
                                                onChange(e);
                                            }}
                                        >

                                            {tiposInformeInconsistencia
                                                ?.map((option: any) => (
                                                    <MenuItem
                                                        key={option.inF_ID}
                                                        value={option.inF_ID}
                                                        disabled={informeYaUsado(option?.inF_ID)}
                                                    >
                                                        {option.inF_DESCRIPCION}
                                                    </MenuItem>
                                                ))}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>

                        </>
                    ) : null}

                </Grid>

                {mostrarCheck ? (
                    <>
                        <Grid container>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="subsanado"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHCheckbox
                                            label="Subsanado"
                                            onChange={(e: any) => {
                                                const { checked } = e.target;

                                                setEstadoSubsanar(!checked);
                                                onChange(checked);
                                                setValue('subsanado', checked);
                                            }}
                                            checked={Boolean(value)}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </>
                ) : null}
                {
                    editButton ? (
                        <Grid container>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Button
                                    className={classes.buttonSubmit}
                                    color='primary'
                                    onClick={handleEditarNovedad}>
                                    Editar novedad
                                </Button>
                            </Grid>
                        </Grid>
                    ) : (
                        <Grid container>
                            {!(String(responseSolicitud?.nombreTarea).includes('Verificación cuenta individual')) && (
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Button
                                        color='primary'
                                        className={classes.buttonSubmit}
                                        onClick={handleAgregarNovedad}
                                        disabled={siEsAprobadaSolcitud || deshabilitarCampos}
                                    >
                                        Agregar novedad
                                    </Button>
                                </Grid>
                            )}
                        </Grid>
                    )
                }
                <h3>Lista de Novedades</h3>
                <TableContainer component={Paper}>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        <Table aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Grupo</TableCell>
                                    <TableCell>Novedad</TableCell>
                                    <TableCell>Tipo de Novedades</TableCell>
                                    <TableCell>Tipo de Causales</TableCell>
                                    <TableCell>Tipo de información 114 - Inconsistencia</TableCell>
                                    <TableCell>Subsanado</TableCell>
                                    <TableCell>Fecha</TableCell>
                                    {!(String(responseSolicitud?.nombreTarea).includes('Verificación cuenta individual')) && (
                                        <TableCell align="center">
                                            Acciones
                                        </TableCell>
                                    )}
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {/* {refresh && null} */}
                                {listadoTablaOrdenado?.map(
                                    (option: any, index: number, a: any) => (
                                        <TableRow key={obtenerKeyFila(option, index)}>
                                            <TableCell>
                                                {option.grupoInconsistencia === 1 ? 'Grupo 1' : 'Grupo 2'}
                                            </TableCell>

                                            <TableCell>
                                                {option?.novedad || ''}
                                            </TableCell>

                                            <TableCell>
                                                {descripcionTipoNovedad(option)}
                                            </TableCell>

                                            <TableCell>
                                                {descripcionCausal(option)}
                                            </TableCell>
                                            <TableCell>
                                                {descripcionInforme(option)}
                                            </TableCell>
                                            {
                                                option.novedad === "NINGUNA" ? <TableCell>
                                                    {option.subsanado === false ? '' : ''}
                                                </TableCell> :
                                                    <TableCell>
                                                        {option.subsanado === true ? 'SI' : 'NO'}
                                                    </TableCell>
                                            }

                                            <TableCell>
                                                {obtenerFechaFila(option)}
                                            </TableCell>

                                            {
                                                siEsAprobadaSolcitud ?
                                                    '' :
                                                    <TableCell align="center">
                                                        {
                                                            mostrarAccionesPorVinculo(option) &&
                                                            !(String(responseSolicitud?.nombreTarea).includes('Verificación cuenta individual')) && (
                                                                <>
                                                                    <IconButton
                                                                        aria-label="Editar"
                                                                        color="primary"
                                                                        disabled={
                                                                            bloquearAccionesPorGrupoActual(option) ||
                                                                            deshabilitarCampos ||
                                                                            normalizarNovedad(option?.novedad) === 'NINGUNA'
                                                                        }
                                                                        onClick={() => {
                                                                            const indexOriginal =
                                                                                listadoTabla.indexOf(option);

                                                                            loadEditInconsistencia(
                                                                                option,
                                                                                indexOriginal,
                                                                            );
                                                                        }}
                                                                    >
                                                                        <Edit />
                                                                    </IconButton>

                                                                    <IconButton
                                                                        aria-label="Eliminar"
                                                                        color="secondary"
                                                                        disabled={
                                                                            bloquearAccionesPorGrupoActual(option) ||
                                                                            deshabilitarCampos ||
                                                                            (
                                                                                deshativarBtnEliminarGp2 &&
                                                                                option.grupoInconsistencia === 1
                                                                            )
                                                                        }
                                                                        onClick={() => handleEliminarNovedad(option)}
                                                                    >
                                                                        <Delete />
                                                                    </IconButton>
                                                                </>
                                                            )
                                                        }
                                                    </TableCell>
                                            }
                                        </TableRow>
                                    ),
                                )}
                            </TableBody>
                        </Table>
                    </Grid>
                </TableContainer>
            </CHPaper>
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    userState: state.users,
});

const mapDispatchToProps = (dispatch: any) => ({
    obtenerGrupos: () => dispatch(startGetAllGroupsInconsistencies()),
    obtenerHerramientas: () => dispatch(startGetAllToolsInconsistencies()),
    obtenerPuntosAtencion: () => dispatch(startGetAllPointsAttention()),
    obtenerUsuarios: (token: string) => dispatch(startLoadUsers(token)),
});

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(FormularioInconsistenciasGrupo1);
