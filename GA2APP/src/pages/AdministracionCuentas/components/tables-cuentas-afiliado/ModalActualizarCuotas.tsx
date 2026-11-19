import React from 'react';
import {
    Button, Card,
    CardContent,
    Container,
    createStyles,
    Dialog,
    Grid,
    makeStyles,
    MenuItem,
    Typography
} from '@material-ui/core';

import { Controller, useForm } from 'react-hook-form';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';

interface Props {
    open: boolean;
    onClose: () => void;
    data: any;
}

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

function ModalActualizarCuotas({
    open,
    onClose,
    data,
}: Props) {

    const classes = useStyles();

    const { control, handleSubmit } = useForm({
        defaultValues: {
            numeroRadicado: '',
            categoria: '',
            noCuotas: 0,
            totalCuotas: 0,
            fechaInicial: new Date(),
            fechaFinal: new Date(),
            observaciones: ''
        },
    });

    const categoryList = [
        'a',
        'b'
    ];

    return (
        <div>
            <Dialog
                open={open}
                onClose={onClose}
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
                                    ACTUALIZAR CUOTAS POR CATEGORIA
                                </Typography>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="numeroRadicado"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Número de Radicado *"
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
                                        name="categoria"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                label="Categoria"
                                                onChange={onChange}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            >
                                                {
                                                    categoryList?.map(
                                                        (option: any) => (
                                                            <MenuItem
                                                                key={option}
                                                                value={option}
                                                            >
                                                                {option}
                                                            </MenuItem>
                                                        ))
                                                }

                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="noCuotas"
                                        control={control}
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
                                                label="No. Cuotas *"
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
                                        name="totalCuotas"
                                        control={control}
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
                                                label="Total de Cuotas"
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
                                        name="fechaInicial"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                label="Fecha Inicial *"
                                                onChange={onChange}
                                                value={value}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>

                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="fechaFinal"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                label="Fecha Final"
                                                onChange={onChange}
                                                value={value}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>

                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="observaciones"
                                        control={control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                value={value}
                                                onChange={onChange}
                                                label="Observaciones"
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
                                    // onClick={formularioPlazosTipoActor.handleSubmit(handleOnClickEditar)}
                                    >
                                        Actualizar
                                    </Button>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={onClose}
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

export default ModalActualizarCuotas;
