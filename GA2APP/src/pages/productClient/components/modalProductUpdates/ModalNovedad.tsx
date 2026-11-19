import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import {
    Button,
    createStyles,
    Dialog,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import moment from 'moment';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import ApiproductoClients from '../../../../services/api-producto.service';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
            marginTop: '1rem'
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        input: {
            boxSizing: 'border-box',
            height: '2em'
        }
    }),
);

type formValues = {
    tipoNovedad: number | null,
    tipoCausal: number | null,
    fechaNovedad: Date | null,
    fechaFinalizacion: Date | null,
    estado: number | null,
    seguroVida: number | null,
    seguroHogar: number | null,
    interesCorriente: any | null,
    interesMora: number | null,
    interesRemanente: number | null,
    capitalRemanente: number | null,
    capital: number | null,
    totalDeuda: number | null,
    valorPagado: number | null,
    cuotaCredito: number | null,
    mesAfectar: Date | null,
    nuevaTasa: number | null,
    saldoCapital: number | null,
    interesesRemanenteCorte: number | null,
    saldoTotalCorte: number | null,
    capitalRemanenteCorte: number | null,
    calificacion: string | null,
    motivoCalificacion: string | null,
    numeroCuotasAmpliar: number | null,
    valorPagar: number | null,
    porcentajeSeguroVida: number | null,
    porcentajeSeguroHogar: number | null,
    porcentajeTasaNominalMensual: number | null,
    valorIncrementar: number | null,
};

export const ModalNovedad = (props: any) => {
    const classes = useStyles();
    const cartera = ApiproductoClients();
    const {
        open,
        handleSubmit,
        handleClose,
        producto,
        tipoNovedades,
        tiposCausales,
        estadosNovedades,
        readOnly,
        novedad,
        idCredito,
        estadoCredito,
        idEstadoCredito,
        productoData
    } = props;

    const form = useForm<formValues>({
        defaultValues: {
            fechaNovedad: new Date(),
            fechaFinalizacion: new Date(),
            mesAfectar: null,
            nuevaTasa: 0,
            calificacion: '',
            motivoCalificacion: ''
        },
        mode: 'onChange'
    });

    const [mostrarCausal, setMostrarCausal] = useState<boolean>(false);
    const [mostrarCuotas, setMostrarCuotas] = useState(false);
    const [mostrarInformacionOtros, setMostrarInformacionOtros] = useState<boolean>(false);
    const [mostrarRestruc, setMostrarRestruc] = useState(false);

    useEffect(() => {
        form.reset({
            fechaNovedad: new Date(),
            fechaFinalizacion: new Date(),
        });

        if (novedad) {
            setValues(novedad);
        } else {
            obtenerSaldoObligacion()
                .then((saldo: any) => {
                    setValues(saldo);
                });
        }

    }, [novedad])

    useEffect(() => {
        obtenerTiposCausalesInmueble();
    }, [idCredito, estadoCredito, idEstadoCredito]);


    const obtenerSaldoObligacion = () =>
        cartera
            .getSaldoObligacion(producto.idCredito)
            .then(async (response: any) => response?.data)
            .catch(() => toast("Error del sistema", { type: toast.TYPE.ERROR }));

    const setValues = (novedad: any) => {

        const currentDate = new Date();
        const dateNovedad = moment(currentDate).startOf('day');
        const dateFinalizacion = moment(currentDate).startOf('day').endOf('month');
        form.setValue('tipoNovedad', novedad?.idTipoNovedad);
        form.setValue('tipoCausal', novedad?.idTipoCausal);

        form.setValue('numeroCuotasAmpliar', novedad?.cuotaCredito);

        form.setValue('porcentajeSeguroVida', novedad?.porcentajeSeguroVida);
        form.setValue('porcentajeSeguroHogar', novedad?.porcentajeSeguroHogar);
        form.setValue('porcentajeTasaNominalMensual', novedad?.porcentaeTasaNominalMensual);

        form.setValue('fechaNovedad', novedad?.fechaNovedad ?? dateNovedad.toDate());
        form.setValue('fechaFinalizacion', novedad?.fechaFinalizacion ?? dateFinalizacion.toDate());
        form.setValue('estado', novedad?.idEstadoNovedad);
        form.setValue('seguroVida', novedad?.seguroVida);
        form.setValue('seguroHogar', novedad?.seguroHogar);
        form.setValue('interesCorriente', novedad?.interesCorriente);
        form.setValue('interesMora', novedad?.interesMora);
        form.setValue('interesRemanente', interesRemanente || productoData.interesesRemanenteCorte.toFixed(0) || 0);
        form.setValue('capitalRemanente', novedad?.capitalRemanente);
        form.setValue('capital', novedad?.capital);
        form.setValue('totalDeuda', novedad?.totalDeuda);
        form.setValue('valorPagado', novedad?.valorPagado);
        form.setValue('cuotaCredito', novedad?.cuotaCredito);

        const tipoNovedad = tipoNovedades.find((x: any) => x?.id === novedad?.idTipoNovedad)?.codigo;
        setMostrarCausal(tipoNovedad === "ACELERACION" ||
            tipoNovedad === "RESTITUCIÓN INMUEBLE" || tipoNovedad === "REDIFERIDO");

    };

    const [capital, setCapital] = useState('');
    const [seguroVida, setSeguroVida] = useState('');
    const [seguroHogar, setSeguroHogar] = useState('');
    const [interesCorriente, setInteresCorriente] = useState('');
    const [interesMora, setInteresMora] = useState('');
    const [capitalRemanente, setCapitalRemanente] = useState('');
    const [interesRemanente, setInteresRemanente] = useState('');
    const [totalDeuda, setTotalDeuda] = useState('');

    const obtenerTiposCausalesInmueble = () => {
        if (estadoCredito === "RESTITUIDO") {
            cartera
                .obtenerRestitucionInmuebleCartera(idCredito)
                .then(async (response: any) => {
                    if (response && response.isSuccess) {
                        const {
                            capital,
                            seguroVida,
                            seguroHogar,
                            interesCorriente,
                            interesMora,
                            capitalRemanente,
                            interesRemanente,
                            totalDeuda,
                        } = response.data;

                        setCapital(capital);
                        setSeguroVida(seguroVida);
                        setSeguroHogar(seguroHogar);
                        setInteresCorriente(interesCorriente);
                        setInteresMora(interesMora);
                        setCapitalRemanente(capitalRemanente);
                        setInteresRemanente(interesRemanente);
                        setTotalDeuda(totalDeuda);

                        form.setValue("capital", capital);
                        form.setValue("seguroVida", seguroVida);
                        form.setValue("seguroHogar", seguroHogar);
                        form.setValue("interesCorriente", interesCorriente);
                        form.setValue("interesMora", interesMora);
                        form.setValue("capitalRemanente", capitalRemanente);
                        form.setValue("interesRemanente", interesRemanente);
                        form.setValue("totalDeuda", totalDeuda);
                    }
                })
                .catch(() => {
                    toast("Error del sistema", { type: toast.TYPE.ERROR });
                });

        }
        // else {
        //     toast("El crédito no tiene el estado RESTITUIDO, no se realizará la solicitud", { type: toast.TYPE.ERROR });
        // }
    };

    const watchTipoCausal = form.watch("tipoCausal");
    const watchTipoNovedadId = form.watch("tipoNovedad");

    const tipoNovedadActual = tipoNovedades.find((x: any) => x?.id === watchTipoNovedadId)?.codigo;

    return (
        <div>
            <Dialog
                fullScreen
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <CHPaper title="CREAR NOVEDAD">
                    <Grid container style={{ width: '80%', display: 'flex', margin: 'auto', marginTop: '90px' }} >
                        <Grid item xs={12} md={6} className={classes.column}>
                            <Controller
                                name="tipoNovedad"
                                control={form.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Tipo de Novedad *"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        readOnly={readOnly}
                                        onChange={(e) => {
                                            const id = e.target.value;
                                            const tipoNovedad = tipoNovedades.find((x: any) => x?.id === id)?.codigo;

                                            if (tipoNovedad === "REDIFERIDO" || tipoNovedad === "RESTITUCIÓN INMUEBLE") {
                                                setMostrarCuotas(true);
                                            } else {
                                                setMostrarCuotas(false);
                                            }
                                            setMostrarCausal(tipoNovedad === "ACELERACION" || tipoNovedad === "REESTRUCTURACION" || tipoNovedad === "OTROS");
                                            setMostrarRestruc(tipoNovedad === "REESTRUCTURACION")
                                            setMostrarInformacionOtros(tipoNovedad === "OTROS");
                                            onChange(e);
                                        }}
                                    >
                                        <MenuItem value="">
                                            Seleccione un tipo de novedad
                                        </MenuItem>
                                        {
                                            tipoNovedades?.map(
                                                (option: any) => (
                                                    <MenuItem key={option.id}
                                                        value={option.id}
                                                        disabled={
                                                            option.codigo !== 'ACELERACION' &&
                                                            option.codigo !== 'RESTITUCIÓN INMUEBLE' &&
                                                            option.codigo !== 'REDIFERIDO' &&
                                                            option.codigo !== 'REESTRUCTURACION' &&
                                                            option.codigo !== 'OTROS'
                                                        }
                                                    >
                                                        {option.nombre}
                                                    </MenuItem>
                                                )
                                            )
                                        }
                                    </CHSelect>
                                )}
                            />
                        </Grid>

                        {
                            mostrarCausal &&

                            <Grid item xs={12} md={6} className={classes.column} >
                                <Controller
                                    name="tipoCausal"
                                    control={form.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Causal *"
                                            value={value || ""}
                                            readOnly={readOnly}
                                            helperText={error ? error.message : null}
                                            onChange={(value) => {
                                                onChange(value)
                                            }}>
                                            <MenuItem value="">
                                                Seleccione la causa de aceleración
                                            </MenuItem>
                                            {
                                                tiposCausales?.map((option: any) => {
                                                    const id = form.getValues('tipoNovedad');
                                                    const tipoNovedad = tipoNovedades.find((x: any) => x?.id === id)?.codigo;

                                                    if (tipoNovedad === 'ACELERACION' &&
                                                        (option.nombre === 'FALLECIDO' ||
                                                            option.nombre === 'INCAPACIDAD' ||
                                                            option.nombre === 'DESVINCULADO DE LA FUERZA')) {
                                                        return (
                                                            <MenuItem key={option.id} value={option.id}>
                                                                {option.nombre}
                                                            </MenuItem>
                                                        )
                                                    }

                                                    if (tipoNovedad === 'REESTRUCTURACION' &&
                                                        (option.nombre === 'AMPLIACION DE PLAZO' ||
                                                            option.nombre === 'AUMENTO VALOR DE CUOTA')) {
                                                        return (
                                                            <MenuItem key={option.id} value={option.id}>
                                                                {option.nombre}
                                                            </MenuItem>
                                                        )
                                                    }
                                                    
                                                    if (tipoNovedad === 'OTROS' &&
                                                        (option.nombre === 'NUEVA TASA CRÉDITO' ||
                                                            option.nombre === 'NUEVA TASA SEGURO VIDA' ||
                                                            option.nombre === 'NUEVA TASA SEGURO HOGAR' ||
                                                            option.nombre === 'CALFICIACION')) {
                                                        return (
                                                            <MenuItem key={option.id} value={option.id}>
                                                                {option.descripcion}
                                                            </MenuItem>
                                                        )
                                                    }
                                                    return null;
                                                })
                                            }
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        }

                        {tipoNovedadActual === 'REESTRUCTURACION' && [10, 11].includes(Number(watchTipoCausal)) && (
                            <Grid item xs={12} md={6} className={classes.column}>
                                <Controller
                                    name="valorIncrementar"
                                    control={form.control}
                                    rules={{ required: 'El valor es obligatorio' }}
                                    render={({ field, fieldState: { error } }) => (
                                        <CHTextField
                                            {...field}
                                            label="Valor a incrementar *"
                                            fullWidth
                                            error={!!error}
                                            helperText={error?.message}
                                            pesosEnterosMask
                                        />
                                    )}
                                />
                            </Grid>
                        )}

                        {mostrarRestruc && (
                            <>
                                <Grid item xs={12} md={6} className={classes.column} >
                                    <Controller
                                        name="cuotaCredito"
                                        control={form.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}

                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                label="Numero de Cuotas a Ampliar"
                                                value={value}
                                                onChange={onChange}
                                                numberMask

                                                InputLabelProps={{ shrink: true }}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6} className={classes.column} >
                                    <Controller
                                        name="valorPagado"
                                        control={form.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}

                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                label="Valor a Pagar"
                                                value={value}
                                                onChange={onChange}
                                                currencyMask

                                                InputLabelProps={{ shrink: true }}
                                            />
                                        )}
                                    />
                                </Grid>

                                <Grid item xs={12} md={6} className={classes.column} >
                                    <Controller
                                        name="porcentajeSeguroVida"
                                        control={form.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}

                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                label="Porcentaje de Seguro Vida"
                                                value={value}
                                                onChange={onChange}
                                                percentMask

                                                InputLabelProps={{ shrink: true }}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6} className={classes.column} >
                                    <Controller
                                        name="porcentajeSeguroHogar"
                                        control={form.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}

                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                label="Porcentaje de Seguro Hogar"
                                                value={value}
                                                onChange={onChange}
                                                percentMask

                                                InputLabelProps={{ shrink: true }}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6} className={classes.column} >
                                    <Controller
                                        name="porcentajeTasaNominalMensual"
                                        control={form.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}

                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                label="Porcentaje Tasa Crédito EA (%)"
                                                value={value}
                                                onChange={onChange}
                                                percentMask

                                                InputLabelProps={{ shrink: true }}
                                            />
                                        )}
                                    />
                                </Grid>



                            </>
                        )}

                        {!mostrarInformacionOtros && (
                            <>
                                <Grid item xs={12} md={6} className={classes.column}>
                                    <Controller
                                        name="fechaNovedad"
                                        control={form.control}
                                        rules={{
                                            required: 'Este campo es requerido.'
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                label="Fecha novedad *"
                                                invalidDateMessage="Fecha no válida"
                                                error={!!error}
                                                value={value}
                                                readOnly={readOnly}
                                                InputProps={{ readOnly }}
                                                helperText={error?.message}
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                onChange={date => {
                                                    onChange(date);
                                                }}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6} className={classes.column}>
                                    <Controller
                                        name="fechaFinalizacion"
                                        control={form.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                error={!!error}
                                                invalidDateMessage="Fecha no válida"
                                                helperText={error?.message}
                                                label="Fecha de finalización *"
                                                value={value}
                                                readOnly={readOnly}
                                                InputProps={{ readOnly }}
                                                onChange={date => {
                                                    onChange(date);
                                                }}
                                            />
                                        )}
                                    />
                                </Grid>
                            </>
                        )}
                        {
                            mostrarInformacionOtros && (
                                <>
                                    <Grid item xs={12} md={6} className={classes.column}>
                                        <Controller
                                            name="mesAfectar"
                                            control={form.control}
                                            rules={{
                                                required: 'Este campo es requerido.'
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHDatePicker
                                                    views={['year', 'month']}
                                                    label="Mes a Afectar *"
                                                    format='yyyy/MM'
                                                    invalidDateMessage="Fecha no válida"
                                                    error={!!error}
                                                    value={value}
                                                    readOnly={readOnly}
                                                    InputProps={{ readOnly }}
                                                    helperText={error?.message}
                                                    InputLabelProps={{
                                                        shrink: true,
                                                    }}
                                                    onChange={onChange}
                                                />
                                            )}
                                        />
                                    </Grid>
                                    <Grid item xs={12} md={6} className={classes.column}>
                                        <Controller
                                            name="nuevaTasa"
                                            control={form.control}
                                            rules={{
                                                required: 'Este campo es requerido.',
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    decimalsMaskNumber
                                                    error={!!error}
                                                    helperText={
                                                        error ? error.message : null
                                                    }
                                                    label="Nueva Tasa (%) *"
                                                    InputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                    }}
                                                    value={value}
                                                    onChange={onChange}
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                </>
                            )
                        }
                        {!mostrarInformacionOtros && (
                            <>
                                <Typography className={classes.titleform} variant="h5" component="h6">
                                    Saldo de la obligación
                                </Typography>

                                <Grid container>
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="seguroVida"
                                            control={form.control}
                                            rules={{
                                                required: 'Este campo es requerido.'
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error?.message}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                        className: classes.input
                                                    }}
                                                    label="Seguro de vida"
                                                    value={Math.round(value || seguroVida || productoData?.acumuladoSeguroVida || 0)}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="seguroHogar"
                                            control={form.control}
                                            rules={{
                                                required: 'Este campo es requerido.'
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error?.message}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                        className: classes.input
                                                    }}
                                                    label="Seguro de hogar"
                                                    value={Math.round(value || seguroHogar || productoData?.acumuladoSeguroHogar || 0)}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="capital"
                                            control={form.control}
                                            rules={{
                                                required: 'Este campo es requerido.'
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error?.message}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                        className: classes.input
                                                    }}
                                                    label="Capital"
                                                    value={capital || productoData?.saldoCapital || 0}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="interesCorriente"
                                            control={form.control}
                                            rules={{
                                                required: 'Este campo es requerido.'
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error?.message}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                        className: classes.input
                                                    }}
                                                    label="Interes Corrientes"
                                                    value={Math.round(Number(interesCorriente) || 0)}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="interesMora"
                                            control={form.control}
                                            rules={{
                                                required: 'Este campo es requerido.'
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error?.message}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                        className: classes.input
                                                    }}
                                                    label="Intereses de mora"
                                                    value={Math.round(value || interesMora || productoData?.acumuladoInteresMora || 0)}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>

                                    {
                                        (producto.idTipoCredito === 1) &&
                                        <>
                                            <Grid className={classes.column} item xs={12} md={3}>
                                                <Controller
                                                    name="capitalRemanente"
                                                    control={form.control}
                                                    rules={{
                                                        required: 'Este campo es requerido.'
                                                    }}
                                                    render={({
                                                        field: { onChange, value },
                                                        fieldState: { error },
                                                    }) => (
                                                        <CHTextField
                                                            error={!!error}
                                                            helperText={error?.message}
                                                            isReadOnly
                                                            inputProps={{
                                                                style: {
                                                                    textAlign: 'right',
                                                                },
                                                                className: classes.input
                                                            }}
                                                            label="Capital Remanente"
                                                            value={capitalRemanente || productoData?.capitalRemanenteCorte || 0}
                                                            onChange={onChange}
                                                            pesosEnterosMask
                                                            InputLabelProps={{ shrink: true }}
                                                        />
                                                    )}
                                                />
                                            </Grid>

                                            <Grid className={classes.column} item xs={12} md={3}>
                                                <Controller
                                                    name="interesRemanente"
                                                    control={form.control}
                                                    rules={{
                                                        required: 'Este campo es requerido.'
                                                    }}
                                                    render={({
                                                        field: { onChange, value },
                                                        fieldState: { error },
                                                    }) => (
                                                        <CHTextField
                                                            error={!!error}
                                                            helperText={error?.message}
                                                            isReadOnly
                                                            inputProps={{
                                                                style: {
                                                                    textAlign: 'right',
                                                                    boxSizing: 'border-box'
                                                                },
                                                                className: classes.input
                                                            }}
                                                            label="Interés Remanente"
                                                            value={interesRemanente || productoData.interesesRemanenteCorte.toFixed(0) || 0}
                                                            onChange={onChange}
                                                            pesosEnterosMask
                                                            InputLabelProps={{ shrink: true }}
                                                        />
                                                    )}
                                                />
                                            </Grid>

                                        </>
                                    }

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="totalDeuda"
                                            control={form.control}
                                            rules={{
                                                required: 'Este campo es requerido.'
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error?.message}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                        className: classes.input
                                                    }}
                                                    label="Total deuda"
                                                    value={Math.round(totalDeuda || productoData?.saldoTotalCorte || 0)}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>

                                    {mostrarCuotas && (
                                        <Grid className={classes.column} item xs={12} md={3}>
                                            <Controller
                                                name="cuotaCredito"
                                                control={form.control}
                                                rules={{
                                                    required: 'Este campo es requerido.',
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHTextField
                                                        onChange={onChange}
                                                        label="Número de Cuotas *"
                                                        type="number"
                                                        value={value}
                                                        error={!!error}
                                                        helperText={error ? error.message : null}
                                                        InputLabelProps={{ shrink: true }}
                                                    />
                                                )}
                                            />
                                        </Grid>
                                    )}
                                </Grid>
                            </>
                        )}

                        {mostrarInformacionOtros && (
                            <>
                                <Typography className={classes.titleform} variant="h5" component="h6">
                                    Información al Corte
                                </Typography>
                                <Grid container>
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="saldoCapital"
                                            control={form.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                    }}
                                                    label="Saldo Capital"
                                                    value={productoData.saldoCapital.toFixed(0)}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="interesRemanente"
                                            control={form.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                    }}
                                                    label="Interes Remanente"
                                                    value={productoData.interesesRemanenteCorte.toFixed(0)}
                                                    onChange={onChange}
                                                    currencyMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="interesMora"
                                            control={form.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                    }}
                                                    label="Interes Corriente"
                                                    value={productoData.interesMora || 0}
                                                    onChange={onChange}
                                                    currencyMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="saldoTotalCorte"
                                            control={form.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                    }}
                                                    label="Saldo al Corte"
                                                    value={productoData.saldoTotalCorte.toFixed(0)}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                </Grid>

                                <Grid container>
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="capitalRemanenteCorte"
                                            control={form.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                    }}
                                                    label="Capital Remanente"
                                                    value={productoData.capitalRemanenteCorte.toFixed(0)}
                                                    onChange={onChange}
                                                    currencyMask

                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                    {/* <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="calificacion"
                                control={form.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                        <CHSelect
                                            label="Calificación"
                                            value={value || ""}
                                            onChange={(value) => {
                                                onChange(value)
                                            }}>
                                            <MenuItem value="">
                                                Seleccione una calificación
                                            </MenuItem>
                                            {
                                                Calificaciones?.map(
                                                    (option: any) => (
                                                        <MenuItem
                                                            key={option.calificacion}
                                                            value={option.calificacion}
                                                        >
                                                            {option.descripcion}
                                                        </MenuItem>
                                                    )
                                            )}

                                        </CHSelect>
                                )}
                            />
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={6}>
                            <Controller
                                name="motivoCalificacion"
                                control={form.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error?.message}
                                        isReadOnly={false}
                                        label="Motivación Calificación"
                                        value={value}
                                        onChange={e => {
                                            if(e.target.value.length <= 300){
                                                onChange(e);
                                            }
                                        }}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                 />
                                )}
                            />
                        </Grid> */}
                                </Grid>
                            </>
                        )}

                        {
                            readOnly &&
                            <>
                                <Typography className={classes.titleform} variant="h5" component="h6">
                                    Estado de la novedad
                                </Typography>

                                <Grid container>
                                    <Grid item xs={12} md={6} className={classes.column}  >
                                        <Controller
                                            name="estado"
                                            control={form.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHSelect
                                                    readOnly
                                                    label="Estado"
                                                    value={value || ""}
                                                    onChange={(value) => {
                                                        onChange(value)
                                                    }}>
                                                    <MenuItem value="">
                                                        Seleccione un estado de novedad
                                                    </MenuItem>
                                                    {
                                                        estadosNovedades?.map(
                                                            (option: any) => (
                                                                <MenuItem
                                                                    key={option.id}
                                                                    value={option.id}
                                                                >
                                                                    {option.nombre}
                                                                </MenuItem>
                                                            )
                                                        )}

                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                                    <Grid className={classes.column} item xs={12} md={6}>
                                        <Controller
                                            name="valorPagado"
                                            control={form.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    error={!!error}
                                                    helperText={error?.message}
                                                    isReadOnly
                                                    inputProps={{
                                                        style: {
                                                            textAlign: 'right',
                                                        },
                                                        className: classes.input
                                                    }}
                                                    label="Valor Pagado"
                                                    value={value}
                                                    onChange={onChange}
                                                    pesosEnterosMask
                                                    InputLabelProps={{ shrink: true }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                </Grid>
                            </>
                        }


                        <Grid container>
                            <Grid item style={{ marginTop: '100px' }}>
                                {
                                    !readOnly &&
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        style={{ marginRight: "20px" }}
                                        onClick={() => {
                                            form.handleSubmit((data) => {
                                                data.cuotaCredito = Number(form.watch('cuotaCredito'));
                                                handleSubmit(data);
                                            })();
                                        }}>
                                        GUARDAR
                                    </Button>
                                }
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="default"
                                    onClick={handleClose}
                                >
                                    ATRÁS
                                </Button>
                            </Grid>
                        </Grid>

                    </Grid>
                </CHPaper>
            </Dialog>
        </div>
    );
};