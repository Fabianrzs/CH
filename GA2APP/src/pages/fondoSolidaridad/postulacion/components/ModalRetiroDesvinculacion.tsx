import React, { useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    MenuItem,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { Controller } from 'react-hook-form';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { TIPO_MODALIDAD, TIPO_SOLICITUD, TIPO_SUBMODALIDAD } from '../../constants/fondoSolidaridad.constants';

export const ModalRetiroDesvinculacion = (props: any) => {
    const {
        cliente,
        handleClose,
        open,
        validacionRetiro,
        mostrarCampoDisfrutePension,
        mostrarCampoInvalidezoMuerte,
        setMostrarCampoDisfrutePension,
        setMostrarCampoInvalidezoMuerte,
        formularioRetiro,
        tipoSolicitud,
        tipoModalidad,
        tipoSubModalidad,
    } = props;

    const [deshabilitarFechasFuturas,setDeshabilitarFechasFuturas] = useState(false);
    const [deshabilitarBotonAceptar,setDeshabilitarBotonAceptar] = useState(false);

    useEffect(() => {
        fechaMaxima();
        fechaMinima();
    },[tipoSolicitud,tipoModalidad])

    const fechaMaximaRetiroFuerza = () => {
        let fechaMaxima;
        if(tipoSolicitud === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
           tipoModalidad === TIPO_MODALIDAD.MECANISMOS_ESPECIALES){
            fechaMaxima = new Date ('Jul 20 2005');
            setDeshabilitarFechasFuturas(true);
        }
        return fechaMaxima;
    }

    const fechaMaxima = () => {
        let fechaMaxima;
        if(tipoSolicitud === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
           tipoModalidad === TIPO_MODALIDAD.MECANISMOS_ESPECIALES){
            fechaMaxima = new Date ('Jul 20 2005');
            setDeshabilitarFechasFuturas(true);
          //  setMostrarCampoDisfrutePension(true);
        }
        return fechaMaxima;
    }

    const fechaMinima = () => {
        let fechaMinima;
        if(tipoSolicitud === TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD &&
            (tipoSubModalidad === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA ||
             tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA  )){
            fechaMinima = new Date ('Jul 21 2005');
            setDeshabilitarFechasFuturas(false);
        }
        return fechaMinima;
    }

    const validarFechaPrimerAporte = (fechaSeleccionada: Date) => {
        const fechaPrimerAporte = new Date (cliente?.fechaPrimeraCuota);
        // si fecha primer aporte es mayor a fecha pensional
        if(fechaPrimerAporte && fechaSeleccionada &&
           fechaPrimerAporte > fechaSeleccionada){
           toast(`No se puede continuar. La fecha de primer aporte no puede ser mayor a la fecha pensional` , { type: toast.TYPE.ERROR });
           setDeshabilitarBotonAceptar(true);
        }else{
           setDeshabilitarBotonAceptar(false);
        }
    }

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogContent style={{ width: '500px' }}>
                    <Grid container>
                        <Grid item md={12}>
                            <Controller
                                name="fechaRetiroFuerza"
                                control={formularioRetiro.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                        label="Fecha Retiro de la Fuerza *"
                                        value={value}
                                        inputProps={{
                                            readOnly: true
                                        }}
                                        minDate={fechaMinima()}
                                        maxDate={fechaMaximaRetiroFuerza()}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        disableFuture={deshabilitarFechasFuturas}
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item md={12}>
                        <Controller
                            name="disfrutePension"
                            control={formularioRetiro.control}
                            rules={{
                                required:
                                    'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={e => {
                                        if(e.target.value === 'SI'){
                                            setMostrarCampoDisfrutePension(true);
                                            setMostrarCampoInvalidezoMuerte(true);
                                        }else{
                                            setMostrarCampoDisfrutePension(false);
                                            setMostrarCampoInvalidezoMuerte(false);
                                        }
                                        onChange(e);
                                    }}
                                    label="¿Cuenta con disfrute de pensión? *"
                                >
                                    <MenuItem key="SI" value="SI">
                                        SI
                                    </MenuItem>
                                    <MenuItem key="NO" value="NO">
                                        NO
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                        {mostrarCampoDisfrutePension && (    
                        <Grid item md={12}>
                            <Controller
                                name="fechaRetiro"
                                control={formularioRetiro.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                        label="Reconocimiento Pensional por Discapacidad *"
                                        value={value}
                                        inputProps={{
                                            readOnly: true
                                        }}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                        )}
                    </Grid>
                    {mostrarCampoInvalidezoMuerte && (    
                    <Grid item md={12}>
                            <Controller
                                name="fechaInvalidezMuerte"
                                control={formularioRetiro.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                        label="Fecha de la Asignación por Invalidez o Muerte *"
                                        value={value}
                                        inputProps={{
                                            readOnly: true
                                        }}
                                        minDate={fechaMinima()}
                                        maxDate={fechaMaximaRetiroFuerza()}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        disableFuture={deshabilitarFechasFuturas}
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                         )}
                    <br />
                    <Grid container>
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                disabled={deshabilitarBotonAceptar}
                                onClick={formularioRetiro.handleSubmit(validacionRetiro)}
                            >
                                ACEPTAR
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                onClick={handleClose}
                                style={{ marginLeft: '10px' }}
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
