import {
    Button, Card,
    CardContent,
    Container,
    createStyles,
    Dialog,
    Grid,
    makeStyles,
    MenuItem,
    TextField,
    Typography
} from '@material-ui/core';
import React from 'react';
import { Controller } from 'react-hook-form';
import CHSelect from '../../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../../components/ch-text-field/CHTextField';

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

export const ModalPlazosGeneral = (props: any) => {

    const classes = useStyles();

    const { handleClose,
        open,
        handleUpdate,
        formularioParametrizacionPlazos
    } = props;

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md">
                    <Card style={{ width: '500px' }}>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    ACTUALIZAR PARÁMETROS PLAZOS GENERALES
                                </Typography>
                            </Grid>
                            <br />
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="plazoId"
                                        control={formularioParametrizacionPlazos.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                label="Línea / Tipo Crédito"
                                                value={value}
                                                onChange={onChange}
                                                readOnly
                                            >
                                                <MenuItem key={1} value={1}>HIPOTECARIO</MenuItem>
                                                <MenuItem key={2} value={2}>LEASING HABITACIONAL</MenuItem>
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="plazoMinimoMes"
                                        control={formularioParametrizacionPlazos.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            maxLength: {
                                                value: 4,
                                                message: 'Máximo 4 dígitos'
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                numberMaskInt
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{ maxLength: 4 }}
                                                label="Plazo Mínimo (Meses) *"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="plazoMinimoAnio"
                                        control={formularioParametrizacionPlazos.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            maxLength: {
                                                value: 4,
                                                message: 'Máximo 4 dígitos'
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                numberMaskInt
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{ maxLength: 4 }}
                                                label="Plazo Mínimo (Años) *"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="plazoMaximoMes"
                                        control={formularioParametrizacionPlazos.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            maxLength: {
                                                value: 4,
                                                message: 'Máximo 4 dígitos'
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                numberMaskInt
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{ maxLength: 4 }}
                                                label="Plazo Máximo (Meses) *"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="plazoMaximoAnio"
                                        control={formularioParametrizacionPlazos.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            maxLength: {
                                                value: 4,
                                                message: 'Máximo 4 dígitos'
                                            }
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                numberMaskInt
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{ maxLength: 4 }}
                                                label="Plazo Máximo (Años) *"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
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
                                        onClick={formularioParametrizacionPlazos.handleSubmit(handleUpdate)}
                                    >
                                        Actualizar
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
    )
}

export default ModalPlazosGeneral;