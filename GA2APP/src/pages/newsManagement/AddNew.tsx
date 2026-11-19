import React, { useEffect, useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Button,
    Box,
} from '@material-ui/core';
import { useHistory, useLocation } from 'react-router-dom';
import { connect } from 'react-redux';
import { Controller, useForm, UseFormReturn, Validate } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import swal from 'sweetalert';
import { AppState } from '../../store/rootReducer';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHSelect from '../../components/ch-select/CHSelect';
import { startGetAllIdentificationTypes } from '../../actions/catalog.action';
import {
    IDENTIFICATIONTYPEDTO,
} from '../../types/catalog.type';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import UtilService from '../../utils/util.service';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import { ClientState } from '../../reducers/client.reducer';
import ApiClients from '../../services/api-clients.service';
import CatalogApi from '../../services/api-catalog.service';
import ApiSchedule from '../../services/api-cronograma.service';
import ApiNovedades from '../../services/api-novedades.service';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import AuthService from '../../services/auth.service';
import { startGetExecutingUnitsSchedule } from '../../actions/schedule.action';
import { ScheduleState } from '../../reducers/schedule.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';



const useStyles = makeStyles((theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        button: {
            width: '60%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        actions: {
            marginLeft: '2rem',
            width: '20%',
        },
    }),
);

function agregarNovedad({
    catalogState,
    scheduleState,
    startGetExecutingUnitsSchedule
}: {
    catalogState: CatalogState;
    scheduleState: ScheduleState;
    startGetExecutingUnitsSchedule: () => void;
}) {
    const classes = useStyles();
    const api = ApiClients();
    const apiConcepto = CatalogApi();
    const apiCronograma = ApiSchedule();
    const apiNov = ApiNovedades();
    let idTipoIdentificacion = 0;
    let numeroDocumento = '';
    let idModulo = 1;
    let token = '';
    let idUnidadEjecutora = 0;
    let observaciones = '';

    const location = useLocation<{ datoPrimario: any, tipoIdentific: number, tipoIdentificacion: number, numeroIdentificacion: any }>();
    const { tipoIdentific, datoPrimario, tipoIdentificacion, numeroIdentificacion } = location.state;

    


    const { control, watch, handleSubmit, setValue, reset, getValues } = useForm({
        defaultValues: {
            idTipoIdentificacion: 0,
            identificacion: '',
            nombreCliente: '',
            codigoMilitar: '',
            idUnidadEjecutora: 0,
            idConceptoNovedad: 0,
            codigoAlta: 0,
            codigoBaja: 0,
            fechaInicioNovedad: new Date(),
            fechaFinNovedad: new Date(),
            periodo: '',
            porcentaje: 0,
            valor: 0,
            esAlta: false,
            esBaja: false,
            observaciones: '',
            token: '',
        }
    })

    const identificationType = watch('idTipoIdentificacion');
    const identification = watch('identificacion');
    const history = useHistory();


    const validateDate: Validate<Date> = (value: Date) => {
        const fechaInicioNovedad = watch("fechaInicioNovedad");
        const date = moment(value);
        const fechaFinNovedad = moment(fechaInicioNovedad).add(2, 'M');
        const fechaInicioMenosDosMeses = moment(fechaInicioNovedad).subtract(2, 'M');

        let message = '';
        if (!date.isValid()) {
            message = 'Ingrese una fecha válida';
        } else if (value > fechaFinNovedad.toDate()) {
            message = 'Por favor no ingrese una fecha mayor a dos meses';
        } else if (value < fechaInicioMenosDosMeses.toDate()) {
            message = 'Por favor ingrese una fecha menor a dos meses';
        }

        return message !== '' ? message : true;
    };

    const validateEndDate: Validate<Date> = (value: Date) => {
        const fechaInicioNovedad = watch("fechaInicioNovedad");
        const date = moment(value);
        const fechaActual = moment();

        let message = '';
        if (value > fechaActual.toDate()) {
            message = "La fecha debe ser menor a la fecha actual";
        } else if (value < fechaInicioNovedad) {
            message = "La fecha debe ser menor a la fecha inicial";
        }

        return message !== '' ? message : true;
    };

    const handleOnClickRegresar = () => {
        const identificationValue = identification || datoPrimario || numeroIdentificacion;
        history.push(`/Gestion?edit=true&idTipoIdentificacion=${identificationType}&identificacion=${identificationValue}`);
    }



    const [products, setProducts] = useState<any[]>([]);
    const [novedades, setNovedades] = useState<any[]>([]);
    const [fechaInicio, setFechaInicio] = useState<Date>();
    const [selectedConcepto, setSelectedConcepto] = useState(null);
    const [selectedUnidadEjecutora, setSelectedUnidadEjecutora] = useState(null);
    const [shouldValidate, setShouldValidate] = useState(true);
    const [isLoading, setIsLoading] = useState(false);
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<any[]>([]);


    const obtenerConceptos = async () => {
        await apiConcepto.ObtenerConceptosNovedades().then(async response => {
            if (response) {
                if (response.isSuccess && response.data) {
                    setProducts(response.data);
                }
            }
        });
    }

    useEffect(() => {
        obtenerConceptos()
        startGetExecutingUnitsSchedule()
        setFechaInicio(new Date())

    }, []);


    const consultarCliente = async (nuevoValor: any) => {
        setIsLoading(true);

        const identificationValue = nuevoValor || datoPrimario || numeroIdentificacion;
        const tipoDeIdentificacion = location.state.tipoIdentific || location.state.tipoIdentificacion || tipoIdentific || tipoIdentificacion || watch('idTipoIdentificacion');

        try {
            const response = await api.getClientByIdentificationTypeIdentificationNumberRequest(
                tipoDeIdentificacion,
                identificationValue
            );

            if (response.isSuccess && response.data != null) {
                const cliente = response.data;

                setValue('nombreCliente', `${cliente.primerNombre} ${cliente.segundoNombre ?? ''} ${cliente.primerApellido} ${cliente.segundoApellido ?? ''}`);
                setValue('idUnidadEjecutora', cliente.idUnidadEjecutora);
                setValue('codigoMilitar', cliente.codigoMilitar || '');

                const novedadesResponse = await apiCronograma.getExecutingUnitsSchedule();
                const unidades = novedadesResponse?.data || [];
                setUnidadesEjecutoras(unidades);

                const responseCronograma = await apiCronograma.ObtenerNovedadesUnidadEjecutora(cliente.idUnidadEjecutora);

                if (responseCronograma.isSuccess) {
                    setNovedades(responseCronograma.data);

                    const newProducts = products.filter((product) =>
                        responseCronograma.data.some((element: any) => element.idConceptoNovedad === product.idConceptoNovedad)
                    );

                    setProducts(newProducts);
                }
            } else {
                toast('El afiliado no existe');
            }
        } catch (error) {
            toast('Hubo un error al cargar la información del cliente');
        } finally {
            setIsLoading(false);
        }
    };



    useEffect(() => {
        if (tipoIdentific !== null || tipoIdentificacion !== null) {
            const selectedType = catalogState.IdentificationType.find(option => option.id === tipoIdentific || tipoIdentificacion);
            if (selectedType) {
                setValue('idTipoIdentificacion', selectedType.id);
                swal({
                    text: 'Para cargar los datos dar clic en el campo Número de identificación y luego por fuera de el.',
                    icon: 'warning',
                });
            }
        }

    }, [catalogState.IdentificationType, tipoIdentific, tipoIdentificacion]);


    const guardarLog = async (tipoDocumento: any, nDocumento: any, fechaInicio: any, idUnidadEjecutora: number, modulo: string) => {
        const user = AuthService.getUserInfo();
        await apiCronograma.InsertarAudoriaNovedades({
            idModulo: 1,
            modulo: modulo,
            tipoDocumento: tipoDocumento,
            nDocumento: nDocumento,
            detalleModificacion: `Se ha insertado una novedad para el afiliado con número de identificación ${numeroDocumento}`,
            fechaModificacion: new Date(),
            usuarioModificacion: user.USR_ID,
            fechaInicio: fechaInicio,
            fechaFin: new Date(),
            token: token,
            idUnidadEjecutora: idUnidadEjecutora,
        });

    };


    const verificarNovedadExistente = async (
        idTipoIdentificacion: number,
        identificacion: string,
        idConceptoNovedad: number,
        esAlta: boolean,
        esBaja: boolean
    ): Promise<boolean> => {
        try {
            const response = await apiNov.getObtenerNovedadesAfiliado(
                idTipoIdentificacion,
                identificacion
            );

            if (response.isSuccess) {
                const novedades = response.data;
                return novedades.some((novedad: any) =>
                    novedad.idConceptoNovedad === idConceptoNovedad &&
                    (
                        (novedad.esAlta === esAlta || novedad.esBaja === esBaja) ||

                        (novedad.idConceptoNovedad !== idConceptoNovedad)
                    )
                );
            }

            return false;
        } catch (error) {
            return false;
        }
    };


    const guardarNovedad = async (data: any) => {


        data.idConceptoNovedad = idConcepto;
        data.esAlta = esAltaChecked;
        data.esBaja = esBajaChecked;
        idTipoIdentificacion = data.idTipoIdentificacion;
        numeroDocumento = data.identificacion;
        idModulo = data.idModulo;
        token = data.token;
        observaciones = data.observaciones;
        idUnidadEjecutora = data.idUnidadEjecutora;

        if (!data.esAlta && !data.esBaja) {
            toast('Por favor marque uno de los checkboxes');
            return;
        }

        const novedadExistente = await verificarNovedadExistente(data.idTipoIdentificacion, data.identificacion, data.idConceptoNovedad, data.esAlta, data.esBaja);

        if (novedadExistente) {
            let alerta = 'Este afiliado ya tiene una novedad con el mismo concepto';
            if (esAltaChecked) {
                alerta += ' en Alta';
            } else if (esBajaChecked) {
                alerta += ' en Baja';
            }
            toast(alerta);
        } else {
            apiCronograma.InsertarNovedadUsuario(data)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                            toast('Novedad insertada correctamente');

                            await guardarLog(idTipoIdentificacion, numeroDocumento, fechaInicio, idUnidadEjecutora, 'GESTIÓN DE NOVEDADES');

                            history.push('/Gestion?edit=true')
                        } else {
                            toast('La novedad no se ingreso correctamente');
                        }
                    }
                });
        }
    };


    const handleConceptoChange = (event: { target: { value: any; }; }) => {
        const selectedValue = event.target.value;
        setSelectedConcepto(selectedValue);
    };

    const handleUnidadEjecutoraChange = (event: { target: { value: any; }; }) => {
        const selectedValue = event.target.value;
        setSelectedUnidadEjecutora(selectedValue);
    };

    const [idConcepto, setIdConcepto] = useState(0);
    const [esAltaChecked, setEsAltaChecked] = useState(false);
    const [esBajaChecked, setEsBajaChecked] = useState(false);
    const [showTokenField, setShowTokenField] = useState(false);
    const [userModified, setUserModified] = useState(false);

    useEffect(() => {
        setValue('codigoAlta', novedades.find(x => x.idConceptoNovedad === watch('idConceptoNovedad'))?.codigoNovedadAlta)
        setValue('codigoBaja', novedades.find(x => x.idConceptoNovedad === watch('idConceptoNovedad'))?.codigoNovedadBaja)
        setIdConcepto(novedades.find(x => x.idConceptoNovedad === watch('idConceptoNovedad'))?.idConceptoNovedad)
    }, [watch('idConceptoNovedad')])


    useEffect(() => {
        setValue('fechaInicioNovedad', scheduleState.executingUnitsSchedule?.find(x => x.idUnidadEjecutora === watch('idUnidadEjecutora'))?.fechaInicio as Date)
        setValue('fechaFinNovedad', scheduleState.executingUnitsSchedule?.find(x => x.idUnidadEjecutora === watch('idUnidadEjecutora'))?.fechaFin as Date)
        setValue('periodo', scheduleState.executingUnitsSchedule?.find(x => x.idUnidadEjecutora === watch('idUnidadEjecutora'))?.periodo as any)
    }, [watch('idUnidadEjecutora')])



    const handleEsAltaChange = () => {
        setEsAltaChecked(!esAltaChecked);
        setEsBajaChecked(esAltaChecked);
    }

    const handleEsBajaChange = () => {
        setEsAltaChecked(esBajaChecked);
        setEsBajaChecked(!esBajaChecked);
    }

    const porcentajes = [0, 2, 3, 4.5, 7, 8, 10];
    const porcentajesSinCero = [2, 3, 4.5, 7, 8, 10];
    const idUnidadEjecutoraValue = watch("idUnidadEjecutora");
    const idConceptoNovedad = watch("idConceptoNovedad")
    const codigoAltaValue = watch('codigoAlta');
    const codigoAltaValueAsString = codigoAltaValue?.toString() ?? '';

    useEffect(() => {
        const selectedOption = idUnidadEjecutoraValue;
        const shouldShowTokenField = idConceptoNovedad === 4 && (selectedOption === 3 || selectedOption === 6 || selectedOption === 22);
        setShowTokenField(shouldShowTokenField);
    }, [idUnidadEjecutoraValue, idConceptoNovedad]);



    return (
        <>
            <CHPaper title="Información novedad">
                <LoadingComponent key="loader" loading={isLoading} />
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="idTipoIdentificacion"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Tipo de Identificación*"
                                    value={(userModified ? value : tipoIdentificacion || tipoIdentific)}
                                    onChange={(e) => {
                                        setUserModified(true);
                                        onChange(e.target.value);
                                    }}
                                >
                                    {catalogState.IdentificationType?.map((option: IDENTIFICATIONTYPEDTO) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="identificacion"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                                minLength: {
                                    value: 4,
                                    message: 'La longitud mínima del campo es de 4 caracteres',
                                },
                                maxLength: {
                                    value: 12,
                                    message: 'La longitud máxima del campo es de 12 caracteres',
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
                                    isReadOnly={false}
                                    label="Número de Identificación*"
                                    value={(userModified ? value : datoPrimario || numeroIdentificacion) || ''}
                                    onChange={(event) => {
                                        setUserModified(true);
                                        const inputValue = event.target.value;
                                        if (identificationType === 1 || identificationType === 2) {

                                            const numericValue = inputValue.replace(/\D/g, '');
                                            onChange(numericValue);
                                        } else {

                                            onChange(inputValue);
                                        }
                                    }}
                                    onBlur={async () => {
                                        if (shouldValidate) {
                                            const newValue = value || datoPrimario || numeroIdentificacion;
                                            setValue('identificacion', newValue);
                                            await consultarCliente(newValue);
                                        }
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreCliente"
                            control={control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            render={({
                                field: { onChange, value },

                            }) => (
                                <CHTextField
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Nombres y Apellidos"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="codigoMilitar"
                            control={control}
                            rules={{
                                pattern: {
                                    value: /^[a-zA-Z0-9]*$/,
                                    message: 'El código militar puede ser alfanumérico.',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Código Militar"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="idUnidadEjecutora"
                            control={control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            render={({ field: { onChange, value }, fieldState: { error }, }) => (
                                <CHSelect
                                    label="Unidad Ejecutora*"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={(event) => {
                                        onChange(event);
                                        handleUnidadEjecutoraChange(event);
                                    }}
                                >
                                    {unidadesEjecutoras?.map((novedad: { idUnidadEjecutora: any; nombreUnidadEjecutora: any; }) => (
                                        <MenuItem key={novedad.idUnidadEjecutora} value={novedad.idUnidadEjecutora}>
                                            {novedad.nombreUnidadEjecutora}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name='idConceptoNovedad'
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Concepto*"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={(event) => {
                                        onChange(event);
                                        handleConceptoChange(event);
                                    }}
                                >
                                    {products?.map((product) => (
                                        <MenuItem key={product.idConceptoNovedad} value={product.idConceptoNovedad}>
                                            {product.descripcionConceptoNovedad}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="codigoAlta"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Código Alta"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    {selectedConcepto !== 3 && (
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="codigoBaja"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Código Baja"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="porcentaje"
                            control={control}
                            rules={{
                                maxLength: {
                                    value: 2,
                                    message:
                                        'La longitud máxima del campo es de 2 caracteres',
                                },
                                pattern: {
                                    value: /^(?:0%|(?:[1-9]\d{0,2}(?:\.\d{1,2})?%))$/,
                                    message: 'Ingresa un porcentaje válido',
                                },
                                validate: (value) => {
                                    const parsedValue = parseFloat(value.toString().replace('%', ''));
                                    if (parsedValue > 10) {
                                        return 'No se permite más del 10%';
                                    }
                                    return true;
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Porcentaje %"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {((codigoAltaValueAsString === '206' || codigoAltaValueAsString === '210') && esAltaChecked)
                                        ? porcentajesSinCero.map((porcentaje) => (
                                            <MenuItem key={porcentaje} value={porcentaje}>
                                                {porcentaje} %
                                            </MenuItem>
                                        ))
                                        : porcentajes.map((porcentaje) => (
                                            <MenuItem key={porcentaje} value={porcentaje}>
                                                {porcentaje} %
                                            </MenuItem>
                                        ))}
                                </CHSelect>

                            )
                            }
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaInicioNovedad"
                            rules={{ validate: validateDate }}
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : ""}
                                    label="Fecha Inicial"
                                    onChange={onChange}
                                    value={UtilService.getDate(value)}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>

                        <Controller
                            name="fechaFinNovedad"
                            rules={{ validate: validateEndDate }}
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : ""}
                                    label="Fecha Final"
                                    onChange={onChange}
                                    value={UtilService.getDate(value)}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly
                                />
                            )}
                        />

                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="periodo"
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHDatePicker
                                    label="Período"
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    views={["month", "year"]}
                                    format="MM/yyyy"
                                    value={value}
                                    disabled
                                    invalidDateMessage="Fecha inválida"
                                />
                            )}
                        />
                    </Grid>

                    {showTokenField && (
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="token"
                                control={control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        InputLabelProps={{ shrink: true }}
                                        label="Token"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="esAlta"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHCheckbox
                                    label="Es de alta"
                                    checked={esAltaChecked}
                                    onChange={handleEsAltaChange}
                                />
                            )}
                        />
                    </Grid>
                    {selectedConcepto !== 3 && (
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="esBaja"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHCheckbox
                                        label="Es de baja"
                                        checked={esBajaChecked}
                                        onChange={handleEsBajaChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}

                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="observaciones"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    multiline
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                            overflowY: 'auto',
                                        },
                                        minLength: 200,
                                    }}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    label="Observaciones"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid item xs={12}>
                    <Box display="flex" justifyContent="center">
                        <Button
                            className={classes.actions}
                            variant="contained"
                            color="primary"
                            onClick={() => {
                                setShouldValidate(false);
                                handleSubmit(guardarNovedad)();
                                setShouldValidate(true);
                            }}
                        >
                            GUARDAR
                        </Button>

                        <Button
                            className={classes.actions}
                            variant="contained"
                            color='default'
                            onClick={handleOnClickRegresar}
                        >
                            REGRESAR
                        </Button>
                    </Box>
                </Grid>
            </CHPaper>
        </>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    clientState: ClientState;
    catalogState: CatalogState;
    formularioEmbargos: UseFormReturn<any>;
    tiposIdentificacion: IDENTIFICATIONTYPEDTO[];


}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
    loginState: state.login,
    scheduleState: state.schedule,
});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetClientByIdentificationTypeIdentificationNumber,
    startGetExecutingUnitsSchedule,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(agregarNovedad);