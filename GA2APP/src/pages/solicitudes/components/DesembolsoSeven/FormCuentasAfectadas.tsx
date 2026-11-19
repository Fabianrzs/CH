import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { createStyles, Grid, MenuItem, RadioGroup, Radio, FormLabel, makeStyles, MuiThemeProvider, createTheme, Button } from '@material-ui/core';
import { connect, useSelector } from 'react-redux';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../../components/ch-select/CHSelect';
import SolicitudApi from '../../../../services/api-solicitud.service';
import CatalogApi from '../../../../services/api-catalog.service';
import ObservacionesSolicitud from '../observaciones/observaciones';
import { ENTIDADBANCARIA } from '../../../../types/catalog.type';
import { TerceroBeneficiarioModel } from '../../../../models/tercero-beneficiario.model';
import { AppState } from '../../../../store/rootReducer';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center'
        },
        columnLabel: {
            padding: '10px'
        }
    })
);

const FormCuentasAfectadas = ({
    responseSolicitud,
    formularioObservaciones,
    seccion,
    trazabilidad,
    formularioVerificacionBeneficiariosPago,
    obtenerPlanillaPdf
}: {
    responseSolicitud: any;
    formularioObservaciones: any;
    seccion: string;
    trazabilidad: any[];
    formularioVerificacionBeneficiariosPago: UseFormReturn<any>;
    obtenerPlanillaPdf: () => void;
}) => {

    const classes = useStyles();
    const apiSolicitud = SolicitudApi();
    const apiCatalog = CatalogApi();
    const [cuentasAfectadas, setCuentasAfectadas] = useState<any[]>([]);
    const [cuentasAfectadasDos, setCuentasAfectadasDos] = useState<any[]>([]);
    const [mostrarInfoPlanillaPago, setMostrarInfoPlanillaPago] = useState(false);
    const [entidadesBancarias, setEntidadesBancarias] = useState<ENTIDADBANCARIA[]>([]);

    const formularioInfoPlanillaPago = useForm({
        defaultValues: {
            cdp: '',
            numeroFactura: ''
        }
    })

    useEffect(() => {
        
        obtenerCuentasPorIdentificacion()
        obtenerCuentasAfectadasCliente()
        obtenerEntidadesBancarias()
    }, []);

    useEffect(() => {
        
        formularioVerificacionBeneficiariosPago.setValue('valorPago', responseSolicitud?.valorRetirar);
        if (trazabilidad &&
            (trazabilidad?.some((item: any) => item?.descripcionTarea.includes('Trámite de pago') && item?.observacion !== null && item?.observacion !== '') ||
                trazabilidad?.some((item: any) => item?.descripcionTarea.includes('Trámite de pago') && item?.descripcionEstadoSolicitud === 'APROBADA'))) {
            if (responseSolicitud && responseSolicitud?.solicitudTerceroBeneficiarioDto[0]) {
                formularioVerificacionBeneficiariosPago.setValue('entidadBancaria', `${responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.entidadObj?.id}_${responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.entidadObj?.descripcion}`);
                formularioVerificacionBeneficiariosPago.setValue('numeroCuentaBeneficiario', responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.numeroCuenta);
                formularioVerificacionBeneficiariosPago.setValue('valor', responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.valorRetirarDos);
            }
        }

        if (trazabilidad && trazabilidad?.some((item: any) => item?.descripcionTarea.includes('Trámite de pago') && item?.descripcionEstadoSolicitud === 'APROBADA')) {
            formularioInfoPlanillaPago.setValue('cdp', responseSolicitud?.numeroFactura);
            formularioInfoPlanillaPago.setValue('numeroFactura', responseSolicitud?.cdp);
            setMostrarInfoPlanillaPago(true);
        }
    }, [trazabilidad, responseSolicitud]);


    const obtenerEntidadesBancarias = () => {
        apiCatalog
            .getAllEntidadBancaria()
            .then(async (response: any) => {
                if (response.data) {
                    setEntidadesBancarias(response?.data);
                }
            })
            .catch(() => {
                toast("Error al consultar entidades bancarias", { type: toast.TYPE.ERROR });
            });
    };

    const obtenerCuentasAfectadasCliente = async () => {
        try {
            const request = {
                solicitudId: responseSolicitud.solicitudId,
                identificacion: responseSolicitud.identificacionCliente,
            };

        

            const response = await apiSolicitud.PostObtenerCuentasAfectadasCliente(request);

            if (response && response.isSuccess) {

                

                let cuentasAfectadas: any = [];
                if (responseSolicitud.nombreTarea === 'Verificación cuenta individual' ||
                    responseSolicitud.nombreTarea === 'Verificación cuenta individual Giro de recursos') {
                    const nombreCuenta = "CUENTA INDIVIDUAL ADMINISTRACIÓN CESANTÍAS";
                    const nombreCuentaAvac = "AHORRO VOLUNTARIO AFILIADO CESANTÍAS";
                    const nombreCuentaCisv = "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA";

                    const cuentasIndividuales = response?.data.filter((x: any) => x?.descripcioN_CUENTA === nombreCuenta);
                    const cuentasAvac = response.data.filter((x: any) => x?.descripcioN_CUENTA === nombreCuentaAvac);
                    const cuentasCisv = response.data.filter((x: any) => x?.descripcioN_CUENTA === nombreCuentaCisv);

                    if (cuentasIndividuales?.length > 0 && cuentasAvac?.length > 0) {
                        cuentasAfectadas = [cuentasIndividuales[0], cuentasAvac[0]];
                    } else if (cuentasIndividuales?.length > 0 && cuentasCisv?.length > 0) {
                        cuentasAfectadas = [cuentasIndividuales[0], cuentasCisv[0]];
                    } else {
                        cuentasAfectadas = [cuentasIndividuales[0]]
                    }
                } else {
                    const nombreCuenta = "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA";
                    const cuentas = response?.data.filter((x: any) => x?.descripcioN_CUENTA === nombreCuenta);

                    if (cuentas.length > 0) {
                        cuentasAfectadas = [cuentas[0]];
                    }
                }

                setCuentasAfectadas(response.data);
                setCuentasAfectadasDos(response.data);
            } else {
                toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
            }
        } catch (error) {
            toast("Error del sistema", { type: toast.TYPE.ERROR });
        }
    };

    const obtenerCuentasPorIdentificacion = () => {
        const request = {
            solicitudId: responseSolicitud.solicitudId,
            identificacion: responseSolicitud.identificacionCliente,
        }

        apiSolicitud
            .PostObtenerConceptosAfectadosCliente(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response?.data !== '') {

                        if (responseSolicitud?.nombreTarea === 'Verificación cuenta individual' ||
                            responseSolicitud?.nombreTarea === 'Verificación cuenta individual Giro de recursos') {

                            setCuentasAfectadasDos(response?.data)
                        } else {
                            setCuentasAfectadasDos(response?.data)

                        }


                    } else {
                        toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
                    }
                }
            })
            .catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
    };

    const uniqueCuentasAfectadas = cuentasAfectadasDos.filter(
        (item, index, self) =>
            index === self.findIndex((t) => t.numerO_CUENTA === item.numerO_CUENTA)
    );

    return (
        <>
            <CHPaper title='Cuentas Afectadas'>
                <Grid container>
                    {uniqueCuentasAfectadas.map((option: any, index: number) => (
                        <><Grid className={classes.column} item xs={12} md={6} lg={4}>
                            <Controller
                                name='numeroCuenta'
                                control={formularioVerificacionBeneficiariosPago.control}
                                render={({
                                    field: { onChange, value }, fieldState: { error },
                                }) => (

                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label='Número de cuenta'
                                        value={option?.numerO_CUENTA}
                                        onChange={onChange}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }} />
                                )} />
                        </Grid>
                            <Grid className={classes.column} item xs={12} md={6} lg={4}>
                                <Controller
                                    name='descripcionCuenta'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (

                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label='Cuenta'
                                            value={option?.descripcioN_CUENTA}
                                            onChange={onChange}
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }} />
                                    )} />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={6} lg={4}>
                                <Controller
                                    name='valorPago'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (

                                        <CHTextField
                                            currencyMask
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label='Valor pago'
                                            value={value}
                                            onChange={onChange}
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }} />
                                    )} />
                            </Grid>
                        </>
                    ))}
                </Grid>
            </CHPaper>

            <CHPaper title='Conceptos Afectados'>
                <Grid container>
                    {cuentasAfectadas?.map((option: any, index: number) => (
                        <>
                            <Grid className={classes.column} item xs={6} md={4} lg={2}>
                                <Controller
                                    name='cuentaAfectados'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (

                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label='Cuenta'
                                            value={option?.numerO_CUENTA}
                                            onChange={onChange}
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }} />
                                    )} />
                            </Grid>
                            <Grid className={classes.column} item xs={8} md={6} lg={4}>
                                <Controller
                                    name='conceptoAfectados'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (

                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label='Concepto'
                                            value={option?.ctC_DESCRIPCION}
                                            onChange={onChange}
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }} />
                                    )} />
                            </Grid>
                            <Grid className={classes.column} item xs={6} md={4} lg={2}>
                                <Controller
                                    name='disponible'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (

                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}

                                            label='Disponible'
                                            value={option?.caC_SALDO}
                                            onChange={onChange}
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }}
                                            currencyMask
                                        />
                                    )} />
                            </Grid>
                            <Grid className={classes.column} item xs={6} md={4} lg={2}>
                                <Controller
                                    name='monto'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (

                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label='Monto'
                                            value={option.caC_VALOR_DESCONTADO}
                                            onChange={onChange}
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }}
                                            currencyMask
                                        />
                                    )} />
                            </Grid>
                            <Grid className={classes.column} item xs={6} md={4} lg={2}>
                                <Controller
                                    name='interesAcumulado'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    render={({
                                        field: { onChange, value }, fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            currencyMask
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label='Interes Acumulado'
                                            onChange={onChange}
                                            isReadOnly
                                            InputLabelProps={{ shrink: true }} />
                                    )} />
                            </Grid>

                        </>
                    ))}
                </Grid>
            </CHPaper>

            <CHPaper title='Información de Pago Nuevo' >
                Lista de Beneficiarios
                <Grid container>
                    <>
                        <Grid className={classes.column} item xs={12} md={3}>
                            {responseSolicitud?.solicitudTerceroBeneficiarioDto?.map((beneficiario: any, index: number) => (
                                <Controller
                                    key={index}
                                    name='tipoDocumento'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    defaultValue={beneficiario.tipoDocObj.descripcion?.split("_")[1] || beneficiario.tipoDocObj.descripcion || ''}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly
                                            label='Tipo de Documento'
                                            value={value?.split("_")[1] || value}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            ))}
                        </Grid>

                        <Grid className={classes.column} item xs={12} md={3}>
                            {responseSolicitud?.solicitudTerceroBeneficiarioDto?.map((beneficiario: any, index: number) => (
                                <Controller
                                    key={index}
                                    name='numeroDocumento'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    defaultValue={beneficiario.numDoc?.split("_")[1] || beneficiario.numDoc || ''}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (

                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly
                                            label="Numero de documento"
                                            value={value?.split("_")[1] || value}
                                            onChange={onChange}
                                        />

                                    )}
                                />
                            ))}
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            {responseSolicitud?.solicitudTerceroBeneficiarioDto?.map((beneficiario: any, index: number) => (
                                <Controller
                                    key={index}
                                    name='NombreRazonSocial'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    defaultValue={beneficiario.razonsocial || ''}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (

                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly
                                            label="Nombre / Razón Social"
                                            value={beneficiario?.razonsocial?.includes('_') ? String(beneficiario?.razonsocial).split('_')[2] : beneficiario?.razonsocial}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            ))}
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            {responseSolicitud?.solicitudTerceroBeneficiarioDto?.map((beneficiario: any, index: number) => (
                                <Controller
                                    key={index}
                                    name='tipoCuenta'
                                    control={formularioVerificacionBeneficiariosPago.control}
                                    defaultValue={beneficiario.tipoCuentaObj.descripcion?.split("_")[1] || beneficiario.tipoCuentaObj.descripcion || ''}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (

                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly
                                            label="Tipo de Cuenta"
                                            value={value}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            ))}
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="entidadBancaria"
                                control={formularioVerificacionBeneficiariosPago.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Entidad Bancaria *"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        readOnly={mostrarInfoPlanillaPago}
                                        onChange={value => {
                                            onChange(value);
                                        }}
                                    >
                                        {entidadesBancarias?.map(
                                            (option: ENTIDADBANCARIA) => {
                                                const dato = `${option.idEntidadBancaria}_${option.nombreRazonSocial}`;
                                                return (
                                                    <MenuItem
                                                        key={
                                                            option.idEntidadBancaria
                                                        }
                                                        value={dato}
                                                    >
                                                        {option.nombreRazonSocial}
                                                    </MenuItem>
                                                );
                                            },
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="numeroCuentaBeneficiario"
                                control={formularioVerificacionBeneficiariosPago.control}
                                rules={{
                                    maxLength: {
                                        value: 21,
                                        message: 'La longitud máxima del campo es de 21 caracteres',
                                    },
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        onPasteCapture={e => {
                                            e.preventDefault();
                                        }}
                                        numberOnlyMask
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label='Número de Cuenta *'
                                        value={value}
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                        isReadOnly={mostrarInfoPlanillaPago}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="valor"
                                control={formularioVerificacionBeneficiariosPago.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        numberMask
                                        currencyMask
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        label="Valor a Retirar *"
                                        value={value}
                                        onChange={onChange}
                                        InputLabelProps={{ shrink: true }}
                                        isReadOnly={mostrarInfoPlanillaPago}
                                    />
                                )}
                            />
                        </Grid>
                    </>
                </Grid>
            </CHPaper>
            {mostrarInfoPlanillaPago && (
                <CHPaper title='Información Planilla de Pago'>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="cdp"
                                control={formularioInfoPlanillaPago?.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="CDP"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}

                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="numeroFactura"
                                control={formularioInfoPlanillaPago?.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Número Factura"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}

                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Button variant="contained" color="primary" style={{ marginTop: '10px' }} onClick={obtenerPlanillaPdf}>
                                DESCARGAR PLANILLA DE PAGO
                            </Button>
                        </Grid>
                    </Grid>
                </CHPaper>
            )}
            <ObservacionesSolicitud
                control={formularioObservaciones}
                seccion={seccion}
                trazabilidad={trazabilidad} />
        </>
    )
};

export default connect()(FormCuentasAfectadas);