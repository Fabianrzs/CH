import React, { useEffect, useState } from 'react';
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
import { CAUSALESTADOCUENTA, MODALIDADAFILIACION, PORCENTAJEDESCUENTO, TIPODECUENTAC } from '../../../../types/catalog.type';
import { startCrearCuentas } from '../../../../actions/catalog.action';
import { startGetAdministracionCuentasAfiliado } from '../../../../actions/client.action';
import { AdministracionParametroModel } from '../../../../models/requests/create-simulation-client';
import { ClientState } from '../../../../reducers/client.reducer';
import { AppState } from '../../../../store/rootReducer';



const useStyles = makeStyles(() =>
    createStyles({
        dialogPaper: {
            width: '90%',
            height: '95%'
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


export const ModalNuevaCuentaUsuario = (props: Props) => {
    
    const classes = useStyles();
    const [loading, setLoading] = useState(false);
    const { startCrearCuentas, startGetAdministracionCuentasAfiliado, clientState, open, handleClose, tipoCuentaC, porcentajeDescuento, causalEstadoCuentas, idUnidadEjecuta, idCliente,modalidad, ejecutora } = props;
    const { control, handleSubmit } = useForm({
        defaultValues: {
            radicado:'',
            numeroCuenta: '',
            SaldoCuenta: '',
            SaldoCanje: '',
            SaldoDisponible: '',
            NumeroCuotas: '',
            FechaPrimerAporte: '',
            FechaUltimoAporte: '',
            TipoCuenta: '',
            CausalDeCuenta: '',
            PorcentajeCuenta: '',
            typerequest: '',
            Modalidad:'',
            AsignacionSalarial:'',
            observacion:'',
        },
    });


    
    const onSubmit = async (data: any) => {
        const result = await swal({
            title: '¿Estás seguro de crear la nueva cuenta?',
            icon: 'info',
            buttons: ["No", "Sí"]
        });
    
        if (result === true) {
            try {
                setLoading(true);
                await startCrearCuentas({
                    ctA_ID: 0,
                    ctA_ID_INTEGRACION: 1,
                    clI_ID: idCliente, 
                    tcT_ID: data.TipoCuenta,
                    ecT_ID: 1, 
                    ccN_ID: data.CausalDeCuenta,
                    dcT_ID: null,
                    ctA_FECHA_CREACION: null,
                    ctA_FECHA_CIERRE: null,
                    ctA_FECHA_PRIMER_APORTE: data.FechaPrimerAporte,
                    ctA_SALDO:0,
                    ctA_SALDO_CANJE: 0,
                    ctA_SALDO_DISPONIBLE:0,
                    ctA_CUOTAS: data.NumeroCuotas,
                    ctA_BLOQUEO: false,
                    ejE_UNIDAD_EJECUTORA: idUnidadEjecuta,
                    ctA_PORCENTAJE: data.PorcentajeCuenta,
                    ctA_ASIGNACION_SALARIAL: data.AsignacionSalarial===''?0:String(data.AsignacionSalarial).replaceAll('$','').replaceAll(',','')
                });
    
                
                swal({
                    icon: 'success',
                    title: '¡Éxito!',
                    text: 'La cuenta se creó correctamente.'
                }).then(() => {
                    handleClose();
                });
            } catch (error) {
                
                swal({
                    icon: 'error',
                    title: '¡Error!',
                    text: 'Hubo un problema al crear la cuenta. Por favor, inténtalo de nuevo más tarde.'
                });
            }
            finally {
                setLoading(false);
            }
        }
    };


    return (
        <>
        {loading && <div className="loader">CREANDO CUENTA...</div>}
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
                            Agregar Cuentas
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

                                    <Grid container >
                                    <Grid item xs={2}>
                                                <TituloIcono
                                                    title1="Número"
                                                    subtitle="Radicado"
                                                    icono="event_available" />
                                            </Grid>
                                            <Grid item xs={3}>
                                                    <Controller
                                                        name="radicado"
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
                                                                defaultValue={0}
                                                                value={value}
                                                                helperText={error ? error.message : null}
                                                                onChange={onChange} />
                                                        )} />
                                                </Grid>
                                        <Grid item xs={1}>
                                            <span>{' '}</span>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Tipo De"
                                                subtitle="Cuenta"
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
                                                title1="Modalidad"
                                                subtitle="Cuenta"
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3} >
                                            <Controller
                                                name="Modalidad"
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
                                                        {modalidad?.map(
                                                            (option: MODALIDADAFILIACION) => (
                                                                <MenuItem
                                                                    key={option.idTipoAfiliacion}
                                                                    value={option.idTipoAfiliacion}
                                                                >
                                                                    {option.descripcionTipoAfiliacion}
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
                                                title1="Porcentaje"
                                                subtitle="Cuenta"
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3} >
                                            <Controller
                                                name="PorcentajeCuenta"
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
                                                        {porcentajeDescuento?.map(
                                                            (option: PORCENTAJEDESCUENTO) => (
                                                                <MenuItem
                                                                    key={option.idPorcentajeDescuento}
                                                                    value={option.idPorcentajeDescuento}
                                                                >
                                                                    {option.descripcionPorcentajeDescuento}
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
                                    <Grid container >
                                    <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Fecha"
                                                subtitle="Primer Aporte"
                                                icono="event_available"
                                            />
                                        </Grid>

                                        <Grid item xs={3} >
                                            <Controller
                                                name="FechaPrimerAporte"
                                                control={control}
                                                rules={{
                                                    required: 'Este campo es requerido.',
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHDatePicker
                                                        error={!!error}
                                                        helperText={error ? error.message : null}
                                                        onChange={onChange}
                                                        value={value}
                                                    />
                                                )}
                                            />
                                        </Grid>

                                         <Grid item xs={1}>
                                            <span>{' '}</span>
                                        </Grid>
                                        <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Fecha"
                                                subtitle="Último Aporte"
                                                icono="event_available"
                                            />
                                        </Grid>

                                        <Grid item xs={3} >
                                            <Controller
                                                name="FechaUltimoAporte"
                                                control={control}
                                                rules={{
                                                    required: 'Este campo es requerido.',
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHDatePicker
                                                        error={!!error}
                                                        helperText={error ? error.message : null}
                                                        onChange={onChange}
                                                        value={value}
                                                    />
                                                )}
                                            />
                                        </Grid>  
                                    </Grid>

                                    <br />
                                    <br />

                                    <Grid container >
                                    <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Número"
                                                subtitle="Cuotas"
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3} >
                                            <Controller
                                                name="NumeroCuotas"
                                                control={control}
                                                rules={{
                                                    required: 'Este campo es requerido.',
                                                    max: {
                                                        value: 168,
                                                        message: 'El número de cuotas no debe ser mayor a 168'
                                                    }
                                                }}
                                                render={({
                                                    field: { onChange, value },
                                                    fieldState: { error },
                                                }) => (
                                                    <CHTextField
                                                        numberMask
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
                                                title1="Causal De"
                                                subtitle="Cuenta"
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3} >
                                            <Controller
                                                name="CausalDeCuenta"
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
                                                        {causalEstadoCuentas?.map(
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
                                       
                                    </Grid>
                                    <br />
                                    <br />
                                    <Grid container>
                                    { ejecutora && (
                                        <><Grid item xs={2}>
                                                <TituloIcono
                                                    title1="Asignación"
                                                    subtitle="Salarial"
                                                    icono="event_available" />
                                            </Grid>
                                            <Grid item xs={3}>
                                                    <Controller
                                                        name="AsignacionSalarial"
                                                        control={control}
                                                        rules={{
                                                            required: 'Este campo es requerido.',
                                                        }}
                                                        render={({
                                                            field: { onChange, value },
                                                            fieldState: { error },
                                                        }) => (
                                                            <CHTextField
                                                                currencyMask
                                                                error={!!error}
                                                                defaultValue={0}
                                                                value={value}
                                                                helperText={error ? error.message : null}
                                                                onChange={onChange} />
                                                        )} />
                                                </Grid>
                                                <Grid item xs={1}>
                                                    <span>{' '}</span>
                                                </Grid>
                                                </>
                                                
                                        )}
                                       
                                        <Grid item xs={2}>
                                            <TituloIcono
                                                title1="Observación"
                                                subtitle="Cuenta"
                                                icono="event_available"
                                            />
                                        </Grid>
                                        <Grid item xs={3}>
                                                    <Controller
                                                        name="observacion"
                                                        control={control}
                                                        
                                                        render={({
                                                            field: { onChange, value },
                                                            fieldState: { error },
                                                        }) => (
                                                            <CHTextField
                                                                value={value}
                                                                helperText={error ? error.message : null}
                                                                onChange={onChange} />
                                                        )} />
                                        </Grid>
                                    </Grid>
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

    open: boolean;
    handleClose: () => void;
    tipoCuentaC: TIPODECUENTAC[];
    porcentajeDescuento: PORCENTAJEDESCUENTO[];
    causalEstadoCuentas: CAUSALESTADOCUENTA[];
    idUnidadEjecuta: number;
    idCliente: number;
    startCrearCuentas: (data: any) => void;
    startGetAdministracionCuentasAfiliado: (data: AdministracionParametroModel) => void;
    modalidad: MODALIDADAFILIACION[];
    ejecutora:boolean;
}

const mapDispatchToProps = {
    
    startGetAdministracionCuentasAfiliado,
};



type Props = LinkMapDispatchToProps & LinkMapStateToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ModalNuevaCuentaUsuario)