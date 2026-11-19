import React, { useEffect, useState } from 'react';
import {
    Button,
    createStyles,
    Grid,
    IconButton,
    InputAdornment,
    makeStyles,
    Menu,
    MenuItem,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TextField,
} from '@material-ui/core';
import { Controller, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import swal from 'sweetalert';
import { Delete, Edit, Save } from '@material-ui/icons';
import Autocomplete from '@material-ui/lab/Autocomplete/Autocomplete';
import AttachFile from '@material-ui/icons/AttachFile';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import {
    BeneficiarioDto,
    ConceptoEmbargosDto,
    TipoEmbargoDto,
    CuentaModelDto,
    TipoProcesoEmbargoDto,
    TipoRetencionDto,
    EmbargoModelDto,
} from '../../types/embargo.model.type';
import {
    CITYDTO,
    DEPARTAMENTODTO,
    ENTIDADBANCARIA,
    IDENTIFICATIONTYPEDTO,
    VALORCATALOGO,
} from '../../types/catalog.type';
import { JuzgadoDto } from '../../types/court.model.type';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import NotificationService from '../../services/notification.service';
import CHPaper from '../../components/ch-paper/CHPaper';
import CuentasList from '../embargos/cuentasList/CuentasList';
import CatalogApi from '../../services/api-catalog.service';
import ApiEmbargos from '../../services/api-embargos.service'
import Expedientes from './components/Expedientes';
import { ModalPagoEmbargos } from './components/ModalPagoEmbargo';



const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        button: {
            width: '60%',
            marginLeft: 'auto',
            marginRight: 20,
            marginTop: '1.5rem',

        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        buttonD: {
            padding: '10px',
            textAlign: 'center',
            marginTop: '15px',
        },
    }),
);


const InformacionPagoEmbargos = (props: props) => {

    const {
        formularioEmbargos,
        formularioBeneficiarios,
        beneficiarios,
        modoActualizar,
        tiposIdentificacion,
        departamentos,
        tiposProcesos,
        tiposEmbargos,
        tiposRetencion,
        cities,
        juzgados,
        listaConceptos,
        beneficiariosData,
        cuentas,
        conceptosData,
        porcentaje,
        variacionPorcentaje,
        datajuzgado,
        obtenerCiudades,
        obtenerJuzgados,
        setBeneficiarios,
        handleOpenModal,
        setListaConceptos,
        setTipoDocumento,
        consultarCuentaConcepto,
        handleOnChangeMostrarPorcentaje,
        consultarJuzgado,
        consultarCliente,
        setCuentas,
        embargoModel,
        handleCloseModal,
        estadoCuentaEmbargo,
        setConcepto,
        setVista
    } = props;

    const classes = useStyles();
    const today = new Date();
    const [entidadesBancarias, setEntidadesBancarias] = useState<any[]>([])
    const [entidadesCuentas, setEntidadesCuentas] = useState<any[]>([]);

    const [beneficiariosModel, setBeneficiariosModel] = useState<
        BeneficiarioDto[]
    >([]);
    const [estadoBoton, setEstadoBoton] = useState<string>(
        'AGREGAR INFORMACIÓN PAGO',
    );
    const [validarValorTipo, setValidarValorType] = useState<any>();

    const [modalOpen, setModalOpen] = useState(false);
    const [editarIndex, setEditarIndex] = useState<number | null>(null);
    const [tipoCuentaSeleccionada2, setTipoCuentaSeleccionada2] = useState<number | null>(null);


    const api = ApiEmbargos();

    const handleModalOpen = () => {
        if (nuevoBeneficiario.length > 0) {
            if (botonTexto === "VER RESPUESTA") {
                setModalOpenpdf(true);
            } else {
                setModalOpen(true);
            }
        } else {
            swal({
                text: 'No hay información para generar el pago.',
                icon: 'warning',
            });
        }
    };

    const [botonOficio, setBotonOficio] = useState("OFICIO DE RESPUESTA");

    const handleOficioRespuesta = () => {
        console.log("generar oficio respuesta");
    }


    const handleModalClose = () => {
        setModalOpen(false);
    };

    const catalogApi = CatalogApi();

    useEffect(() => {
        if (modoActualizar) {
            if (beneficiariosData.length > 0) {
                setBeneficiariosModel(beneficiariosData);
            }
        }
        getAllEntidadBancaria();
        TipoCuenta();
        getTiposEmbargos();

    }, []);

    useEffect(() => {
        const beneficiario = beneficiarios.find(b => b.bpE_BENEFICIARIO);
        if (beneficiario) return;

        if (datajuzgado && datajuzgado.nroCuenta) {
            formularioBeneficiarios.setValue('nombreRazonSocial', embargoModel.embargo?.juzgado);
            formularioBeneficiarios.setValue('valor', embargoModel.conceptos[0]?.valor);
            formularioBeneficiarios.setValue('confnumeroIdentificacion', datajuzgado.nroCuenta);
            formularioBeneficiarios.setValue('numeroIdentificacion', datajuzgado.nroCuenta);
        }
    }, [datajuzgado, beneficiarios]);



    const getAllEntidadBancaria = async () => {
        await catalogApi
            .getAllEntidadBancaria()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        setEntidadesBancarias(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error al obtener entidades bancarias');
            });
    }

    const [formularioActivo, setFormularioActivo] = useState<'demandante' | 'pago' | null>(null);

    const seleccionarRegistro = (beneficiarioSeleccionado: any, index: number) => {
        formularioBeneficiarios.reset({
            tipoDocumentoId: beneficiarioSeleccionado.tipoDocumentoId,
            numeroIdentificacion: beneficiarioSeleccionado.numeroIdentificacion,
            confnumeroIdentificacion: beneficiarioSeleccionado.numeroIdentificacion,
            nombreRazonSocial: beneficiarioSeleccionado.nombreRazonSocial,
            tipoCuenta: beneficiarioSeleccionado.tipoCuenta,
            entidad: beneficiarioSeleccionado.entidad,
            numeroCuenta: beneficiarioSeleccionado.numeroCuenta,
            confNumeroCuenta: beneficiarioSeleccionado.confNumeroCuenta,
            valor: beneficiarioSeleccionado.valor,
            valorPorcentaje: beneficiarioSeleccionado.valorPorcentaje,
        });

        setFormularioActivo('pago');
        setEditarIndex(index);
        setEstadoBoton('EDITAR BENEFICIARIO');
    };


    const cancelarBeneficiario = async () => {
        setEstadoBoton('AGREGAR BENEFICIARIO');
        formularioBeneficiarios.resetField('');
        formularioBeneficiarios.setValue('tipoCuenta', '');
        formularioBeneficiarios.setValue('entidad', '');
        formularioBeneficiarios.setValue('tipoDocumentoId', '');
        formularioBeneficiarios.setValue('numeroIdentificacion', '');
        formularioBeneficiarios.setValue('confnumeroIdentificacion', '');
        formularioBeneficiarios.setValue('nombreRazonSocial', '');
        formularioBeneficiarios.setValue('confNumeroCuenta', '');
        formularioBeneficiarios.setValue('numeroCuenta', '');
        formularioBeneficiarios.setValue('valorPorcentaje', '');
        formularioBeneficiarios.setValue('valor', null);

    };

    const resetBeneficiarioNuevo = async () => {
        setEstadoBoton('AGREGAR BENEFICIARIO');
        formularioBeneficiarios.setValue('id', '');
        formularioBeneficiarios.setValue('tipoDocumentoId', '');
        formularioBeneficiarios.setValue('numeroIdentificacion', '');
        formularioBeneficiarios.setValue('primerNombre', '');
        formularioBeneficiarios.setValue('segundoNombre', '');
        formularioBeneficiarios.setValue('primerApellido', '');
        formularioBeneficiarios.setValue('segundoApellido', '');
        formularioBeneficiarios.setValue('demandante', false);
    };
    const TipoCuenta = async () => {

        catalogApi.getAllCatalogoPorId(67)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        setEntidadesCuentas(response.data);
                        
                    }
                }
            })
            .catch(() => {
                toast('Error al obtener el catalogo 67', { type: toast.TYPE.ERROR });
            });

    }


    const eliminarRegistro = async (numero: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...beneficiariosModel];
            temp.splice(
                temp.findIndex(item => item.numeroIdentificacion === numero),
                1,
            );
            setBeneficiariosModel(temp);
            setBeneficiarios(temp);
        }
    };


    const eliminarRegistroPago = async (numero: string) => {

        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminar este registro?',
        );

        if (confirmAlert) {
            setNuevoBeneficiario(prev => {
                const updatedList = prev.filter(item => item.numeroIdentificacion !== numero);
                return [...updatedList];
            });
        }
    };

    interface Beneficiario {
        tipoDocumentoId: any;
        numeroIdentificacion: any;
        nombreRazonSocial: any;
        confNumeroCuenta: any;
        numeroCuenta: any;
        valorPorcentaje: any;
        valor: string;
        tipoCuenta: any;
        entidad: any;
    }

    const [nuevoBeneficiario, setNuevoBeneficiario] = useState<any[]>([]);
    const [beneficiarioSeleccionado, setBeneficiarioSeleccionado] = useState<Beneficiario | null>(null);
    // const resultadoSaldo = embargoModel?.conceptos[0].valor;
    const resultadoSaldo = embargoModel?.conceptos?.length ? embargoModel.conceptos[0].valor : 0;
    const [mostrarPorcentajes, setMostrarPorcentajes] = useState(false);
    const [tipoEmbargo, setTipoEmbargo] = useState(false);
    const [tipoDocumentoSeleccionado, setTipoDocumentoSeleccionado] = useState<any>('');
    const [entidadSeleccionada, setEntidadSeleccionada] = useState<any>('');
    const [tipoCuentaSeleccionada, setTipoCuentaSeleccionado] = useState<any>('');

    const getTiposEmbargos = async () => {
        await api
            .getTiposEmbargos()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        setTipoEmbargo(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Tipos de embargo no cargados');
            });
    }

    const handleTipoDocumentoChange = (e: any) => {
        const selectedValue = e.target.value;

        const selectedOption = tiposIdentificacion.find((option: IDENTIFICATIONTYPEDTO) => option.id === selectedValue);

        if (selectedOption) {
            setTipoDocumentoSeleccionado({
                id: selectedOption.id,
                descripcion: selectedOption.descripcion,
            });
        }
    };

    const [idEntidad, setIdEntidad] = useState<any>();

    const handleEntidadChange = (e: any) => {
        const selectedId = e.target.value;
        

        const entidadSeleccionada = entidadesBancarias.find((entidad) => entidad.idEntidadBancaria === selectedId);

        if (entidadSeleccionada) {
            setEntidadSeleccionada(entidadSeleccionada.nombreRazonSocial);
            setIdEntidad(selectedId);
        }
    };

    const handleTipoCuentaChange = (e: any) => {
        const selectedId = e.target.value;

        const tipoCuentaSeleccionado =
            entidadesCuentas
                .filter(option => selectedId === option.idValor)
                .map(option => option.descripcion);

        if (tipoCuentaSeleccionado) {
            setTipoCuentaSeleccionado(tipoCuentaSeleccionado);
        }
    };

    const handleSelectChange = async (e: any) => {

        const expediente = await formularioEmbargos.getValues('expedienteBancoAgrario');
        const juzgado = await formularioEmbargos.getValues('juzgadoId');
        if (juzgado) {
            consultarJuzgado(juzgado, expediente);
        }

    };

    const activarFormularioPago = () => {
        if (formularioActivo !== 'pago') {
            setFormularioActivo('pago');
        }
    };

    interface BeneficiarioDto {
        tipoDocumentoId: number;
        numeroIdentificacion: string;
        primerNombre: string;
        segundoNombre?: string;
        primerApellido: string;
        segundoApellido?: string;
        demandante: string;
    }

    const [modoEdicion, setModoEdicion] = useState(null);
    const [datosEditados, setDatosEditados] = useState<Record<number, Partial<BeneficiarioDto>>>({});

    const handleChange = (index: number, field: keyof BeneficiarioDto, value: any) => {
        setDatosEditados(prev => ({
            ...prev,
            [index]: {
                ...prev[index],
                [field]: field === 'demandante' ? value === "SI" : value,
            },
        }));
    };

    const handleGuardar = (index: number) => {
        const nuevosBeneficiarios = [...beneficiarios];
        nuevosBeneficiarios[index] = {
            ...nuevosBeneficiarios[index],
            ...datosEditados[index],
            demandante: datosEditados[index]?.demandante ?? nuevosBeneficiarios[index].demandante,
        };
        setBeneficiarios(nuevosBeneficiarios);
        setModoEdicion(null);
    };

    useEffect(() => {
        const tipoDocumentoId = formularioBeneficiarios.watch("tipoDocumentoId");
        const numeroIdentificacion = formularioBeneficiarios.watch("numeroIdentificacion");
        const confNumeroIdentificacion = formularioBeneficiarios.watch("confnumeroIdentificacion");

        if (
            tipoDocumentoId &&
            numeroIdentificacion &&
            confNumeroIdentificacion &&
            numeroIdentificacion === confNumeroIdentificacion
        ) {
            consultarCliente(tipoDocumentoId, numeroIdentificacion);
        }
    }, [
        formularioBeneficiarios.watch("tipoDocumentoId"),
        formularioBeneficiarios.watch("numeroIdentificacion"),
        formularioBeneficiarios.watch("confnumeroIdentificacion")
    ]);

    const [modalOpenpdf, setModalOpenpdf] = useState(false);

    const [botonTexto, setBotonTexto] = useState("GENERAR PAGO");
    const handleCambioTextoBoton = () => {
        setBotonTexto("VER RESPUESTA")
        setModalOpen(false);
        setModalOpenpdf(true);
    };

    const [linkDodo, setLinkDodo] = useState('');
    useEffect(() => {
        obtenerUrlDodo()
    }, [formularioEmbargos.watch('radicadoWorkManager')]);
    const obtenerUrlDodo = async () => {

        const radicado = formularioEmbargos.getValues('radicadoWorkManager');
        const enlaceCompleto = `http://cpvmp-112p:91/LinkFormulario/FormularioWM/Index?columna=Cod_Barras&valorColumna=${radicado}&idFormulario=20058`;
        setLinkDodo(enlaceCompleto);

    }


    const [loadingBeneficiarios, setLoadingBeneficiarios] = useState(true);

    useEffect(() => {
        if (beneficiarios && beneficiarios.length > 0) {
            setLoadingBeneficiarios(false);
        }
    }, [beneficiarios]);


    useEffect(() => {
        if (loadingBeneficiarios) return;

        const beneficiario = beneficiarios.find(b => b.bpE_BENEFICIARIO);

        if (beneficiario) {
            const nombres = [
                beneficiario.primerNombre,
                beneficiario.segundoNombre,
                beneficiario.primerApellido,
                beneficiario.segundoApellido
            ].filter(Boolean).join(' ').toUpperCase();

            formularioBeneficiarios.reset({
                numeroIdentificacion: beneficiario.numeroIdentificacion,
                confnumeroIdentificacion: beneficiario.numeroIdentificacion,
                nombreRazonSocial: nombres,
            });
        }
    }, [loadingBeneficiarios, beneficiarios]);

    useEffect(() => {
        const beneficiarioEsTrue = embargoModel?.[0]?.beneficiarios?.[0]?.bpE_BENEFICIARIO === true;
        const valorConcepto = embargoModel?.conceptos?.[0]?.valor;

        if (beneficiarioEsTrue && valorConcepto !== undefined) {
            if (mostrarPorcentajes) {
                formularioBeneficiarios.setValue("valor", String(valorConcepto), {
                    shouldDirty: true,
                    shouldValidate: true,
                });
            } else {
                formularioBeneficiarios.setValue("valor", valorConcepto.toFixed(2), {
                    shouldDirty: true,
                    shouldValidate: true,
                });
            }
        }
    }, [embargoModel, mostrarPorcentajes]);


    useEffect(() => {
        const numeroCuenta = datajuzgado?.nroCuenta;
        const beneficiarioTrue = embargoModel?.[0]?.beneficiarios?.[0]?.bpE_BENEFICIARIO === true;

        if (!beneficiarioTrue && numeroCuenta) {
            formularioBeneficiarios.setValue('numeroCuenta', numeroCuenta);
            formularioBeneficiarios.setValue('confNumeroCuenta', numeroCuenta);
        } else {

            formularioBeneficiarios.setValue('numeroCuenta', '');
            formularioBeneficiarios.setValue('confNumeroCuenta', '');
        }
    }, [embargoModel, datajuzgado]);

    const [valorVisual, setValorVisual] = useState('');

    useEffect(() => {
        const subscription = formularioBeneficiarios.watch((value, { name }) => {
            if (name === "valor" && (value?.valor === undefined || value.valor === null || value.valor === 0)) {
                setValorVisual('');
            }
        });

        return () => subscription.unsubscribe();
    }, [formularioBeneficiarios]);

    const formatCurrency = (val: any) =>
        new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
        }).format(val || 0);

    const [valorCalculado, setValorCalculado] = useState<any>(0);

    const recibirValorDesdeHijo = (valor: any) => {
        setValorCalculado(valor);
    };


    useEffect(() => {
        if (valorCalculado !== undefined && valorCalculado !== null && !Number.isNaN(valorCalculado)) {
            const visual = mostrarPorcentajes
                ? `${valorCalculado} %`
                : `$ ${new Intl.NumberFormat('es-CO').format(valorCalculado)}`;
    
            setValorVisual(visual);
            formularioBeneficiarios.setValue("valor", valorCalculado);
        }
    }, [valorCalculado, mostrarPorcentajes]);
    





    return (
        <>
            <CHPaper title="Información solicitud">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroSolicitud"
                            control={formularioEmbargos.control}
                            rules={{
                                pattern: {
                                    value: /^[0-9]+$/g,
                                    message: 'Este campo debe contener solo números'
                                },
                                required: {
                                    value: false,
                                    message: 'Este campo debe contener solo números',
                                },

                                maxLength: {
                                    value: 10,
                                    message:
                                        'La longitud máxima del campo es de 10 números',
                                },
                                minLength: {
                                    value: 10,
                                    message: 'No se permite menos de 10 números'
                                },
                                min: {
                                    value: 9,
                                    message: 'Número de radicado inválido, la longitud requerida son 10 números'
                                },
                            }}
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="Número Solicitud"
                                    value={value}
                                    onChange={() => { }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoIdentificacion"
                            control={formularioEmbargos.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Tipo de Identificación *"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    disabled
                                    onChange={e => {
                                        setTipoDocumento(
                                            e.target.value as number,
                                        );
                                        onChange(e);
                                    }}
                                >
                                    {tiposIdentificacion?.map(
                                        (option: IDENTIFICATIONTYPEDTO) => (
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
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="identificacionCliente"
                            control={formularioEmbargos.control}
                            rules={{
                                pattern: {
                                    value: /^[0-9]+$/g,
                                    message: 'Este campo es requerido y debe contener solo números'
                                },
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido y debe contener solo números',
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
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Número de Identificación *"
                                    value={value}
                                    onBlur={() => {
                                        consultarCuentaConcepto()
                                        consultarCliente(formularioEmbargos.watch('tipoIdentificacion'), formularioEmbargos.watch('identificacionCliente'))
                                    }}
                                    numberMask
                                    onChange={() => { }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreCliente"
                            control={formularioEmbargos.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            render={({ field: { onChange, value } }) => {

                                const nombresApellidos = value ? value.split(' ') : [];
                                const nombres = nombresApellidos.slice(0, -2).join(' ');

                                return (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Nombres"
                                        value={nombres}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                    />
                                );
                            }}
                        />
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="nombreCliente"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido',
                                    },
                                }}
                                render={({ field: { onChange, value } }) => {
                                    const nombresApellidos = value ? value.split(' ') : [];
                                    const apellidos = nombresApellidos.slice(2, 4).join(' ');

                                    return (
                                        <CHTextField
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }}
                                            label="Apellidos"
                                            value={apellidos}
                                            onChange={onChange}
                                            inputProps={{
                                                style: {
                                                    textTransform: 'uppercase',
                                                },
                                            }}
                                        />
                                    );
                                }}
                            />
                        </Grid>
                        <Grid container alignItems="center" className={classes.column} item xs={12} md={3}>

                            <Controller
                                name="radicadoWorkManager"
                                control={formularioEmbargos.control}
                                rules={{
                                    pattern: {
                                        value: /^[\d-\s]*$/g,
                                        message: 'Este campo debe contener solo números y guiones'
                                    },
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido',
                                    },
                                    maxLength: {
                                        value: 23,
                                        message: 'La longitud máxima del campo es de 23 caracteres',
                                    },
                                    minLength: {
                                        value: 23,
                                        message: ''
                                    },
                                    min: {
                                        value: 22,
                                        message: 'Número de radicado inválido, la longitud requerida es de 23 caracteres'
                                    },
                                    validate: (value: string) => {
                                        if (value.length !== 23) {
                                            return 'La longitud del campo debe ser de 23 caracteres';
                                        }
                                        return true;
                                    },
                                }}
                                render={({ field: { value }, fieldState: { error } }) => (
                                    <>
                                        <Grid item xs={10}>
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                isReadOnly
                                                InputLabelProps={{ shrink: true }}
                                                label="Número Radicado Asociado *"
                                                value={value}
                                                onChange={() => { }} />
                                        </Grid>
                                        <Grid item xs={2} style={{ display: "flex", justifyContent: "center" }}>
                                            <a href={linkDodo} target="_blank" rel="noopener noreferrer">
                                                <AttachFile />
                                            </a>
                                        </Grid>
                                    </>
                                )}
                            />


                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaRadicadoWorkManager"
                                control={formularioEmbargos.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHDatePicker
                                        label="Fecha Radicado"
                                        onChange={onChange}
                                        value={value}
                                        inputProps={{ readOnly: true }}
                                        minDate={new Date("1-01-1950")}
                                        maxDate={today}
                                        invalidDateMessage="Fecha inválida"
                                        autoOk
                                        disabled
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="radicadoJuzgado"
                                control={formularioEmbargos.control}
                                rules={{
                                    pattern: {
                                        value: /[0-9]*/,
                                        message: 'Este campo debe contener solo números'
                                    },
                                    required: {
                                        value: true,
                                        message: 'Este campo debe contener solo números',
                                    },

                                    maxLength: {
                                        value: 50,
                                        message:
                                            'Excedió los caracteres',
                                    },

                                }}
                                render={({
                                    field: { value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Número Oficio Juzgado *"
                                        value={value}
                                        onChange={() => { }}
                                        type="number"
                                        onKeyPress={(event) => {
                                            const keyCode = event.keyCode || event.which;
                                            const keyValue = String.fromCharCode(keyCode);
                                            const regex = /^[0-9]*$/;
                                            if (!regex.test(keyValue)) {
                                                event.preventDefault();
                                            }
                                        }}

                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaRadicadoJuzgado"
                                control={formularioEmbargos.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHDatePicker
                                        label="Fecha Oficio Juzgado"
                                        inputProps={{ readOnly: true }}
                                        onChange={onChange}
                                        value={value}
                                        minDate={new Date("1-01-1950")}
                                        maxDate={today}
                                        autoOk
                                        disabled
                                    />
                                )}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="idDepartamento"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => {
                                    const [anchorEl, setAnchorEl] = useState(null);
                                    const dept = departamentos.find(d => d.id === Number(value));
                                    const displayValue = dept && dept.nombre !== "(SIN IDENTIFICAR)" ? dept.nombre : '';
                                    const handleOpenMenu = (event: any) => setAnchorEl(event.currentTarget);
                                    const handleCloseMenu = () => setAnchorEl(null);
                                    const handleSelect = (option: any) => {
                                        onChange(option.id);
                                        handleCloseMenu();

                                        formularioEmbargos.setValue('idCiudad', '');
                                        formularioEmbargos.setValue('juzgadoId', '');
                                        formularioEmbargos.setValue('codigoInterno', '');
                                        formularioEmbargos.setValue('codigoOficina', '');
                                        formularioEmbargos.setValue('numeroCuenta', '');

                                        obtenerCiudades(option.id);
                                    };

                                    return (
                                        <>
                                            <CHTextField
                                                label="Departamento Juzgado*"
                                                value={displayValue}
                                                onChange={(e) => {
                                                    const id = e.target.value;

                                                    if (id === '') {
                                                        formularioEmbargos.setValue('idCiudad', '');
                                                        formularioEmbargos.setValue('juzgadoId', '');
                                                        formularioEmbargos.setValue('codigoInterno', '');
                                                        formularioEmbargos.setValue('codigoOficina', '');
                                                        formularioEmbargos.setValue('numeroCuenta', '');
                                                    } else {
                                                        formularioEmbargos.setValue('idCiudad', '');
                                                        formularioEmbargos.setValue('juzgadoId', '');
                                                        formularioEmbargos.setValue('codigoInterno', '');
                                                        formularioEmbargos.setValue('codigoOficina', '');
                                                        formularioEmbargos.setValue('numeroCuenta', '');
                                                        obtenerCiudades(id as unknown as number);
                                                    }

                                                    onChange(id);

                                                }}
                                                onClick={handleOpenMenu}
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                disabled
                                                InputProps={{
                                                    endAdornment: (
                                                        <InputAdornment
                                                            position="end"
                                                            style={{ paddingRight: '4px' }}
                                                        >
                                                            <IconButton
                                                                aria-label="open menu"
                                                                style={{ padding: '4px' }}
                                                                onClick={handleOpenMenu}
                                                            >
                                                                <ExpandMoreIcon fontSize="small" />
                                                            </IconButton>
                                                        </InputAdornment>
                                                    ),
                                                }}
                                                fullWidth
                                            />
                                            <Menu
                                                anchorEl={anchorEl}
                                                open={Boolean(anchorEl)}
                                                onClose={handleCloseMenu}
                                            >
                                                {departamentos.map
                                                    ((option) => (
                                                        <MenuItem
                                                            key={option.id}
                                                            value={option.id}
                                                            onClick={() => handleSelect(option)}
                                                        >
                                                            {option.nombre}
                                                        </MenuItem>
                                                    ))}
                                            </Menu>
                                        </>
                                    );
                                }}
                            />

                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="idCiudad"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: 'Este campo es requerido',

                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => {
                                    const [anchorEl, setAnchorEl] = useState(null);
                                    const city = cities.find((city) => city.id === Number(value));
                                    const displayValue = city && city.nombre !== "(SIN IDENTIFICAR)" ? city.nombre : '';


                                    const handleOpenMenu = (event: any) => setAnchorEl(event.currentTarget);
                                    const handleCloseMenu = () => setAnchorEl(null);
                                    const handleSelect = (option: any) => {
                                        onChange(option.id);
                                        handleCloseMenu();

                                        formularioEmbargos.setValue('juzgadoId', '');
                                        formularioEmbargos.setValue('codigoInterno', '');
                                        formularioEmbargos.setValue('codigoOficina', '');
                                        formularioEmbargos.setValue('numeroCuenta', '');

                                        const idDepto = formularioEmbargos.getValues('idDepartamento');
                                        obtenerJuzgados(idDepto, option.id);
                                    };

                                    return (
                                        <>
                                            <CHTextField
                                                label="Municipio Juzgado*"
                                                value={displayValue}
                                                disabled
                                                onChange={(e) => {
                                                    const id = e.target.value;

                                                    if (id === '') {
                                                        formularioEmbargos.setValue('juzgadoId', '');
                                                        formularioEmbargos.setValue('codigoInterno', '');
                                                        formularioEmbargos.setValue('codigoOficina', '');
                                                        formularioEmbargos.setValue('numeroCuenta', '');
                                                    } else {
                                                        formularioEmbargos.setValue('juzgadoId', '');
                                                        formularioEmbargos.setValue('codigoInterno', '');
                                                        formularioEmbargos.setValue('codigoOficina', '');
                                                        formularioEmbargos.setValue('numeroCuenta', '');
                                                        const idDepto = formularioEmbargos.getValues('idDepartamento');
                                                        obtenerJuzgados(idDepto, id as unknown as number);
                                                    }

                                                    onChange(id);
                                                }}
                                                onClick={handleOpenMenu}
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                InputProps={{
                                                    endAdornment: (
                                                        <InputAdornment
                                                            position="end"
                                                            style={{ paddingRight: '4px' }}
                                                        >
                                                            <IconButton
                                                                aria-label="open menu"
                                                                style={{ padding: '4px' }}
                                                                onClick={handleOpenMenu}
                                                            >
                                                                <ExpandMoreIcon fontSize="small" />
                                                            </IconButton>
                                                        </InputAdornment>
                                                    ),
                                                }}
                                                fullWidth
                                            />
                                            <Menu
                                                anchorEl={anchorEl}
                                                open={Boolean(anchorEl)}
                                                onClose={handleCloseMenu}
                                            >
                                                {cities?.map((city) => (
                                                    city.id !== 0 && (
                                                        <MenuItem
                                                            key={city.id}
                                                            value={city.id}
                                                            onClick={() => handleSelect(city)}
                                                        >
                                                            {city.nombre}
                                                        </MenuItem>
                                                    )
                                                ))}
                                            </Menu>
                                        </>
                                    );
                                }}
                            />
                        </Grid>

                        <Grid className={classes.buttonD} item xs={12} md={3}>
                            <Controller
                                name="juzgadoId"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido',
                                    },
                                    validate: {
                                        notEmptyUUID: value => value !== '00000000-0000-0000-0000-000000000000' || 'Este campo es requerido',
                                    }
                                }}
                                render={({ field, fieldState: { error } }) => (
                                    <Autocomplete
                                        options={juzgados}
                                        getOptionLabel={(option) => option.nombre}
                                        onChange={(event, value) => {
                                            field.onChange(value ? value.idJuzgado : null);
                                            handleSelectChange(event);
                                        }}
                                        value={field.value ? juzgados.find((juzgado) => juzgado.idJuzgado === field.value) : null}
                                        disableClearable
                                        onInputChange={handleSelectChange}
                                        renderInput={(params) => (
                                            <TextField
                                                {...params}
                                                label="Juzgado *"
                                                name="juzgadoId"
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                disabled
                                            />
                                        )}

                                    />
                                )}
                            />

                        </Grid>

                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="numeroCuenta"
                                control={formularioEmbargos.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Cuenta Judicial"
                                        value={juzgados.length > 0 ? value : 'N/A'}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="codigoOficina"
                                control={formularioEmbargos.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) =>
                                (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Oficina de Destino"
                                        value={juzgados.length > 0 && value !== null ? value.toString().padStart(4, '0') : 'N/A'}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="codigoInterno"
                                control={formularioEmbargos.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Código Interno"
                                        value={juzgados.length > 0 ? value : 'N/A'}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="emailRespuesta"
                                control={formularioEmbargos.control}
                                rules={{
                                    pattern: {
                                        value: /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/,
                                        message: 'Este campo debe ser un correo electrónico válido, no se permite escribir el correo en mayúsculas'
                                    },
                                    validate: {
                                        checkDomain: value => {
                                            const domain = value.split('@')[1];
                                            const validDomains = ['gmail.com', 'hotmail.com', 'yahoo.com', 'outlook.com'];
                                            if (!validDomains.includes(domain)) {
                                                return `Solo se permiten correos electrónicos con los siguientes dominios: ${validDomains.join(', ')}`;
                                            }
                                            return undefined;
                                        },
                                        noSpaces: value => {
                                            if (/\s/.test(value)) {
                                                return 'El correo electrónico no puede contener espacios';
                                            }
                                            return undefined;
                                        },
                                        noDuplicateCom: value => {
                                            if ((value.match(/\.com/g) || []).length > 1) {
                                                return 'El correo electrónico no puede tener más de un ".com"';
                                            }
                                            return undefined;
                                        },
                                        onlyOneCom: value => {
                                            if ((value.match(/\.com/g) || []).length > 1 || value.indexOf('.com') !== value.length - 4) {
                                                return 'El correo electrónico solo puede tener un ".com" y debe estar al final del campo';
                                            }
                                            return undefined;
                                        },
                                        noMoreChars: value => {
                                            if (value.indexOf('.com') !== value.length - 4) {
                                                return 'No se permiten caracteres adicionales después de ".com"';
                                            }
                                            return undefined;
                                        }
                                    }
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        label="Correo Electrónico *"
                                        type="email"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                        onKeyPress={e => {
                                            if (!(e.target instanceof HTMLInputElement)) {
                                                return;
                                            }
                                            const email = e.target.value;
                                            const dotComIndex = email.indexOf(".com");
                                            if (dotComIndex !== -1 && e.key !== "Backspace" && e.key !== "Delete" && e.key !== "ArrowLeft" && e.key !== "ArrowRight") {
                                                e.preventDefault();
                                            }
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="expedienteBancoAgrario"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido y solo acepta números',
                                    },
                                    minLength: {
                                        value: 11,
                                        message: 'No se permite menos de 11 dígitos'
                                    },
                                    maxLength: {
                                        value: 11,
                                        message:
                                            'La longitud máxima del campo es de 11 dígitos',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        type="number"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Número Expedientes *"
                                        value={value}
                                        onChange={e => {
                                            if (e.target.value.length <= 11) {
                                                onChange(e);
                                            }
                                            const resultado = `${formularioEmbargos.getValues(
                                                'codigoInterno',
                                            )}${e.target.value}`;
                                            if (resultado.length <= 11) {
                                                formularioEmbargos.setValue(
                                                    'proceso',
                                                    resultado,
                                                );
                                            }
                                        }}
                                        onKeyPress={(event) => {
                                            const keyCode = event.keyCode || event.which;
                                            const keyValue = String.fromCharCode(keyCode);
                                            const regex = /^[0-9]*$/;
                                            if (!regex.test(keyValue)) {
                                                event.preventDefault();
                                            }
                                        }}

                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="procesoId"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Tipo Proceso *"
                                        value={value}
                                        onChange={onChange}
                                        disabled
                                    >
                                        {tiposProcesos?.map(
                                            (option: TipoProcesoEmbargoDto) => (
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
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="proceso"
                                control={formularioEmbargos.control}
                                rules={{

                                    maxLength: {
                                        value: 23,
                                        message:
                                            'La longitud máxima del campo es de 23 caracteres',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        type="number"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        InputLabelProps={{ shrink: true }}
                                        label="Número Proceso"
                                        onChange={e => {
                                            if (e.target.value.length <= 23) {
                                                onChange(e);
                                            }
                                        }}
                                        value={value}
                                        isReadOnly
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={3} md={3}>
                            <Controller
                                name="desembargar"
                                control={formularioEmbargos.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHCheckbox
                                        label="Desembargar"
                                        onChange={onChange}
                                        checked={value}

                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={12}>
                            <Controller
                                name="observaciones"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: false
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
                                                overflowY: 'auto',
                                            },
                                        }}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Observaciones"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}

                            />
                        </Grid>
                        <Grid container>
                            <Grid className={classes.column} item xs={12} md={6}>
                                <Controller
                                    name="cdp"
                                    control={formularioEmbargos.control}
                                    rules={{
                                        required: false
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
                                                    overflowY: 'auto',
                                                },
                                            }}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            label="CDP"
                                            value={value}
                                            onChange={onChange}
                                        />
                                    )}

                                />
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </CHPaper>
            <Expedientes embargoModel={embargoModel} listaConceptos={listaConceptos} cuentas={cuentas} onValorCalculado={recibirValorDesdeHijo}/>
            <CHPaper title="Demandante">
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Tipo de Identificación</TableCell>
                            <TableCell>Número de Identificación</TableCell>
                            <TableCell>Primer Nombre</TableCell>
                            <TableCell>Segundo Nombre</TableCell>
                            <TableCell>Primer Apellido</TableCell>
                            <TableCell>Segundo Apellido</TableCell>
                            <TableCell>Demandante</TableCell>
                            <TableCell align="center">Acciones</TableCell>
                        </TableRow>
                    </TableHead>

                    <TableBody>
                        {beneficiarios.map((beneficiario, index) => {
                            const beneficiarioActual = beneficiarios[index];
                            const valorDemandante = datosEditados[index]?.demandante ?? beneficiarioActual.demandante;
                            const valorSeleccionado = valorDemandante ? "SI" : "NO";

                            return (
                                <TableRow key={beneficiario.numeroIdentificacion}>
                                    <TableCell>
                                        {modoEdicion === index ? (
                                            <select
                                                value={datosEditados[index]?.tipoDocumentoId || beneficiario.tipoDocumentoId}
                                                onChange={(e) => handleChange(index, 'tipoDocumentoId', e.target.value)}
                                            >
                                                {tiposIdentificacion.map(tipo => (
                                                    <option key={tipo.id} value={tipo.id}>
                                                        {tipo.descripcion}
                                                    </option>
                                                ))}
                                            </select>
                                        ) : (
                                            tiposIdentificacion.find(tipo => tipo.id === beneficiario.tipoDocumentoId)?.descripcion || ''
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {modoEdicion === index ? (
                                            <TextField
                                                value={datosEditados[index]?.numeroIdentificacion || beneficiario.numeroIdentificacion}
                                                onChange={(e) => handleChange(index, 'numeroIdentificacion', e.target.value)}
                                            />
                                        ) : (
                                            beneficiario.numeroIdentificacion
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {modoEdicion === index ? (
                                            <TextField
                                                value={datosEditados[index]?.primerNombre || beneficiario.primerNombre}
                                                onChange={(e) => handleChange(index, 'primerNombre', e.target.value)}
                                            />
                                        ) : (
                                            beneficiario.primerNombre
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {modoEdicion === index ? (
                                            <TextField
                                                value={datosEditados[index]?.segundoNombre || beneficiario.segundoNombre}
                                                onChange={(e) => handleChange(index, 'segundoNombre', e.target.value)}
                                            />
                                        ) : (
                                            beneficiario.segundoNombre || ''
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {modoEdicion === index ? (
                                            <TextField
                                                value={datosEditados[index]?.primerApellido || beneficiario.primerApellido}
                                                onChange={(e) => handleChange(index, 'primerApellido', e.target.value)}
                                            />
                                        ) : (
                                            beneficiario.primerApellido
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {modoEdicion === index ? (
                                            <TextField
                                                value={datosEditados[index]?.segundoApellido || beneficiario.segundoApellido}
                                                onChange={(e) => handleChange(index, 'segundoApellido', e.target.value)}
                                            />
                                        ) : (
                                            beneficiario.segundoApellido || ''
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {modoEdicion === index ? (
                                            <select
                                                value={valorSeleccionado}
                                                onChange={(e) => handleChange(index, 'demandante', e.target.value === "SI")}
                                            >
                                                <option value="SI">SI</option>
                                                <option value="NO">NO</option>
                                            </select>
                                        ) : (
                                            valorSeleccionado
                                        )}
                                    </TableCell>
                                    <TableCell align="center">
                                        {modoEdicion === index ? (
                                            <IconButton color="primary" onClick={() => handleGuardar(index)}>
                                                <Save />
                                            </IconButton>
                                        ) : (
                                            <IconButton color="primary" onClick={() => setModoEdicion(index)}>
                                                <Edit />
                                            </IconButton>
                                        )}
                                        <IconButton color="secondary" onClick={() => eliminarRegistro(beneficiario.numeroIdentificacion)}>
                                            <Delete />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            );
                        })}
                    </TableBody>

                </Table>
            </CHPaper>
            <CHPaper title="Información de Pago">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoDocumentoId"
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            control={formularioBeneficiarios.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo de Identificación *"
                                    value={value}
                                    onFocus={activarFormularioPago}
                                    onChange={(e) => {
                                        onChange(e);
                                        const selectedValue = e.target.value;
                                        onChange(selectedValue);
                                        formularioBeneficiarios.setValue("tipoDocumentoId", selectedValue, { shouldValidate: true, shouldDirty: true });
                                        handleTipoDocumentoChange(e);

                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                >
                                    {tiposIdentificacion?.map(
                                        (option: IDENTIFICATIONTYPEDTO) => (
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
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroIdentificacion"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },

                                maxLength: {
                                    value: 12,
                                    message:
                                        'La longitud máxima del campo es de 12 caracteres',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        maxLength: 12,
                                    }}
                                    label="No. Identificación *"
                                    value={value}
                                    onFocus={activarFormularioPago}
                                    onChange={e => {
                                        if (
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 1 ||
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 2 ||
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 5
                                        ) {
                                            if (e.target.value.length <= 12) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[a-zA-ZñÑátipoDocumentoIdéíóúÁÉÍÓÚ\W]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        } else {
                                            onChange(e);
                                        }

                                    }}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        const regex = /^[0-9]*$/;
                                        if (!regex.test(keyValue)) {
                                            event.preventDefault();
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="confnumeroIdentificacion"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },

                                maxLength: {
                                    value: 12,
                                    message:
                                        'La longitud máxima del campo es de 12 caracteres',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        maxLength: 12,
                                    }}
                                    label="Confirmación No. Identificación *"
                                    value={value}
                                    onChange={e => {
                                        if (
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 1 ||
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 2 ||
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 5
                                        ) {
                                            if (e.target.value.length <= 12) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        } else {
                                            onChange(e);
                                        }
                                    }}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        const regex = /^[0-9]*$/;
                                        if (!regex.test(keyValue)) {
                                            event.preventDefault();
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreRazonSocial"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                                maxLength: {
                                    value: 25,
                                    message:
                                        'La longitud máxima del campo es de 25 caracteres',
                                },
                            }}
                            render={({ field: { onChange, value } }) => (


                                <CHTextField
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="Nombre / Razón Social *"
                                    value={value}
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
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoCuenta"
                            control={formularioBeneficiarios.control}
                            rules={{ required: 'Este campo es requerido' }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    label="Tipo de Cuenta *"
                                    value={value ?? ''}
                                    error={!!error}
                                    helperText={error?.message ?? ''}
                                    onChange={(e) => {
                                        // const selectedValue = e.target.value;
                                        const idTipoCuenta = e.target.value as number;
                                        onChange(idTipoCuenta);
                                        formularioBeneficiarios.setValue("tipoCuenta", idTipoCuenta, {
                                            shouldValidate: true,
                                            shouldDirty: true,
                                        });
                                        handleTipoCuentaChange(e);
                                        setValidarValorType(idTipoCuenta);
                                        setTipoCuentaSeleccionada2(idTipoCuenta);
                                    }}
                                >
                                    {entidadesCuentas?.map((option: VALORCATALOGO) => (
                                        <MenuItem key={option.idValor} value={option.idValor}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="entidad"
                            control={formularioBeneficiarios.control}
                            rules={{ required: 'Este campo es requerido' }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    label="Entidad *"
                                    value={value ?? ''}
                                    error={!!error}
                                    helperText={error?.message ?? ''}
                                    onChange={(e) => {
                                        const selectedValue = e.target.value;
                                        onChange(selectedValue);
                                        formularioBeneficiarios.setValue("entidad", selectedValue, {
                                            shouldValidate: true,
                                            shouldDirty: true,
                                        });
                                        handleEntidadChange(e);
                                    }}
                                >
                                    {entidadesBancarias?.map((option: ENTIDADBANCARIA) => (
                                        <MenuItem key={option.idEntidadBancaria} value={option.idEntidadBancaria}>
                                            {option.nombreRazonSocial}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    {validarValorTipo !== 322 && (
                        <>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="numeroCuenta"
                                    control={formularioBeneficiarios.control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido',
                                        },

                                        maxLength: {
                                            value: 21,
                                            message:
                                                'La longitud máxima del campo es de 21 caracteres',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            label="N° Cuenta *"

                                            value={value}
                                            onChange={e => {
                                                if (
                                                    formularioBeneficiarios.getValues(
                                                        'tipoDocumentoId',
                                                    ) === 1 ||
                                                    formularioBeneficiarios.getValues(
                                                        'tipoDocumentoId',
                                                    ) === 2 ||
                                                    formularioBeneficiarios.getValues(
                                                        'tipoDocumentoId',
                                                    ) === 5
                                                ) {
                                                    if (e.target.value.length <= 21) {
                                                        onChange(
                                                            e.target.value.replace(
                                                                /[a-zA-ZñÑátipoDocumentoIdéíóúÁÉÍÓÚ\W]/g,
                                                                '',
                                                            ),
                                                        );
                                                    }
                                                } else {
                                                    onChange(e);
                                                }
                                                consultarCliente(formularioBeneficiarios.watch('tipoDocumentoId'), e.target.value)
                                            }}
                                            onKeyPress={(event) => {
                                                const keyCode = event.keyCode || event.which;
                                                const keyValue = String.fromCharCode(keyCode);
                                                const regex = /^[0-9]*$/;
                                                if (!regex.test(keyValue)) {
                                                    event.preventDefault();
                                                }
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                        </>
                    )}
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="confNumeroCuenta"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: validarValorTipo === 322
                                        ? 'El Número de Obligación es obligatorio.'
                                        : 'La Confirmación del Número de Cuenta es obligatoria.'
                                },
                                maxLength: {
                                    value: 21,
                                    message: 'La longitud máxima del campo es de 21 caracteres',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label={validarValorTipo === 322 ? " N° Obligación" : " Confirmación N° Cuenta"}
                                    value={value}
                                    onChange={e => {
                                        if (
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 1 ||
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 2 ||
                                            formularioBeneficiarios.getValues(
                                                'tipoDocumentoId',
                                            ) === 5
                                        ) {
                                            if (e.target.value.length <= 21) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        } else {
                                            onChange(e);
                                        }
                                        consultarCliente(formularioBeneficiarios.watch('tipoDocumentoId'), e.target.value)
                                    }}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        const regex = /^[0-9]*$/;
                                        if (!regex.test(keyValue)) {
                                            event.preventDefault();
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorPorcentaje"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: mostrarPorcentajes ? 'Este campo es requerido' : false,
                            }}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    label="Valor o Porcentaje"
                                    value={value ?? "0"}
                                    onChange={(e) => {
                                        formularioEmbargos.setValue('valor', null);
                                        const selectedValue = e.target.value;
                                        onChange(selectedValue);

                                        if (selectedValue === 'PORCENTAJE') {
                                            setMostrarPorcentajes(true);
                                        } else {
                                            setMostrarPorcentajes(false);
                                        }
                                    }}
                                >
                                    {tiposEmbargos?.map(
                                        (option: TipoEmbargoDto) => (
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
                    {/* <Grid className={classes.column} item xs={12} md={3}>
                        {mostrarPorcentajes ? (
                            <Controller
                                name="valor"
                                control={formularioBeneficiarios.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                    validate: (value) => {
                                        if (mostrarPorcentajes && Number(value) > 100) {
                                            return 'El valor no debe ser mayor a 100';
                                        }
                                        return true;
                                    },
                                    min: {
                                        value: 1,
                                        message:
                                            'El valor debe ser mayor a cero',
                                    },
                                    max: {
                                        value: 100,
                                        message:
                                            'El valor no debe ser mayor a 100',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        label="Valor a pagar"
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={(e) => {
                                            const inputValue = e.target.value;
                                            onChange(inputValue);
                                        }}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        {...(mostrarPorcentajes ? { inputProps: { style: { textAlign: 'right' }, endAdornment: '%' } } : {})}
                                        percentMask
                                    />
                                )}
                            />
                        ) : (

                            <Controller
                                name="valor"
                                control={formularioBeneficiarios.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido.',
                                    },
                                    max: {
                                        value: valorCalculado,
                                        message: 'El valor no debe ser mayor al saldo disponible.',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => {

                                    useEffect(() => {
                                        if (value !== undefined && value !== null && !Number.isNaN(value)) {
                                            const visual = `$ ${new Intl.NumberFormat('es-CO').format(value)}`;
                                            setValorVisual(visual);
                                        } else {
                                            setValorVisual('');
                                        }
                                    }, [value]);

                                    return (
                                        <CHTextField
                                            label="Valor a pagar"
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            value={valorVisual}
                                            onChange={(e) => {
                                                const input = e.target.value;
                                                const clean = input.replace(/[^\d]/g, '');
                                                const numeric = Number(clean);

                                                onChange(numeric);

                                                const formatted = clean
                                                    ? `$ ${new Intl.NumberFormat('es-CO').format(numeric)}`
                                                    : '';
                                                setValorVisual(formatted);
                                            }}
                                        />
                                    );
                                }}
                            />
                        )}
                    </Grid> */}
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valor"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: 'Este campo es requerido',
                                validate: (value) => {
                                    if (mostrarPorcentajes && Number(value) > 100) {
                                        return 'El valor no debe ser mayor a 100';
                                    }
                                    if (!mostrarPorcentajes && Number(value) > valorCalculado) {
                                        return 'El valor no debe ser mayor al saldo disponible.';
                                    }
                                    return true;
                                },
                                min: {
                                    value: 1,
                                    message: 'El valor debe ser mayor a cero',
                                }
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    label="Valor a pagar"
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    value={valorVisual}
                                    onChange={(e) => {
                                        const input = e.target.value.replace(/[^\d]/g, '');
                                        const numeric = Number(input);

                                        onChange(numeric);

                                        const visual = mostrarPorcentajes
                                            ? `${numeric} %`
                                            : `$ ${new Intl.NumberFormat('es-CO').format(numeric)}`;

                                        setValorVisual(visual);
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
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
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={() => {
                                if (formularioActivo === 'pago') {
                                    const valoresFormulario = formularioBeneficiarios.getValues();


                                    const valorCrudo = formularioBeneficiarios.watch("valor");
                                    let valorIngresado = 0;

                                    if (typeof valorCrudo === "string" && valorCrudo.trim() !== "") {
                                        valorIngresado = parseFloat(valorCrudo.replace(/[^\d.,-]/g, '').replace(',', '.')) || 0;
                                    } else if (typeof valorCrudo === "number") {
                                        valorIngresado = valorCrudo;
                                    } else if (resultadoSaldo) {
                                        valorIngresado = parseFloat(resultadoSaldo) || 0;
                                    }

                                    const nuevoItem = {
                                        ...valoresFormulario,
                                        valor: valorIngresado,
                                    };

                                    if (editarIndex !== null) {
                                        const copia = [...nuevoBeneficiario];
                                        copia[editarIndex] = nuevoItem;
                                        setNuevoBeneficiario(copia);
                                        setEditarIndex(null);
                                        setEstadoBoton('AGREGAR BENEFICIARIO');
                                    } else {
                                        setNuevoBeneficiario(prev => [...prev, nuevoItem]);
                                    }

                                    cancelarBeneficiario();
                                    resetBeneficiarioNuevo();
                                }
                            }}
                            disabled={formularioActivo !== 'pago'}
                        >
                            {estadoBoton}
                        </Button>

                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={cancelarBeneficiario}
                        >
                            CANCELAR
                        </Button>
                    </Grid>
                </Grid>
                <br />
                <h3>Informacion De Pago</h3>
                <br />
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Tipo ID</TableCell>
                            <TableCell>Número de Identificación</TableCell>
                            <TableCell>Nombre / Razón Social</TableCell>
                            <TableCell>Tipo Cuenta</TableCell>
                            <TableCell>Entidad</TableCell>
                            <TableCell>Número de Cuenta</TableCell>
                            <TableCell>Número de Obligación</TableCell>
                            <TableCell>Valor a retirar</TableCell>
                            <TableCell>Acciones</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {nuevoBeneficiario.map((beneficiario, index) => (
                            <TableRow key={index}>
                                <TableCell> {tiposIdentificacion.find(option => option.id === beneficiario.tipoDocumentoId)?.descripcion || "No encontrado"}</TableCell>
                                <TableCell>{beneficiario.numeroIdentificacion}</TableCell>
                                <TableCell>{beneficiario.nombreRazonSocial}</TableCell>
                                <TableCell>{entidadesCuentas
                                    .filter(option => beneficiario.tipoCuenta === option.idValor)
                                    .map(option => option.descripcion)}</TableCell>
                                <TableCell>
                                    {entidadesBancarias
                                        .filter(option => beneficiario.entidad === option.idEntidadBancaria)
                                        .map(option => option.nombreRazonSocial)}</TableCell>
                                <TableCell>{beneficiario.numeroCuenta}</TableCell>
                                <TableCell>{beneficiario.confNumeroCuenta}</TableCell>
                                <TableCell>{formatCurrency(beneficiario.valor || resultadoSaldo)}</TableCell>
                                <TableCell align="center">
                                    <IconButton
                                        color="primary"
                                        onClick={() => seleccionarRegistro(beneficiario, index)}
                                    >
                                        <Edit />
                                    </IconButton>

                                    <IconButton
                                        aria-label="Eliminar"
                                        color="primary"
                                        onClick={() => {
                                            eliminarRegistroPago(beneficiario.numeroIdentificacion);
                                        }}
                                    >
                                        <Delete />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </CHPaper>
            <CHPaper title="Conceptos a embargar">
                <CuentasList
                    cuentas={cuentas}
                    handleOpenModal={handleOpenModal}
                    handleCloseModal={handleCloseModal}
                    listaConceptos={listaConceptos}
                    setListaConceptos={setListaConceptos}
                    tiposEmbargos={tiposEmbargos}
                    tiposRetencion={tiposRetencion}
                    conceptosData={conceptosData}
                    modoActualizar={modoActualizar}
                    porcentaje={porcentaje}
                    variacionPorcentaje={variacionPorcentaje}
                    handleOnChangeMostrarPorcentaje={handleOnChangeMostrarPorcentaje}
                    setCuentas={setCuentas}
                    estadoCuentaEmbargo={estadoCuentaEmbargo}
                    setConcepto={setConcepto}
                    soloVisual
                />
            </CHPaper>
            <CHPaper title="Observaciones">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="observacionesAdicionales"
                            control={formularioEmbargos.control}
                            rules={{
                                required: false
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    multiline
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                            overflowY: 'auto',
                                        },
                                    }}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="Observaciones"
                                    placeholder='Si tiene observaciones adicionales ingreselas'
                                    value={value}
                                    onChange={onChange}
                                />
                            )}

                        />
                    </Grid>
                </Grid>
            </CHPaper>
            <Grid
                className={classes.columnButtonAdd}
                item
                xs={12}
                md={12}
            >
                <Button
                    className={classes.button}
                    variant="contained"
                    color="primary"
                    style={{ width: '200px' }}
                    onClick={handleModalOpen}
                >
                    {botonTexto}
                </Button>
            </Grid>
            <Grid
                className={classes.columnButtonAdd}
                item
                xs={12}
                md={12}
            >
                <Button
                    className={classes.button}
                    variant="contained"
                    color="primary"
                    style={{ width: '200px' }}
                    onClick={handleOficioRespuesta}
                >
                    {botonOficio}
                </Button>
            </Grid>
            <ModalPagoEmbargos
                open={modalOpen}
                handleClose={handleModalClose}
                beneficiarioSeleccionado={beneficiarioSeleccionado}
                tipoDocumentoSeleccionado={tipoDocumentoSeleccionado}
                entidadSeleccionada={entidadSeleccionada}
                tipoCuentaSeleccionada={tipoCuentaSeleccionada}
                formularioEmbargos={formularioEmbargos}
                onPagoExitoso={handleCambioTextoBoton}
                nuevoBeneficiario={nuevoBeneficiario}
                idEntidad={idEntidad}
                tipoCuentaSeleccionada2={tipoCuentaSeleccionada2}
                setVista={setVista}
            />
        </>
    )
}

interface LinkMapDispatchToProps {
    formularioEmbargos: UseFormReturn<any>;
    formularioPagoEmbargos: UseFormReturn<any>;
    formularioBeneficiarios: UseFormReturn<any>;
    beneficiarios: BeneficiarioDto[];
    tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
    departamentos: DEPARTAMENTODTO[];
    cities: CITYDTO[];
    juzgados: JuzgadoDto[];
    datajuzgado: any;
    tiposProcesos: TipoProcesoEmbargoDto[];
    tiposEmbargos: TipoEmbargoDto[];
    tiposRetencion: TipoRetencionDto[];
    modoActualizar: boolean;
    beneficiariosData: BeneficiarioDto[];
    listaConceptos: ConceptoEmbargosDto[];
    cuentas: CuentaModelDto[];
    conceptosData: ConceptoEmbargosDto[];
    porcentaje: string;
    variacionPorcentaje: string;
    tipoDocumento: number;
    mostrarPorcentaje: any,
    setErrorPorcentaje: any,
    setMensaje: any,
    errorPorcentaje: any,
    mensaje: any,
    valor: any,
    formularioConceptos: UseFormReturn<any>;
    obtenerCiudades: (idDepto: number) => void;
    obtenerJuzgados: (idDepto: number, idCiudad: number) => void;
    setBeneficiarios: (beneficiarios: BeneficiarioDto[]) => void;
    handleOpenModal: (conceptoId: number) => void;
    setListaConceptos: (listaConceptos: ConceptoEmbargosDto[]) => void;
    setTipoDocumento: (value: number) => void;
    setDocumento: (value: string) => void;
    consultarCuentaConcepto: () => void;
    handleOnChangeMostrarPorcentaje: (value: boolean) => void;
    consultarJuzgado: (idJugado: string, codigoInterno: string) => void;
    consultarCliente: (idTipoIdentificacion: number, identificacion: string) => void;
    setCuentas: (data: any) => void;
    embargoModel: EmbargoModelDto[];
    handleCloseModal: any;
    estadoCuentaEmbargo: any;
    setConcepto: ConceptoEmbargosDto[];
    onValorCalculado: (valor: number) => void;
    setVista: () => void;
}

type props = LinkMapDispatchToProps;



export default InformacionPagoEmbargos;