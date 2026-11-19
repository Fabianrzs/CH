
import React, { useEffect, useState } from 'react';
import { Controller } from 'react-hook-form';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import CHTextField from '../../ch-text-field/CHTextField';

import CatalogApi from '../../../services/api-catalog.service';
import CHSelect from '../../ch-select/CHSelect';
import { VALORCATALOGO } from '../../../types/catalog.type';

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

export const ModalEditarMatricula = (props: any) => {

    const {open, handleClose, handleOnClick, formulario} = props;
  
    const classes = useStyles();

    const apiDos = CatalogApi();
    const [tipoMatriculas, setTipoMatriculas] = useState([]);

    useEffect(() => {
        obtenerCatalogoTipoMatricula();
    }, []);

    const obtenerCatalogoTipoMatricula = async () => {
        await apiDos
            .getObtenerValoresCatalogosPorIdCatalogo(90)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        setTipoMatriculas(response.data);
                        
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };
    return (
        <div>
              <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogContent style={{ width: '600px' }}>
                    <Grid item>
                        <br />
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                           ACTUALIZAR INFORMACIÓN MATRÍCULA
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={10}>
                            <Controller
                                name="numeroMatricula"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="No. matricula"
                                        value={value}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        onChange={e => {
                                            if (
                                                e.target.value.length <= 15
                                            ) {
                                                onChange(e);
                                            }
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item md={10}>
                            <Controller
                                name="cedulaCatastral"
                                rules={{
                                  
                                    maxLength: {
                                        value: 40,
                                        message:
                                            'La longitud máxima del campo es de 40 caracteres',
                                    },
                                }}
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Cédula catastral"
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                     />
                                )}
                            />
                        </Grid>
                        <Grid item md={10}>
                            <Controller
                                name="areaPrivadaConstruida"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Área privada construida *"
                                        value={value}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        onChange={ e => {
                                            if (
                                                e.target.value.length <= 5
                                            ) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[A-Za-z_\W]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        }}
                                     />
                                )}
                            />
                        </Grid>
                        <Grid item md={10}>
                            <Controller
                                name="areaAvaluoComercial"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        currencyMask
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Valor avalúo comercial *"
                                        isReadOnly
                                        value={value}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        onChange={onChange}
                                     />
                                )}
                            />
                        </Grid>
                        <Grid item md={10}>
                            <Controller
                                name="areaAvaluoCatastral"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        currencyMask
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Valor avalúo catastral *"
                                        value={value}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        onChange={onChange}
                                     />
                                )}
                            />
                        </Grid>
                        <Grid item md={10} className={classes.column}>
                                <Controller
                                    name="tipo_Matricula"
                                    control={formulario.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo de Matricula"
                                            value={value}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                        {tipoMatriculas.map(
                                        (item: VALORCATALOGO) => (
                                            <MenuItem
                                            key={item.idValor}
                                            value={item.idValor}
                                        >
                                            {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                        </CHSelect>
                                    )}
                                />
                        </Grid>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={formulario.handleSubmit(handleOnClick)}
                            >
                                ACTUALIZAR
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
