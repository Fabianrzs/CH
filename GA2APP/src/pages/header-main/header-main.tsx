import { Grid, Typography } from '@material-ui/core';
import React from 'react';
import HeaderPage from './components/HeaderPage';
import './header-main.style.scss';

export const HeaderMain = () => (
    <Grid container>
        <Grid item xs={6}>
            <Grid item xs={12}>
                <Typography
                    variant="body1"
                    gutterBottom
                    className="alinear-izquierda azul"
                >
                    Bienvenido(a)
                </Typography>
            </Grid>
            <Grid item xs={12}>
                <Typography
                    variant="h6"
                    component="h6"
                    className="alinear-izquierda azul"
                >
                    <strong>Prueba Prueba Prueba</strong>
                </Typography>
            </Grid>
        </Grid>
        <Grid item xs={6}>
            <HeaderPage />
        </Grid>
    </Grid>
);
