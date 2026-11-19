import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    Button,
    createStyles,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    FormControl,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import { ClientState } from '../../../../reducers/client.reducer';
import CHTextFieldV2 from '../../../../components/ch-text-field/CHTextFieldV2';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHDatePickerV2 from '../../../../components/ch-datepicker/CHDatepickerV2';
import CHSelectV2 from '../../../../components/ch-select/CHSelectV2';
import { AppState } from '../../../../store/rootReducer';
import { CatalogState } from '../../../../reducers/catalog.reducer';
import { InclusionSaldoDto, InclusionSaldoRequestDto } from '../../../../types/client.model.type';
import UtilService from '../../../../utils/util.service';
import CHCheckboxV2 from '../../../../components/ch-checkbox/CHCheckboxV2';
import { startPostInclusionSaldos } from '../../../../actions/client.action';
import ApiClients from '../../../../services/api-clients.service';

const concepto = [
    { id: 1, descripcion: 'AHORROS OBLIGATORIOS' },
    { id: 2, descripcion: 'AHORROS VOLUNTARIOS' },
    { id: 3, descripcion: 'CESANTÍAS' },
    { id: 4, descripcion: 'AHORROS RETROACTIVOS' },
    { id: 5, descripcion: 'CESANTÍAS RETROACTIVOS' },
    { id: 6, descripcion: 'FONDO SOLIDARIDAD' },
    { id: 7, descripcion: 'CONCILIACIONES' },
    { id: 8, descripcion: 'POR LEGALIZAR' },
    { id: 9, descripcion: 'INTERESES APORTES' },
    { id: 10, descripcion: 'INTERESES CESANTÍAS' },
    { id: 11, descripcion: 'SUBSIDIO' },
    { id: 12, descripcion: 'EXCEDENTES FINANCIEROS' },
    { id: 13, descripcion: 'COMPENSACIONES' },
    { id: 14, descripcion: 'PROVISIÓN' },
    { id: 15, descripcion: 'CESANTÍAS DOCEAVAS' },
    { id: 16, descripcion: 'RETROACTIVOS FONDO SOLIDARIDAD' },
    { id: 17, descripcion: 'RENDIMIENTOS CESANTÍAS' },
    { id: 19, descripcion: 'CI MENOR CUANTIA' },
    { id: 22, descripcion: 'FONDO NUEVAS CONVOCATORIAS' },
    { id: 23, descripcion: 'AHORRO AVAC' },
    { id: 24, descripcion: 'INTERESES AHORRO AVAC' },
];

const useStyles = makeStyles(() =>
    createStyles({
        dialogPaper: {
            width: '60%',
            height: '50%'
        },
        dialogTitles: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between'
        },
        buttonClose: {
            position: 'absolute',
            right: '8',
            top: '8'
        },
        root: {
            flexGrow: 1,
        },
        gridPadding: {
            padding: '1rem'
        },
        cardContent: {
            margin: '0',
            padding: '0rem'
        },
        dialogRoot: {
            padding: '.5rem'
        },
        marginTops: {
            marginTop: '1rem'
        },
        iconRed: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#FF0000',
            margin: '10px'
          },
    })
);

function ModalEditarInclusionSaldos({
    open,
    data,
    handleClose,
    handleCloseReturn,
}: {
    open: boolean;
    data: any;
    handleClose: (data: any) => void;
    handleCloseReturn: () => void;
}) {
    const [hasLoadedFields, setHasLoadedFields] = useState(false);
    const {control, setValue, watch, handleSubmit} = useForm<InclusionSaldoDto>({
        defaultValues: {
            periodoDesde: new Date(),
            periodoHasta: new Date(),
            cuotas: 0,
            idConcepto: 0,
            valor: 0,
        }
    })

    useEffect(() => {
        setHasLoadedFields(!open)
    }, [open])

    if (!hasLoadedFields && data != null) {
        setHasLoadedFields(true)
        const fields = Object.keys(data as InclusionSaldoRequestDto);
        fields.forEach(field => {
            const value = data[field as keyof InclusionSaldoRequestDto];
            setValue(field as any, value);
        })

        setValue('cuotasAntiguas', data.cuotas)
    } 

    const validateDatePeriodoDesde = (data: Date) => {
        const date = moment(data);
        if (date > moment()){
            return "La fecha debe ser inferior a la fecha actual"
        } 
        if (date > moment(watch('periodoHasta'))){
            return "La fecha debe ser inferior a período hasta"
        } 
        if(!date.isValid()) {
            return "Ingrese una fecha válida"
        }
        return true;
    }

    const validateDatePeriodoHasta = (data: Date) => {
        const date = moment(data);
        if (date > moment()){
            return "La fecha debe ser inferior a la fecha actual"
        } 
        if (date < moment(watch('periodoDesde'))){
            return "La fecha debe ser superior a período desde"
        } 
        if(!date.isValid()) {
            return "Ingrese una fecha válida"
        }
        return true;
    }

    const handleEdit = (data: Date) => {
        handleClose(data)
    }

    return (
        <Grid item container>
            <Grid item xs={12} sm={6}>
                <Dialog
                    fullWidth
                    maxWidth="md"
                    open={open}
                    aria-labelledby="form-dialog-title"
                >
                    <DialogTitle id="form-dialog-title">Inclusión de saldo</DialogTitle>
                    <DialogContent>
                        <Grid container>
                            <Grid item md={4}>
                                <Controller
                                    name="periodoDesde"
                                    rules={{
                                        validate: validateDatePeriodoDesde
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePickerV2
                                            error={!!error}
                                            helperText={error ? error.message : ''}
                                            label="Período desde"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item md={4}>
                                <Controller
                                    name="periodoHasta"
                                    control={control}
                                    rules={{
                                        validate: validateDatePeriodoHasta
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePickerV2
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Período hasta"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item md={4}>
                                <Controller
                                    name="idConcepto"
                                    rules={{
                                        validate: () => watch('idConcepto') < 1 ? 'Este campo es requerido' : true
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelectV2
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Concepto"
                                            onChange={onChange}
                                            value={value}
                                        >
                                            {concepto?.map(
                                                (option: any) => (
                                                    <MenuItem key={option.id} value={option.id}>
                                                        {option.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelectV2>
                                    )}
                                />
                            </Grid>
                            <Grid spacing={1} md={3}>
                                <Controller
                                    name="valor"
                                    control={control}
                                    rules={{
                                        validate: () => +watch('valor').toString().replaceAll('$','').replaceAll(',','') < 1 || watch('valor') == null ? 'Este campo es requerido y mayor a 0' : true
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextFieldV2
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            label="Valor"
                                            onChange={onChange}
                                            value={value}
                                            currencyMask
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid spacing={1} md={3}>
                                <Controller
                                    name="cuotas"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextFieldV2
                                            label="Número de cuotas"
                                            onChange={onChange}
                                            value={value}
                                            isReadOnly={watch('idConcepto') !== 1}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </DialogContent>
                    <DialogActions>
                        <Button
                            type="button"
                            onClick={handleCloseReturn}
                            color="primary"
                        >
                            Cancelar
                        </Button>
                        <Button type="button" color="primary" onClick={handleSubmit(handleEdit)}>
                            Editar
                        </Button>
                    </DialogActions>
                </Dialog>
            </Grid>
        </Grid>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    clientState: state.client,
})

const mapDispatchToProps = {    
    startPostInclusionSaldos
}

export default connect(mapStateToProps, mapDispatchToProps)(ModalEditarInclusionSaldos);