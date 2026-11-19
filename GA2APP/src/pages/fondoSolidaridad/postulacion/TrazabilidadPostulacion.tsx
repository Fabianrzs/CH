import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import UtilService from '../../../utils/util.service';

const TrazabilidadPostulacion = (props: props) => {
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {datosSolicitudPostulacion} = props;
    const [informacionTrazabilidad, setInformacionTrazabilidad] = useState<any []>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        consultarInformacionTrazabilidad(datosSolicitudPostulacion?.idSolicitud);
    },[datosSolicitudPostulacion])

    const consultarInformacionTrazabilidad = async (idSolicitud: string) => {
        setLoading(true);
        await apiFondoSolidaridad
            .getObtenerInformacionTrazabilidad(idSolicitud)
            .then(async (response: any) => {
                if (response.data) {
                    setLoading(false);
                    setInformacionTrazabilidad(response.data);
                }else{
                    setLoading(false);
                }
            })
            .catch(e => {
                setLoading(false);
                toast(`Error al consultar información trazabilidad ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    return(
        <>
           <LoadingComponent loading={loading} />
           <CHPrivateContainer>
                <CHPaper title="Información Trazabilidad">
                    <table className="table">
                        <thead>
                            <tr>
                                <th>Actividad</th>
                                <th>Usuario</th>
                                <th>Estado</th>
                                <th>Observaciones</th>
                                <th>Fecha de Recibido</th>
                            </tr>
                        </thead>
                        <tbody>
                            {informacionTrazabilidad?.map((item, index) => (
                                <tr key={`table-schedule${index}`}>
                                    <td className='dateReport3'>{item.activity}</td>
                                    <td className='dateReport3'>{item.userName}</td>
                                    <td className='dateReport3'>{item.status}</td>
                                    <td className='dateReport3'>{item.observation ? item.observation.toUpperCase() : ''}</td>
                                    <td className='dateReport3'>{UtilService.getFullDateWithTimeDirection(new Date(String(item.dateCreated).toString()))}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </CHPaper>
           </CHPrivateContainer>
        </>
    )
}

type props = {
    datosSolicitudPostulacion: any;
}

export default TrazabilidadPostulacion;