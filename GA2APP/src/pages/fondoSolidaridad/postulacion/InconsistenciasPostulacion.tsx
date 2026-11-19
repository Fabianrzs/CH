import React from 'react';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';

const InconsistenciasPostulacion = (props: propsType) => {
    const {inconsistenciaSolicitud} = props;
    return (
        <>
        <CHPrivateContainer>
             <CHPaper title="Información Inconsistencias">
                 <table className="table">
                     <thead>
                         <tr>
                             <th>Descripción</th>
                         </tr>
                     </thead>
                     <tbody>
                         {inconsistenciaSolicitud?.map((item, index) => (
                             <tr key={`table-schedule${index}`}>
                                 <td className='dateReport3'>{item.descripcion}</td>
                             </tr>
                         ))}
                     </tbody>
                 </table>
             </CHPaper>
        </CHPrivateContainer>
     </>
    )
}

type propsType = {
    inconsistenciaSolicitud: any[];
};

export default InconsistenciasPostulacion;