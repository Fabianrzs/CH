import React, { useEffect, useState } from 'react';
import { Grid, makeStyles, createStyles, Button, Dialog, DialogContent, DialogTitle, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import CHTextFieldV2 from '../../../components/ch-text-field/CHTextFieldV2';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { AfiliacionesExtraordinariasDTO, AfiliacionesExtraordinariasDTOInicial } from '../../../types/typeAffiliate.type';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CHDatePickerV2 from '../../../components/ch-datepicker/CHDatepickerV2';
import { AportesCategoriaAfiliaciones } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import '../affiliateExtraordinary.styles.scss';
import NotificationService from '../../../services/notification.service';
import { NotificationCardModel, NotificationCardModelEnum } from '../../../models/requests/notification-cards-model';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '120px',
            marginLeft: '.5rem',
            borderRadius: '0%',
            background: '#fec917',
            color: '#1c3b90',
            textAlign: 'right',
            align: 'right',
            marginBottom: '20px',

        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },

        icon: {
            padding: '10px',
            textAlign: 'inherit',
            color: '#1C3F8B',
        },

        buttons: {
            color: '#fff',
            width: '120px',
            textAlign: 'center',
            borderRadius: '0%',
            background: '#42b7ef',
            marginBottom: '20px',
            marginLeft: '38rem',
        }

        , title: {
            textAlign: 'center',
            background: '#42b7ef',
            color: '#fff',
        }

        , table: {
            color: '#f1c3b90ff',
        },
        textField: {
            '& p':{
              color:'blue',
            },
          },

    }),
);

function EditAportes({
    isOpen,
    onClose,
    data,
    setData,
    asignacionBasicaMensual,
    calidad,
    porcentaje,
    idTipoAfiliacionExtraordinaria
}: {
    isOpen: boolean;
    onClose: () => void;
    data: any;
    setData: (data: any) => void;
    asignacionBasicaMensual: number;
    calidad: number;
    porcentaje: string;
    idTipoAfiliacionExtraordinaria: number
}) {
    const { control, setValue, watch, handleSubmit } = useForm<AportesCategoriaAfiliaciones>()
    const handleClose = () => {
        onClose();
    };

    useEffect(() => {
        setHasLoadedFields(false);
    }, [isOpen])
    const [hasLoadedFields, setHasLoadedFields] = useState(false);

    useEffect(()  => {
        const showMessage = async () => {
            await NotificationService.genericAlert(
                'Información',
                `El valor de esta cuota es de ${  porcentaje  } de la asignación básica mensual`,
                'warning'
            );
        } 
        
        if(watch('derechoLiquidacionCuotas')){
            showMessage()
        }
    }, [watch('derechoLiquidacionCuotas')])

    if (!hasLoadedFields && data != null) {
        setHasLoadedFields(true);
        const fields = Object.keys(data);
        fields.forEach(field => {
            const value = data[field as keyof AportesCategoriaAfiliaciones];
            setValue(field as any, value);
        });
    }

    const classes = useStyles();

    const validateDate = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (date > moment()) {
            message = "La fecha del último aporte debe ser menor a la fecha actual"
        }

        return message !== '' ? message : true;
    }

    const validateFechaPrimerAporte = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (date > moment()) {
            message = "La fecha del primer aporte debe ser menor a la fecha actual"
        } else if (date > moment(watch('apC_FECHA_PRIMERA_CUOTA'))) {
            message = "La fecha del primer aporte debe ser menor a la fecha de primer aporte"
        } else if (date > moment(watch('apC_FECHA_ULTIMA_COUTA'))) {
            message = "La fecha del primer aporte debe ser menor a la fecha de último aporte"
        }
        return message !== '' ? message : true;
    }

    const validateDatePeriodo = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (data > new Date()) {
            message = "La fecha debe ser menor a la fecha actual"
        } else if (date > moment(watch('periodoAportesHasta'))) {
            message = "La fecha de período aportes desde debe ser mayor a fecha período aportes hasta"
        }

        return message !== '' ? message : true;
    }

    const validateDatePeriodoHasta = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (data > new Date()) {
            message = "La fecha debe ser menor a la fecha actual"
        } else if (date < moment(watch('periodoAportesDesde'))) {
            message = "La fecha de período aportes desde debe ser mayor a fecha período aportes hasta"
        }

        return message !== '' ? message : true;
    }

    const submitAportes = (data: any) => {
        setData(data);
    }

    return (
        <Dialog className={classes.table}
            fullWidth
            maxWidth="md"
            open={isOpen}
            aria-labelledby="form-dialog-title"

        >
            <DialogTitle className={classes.title} id="form-dialog-title">EDITAR APORTES</DialogTitle>
            <DialogContent>
                <Grid item container>
                {idTipoAfiliacionExtraordinaria === 2 || idTipoAfiliacionExtraordinaria === 4 || idTipoAfiliacionExtraordinaria === 6
                    || idTipoAfiliacionExtraordinaria === 8 || idTipoAfiliacionExtraordinaria === 10 || idTipoAfiliacionExtraordinaria === 12
                    || idTipoAfiliacionExtraordinaria === 14 || idTipoAfiliacionExtraordinaria === 16 || idTipoAfiliacionExtraordinaria === 18
                    || idTipoAfiliacionExtraordinaria === 20 ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="apC_FECHA_PRIMERA_CUOTA"
                                control={control}
                    
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePickerV2
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Fecha De Primer Aporte"
                                        onChange={onChange}
                                        value={moment(value).format('yyyy/MM/DD')}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {idTipoAfiliacionExtraordinaria === 2 || idTipoAfiliacionExtraordinaria === 4 || idTipoAfiliacionExtraordinaria === 6
                    || idTipoAfiliacionExtraordinaria === 8 || idTipoAfiliacionExtraordinaria === 10 || idTipoAfiliacionExtraordinaria === 12
                    || idTipoAfiliacionExtraordinaria === 14 || idTipoAfiliacionExtraordinaria === 16 || idTipoAfiliacionExtraordinaria === 18
                    || idTipoAfiliacionExtraordinaria === 20 ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="apC_FECHA_ULTIMA_COUTA"
                                control={control}
                           
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePickerV2
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Fecha De Último Aporte"
                                        onChange={onChange}
                                        value={moment(value).format('yyyy/MM/DD')}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="apC_CUOTAS_ACUMULADAS"
                                rules={{
                                    required: 'Este campo es requerido',
                                    min: {
                                        value: 0,
                                        message: 'El valor de número de cuotas mínimo es 0'
                                    },
                                    max: {
                                        value: 168,
                                        message: 'El valor de número de cuotas máximo es 168'
                                    },
                                    pattern: {
                                        value: /^[\d]*$/g,
                                        message: "No se permiten decimales"
                                    }
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextFieldV2
                                        label="Número De Cuotas"
                                        onChange={onChange}
                                        error={!!error}
                                        value={value}
                                        isReadOnly
                                        type="number"
                                        helperText={
                                            error ? error.message : null
                                        }

                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="ratificacionCuotas"
                                rules={{
                                    min: {
                                        value: 0,
                                        message: 'El valor de número de cuotas mínimo es 0'
                                    },
                                    max: {
                                        value: 168,
                                        message: 'El valor de número de cuotas máximo es 168'
                                    },
                                    pattern: {
                                        value: /^[\d]*$/g,
                                        message: "No se permiten decimales"
                                    }
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextFieldV2
                                        label="Ratificación De Cuotas"
                                        onChange={onChange}
                                        value={value === 0 ? null : value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        numberMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                                <Controller
                                    name="observaciones"
                                    control={control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextFieldV2
                                            label="Observaciones"
                                            onChange={onChange}
                                            value={value}
                                            multiline
                                            maxRows={5}
                                            inputProps={{
                                                style: {
                                                    textTransform: 'uppercase',
                                                },
                                            }}
                                        />
                                    )}
                                />
                            
                        </Grid>
                    </Grid>
                    {idTipoAfiliacionExtraordinaria === 2 || idTipoAfiliacionExtraordinaria === 4 || idTipoAfiliacionExtraordinaria === 6
                    || idTipoAfiliacionExtraordinaria === 8 || idTipoAfiliacionExtraordinaria === 10 || idTipoAfiliacionExtraordinaria === 12
                    || idTipoAfiliacionExtraordinaria === 14 || idTipoAfiliacionExtraordinaria === 16 || idTipoAfiliacionExtraordinaria === 18
                    || idTipoAfiliacionExtraordinaria === 20 ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="liquidacionCuotaFondoSolidaridad"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="¿Liquidar Cuota Fondo Solidaridad?"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {watch("liquidacionCuotaFondoSolidaridad") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <CHTextFieldV2
                                label="Liquidación Fondo Solidaridad"
                                value={UtilService.formatCurrency(asignacionBasicaMensual * 0.07)}
                                inputProps= {{style: {textAlign: 'right'}}}
                                isReadOnly
                            />
                        </Grid>
                    </Grid>) : null}
                    {idTipoAfiliacionExtraordinaria === 2 || idTipoAfiliacionExtraordinaria === 4 || idTipoAfiliacionExtraordinaria === 6
                    || idTipoAfiliacionExtraordinaria === 8 || idTipoAfiliacionExtraordinaria === 10 || idTipoAfiliacionExtraordinaria === 12
                    || idTipoAfiliacionExtraordinaria === 14 || idTipoAfiliacionExtraordinaria === 16 || idTipoAfiliacionExtraordinaria === 18
                    || idTipoAfiliacionExtraordinaria === 20 ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="reintegroAportes"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="¿Reintegro Aportes?"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {idTipoAfiliacionExtraordinaria === 2 || idTipoAfiliacionExtraordinaria === 4 || idTipoAfiliacionExtraordinaria === 6
                    || idTipoAfiliacionExtraordinaria === 8 || idTipoAfiliacionExtraordinaria === 10 || idTipoAfiliacionExtraordinaria === 12
                    || idTipoAfiliacionExtraordinaria === 14 || idTipoAfiliacionExtraordinaria === 16 || idTipoAfiliacionExtraordinaria === 18
                    || idTipoAfiliacionExtraordinaria === 20 ? null : (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="derechoLiquidacionCuotas"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="¿Tiene Derecho Liquidación Cuotas?"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>)}
                    {watch("derechoLiquidacionCuotas") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="periodoAportesDesde"
                                rules={{
                                    validate: () => watch('periodoAportesDesde') > watch('periodoAportesHasta') ? 'El período de aportes desde debe ser menor al período de aportes hasta' : true
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePickerV2
                                        views={['year', 'month']}
                                        defaultValue=""
                                        label="Período Aportes Desde"
                                        onChange={onChange}
                                        value={moment(value).startOf('month')}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        format="MM/yyyy"
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {watch("derechoLiquidacionCuotas") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="periodoAportesHasta"
                                control={control}
                                rules={{
                                    validate: () => watch('periodoAportesDesde') > watch('periodoAportesHasta') ? 'El período de aportes desde debe ser menor al período de aportes hasta' : true
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePickerV2
                                        views={['year', 'month']}
                                        defaultValue=""
                                        label="Período Aportes Hasta"
                                        onChange={onChange}
                                        value={moment(value).endOf('month')}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        format="MM/yyyy"
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {watch ("derechoLiquidacionCuotas") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="valorDerechoLiquidacionCuotas"
                                control={control}
                                render={({
                                    field: { onChange },
                                }) => (
                                    <CHTextFieldV2
                                        isReadOnly
                                        className={classes.textField}
                                        label="Valor Derecho Liquidación Cuotas"
                                        value={calidad === 1 ? Math.round(moment(watch('periodoAportesHasta')).diff(watch('periodoAportesDesde'), 'months', true)) * asignacionBasicaMensual * 0.08 : Math.round(moment(watch('periodoAportesHasta')).diff(watch('periodoAportesDesde'), 'months', true)) * asignacionBasicaMensual * 0.045}
                                        onChange={onChange}
                                        currencyMask
                                        inputProps= {{style:{textAlign:'right'}}}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>): null}
                    {watch ("derechoLiquidacionCuotas") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="numeroCuotasLiquidacion"
                                control={control}
                                render={({
                                    field: { onChange },
                                    fieldState: { error }
                                }) => (
                                    <CHTextFieldV2
                                        isReadOnly
                                        error={!!error}
                                        label="Número De Cuotas (Meses) A Liquidar"
                                        value={Math.round(moment(watch('periodoAportesHasta')).diff(watch('periodoAportesDesde'), 'months', true)) }
                                        onChange={onChange}
                                        helperText={error ? error.message : null}

                                    />
                                )}
                            />
                        </Grid>
                    </Grid>): null}
                    <Grid>
                        <div className="buttons">
                            <Button
                                className={classes.buttons}
                                type="button"
                                onClick={handleClose}
                            > 
                                REGRESAR
                            </Button>
                            <Button
                                className={classes.button}
                                onClick={handleSubmit(submitAportes)}
                                type="submit"
                            >
                                ACTUALIZAR
                            </Button>
                        </div>
                    </Grid>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}

export default connect()(EditAportes);


