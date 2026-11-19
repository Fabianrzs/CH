import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { debounce, sortBy } from 'lodash';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { useForm } from 'react-hook-form';
import { AppState } from '../../store/rootReducer';
import history from '../../utils/history.router';
import { EmbargosState } from '../../reducers/embargos.reducer';
import {
    startGetTiposEmbargo,
    startGetTiposProcesoEmbargo,
    startGetTiposRetencion,
} from '../../actions/embargos.action';
import { startGetAllTypeDocuments } from '../../actions/catalog.action';
import { CatalogState } from '../../reducers/catalog.reducer';
import {
    BeneficiarioDto,
    ConceptoEmbargosDto,
    CuentaModelDto,
    EmbargoDto,
    EmbargoModelDto,
} from '../../types/embargo.model.type';
import apiEmbargos from '../../services/api-embargos.service';
import InformacionPagoEmbargos from './InformacionPagoEmbargos';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import { LoadingComponent } from '../../components/loading/loading.component';
import apiCourt from '../../services/api-courts.service';
import { JuzgadoDto } from '../../types/court.model.type';
import { CITYDTO, DEPARTAMENTODTO } from '../../types/catalog.type';
import apiCatalogs from '../../services/api-catalog.service';
import NotificationService from '../../services/notification.service';
import apiClient from '../../services/api-clients.service';
import { ModalAgregarConceptos } from '../embargos/components/ModalAgregarConceptos';
import UtilService from '../../utils/util.service';
import {
    convertEmbargoModelInsert,
    convertEmbargoModelUpdate,
} from '../embargos/constants/conversionesEmbargo';
import { conceptoEmbargoForm, embargoForm } from '../embargos/constants/embargosform.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '70%',
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
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
        updateButton: {
            marginLeft: '55%',
        },
        insertButton: {
            marginLeft: '70%',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
    }),
);

const PagoEmbargos = (props: props) => {
    const classes = useStyles();
    const apiEmbargo = apiEmbargos();
    const api = apiCourt();
    const apiCliente = apiClient();
    const apiCatalog = apiCatalogs();

    const {
        embargoState,
        catalogState,
        modoActualizar,
        embargoModel,
        setModoActualizar,
        startGetTiposEmbargo,
        startGetTiposProcesoEmbargo,
        startGetAllTypeDocuments,
        startGetTiposRetencion,
        handleOnClickCancelarEditar,
        setVista
    } = props;
    const [mostrarPorcentaje, setMostrarPorcentaje] = useState(false);
    const [departaments, setDepartaments] = useState<DEPARTAMENTODTO[]>([]);
    const [cities, setCities] = useState<CITYDTO[]>([]);
    const [juzgados, setJuzgados] = useState<JuzgadoDto[]>([]);
    const [beneficiarios, setBeneficiarios] = useState<BeneficiarioDto[]>([]);
    const [openModalAgregarConcepto, setOpenModalAgregarConcepto] =
        useState(false);
    const [listaConceptos, setListaConceptos] = useState<ConceptoEmbargosDto[]>(
        [],
    );
    const [conceptos, setConceptos] = useState<ConceptoEmbargosDto[]>([]);
    const [conceptoId, setConceptoId] = useState(0);
    const porcentaje = 'ae7d141a-fd88-4f57-a199-4db5b5301946';
    const variacionPorcentaje = 'f65c4c4a-174e-4406-8aed-32b5b47cf0b3';
    const valor = '95ca1858-b89d-4631-9893-22932c925184';
    const [loading, setLoading] = useState(false);
    const [cuentas, setCuentas] = useState<CuentaModelDto[]>([]);
    const [tipoDocumento, setTipoDocumento] = useState(0);
    const [documento, setDocumento] = useState('');



    useEffect(() => {
        consultarJuzgado(
            embargoModel.embargo?.juzgadoId,
            embargoModel.embargo?.expedienteBancoAgrario
        );
        startGetAllTypeDocuments();
        obtenerDepartamentos();
        loadTiposEmbargo();
        loadTiposProcesosEmbargo();
        loadTiposRetencion();
    }, []);

    useEffect(() => {


        CargarFormularios();

    }, []);

    const CargarFormularios = async () => {

        if (modoActualizar) {
            setBeneficiarios(embargoModel?.beneficiarios);
            formularioEmbargos.setValue('numeroSolicitud', embargoModel.embargo?.numeroSolicitud);
            formularioEmbargos.setValue('id', embargoModel.embargo?.id);
            formularioEmbargos.setValue(
                'tipoIdentificacion',
                embargoModel.embargo?.tipoIdentificacionId,
            );
            formularioEmbargos.setValue(
                'identificacionCliente',
                embargoModel.embargo?.identificacionCliente,
            );
            formularioEmbargos.setValue(
                'nombreCliente',
                embargoModel.embargo?.nombreDemandado,
            );
            formularioEmbargos.setValue(
                'radicadoWorkManager',
                embargoModel.embargo?.radicadoWorkManager,
            );
            formularioEmbargos.setValue(
                'fechaRadicadoWorkManager',
                embargoModel.embargo?.fechaRadicadoWorkManager,
            );
            formularioEmbargos.setValue(
                'radicadoJuzgado',
                embargoModel.embargo?.radicadoJuzgado,
            );
            formularioEmbargos.setValue(
                'fechaRadicadoJuzgado',
                embargoModel.embargo?.fechaRadicadoJuzgado,
            );
            formularioEmbargos.setValue(
                'emailRespuesta',
                embargoModel.embargo?.emailRespuesta,
            );
            formularioEmbargos.setValue(
                'expedienteBancoAgrario',
                embargoModel.embargo?.expedienteBancoAgrario,
            );
            formularioEmbargos.setValue(
                'procesoId',
                embargoModel.embargo?.procesoId,
            );
            formularioEmbargos.setValue(
                'tipoEmbargoId',
                embargoModel.embargo?.tipoEmbargoId,
            );
            formularioEmbargos.setValue(
                'remanente',
                embargoModel.embargo?.remanente,
            );
            formularioEmbargos.setValue(
                'observaciones',
                embargoModel.embargo?.observaciones,
            );
            const idJuzgado =
                embargoModel.embargo?.juzgadoId.toUpperCase() as string;
            obtenerJuzgadoById(idJuzgado);
            consultarCuentaConceptoParam(
                embargoModel.embargo?.tipoIdentificacionId,
                embargoModel.embargo?.identificacionCliente,
            );
            setConceptos(embargoModel?.conceptos);

        }
    }

    const formularioEmbargos = useForm<EmbargoDto>({
        defaultValues: embargoForm,
    });

    const formularioBeneficiarios = useForm({
        defaultValues: {
            id: '',
            tipoDocumentoId: '',
            tipoIdentificacion: '',
            numeroIdentificacion: '',
            primerNombre: '',
            segundoNombre: '',
            primerApellido: '',
            segundoApellido: '',
            demandante: false,
            nombreRazonSocial: '',
        },
    });

    const formularioConceptos = useForm<ConceptoEmbargosDto>({
        defaultValues: conceptoEmbargoForm,
    });

    const handleOpenModal = (conceptoId: number) => {
        const index = listaConceptos.findIndex(
            x => x.conceptoId === conceptoId,
        );
        formularioConceptos.reset();

        if (index !== -1) {
            formularioConceptos.setValue(
                'tipoEmbargoId',
                listaConceptos[index].tipoEmbargoId,
            );
            formularioConceptos.setValue('valor', listaConceptos[index].valor);
            formularioConceptos.setValue(
                'fechaInicio',
                listaConceptos[index].fechaInicio,
            );
            formularioConceptos.setValue(
                'tipoRetencionId',
                listaConceptos[index].tipoRetencionId,
            );
            formularioConceptos.setValue(
                'id',
                listaConceptos[index].id,
            )
        }

        setConceptoId(conceptoId);
        setOpenModalAgregarConcepto(true);
    };

    const handleCloseModal = () => {

        setOpenModalAgregarConcepto(false);
    };

    const loadTiposEmbargo = async () => {
        await startGetTiposEmbargo();
    };

    const loadTiposProcesosEmbargo = async () => {
        await startGetTiposProcesoEmbargo();
    };

    const loadTiposRetencion = async () => {
        await startGetTiposRetencion();
    };

    const consultarCuentaConcepto = async () => {
        await apiCliente
            .getCuentasConceptosPorTipoNumeroIdentificacion(
                tipoDocumento,
                documento,
            )
            .then(async response => {
                if (response) {
                    const array = response.data?.cuentas;
                    if (response.isSuccess && array) {
                        setCuentas(array);

                        formularioEmbargos.setValue(
                            'nombreCliente',
                            `${response.data?.primerNombre} ${response.data?.segundoNombre} ${response.data?.primerApellido} ${response.data?.segundoApellido}`,
                        );
                    }
                }
            })
            .catch(() => {
                toast('Error al obtener las cuentas');
            });
    };

    const consultarCuentaConceptoParam = async (
        tipoDocumento: number,
        documento: string,
    ) => {
        await apiCliente
            .getCuentasConceptosPorTipoNumeroIdentificacion(
                tipoDocumento,
                documento,
            )
            .then(async response => {
                if (response) {
                    const array = response.data?.cuentas;
                    if (response.isSuccess && array) {
                        setCuentas(array);
                    }
                }
            })
            .catch(() => {
                toast('Error al obtener las cuentas');
            });
    };
    const [dataJuzgado, setDataJuzgado] = useState<JuzgadoDto>();

    const consultarJuzgado = async (idJuzgado: string, expediente: string) => {
        await api
            .getCourtById(idJuzgado)
            .then(async response => {
                setDataJuzgado(response.data);
                if (response) {
                    if (response.isSuccess) {
                        const value = response.data as JuzgadoDto;
                        formularioEmbargos.setValue(
                            'codigoInterno',
                            value?.codigoInterno,
                        );
                        formularioEmbargos.setValue(
                            'codigoOficina',
                            Number(value?.codigoOficina),
                        );
                        formularioEmbargos.setValue(
                            'numeroCuenta',
                            Number(value?.nroCuenta),
                        );
                        if (value?.correoElectronico !== null) {
                            formularioEmbargos.setValue(
                                'emailRespuesta',
                                value?.correoElectronico,
                            );
                        }
                        if (expediente !== '') {
                            formularioEmbargos.setValue(
                                'proceso',
                                `${value?.codigoInterno}${expediente}`,
                            );
                        }
                        if (
                            response.isSuccess &&
                            value.idCiudad &&
                            value.idDepartamento
                        ) {
                            formularioEmbargos.setValue(
                                'idDepartamento',
                                value?.idDepartamento.toString(),
                            );
                            obtenerCiudades(value?.idDepartamento);
                            formularioEmbargos.setValue(
                                'idCiudad',
                                value?.idCiudad.toString(),
                            );
                        }
                        else {
                            formularioEmbargos.setValue(
                                'proceso',
                                value?.codigoInterno,
                            );
                        }
                    }
                }
            })
            .catch(() => {
                toast('Error al obtener juzgado');
            });
    };

    const obtenerDepartamentos = async () => {
        const response = await apiCatalog.getAllStates();
        if (response.data !== null) {
            const datos = (await response.data) as DEPARTAMENTODTO[];
            setDepartaments(datos);
        }
    };

    const obtenerCiudades = async (idDepto: number) => {
        const value = idDepto.toString();
        const response = await apiCatalog.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCities(datos);
        }
    };

    const obtenerJuzgadoById = async (idJuzgado: string) => {
        await api
            .getCourtById(idJuzgado)
            .then(async response => {
                if (response) {
                    const value = response.data as JuzgadoDto;
                    setJuzgados([value]);
                    if (
                        response.isSuccess &&
                        value.idCiudad &&
                        value.idDepartamento
                    ) {
                        formularioEmbargos.setValue(
                            'idDepartamento',
                            value?.idDepartamento.toString(),
                        );
                        obtenerCiudades(value?.idDepartamento);
                        formularioEmbargos.setValue(
                            'idCiudad',
                            value?.idCiudad.toString(),
                        );
                        obtenerJuzgados(value?.idDepartamento, value?.idCiudad);
                        formularioEmbargos.setValue(
                            'juzgadoId',
                            value?.idJuzgado,
                        );
                    }
                }
            })
            .catch(() => {
                toast('Por favor, llenar todos los campos obligatorios');
            });
    };

    const obtenerJuzgados = async (idDepto: number, idCiudad: number) => {
        const response = await api.postAllCourtsByCity(idDepto, idCiudad);
        if (response.data !== null) {
            const datos = response.data as JuzgadoDto[];
            const datosOrdenados = sortBy(datos, 'nombre');
            if (juzgados.length === 0) {
                setJuzgados((juzgados) => [...juzgados, ...datosOrdenados]);
            } else {
                setJuzgados([]);
                setJuzgados(datosOrdenados);

            }
        }
    };

    const handleOnchangeModo = (value: boolean) => setModoActualizar(value);

    const [valorTotal, setValorTotal] = useState(0);

    const agregarConcepto = (data: any) => {
        const array = listaConceptos.map(item => {
            if (item.conceptoId === conceptoId) {
                const newItem = {
                    ...item,
                    tipoEmbargoId: data.tipoEmbargoId,
                    valor: UtilService.cleanCurrencyValue(data.valor),
                    fechaInicio: data.fechaInicio,
                    fechaFin: data.fechaFin,
                    tipoRetencionId: data.tipoRetencionId,
                    indicadorCesantias: data.indicadorCesantias,
                };
                if (conceptos.findIndex(x => data.id === x.id) !== -1) {
                    newItem.id = data.id
                    setConceptos(conceptos?.map(x => data.id === x.id ? newItem : x));
                } else {
                    setConceptos([...conceptos, newItem]);
                }

                setValorTotal(valorTotal + newItem.valor)
                return newItem;
            }
            return item;
        });
        setListaConceptos(array as ConceptoEmbargosDto[]);
        handleCloseModal();
    };

    const handleOnChangeMostrarPorcentaje = (value: boolean) =>
        setMostrarPorcentaje(value);

    const insertarEmbargo = (embargo: EmbargoModelDto) =>
        apiEmbargo.postInsertEmbargo(embargo);

    const actualizarEmbargo = (embargo: EmbargoModelDto) =>
        apiEmbargo.postUpdateEmbargo(embargo);


    const handleOnClick = async (embargo: EmbargoDto) => {
        setLoading(true);
        let numeroDemandantes = 0;
        beneficiarios.forEach(item => {
            if (item.demandante) {
                numeroDemandantes += 1;
            }
        });
        if (
            embargo?.clienteTipoIdentificacion === 0 ||
            embargo?.identificacionCliente.trim() === '' ||
            embargo?.radicadoWorkManager.trim() === '' ||
            embargo?.radicadoJuzgado.trim() === '' ||
            embargo?.emailRespuesta.trim() === '' ||
            embargo?.expedienteBancoAgrario.trim() === ''
        ) {
            setLoading(false);
            swal({
                text: 'Deben diligenciarse todos los campos',
                icon: 'warning',
            });
        } else if (Number(embargo?.identificacionCliente) < 0) {
            toast('El número de identificación no puede ser negativo.');
        } else if (embargo?.radicadoWorkManager === '') {
            toast('El Radicado Work Manager es un campo requerido.');
        } else if (embargo?.radicadoJuzgado === '') {
            toast('El Radicado del Juzgado es un campo requerido.');
        } else if (embargo?.emailRespuesta === '') {
            toast('El email de respuesta es un campo requerido');
        } else if (embargo?.expedienteBancoAgrario === '') {
            toast('El nombre del demandante es un campo requerido');
        } else if (embargo?.procesoId === '') {
            toast('El tipo de proceso es un campo requerido');
            setLoading(false);
        } else if (embargo?.identificacionCliente.trim().length < 4) {
            toast('El número de identificación debe ser menor a 4 caracteres.');
        } else if (embargo?.identificacionCliente.trim().length < 4) {
            toast('El número de identificación debe ser menor a 4 caracteres.');
        } else if (beneficiarios.length === 0) {
            toast('No existen demandantes y beneficiarios registrados');
            setLoading(false);
        } else if (conceptos.length === 0) {
            toast('No existen conceptos embargados registrados');
            setLoading(false);
        } else if (numeroDemandantes === 0) {
            toast('Debe registrarse un demandante');
            setLoading(false);
        } else if (numeroDemandantes > 1) {
            toast('Solo debe registrarse un demandante');
            setLoading(false);
        } else if (!modoActualizar) {
            const respuesta = await insertarEmbargo(
                convertEmbargoModelInsert(embargo, beneficiarios, conceptos),
            );
            if (respuesta?.isSuccess === false) {
                swal({
                    text: respuesta.returnMessage,
                    icon: 'warning',
                });
                await setLoading(false);
            } else {
                formularioEmbargos.reset();
                formularioBeneficiarios.reset();
                formularioConceptos.reset();
                await setLoading(false);
                await NotificationService.genericAlert(
                    'Guardado',
                    'Se ha guardado correctamente la información',
                );
                history.push('/EmbargosInicio');
            }
        } else {
            const respuesta = await actualizarEmbargo(
                convertEmbargoModelUpdate(embargo, beneficiarios, conceptos),
            );
            if (respuesta?.isSuccess === false) {
                swal({
                    text: respuesta.returnMessage,
                    icon: 'warning',
                });
                await setLoading(false);
            } else {
                await handleOnchangeModo(false);
                formularioEmbargos.reset();
                formularioBeneficiarios.reset();
                formularioConceptos.reset();
                await setLoading(false);
                await NotificationService.genericAlert(
                    'Actualizado con éxito',
                    'Se realizó con éxito la actualización.',
                );
                handleOnClickCancelarEditar();
            }
        }
    };

    const handleOnClickRegresar = () => history.push('/EmbargosInicio');

    const [estadoCuentaEmbargo, setEstadoCuentaEmbargo] = useState();
    const [concepto, setConcepto] = useState([]);

    const consultarCliente = debounce(async (idTipoIdentificacion, identificacion) => {
        if (!idTipoIdentificacion || !identificacion) return;
    
        try {
            const response = await apiCliente.getClientByIdentificationTypeIdentificationNumberRequest(
                idTipoIdentificacion,
                identificacion
            );
            if (response.data) {
                formularioEmbargos.setValue(
                    'clienteNombre',
                    `${response.data.PrimerNombre} ${response.data.SegundoNombre} ${response.data.PrimerApellido} ${response.data.SegundoApellido}`
                );

                setEstadoCuentaEmbargo(response.data.cuentas);
            }    
           
        } catch (error) {
            toast('Error al obtener cliente');
        }
        await apiEmbargo.getObtenerEmbargosByIdentificacion(identificacion)
                .then(async (response) => {
                    if (response.data != null) {
                        const allConceptos = [];
                        response.data.forEach((item) => {
                            if (item.embargo && item.embargo.remanente === false) {
                                allConceptos.push(...(item.conceptos || []));
                            }
                        });
                        setConcepto(allConceptos);
                    }
                })
                .catch(() => {
                    toast('Error al consultar los embargos.');
                });

            setLoading(false);
    }, 500);


    return (
        <CHPrivateContainer>
            <InformacionPagoEmbargos
                formularioEmbargos={formularioEmbargos}
                formularioBeneficiarios={formularioBeneficiarios}
                beneficiarios={beneficiarios}
                tiposIdentificacion={catalogState.IdentificationType}
                departamentos={departaments}
                tiposProcesos={embargoState.tiposProcesos}
                cities={cities}
                juzgados={juzgados}
                datajuzgado={dataJuzgado}
                modoActualizar={modoActualizar}
                listaConceptos={listaConceptos}
                tiposEmbargos={embargoState.tiposEmbargo}
                tiposRetencion={embargoState.tiposRetencion}
                beneficiariosData={embargoModel?.beneficiarios}
                conceptosData={embargoModel?.conceptos}
                cuentas={cuentas}
                tipoDocumento={tipoDocumento}
                obtenerCiudades={obtenerCiudades}
                obtenerJuzgados={obtenerJuzgados}
                setBeneficiarios={setBeneficiarios}
                handleOpenModal={handleOpenModal}
                setListaConceptos={setListaConceptos}
                setTipoDocumento={setTipoDocumento}
                setDocumento={setDocumento}
                consultarCuentaConcepto={consultarCuentaConcepto}
                porcentaje={porcentaje}
                variacionPorcentaje={variacionPorcentaje}
                handleOnChangeMostrarPorcentaje={
                    handleOnChangeMostrarPorcentaje
                }
                consultarJuzgado={consultarJuzgado}
                consultarCliente={consultarCliente}
                setCuentas={setCuentas}
                embargoModel={embargoModel}
                handleCloseModal={handleCloseModal}
                estadoCuentaEmbargo={estadoCuentaEmbargo}
                setConcepto={concepto}
                setVista={setVista}
            />


            <Grid container>
                <Grid
                    className={classes.columnButtonAdd}
                    item
                    xs={12}
                    md={12}
                >

                    <Button
                        className={classes.button}
                        variant="contained"
                        color="default"
                        style={{ width: '200px' }}
                        onClick={
                            modoActualizar
                                ? handleOnClickCancelarEditar
                                : handleOnClickRegresar
                        }

                    >
                        REGRESAR
                    </Button>

                </Grid>
            </Grid>
            <br />
            <LoadingComponent loading={loading} />
            <ModalAgregarConceptos
                open={openModalAgregarConcepto}
                handleClose={handleCloseModal}
                formularioConceptos={formularioConceptos}
                tiposEmbargos={embargoState.tiposEmbargo}
                tiposRetencion={embargoState.tiposRetencion}
                agregarConcepto={agregarConcepto}
                mostrarPorcentaje={mostrarPorcentaje}
                handleOnChangeMostrarPorcentaje={
                    handleOnChangeMostrarPorcentaje
                }
                porcentaje={porcentaje}
                variacionPorcentaje={variacionPorcentaje}
                valor={valor}
                modoActualizar={modoActualizar}
            />
        </CHPrivateContainer>
    );
}

interface LinkMapStateToProps {
    embargoState: EmbargosState;
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    embargoState: state.embargo,
    catalogState: state.catalog,
});
interface LinkMapDispatchToProps {
    startGetTiposEmbargo: () => void;
    startGetTiposProcesoEmbargo: () => void;
    startGetAllTypeDocuments: () => void;
    modoActualizar: boolean;
    setModoActualizar: (value: boolean) => void;
    embargoModel: any;
    startGetTiposRetencion: () => void;
    handleOnClickCancelarEditar: () => void;
    setVista: () => void;
}

const mapDispatchToProps = {
    startGetTiposEmbargo,
    startGetTiposProcesoEmbargo,
    startGetAllTypeDocuments,
    startGetTiposRetencion,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(PagoEmbargos);


