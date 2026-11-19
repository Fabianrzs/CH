import React from 'react';
import {
    Button,
    Dialog,
    Card,
    CardContent,
    Container,
    Grid,
    Typography,
    makeStyles,
    createStyles,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';

import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';

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

export const ModalAgregarFuerzas = (props: any) => {
    const {
        handleClose,
        open,
        handleAgregar,
        formularioFuerza
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
                    <Card style={{width: '500px'}}>
                        <CardContent>
                            <Grid item>
                                    <Typography
                                     className={classes.titleform}
                                     variant="h5"
                                     component="h6"
                                    >
                                        AGREGAR FUERZA                                       
                                    </Typography>
                            </Grid> 
                            <br/>  
                            <Grid container spacing={3}>                               
                                <Grid item xs={12}>
                                <Controller
                                        name="codigo"
                                        control={formularioFuerza.control}
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
                                                label="Abreviatura *"
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                isReadOnly={false}
                                                InputLabelProps={{ shrink: true }}
                                                value={value}
                                                onChange={e => {
                                                    if(e.target.value.length <= 5)
                                                    onChange(e);
                                                }}
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}
                                                style={{
                                                    margin: "center"
                                                }}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>                               
                                <Grid item xs={12}>
                                <Controller
                                        name="descripcion"
                                        control={formularioFuerza.control}
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
                                                label="Descripción *"
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                isReadOnly={false}
                                                InputLabelProps={{ shrink: true }}
                                                value={value}
                                                onChange={e => {
                                                    if(e.target.value.length <= 50)
                                                    onChange(e);
                                                }}
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}
                                                style={{
                                                    margin: "center"
                                                }}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>                               
                                <Grid item xs={12}>
                                <Controller
                                        name="esSoldado"
                                        control={formularioFuerza.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHCheckbox 
                                                onChange={onChange}
                                                checked={value}
                                                color='primary'
                                                label="¿Es Soldado?"
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
                                        onClick={formularioFuerza.handleSubmit(handleAgregar)}
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