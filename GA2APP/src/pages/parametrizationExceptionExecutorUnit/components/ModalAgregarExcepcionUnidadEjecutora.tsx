import {
    Button,
    Dialog,
    Card,
    CardContent,
    Container,
    Grid,
    Typography,
    MenuItem,
    createStyles,
    makeStyles,
} from '@material-ui/core';
import React from 'react';
import { Controller } from 'react-hook-form';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { UnidadEjecutoraDto } from '../../../types/simulador.model.type';

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
    }),
);
export const ModalAgregarExcepcionUnidadEjecutora = (props: any) => {    
    const {
        handleClose,
        open,
        handleOnClickAgregar,
        allExecutorUnit,  
        formularioUnidadEjecutora  
    } = props;
    const classes = useStyles();   
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md">
                    <Card>
                        <CardContent> 
                        <Grid item>
                                    <Typography
                                     className={classes.titleform}
                                     variant="h5"
                                     component="h6"
                                    >
                                        AGREGAR EXCEPCIÓN UNIDAD EJECUTORA                                       
                                    </Typography>
                                </Grid>
                                     <br/>
                                <Grid container spacing={3}>                               
                                <Grid item xs={12}>
                                <Controller
                                name="idParametroUnidadEjecutora"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formularioUnidadEjecutora.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        label="Unidad ejecutora *"
                                        onChange={onChange}     
                                        value={value}  
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }         
                                    >
                                        {allExecutorUnit?.map(
                                            (option: UnidadEjecutoraDto) => (
                                                <MenuItem
                                                    key={option.idUnidadEjecutora}
                                                    value={option.idUnidadEjecutora}
                                                >
                                                    {option.nombreUnidadEjecutora}
                                                </MenuItem>
                                            ),
                                        )} 
                                </CHSelect>
                                )}
                            />
                            </Grid>
                            </Grid>                                                                
                            <Grid container spacing={3}>                               
                                <Grid item xs={12}>
                                <Controller
                                    name="tasaEA"
                                    control={formularioUnidadEjecutora.control}
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
                                            InputLabelProps={{ shrink: true }}
                                            label="Tasa *"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            value={value}
                                            onChange={e => {
                                                if (
                                                    e.target.value.length <=
                                                    5
                                                ) {
                                                    onChange(e);
                                                }
                                            }}
                                        />
                                    )}
                                />
                                </Grid>
                            </Grid>                          
                            <Grid container spacing={3}>
                            <Grid item xs={12}>
                            <Controller
                                name="estado"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formularioUnidadEjecutora.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    label="Estado"
                                    name="estado"  
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    value={value}
                                    readOnly                       
                                >
                                        <MenuItem value={1}>ACTIVO</MenuItem>
                                    </CHSelect>
                                )}
                            />
                            </Grid>
                         </Grid>
                            <Grid container spacing={3}>
                                <Grid item>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={formularioUnidadEjecutora.handleSubmit(handleOnClickAgregar)}
                                    >
                                        Agregar
                                    </Button>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={handleClose}
                                        style={{marginLeft: "10px"}}
                                    >
                                        Cancelar
                                    </Button>
                                </Grid>
                            </Grid>
                        </CardContent>
                    </Card>                   
                </Container>
            </Dialog>
        </div>
    );
};