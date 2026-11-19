import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { sortBy } from 'lodash';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import history from '../../utils/history.router';
import { AppState } from '../../store/rootReducer';
import { desembargoForm, conceptoDesembargoForm } from '../desembargosConceptos/constants/desembargosform.model.type';
import { CatalogState } from '../../reducers/catalog.reducer';
import { startGetAllTypeDocuments } from '../../actions/catalog.action';
import { startGetTiposDesembargo, startGetTiposProcesoDesembargo } from '../../actions/desembargos.action';
import ApiDesembargo from '../../services/api-desembargos.service';
import apiCourt from '../../services/api-courts.service';
import apiClient from '../../services/api-clients.service';
import apiCatalogs from '../../services/api-catalog.service';
import { DesembargoDto, ObtenerDesembargoDto, ConceptoDesembargosDto, CuentaModelDto, DesembargoModelDto } from '../../types/desembargo.model.type';
import { DesembargosState } from '../../reducers/desembargos.reducer';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import {
    CITYDTO,
    DEPARTAMENTODTO,
    IDENTIFICATIONTYPEDTO,
} from '../../types/catalog.type';
import {
    startGetTiposRetencion,
} from '../../actions/embargos.action';
import { JuzgadoDto } from '../../types/court.model.type';
import UtilService from '../../utils/util.service';
import NotificationService from '../../services/notification.service';
import {
    BeneficiarioDto,
    EmbargoDto,
    EmbargoModelDto,
} from '../../types/embargo.model.type';
import { embargoForm, conceptoEmbargoForm } from '../embargos/constants/embargosform.model.type';
import apiEmbargos from '../../services/api-embargos.service';
import { convertEmbargoModelInsert, convertEmbargoModelUpdate } from '../embargos/constants/conversionesEmbargo';
import InformacionDesembargos from './InformacionDesembargo';
import InformacionEmbargos from '../embargos/InformacionEmbargos';
import { EmbargosState } from '../../reducers/embargos.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';
import { ModalAgregarConceptos } from '../embargos/components/ModalAgregarConceptos';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
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
    }),
);

const Desembargo = (props: props) => {

    const classes = useStyles();
    const apiDesembargo = ApiDesembargo();
    const apiEmbargo = apiEmbargos();
    const api = apiCourt();
    const apiCliente = apiClient();
    const apiCatalog = apiCatalogs();

    const {
        desembargoState,
        embargoState,
        catalogState,
        modoActualizar,
        desembargoModel,
        embargoModel,
        setModoActualizar,
        startGetTiposEmbargo,
        startGetTiposProcesoEmbargo,
        startGetTiposDesembargo,
        startGetTiposProcesoDesembargo,
        startGetAllTypeDocuments,
        startGetTiposRetencion,
        handleOnClickCancelarEditar,
    } = props;

    const [mostrarPorcentaje, setMostrarPorcentaje] = useState(false);
    const [departaments, setDepartaments] = useState<DEPARTAMENTODTO[]>([]);
    const [cities, setCities] = useState<CITYDTO[]>([]);
    const [juzgados, setJuzgados] = useState<JuzgadoDto[]>([]);
    const [beneficiarios, setBeneficiarios] = useState<BeneficiarioDto[]>([]);
    const [openModalAgregarConcepto, setOpenModalAgregarConcepto] =
        useState(false);
    const [listaConceptos, setListaConceptos] = useState<ConceptoDesembargosDto[]>(
        [],
    );
    const [conceptos, setConceptos] = useState<ConceptoDesembargosDto[]>([]);
    const [conceptoId, setConceptoId] = useState(0);
    const porcentaje = 'ae7d141a-fd88-4f57-a199-4db5b5301946';
    const variacionPorcentaje = 'f65c4c4a-174e-4406-8aed-32b5b47cf0b3';
    const valor = '95ca1858-b89d-4631-9893-22932c925184';
    const [loading, setLoading] = useState(false);
    const [cuentas, setCuentas] = useState<CuentaModelDto[]>([]);
    const [tipoDocumento, setTipoDocumento] = useState(0);
    const [documento, setDocumento] = useState('');

    useEffect(() => {
        startGetAllTypeDocuments();
        obtenerDepartamentos();
        loadTiposEmbargo();
        loadTiposProcesosEmbargo();
        loadTiposRetencion();
    }, []);

    useEffect(() => {
        if (modoActualizar) {
            formularioEmbargos.setValue('id', desembargoModel.desembargo?.id);
            formularioEmbargos.setValue(
                'tipoIdentificacion',
                desembargoModel.desembargo?.tipoIdentificacionId,
            );
            formularioEmbargos.setValue(
                'identificacionCliente',
                desembargoModel.desembargo?.identificacionCliente,
            );
            formularioEmbargos.setValue(
                'nombreDemandado',
                desembargoModel.desembargo?.nombreDemandado,
            );
            formularioEmbargos.setValue(
                'radicadoWorkManager',
                desembargoModel.desembargo?.radicadoWorkManager,
            );
            formularioEmbargos.setValue(
                'fechaRadicadoWorkManager',
                desembargoModel.desembargo?.fechaRadicadoWorkManager,
            );
            formularioEmbargos.setValue(
                'radicadoJuzgado',
                desembargoModel.desembargo?.radicadoJuzgado,
            );
            formularioEmbargos.setValue(
                'fechaRadicadoJuzgado',
                desembargoModel.desembargo?.fechaRadicadoJuzgado,
            );
            formularioEmbargos.setValue(
                'emailRespuesta',
                desembargoModel.desembargo?.emailRespuesta,
            );
            formularioEmbargos.setValue(
                'expedienteBancoAgrario',
                desembargoModel.desembargo?.expedienteBancoAgrario,
            );
            formularioEmbargos.setValue(
                'procesoId',
                desembargoModel.desembargo?.procesoId,
            );
            formularioEmbargos.setValue(
                'tipoEmbargoId',
                desembargoModel.desembargo?.tipoEmbargoId,
            );
            formularioEmbargos.setValue(
                'remanente',
                desembargoModel.desembargo?.remanente,
            );
            formularioEmbargos.setValue(
                'observaciones',
                desembargoModel.desembargo?.observaciones,
            );
            const idJuzgado =
                desembargoModel.desembargo?.juzgadoId.toUpperCase() as string;
            obtenerJuzgadoById(idJuzgado);
            setBeneficiarios(desembargoModel?.beneficiarios);
        
            consultarCuentaConceptoParam(
                desembargoModel.desembargo?.tipoIdentificacionId,
                desembargoModel.desembargo?.identificacionCliente,
            );
            setConceptos(desembargoModel?.conceptos);
            consultarJuzgado(
                idJuzgado,
                desembargoModel.desembargo?.expedienteBancoAgrario,
            );
        }
    }, []);

    const formularioDesembargos = useForm<ObtenerDesembargoDto>({
        defaultValues: desembargoForm,
    });

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
        },
    });

    const formularioConceptos = useForm<ConceptoDesembargosDto>({
        defaultValues: conceptoDesembargoForm,
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
        await startGetTiposDesembargo();
    };

    const loadTiposProcesosEmbargo = async () => {
        await startGetTiposProcesoDesembargo();
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
                        formularioDesembargos.setValue(
                            'nombreDemandado',
                            `${response.data?.primerNombre} ${response.data?.segundoNombre} ${response.data?.primerApellido} ${response.data?.segundoApellido}`,
                        );
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
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
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerJuzgadoById = async (idJuzgado: string) => {
        await api
            .getCourtById(idJuzgado)
            .then(async response => {
                if (response) {
                    const value = response.data as JuzgadoDto;
                    setJuzgados(value);
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
                        formularioDesembargos.setValue(
                            'juzgadoId',
                            value?.idJuzgado.toUpperCase() as string,
                        );
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const consultarJuzgado = async (idJuzgado: string, expediente: string) => {
        await api
            .getCourtById(idJuzgado)
            .then(async response => {
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
                        } else {
                            formularioEmbargos.setValue(
                                'proceso',
                                value?.codigoInterno,
                            );
                        }
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
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

    const obtenerJuzgados = async (idDepto: number, idCiudad: number) => {
        const response = await api.postAllCourtsByCity(idDepto, idCiudad);
        if (response.data !== null) {
            const datos = response.data as JuzgadoDto[];
            const datosOrdenados = sortBy(datos, 'nombre');
            setJuzgados(...juzgados, datosOrdenados);
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
                if(conceptos.findIndex(x => data.id === x.id) !== -1){
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
        setListaConceptos(array as ConceptoDesembargosDto[]);
        handleCloseModal();
    };

    const handleOnChangeMostrarPorcentaje = (value: boolean) =>
        setMostrarPorcentaje(value);

    const insertarEmbargo = (embargo: EmbargoModelDto) =>
        apiEmbargo.postInsertEmbargo(embargo);

    const actualizarEmbargo = (embargo: EmbargoModelDto) =>
        apiEmbargo.postUpdateEmbargo(embargo);

    const handleOnClickValidation = async (data: EmbargoDto) => {
        let cliente: any = null;
        await apiCliente
            .getClientByIdentificationTypeIdentificationNumberRequest(
                data.clienteTipoIdentificacion !== null
                    ? data.clienteTipoIdentificacion
                    : 0,
                data.identificacionCliente,
            )
            .then(async response => {
                cliente = response.data;
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

        if (cliente === null) {
            swal({
                text: 'El Cliente no existe',
                icon: 'warning',
            });
        } else {
            handleOnClick(data);
        }
    };

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
            embargo?.expedienteBancoAgrario.trim() === '' ||
            embargo?.observaciones.trim() === ''
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
        } else if (embargo?.observaciones === '') {
            toast('Las observaciones no es un campo requerido');
        } else if (embargo?.identificacionCliente.trim().length < 4) {
            toast('El número de identificación debe ser menor a 4 caracteres.');
        } else if (embargo?.identificacionCliente.trim().length < 4) {
            toast('El número de identificación debe ser menor a 4 caracteres.');
        } else if (beneficiarios.length === 0) {
            toast('No existen demandantes y beneficiarios registrados');
            setLoading(false);
        } else if (conceptos.length === 0) {
            toast('No existen conceptos registrados');
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

    const handleOnClickRespuesta = async () => {
        const solicitudId = formularioEmbargos.getValues('id');
        await apiEmbargo
            .postObtenerRespuesta(solicitudId)
            .then(async response => {
                const mimeType = response.type;
                if (mimeType) {
                    const file = new Blob([response], { type: mimeType });
                    const fileURL = URL.createObjectURL(file);
                    window.open(fileURL, '_blank');
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleOnClickRegresar = () => history.push('/EmbargosInicio');

    const consultarCliente = async (idTipoIdentificacion: number, identificacion: string) => {
        await apiCliente
            .getClientByIdentificationTypeIdentificationNumberRequest(
                idTipoIdentificacion !== null
                    ? idTipoIdentificacion
                    : 0,
                identificacion,
            )
            .then(async response => {
                if(response.data != null){
                    formularioEmbargos.setValue('clienteNombre', `${response.data.PrimerNombre} ${response.data.SegundoNombre} ${response.data.PrimerApellido} ${response.data.SegundoApellido}`)
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    return (
        <CHPrivateContainer>
            <InformacionEmbargos
                formularioEmbargos={formularioEmbargos}
                formularioBeneficiarios={formularioBeneficiarios}
                beneficiarios={beneficiarios}
                tiposIdentificacion={catalogState.IdentificationType}
                departamentos={departaments}
                tiposProcesos={embargoState.tiposProcesos}
                cities={cities}
                juzgados={juzgados}
                modoActualizar={modoActualizar}
                listaConceptos={listaConceptos}
                tiposEmbargos={embargoState.tiposEmbargo}
                tiposRetencion={embargoState.tiposRetencion}
                beneficiariosData={embargoModel?.beneficiarios}
                conceptosData={embargoModel?.conceptos}
                cuentas={cuentas}
                setCuentas={setCuentas}
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
            />
           
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

};

interface LinkMapStateToProps {
    desembargoState: DesembargosState;
    embargoState: EmbargosState;
    catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
    desembargoState: state.desembargo,
    embargoState: state.embargo,
    catalogState: state.catalog,
});

interface LinkMapDispatchToProps {
    startGetTiposDesembargo: () => void;
    startGetTiposProcesoDesembargo: () => void;
    startGetTiposEmbargo: () => void;
    startGetTiposProcesoEmbargo: () => void;
    startGetAllTypeDocuments: () => void;
    modoActualizar: boolean;
    setModoActualizar: (value: boolean) => void;
    desembargoModel: any;
    embargoModel: any;
    startGetTiposRetencion: () => void;
    handleOnClickCancelarEditar: () => void;
}

const mapDispatchToProps = {
    startGetTiposDesembargo,
    startGetTiposProcesoDesembargo,
    startGetAllTypeDocuments,
    startGetTiposRetencion,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Desembargo);
