import React, { useState } from 'react';
import { Button, createStyles, 
         Grid, 
         makeStyles, 
         MenuItem, 
        } from '@material-ui/core';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHPaper from '../../../../components/ch-paper/CHPaper';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        button: {
          width: '100%',
          marginLeft: '.5rem',
          marginTop: '1.5rem',
          backgroundColor: '#1C3F8B',
          color: '#fff',
      },
    }),
);

const Normalizaciones = () => {

  const classes = useStyles();
  const [efectoPago, setEfectoPago] = useState(101);
  const [efectoCargo, setEfectoCargo] = useState(201);

  const handleNormalizaciones = () => {
    swal({
        text: "¿Está seguro que desea aplicar la novedad de Normalizaciones?",
        icon: "warning",
        buttons: ["Sí", "No"]
    }).then (respuesta =>{
        if(!respuesta){                    
          toast.success('Novedad aplicada con éxito');
        }
    })
  }

  return (
    <CHPaper title="Normalizaciones">
        <Grid container>
          <Grid className={classes.column} item xs={12} md={3}>
              <CHSelect
                label="Efecto Pago o Abono"
                name="efectoPago"
                value={efectoPago}
                onChange={e => setEfectoPago(e.target.value as number)}
              >
                  <MenuItem value="101">Devolución de Interés Corriente</MenuItem>
                  <MenuItem value="102">Devolución Interés Mora</MenuItem>
                  <MenuItem value="103">Devolución Seguro</MenuItem>
                  <MenuItem value="104">Aplicación Pago</MenuItem>
                  <MenuItem value="105">Pago Cuota Chica</MenuItem>
                  <MenuItem value="106">Pago Cola Final</MenuItem>
                  <MenuItem value="107">Pago Rediferido</MenuItem>
                  <MenuItem value="108">Pago Reestructuración</MenuItem>
              </CHSelect>
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
              <CHSelect
                label="Efecto Cargo"
                name="efectoCargo"
                value={efectoCargo}
                onChange={e => setEfectoCargo(e.target.value as number)}
              >
                  <MenuItem value="201">Cargo Interés Corriente</MenuItem>
                  <MenuItem value="202">Cargo Interés Mora</MenuItem>
                  <MenuItem value="203">Cargo Seguro</MenuItem>
                  <MenuItem value="204">Anulación Pago</MenuItem>
                  <MenuItem value="205">Cargo Cuota Chica</MenuItem>
                  <MenuItem value="206">Anulación Cola Final</MenuItem>
                  <MenuItem value="207">Cargo Rediferido</MenuItem>
                  <MenuItem value="208">Cargo Reestructuración</MenuItem>
                  <MenuItem value="209">Devolución Saldo a favor</MenuItem>
              </CHSelect>
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
                <Button
                    variant="contained"
                    color="primary"
                    className={classes.button}
                    onClick={handleNormalizaciones}
                >
                    APLICAR NOVEDAD
                </Button>
            </Grid>                      
        </Grid> 
    </CHPaper>
  )
}

export default Normalizaciones