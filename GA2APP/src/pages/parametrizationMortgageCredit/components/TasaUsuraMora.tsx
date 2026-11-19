import { Button, createStyles, Grid, makeStyles, Theme } from "@material-ui/core";
import React from "react";
import { Controller, useForm } from "react-hook-form";
import CHDatePicker from "../../../components/ch-datepicker/CHDatepicker";
import CHPaper from "../../../components/ch-paper/CHPaper";
import CHPrivateContainer from "../../../components/ch-private-container/CHPrivateContainer";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import UtilService from "../../../utils/util.service";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        checkContainer: {
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            paddingLeft: 30,
            color: theme.palette.success.main,
        },
        inactiveContainer: {
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            paddingLeft: 45,
            color: '#C44',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);


const TasaUsuraMora = () =>{

    const classes = useStyles();
    const formularioParametrosSimuladorTasa = useForm({
        defaultValues: {
  
            tasaMora: 0,
            tasaUsura: 0,
            fechaInicialTasaUsura: new Date(),
            fechaFinalTasaUsura: new Date(),

        }
    })

    const guardar = (data: any) => {
        console.log(data,"data")
    }

    return(
        
        <div style = {{marginTop: "30px", width: "100%"}}>
        {/* <CHPrivateContainer> */}
        <CHPaper  title="TASAS " >
      
        <Grid container>
        <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tasaUsura"
                            control={formularioParametrosSimuladorTasa.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                min: {
                                    value: 0,
                                    message:
                                        'No puede ser menor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'No puede ser mayor a 100',
                                }
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                decimalsMaskByNumber
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Tasa Usura (%)*"
                                   
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tasaMora"
                            control={formularioParametrosSimuladorTasa.control}

                            rules={{
                                required: 'Este campo es requerido.',
                                min: {
                                    value: 0,
                                    message:
                                        'No puede ser menor a 0',
                                },
                                max: {
                                    value: 100,
                                    message:
                                        'No puede ser mayor a 100',
                                }
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                decimalsMaskByNumber
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Tasa Mora (%) *"
                                   
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={3} className={classes.column}>
                        <Controller
                            name="fechaInicialTasaUsura"
                            control={formularioParametrosSimuladorTasa.control}
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
                                    label="Fecha inicial Tasa Usura"
                                    onChange={onChange}
                                    value={  UtilService.getDate(value) ===
                                        undefined
                                            ? ''
                                            : UtilService.getDate(value) }
                                    disableFuture
                                    InputLabelProps={{ shrink: true }}
                            />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={3} className={classes.column}>
                        <Controller
                            name="fechaFinalTasaUsura"
                            control={formularioParametrosSimuladorTasa.control}
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
                                    label="Fecha final Tasa Usura"
                                    onChange={onChange}
                                    value={  UtilService.getDate(value) ===
                                        undefined
                                            ? ''
                                            : UtilService.getDate(value) }
                                    disableFuture
                                    InputLabelProps={{ shrink: true }}
                            />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={3}>
                    <Button
                        onClick={formularioParametrosSimuladorTasa.handleSubmit(guardar)}
                        variant="contained"
                        style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                    >
                        Guardar
                    </Button>
                </Grid>
         </Grid>
         </CHPaper>
         {/* </CHPrivateContainer> */}
        </div>
        
    )
}
export default TasaUsuraMora