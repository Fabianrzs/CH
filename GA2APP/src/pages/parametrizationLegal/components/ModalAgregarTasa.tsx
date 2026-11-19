import {
    Button,
    Dialog,
    Card,
    CardContent,
    Container,
    Grid,
    Typography,
    makeStyles,
    createStyles
} from '@material-ui/core';
import React from 'react';
import { Controller } from 'react-hook-form';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
    createStyles({   
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },       
    }),
);

export const ModalAgregar = (props: any) => {
    const {
        handleClose,
        open,
        handleOnClickSave,
        formularioParametrizacionLegal
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
                    <Card style={{width: '360px'}}>
                        <CardContent>
                        <Grid item>
                            <Typography
                                className={classes.titleform}
                                variant="h5"
                                component="h6"
                            >
                                AGREGAR TASA FRECH                                     
                            </Typography>
                            </Grid> 
                            <br/>  
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                <Controller
                                        name="tasaFrech"
                                        control={formularioParametrizacionLegal.control}
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
                                                        InputLabelProps={{ shrink: true }}
                                                        inputProps={{
                                                            style: {
                                                                textAlign: 'left',
                                                            },
                                                        }}
                                                        label="Tasa Frech *"
                                                        value={value}
                                                        onChange={onChange}
                                                    />
                                             )}
                                        />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                <Controller
                                    name="vigenciaTasaFrech"
                                    control={formularioParametrizacionLegal.control}
                                    rules={{
                                        required: 'Este campo es requerido.'
                                    }}
                                    render={({ field: { onChange, value },  
                                        fieldState: { error }, 
                                    }) => (
                                        <CHDatePicker
                                            label="Vigencia Tasa Frech *"
                                            onChange={onChange}
                                            value={value}
                                            inputProps={{ readOnly: true }}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            autoOk
                                        />
                                     )}
                                />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={formularioParametrizacionLegal.handleSubmit(handleOnClickSave)}
                                    >
                                        Agregar
                                    </Button>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={handleClose}
                                        style={{
                                            marginLeft: "10px"
                                        }}
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
