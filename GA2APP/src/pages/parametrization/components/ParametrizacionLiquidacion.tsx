import React, { useEffect, useState } from 'react';
import { createStyles,
         Button, 
         Grid, 
         makeStyles} from '@material-ui/core';
import { connect } from 'react-redux';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { AppState } from '../../../store/rootReducer';
import { LiquidacionGeneralDto } from '../../../types/parametrization.model.type';
import { startPostUpdateParametrizationLiquidacionGeneral 
} from '../../../actions/parametrization.action';
import ApiParametrization from '../../../services/api-parametrization.service';
import { LiquidacionGeneralForm } from '../constants/form.model.type';

const useStyles = makeStyles(() =>
    createStyles({        
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        }, 
        column: {
            padding: '10px',
            textAlign: 'center',
        },   
        button: {
         width: '100%',
         marginLeft: '.5rem',
         marginTop: '1.5rem',
               },   
    }),
);

const ParametrizacionLiquidacion = (props: props) => {

    const classes = useStyles();
    const api = ApiParametrization();
    const {startPostUpdateParametrizationLiquidacionGeneral} = props;

    const formularioLiquidacionGeneral = useForm ({
        defaultValues:{
            idLiquidacion: 0,
            fechaCorte: '',
            fechaPago: '',
            estado: 1,
            fechaModificacion: new Date(),
            modificadoPor: 0,
        }
    })

    useEffect(() => {
        obtenerDatos();
    },[])

    const obtenerDatos = async() => {
        await api.GetAllParametrizationLiquidacionGeneral()
        .then(async (response) => {
            const datos = response.data as LiquidacionGeneralDto;
            formularioLiquidacionGeneral.setValue('idLiquidacion',datos.idLiquidacion);
            formularioLiquidacionGeneral.setValue('fechaCorte',String(datos.fechaCorte));
            formularioLiquidacionGeneral.setValue('fechaPago',String(datos.fechaPago));
            formularioLiquidacionGeneral.setValue('estado',datos.estado);
            formularioLiquidacionGeneral.setValue('fechaModificacion',datos.fechaModificacion);
            formularioLiquidacionGeneral.setValue('modificadoPor',datos.modificadoPor);

        })
        .catch(() => {
            toast("Error del sistema", { type: toast.TYPE.ERROR });
        });
    }

    const handleStateUpdate = async(data: any) => {
        const datosLiquidacionGeneral = {
            idLiquidacion: data.idLiquidacion,
            fechaCorte: Number(data.fechaCorte),
            fechaPago: Number(data.fechaPago),
            estado: data.estado,
            fechaModificacion: new Date(),
            modificadoPor: data.modificadoPor,
        }
        await startPostUpdateParametrizationLiquidacionGeneral(datosLiquidacionGeneral);
        obtenerDatos();
        toast.success("Información actualizada");
    }

    const handleUpdate = (data: any) => {
            swal({
                text: "¿Está seguro que desea actualizar la información?",
                icon: "warning",
                buttons: ["Sí", "No"]
            }).then (respuesta =>{
                if(!respuesta){   
                    handleStateUpdate(data);
                }
            })
    }

    const handleCancelar = (data: any) => {
        obtenerDatos();
    }

    return  (  
        <>
        <Grid container>
            <Grid className={classes.column} item xs={12} md={3}> 
            <Controller
                    name="fechaCorte"
                    control={formularioLiquidacionGeneral.control}
                    rules={{
                        required: 'Este campo es requerido.',
                        min: {
                            value: 1,
                            message:
                                'No puede ser igual o menor a 0',
                        },
                        max: {
                            value: 30,
                            message:
                                'No puede ser mayor a 30',
                        }
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                                }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'left',
                                        },
                                    }}
                                    label="Fecha Corte *"
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value.length <=
                                            2
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
                    name="fechaPago"
                    control={formularioLiquidacionGeneral.control}
                    rules={{
                        required: 'Este campo es requerido.',
                        min: {
                            value: 1,
                            message:
                                'No puede ser igual o menor a 0',
                        },
                        max: {
                            value: 30,
                            message:
                                'No puede ser mayor a 30',
                        }
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                                }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'left',
                                        },
                                    }}
                                    label="Fecha de Pago *"
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value.length <=
                                            2
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
            <Grid container> 
            <Grid className={classes.column} item xs={12} md={6} />                              
            <Grid className={classes.column} item xs={12} md={3}>
                    <Button 
                        className={classes.button}
                        variant="contained"
                        color="primary"
                        onClick={formularioLiquidacionGeneral.handleSubmit(handleUpdate)}  
                    >
                        ACTUALIZAR
                    </Button>
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Button 
                        className={classes.button}
                        variant="contained"
                        color="default"
                        onClick={handleCancelar}  
                    >
                        CANCELAR
                    </Button>
                </Grid>
            </Grid>          
        </Grid>
        </> 
    );
}

const mapStateToProps = (state: AppState) => ({
    parametrizacionState: state.parametrization,
});
interface LinkMapDispatchToProps {
    startPostUpdateParametrizationLiquidacionGeneral: (liquidacionGeneral: LiquidacionGeneralDto) => void;
}

const mapDispatchToProps = {
    startPostUpdateParametrizationLiquidacionGeneral
};

type props = LinkMapDispatchToProps;

export default connect (mapStateToProps,mapDispatchToProps)(ParametrizacionLiquidacion);
