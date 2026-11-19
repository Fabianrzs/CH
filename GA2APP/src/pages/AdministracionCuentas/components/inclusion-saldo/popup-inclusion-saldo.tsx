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
import ModalEditarInclusionSaldos from './popup-editar-opciones-inclusion-saldos';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import NotificationService from '../../../../services/notification.service';

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

const api = ApiClients();

function ModalInclusionSaldos({
    open,
    catalogState,
    clientState,
    data,
    firstLoad,
    approbed,
    handleClose,
    handleCloseReturn,
    startPostInclusionSaldos,
    setFirstLoadTrue
}: {
    open: boolean;
    data: any;
    catalogState: CatalogState;
    clientState: ClientState;
    firstLoad: boolean;
    approbed: boolean;
    handleClose: (data: any) => void;
    handleCloseReturn: () => void;
    startPostInclusionSaldos: (inclusionSaldo: InclusionSaldoRequestDto) => void;
    setFirstLoadTrue: () => void;
}) {
    const classes = useStyles();
    const [loading, setLoading] = useState<boolean>(false);
    const [hasLoadedFields, setHasLoadedFields] = useState<boolean>(false);
    const [isOpenEditar, setIsOpenEditar] = useState<boolean>(false);
    const [opcionEditar, setOpcionEditar] = useState<any>();
    const [tableElements, setTableElements] = useState<any[]>(data?.opciones);
    const [lockButtons, setLockButtons] = useState<boolean>();
    const { control, setValue, reset, watch, handleSubmit } = useForm<InclusionSaldoDto>({
        defaultValues: {
            periodoDesde: new Date(),
            periodoHasta: new Date(),
            cuotas: 0,
            cuotasAntiguas: 0,
            idConcepto: 0,
            valor: 0,
        }
    })

    const {
        control: controlForm,
        watch: watchForm,
        getValues: getValuesForm,
        setValue: setValueForm,
        handleSubmit: handleSubmitForm,
        reset: resetForm
    } = useForm<InclusionSaldoRequestDto>({
        defaultValues: {
            opciones: [],
            observaciones: '',
            fechaFinalRecaudo: new Date(),
            fechaInicialRecaudo: new Date(),
            generaOrdenRecaudo: false,
            idInclusionSaldoEncabezado: 0
        }
    })



    useEffect(() => {
        setHasLoadedFields(!open)
        if (data == null) {
            setLockButtons(false)
            setTableElements([])
            reset({
                periodoDesde: new Date(),
                periodoHasta: new Date(),
                cuotas: 0,
                cuotasAntiguas: 0,
                idConcepto: 0,
                valor: 0,
                idInclusionSaldoOpciones: 0
            })

            resetForm({
                aprobacion: false,
                fechaFinalRecaudo: new Date(),
                fechaInicialRecaudo: new Date(),
                generaOrdenRecaudo: false,
                idCliente: 0,
                idInclusionSaldoEncabezado: 0,
                observaciones: '',
                opciones: []
            })
        } else {
            setLockButtons(data.aprobacion)
        }
    }, [open])

    if (!hasLoadedFields && data != null) {
        setHasLoadedFields(true)
        const fields = Object.keys(data as InclusionSaldoRequestDto);
        fields.forEach(field => {
            const value = data[field as keyof InclusionSaldoRequestDto];
            setValueForm(field as any, value);
        })
        setTableElements(data?.opciones)
    }

    const addElementsToTable = (data: InclusionSaldoDto) => {
        let arrayElements = tableElements
        const elementToAdd = {
            periodoDesde: data.periodoDesde,
            periodoHasta: data.periodoHasta,
            cuotas: data.cuotas,
            cuotasAntiguas: data.cuotas,
            idConcepto: data.idConcepto,
            valor: +data.valor.toString().replaceAll('$', '').replaceAll(',', '')
        }

        arrayElements = arrayElements.concat(elementToAdd)
        setTableElements(arrayElements)

        reset({
            periodoDesde: new Date(),
            periodoHasta: new Date(),
            cuotas: 0,
            idConcepto: 0,
            valor: 0,
        })
    }

    const validateDatePeriodoDesde = (data: Date) => {
        const date = moment(data);
        if (date > moment()) {
            return "La fecha debe ser inferior a la fecha actual"
        }
        if (date > moment(watch('periodoHasta'))) {
            return "La fecha debe ser inferior a período hasta"
        }
        if (!date.isValid()) {
            return "Ingrese una fecha válida"
        }
        return true;
    }

    const validateDatePeriodoHasta = (data: Date) => {
        const date = moment(data);
        if (date > moment()) {
            return "La fecha debe ser inferior a la fecha actual"
        }
        if (date < moment(watch('periodoDesde'))) {
            return "La fecha debe ser superior a período desde"
        }
        if (!date.isValid()) {
            return "Ingrese una fecha válida"
        }
        return true;
    }

    const saveForm = async (data: InclusionSaldoRequestDto) => {
        handleCloseReturn()
        data.opciones = tableElements;
        data.aprobacion = false;
        data.idCliente = clientState.admincuentas.clienteId;
        handleClose(data)
    }

    const submitForm = async (data: InclusionSaldoRequestDto) => {
        handleCloseReturn()
        data.opciones = tableElements;
        data.aprobacion = true;
        data.idCliente = clientState.admincuentas.clienteId;
        handleClose(data)
    }

    const handleCloseEditOpciones = (data: any) => {
        data.valor = data.valor.toString().replaceAll('$', '').replaceAll(',', '')
        setTableElements(tableElements?.map(x => data.idInclusionSaldoOpciones === x.idInclusionSaldoOpciones ? data : x))
        setIsOpenEditar(false)
    }

    const handleCloseEdit = () => {
        setIsOpenEditar(false)
    }

    const editOpcionesInclusionSaldos = (data: any) => {
        setOpcionEditar(data)
        setIsOpenEditar(true)
    }

    const closeReturn = () => {
        handleCloseReturn()
    }

    const deleteOpcionesInclusionSaldos = async (data: any, index: number) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Desea eliminar la Orden de Recaudo?',
        );
        if(confirmAlert){
            if(data.idInclusionSaldoOpciones > 0 && data.idInclusionSaldoOpciones != null){
                await api.eliminarInclusionSaldos(data.idInclusionSaldoOpciones)
            }
            setTableElements(tableElements?.filter(x => x !== data))
        }
    }
    

    return (
        <Grid item container>
            <LoadingComponent key="loader" loading={loading} />
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
                                        validate: () => +watch('valor').toString().replaceAll('$', '').replaceAll(',', '') < 1 || watch('valor') == null ? 'Este campo es requerido y mayor a 0' : true
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
                        <Grid className="container-23" style={{ paddingLeft: '70%' }}>
                            <Grid>
                                <button type="button" className="bgcian button-i blue flex" onClick={handleSubmit(addElementsToTable)}>
                                    <span className="ico fch-Agregar-0-1 icoMid-2 separador-marg-6" />
                                    AGREGAR
                                </button>
                            </Grid>
                        </Grid>
                        <Grid className="container-17">
                            <table className="table">
                                <thead>
                                    <th className="tab-space-01">Período desde</th>
                                    <th className="tab-space-01">Período hasta</th>
                                    <th className="tab-space-01">Concepto</th>
                                    <th className="tab-space-01">Valor</th>
                                    <th className="tab-space-01">Cuotas</th>
                                    {!lockButtons ? (
                                        <th className="tab-space-01">Acciones</th>
                                    ) : null}
                                </thead>
                                <tbody>
                                    {tableElements?.map((element, index) => (
                                        <tr key={`table-${index}`}>
                                            <td data-label="Número Cuenta" className="tab-space-01">{UtilService.getDate(element.periodoDesde)}</td>
                                            <td data-label="Número Cuenta" className="tab-space-01">{UtilService.getDate(element.periodoHasta)}</td>
                                            <td data-label="Número Cuenta" className="tab-space-01">{concepto.find(x => x.id === element.idConcepto)?.descripcion}</td>
                                            <td data-label="Número Cuenta" className="tab-space-01">{UtilService.formatCurrency(element.valor)}</td>
                                            <td data-label="Número Cuenta" className="tab-space-01">{element.cuotas > 0 ? element.cuotas : ''}</td>
                                            {!lockButtons ? (
                                                <td className="tab-space-01 ">
                                                    <Grid>
                                                        <span aria-label="Detalle">
                                                            <span aria-label="Bloqueos" onClick={() => editOpcionesInclusionSaldos(element)} ><span className="ico fch-Hoja-Vida-0-1 icoMid-3" /></span>
                                                            <span aria-label="Eliminar" onClick={() => deleteOpcionesInclusionSaldos(element, index)}>
                                                                <span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} />
                                                            </span>
                                                        </span>
                                                    </Grid>
                                                </td>
                                            ) : null}
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </Grid>
                        <Grid container>
                            <Grid item md={12}>
                                <Controller
                                    name="observaciones"
                                    control={controlForm}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextFieldV2
                                            label="Observaciones"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item md={3}>
                                <Controller
                                    name="generaOrdenRecaudo"
                                    control={controlForm}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHCheckboxV2
                                            label="¿Genera orden de recaudo?"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item md={3}>
                                <Controller
                                    name="fechaInicialRecaudo"
                                    control={controlForm}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePickerV2
                                            label="Fecha Inicial recaudo"
                                            onChange={onChange}
                                            value={value}
                                            disabled={!watchForm('generaOrdenRecaudo')}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid spacing={1} md={3}>
                                <Controller
                                    name="fechaFinalRecaudo"
                                    control={controlForm}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHDatePickerV2
                                            label="Fecha final recaudo"
                                            onChange={onChange}
                                            value={value}
                                            disabled={!watchForm('generaOrdenRecaudo')}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </DialogContent>
                    <DialogActions>
                        <Button
                            type="button"
                            onClick={closeReturn}
                            color="primary"
                        >
                            Cancelar
                        </Button>
                        <Button type="button" color="primary" onClick={handleSubmitForm(saveForm)} disabled={lockButtons}>
                            Guardar
                        </Button>
                        <Button type="button" color="primary" onClick={handleSubmitForm(submitForm)} disabled={lockButtons || !watchForm('generaOrdenRecaudo')}>
                            Aprobar
                        </Button>
                    </DialogActions>
                </Dialog>
                <ModalEditarInclusionSaldos
                    handleClose={handleCloseEditOpciones}
                    data={opcionEditar}
                    handleCloseReturn={handleCloseEdit}
                    open={isOpenEditar}
                />
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

export default connect(mapStateToProps, mapDispatchToProps)(ModalInclusionSaldos);