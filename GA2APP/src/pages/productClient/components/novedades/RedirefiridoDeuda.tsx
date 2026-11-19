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

const RedirefiridoDeuda = () => {
  
  const classes = useStyles();

  const [tipoRediferido, setTipoRediferido] = useState(1);

  const handleRediferidoDeuda = () => {
    swal({
        text: "¿Está seguro que desea aplicar la novedad de Rediferido de la deuda?",
        icon: "warning",
        buttons: ["Sí", "No"]
    }).then (respuesta =>{
        if(!respuesta){                    
          toast.success('Novedad aplicada con éxito');
        }
    })
  }

  return (
    <CHPaper title="Referido de la deuda">
        <Grid container>
          <Grid className={classes.column} item xs={12} md={3}>
              <CHSelect
                label="Tipo Rediferido"
                name="tipoBloqueo"
                value={tipoRediferido}
                onChange={e => setTipoRediferido(e.target.value as number)}
              >
                  <MenuItem value="1">De la deuda total del crédito</MenuItem>
                  <MenuItem value="2">De la cuota o cuotas facturadas</MenuItem>
              </CHSelect>
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
                <Button
                    variant="contained"
                    color="primary"
                    className={classes.button}
                    onClick={handleRediferidoDeuda}
                >
                    APLICAR NOVEDAD
                </Button>
            </Grid>                      
        </Grid> 
    </CHPaper>
  )
}

export default RedirefiridoDeuda;