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
import { Controller, useForm, UseFormSetValue } from 'react-hook-form';
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
// llego a qui
const AddressComponentV2 = ({
    loginState,
    addressState,
    onOpen,
    onClose,
    startObtenerDireccionesTipos,
    startObtenerDireccionesCardinales,
    startObtenerDireccionesAbecedarios,
    startObtenerDireccionesBis,
    tipoDireccion,
}: {
    loginState: any;
    addressState: AddressState;
    onOpen: boolean;
    onClose: (value: any) => void;
    startObtenerDireccionesTipos: (claims: any) => void;
    startObtenerDireccionesCardinales: (claims: any) => void;
    startObtenerDireccionesAbecedarios: (claims: any) => void;
    startObtenerDireccionesBis: (claims: any) => void;
    tipoDireccion: boolean;
}) => {
    const classes = useStyles();
    let value: Address | null = null;

    const loadData = () => {
        startObtenerDireccionesTipos(loginState.claims);
        startObtenerDireccionesCardinales(loginState.claims);
        startObtenerDireccionesAbecedarios(loginState.claims);
        startObtenerDireccionesBis(loginState.claims);
    };

    useEffect(() => {
        
        loadData();
    }, []);

    const { handleSubmit, control, formState: {errors}, reset} = useForm();
    const onSubmit = (address: Address) => {
             
        agregarCamposDireccion(address);
    }
    const handleClose = () => {
        reset();
        onClose(value);
    };
    const agregarCamposDireccion = (address: Address) => {
        
        const tipoCalleValor = addressState.direccionesTipos.find(

            x => x.id === address.idTipoCalle,

        )?.descripcion;

        const letra1Valor = addressState.direccionesAbecedario.find(

            x => x.id === address.idLetra1,

        )?.descripcion;

        const bis1Valor = addressState.direccionesAbecedario.find(
            x =>
                x.id === address.direccionBis1,
        )?.descripcion;

        const cardinal1Valor = addressState.direccionesCardinales.find(
            x =>
                x.id === address.idCardinal1,
        )?.descripcion;

        const letra2Valor = addressState.direccionesAbecedario.find(

            x => x.id === address.idLetra2,

        )?.descripcion;

        const bis2Valor = addressState.direccionesAbecedario.find(
            x =>
                x.id === address.direccionBis2,
        )?.descripcion;

        const cardinal2Valor = addressState.direccionesCardinales.find(
            x =>
                x.id === address.idCardinal2,
        )?.descripcion;

        const letra3Valor = addressState.direccionesAbecedario.find(
            x => x.id === address.idLetra3,
        )?.descripcion;


        const cardinal3Valor = addressState.direccionesCardinales.find(
            x =>
                x.id === address.idCardinal3,
        )?.descripcion;
        value = {
            idTipoCalle:address.idTipoCalle,
            tipoCalleValue: tipoCalleValor || '',
            numeroCalle1: address.numeroCalle1,
            idLetra1: address.idLetra1,
            direccionLetra1Value: letra1Valor == null || letra1Valor === 'N/A' ? '' : letra1Valor,
            direccionBis1: address.direccionBis1,
            direccionBis1Value: bis1Valor == null ? '' : bis1Valor,
            idCardinal1: address.idCardinal1,
            direccionCardinal1Value: cardinal1Valor == null || cardinal1Valor === 'N/A' ? '' : cardinal1Valor,
            numeroCalle2: address.numeroCalle2,
            idLetra2: address.idLetra2,
            direccionLetra2Value: letra2Valor == null || letra2Valor === 'N/A' ? '' : letra2Valor,
            direccionBis2: address.direccionBis2,
            direccionBis2Value: bis2Valor == null ? '' : bis2Valor,
            idCardinal2: address.idCardinal2,
            direccionCardinal2Value: cardinal2Valor == null || cardinal2Valor === 'N/A' ? '' : cardinal2Valor,
            numeroCalle3: address.numeroCalle3,
            idLetra3: address.idLetra3,
            direccionLetra3Value: letra3Valor == null || letra3Valor === 'N/A' ? '' : letra3Valor,
            idCardinal3: address.idCardinal3,
            direccionCardinal3Value: cardinal3Valor == null || cardinal3Valor === 'N/A' ? '' : cardinal3Valor,
            direccionComplemento: address.direccionComplemento,
        };
        onClose(value);
        reset();
    };

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
                    <form onSubmit={handleSubmit(onSubmit)}>
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
                                                required: 'Este campo es requerido.',
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
                                                required: 'Este campo es requerido.',
                                                
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
                                                    label="Número"
                                                    value={value}
                                                    numberMask
                                                    inputProps={
                                                        {maxLength: 5}
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
                                                    required: 'Este campo es requerido.',
                                                    
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
                                                        label="Número"
                                                        numberMask
                                                        inputProps={
                                                            {maxLength: 5}
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
                                                    required: 'Este campo es requerido.',
                                                    
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
                                                        label="Número"
                                                        numberMask
                                                        inputProps={
                                                            {maxLength: 5}
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
                                           
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                   
                                                    isReadOnly={false}
                                                    label="Complemento dirección"
                                                    value={value}
                                                    inputProps={
                                                        {maxLength: 50}
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
                        </form>
                        
                    </DialogContent>
                    <DialogActions>
                        <Button
                            type="button"
                            onClick={handleClose}
                            color="primary"
                        >
                            Cancelar
                        </Button>
                        <Button type="button" onClick={handleSubmit(onSubmit)} color="primary">
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

export default connect(mapStateToProps, mapDispatchToProps)(AddressComponentV2);
