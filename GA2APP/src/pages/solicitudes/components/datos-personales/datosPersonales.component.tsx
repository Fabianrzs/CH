import React, { useEffect, useState } from 'react';
import { createMuiTheme, createStyles, Grid, makeStyles } from '@material-ui/core';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import UtilService from '../../../../utils/util.service';
import ApiClients from '../../../../services/api-clients.service';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import { ID_SUBMODALIDAD_SEGUNDO_PAGO } from '../../../../constants/constants';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
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


function DatosPersonales({
    responseSolicitud,
    control,
    setValidarRetiro,
    siEsAprobadaSolcitud,
    campoSoloLecturaRadicacionAprobada,
    setToastId,
    setMostrarMensaje,
    formularioAcreditacion
}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    control: UseFormReturn<any>;
    setValidarRetiro: React.Dispatch<React.SetStateAction<number>>;
    siEsAprobadaSolcitud: boolean;
    campoSoloLecturaRadicacionAprobada: any;
    setToastId: (value: React.ReactText | null | undefined) => void;
    setMostrarMensaje: (value: boolean) => void;
    formularioAcreditacion: UseFormReturn<any>;
}) {

    const apiDos = ApiClients();
    const [datoCuota, setDatoCuotas] = useState<any>(0);
    const [tipoDoc, setTipoDoc] = useState<string>('');
    const [alertaMostrada, setAlertaMostrada] = useState(false);
    const [descripcionProceso, setDescripcionProceso] = useState('');
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    tomorrow.setHours(0, 0, 0, 0);

    useEffect(() => {
        obtenerNumerosCuota();
        tipoSolicitud();
    }, [])

    const classes = useStyles();
    const today = new Date();

    const [esDesafiliacionSoloAportes, setEsDesafiliacionSoloAportes] = useState(false);
    const [totalConceptosAportes, setTotalConceptosAportes] = useState(0);

    const obtenerNumerosCuota = async () => {
        const identificationType = Number(responseSolicitud?.tipoIdentificacion || '');
        const identification = responseSolicitud?.identificacionCliente || '';

        await apiDos
            .getClientByIdentificationTypeIdentificationNumberRequest(
                identificationType,
                identification
            )
            .then((response: any) => {
                if (response.isSuccess) {
                    setDatoCuotas(response.data);
                    setTipoDoc(response?.data.nombreTipoIdentificacion);

                    const solicitudes = response?.data?.solicitudes || [];

                    const solicitudV8 = solicitudes.find((item: any) =>
                        item?.actividad === 'Trámite de pago V 8' &&
                        [
                            'COMPRA O ADQUISICIÓN DE VIVIENDA USADA ',
                            'COMPRA O ADQUISICIÓN DE VIVIENDA NUEVA ',
                        ].includes(item?.modalidad)
                    );

                    if (solicitudV8?.modalidad) {
                        control.setValue('tipoVivienda', solicitudV8.modalidad);
                    } else {
                        control.setValue('tipoVivienda', '');
                    }


                    const modalidad = responseSolicitud?.descripcionModalidad || "";
                    const subModalidad = responseSolicitud?.descripcionSubModalidad || "";

                    const subModalidadesUsanAportes = [
                        "VOLUNTARIA",
                        "POR NO ACREDITACIÓN"
                    ];

                    const aplicaDesafiliacionSoloAportes =
                        modalidad === "DESAFILIACIÓN SÓLO APORTES" &&
                        subModalidadesUsanAportes.includes(subModalidad);

                    let totalSaldos = 0;

                    if (aplicaDesafiliacionSoloAportes && response?.data?.cuentas?.length > 0) {
                        response.data.cuentas.forEach((cuenta: any) => {
                            cuenta.conceptos?.forEach((concepto: any) => {
                                const desc = concepto.conceptoDescripcion?.toUpperCase() || "";

                                if (
                                    desc === "AHORROS OBLIGATORIOS" ||
                                    desc === "AHORROS RETROACTIVOS" ||
                                    desc === "AHORROS VOLUNTARIOS" ||
                                    desc === "INTERESES APORTES"
                                ) {
                                    // asegurar número
                                    totalSaldos += Number(concepto.saldo || 0);
                                }
                            });
                        });
                    }

                    // guardar el total (aunque sea 0)
                    setTotalConceptosAportes(totalSaldos);

                    // es desafiliación solo aportes se considera verdadero sólo si aplica y hay cuentas (o si la regla tuya no requiere saldo>0, quita `&& totalSaldos >= 0`)
                    setEsDesafiliacionSoloAportes(aplicaDesafiliacionSoloAportes);

                    if (
                        ID_SUBMODALIDAD_SEGUNDO_PAGO.some(
                            (item) => item === responseSolicitud?.tipoSubModalidadId
                        ) &&
                        response?.data?.valorSubsidio > 0
                    ) {
                        control.setValue('valorSolicitud', response?.data?.valorSubsidio);

                        setValidarRetiro(
                            UtilService.cleanCurrencyValue(response?.data?.valorSubsidio)
                        );
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const tipoSolicitud = async () => {
        if (
            responseSolicitud?.nombreTarea?.endsWith('V 14') &&
            responseSolicitud?.descripcionSubModalidad === 'PRIMER PAGO'
        ) {
            setDescripcionProceso('SOLICITUD VIVIENDA 14');
        } else {
            setDescripcionProceso(responseSolicitud?.descripcionProceso || '');
        }
    };

    useEffect(() => {
        const subModalidad = responseSolicitud?.descripcionSubModalidad ?? "";

        const subModalidadesBloqueo = [
            "RETIRO EXPRESS DE LA INSTITUCION",
            "RETIRO NORMAL DE LA INSTITUCION",
            "POR MUERTE DEL AFILIADO",
            "VOLUNTARIA",
            "POR NO ACREDITACIÓN"
        ];

        const esBloqueo = subModalidadesBloqueo.includes(subModalidad);

        let valorInicial: any = "";

        if (esDesafiliacionSoloAportes) {
            valorInicial = totalConceptosAportes;
        } else if (esBloqueo) {
            valorInicial = responseSolicitud?.valorTotal;
        } else {
            valorInicial =
                control.getValues("valorSolicitud") ||
                responseSolicitud?.valorRetirar ||
                "";
        }

        const valorNumerico = UtilService.cleanCurrencyValue(valorInicial);

        if (valorNumerico > 0) {
            control.setValue("valorSolicitud", valorInicial, {
                shouldValidate: true,
                shouldDirty: false,
            });

            setValidarRetiro(valorNumerico);
        }
    }, [
        responseSolicitud?.valorRetirar,
        responseSolicitud?.valorTotal,
        responseSolicitud?.descripcionSubModalidad,
        esDesafiliacionSoloAportes,
        totalConceptosAportes,
    ]);

    const descripcionProcesoNormalizada = String(
        responseSolicitud?.descripcionProceso || '',
    )
        .trim()
        .toUpperCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '');


    const esSubsidioSecuestrados =
        descripcionProcesoNormalizada ===
        'SUBSIDIO SECUESTRADOS';


    return (
        <>
            <CHPaper title="Información Solicitud">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <CHTextField
                            isReadOnly
                            label="Nombres"
                            value={responseSolicitud?.nombresCliente || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <CHTextField
                            isReadOnly
                            label="Apellidos"
                            value={responseSolicitud?.apellidosCliente || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <CHTextField
                            isReadOnly
                            label="Tipo de Documento"
                            value={
                                tipoDoc || ''
                            }
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <CHTextField
                            numberMask
                            isReadOnly
                            label="Número de Documento"
                            value={
                                responseSolicitud?.identificacionCliente || ''
                            }
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <CHTextField
                            isReadOnly
                            label="Tipo de Solicitud"
                            value={descripcionProceso || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <CHTextField
                            isReadOnly
                            label="Modalidad"
                            value={
                                responseSolicitud?.descripcionModalidad || ''
                            }
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6} lg={3}>
                        <CHTextField
                            isReadOnly
                            label="Sub-Modalidad"
                            value={
                                responseSolicitud?.descripcionSubModalidad || ''
                            }
                        />
                    </Grid>

                    <Grid className={classes.columnLabel} item xs={12} md={6} lg={2}>
                        <Controller
                            name="valorSolicitud"
                            control={control?.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                validate: (value) => {
                                    const valueNumber = UtilService.cleanCurrencyValue(value);
                                    return valueNumber < 1 ? 'Este valor no es aceptado' : true;
                                }
                            }}
                            render={({ field: { onChange, value, }, fieldState: { error } }) => {
                                const subModalidad = responseSolicitud?.descripcionSubModalidad ?? "";

                                const subModalidadesBloqueo = [
                                    "RETIRO EXPRESS DE LA INSTITUCION",
                                    "RETIRO NORMAL DE LA INSTITUCION",
                                    "POR MUERTE DEL AFILIADO",
                                    "VOLUNTARIA",
                                    "POR NO ACREDITACIÓN"
                                ];

                                const esBloqueo = subModalidadesBloqueo.includes(subModalidad);

                                let valorForzado: any;
                                if (esDesafiliacionSoloAportes) {
                                    valorForzado = totalConceptosAportes;
                                } else if (esBloqueo) {
                                    valorForzado = responseSolicitud?.valorTotal;
                                } else {
                                    const valorBase = value ?? responseSolicitud?.valorRetirar;

                                    valorForzado =
                                        valorBase === 0 || valorBase === "0"
                                            ? ""
                                            : valorBase;
                                }

                                return (
                                    <CHTextField
                                        className={classes.columnTextAdd}
                                        currencyMask
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={
                                            esDesafiliacionSoloAportes ||
                                            esBloqueo ||
                                            ((responseSolicitud?.descripcionModalidad === "ACREDITACION V8" ||
                                                siEsAprobadaSolcitud ||
                                                responseSolicitud?.secciones?.includes("INCONSISTENCIA")) &&
                                                responseSolicitud?.soL_RECHAZADA !== "1") ||
                                            campoSoloLecturaRadicacionAprobada ||
                                            (ID_SUBMODALIDAD_SEGUNDO_PAGO.some(
                                                item => item === responseSolicitud?.tipoSubModalidadId
                                            ) && value > 0)
                                        }
                                        label="Valor Solicitud"
                                        value={valorForzado}
                                        onChange={(e) => {
                                            if (!esBloqueo && !esDesafiliacionSoloAportes) {
                                                onChange(e);

                                                const valueNumber = UtilService.cleanCurrencyValue(e.target.value);
                                                setValidarRetiro(valueNumber);
                                            }
                                        }}
                                        inputProps={{
                                            style: { textAlign: "left" },
                                        }}
                                        required
                                        InputLabelProps={{ shrink: true }}
                                        onBlur={(e) => {
                                            if (esBloqueo || esDesafiliacionSoloAportes) return;

                                            const valueNumber = UtilService.cleanCurrencyValue(
                                                e.target.value || valorForzado
                                            );

                                            setValidarRetiro(valueNumber);

                                            if (responseSolicitud) {
                                                if (
                                                    responseSolicitud.nombreTarea !== "Radicación V 8" &&
                                                    responseSolicitud.nombreTarea !== "Radicación V 14"
                                                ) {
                                                    responseSolicitud.valorTotal = datoCuota.saldoDisponible;
                                                } else {
                                                    let salTotalV8V14 = 0;
                                                    if (datoCuota && datoCuota.cuentas) {
                                                        datoCuota.cuentas.forEach((item: any) => {
                                                            if (item.tipoCuenta === "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA") {
                                                                salTotalV8V14 += item.saldoDisponibleEmbargos;
                                                            }
                                                        });
                                                    }

                                                    responseSolicitud.valorTotal = salTotalV8V14.toString();
                                                }
                                            }

                                            const valorTo = responseSolicitud?.valorTotal ?? "0";
                                            const valorTotal = parseFloat(valorTo);
                                            const subModalidadSegundoPago = ID_SUBMODALIDAD_SEGUNDO_PAGO.some(
                                                (item) => item === responseSolicitud?.tipoSubModalidadId
                                            );

                                            if (valorTotal && valueNumber > valorTotal && !subModalidadSegundoPago) {
                                                if (!alertaMostrada) {
                                                    setMostrarMensaje(true);
                                                    setToastId(
                                                        toast(
                                                            `No cuenta con el saldo suficiente para realizar esta Solicitud. Su saldo es ${UtilService.formatCurrency(valorTotal)}`,
                                                            {
                                                                type: toast.TYPE.ERROR,
                                                                autoClose: false,
                                                                closeButton: false,
                                                                closeOnClick: false,
                                                            }
                                                        )
                                                    );
                                                    setAlertaMostrada(true);
                                                }
                                            } else {
                                                setMostrarMensaje(false);
                                                setAlertaMostrada(false);
                                            }
                                        }}
                                    />
                                );
                            }}
                        />
                    </Grid>

                    <Grid className={classes.columnLabel} item xs={12} md={6} lg={3}>
                        <Controller
                            name="numero_Folio"
                            control={formularioAcreditacion?.control || control?.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                min: {
                                    value: 1,
                                    message: 'No puede ser un valor menor a 1',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    className={classes.columnTextAdd}
                                    numberMask
                                    error={!!(error || (value === 0))}
                                    helperText={error ? error.message : null}
                                    isReadOnly={((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') ||
                                        campoSoloLecturaRadicacionAprobada}
                                    label="Número de folios *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    {esSubsidioSecuestrados && (
                        <Grid
                            className={classes.column}
                            item
                            xs={12}
                            md={6}
                            lg={3}
                        >
                            <Controller
                                name="fechaLiberacion"
                                control={control?.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: {
                                        onChange,
                                        value,
                                    },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        error={!!error}
                                        helperText={
                                            error
                                                ? error.message
                                                : ''
                                        }
                                        label="Fecha de liberación *"
                                        onChange={onChange}
                                        value={value || null}
                                        disabled={
                                            (
                                                siEsAprobadaSolcitud ||
                                                responseSolicitud
                                                    ?.secciones
                                                    ?.includes(
                                                        'INCONSISTENCIA',
                                                    )
                                            ) &&
                                            responseSolicitud
                                                ?.soL_RECHAZADA !==
                                            '1'
                                        }
                                        inputProps={{
                                            readOnly: true,
                                        }}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        autoOk
                                        minDate={
                                            new Date(
                                                '1-01-1950',
                                            )
                                        }
                                        maxDate={today}
                                    />
                                )}
                            />
                        </Grid>
                    )}
                    {(responseSolicitud?.nombreTarea.includes("V 8") || responseSolicitud?.nombreTarea === "Verificación en tiempo") && (
                        <>

                            <Grid item xs={12} md={6} lg={3} className={classes.column}>
                                <Controller
                                    name="fecha_Firma_Escritura"
                                    control={control?.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (
                                        <CHDatePicker
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Fecha firma de escritura*"
                                            onChange={onChange}
                                            value={value}
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{
                                                readOnly: ((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') ||
                                                    campoSoloLecturaRadicacionAprobada
                                            }}
                                            readOnly={((responseSolicitud?.descripcionModalidad === "ACREDITACION V8" || siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') ||
                                                campoSoloLecturaRadicacionAprobada}
                                            minDate={responseSolicitud?.descripcionModalidad === "ACREDITACION V8" ? undefined : tomorrow} />
                                    )} />
                            </Grid>
                            {/* <Grid item xs={12} md={6} lg={4} className={classes.column}>

                                <Controller
                                    name="tipoVivienda"
                                    control={control?.control}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (
                                        <CHTextField

                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly
                                            label="Tipo de Vivienda"
                                            value={value || ''}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            </Grid> */}
                        </>
                    )}
                </Grid>
            </CHPaper>
        </>
    );
}

export default DatosPersonales;