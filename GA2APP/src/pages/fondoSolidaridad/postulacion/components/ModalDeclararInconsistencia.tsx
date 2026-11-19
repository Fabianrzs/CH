import { Button, 
         createStyles, 
         Dialog, 
         DialogContent, 
         Grid, 
         InputLabel, 
         makeStyles, 
         Typography} from '@material-ui/core';
import React from 'react';
import { Controller } from 'react-hook-form';
import CHTextField from '../../../../components/ch-text-field/CHTextField';

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
		column: {
			padding: '4px',
			textAlign: 'center',
		},
	}),
);

export const ModalDeclararInconsisntencia = (props: any) => {
    const classes = useStyles();
    const {open,
           handleClose,
           formularioInconsistencia,
           generarInconsistencia} = props;

    return(
        <>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                 <DialogContent style={{ width: '900px' }}>
                    <Grid item>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            GENERAR CARTA DE INCONSISTENCIA
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={12}>
                           <InputLabel style={{paddingLeft:'3px'}}><b>ASUNTO:</b> CARTA DE INCONSITENCIA </InputLabel>
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="radicadoRespuesta"
                                control={formularioInconsistencia.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        label="Radicado Respuesta"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                        isReadOnly
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={12}>
                            <Controller
                                name="descripcion"
                                control={formularioInconsistencia.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        label="Descripción de la Inconsistencia"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        variant="outlined"
                                        multiline
                                        rows={10}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container justifyContent="flex-end">
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={generarInconsistencia}
                            >
                                GENERAR
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                onClick={handleClose}
                                style={{ marginLeft: '10px' }}
                            >
                                CANCELAR
                            </Button>
                        </Grid>
                    </Grid>
                </DialogContent>
            </Dialog>
        </>
    )
}