import React from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import moment from 'moment';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import { ESTADO_CONVOCATORIA } from '../../constants/fondoSolidaridad.constants';

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
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

export const ModalCierreConvocatoria = (props: any) => {
    const {
        handleClose,
        open,
        cierreConvocatoria,
        datosConvocatoria,
        formulario,
        camposDeshabilitados
    } = props;
    const classes = useStyles();

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
                            CIERRE CONVOCATORIA
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={12}>
                        <Controller
                            name="numeroResolucionCierre"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Número de Resolución de Cierre *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={camposDeshabilitados}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={12}>
                            <Controller
                                name="fechaApertura"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        label="Fecha de Apertura"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        value={moment(value).format('YYYY/MM/DD')}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item md={12}>
                            <Controller
                                name="fechaCierre"
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                control={formulario.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHDatePicker
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        label="Fecha de Cierre"
                                        readOnly
                                        inputProps={{ readOnly: true }}
                                        KeyboardButtonProps={{
                                            style: { display: 'none' }
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={12}>
                        <Controller
                            name="numeroBeneficiario"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    label="Número de Beneficiarios *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={camposDeshabilitados}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={12}>
                        <Controller
                            name="numeroConvocatoria"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Número de Convocatoria"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                        </Grid>
                    </Grid>
                    <Grid container>
                    <Grid item md={6} style={{
                        marginTop: "100px"
                    }}
                    >
                        Valor aprobado por la junta
                    </Grid> 
                    <Grid item md={6}>
                        <Controller
                            name="valorOficial"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                validate: (value) => value === 0 || value === '$0'? 'El valor debe ser mayor a cero' : true
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    label="Valor Oficial"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={camposDeshabilitados}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                         <Controller
                            name="valorSubOficial"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                validate: (value) => value === 0 || value === '$0'? 'El valor debe ser mayor a cero' : true
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    label="Valor Sub Oficial - Nivel Ejecutivo"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={camposDeshabilitados}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                          <Controller
                            name="valorSoldadoProfesionalAgente"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                validate: (value) => value === 0 || value === '$0'? 'El valor debe ser mayor a cero' : true
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    label="Valor Soldado Profesional - Agente"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={camposDeshabilitados}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                 </Grid> 
                 <Grid container>
                        <Grid item md={12}>
                        <Controller
                            name="acuerdo"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Acuerdo *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={camposDeshabilitados}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    multiline
                                    rows={2}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={onChange}

                                />
                            )}
                        />
                        </Grid>
                    </Grid>
                 <Grid container>
                    <Grid item md={12}>
                        <Controller
                            name="fechaPublicacion"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { onChange, value },
                                    fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Publicación Diario Oficial"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: true }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    minDate= {new Date()}
                                    readOnly={camposDeshabilitados}
                                    autoOk
                                />
                            )}
                        />
                        </Grid>
                    </Grid>
                    <Grid container>
                    <Grid item md={12}>
                        <Controller
                            name="fechaNotificacion"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { onChange, value },
                                    fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha de Notificación"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: true }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    minDate= {new Date()}
                                    readOnly={camposDeshabilitados}
                                    autoOk
                                />
                            )}
                        />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container>
                     <Grid className={classes.column} item xs={12} md={8} />
                     <Grid className={classes.column} item xs={12} md={2}>
                            {datosConvocatoria?.nombreEstado !== ESTADO_CONVOCATORIA.CERRADA &&
                             datosConvocatoria?.nombreEstado !== ESTADO_CONVOCATORIA.ANULADO && (
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    onClick={formulario.handleSubmit(cierreConvocatoria)}
                                >
                                 {datosConvocatoria?.nombreEstado === ESTADO_CONVOCATORIA.CERRADA? 'ACTUALIZAR' : 'ACEPTAR' } 
                                </Button>
                            )}
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={2}>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                onClick={handleClose}
                            >
                                CANCELAR
                            </Button>
                        </Grid>
                      </Grid>
                    <br />
                </DialogContent>
            </Dialog>
        </div>
    );
};
