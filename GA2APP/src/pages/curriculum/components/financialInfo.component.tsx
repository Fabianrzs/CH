import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Grid, createStyles, makeStyles, Button } from '@material-ui/core';
import { Controller } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import { ClientState } from '../../../reducers/client.reducer';
import { ClienteDto } from '../../../types/client.model.type';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ACTIVIDADECONOMICA, CURRENCYTYPE } from '../../../types/catalog.type';
import { startGetCurrencyTypes } from '../../../actions/catalog.action';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { LoadingComponent } from '../../../components/loading/loading.component';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import UtilService from '../../../utils/util.service';
import { startUpdateClient, startValidatePermisoActualizacion } from '../../../actions/client.action';
import { ClientForm } from '../constants/form.mode.type';
import NotificationService from '../../../services/notification.service';
import history from '../../../utils/history.router';
import AuthService from '../../../services/auth.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
            borderRadius: '10%',
            background: '#1C3F8B',
            color: '#FFFFFF',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
        disabledAutocomplete: {
            '& .MuiInputBase-root.Mui-disabled': {
                color: 'rgba(0, 0, 0, 0.87)',
                WebkitTextFillColor: 'rgba(0, 0, 0, 0.87)',
                opacity: 1,
            },
        },
    }),
);

function FinancialInformation({
    clientState,
    catalogState,
    clientModel,
    loginState,
    control,
    setValue,
    watch,
    handleSubmit,
    ingresoAdicional,
    totalActivos,
    totalPasivos,
    startUpdateClient
}: {
    clientState: ClientState;
    catalogState: CatalogState;
    clientModel: ClienteDto;
    loginState: LoginState;
    control: any;
    setValue: any;
    watch: any;
    handleSubmit: any;
    ingresoAdicional: any;
    totalActivos: any;
    totalPasivos: any;
    startUpdateClient: (client: ClienteDto, claims: string) => void;
}) {
    const classes = useStyles();
    useEffect(() => {
        startGetCurrencyTypes();
    }, [])
    const [disabled, setDisabled] = useState(false);
    const [isUpdating, setIsUpdating] = useState(false);
    const [actividadEconomicaValue, setActividadEconomicaValue] =
        React.useState<ACTIVIDADECONOMICA | null>(null);

    const defaultEconomicActivity = catalogState.economicActivities?.find(
        (x: ACTIVIDADECONOMICA) => x.id === clientModel.idActividadEconomica,
    );

    if (
        defaultEconomicActivity != null &&
        actividadEconomicaValue == null &&
        defaultEconomicActivity.id > 0
    ) {
        setActividadEconomicaValue(defaultEconomicActivity);
    }

    const onChangeEconomicActivity = (
        event: any,
        newValue: ACTIVIDADECONOMICA | null,
    ) => {
        setActividadEconomicaValue(newValue);
        setValue('idActividadEconomica', newValue?.id);
    };


    const [tipoMonedaValue, setTipoMonedaValue] =
        React.useState<CURRENCYTYPE | null>(null);

    const defaultTipoMoneda = catalogState.currencyTypes?.find(
        (x: CURRENCYTYPE) => x.idMoneda === clientModel.idMoneda,
    );

    if (defaultTipoMoneda != null && tipoMonedaValue == null) {
        setTipoMonedaValue(defaultTipoMoneda);
    }

    const onChangeCurrencyType = (
        event: any,
        newValue: CURRENCYTYPE | null,
    ) => {
        setTipoMonedaValue(newValue);
        setValue('idMoneda', newValue?.idMoneda);
    };

    const [transaccionesExtranjeraValue, setTransaccionesExtranjeraValue] =
        React.useState(clientModel.transaccionesMonedaExtranjera);

    const onChangeTransaccionesExtranjera = (e: any) => {
        setTransaccionesExtranjeraValue(e.target.checked);
        setValue('transaccionesMonedaExtranjera', e.target.checked);
    };

    const [hasIngresosAdicionalesValue, setHasIngresosAdicionalesValue] =
        React.useState(ingresoAdicional > 0);

    useEffect(() => {
        setHasIngresosAdicionalesValue(
            +ingresoAdicional.toString().replaceAll('$', '').replaceAll(',', '') > 0,
        );
    }, [ingresoAdicional]);

    const [patrimonio, setPatrimonio] =
        React.useState(clientModel.totalPatrimonio);

    useEffect(() => {
        if (totalActivos !== '-$_') {
            setPatrimonio(
                +totalActivos.toString().replaceAll('$', '').replaceAll(',', '') -
                +totalPasivos.toString().replaceAll('$', '').replaceAll(',', '')
            );
        }
        if (loginState.opciones?.filter(x => x.codigo === 'ACTUALIZAHVFINANCIERA').length > 0) {
            setDisabled(false)
        } else {
            setDisabled(true)
        }
    }, [totalActivos]);

    useEffect(() => {
        if (totalPasivos !== '-$_') {
            setPatrimonio(
                +totalActivos.toString().replaceAll('$', '').replaceAll(',', '') -
                +totalPasivos.toString().replaceAll('$', '').replaceAll(',', '')
            );
        }
    }, [totalPasivos]);

    const [clientInfo, setClientInfo] = React.useState(ClientForm);

    const handleClickUpdate = async (data: ClienteDto) => {
        if (isUpdating) {
            return;
        }

        if (loginState.opciones?.filter(x => x.codigo === 'ACTUALIZAHVFINANCIERA').length === 0) {
            NotificationService.genericAlwarning(
                'Advertencia',
                'No cuenta con permisos para realizar esta acción.',
            );
            return;
        }

        try {
            setIsUpdating(true);
            await validatePermisoActualizar(data);
        } finally {
            setIsUpdating(false);
        }
    };

    const validarFechaNacimiento = (fechaNacimiento: any): boolean => {
        const fecha = moment(fechaNacimiento);

        if (
            fechaNacimiento === null ||
            fechaNacimiento === undefined ||
            fechaNacimiento === '' ||
            !fecha.isValid()
        ) {
            NotificationService.genericAlwarning(
                'Advertencia',
                'Ingrese una fecha de nacimiento válida.'
            );
            return false;
        }

        if (fecha.isSameOrBefore(moment().subtract(100, 'years'), 'day')) {
            NotificationService.genericAlwarning(
                'Advertencia',
                'Ingrese una fecha de nacimiento válida.'
            );
            return false;
        }

        if (fecha.isAfter(moment(), 'day')) {
            NotificationService.genericAlwarning(
                'Advertencia',
                'La fecha de nacimiento no debe ser mayor a la actual.'
            );
            return false;
        }

        return true;
    };

    // async function validatePermisoActualizar(data: any) {

    //     data.lugarNacimientoExtranjero = data.lugarNacimientoExtranjero?.toString();
    //     data.ingresoMensual = data.ingresoMensual.toString().includes('$')
    //         ? +data.ingresoMensual
    //             .toLocaleString()
    //             .replaceAll('$', '')
    //             .replaceAll(',', '')
    //         : +data.ingresoMensual;
    //     data.egresoMensual = data.egresoMensual.toString().includes('$')
    //         ? +data.egresoMensual
    //             .toLocaleString()
    //             .replaceAll('$', '')
    //             .replaceAll(',', '')
    //         : +data.egresoMensual;
    //     data.totalActivos = data.totalActivos.toString().includes('$')
    //         ? +data.totalActivos
    //             .toLocaleString()
    //             .replaceAll('$', '')
    //             .replaceAll(',', '')
    //         : +data.totalActivos;
    //     data.totalPasivos = data.totalPasivos.toString().includes('$')
    //         ? +data.totalPasivos
    //             .toLocaleString()
    //             .replaceAll('$', '')
    //             .replaceAll(',', '')
    //         : +data.totalPasivos;
    //     data.totalPatrimonio = data.totalPatrimonio.toString().includes('$')
    //         ? +data.totalPatrimonio
    //             .toLocaleString()
    //             .replaceAll('$', '')
    //             .replaceAll(',', '')
    //         : +data.totalPatrimonio;

    //     data.montoTransaccionesExtranjero = data.montoTransaccionesExtranjero
    //         .toString()
    //         .includes('$')
    //         ? +data.montoTransaccionesExtranjero
    //             .toLocaleString()
    //             .replaceAll('$', '')
    //             .replaceAll(',', '')
    //         : +data.montoTransaccionesExtranjero;

    //     data.ingresoAdicional =
    //         data.ingresoAdicional != null &&
    //             data.ingresoAdicional.toString().includes('$')
    //             ? +data.ingresoAdicional
    //                 .toLocaleString()
    //                 .replaceAll('$', '')
    //                 .replaceAll(',', '')
    //             : +data.ingresoAdicional;

    //     if (data.actividadEconomica) {
    //         data.idActividadEconomica = data.actividadEconomica.id;
    //     } else {
    //         data.idActividadEconomica = 0;
    //     }

    //     setClientInfo(data);


    //     if (await validateRequiredFields(data)) {
    //         data.actualiza = 2;
    //         const user = AuthService.getUserInfo();
    //         data.usuario = user.USR_USERNAME;
    //         await startUpdateClient(data, loginState.claims);
    //         // history.push('/HojaDeVida');
    //     }
    //     return true;
    // }
    async function validatePermisoActualizar(data: any) {

        if (!validarFechaNacimiento(data.fechaNacimiento)) {
            return false;
        }

        data.lugarNacimientoExtranjero = data.lugarNacimientoExtranjero?.toString();

        data.ingresoMensual = data.ingresoMensual.toString().includes('$')
            ? +data.ingresoMensual
                .toLocaleString()
                .replaceAll('$', '')
                .replaceAll(',', '')
            : +data.ingresoMensual;

        data.egresoMensual = data.egresoMensual.toString().includes('$')
            ? +data.egresoMensual
                .toLocaleString()
                .replaceAll('$', '')
                .replaceAll(',', '')
            : +data.egresoMensual;

        data.totalActivos = data.totalActivos.toString().includes('$')
            ? +data.totalActivos
                .toLocaleString()
                .replaceAll('$', '')
                .replaceAll(',', '')
            : +data.totalActivos;

        data.totalPasivos = data.totalPasivos.toString().includes('$')
            ? +data.totalPasivos
                .toLocaleString()
                .replaceAll('$', '')
                .replaceAll(',', '')
            : +data.totalPasivos;

        data.totalPatrimonio = data.totalPatrimonio.toString().includes('$')
            ? +data.totalPatrimonio
                .toLocaleString()
                .replaceAll('$', '')
                .replaceAll(',', '')
            : +data.totalPatrimonio;

        data.montoTransaccionesExtranjero = data.montoTransaccionesExtranjero
            .toString()
            .includes('$')
            ? +data.montoTransaccionesExtranjero
                .toLocaleString()
                .replaceAll('$', '')
                .replaceAll(',', '')
            : +data.montoTransaccionesExtranjero;

        data.ingresoAdicional =
            data.ingresoAdicional != null &&
                data.ingresoAdicional.toString().includes('$')
                ? +data.ingresoAdicional
                    .toLocaleString()
                    .replaceAll('$', '')
                    .replaceAll(',', '')
                : +data.ingresoAdicional;

        if (data.actividadEconomica) {
            data.idActividadEconomica = data.actividadEconomica.id;
        } else {
            data.idActividadEconomica = 0;
        }

        setClientInfo(data);

        if (await validateRequiredFields(data)) {
            data.actualiza = 2;
            const user = AuthService.getUserInfo();
            data.usuario = user.USR_USERNAME;
            await startUpdateClient(data, loginState.claims);
        }

        return true;
    }

    async function validateRequiredFields(data: any) {
        let retBool = false;
        const client = data;
        if (client.idActividadEconomica === 0) {
            toast('El campo actividad económica es requerido.');
            return retBool;
        }
        retBool = true;
        return retBool;
    }


    const actividadEconomicaActual = watch('actividadEconomica');

    useEffect(() => {
        if (!catalogState?.economicActivities?.length) return;

        if (!actividadEconomicaActual) {
            const actividadPorDefecto = catalogState.economicActivities.find(
                (item: any) => item.descripcion === 'ACTIVIDADES DE DEFENSA'
            );

            if (actividadPorDefecto) {
                setValue('actividadEconomica', actividadPorDefecto, {
                    shouldValidate: true,
                    shouldDirty: false,
                });
            }
        }
    }, [catalogState?.economicActivities, actividadEconomicaActual, setValue]);







    return (
        <CHPaper title="Información financiera">

            <LoadingComponent key="loader" loading={clientState.loading} />
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>

                    <Controller
                        name="actividadEconomica"
                        control={control}
                        rules={{ required: 'Actividad económica requerida' }}
                        render={({ field }) => (
                            <CHAutoComplete
                                {...field}
                                value={field.value || null}
                                options={catalogState?.economicActivities}
                                getOptionLabel={(option) => option.descripcion}
                                onChange={(_, value) => field.onChange(value)}
                                renderInput={(params) => (
                                    <CHTextField
                                        {...params}
                                        label="Actividad económica"
                                    />
                                )}
                            />
                        )}
                    />

                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="ingresoMensual"
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={disabled}
                                label="Ingreso Mensual"
                                value={value}
                                onChange={onChange}
                                currencyMask
                                currencyMaskOptions={{ allowLeadingZeroes: true }}
                                onFocus={() => {
                                    if (value === 0 || value === "0") onChange("");
                                }}

                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="egresoMensual"
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={disabled}
                                label="Egreso Mensual"
                                value={value}
                                onChange={onChange}
                                currencyMask
                                currencyMaskOptions={{ allowLeadingZeroes: true }}
                                onFocus={() => {
                                    if (value === 0 || value === "0") onChange("");
                                }}

                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="totalActivos"
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={disabled}
                                label="Total Activos"
                                value={value}
                                onChange={onChange}
                                currencyMask
                                currencyMaskOptions={{ allowLeadingZeroes: true }}
                                onFocus={() => {
                                    if (value === 0 || value === "0") onChange("");
                                }}

                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="totalPasivos"
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={disabled}
                                label="Total Pasivos"
                                value={value}
                                onChange={onChange}
                                currencyMask
                                currencyMaskOptions={{ allowLeadingZeroes: true }}
                                onFocus={() => {
                                    if (value === 0 || value === "0") onChange("");
                                }}

                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="totalPatrimonio"
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly
                                label="Total Patrimonio"
                                value={patrimonio}
                                onChange={onChange}
                                currencyMask
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="ingresoAdicional"
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={disabled}
                                label="Ingresos adicionales"
                                value={value}
                                onChange={onChange}
                                currencyMask
                                currencyMaskOptions={{ allowLeadingZeroes: true }}
                                onFocus={() => {
                                    if (value === 0 || value === "0") onChange("");
                                }}

                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="conceptoIngresosAdicionales"
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                disabled={!hasIngresosAdicionalesValue}
                                helperText={error ? error.message : null}
                                isReadOnly={disabled}
                                label="Concepto Ingresos Adicionales"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
            </Grid>
            <Grid container spacing={3}>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHCheckbox
                        label="Transacciones en moneda extranjera"
                        onChange={onChangeTransaccionesExtranjera}
                        checked={transaccionesExtranjeraValue}
                        disabled={disabled}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHAutoComplete
                        disabled={disabled ? true : !transaccionesExtranjeraValue}
                        value={
                            transaccionesExtranjeraValue
                                ? tipoMonedaValue
                                : null
                        }
                        options={catalogState?.currencyTypes}
                        getOptionLabel={option => option.descripcionMoneda.toUpperCase()}
                        onChange={onChangeCurrencyType}
                        renderInput={params => (
                            <CHTextField {...params} label="Moneda"
                                className={disabled ? classes.disabledAutocomplete : ''}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="montoTransaccionesExtranjero"
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={disabled}
                                label="Monto transacciones en moneda extranjera"
                                value={
                                    transaccionesExtranjeraValue ? value : ''
                                }
                                disabled={disabled ? true : !transaccionesExtranjeraValue}
                                onChange={onChange}
                                currencyMask
                                currencyMaskOptions={{ allowLeadingZeroes: true }}
                                onFocus={() => {
                                    if (value === 0 || value === "0") onChange("");
                                }}

                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fechaActualizaDatosFinancieros"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Fecha actualización financiera"
                                value={UtilService.getFullDate(value)}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />

                </Grid>
            </Grid>
            <div className="buttons">
                <Button
                    className={classes.button}
                    onClick={handleSubmit(handleClickUpdate)}
                    disabled={disabled || isUpdating}
                >
                    {isUpdating ? 'Guardando...' : 'Actualizar'}
                </Button>

            </div>
        </CHPaper>
    );
}

const mapStateToProps = (state: AppState) => ({

    catalogState: state.catalog,
    loginState: state.login,
});

const mapDispatchToProps = {
    startGetCurrencyTypes,
    startUpdateClient,
    startValidatePermisoActualizacion
};
export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(FinancialInformation);
