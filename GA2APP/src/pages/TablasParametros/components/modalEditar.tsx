import React from 'react';
import {
    Button,
    Dialog,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    Checkbox,
    DialogContent,
} from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { TablaTitleParametroDto } from '../../../types/tablaParametro.model.type';
import {
    ModalTablaParametroForm, TABLA_PARAMETRO,
} from '../constants/tablaParametroform.model.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center'
        },
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        titleCampo: {
            fontSize: '10px',

        },
    }),
);

export const ModalEditar = (props: any) => {
    const {
        data,
        handleClose,
        open,
        handleOnClickEditar,
        handleOnChange,
        dataEdit,
        valueTablaParametroSeleccionado
    } = props;
    const classes = useStyles();
    const formularioModal = useForm({
        defaultValues: ModalTablaParametroForm,
    });

    const formatLabel = (nombreColumna: string) => {
        const sinPrefijo = nombreColumna.startsWith('ENT_') ? nombreColumna.replace(/^ENT_/, '') : nombreColumna;
        
        switch (sinPrefijo) {
            case 'NOMBRE_RAZON_SOCIAL':
                return 'NOMBRE Y/O RAZON SOCIAL';
            case 'OTRO_NOMBRE_ESPECIAL':
                return 'TEXTO FORMATEADO ESPECIAL';
            default:
                return sinPrefijo.replace(/_/g, ' ').toUpperCase();
        }
    };

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="lg"
            >
                 <DialogContent style={{ width:'600px', height:'450px'}}>
                    <Typography className={classes.titleform} variant="h5" component="h6">
                        EDITAR REGISTRO
                    </Typography>
                    <Grid container>
                        {data
                            .filter((campo: TablaTitleParametroDto) => !campo.nombreColumna.includes('_ID')).map((campo: TablaTitleParametroDto) => (
                                <><br /><Grid key={campo.nombreColumna} container spacing={3}>
                                    <Grid item xs={7}>
                                        <CHTextField
                                            name={campo.nombreColumna}
                                            label={formatLabel(campo.nombreColumna)}
                                            onChange={handleOnChange}
                                            inputProps={{
                                                style: {
                                                    width: '400px'
                                                }
                                            }}
                                            value={dataEdit?.data}
                                        />
                                    </Grid>
                                </Grid></>
                            ))}
                        <br />
                        <br />
                        <br />
                        <br />
                        {valueTablaParametroSeleccionado === TABLA_PARAMETRO.FUERZAS && (
                             <Grid key='activo' container spacing={3}>
                                <Grid item xs={3}>
                                    ¿ACTIVO?
                                </Grid>
                                <Grid item xs={7}>
                                    <Checkbox
                                        name='Activo'
                                        onChange={handleOnChange}
                                        inputProps={{
                                            style: {
                                                width: '400px'
                                            }
                                        }}
                                    />
                                </Grid>
                            </Grid>
                        )}
                         {valueTablaParametroSeleccionado !== TABLA_PARAMETRO.FUERZAS && (
                        <Grid key='activo' container spacing={3}>
                            <Grid item xs={3}>
                                ¿ACTIVO?
                            </Grid>
                            <Grid item xs={7}>
                                <Checkbox
                                    name='Activo'
                                    onChange={handleOnChange}
                                    inputProps={{
                                        style: {
                                            width: '400px'
                                        }
                                    }}
                                    checked
                                />
                            </Grid>
                        </Grid>
                         )}
                        <br />
                        <br />
                        <br />
                        <Grid container>
                            <Grid item>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    onClick={handleOnClickEditar}
                                >
                                    Actualizar
                                </Button>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="default"
                                    onClick={handleClose}
                                    style={{ marginLeft: "10px" }}
                                >
                                    Cancelar
                                </Button>
                           </Grid>
                         </Grid>
                      </Grid>
                </DialogContent>
            </Dialog>
        </div>
    );
};