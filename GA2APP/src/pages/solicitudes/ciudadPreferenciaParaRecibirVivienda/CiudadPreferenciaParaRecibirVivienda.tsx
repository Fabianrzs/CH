import React from 'react';
import { Grid, makeStyles, createStyles, MenuItem } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

function CiudadPreferenciaParaRecibirVivienda() {
    const classes = useStyles();

    const ciudad = useForm({
        defaultValues: {
            departamento1: 0,
            departamento2: 0,
            departamento3: 0,
            ciudad1: 0,
            ciudad2: 0,
            ciudad3: 0,
        },
    });

    return (
        <>
            <CHPaper title="Ciudad de preferencia para recibir su vivienda">
                <p className="paragraphText">
                    Manifiesto que en el evento de ser adjuticatorio adentro de
                    la presente convocatoria, mediante el mecanismo de solución
                    de vivienda (adjudicación),{' '}
                    <b>
                        <u>
                            estoy dispuesto a aceptar una de las viviendas de
                            CAJA HONOR tenga disponibles en los diferentes
                            proyectos dirigidos a mi categoría
                        </u>
                    </b>
                    . Si usted llegará a ser beneficiario del fondo de
                    solidaridad ¿En que ciudad le gustaría solucionar su
                    vivienda?. En la casilla 1diligencie la de mayor
                    preferencia.
                </p>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento1"
                            control={ciudad.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Departamento"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        Departamentos
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudad1"
                            control={ciudad.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Ciudad 1 (de mayor preferencia)"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        Ciudad 1
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento2"
                            control={ciudad.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Departamento"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        Departamentos
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudad2"
                            control={ciudad.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Ciudad 2"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        Ciudad 1
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento3"
                            control={ciudad.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Departamento"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        Departamentos
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudad3"
                            control={ciudad.control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Ciudad 3"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        Ciudad 1
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    );
}

export default CiudadPreferenciaParaRecibirVivienda;
