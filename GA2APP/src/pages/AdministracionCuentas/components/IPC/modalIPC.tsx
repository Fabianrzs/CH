import React, { useState } from 'react';
import moment from 'moment';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    Typography,
    CircularProgress
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHDatePickerV3 from '../../../../components/ch-datepicker/CHDatepickerV3';
import CHTextFieldV2 from '../../../../components/ch-text-field/CHTextFieldV2';

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

export const ModalIPC = (props: any) => {

    const classes = useStyles();
    const {
        open,
        onClose,
        handleAdd,
        formulario,
        buttonText,

    } = props;
    const [botonAgregarBloqueado, setBotonAgregarBloqueado] = useState(false);
    const [load, setLoad] = useState(false);

    const handleAgregar = async () => {
        setLoad(true);
        setBotonAgregarBloqueado(true);
        await formulario.handleSubmit(handleAdd)();
        setLoad(false);
        setBotonAgregarBloqueado(false);
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
                        Editar I.P.C
                    </Typography>
                </Grid>
                <br />
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                    <Controller
                            name="FechaActualizacion"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',

                                    pattern: {
                                        value: /^([1][0-9][2-9][2-9]|[2][0-9][0-9][0-9])([/.]|[-.])([0-9])([0-9])/g,
                                        message: 'Fecha no válida'
                                    }
                            }}
                            render={({
                                field: { onChange, value, onBlur },
                                fieldState: { error },
                            }) => (
                                <CHDatePickerV3
                                    views={['year', 'month']}
                                    error={!!error}
                                    disableFuture
                                    invalidDateMessage="Fecha no válida"
                                    maxDateMessage="La fecha no debe ser posterior a la Actual"
                                    minDateMessage='La fecha no debe ser anterior a la fecha mínima'
                                    minDate={moment('1910-12').format('YYYY-MM')}
                                    label="Periodo *"
                                    value={value}
                                    onChange={onChange}
                                    onBlur={onBlur}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                        <Controller
                            name="ValorIPC"
                            control={formulario.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                                min: {
                                    value: -100,
                                    message:
                                        'No puede ser menor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'No puede ser mayor a 100',
                                },
                                pattern: {
                                    value: /^([-]?[0]?[100]?[0-9]{1})+([.][0-9]{2})?$/,
                                    message: 'Porcentaje no válido'
                                }
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextFieldV2
                                    label="Valor I.P.C*"
                                    type="number"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={e => {
                                        const valor = e.target.value
                                            .toString()
                                            .replace('%', '')
                                            .replace(',', '.');
                                        if (e.target.value.length <= 6)

                                            onChange(valor);
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                        <Controller
                            name="PorcentajeIPC"
                            control={formulario.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                                min: {
                                    value: 0,
                                    message:
                                        'No puede ser menor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'No puede ser mayor a 100',
                                },
                                pattern: {

                                    value: /^([0]?[100]?[0-9]{1})+([.][0-9]{2})?$/,
                                    message: 'Porcentaje no válido'
                                }
                            }}
                            shouldUnregister={false}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextFieldV2
                                    label="Porcentaje Abono Intereses*"
                                    percentMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={e => {
                                        const valor = e.target.value
                                            .toString()
                                            .replace('%', '')
                                            .replace(',', '.');
                                        if (e.target.value.length <= 6)

                                            onChange(valor);
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <br />
                <Grid item xs={4}>
                    {load && (
                        <div
                            style={{
                                position: 'fixed',
                                top: '50%',
                                left: '50%',
                                transform: 'translate(-50%, -50%)',
                                zIndex: 9999,
                            }}
                        >
                            <CircularProgress />
                        </div>
                    )}
                    <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                        <Button
                            size="small"
                            variant="contained"
                            color="primary"
                            onClick={handleAgregar}
                            disabled={botonAgregarBloqueado}
                        >
                            {buttonText}
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
                    </div>
                </Grid>
            </DialogContent>
        </Dialog>
    );
};