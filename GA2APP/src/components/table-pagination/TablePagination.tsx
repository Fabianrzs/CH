import React, { ChangeEvent, useState } from 'react';
import {
    Grid,
} from '@material-ui/core';
import { PaginationTable } from '../../types/Auditoria.model.type';


function TablePagination({
    
    paginaActual,
    numPaginas,
    numFilas,
    changePagination
 
}: {

    paginaActual?:number;
    numPaginas?:number;
    numFilas?:number[];
    changePagination:(data:PaginationTable)=> void
   
   
}) {

    const [selectValue, setSelectValue] = useState<any>(paginaActual);

  

    const handleClickfirst = () => {

        const data={   
          PageNum:1,
          Pagesize: selectValue,
        } as PaginationTable;

        changePagination(data);

    };

    const handleClickend = () => {

      

        const data={   
            PageNum:numPaginas,
            Pagesize: selectValue,
          } as PaginationTable;
  
          changePagination(data);

          
    };

    const handleClickNext = () => {

        
        console.log("numero Row=> ",selectValue);
    };

    const handleClickendPre = () => {
        console.log("numero Row=> ",selectValue);
    };


    const handleChange = (event: ChangeEvent<HTMLSelectElement>) => {

        console.log("Valor=> ",event.target.value);

        setSelectValue(event.target.value);
        const data={   
            PageNum:1,
            Pagesize: selectValue,
          } as PaginationTable;

          changePagination(data);

    };

    return (
        <Grid className="container-24">
        <ul className="pag">
            <li>
                <select className="select-rows"
                onChange={handleChange}>
                     {numFilas?.map((row,index) => (       
                        <option value={row}  key={`table-${index}`}>{row} rows</option>    
                    ))}
             </select>
            </li>
            <li><span  aria-label="Mute volume" className="pointermano" onClick={handleClickfirst} ><span className="ico fch-Atras-0-1 purple-2"/></span></li>
            <li><span aria-label="Mute volume" className="pointermano" onClick={handleClickendPre}><span className="ico fch-Anterior-0-1 purple-2"/></span></li>
            <li className="cian" >{ paginaActual } de  { numPaginas }</li>
            <li><span  aria-label="Mute volume" className="pointermano" onClick={handleClickNext}><span className="ico fch-Siguiente-0-1 purple-2"/></span></li>
            <li><span aria-label="Mute volume" className="pointermano" onClick={handleClickend} ><span className="ico fch-Adelante-0-1 purple-2"/></span></li>
        </ul>
    </Grid>
    );
}

export default TablePagination;