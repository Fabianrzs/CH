
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import NumberFormat from 'react-number-format';
import {
    Button,
    createStyles,
    Dialog,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import ApiproductoClients from '../../../../services/api-producto.service';
import NotificationService from '../../../../services/notification.service';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import UtilService from '../../../../utils/util.service';


const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
            marginTop: '1rem'
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        columnDate: {
            minWidth: '110px',
        },
        input: {
            boxSizing: 'border-box',
            height: '2em'
        }
    }),
);

type formValues = {
    tipoPago: string | null,
    idNovedad: string | null,
    valorPago: number | null,
    aplicarPago: boolean
};


export const ModalPagoNovedad = (props: any) => {
    const classes = useStyles();
    const {
        open,
        handleClose,
        producto,
        productoData,
        novedad,
        clienteRequest,
        idCredito,
        startGetProductoByIdentification,
        startGetProductosByIdentification
    } = props;

    const form = useForm<formValues>({
        mode: 'onChange',
        defaultValues: {
            tipoPago: 'TOTAL',
            aplicarPago: false
        }
    });

    const cartera = ApiproductoClients();

    const [tipoPago, setTipoPago] = useState<string>('TOTAL');
    const [resultadoPago, setResultadoPago] = useState<any>(null);
    const [pagoAplicado, setPagoAplicado] = useState<any>(false);


    useEffect(() => {
        form.reset();
        setResultadoPago(null);
        setPagoAplicado(false);

        if(novedad) {
            setValues(novedad);
        }

    }, [novedad])

    const setValues = (novedad: any) => {
        form.setValue('idNovedad', novedad?.idNovedad);
        form.setValue('tipoPago', 'TOTAL');
        form.setValue('valorPago', productoData?.saldoTotalCorte);
    };

    const handleRealizaPagoNovedad = async(data: any) => {

        if(UtilService.cleanCurrencyValue(form.watch('valorPago')) === 0){
            form.setError('valorPago', { type: "focus", message: "El valor a pagar debe ser mayor que cero" }, { shouldFocus: true });
            return;
        }
        form.clearErrors('valorPago');

        swal({
            title: 'Realizar Pago',
            text: '¿ Está seguro ?',
            icon: 'warning',
            buttons: {
                confirm: { text: 'SI', value: true, visible: true },
                cancel: { text: 'NO', value: false, visible: true }
            },
        })
        .then(async (result) => {
            if(result) {
                const request = {
                    idNovedad: data.idNovedad,
                    valorPago: data.valorPago,
                    aplicarPago: data.aplicarPago,
                    fechaAplicacion: novedad?.fechaNovedad
                  //  fechaAplicacion: UtilService.getFullDateComplete(novedad?.fechaNovedad)
                };

        
                await cartera
                .postAplicarPagoNovedadCartera(request)
                .then(async (response) => {
                    if (response) {
                        if (response.isSuccess) {

                            if(data.aplicarPago) {
                                setPagoAplicado(true);
                            }

                            setResultadoPago(response.data);
                            startGetProductoByIdentification(clienteRequest?.idTipoIdentificacion || 0,clienteRequest?.identificacion,idCredito);
                            startGetProductosByIdentification(clienteRequest?.idTipoIdentificacion || 0,clienteRequest?.identificacion);
                            await NotificationService.genericAlert(
                             `${data.aplicarPago ? 'Pago' : 'Simulación'} con éxito`,
                                'Se aplico el pago a la novedad'
                            );
                        } else {
                            await NotificationService.genericAlertError(
                                `${data.aplicarPago ? 'Pago' : 'Simulación'} con error`,
                                response?.returnMessage ?? 'No se pudo aplicar el pago a la novedad'
                            );
                      }
                    }
                }
                ).catch(() => {
                    toast("Error del sistema", { type: toast.TYPE.ERROR });
                });
            }
        });
    }

    const validarValor = () => {
        let mensaje = '';
       
        if(UtilService.cleanCurrencyValue(form.watch('valorPago')) > productoData?.saldoTotalCorte){
            mensaje = `El valor no debe ser mayor o igual al valor ${UtilService.formatCurrency(productoData?.saldoTotalCorte)}`
        }
        return mensaje !== '' ? mensaje : true;
    }

    return (
        <div>
            <Dialog
                fullScreen
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <CHPaper title="REALIZAR PAGO">
                    <Grid container style={{ width: '80%', display: 'flex', margin: 'auto', marginTop: '90px' }} >   
                        <Grid container>
                            <Grid item xs={12} md={6} className={classes.column}  >
                                <Controller
                                    name="tipoPago"
                                    control={form.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo de Pago *"
                                            value={value || ""}
                                            helperText={error ? error.message : null}
                                            onChange={(e) => {
                                                const value = (e.target.value as (string | null)) ?? 'TOTAL';
                                                setTipoPago(value);
                                                onChange(e);
                                                if(value === 'TOTAL'){
                                                    form.setValue('valorPago', productoData?.saldoTotalCorte);
                                                }else{
                                                    form.setValue('valorPago', 0);
                                                }
                                            }}>
                                                <MenuItem value="PARCIAL">
                                                    PARCIAL
                                                </MenuItem>
                                                <MenuItem value="TOTAL">
                                                    TOTAL
                                                </MenuItem>
                                        </CHSelect>
                                    )}
                                />
                            </Grid>

                            <Grid className={classes.column} item xs={12} md={6}>
                                <Controller
                                        name="valorPago"
                                        control={form.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            validate: validarValor
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <NumberFormat
                                                thousandSeparator=','
                                                decimalSeparator='.'
                                                prefix='$'
                                                decimalScale={2}
                                                value={value}
                                                customInput={CHTextField}
                                                onValueChange={(values) => {
                                                    onChange(values.floatValue);
                                                }}
                                                label="Valor a pagar"
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                    className: classes.input
                                                }}
                                                error={!!error}
                                                helperText={error?.message}
                                                isReadOnly={tipoPago === 'TOTAL'}
                                            />
                                        )}
                                    />
                            </Grid>
                        </Grid>

                        { !pagoAplicado &&
                            <Grid container>
                                <Controller
                                    name="aplicarPago"
                                    control={form.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHCheckbox
                                            label="¿Aplicar Pago?*"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        }

                        { resultadoPago != null &&
                            <>
                                <Typography className={classes.titleform}  variant="h5" component="h6">
                                    VALORES APLICADOS
                                </Typography>

                                <Grid container>
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <CHTextField
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right',
                                                },
                                                className: classes.input
                                            }}
                                            label="Seguro de vida"
                                            value={resultadoPago?.abonoSeguroVida ?? ''}
                                            pesosEnterosMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    </Grid>

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <CHTextField
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right',
                                                },
                                                className: classes.input
                                            }}
                                            label="Seguro de hogar"
                                            value={resultadoPago?.abonoSeguroHogar ?? ''}
                                            pesosEnterosMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    </Grid>

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <CHTextField
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right',
                                                },
                                                className: classes.input
                                            }}
                                            label="Capital"
                                            value={resultadoPago?.abonoCapital ?? ''}
                                            pesosEnterosMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    </Grid>

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <CHTextField
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right',
                                                },
                                                className: classes.input
                                            }}
                                            label="Interes Corrientes"
                                            value={resultadoPago?.abonoInteresCorriente ?? ''}
                                            pesosEnterosMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    </Grid>

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <CHTextField
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right',
                                                },
                                                className: classes.input
                                            }}
                                            label="Intereses de mora"
                                            value={resultadoPago?.abonoInteresMora ?? ''}
                                            pesosEnterosMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    </Grid>

                                    {
                                        (producto.idTipoCredito === 1) && 
                                            <>
                                                <Grid className={classes.column} item xs={12} md={3}>
                                                    <CHTextField
                                                        isReadOnly
                                                        inputProps={{
                                                            style: {
                                                                textAlign: 'right',
                                                            },
                                                            className: classes.input
                                                        }}
                                                        label="Capital Remanente"
                                                        value={resultadoPago?.capitalRemanente ?? ''}
                                                        pesosEnterosMask
                                                        InputLabelProps={{ shrink: true }}
                                                    />
                                                </Grid>

                                                <Grid className={classes.column} item xs={12} md={3}>
                                                    <CHTextField
                                                        isReadOnly
                                                        inputProps={{
                                                            style: {
                                                                textAlign: 'right',
                                                                boxSizing: 'border-box'
                                                            },
                                                            className: classes.input
                                                        }}
                                                        label="Interés Remanente"
                                                        value={resultadoPago?.interesRemanente ?? ''}
                                                        pesosEnterosMask
                                                        InputLabelProps={{ shrink: true }}
                                                    />

                                                </Grid>    

                                            </>
                                    }

                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <CHTextField
                                            isReadOnly
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right',
                                                },
                                                className: classes.input
                                            }}
                                            label="Valor Pago"
                                            value={resultadoPago?.valorPago ?? ''}
                                            pesosEnterosMask
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    </Grid>
                                </Grid>
                            </>
                        }

                        <Grid container>
                            <Grid item style={{ marginTop: '100px' }}>
                                { !pagoAplicado &&
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        style={{ marginRight: "20px" }}
                                        onClick={form.handleSubmit(handleRealizaPagoNovedad)}>
                                        REALIZAR PAGO
                                    </Button>
                                }
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="default"
                                    onClick={handleClose}
                                >
                                    ATRÁS
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                </CHPaper>
            </Dialog>
        </div>
    );
};