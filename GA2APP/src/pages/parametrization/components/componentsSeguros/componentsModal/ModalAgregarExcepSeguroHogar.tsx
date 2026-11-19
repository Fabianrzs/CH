import React from 'react';
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
    TextField,
    Typography,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHSelect from '../../../../../components/ch-select/CHSelect';
import { CITYDTO, DEPARTAMENTODTO } from '../../../../../types/catalog.type';

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

export const ModalAgregarExcepSeguroHogar = (props: any) => {
    const classes = useStyles();

    const {
        open,
        handleClose,
        allDepartaments,
        handleOnClickAgregar,
        allCitiesByDepartament,
        startGetCitiesByDepartment,
        handleOnChange,
        handleOnChangeCity,
        seguroForm,
        formularioSeguroUbicacion,
    } = props;

    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md">
                    <Card>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    AGREGAR EXCEPCIÓN SEGURO HOGAR
                                </Typography>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <br />
                                    <Typography>Departamento</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <Controller
                                        name="nombre"
                                        control={
                                            formularioSeguroUbicacion.control
                                        }
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                label=""
                                                onChange={e => {
                                                    onChange(e);
                                                    startGetCitiesByDepartment(
                                                        e.target.value,
                                                    );
                                                }}
                                                value={value}
                                            >
                                                {allDepartaments?.map(
                                                    (
                                                        option: DEPARTAMENTODTO,
                                                    ) => (
                                                        <MenuItem
                                                            key={option.id}
                                                            value={option.id}
                                                        >
                                                            {option.nombre}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <br />
                                    <Typography>Municipio</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <Controller
                                        name="dpcId"
                                        control={
                                            formularioSeguroUbicacion.control
                                        }
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                label=""
                                                onChange={onChange}
                                                value={value}
                                            >
                                                {allCitiesByDepartament?.map(
                                                    (option: CITYDTO) => (
                                                        <MenuItem
                                                            key={option.id}
                                                            value={
                                                                option.codigo
                                                            }
                                                        >
                                                            {option.nombre}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <Typography>Seguro todo riesgo </Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <Controller
                                        name="seguroTodoRiesgo"
                                        control={
                                            formularioSeguroUbicacion.control
                                        }
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <TextField
                                                onChange={onChange}
                                                InputLabelProps={{
                                                    shrink: true,
                                                }}
                                                value={value}
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
                                        onClick={formularioSeguroUbicacion.handleSubmit(
                                            handleOnClickAgregar,
                                        )}
                                    >
                                        Agregar
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

export default ModalAgregarExcepSeguroHogar;
