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
    TextField,
    Typography
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHSelect from '../../../../../components/ch-select/CHSelect';
import { UnidadEjecutoraDto } from '../../../../../types/simulador.model.type';
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

const ModalPlazosEditarExcepUnidadEjecutora = (props: any) => {

    const classes = useStyles();

    const { open,
        handleClose,
        allExecutorUnit,
        handleOnClickEditar,
        formularioPlazosUnidadEjecutora
         } = props;

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md">
                    <Card>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    ACTUALIZAR EXCEPCIÓN PLAZOS UNIDAD EJECUTORA
                                </Typography>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                <Controller
                                        name="unidadEjecutoraId"  
                                        control={formularioPlazosUnidadEjecutora.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                            label="Unidad Ejecutora *"
                                            onChange={onChange}
                                            value={value}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        >
                                            {
                                                allExecutorUnit?.map(
                                                    (option: UnidadEjecutoraDto) => (
                                                        <MenuItem
                                                            key={option.idUnidadEjecutora}
                                                            value={option.idUnidadEjecutora}
                                                        >
                                                            {option.nombreUnidadEjecutora}
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
                                    name="plazoMinimoMes"
                                    control={formularioPlazosUnidadEjecutora.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMaskInt
                                            InputLabelProps={{ shrink: true }}
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
                                    control={formularioPlazosUnidadEjecutora.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMaskInt
                                            InputLabelProps={{ shrink: true }}
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
                                    control={formularioPlazosUnidadEjecutora.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMaskInt
                                            InputLabelProps={{ shrink: true }}
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
                                    control={formularioPlazosUnidadEjecutora.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMaskInt
                                            InputLabelProps={{ shrink: true }}
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
                                        onClick={formularioPlazosUnidadEjecutora.handleSubmit(handleOnClickEditar)}
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

export default ModalPlazosEditarExcepUnidadEjecutora;