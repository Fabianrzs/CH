import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import CHPaper from '../../../components/ch-paper/CHPaper';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';

function PostuladosRechazados(props: typeProps) {

   const apiFondoSolidaridad = ApiFondoSolidaridad();
   const {datosConvocatoria, setTotalPostuladosRechazados} = props;
   const [datosPostulantes,setDatosPostulantes] = useState<any[]>([]);
   const [loading,setLoading] = useState(false);
   const [sinResultados,setSinResultados] = useState(false);

   useEffect(() => {
    consultarListaPostulantesRechazados(datosConvocatoria?.idConvocatoria);
   },[datosConvocatoria])

   const consultarListaPostulantesRechazados = async (idConvocatoria: string) => {
    setLoading(true);
    await apiFondoSolidaridad
        .getConsultarListaPostulantesRechazados(idConvocatoria)
        .then(async response => {
            if(response.data !== null ){
                if(response.data.length !== 0){
                    setSinResultados(false);
                    setDatosPostulantes(response.data);
                    setTotalPostuladosRechazados(response?.data?.length);
                    setLoading(false);
                }
                else {
                    setLoading(false);
                    setSinResultados(true);
                }
                    
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
        <CHPaper title="Postulados Rechazados">
            No se encontraron resultados 
        </CHPaper>
    </>;
}

if (loading) {
    return <CHLoadingProcess />;
}

    return (
        <>
          <CHPaper title="Postulados Rechazados">
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
                          Etapa del Proceso
                      </th>
                  </tr>
              </thead>
              <tbody>
					{datosPostulantes?.map((row, index) => (
						<tr key={`table-${index}`}>
							<td>{row?.noRadicado}</td>
							<td>{row?.modalidadName}</td>
							<td>{row?.afiliado}</td>
                            <td>{row?.unidadEjecutora}</td>
                            <td>{row?.categoria}</td>
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
    setTotalPostuladosRechazados: (value: number) =>void;
}

export default PostuladosRechazados;