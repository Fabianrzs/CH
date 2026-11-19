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
import CHSelectV2 from '../../../../components/ch-select/CHSelectV2';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import CatalogApi from '../../../../services/api-catalog.service';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';

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

export const ModalAplicacionMasivaPagos = (props: any) => {
    const {
        open,
        handleClose,
        handleOnClick,
        formularioAplicacionMasiva,
        nombreArchivo,
        handleFileChosen,
        loading
    } = props;

    const classes = useStyles();
    const [formatoAplicar, setFormatoAplicar] = useState([])


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
                                Formato A Aplicar
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="formatoAplicar"
                                control={formularioAplicacionMasiva.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelectV2
                                        defaultValue=""
                                        label=""
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        
                                    >
                                     {formatoAplicar.map((member: any, index) => (
                                        <MenuItem
                                            key={`member-${index}`}
                                            value={member.id}
                                        >
                                            {member.descripcion}
                                    
                                        </MenuItem>
                                    ))}
                                    </CHSelectV2>
                                )}
                            />
                        </Grid>
                    </Grid>


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
                                control={formularioAplicacionMasiva.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        label=""
                                        inputProps={{ readOnly: true }}
                                        onChange={(date, value) => {
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
                                control={formularioAplicacionMasiva.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                    validate: validateDate
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
                                        disabled
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>



                    <Grid container>
                        <Grid item md={5}>
                            <br />
                            <Typography
                                style={{
                                    width: '350px',
                                }}
                            >
                                Valor Total
                            </Typography>
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="valor"
                                control={formularioAplicacionMasiva.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                    maxLength: {
                                        value: 18,
                                        message:
                                            'La longitud máxima del campo es de 12 caracteres',
                                    },
                                    min: {
                                        value: 1,
                                        message:
                                            'El valor debe ser mayor a cero',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        inputProps={{ style: { textAlign: 'right' } }}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        currencyMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>



                    <Grid className={classes.buttonPagos} container> 
                            <Controller
                                name="aplicanPagos"
                                control={formularioAplicacionMasiva.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="¿Aplican Pagos?*"
                                        onChange={onChange}
                                        checked={value}

                                    />
                                )}
                            />
                    </Grid>


                    <Grid container justifyContent="center" alignItems="center">
                        <Grid item xs={5}>
                            <Controller
                                name="archivo"
                                control={formularioAplicacionMasiva.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange },
                                    // fieldState: { error },
                                }) => (
                                    <Button
                                        variant="contained"
                                        className={classes.button}
                                        color="primary"
                                        component="label"
                                        onChange={(e: any) => {
                                            handleFileChosen(
                                                e.target.files != null
                                                    ? e.target.files[0]
                                                    : '',
                                            );
                                            onChange(e);
                                        }}
                                    >
                                        Seleccionar Archivo
                                        <input
                                            type="file"
                                            accept=".csv, .txt, .xlsx, .xlx"
                                            hidden
                                        />
                                    </Button>
                                )}
                            />
                        </Grid>
                        <Grid item xs={7}>
                            {nombreArchivo ? (
                                <Chip
                                    style={{
                                        marginTop: '20px',
                                        marginLeft: '10px',
                                    }}
                                    label={nombreArchivo}
                                />
                            ) : (
                                ''
                            )}
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
                                onClick={formularioAplicacionMasiva.handleSubmit(
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
