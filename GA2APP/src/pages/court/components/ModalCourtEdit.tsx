import React, { useEffect, useState } from 'react';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    MenuItem,
    Typography
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import CHSelect from '../../../components/ch-select/CHSelect';
import { CITYDTO, DEPARTAMENTODTO } from '../../../types/catalog.type';
import ApiCatalog from '../../../services/api-catalog.service';


const useStyles = makeStyles((theme) =>
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

export const ModalCourtEdit = (props: any) => {
    const classes = useStyles();

    const {
        open,
        onClose,
        allDepartaments,
        handleEdit,
        formulario,
        allTipoActor
    } = props;

    const deshabilitar = formulario.watch('tipoActor') === '1';
    const [value, setValue] = useState("");
    const [nuevoTipoActorEnabled, setNuevoTipoActorEnabled] = useState(false);
    const [noJuzgado, setNoJuzgado] = useState(false);
    const idModulo = 2;
    const selectedOption = allTipoActor.find((member: any) => member.idTipoActorEmbargo === value);

    useEffect(() => {
        setNoJuzgado(formulario.getValues('tipoActor') === 5)
    }, [selectedOption]);

    const api = ApiCatalog();
    const [departamentValue, setDepartamentValue] = useState<DEPARTAMENTODTO | null>(null);
    const [nullDepartamentValue, setNullDepartamentValue] = useState(false);
    const [cityValue, setCityValue] = useState<CITYDTO | null>(null);
    const [nullCityValue, setNullCityValue] = useState(false);
    const [cities, setCities] = useState<CITYDTO[]>([]);

    const onChangeDepartament = (event: any, departament: DEPARTAMENTODTO) => {
        const city: CITYDTO | null = null;
        setNullCityValue(city == null);
        setCityValue(city);
        setNullDepartamentValue(departament == null);
        setDepartamentValue(departament);
        formulario.setValue('idDepartamento', departament?.id);
        obtenerCiudades(departament?.id);
        if (departament) {
            formulario.clearErrors('idDepartamento');
        } else {
            formulario.setError('idDepartamento', {
                type: 'custom',
                message: 'Este campo es requerido.',
            });
        }
    };

    const onChangeCity = (event: any, city: CITYDTO) => {
        setNullCityValue(city == null);
        setCityValue(city);
        if (city) {
            formulario.setValue('idCiudad', city.id);
            formulario.clearErrors('idCiudad');
        }
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


    return (
        <Dialog
            fullWidth
            maxWidth="sm"
            open={open}
            onClose={onClose}
            aria-labelledby="form-dialog-title"
        >
            <DialogContent>
                <Grid item>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        ACTUALIZAR DESPACHO JUDICIAL
                    </Typography>
                </Grid>
                <br />
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                        <Controller
                            name='tipoActor'
                            control={formulario.control}
                            rules={{
                                validate: () => formulario.getValues("tipoActor") !== 0,
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de actor *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={(event) => {
                                        onChange(event);
                                        const selectedOption = allTipoActor.find(
                                            (member: any) => member.idTipoActorEmbargo === event.target.value
                                        );
                                        setNoJuzgado(selectedOption?.idTipoActorEmbargo === 5);
                                    }}
                                >
                                    {allTipoActor?.map((member: any, index: any) => (
                                        <MenuItem key={member.idTipoActorEmbargo} value={member.idTipoActorEmbargo}>
                                            {member.descripcionTipoActorEmbargo}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                {(formulario.getValues("tipoActor")) === 7 && (
                    <Grid container spacing={3}>
                        <Grid item xs={10}>
                            <Controller
                                name="otroTipoActor"
                                control={formulario.control}
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
                                    <CHTextField
                                        label="Otro Tipo Actor *"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        style={{
                                            margin: "center"
                                        }}
                                        onKeyPress={(event) => {
                                            const keyCode = event.keyCode || event.which;
                                            const keyValue = String.fromCharCode(keyCode);
                                            const regex = /^[a-zA-Z\s]*$/;
                                            if (!regex.test(keyValue)) {
                                                event.preventDefault();
                                            }
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                )}
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
                                    value={value}
                                    onChange={onChange}

                                />
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
                                    message: 'La longitud máxima del campo es de 12 caracteres',
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
                                    label={formulario.getValues('tipoActor') === 5 ? "Código *" : "Código"}
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
                {(formulario.getValues("tipoActor")) === 5 && (
                    <><Grid container spacing={3}>
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
                                        disabled={deshabilitar}
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
                                        minLength: {
                                            value: 12,
                                            message: 'La longitud minima del campo es de 12 caracteres',
                                        },
                                        maxLength: {
                                            value: 12,
                                            message: 'La longitud máxima del campo es de 12 caracteres',
                                        },
                                    }}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            label="Cuenta de Depósito *"
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{ maxLength: 12, minLength: 12 }}
                                            value={value}
                                            onChange={onChange} />
                                    )} />
                            </Grid>
                        </Grid></>
                )}
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                        <Controller
                            name="idDepartamento"
                            control={formulario.control}
                            rules={{
                                validate: () => formulario.getValues("idDepartamento") !== 0,
                            }}
                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    value={
                                        nullDepartamentValue
                                            ? null
                                            : departamentValue
                                    }
                                    options={allDepartaments}
                                    getOptionLabel={option => option.nombre}
                                    onChange={(
                                        evento: any,
                                        departament: any,
                                    ) => {
                                        onChangeDepartament(
                                            evento,
                                            departament,
                                        );
                                    }}
                                    renderInput={params => (
                                        <CHTextField
                                            label="Departamento *"
                                            {...params}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            InputLabelProps={{
                                                shrink: true,
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
                                validate: () => formulario.getValues("idCiudad") !== 0,
                            }}

                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    value={nullCityValue ? null : cityValue}
                                    options={cities}
                                    getOptionLabel={option => option.nombre}
                                    onChange={(evento: any, city: any) => {
                                        onChangeCity(evento, city);
                                    }}
                                    renderInput={params => (
                                        <CHTextField
                                            label="Ciudad *"
                                            {...params}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            InputLabelProps={{
                                                shrink: true,
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
                                pattern: {
                                    value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
                                    message: 'Este campo debe ser un correo electrónico válido'
                                },
                                validate: {
                                    checkDomain: value => {
                                        const domain = value.split('@')[1];
                                        const actorType = formulario.getValues('tipoActor');
                                        const validDomains = [
                                            'cajahonor.gov.co',
                                            'cortesuprema.gov.co',
                                            'consejodeestado.gov.co',
                                            'corteconstitucional.gov.co',
                                            'cendoj.ramajudicial.gov.co',
                                            'deaj.ramajudicial.gov.co',
                                            'consejosuperior.ramajudicial.gov.co',
                                        ];

                                        if (actorType !== 5) {
                                            validDomains.push('outlook.com', 'gmail.com', 'hotmail.com');
                                        }

                                        if (!validDomains.includes(domain)) {
                                            return `Solo se permiten correos electrónicos con los siguientes dominios: ${validDomains.join(', ')}`;
                                        }
                                        return true;
                                    },
                                    noSpaces: value => {
                                        if (/\s/.test(value)) {
                                            return 'El correo electrónico no puede contener espacios';
                                        }
                                        return undefined;
                                    },
                                    noDuplicateCom: value => {
                                        if ((value.match(/\.com/g) || []).length > 2) {
                                            return 'El correo electrónico no puede tener más de un ".com" o un ".gov.co"';
                                        }
                                        return undefined;
                                    },
                                }
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    label={formulario.getValues('tipoActor') === 5 ? "Correo Electrónico *" : "Correo Electrónico"}
                                    type="email"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
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

                <br />
                <Grid item xs={8}>
                    <Button
                        size="small"
                        variant="contained"
                        color="primary"
                        onClick={formulario.handleSubmit(handleEdit)}
                    >
                        Actualizar
                    </Button>
                    <Button
                        size="small"
                        variant="contained"
                        color="default"
                        onClick={onClose}
                        style={{
                            marginLeft: '10px',
                        }}
                    >
                        Cancelar
                    </Button>
                </Grid>
            </DialogContent>
        </Dialog >

    );
};