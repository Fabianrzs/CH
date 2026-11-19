import React, { useEffect, useState } from 'react';
import {
    Button,
    Card,
    CardContent,
    Container,
    createStyles,
    Dialog,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHSelect from '../../../components/ch-select/CHSelect';
import ApiCargueNomina from '../../../services/api-carguenomina.service';


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

export const ModalRechazarNomina = (props: any) => {
    const classes = useStyles();
    const apiCargueNomina = ApiCargueNomina();
    const [listaCausalesRechazo, setListaCausalesRechazo] = useState<any[]>([]);
    const {
        open,
        handleClose,
        rechazarArchivosCargados,
        itemRechazo,
        formularioRechazarNomina
    } = props;

    useEffect(() => {
        consultarListaCausalesRechazo();
    }, [])

    const consultarListaCausalesRechazo = async() => {
        await apiCargueNomina.getObtenerListaCausalesRechazo()
       .then((response: any) => {
            if(response.data){
                setListaCausalesRechazo(response.data);
            }
        })  .catch((e) => {
           toast(`Error al consultar lista causales rechazo ${e.message}`, { type: toast.TYPE.ERROR });
       });
    }

    const handleChangeRechazo = (event: any) => {
        
        rechazarArchivosCargados(itemRechazo)
        handleClose(false)
    };

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md" style={{width: '500px'}}>
                    <Card>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    RECHAZAR CARGUE NÓMINA
                                </Typography>
                            </Grid>
                            <Grid container spacing={4}>
                                <Grid item md={12} >
                                    <Controller
                                        name="causalRechazo"
                                        control={formularioRechazarNomina.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                label="Causal de Rechazo *"
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            >
                                                {listaCausalesRechazo.map((causal:any) => (
                                                        <MenuItem key="0" value={causal?.crN_ID}>
                                                            {causal?.crN_DESCRIPCION}
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
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={formularioRechazarNomina.handleSubmit(
                                            handleChangeRechazo,
                                        )}
                                    >
                                        Rechazar
                                    </Button>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={handleClose}
                                        style={{ marginLeft: '10px' }}
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

export default ModalRechazarNomina;
