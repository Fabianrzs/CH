import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { makeStyles, createStyles, Grid, Button } from '@material-ui/core';
import moment from 'moment';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import SolicitudApi from '../../../../services/api-solicitud.service';
import UtilService from '../../../../utils/util.service';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		}
	}),
);

const api = SolicitudApi()

function TrazabilidadSolicitud({
	responseSolicitud,
	observaciones,
}: {
	responseSolicitud: any;
	observaciones: string;
}) {
	const classes = useStyles();

	const [trazabilidad, setTrazabilidad] = useState<any[]>([])

	useEffect(() => {
		obtenerTrazabilidadSolicitud()
	}, [])

	const obtenerTrazabilidadSolicitud = async () => {
		await api.obtenerTrazabilidadSolicitud(responseSolicitud.solicitudId)
			.then((response) => {
				setTrazabilidad(response.data)
			})
	}

	return (
		<CHPaper title='Trazabilidad'>
			<Grid container>
				<table className="table">
					<thead>
						<tr>
							<th>Actividad</th>
							<th>Usuario</th>
							<th>Estado</th>
							<th>Observaciones</th>
							<th>Fecha de Inicio</th>
							<th>Fecha Fin</th>
						</tr>
					</thead>
					<tbody>
					{trazabilidad?.map((row, index) => (
							<tr key={`table-${index}`}>
								<td style={{ width: '21%' }}>{row.descripcionTarea}</td>
								<td style={{ width: '17%' }}>{row.nombreUsuario}</td>
								<td style={{ width: '7%', padding: '35px' }}>{row.descripcionEstadoSolicitud}</td>
								<td style={{ width: '23%' }}>{row.descripcionEstadoSolicitud === "PENDIENTE" && responseSolicitud.solicitudTareaId === row.idTareaSolicitud ? `${observaciones ?? ''}` : row.observacion}</td>
								<td style={{ width: '16%'}}> {UtilService.getFullDateWithTimeDirection(new Date(String(row?.fechaCreacion).toString()))}
								</td>
								<td style={{ width: '16%'}}>{row?.fechaFin && row?.fechaFin.includes('0001-01-01')? '' : UtilService.getFullDateWithTimeDirection(new Date(String(row?.fechaFin).toString()))}
								</td>
							</tr>
						))}
					</tbody>
				</table>
			</Grid>
		</CHPaper>
	)
}

export default TrazabilidadSolicitud;
