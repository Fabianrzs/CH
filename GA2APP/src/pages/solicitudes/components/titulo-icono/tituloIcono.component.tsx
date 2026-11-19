import React from 'react';
import { Box, createStyles, Grid, Icon, makeStyles } from '@material-ui/core';

const useStyles = makeStyles(() =>
    createStyles({
        tituloicono: {
            marginRight: '15px',
            fontSize: '60px',
            color: '#1C3F8B',
        },
        titulo: {
            fontWeight: 600,
            color: '#1C3F8B',
            fontSize: '1.0rem',
        },
        subtitulo: {
            fontWeight: 600,
            color: '#43b5f1',
            fontSize: '1.0rem',
        },
    }),
);

function TituloIcono({
    title1,
    subtitle,
    icono
}: {
    title1: string;
    subtitle: string;
    icono: string
}) {
    const classes = useStyles();

    return (
        <>
            <Grid
                container
                direction="row"
                justifyContent="center"
                alignItems="center"
                alignContent="center"
                spacing={2}>
                <Grid item xs={6} >
                    <Box borderRight={1} >
                        <Icon className={classes.tituloicono}>{icono}</Icon>
                    </Box>
                </Grid>
                <Grid item xs={6}>
                    <Grid
                        container
                        direction="column"
                        justifyContent="center"
                        alignItems="flex-start"
                        alignContent="center">
                        <Grid item xs={12} className={classes.titulo}>
                            {title1}
                        </Grid>
                        <Grid item xs={12} className={classes.subtitulo}>
                            {subtitle}
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </>
    );
}

export default TituloIcono;
