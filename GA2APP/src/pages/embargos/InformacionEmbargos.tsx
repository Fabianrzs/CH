import React, { useEffect, useRef, useState } from 'react';
import {
    Box,
    Button,
    Checkbox,
    createStyles,
    FormControlLabel,
    Grid,
    IconButton,
    InputAdornment,
    makeStyles,
    Menu,
    MenuItem,
    Tab,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Tabs,
    TextareaAutosize,
    TextField,
    Typography,
} from '@material-ui/core';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import { Delete, Edit, Add } from '@material-ui/icons';
import ClearIcon from '@material-ui/icons/Clear';

import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import AttachFileIcon from '@material-ui/icons/AttachFile';
import jsPDF from "jspdf";
import Autocomplete from '@material-ui/lab/Autocomplete/Autocomplete';
import { connect } from 'react-redux';
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
    EmbargoDto,
} from '../../types/embargo.model.type';
import {
    CITYDTO,
    DEPARTAMENTODTO,
    IDENTIFICATIONTYPEDTO,
} from '../../types/catalog.type';
import { DespachosJudiciales, JuzgadoDto } from '../../types/court.model.type';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import NotificationService from '../../services/notification.service';
import CHPaper from '../../components/ch-paper/CHPaper';
import CuentasList from './cuentasList/CuentasList';
import GenerarGuid from '../solicitudes/components/funciones/guid.funtions';
import history from '../../utils/history.router'
import AddressComponent from '../../components/modals/address/address.component';
import { setFormDataAddres } from '../productClient/components/address.component.constants';
import { Address } from '../../types/address.type';
import { ModalPreVisualRespuesta } from './components/ModalPreVisualRespuesta';
import { ModalCourtAdd } from '../court/components/ModalCourtAdd';
import { juzgadoForm } from '../court/constants/courtform.model.type';
import { startPostInsertCourt, startPostUpdateCourt } from '../../actions/courts.action';
import { GUID_DEFAULT } from '../../constants/constants';
import ApiCatalog from '../../services/api-catalog.service';
import CHAutoComplete from '../../components/ch-auto-complete/CHAutoComplete';



const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnButtonAdd2: {
            marginLeft: '53rem',
            marginTop: '-4rem',
            textAlign: 'center',
        },
        button: {
            width: '60%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        buttonD: {
            padding: '10px',
            textAlign: 'center',
            marginTop: '15px',
        },
        buttonA: {
            padding: '10px',
            textAlign: 'center',
            marginTop: '28px',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        buttonmax: {
            width: '20%',
            marginRight: '1rem',
            marginTop: '1.5rem',
        },
        thickerIcon: {
            fontSize: '30px',
            fontWeight: 'bold',
        }

    }),
);




const InformacionEmbargos = (props: props) => {


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
        tipoDocumento,
        obtenerCiudades,
        obtenerJuzgados,
        setBeneficiarios,
        handleOpenModal,
        handleCloseModal,
        setListaConceptos,
        setTipoDocumento,
        setDocumento,
        consultarCuentaConcepto,
        handleOnChangeMostrarPorcentaje,
        consultarJuzgado,
        consultarCliente,
        setCuentas,
        estadoCuentaEmbargo,
        setConcepto,
        nombreUnno,
        nombreDoss,
        apellidoUnno,
        apellidoDoss,
        saldoConcepto
    } = props;

    const { startPostInsertCourt, startPostUpdateCourt } = props;



    const classes = useStyles();
    const [estadoBoton, setEstadoBoton] = useState<string>(
        'AGREGAR BENEFICIARIO',
    );
    const [beneficiariosModel, setBeneficiariosModel] = useState<
        BeneficiarioDto[]
    >([]);
    const [beneficiarioAnterior, setBeneficiarioAnterior] =
        useState<string>('');
    const [idAnterior, setIdAnterior] = useState<string>('');
    const [tipoRadicado, setTipoRadicado] = useState(0);

    let identificacion: IDENTIFICATIONTYPEDTO | undefined = {
        id: 0,
        codigo: '',
        descripcion: '',
        esEmpresarial: false,
        esActivo: false,
        ERP: 0,
        vigia: 0,
        embargo: 0,
    };

    useEffect(() => {

        if (modoActualizar) {
            if (beneficiariosData && beneficiariosData.length > 0) {
                setBeneficiariosModel(beneficiariosData);
            }
        }
        obtenerTipoActorEmbargo();
    }, []);

    const formulario = useForm<JuzgadoDto>({
        defaultValues: juzgadoForm,
    });
    const handleOpenModalAdd = async () => {
        formulario.reset();
        setOpenModalAdd(true);
    };

    const handleCloseModalAdd = async () => {
        setOpenModalAdd(false);
    };
    const handleAdd = async (data: JuzgadoDto) => {
        data.idJuzgado = GUID_DEFAULT;
        startPostInsertCourt(data);
        handleCloseModalAdd();
    };

    const handleEdit = async (data: JuzgadoDto) => {
        await startPostUpdateCourt(data);
        handleCloseModalAdd();
    };
    const seleccionarRegistro = async (item: BeneficiarioDto) => {
        setBeneficiarioAnterior(item.numeroIdentificacion);
        setIdAnterior(item.numeroIdentificacion);
        formularioBeneficiarios.setValue('id', item.id);
        formularioBeneficiarios.setValue(
            'tipoDocumentoId',
            item.tipoDocumentoId,
        );
        formularioBeneficiarios.setValue(
            'numeroIdentificacion',
            item.numeroIdentificacion,
        );
        formularioBeneficiarios.setValue('primerNombre', item.primerNombre);
        formularioBeneficiarios.setValue('segundoNombre', item.segundoNombre);
        formularioBeneficiarios.setValue('primerApellido', item.primerApellido);
        formularioBeneficiarios.setValue(
            'segundoApellido',
            item.segundoApellido,
        );
        formularioBeneficiarios.setValue('demandante', item.demandante);
        formularioBeneficiarios.setValue('bpE_BENEFICIARIO', item.bpE_BENEFICIARIO);
        setEstadoBoton('EDITAR BENEFICIARIO');
    };

    const cancelarBeneficiario = async () => {
        setEstadoBoton('AGREGAR BENEFICIARIO');
        formularioBeneficiarios.setValue('id', '');
        formularioBeneficiarios.setValue('tipoDocumentoId', '');
        formularioBeneficiarios.setValue('numeroIdentificacion', '');
        formularioBeneficiarios.setValue('primerNombre', '');
        formularioBeneficiarios.setValue('segundoNombre', '');
        formularioBeneficiarios.setValue('primerApellido', '');
        formularioBeneficiarios.setValue('segundoApellido', '');
        formularioBeneficiarios.setValue('demandante', false);
        formularioBeneficiarios.setValue('bpE_BENEFICIARIO', false);
        formularioBeneficiarios.reset();
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
        formularioBeneficiarios.setValue('bpE_BENEFICIARIO', false);
    };
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

    const agregarRegistroBeneficiario = async (data: any) => {
        const guid = GenerarGuid();
        if (data.tipoDocumentoId === 8) {
            const camposProhibidos = [
                data.segundoNombre?.trim(),
                data.primerApellido?.trim(),
                data.segundoApellido?.trim(),
            ];

            const hayCamposProhibidos = camposProhibidos.some(campo => campo && campo.length > 0);

            if (hayCamposProhibidos) {
                toast('Solo se permite ingresar el primer nombre con este tipo de documento.', {
                    type: toast.TYPE.ERROR,
                });
                return;
            }
        }
        if (data.tipoDocumentoId === 0) {
            formularioBeneficiarios.register('tipoDocumentoId', {
                required: true,
            });
            formularioBeneficiarios.register('tipoDocumentoId', {
                required: 'Este campo es requerido',
            });
        } else {
            const beneficiarioNuevo = {
                id: guid.newGuid(),
                tipoDocumentoId: data.tipoDocumentoId,
                numeroIdentificacion: data.numeroIdentificacion,
                primerNombre: data.primerNombre,
                segundoNombre: data.segundoNombre,
                primerApellido: data.primerApellido,
                segundoApellido: data.segundoApellido,
                demandante: data.demandante,
                bpE_BENEFICIARIO: data.bpE_BENEFICIARIO,
                insert: true,
            };
            let beneficiarios: BeneficiarioDto[] = [];
            if (estadoBoton === 'AGREGAR BENEFICIARIO') {
                setBeneficiariosModel(oldArray => [
                    ...oldArray,
                    beneficiarioNuevo,
                ]);
                beneficiarios = [...beneficiariosModel];
                beneficiarios.push(beneficiarioNuevo);
            } else {
                const temp = [...beneficiariosModel];
                const posicionActual = temp.findIndex(
                    item => item.numeroIdentificacion === beneficiarioAnterior,
                );
                beneficiarioNuevo.insert = false;
                beneficiarioNuevo.id = temp[posicionActual].id;
                if (
                    beneficiarioNuevo.numeroIdentificacion !==
                    temp[posicionActual].numeroIdentificacion
                ) {
                    temp[posicionActual].numeroIdentificacion =
                        beneficiarioNuevo.numeroIdentificacion;
                } else {
                    beneficiarioNuevo.numeroIdentificacion = idAnterior;
                }
                temp.splice(posicionActual, 1);
                temp.splice(posicionActual, 0, beneficiarioNuevo);
                beneficiarios = temp;
                setBeneficiariosModel(temp);
            }
            setBeneficiarios(beneficiarios);
            setBeneficiarioAnterior('');
            await resetBeneficiarioNuevo();
        }
    };


    const agregarBeneficiario = async (data: any) => {
        const { demandante, beneficiarioPago, numeroIdentificacion } = data;
        const demandanteExistente = beneficiariosModel.some(item => item.demandante);

        if (!demandanteExistente && !demandante) {
            toast('Debe seleccionar primero un Demandante', {
                type: toast.TYPE.ERROR,
            });
            return;
        }

        const datoEncontrado = beneficiariosModel.findIndex(
            item =>
                item.numeroIdentificacion === numeroIdentificacion &&
                item.numeroIdentificacion !== beneficiarioAnterior,
        );

        if (datoEncontrado === -1) {
            await agregarRegistroBeneficiario(data);
            formularioBeneficiarios.reset();
        } else {
            toast('El beneficiario ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };




    const today = new Date();

    const [chTextFieldEnabled, setCHTextFieldEnabled] = useState(false);

    useEffect(() => {
        setInconsistencia(!formularioEmbargos.getValues('Inconsistencia'));
        formularioEmbargos.setValue('proceso', '')
    }, [formularioEmbargos.watch('idDepartamento')])

    useEffect(() => {
        formularioEmbargos.setValue('proceso', '')
    }, [formularioEmbargos.watch('idCiudad')])

    useEffect(() => {
        const resultado = `${formularioEmbargos.getValues(
            'codigoInterno',
        )}${formularioEmbargos.watch('expedienteBancoAgrario')}`;
        if (resultado.length <= 23) {
            formularioEmbargos.setValue(
                'proceso',
                resultado,
            );
        }
    }, [formularioEmbargos.watch('expedienteBancoAgrario')])

    const [selectedTipoDocumento, setSelectedTipoDocumento] = useState<number>(0);
    const { control, getValues, watch, setValue } = useForm();
    const [open, setOpen] = useState(false);
    const [direccionValue, setDireccionValue] = useState('');
    const [selectedValue, setSelectedValue] = useState('');
    const [isReadOnly, setIsReadOnly] = useState(false);
    const [openModalAdd, setOpenModalAdd] = useState(false);
    const [allTipoActor, setAllTipoActor] = useState<any[]>([]);
    const [preRespuesta, setPreRespuesta] = useState(false);
    const [selectedTab, setSelectedTab] = useState(0);
    const tipoDocument = formularioBeneficiarios.getValues('tipoDocumentoId');

    const [esTipoEspecial, setEsTipoEspecial] = useState(false);



    const handleTabChange = (event, newValue) => {
        setSelectedTab(newValue);
    };
    const api = ApiCatalog();
    const obtenerTipoActorEmbargo = async () => {
        await api
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
                toast('Error al obtener los tipos de actor');
            });
    };

    useEffect(() => {
        if (direccionValue === '' && formularioEmbargos.watch('direccion') !== '') {
            setDireccionValue(formularioEmbargos.watch('direccion'))
        }
    }, [formularioEmbargos.watch('direccion')])

    const handleOpenAddress = () => {
        setOpen(true);
    };

    const onClose = (value: Address) => {
        setDireccionValue('')
        formularioEmbargos.setValue('direccion', '');
        if (value != null) {
            const result = setFormDataAddres(value);
            setDireccionValue(result)
            formularioEmbargos.setValue('direccion', result);
        }

        setOpen(false);
    };

    useEffect(() => {
        setIsReadOnly(false);
    }, [juzgados]);


    const handleSelectChange = async (e: any) => {

        const expediente = await formularioEmbargos.getValues('expedienteBancoAgrario');
        const juzgado = await formularioEmbargos.getValues('juzgadoId');
        if (juzgado) {
            setSelectedValue(juzgado);
            consultarJuzgado(juzgado, expediente);
        }

    };

    const [tipoDocumentoSeleccionado, setTipoDocumentoSeleccionado] = useState<any>('');
    const [nombreUno, setNombreUno] = useState();
    const [nombreDos, setNombreDos] = useState();
    const [apellidoUno, setApellidoUno] = useState('');
    const [apellidoDos, setApellidoDos] = useState('');
    const [nombreDepartamento, setNombreDepartamento] = useState('');
    const [nombreMunicipio, setNombreMunicipio] = useState('');
    const [juzgado, setJuzgado] = useState('');
    const [tipoProceso, setTipoProceso] = useState('');
    const [linkDodo, setLinkDodo] = useState('')
    const [inconsistencia, setInconsistencia] = useState(true);

    useEffect(() => {
        obtenerUrlDodo();
        setInconsistencia(true);
    }, [formularioEmbargos.watch('radicadoWorkManager')])

    useEffect(() => {
        const ProcesoId = formularioEmbargos.getValues('procesoId')
        if (ProcesoId === "00000000-0000-0000-0000-000000000000") {
            formularioEmbargos.setValue('procesoId', '');
        }
    }, [formularioEmbargos.watch('procesoId')])





    const obtenerUrlDodo = async () => {

        const radicado = formularioEmbargos.getValues('radicadoWorkManager');
        const enlaceCompleto = `http://cpvmp-112p:91/LinkFormulario/FormularioWM/Index?columna=Cod_Barras&valorColumna=${radicado}&idFormulario=20058`;
        setLinkDodo(enlaceCompleto);

    }
    const obtenerApellidos = (value: any) => {
        const nombresApellidos = value ? value.split(' ') : [];
        let apellidos = '';

        if (nombresApellidos.length > 1) {
            if (nombresApellidos[1].toLowerCase() === 'de la hoz' && nombresApellidos.length > 3) {
                apellidos = nombresApellidos.slice(1, 5).join(' ');
            } else {
                apellidos = nombresApellidos.slice(2, 3).join(' ');
            }
        }

        return apellidos;
    };

    const obtenerSegundoApellido = (value: any) => {
        const nombresApellidos = value ? value.split(' ') : [];
        let apellidos = '';
        if (nombresApellidos.length > 3) {
            if (nombresApellidos[2].toLowerCase() === 'de' && nombresApellidos.length > 5) {
                apellidos = nombresApellidos.slice(2, 6).join(' ');
            } else if (nombresApellidos[2].toLowerCase() === 'la' && nombresApellidos.length > 5) {
                apellidos = nombresApellidos.slice(2, 6).join(' ');
            } else if (nombresApellidos[2].toLowerCase() === 'hoz' && nombresApellidos.length > 5) {
                apellidos = nombresApellidos.slice(2, 6).join(' ');
            } else {
                apellidos = nombresApellidos.slice(3, 6).join(' ');
            }


            return apellidos;
        }

        return '';
    }


    const idDepartamento = formularioEmbargos.watch('idDepartamento');
    const departamentoSeleccionado = departamentos.find(option => option.id === parseInt(idDepartamento, 10));
    const idMunicipio = formularioEmbargos.watch('idCiudad');
    const municipioSeleccionado = cities.find(option => option.id === parseInt(idMunicipio, 10));
    const dataJuzgados = formularioEmbargos.watch('juzgadoId');
    const juzgadoSeleccionado = Array.isArray(juzgados)
        ? juzgados?.find(option => option?.idJuzgado === dataJuzgados)
        : null;
    const procesoSeleccionado = formularioEmbargos.watch('procesoId');
    const tipoDeProceso = tiposProcesos.find(option => option.id === procesoSeleccionado)?.descripcion || '';


    const handleDepartamento = () => {
        if (departamentoSeleccionado) {
            setNombreDepartamento(departamentoSeleccionado.nombre);
        }
    };

    const handleMunicipio = () => {
        if (municipioSeleccionado) {
            setNombreMunicipio(municipioSeleccionado.nombre);
        }
    };

    const handleJuzgado = () => {
        if (juzgadoSeleccionado) {
            setJuzgado(juzgadoSeleccionado.nombre)

        }
    }

    const handleProcesoId = () => {
        if (tipoDeProceso) {
            setTipoProceso(tipoDeProceso)
        }
    }


    const [formData, setFormData] = useState({
        numeroSolicitud: 0,
        tipoIdentificacion: "",
        identificacionCliente: 0,
        nombreCliente: "",
        radicadoWorkManager: "",
        direccion: "",
        fechaRadicadoWorkManager: new Date(),
        radicadoJuzgado: 0,
        fechaRadicadoJuzgado: new Date(),
        idDepartamento: "",
        idCiudad: "",
        juzgadoId: ""
    });

    const handleNumeroSolicitudChange = (value: string) => {
        setFormData((prevData) => ({
            ...prevData,
            numeroSolicitud: value !== '' ? parseInt(value, 10) : 0,
        }));
    };

    const handleTipoIdentificacionChange = (e: any) => {
        const selectedValue = e?.target?.value;

        const selectedOption = tiposIdentificacion.find((option: IDENTIFICATIONTYPEDTO) => option.id === selectedValue);

        if (selectedOption) {
            setTipoDocumentoSeleccionado({
                id: selectedOption.id,
                descripcion: selectedOption.descripcion,
            });
        }
    };



    const handleFechaRadicadoWorkManagerChange = (date: any) => {

        if (date instanceof Date && !Number.isNaN(date.getTime())) {

            setFormData({
                ...formData,
                fechaRadicadoWorkManager: date,
            });

            return date;
        }

        return null;
    };

    const handleFechaRadicadoChange = (date: any) => {

        if (date instanceof Date && !Number.isNaN(date.getTime())) {

            setFormData({
                ...formData,
                fechaRadicadoJuzgado: date,
            });

            return date;
        }

        return null;
    };


    function formatDate(date: any) {
        const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
        return date.toLocaleDateString('es-ES', options);
    }

    // useEffect(() => {
    //     handleTipoIdentificacionChange(tiposIdentificacion);
    // }, [tiposIdentificacion]);

    useEffect(() => {
        handleDepartamento();
    }, [departamentoSeleccionado]);

    useEffect(() => {
        handleMunicipio();
    }, [municipioSeleccionado])

    useEffect(() => {
        handleJuzgado();
    }, [juzgadoSeleccionado])

    useEffect(() => {
        handleProcesoId();
    }, [tipoDeProceso])

    const [modalOpen, setModalOpen] = useState(false);

    const handleModalOpen = () => {
        generatePDF();
        setModalOpen(true);
    };

    const handleModalClose = () => {
        setModalOpen(false);
    };


    const [dataFormulario, setDataFormulario] = useState({});

    const generatePDF = () => {
        /* eslint-disable new-cap */
        const pdf = new jsPDF();
        /* eslint-enable new-cap */


        const xPosition = 20;
        const yPosition = 20;

        const identificacion = formularioEmbargos.watch('identificacionCliente');
        const primerNombre = nombreUno || '';
        const segundoNombre = nombreDos || '';
        const primerApellido = apellidoUno || '';
        const segundoApellido = apellidoDos || '';
        const radicadoWorkManager = formularioEmbargos.watch('radicadoWorkManager');

        const fechaRadicado = formData.fechaRadicadoWorkManager || '';
        const formattedDate = formatDate(fechaRadicado);

        const radicadoJuzgado = formularioEmbargos.watch('radicadoJuzgado');

        const fechaRadicadoJuzgado = formData.fechaRadicadoJuzgado || '';
        const formattedDateJuzgado = formatDate(fechaRadicadoJuzgado);

        const cuentaJudicial = formularioEmbargos.watch('numeroCuenta');
        const codigoOficina = formularioEmbargos.watch('codigoOficina');
        const expedienteBancoAgrario = formularioEmbargos.watch('expedienteBancoAgrario');
        const codigoInterno = formularioEmbargos.watch('codigoInterno');
        const emailRespuesta = formularioEmbargos.watch('emailRespuesta');
        const proceso = formularioEmbargos.watch('proceso');


        const dataRespuesta = {
            iden: identificacion,
            primNombre: primerNombre,
            segNombre: segundoNombre,
            primApellido: primerApellido,
            segApellido: segundoApellido,
            radicadoFlujo: radicadoWorkManager,
            direc: direccionValue,
            radicadoFecha: formattedDate,
            juzgadoRadicado: radicadoJuzgado,
            fechaOficioJuzg: formattedDateJuzgado,
            departJuzgado: nombreDepartamento,
            municipJuzgado: nombreMunicipio,
            nombreJuzgado: juzgado,
            cuentaJudicial: cuentaJudicial,
            ofiDestino: codigoOficina,
            numExpedientes: expedienteBancoAgrario,
            codigoInterno: codigoInterno,
            correoElect: emailRespuesta,
            tipoProces: tipoProceso,
            numProceso: proceso,

        }

        setDataFormulario(dataRespuesta)

        return pdf;
    };

    const handleDepartamentoChange = (event, onChange) => {
        const id = event.target.value;
        formularioEmbargos.setValue('idCiudad', '');
        formularioEmbargos.setValue('juzgadoId', '');
        formularioEmbargos.setValue('codigoInterno', '');
        formularioEmbargos.setValue('codigoOficina', '');
        formularioEmbargos.setValue('numeroCuenta', '');
        obtenerCiudades(id as number);
        onChange(event);
    };

    const depa = watch('idCiudad');

    const [municipioNombre, setMunicipioNombre] = useState('');

    useEffect(() => {
        // Busca el nombre del municipio basado en el ID cuando el valor inicial es proporcionado.
        const municipio = cities.find((city) => city.id === depa);
        if (municipio) {
            setMunicipioNombre(municipio.nombre);
        }
    }, [depa, cities]);

    const [alertaMostrada, setAlertaMostrada] = useState(false);


    return (
        <>
            <CHPaper title="Información embargo">
                <AddressComponent
                    onOpen={open}
                    onClose={onClose}
                    control={formularioEmbargos.control}
                    getValues={formularioEmbargos.watch}
                    tipoDireccion={1 === 1}
                    setValue={formularioEmbargos.setValue}
                />
                <Grid container>
                    {formularioEmbargos.getValues('numeroSolicitud') > 0 && (
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
                                        value: inconsistencia,
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
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Número Solicitud"
                                        value={value}
                                        onChange={e => {
                                            if (
                                                tipoRadicado === 1 ||
                                                tipoRadicado === 2 ||
                                                tipoRadicado === 5
                                            ) {
                                                if (e.target.value.length <= 10) {
                                                    onChange(
                                                        e.target.value.replace(
                                                            /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W%-+=`~]/g,
                                                            '',
                                                        ),
                                                    );
                                                }
                                            } else if (
                                                tipoRadicado === 0 ||
                                                tipoRadicado === 3 ||
                                                tipoRadicado === 4 ||
                                                tipoRadicado === 6 ||
                                                tipoRadicado === 7 ||
                                                tipoRadicado === 8
                                            ) {
                                                if (e.target.value.length <= 10) {
                                                    onChange(
                                                        e.target.value.replace(
                                                            /[ñÑáéíóúÁÉÍÓÚ\W%-+=`~]/g,
                                                            '',
                                                        ),
                                                    );
                                                }
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
                    )}
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
                                    onChange={e => {
                                        setTipoDocumento(
                                            e.target.value as number,
                                        );
                                        onChange(e);
                                        handleTipoIdentificacionChange(e);
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
                    {formularioEmbargos.watch('tipoIdentificacion') === 6 ? (
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
                                        value: inconsistencia,
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
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={modoActualizar}
                                        InputLabelProps={{ shrink: true }}
                                        label="Número de Identificación *"
                                        value={value}

                                        onChange={e => {
                                            if (
                                                tipoDocumento === 1 ||
                                                tipoDocumento === 2 ||
                                                tipoDocumento === 5
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
                                            setDocumento(e.target.value);
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    ) : (
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
                                        value: inconsistencia,
                                        message: 'Este campo es requerido y debe contener solo números',
                                    },
                                    minLength: {
                                        value: 4,
                                        message:
                                            'La longitud mínima del campo es de 4 caracteres',
                                    },

                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={modoActualizar}
                                        InputLabelProps={{ shrink: true }}
                                        label="Número de Identificación *"
                                        value={value}
                                        numberMask
                                        onChange={e => {
                                            if (
                                                tipoDocumento === 1 ||
                                                tipoDocumento === 2 ||
                                                tipoDocumento === 5
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
                                            setDocumento(e.target.value);
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreCliente"
                            control={formularioEmbargos.control}

                            render={({ field: { onChange, value } }) => {

                                const nombresApellidos = value ? value.split(' ') : [];
                                const nombres = nombresApellidos.slice(0, 1).join(' ');

                                setNombreUno(nombres)

                                return (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Primer Nombre"
                                        value={nombreUnno || ''}

                                        onChange={(e) => {
                                            onChange(e);
                                        }}
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

                                render={({ field: { onChange, value } }) => {

                                    const nombresApellidos = value ? value.split(' ') : [];
                                    const nombres = nombresApellidos.slice(1, 2).join(' ');
                                    setNombreDos(nombres)

                                    return (
                                        <CHTextField
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }}
                                            label="Segundo Nombre"
                                            value={nombreDoss || ''}

                                            onChange={(e) => {
                                                onChange(e);
                                            }}
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
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="nombreCliente"
                                control={formularioEmbargos.control}

                                render={({ field: { onChange, value } }) => {
                                    const apellidos = obtenerApellidos(value);

                                    useEffect(() => {
                                        setApellidoUno(apellidos);
                                    }, [apellidos]);

                                    return (
                                        <CHTextField
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }}
                                            label="Primer Apellido"
                                            value={apellidoUnno || ''}
                                            // value={apellidos}
                                            onChange={(e) => {
                                                onChange(e);
                                            }}
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
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="nombreCliente"
                                control={formularioEmbargos.control}

                                render={({ field: { onChange, value } }) => {
                                    const apellidos = obtenerSegundoApellido(value);

                                    useEffect(() => {
                                        setApellidoDos(apellidos);
                                    }, [apellidos]);

                                    return (
                                        <CHTextField
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }}
                                            label="Segundo Apellido"
                                            value={apellidoDoss || ''}

                                            onChange={(e) => {
                                                onChange(e);
                                            }}
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

                        <Grid className={classes.column} item container spacing={1} xs={12} md={3} alignItems="center">
                            <Controller
                                name="radicadoWorkManager"
                                control={formularioEmbargos.control}
                                rules={{
                                    pattern: {
                                        value: /^[\d-\s]*$/g,
                                        message: 'Este campo debe contener solo números y guiones'
                                    },
                                    required: {
                                        value: inconsistencia,
                                        message: 'Este campo es requerido',
                                    },
                                    maxLength: {
                                        value: 25,
                                        message: 'La longitud máxima del campo es de 25 caracteres incluyendo los guiones',
                                    },
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <div style={{ display: 'flex', alignItems: 'center' }}>
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}

                                            InputLabelProps={{ shrink: true }}
                                            label="Número Radicado Solicitud *"
                                            value={value}
                                            onChange={() => { }}

                                        />
                                        <a href={linkDodo} target="_blank" rel="noopener noreferrer">
                                            <AttachFileIcon />
                                        </a>
                                    </div>
                                )}
                            />

                        </Grid>



                    </Grid>
                    <Grid container>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="direccion"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                defaultValue=""
                                render={({
                                    field: { value, onChange },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        label="Dirección"
                                        value={direccionValue}
                                        onChange={(e) => {
                                            onChange(e.target.value);
                                        }}
                                        onClick={handleOpenAddress}
                                    />
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
                                        onChange={(date) => {
                                            onChange(date);
                                            handleFechaRadicadoWorkManagerChange(date);
                                        }}
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
                                    required: 'Este campo es requerido.',

                                    maxLength: {
                                        value: 50,
                                        message:
                                            'Excedió los caracteres',
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
                                        label="Número Oficio Juzgado*"
                                        value={value}
                                        onChange={onChange}

                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaRadicadoJuzgado"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: inconsistencia,
                                        message: 'Este campo es requerido',
                                    },
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHDatePicker
                                        label="Fecha Oficio Juzgado *"
                                        inputProps={{ readOnly: true }}
                                        error={!!error && value !== ''}
                                        helperText={error && value !== '' ? error.message : null}
                                        onChange={(date) => {
                                            onChange(date);
                                            handleFechaRadicadoChange(date);
                                        }}
                                        value={value}
                                        minDate={new Date("1-01-1950")}
                                        maxDate={today}
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="idDepartamento"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: inconsistencia,
                                        message: 'Este campo es requerido',
                                    },
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
                                                InputProps={{
                                                    endAdornment: (
                                                        <InputAdornment
                                                            position="end"
                                                            style={{ paddingRight: '4px' }}
                                                        >
                                                            <IconButton
                                                                aria-label="clear selection"
                                                                onClick={(e) => {
                                                                    e.stopPropagation();
                                                                    formularioEmbargos.setValue('idDepartamento', '');
                                                                    formularioEmbargos.setValue('idCiudad', '');
                                                                    formularioEmbargos.setValue('juzgadoId', '');
                                                                    formularioEmbargos.setValue('codigoInterno', '');
                                                                    formularioEmbargos.setValue('codigoOficina', '');
                                                                    formularioEmbargos.setValue('numeroCuenta', '');
                                                                    onChange('');
                                                                }}
                                                                style={{ padding: '4px' }}
                                                            >
                                                                <ClearIcon />
                                                            </IconButton>
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
                                    required: {
                                        value: inconsistencia,
                                        message: 'Este campo es requerido',
                                    },
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
                                                                aria-label="clear selection"
                                                                onClick={(e) => {
                                                                    e.stopPropagation();
                                                                    formularioEmbargos.setValue('idCiudad', '');
                                                                    formularioEmbargos.setValue('juzgadoId', '');
                                                                    formularioEmbargos.setValue('codigoInterno', '');
                                                                    formularioEmbargos.setValue('codigoOficina', '');
                                                                    formularioEmbargos.setValue('numeroCuenta', '');
                                                                    onChange('');
                                                                }}
                                                                style={{ padding: '4px' }}
                                                            >
                                                                <ClearIcon />
                                                            </IconButton>
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
                                        value: inconsistencia,
                                        message: 'Este campo es requerido',
                                    },
                                    validate: {
                                        notEmptyUUID: value => value !== '00000000-0000-0000-0000-000000000000' || 'Este campo es requerido',
                                    }
                                }}
                                render={({ field, fieldState: { error } }) => (
                                    <Autocomplete
                                        options={juzgados}
                                        getOptionLabel={(option) => option ? option.nombre : ''}
                                        onChange={(event, value) => {
                                            field.onChange(value ? value.idJuzgado : null);
                                            handleSelectChange(event);
                                        }}
                                        value={field.value ? juzgados.find((juzgado) => juzgado.idJuzgado === field.value) : null}
                                        onInputChange={handleSelectChange}
                                        renderInput={(params) => (
                                            <TextField
                                                {...params}
                                                label="Juzgado *"
                                                name="juzgadoId"
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                            />
                                        )}

                                    />
                                )}
                            />

                        </Grid>
                        <Grid className={classes.buttonA}>
                            <Button
                                variant="outlined"
                                onClick={handleOpenModalAdd}
                                disabled={formularioEmbargos.getValues('juzgadoId')}
                            >
                                AGREGAR DESPACHO JUDICIAL
                            </Button>
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
                                name="expedienteBancoAgrario"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: inconsistencia,
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
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        label="Número Expediente *"
                                        value={value}
                                        onChange={e => {
                                            if (e.target.value.length <= 11) {
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
                                name="codigoInterno"
                                control={formularioEmbargos.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Código Interno"
                                        value={juzgados.length > 0 ? value : 'N/A'}
                                        onChange={onChange}
                                        inputProps={{
                                            maxLength: 12,
                                        }}
                                        onBlur={() => {
                                            if (value && value.length > 0) {
                                                setIsReadOnly(true);
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
                                name="emailRespuesta"
                                control={formularioEmbargos.control}
                                rules={inconsistencia ? {} : {
                                    pattern: {
                                        value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
                                        message: 'Este campo debe ser un correo electrónico válido'
                                    },
                                    validate: {
                                        checkDomain: value => {
                                            if (!value) return true; // Ignorar la validación si el valor es vacío
                                            const domain = value.split('@')[1];
                                            const validDomains = [
                                                'cajahonor.gov.co',
                                                'cortesuprema.gov.co',
                                                'consejodeestado.gov.co',
                                                'corteconstitucional.gov.co',
                                                'cendoj.ramajudicial.gov.co',
                                                'deaj.ramajudicial.gov.co',
                                                'consejosuperior.ramajudicial.gov.co',
                                            ];
                                            if (!validDomains.includes(domain)) {
                                                return `Solo se permiten correos electrónicos con los siguientes dominios: ${validDomains.join(', ')}`;
                                            }
                                            return true;
                                        },
                                        noSpaces: value => {
                                            if (!value) return true; // Ignorar la validación si el valor es vacío
                                            if (/\s/.test(value)) {
                                                return 'El correo electrónico no puede contener espacios';
                                            }
                                            return true;
                                        },
                                        noDuplicateCom: value => {
                                            if (!value) return true; // Ignorar la validación si el valor es vacío
                                            if ((value.match(/\.com/g) || []).length > 2) {
                                                return 'El correo electrónico no puede tener más de un ".com" o un ".gov.co"';
                                            }
                                            return true;
                                        },
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
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="procesoId"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: inconsistencia,
                                        message: 'Este campo es requerido',
                                    },
                                }}

                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        label="Tipo Proceso *"
                                        name="procesoId"
                                        value={value}
                                        onChange={e => {
                                            const otrosProcesosId = "da7002f2-59db-4dfa-9486-320b36dfd5af";
                                            const selectedValue = e.target.value;
                                            const selectedOption = tiposProcesos.find((option) => option.id === selectedValue);
                                            const chTextFieldEnabled = selectedOption?.id === otrosProcesosId;
                                            setCHTextFieldEnabled(chTextFieldEnabled);
                                            onChange(e);

                                        }}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                    >
                                        {tiposProcesos.map((option: TipoProcesoEmbargoDto) => (
                                            <MenuItem key={option.id} value={option.id}>
                                                {option.descripcion}
                                            </MenuItem>
                                        ))}
                                    </CHSelect>
                                )}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="proceso"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: inconsistencia,
                                        message: 'Este campo es requerido',
                                    },
                                    minLength: {
                                        value: 7,
                                        message: 'No se permite menos de 11 dígitos'
                                    },
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        type="number"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        InputLabelProps={{ shrink: true }}
                                        label="Número Proceso *"
                                        onChange={e => {
                                            onChange(e);
                                        }}
                                        value={value}
                                        isReadOnly
                                    />
                                )}
                            />
                        </Grid>

                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={12}>
                            {chTextFieldEnabled && (
                                <Controller
                                    name="otrosProcesos"
                                    control={formularioEmbargos.control}
                                    rules={{
                                        required: {
                                            value: inconsistencia,
                                            message: 'Este campo es requerido',
                                        }
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            multiline
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            inputProps={{
                                                style: {
                                                    textTransform: 'uppercase',
                                                    overflowY: 'auto',
                                                },
                                            }}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            label="Otros Procesos*"
                                            value={value}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            )}
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={12}>
                            <Controller
                                name="observaciones"
                                control={formularioEmbargos.control}
                                rules={{
                                    required: {
                                        value: inconsistencia,
                                        message: 'Este campo es requerido',
                                    }
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        multiline
                                        maxRows={3}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                                overflowY: 'auto',
                                            },
                                        }}
                                        InputLabelProps={{ shrink: true }}
                                        label="Observaciones *"
                                        value={value}
                                        // onChange={(e) => onChange(e.target.value.toUpperCase())}
                                        onChange={(e) => {
                                            const input = e.target;
                                            const start = input.selectionStart;
                                            const end = input.selectionEnd;
                                            const upper = input.value.toUpperCase();
                                            
                                            onChange(upper);
                                        
                                            setTimeout(() => {
                                                input.setSelectionRange(start, end);
                                            }, 0);
                                        }}
                                    />
                                )}
                            />
                        </Grid>

                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={3} md={3}>
                            <Controller
                                name="Inconsistencia"
                                control={formularioEmbargos.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHCheckbox
                                        label="¿Inconsistencia?"
                                        onChange={(e) => {
                                            onChange(e.target.checked);
                                            setInconsistencia(!e.target.checked);
                                        }}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={3} md={3}>
                            <Controller
                                name="remanente"
                                control={formularioEmbargos.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHCheckbox
                                        label="¿Remanente?"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>

                    </Grid>
                </Grid>
                <ModalCourtAdd
                    open={openModalAdd}
                    formulario={formulario}
                    allDepartaments={departamentos}
                    onClose={handleCloseModalAdd}
                    handleAdd={handleAdd}
                    handleEdit={handleEdit}
                    allTipoActor={allTipoActor}
                />
            </CHPaper>

            <CHPaper title="Demandante">
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
                                    onChange={(e) => {
                                        onChange(e.target.value);
                                        const valorTipo = parseInt(e.target.value as string, 10);
                                        setSelectedTipoDocumento(parseInt(e.target.value as string, 10));
                                        setEsTipoEspecial(valorTipo === 8);
                                        setAlertaMostrada(false);
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
                                    value: 15,
                                    message: 'La longitud máxima del campo es de 15 caracteres',
                                },
                                minLength: {
                                    value: 4,
                                    message:
                                        'La longitud mínima del campo es de 4 caracteres',
                                },
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="No. Identificación *"
                                    value={value}
                                    onChange={(e) => {
                                        const tipoDocumento = formularioBeneficiarios.getValues('tipoDocumentoId');

                                        if (!tipoDocumento && !alertaMostrada) {
                                            toast('Por favor, seleccione un tipo de documento');
                                            setAlertaMostrada(true);
                                        } else if (tipoDocumento) {
                                            setAlertaMostrada(false);

                                            if (
                                                tipoDocumento === 1 ||
                                                tipoDocumento === 2 ||
                                                tipoDocumento === 3 ||
                                                tipoDocumento === 4 ||
                                                tipoDocumento === 5 ||
                                                tipoDocumento === 6 ||
                                                tipoDocumento === 7 ||
                                                tipoDocumento === 8
                                            ) {
                                                if (e.target.value.length <= 15) {
                                                    onChange(
                                                        e.target.value.replace(/[a-zA-ZñÑáéíóúÁÉÍÓÚ\W]/g, '')
                                                    );
                                                }
                                                if (tipoDocumento === 6) {
                                                    onChange(e.target.value.replace(/[^a-zA-Z0-9]+/g, ''));
                                                }
                                                consultarCliente(tipoDocumento, e.target.value);
                                            } else {
                                                onChange(e);
                                            }
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
                            name="primerNombre"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido.',
                                },
                                // pattern: /^[ A-zÀ-úñÑ ]+$/g
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? 'El primer nombre solo debe contener letras' : null}
                                    inputProps={{
                                        style: { textTransform: 'uppercase' },
                                    }}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="Primer Nombre *"
                                    value={value}
                                    onChange={onChange}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        
                                    }}

                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="segundoNombre"
                            control={formularioBeneficiarios.control}
                            defaultValue=""
                                                    
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? 'El segundo nombre solo debe contener letras' : null}
                                    
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: { textTransform: 'uppercase' },
                                    }}
                                    label="Segundo Nombre"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={esTipoEspecial}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        const regex = /^[A-zÀ-úñÑ]*$/;
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
                            name="primerApellido"
                            control={formularioBeneficiarios.control}
                            rules={{
                                required: !esTipoEspecial
                                    ? { value: true, message: 'Este campo es requerido.' }
                                    : false,
                                pattern: /^[ A-zÀ-úñÑ ]+$/g
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? 'El primer apellido solo debe contener letras' : null}
                                    isReadOnly={esTipoEspecial}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: { textTransform: 'uppercase' },
                                    }}
                                    label="Primer Apellido *"
                                    value={value}
                                    onChange={onChange}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        const regex = /^[A-zÀ-úñÑ]*$/;
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
                            name="segundoApellido"
                            control={formularioBeneficiarios.control}
                                                   
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? 'El segundo apellido solo debe contener letras' : null}
                                    isReadOnly={esTipoEspecial}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: { textTransform: 'uppercase' },
                                    }}
                                    label="Segundo Apellido"
                                    value={value}
                                    onChange={onChange}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        const regex = /^[A-zÀ-úñÑ]*$/;
                                        if (!regex.test(keyValue)) {
                                            event.preventDefault();
                                        }
                                    }}

                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={3} md={3}>
                        <Controller
                            name="demandante"
                            control={formularioBeneficiarios.control}
                            render={({ field: { onChange, value } }) => (
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            name="Demandante"
                                            onChange={onChange}
                                            checked={value}
                                            color='primary'
                                            disabled={beneficiariosModel.some(item => item.demandante)}
                                        />
                                    }
                                    label="¿Demandante?"
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={3} md={3}>
                        <Controller
                            name="bpE_BENEFICIARIO"

                            control={formularioBeneficiarios.control}
                            render={({ field: { onChange, value } }) => (
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            name="Beneficiario de Pago"
                                            onChange={onChange}
                                            checked={value}
                                            color='primary'
                                        />
                                    }
                                    label="¿Beneficiario de Pago?"
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
                            onClick={formularioBeneficiarios.handleSubmit(agregarBeneficiario)}

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
                            <TableCell>Beneficiario de Pago</TableCell>
                            <TableCell align="center">Acciones</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {beneficiarios && beneficiarios.map((option: BeneficiarioDto) => {
                            if (option.tipoDocumentoId) {
                                identificacion = tiposIdentificacion.find(
                                    item => item.id === option.tipoDocumentoId,
                                );
                                return (
                                    <TableRow key={option.numeroIdentificacion}>
                                        <TableCell>
                                            {identificacion?.descripcion || ''}
                                        </TableCell>
                                        <TableCell>
                                            {option.numeroIdentificacion}
                                        </TableCell>
                                        <TableCell style={{ textTransform: 'uppercase' }}>
                                            {option.primerNombre}
                                        </TableCell>
                                        <TableCell style={{ textTransform: 'uppercase' }}>
                                            {option.segundoNombre || ''}
                                        </TableCell>
                                        <TableCell style={{ textTransform: 'uppercase' }}>
                                            {option.primerApellido}
                                        </TableCell>
                                        <TableCell style={{ textTransform: 'uppercase' }}>
                                            {option.segundoApellido || ''}
                                        </TableCell>
                                        <TableCell>
                                            {option.demandante ? 'SI' : 'NO'}
                                        </TableCell>
                                        <TableCell>
                                            {option.bpE_BENEFICIARIO ? 'SI' : 'NO'}
                                        </TableCell>
                                        <TableCell align="center">
                                            <IconButton
                                                aria-label="Editar"
                                                color="primary"
                                                onClick={() => {
                                                    seleccionarRegistro(option);
                                                }}
                                            >
                                                <Edit />
                                            </IconButton>
                                            <IconButton
                                                aria-label="Eliminar"
                                                color="primary"
                                                onClick={() => {
                                                    eliminarRegistro(
                                                        option.numeroIdentificacion,
                                                    );
                                                }}
                                            >
                                                <Delete />
                                            </IconButton>
                                        </TableCell>
                                    </TableRow>
                                );
                            }
                            return null;
                        })}
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
                    saldoConcepto={saldoConcepto}
                />
            </CHPaper>
            {preRespuesta && (
                <Button
                    className={classes.buttonAdd}
                    variant="outlined"
                    onClick={handleModalOpen}
                >
                    GENERAR DATOS RESPUESTA
                </Button>
            )}
            <ModalPreVisualRespuesta
                open={modalOpen}
                handleClose={handleModalClose}
                dataFormulario={dataFormulario}
            />
        </>
    );
};

interface LinkMapDispatchToProps {
    formularioEmbargos: UseFormReturn<any>;
    formularioBeneficiarios: UseFormReturn<any>;
    beneficiarios: BeneficiarioDto[];
    tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
    departamentos: DEPARTAMENTODTO[];
    cities: CITYDTO[];
    juzgados: JuzgadoDto[];
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
    obtenerCiudades: (idDepto: number) => void;
    obtenerJuzgados: (idDepto: number, idCiudad: number) => void;
    setBeneficiarios: (beneficiarios: BeneficiarioDto[]) => void;
    handleOpenModal: (conceptoId: number) => void;
    handleCloseModal: any;
    setListaConceptos: (listaConceptos: ConceptoEmbargosDto[]) => void;
    setTipoDocumento: (value: number) => void;
    setDocumento: (value: string) => void;
    consultarCuentaConcepto: () => void;
    handleOnChangeMostrarPorcentaje: (value: boolean) => void;
    consultarJuzgado: (idJugado: string, codigoInterno: string) => void;
    consultarCliente: (idTipoIdentificacion: number, identificacion: string) => void;
    setCuentas: (data: any) => void;
    estadoCuentaEmbargo: any;
    setConcepto: ConceptoEmbargosDto[];
    startPostInsertCourt: (juzgado: JuzgadoDto) => void;
    startPostUpdateCourt: (juzgado: JuzgadoDto) => void;
    nombreUnno: any;
    nombreDoss: any;
    apellidoUnno: any;
    apellidoDoss: any;
    saldoConcepto: any;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,

});


const mapDispatchToProps = {
    startPostInsertCourt,
    startPostUpdateCourt,
};

type props = LinkMapDispatchToProps;



export default connect(mapStateToProps, mapDispatchToProps)(InformacionEmbargos);
