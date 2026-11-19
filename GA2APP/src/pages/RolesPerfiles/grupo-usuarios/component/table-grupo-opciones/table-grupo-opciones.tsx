import React, { ChangeEvent, useEffect, useState } from 'react';
import {
  Grid,
} from '@material-ui/core';
import ContainerTitle from '../../../../../components/containe-title/container-title';
import { GrupoUsersModel } from '../../../../../types/grupoUsers.model';
import { GrupoOpcionesModel } from '../../../../../types/grupo-opciones.model.type';



function TableGroupOpciones({
  grupo,
  opcions, 
  setGroupOpciones
}: {
   grupo: GrupoUsersModel[];
   opcions:GrupoOpcionesModel[]; 
   setGroupOpciones:(grupo:GrupoUsersModel) => void;

}) {

  const [checked] = React.useState<GrupoOpcionesModel[]>(opcions);
  const [items,setItems] = useState<GrupoUsersModel[]>();


    const handleChange=(value:GrupoUsersModel)=>() =>{

       setGroupOpciones(value);
   
    }

    useEffect(() => {
      setItems(grupo);   
    }, [grupo]);

    const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {
    
      if(event.target.value.toUpperCase()===''){
        setItems(grupo);
      }else{  
        setItems(filterItems(event.target.value.toUpperCase()));
      }
     
    }


    const filterItems = (value:string) => {
      const filter = grupo.filter(
        (x) =>  
          x.nombre?.toUpperCase().includes(value) ||
          x.descripcion?.toUpperCase().includes(value)
                
      );
      return filter;
    };

  
    return (
      <Grid>

    <ContainerTitle
      title="Grupo de Opciones"
    />

      <Grid className="container-24">
          <input type="search" placeholder="Buscar.." onChange={handleChangeBuscar}/>
      </Grid>

      
      <Grid className="container-17">
      <table className="table">
          <thead className="tab-space-01">
              <th> Items</th>
              <th className="tab-space-01">Código</th>
              <th className="tab-space-01">Nombre</th>
              <th className="tab-space-01">Descripción</th>
              <th className="tab-space-01">Estado</th>          
          </thead>
          <tbody>

          {items?.map((row, index) => (
              <tr key={`table-${index}`}>
                 <td className="tab-space-01">   
                   <label className="container-14 cian" htmlFor={`checkbox-${index}`}>
                            <input 
                            type="checkbox" 
                             id={`checkbox-${index}`} 
                              defaultChecked={ checked.findIndex(x => x.id === row.id)!== -1 } 
                             onChange={handleChange(row)} />
                            <span className="checkmark"/>
                    </label>      
                 </td>
                  <td data-label="Descripción" className="tab-space-01"> { row.codigo} </td>
                  <td data-label="Tasa-EA" className="tab-space-01">{row.nombre}</td>
                  <td data-label="Tasa-EA" className="tab-space-01">{row.descripcion}</td>
                  <td data-label="Plazo-Mínimo" className="tab-space-01">{ row.estado }</td>                       
              </tr>
          
               ))}
          </tbody>
      </table>  
  </Grid>
  </Grid>
   
    );
}

export default TableGroupOpciones;