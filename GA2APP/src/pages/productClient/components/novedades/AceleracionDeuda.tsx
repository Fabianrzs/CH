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

const aceleracionDeuda = () => {
  
  const classes = useStyles();
  const [tipoBloqueo, setTipoBloqueo] = useState(21);

  const handleAceleracionDeuda = () => {
      swal({
        text: "¿Está seguro que desea aplicar la novedad de Aceleración de la Deuda?",
        icon: "warning",
        buttons: ["Sí", "No"]
      }).then (respuesta =>{
        if(!respuesta){                    
           toast.success('Novedad aplicada con éxito');
        }
    })
  } 

  return (
    <CHPaper title="Aceleración de la deuda">
        <Grid container>
          <Grid className={classes.column} item xs={12} md={3}>
              <CHSelect
                label="Tipo Bloqueo"
                name="tipoBloqueo"
                value={tipoBloqueo}
                onChange={e => setTipoBloqueo(e.target.value as number)}
              >
                  <MenuItem value="21">Fallecido</MenuItem>
                  <MenuItem value="22">Fraude</MenuItem>
                  <MenuItem value="23">Refinanciado</MenuItem>
                  <MenuItem value="24">Reestructurado</MenuItem>
                  <MenuItem value="25">Oficial de cumplimiento</MenuItem>
                  <MenuItem value="26">Listas Restrictivas</MenuItem>
                  <MenuItem value="27">Incapacidad</MenuItem>
                  <MenuItem value="28">Juridico</MenuItem>
                  <MenuItem value="29">Embargo</MenuItem>
                  <MenuItem value="30">Desvinculado de C.H.</MenuItem>
              </CHSelect>
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
                <Button
                    variant="contained"
                    color="primary"
                    className={classes.button}
                    onClick={handleAceleracionDeuda}
                >
                    APLICAR NOVEDAD
                </Button>
            </Grid>
        </Grid>
    </CHPaper>
  )
}

export default aceleracionDeuda;