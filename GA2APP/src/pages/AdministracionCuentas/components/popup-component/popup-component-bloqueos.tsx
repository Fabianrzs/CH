import React, { useEffect, useState } from 'react';
import {
    Dialog,
    DialogContent, Typography, Button, makeStyles, createStyles, DialogActions,
    Grid, CardContent, Card, MenuItem
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import TituloIcono from '../titulo-icono/tituloIcono.component';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { CAUSALBLOQUEOCUENTA } from '../../../../types/catalog.type';
import { startActualizaBloqueoCuentas } from '../../../../actions/catalog.action';
import CatalogApi from '../../../../services/api-catalog.service';
import CHCheckboxV2 from '../../../../components/ch-checkbox/CHCheckboxV2';
import { LoadingV2Component } from '../../../../components/loading/loadingV2.component';



const useStyles = makeStyles(() =>
    createStyles({
        dialogPaper: {
            width: '60%',
            height: '60%'
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
        maxheight: {
            height: '100%'
        },
        titulo: {
            marginLeft: '30%'
        }
    })
);


const api = CatalogApi();

export const ModalBloqueoDeCuenta = (props: Props) => {

    const { startActualizaBloqueoCuentas, open, handleClose, tipoCuenta, cuentaIdOrIdConcepto, tipoBloqueos, valueCuenta, responseAdminCuenta, idCuenta, onBloqueoExitoso, numeroCuenta } = props;
    const { control, watch, handleSubmit, getValues, setValue } = useForm({
        defaultValues: {
            causalBloqueoId: 0,
            tipoCuentaConcepto: '',
            conceptoId: 0,
            porcentaje: '',
            valor: '',
            observaciones: '',
            typerequest: '',
            bloquearTodosConceptos: false
        },
    });


    const conceptoIdSeleccionado = watch('conceptoId');
    const porcentaje = watch('porcentaje');

    const [causalesBloqueo, setCausalesBloqueo] = useState<any[]>([]);
    const [loadingCausales, setLoadingCausales] = useState<boolean>(true);


    const fetchCausales = async () => {
        try {
            const response = await api.getCausalesBloqueoCuenta();
            if (response?.isSuccess) {
                setCausalesBloqueo(response.data);
            }
        } catch {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        } finally {
            setLoadingCausales(false);
        }
    };

    useEffect(() => {
        fetchCausales()

    }, [])

    const classes = useStyles();

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
    const [mostrarPorcentaje, setMostrarPorcentaje] = useState(false);

    useEffect(() => {
        if (!loadingCausales) {
            setIsLoading(false);
        } else {
            setIsLoading(true);
        }
    }, [loadingCausales]);


    const onSubmit = async (data: any) => {
        setIsSubmitting(true);

        try {
            const valorBloqueo = data.valor === '' ? 0 : parseInt(data.valor.replace('$', '').replace(',', ''), 10);

            const payloadBloqueo = {
                CausalBloqueoId: data.causalBloqueoId,
                cuentaId: cuentaIdOrIdConcepto,
                conceptoId: conceptoIdSeleccionado,
                valor: valorBloqueo,
                estado: false,
                tipoBloqueo: porcentaje
            };

            if (conceptoIdSeleccionado && !data.bloquearTodosConceptos) {
                payloadBloqueo.conceptoId = conceptoIdSeleccionado;
            }

            await startActualizaBloqueoCuentas(payloadBloqueo);

            const conceptoIdParaBloqueo = data.bloquearTodosConceptos ? undefined : conceptoIdSeleccionado;

            onBloqueoExitoso(
                numeroCuenta,
                valorBloqueo,
                conceptoIdParaBloqueo,
                porcentaje,
                cuentaIdOrIdConcepto
            );

            handleClose();

        } finally {
            setIsSubmitting(false);
        }
    };


    return (

        <>
            <Dialog
                classes={{ paper: classes.dialogPaper }}
                open={open}
                onClose={handleClose}
                maxWidth="lg"
            >
                {isLoading ? (
                    <LoadingV2Component
                        key="loader"
                        loading
                        tittle="Consultando Causales"
                    />
                ) : (
                    <>
                        {isSubmitting ? (
                            <div className="p-10 flex flex-col items-center justify-center min-h-[300px]">
                                <span className="loader" />
                                <p className="mt-4 text-lg">Procesando bloqueo, por favor espere...</p>
                            </div>
                        ) : (
                            <>
                                <Grid className={classes.titulo}>
                                    <div >
                                        <Typography
                                            variant="h5"
                                            component="h5"
                                            className={classes.dialogTitles}
                                        >
                                            Bloquear Cuentas ó Conceptos
                                        </Typography>
                                    </div>
                                    <div className={classes.marginTops}>
                                        {' '}
                                    </div>
                                </Grid>
                                <Grid className='subcontainerModal'>
                                    <div className={classes.maxheight} >
                                        <Card className={classes.maxheight}>
                                            <Grid className="container-ico-t marginTopContainerPopup">
                                                <span className="ico fch-Puntual-0 blue-2 icoLit" />
                                                <span className="ico fch-Arrow-0-0 gray-2 icoLit-2 ico-b" />
                                                <span className="tittle-tab blue-2 tittle-we">
                                                    Bloquear Cuenta Número : {valueCuenta.replace(/^Cuenta\s*/i, '')}
                                                </span>
                                            </Grid>

                                            <CardContent className={classes.maxheight}>
                                                <DialogContent className={classes.maxheight}>
                                                    <Grid container justifyContent="space-around">
                                                        <Grid item xs={3}>
                                                            <TituloIcono
                                                                title1="Tipo"
                                                                subtitle="de causal"
                                                                icono="event_available"
                                                            />
                                                        </Grid>
                                                        <Grid item xs={6} >
                                                            <Controller
                                                                name="causalBloqueoId"
                                                                control={control}
                                                                rules={{
                                                                    required: 'Este campo es requerido.',
                                                                }}
                                                                render={({
                                                                    field: { onChange, value },
                                                                    fieldState: { error },
                                                                }) => (
                                                                    <CHSelect
                                                                        error={!!error}
                                                                        label=""
                                                                        value={value}
                                                                        helperText={error ? error.message : null}
                                                                        onChange={onChange}
                                                                    >
                                                                        {causalesBloqueo?.map(
                                                                            (option: any) => (
                                                                                <MenuItem
                                                                                    key={option.idCausal}
                                                                                    value={option.idCausal}
                                                                                >
                                                                                    {option.descripcionCausal}
                                                                                </MenuItem>
                                                                            )
                                                                        )}
                                                                    </CHSelect>
                                                                )}
                                                            />
                                                        </Grid>
                                                    </Grid>
                                                    <br />
                                                    <br />
                                                    <Grid container justifyContent="space-around">
                                                        <Grid item xs={3}>
                                                            <TituloIcono
                                                                title1="Tipo de "
                                                                subtitle="Cuenta"
                                                                icono="event_available"
                                                            />
                                                        </Grid>
                                                        <Grid item xs={6} >
                                                            <Controller
                                                                name="tipoCuentaConcepto"
                                                                control={control}
                                                                render={({
                                                                    field: { onChange },
                                                                    fieldState: { error },
                                                                }) => (
                                                                    <CHTextField
                                                                        isReadOnly={false}
                                                                        error={!!error}
                                                                        helperText={error ? error.message : null}
                                                                        value={valueCuenta}
                                                                        onChange={onChange}
                                                                    />
                                                                )}
                                                            />
                                                        </Grid>
                                                    </Grid>
                                                    <br />
                                                    <br />

                                                    <Grid container justifyContent="space-around">
                                                        <Grid item xs={3}>
                                                            <TituloIcono
                                                                title1="Concepto"
                                                                subtitle=""
                                                                icono="event_available"
                                                            />
                                                        </Grid>
                                                        <Grid item xs={6} >
                                                            <Controller
                                                                name="conceptoId"
                                                                control={control}
                                                                render={({
                                                                    field: { onChange, value },
                                                                }) => (
                                                                    <CHSelect
                                                                        label=""
                                                                        value={watch('bloquearTodosConceptos') ? 0 : value}
                                                                        onChange={onChange}
                                                                        disabled={watch('bloquearTodosConceptos')}
                                                                    >
                                                                        {responseAdminCuenta?.cuentas?.find((x: any) => x.cuentaId === idCuenta)?.conceptos.map(
                                                                            (option: any) => (
                                                                                <MenuItem
                                                                                    key={option.id}
                                                                                    value={option.id}
                                                                                >
                                                                                    {option.descripcionConcepto}
                                                                                </MenuItem>
                                                                            )
                                                                        )}
                                                                    </CHSelect>
                                                                )}
                                                            />
                                                            <Controller
                                                                name="bloquearTodosConceptos"
                                                                control={control}
                                                                render={({
                                                                    field: { onChange, value },
                                                                }) => (
                                                                    <CHCheckboxV2
                                                                        label='Bloquear todos los conceptos'
                                                                        checked={value}
                                                                        onChange={onChange}
                                                                    />
                                                                )}
                                                            />
                                                        </Grid>
                                                    </Grid>
                                                    <br />
                                                    <br />
                                                    <Grid container justifyContent="space-around">
                                                        <Grid item xs={3}>
                                                            <TituloIcono
                                                                title1="Valor o"
                                                                subtitle="Porcentaje"
                                                                icono="event_available"
                                                            />
                                                        </Grid>

                                                        <Grid item xs={6}>
                                                            {mostrarPorcentaje ? (
                                                                <Controller
                                                                    name="valor"
                                                                    control={control}
                                                                    defaultValue=""
                                                                    rules={{
                                                                        required: { value: true, message: "Este campo es requerido." },
                                                                        validate: (val) => {
                                                                            const cleanVal = (val || "").toString().replace(/[^0-9]/g, "");
                                                                            if (!cleanVal) return "Este campo es requerido.";
                                                                            const num = Number(cleanVal);
                                                                            if (num < 0 || num > 100) return "Debe ser un número entre 0 y 100.";
                                                                            return true;
                                                                        },
                                                                    }}
                                                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                                                        <CHTextField
                                                                            label="Porcentaje *"
                                                                            error={!!error}
                                                                            helperText={error ? error.message : null}
                                                                            value={value ? `${value}%` : ""}
                                                                            onChange={(e) => {
                                                                                let val = e.target.value.replace(/[^0-9]/g, "");
                                                                                if (Number(val) > 100) val = "100";
                                                                                onChange(val);
                                                                            }}
                                                                            inputProps={{
                                                                                style: { textAlign: "right" },
                                                                                inputMode: "numeric",
                                                                            }}
                                                                            percentMask
                                                                        />
                                                                    )}
                                                                />
                                                            ) : (
                                                                <Controller
                                                                    name="valor"
                                                                    control={control}
                                                                    rules={{
                                                                        required: { value: true, message: "Este campo es requerido." },
                                                                        validate: (val) => {
                                                                            const cleanVal = (val || "").toString().replace(/[^0-9]/g, "");
                                                                            if (!cleanVal || Number(cleanVal) <= 0)
                                                                                return "Debe ingresar un valor válido.";
                                                                            return true;
                                                                        },
                                                                    }}
                                                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                                                        <CHTextField
                                                                            label="Valor *"
                                                                            error={!!error}
                                                                            helperText={error ? error.message : null}
                                                                            value={value ?? ""}
                                                                            onChange={(e) => {
                                                                                const val = e.target.value.replace(/[^0-9]/g, "");
                                                                                onChange(val);
                                                                            }}
                                                                            inputProps={{
                                                                                style: { textAlign: "right" },
                                                                            }}
                                                                            currencyMask
                                                                        />
                                                                    )}
                                                                />
                                                            )}
                                                        </Grid>
                                                    </Grid>

                                                    <br />
                                                    <br />
                                                    <Grid container justifyContent="space-around">
                                                        <Grid item xs={3}>
                                                            <TituloIcono
                                                                title1="Observaciones"
                                                                subtitle=""
                                                                icono="event_available"
                                                            />
                                                        </Grid>
                                                        <Grid item xs={6} >
                                                            <Controller
                                                                name="observaciones"
                                                                control={control}
                                                                render={({
                                                                    field: { onChange, value },
                                                                    fieldState: { error },
                                                                }) => (
                                                                    <CHTextField
                                                                        maxRows={5}
                                                                        multiline
                                                                        error={!!error}
                                                                        helperText={error ? error.message : null}
                                                                        value={value}
                                                                        onChange={onChange}
                                                                    />
                                                                )}
                                                            />
                                                        </Grid>
                                                    </Grid>
                                                </DialogContent>
                                            </CardContent>
                                        </Card>
                                    </div>
                                </Grid>
                                <DialogActions className='maginButtonAddClose'>
                                    <Button onClick={handleClose} variant="contained">
                                        Cancelar
                                    </Button>
                                    <Button onClick={handleSubmit(onSubmit)} variant="contained">
                                        Guardar
                                    </Button>
                                </DialogActions>
                            </>
                        )}
                    </>
                )}
            </Dialog>
        </>

    );
}


interface LinkMapDispatchToProps {
    valueCuenta: string;
    open: boolean;
    handleClose: () => void;
    tipoCuenta: string;
    cuentaIdOrIdConcepto: number;
    tipoBloqueos: CAUSALBLOQUEOCUENTA[];
    startActualizaBloqueoCuentas: (data: any) => void;
    responseAdminCuenta: any;
    idCuenta: any;
    onBloqueoExitoso: (numeroCuenta: any, valorBloqueado: number, conceptoIdParaBloqueo: number | undefined, porcentaje: any, cuentaIdOrIdConcepto: number) => void;
    numeroCuenta: any;
}

const mapDispatchToProps = {
    startActualizaBloqueoCuentas
};



type Props = LinkMapDispatchToProps;

export default connect(null, mapDispatchToProps)(ModalBloqueoDeCuenta)