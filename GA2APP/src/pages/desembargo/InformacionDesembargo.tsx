import React, { useEffect, useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    Button,
    Theme,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import AttachFileIcon from '@material-ui/icons/AttachFile';
import swal from 'sweetalert';
import { debug } from 'console';
import { useLocation } from 'react-router-dom';
import { AppState } from '../../store/rootReducer';
import history from '../../utils/history.router';
import CHPaper from '../../components/ch-paper/CHPaper';
import { startGetAllIdentificationTypes } from '../../actions/catalog.action';
import CHTextField from '../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import UtilService from '../../utils/util.service';
import ApiEmbargos from '../../services/api-embargos.service';
import ApiSolicitud from '../../services/api-solicitud.service';
import { desembargoForm } from '../desembargosConceptos/constants/desembargosform.model.type';
import { ObtenerDesembargoDto } from '../../types/desembargo.model.type';
import ApiClients from '../../services/api-clients.service';
import ApiCatalog from '../../services/api-catalog.service';
import ConceptosEmbargoModal from './ConceptosEmbargoModal';
import { ConceptoEmbargosDto } from '../../types/embargo.model.type';
import { ModalArchivoRespuesta } from '../embargos/components/ModalArchivoRespuesta';
import { LoadingComponent } from '../../components/loading/loading.component';


const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        buttonContainer: {
            display: 'flex',
            justifyContent: 'flex-end',
            alignItems: 'center',
        },
        button: {
            width: '200px',
            marginLeft: '.8rem',
        },
        regresarButton: {
            marginLeft: '1rem',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        alineationRight: {
            marginLeft: '54rem',
        },
        modal: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            backgroundColor: 'rgba(0, 0, 0, 0.5)',
            zIndex: 9999,
            '& .MuiPaper-root': {
                margin: 'auto',
                outline: 'none',
                borderRadius: 8,
                width: '50%',
                height: '50%',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                [theme.breakpoints.down('sm')]: {
                    width: '80%',
                    height: '80%',
                },
            },
        },
        paper: {
            backgroundColor: theme.palette.background.paper,
            border: '2px solid #000',
            boxShadow: theme.shadows[5],
            padding: theme.spacing(2, 4, 3),
            width: '80%',
            height: '80%',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            borderRadius: '3%'
        },
        header: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            marginBottom: '1rem',
        },
        title: {
            color: '#1C3F8B',
            textAlign: 'center',
            fontSize: '1.5rem',
            fontWeight: 'bold',
            margin: '0 0 20px 0',
        },
        actions: {
            display: 'flex',
            justifyContent: 'flex-end',
            color: 'default',
        },
        customCheckbox: {
            color: 'gray',
            '&$checked': {
                color: 'gray',
            },
        },
        iconoDodoRadicado: {
            marginTop: '3%',
        },
        enlaceDodo: {
            cursor: 'pointer',
        }
    }),
);

function InfoDesembargo({
    data,
    openModal,
    embargo,
    allTipoActor,
    onChangeCheck,
    cuentas,
    conceptos,
    totalDesembargo,
    totalEmbargo,
    totalProcesado
}: {
    data: any[] | null;
    openModal: () => void;
    embargo:any;
    allTipoActor:any;
    onChangeCheck:any;
    cuentas:any;
    conceptos: any;
    totalDesembargo: any;
    totalEmbargo: number;
    totalProcesado: any;
}) {
    const [elementos, setElementos] = useState<any[]>([]);
    const [todosSeleccionados, setTodosSeleccionados] = useState(false);
    const [desmarcarCheckbox, setDesmarcarCheckbox] = useState(false);

    const saldoPorCuenta = React.useMemo(
        () =>
            (Array.isArray(conceptos) ? conceptos : []).reduce<Record<number, number>>(
                (acc, c) => {
                    acc[c.cuentaId] = c.saldo ?? 0;
                    return acc;
                },
                {}
            ),
        [conceptos]
    );

    if (data === null || !Array.isArray(data)) {
        return <div>No se encontraron resultados</div>;
    }

    const handleAbrirProceso = (id: any) => {
        openModal();
    };

    const handleSeleccionar = (id: any) => {
        const newSelection = elementos.map(elemento =>
            elemento.embargo.id === id
                ? { ...elemento, seleccionado: !elemento.seleccionado }
                : elemento
        );
        setElementos(newSelection);

        if (todosSeleccionados) {
            setTodosSeleccionados(false);
        }

        handleAbrirProceso(id);

        if (desmarcarCheckbox) {
            setDesmarcarCheckbox(false);
        }
    };

    type UniqueItems = {
        [key: string]: boolean;
    };

    const uniqueItems: UniqueItems = {};

    return (

        <CHPaper title="Información desembargo">
            <Grid container>
                <Grid item xs={12} className="container-1">
                    <table className="table" style={{ width: '100%', borderCollapse: 'separate' }}>
                        <thead>
                            <th className="tab-space-01">
                                <span>Procesar</span>
                            </th>
                            <th className="tab-space-01">No. Radicado Asociado</th>
                            <th className="tab-space-01">Números Expedientes</th>
                            <th className="tab-space-01">Tipo de Proceso</th>
                            <th className="tab-space-01">Tipo de Actor</th>
                            <th className="tab-space-01">Cuenta de Depósito</th>
                            <th className="tab-space-01">Razón Social</th>
                            <th className="tab-space-01">Total Desembargo</th>
                        </thead>
                        <tbody>
                            {data?.map((row, index) => {
                                if (row.embargo.radicadoWorkManager in uniqueItems || row.embargo.expedienteBancoAgrario in uniqueItems) {
                                    return null;
                                }
                                uniqueItems[row.embargo.radicadoWorkManager] = true;
                                uniqueItems[row.embargo.expedienteBancoAgrario] = true;

                                return (
                                    <tr key={`table-${index}`} style={{ borderBottom: '1px solid #ccc', padding: '8px' }}>
                                        <td>
                                            <Button
                                                variant="contained"
                                                color="primary"
                                                onClick={() => handleSeleccionar(row.embargo.id)}
                                            >
                                                Detalle
                                            </Button>
                                        </td>
                                        <td>{row.embargo.radicadoWorkManager}</td>
                                        <td>{row.embargo.expedienteBancoAgrario}</td>
                                        <td>{row.embargo.tipoProceso}</td>
                                        <td>{row.embargo?.tipoActor}</td>
                                        <td>{row.conceptos[0]?.numeroCuenta}</td>
                                        <td>{row.embargo.juzgado}</td>
                                        <td>{UtilService.formatCurrency(totalProcesado || totalEmbargo)}</td>



                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </Grid>
            </Grid >
        </CHPaper >
    );
};

const InformacionDesembargos = (props: any) => {
    const api = ApiEmbargos();
    const apiCliente = ApiClients();
    const apiActor = ApiCatalog();
    const apiSolicitud = ApiSolicitud();
    const classes = useStyles();

    const [estadoEmbargo, setEstadoEmbargo] = useState();
    const [estadosEmbargo, setEstadosEmbargo] = useState();
    const { catalogState } = props;
    const [desembargos, setDesembargos] = useState<ObtenerDesembargoDto[]>([]);
    const [embargo, setEmbargo] = useState<any>();
    const [conceptosCuenta, setConceptosCuenta] = useState<ConceptoEmbargosDto>();
    const [hasLoadedFields, setHasLoadedFields] = useState(false);
    const [idTipoIdentificacion, setIdTipoIdentificacion] = useState();
    const [identificacion, setIdentificacion] = useState();
    const [nombreDemandado, setNombreDemandado] = useState();
    const [numeroRadicado, setNumeroRadicado] = useState();
    const [fechaRadicado, setFechaRadicado] = useState();
    const [numeroOficio, setNumeroOficio] = useState();
    const [fechaRadicadoJuzgado, setFechaRadicadoJuzgado] = useState();
    const [open, setOpen] = useState(false);
    const [cuentas, setCuentas] = useState<any[]>();
    const [allTipoActor, setAllTipoActor] = useState<any[]>([]);
    const [linkDodo, setLinkDodo] = useState('');
    const [loading, setLoading] = useState(false);
    const [totalEmbargo, setTotalEmbargo] = useState(0);
    const [totalDesembargo, setTotalDesembargo] = useState<number | null>(null);
    const [fechaRadicadoWorkManager, setFechaRadicadoWorkManager] = useState();
    const [totalProcesado, setTotalProcesado] = useState<number>(0);

    const location:any = useLocation();
    const { datoEstado, row }  = location.state || {};


    const handleTotalDesdeModal = (total: number) => {
        setTotalDesembargo(total);
    };


    const formularioDesembargos = useForm({
        defaultValues: {
            tipoIdentificacionId: 0,
            identificacionCliente: '',
            nombreDemandado: '',
            radicadoWorkManager: '',
            fechaRadicadoWorkManager: new Date(),
            radicadoJuzgado: '',
            fechaRadicadoJuzgado: new Date(),
            id: '',
        },
    });

    useEffect(() => {
        const url = window.location.href;
        const idAndRadicado = url.split('=')[1];
        const id = idAndRadicado.split('%')[0];

        api.getObtenerEmbargosById(id)
            .then((response) => {
                if (response.isSuccess) {
                    setEmbargo([response.data]);
                    setEstadoEmbargo(response.data.embargo.estado);
                    setFechaRadicadoWorkManager(response.data.embargo.fechaRadicadoWorkManager);
                    setFechaRadicadoJuzgado(response.data.embargo.fechaRadicadoJuzgado);
                    obtenerUrlDodo(response?.data?.embargo?.radicadoWorkManager);
                }
            });
    }, [fechaRadicadoWorkManager]);

    useEffect(() => {
        if (embargo && embargo.length > 0) {
            apiCliente
                .getCuentasConceptosPorTipoNumeroIdentificacion(
                    embargo[0].embargo.tipoIdentificacionId,
                    embargo[0].embargo.identificacionCliente,
                )
                .then(async response => {
                    if (response) {
                        const array = response.data?.cuentas;

                        if (response.isSuccess && array) {
                            setCuentas(array);
                            const cuentaValida = array.find((cuentas: { estadoCuenta: string; }) =>
                                !cuentas.estadoCuenta?.toUpperCase().includes("CANCELADA")
                            );
                            if (cuentaValida) {
                                setConceptosCuenta(cuentaValida.conceptos);
                            } else {
                                setConceptosCuenta(array[0].conceptos);
                            }
                        }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        }
    }, [embargo]);

    useEffect(() => {
        const fetchData = async () => {
            if (embargo && embargo.length > 0 && conceptosCuenta !== null) {
                obtenerTipoActorEmbargo();
                await calcularEmbargos(conceptosCuenta, embargo[0]?.conceptos);
            }
        };
        fetchData();
    }, [embargo, conceptosCuenta]);

    const obtenerTipoActorEmbargo = async () => {
        await apiActor
            .obtenerTipoActorEmbargo()
            .then(async response => {
                if (response.isSuccess) {
                    const datos = response.data;
                    setAllTipoActor(datos);
                } else {
                    setAllTipoActor([]);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };
    const formularioBusquedaDesembargos = useForm({
        defaultValues: desembargoForm,
    });

    const calcularEmbargos = async (conceptosCuenta: any, conceptosEmbargo: any) => {
      
        let total = 0;
        const conceptosCuentaActualizados = conceptosCuenta?.map((cuenta:any) => {
            const embargo = conceptosEmbargo?.find((e:any) => e.conceptoId === cuenta.id);

            if (!embargo) return { ...cuenta, valorEmbargo: 0 };

            let valorCalculado = 0;
            if (embargo.tipoEmbargo === "PORCENTAJE") {
                valorCalculado = Number(cuenta.saldo) * (Number(embargo.valor) / 100);
            } else if (embargo.tipoEmbargo === "VALOR") {
                valorCalculado = Number(embargo.valor);
            }
            total += valorCalculado;
            return { ...cuenta, valorEmbargo: valorCalculado };
        });

        setTotalEmbargo(total);

        return {
            totalEmbargo: total,
            conceptosCuentaActualizados
        };
    };


    if (!hasLoadedFields && embargo != null && embargo[0] && embargo[0].embargo) {
        setHasLoadedFields(true);
        setIdTipoIdentificacion(embargo[0].embargo.tipoIdentificacionId);
        setIdentificacion(embargo[0].embargo.identificacionCliente);
        setNombreDemandado(embargo[0].embargo.nombreDemandado);
        setNumeroRadicado(embargo[0].embargo.radicadoWorkManager);
        setNumeroOficio(embargo[0].embargo.radicadoJuzgado);
        setEstadosEmbargo(embargo[0].embargo.estadoEmbargo);
        formularioBusquedaDesembargos.setValue("observaciones", embargo[0].embargo.observaciones)
    }

    const handleOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleOnClickRegresar = async () => history.push('/DesembargoInicio');

    const [isChecked, setIsChecked] = useState(false);



    const [desembargosLista, setDesembargosLista] = useState<any[]>([]);
    const buildDesembargosLista = (checked: boolean, data: any) => {
        if (checked) {
            setDesembargosLista([...desembargos as any[], data]);
        } else {
            setDesembargosLista(desembargosLista?.filter(x => x.id !== data.id));
        }
    };

    const [dataConceptos, setDataConceptos] = useState<any[]>([]);

    const onChangeCheck = (e: any, data: any) => {
        buildDesembargosLista(e.checked, data);
        setOpen(true);
        setDataConceptos(data.conceptos);
    };

    const [modalOpen, setModalOpen] = useState(false);
    const [documentoURL, setDocumentoURL] = useState('');
    const [resolver, setResolver] = useState<((value: boolean | null) => void) | null>(null);

    const verificarYNotificarRemanente = async () => {
        try {
            if (!embargo || embargo.length === 0 || !embargo[0]?.embargo?.identificacionCliente) return;

            const respo = await api.getObtenerEmbargosByIdentificacion(embargo[0].embargo.identificacionCliente);
            if (!respo || !respo.data || respo.data.length === 0) return;

            respo.data.forEach((item: { embargo: any; }) => {
                const emb = item.embargo;
                if (emb?.remanente === true) {

                    toast(`El embargo en estado remanente con radicado ${emb.radicadoWorkManager} fue procesado exitosamente.`);
                }
            });

        } catch (error) {
            console.error('Error al verificar el remanente del embargo:', error);
            toast.error('Ocurrió un error al verificar el estado del embargo.');
        }
    };


    const handleRespuesta = async () => {
        try {
            setLoading(true);
            const response = await api.postObtenerRespuesta(embargo[0].embargo.id);
            const mimeType = response.type;
            if (mimeType) {
                const file = new Blob([response], { type: mimeType });
                const fileURL = URL.createObjectURL(file);
                setDocumentoURL(fileURL);
                await new Promise<boolean | null>((resolve) => {
                    setResolver(() => resolve);
                    setModalOpen(true);
                });
                await handleOnClickRegresar();
                setLoading(true);
            }
        } catch (error) {
            toast('Error al generar respuesta');
            setLoading(true);
        }
    };

    const handleOnClickRespuesta = async (conceptos: any) => {
        try {

            const response = await api.postObtenerRespuesta(embargo[0].embargo.id);
            const mimeType = response.type;
            if (mimeType) {
                const file = new Blob([response], { type: mimeType });
                const fileURL = URL.createObjectURL(file);
                setDocumentoURL(fileURL);

                const estadosValue = await new Promise<boolean | null>((resolve) => {
                    setModalOpen(true);
                    setResolver(() => resolve);
                });
                if (estadosValue === true) {
                    try {
                        const response = await api.deleteEmbargoConcepto(conceptos);
                        if (response?.isSuccess === false) {
                            throw new Error(response.returnMessage);
                        } else {
                            await verificarYNotificarRemanente();
                            swal({
                                icon: 'success',
                                text: `El valor retenido ha sido desembargado con éxito.`,
                            });
                        }
                    } catch (error) {
                        console.error(`Error al eliminar el embargo ${conceptos.id}`, error);
                    }
                }
            }
        } catch (error) {
            toast('Error al generar respuesta');
        }
    };

    const handleCloseModalRespuesta = async () => {
        setModalOpen(false);
        setDocumentoURL('');
        await handleOnClickRegresar();
        setLoading(false);
    };

    const onCloseModalRespuesta = async () => {
        setModalOpen(false);
        setDocumentoURL('');
        setLoading(false);
    };

    const [selectedConceptoData, setSelectedConceptoData] = useState<any>(null);

    const handleConceptos = async (conceptosSeleccionados: any[]) => {
        setSelectedConceptoData(conceptosSeleccionados);
    };

    const handleProcesarClick = async () => {
        setLoading(true);
        if (selectedConceptoData && formularioBusquedaDesembargos.getValues("observaciones")) {
            const conceptos = {
                id: selectedConceptoData,
                descripcion: formularioBusquedaDesembargos.getValues("observaciones"),
            };
            await handleOnClickRespuesta(conceptos);

        } else {
            toast('Agregar una observacion y seleccionar los conceptos.');
        }

        setLoading(false);
    };


    const obtenerUrlDodo = async (radicado: string) => {

        if (radicado === '') {
            toast('Debe diligenciar el Número de Radicado', { type: toast.TYPE.ERROR });
            return;
        }

        if (radicado) {
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
            <CHPaper title="Información de solicitud">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoIdentificacionId"
                            control={formularioDesembargos.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    defaultValue=""
                                    label="Tipo de Identificación *"
                                    value={catalogState.IdentificationType.find((x: any) => x.id === idTipoIdentificacion)?.descripcion}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="identificacionCliente"
                            control={formularioBusquedaDesembargos.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Número de Identificación *"
                                    value={identificacion}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreDemandado"
                            control={formularioBusquedaDesembargos.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Nombres y Apellidos"
                                    value={nombreDemandado}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={2}>
                        <Controller
                            name="radicadoWorkManager"
                            control={formularioBusquedaDesembargos.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    label="Número Radicado Asociado"
                                    value={numeroRadicado}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item className={classes.iconoDodoRadicado}>
                        <a href={linkDodo || undefined} className={classes.enlaceDodo} target="_blank" rel="noopener noreferrer">
                            <AttachFileIcon />
                        </a>
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaRadicadoWorkManager"
                            control={formularioDesembargos.control}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Fecha Radicado"
                                    onChange={onChange}
                                    value={UtilService.getDate(fechaRadicadoWorkManager)}
                                    inputProps={{ readOnly: true }}
                                    isReadOnly
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="radicadoJuzgado"
                            control={formularioBusquedaDesembargos.control}
                            render={({
                                field: { onChange },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    label="Número Oficio Juzgado"
                                    value={numeroOficio}
                                    onChange={onChange}
                                    type="number"
                                />
                            )}
                        />

                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaRadicadoJuzgado"
                            control={formularioBusquedaDesembargos.control}
                            render={({ field: { onChange } }) => (
                                <CHTextField
                                    label="Fecha Oficio Juzgado"
                                    inputProps={{ readOnly: true }}
                                    onChange={onChange}
                                    value={UtilService.getDate(fechaRadicadoJuzgado)}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
            <CHPaper title="Observaciones">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="observaciones"
                            control={formularioBusquedaDesembargos.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo no es requerido',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    multiline
                                    maxRows={3}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="Observaciones *"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>

            <InfoDesembargo
                data={embargo}
                onChangeCheck={onChangeCheck}
                cuentas={cuentas as any[]}
                openModal={handleOpen}
                embargo={embargo}
                allTipoActor={allTipoActor}
                conceptos={conceptosCuenta}
                totalDesembargo={totalDesembargo}
                totalEmbargo={totalEmbargo}
                totalProcesado={totalProcesado}
            />

            <ModalArchivoRespuesta
                open={modalOpen}
                documentoURL={documentoURL}
                handleClose={handleCloseModalRespuesta}
                onClose={onCloseModalRespuesta}
                resolver={resolver}
                estado={estadosEmbargo}
                modulo='DESEMBARGO'
                moduloPago= ""
                setVista = {()=> ''}
            />
            <LoadingComponent loading={loading} />
            <Grid container>
                <Grid className={classes.buttonContainer}>

                    <Button
                        className={classes.button}
                        variant="contained"
                        color="default"
                        onClick={
                            estadosEmbargo !== "398"
                                ? handleProcesarClick
                                : handleRespuesta
                        }
                    >
                        {estadosEmbargo !== "398" ? 'GUARDAR' : 'VER RESPUESTA'}
                    </Button>


                    <Button
                        className={`${classes.button} ${classes.regresarButton}`}
                        variant="contained"
                        color="default"
                        onClick={handleOnClickRegresar}
                    >
                        REGRESAR
                    </Button>
                    <ConceptosEmbargoModal
                        open={open}
                        handleClose={handleClose}
                        data={dataConceptos}
                        conceptos={conceptosCuenta}
                        cuentas={cuentas}
                        embargo={embargo}
                        tipoDocumentoId={undefined}
                        numeroDocumento={undefined}
                        onSelectedConceptoChange={handleConceptos}
                        onProcesarTotal={(total: number) => setTotalProcesado(total)}
                        estadoEmbargo={estadoEmbargo}
                        datoEstado={datoEstado}
                    />
                </Grid>
            </Grid>
        </>
    );
};




const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
});


const mapDispatchToProps = {
    startGetAllIdentificationTypes,
};




export default connect(mapStateToProps, mapDispatchToProps)(InformacionDesembargos);