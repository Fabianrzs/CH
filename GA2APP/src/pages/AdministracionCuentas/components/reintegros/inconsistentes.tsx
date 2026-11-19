import React from 'react';

const Inconsistentes = (props: props) => {

    const {listaInconsistentes} = props;

    return (
      <>
      {listaInconsistentes.length > 0 && (
         <table className="credit-table"    
         style={{
             margin: '20px',
             width: '95%',
         }}>
           <thead>
               <tr>
                   <th style={{ textAlign: 'center' }}>
                       Cédula del Afiliado
                   </th>
                   <th style={{ textAlign: 'center' }}>
                       Nombres
                   </th>
                   <th style={{ textAlign: 'center' }}>
                       Apellidos
                   </th>
                   <th style={{ textAlign: 'center' }}>
                       Observaciones
                   </th>
               </tr>
           </thead>
            <tbody>
            {
                listaInconsistentes?.map((row: any, index: any) => (
                        <tr  key={`table-${index}`}>
                            <td>{row?.numeroDocumento}</td>
                            <td>{row?.nombres}</td>
                            <td>{row?.apellidos}</td>
                            <td>{row?.observationAfiliado ? row?.observationAfiliado.toUpperCase() : ''}</td>
                        </tr>
                    ))
                }
           </tbody>
       </table>
      )}
      </>
    );
}

type props = {
    listaInconsistentes: any[];
}

export default Inconsistentes;