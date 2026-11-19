import React, { ChangeEvent, useEffect, useState } from 'react';
import swal from 'sweetalert';
import {
  Grid,
} from '@material-ui/core';
import ContainerTitle from '../../../../components/containe-title/container-title';
import { GrupoUsersModel } from '../../../../types/grupoUsers.model';
import history from '../../../../utils/history.router';


function TableGroupoOpciones({
  grupo,
  setEditGrupo,
  deleteGrupo,
}: {
   grupo: GrupoUsersModel[];
   setEditGrupo: (grupo:GrupoUsersModel) => void;
   deleteGrupo: (grupo:GrupoUsersModel) => void;

}) {

  const [items,setItems] = useState<GrupoUsersModel[]>();
  const onClickEliminar=(grupo : GrupoUsersModel)=>(event: React.MouseEvent) =>{

      swal({
        text: `¿Seguro que quiere eliminar este grupo de opciones ${grupo.nombre} ?`,
        icon: "warning",
        buttons: ["No","Sí"]
    }).then (respuesta =>{
        if(respuesta){   
          deleteGrupo(grupo); 
          setItems(items?.filter(x => x.id !== grupo.id)) 
        }
    })

     event.preventDefault();
    }

    

    useEffect(() => {
      setItems(grupo);   
    }, [grupo]);


    const onClickEditarGrupoUsuario=(grupo: GrupoUsersModel)=>(event: React.MouseEvent) =>{
         
        setEditGrupo(grupo);    
        event.preventDefault();   
    }

    const filterItems = (value:string) => {
      const filter = grupo.filter(
        (x) =>  
          x.nombre?.toUpperCase().includes(value) ||
          x.descripcion?.toUpperCase().includes(value)
                
      );
      return filter;
    };

    const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {
    
      if(event.target.value.toUpperCase()===''){
        setItems(grupo);
      }else{  
        setItems(filterItems(event.target.value.toUpperCase()));
      }
     
    }

    const onClickCreaGrupoOpciones=()=>(event: React.MouseEvent) =>{       
        history.push("/CrearGrupoOpciones")
        event.preventDefault();
    }


    return (
      <Grid>

    <ContainerTitle
      title="Administración de Grupo Opciones"
    />

      <Grid className="container-24">
          <input type="search" placeholder="Buscar.." onChange={handleChangeBuscar}/>
      </Grid>

      
      <Grid className="container-23">
      <Grid>
        <button type="button" className="bgcian button-i blue flex" onClick={onClickCreaGrupoOpciones()} >    
        <span className="ico fch-Agregar-0-1 icoMid-2 separador-marg-6"/> 
          AGREGAR  
        </button>
        </Grid>
      </Grid>
    
      <Grid className="container-17">
      <table className="table">
          <thead>
              <th className="tab-space-01">Código</th>
              <th className="tab-space-01">Nombre</th>
              <th className="tab-space-01">Descripción</th>
              <th className="tab-space-01">Estado</th>
              <th className="tab-space-01">Acciones</th>
   
          </thead>
          <tbody>

          {items?.map((row, index) => (
              <tr key={`table-${index}`}>
                  <td data-label="Descripción" className="tab-space-01"> { row.codigo} </td>
                  <td data-label="Tasa-EA" className="tab-space-01">{row.nombre}</td>
                  <td data-label="Tasa-EA" className="tab-space-01">{row.descripcion}</td>
                  <td data-label="Plazo-Mínimo" className="tab-space-01">{ row.estado }</td>
                  <td data-label="Estado" className="tab-space-01 actions">   
                  <Grid>
                      <span  aria-label="Editar" onClick={onClickEditarGrupoUsuario(row)} ><span className="ico fch-Editar-0-1 icoMid-3"/></span>
                      <span  aria-label="Eliminar" onClick={onClickEliminar(row)} ><span className="ico fch-Eliminar-0-1 icoMid-3"/></span>
                  </Grid>               
                     
                   </td>          
              </tr>
          
               ))}
          </tbody>
      </table>  
  </Grid>
  </Grid>
   
    );
}

export default TableGroupoOpciones;