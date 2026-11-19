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

export const ModalEditarCategoria = (props: any) => {
    const {
        handleClose,
        open,
        handleEditar,
        formularioCategoria,
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
                                        ACTUALIZAR CATEGORÍA                                        
                                    </Typography>
                            </Grid> 
                            <br/>  
                            <Grid container spacing={3}>                               
                                <Grid item xs={12}>
                                <Controller
                                        name="descripcion"
                                        control={formularioCategoria.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                                    }) => (
                                                    <CHTextField
                                                        InputLabelProps={{ shrink: true }}
                                                        error={!!error}
                                                        helperText={
                                                            error ? error.message : null
                                                        }
                                                        label="Descripción *"
                                                        value={value}
                                                        inputProps={{
                                                            style: {
                                                                textTransform: 'uppercase',
                                                            },
                                                        }}
                                                        onChange={e => {
                                                            if(e.target.value.length <= 40){
                                                                onChange(e);
                                                            }
                                                          }
                                                        }
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
                                        onClick={formularioCategoria.handleSubmit(handleEditar)}
                                    >
                                        Actualizar
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