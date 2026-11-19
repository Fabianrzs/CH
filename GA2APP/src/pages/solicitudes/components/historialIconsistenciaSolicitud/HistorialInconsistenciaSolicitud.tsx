import { Grid } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import moment from 'moment';
import CHPaper from "../../../../components/ch-paper/CHPaper";
import Inconsistencia from "../../../../services/api-inconsistencia.service";


const HistorialInconsistenciaSolicitud = (props: any) => {
    const { responseSolicitud } = props
    const inconsistenciaApi = Inconsistencia()
    const [inconsistencia, setInconsistencia] = useState<any[]>([])

    useEffect(() => {

        const IdSolicitud = responseSolicitud.solicitudId;
        inconsistenciaApi
            .GetInconsistencias(IdSolicitud)
            .then(response => {
                if (response) {
                    setInconsistencia(response.data);
                } else {
                    toast(`error ${response.returnMessage}`, { type: toast.TYPE.ERROR });
                }
            })
            .catch((e) => {
                toast(`error ${e}`, { type: toast.TYPE.ERROR });
            });

    }, [])


    return (
        <CHPaper title='Inconsistencia'>
            <Grid container>
                <table className="table">
                    <thead>
                        <tr>
                            <th>Fecha de Creación</th>
                            <th>Nombre Regla</th>
                            <th>Nombre</th>

                        </tr>
                    </thead>
                    <tbody>
                        {inconsistencia?.map((row, index) => (
                            <tr key={`table-${index}`}>
                                <td>{moment(row.fechacreacion).format('DD/MM/YYYY hh:mm A')}</td>
                                <td>{row.nombrE_REGLA}</td>
                                <td>{String(row.nombrE_REGLA).includes('BLOQUEO') && String(row.trA_NOMBRE).includes('Verificación novedades en los datos') ? `Seguridad documental grupo 1 ${String(row.trA_NOMBRE).split('Verificación novedades en los datos')[1].trim()}` : row.trA_NOMBRE}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </Grid>
        </CHPaper>
    )
}

export default HistorialInconsistenciaSolicitud
