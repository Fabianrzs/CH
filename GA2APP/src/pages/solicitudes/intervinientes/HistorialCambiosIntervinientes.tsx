import { Grid } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import ApiSolicitudes from '../../../services/api-solicitud.service';
import UtilService from '../../../utils/util.service';

const HistorialCambiosIntervinientes = (props: PropsType) => {  

    const apiSolicitud = ApiSolicitudes();
    const {idInterviniente} = props;
    const [historialCambios,setHistorialCambios] = useState<any[]>([]);

    useEffect(() => {
      obtenerHistoriaCambiosIntervinientes(idInterviniente);
    }, [idInterviniente])

    const obtenerHistoriaCambiosIntervinientes = async(idInterviniente: string) => {
      
      await apiSolicitud.GetObtenerHistorialCambiosIntervinientes(idInterviniente)
        .then(response => {
            if(response.data){
              setHistorialCambios(response.data);
            }
        })
        .catch((error: any) => {
          toast(`Error al consultar Historial de Cambios intervinientes: ${error.message}`, { type: toast.TYPE.ERROR });
        })
    }

    return(
        <Grid container>
        <CHPaperNew>
          <table className="table">
            <thead>
              <tr>
                <th className='column2'>Pestaña</th>
                <th className='column2'>Campo</th>
                <th className='column2'>Valor Anterior</th>
                <th className='column2'>Valor Actualización</th>
                <th className='column2'>Módulo</th>
                <th className='column2'>Fecha Modificación</th>
                <th className='column2'>Usuario</th>
              </tr>
            </thead>
            <tbody>
              {historialCambios.map((row: any, index: any) => (
                  <tr key={`table-${index}`}>
                      <td className='tab-space-01'>Información Básica</td>
                      <td className='tab-space-01'>{row?.campo}</td>
                      <td className='tab-space-01'>{row?.vlrAnterior}</td>
                      <td className='tab-space-01'>{row?.vlrNuevo}</td>
                      <td>{row?.modulo}</td>
                      <td className='tab-space-01'> {UtilService.getFullDateWithTimeDirection(new Date(String(row?.fechaModificacion).toString()))}</td>
                      <td className='tab-space-01'> {row?.nombreUsuario}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </CHPaperNew>
      </Grid>
    )
}

type PropsType = {
  idInterviniente: string;
}

export default HistorialCambiosIntervinientes;