import React, { ChangeEvent, useEffect, useState } from 'react';
import {
  Grid,
} from '@material-ui/core';
import ContainerTitle from '../../../components/containe-title/container-title';
import { Auditoria } from '../../../types/Auditoria.model.type';
import UtilService from '../../../utils/util.service';
import { UserState } from '../../../reducers/user.reducer';


function TableDetalleAuditoria({
  detalle,
  usersState
}: {
  detalle: Auditoria[]; 
  usersState: UserState;
}) {


  const [items,setItems] = useState<Auditoria[]>();

  const [items2,setItems2] = useState<Auditoria[]>();

    useEffect(() => {
      setItems(detalle);
      
      setItems2([detalle[0]])
    }, [detalle]);

   const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {
    
    if(event.target.value.toUpperCase()===''){
      setItems(detalle);
    }else{  
      setItems(filterItems(event.target.value.toUpperCase()));
    }
   
  }

  const filterItems = (value:string) => {
    const filter = detalle.filter(
      (x) =>  
        x.nombreColumna?.toUpperCase().includes(value) ||
        x.valorAnterior?.toUpperCase().includes(value) ||
        x.valorNuevo?.toUpperCase().includes(value) 
                
    );
    return filter;
  };


  const getCssClass=(value:Auditoria) =>{
    if (value.tipoAccion==='UPDATE' && value.valorNuevo!==value.valorAnterior) return "updatecellda";
    
    return "";
  }

    return (
      <Grid>

    <ContainerTitle
      title="Detalle Auditoria"
    />

<Grid className="container-17">
      <table className="table">
          <thead>
              <th className="tab-space-01">Nombres tabla</th>
              <th className="tab-space-01">Tipo de accion</th>
              <th className="tab-space-01">Fecha</th>
              <th className="tab-space-01">Nombre usuario</th>
             
   
          </thead>
          <tbody>

          {items2?.map((row, index) => (
              <tr key={`table-${index}`}>
                  <td data-label="Tasa-EA" className="tab-space-01">{row.nombreTabla}</td>
                  <td data-label="Plazo-Mínimo" className="tab-space-01">{ row.tipoAccion }</td>
                  <td data-label="Plazo-Máximo" className="tab-space-01">{UtilService.getDate(row.fechaRegistro) }</td>
                  <td data-label="Estado" className="tab-space-01">  {`${usersState.users.find(x => x.id === row.usurioId)?.nombre} ${usersState.users.find(x => x.id === row.usurioId)?.apellido}`} </td>    
                           
              </tr>
          
               ))}
          </tbody>
      </table>  
  </Grid>

      <Grid className="container-24">
          <input type="search" placeholder="Buscar.." onChange={handleChangeBuscar}/>
      </Grid>

      
      <Grid className="container-17">
      <table className="table">
          <thead>

              <th className="tab-space-01">Nombre columna</th>
              <th className="tab-space-01">Valor</th>
              <th className="tab-space-01">Valor anterior</th>
              
          </thead>
          <tbody>

          {items?.map((row, index) => (
              <tr key={`table-${index} `} >
                  <td data-label="Tasa-EA" className={`tab-space-01 ${getCssClass(row)}`}>{row.nombreColumna}</td>
                  <td data-label="Plazo-Mínimo" className={`tab-space-01 ${getCssClass(row)}`}>{ row.valorNuevo }</td>
                  <td data-label="Plazo-Máximo" className={`tab-space-01 ${getCssClass(row)}`}>{ row.valorAnterior}</td>                            
              </tr>
          
               ))}
          </tbody>
      </table>  
  </Grid>
  </Grid>
   
    );
}


export default TableDetalleAuditoria;