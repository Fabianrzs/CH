import {
    Button,
    Dialog,
    Card,
    CardContent,
    Container,
    Grid,
    Typography,
    MenuItem,
    makeStyles,
    createStyles,
} from '@material-ui/core';
import React from 'react';
import { Controller } from 'react-hook-form';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';

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

export const ModalAgregarTasaEA = (props: any) => {
    const {
        handleClose,
        open,
        handleOnClickSave,
        formularioParametrosSimulador
    } = props;
    const classes = useStyles();
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="lg">
                    <Card style={{ width: '500px' }}>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    AGREGAR PARAMETRIZACIÓN DE PRODUCTO
                                </Typography>
                            </Grid>
                            <br />
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="descripcion"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }

                                                label="Descripción *"
                                                value={value}
                                                onChange={e => {
                                                    if (e.target.value.length <= 255) {
                                                        onChange(e);
                                                    }
                                                }
                                                }
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="tasaEA"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            min: {
                                                value: 0,
                                                message:
                                                    'No puede ser menor a 0',
                                            },
                                            max: {
                                                value: 100,
                                                message:
                                                    'No puede ser mayor a 100',
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                decimalsMaskByNumber
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'left',
                                                    },
                                                }}
                                                label="Tasa EA (%) *"
                                                value={value || 0}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>

                            {/* <Grid container spacing={3}>
                                <Grid item xs={12}>
                                       <Controller
                                        name="tasaNM"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            min: {
                                                value: 0,
                                                message:
                                                    'No puede ser menor a 0',
                                            },
                                            max: {
                                                value: 100,
                                                message:
                                                    'No puede ser mayor a 100',
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                                    }) => (
                                                    <CHTextField
                                                        decimalsMaskByNumber
                                                        error={!!error}
                                                        helperText={
                                                            error ? error.message : null
                                                        }
                                                        InputLabelProps={{ shrink: true }}
                                                        inputProps={{
                                                            style: {
                                                                textAlign: 'left',
                                                            },
                                                        }}
                                                        label="Tasa N.M(%) *"
                                                        value={value || 0}
                                                        onChange={onChange}
                                                    />
                                             )}
                                        />
                                </Grid>
                            </Grid> */}


                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="minimoPlazoMeses"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                numberMaskInt
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                InputLabelProps={{ shrink: true }}
                                                label="Plazo Mínimo (Meses) *"
                                                value={value}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="maximoPlazoMeses"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                numberMaskInt
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                InputLabelProps={{ shrink: true }}
                                                label="Plazo Máximo (Meses) *"
                                                value={value}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>

                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="min_Canon_Inicial"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            min: {
                                                value: 0,
                                                message:
                                                    'No puede ser menor a 0',
                                            },
                                            max: {
                                                value: 100,
                                                message:
                                                    'No puede ser mayor a 100',
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                decimalsMaskByNumber
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'left',
                                                    },
                                                }}
                                                label="Mínimo cánon inicial*"
                                                value={value || 0}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="max_Canon_Inicial"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            min: {
                                                value: 0,
                                                message:
                                                    'No puede ser menor a 0',
                                            },
                                            max: {
                                                value: 100,
                                                message:
                                                    'No puede ser mayor a 100',
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                decimalsMaskByNumber
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'left',
                                                    },
                                                }}
                                                label="Maximo cánon inicial 30%*"
                                                value={value || 0}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="max_Opc_Compra"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            min: {
                                                value: 0,
                                                message:
                                                    'No puede ser menor a 0',
                                            },
                                            max: {
                                                value: 100,
                                                message:
                                                    'No puede ser mayor a 100',
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                decimalsMaskByNumber
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'left',
                                                    },
                                                }}
                                                label=" Máximo opción de compra(%)* "
                                                value={value || 0}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="seguroHogar"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',

                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                label="Seguro Hogar*"
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            >
                                                <MenuItem value="SI">SI</MenuItem>
                                                <MenuItem value="NO">NO</MenuItem>
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="estado"
                                        control={formularioParametrosSimulador.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                label="Estado"
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                                readOnly
                                            >
                                                <MenuItem value="1">ACTIVO</MenuItem>
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={formularioParametrosSimulador.handleSubmit(handleOnClickSave)}
                                    >
                                        Agregar
                                    </Button>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={handleClose}
                                        style={{ marginLeft: "10px" }}
                                    >
                                        Cancelar
                                    </Button>
                                </Grid>
                            </Grid>
                        </CardContent>
                    </Card>
                </Container>
            </Dialog>
        </div>
    );
};