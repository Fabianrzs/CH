import React, { useState, useEffect } from 'react';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import CuentasReintegros from './cuentas-reintegros';
import { TIPO_REINTEGRO, tipo_valor } from './constants/reintegros-constants';
import UtilService from '../../../../utils/util.service';
import ApiReintegro from '../../../../services/api-reintegros.service';
import NotificationService from '../../../../services/notification.service';
import { LoadingComponent } from '../../../../components/loading/loading.component';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        window: {
            width:'60%',
            marginLeft: '20%'
        },
    }),
);

export const ModalAgregarReintegro = (props: any) => {
    const classes = useStyles();
    const {open,
           setOpen,
           tipoValor,
           formulario,
           catalogState,
           cuentas,
           idReintegro,
           setTipoValor,
           loadingAplicarReintegros,
           obtenerReintegrosPorId,
           eliminarRegistrosTablas,
           mostrarCampoObservacion,
           setMostrarCampoObservacion,
           consultarCuentaConcepto,
           handleDeclararInconsistente,
           setListaDevolucionRecursosSeleccionada,
           } = props;

    const apiReintegro = ApiReintegro();
    const [listaConceptosSeleccionada,setListaConceptosSeleccionada] = useState<any []>([]);
    const [saldoMayor,setSaldoMayor] = useState(false);
    const [deshabilitarBotonLimpiar,setDeshabilitarBotonLimpiar] = useState(false);
    const [funcionAplicada,setFuncionAplicada]= useState(0);
    const [loading,setLoading]= useState(false);

    useEffect(() => { 

        let conceptosDeshabilitados = 0;
            cuentas?.forEach((item: any) => {
                if(item.conceptos.length === 0 || 
                  (item?.estadoCuenta === 'CANCELADA' && item?.saldo === 0)||
                   item?.saldo === 0){
                    conceptosDeshabilitados += 1;
                }
        })

        if(conceptosDeshabilitados === cuentas.length){
         setDeshabilitarBotonLimpiar(true);
        }
        else if(formulario.watch('estado') === 'PROCESADO'){
         setDeshabilitarBotonLimpiar(true);
         formulario.setValue('tipoValor','');
        }else{
         setDeshabilitarBotonLimpiar(false);
        }
    },[formulario.watch('estado'),cuentas])

    const guardar = async(data: any) => {

        formulario.clearErrors('tipoValor');
        formulario.clearErrors('valorSolicitado');
       
        eliminarRegistrosTablas();
        let tipoReintegro = 0;
        let valorSolicitado = 0;

        const observacion = data.observationAfiliado.trim();
        // Si es inconsistente
        if(observacion !== ''){

            formulario.register('tipoValor',{
                required: {
                    value: false,
                    message: '',
                },
            })

            formulario.register('valorSolicitado',{
                required: {
                    value: false,
                    message: '',
                },
            })

            const confirmAlert = await NotificationService.confirmAlert(
                `¿Está seguro que desea realizar esta acción?`,
            );

            if(confirmAlert){
                limpiarConceptos();
                formulario.register('tipoValor', {
                    required: {
                        value: false,
                        message: '',
                    },
                });
                formulario.register('valorSolicitado', {
                    required: {
                        value: false,
                        message: '',
                    },
                });

                const afiliadoActualizado = {
                    id_Afiliado: data.id,
                    numeroDocumento: data.numeroDocumento,
                    nombres: data.nombres,
                    tipoDocumentoId: data.tipoDocumento,
                    apellidos: data.apellidos,
                    idReintegro: data.idReintegro,
                    id_Tipo_Reintegro: TIPO_REINTEGRO.INCONSISTENTES,
                    valorSolicitado: valorSolicitado,
                    estadoAfiliao: 'PROCESADO',
                    observationAfiliado: data.observationAfiliado,
                    tipoValor: data.tipoValor,
                }
                setLoading(true);
                await apiReintegro
                .postActualizarAfiliadoReintegro(afiliadoActualizado)
                .then(async response => {
                    if(response.data){
                        setLoading(false);
                    }
                })
                .catch(() => {
                    setLoading(false);
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
            }
            handleOnClose();
        }
        else{
            setLoading(true);
            setMostrarCampoObservacion(false);
            if(data.tipoValor === ''){
                formulario.setError('tipoValor', {type: 'custom', message: 'Este campo es requerido'});
                setLoading(false);
                return;
            }
    
            if(data.valorSolicitado === ''){
                formulario.setError('valorSolicitado', {type: 'custom', message: 'Este campo es requerido'});
                setLoading(false);
                return;
            }

            tipoReintegro = data.tipoReintegro;

            let valorSolicitadoMayorEnBloqueos = false;
            let valorSolicitadoMayorEnDesbloqueos = false;
            let valorSolicitadoMayorEnDevolucionRecursos= false;

            if(data.tipoValor === 'VALOR'){
                
                if(String(data.valorSolicitado).includes('$')){
                    valorSolicitado = UtilService.cleanCurrencyValue(data.valorSolicitado);
                }else{
                    valorSolicitado = data.valorSolicitado
                }

                listaConceptosSeleccionada.forEach(item => {

                    // Validación valor solicitado en Bloqueos
                    if( data.tipoReintegro === TIPO_REINTEGRO.BLOQUEOS  &&
                        valorSolicitado > 0 &&
                        valorSolicitado > item.saldo){
                        valorSolicitadoMayorEnBloqueos = true;
                    }

                    // Validación valor solicitado en Desbloqueos
                    if(
                        data.tipoReintegro === TIPO_REINTEGRO.DESBLOQUEOS  &&
                        valorSolicitado > 0 && 
                        valorSolicitado > item.valorBloqueoEmbargo){
                        valorSolicitadoMayorEnDesbloqueos =  true;
                    }

                    // Validación valor solicitado en Devolucion de recursos
                    if(data.tipoReintegro === TIPO_REINTEGRO.DEVOLUCION_DE_RECURSOS  &&
                        valorSolicitado > 0 &&
                        valorSolicitado > item.valorBloqueoEmbargo + item.saldo){
                        valorSolicitadoMayorEnDevolucionRecursos = true;
                    }
                })

            }else{
                valorSolicitado = Number(data.valorSolicitado.replace('%',''));
            }

             // Validación valor solicitado mayor a valor disponible en bloqueos
             if(valorSolicitadoMayorEnBloqueos){
                setListaConceptosSeleccionada([]);
                setFuncionAplicada(3);
                setSaldoMayor(true);
                formulario.setError('valorSolicitado', {type: 'custom', message: 'El valor no puede ser mayor al valor disponible'});
                setLoading(false);
                return;
            }

            // Validación  valor solicitado mayor a valor disponible en desbloqueos
            if(valorSolicitadoMayorEnDesbloqueos){
                setListaConceptosSeleccionada([]);
                setSaldoMayor(true);
                formulario.setError('valorSolicitado', {type: 'custom', message: 'El valor no puede ser mayor al valor bloqueo'});
                setLoading(false);
                return;
            }

            // Validación  valor solicitado mayor a valor bloqueo + valor disponible
            if(valorSolicitadoMayorEnDevolucionRecursos){
                setListaConceptosSeleccionada([]);
                setSaldoMayor(true);
                formulario.setError('valorSolicitado', {type: 'custom', message: 'El valor no puede ser mayor al valor bloqueo más disponible '});
                setLoading(false);
                return;
            }

            const afiliadoActualizado = {
                id_Afiliado: data.id,
                numeroDocumento: data.numeroDocumento,
                nombres: data.nombres,
                tipoDocumentoId: data.tipoDocumento,
                apellidos: data.apellidos,
                idReintegro: data.idReintegro,
                id_Tipo_Reintegro: tipoReintegro,
                valorSolicitado: valorSolicitado,
                estadoAfiliao: data.estado,
                observationAfiliado: data.observationAfiliado,
                tipoValor: data.tipoValor,
            }
            
                if(listaConceptosSeleccionada && valorSolicitado === 0){
                    obtenerReintegrosPorId(idReintegro);
                    await NotificationService.genericAlert(
                        'Atención',
                        `El Valor Solicitado debe ser mayor a cero`,
                        'warning',
                    );
                    setLoading(false);
                    return;
                }

                if(listaConceptosSeleccionada.length === 0 && valorSolicitado !== 0){
                    obtenerReintegrosPorId(idReintegro);
                    await NotificationService.genericAlert(
                        'Atención',
                        `Debe seleccionar conceptos de la cuenta`,
                        'warning',
                    );
                    setLoading(false);
                    return;
                }

                formulario.clearErrors('valorSolicitado');
                setSaldoMayor(false);
                funcionActualizarConceptos(afiliadoActualizado,data,valorSolicitado);
        }
    }

    const funcionActualizarConceptos = async(afiliadoActualizado: any, data: any, valorSolicitado: number ) => {
        await apiReintegro
        .postActualizarAfiliadoReintegro(afiliadoActualizado)
        .then(async response => {
            if (response.data) {

                const arrayConceptos: any[] = [];

                listaConceptosSeleccionada.forEach(item => {
                    const nuevoConcepto = {
                        idAfiliado: data.id,
                        cuentaId: item?.cuentaId,
                        conceptoId: item?.conceptoId,
                        valorSueldo: valorSolicitado,
                        tipoValor: data.tipoValor,
                        tipoReintegro: formulario.getValues('tipoReintegro')
                    }
                    arrayConceptos.push(nuevoConcepto);
                })

                if(arrayConceptos){
                    await apiReintegro
                    .postActualizarSaldoConceptoCuenta(arrayConceptos)
                    .then(async response => {
                        if(response){
                            setListaConceptosSeleccionada([]);
                            setFuncionAplicada(1);
                            await NotificationService.genericAlert(
                                'Exitoso!',
                                'Se realizó la petición con éxito.',
                            );
                            setLoading(false);
                        }
                    })
                    .catch(() => {
                        setLoading(false);
                        toast('Error del sistema', { type: toast.TYPE.ERROR });
                    });
                }
               consultarCuentaConcepto(data.tipoDocumento,String(data.numeroDocumento));
            }
        })
        .catch(() => {
            setLoading(false);
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
    }

    const limpiarConceptos = async() => {
        setLoading(true);
        formulario.clearErrors('tipoValor');
        formulario.clearErrors('valorSolicitado');

        const conceptos: any[] = [];
        if(cuentas){
            cuentas.forEach((cuenta: any) => {
                cuenta.conceptos?.forEach((concepto: any) => {
                    if(cuenta.cuentaId === concepto.cuentaId){
                        conceptos.push(concepto);
                    }
               })
            })
        }

        const limpiarConceptos = conceptos.map((item: any) => (
            {
                cuentaId: item.cuentaId,
                conceptoId: item.conceptoId,
                idAfiliado: formulario.getValues('id'),
                idReintegro: formulario.getValues('tipoReintegro'),
            }
        ))

        await apiReintegro
        .postLimpiarSaldoConceptoCuenta(limpiarConceptos)
        .then(async response => {
            if(response.data){
                setListaConceptosSeleccionada([]);
                setFuncionAplicada(2);
                setSaldoMayor(false);
                consultarCuentaConcepto(Number(formulario.getValues('tipoDocumento')),String(formulario.getValues('numeroDocumento')));
                formulario.setValue('valorSolicitado','');
                formulario.setValue('tipoValor','');
                await NotificationService.genericAlert(
                    'Exitoso!',
                    'Información restaurada con éxito.',
                );
                setLoading(false);
            }
        })
        .catch(() => {
            setLoading(false);
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
    }

    const handleOnClose = () => {
        formulario.reset();
        setListaConceptosSeleccionada([]);
        setOpen(false);
        obtenerReintegrosPorId(idReintegro);
        setMostrarCampoObservacion(false);
        setListaDevolucionRecursosSeleccionada([]);
    }

    return (
        <>
        <Dialog
            fullWidth
            maxWidth="lg"
            open={open}
            onClose={handleOnClose}
            aria-labelledby="form-dialog-title"
            className={classes.window}
        >
            <DialogContent>
                <Grid item>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        AGREGAR REINTEGRO
                    </Typography>
                </Grid>
                <br />
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                        <Controller
                            name='tipoDocumento'
                            control={formulario.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido.',
                                },
                            }}
                            render={({ 
                                field: { onChange, value },
                                fieldState: { error }, 
                            }) => (
                                <CHSelect
                                 error={!!error}
                                 label="Tipo Documento *"
                                 value={value}
                                 readOnly
                                 helperText={error ? error.message : null}
                                 onChange={onChange}
                             >
                                 {catalogState?.IdentificationType.map((option: IDENTIFICATIONTYPEDTO) => (
                                     <MenuItem
                                         key={option.id}
                                         value={option.id}
                                     >
                                         {option.descripcion}
                                 
                                     </MenuItem>
                                 ))} 
                             </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                        <Controller
                            name="numeroDocumento"
                            control={formulario.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido.',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Documento *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
                                        if(e.target.value.length <= 12){
                                            onChange(e);
                                        }
                                    }}
                               />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                        <Controller
                            name="nombres"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label= "Nombres"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textTransform:
                                                'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if (e.target.value.length <= 25)
                                            onChange(e);
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                    <Controller
                            name="apellidos"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label= "Apellidos"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textTransform:
                                                'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if (e.target.value.length <= 25)
                                            onChange(e);
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                        <Controller
                            name="unidadEjecutora"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Unidad Ejecutora"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textTransform:
                                                'uppercase',
                                        },
                                    }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                    <Controller
                            name="tipoValor"
                            control={formulario.control}
                            rules={{
                                required: {
                                    value: formulario.watch('observationAfiliado').trim() === '' && mostrarCampoObservacion,
                                    message: formulario.watch('observationAfiliado').trim() !== ''? '' : 'Este campo es requerido',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo Valor *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    readOnly={formulario.watch('estado') === 'PROCESADO'}
                                    onChange={e => {
                                        setTipoValor(String(e.target.value));
                                        formulario.setValue('valorSolicitado','');
                                        onChange(e);
                                    }}
                                >
                                {tipo_valor?.map(option => (
                                    <MenuItem
                                        key={option.descripcion}
                                        value={option.descripcion}
                                    >
                                        {option.descripcion}
                                
                                    </MenuItem>
                                ))} 
                            </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={10}>
                { tipoValor === 'VALOR'?
                    (  <Controller
                            name="valorSolicitado"
                            control={formulario.control}
                            rules={{
                                validate: () => saldoMayor === true? 'El valor no puede ser mayor al saldo disponible' : true
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Valor Solicitado *"
                                    currencyMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={formulario.watch('estado') === 'PROCESADO'}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={e => {
                                        setSaldoMayor(false);
                                        formulario.clearErrors('valorSolicitado');
                                        onChange(e);
                                    }}
                                />
                            )}
                       />
                    ) : (
                        <Controller
                            name="valorSolicitado"
                            control={formulario.control}
                            rules={{
                                validate: () => Number(formulario.watch('valorSolicitado').replace('%','')) < 0 || 
                                Number(formulario.watch('valorSolicitado').replace('%','')) > 100 ? 'El valor debe ser entre 0 y 100' : true
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Valor Solicitado *"
                                    percentMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={formulario.watch('estado') === 'PROCESADO'}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                           )}
                      />
                    )}
                    </Grid>
                </Grid>
                <br />
                    <LoadingComponent loading={loadingAplicarReintegros} />
                    <CuentasReintegros 
                       cuentas={cuentas}
                       tipoReintegro={formulario.watch('tipoReintegro')}
                       estadoAfiliadoEnEvaluacion={formulario.watch('estado') === 'EN EVALUACION'}
                       listaConceptosSeleccionada={listaConceptosSeleccionada}
                       funcionAplicada={funcionAplicada}
                       setListaConceptosSeleccionada={setListaConceptosSeleccionada} 
                    />
                <br />
                {loading?(
                    <LoadingComponent loading={loading} />
                ): (
                    <>
                    <Grid item xs={6}>
                    <Button
                        size="small"
                        variant="contained"
                        color="primary"
                        disabled={formulario.watch('estado') === 'PROCESADO'}
                        onClick={handleDeclararInconsistente}
                    >
                        Declarar Inconsistente
                    </Button>
                    </Grid>
                    <Grid container spacing={3}>
                    <Grid item xs={10}>
                        {
                            (mostrarCampoObservacion && (
                            <Controller
                                name="observationAfiliado"
                                control={formulario.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        multiline
                                        rows={4}
                                        inputProps={{
                                            style: {
                                                textTransform:
                                                    'uppercase',
                                            },
                                        }}
                                        onChange={onChange}
                                    />
                                )}
                              />
                            ))
                        }
                    </Grid>
                  </Grid>
                  <br />
                <br />
                <Grid container>
                <Grid className={classes.column} item xs={12} md={4} />
                    <Grid className={classes.column} item xs={12} md={2}>
                        <Button
                            size="small"
                            variant="contained"
                            color="primary"
                            disabled={formulario.watch('estado') === 'PROCESADO'}
                            onClick={formulario.handleSubmit(guardar)}
                        >
                         Guardar
                        </Button>
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={2}>
                    <Button
                        size="small"
                        variant="contained"
                        color="default"
                        disabled={deshabilitarBotonLimpiar}
                        onClick={limpiarConceptos}
                    >
                        Limpiar
                    </Button>
                   </Grid>
                    <Grid className={classes.column} item xs={12} md={2}>
                    <Button
                        size="small"
                        variant="contained"
                        color="default"
                        onClick={handleOnClose}
                    >
                        Salir
                    </Button>
                </Grid>
                </Grid>
                 </>
                )}
            </DialogContent>
        </Dialog>
        </>
    );
};