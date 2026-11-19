import React from 'react';
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
import { TablaTitleParametroDto } from '../../../types/tablaParametro.model.type';
import {
    ModalTablaParametroForm,
} from '../constants/tablaParametroform.model.type';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';

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

export const ModalFuerza = (props: any) => {
    const {
        handleClose,
        open,

    } = props;
    const classes = useStyles();

    const { control, watch, handleSubmit, setValue, reset, getValues } = useForm({
        defaultValues: {
            descripcion: '',
            sigla: '',
            codigo: 0,
            baseSalarial: 0,
            anio: '',
            categoria: '',
            especial: false,
            civil: false,
            activo: false,
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
                                name="descripcion"
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
                                        label="Descripción"
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
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={8}>
                            <Controller
                                name="baseSalarial"
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
                                        label="Última Base Salarial"
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
                        <Grid className={classes.column} item xs={12} md={2}>
                            <Controller
                                name="anio"
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
                                        label="Año"
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
                                name="categoria"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Categoría"
                                        value={value}
                                        onChange={(e) => {
                                            onChange(e.target.value);
                                        }}
                                    >
                                        {listaCategoria.map((option: datosCategoria) => (
                                            <MenuItem key={option.id} value={option.id}>
                                                {option.dato}
                                            </MenuItem>
                                        ))}
                                    </CHSelect>
                                )}
                            />
                        </Grid>

                        <Grid className="item-containerModal" md={10}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="especial"

                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHCheckbox
                                            label="Especial"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid className="item-containerModal" md={10}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="civil"

                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHCheckbox
                                            label="Civil"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid className="item-containerModal" md={10}>
                            <Grid className='subcontainerModal'>
                                <Controller
                                    name="activo"

                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHCheckbox
                                            label="Activo"
                                            onChange={onChange}
                                            checked={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Typography className={classes.titleform} variant="h5" component="h6">
                                Histórico Base Salarial
                            </Typography>
                        </Grid>

                        {/* <Grid container md={10}> */}
                            <Table>
                                <TableHead className="credit-table">
                                    <TableRow>
                                        <TableCell>BASE SALARIAL</TableCell>
                                        <TableCell>AÑO</TableCell>
                                        <TableCell />
                                    </TableRow>
                                </TableHead>

                                <TableBody>
                                    {datosLista.map((datosList, index) => (
                                        <TableRow key={`datosList-${index}`}>
                                            <TableCell>{datosList.baseSalarial}</TableCell>
                                            <TableCell>{datosList.año}</TableCell>

                                           
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        {/* </Grid> */}

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