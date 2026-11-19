import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import CHPaper from '../../../components/ch-paper/CHPaper';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';

function PostuladosCalificados(props: typeProps) {
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {datosConvocatoria,
           datosPostulantesCalificados,
           setDatosPostulantesCalificados,
           setTotalPostuladosCalificados} = props;
  
    const [loading,setLoading] = useState(false);
    const [sinResultados,setSinResultados] = useState(false);
  
   useEffect(() => {
    consultarListaPostulantesCalificados(datosConvocatoria?.idConvocatoria);
   },[datosConvocatoria])

   const consultarListaPostulantesCalificados = async (idConvocatoria: string) => {
    setLoading(true);
    await apiFondoSolidaridad
        .getConsultarListaPostulantesConvocatoriaPorPuntaje(idConvocatoria)
        .then(async response => {
            if(response.data !== null){
                    setSinResultados(false);
                    setDatosPostulantesCalificados(response.data);
                    setTotalPostuladosCalificados(response?.data?.length);
                    setLoading(false);
             } else {
              setLoading(false);
              setSinResultados(true);
            }
        })
        .catch(() => {
            setLoading(false);
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
};

if(sinResultados){
    return <>
        <CHPaper title="Postulados Calificados">
            No se encontraron resultados 
        </CHPaper>
    </>;
}

if (loading) {
    return <CHLoadingProcess />;
}

return (
        <>
          <CHPaper title="Postulados Calificados">
            <table className="credit-table"  
            style={{
                margin: '20px',
                width: '95%',
            }}>
              <thead>
                  <tr>
                      <th style={{ textAlign: 'center' }}>
                          Radicado
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Calidad de Postulación
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Afiliado
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Unidad Ejecutora
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Categoría
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Modalidad de Solución de Vivienda
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Puntaje
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Usuario
                      </th>
                      <th style={{ textAlign: 'center' }}>
                          Etapa del Proceso
                      </th>
                  </tr>
              </thead>
              <tbody>
					{datosPostulantesCalificados?.map((row: any, index: any) => (
						<tr key={`table-${index}`}>
							<td>{row.noRadicado}</td>
							<td>{row?.modalidadName}</td>
							<td>{row?.afiliado}</td>
                            <td>{row?.unidadEjecutora}</td>
                            <td>{row?.categoria}</td>
                            <td>{row?.modalidadSolucionVivienda}</td>
                            <td>{row?.puntaje}</td>
                            <td>{row?.usuario}</td>
                            <td>{row?.etapaProceso}</td>
						</tr>
					))}
				</tbody>
          </table>
          </CHPaper>
        </>
    );
}

type typeProps = {
    datosConvocatoria: any;
    datosPostulantesCalificados: any[];
    setDatosPostulantesCalificados: (value: any[]) => void;
    setTotalPostuladosCalificados: (value: number) => void;
}

export default PostuladosCalificados;