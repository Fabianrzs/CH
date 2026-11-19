import React, { useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    TextField,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    Checkbox,
    Box,
    MenuItem,
    TableCell,
    Table,
    TableHead,
    TableRow,
    TableBody,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { useDispatch, useSelector } from 'react-redux';
import { TablaTitleParametroDto } from '../../../types/tablaParametro.model.type';
import {
    ModalTablaParametroForm,
} from '../constants/tablaParametroform.model.type';
import { CatalogState } from '../../../reducers/catalog.reducer';

import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { GetAllIdentificationTypes } from '../../../actions/catalog.action';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';

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
            marginBottom: '10px'
        },
        titleCampo: {
            fontSize: '10px',

        },
        table: {
            color: '#f1c3b90ff',
        },
    }),
);

function ModalUnidadesEjecutoras ({
    open,
    handleClose,
    catalogState,
}: {

    open: any;
    handleClose: any;
    catalogState: CatalogState | undefined;

}) {

    // const {
    //     handleClose,
    //     open,

    // } = props;
    
    const classes = useStyles();
    const dispatch = useDispatch();
    const catalogo = useSelector((state: { catalog: CatalogState }) => state.catalog);
    const { control, watch, handleSubmit, setValue, reset, getValues } = useForm({
        defaultValues: {
            tipoIdentificacion: '',
            numeroIdentificacion: '',
            nombre: '',
            sigla: '',
            fecha: new Date(),
            fuerza: '',
            codigo: 0,
            tasaAporte: 0,
            areaNegociacion: 0,
            aval: false,

        }
    })

    interface datosCategoria {
        id: number,
        dato: string,
    }

    const listaCategoria = [
        {
            id: 1,
            dato: 'AGENTE',
        }
    ]

    const datosLista = [
        {
            baseSalarial: '$234.098',
            año: '2024',
        }
    ]



    return (
        <Dialog
            open={open}
            onClose={handleClose}
            className={classes.table}
            aria-labelledby="form-dialog-title"

        >
            <CHPaper title="Agregar">
                <Grid container>
                    <Typography className={classes.titleform} variant="h5" component="h6">
                        Nuevo registro
                    </Typography>

                    <Grid container>
                    <Grid className={classes.column} item xs={12} md={10}>
                        <Controller
                            control={control}
                            name="tipoIdentificacion"
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de identificación*"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={(e: any) => {
                                        
                                        onChange(e);
                                    }}
                                >
                                    {catalogState?.IdentificationType?.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}

                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={10}>
                            <Controller
                                name="numeroIdentificacion"
                                control={control}
                                // rules={{
                                //     required: {
                                //         value: true,
                                //         message: 'Este campo es requerido',
                                //     },
                                // }}
                                render={({
                                    field: { onChange, value },

                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Número de identificación"
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

                        <Grid className={classes.column} item xs={12} md={10}>
                            <Controller
                                name="nombre"
                                control={control}
                                // rules={{
                                //     required: {
                                //         value: true,
                                //         message: 'Este campo es requerido',
                                //     },
                                // }}
                                render={({
                                    field: { onChange, value },

                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Nombre"
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
                        <Grid className={classes.column} item xs={12} md={10}>
                            <Controller
                                name="sigla"
                                control={control}
                                // rules={{
                                //     required: {
                                //         value: true,
                                //         message: 'Este campo es requerido',
                                //     },
                                // }}
                                render={({
                                    field: { onChange, value },

                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Sigla"
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
                        <Grid className={classes.column} item xs={12} md={10}>
                        <Controller
                            name="fecha"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha"
                                    onChange={onChange}
                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={10}>
                            <Controller
                                name="codigo"
                                control={control}
                                // rules={{
                                //     required: {
                                //         value: true,
                                //         message: 'Este campo es requerido',
                                //     },
                                // }}
                                render={({
                                    field: { onChange, value },

                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Código"
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        numberMask
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={10}>
                            <Controller
                                name="tasaAporte"
                                control={control}
                                // rules={{
                                //     required: {
                                //         value: true,
                                //         message: 'Este campo es requerido',
                                //     },
                                // }}
                                render={({
                                    field: { onChange, value },

                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Tasa Aporte"
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
                        <Grid className={classes.column} item xs={12} md={10}>
                            <Controller
                                name="areaNegociacion"
                                control={control}
                                // rules={{
                                //     required: {
                                //         value: true,
                                //         message: 'Este campo es requerido',
                                //     },
                                // }}
                                render={({
                                    field: { onChange, value },

                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        label="Área negociación"
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        numberMask
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className="item-containerModal" md={10}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="aval"

                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHCheckbox
                                            label="Aval"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>

                        <Grid item xs={12} style={{marginTop:'80px'}}>
                            <Box display="flex" justifyContent="center">
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"

                                >
                                    Aceptar
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
                            </Box>

                        </Grid>
                    </Grid>
                </Grid>
            </CHPaper>
        </Dialog>
    );
};

export default ModalUnidadesEjecutoras;