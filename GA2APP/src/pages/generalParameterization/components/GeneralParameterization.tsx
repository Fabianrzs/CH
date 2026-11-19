import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import {
    Button, createStyles,
    Grid,
    InputAdornment,
    makeStyles,
    Typography
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { startPostUpdateSimuladorGeneral } from '../../../actions/simuladorGeneral.action';
import { AppState } from '../../../store/rootReducer';
import { SimuladorGeneralDto } from '../../../types/simuladorGeneral.model.type';
import { SimuladorGeneralForm } from '../constants/simuladorGeneralForm.model.type';
import ApiSimuladorGeneral from '../../../services/api-simuladorGeneral.service';
import UtilService from '../../../utils/util.service';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import CarteraApi from '../../../services/api-cartera-service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center'
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
    }),
);

const GeneralParameterization = (props: props) => {

    const classes = useStyles();
    const api = ApiSimuladorGeneral();
    const carteraApi = CarteraApi();

    const { startPostUpdateSimuladorGeneral } = props;
    const [activarTU, setActivarTU] = useState<boolean>(false)
    const [activarBlob, setActivarBlob] = useState<boolean>(false)
    const [ahorroInteresesActivar, setAhorrosActivar] = useState<boolean>(false)
    const [cesantiasInteresesActivar, setCesantiasInteresesActivar] = useState<boolean>(false)
    const [rangoAscensoActivar, setRangoAscensoActivar] = useState<boolean>(false)
    const [rangoIncluirObligacionesExternasCapacidadPago, setRangoIncluirObligacionesExternasCapacidadPago] = useState<boolean>(false)

    const formularioParametrosGenerales = useForm<SimuladorGeneralDto>({
        defaultValues: SimuladorGeneralForm,
    })

    useEffect(() => {
        obtenerDatos();
    }, [])
    useEffect(() => {
        carteraApi
            .getActiveTU()
            .then((response: any) => {
                if (response) {
                    console.log(response.data)
                    setActivarTU(response.data.activeTu)
                }
            })
            .catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
    }, [])

    const onChangeActivarTU = (e: any) => {
        setActivarTU(e.target.checked);
        formularioParametrosGenerales.setValue('activeTu', e.target.checked);
        setActivarTU(e.target.checked)

    };
    const onChangeActivarBlob = (e: any) => {
        setActivarBlob(e.target.checked);
        formularioParametrosGenerales.setValue('activeBlob', e.target.checked);
        setActivarBlob(e.target.checked)

    };

    const onChangeAhorroIntereses = (e: any) => {
        setAhorrosActivar(e.target.checked);
        formularioParametrosGenerales.setValue('ahorroIntereses', e.target.checked);
        setAhorrosActivar(e.target.checked)

    };
    const onChangeCesantiasIntereses = (e: any) => {
        setCesantiasInteresesActivar(e.target.checked);
        formularioParametrosGenerales.setValue('cesantiasIntereses', e.target.checked);
        setCesantiasInteresesActivar(e.target.checked)

    };
    const onChangeRangoAscenso = (e: any) => {
        setRangoAscensoActivar(e.target.checked);
        formularioParametrosGenerales.setValue('rangoAscenso', e.target.checked);
        setRangoAscensoActivar(e.target.checked)

    };
    const onChangeRangoIncluirObligacionesExternasCapacidadPago = (e: any) => {
       setRangoIncluirObligacionesExternasCapacidadPago(e.target.checked);
        formularioParametrosGenerales.setValue('incluirObligacionesExternasCapacidadPago', e.target.checked);
        setRangoIncluirObligacionesExternasCapacidadPago(e.target.checked)

    };

    const obtenerDatos = async () => {
        await api.getParametrosGenerales()
            .then(async (response) => {
                const datos = response.data as SimuladorGeneralDto;
                formularioParametrosGenerales.setValue('parametroSalarioMinimo', datos.parametroSalarioMinimo);
                formularioParametrosGenerales.setValue('porcentajeInflacion', datos.porcentajeInflacion);
                formularioParametrosGenerales.setValue('porcentajeViviendaVis', datos.porcentajeViviendaVis);
                formularioParametrosGenerales.setValue('porcentajeViviendaNoVis', datos.porcentajeViviendaNoVis);
                formularioParametrosGenerales.setValue('limiteSalariosVis', datos.limiteSalariosVis);
                formularioParametrosGenerales.setValue('diasPreaprobado', datos.diasPreaprobado);
                formularioParametrosGenerales.setValue('diasAprobado', datos.diasAprobado !== 0 ? datos.diasAprobado : 180);
                formularioParametrosGenerales.setValue('puntajeMinimoCentrales', datos.puntajeMinimoCentrales);
                formularioParametrosGenerales.setValue('tarifaUnicaAvaluos', datos.tarifaUnicaAvaluos);
                formularioParametrosGenerales.setValue('tarifaEstudioTitulos', datos.tarifaEstudioTitulos);
                formularioParametrosGenerales.setValue('porcentajeMinimoEndeudamiento', datos.porcentajeMinimoEndeudamiento);
                formularioParametrosGenerales.setValue('vigenciaDiasConsultaCentrales', datos.vigenciaDiasConsultaCentrales);
                formularioParametrosGenerales.setValue('activeTu', datos.activeTu);
                formularioParametrosGenerales.setValue('score_Min_TU', datos.score_Min_TU);
                formularioParametrosGenerales.setValue('maximoMesesAlivioTasa', datos.maximoMesesAlivioTasa);
                formularioParametrosGenerales.setValue('numeroMesesAlivio', datos.numeroMesesAlivio);
                formularioParametrosGenerales.setValue('factorAhorro', datos.factorAhorro || 0);
                formularioParametrosGenerales.setValue('factorCesantias', datos.factorCesantias || 0);
                formularioParametrosGenerales.setValue('factorSubsidio', datos.factorSubsidio || 0);
                formularioParametrosGenerales.setValue('porcentajeIncrementoAnual', datos.porcentajeIncrementoAnual || 0);
                formularioParametrosGenerales.setValue('valorMinimoCuota', datos.valorMinimoCuota || 0);
                formularioParametrosGenerales.setValue('valorTasaUsura', datos.valorTasaUsura || 0);
                formularioParametrosGenerales.setValue('valorTasaMora', datos.valorTasaMora || 0);
                formularioParametrosGenerales.setValue('fechaTasaUsura', datos.fechaTasaUsura || undefined);
                formularioParametrosGenerales.setValue('fechaTasaMora', datos?.fechaTasaMora);
                formularioParametrosGenerales.setValue('valor_SMMLV', datos?.valor_SMMLV);
                formularioParametrosGenerales.setValue('mesesDiferir', datos.mesesDiferir || 0);
                formularioParametrosGenerales.setValue('activeBlob', datos.activeBlob);
                formularioParametrosGenerales.setValue('valorIpvAnual', datos?.valorIpvAnual);
                formularioParametrosGenerales.setValue('valorIpvAnualOtrasCiudades', datos?.valorIpvAnualOtrasCiudades);
                formularioParametrosGenerales.setValue('tasaUsuraPeriodica', datos?.tasaUsuraPeriodica);
                formularioParametrosGenerales.setValue('ahorroIntereses', datos?.ahorroIntereses);
                formularioParametrosGenerales.setValue('cesantiasIntereses', datos?.cesantiasIntereses);
                formularioParametrosGenerales.setValue('rangoAscenso', datos?.rangoAscenso)
                formularioParametrosGenerales.setValue('incluirObligacionesExternasCapacidadPago', datos?.incluirObligacionesExternasCapacidadPago)
                setAhorrosActivar(!!datos?.ahorroIntereses);
                setCesantiasInteresesActivar(!!datos?.cesantiasIntereses);
                setRangoAscensoActivar(!!datos?.rangoAscenso);
                setRangoIncluirObligacionesExternasCapacidadPago(!!datos?.incluirObligacionesExternasCapacidadPago);
            })
            .catch(() => {
                toast("Error al consultar información parámetros", { type: toast.TYPE.ERROR });
            });
    }

    const UpdateState = async (data: any) => {
        const porcentajeIn = data.porcentajeInflacion.toString().replace("%", "")
        const porcentajeVVis = data.porcentajeViviendaVis.toString().replace("%", "")
        const porcentajeVNoVis = data.porcentajeViviendaNoVis.toString().replace("%", "")
        const valorTasaUsura1 = data.valorTasaUsura.toString().replace("%", "")
        const valorTasaMora1 = data.valorTasaMora.toString().replace("%", "")
        const factorAhorro1 = data.factorAhorro.toString().replace("%", "")
        const valorIpvAnual = data.valorIpvAnual.toString().replace("%", "")
        const valorIpvAnualOtrasCiudades = data.valorIpvAnualOtrasCiudades.toString().replace("%", "")
        const porcentajeIA = data.porcentajeIncrementoAnual.toString().replace("%", "")
        const porcentajeMinimoEndeudamientoIA = data.porcentajeMinimoEndeudamiento.toString().replace("%", "")
        const parametrosGenerales: SimuladorGeneralDto = {
            parametroSalarioMinimo: UtilService.cleanCurrencyValue(data.parametroSalarioMinimo),
            porcentajeInflacion: porcentajeIn,
            porcentajeViviendaVis: porcentajeVVis,
            porcentajeViviendaNoVis: porcentajeVNoVis,
            limiteSalariosVis: data.valor_SMMLV,
            diasPreaprobado: data.diasPreaprobado,
            diasAprobado: data.diasAprobado,
            puntajeMinimoCentrales: data.puntajeMinimoCentrales,
            porcentajeMinimoEndeudamiento: porcentajeMinimoEndeudamientoIA,
            vigenciaDiasConsultaCentrales: data.vigenciaDiasConsultaCentrales,
            tarifaEstudioTitulos: UtilService.cleanCurrencyValue(data.tarifaEstudioTitulos),
            maximoMesesAlivioTasa: data.maximoMesesAlivioTasa,
            numeroMesesAlivio: data.numeroMesesAlivio,
            factorAhorro: factorAhorro1,
            factorCesantias: data.factorCesantias,
            factorSubsidio: data.factorSubsidio,
            porcentajeIncrementoAnual: porcentajeIA,
            tarifaUnicaAvaluos: UtilService.cleanCurrencyValue(data.tarifaUnicaAvaluos),
            valorMinimoCuota: UtilService.cleanCurrencyValue(data.valorMinimoCuota),
            valorTasaUsura: valorTasaUsura1,
            valorTasaMora: valorTasaMora1,
            fechaTasaUsura: data.fechaTasaUsura,
            fechaTasaMora: data.fechaTasaMora,
            score_Min_TU: data.score_Min_TU,
            activeTu: activarTU,
            activeBlob: activarBlob,
            valor_SMMLV: data.valor_SMMLV,
            mesesDiferir: parseInt(data.mesesDiferir, 10),
            valorIpvAnual: valorIpvAnual,
            valorIpvAnualOtrasCiudades: valorIpvAnualOtrasCiudades,
            tasaUsuraPeriodica: data.tasaUsuraPeriodica,
            ahorroIntereses: ahorroInteresesActivar,
            cesantiasIntereses:cesantiasInteresesActivar,
            rangoAscenso: rangoAscensoActivar,
            incluirObligacionesExternasCapacidadPago: rangoIncluirObligacionesExternasCapacidadPago
        }
        await startPostUpdateSimuladorGeneral(parametrosGenerales);
        obtenerDatos();
        toast.success("Información actualizada");
    }

    const handleOnClickUpdate = (data: any) => {
        swal({
            text: "¿Está seguro que desea actualizar la información?",
            icon: "warning",
            buttons: ["Sí", "No"]
        }).then(respuesta => {
            if (!respuesta) {
                UpdateState(data);
            }
        })
    }

    return (
        <CHPrivateContainer>
            <CHPaper title="Parámetrización General">
                <Typography className={classes.titleform} variant="h5" component="h6">
                    Parámetros Generales
                </Typography>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="parametroSalarioMinimo"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor SMMLV"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="porcentajeInflacion"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'El valor no puede ser mayor a 100',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="%Valor IPC anual"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="porcentajeViviendaVis"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'El valor no puede ser mayor a 100',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="% Financiación Vivienda VIS"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="porcentajeViviendaNoVis"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'El valor no puede ser mayor a 100',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="% Financiación Vivienda NO VIS"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
             
                <Grid container>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valor_SMMLV"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Límite SMMLV Vivienda VIS"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value || 0}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tarifaUnicaAvaluos"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Tarifa Avalúos"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tarifaEstudioTitulos"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Tarifa Estudio de Título"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorMinimoCuota"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor Cuota Minimo"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="vigenciaDiasConsultaCentrales"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Vigencia Consulta Central de Riesgo (días)"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="diasPreaprobado"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Vigencia de Preaprobado (días)"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value || 0}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="diasAprobado"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Vigencia de Aprobación (días)"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value || 0}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="mesesDiferir"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Plazo a diferir la deuda (meses)"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value || 0}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="porcentajeMinimoEndeudamiento"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'El valor debe ser menor a 100',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="% Mínimo de endeudamiento"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="activeTu"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <CHCheckbox
                                    label="Activar Transunion"
                                    onChange={onChangeActivarTU}
                                    checked={activarTU}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="activeBlob"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <CHCheckbox
                                    label="Activar BlobStorage"
                                    onChange={onChangeActivarBlob}
                                    checked={activarBlob}
                                />
                            )}
                        />
                    </Grid>
                   
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ahorroIntereses"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <CHCheckbox
                                    label="Interés sobre interés de ahorro"
                                    onChange={onChangeAhorroIntereses }
                                    checked={ahorroInteresesActivar}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="cesantiasIntereses"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <CHCheckbox
                                    label="Interés sobre interés de cesantías"
                                    onChange={onChangeCesantiasIntereses}
                                    checked={cesantiasInteresesActivar}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="rangoAscenso"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <CHCheckbox
                                    label="Cálculo especial de rango"
                                    onChange={onChangeRangoAscenso }
                                    checked={rangoAscensoActivar}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="incluirObligacionesExternasCapacidadPago"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <CHCheckbox
                                    label="Incluir obligaciones externas en la capacidad de pago"
                                    onChange={onChangeRangoIncluirObligacionesExternasCapacidadPago }
                                    checked={rangoIncluirObligacionesExternasCapacidadPago}
                                />
                            )}
                        />
                    </Grid>



                </Grid>

                  

                <Typography className={classes.titleform} variant="h5" component="h6">
                    Reajuste de Valorización de Garantías
                </Typography>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorIpvAnual"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor IPV Anual - Bogotá"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorIpvAnualOtrasCiudades"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor IPV Anual - Otras Ciudades"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Typography className={classes.titleform} variant="h5" component="h6">
                    Alivios Financieros
                </Typography>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="maximoMesesAlivioTasa"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Máximo Meses Alivio de Tasa (FRECH)"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroMesesAlivio"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a 0',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Meses que Aplica Alivio"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Typography className={classes.titleform} variant="h5" component="h6">
                    Factores y Porcentajes
                </Typography>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="factorAhorro"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Factor Ahorro"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="factorCesantias"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    decimalsMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Factor Cesantias"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="factorSubsidio"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    decimalsMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Factor Subsidio"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="score_Min_TU"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    decimalsMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Score Mínimo"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="porcentajeIncrementoAnual"
                            control={formularioParametrosGenerales.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    percentMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Porcentaje Incremento Anual Salario"
                                    style={{
                                        marginLeft: '10px',
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}

                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Typography className={classes.titleform} variant="h5" component="h6">
                    Tasas Usura
                </Typography>
                <Grid container>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorTasaUsura"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                min: {
                                    value: 0,
                                    message:
                                        'No puede ser menor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'No puede ser mayor a 100',
                                }
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    decimalsMaskByNumber
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Tasa Usura EA (%)"

                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tasaUsuraPeriodica"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                min: {
                                    value: 0,
                                    message:
                                        'No puede ser menor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'No puede ser mayor a 100',
                                }
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    decimalsMaskByNumber
                                    isReadOnly
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Tasa Usura Periódica (%)"
                                    InputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                        endAdornment: (
                                            <InputAdornment position="end">
                                                {'    '}
                                                NMMV  {' '}
                                            </InputAdornment>
                                        ),
                                    }}
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>


                    <Grid item xs={12} md={3} className={classes.column}>
                        <Controller
                            name="fechaTasaMora"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker

                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Fecha Inicial Tasa Usura"
                                    onChange={onChange}
                                    value={value}

                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={3} className={classes.column}>
                        <Controller
                            name="fechaTasaUsura"
                            control={formularioParametrosGenerales.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker

                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Fecha Final Tasa Usura"
                                    onChange={onChange}
                                    value={value}

                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Typography className={classes.titleform} variant="h5" component="h6">
                    Mora
                </Typography>
                <Grid container>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="valorTasaMora"
                            control={formularioParametrosGenerales.control}

                            rules={{
                                required: 'Este campo es requerido.',
                                min: {
                                    value: 0,
                                    message:
                                        'No puede ser menor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'No puede ser mayor a 100',
                                }
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    decimalsMaskByNumber
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Tasa Mora"

                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}

                                    InputLabelProps={{ shrink: true }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={8} />
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            onClick={formularioParametrosGenerales.handleSubmit(handleOnClickUpdate)}                            >
                            ACTUALIZAR
                        </Button>
                    </Grid>
                </Grid>

            </CHPaper>
        </CHPrivateContainer>
    )
}

const mapStateToProps = (state: AppState) => ({
    simuladorState: state.simuladorGeneral
})
interface LinkMapDispatchToProps {
    startPostUpdateSimuladorGeneral: (simulador: SimuladorGeneralDto) => void;
}

const mapDispatchToProps = {
    startPostUpdateSimuladorGeneral
}

type props = LinkMapDispatchToProps;
export default connect(mapStateToProps, mapDispatchToProps)(GeneralParameterization);