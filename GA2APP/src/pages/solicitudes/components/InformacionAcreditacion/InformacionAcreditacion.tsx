import React, { useEffect, useRef, useState } from 'react';
import { Button, createMuiTheme, createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { Control, Controller, useForm, useFormContext, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import UtilService from '../../../../utils/util.service';
import ApiClients from '../../../../services/api-clients.service';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { MatriculaInmobiliariaModel } from '../../../../models/matricula-inmobiliaria.model';
import { ConsultarSolicitudModel } from '../../../../models/requests/consultar-solicitud.model';
import apiSolicitud from '../../../../services/api-solicitud.service';


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        column3: {
            padding: '10px',
            textAlign: 'right',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnLabel: {
            padding: '10px',
        },
        columnTextAdd: {
            textAlign: 'right',
            width: '200px',
        },
    }),
);

function InformacionAcreditacion({
    responseSolicitud,
    matriculaAcre,
    vrRetiroAcre,
    formularioAcreditacion,
    control,
}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    matriculaAcre: string;
    vrRetiroAcre: number;
    formularioAcreditacion: UseFormReturn<any>;
    control: UseFormReturn<any>;
}) {
    const classes = useStyles();


    const opcionesCausal = [
        { label: 'Inmueble no Registra Compra', value: 'inmueble_no_compra' },
        { label: 'El inmueble está a nombre de un tercero', value: 'inmueble_tercero' },
    ];
    const opcionesCausalesAcredito = [
        { label: 'El inmueble corresponde a lote', value: 'inmueble_lote' },
        { label: 'El valor del inmueble es inferior al tope', value: 'vrl_inmueble' },
        { label: 'La escritura no refleja el valor del giro', value: 'escritura_giro' },
        { label: 'El inmueble esta a nombre de un tercero', value: 'inmueble_tercero' },
    ];
    const opcionesAccionRealizar = [
        { label: 'Efectuar Resciliación', value: 'efectuar_resiliacion' },
        { label: 'Iniciar Actuación Judicial', value: 'iniciar_judicial' },
    ];
    // const { setValue, reset, watch, handleSubmit } = useForm();
    const methods = formularioAcreditacion || control;

    if (!methods) {
        console.error('Error: methods no está definido en InformacionAcreditacion');
        return null;
    }

    const {
        control: rhfControl,
        setValue,
        watch,
        reset,
        handleSubmit,
    } = methods;

    if (!control) {
        console.error("Error: control no está definido en InformacionAcreditacion");
        return null;
    }

    const [loading, setLoading] = useState(false);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [responseSolicitudes, setResponseSolicitudes] = useState<any>([]);
    const [trazabilidad, setTrazabilidad] = useState<any>(null);
    const api = apiSolicitud();

    const consultarTrazabilidad = (solicitudId: string) => {
        api.obtenerTrazabilidadSolicitud(solicitudId)
            .then((response) => {
                setTrazabilidad(response?.data);
            }).catch((e) => {
                toast(`Error al consultar trazabilidad ${e.message}`, { type: toast.TYPE.ERROR });
            });
    }

    const [datoMatricula, setDatoMatricula] = useState<string | undefined>();


    const consultaSolicitud = async (consulta: ConsultarSolicitudModel) => {

        setLoading(true);
        try {
            const response = await api.PostConsultarSolicitud(consulta);
            setIsLoading(false);


            if (response && response.data) {
                setDatoMatricula(response.data.solicitudMatriculaDto?.[0]?.numeroMatricula);
                if (response.data.solicitudPropietarioDto && response.data.solicitudPropietarioDto.length > 0) {
                    setResponseSolicitudes(response.data);
                    consultarTrazabilidad(response?.data?.solicitudId);
                    setLoading(false);
                } else {
                    setResponseSolicitudes(response.data);
                    setLoading(false);
                }
            }
        } catch (error) {
            setLoading(false);
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        }
    };

    const [hasFetched, setHasFetched] = useState(false);

    useEffect(() => {

        if (responseSolicitud) {

            const consulta = {
                tipoSubModalidadId: responseSolicitud?.tipoSubModalidadId || "",
                solicitudTareaId: responseSolicitud?.solicitudTareaId,
                identificacion: responseSolicitud?.identificacionCliente || "",
                tipoIdentificacion: responseSolicitud?.tipoIdentificacion || 1,
                tipoCuenta: "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA",
                tipoVinculacion: "AFILIADO ADMINISTRACION CESANTIAS",

            };


            consultaSolicitud(consulta);
            // setHasFetched(true);
        }
    }, [responseSolicitud]);


    return (
        <>
            <CHPaper title="Información Acreditación">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="acuerdo"
                            // control={formularioAcreditacion?.control || control?.control}
                            control={rhfControl}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Acuerdo"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'left',
                                        },
                                    }}
                                    // required
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly

                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="noRadicado"
                            // control={control?.control}
                            control={rhfControl}
                            // rules={{
                            //     required: 'Este campo es requerido.',

                            // }}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="No. Radicado"
                                    value={responseSolicitud?.numeroRadicadoSG}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'left',
                                        },
                                    }}
                                    // required
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly

                                />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={6} lg={3} className={classes.column}>
                        <Controller
                            name="fechaGiro"
                            // control={control?.control}
                            control={rhfControl}
                            rules={{
                                required: 'Este campo es requerido',
                                // validate: {
                                //     validDate: (value) => {
                                //         const currentDate = new Date();
                                //         currentDate.setHours(0, 0, 0, 0);

                                //         if (!value) {
                                //             return 'La fecha es requerida';
                                //         }

                                //         const selectedDate = new Date(value);
                                //         selectedDate.setHours(0, 0, 0, 0);

                                //         const tomorrow = new Date();
                                //         tomorrow.setDate(tomorrow.getDate() + 1);
                                //         tomorrow.setHours(0, 0, 0, 0);

                                //         return selectedDate >= tomorrow || 'La fecha debe ser a partir de mañana';
                                //     },
                                // },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha Giro"
                                    onChange={onChange}
                                    value={value}
                                    minDate={new Date().toISOString().split('T')[0]}
                                    readOnly={responseSolicitud?.descripcionModalidad === "ACREDITACION V8"}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="numeroMatricula"
                            // control={control?.control}
                            control={rhfControl}
                            // rules={{
                            //     required: 'Este campo es requerido.',

                            // }}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Matricula"
                                    value={datoMatricula}
                                    onChange={(e) => {
                                        onChange(e);
                                    }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'left',
                                        },
                                    }}
                                    // required
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly

                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.columnLabel} item xs={12} md={6} lg={3}>
                        <Controller
                            name="valorGirado"
                            // control={control?.control}
                            control={rhfControl}
                            // rules={{
                            //     required: 'Este campo es requerido.',
                            //     validate: (value) => {
                            //         const valueNumber = UtilService.cleanCurrencyValue(value);
                            //         return valueNumber < 1 ? 'Este valor no es aceptado' : true;
                            //     }
                            // }}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Valor Girado"
                                    value={responseSolicitud?.valorRetirar || vrRetiroAcre || value}
                                    onChange={(e) => {
                                        onChange(e);
                                    }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'left',
                                        },
                                    }}
                                    // required
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly

                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="documentoBeneficiarioPago"
                            // control={control?.control}
                            control={rhfControl}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Identificación Beneficiario de Pago"
                                    value={responseSolicitud?.identificacionCliente}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'left',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly

                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="beneficiarioPago"
                            // control={control?.control}
                            control={rhfControl}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Beneficiario de Pago"
                                    value={`${responseSolicitud?.nombresCliente} ${responseSolicitud?.apellidosCliente}`}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'left',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly

                                />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={6} lg={3} className={classes.column}>
                        <Controller
                            name="fechaSeguimiento"
                            // control={control?.control}
                            control={rhfControl}
                            rules={{
                                required: 'Este campo es requerido',

                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha de Seguimiento"
                                    onChange={onChange}
                                    value={value}
                                    minDate={new Date().toISOString().split('T')[0]}
                                    readOnly={responseSolicitud?.descripcionModalidad === "ACREDITACION V8"}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={6} lg={3} className={classes.column}>
                        <Controller
                            name="fechaProximoSeguimiento"
                            // control={control?.control}
                            control={rhfControl}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => {

                                const fechaActual = value ? new Date(value) : new Date();
                                fechaActual.setMonth(fechaActual.getMonth() + 1);


                                const fechaModificada = `${fechaActual.getFullYear()}-${(fechaActual.getMonth() + 1)
                                    .toString().padStart(2, '0')}-${fechaActual.getDate().toString().padStart(2, '0')}`;

                                return (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Fecha de Próximo Seguimiento"
                                        onChange={(newDate) => {
                                            if (!newDate) return;

                                            const nuevaFecha = new Date(newDate as any);
                                            nuevaFecha.setMonth(nuevaFecha.getMonth() + 1);


                                            const fechaFormateada = `${nuevaFecha.getFullYear()}-${(nuevaFecha.getMonth() + 1)
                                                .toString().padStart(2, '0')}-${nuevaFecha.getDate().toString().padStart(2, '0')}`;

                                            onChange(fechaFormateada);
                                        }}
                                        value={fechaModificada}
                                        minDate={new Date()}
                                    // readOnly={responseSolicitud?.descripcionModalidad === "ACREDITACION V8"}
                                    />
                                );
                            }}
                        />
                    </Grid>

                    {/* se habilitara con sub modalidad verificacion de grupo */}
                    {/* <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="acredito"
                            control={control?.control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Acredito"
                                    value={value || ''}
                                    onChange={(e) => {
                                        const newValue = e.target.value;
                                        onChange(newValue);
                                        setValue('acredito', newValue);
                                        if (newValue !== 'NO') {
                                            setValue('causal', '');
                                        }
                                    }}

                                >
                                    <MenuItem value="SI">SÍ</MenuItem>
                                    <MenuItem value="NO">NO</MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid> */}

                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="cumpleRequisito"
                            // control={formularioAcreditacion?.control || control}
                            control={rhfControl}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Cumple Requisito"
                                    value={value ?? responseSolicitud?.cumpleRequisito ?? ''}
                                    onChange={(e) => {
                                        const newValue = e.target.value;
                                        onChange(newValue);

                                        if (newValue !== 'NO') {
                                            setValue('causal', '');
                                        }
                                    }}
                                >
                                    <MenuItem value="SI">SÍ</MenuItem>
                                    <MenuItem value="NO">NO</MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    {watch('cumpleRequisito') === 'NO' && (
                        <Grid className={classes.column} item xs={12} md={6} lg={3}>
                            <Controller
                                name="causal"
                                // control={control?.control}
                                // control={formularioAcreditacion?.control || control}
                                control={rhfControl}
                                rules={{ required: 'Este campo es requerido.' }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Causal"
                                        value={value || ''}
                                        onChange={onChange}
                                        required
                                    >
                                        {opcionesCausal.map((opcion) => (
                                            <MenuItem key={opcion.value} value={opcion.value}>
                                                {opcion.label}
                                            </MenuItem>
                                        ))}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    )}

                    {watch('acredito') === 'NO' && (
                        <Grid className={classes.column} item xs={12} md={6} lg={3}>
                            <Controller
                                name="causal"
                                // control={control?.control}
                                control={rhfControl}
                                rules={{ required: 'Este campo es requerido.' }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Causal"
                                        value={value || ''}
                                        onChange={onChange}
                                        required
                                    >
                                        {opcionesCausalesAcredito.map((opcion) => (
                                            <MenuItem key={opcion.value} value={opcion.value}>
                                                {opcion.label}
                                            </MenuItem>
                                        ))}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    )}

                    {watch('acredito') === 'NO' && (
                        <Grid className={classes.column} item xs={12} md={6} lg={3}>
                            <Controller
                                name="accionRealizar"
                                
                                // control={formularioAcreditacion?.control || control?.control}
                                control={rhfControl}
                                rules={{ required: 'Este campo es requerido.' }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Acción a Realizar"
                                        value={value || ''}
                                        onChange={onChange}
                                        required
                                    >
                                        {opcionesAccionRealizar.map((opcion) => (
                                            <MenuItem key={opcion.value} value={opcion.value}>
                                                {opcion.label}
                                            </MenuItem>
                                        ))}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    )}

                </Grid>
            </CHPaper>
        </>
    );

}

export default InformacionAcreditacion;