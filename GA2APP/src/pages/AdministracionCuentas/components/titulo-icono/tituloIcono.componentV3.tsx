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
            whiteSpace: 'normal', 
            wordBreak: 'break-word', 
        },
        subtitulo: {
            fontWeight: 600,
            color: '#43b5f1',
            fontSize: '1.0rem',
            whiteSpace: 'normal',
            wordBreak: 'break-word',
        },
        fixedHeightBox: {
            minHeight: '80px', 
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
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
    icono: string;
}) {
    const classes = useStyles();
 
    return (
        <Grid
            container
            direction="row"
            justifyContent="center"
            alignItems="center"
            spacing={6}>
            <Grid item xs={6}>
                <Box borderRight={1}>
                    <Icon className={classes.tituloicono}
                    >{icono}</Icon>
                </Box>
            </Grid>
            <Grid item xs={6}>
                <Box className={classes.fixedHeightBox}>
                    <div className={classes.titulo}>{title1}</div>
                    <div className={classes.subtitulo}>{subtitle}</div>
                </Box>
            </Grid>
        </Grid>
    );
}
 
export default TituloIcono;