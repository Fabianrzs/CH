import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
    Paper,
    Theme,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import {
    startConsultaLegalizacion,
    startVerificacionTerceros,
} from '../../actions/credit.actions';
import CHPaper from '../ch-paper/CHPaper';
import CHSelect from '../ch-select/CHSelect';
import { AppState } from '../../store/rootReducer';
import CHTextField from '../ch-text-field/CHTextField';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHPrivateContainer from '../ch-private-container/CHPrivateContainer';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import CreditApi from '../../services/api-credit.service';
import CHDatePicker from '../ch-datepicker/CHDatepicker';
import NotificationService from '../../services/notification.service';
import BotoneraInformacionSolicitudComponent from '../../pages/solicitudes/components/botonera/botonera-informacion-solicitud-component';


const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        column: {
            padding: '10px',
           
        },
        button: {
          width:'200px',
            marginLeft: '.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        root: {
            minWidth: '100%',
            margin: '20px',
        },
        bullet: {
            display: 'inline-block',
            margin: '0 2px',
            transform: 'scale(0.8)',
        },
        title: {
            fontSize: 14,
        },
        card: {
            margin: '30px',
        },

        rootDos: {
            flexGrow: 1,
        },
        paper: {
            padding: theme.spacing(2),
            textAlign: 'center',
            color: theme.palette.text.secondary,
        },
    }),
);

const api = CreditApi();

function RequestInformationLegalizationData({
    procedureRequest,
    startVerificacionTerceros,
    terceros,
    isLoading,
}: {
    procedureRequest: ProcedureRequestModel;

    startVerificacionTerceros: (datos: any) => void;
  
    terceros: any;
    isLoading:boolean;
}) {
    const [encontrado, setEncontrado] = useState('');
    const [informacionEscritura, setInformacionEscritura] = useState(false);
    const [datosLegalizacion, setDatosLegalizacion] = useState(false);
  
    const [verNuemeroCDP, setVerNuemeroCDP] = useState(false);
    const [numeroCDP, setNumeroCDP] = useState("false");
    const {
        control: control1,
        handleSubmit,
        setValue,
    } = useForm<any>({
        defaultValues: {
            verificarDesembolso: '',
            observacion: '',
        },
    });

    const {
        control: control2,
        handleSubmit: handleSubmit2,
        setValue: setValue2,
    } = useForm<any>({
        defaultValues: {
            validaFirmas: '',
            observacion: '',
        },
    });

    const formularioEscritura = useForm<any>({
        defaultValues: {
            nro_solicitud:'',
            cli_identificacion: '',
            escritura: '',
            fecha_escritura: new Date(),
            notaria: '',
        },
    });

  
    useEffect(() => {
        if (terceros) {
            if (terceros.encontrado === 'N') {
                setEncontrado(
                    'No se han encontrado registros en la verificación de terceros',
                );
            } else if (terceros.encontrado === 'S') {
                setEncontrado(
                    'Si se han encontrado registros en la verificación de terceros',
                );
            }
        }
    }, [terceros]);

    useEffect(() => {
        const datos: any = {
            pe_Origen: 'GA2',
            pe_Dato: String(procedureRequest.clientIdentificationNumber), 
            pe_TipoVerificacion: '1',
            pe_Porcentaje: '' 
        };
        traerDatos();
        startVerificacionTerceros(datos);
        formularioEscritura.setValue('nro_solicitud',procedureRequest.requestNumber);
        formularioEscritura.setValue('cli_identificacion',procedureRequest.clientIdentificationNumber);
        
        const datosBusquedaEscritura = {
            solicitud_id: procedureRequest.requestID,
            cli_identificacion: String(procedureRequest.clientIdentificationNumber)
        }
        api
        .postObtenerEscritura(datosBusquedaEscritura)
        .then(async response => {
            if(response.data){
                formularioEscritura.setValue('escritura',response?.data.escritura);
                formularioEscritura.setValue('fecha_escritura',response?.data.fecha_escritura);
                formularioEscritura.setValue('notaria',response?.data.notaria);
                setInformacionEscritura(true);
            }
          })
         .catch(() => {
             toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
    }, []);

    const traerDatos = async () => {
        await api
            .GetLegalizacionData(procedureRequest.requestID)
            .then(async response => {
                cargarDatos(response.data);
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const onSubmit = async (data: any) => {
        await api
            .postLegalizacion({
                idSolicitudCredito: procedureRequest.requestID,
                observacionValidaDesembolso: data.observacion,
                validaDesembolso: data.verificarDesembolso,
                fechaActualiza: moment('08/02/2022').format(),
            })
            .then(async response => {
                cargarDatos(response.data);
                toast('Éxito , se actualizó correctamente', {
                    type: toast.TYPE.SUCCESS,
                });
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const onSubmit2 = async (dataDos: any) => {
        await api
            .postLegalizacionFirmas({
                idSolicitudCredito: procedureRequest.requestID,
                ObservacionValidaFirmas: dataDos.observacion,
                validaFirmas: dataDos.validaFirmas,
                fechaActualiza: moment('08/02/2022').format(),
            })
            .then(async response => {
                cargarDatos(response.data);
                toast('Éxito , se actualizó correctamente', {
                    type: toast.TYPE.SUCCESS,
                });
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const cargarDatos = (datos: any) => {
        setValue2('validaFirmas', datos.validaFirmas);
        setValue2('observacion', datos.observacionValidaFirmas);

        setValue('verificarDesembolso', datos.validaDesembolso);
        setValue('observacion', datos.observacionValidaDesembolso);
    };

    const classes = useStyles();


    const guardarEscritura = async(data: any) => {
        
        const datosEscritura = {
            solicitud_id: procedureRequest.requestID,
            cli_identificacion: String(procedureRequest.clientIdentificationNumber),
            escritura: data.escritura, 
            fecha_escritura: new Date(data.fecha_escritura),
            notaria: data.notaria,
        }

        if(informacionEscritura){
            await api
            .postActualizarEscritura(datosEscritura)
            .then(async response => {
                await NotificationService.genericAlert(
                     'Guardado',
                   'Información almacenada',
                 );
              })
             .catch(() => {
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        } else {

            await api
            .postCrearEscritura(datosEscritura)
            .then(async response => {
                await NotificationService.genericAlert(
                     'Guardado',
                   'Información almacenada',
                 );
              })
             .catch(() => {
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        }
    }



    if (isLoading) {
        return (
            <CHPrivateContainer>
                <CHLoadingProcess />
            </CHPrivateContainer>
        );
    }

    return (
        <>
          

            <CHPaper title="Verificación Desembolso">
                <Grid container>
                    <Grid item xs={12} md={4}>
                        <Controller
                            name="verificarDesembolso"
                            control={control1}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Verificación Desembolso *"
                                    onChange={onChange}
                                    value={value}
                                >
                                    <MenuItem value="">SELECCIONE</MenuItem>
                                    <MenuItem value="S">SI</MenuItem>
                                    <MenuItem value="N">NO</MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="observacion"
                            control={control1}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    label="Observación *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <div className="buttons">
                        <Button
                            onClick={handleSubmit(onSubmit)}
                            className={classes.button}
                            variant="outlined"
                        >
                            ACTUALIZAR
                        </Button>
                    </div>
                </Grid>
            </CHPaper>
            <CHPaper title="Verificación de Firmas">
                <Grid container>
                    <Grid item xs={12} md={4}>
                        <Controller
                            name="validaFirmas"
                            control={control2}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Verificación firmas *"
                                    onChange={onChange}
                                    value={value}
                                >
                                    <MenuItem value="">SELECCIONE</MenuItem>
                                    <MenuItem value="S">SI</MenuItem>
                                    <MenuItem value="N">NO</MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="observacion"
                            control={control2}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    label="Observación *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <div className="buttons">
                        <Button
                            onClick={handleSubmit2(onSubmit2)}
                            className={classes.button}
                            variant="outlined"
                        >
                            ACTUALIZAR
                        </Button>
                    </div>
                </Grid>
            </CHPaper>
            <CHPaper title="Datos Inmueble">
                <Grid container>
                    <Grid item xs={12} md={4}>
                        <Controller
                            name="escritura"
                            control={formularioEscritura.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    label="Escritura"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="fecha_escritura"
                            control={formularioEscritura.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Fecha Escritura"
                                    onChange={onChange}
                                    value={value}
                                    disableFuture
                                    InputLabelProps={{ shrink: true }}
                            />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={4}>
                        <Controller
                            name="notaria"
                            control={formularioEscritura.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMaskInt
                                    error={!!error}
                                    label="Notaria"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={(e) => {
                                       const inputValue = e.target.value;
                                        if (/^\d{0,3}$/.test(inputValue)) {
                                            onChange(inputValue);
                                        }
                                    }}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <div className="buttons">
                        <Button
                            onClick={formularioEscritura.handleSubmit(guardarEscritura)}
                            className={classes.button}
                            variant="outlined"
                        >
                            GUARDAR
                        </Button>
                    </div>
                </Grid>
               
            </CHPaper>
         

        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    legalizacion: state.credit.legalizacion,
    answerLegalizacion: state.credit.answerLegalizacion,
    firmas: state.credit.firmas,
    terceros: state.credit.terceros,
});

const mapDispatchToProps = {
    startConsultaLegalizacion,

    startVerificacionTerceros,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RequestInformationLegalizationData);
