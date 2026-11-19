import { Grid, makeStyles, createStyles, Button} from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import NotificationService from '../../../services/notification.service';
import Convocatoria from './Convocatoria';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import { ESTADO } from '../constants/fondoSolidaridad.constants';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },   
        columnRight: {
            padding: '10px',
            textAlign: 'right',
        },
        button: {
            marginTop: '1.5rem',
            marginRight: '.5rem',
        },
    }),
);

const AgregarConvocatoria = (props: typeProps) => {
    
    const classes = useStyles();
    const {setAgregarConvocatoria, consultarConvocatorias} = props;
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const [convocatoriaInicio,setConvocatoriaInicio] = useState(false);

    const formularioConvocatoria = useForm({
        defaultValues:{
            idConvocatoria: '',
            fechaApertura: new Date(),
            fechaCierre: new Date(),
            fechaPublicacion: new Date (),
            fechaResolucion: new Date (),
            numeroBeneficiario: null, 
            numeroResolucion: null,
            numeroConvocatoria: null,
            numeroDiarioOficial: null,
            estadoConvocatoriaId: '',
        }
    })

    useEffect(() => {
        consultarUltimoNumeroConvocatoria();
    }, []);

    const consultarUltimoNumeroConvocatoria = async () => {
        await apiFondoSolidaridad
            .getObtenerNumeroConvocatoriaProximaParaCrear()
            .then(async response => {
                if(response.data){
                    formularioConvocatoria.setValue('numeroConvocatoria',response.data);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const agregarConvocatoriaOnClick = (data: any) => {
        const datosConvocatoria = {
            fechaApertura: data.fechaApertura,
            fechaCierre: data.fechaCierre,
            fechaPublicacion: data.fechaPublicacion,
            numeroBeneficiario: Number(data.numeroBeneficiario), 
            numeroResolucion: Number(data.numeroResolucion),
            numeroDiarioOficial: Number(data.numeroDiarioOficial),
            estadoConvocatoriaId: ESTADO.APERTURA,
            fechaResolucion: data.fechaResolucion,
            numeroConvocatoria: data.numeroConvocatoria
        }
        crearConvocatoria(datosConvocatoria);
    }

    const crearConvocatoria = async (convocatoria: any) => {
        await apiFondoSolidaridad
            .postCrearConvocatoria(convocatoria)
            .then(async response => {
                if(response.data && response.returnMessage === 'Creado con éxito'){
                    await NotificationService.genericAlert(
                        'Guardado',
                        'Se ha guardado correctamente la información',
                    );
                    setConvocatoriaInicio(true);
                    setAgregarConvocatoria(false);
                    formularioConvocatoria.reset();
                    const busqueda = {
                        numeroConvocatoria:'',
                        estado: '',
                        fechaDesde: '',
                        fechaHasta: '',
                        fechaPublicacion: '',
                     }
                    consultarConvocatorias(busqueda);
                }else if (response.data === null && 
                          response.isSuccess === false && 
                          response.returnMessage !== 'Creado con éxito') {
                    await NotificationService.genericAlert(
                        '',
                        response.returnMessage,
                        'warning',
                    );
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const regresarInicio = () => setConvocatoriaInicio(true);

    if(convocatoriaInicio){
       return ( 
            <Convocatoria/> 
       )
    }

    return (
      
        <CHPaper title="Información convocatoria">
        <Grid container>
            <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="fechaApertura"
                    control={formularioConvocatoria.control}
                    rules={{
                        required: {
                            value: true,
                            message: 'Este campo es requerido',
                        },
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHDatePicker
                            readOnly
                            label="Fecha Apertura"
                            onChange={onChange}
                            value={value}
                            inputProps={{ readOnly: true }}
                            error={!!error}
                            helperText={error ? error.message : null}
                            autoOk
                        />
                    )}
                />
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="fechaCierre"
                    control={formularioConvocatoria.control}
                    rules={{
                        required: {
                            value: true,
                            message: 'Este campo es requerido',
                        },
                    }}
                    render={({ field: { onChange, value },
                               fieldState: { error },
                     }) => (
                        <CHDatePicker
                            label="Fecha Cierre *"
                            onChange={onChange}
                            value={value}
                            inputProps={{ readOnly: true }}
                            error={!!error}
                            helperText={error ? error.message : null}
                            minDate= {new Date()}
                            autoOk
                        />
                    )}
                />
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                    name="numeroConvocatoria"
                    control={formularioConvocatoria.control}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHTextField
                            isReadOnly
                            InputLabelProps={{ shrink: true }}
                            label="Número de Convocatoria "
                            value={value}
                            onChange={onChange}

                        />
                    )}
                />
            </Grid>
             <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                    name="numeroBeneficiario"
                    control={formularioConvocatoria.control}
                    rules={{
                        required: {
                            value: true,
                            message: 'Este campo es requerido',
                        },
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHTextField
                            error={!!error}
                            helperText={error ? error.message : null}
                            isReadOnly={false}
                            InputLabelProps={{ shrink: true }}
                            label="Número de Beneficiarios *"
                            value={value}
                            onChange={e => {
                                if (
                                    e.target.value.length <=
                                    4
                                ) {
                                    onChange(
                                        e.target.value.replace(
                                            /[\WA-Za-z]/g,
                                            '',
                                        ),
                                    );
                                }
                            }}

                        />
                    )}
                />
            </Grid>
          </Grid>
          <Grid container>
          <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                    name="numeroResolucion"
                    control={formularioConvocatoria.control}
                    rules={{
                        required: {
                            value: true,
                            message: 'Este campo es requerido',
                        },
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHTextField
                            error={!!error}
                            helperText={error ? error.message : null}
                            isReadOnly={false}
                            InputLabelProps={{ shrink: true }}
                            label="Número de Resolución de Apertura *"
                            value={value}
                            onChange={e => {
                                if (
                                    e.target.value.length <=
                                    4
                                ) {
                                    onChange(
                                        e.target.value.replace(
                                            /[\WA-Za-z]/g,
                                            '',
                                        ),
                                    );
                                }
                            }}

                        />
                    )}
                />
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="fechaResolucion"
                    control={formularioConvocatoria.control}
                    render={({ field: { onChange, value },
                               fieldState: { error },
                    }) => (
                        <CHDatePicker
                            label="Fecha de Resolución de Apertura"
                            onChange={onChange}
                            value={value}
                            inputProps={{ readOnly: true }}
                            error={!!error}
                            helperText={error ? error.message : null}
                            autoOk
                        />
                    )}
                />
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                    name="numeroDiarioOficial"
                    control={formularioConvocatoria.control}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHTextField
                            error={!!error}
                            helperText={error ? error.message : null}
                            isReadOnly={false}
                            InputLabelProps={{ shrink: true }}
                            label="Número Diario Oficial"
                            value={value}
                            onChange={e => {
                                if (
                                    e.target.value.length <=
                                    6
                                ) {
                                    onChange(
                                        e.target.value.replace(
                                            /[\WA-Za-z]/g,
                                            '',
                                        ),
                                    );
                                }
                            }}

                        />
                    )}
                />
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="fechaPublicacion"
                    control={formularioConvocatoria.control}
                    render={({ field: { onChange, value },
                               fieldState: { error },
                    }) => (
                        <CHDatePicker
                            label="Fecha Publicación"
                            onChange={onChange}
                            value={value}
                            inputProps={{ readOnly: true }}
                            error={!!error}
                            helperText={error ? error.message : null}
                            autoOk
                        />
                    )}
                />
            </Grid>
          </Grid>
          <Grid container> 
            <Grid className={classes.columnRight} item xs={12} md={12}>
                <Button
                    className={classes.button}
                    variant="contained"
                    color="primary"
                    style={{ width: '250px' }}
                    onClick={formularioConvocatoria.handleSubmit(agregarConvocatoriaOnClick)}
                >
                    AGREGAR CONVOCATORIA
                </Button>
                <Button
                    className={classes.button}
                    variant="contained"
                    color="default"
                    style={{ width: '200px' }}
                    onClick={regresarInicio}
                >
                    CANCELAR
                </Button>
             </Grid>
            </Grid>
        </CHPaper>
    )
}

type typeProps = {
    setAgregarConvocatoria: (value: boolean) => void;
    consultarConvocatorias: (busqueda: any) => void;
}

export default AgregarConvocatoria;