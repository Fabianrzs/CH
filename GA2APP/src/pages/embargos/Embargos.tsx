import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { sortBy } from 'lodash';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { useForm, useFormContext } from 'react-hook-form';
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
import InformacionEmbargos from './InformacionEmbargos';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import { LoadingComponent } from '../../components/loading/loading.component';
import {
    conceptoEmbargoForm,
    embargoForm,
} from './constants/embargosform.model.type';
import apiCourt from '../../services/api-courts.service';
import { JuzgadoDto } from '../../types/court.model.type';
import { CITYDTO, DEPARTAMENTODTO } from '../../types/catalog.type';
import apiCatalogs from '../../services/api-catalog.service';
import NotificationService from '../../services/notification.service';
import apiClient from '../../services/api-clients.service';
import { ModalAgregarConceptos } from './components/ModalAgregarConceptos';
import UtilService from '../../utils/util.service';
import {
    convertEmbargoModelInsert,
    convertEmbargoModelUpdate,
} from './constants/conversionesEmbargo';
import { ModalArchivoRespuesta } from './components/ModalArchivoRespuesta';
import { Movimiento } from '../../types/client.model.type';

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
    }),
);

const Embargos = (props: props) => {
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
        handleSearch,
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

    const [modalOpen, setModalOpen] = useState(false);
    const [documentoURL, setDocumentoURL] = useState('');
    const [estadoCuentaEmbargo, setEstadoCuentaEmbargo] = useState();
    const [estado, setEstado] = useState('');


    const [conceptMovements, setConceptMovements] = useState<Movimiento[]>([]);



    useEffect(() => {
        startGetAllTypeDocuments();
        obtenerDepartamentos();
        loadTiposEmbargo();
        loadTiposProcesosEmbargo();
        loadTiposRetencion();
    }, []);

    useEffect(() => {
        consultarCuentaConcepto();
    }, []);


    useEffect(() => {
        if (modoActualizar) {
            setLoading(true);
            consultarCliente(embargoModel.embargo?.tipoIdentificacionId, embargoModel.embargo?.identificacionCliente);
            formularioEmbargos.setValue('id', embargoModel.embargo?.id);
            formularioEmbargos.setValue(
                'numeroSolicitud',
                embargoModel.embargo?.numeroSolicitud,
            );
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
                'Inconsistencia',
                embargoModel.embargo?.inconsistencia,
            );
            formularioEmbargos.setValue(
                'observaciones',
                embargoModel.embargo?.observaciones,
            );
            formularioEmbargos.setValue(
                'aclaracion',
                embargoModel.embargo?.aclaracion,
            );
            formularioEmbargos.setValue(
                'direccion',
                embargoModel.embargo?.direccion,
            );
            setEstado(embargoModel.embargo?.estadoEmbargo);
            const idJuzgado =
                embargoModel.embargo?.juzgadoId.toUpperCase() as string;
            obtenerJuzgadoById(idJuzgado);
            setBeneficiarios(embargoModel?.beneficiarios);
            // consulta cuentas y conceptos
            consultarCuentaConceptoParam(
                embargoModel.embargo?.tipoIdentificacionId,
                embargoModel.embargo?.identificacionCliente,
            );
            setConceptos(embargoModel?.conceptos);
            consultarJuzgado(
                idJuzgado,
                embargoModel.embargo?.expedienteBancoAgrario,
            );
        }
    }, []);

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
            beneficiarioPago: false,
            bpE_BENEFICIARIO: false,
        },
    });

    const formularioConceptos = useForm<ConceptoEmbargosDto>({
        defaultValues: conceptoEmbargoForm,
    });

    const [saldoConcepto, setSaldoConcepto] = useState(0);
    const [mostrarAlerta, setMostrarAlerta] = useState(false);

    const handleOpenModal = (conceptoId: number) => {
        setMostrarAlerta(false); 
        const index = listaConceptos.findIndex(
            x => x.conceptoId === conceptoId,
        );

        const item = cuentas[0]?.conceptos.filter(item => item.id === conceptoId);

        

        formularioConceptos.reset();

        if (index !== -1) {

            const conceptoSeleccionado = listaConceptos[index];

            setSaldoConcepto(conceptoSeleccionado.saldo);

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
                'fechaFin',
                listaConceptos[index].fechaFin,
            );
            formularioConceptos.setValue(
                'tipoRetencionId',
                listaConceptos[index].tipoRetencionId,
            );
            formularioConceptos.setValue(
                'id',
                listaConceptos[index].id,
            )
        } if (conceptoId === 1) {
            formularioConceptos.setValue(
                'conceptoId',
                conceptoId,
            );
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
                toast('Por favor, llenar todos los campos obligatorios');
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

        const saldoConcepto = listaConceptos?.find(x => x.id === data.id)?.saldo;

        if (data.tipoRetencionId === 'aae6c046-06ad-40c3-8fd5-9776eddc12e8' && data.tipoEmbargoId !== 'ae7d141a-fd88-4f57-a199-4db5b5301946') {
            toast('El tipo de embargo tiene que ser por porcentaje');
        } else {
            console.log('.');
            if ((saldoConcepto ?? 0) < +data.valor && conceptoId !== 1) {
                formularioConceptos.setError('valor', {
                    type: 'custom',
                    message: 'El valor a embargar supera el 50% del saldo.',
                });
            } else {
                formularioConceptos.clearErrors('valor');

                if (conceptoId === 1) {
                    const updatedListaConceptos = listaConceptos.map(item => {
                        const newItem = {
                            tipoEmbargoId: data.tipoEmbargoId,
                            valor: data.valor,
                            fechaInicio: data.fechaInicio,
                            fechaFin: data.fechaFin,
                            tipoRetencionId: data.tipoRetencionId,
                            indicadorCesantias: data.indicadorCesantias,
                            id: item.id,
                        };

                        return {
                            ...item,
                            ...newItem,
                        };
                    });

                    setConceptos(updatedListaConceptos);
                    setListaConceptos(updatedListaConceptos);
                }
                else {
                    const array = listaConceptos.map(item => {
                        if (item.conceptoId === conceptoId) {
                            const newItem = {
                                ...item,
                                tipoEmbargoId: data.tipoEmbargoId,
                                valor: data.valor,
                                fechaInicio: data.fechaInicio,
                                fechaFin: data.fechaFin,
                                tipoRetencionId: data.tipoRetencionId,
                                indicadorCesantias: data.indicadorCesantias,
                            };

                            if (conceptos.findIndex(x => data.id === x.id) !== -1) {
                                newItem.id = data.id;
                                setConceptos(conceptos?.map(x => data.id === x.id ? newItem : x));
                            } else {
                                setConceptos([...conceptos, newItem]);
                            }

                            setValorTotal(valorTotal + newItem.valor);
                            return newItem;
                        }
                        return item;
                    });

                    setListaConceptos(array as ConceptoEmbargosDto[]);
                }

                handleCloseModal();
            }
        }
    };


    const handleOnChangeMostrarPorcentaje = (value: boolean) =>
        setMostrarPorcentaje(value);

    const insertarEmbargo = async (embargo: EmbargoModelDto) => {
        await apiEmbargo.postInsertEmbargo(embargo);

    }

    const actualizarEmbargo = (embargo: EmbargoModelDto) =>
        apiEmbargo.postUpdateEmbargo(embargo);


    const [observacionesBloqueadas, setObservacionesBloqueadas] = useState(false);
    const [respModal, setRespModal] = useState();


    const handleOnClickValidation = async () => {
       
        await formularioEmbargos.handleSubmit(handleOnClick)(); 
    };
    


    const handleOnClick = async (embargo: EmbargoDto) => {
        try {
            setLoading(true);

            if (!embargo) throw new Error('No se proporcionó un embargo válido.');

            if (!embargo.Inconsistencia) {
                const numeroDemandantes = beneficiarios.filter(item => item.demandante).length;

                if (
                    embargo.clienteTipoIdentificacion === 0 ||
                    embargo.identificacionCliente.trim() === '' ||
                    embargo.emailRespuesta.trim() === '' ||
                    embargo.expedienteBancoAgrario.trim() === ''
                ) {
                    throw new Error('Deben diligenciarse todos los campos obligatorios.');
                }
                if (Number(embargo.identificacionCliente) < 0) {
                    throw new Error('El número de identificación no puede ser negativo.');
                }
                if (!embargo.radicadoJuzgado.trim()) {
                    console.warn('El Radicado del Juzgado está vacío, pero no es obligatorio.');
                }
                if (embargo.identificacionCliente.trim().length < 4) {
                    throw new Error('El número de identificación debe tener al menos 4 caracteres.');
                }
                if (beneficiarios.length === 0) {
                    throw new Error('No existen demandantes y beneficiarios registrados.');
                    console.warn('No existen demandantes y beneficiarios registrados.');
                }
                if (conceptos.length === 0) {
                    throw new Error('No existen conceptos embargados registrados.');
                }
                if (numeroDemandantes === 0) {
                    throw new Error('Debe registrarse al menos un demandante.');
                }
                if (numeroDemandantes > 1) {
                    throw new Error('Solo debe registrarse un demandante.');
                }
            }

            if (embargo.Inconsistencia) {
                const EstadoEmbargo = {
                    id: embargo.id,
                    Inconsistencia: embargo.Inconsistencia,
                    aclaracion: embargo.aclaracion
                };

                const response = await apiEmbargo.postUpdateEstadoEmbargo(EstadoEmbargo);
                if (!response?.isSuccess) throw new Error(response.returnMessage);

                await handleOnchangeModo(false);
                resetForms();
                await showSuccessAlert();
                setLoading(false);
                handleOnClickCancelarEditar();
                history.push('/EmbargosInicio');
                handleSearch();
            } else if (modoActualizar) {
                const respuesta = await actualizarEmbargo(convertEmbargoModelUpdate(embargo, beneficiarios, conceptos));
                if (!respuesta?.isSuccess) throw new Error(respuesta.returnMessage);
                await handleOnClickRespuesta();
            }
        } catch (error) {
            setLoading(false);
            swal({
                text: `No se pudo procesar el embargo: ${error.message}`,
                icon: 'error',
            });
        }
    };

    const resetForms = () => {
        formularioEmbargos.reset();
        formularioBeneficiarios.reset();
        formularioConceptos.reset();
    };

    const showSuccessAlert = async () => {
        await NotificationService.genericAlert('Procesada', 'Se ha procesado correctamente la información');
    };

    const [resolver, setResolver] = useState<((value: boolean | null) => void) | null>(null);
    const handleOnClickRespuesta = async () => {
        const solicitudId = formularioEmbargos.getValues('id');
        try {
            const response = await apiEmbargo.postObtenerRespuesta(solicitudId);
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
            }
        } catch (error) {
            toast('Error al generar respuesta');
        }
    };

    const handleCloseModalRespuesta = async () => {
        setModalOpen(false);
        setDocumentoURL('');

        handleOnchangeModo(false);
        resetForms();
        await showSuccessAlert();
        setLoading(false);
        handleOnClickCancelarEditar();
        history.push('/EmbargosInicio');
        handleSearch();

    };
    const onCloseModalRespuesta = async () => {
        setModalOpen(false);
        setDocumentoURL('');
        setLoading(false);
    };



    const handleOnClickRegresar = async () => history.push('/EmbargosInicio');
    const [concepto, setConcepto] = useState([]);
    const [movimientos, setMovimientos] = useState([]);


    // const consultarCliente = async (idTipoIdentificacion: number, identificacion: string) => {
    //     await apiCliente
    //         .getClientByIdentificationTypeIdentificationNumberRequest(
    //             idTipoIdentificacion !== null
    //                 ? idTipoIdentificacion
    //                 : 0,
    //             identificacion,
    //         )
    //         .then(async response => {
    //             if (response.data != null) {
    //                 formularioEmbargos.setValue('clienteNombre', `${response.data.PrimerNombre} ${response.data.SegundoNombre} ${response.data.PrimerApellido} ${response.data.SegundoApellido}`)
    //                 console.log('cuentas movimientos', response.data.cuentas);
    //                 setEstadoCuentaEmbargo(response.data.cuentas)


    //             }
    //         })
    //         .catch(() => {
    //             toast('Ingrese el tipo de documento');
    //         });

    //     await apiEmbargo.getObtenerEmbargosByIdentificacion(identificacion)
    //         .then(async response => {
    //             if (response.data != null) {
    //                 const allConceptos = [];

    //                 response.data.forEach((item) => {
    //                     if (item.embargo && item.embargo.remanente === false) {
    //                         allConceptos.push(...(item.conceptos || []));
    //                     }
    //                 });

    //                 setConcepto(allConceptos);

    //             }
    //         })
    //         .catch(() => {

    //         });
    //     setLoading(false);
    // }

    const [nombreUnno, setNombreUnno] = useState();
    const [nombreDoss, setNombreDoss] = useState();
    const [apellidoUnno, setApellidoUnno] = useState();
    const [apellidoDoss, setApellidoDoss] = useState();

    const consultarCliente = async (idTipoIdentificacion: number, identificacion: string) => {
        if (idTipoIdentificacion) {
            await apiCliente
                .getClientByIdentificationTypeIdentificationNumberRequest(idTipoIdentificacion, identificacion)
                .then(async (response) => {
                    if (response.data != null) {
                        formularioEmbargos.setValue(
                            'clienteNombre',
                            `${response.data.PrimerNombre} ${response.data.SegundoNombre} ${response.data.PrimerApellido} ${response.data.SegundoApellido}`
                        );

                        const { primerNombre, segundoNombre, primerApellido, segundoApellido } = response.data;

                        formularioEmbargos.setValue('clienteNombre', `${primerNombre} ${segundoNombre} ${primerApellido} ${segundoApellido}`);

                        setNombreUnno(primerNombre);
                        setNombreDoss(segundoNombre);
                        setApellidoUnno(primerApellido);
                        setApellidoDoss(segundoApellido);

                        console.log('cuentas movimientos', response.data.cuentas);


                        setEstadoCuentaEmbargo(response.data.cuentas);
                    }
                })
                .catch(() => {
                    toast('Error al consultar el cliente.');
                });

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
        } else {
            toast('Ingrese el tipo de documento');
        }
    };

    const [bloquearBoton, setBloquearBoton] = useState(false);


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
                tipoDocumento={tipoDocumento}
                obtenerCiudades={obtenerCiudades}
                obtenerJuzgados={obtenerJuzgados}
                setBeneficiarios={setBeneficiarios}
                handleOpenModal={handleOpenModal}
                handleCloseModal={handleCloseModal}
                setListaConceptos={setListaConceptos}
                setTipoDocumento={setTipoDocumento}
                setDocumento={setDocumento}
                consultarCuentaConcepto={consultarCuentaConcepto}
                porcentaje={porcentaje}
                variacionPorcentaje={variacionPorcentaje}
                handleOnChangeMostrarPorcentaje={handleOnChangeMostrarPorcentaje}
                consultarJuzgado={consultarJuzgado}
                consultarCliente={consultarCliente}
                setCuentas={setCuentas}
                estadoCuentaEmbargo={estadoCuentaEmbargo}
                setConcepto={concepto}
                nombreUnno={nombreUnno}
                nombreDoss={nombreDoss}
                apellidoUnno={apellidoUnno}
                apellidoDoss={apellidoDoss}
                saldoConcepto={saldoConcepto}
            />
            <ModalArchivoRespuesta
                open={modalOpen}
                documentoURL={documentoURL}
                handleClose={handleCloseModalRespuesta}
                onClose={onCloseModalRespuesta} s
                resolver={resolver}
                estado={estado}
            />
            <Grid container>
                <div
                    className={
                        modoActualizar
                            ? classes.updateButton
                            : classes.insertButton
                    }
                >
                    {embargoModel.embargo?.estadoEmbargo === '397' || embargoModel.embargo?.estadoEmbargo === '396' ? (
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            style={{ width: '200px' }}
                            onClick={handleOnClickRespuesta}
                        >
                            VER RESPUESTA
                        </Button>
                    ) : null}
                    {embargoModel.embargo?.estadoEmbargo === '553' ||
                        embargoModel.embargo?.estadoEmbargo === '395' ? (
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            style={{ width: '200px' }}
                            onClick={handleOnClickValidation}
                            disabled={bloquearBoton}
                        >
                            EMBARGAR
                        </Button>
                    ) : null}
                </div>
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
                    CANCELAR
                </Button>
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
                conceptos={estadoCuentaEmbargo}
                saldoConcepto={saldoConcepto}
            />
        </CHPrivateContainer>
    );
};

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
    handleSearch: () => void;
}

const mapDispatchToProps = {
    startGetTiposEmbargo,
    startGetTiposProcesoEmbargo,
    startGetAllTypeDocuments,
    startGetTiposRetencion,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Embargos);
