import React, { useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    Chip,
    MenuItem,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import moment from 'moment';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CatalogApi from '../../../../services/api-catalog.service';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';
import CHDatePickerV3 from '../../../../components/ch-datepicker/CHDatepickerV3';

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

        buttonPagos: {
            width: '30%',
            backgroundColor: '#f9fafc',
            marginLeft: '62%',
        },
    }),
);

const api = CatalogApi()

export const ModalGenerarExtracto = (props: any) => {
    const {
        open,
        handleClose,
        handleOnClick,
        formularioConsulta,
        nombreArchivo,
        
    } = props;

    const classes = useStyles();
    const [formatoAplicar, setFormatoAplicar] = useState([])
    const [loading, setLoading] = useState(false);


    useEffect(() => {
        api.ObtenerFormatoAAplicar()
        .then((result) => {
            setFormatoAplicar(result.data)
        })
    }, [])

    if (loading) {
		return <CHLoadingProcess />;
	}


    const validateDate = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (date > moment(new Date).add(1, 'months')) {
            message = "La fecha no debe ser mayor a 30 días"
        } else if (date < moment(new Date).add(-1, 'months')) {
            message = "La fecha no debe ser menor a 30 días"
        }

        return message !== '' ? message : true;
    }

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogContent style={{ width: '600px' }}>
                    <Grid item>
                        <br />
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            EXTRACTO MENSUAL DE CRÉDITO
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
                                Fecha de Aplicación
                            </Typography>
                        </Grid>

                    </Grid>
                 
                        <Grid  item xs={12} md={3}>
                            <Controller
                                name="fechaReportes"
                                control={formularioConsulta.control}
                                rules={{
                                    required: 'Es requirido',
                                    pattern: {
                                        value: /^\d{4}([/.]|[-.])(0?[1-9]|1[1-2])$/,
                                        message: 'Fecha no válida',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePickerV3
                                        views={['year', 'month']}
                                        error={!!error}
                                        label="Fecha *"
                                        invalidDateMessage="Fecha no válida"
                                        minDateMessage="La fecha no debe ser anterior a la fecha mínima"
                                        maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                                        minDate={moment('1910-12').format(
                                            'YYYY-MM',
                                        )}
                                        disableFuture
                                        maxDate={new Date()}
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                  

             
                    <br />
                    <Grid container>
                        <Grid item xs={12}>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                style={{ marginLeft: '60%' }}
                                onClick={formularioConsulta.handleSubmit(
                                    handleOnClick,
                                )}
                            >
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

