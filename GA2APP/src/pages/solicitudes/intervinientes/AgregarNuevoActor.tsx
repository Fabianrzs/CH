import { Button, createStyles, Grid, makeStyles, MenuItem, FormControlLabel, RadioGroup, Radio, FormLabel } from "@material-ui/core";
import React, { useEffect } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import CHAutoComplete from "../../../components/ch-auto-complete/CHAutoComplete";
import CHDatePicker from "../../../components/ch-datepicker/CHDatepicker";
import CHPageTitle from "../../../components/ch-page-title/CHPageTitle";
import CHPaper from "../../../components/ch-paper/CHPaper";
import CHPrivateContainer from "../../../components/ch-private-container/CHPrivateContainer";
import CHSelect from "../../../components/ch-select/CHSelect";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import { LoadingComponent } from "../../../components/loading/loading.component";
import CatalogApi from "../../../services/api-catalog.service";
import { Address } from "../../../types/address.type";
import history from '../../../utils/history.router';
import AddressComponentV2 from '../../../components/modals/address/addressV2.component'
import { setFormDataAddres } from "../../../components/modals/address/constants/address.components.constants";
import SolicitudApi from "../../../services/api-solicitud.service";
import ApiClients from '../../../services/api-clients.service';
import NotificationService from "../../../services/notification.service";

const useStyles = makeStyles((theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            backgroundColor: '#1C3F8B',
            color: '#FFFFFF',
            marginLeft: '.5rem',
            '&:hover': {
                backgroundColor: '#FFFFFF !important',
                color: 'black !important'
            },
        },
        container: {
            maxHeight: 440,
        },
        alternateRow: {
            backgroundColor: '#D3D3D3',
        },


    }));

const AgregarNuevoActor = () => {

    const classes = useStyles();
    const apiCatalogo = CatalogApi();
    const apiSolicitud = SolicitudApi();
    const apiClientes = ApiClients();
    const [loading, setLoading] = React.useState(false)
    const [listaTipoIdentificacion, setListaTipoIdentificacion] = React.useState([])
    const [nuevoListadoIdenticacion, setNuevoListadoIdenticacion] = React.useState([])
    const [listaDepartamentos, setListaDepartamentos] = React.useState([])
    const [listaEntidadesBancarias, setListaEntidadesBancarias] = React.useState([])
    const [listaDCiudades, setListaDCiudades] = React.useState([])
    const [listaTipoActores, setListaTipoActores] = React.useState([])
    const [listaTipoCuentas, setListaTipoCuenta] = React.useState([])
    const [open, setOpen] = React.useState(false);
    const [mostrar, setMostrar] = React.useState(false);
    const [mostrarCamposNombres, setMostrarCamposNombres] = React.useState(false);
    const [tipoActorSeleccionado, setTipoActorSeleccionado] = React.useState<any>(null);
    const CODIGO_BENEFICIARIO_ESTUDIO = 'BEST';

    const formularioTipoActor = useForm({
        defaultValues: {
            tipoDocumento: '',
            numeroDocumento: '',
            nombre: '',
            segundoNombre: '',
            primerApellido: '',
            segundoApellido: '',
            personaJuridicaNatural: '',
            celular: '',
            correoElectronico: '',
            departamento: '',
            ciudad: '',
            direccion: '',
            barrio: '',
            telefono: '',
            tipoActor: '',
            tipoCuenta: '',
            numeroCuentaBancaria: '',
            entidadBancaria: '',
            fechaExpedicionCedula: null,
            constructora: 'no',
            convenioEntidad: 'no',
            tipoDocumentoAfiliado: '',
            numeroDocumentoAfiliado: '',
        }
    })

    useEffect(() => {
        apiCatalogo.
            getAllStates()
            .then(async response => {
                if (response) {
                    if (response.data !== null) {
                        setListaDepartamentos(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])

    useEffect(() => {
        apiCatalogo.
            getAllCatalogoPorId(67)
            .then(async response => {
                if (response) {
                    if (response.data !== null) {
                        setListaTipoCuenta(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])

    useEffect(() => {
        apiCatalogo.
            getAllEntidadBancaria()
            .then(async response => {
                if (response) {
                    if (response.data !== null) {
                        setListaEntidadesBancarias(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])

    useEffect(() => {
        apiCatalogo.
            getAllCities()
            .then(async response => {
                if (response) {
                    if (response.data !== null) {
                        setListaDCiudades(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])

    useEffect(() => {
        apiCatalogo
            .getAllTypeDocuments()
            .then(async response => {
                if (response) {
                    if (response.data !== null) {
                        setListaTipoIdentificacion(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])

    useEffect(() => {
        apiSolicitud
            .GetConsultarIntervinientesActores()
            .then(async response => {
                if (response) {
                    if (response.data !== null) {
                        setListaTipoActores(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, [])

    const filterCiudadesByDepartamento = (departamentoId: any) => {
        if (!departamentoId) return [];
        return listaDCiudades.filter((ciudad: any) => ciudad.idDepartamento === departamentoId);
    };
    const estadoPersona = (item: any) => {
        if (item === 'juridica') {
            const tipoIdentificacion = listaTipoIdentificacion.filter((x: any) => x.descripcion === "NÚMERO DE IDENTIFICACIÓN TRIBUTARIA")
            setNuevoListadoIdenticacion(tipoIdentificacion)
            setMostrarCamposNombres(false);
        } else {
            setNuevoListadoIdenticacion(listaTipoIdentificacion)
            setMostrarCamposNombres(true);
        }
    }

    const estadoConstructora = (item: any) => {
        if (item === 'si') {
            setMostrar(true)
        } else {
            setMostrar(false)
        }
    }

    const cleanPhoneNumber = (number: any) => number.replace(/[()\s-]/g, '');

    const crearTipoActor = async (data: any) => {

        setLoading(true)
        const numeroCelular = cleanPhoneNumber(data.celular);
        const numeroTelefonico = cleanPhoneNumber(data.telefono);
        const request = {
            solId: null,
            tipoDocumento: data.tipoDocumento,
            numeroDocumento: data.numeroDocumento,
            nombre: data.nombre,
            segundoNombre: data.segundoNombre || '',
            primerApellido: data.primerApellido || '',
            segundoApellido: data.segundoApellido || '',
            personaJuridicaNatural: data.personaJuridicaNatural,
            celular: numeroCelular,
            correoElectronico: data.correoElectronico,
            departamento: data.departamento,
            ciudad: data.ciudad,
            direccion: data.direccion,
            barrio: data.barrio,
            telefono: numeroTelefonico,
            tipoActor: data.tipoActor,
            tipoCuenta: data.tipoCuenta,
            numeroCuentaBancaria: data.numeroCuentaBancaria,
            entidadBancaria: data.entidadBancaria === '' || data.entidadBancaria === undefined ? 0 : data.entidadBancaria,
            fechaExpedicionCedula: data.fechaExpedicionCedula,
            constructora: data.constructora,
            convenioEntidad: data.convenioEntidad,
            numeroDocumentoAfiliado: data.numeroDocumentoAfiliado,
            tipoDocumentoAfiliado: String(data.tipoDocumentoAfiliado)
        }

        const tipoActor: any = listaTipoActores.find((item: any) => item?.itA_ID === data.tipoActor);
        if (tipoActor?.itA_CODIGO === CODIGO_BENEFICIARIO_ESTUDIO) {
            const cedula = Number(data.numeroDocumentoAfiliado);
            await apiClientes.postObtenerInformacionClienteCedula(cedula)
                .then(async response => {
                    if (response) {
                        if (response?.data !== null) {
                            await apiSolicitud.
                                PostGuadarTipoActor(request)
                                .then(async response => {
                                    if (response) {
                                        if (response.isSuccess) {

                                            await NotificationService.genericAlert(
                                                'Agregado con éxito',
                                                'Se agregó el actor con éxito.',
                                            );
                                            const item = response?.data
                                            setLoading(false)
                                            history.push({
                                                pathname: '/GestionIntervinientes',
                                                state: { item }
                                            });

                                        }
                                    }
                                })
                                .catch(() => {
                                    toast('Error al guardar el tipo actor', { type: toast.TYPE.ERROR });
                                    setLoading(false)
                                });
                        } else {
                            await NotificationService.genericAlert(
                                'Información',
                                `El número de documento del afiliado ingresado no existe`,
                                'warning'
                            );
                            setLoading(false)
                        }
                    }
                })
                .catch(() => {
                    toast('Error al consultar información afiliado', { type: toast.TYPE.ERROR });
                    setLoading(false)
                });
        } else {

            await apiSolicitud.
                PostGuadarTipoActor(request)
                .then(async response => {
                    if (response) {
                        if (response.isSuccess) {

                            await NotificationService.genericAlert(
                                'Agregado con éxito',
                                'Se agregó el actor con éxito.',
                            );
                            const item = response?.data
                            setLoading(false)
                            history.push({
                                pathname: '/GestionIntervinientes',
                                state: { item }
                            });

                        }
                    }
                })
                .catch(() => {
                    toast('Error al guardar el tipo actor', { type: toast.TYPE.ERROR });
                    setLoading(false)
                });
        }

    }

    const handleOpenAddress = () => {
        setOpen(true);
    };

    const onCloseAddress = (value: Address) => {
        if (value) {
            const result = setFormDataAddres(value);
            formularioTipoActor.setValue('direccion', result);
        }
        setOpen(false);
    };

    return (
        <>
            <CHPrivateContainer>
                <LoadingComponent key="loader" loading={loading} />
                <CHPageTitle
                    topText="Intervinientes"
                    subText=""
                    title1="Creación"
                    title2="Tipo Actor "
                />
                <CHPaper title="Creación de Tipo Actor">
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={1}>
                            <FormLabel component="legend">Tipo de Persona *</FormLabel>
                            <Controller
                                name="personaJuridicaNatural"
                                control={formularioTipoActor.control}
                                render={({ field }) => (

                                    <RadioGroup

                                        aria-label="personaJuridicaNatural"
                                        name="personaJuridicaNatural"
                                        value={field.value}
                                        onChange={(event) => {

                                            field.onChange(event.target.value)
                                            estadoPersona(event.target.value)
                                        }}
                                    >
                                        <FormControlLabel value="natural"
                                            control={<Radio color="primary" />}
                                            label="Natural"

                                        />
                                        <FormControlLabel
                                            value="juridica"
                                            control={<Radio color="primary" />}
                                            label="Juridica"
                                        />
                                    </RadioGroup>
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="tipoDocumento"
                                control={formularioTipoActor.control}
                                rules={{ required: "Campo obligatorio" }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        label="Tipo de Documento *"
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                    >
                                        {nuevoListadoIdenticacion?.map(
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
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="numeroDocumento"
                                control={formularioTipoActor.control}
                                rules={{
                                    required: "Campo obligatorio",
                                    minLength: {
                                        value: 5,
                                        message:
                                            'La longitud mínima del campo es de 5 caracteres',
                                    },
                                    maxLength: {
                                        value: 15,
                                        message:
                                            'La longitud máxima del campo es de 15 caracteres',
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
                                        numberMask
                                        InputLabelProps={{ shrink: true }}
                                        label="No. Documento *"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        {!mostrarCamposNombres && (
                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="nombre"
                                    control={formularioTipoActor.control}
                                    rules={{
                                        required: "Campo obligatorio",
                                        minLength: {
                                            value: 4,
                                            message:
                                                'La longitud mínima del campo es de 4 caracteres',
                                        },
                                        maxLength: {
                                            value: 30,
                                            message:
                                                'La longitud máxima del campo es de 30 caracteres',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}

                                            InputLabelProps={{ shrink: true }}
                                            label="Nombre/Razon Social *"
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
                        )}
                        {mostrarCamposNombres && (
                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="segundoNombre"
                                    control={formularioTipoActor.control}
                                    rules={{
                                        required: "Campo obligatorio",
                                        minLength: {
                                            value: 4,
                                            message:
                                                'La longitud mínima del campo es de 4 caracteres',
                                        },
                                        maxLength: {
                                            value: 30,
                                            message:
                                                'La longitud máxima del campo es de 30 caracteres',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            label="Primer Nombre *"
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
                        )}
                        {mostrarCamposNombres && (
                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="primerApellido"
                                    control={formularioTipoActor.control}
                                    rules={{
                                        minLength: {
                                            value: 4,
                                            message:
                                                'La longitud mínima del campo es de 4 caracteres',
                                        },
                                        maxLength: {
                                            value: 30,
                                            message:
                                                'La longitud máxima del campo es de 30 caracteres',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            label="Segundo Nombre"
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
                        )}
                        {mostrarCamposNombres && (
                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="segundoApellido"
                                    control={formularioTipoActor.control}
                                    rules={{
                                        required: "Campo obligatorio",
                                        minLength: {
                                            value: 4,
                                            message:
                                                'La longitud mínima del campo es de 4 caracteres',
                                        },
                                        maxLength: {
                                            value: 30,
                                            message:
                                                'La longitud máxima del campo es de 30 caracteres',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            label="Primer Apellido *"
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
                        )}
                        {mostrarCamposNombres && (
                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="nombre"
                                    control={formularioTipoActor.control}
                                    rules={{
                                        minLength: {
                                            value: 4,
                                            message:
                                                'La longitud mínima del campo es de 4 caracteres',
                                        },
                                        maxLength: {
                                            value: 30,
                                            message:
                                                'La longitud máxima del campo es de 30 caracteres',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            label="Segundo Apellido"
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
                        )}
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="celular"
                                rules={{
                                    required: 'Este campo es requerido.',
                                    pattern: /\(?([3-3]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                                }}
                                control={formularioTipoActor.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        InputLabelProps={{ shrink: true }}
                                        label="Celular *"
                                        onChange={onChange}
                                        value={value}
                                        helperText={error ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos' : null}
                                        cellPhoneMask
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="correoElectronico"
                                control={formularioTipoActor.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHTextField
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        label="Correo Electrónico"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="departamento"
                                control={formularioTipoActor.control}

                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHAutoComplete
                                        options={listaDepartamentos}
                                        getOptionLabel={(option: any) => option.nombre}
                                        onChange={(_, data) => {
                                            onChange(data?.id)
                                        }}
                                        renderInput={(params) => (
                                            <CHTextField
                                                {...params}
                                                label="Departamento"
                                            />
                                        )}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="ciudad"
                                control={formularioTipoActor.control}

                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHAutoComplete
                                        options={filterCiudadesByDepartamento(formularioTipoActor.watch('departamento'))}
                                        getOptionLabel={(option) => formularioTipoActor.getValues('departamento') === undefined ? '' : option?.nombre}
                                        onChange={(_, data) => onChange(data?.id)}
                                        renderInput={(params) => (
                                            <CHTextField
                                                {...params}
                                                label="Ciudad"

                                            />
                                        )}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="direccion"
                                control={formularioTipoActor.control}

                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
                                        label="Dirección"
                                        isReadOnly
                                        value={value}
                                        onClick={handleOpenAddress}
                                        onChange={(onChange)}
                                    />

                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="barrio"
                                control={formularioTipoActor.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHTextField
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        label="Barrio"
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
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="telefono"
                                rules={{

                                    pattern: /\(?([6-6]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                                }}
                                control={formularioTipoActor.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        label="Teléfono"
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                        value={value}
                                        error={!!error}
                                        helperText={error ? 'El Teléfono debe comenzar por 6 y tener una longitud de 10 dígitos' : null}
                                        cellPhoneMask
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="tipoActor"
                                control={formularioTipoActor.control}
                                rules={{ required: "Campo obligatorio" }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={
                                            error
                                                ? error.message
                                                : null
                                        }
                                        onChange={e => {
                                            const tipoActor = listaTipoActores.find((item: any) => item?.itA_ID === e.target?.value);
                                            if(tipoActor){
                                                setTipoActorSeleccionado(tipoActor);
                                            }
                                            onChange(e);
                                        }}
                                        value={value}
                                        label="Tipo Actor *"
                                    >
                                        {listaTipoActores?.map(
                                            (tipoActor: any) => (
                                                <MenuItem
                                                    key={
                                                        tipoActor?.itA_ID
                                                    }
                                                    value={
                                                        tipoActor?.itA_ID
                                                    }
                                                >
                                                    {
                                                        tipoActor?.itA_NOMBRE
                                                    }
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="tipoCuenta"
                                control={formularioTipoActor.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (

                                    <CHSelect
                                        label="Tipo de Cuenta"
                                        value={value}
                                        onChange={e => {
                                            onChange(e);
                                        }}
                                    >
                                        {listaTipoCuentas?.map(
                                            (option: any) => {
                                                const dato = `${option.idValor}_${option.descripcion}`;
                                                return (
                                                    <MenuItem
                                                        key={option.idValor}
                                                        value={dato}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                );
                                            },
                                        )}
                                    </CHSelect>

                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="numeroCuentaBancaria"
                                control={formularioTipoActor.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHTextField
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        label="Número de Cuenta Bancaria"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="entidadBancaria"
                                control={formularioTipoActor.control}

                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHAutoComplete

                                        options={listaEntidadesBancarias}
                                        getOptionLabel={(option: any) => option.nombreRazonSocial}
                                        onChange={(_, data) => {
                                            onChange(data?.codigoBanco)
                                        }}
                                        renderInput={(params) => (
                                            <CHTextField
                                                {...params}
                                                label="Entidad Bancaria"
                                            />
                                        )}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="fechaExpedicionCedula"
                                control={formularioTipoActor.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHDatePicker
                                        InputLabelProps={{ shrink: true }}
                                        label="Fecha de Expedición de la Cédula"
                                        value={value}
                                        onChange={onChange}

                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={1}>
                            <FormLabel component="legend">Constructora</FormLabel>
                            <Controller
                                name="constructora"
                                control={formularioTipoActor.control}
                                render={({ field }) => (
                                    <RadioGroup
                                        style={{ display: 'flex', }}
                                        aria-label="constructora"
                                        name="constructora"
                                        value={field.value}
                                        onChange={(event) => {
                                            field.onChange(event.target.value);
                                            estadoConstructora(event.target.value);
                                        }}
                                    >
                                        <FormControlLabel
                                            value="si"
                                            control={<Radio color="primary" />}
                                            label="Si"
                                        />
                                        <FormControlLabel
                                            value="no"
                                            control={<Radio color="primary" />}
                                            label="No"
                                        />
                                    </RadioGroup>
                                )}
                            />
                        </Grid>
                        {mostrar && (
                            <Grid className={classes.column} item xs={12} md={1}>
                                <FormLabel component="legend">Convenio/Entidad</FormLabel>
                                <Controller
                                    name="convenioEntidad"
                                    control={formularioTipoActor.control}
                                    render={({ field }) => (
                                        <RadioGroup
                                            aria-label="convenioEntidad"
                                            name="convenioEntidad"
                                            value={field.value}
                                            onChange={(event) => {

                                                field.onChange(event.target.value)

                                            }}
                                        >
                                            <FormControlLabel value="si"
                                                control={<Radio color="primary" />}
                                                label="Si"

                                            />
                                            <FormControlLabel
                                                value="no"
                                                control={<Radio color="primary" />}
                                                label="No"
                                            />
                                        </RadioGroup>
                                    )}
                                />
                            </Grid>
                        )}

                        {tipoActorSeleccionado?.itA_CODIGO === CODIGO_BENEFICIARIO_ESTUDIO &&
                            (
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="tipoDocumentoAfiliado"
                                        control={formularioTipoActor.control}
                                        rules={{ required: "Campo obligatorio" }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                label="Tipo de Documento Afiliado *"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                            >
                                                {nuevoListadoIdenticacion?.map(
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
                            )}
                        {tipoActorSeleccionado?.itA_CODIGO === CODIGO_BENEFICIARIO_ESTUDIO &&
                            (
                                <Grid className={classes.column} item xs={12} md={4}>
                                    <Controller
                                        name="numeroDocumentoAfiliado"
                                        control={formularioTipoActor.control}
                                        rules={{
                                            required: "Campo obligatorio",
                                            minLength: {
                                                value: 5,
                                                message:
                                                    'La longitud mínima del campo es de 5 caracteres',
                                            },
                                            maxLength: {
                                                value: 15,
                                                message:
                                                    'La longitud máxima del campo es de 15 caracteres',
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
                                                numberMask
                                                InputLabelProps={{ shrink: true }}
                                                label="No. Documento Afiliado *"
                                                value={value}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            )}
                    </Grid>
                </CHPaper>
                <div className="buttons">
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={formularioTipoActor.handleSubmit(crearTipoActor)}
                    >
                        Crear
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={() => history.push('/GestionIntervinientes')}
                    >
                        Regresar
                    </Button>
                </div>
            </CHPrivateContainer>

            <AddressComponentV2
                onOpen={open}
                onClose={onCloseAddress}
                tipoDireccion={1 === 1}
            />
        </>
    )
}

export default AgregarNuevoActor;