import React, { useDebugValue, useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    MenuItem,
    FormControlLabel,
    Checkbox,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { toast } from 'react-toastify';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import {
    TipoEmbargoDto,
    TipoRetencionDto,
} from '../../../types/embargo.model.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import UtilService from '../../../utils/util.service';


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
    }),
);
export const ModalAgregarConceptos = (props: any) => {
    const {
        handleClose,
        open,
        formularioConceptos,
        agregarConcepto,
        tiposEmbargos,
        porcentaje,
        variacionPorcentaje,
        valor,
        handleOnChangeMostrarPorcentaje,
        mostrarPorcentaje,
        tiposRetencion,
        modoActualizar,
        cuentas,
        saldos,
        handleCloseModal,
        conceptos,
        saldoConcepto
    } = props;
    const classes = useStyles();
    const [mostrarAlerta, setMostrarAlerta] = useState(false);
    const [mostrarCampoIngresos, setMostrarCampoIngresos] = useState(false);
    const resultadoSaldo = saldos



    function esVariacionPorcentaje(tipoEmbargoId: any, variacionPorcentaje: any) {
        return tipoEmbargoId === variacionPorcentaje;
    }



    const idingresos = 1
    const idSaldos = 2

    function sobreIngreso(tipoRetencionId: any, idingresos: any) {
        return tipoRetencionId === idingresos
    }
    function sobreSaldo(tipoRetencionId: any, idSaldos: any) {
        return tipoRetencionId === idSaldos
    }

    const handleTipoEmbargoChange = (e: any) => {
        if (!formularioConceptos.getValues('fechaInicio') || !formularioConceptos.getValues('fechaFin') || !formularioConceptos.getValues('valor')) {
            toast.warn('Por favor ingrese la o las fechas y luego el valor, si cambia alguna de las fechas el valor se restablecera');
        }
    };

    const VALOR_ID = '95ca1858-b89d-4631-9893-22932c925184';
    const PORCENTAJE_ID = 'ae7d141a-fd88-4f57-a199-4db5b5301946';
    const idTipoEmbargo = formularioConceptos.watch('tipoEmbargoId');
    const idTipoRetencion = formularioConceptos.watch('tipoRetencionId');
    const retencionIngresos = 'aae6c046-06ad-40c3-8fd5-9776eddc12e8';
    const retencionSaldos = 'e41edf0d-b5c9-4b0c-9b40-eaa1cef15552';


    useEffect(() => {
        formularioConceptos.unregister('valor');


        formularioConceptos.setValue('valor', null);
        formularioConceptos.clearErrors('valor');


        if (idTipoEmbargo === PORCENTAJE_ID) {
            formularioConceptos.register('valor', {
                required: 'Este campo es requerido',
                min: { value: 1, message: 'El valor debe ser mayor a cero' },
                max: { value: 100, message: 'El valor no debe ser mayor a 100' },
            });
        } else if (idTipoEmbargo === VALOR_ID) {
            formularioConceptos.register('valor', {
                required: 'Este campo es requerido',
                min: { value: 1, message: 'El valor debe ser mayor a cero' },
            });
        }


        // formularioConceptos.trigger('valor');
    }, [idTipoEmbargo, formularioConceptos]);





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
                            {modoActualizar
                                ? 'ACTUALIZAR EMBARGO A CONCEPTO'
                                : 'AGREGAR EMBARGO A CONCEPTO'}
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container spacing={2}>
                        <Grid item md={6}>
                            <Controller
                                name="tipoEmbargoId"
                                control={formularioConceptos.control}
                                rules={{
                                    validate: () => {
                                        const conceptoId = formularioConceptos.getValues('conceptoId');
                                        const tipoEmbargoId = formularioConceptos.getValues('tipoEmbargoId');
                                        if (conceptoId === 1 && tipoEmbargoId !== porcentaje) {
                                            toast.warning('El EMBARGO MASIVO ES POR PORCENTAJE');
                                            return '';
                                        }
                                        return undefined;
                                    },
                                    required: {
                                        value: true,
                                        message: 'Este Campo es requerido'
                                    }
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        label="Tipo Embargo *"
                                        value={value}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        onChange={e => {
                                            handleTipoEmbargoChange(e);

                                            formularioConceptos.setValue(
                                                'valor',
                                                null,
                                            );
                                            if (e.target.value === porcentaje) {
                                                handleOnChangeMostrarPorcentaje(
                                                    true,
                                                );
                                            } else if (
                                                e.target.value ===
                                                variacionPorcentaje
                                            ) {
                                                handleOnChangeMostrarPorcentaje(
                                                    true,
                                                );
                                            } else if (
                                                e.target.value === valor
                                            ) {
                                                handleOnChangeMostrarPorcentaje(
                                                    false,
                                                );
                                            }
                                            setMostrarAlerta(false);
                                            onChange(e);
                                        }}
                                    >
                                        {tiposEmbargos?.map(
                                            (option: TipoEmbargoDto) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                        <Grid item md={6}>
                            <Controller
                                name="fechaInicio"
                                control={formularioConceptos.control}

                                rules={{
                                    validate: () => {
                                        const tipoRetencionId = formularioConceptos.getValues('tipoRetencionId');


                                        if (tipoRetencionId === retencionIngresos && !formularioConceptos.getValues('fechaInicio')) {
                                            toast.warning('El campo "Fecha inicio" es obligatorio para este tipo de retención.');
                                            return 'Este campo es requerido';
                                        }

                                        return undefined;
                                    },
                                }}

                                // render={({
                                //     field: { onChange, value },
                                //     fieldState: { error },
                                // }) => (
                                //     <CHDatePicker
                                //         onChange={e => {
                                //             formularioConceptos.setValue(
                                //                 'valor',
                                //                 null,
                                //             );
                                //             onChange(e);
                                //         }}
                                //         InputLabelProps={{ shrink: true }}
                                //         label="Fecha de inicio embargo*"
                                //         value={value}
                                //         minDate={moment(new Date())
                                //             .subtract(10, 'y')
                                //             .format('YYYY-MM-DD')}
                                //         error={!!error}
                                //         helperText={
                                //             error ? error.message : null
                                //         }
                                //         autoOk

                                //     />
                                // )}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => {

                                    const tipoRetencionId = formularioConceptos.getValues('tipoRetencionId');
                                    const isRequired = tipoRetencionId === retencionIngresos;

                                    return (
                                        <CHDatePicker
                                            onChange={e => {
                                                formularioConceptos.setValue('valor', null);
                                                onChange(e);
                                            }}
                                            InputLabelProps={{ shrink: true }}
                                            label={`Fecha de inicio embargo${isRequired ? '*' : ''}`}
                                            value={value}
                                            minDate={moment(new Date())
                                                .subtract(10, 'y')
                                                .format('YYYY-MM-DD')}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            autoOk
                                        />
                                    );
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item md={6}>
                            <Controller
                                name="fechaFin"
                                control={formularioConceptos.control}
                                defaultValue={formularioConceptos.getValues('fechaFin') || ''}
                                rules={{
                                    validate: () => {
                                        const tipoRetencionId = formularioConceptos.getValues('tipoRetencionId');


                                        if (tipoRetencionId === retencionSaldos && formularioConceptos.getValues('fechaFin')) {
                                            return undefined;
                                        }

                                        if (tipoRetencionId === retencionIngresos && formularioConceptos.getValues('fechaFin')) {
                                            return undefined;
                                        }
                                        return undefined;
                                    },
                                }}
                                render={({ field: { onChange, value }, fieldState: { error }, }) => (
                                    <CHDatePicker
                                        style={{
                                            marginLeft: '10px',
                                        }}
                                        onChange={e => {
                                            formularioConceptos.setValue(
                                                'valor',
                                                null,
                                            );
                                            onChange(e);
                                        }}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        label="Fecha fin embargo"
                                        minDate={moment(new Date())
                                            .subtract(10, 'y')
                                            .format('YYYY-MM-DD')}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        autoOk
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item md={6}>
                            {mostrarPorcentaje ? (
                                <Controller
                                    name="valor"
                                    control={formularioConceptos.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                        min: {
                                            value: 1,
                                            message: 'El valor debe ser mayor a cero',
                                        },
                                        max: {
                                            value: 50,
                                            message: 'El valor no debe ser mayor a 50%',
                                        },
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <>
                                            <CHTextField
                                                isReadOnly={false}
                                                label="Valor embargo *"
                                                style={{ marginLeft: '10px' }}
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{ style: { textAlign: 'right' } }}
                                                value={value}
                                                onChange={(e) => {
                                                    const valor = e.target.value.toString().replace('%', '');
                                                    onChange(valor);
                                                }}
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                percentMask
                                            />

                                        </>
                                    )}
                                />
                            ) : (
                                <Controller
                                    name="valor"
                                    control={formularioConceptos.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                        min: {
                                            value: 1,
                                            message: 'El valor debe ser mayor a cero',
                                        },
                                        // validate: value => {
                                        //     if (Number(value) > saldoConcepto / 2) {
                                        //         return 'No se puede embargar más del 50% del saldo';
                                        //     }
                                        //     return true;
                                        // }

                                    }}

                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <>
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                isReadOnly={false}
                                                label="Valor embargo *"
                                                style={{ marginLeft: '10px' }}
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: { textAlign: 'right' },
                                                }}
                                                value={value}
                                                onChange={(e) => {
                                                    const valor = UtilService.cleanCurrencyValue(e.target.value);

                                                    onChange(valor);
                                                    if (valor >= resultadoSaldo) {
                                                        setMostrarAlerta(true);
                                                    } else {
                                                        setMostrarAlerta(false);
                                                    }
                                                }}
                                                currencyMask
                                            />
                                            {mostrarAlerta && (
                                                <p style={{ color: 'red' }}>
                                                    Esta superando el valor del saldo a embargar.
                                                </p>
                                            )}
                                        </>
                                    )}
                                />
                            )}
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item md={6}>
                            <Controller
                                name="tipoRetencionId"
                                rules={{
                                    validate: () => {
                                        const tipoRetencionId = formularioConceptos.getValues('tipoRetencionId');
                                        const fechaInicioValue = formularioConceptos.getValues('fechaInicio');
                                        const fechaFinValue = formularioConceptos.getValues('fechaFin');
                                        const conceptoId = formularioConceptos.getValues('conceptoId');
                                        const isSobreSaldo = sobreSaldo(tipoRetencionId, idSaldos);
                                        if (isSobreSaldo && !fechaInicioValue) {
                                            toast.warning('El campo fecha de inicio es requerido al escoger el tipo de retención SOBRE SALDOS');
                                            return '';
                                        }

                                        const isSobreIngreso = sobreIngreso(tipoRetencionId, idingresos);
                                        if (isSobreIngreso && !fechaInicioValue && !fechaFinValue) {
                                            toast.warning('Los campos fecha de inicio y fin son requeridos al escoger el tipo de retención SOBRE INGRESOS');
                                            return '';
                                        }
                                        return undefined;

                                    },
                                    required: 'Este campo es requerido'
                                }}
                                control={formularioConceptos.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        label="Tipo retención *"
                                        value={value}
                                        onChange={(e) => {
                                            onChange(e);
                                            const selectedOption = tiposRetencion.find((option: any) => option.id === e.target.value);
                                            setMostrarCampoIngresos(selectedOption?.descripcion === 'SOBRE INGRESOS');
                                        }}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    >
                                        {tiposRetencion?.map(
                                            (option: TipoRetencionDto) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>

                        <Grid item xs={3} md={3}>
                            <br />
                            <Controller
                                name="indicadorCesantias"
                                control={formularioConceptos.control}
                                render={({ field: { onChange, value } }) => (
                                    <FormControlLabel
                                        label="Indicador de cesantías?"
                                        style={{
                                            fontFamily: 'OpenSans-Regular',
                                            color: 'rgba(0, 0, 0, 0.54)',
                                            fontSize: '2rem',
                                        }}
                                        control={
                                            <Checkbox
                                                style={{ marginLeft: '10px' }}
                                                onChange={onChange}
                                                checked={value}
                                                color="primary"
                                            />
                                        }
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={formularioConceptos.handleSubmit(
                                    agregarConcepto,
                                )}
                            >
                                {modoActualizar ? 'ACTUALIZAR' : 'AGREGAR'}
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
