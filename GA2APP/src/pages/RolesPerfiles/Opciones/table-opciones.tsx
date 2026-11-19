import React, { ChangeEvent, useEffect, useState } from 'react';
import swal from 'sweetalert';
import {
  Grid,
} from '@material-ui/core';
import ContainerTitle from '../../../components/containe-title/container-title'  // '../../../../components/containe-title/container-title';
import history from '../../../utils/history.router';
import { GrupoOpcionesModel } from '../../../types/grupo-opciones.model.type';


function TableOpcion({
  allOptiions,
  setEditGrupo,
  deleteGrupo,
}: {
    allOptiions: GrupoOpcionesModel[];
    setEditGrupo: (grupo:GrupoOpcionesModel) => void;
    deleteGrupo: (grupo:GrupoOpcionesModel) => void;

}) {

  const [items,setItems] = useState<GrupoOpcionesModel[]>();


    const onClickEliminar=(grupo : GrupoOpcionesModel)=>(event: React.MouseEvent) =>{

      swal({
        text: `¿Seguro que quiere eliminar al usuario ${grupo.nombre} ?`,
        icon: "warning",
        buttons: ["No","Sí"]
    }).then (respuesta =>{
        if(respuesta){   
            deleteGrupo(grupo);      
        }
    })

     event.preventDefault();

    }

    useEffect(() => {
      setItems(allOptiions);   
    }, [allOptiions]);


    const onClickEditarGrupoUsuario=(grupo: GrupoOpcionesModel)=>(event: React.MouseEvent) =>{
         
        setEditGrupo(grupo);    
        event.preventDefault();   
    }

    const onClickCreaGrupoUsurio=()=>(event: React.MouseEvent) =>{
       
        history.push("/CrearOpciones")
        event.preventDefault();

    }

    const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {
    
      if(event.target.value.toUpperCase()===''){
        setItems(allOptiions);
      }else{  
        setItems(filterItems(event.target.value.toUpperCase()));
      }
     
    }

    const filterItems = (value:string) => {
      const filter = allOptiions.filter(
        (x) =>  
          x.nombre?.toUpperCase().includes(value) ||
          x.descripcion?.toUpperCase().includes(value)
                
      );
      return filter;
    };


    return (
      <Grid>

    <ContainerTitle
      title="Opciones"
    />

      <Grid className="container-24">
          <input type="search" placeholder="Buscar.." onChange={handleChangeBuscar}/>
      </Grid>

      
      <Grid className="container-23">
      <Grid>
        <button type="button" className="bgcian button-i blue flex" onClick={onClickCreaGrupoUsurio()} >    
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
              <th className="tab-space-01">Acciones</th>
   
          </thead>
          <tbody>

          {items?.map((row, index) => (
            <>
            <tr key={`table-${index}`}>
                <td data-label="Descripción" className="tab-space-01"> { row.codigo} </td>
                <td data-label="Tasa-EA" className="tab-space-01">{row.nombre}</td>
                <td data-label="Tasa-EA" className="tab-space-01">{row.descripcion}</td>
                <td data-label="Estado" className="tab-space-01 actions">   
                <Grid>
                    <span  aria-label="Editar" onClick={onClickEditarGrupoUsuario(row)} ><span className="ico fch-Editar-0-1 icoMid-3"/></span>
                    <span  aria-label="Eliminar" onClick={onClickEliminar(row)} ><span className="ico fch-Eliminar-0-1 icoMid-3"/></span>
                </Grid>               
                    
                  </td>          
              </tr>     
              {row.subOpciones?.map((element, index) => (
                <tr key ={`index${index}`}>
                <td data-label="Descripción" className="tab-space-01"> { element.codigo} </td>
                <td data-label="Tasa-EA" className="tab-space-01">{element.nombre}</td>
                <td data-label="Tasa-EA" className="tab-space-01">{element.descripcion}</td>
                <td data-label="Estado" className="tab-space-01 actions">   
                <Grid>
                    <span  aria-label="Editar" onClick={onClickEditarGrupoUsuario(element)} ><span className="ico fch-Editar-0-1 icoMid-3"/></span>
                    <span  aria-label="Eliminar" onClick={onClickEliminar(element)} ><span className="ico fch-Eliminar-0-1 icoMid-3"/></span>
                </Grid>               
                    
                  </td>     
                </tr>
              ))}    
            </>
                 
            ))}
          </tbody>
      </table>  
  </Grid>
  </Grid>
   
    );
}

export default TableOpcion;