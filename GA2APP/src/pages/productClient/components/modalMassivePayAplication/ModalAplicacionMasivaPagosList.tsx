import React from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { startOfMonth, subMonths } from 'date-fns';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { EXECUTINGUNITDTO } from '../../../../types/catalog.type';
import { procesoListarAplicacionMasivaPago } from '../../../../types/producto.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        button: {
            width: '100%',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
    }),
);

export const ModalAplicacionMasivaPagosList = (props: any) => {
    const classes = useStyles();
    const {
        open,
        handleClose,
        handleOnClick,
        unidadesEjecutoras,
        formState
    } = props;

    const formList = useForm<procesoListarAplicacionMasivaPago>();

    if(open) {
        const currentValues = formList.getValues();
        formList.setValue("fechaAplicacion", formState?.fechaAplicacion ?? currentValues.fechaAplicacion ?? moment(new Date()).endOf('month').startOf('day').toDate());
        formList.setValue("fechaProceso", formState?.fechaProceso ?? new Date());
        formList.setValue("tipoCredito", formState?.tipoCredito ?? currentValues.tipoCredito);
        formList.setValue("unidadEjecutora", formState?.unidadEjecutora ?? currentValues.unidadEjecutora);
    }

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md">
                <DialogContent style={{ width: '600px' }}>
                    <Grid item>
                        <br />
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            PROCESO APLICACIÓN MASIVA DE PAGOS   
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Fecha De Aplicación
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="fechaAplicacion"
                                control={formList.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={(date, _) => {
                                            const lastDay = moment(date).endOf('month').startOf('day').toDate();
                                            onChange(lastDay);
                                        }}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        value={value}
                                        autoOk
                                        views={['year', 'month']}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Fecha Pago*
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="fechaProceso"
                                control={formList.control}
                                rules={{
                                    required: 'Este campo es requerido'
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        value={value}
                                        autoOk
                                        // disabled
                                        // minDate={startOfMonth(new Date())}
                                        // minDate={startOfMonth(subMonths(new Date(), 7))} SOLICITUD DE EDGAR PARA PRUEBAS CARTERA 03-08-2026
                                        // maxDate={new Date()} SOLICITUD DE EDGAR PARA PRUEBAS CARTERA 03-08-2026
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Unidad Ejecutora
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="unidadEjecutora"
                                control={formList.control}
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
                                    <CHSelect
                                        defaultValue=""
                                        label=""
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    >
                                        {unidadesEjecutoras?.filter((unidad: EXECUTINGUNITDTO) => unidad.idUnidadPrincipal == null)?.map((option: EXECUTINGUNITDTO) => {
                                            //    {setListadoALternativos?.map((option: EXECUTINGUNITDTO) => {
                                            let menuItemContent;
                                            if (option.id === 6) {
                                                menuItemContent = `${option.nombre}. (DISAN)`;
                                            } else if (option.id === 12 || option.id === 17 || option.id === 20) {
                                                menuItemContent = `${option.nombre}. (${option.sigla})`;
                                            } else {
                                                menuItemContent = option.nombre;
                                            }

                                            return (
                                                <MenuItem key={option.id} value={option.id}>
                                                    {menuItemContent}
                                                </MenuItem>
                                            );
                                        })}

                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Tipo Crédito
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="tipoCredito"
                                control={formList.control}
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
                                    <CHSelect
                                        label=""
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }>
                                        <MenuItem value={1}>LEASING HABITACIONAL</MenuItem>
                                        <MenuItem value={2}>HIPOTECARIO</MenuItem>
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item xs={12}>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                style={{ marginLeft: '60%' }}
                                onClick={formList.handleSubmit(handleOnClick)}>
                                Aceptar
                            </Button>
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
                    <br />
                </DialogContent>
            </Dialog>
        </div>
    );
};
