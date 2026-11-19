import React, { ChangeEvent, useEffect, useState } from 'react';
import {
  Grid,
} from '@material-ui/core';
import ContainerTitle from '../../../../components/containe-title/container-title';
import { GrupoOpcionesModel } from '../../../../types/grupo-opciones.model.type';
import CHCheckboxV2 from '../../../../components/ch-checkbox/CHCheckboxV2';



function TableOpciones({
  Allopciones,
  OpcionsGroup, 
  setOpciones,
  setSubOpciones,
}: {
    Allopciones: GrupoOpcionesModel[];
    OpcionsGroup:GrupoOpcionesModel[]; 
    setOpciones:(grupo:GrupoOpcionesModel) => void;
    setSubOpciones:(grupo:any) => void;

}) {

  const [items,setItems] = useState<GrupoOpcionesModel[]>();


    const handleChange=(value:GrupoOpcionesModel)=>() =>{

      setOpciones(value);
   
    }

    const handleChangeSubOpciones = (value: any) => () => {
      setSubOpciones(value)
    }

    
    useEffect(() => {
      OpcionsGroup.forEach(element => {
        const index = Allopciones.findIndex(x => x.id === element.id)
        if(Allopciones?.length > 0){
          Allopciones[index].checked = true;

          element.subOpciones.forEach(item => {
            const indexItem = Allopciones[index].subOpciones.findIndex(x => x.codigo === item.codigo)
            Allopciones[index].subOpciones[indexItem].checked = item.checked
          });
        }
      });
    }, [OpcionsGroup]);

    useEffect(() => {
      console.log(Allopciones)
      setItems(Allopciones)
    }, [Allopciones])

    const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {
    
      if(event.target.value.toUpperCase()===''){
        setItems(Allopciones);
      }else{  
        setItems(filterItems(event.target.value.toUpperCase()));
      }
     
    }


    const filterItems = (value:string) => {
      const filter = Allopciones.filter(
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

      
      <Grid className="container-17">
      <table className="table">
          <thead className="tab-space-01">
              <th> Items</th>
              <th className="tab-space-01">Código</th>
              <th className="tab-space-01">Nombre</th>
              <th className="tab-space-01">Descripción</th>
                 
          </thead>
          <tbody>

          {items?.map((row, index) => (
            <>
            <tr key={`table-${index}`}>
                 <td className="tab-space-01">   
                    <CHCheckboxV2 
                      id={`checkbox-${index}`} 
                      defaultChecked={row.checked} 
                      onChange={handleChange(row)} 
                      label=''
                    />
                 </td>
                  <td data-label="Descripción" className="tab-space-01"> { row.codigo} </td>
                  <td data-label="Tasa-EA" className="tab-space-01">{row.nombre}</td>
                  <td data-label="Tasa-EA" className="tab-space-01">{row.descripcion}</td> 
              </tr>
              {row.subOpciones?.map((element, index) => (
                <tr key ={`index${index}`}>
                  <td className="tab-space-01">   
                      <CHCheckboxV2 
                        id={`checkbox-subOpciones-${index}`}
                        onChange={handleChangeSubOpciones(element)} 
                        defaultChecked={element.checked} 
                        label='' 
                      />
                 </td>
                  <td data-label="Descripción" className="tab-space-01"> { element.codigo} </td>
                  <td data-label="Tasa-EA" className="tab-space-01">{element.nombre}</td>
                  <td data-label="Tasa-EA" className="tab-space-01">{element.descripcion}</td> 
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

export default TableOpciones;