import React, { useEffect, useRef, useState } from 'react';
import { createMuiTheme, createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { Controller, useForm, useFormContext, UseFormReturn } from 'react-hook-form';
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

function InformacionResciliacion({
    responseSolicitud,
    valorSolicitudV8,
    // acuerdoSolicitud,

}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    valorSolicitudV8: number;
    // acuerdoSolicitud: any;
}) {
    const classes = useStyles();

    const opcionesCausalResiliacion = [
        { label: 'Voluntad de las partes', value: 'voluntad_partes' },
        { label: 'Crédito negado', value: 'credito_negado' },
        { label: 'Mejor oferta para el vendedor', value: 'oferta_vendedor' },
        { label: 'Mejor oferta para el afiliado', value: 'oferta_afiliado' },
        { label: 'Denuncia ante Fiscalía', value: 'denuncia_fiscalia' },
        { label: 'Proceso ordinario ante Juzgado', value: 'proceso_juzgado' },
        { label: 'Fallecimiento de una de las partes', value: 'fallecimiento' },
    ];
    
    const { control, setValue, reset, watch, handleSubmit } = useFormContext();

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
        debugger
        setLoading(true);
        try {
            const response = await api.PostConsultarSolicitud(consulta);
            setIsLoading(false);


            if (response && response.data) {
                if (response.data.solicitudPropietarioDto && response.data.solicitudPropietarioDto.length > 0) {

                    setResponseSolicitudes(response.data);
                    consultarTrazabilidad(response?.data?.solicitudId);
                    setLoading(false);

                    setDatoMatricula(response.data.solicitudMatriculaDto?.[0]?.numeroMatricula);
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



    const hasFetched = useRef(false);

    useEffect(() => {
        if (responseSolicitud && !hasFetched.current) {
            const consulta = {
                tipoSubModalidadId: responseSolicitud?.tipoSubModalidadId || "",
                solicitudTareaId: responseSolicitud?.solicitudTareaId || "00000000-0000-0000-0000-000000000000",
                identificacion: responseSolicitud?.identificacionCliente || "",
                tipoIdentificacion: responseSolicitud?.tipoIdentificacion || 1,
                tipoCuenta: "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA",
                tipoVinculacion: "AFILIADO ADMINISTRACION CESANTIAS"
            };

            hasFetched.current = true;
            consultaSolicitud(consulta);
        }
    }, [responseSolicitud]);




    return (
        <>
            <CHPaper title="Información Resciliación y/o Acción Judicial">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="acuerdo"
                            control={control}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Acuerdo"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
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
                            control={control}
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
                                            textAlign: 'right',
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
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                                validate: {
                                    validDate: (value) => {
                                        const currentDate = new Date();
                                        currentDate.setHours(0, 0, 0, 0);

                                        if (!value) {
                                            return 'La fecha es requerida';
                                        }

                                        const selectedDate = new Date(value);
                                        selectedDate.setHours(0, 0, 0, 0);

                                        const tomorrow = new Date();
                                        tomorrow.setDate(tomorrow.getDate() + 1);
                                        tomorrow.setHours(0, 0, 0, 0);

                                        return selectedDate >= tomorrow || 'La fecha debe ser a partir de mañana';
                                    },
                                },
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
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="numeroMatricula"
                            control={control}
                            // rules={{
                            //     required: 'Este campo es requerido.',

                            // }}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Matricula"
                                    value={responseSolicitud?.solicitudMatriculaDto?.[0]?.numeroMatricula || ''}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
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
                            control={control}
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
                                    value={responseSolicitud?.valorRetirar || valorSolicitudV8}
                                    onChange={(e) => {
                                        onChange(e);
                                        // control.trigger(name);
                                    }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
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
                            control={control}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Identificación Beneficiario de Pago"
                                    value={responseSolicitud?.identificacionCliente}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
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
                            control={control}
                            render={({ field: { onChange, value, ref, name }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Beneficiario de Pago"
                                    value={`${responseSolicitud?.nombresCliente} ${responseSolicitud?.apellidosCliente}`}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
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
                            name="fechaFirma"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                                validate: {
                                    validDate: (value) => {
                                        const currentDate = new Date();
                                        currentDate.setHours(0, 0, 0, 0);

                                        if (!value) {
                                            return 'La fecha es requerida';
                                        }

                                        const selectedDate = new Date(value);
                                        selectedDate.setHours(0, 0, 0, 0);

                                        const tomorrow = new Date();
                                        tomorrow.setDate(tomorrow.getDate() + 1);
                                        tomorrow.setHours(0, 0, 0, 0);

                                        return selectedDate >= tomorrow || 'La fecha debe ser a partir de mañana';
                                    },
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha de Firma Resciliación"
                                    onChange={onChange}
                                    value={value}

                                    minDate={new Date().toISOString().split('T')[0]}
                                />
                            )}
                        />
                    </Grid>


                    {/* se habilitara con sub modalidad verificacion de grupo */}
                    {/* <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <Controller
                            name="acredito"
                            control={control}
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
                            name="causal"
                            control={control}
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

                </Grid>
            </CHPaper>
        </>
    );

}

export default InformacionResciliacion;