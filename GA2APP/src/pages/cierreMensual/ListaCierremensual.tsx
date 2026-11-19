import React, { useEffect } from 'react';
import { Button, createStyles, Grid, makeStyles } from "@material-ui/core";
import { connect } from "react-redux";
import { AppState } from "../../store/rootReducer";
import { startgetListaProgramacionCierre } from '../../actions/cierreMensual.action';
import { CierreMensualState } from '../../reducers/cierreMensual.reducer';
import ContainerTitle from '../../components/containe-title/container-title';
import CHPaperNew from '../../components/ch-paper-new/CHPaperNew';
import { LoadingComponent } from '../../components/loading/loading.component';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import { ListaCierreMensual } from '../../types/cierre-mensual.model.type';


const useStyles = makeStyles(() =>
  createStyles({
    column: {
      padding: '10px',
      textAlign: 'center',
    },
    buttonprincipal: {
      width: '250px',
      height: '50px',
      marginLeft: '1rem',
      background: '#FEC917'
    },
    buttonsecundario: {
      width: '250px',
      height: '50px',
      marginLeft: '1rem',
      background: '#43b5f1'
    },
    processButton: {
      backgroundColor: '#1C3F8B',
      color: '#fff',
      margin: '8px 0',
    },
    root: {
      flexGrow: 1,
    }
  }),
);

const ListarCierreMensual = (props: Props) => {

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

    const {startgetListaProgramacionCierre,cierreMensualState} = props;
    const classes = useStyles();

    console.log(cierreMensualState);
 
    useEffect(() => { 
        startgetListaProgramacionCierre();
    }, []); 

    const descargaReporte= (data:ListaCierreMensual) => () => {
        
        console.log(data);
    };

    return( <div className={classes.root}>

<LoadingComponent key="loader" loading={false} />

<PageIndicators
topTextNivel1='Cierre mensual'
topTextNivel2='Reporte' />

<IconTitle
title='Cierre Mensual'
subTitle='Reporte'
icon='fch-Buscar-0-1'
/>
           

 <CHPaperNew>

<ContainerTitle
  title="Cierre mensual reporte"
/>
      
  <Grid className="container-24">
      <input type="search" placeholder="Buscar.." />
  </Grid>
  

  <Grid className="container-17">
  <table className="table">
      <thead>
        
          <th className="tab-space-01">Periodo</th>
          <th className="tab-space-01">Conceptos ahorro</th>
          <th className="tab-space-01">Conceptos cesantias</th>
          <th className="tab-space-01">IPC</th>
          <th className="tab-space-01">Tipo de proceso</th>
          <th className="tab-space-01">Estado</th>
          <th className="tab-space-01">Acciones</th>
      </thead>
      <tbody>
      {cierreMensualState.lista?.map((row, index) => (
          <tr key={`table-${index}`}>
             
            <td data-label="Tasa-EA" className="tab-space-01">{`${ meses[row.mes].toString()} del ${ row.anio }`} </td>
            <td data-label="Estado" className="tab-space-01">  { row.conceptosAhorro } </td>  
            <td data-label="Estado" className="tab-space-01">  { row.conceptosCesantias} </td> 
            <td data-label="Estado" className="tab-space-01">  { row.ipc } </td> 
            <td data-label="Estado" className="tab-space-01">  { row.tipoProcesoDesc } </td> 
            <td data-label="Estado" className="tab-space-01">  { row.estadoDesc} </td> 

            <td data-label="Estado" className="tab-space-01">  
            
            {row.estado===3 &&
                <Button variant="contained" color="primary"  onClick={descargaReporte(row)}>
                   Descargar reporte
                </Button>
            }
            
            </td> 
                                   
          </tr>
      
           ))}
      </tbody>
  </table>
</Grid>
</CHPaperNew>
      </div>
     )

}


  

interface LinkMapStateToProps {
    cierreMensualState: CierreMensualState;
  }
  
  const mapStateToProps = (state: AppState) => ({
    cierreMensualState: state.cierreMensual,
  });
  
  
  interface LinkMapDispatchToProps {
    startgetListaProgramacionCierre: () => void;
  
  }
  
  const mapDispatchToProps = {
    startgetListaProgramacionCierre,
 
  };
  
  
  
  type Props =  LinkMapDispatchToProps & LinkMapStateToProps;
  
  export default connect(mapStateToProps, mapDispatchToProps)(ListarCierreMensual);