import React, { ChangeEvent, useState } from 'react';
import './table-grupos-usuarios.scss';
import {
  Grid,
} from '@material-ui/core';
import ContainerTitle from '../../../../../components/containe-title/container-title';
import { GrupoUsersModel } from '../../../../../types/grupoUsers.model';

function TableGupoUsuarios({
  grupoUsuarios,
  setGroup,
  usuarioGrupo,
}: {
    grupoUsuarios: GrupoUsersModel[]; 
    setGroup: (value:GrupoUsersModel) => void;
    usuarioGrupo:GrupoUsersModel[];
}) {

  const [items,setItems] = useState<GrupoUsersModel[]>(grupoUsuarios ?? []);
  const [checked] = React.useState<GrupoUsersModel[]>(usuarioGrupo);


   const handleChange=(value:GrupoUsersModel)=>() =>{
    setGroup(value);
   }

   const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {
    
    if(event.target.value.toUpperCase()===''){
      setItems(grupoUsuarios);
    }else{  
      setItems(filterItems(event.target.value.toUpperCase()));
    }
   
  }


  const filterItems = (value:string) => {
    const filter = grupoUsuarios.filter(
      (x) =>  
        x.nombre?.toUpperCase().includes(value) ||
        x.descripcion?.toUpperCase().includes(value)        
    );
    return filter;
  };


 
    return (
      <Grid>

    <ContainerTitle
      title=" Grupo de usuarios"
    />
          
      <Grid className="container-24">
          <input type="search" placeholder="Buscar.." onChange={handleChangeBuscar}/>
      </Grid>
      

      <Grid className="container-17">
      <table className="table">
          <thead>
              <th className="tab-space-01">Items</th>
              <th className="tab-space-01">Código</th>
              <th className="tab-space-01">Nombre</th>
              <th className="tab-space-01">Descripción</th>
              <th className="tab-space-01">Estado</th>
          </thead>
          <tbody>

          {items?.map((row, index) => (


              <tr key={`table-${index}`}>
                  <td data-label="Descripción" className="tab-space-01"> 

                  <label className="container-14 cian" htmlFor={`checkbox-${index}`}>
                            <input 
                            type="checkbox" 
                            id={`checkbox-${index}`} 
                             defaultChecked={ checked.findIndex(x => x.id === row.id)!== -1 } 
                             onChange={handleChange(row)} />
                            <span className="checkmark"/>
                    </label>
                  </td>
                  <td data-label="Tasa-EA" className="tab-space-01">{row.codigo} </td>
                  <td data-label="Plazo-Mínimo" className="tab-space-01">{ row.nombre }</td>
                  <td data-label="Plazo-Máximo" className="tab-space-01">{ row.descripcion }</td>
                  <td data-label="Estado" className="tab-space-01">  { row.estado } </td>                             
              </tr>
          
               ))}
          </tbody>
      </table>
  </Grid>
  </Grid>
    
  );
}

export default TableGupoUsuarios;