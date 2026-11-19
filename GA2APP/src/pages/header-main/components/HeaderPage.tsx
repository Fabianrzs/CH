import React from 'react';
import 'date-fns';
import { connect } from 'react-redux';
import { Grid, Typography } from '@material-ui/core';

const HeaderPage = () => {
    const fecha = new Date();
    const meses = [
        'Enero',
        'Febrero',
        'Marzo',
        'Abril',
        'Mayo',
        'Junio',
        'Julio',
        'Agosto',
        'Septiembre',
        'Octubre',
        'Noviembre',
        'Diciembre',
    ];
    const dias = [
        'Domingo',
        'Lunes',
        'Martes',
        'Miercoles',
        'Jueves',
        'Viernes',
        'Sabado',
    ];
    const dia_semana = dias[fecha.getDay()].toString();
    const dia_numero = fecha.getDate().toString();
    const mes = meses[fecha.getMonth()].toString();
    const ayo = fecha.getFullYear().toString();
    const horas = fecha.getHours().toString();
    const minutos = fecha.getMinutes().toString();
    const segundos = fecha.getSeconds().toString();

    return (
        <div>
            <Grid container>
                <Grid item xs={12}>
                    <Typography
                        variant="body1"
                        gutterBottom
                        className="alinear-derecha azul"
                    >
                        Su ultima visita fue: {dia_semana} {dia_numero} de {mes}{' '}
                        de {ayo} {horas}:{minutos}:{segundos}
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <Typography
                        variant="body1"
                        gutterBottom
                        className="alinear-derecha azul"
                    >
                        Fecha y hora actual: {dia_semana} {dia_numero} de {mes}{' '}
                        de {ayo} {horas}:{minutos}:{segundos}
                    </Typography>
                </Grid>
            </Grid>
        </div>
    );
};

export default connect()(HeaderPage);
