import {
    Button,
    Dialog,
    Card,
    CardContent,
    Container,
    Grid,
    Typography,
    MenuItem,
    makeStyles,
    createStyles,
    } from '@material-ui/core';
    import React from 'react';
    import { Controller } from 'react-hook-form';
    import CHSelect from '../../../components/ch-select/CHSelect';
    import CHTextField from '../../../components/ch-text-field/CHTextField';
    import { TipoActorDto } from '../../../types/simulador.model.type';
    
    const useStyles = makeStyles(() =>
        createStyles({   
            titleform: {
                width: '100%',
                textAlign: 'center',
                color: '#1C3F8B',
                padding: 1,
                fontWeight: 'bold',
                fontSize: '1rem',
            },       
        }),
    );
    export const ModalEditarExcepTipoAfiliado = (props: any) => {
        const {
            handleClose,
            open,
            handleOnClickUpdate,
            allActorType,
            formularioTipoActor
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
                                            ACTUALIZAR EXCEPCIÓN TIPO AFILIADO                                        
                                        </Typography>
                                    </Grid> 
                                    <br />
                                    <Grid container spacing={3}>                               
                                    <Grid item xs={12}>
                                    <Controller
                                            name="tipoId"
                                            control={formularioTipoActor.control}
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
                                                <CHSelect
                                                    label="Tipo Afiliado *"
                                                    onChange={onChange}
                                                    value={value}
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                >
                                                {allActorType?.map(
                                                    (option: TipoActorDto) => (
                                                        <MenuItem
                                                            key={option.idTipoActor}
                                                            value={option.idTipoActor}
                                                        >
                                                            {option.descripcionTipoActor}
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
                                            name="parametroTasaEa"
                                            control={formularioTipoActor.control}
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
                                                            label="Tasa *"
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
                                    name="estado"
                                    control={formularioTipoActor.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Estado"
                                            value={value}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                             <MenuItem value={1}>ACTIVO</MenuItem>
                                             <MenuItem value={0}>INACTIVO</MenuItem>
                                        </CHSelect>
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
                                            onClick={formularioTipoActor.handleSubmit(handleOnClickUpdate)}
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
    