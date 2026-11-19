import { createStyles, Grid, makeStyles, MenuItem, TextField } from "@material-ui/core";
import React from "react";
import { Controller, useForm } from "react-hook-form";
import CHAutoComplete from "../../../components/ch-auto-complete/CHAutoComplete";
import CHDatePicker from "../../../components/ch-datepicker/CHDatepicker";
import CHPaper from "../../../components/ch-paper/CHPaper";
import CHSelect from "../../../components/ch-select/CHSelect";
import CHTextField from "../../../components/ch-text-field/CHTextField";

const useStyles = makeStyles(() =>
    createStyles({
        columnButtonAdd: {
            textAlign: 'right',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        columnLabel: {
            padding: '10px',
        },
        bloqueoMatricula: {
            color: 'Red'
        }
    }),
);

const AsignarTareas = (props: any) => {
    const {
        prueba
    } = props

    const formularioAsignar = useForm({
        defaultValues: {
            usuarioOrigen: '',
            usuarioDestino: '',
            asunto: '',
            descripcion: '',
            fechaInicio: new Date(),
            fechaFin: new Date(),
            parametrizacionConsulta: '',
            estado: '',
        }
    });


    const listaActividad = [
        { id: '1', actividad: 'SOLICITUD' },
        { id: '2', actividad: 'CREDITO' },
        { id: '3', actividad: 'DESEMBARGO' },
    ]
    const classes = useStyles();
    return (
        <>
            <CHPaper title='Asignación de tareas'>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="usuarioOrigen"

                            control={formularioAsignar.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHAutoComplete

                                    id="usuario"
                                    options={[
                                        // Lista de usuarios para el autocompletado
                                        { label: 'Usuario 1', value: 'usuario1' },
                                        { label: 'Usuario 2', value: 'usuario2' },
                                        { label: 'Usuario 3', value: 'usuario3' },
                                    ]}
                                    getOptionLabel={(option) => option.label}
                                    renderInput={(params) =>
                                        <CHTextField
                                            {...params} label="Usuario Origen"
                                        />
                                    }
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="usuarioDestino"

                            control={formularioAsignar.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHAutoComplete

                                    id="usuarioDestino"
                                    options={[
                                        // Lista de usuarios para el autocompletado
                                        { label: 'Usuario 1', value: 'usuario1' },
                                        { label: 'Usuario 2', value: 'usuario2' },
                                        { label: 'Usuario 3', value: 'usuario3' },
                                    ]}
                                    getOptionLabel={(option) => option.label}
                                    renderInput={(params) =>
                                        <CHTextField
                                            {...params} label="Usuario Destino"
                                        />
                                    }
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="asunto"
                            control={formularioAsignar.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Asunto"
                                        value={value}
                                        onChange={onChange}
                                    />

                                </>
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="descripcion"
                            control={formularioAsignar.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Descripción"
                                        value={value}
                                        onChange={onChange}
                                    />

                                </>
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="fechaInicio"
                            control={formularioAsignar.control}
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
                                    label="Fecha Inicio"
                                    onChange={onChange}
                                    value={value}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="fechaFin"
                            control={formularioAsignar.control}
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
                                    label="Fecha Fin"
                                    onChange={onChange}
                                    value={value}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="parametrizacionConsulta"
                            control={formularioAsignar.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo Fuente"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    <MenuItem value="">Seleccionar</MenuItem>
                                    {listaActividad.map((resource, index) => (
                                        <MenuItem
                                            key={`resource-${index}`}
                                            value={resource.id}
                                        >
                                            {resource.actividad}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="estado"
                            control={formularioAsignar.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                error={!!error}
                                helperText={
                                    error ? error.message : null
                                }
                                isReadOnly={false}
                                label="Descripción"
                                value={value}
                                onChange={onChange}
                            />
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    )
}

export default AsignarTareas