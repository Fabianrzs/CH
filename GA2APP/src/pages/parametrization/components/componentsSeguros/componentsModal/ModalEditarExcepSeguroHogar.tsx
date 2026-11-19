import React from 'react';
import {
    Button,
    Card,
    CardContent,
    Container,
    createStyles,
    Dialog,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
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

export const ModalEditarExcepSeguroHogar = (props: any) => {
    const classes = useStyles();

    const {
        open,
        handleClose,
        handleOnClickEditar,
        formularioSeguroUbicacion,
        parametrizationState,
    } = props;

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md" style={{ width: '500px' }}>
                    <Card>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    ACTUALIZAR SEGURO POR REGIÓN
                                </Typography>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Controller
                                        name="id"
                                        control={
                                            formularioSeguroUbicacion.control
                                        }
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                label="Región"
                                                onChange={onChange}
                                                value={value}
                                                readOnly
                                            >
                                                {parametrizationState.segurosRegion?.map(
                                                    (option: any) => (
                                                        <MenuItem
                                                            key={option.id}
                                                            value={option.id}
                                                        >
                                                            {option.nombre}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    {/* <Controller
                                        name="seguro"
                                        control={
                                            formularioSeguroUbicacion.control
                                        }
                                        rules={{
                                            required: 'Este campo es requerido',
                                            validate: (v) =>
                                                parseFloat(v) < 1 || 'El valor debe ser menor a 1',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                decimalsMaskDos
                                                label="Seguro todo riesgo *"
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={(e) => {
                                                    const raw = e.target.value.replace(',', '.');
                                                    const num = Number(raw);

                                                    if (raw === '' || raw === '0.' || Number.isNaN(num)) {
                                                        onChange(raw);
                                                        return;
                                                    }

                                                    if (num < 1) {
                                                        onChange(raw);
                                                    }
                                                }}
                                            />
                                        )}
                                    /> */}

                                    <Controller
                                        name="seguro"
                                        control={formularioSeguroUbicacion.control}
                                        rules={{
                                            required: 'Este campo es requerido',
                                            validate: (v) => {
                                                const num = Number(String(v).replace(',', '.'));

                                                if (Number.isNaN(num)) return 'Valor inválido';
                                                if (num >= 1) return 'El valor debe ser menor a 1';

                                                return true;
                                            },
                                        }}
                                        render={({ field, fieldState: { error } }) => (
                                            <CHTextField
                                                decimalsMaskDos
                                                label="Seguro todo riesgo *"
                                                InputLabelProps={{ shrink: true }}
                                                {...field}
                                                error={!!error}
                                                helperText={error?.message}
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
                                        onClick={formularioSeguroUbicacion.handleSubmit(
                                            handleOnClickEditar,
                                        )}
                                    >
                                        Actualizar
                                    </Button>
                                    {/* <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={formularioSeguroUbicacion.handleSubmit(
                                            (data: any) => {
                                                console.log('SUBMIT OK', data);
                                                handleOnClickEditar(data);
                                            },
                                            (errors: any) => {
                                                console.log('ERRORES', errors);
                                            }
                                        )}
                                    >
                                        Actualizar
                                    </Button> */}

                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={handleClose}
                                        style={{ marginLeft: '10px' }}
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

export default ModalEditarExcepSeguroHogar;
