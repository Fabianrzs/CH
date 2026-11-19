import React from 'react';
import {
    Button,
    Dialog,
    TextField,
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

export const ModalAgregarGenero = (props: any) => {
    const {
        handleClose,
        open,
        handleAgregar,
        formularioGenero
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
                    <Card style={{width: '400px'}}>
                        <CardContent>
                            <Grid item>
                                    <Typography
                                     className={classes.titleform}
                                     variant="h5"
                                     component="h6"
                                    >
                                        AGREGAR GÉNERO                                       
                                    </Typography>
                            </Grid> 
                            <br/>  
                            <Grid container spacing={3}>                               
                                <Grid item xs={12}>
                                <Controller
                                        name="codigo"
                                        control={formularioGenero.control}
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
                                                label="Código *"
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                isReadOnly={false}
                                                InputLabelProps={{ shrink: true }}
                                                value={value}
                                                onChange={e => {
                                                    if(e.target.value.length <= 10)
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
                                        control={formularioGenero.control}
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
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={formularioGenero.handleSubmit(handleAgregar)}
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