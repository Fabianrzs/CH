import React, { useState, useEffect } from 'react';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import { SetError } from '../../../actions/catalog.action';
import CHSelect from '../../../components/ch-select/CHSelect';
import ApiCourt from '../../../services/api-courts.service'
import { despachosJudicialesForm } from '../../embargos/constants/embargosform.model.type';
import { CuentaDepositoDto, JuzgadoDto } from '../../../types/court.model.type';
import { juzgadoForm } from '../constants/courtform.model.type';
import { CITYDTO, DEPARTAMENTODTO } from '../../../types/catalog.type';
import ApiCatalog from '../../../services/api-catalog.service';
import NotificationService from '../../../services/notification.service';
import { ModalTipoActor } from './ModalTipoActor';




const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);


export const ModalCourtAdd = (props: any) => {
    const classes = useStyles();

    const {
        open,
        onClose,
        allDepartaments,
        handleAdd,
        handleEdit,
        allTipoActor,
        formulario,
        estado,
        informacion,
        formularioActor,
    } = props;



    const deshabilitar = formulario.watch('tipoActor') === '1';

    const [actores, setActores] = useState([]);
    const [nuevoTipoActorEnabled, setNuevoTipoActorEnabled] = useState();
    const [loading, setLoading] = useState(false);
    const [isButton, setIsButton] = useState(Boolean);
    const [bandera, setBandera] = useState(Boolean);
    const api = ApiCatalog();
    const apiCourt = ApiCourt();
    const [departamentValue, setDepartamentValue] = useState<DEPARTAMENTODTO | null>(null);
    const [nullDepartamentValue, setNullDepartamentValue] = useState(false);
    const [cityValue, setCityValue] = useState<CITYDTO | null>(null);
    const [nullCityValue, setNullCityValue] = useState(false);
    const [cities, setCities] = useState<CITYDTO[]>([]);
    const [newForm, setNewForm] = useState(false);
    const [newTipoActor, setNewTipoActor] = useState<any[]>([]);
    const [tipoActor, setTipoActor] = useState();
    const [cache, setCache] = useState<JuzgadoDto[]>([]);

    interface TipoActor {
        descripcionTipoActorEmbargo: string;
    }

    const [descripcionJuzgado, setDescripcionJuzgado] = useState<TipoActor | null>(null);
    const descripTipoActor = descripcionJuzgado?.descripcionTipoActorEmbargo;



    useEffect(() => {
        setCache([informacion]);
    }, [informacion]);

    useEffect(() => {
        obtenerTipoActorEmbargo();
        if (open) {
            if (!estado) {
                onChangeDepartament(null, null);
                onChangeCity(null, null);
            } else {
                editar(estado);
            }
        }
    }, [open]);


    useEffect(() => {
        EstadoForm();
        setCache([informacion]);
    }, [newForm]);

    const EstadoForm = async () => {
        if (newForm === false) {

            formulario.setValue("nombre", "");
            formulario.setValue("codigoInterno", "");
            formulario.setValue("nroCuenta", "");
            formulario.setValue("codigoOficina", "");
            formulario.setValue("idDepartamento", null);
            formulario.setValue("idCiudad", null);
            formulario.setValue("idJuzgado", null);
            formulario.setValue("correoElectronico", "");
            formulario.setValue("tipoActor", "");
            if (bandera === false) {
                setIsButton(false);
            }
        } else {
            formulario.setValue("otroTipoActor", "");
            formulario.setValue("otroTipoActorComf", "");
        }

    }
    const editar = async (estado: JuzgadoDto) => {
        if (estado) {
            setIsButton(true);
            await onChangeDepartament(null, estado.DepartamentValue);
            await onChangeCity(null, estado.setCityValue);
        }
    }


    const validarCampos = async (cuentaDeposito: CuentaDepositoDto) => {

        if (cache[0]?.codigoInterno !== cuentaDeposito.codigoInterno && cache[0]?.nroCuenta !== cuentaDeposito.nCuenta) {
            try {
                if (cuentaDeposito.codigoInterno !== '' || cuentaDeposito.nCuenta !== '') {
                    setLoading(true);

                    const apiCourt = ApiCourt();
                    const response = await apiCourt.postAllCourtsByCodeAndAccountnumber(cuentaDeposito);
                    const datos = response.data as JuzgadoDto[];
                    setCache(response.data)
                    if (datos.length > 0) {
                        formulario.setValue("nombre", datos[0]?.nombre || "");
                        formulario.setValue("codigoInterno", datos[0]?.codigoInterno || "");
                        formulario.setValue("nroCuenta", datos[0]?.nroCuenta || "");
                        formulario.setValue("codigoOficina", datos[0]?.codigoOficina || "");
                        formulario.setValue("idDepartamento", datos[0]?.idDepartamento || null);
                        formulario.setValue("idCiudad", datos[0]?.idCiudad || null);
                        formulario.setValue("idJuzgado", datos[0]?.idJuzgado || null);
                        formulario.setValue("correoElectronico", datos[0]?.correoElectronico || "");
                        formulario.setValue("tipoActor", datos[0]?.tipoActor || "");
                        DepartamentCity(datos);
                        setIsButton(true);
                        toast('Ya se encuentra registrado un juzgado con este código y cuenta de depósito')
                        editar(estado);
                        setBandera(true)

                    } else {
                        setIsButton(false);
                        setBandera(false);
                        setCache([])

                    }
                    setLoading(false);
                }
            } catch (error) {

                toast('Error del sistema', { type: toast.TYPE.ERROR });
                setLoading(false);
            }


        }




    };
    const DepartamentCity = async (data: any) => {
        let departamento: any = null;
        let ciudad: any = null;
        await api
            .getAllStates()
            .then(async (response) => {
                if (response.data) {
                    departamento = response.data?.find(
                        (item: any) => item.id === data[0]?.idDepartamento
                    );
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

        setDepartamentValue((prevDepartamento) => departamento || prevDepartamento);
        obtenerCiudades(data[0]?.idDepartamento);

        await api
            .getCityOfState(data[0]?.idDepartamento.toString())
            .then(async (response) => {
                if (response.data) {
                    ciudad = response.data?.find(
                        (item: any) => item.id === data[0]?.idCiudad
                    );
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

        setNullCityValue(false);
        setCityValue((prevCiudad) => ciudad || prevCiudad);
        const selectedOption = actores.find(
            (member: any) => member.idTipoActorEmbargo === data.tipoActor
        );
        const dato = {
            DepartamentValue: departamento || null,
            setCityValue: ciudad || null,
        };


        editar(dato)
    }

    const onChangeDepartament = (event: any, departament: DEPARTAMENTODTO | null) => {
        setDepartamentValue(departament);
        setCities([]);
        onChangeCity(null, null)
        formulario.setValue('idDepartamento', departament?.id ?? null);
        obtenerCiudades(departament?.id ?? null);
    };

    const onChangeCity = (event: any, city: CITYDTO | null) => {
        setCityValue(city);
        formulario.setValue('idCiudad', city?.id ?? null);
    };
    const obtenerCiudades = async (idDepto: number | null) => {
        if (idDepto != null) {
            const value = idDepto.toString();
            await api.getCityOfState(value)
                .then(async response => {
                    if (idDepto !== 0) {
                        const datos = response.data as CITYDTO[];
                        setCities(datos);
                    } else {
                        setCities([]);
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        }
    };

    const obtenerTipoActorEmbargo = async () => {
        await api
            .obtenerTipoActorEmbargo()
            .then(async response => {
                if (response.isSuccess) {
                    const datos = response.data;
                    setActores(datos);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    const AgregarTipoActorEmbargo = async (data: JuzgadoDto) => {

        const actor = {
            TPR_NOMBRE: data.otroTipoActor ? data.otroTipoActor.toUpperCase() : null,
        };
    
        try {
            const response = await apiCourt.AgregarTipoActorEmbargo(actor);
            if (response.isSuccess) {
                const datos = response.data;
                setNewTipoActor(datos);
                formularioActor.setValue('otroTipoActorComf', '')
                formularioActor.setValue('otroTipoActor', '')
                await obtenerTipoActorEmbargo();
                await NotificationService.genericAlert(
                    "Nuevo tipo de actor",
                    "Se agregó con éxito el nuevo tipo de actor"
                );
            } else {
                setNewTipoActor([]);
            }
    
            return response.data;
        } catch (error: any) {
            const errorMessage = error.returnMessage || "EL TIPO DE ACTOR ESTA REGISTRADO";
            await NotificationService.genericAlertError("ERROR", errorMessage);
            throw error;
        }
    };
    

    const EliminarTipoActorEmbargo = async () => {

        if (!tipoActor || tipoActor === 0 || typeof tipoActor !== 'number') {
            formularioActor.setError('tipoActor', { type: 'manual', message: 'Este campo es obligatorio para eliminar' });
            return Promise.reject(new Error('Tipo de actor no válido'));
        }

        try {
            const response = await apiCourt.EliminarTipoActorEmbargo(tipoActor);
            if (response.isSuccess) {
                formularioActor.setValue('tipoActorSecond', null);  

                await NotificationService.genericAlert(
                    "ELIMINADO",
                    "Se eliminó con éxito el nuevo tipo de actor"
                );
                await obtenerTipoActorEmbargo();
            }
            return response.data;
        } catch (error) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
            throw error;
        }
    };

  
    

    const Close = async () => {
        onClose();
        setNewForm(false);
        onChangeDepartament(null, null);
        onChangeCity(null, null)
        setCities([]);
        formulario.setValue('idCiudad', null);
        setIsButton(false);
        setCache([])
    }
    const handleChangeTipoActor = (event: any) => {
        formulario.setValue("tipoActor", event.target.value);

        if (event.target.value !== "5") {
            formulario.register('correoElectronico', {
                required: false
            });
            formulario.clearErrors('correoElectronico')
        }
    };


    return (
        <Dialog
            fullWidth
            maxWidth="sm"
            open={open}
            aria-labelledby="form-dialog-title"
        >
            <DialogContent>
                {newForm ? (
                    <Grid item>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            AGREGAR NUEVO TIPO DE ACTOR
                        </Typography>
                    </Grid>
                ) : (
                    <Grid item>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            {isButton ? 'ACTUALIZAR DESPACHO JUDICIAL' : 'AGREGAR NUEVO DESPACHO JUDICIAL'}
                        </Typography>
                    </Grid>
                )}
                <br />
                {newForm ? (
                    <>
                        <ModalTipoActor
                            actores={actores}
                            formularioActor={formularioActor}
                            setTipoActor={setTipoActor}
                            setNewForm={setNewForm}
                            EliminarTipoActorEmbargo={EliminarTipoActorEmbargo}
                            AgregarTipoActorEmbargo={AgregarTipoActorEmbargo}

                        />
                    </>
                ) : (
                    <>
                        <Grid container spacing={3}>
                            <Grid item xs={10}>
                                <Controller
                                    name='tipoActor'
                                    control={formulario.control}
                                    rules={{
                                        validate: () => formulario.getValues("tipoActor") !== 0,
                                        required: 'Este campo es obligatorio',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo de actor *"
                                            value={value}
                                            helperText={error ? error.message : null}
                                            // onChange={(event) => {
                                            //     handleChangeTipoActor(event);
                                            //     onChange(event);
                                            //     const selectedOption = actores.find((member: any) => member.idTipoActorEmbargo === event.target.value);
                                            //     setNuevoTipoActorEnabled(formulario.getValues("tipoActor"));
                                            //     setDescripcionJuzgado(selectedOption)
                                            // }}
                                            onChange={(event) => {
                                                handleChangeTipoActor(event);
                                                onChange(event);

                                                // Encontrar el actor seleccionado
                                                const selectedOption = actores.find((member: any) => member.idTipoActorEmbargo === event.target.value);

                                                // Verificar si se encontró una opción antes de actualizar el estado
                                                if (selectedOption) {
                                                    setDescripcionJuzgado(selectedOption); // Solo se actualiza si se encuentra una opción válida
                                                }

                                                // Actualizar otro estado basado en el valor seleccionado
                                                setNuevoTipoActorEnabled(formulario.getValues("tipoActor"));
                                            }}
                                        >
                                            {actores?.map((member: any, index: any) => (
                                                <MenuItem
                                                    key={member.idTipoActorEmbargo}
                                                    value={member.idTipoActorEmbargo}
                                                >
                                                    {member.descripcionTipoActorEmbargo}

                                                </MenuItem>
                                            ))}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            <Grid item xs={10}>
                                <Controller
                                    name="codigoInterno"
                                    control={formulario.control}
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
                                        minLength: {
                                            value: 12,
                                            message:
                                                'La longitud máxima del campo es de 12 caracteres',
                                        }
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            label="Código *"
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            value={value}
                                            onChange={e => {
                                                const numericValue = e.target.value.replace(/[^0-9]/g, '');
                                                if (numericValue.length <= 12) {
                                                    onChange(numericValue);

                                                }
                                            }}
                                            disabled={deshabilitar}
                                            onBlur={() => {
                                                const codigoYDeposito = {
                                                    codigoInterno: formulario.getValues('codigoInterno'),
                                                    nCuenta: formulario.getValues('nroCuenta')
                                                };
                                                validarCampos(codigoYDeposito);
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            <Grid item xs={10}>
                                <Controller
                                    name="nroCuenta"
                                    control={formulario.control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido',
                                        },
                                        maxLength: {
                                            value: 12,
                                            message: 'La longitud máxima del campo es de 12 caracteres',
                                        },
                                        minLength: {
                                            value: 12,
                                            message: 'La longitud máxima del campo es de 12 caracteres',
                                        }
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            label="Cuenta de Depósito *"
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{
                                                inputMode: 'numeric',
                                                maxLength: 12,
                                                minLength: 12,
                                            }}
                                            value={value}
                                            onChange={e => {
                                                const numericValue = e.target.value.replace(/[^0-9]/g, '');
                                                onChange(numericValue);
                                            }}
                                            disabled={deshabilitar}
                                            onBlur={() => {
                                                const codigoYDeposito = {
                                                    codigoInterno: formulario.getValues('codigoInterno'),
                                                    nCuenta: formulario.getValues('nroCuenta')
                                                };
                                                validarCampos(codigoYDeposito);
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            <Grid item xs={10}>
                                <Controller
                                    name="codigoOficina"
                                    control={formulario.control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido',
                                        },
                                        maxLength: {
                                            value: 4,
                                            message: 'La longitud máxima del campo es de 4 caracteres',
                                        },
                                        minLength: {
                                            value: 4,
                                            message: 'La longitud mínima del campo es de 4 caracteres',
                                        },
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            label="Oficina de Destino *"
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            value={value}
                                            inputProps={{
                                                inputMode: 'numeric',
                                                maxLength: 4,
                                                minLength: 4,
                                            }}
                                            onChange={e => {
                                                const numericValue = e.target.value.replace(/[^0-9]/g, '');
                                                onChange(numericValue);
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>

                        <Grid container spacing={3}>
                            <Grid item xs={10}>
                                <Controller
                                    name="nombre"
                                    control={formulario.control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido',
                                        },
                                        minLength: {
                                            value: 6,
                                            message: 'La longitud mínima del campo es de 6 caracteres',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            label="Nombre Despacho Judicial *"
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            value={value}
                                            // onChange={onChange}
                                            onChange={(e) => {
                                                const newValue = e.target.value;
                                                if (/^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9.\s-]*$/.test(newValue)) {
                                                    onChange(newValue);
                                                }
                                            }}
                                            inputProps={{
                                                style: {
                                                    // textTransform: 'uppercase',
                                                },
                                            }}
                                            style={{
                                                margin: "center"
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>

                        <Grid container spacing={3}>
                            <Grid item xs={10}>
                                <Controller
                                    name="idDepartamento"
                                    control={formulario.control}
                                    rules={{
                                        required: "Este campo es obligatorio",
                                        // validate: value => value !== 0 && /^[a-zA-Z\s]+$/.test(value),
                                    }}
                                    render={({ field, fieldState: { error } }) => (
                                        <CHAutoComplete
                                            value={nullDepartamentValue ? null : departamentValue}
                                            options={allDepartaments}
                                            getOptionLabel={option => option.nombre}
                                            onChange={(evento, departament) => {
                                                field.onChange(departament ? departament.nombre : '');
                                                onChangeDepartament(evento, departament);
                                            }}
                                            renderInput={params => (
                                                <CHTextField
                                                    {...params}
                                                    label="Departamento *"
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    InputLabelProps={{
                                                        shrink: true,
                                                    }}
                                                    inputProps={{
                                                        ...params.inputProps,
                                                        onInput: (e) => {
                                                            e.target.value = e.target.value.replace(/[^a-zA-Z\s]/g, '');
                                                        }
                                                    }}
                                                />
                                            )}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            <Grid item xs={10}>
                                <Controller
                                    name="idCiudad"
                                    control={formulario.control}
                                    rules={{
                                        required: "Este campo es obligatorio",
                                        // validate: value => value !== 0 && /^[a-zA-Z\s]+$/.test(value),
                                    }}
                                    render={({ field, fieldState: { error } }) => (
                                        <CHAutoComplete
                                            value={nullCityValue ? null : cityValue}
                                            options={cities}
                                            getOptionLabel={option => option.nombre}
                                            onChange={(evento, city) => {
                                                field.onChange(city ? city.nombre : '');
                                                onChangeCity(evento, city);
                                            }}
                                            renderInput={params => (
                                                <CHTextField
                                                    {...params}
                                                    label="Ciudad *"
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    InputLabelProps={{
                                                        shrink: true,
                                                    }}
                                                    inputProps={{
                                                        ...params.inputProps,
                                                        onInput: (e) => {
                                                            e.target.value = e.target.value.replace(/[^a-zA-Z\s]/g, '');
                                                        }
                                                    }}
                                                />
                                            )}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>

                        <Grid container spacing={3}>
                            <Grid item xs={10}>
                                <Controller
                                    name="correoElectronico"
                                    control={formulario.control}
                                    rules={{
                                        required: 'Este campo es obligatorio',
                                        pattern: {
                                            value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
                                            message: 'Este campo debe ser un correo electrónico válido',
                                        },
                                        validate: {
                                            checkDomain: (value) => {
                                                const domain = value ? value.split('@')[1].toLowerCase() : '';
                                                const tipoActor = formulario.getValues('tipoActor');

                                                if (descripTipoActor === 'ICBF') {
                                                    const domainIcbf = [
                                                        'icbf.gov.co',
                                                        'cajahonor.gov.co',
                                                        'cortesuprema.gov.co',
                                                        'consejodeestado.gov.co',
                                                        'corteconstitucional.gov.co',
                                                        'cendoj.ramajudicial.gov.co',
                                                        'deaj.ramajudicial.gov.co',
                                                        'consejosuperior.ramajudicial.gov.co',
                                                        'outlook.com',
                                                        'gmail.com',
                                                        'hotmail.com',
                                                    ]
                                                    if (!domainIcbf.includes(domain)) {
                                                        return `Solo se permiten correos electrónicos con los siguientes dominios: ${domainIcbf.join(', ')}`;
                                                    }
                                                } else if (descripTipoActor === 'PROCURADURÍA') {
                                                    const domainProcuraduria = [
                                                        'procuraduria.gov.co',
                                                        'cajahonor.gov.co',
                                                        'cortesuprema.gov.co',
                                                        'consejodeestado.gov.co',
                                                        'corteconstitucional.gov.co',
                                                        'cendoj.ramajudicial.gov.co',
                                                        'deaj.ramajudicial.gov.co',
                                                        'consejosuperior.ramajudicial.gov.co',
                                                        'outlook.com',
                                                        'gmail.com',
                                                        'hotmail.com',
                                                    ]
                                                    if (!domainProcuraduria.includes(domain)) {
                                                        return `Solo se permiten correos electrónicos con los siguientes dominios: ${domainProcuraduria.join(', ')}`;
                                                    }
                                                } else if (tipoActor === 5) {
                                                    const validDomains = [
                                                        'cajahonor.gov.co',
                                                        'cortesuprema.gov.co',
                                                        'consejodeestado.gov.co',
                                                        'corteconstitucional.gov.co',
                                                        'cendoj.ramajudicial.gov.co',
                                                        'deaj.ramajudicial.gov.co',
                                                        'consejosuperior.ramajudicial.gov.co',
                                                        'notificacionesrj.gov.co',
                                                    ];

                                                    if (!validDomains.includes(domain)) {
                                                        return `Solo se permiten correos electrónicos con los siguientes dominios: ${validDomains.join(', ')}`;
                                                    }
                                                } else {
                                                    const validDomains = [
                                                        'cajahonor.gov.co',
                                                        'cortesuprema.gov.co',
                                                        'consejodeestado.gov.co',
                                                        'corteconstitucional.gov.co',
                                                        'cendoj.ramajudicial.gov.co',
                                                        'deaj.ramajudicial.gov.co',
                                                        'consejosuperior.ramajudicial.gov.co',
                                                        'outlook.com',
                                                        'gmail.com',
                                                        'hotmail.com',
                                                    ];
                                                    if (domain && !validDomains.includes(domain)) {
                                                        return `Solo se permiten correos electrónicos con los siguientes dominios: ${validDomains.join(', ')}`;
                                                    }
                                                }
                                                return true;
                                            },
                                            noSpaces: value => {
                                                if (value && /\s/.test(value)) {
                                                    return 'El correo electrónico no puede contener espacios';
                                                }
                                                return undefined;
                                            },
                                            noDuplicateCom: value => {
                                                if (value && (value.match(/\.com/g) || []).length > 1) {
                                                    return 'El correo electrónico no puede tener más de un ".com"';
                                                }
                                                return undefined;
                                            },
                                        },
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            label='Correo Electrónico *'
                                            type="email"
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            value={value}
                                            onChange={onChange}
                                        />
                                    )}
                                />

                            </Grid>
                        </Grid>
                        <Grid item xs={12}>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={isButton ? formulario.handleSubmit(handleEdit) : formulario.handleSubmit(handleAdd)}
                                style={{
                                    marginLeft: '10px',
                                }}
                            >
                                {isButton ? 'ACTUALIZAR' : 'GUARDAR'}
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={() => setNewForm(newForm => !newForm)}
                                style={{
                                    marginLeft: '10px',
                                }}
                            >
                                AGREGAR NUEVO TIPO DE ACTOR
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                onClick={() => Close()}
                                style={{
                                    marginLeft: '10px',
                                }}
                            >
                                Cancelar
                            </Button>
                        </Grid>
                    </>
                )}

            </DialogContent>
        </Dialog>
    );

};