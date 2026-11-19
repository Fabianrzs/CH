import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { Grid, makeStyles, createStyles, MenuItem, Button } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { ClientState } from '../../../reducers/client.reducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import {
    CITYDTO,
    DEPARTAMENTODTO,
    EXECUTINGUNITDTO,
    GRADEDTO,
    FUERZADTO,
    PAISDTO,
    SEXODTO,
    CATEGORIADTO,
} from '../../../types/catalog.type';
import { ClientForm } from '../constants/form.mode.type';
import { ClienteDto } from '../../../types/client.model.type';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import UtilService from '../../../utils/util.service';
import AuthService from '../../../services/auth.service';

import { LoginState } from '../../../reducers/login.reducer';
import { startUpdateClient, startValidatePermisoActualizacion } from '../../../actions/client.action';
import { AppState } from '../../../store/rootReducer';
import ApiClients from '../../../services/api-clients.service';
import NotificationService from '../../../services/notification.service';
import history from '../../../utils/history.router';

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
        disabledAutocomplete: {
            '& .MuiInputBase-root.Mui-disabled': {
                color: 'rgba(0, 0, 0, 0.87)',
                WebkitTextFillColor: 'rgba(0, 0, 0, 0.87)',
                opacity: 1,
            },
        },
    }),
);

function PersonalInformation({
    clientState,
    catalogState,
    clientModel,
    loginState,
    control,
    setValue,
    watch,
    handleSubmit,
    fechaNacimiento,
    fechaAlta,
    categoriaSoldado,
    tipoIdentificacion,
    lugarNacimientoExtranjero,
    idTipoIdentificacion,
    startUpdateClient,
    startValidatePermisoActualizacion
}: {
    loginState: LoginState;
    clientState: ClientState;
    catalogState: CatalogState;
    clientModel: ClienteDto;
    control: any;
    setValue: any;
    watch: any;
    handleSubmit: any;
    fechaNacimiento: any;
    fechaAlta: any;
    categoriaSoldado: boolean;
    tipoIdentificacion: boolean;
    lugarNacimientoExtranjero: string;
    idTipoIdentificacion: number | null;
    startUpdateClient: (client: ClienteDto, claims: string) => void;
    startValidatePermisoActualizacion: (perfil: string) => void;
}) {
    const classes = useStyles();
    const [paisValue, setPaisValue] = React.useState<PAISDTO | null>(null);
    const [fechaPrimerAporte, setFechaPrimerAporte] = useState<any>()
    const [loadedAporte, setLoadedAporte] = useState(false)
    const [disabled, setDisabled] = useState(false)

    const statusClient: any = clientState.client?.estadoCliente
    const aporte: any = clientState.client?.aportesCategoria?.at(0)?.apC_FECHA_PRIMERA_CUOTA

    if (aporte != null && !loadedAporte) {
        setLoadedAporte(true)
        setFechaPrimerAporte(aporte)
    }
    useEffect(() => {
        if (loginState.opciones?.filter(x => x.codigo === 'ACTUALIZAHVPERSONAL').length > 0) {
            setDisabled(false)
        } else {
            setDisabled(true)
        }
    })


    const defaultCountry = catalogState.countries?.find(
        (x: PAISDTO) => x.codigo === clientModel.idPaisNacimiento,
    );

    if (defaultCountry != null && paisValue == null && defaultCountry.id > 0) {
        setPaisValue(defaultCountry);
    }

    const onChangeCountry = (event: any, newValue: PAISDTO | null) => {
        setPaisValue(newValue);
        setValue('idPaisNacimiento', newValue?.codigo);
        clientModel.idPaisNacimiento = newValue?.id;
    };


    const catalogCity = [...catalogState.City];
    const [departamentoValue, setDepartamentoValue] =
        React.useState<DEPARTAMENTODTO | null>(null);
    const [ciudadNacimientoValues, setCiudadNacimientoValues] =
        React.useState<CITYDTO[]>(catalogCity);

    const defaultDepartment = catalogState.departments?.find(
        (x: DEPARTAMENTODTO) =>
            x.codigo === clientModel.idDepartamentoNacimiento,
    );

    if (
        defaultDepartment != null &&
        departamentoValue == null &&
        defaultDepartment.id > 0
    ) {
        setDepartamentoValue(defaultDepartment);
        setCiudadNacimientoValues(
            catalogCity.filter(
                x => x.idDepartamento === defaultDepartment?.codigo,
            ),
        );
    }


    const [ciudadNacimientoValue, setCiudadNacimientoValue] =
        React.useState<CITYDTO | null>(null);
    const [nullCity, setNullCity] = React.useState(false);

    const defaultBirthCity = catalogCity?.find(
        (x: CITYDTO) =>
            x.codigo === clientModel.idCiudadNacimiento &&
            x.idDepartamento === clientModel.idDepartamentoNacimiento,
    );

    if (
        defaultBirthCity != null &&
        ciudadNacimientoValue == null &&
        defaultBirthCity.id > 0
    ) {
        setCiudadNacimientoValue(defaultBirthCity);
    }

    const onChangeDepartment = (
        event: any,
        newValue: DEPARTAMENTODTO | null,
    ) => {
        setCiudadNacimientoValues(
            catalogCity.filter(x => x.idDepartamento === newValue?.codigo),
        );
        setDepartamentoValue(newValue);
        setNullCity(true);
        setValue('idDepartamentoNacimiento', newValue?.id);
        clientModel.idDepartamentoNacimiento = newValue?.id;
        clientModel.idCiudadNacimiento = 0;
        setValue('idCiudadNacimiento', 0);
    };

    const onChangeCity = (event: any, newValue: CITYDTO | null) => {
        setNullCity(false);
        setCiudadNacimientoValue(newValue);
        setValue('idCiudadNacimiento', newValue?.codigo);
        clientModel.idCiudadNacimiento = newValue?.id;
    };


    const [ciudadExpedicionValue, setCiudadExpedicionValue] =
        React.useState<CITYDTO | null>(null);

    const defaultExpeditionCity = catalogCity?.find(
        (x: CITYDTO) =>
            x.codigo === clientModel.idCiudadExpedicionDocumento &&
            x.idDepartamento === clientModel.idDepartamentoExpedicionDocumento,
    );

    if (
        defaultExpeditionCity != null &&
        ciudadExpedicionValue == null &&
        defaultExpeditionCity.id > 0
    ) {
        setCiudadExpedicionValue(defaultExpeditionCity);
    }

    const onChangeExpeditionCity = (event: any, newValue: CITYDTO | null) => {
        setCiudadExpedicionValue(newValue);
        setValue('idCiudadExpedicionDocumento', newValue?.codigo);
        setValue('idDepartamentoExpedicionDocumento', newValue?.idDepartamento);
    };


    const [gradoValue, setGradoValue] = React.useState<GRADEDTO | null>(null)
    const [grados, setGrados] = React.useState<GRADEDTO[]>(catalogState.Grade?.filter(
        (x: GRADEDTO) => x.fuerza === gradoValue?.fuerza
    ))
    const [fuerzaValue, setFuerzaValue] = React.useState<FUERZADTO | null>(null);
    const [categoriaValue, setCategoriaValue] = React.useState<CATEGORIADTO | null>(null);
    const [nullGradoValue, setNullGradoValue] = React.useState(false);
    const [unidadesEjecutoras, setunidadesEjecutora] = React.useState<EXECUTINGUNITDTO[]>(catalogState.ExecutingUnit);

    const defaultGrado = catalogState.Grade?.find(
        (x: GRADEDTO) => x.id === clientModel.idGrado,
    );

    const defaultFuerza = catalogState.Force?.find(
        (x: FUERZADTO) => x.id === clientModel.idFuerza,
    );

    const defaultCategoria = catalogState.categories?.find(
        (x: CATEGORIADTO) => x.id === clientModel.idCategoria,
    );

    const gradosList = catalogState.Grade?.filter(
        (x: GRADEDTO) => x.fuerza === gradoValue?.fuerza
    );
    const fuerzasList = catalogState.Force;
    const categoriasList = catalogState.categories;

    if (defaultGrado != null && gradoValue == null && defaultGrado.id > 0) {
        setGradoValue(defaultGrado);
    }

    if (defaultFuerza != null && fuerzaValue == null && defaultFuerza.id > 0) {
        setFuerzaValue(defaultFuerza);
    }

    if (defaultCategoria != null && categoriaValue == null && defaultCategoria.id > 0) {
        setCategoriaValue(defaultCategoria);
    }

    const onChangeGrado = (event: any, newValue: GRADEDTO | null) => {
        setNullGradoValue(newValue == null)
        setGradoValue(newValue);
        clientModel.idGrado = newValue?.id;
        setValue('idGrado', newValue?.id);
    };

    const onChangeFuerza = (event: any, newValue: FUERZADTO) => {
        setFuerzaValue(newValue);
        clientModel.idFuerza = newValue?.id;
        setValue('idFuerza', newValue?.id);
        const unitsFilter = catalogState.ExecutingUnit.filter(x => x.fuerza === newValue?.id)
        setGrados(catalogState.Grade.filter(x => x.fuerza === newValue?.id));
        const unidadCero: EXECUTINGUNITDTO = {
            id: 0,
            identificacion: '',
            nombre: '',
            sigla: '',
            fecha: '',
            codigo: '',
            tasaAporte: '',
            areaNegocio: '',
            esAval: false,
            fuerza: 0
        };
        setUnidadEjecutoraValue(unidadCero);
        setunidadesEjecutora(unitsFilter);
    };

    const onChangeCategoria = (event: any, newValue: CATEGORIADTO) => {
        setCategoriaValue(newValue);
        clientModel.idCategoria = newValue?.id;
        setValue('idCategoria', newValue?.id);
    };

    const [unidadEjecutoraValue, setUnidadEjecutoraValue] =
        React.useState<EXECUTINGUNITDTO | null>(null);

    const defaultExecutingUnit = catalogState.ExecutingUnit?.find(
        (x: EXECUTINGUNITDTO) => x.id === clientModel.idUnidadEjecutora,
    );

    if (defaultExecutingUnit != null && unidadEjecutoraValue == null) {
        setUnidadEjecutoraValue(defaultExecutingUnit);
    }

    const onChangeExecUnit = (
        event: any,
        newValue: EXECUTINGUNITDTO | null,
    ) => {
        clientModel.idUnidadEjecutora = newValue?.id;
        setUnidadEjecutoraValue(newValue);
        setValue('idUnidadEjecutora', newValue?.id);
    };
    const [clientInfo, setClientInfo] = React.useState(ClientForm);

    const handleClickUpdate = async (data: ClienteDto) => {

        if (loginState.opciones?.filter(x => x.codigo === 'ACTUALIZAHVPERSONAL').length > 0)
            await validatePermisoActualizar(data)
        else {
            NotificationService.genericAlwarning(
                'Advertencia',
                'No cuenta con permisos para realizar esta acción.',
            );
        }
    };

    async function validatePermisoActualizar(data: any) {

        data.lugarNacimientoExtranjero = data.lugarNacimientoExtranjero?.toString();

        setClientInfo(data);
        if (await validateRequiredFields(data)) {
            data.actualiza = 1;
            const user = AuthService.getUserInfo();
            data.usuario = user.USR_USERNAME;
            await startUpdateClient(data, loginState.claims);
            history.push('/HojaDeVida');
        }
        return true;
    }

    const correo = watch('correo');
    const tipoCorreo = watch('idTipoCorreo');
    const idPaisNacimiento = watch('idPaisNacimiento');
    const idPaisResidencia = watch('idPaisResidencia');
    async function validateRequiredFields(data: any) {
        let retBool = false;
        const client = data;
        const domainContains = catalogState.emailDomains.find(
            x => correo?.toLowerCase().includes(x.dominio) && tipoCorreo === x.tipoCorreo,
        );

        if (
            client.fechaNacimiento === null ||
            client.fechaNacimiento === undefined ||
            client.fechaNacimiento === '' ||
            !moment(client.fechaNacimiento).isValid()
        ) {
            toast('Ingrese una fecha de nacimiento válida.');
        } else if (moment(client.fechaNacimiento).isSameOrBefore(moment().subtract(100, 'years'), 'day')) {
            toast('Ingrese una fecha de nacimiento válida.');
        } else if (moment(client.fechaNacimiento).isAfter(moment(), 'day')) {
            toast('Ingrese una fecha de nacimiento válida.');
        } else if (!moment(client.fechaExpedicionDocumento).isValid()) {
            toast('Ingrese una fecha de expedición válida.')
        } else if (moment(client.fechaExpedicionDocumento) < moment(fechaNacimiento).add(16, 'y',)) {
            toast('Ingrese una fecha de expedición válida.')
        } else if (moment(client.fechaExpedicionDocumento) > moment()) {
            toast('Ingrese una fecha de expedición válida.')
        } else if (moment(client.fechaAlta) < moment().subtract(30, 'years')) {
            toast('Ingrese una fecha de alta válida.')
        } else if (moment(client.fechaAlta) > moment()) {
            toast('Ingrese una fecha de alta válida.')
        } else if (moment(client.fechaInscripcion) < moment(client.fechaAlta) && moment(client.fechaInscripcion).year !== moment(client.fechaAlta).year && moment(client.fechaInscripcion).month !== moment(client.fechaAlta).month) {
            toast('Ingrese una fecha de inscripción válida.')
        } else if (moment(client.fechaInscripcion) > moment()) {
            toast('Ingrese una fecha de inscripción válida.')

        } else if (moment(client?.fechaAltaCalculada).year() > 1900 && (moment(client.fechaAltaCalculada) < moment(client.fechaAlta))) {
            toast('Ingrese una fecha de alta calculada válida.')
        } else if (moment(client.fechaAltaCalculada) > moment()) {
            toast('Ingrese una fecha de alta calculada válida.')
        } else if (client.idPaisNacimiento === 0) {
            toast('El campo país de nacimiento es requerido.');
        } else if (client.idPaisNacimiento === 169 && client.idDepartamentoNacimiento === 0) {
            toast('El campo departamento de nacimiento es requerido.');
        } else if (client.idPaisNacimiento === 169 && client.idCiudadNacimiento === 0) {
            toast('El campo ciudad de nacimiento es requerido.');
        } else if (client.fechaNacimiento == null) {
            toast('El fecha de nacimiento es requerido.');
        } else if (client.idTipoIdentificacion === 0) {
            toast('El tipo de identificación es requerido.');
        } else if (client.fechaExpedicionDocumento == null) {
            toast('El fecha de expedición es requerido.');
        } else if (client.idPaisNacimiento === 169 && client.idCiudadExpedicionDocumento === 0) {
            toast('El campo ciudad de expedición es requerido.');
        } else if (client.sexo === '') {
            toast('El campo sexo es requerido.');
        } else if (client.estadoCivil === '') {
            toast('El campo estado civil es requerido.');
        } else if (client.idGrado === 0 || client.idGrado == null) {
            toast('El campo grado es requerido.');
        } else if (client.idGrado === 0 || client.idGrado == null) {
            toast('El campo grado es requerido.');
        } else if (client.idFuerza === 0) {
            toast('El campo fuerza es requerido.');
        } else if (client.idCategoria === 0) {
            toast('El campo categoria es requerido.');
        } else if (client.idUnidadEjecutora === 0 || client.idUnidadEjecutora == null) {
            toast('El campo unidad ejecutora es requerido.');
        } else if (client.fechaInscripcion == null) {
            toast('El campo fecha de inscripción es requerido.');
        } else if (!moment(client.fechaInscripcion).isValid()) {
            toast('Ingrese una fecha de inscripción válida.');
        } else if (client.fechaAlta == null) {
            toast('El campo fecha alta es requerido.');
        } else if (!moment(client.fechaAlta).isValid()) {
            toast('Ingrese una fecha de alta válida.');
        } else {
            retBool = true;
        }

        return retBool;
    }

    return (
        <CHPaper title="Información personal">
            <LoadingComponent key="loader" loading={clientState.loading} />
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="idPaisNacimiento"

                        rules={{
                            required: 'Este campo es requerido',

                        }}
                        control={control}
                        defaultValue={0}
                        render={({ fieldState: { error } }) => (
                            <CHAutoComplete
                                disabled={disabled}
                                value={paisValue}
                                options={catalogState?.countries}
                                getOptionLabel={option => option.nombre}
                                onChange={onChangeCountry}
                                renderInput={params => (
                                    <CHTextField
                                        {...params}
                                        label="País de Nacimiento"
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        className={disabled ? classes.disabledAutocomplete : ''}
                                    />
                                )}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="idDepartamentoNacimiento"
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        control={control}
                        defaultValue={0}
                        render={({ fieldState: { error } }) => (
                            <CHAutoComplete
                                value={paisValue?.id === 1 ? departamentoValue : null}
                                disabled={disabled ? true : paisValue?.id !== 1}
                                options={catalogState?.departments}
                                getOptionLabel={option => option.nombre}
                                onChange={onChangeDepartment}
                                renderInput={params => (
                                    <CHTextField
                                        isReadOnly={disabled}
                                        {...params}
                                        label="Departamento de Nacimiento"
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        className={disabled ? classes.disabledAutocomplete : ''}
                                    />
                                )}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    {paisValue?.codigo === 169 ? (
                        <Controller
                            name="idCiudadNacimiento"
                            control={control}
                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    disabled={disabled}
                                    value={
                                        ciudadNacimientoValue
                                    }
                                    options={ciudadNacimientoValues}
                                    getOptionLabel={option => option.nombre}
                                    onChange={onChangeCity}
                                    renderInput={params => (
                                        <CHTextField
                                            isReadOnly={disabled}
                                            {...params}
                                            label="Ciudad de Nacimiento"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            className={disabled ? classes.disabledAutocomplete : ''}
                                        />
                                    )}
                                />
                            )}
                        />
                    ) : (
                        <Controller
                            name="lugarNacimientoExtranjero"
                            control={control}
                            defaultValue=""
                            render={({
                                field: { onChange },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={disabled}
                                    label="Ciudad de Nacimiento"
                                    value={lugarNacimientoExtranjero}
                                    onChange={onChange}
                                />
                            )}
                        />
                    )}
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>

                    <Controller
                        name="fechaNacimiento"
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHDatePicker

                                readOnly={disabled}
                                label="Fecha de Nacimiento"
                                onChange={onChange}
                                value={value}
                                invalidDateMessage="Fecha inválida"
                                minDate={moment().subtract(100, 'years')}
                                minDateMessage={`La fecha debe estar entre ${moment()
                                    .subtract(100, 'y')
                                    .format('YYYY-MM-DD')} y ${moment().format(
                                        'YYYY-MM-DD',
                                    )}`}
                                maxDate={new Date()}
                                maxDateMessage="La fecha de nacimiento no debe ser mayor a la actual"
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly
                                label="Tipo de Identificación"
                                value={catalogState?.IdentificationType?.find(
                                    x => x.id === clientModel.idTipoIdentificacion
                                )?.descripcion}
                                InputLabelProps={{ shrink: true }}
                            />
                        )}
                        name="idTipoIdentificacion"
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    {
                        idTipoIdentificacion === 1 || idTipoIdentificacion === 2 ? (
                            <Controller
                                name="identificacion"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                    minLength: {
                                        value: 4,
                                        message:
                                            'La longitud mínima del campo es de 4 caracteres',
                                    },
                                    maxLength: {
                                        value: 12,
                                        message:
                                            'La longitud máxima del campo es de 12 caracteres',
                                    },
                                }}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        label="Número de Identificación"
                                        value={value}
                                        onChange={onChange}
                                        type="number"
                                    />
                                )}
                            />
                        ) : (
                            <Controller
                                name="identificacion"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                    minLength: {
                                        value: 4,
                                        message:
                                            'La longitud mínima del campo es de 4 caracteres',
                                    },
                                    maxLength: {
                                        value: 12,
                                        message:
                                            'La longitud máxima del campo es de 12 caracteres',
                                    },
                                }}
                                defaultValue=""
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={disabled}
                                        label="Número de Identificación"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        )
                    }
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    {tipoIdentificacion ? (
                        <Controller
                            name="fechaExpedicionDocumento"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    readOnly={disabled}
                                    label="Fecha de Expedición"
                                    onChange={onChange}
                                    value={value}
                                    minDate={moment(fechaNacimiento).add(
                                        16,
                                        'y',
                                    )}
                                    minDateMessage={`La fecha de expedición debe ser posterior a ${moment(
                                        fechaNacimiento,
                                    )
                                        .add(16, 'y')
                                        .format('YYYY-MM-DD')}`}
                                    invalidDateMessage="Fecha inválida"
                                    maxDate={new Date()}
                                    maxDateMessage="La fecha de expedición no debe ser mayor a la actual"
                                />
                            )}
                        />
                    ) : (
                        <Controller
                            name="fechaExpedicionDocumento"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha de Expedición"
                                    onChange={onChange}
                                    value={value}
                                    minDate={moment(fechaNacimiento)}
                                    minDateMessage="La fecha de expedición debe ser mayor a la fecha de nacimiento"
                                    invalidDateMessage="Fecha inválida"
                                    maxDate={new Date()}
                                    maxDateMessage="La fecha de expedición no debe ser mayor a la actual"
                                    readOnly={disabled}
                                />
                            )}
                        />
                    )}
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    {clientModel.idPaisExpedicionDocumento === 169 || clientModel.idPaisNacimiento === 169 ? (
                        <Controller
                            name="idCiudadExpedicion"
                            control={control}
                            defaultValue={0}
                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    disabled={disabled}
                                    value={ciudadExpedicionValue}
                                    options={catalogState?.City}
                                    getOptionLabel={option => option.nombre}
                                    onChange={onChangeExpeditionCity}
                                    renderInput={params => (
                                        <CHTextField
                                            isReadOnly={disabled}
                                            {...params}
                                            label="Ciudad de Expedición"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            className={disabled ? classes.disabledAutocomplete : ''}
                                        />
                                    )}
                                />
                            )}
                        />
                    ) : (
                        <Controller
                            name="lugarExpedicionExtranjero"
                            control={control}
                            defaultValue=""
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={disabled}
                                    label="Lugar de Expedición"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                />
                            )}
                        />
                    )}
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="primerNombre"
                        rules={{
                            required: {
                                value: true,
                                message: 'Este campo es requerido.',
                            },
                            pattern: /^[ A-zÀ-ú ]+$/g
                        }}
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                isReadOnly={disabled}
                                error={!!error}
                                helperText={error ? 'El nombre solo debe contener letras' : null}

                                label="Primer Nombre"
                                value={value}
                                onChange={onChange}
                                inputProps={{ pattern: '' }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="segundoNombre"
                        control={control}
                        rules={{
                            pattern: /^[ A-zÀ-ú ]+$/g
                        }}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? 'El nombre solo debe contener letras' : null}
                                isReadOnly={disabled}
                                label="Segundo Nombre"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="primerApellido"
                        rules={{
                            required: {
                                value: true,
                                message: 'Este campo es requerido.',
                            },
                            pattern: /^[ A-zÀ-ú ]+$/g
                        }}
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? 'El apellido debe contener solo letras' : null}
                                isReadOnly={disabled}
                                label="Primer Apellido"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="segundoApellido"
                        rules={{
                            pattern: /^[ A-zÀ-ú ]+$/g
                        }}
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? 'El apellido debe contener solo letras' : null}
                                isReadOnly={disabled}
                                label="Segundo Apellido"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHSelect
                                readOnly={disabled}
                                label="Sexo"
                                onChange={onChange}
                                value={value}
                            >
                                {catalogState.sextypes?.map(
                                    (option: SEXODTO) => (
                                        <MenuItem
                                            key={option.codigo}
                                            value={option.codigo}
                                        >
                                            {option.descripcion}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        )}
                        name="sexo"
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHSelect
                                defaultValue=""
                                label="Estado Civil"
                                value={value}
                                onChange={onChange}
                                readOnly={disabled}
                            >
                                {catalogState.civilstates?.map(
                                    (option: any) => (
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
                        name="estadoCivil"
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHSelect
                                defaultValue=""
                                label="Ocupación"
                                value={value}
                                onChange={onChange}
                                readOnly={disabled}
                            >
                                {catalogState.professions?.map(
                                    (option: any) => (
                                        <MenuItem
                                            key={option.id}
                                            value={option.id}
                                        >
                                            {option.descripcion.toUpperCase()}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        )}
                        name="idProfesion"
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        control={control}
                        name="idGrado"
                        render={() => (
                            <CHAutoComplete
                                disabled={disabled}
                                value={nullGradoValue ? null : gradoValue}
                                options={grados}
                                getOptionLabel={option => option.descripcion}
                                onChange={onChangeGrado}
                                renderInput={params => (
                                    <CHTextField isReadOnly={disabled} {...params} label="Grado"
                                        className={disabled ? classes.disabledAutocomplete : ''}
                                    />
                                )}
                            />)}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        control={control}
                        name="idFuerza"
                        render={() => (
                            <CHAutoComplete
                                disabled={disabled}
                                value={fuerzaValue}
                                options={fuerzasList}
                                getOptionLabel={option => option.descripcion}
                                onChange={onChangeFuerza}
                                renderInput={params => (
                                    <CHTextField isReadOnly={disabled} {...params} label="Fuerza"
                                        className={disabled ? classes.disabledAutocomplete : ''}
                                    />
                                )}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        control={control}
                        name="idCategoria"
                        render={() => (
                            <CHAutoComplete
                                disabled={disabled}
                                value={categoriaValue}
                                options={categoriasList}
                                getOptionLabel={option => option.descripcion}
                                onChange={onChangeCategoria}
                                renderInput={params => (
                                    <CHTextField isReadOnly={disabled} {...params} label="Categoría"
                                        className={disabled ? classes.disabledAutocomplete : ''}
                                    />
                                )}
                            />)}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <CHAutoComplete
                        disabled={disabled}
                        value={unidadEjecutoraValue}
                        options={unidadesEjecutoras}
                        getOptionLabel={option => option.nombre}
                        onChange={onChangeExecUnit}
                        renderInput={params => (
                            <CHTextField isReadOnly={disabled} {...params} label="Unidad Ejecutora"
                                className={disabled ? classes.disabledAutocomplete : ''}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fechaAlta"
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHDatePicker
                                readOnly={disabled}
                                label="Fecha Alta"
                                onChange={onChange}
                                value={value}
                                maxDate={new Date()}
                                maxDateMessage="La fecha de alta no debe ser mayor a la actual"
                                invalidDateMessage="Fecha inválida"

                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fechaInscripcion"
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHDatePicker
                                label="Fecha Inscripción"
                                onChange={onChange}
                                value={value}
                                minDate={moment(fechaAlta).startOf('month')}
                                // minDateMessage="La fecha de inscripción debe ser mayor a la fecha de alta"
                                maxDate={new Date()}
                                // maxDateMessage="La fecha de inscripción no debe ser mayor a la actual"
                                // invalidDateMessage="Fecha inválida"
                                readOnly={disabled}
                            />
                        )}
                    />
                </Grid>
                {categoriaSoldado &&
                    moment(clientModel.fechaAltaCalculada).year() > 1900 ? (
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaAltaCalculada"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    disabled
                                    label="Fecha Alta Calculada"
                                    onChange={onChange}
                                    value={value}
                                    invalidDateMessage="Fecha inválida"
                                    minDate={moment(fechaAlta)}
                                    minDateMessage="La fecha de alta calculada debe ser mayor a la fecha de alta"
                                    maxDate={new Date()}
                                    maxDateMessage="La fecha de alta calculada no debe ser mayor a la actual"
                                />
                            )}
                        />
                    </Grid>
                ) : null}
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="validacionBiometrica"
                        control={control}

                        render={({ field: { onChange, value } }) => (
                            <CHCheckbox
                                readOnly
                                label="Validación biométrica"
                                onChange={onChange}
                                checked={value}
                                disabled
                            />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="estadoCliente"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Estado Cliente"
                                value={clientState?.client?.estadoCliente}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tasaAportes"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Tasa aportes"
                                value={value?.toFixed(2)}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tasaVoluntariaAportes"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Tasa voluntaria aportes"
                                value={value?.toFixed(2)}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fechaPrimeraCuota"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Fecha primera cuota"
                                value={UtilService.getDate(value)}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />

                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fechaUltimaCuota"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Fecha última cuota"
                                value={UtilService.getDate(value)}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />

                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="numeroCuotas"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Número de cuotas"
                                value={value}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="codigoMilitar"

                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}

                                isReadOnly={disabled}
                                label="Código Militar"
                                value={value}
                                onChange={onChange}

                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="situacionActual"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Situación actual"
                                value={value}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHSelect
                                defaultValue=""
                                label="Tipo Vinculación"
                                value={value}
                                onChange={onChange}
                                readOnly={disabled}
                            >
                                {clientState.client?.tiposVinculacion?.map(
                                    (option: any) => (
                                        <MenuItem
                                            key={option.idTipoActor}
                                            value={option.idTipoActor}
                                        >
                                            {option.descripcionTipoActor}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        )}
                        name="idTipoActor"
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="regimen"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Régimen"
                                value={value}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="numeroValidacionVigia"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue="ACTUALIZADO"
                                label="Número de validación inspector"
                                value={value}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tasaSalarioBase"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Tasa salario base"
                                value={value?.toFixed(2)}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="valorSubsidio"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Valor estimado del subsidio"
                                value={clientModel.numeroCuotas !== 168 ? 'EL AFILIADO NO CUMPLE CON LOS REQUISITOS PARA EL SUBSIDIO DE VIVIENDA' : UtilService.formatCurrency(value)}
                                InputLabelProps={{
                                    shrink: true,
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fechaActualizaDatos"
                        control={control}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                isReadOnly
                                defaultValue=""
                                label="Fecha actualización de datos"
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
                    variant="outlined"
                    type="submit"
                    onClick={handleSubmit(handleClickUpdate)}
                >
                    GUARDAR
                </Button>

            </div>
        </CHPaper>

    );
}
const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
});
const mapDispatchToProps = {
    startUpdateClient,
    startValidatePermisoActualizacion
};
export default connect(mapStateToProps, mapDispatchToProps)(PersonalInformation);
