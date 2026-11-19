import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import swal from 'sweetalert';
import { Button, createStyles, Grid, makeStyles, Typography } from '@material-ui/core';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { AppState } from '../../../../store/rootReducer';
import { startPostUpdateParametrizationSeguroGeneral } from '../../../../actions/parametrization.action';
import { SegurosDto } from '../../../../types/parametrization.model.type';
import ApiParametrization from '../../../../services/api-parametrization.service';

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

const ParametrizacionSeguros = (props: props) => {
    const classes = useStyles();
    const api = ApiParametrization();
    const { startPostUpdateParametrizationSeguroGeneral } = props;

    const formularioSegurosGeneral = useForm({
        defaultValues:{
            idSeguro: 0,
            seguroVida: 0,
            seguroTodoRiesgo: 0,
            fechaModificacion: new Date(),
            modificadoPor: 0,
            seguroEstado: false,
            seg_Iva: 0,

        }
    })

    useEffect(() => {
        obtenerDatos();
    }, []);

    const obtenerDatos = async () => {
        await api
            .GetAllParametrizationSegurosGeneral()
            .then(async response => {
                const datos = response.data[0] as SegurosDto;
                formularioSegurosGeneral.reset();
                formularioSegurosGeneral.setValue('idSeguro',datos.idSeguro);
                formularioSegurosGeneral.setValue('seguroVida',datos.seguroVida);
                formularioSegurosGeneral.setValue('seg_Iva',datos.seg_Iva);
                formularioSegurosGeneral.setValue('fechaModificacion',datos.fechaModificacion);
                formularioSegurosGeneral.setValue('modificadoPor',datos.modificadoPor);
                formularioSegurosGeneral.setValue('seguroEstado',datos.seguroEstado);
           })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleOnClickUpdate = (data: any) => {

        const datosSeguroGeneral: SegurosDto = {
            idSeguro: data.idSeguro,
            seguroVida: Number(data.seguroVida),
            seg_Iva: data.seg_Iva,
            fechaModificacion: new Date(),
            modificadoPor: data.modificadoPor,
            seguroEstado: data.seguroEstado,
            seguroTodoRiesgo:data.seguroTodoRiesgo,
        }
   
        swal({
            text: '¿Está seguro que desea actualizar la información?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                handleUpdateState(datosSeguroGeneral);
            }
        });
    };

    const handleUpdateState = async (seguroGeneral: SegurosDto) => {
        await startPostUpdateParametrizationSeguroGeneral(seguroGeneral);
        obtenerDatos();
        toast.success('Información actualizada');
    };

    return (
        <>
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="seguroVida"
                    control={formularioSegurosGeneral.control}
                    rules={{
                        required:
                            'Este campo es requerido.',
                            min: {
                                value: 0,
                                message: 'No puede ser menor a 0'
                            },
                            max: {
                                value: 1,
                                message: 'No puede ser mayor a 1'
                            },
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                        }) => (
                                <CHTextField
                                    decimalsMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message || 'No es un número valido' : null
                                    }
                                    label="Seguro Vida (%) *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="seg_Iva"
                    control={formularioSegurosGeneral.control}
                    rules={{
                        required: 'Este campo es requerido',
                        min: {
                            value: 0,
                            message: 'No puede ser menor a 0'
                        },
                        max: {
                            value: 99,
                            message: 'No puede ser mayor a 99'
                        },
                    }}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                        }) => (
                            <CHTextField
                                decimalsMask
                                error={!!error}
                                helperText={
                                    error ? error.message || 'No es un número valido' : null
                                }
                                label="Tarifa IVA (%) *"
                                value={value}
                                onChange={onChange}
                                InputLabelProps={{ shrink: true }}
                                inputProps={{
                                    style: {
                                        textAlign: 'right',
                                    },
                                }}
                        />
                        )}
                    />
                  
                </Grid>
                <Grid className={classes.column} item xs={12} md={2}>
                    <Button
                        className={classes.button}
                        variant="contained"
                        color="default"
                        onClick={formularioSegurosGeneral.handleSubmit(handleOnClickUpdate)}
                    >
                        ACTUALIZAR
                    </Button>
                </Grid>
                <Grid className={classes.column} item xs={12} md={2}>
                    <Button
                        className={classes.button}
                        variant="contained"
                        color="default"
                        style={{
                            marginLeft: '10px'
                        }}
                        onClick={obtenerDatos}
                    >
                        CANCELAR
                    </Button>
                </Grid>
            </Grid>
         
        </>

    );
};

const mapStateToProps = (state: AppState) => ({
    parametrizacionState: state.parametrization,
});

interface LinkMapDispatchToProps {
    startPostUpdateParametrizationSeguroGeneral: (
        seguroGeneral: SegurosDto,
    ) => void;
}

const mapDispatchToProps = {
    startPostUpdateParametrizationSeguroGeneral,
};

type props = LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ParametrizacionSeguros);
