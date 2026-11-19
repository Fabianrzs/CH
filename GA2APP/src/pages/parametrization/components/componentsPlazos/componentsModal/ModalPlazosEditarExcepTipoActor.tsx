import React from 'react';
import { Button, Card, 
         CardContent, 
         Container,
         createStyles,
         Dialog, 
         Grid, 
         makeStyles, 
         MenuItem, 
         Typography} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHSelect from '../../../../../components/ch-select/CHSelect';
import { TipoActorDto } from '../../../../../types/simulador.model.type';
import CHTextField from '../../../../../components/ch-text-field/CHTextField';

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

const ModalPlazosEditarExcepTipoActor = (props: any) => {
  const classes = useStyles();

  const {open,
         handleClose,
         allActorType,
         handleOnClickEditar,
         formularioPlazosTipoActor} = props;

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
                            ACTUALIZAR EXCEPCIÓN PLAZOS TIPO AFILIADO                                       
                        </Typography>
                        </Grid> 
                        <Grid container spacing={3}> 
                            <Grid item xs={12}>
                            <Controller
                                name="idTipoActor"  
                                control={formularioPlazosTipoActor.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    label= "Tipo Afiliado *"
                                    onChange={onChange}                                     
                                    value={value}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                            >
                                {
                                    allActorType?.map(
                                        (option: TipoActorDto) => (
                                            <MenuItem 
                                                key={option.idTipoActor}
                                                value={option.idTipoActor}
                                            >
                                                {option.descripcionTipoActor}
                                            </MenuItem>
                                        ))
                                }
                               
                            </CHSelect>
                                )}
                               />
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            <Grid item xs={12}>
                            <Controller
                                    name="plazoMinimoMes"
                                    control={formularioPlazosTipoActor.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        maxLength: {
                                            value: 4,
                                            message: 'Máximo 4 dígitos'
                                        }
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMaskInt
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{ maxLength: 4 }}
                                            label="Plazo Mínimo (Meses) *"
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        />
                                        )}
                                    />
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            <Grid item xs={12}>
                            <Controller
                                    name="plazoMinimoAnio"
                                    control={formularioPlazosTipoActor.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        maxLength: {
                                            value: 4,
                                            message: 'Máximo 4 dígitos'
                                        }
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMaskInt
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{ maxLength: 4 }}
                                            label="Plazo Mínimo (Años) *"
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        />
                                        )}
                                    />
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            <Grid item xs={12}>
                            <Controller
                                    name="plazoMaximoMes"
                                    control={formularioPlazosTipoActor.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        maxLength: {
                                            value: 4,
                                            message: 'Máximo 4 dígitos'
                                        }
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMaskInt
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{ maxLength: 4 }}
                                            label="Plazo Máximo (Meses) *"
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        />
                                        )}
                                    />
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            <Grid item xs={12}>
                            <Controller
                                    name="plazoMaximoAnio"
                                    control={formularioPlazosTipoActor.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                        maxLength: {
                                            value: 4,
                                            message: 'Máximo 4 dígitos'
                                        }
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            numberMaskInt
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{ maxLength: 4 }}
                                            label="Plazo Máximo (Años) *"
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
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
                                    onClick={formularioPlazosTipoActor.handleSubmit(handleOnClickEditar)}
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
  )
}

export default ModalPlazosEditarExcepTipoActor;
