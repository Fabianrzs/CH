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

function ModalDevolucionSaldo({
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
    const { control, watch, reset, setValue } = useForm({
        defaultValues: {
            idConcepto: 1,
            cueotas: '',
            nombreCuenta: '',
            saldoDisponible: 0
        },
    });


    const closeReturn = () => {
        handleCloseReturn()
    }

    const nombreCuentaSeleccionada = watch('nombreCuenta');
    const cuentas = dataCuentas?.cuentas || [];

    useEffect(() => {
        
        if (open && dataCuentas?.cuentas?.length) {
            const cuentaInicial = dataCuentas.cuentas.find(
                (c: any) => c.saldoDisponible > 0
            ) || dataCuentas.cuentas[0];
    
            reset({
                idConcepto: 1,
                cueotas: cuentaInicial?.cueotas ?? '',
                nombreCuenta: cuentaInicial?.nombreCuenta ?? '',
                saldoDisponible: cuentaInicial?.saldoDisponible ?? 0
            });
        }
    }, [open, dataCuentas, reset]);
    
    useEffect(() => {
        if (nombreCuentaSeleccionada && cuentas.length) {
            const cuentaSeleccionada = cuentas.find(
                (c: any) => c.nombreCuenta === nombreCuentaSeleccionada
            );
            if (cuentaSeleccionada) {
                setValue('cueotas', cuentaSeleccionada.cueotas ?? '');
                setValue('saldoDisponible', cuentaSeleccionada.saldoDisponible ?? 0);
            }
        }
    }, [nombreCuentaSeleccionada, cuentas, setValue]);


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
                    <DialogTitle id="form-dialog-title">Devolución Saldo</DialogTitle>
                    <DialogContent>
                        {cuentas.length === 1 ? (
                            <>
                                <Controller
                                    name="nombreCuenta"
                                    control={control}
                                    render={({ field }) => (
                                        <CHTextFieldV2
                                            {...field}
                                            fullWidth
                                            label="Nombre de la Cuenta"
                                            variant="outlined"
                                            InputProps={{ readOnly: true }}
                                        />
                                    )}
                                />
                                <Controller
                                    name="saldoDisponible"
                                    control={control}
                                    render={({ field }) => (
                                        <CHTextFieldV2
                                            {...field}
                                            fullWidth
                                            label="Saldo Disponible"
                                            variant="outlined"
                                            InputProps={{ readOnly: true }}
                                        />
                                    )}
                                />
                            </>
                        ) : (
                            <>
                                <Controller
                                    name="nombreCuenta"
                                    control={control}
                                    render={({ field }) => (
                                        <CHTextFieldV2
                                            {...field}
                                            select
                                            fullWidth
                                            label="Seleccionar Cuenta"
                                            variant="outlined"
                                        >
                                            {cuentas.map((cuenta: any, index: any) => (
                                                <MenuItem key={index} value={cuenta.nombreCuenta}>
                                                    {cuenta.nombreCuenta}
                                                </MenuItem>
                                            ))}
                                        </CHTextFieldV2>
                                    )}
                                />
                                <Controller
                                    name="saldoDisponible"
                                    control={control}
                                    render={({ field }) => (
                                        <CHTextFieldV2
                                            {...field}
                                            fullWidth
                                            label="Saldo Disponible"
                                            variant="outlined"
                                            InputProps={{ readOnly: true }}
                                        />
                                    )}
                                />
                            </>
                        )}
                    </DialogContent>



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

export default connect(mapStateToProps)(ModalDevolucionSaldo);