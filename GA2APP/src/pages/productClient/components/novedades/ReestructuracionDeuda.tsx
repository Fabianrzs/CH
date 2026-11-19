import React, {useState} from 'react';
import { Button, createStyles, Grid, 
         makeStyles, 
        } from '@material-ui/core';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import UtilService from '../../../../utils/util.service';
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

const ReestructuracionDeuda = () => {

const classes = useStyles(); 
const [fecha, setFecha] = useState(new Date);
const [valorPago, setValorPago] = useState(0);

const handleOnChange = ({target}: any) => {
    console.log('target', target.value)
    const numero = UtilService.cleanCurrencyValue(target.value);
    setValorPago(numero);
}

const handleReestructuracionDeuda = () => {
    swal({
      text: "¿Está seguro que desea aplicar la novedad de Reestructuración de la Deuda?",
      icon: "warning",
      buttons: ["Sí", "No"]
    }).then (respuesta =>{
      if(!respuesta){                    
         toast.success('Novedad aplicada con éxito');
      }
  })
} 

  return (
    <CHPaper title="Reestructuración de la deuda">
      <Grid container>
      <Grid className={classes.column} item xs={12} md={3}>
             <CHTextField                                  
                label="No. Reestructuración"
                name="NumeroReestructuracion"
                value = "1"
                InputLabelProps={{ shrink: true }}
                isReadOnly
            />       
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
             <CHTextField                                  
                label="Valor Pago"
                name="valorPago"
                onChange={handleOnChange}
                value = {valorPago}
                currencyMask
            />       
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
                <CHDatePicker  
                    name="fechaDesembolso"                         
                    label="Fecha Desembolso"
                    onChange={ (newValue) => {
                            setFecha(newValue as Date)
                         } 
                     }
                    value={fecha}
                />
        </Grid>
        <Grid className={classes.column} item xs={12} md={3}>
            <Button
                variant="contained"
                color="primary"
                className={classes.button}
                onClick={handleReestructuracionDeuda}
            >
                APLICAR NOVEDAD
            </Button>
        </Grid>
    </Grid>
</ CHPaper>
  )
}

export default ReestructuracionDeuda;