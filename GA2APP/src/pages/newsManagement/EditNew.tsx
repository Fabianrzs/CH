import React, { useEffect, useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Button,
    Box,
} from '@material-ui/core';
import { useHistory } from 'react-router-dom';
import { connect } from 'react-redux';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
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
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import { ClientState } from '../../reducers/client.reducer';
import ApiClients from '../../services/api-clients.service';
import CatalogApi from '../../services/api-catalog.service';
import ApiSchedule from '../../services/api-cronograma.service';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import ApiNovedades from '../../services/api-novedades.service';
import AuthService from '../../services/auth.service';
import { ScheduleState } from '../../reducers/schedule.reducer';
import { startGetExecutingUnitsSchedule } from '../../actions/schedule.action';
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

function editarNovedad({
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
    let idTipoIdentificacion = 0;
    let numeroDocumento = '';
    let idModulo = 1;
    let token = '';
    let idUnidadEjecutora = 0;
    let idConceptNovedad = 0;


    const { control, watch, handleSubmit, setValue, reset } = useForm({
        defaultValues: {
            idNovedad: 0,
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

    const validateDate = (data: Date) => {
        let message = '';
        if (data > new Date()) {
            message = "La fecha debe ser menor a la fecha actual"
        }

        return message !== '' ? message : true;
    }

    const validateEndDate = (data: Date) => {
        let message = '';
        if (data > new Date()) {
            message = "La fecha debe ser menor a la fecha actual"
        } else if (data < new Date(watch('fechaInicioNovedad'))) {
            message = "La fecha debe ser menor a la fecha inicial"
        }
        return message !== '' ? message : true;
    }

    const today = new Date();
    const history = useHistory();
    
    

    const [products, setProducts] = useState<any[]>([]);
    const apiNovedades = ApiNovedades();
    const [novedades, setNovedades] = useState<any[]>([]);
    const identificationType = watch('idTipoIdentificacion');
    const identification = watch('identificacion');
    const [fechaInicio, setFechaInicio] = useState<Date>();
    const [esAltaChecked, setEsAltaChecked] = useState(false);
    const [esBajaChecked, setEsBajaChecked] = useState(false);
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<any[]>([]);
    const [selectedConcepto, setSelectedConcepto] = useState(null);
    const [selectedUnidadEjecutora, setSelectedUnidadEjecutora] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [shouldValidate, setShouldValidate] = useState(true);


    const porcentajes = [0, 2, 3, 4.5, 7, 8, 10];
    const porcentajesSinCero = [2, 3, 4.5, 7, 8, 10];
    const codigoAltaValue = watch('codigoAlta');
    const codigoAltaValueAsString = codigoAltaValue?.toString() ?? '';
    const idUnidadEjecutoraValue = watch("idUnidadEjecutora");
    const showTokenField =
        (idUnidadEjecutoraValue === 3 || idUnidadEjecutoraValue === 6 || idUnidadEjecutoraValue === 22) &&
        idConceptNovedad === 13;


    const obtenerConceptos = async () => {
        await apiConcepto.ObtenerConceptosNovedades().then(async response => {
            if (response) {
                if (response.isSuccess && response.data) {
                    setProducts(response.data);
                }
            }
        });
    }


    const consultarCliente = async () => {
        setIsLoading(true);

        try {
            const response = await api.getClientByIdentificationTypeIdentificationNumberRequest(
                watch('idTipoIdentificacion'),
                watch('identificacion')
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

                    if (newProducts.length > 0) {
                        setProducts(newProducts);
                    }


                }
            } else {
                toast('El afiliado no existe');
            }
        } catch (error: any) {
            toast.error(`Hubo un error al cargar la información del cliente ${error.message}`, { type: toast.TYPE.ERROR });
        } finally {
            setIsLoading(false);
        }
    };

    const obtenerNovedad = async () => {
        const id = window.location.href.split('=')[1];
        await apiCronograma.ObtenerNovedadAfiliadoPorId(+id)
            .then((response: { isSuccess: any; data: { idTipoIdentificacion: number; identificacion: string; idUnidadEjecutora: number; idConceptoNovedad: number; codigoAlta: number; codigoBaja: number; fechaInicioNovedad: Date; fechaFinNovedad: Date; porcentaje: number; esAlta: boolean | ((prevState: boolean) => boolean); esBaja: boolean | ((prevState: boolean) => boolean); observaciones: string; token: string; codigoNovedadAlta: number; codigoNovedadBaja: number }; }) => {
                if (response) {

                    if (response.isSuccess) {
                        setValue('idTipoIdentificacion', response.data.idTipoIdentificacion)
                        setValue('identificacion', response.data.identificacion)
                        consultarCliente()
                        setValue('idUnidadEjecutora', response.data.idUnidadEjecutora)
                        setValue('idConceptoNovedad', response.data.idConceptoNovedad)
                        setValue('codigoAlta', response.data.codigoNovedadAlta)
                        setValue('codigoBaja', response.data.codigoNovedadBaja)
                        setValue('fechaInicioNovedad', response.data.fechaInicioNovedad)
                        setValue('fechaFinNovedad', response.data.fechaFinNovedad)
                        setValue('porcentaje', response.data.porcentaje)
                        setEsAltaChecked(response.data.esAlta)
                        setEsBajaChecked(response.data.esBaja)
                        setValue('idNovedad', +id)
                        setValue('observaciones', response.data.observaciones)
                        setValue('token', response.data.token)
                    }
                }
            })
    }


    const guardarNovedad = async (data: any) => {

        if (!data.idConceptoNovedad) {
            data.idConceptoNovedad = idConcepto;
        }

        idConceptNovedad = data.idConceptoNovedad;
        data.esAlta = esAltaChecked;
        data.esBaja = esBajaChecked;
        idTipoIdentificacion = data.idTipoIdentificacion;
        numeroDocumento = data.identificacion;
        idModulo = data.idModulo;
        token = data.token;
        idUnidadEjecutora = data.idUnidadEjecutora;


        try {
            const response = await apiCronograma.EditarNovedadAfiliado(data);

            if (response && response.isSuccess) {
                toast('Novedad modificada correctamente');
                const user = AuthService.getUserInfo();
                await guardarLogEditar(idTipoIdentificacion, numeroDocumento, fechaInicio, idUnidadEjecutora, 'GESTIÓN DE NOVEDADES');

                await apiNovedades.getObtenerNovedadesAfiliado(
                    identificationType as number,
                    identification
                ).then((response: any) => {
                    if (response && response.isSuccess) {
                        setNovedades(response.data);

                    }
                });
                history.push('/Gestion?edit=true')
            }
        } catch (error) {
            toast('Novedad no modificada');
        }
    };

    const guardarLogEditar = async (tipoDocumento: any, nDocumento: any, fechaInicio: any, idUnidadEjecutora: number, modulo: string) => {
        const user = AuthService.getUserInfo();
        await apiCronograma.InsertarAudoriaNovedades({
            idModulo: 1,
            modulo: modulo,
            tipoDocumento: tipoDocumento,
            nDocumento: nDocumento,
            detalleModificacion: `Se ha editado una novedad para el afiliado con número de identificación ${numeroDocumento}`,
            fechaModificacion: new Date(),
            usuarioModificacion: user.USR_ID,
            fechaInicio: fechaInicio,
            fechaFin: new Date(),
            token: token,
            idUnidadEjecutora: idUnidadEjecutora
        });
    };

    const handleOnClickRegresar = () => {
        history.push(`/Gestion?edit=true&idTipoIdentificacion=${identificationType}&identificacion=${identification}`);
    }

    useEffect(() => {
        obtenerConceptos();
        obtenerNovedad();
        startGetExecutingUnitsSchedule();
        setFechaInicio(new Date());
    }, []);


    const handleConceptoChange = (event: React.ChangeEvent<{ value: unknown }>) => {
        const value = event.target.value as number;

        setIdConcepto(value);
    };

    const handleUnidadEjecutoraChange = (event: { target: { value: any; }; }) => {
        const selectedValue = event.target.value;
        setSelectedUnidadEjecutora(selectedValue);
    };

    const [idConcepto, setIdConcepto] = useState(0)

    useEffect(() => {
        if (novedades.length > 0) {
            setValue('codigoAlta', novedades.find(x => x.idConceptoNovedad === watch('idConceptoNovedad'))?.codigoNovedadAlta)
            setValue('codigoBaja', novedades.find(x => x.idConceptoNovedad === watch('idConceptoNovedad'))?.codigoNovedadBaja)
            setIdConcepto(novedades.find(x => x.idConceptoNovedad === watch('idConceptoNovedad'))?.idConceptoNovedad)
        }
    }, [watch('idConceptoNovedad')])

    useEffect(() => {
        obtenerNovedadesUnidadEjecutora()
        setValue('periodo', scheduleState.executingUnitsSchedule?.find(x => x.idUnidadEjecutora === watch('idUnidadEjecutora'))?.periodo as any)
    }, [watch('idUnidadEjecutora')])

    const obtenerNovedadesUnidadEjecutora = async () => {
        await apiCronograma.ObtenerNovedadesUnidadEjecutora(watch('idUnidadEjecutora'))
            .then((responseCronograma) => {
                if (responseCronograma) {
                    if (responseCronograma.isSuccess && responseCronograma.data.length > 0) {
                        setNovedades(responseCronograma.data)
                        const newProducts: any[] = []
                        responseCronograma.data.forEach((element: any) => {
                            const product = products.find(x => x.idConceptoNovedad === element.idConceptoNovedad);
                            if (product != null) {
                                newProducts.push(product)
                            }
                        });
                        setProducts(newProducts)
                        setValue('idConceptoNovedad', responseCronograma.data.find((x: any) => x.idNovedad === idConceptNovedad)?.idConceptoNovedad)
                    }
                }
            })
    }

    useEffect(() => {
        obtenerNovedadesUnidadEjecutora();
    }, [watch('idUnidadEjecutora')]);

    const handleEsAltaChange = () => {
        setEsAltaChecked(!esAltaChecked);
        setEsBajaChecked(esAltaChecked);
    }

    const handleEsBajaChange = () => {
        setEsAltaChecked(esBajaChecked);
        setEsBajaChecked(!esBajaChecked);
    }


    return (
        <>
            <CHPaper title="Edición novedad">
                <LoadingComponent key="loader" loading={isLoading} />
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="idTipoIdentificacion"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Tipo de Identificación"
                                    value={catalogState.IdentificationType.find(x => x.id === value)?.descripcion}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="identificacion"
                            control={control}
                            rules={{
                                pattern: {
                                    value: /^[0-9]+$/g,
                                    message: 'Este campo es requerido y debe contener solo números'
                                },
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido y debe contener solo números',
                                },
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
                                min: {
                                    value: 1,
                                    message: 'Número de cédula inválido'
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    label="Número de Identificación *"
                                    value={value}
                                    numberMask
                                    onChange={onChange}
                                    onBlur={async () => {
                                        if (shouldValidate) {
                                            const newValue = value;
                                            setValue('identificacion', newValue);
                                        }
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
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Código Militar"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="idUnidadEjecutora"
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error }, }) => (
                                <CHSelect
                                    label="Unidad Ejecutora"
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
                                    label="Concepto"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={(event: React.ChangeEvent<{ value: unknown }>) => {
                                        onChange(event.target.value as number);
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
                                    {(codigoAltaValueAsString === '206' && (esAltaChecked))
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
                            rules={{
                                validate: validateDate
                            }}
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : ""}
                                    label="Fecha Inicial"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: true }}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                    minDate={new Date("1-01-1950")}
                                    maxDate={today}
                                    disabled
                                />

                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            rules={{
                                validate: validateEndDate
                            }}
                            name="fechaFinNovedad"
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : ""}
                                    label="Fecha Final"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{ readOnly: true }}
                                    InputLabelProps={{ shrink: true }}
                                    autoOk
                                    minDate={new Date("1-01-1950")}
                                    maxDate={today}
                                    disabled
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
                                    views={['year', 'month']}
                                    defaultValue=""
                                    label="Período"
                                    onChange={onChange}
                                    value={moment(value).endOf('month')}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    format="MM/yyyy"
                                    disabled
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
                                        isReadOnly
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
                                    maxRows={3}
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
                            color='primary'
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

export default connect(mapStateToProps, mapDispatchToProps)(editarNovedad);