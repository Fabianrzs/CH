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
import UtilService from '../../../../utils/util.service';
import CHCheckboxV2 from '../../../../components/ch-checkbox/CHCheckboxV2';
import ApiClients from '../../../../services/api-clients.service';
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

function ModalLiquidacionCuentas({
    open,
    catalogState,
    clientState,
    dataCuentas,
    firstLoad,
    approbed,
    handleCloseReturn,
}: {
    open: boolean;
    dataCuentas: any;
    catalogState: CatalogState;
    clientState: ClientState;
    firstLoad: boolean;
    approbed: boolean;
    handleCloseReturn: () => void;
}) {
    const classes = useStyles();
    const [loading, setLoading] = useState<boolean>(false);
    const { control, watch, reset } = useForm({
        defaultValues: {
          idConcepto: 1,
          cueotas: '',
          nombreCuenta: '',
        },
      });

    const closeReturn = () => {
        handleCloseReturn()
    }

    useEffect(() => {
        if (open && dataCuentas?.cuentas?.length) {
          reset({
            idConcepto: 1,
            cueotas: dataCuentas.cuentas[0]?.cueotas ?? '',
            nombreCuenta: dataCuentas.cuentas[0]?.nombreCuenta ?? '',
          });
        }
      }, [open, dataCuentas, reset]);
      


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
                    <DialogTitle id="form-dialog-title">Liquidación Cuotas</DialogTitle>

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
                    <Grid item xs={12} sm={6}>
                        <Controller
                            name="cueotas"
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextFieldV2
                                    label="Número de cuotas"
                                    onChange={onChange}
                                    value={value}
                                    isReadOnly={watch('idConcepto') !== 1}
                                    error={!!error}
                                    helperText={error?.message}
                                />
                            )}
                        />
                    </Grid>


                    <Grid item xs={12} sm={6}>
                        <Controller
                            name="nombreCuenta"
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextFieldV2
                                    label="Nombre de la cuenta"
                                    onChange={onChange}
                                    value={value}
                                    isReadOnly
                                    error={!!error}
                                    helperText={error?.message}
                                />
                            )}
                        />
                    </Grid>

                    <DialogActions>
                        <Button
                            type="button"
                            onClick={closeReturn}
                            color="primary"
                        >
                            Cancelar
                        </Button>


                    </DialogActions>
                </Dialog>
            </Grid>
        </Grid>
    )
}


const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    clientState: state.client,
})

export default connect(mapStateToProps)(ModalLiquidacionCuentas);