import { connect } from 'react-redux';
import {
    Button,
    createStyles,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    FormControl,
    Grid,
    makeStyles,
    MenuItem,
    Theme,
} from '@material-ui/core';
import React, { useEffect } from 'react';
import { Controller, useFormContext, UseFormSetValue, UseFormTrigger, FormProvider } from 'react-hook-form';
import { toast } from 'react-toastify';
import { AppState } from '../../../store/rootReducer';
import { Address } from '../../../types/address.type';
import { LoadingComponent } from '../../loading/loading.component';
import {
    startObtenerDireccionesTipos,
    startObtenerDireccionesCardinales,
    startObtenerDireccionesAbecedarios,
    startObtenerDireccionesBis,
} from '../../../actions/address.action';
import CHTextField from '../../ch-text-field/CHTextField';
import CHSelect from '../../ch-select/CHSelect';
import { AddressState } from '../../../reducers/address.reducer';


const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        formControl: {
            margin: theme.spacing(0),
        },
        containerForm: {
            padding: theme.spacing(1),
        },
        form: {
            flexGrow: 1,
            margin: theme.spacing(1),
            fontSize: '10px',
        },
        textfields: {},
        errorscolors: {
            color: 'red',
        },
    }),
);

const AddressComponent = ({
    loginState,
    control,
    getValues,
    addressState,
    onOpen,
    onClose,
    startObtenerDireccionesTipos,
    startObtenerDireccionesCardinales,
    startObtenerDireccionesAbecedarios,
    startObtenerDireccionesBis,
    tipoDireccion,
    setValue,
}: {
    loginState: any;
    control: any;
    getValues: any;
    addressState: AddressState;
    onOpen: boolean;
    onClose: (value: any) => void;
    startObtenerDireccionesTipos: (claims: any) => void;
    startObtenerDireccionesCardinales: (claims: any) => void;
    startObtenerDireccionesAbecedarios: (claims: any) => void;
    startObtenerDireccionesBis: (claims: any) => void;
    tipoDireccion: boolean;
    setValue: UseFormSetValue<any>;
}) => {
    const classes = useStyles();
    let value: Address | null = null;

    const methods = useFormContext();
    const trigger = methods?.trigger;


    const loadData = () => {
        startObtenerDireccionesTipos(loginState.claims);
        startObtenerDireccionesCardinales(loginState.claims);
        startObtenerDireccionesAbecedarios(loginState.claims);
        startObtenerDireccionesBis(loginState.claims);
    };

    useEffect(() => {
        loadData();
    }, []);

    const ObligatorioCamposAddress = () => {
        if (
            getValues('idTipoCalle') === '' ||
            getValues('idTipoCalle') === 'N/A'
        ) {
            toast('Ingrese un tipo de calle obligatorio (*)');
        } else if (getValues('numeroCalle1') === '') {
            toast('Ingrese  el número de calle obligatorio (*)');
        } else if (getValues('numeroCalle2') === '') {
            toast('Ingrese  el número de calle obligatorio (*)');
        } else if (getValues('numeroCalle3') === '') {
            toast('Ingrese  el número de calle obligatorio (*)');
        } else {
            handleConfimValue();
        }
    };

    const handleConfimValue = () => {
        if (getValues('idTipoCalle') === 0) {
            toast('Seleccione el tipo de calle');
        } else if (getValues('numeroCalle1') === 0) {
            toast('Seleccione el tipo de calle');
        } else if (getValues('numeroCalle2') === 0) {
            toast('Seleccione el tipo de calle');
        } else if (getValues('numeroCalle3') === 0) {
            toast('Seleccione el tipo de calle');


        } else {
            agregarCamposDireccion();
        }
    };


    /**
     * ⚠️ IMPORTANTE:
     * Este componente se reutiliza en varios módulos.
     * La validación NO debe forzar estructura siempre.
     * Si existe direccionComplemento, se permite guardar sin validar campos estructurados.
     * NO usar trigger() aquí sin aislar el contexto.
     */


    const submit = () => {
        if (getValues('direccionComplemento')) {
            agregarCamposDireccion();
        } else {
            ObligatorioCamposAddress();
        }
    }



    const agregarCamposDireccion = () => {
        const tipoCalleValor = addressState.direccionesTipos.find(

            x => x.id === getValues('idTipoCalle'),

        )?.descripcion;

        const letra1Valor = addressState.direccionesAbecedario.find(

            x => x.id === getValues('idLetra1') && +getValues('idLetra1') > 0,

        )?.descripcion;

        const bis1Valor = addressState.direccionesAbecedario.find(
            x =>
                x.id === +getValues('direccionBis1') &&
                +getValues('direccionBis1') > 0,
        )?.descripcion;

        const cardinal1Valor = addressState.direccionesCardinales.find(
            x =>
                x.id === +getValues('idCardinal1') &&
                +getValues('idCardinal1') > 0,
        )?.descripcion;

        const letra2Valor = addressState.direccionesAbecedario.find(

            x => x.id === +getValues('idLetra2') && +getValues('idLetra2') > 0,

        )?.descripcion;

        const bis2Valor = addressState.direccionesAbecedario.find(
            x =>
                x.id === +getValues('direccionBis2') &&
                +getValues('direccionBis2') > 0,
        )?.descripcion;

        const cardinal2Valor = addressState.direccionesCardinales.find(
            x =>
                x.id === +getValues('idCardinal2') &&
                +getValues('idCardinal2') > 0,
        )?.descripcion;

        const letra3Valor = addressState.direccionesAbecedario.find(
            x => x.id === +getValues('idLetra3') && +getValues('idLetra3') > 0,
        )?.descripcion;


        const cardinal3Valor = addressState.direccionesCardinales.find(
            x =>
                x.id === +getValues('idCardinal3') &&
                +getValues('idCardinal3') > 0,
        )?.descripcion;
        value = {
            idTipoCalle:
                getValues('idTipoCalle') !== 'N/A'
                    ? getValues('idTipoCalle')
                    : '',
            tipoCalleValue: tipoCalleValor || '',
            numeroCalle1: getValues('numeroCalle1') || '',
            idLetra1: getValues('idLetra1') || '',
            direccionLetra1Value: letra1Valor == null ? '' : letra1Valor,
            direccionBis1:
                getValues('direccionBis1') !== 'N/A'
                    ? getValues('direccionBis1')
                    : '',
            direccionBis1Value: bis1Valor == null ? '' : bis1Valor,
            idCardinal1: getValues('idCardinal1') || '',
            direccionCardinal1Value:
                cardinal1Valor == null ? '' : cardinal1Valor,
            numeroCalle2: getValues('numeroCalle2') || '',
            idLetra2: getValues('idLetra2') || '',
            direccionLetra2Value: letra2Valor == null ? '' : letra2Valor,
            direccionBis2:
                getValues('direccionBis2') !== 'N/A'
                    ? getValues('direccionBis2')
                    : '',
            direccionBis2Value: bis2Valor == null ? '' : bis2Valor,
            idCardinal2: getValues('idCardinal2') || '',
            direccionCardinal2Value:
                cardinal2Valor == null ? '' : cardinal2Valor,
            numeroCalle3: getValues('numeroCalle3') || '',
            idLetra3: getValues('idLetra3') || '',
            direccionLetra3Value: letra3Valor == null ? '' : letra3Valor,
            idCardinal3: getValues('idCardinal3') || '',
            direccionCardinal3Value:
                cardinal3Valor == null ? '' : cardinal3Valor,
            direccionComplemento: getValues('direccionComplemento') || '',
        };
        onClose(value);
    };

    const handleClose = () => {
        onClose(null);
    };


    useEffect(() => {
        if (!onOpen) return;

        const dir = getValues('direccion');

        const isFilled = (v: any) => {
            if (v === null || v === undefined) return false;
            if (typeof v === 'number') return v > 0;
            const s = String(v).trim();
            if (s === '' || s === '0') return false;
            return true;
        };

        const structuredFields = [
            'idTipoCalle',
            'numeroCalle1',
            'numeroCalle2',
            'numeroCalle3',
            'idLetra1',
            'idLetra2',
            'idLetra3',
            'idCardinal1',
            'idCardinal2',
            'idCardinal3',
            'direccionBis1',
            'direccionBis2',
        ];

        const hasStructured = structuredFields.some((f) => isFilled(getValues(f)));

        if (!hasStructured && dir) {
            setValue('direccionComplemento', dir);
        } else {
            setValue('direccionComplemento', '');
        }
    }, [onOpen]);




    return (
        <Grid item container>
            <LoadingComponent key="loader" loading={addressState.loading} />
            <Grid item xs={12} sm={6}>
                <Dialog
                    fullWidth
                    maxWidth="md"
                    open={onOpen}
                    aria-labelledby="form-dialog-title"
                >
                    <DialogTitle id="form-dialog-title">DIRECCIÓN</DialogTitle>
                    <DialogContent>
                        <DialogContentText>
                            Por favor ingrese los campos de la dirección.
                        </DialogContentText>
                        <Grid item container className={classes.containerForm}>
                            <div>
                                <Grid container spacing={1}>
                                    <Grid item xs={12} sm={3}>
                                        <Controller
                                            name="idTipoCalle"
                                            control={control}
                                            rules={{
                                                validate: value =>
                                                    value && value !== 'N/A'
                                                        ? true
                                                        : 'Este campo es requerido.',
                                            }}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Tipo calle"
                                                >
                                                    <MenuItem
                                                        key="N/A"
                                                        value="N/A"
                                                    >
                                                        N/A
                                                    </MenuItem>
                                                    {addressState.direccionesTipos?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid item xs={12} sm={3}>
                                        <Controller
                                            control={control}
                                            rules={{
                                                required: 'Requerido',
                                                validate: (v) => Number(v) > 0 || 'Requerido',
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={
                                                        error
                                                            ? error.message
                                                            : null
                                                    }
                                                    isReadOnly={false}
                                                    label="Número*"
                                                    value={value}
                                                    numberMask
                                                    inputProps={
                                                        { maxLength: 5 }
                                                    }
                                                    onChange={e => {
                                                        onChange(e);
                                                    }}
                                                />
                                            )}
                                            name="numeroCalle1"
                                        />
                                    </Grid>
                                    <Grid item xs={12} sm={2}>
                                        <Controller
                                            name="idLetra1"
                                            control={control}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Letra"
                                                >
                                                    {addressState.direccionesAbecedario?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid item xs={12} sm={2}>
                                        <Controller
                                            name="direccionBis1"
                                            control={control}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Bis"
                                                >
                                                    <MenuItem
                                                        key="N/A"
                                                        value="N/A"
                                                    >
                                                        N/A
                                                    </MenuItem>
                                                    {addressState.direccionesTipos?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid item xs={12} sm={2}>
                                        <Controller
                                            name="idCardinal1"
                                            control={control}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Cardinalidad"
                                                >
                                                    {addressState.direccionesCardinales?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid
                                        item
                                        xs={12}
                                        sm={1}
                                        className={classes.textfields}
                                    >
                                        <FormControl>
                                            <Controller
                                                control={control}
                                                rules={{
                                                    required: 'Requerido',
                                                    validate: (v) => Number(v) > 0 || 'Requerido',
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHTextField
                                                        error={!!error}
                                                        helperText={
                                                            error
                                                                ? error.message
                                                                : null
                                                        }
                                                        isReadOnly={false}
                                                        label="Número*"
                                                        numberMask
                                                        inputProps={
                                                            { maxLength: 5 }
                                                        }
                                                        value={value}
                                                        onChange={e => {
                                                            onChange(e);
                                                        }}
                                                    />
                                                )}
                                                name="numeroCalle2"
                                            />
                                        </FormControl>
                                    </Grid>
                                    <Grid item xs={12} sm={2}>
                                        <Controller
                                            name="idLetra2"
                                            control={control}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Letra"
                                                >
                                                    {addressState.direccionesAbecedario?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid item xs={12} sm={2}>
                                        <Controller
                                            name="direccionBis2"
                                            control={control}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Bis"
                                                >
                                                    <MenuItem
                                                        key="N/A"
                                                        value="N/A"
                                                    >
                                                        N/A
                                                    </MenuItem>
                                                    {addressState.direccionesTipos?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>

                                    <Grid item xs={12} sm={2}>
                                        <Controller
                                            name="idCardinal2"
                                            control={control}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Cardinalidad"
                                                >
                                                    {addressState.direccionesCardinales?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid item xs={12} sm={1}>
                                        <FormControl>
                                            <Controller
                                                control={control}
                                                rules={{
                                                    required: 'Requerido',
                                                    validate: (v) => Number(v) > 0 || 'Requerido',
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHTextField
                                                        error={!!error}

                                                        helperText={
                                                            error
                                                                ? error.message
                                                                : null
                                                        }
                                                        isReadOnly={false}
                                                        label="Número*"
                                                        numberMask
                                                        inputProps={
                                                            { maxLength: 5 }
                                                        }
                                                        value={value}
                                                        onChange={e => {
                                                            onChange(e);
                                                        }}
                                                    />
                                                )}
                                                name="numeroCalle3"
                                            />
                                        </FormControl>
                                    </Grid>

                                    <Grid item xs={12} sm={2}>
                                        <Controller
                                            name="idLetra3"
                                            control={control}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Letra"
                                                >
                                                    {addressState.direccionesAbecedario?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>

                                    <Grid item xs={12} sm={3}>
                                        <Controller
                                            name="idCardinal3"
                                            control={control}
                                            defaultValue=""
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
                                                    onChange={onChange}
                                                    value={value}
                                                    label="Cardinalidad"
                                                >
                                                    {addressState.direccionesCardinales?.map(
                                                        direccionTipo => (
                                                            <MenuItem
                                                                key={
                                                                    direccionTipo.id
                                                                }
                                                                value={
                                                                    direccionTipo.id
                                                                }
                                                            >
                                                                {
                                                                    direccionTipo.descripcion
                                                                }
                                                            </MenuItem>
                                                        ),
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>

                                    <Grid
                                        item
                                        xs={12}
                                        sm={6}
                                        className={classes.textfields}
                                    >
                                        <Controller
                                            name="direccionComplemento"
                                            control={control}
                                            rules={{
                                                required: {
                                                    value: !tipoDireccion,
                                                    message:
                                                        'Este campo es requerido.',
                                                },

                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={
                                                        error
                                                            ? error.message
                                                            : null
                                                    }
                                                    isReadOnly={false}
                                                    label="Complemento dirección"
                                                    value={value}
                                                    inputProps={
                                                        { maxLength: 50 }
                                                    }
                                                    onChange={e => {
                                                        onChange(e);
                                                    }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                </Grid>
                            </div>
                        </Grid>
                    </DialogContent>
                    <DialogActions>
                        <Button
                            type="button"
                            onClick={handleClose}
                            color="primary"
                        >
                            Cancelar
                        </Button>
                        <Button type="button" onClick={submit} color="primary">
                            Ingresar
                        </Button>
                    </DialogActions>
                </Dialog>
            </Grid>
        </Grid>
    );
};

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    addressState: state.address,
});

const mapDispatchToProps = {
    startObtenerDireccionesTipos,
    startObtenerDireccionesCardinales,
    startObtenerDireccionesAbecedarios,
    startObtenerDireccionesBis,
};

export default connect(mapStateToProps, mapDispatchToProps)(AddressComponent);
