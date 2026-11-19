import React from 'react';
import { createStyles,
         Grid, 
         makeStyles, 
         Typography } from '@material-ui/core';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import ParametrizacionLiquidacion from './components/ParametrizacionLiquidacion';

const useStyles = makeStyles(() =>
    createStyles({        
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        }, 
        column: {
            padding: '10px',
            textAlign: 'center',
        },      
        
    }),
);

const Liquidacion = () => {

  const classes = useStyles();

  return (
        <CHPrivateContainer>   
        <CHPaper title="Parametrización de Liquidación">          
        <Grid container>
            <Typography
                className={classes.titleform}
                variant="h5"
                component="h6"
            >
                Parámetros Generales
            </Typography>
        </Grid>
        <Grid container>
            <ParametrizacionLiquidacion />
        </Grid>
        </CHPaper>
    </CHPrivateContainer>
  );
}

export default Liquidacion;
