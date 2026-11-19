
import React, { useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    DialogTitle,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import CHSelect from '../../ch-select/CHSelect';

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
        columnDate: {
            minWidth: '110px',

        },
    }),
);


export const ModalCreditCommitee = (props: any) => {

    const {open, handleClose, handleOnClick, formulario, causalesRechazo, estadoSolicitud} = props;
    const [ocultarCampo,setOcultarCampo] = useState(true);
  
    const classes = useStyles();
    return (
        <div>
              <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogTitle>RESUMEN DE PAGO</DialogTitle>
                <DialogContent style={{ width: '600px' }}>
                    <Grid container>
                        <Grid item md={10}>
                            <Controller
                                name="resultado"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        label="Resultado *"
                                        value={value}
                                        onChange={e => {
                                         
                                            if(e.target.value === 6){
                                                setOcultarCampo(true);
                                            }
                                            else{
                                                setOcultarCampo(false);
                                            }
                                            onChange(e);
                                        }}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    >
                                        {estadoSolicitud?.map(
                                            (option: any) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    {ocultarCampo && (  <Grid container>
                        <Grid item md={10}>
                            <Controller
                                name="causal"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        label="Causal *"
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                    >
                                        {causalesRechazo?.map(
                                            (option: any) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>)}
                  
                    <br />
                    <Grid container>
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={formulario.handleSubmit(handleOnClick)}
                            >
                                CONFIRMAR
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
                    <br />
                </DialogContent>
            </Dialog>
        </div>
    );
};
