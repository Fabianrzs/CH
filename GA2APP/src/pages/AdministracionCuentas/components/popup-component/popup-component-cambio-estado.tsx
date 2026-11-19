import React, { useState } from 'react';
import {
    Dialog,
    DialogContent, Typography, Button, makeStyles, createStyles, DialogActions,
    Grid, CardContent, Card, MenuItem,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import swal from 'sweetalert';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import TituloIcono from '../titulo-icono/tituloIcono.component';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import { CAUSALESTADOCUENTA, ESTADOCUENTA, PORCENTAJEDESCUENTO, TIPODECUENTAC } from '../../../../types/catalog.type';


import { AdministracionParametroModel } from '../../../../models/requests/create-simulation-client';
import { ClientState } from '../../../../reducers/client.reducer';
import { AppState } from '../../../../store/rootReducer';

const useStyles = makeStyles(() =>
    createStyles({
        dialogPaper: {
            width: '90%',
            height: '85%'
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
        }
    })
);

export const ModalCambioEstado = (props: Props) => {

    const { 
        startCambioEstado,
        clientState, 
        open, 
        handleClose, 
        tipoCuentaC, 
        porcentajeDescuento, 
        causalEstadoCuentas, 
        idUnidadEjecuta, 
        idCliente,
        cuenta,
        cuentaId,
        estadosCuenta
    } = props;
    const [datosCausalEstadoCuenta, setCausalEstadoCuenta] = useState<CAUSALESTADOCUENTA[]>([]);
    const [nuevoEstado, setNuevoEstado] = useState<number>(0);
    const { control, handleSubmit } = useForm({
        defaultValues: {
            Radicado: '',
            TipoCuenta: cuenta.tipoCuentaId,
            EstadoActual:cuenta.estadoCuentaId,
            EstadoNuevo:'',
            Observaciones:'',
            CausalDeCuenta: '',
        },
    });

    const onSubmit = (data: any) => {

        swal({
            text: "¿Está seguro de cambiar el estado a la cuenta?",
            icon: "info",
            buttons: ["No", "Sí"]
        }).then(async respuesta => {
            if (respuesta) {
                await startCambioEstado({
                    ctaID:cuentaId,
                    ectId:nuevoEstado
                }); 
            }
        })

    }

    const classes = useStyles();
    const setCausalEstadosCuenta = async (idEstado:number) =>{
        setCausalEstadoCuenta(causalEstadoCuentas.filter(x=>x.estadoCuentaId===idEstado));
        setNuevoEstado(idEstado);
    }
    return (
        <>
            <div>
                <Dialog classes={{ paper: classes.dialogPaper }}
                    open={open}
                    onClose={handleClose}
                    maxWidth="lg"

                >
                    <div >
                        <Typography
                            variant="h5"
                            component="h5"
                            className={classes.dialogTitles}
                        >
                            Cambio de estado
                        </Typography>
                    </div>
                    <div className={classes.marginTops}>
                        {' '}
                    </div>
                    <div className={classes.maxheight} >

                        <Card className={classes.maxheight}>

                            <Grid className="container-ico-t marginTopContainerPopup">
                                <span className="ico fch-Puntual-0 blue-2 icoLit" />
                                <span className="ico fch-Arrow-0-0 gray-2 icoLit-2 ico-b" />
                                <span className="tittle-tab blue-2 tittle-we"> </span>
                            </Grid>
                            <CardContent className={classes.maxheight}>

                                <DialogContent className={classes.maxheight}>

                                    <Grid container>
                                    <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Radicado cambio"
                                                subtitle=""
                                                icono="event_available"
                                            />
                                        </Grid>
                                        

                                        <Grid item xs={3}>
                                            <Controller
                                                name="Radicado"
                                                control={control}
                                                rules={{
                                                    required: 'Este campo es requerido.',
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHTextField   
                                                        error={!!error}
                                                        helperText={error ? error.message : null}
                                                        value={value}
                                                        onChange={onChange}
                                                    />
                                                )}
                                            />
                                        </Grid>

                                        <Grid item xs={1}>
                                            <span>{' '}</span>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Tipo de cuenta"
                                                subtitle=""
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3} >
                                            <Controller
                                                name="TipoCuenta"
                                                control={control}
                                                rules={{
                                                    required: 'Este campo es requerido.',
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHSelect
                                                        disabled
                                                        error={!!error}
                                                        label=""
                                                        value={value}
                                                        helperText={error ? error.message : null}
                                                        onChange={onChange}
                                                    >
                                                        {tipoCuentaC?.map(
                                                            (option: TIPODECUENTAC) => (
                                                                <MenuItem
                                                                    key={option.idTipoCuentaC}
                                                                    value={option.idTipoCuentaC}
                                                                >
                                                                    {option.descripcionTipoCuentaC}
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
                                    <Grid container>
                                    <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Estado actual"
                                                subtitle="Cuenta"
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3} >
                                            <Controller
                                                name="EstadoActual"
                                                control={control}
                                                rules={{
                                                    required: 'Este campo es requerido.',
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHSelect
                                                    disabled
                                                        error={!!error}
                                                        label=""
                                                        value={value}
                                                        helperText={error ? error.message : null}
                                                        onChange={onChange}
                                                    >
                                                        {estadosCuenta?.map(
                                                            (option: ESTADOCUENTA) => (
                                                                <MenuItem
                                                                    key={option.idEstado}
                                                                    value={option.idEstado}
                                                                >
                                                                    {option.estado}
                                                                </MenuItem>
                                                            )
                                                        )}

                                                    </CHSelect>
                                                )}
                                            />
                                        </Grid>
                                      
                                        <Grid item xs={1}>
                                            <span>{' '}</span>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Nuevo estado"
                                                subtitle="Cuenta"
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3} >
                                            <Controller
                                                name="EstadoNuevo"
                                                control={control}
                                                render={({
                                                    field: { onChange, value=nuevoEstado },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHSelect
                                                        error={!!error}
                                                        label=""
                                                        value={nuevoEstado}
                                                        helperText={error ? error.message : null}
                                                        onChange={(event)=>{
                                                            setCausalEstadosCuenta(Number(event.target.value))
                                                        }}
                                                    >
                                                       {estadosCuenta?.map(
                                                            (option: ESTADOCUENTA) => (
                                                                <MenuItem
                                                                    key={option.idEstado}
                                                                    value={option.idEstado}
                                                                >
                                                                    {option.estado}
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
                                    <Grid container>
                                        <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Causal de cuenta"
                                                subtitle=""
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3} >
                                            <Controller
                                                name="CausalDeCuenta"
                                                control={control}
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
                                                        {datosCausalEstadoCuenta?.map(
                                                            (option: CAUSALESTADOCUENTA) => (
                                                                <MenuItem
                                                                    key={option.causalEstadoId}
                                                                    value={option.causalEstadoId}
                                                                >
                                                                    {option.causalCuentaDescripcion}
                                                                </MenuItem>
                                                            )
                                                        )}

                                                    </CHSelect>
                                                )}
                                            />
                                        </Grid>
                                        <Grid item xs={1}>
                                            <span>{' '}</span>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Observación"
                                                subtitle="Cambio"
                                                icono="event_available"
                                            />
                                        </Grid>

                                        <Grid item xs={3} >
                                            <Controller
                                                name="Observaciones"
                                                control={control}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHTextField
                                                        error={!!error}
                                                        helperText={error ? error.message : null}
                                                        value={value}
                                                        onChange={onChange}
                                                    />
                                                )}
                                            />
                                        </Grid>
                                       
                                    </Grid>
                                    <br />
                                    <br /> 
                                </DialogContent>
                            </CardContent>
                        </Card>
                    </div>
                    <DialogActions className='maginButtonAddClose'>
                        <Button onClick={handleClose} variant="contained">
                            Cancelar
                        </Button>
                        <Button onClick={handleSubmit(onSubmit)} variant="contained">
                            Guardar
                        </Button>
                    </DialogActions>
                </Dialog>
            </div>
        </>
    );
}
interface LinkMapStateToProps {
    clientState: ClientState;
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client
});

interface LinkMapDispatchToProps {
    startCambioEstado:(data: any) => void;
    open: boolean;
    handleClose: () => void;
    tipoCuentaC: TIPODECUENTAC[];
    porcentajeDescuento: PORCENTAJEDESCUENTO[];
    causalEstadoCuentas: CAUSALESTADOCUENTA[];
    idUnidadEjecuta: number;
    idCliente: number;
    cuentaId:number;
    cuenta:any;
    estadosCuenta:ESTADOCUENTA[];
}

const mapDispatchToProps = {
    
};
type Props = LinkMapDispatchToProps & LinkMapStateToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ModalCambioEstado)