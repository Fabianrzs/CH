import React, { useEffect, useState } from 'react';
import { Grid, makeStyles, createStyles, Button, Dialog, DialogContent, DialogTitle, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CHDatePickerV2 from '../../../components/ch-datepicker/CHDatepickerV2';
import { DependienteCategoria } from '../../../types/client.model.type';
import AddressComponent from '../../../components/modals/address/address.component'
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import { Address } from '../../../types/address.type';
import NotificationService from '../../../services/notification.service';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';

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
        }

    }),
);

function EditAportes({
    isOpen,
    onClose,
    catalogState,
    data,
    numeroCuotasFallecido,
    setData,
    calidad,
    asignacionBasicaMensual,
    porcentaje,
    agregarBeneficiario,
    index,
    porcentajeAcumulado,
}: {
    isOpen: boolean;
    onClose: (value: any) => void;
    catalogState: CatalogState;
    data: DependienteCategoria;
    numeroCuotasFallecido: number;
    setData: (data: DependienteCategoria) => void;
    calidad: number;
    asignacionBasicaMensual: number,
    porcentaje: number,
    agregarBeneficiario: boolean,
    index: number,
    porcentajeAcumulado: number
}) {
    const { control, setValue, watch, handleSubmit, reset, getValues, setError, clearErrors } = useForm<DependienteCategoria>()
    const classes = useStyles();
    const [firstLoad, setFirstLoad] = useState(false);
   
    const [open, setOpen] = useState(false);

    const handleOpenAddress = () => {
        setOpen(true);
    };

    useEffect(()  => {
        const showMessage = async () => {
            await NotificationService.genericAlert(
                'Información',
                `El valor de esta cuota es de ${  porcentaje * 100  }% de la asignación básica mensual`,
                'warning'
            );
        } 
        
        if(watch('derechoLiquidacionCuotasBeneficiarios')){
            showMessage()
        }
    }, [watch('derechoLiquidacionCuotasBeneficiarios')])

    const onCloseAddress = (value: Address) => {

        if (value !== null) {
            const result = setFormDataAddres(value);
            setValue('direccion', result);
            setOpen(false);
        } else {
            setValue('direccion', '');
            setOpen(false);
        }
    };

    const handleClose = () => {
        const value: any = {};
        onClose(value);
    };

    useEffect(() => {
        setHasLoadedFields(false);
        setFirstLoad(false)
    }, [isOpen])
    const [hasLoadedFields, setHasLoadedFields] = useState(false);

    const [participacionTotal, setParticipacionTotal] = useState(0)

    if (agregarBeneficiario && !firstLoad) {
        setFirstLoad(true)
        reset()
    } else if (!hasLoadedFields && data != null) {
        setHasLoadedFields(true);
        const fields = Object.keys(data);
        fields.forEach(field => {
            const value = data[field as keyof DependienteCategoria];
            setValue(field as any, value);
        });

        const part = porcentajeAcumulado - getValues('participacion')

        setParticipacionTotal(part)

        setValue("periodoAportesDesdeBeneficiarios", new Date(moment(watch("periodoAportesDesdeBeneficiarios")).startOf("month").toString()))
        setValue("periodoAportesHastaBeneficiarios", new Date(moment(watch("periodoAportesHastaBeneficiarios")).endOf("month").toString()))
        setValue("index", index)
    }

    const validateDate = (data: Date) => {
        const date = moment(data);
        let message = '';
        if (!date.isValid()) {
            message = "Ingrese una fecha válida"
        } else if (data > new Date()) {
            message = "La fecha del último aporte debe ser menor a la fecha actual"
        }

        return message !== '' ? message : true;
    }

    const submit = (data: DependienteCategoria) => {
        const value: any = {};
        
        setData(data);
        onClose(value)
    }

    useEffect(() => {
        if(watch('participacion') != null){
            if(participacionTotal + +watch('participacion').toString().replace('%','') > 100){
                setError("participacion", { type: "focus" }, { shouldFocus: true })
            } else {
                clearErrors('participacion')
            }
        }
    }, [watch('participacion')])

    return (
        <Dialog className={classes.table}
            fullWidth
            maxWidth="md"
            open={isOpen}
            aria-labelledby="form-dialog-title"

        >
            <AddressComponent
                onOpen={open}
                onClose={onCloseAddress}
                control={control}
                getValues={watch}
                tipoDireccion
                setValue={setValue}
            />
            <DialogTitle className={classes.title} id="form-dialog-title">EDITAR BENEFICIARIO</DialogTitle>
            <DialogContent>
                <Grid item container>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="idTipoIdentificacion"
                                rules={{
                                    validate: () => watch('idTipoIdentificacion') < 1 || watch('idTipoIdentificacion') == null ? 'Este campo es obligatorio' : true
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        error={!!error}
                                        label="Tipo Identificación Beneficiario*"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}

                                    >
                                        {catalogState.IdentificationType?.map(
                                            (option: IDENTIFICATIONTYPEDTO) => (
                                                <MenuItem key={option.id} value={option.id}>
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="identificacion"
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
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Número Identificación Del Beneficiario*"
                                        onChange={onChange}
                                        value={value}
                                        numberMask
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
                                name="nombre"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Nombres y Apellidos Del Beneficiario*"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
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

                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="direccion"
                                
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Dirección Del Beneficiario"
                                        onChange={onChange}
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        onClick={handleOpenAddress}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}

                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="correo"
                                rules={{
                                    pattern: {
                                        value: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,
                                        message: 'El formato de correo es incorrecto'
                                    },
                                    required: 'Este campo es requerido.',
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Correo Del Beneficiario*"
                                        onChange={onChange}
                                        value={value}
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
                                name="telefono"
                                rules={{
                                    required: 'Este campo es requerido.',
                                    pattern: /\(?([3-3]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Celular Del Beneficiario*"
                                        onChange={onChange}
                                        value={value}
                                        helperText={error ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos' : null}
                                        cellPhoneMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                     <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="participacion"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: {error}
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? 'La participación de todos los beneficiarios no debe superar el 100%' : ''}
                                        label="Participación"
                                        onChange={onChange}
                                        value={value}
                                        percentMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    {/* <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="conservaAntiguedad"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="¿El Beneficiario Conserva Antigüedad?"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    {watch("conservaAntiguedad") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <CHTextField
                                label="Número De Cuotas Asociadas"
                                isReadOnly
                                value={numeroCuotasFallecido}
                            />
                        </Grid>
                    </Grid>) : null} */}
                    {/* <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="numeroActoAdministrativoResolucionesReconocimientoPensional"
                                rules={{
                                    required: 'Este campo es requerido',
                                    min: {
                                        value: 1,
                                        message: 'No se aceptan valores negativos'
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
                                    <CHTextField
                                        error={!!error}
                                        label="Número Acto Administrativo Resoluciones Reconocimiento Pensional*"
                                        onChange={onChange}
                                        value={value === 0 ? null : value}
                                        inputProps={{ style: { textAlign: 'right', padding: '16px' } }}
                                        InputLabelProps={{ shrink: true }}
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
                                name="fechaNotificacionResolucionesReconocimientoPensional"
                                rules={{
                                    validate: validateDate
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePickerV2
                                        label="Fecha Notificación Resoluciones Reconocimiento Pensional*"
                                        onChange={onChange}
                                        openTo="year"
                                        value={value}
                                        error={!!error}
                                        inputProps={{ style: { textAlign: 'right', padding: '16px' } }}
                                        InputLabelProps={{ shrink: true }}
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
                                name="porcentajeReconocimientoPensionalDistribucionAportes"
                                rules={{
                                    required: 'Este campo es requerido',
                                    min: {
                                        value: 1,
                                        message: 'No se aceptan valores negativos'
                                    },
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Porcentaje Reconocimiento Pensional Distribución Aportes*"
                                        onChange={onChange}
                                        inputProps={{ style: { textAlign: 'right' } }}
                                        value={value === 0 ? null : value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        percentMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="porcentajeReconocimientoPensionalDistribucionSubsidio"
                                rules={{
                                    required: 'Este campo es requerido',
                                    min: {
                                        value: 1,
                                        message: 'No se aceptan valores negativos'
                                    },
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Porcentaje Reconocimiento Pensional Distribución Subsidio*"
                                        onChange={onChange}
                                        inputProps={{ style: { textAlign: 'right' } }}
                                        value={value === 0 ? null : value}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        percentMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="numeroActoAdministrativoResolucionesReconocimientoPrestacionesSociales"
                                rules={{
                                    required: 'Este campo es requerido',
                                    min: {
                                        value: 1,
                                        message: 'No se aceptan valores negativos'
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
                                    <CHTextField
                                        error={!!error}
                                        label="Número Acto Administrativo Resoluciones Reconocimieno Prestaciones Sociales*"
                                        onChange={onChange}
                                        value={value === 0 ? null : value}
                                        inputProps={{ style: { textAlign: 'right', padding: '16px' } }}
                                        InputLabelProps={{ shrink: true }}
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
                                name="fechaNotificacionResolucionesReconocimientoPrestacionesSociales"
                                rules={{
                                    validate: validateDate
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHDatePickerV2
                                        label="Fecha Notificación Resoluciones Reconocimiento Prestaciones Sociales*"
                                        onChange={onChange}
                                        openTo="year"
                                        value={value}
                                        error={!!error}
                                        inputProps={{ style: { textAlign: 'right', padding: '16px' } }}
                                        InputLabelProps={{ shrink: true }}
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
                                name="porcentajeReconocimientoPrestacionesSocialesDistribucionCesantias"
                                rules={{
                                    required: 'Este campo es requerido',
                                    min: {
                                        value: 1,
                                        message: 'No se aceptan valores negativos'
                                    },

                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Porcentaje Reconocimiento Prestaciones Prestaciones Sociales Distribución Cesantías*"
                                        onChange={onChange}
                                        value={value === 0 ? null : value}
                                        inputProps={{ style: { textAlign: 'right' } }}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        percentMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="derechoLiquidacionCuotasBeneficiarios"
                                control={control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="¿El Beneficiario Tiene Derecho Liquidación Cuotas?"
                                        onChange={onChange}
                                        checked={value}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    {watch("derechoLiquidacionCuotasBeneficiarios") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="periodoAportesDesdeBeneficiarios"
                                rules={{
                                    validate: () => watch('periodoAportesDesdeBeneficiarios') > watch('periodoAportesHastaBeneficiarios') ? 'El período de aportes desde debe ser menor al período de aportes hasta' : true
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
                    {watch("derechoLiquidacionCuotasBeneficiarios") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="periodoAportesHastaBeneficiarios"
                                control={control}
                                rules={{
                                    validate: () => watch('periodoAportesDesdeBeneficiarios') > watch('periodoAportesHastaBeneficiarios') ? 'El período de aportes desde debe ser menor al período de aportes hasta' : true
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
                    {watch("derechoLiquidacionCuotasBeneficiarios") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="valorDerechoLiquidacionCuotasBeneficiarios"
                                control={control}
                                render={({
                                    field: { onChange },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Valor Derecho Liquidación Cuotas"
                                        value={calidad === 1 ? Math.round(moment(watch('periodoAportesHastaBeneficiarios')).diff(watch('periodoAportesDesdeBeneficiarios'), 'months', true)) * asignacionBasicaMensual * 0.08 : Math.round(moment(watch('periodoAportesHastaBeneficiarios')).diff(watch('periodoAportesDesdeBeneficiarios'), 'months', true)) * asignacionBasicaMensual * 0.045}
                                        onChange={onChange}
                                        inputProps= {{style:{textAlign:'right'}}}
                                        currencyMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>) : null}
                    {watch ("derechoLiquidacionCuotasBeneficiarios") ? (<Grid className="item-containerModal" md={5}>
                        <Grid className='subcontainerModal'>
                            <Controller
                                name="numeroCuotasderechoLiquidacionCuotasBeneficiarios"
                                control={control}
                                render={({
                                    field: { onChange },
                                    fieldState: { error }
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        label="Número De Cuotas (Meses) A Liquidar"
                                        value={Math.round(moment(watch('periodoAportesHastaBeneficiarios')).diff(watch('periodoAportesDesdeBeneficiarios'), 'months', true)) -1 }
                                        onChange={onChange}
                                        helperText={error ? error.message : null}

                                    />
                                )}
                            />
                        </Grid>
                    </Grid>): null} */}
                    <Grid >
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
                                onClick={handleSubmit(submit)}
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


