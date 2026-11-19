import {
	Button,
	CircularProgress,
	createStyles,
	Grid,
	makeStyles,
} from '@material-ui/core';
import React from 'react';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { SolicitudSubsidioResolucion } from './ResolucionSubsidios';

const useStyles = makeStyles(() =>
	createStyles({
		checkBox: {
			backgroundColor: '#0075ff',
			accentColor: '#1c3f8b',
			marginTop: '6px',
			width: '18px',
			height: '18px',
			cursor: 'pointer',
		},
		columnButtonAnular: {
			textAlign: 'right',
		},
		buttonAnular: {
			width: '275px',
			marginLeft: '.5rem',
		},
		contenedorCarga: {
			padding: '20px',
			textAlign: 'center',
		},
		mensajeSinResultados: {
			padding: '20px',
			textAlign: 'center',
		},
	}),
);

const obtenerRadicado = (
	subsidio: SolicitudSubsidioResolucion,
): string => {
	if (subsidio.radicadoGestionDocumental) {
		return subsidio.radicadoGestionDocumental;
	}

	if (subsidio.numeroRadicadoSG) {
		return subsidio.numeroRadicadoSG;
	}

	return '';
};

const obtenerCdp = (
	subsidio: SolicitudSubsidioResolucion,
): string => {
	if (subsidio.cdp) {
		return subsidio.cdp;
	}

	return '-';
};

const obtenerTercero = (
	subsidio: SolicitudSubsidioResolucion,
): string => {
	if (subsidio.tercero) {
		return subsidio.tercero;
	}

	if (subsidio.nombreCliente) {
		return subsidio.nombreCliente;
	}

	return '-';
};

const obtenerFactura = (
	subsidio: SolicitudSubsidioResolucion,
): string => {
	if (subsidio.factura) {
		return subsidio.factura;
	}

	return '-';
};

const formatearMoneda = (
	valor: number | null | undefined,
): string =>
	new Intl.NumberFormat('es-CO', {
		style: 'currency',
		currency: 'COP',
		minimumFractionDigits: 0,
		maximumFractionDigits: 0,
	}).format(Number(valor ?? 0));

const Subsidios = ({
	subsidios,
	cargando,
	subsidiosSeleccionados,
	setSubsidiosSeleccionados,
	anular,
}: Props) => {
	const classes = useStyles();

	const estaSeleccionado = (
		solicitudTareaId: string,
	): boolean =>
		subsidiosSeleccionados.some(
			(subsidioSeleccionado) =>
				subsidioSeleccionado.solicitudTareaId ===
				solicitudTareaId,
		);

	const seleccionarSubsidio = (
		subsidio: SolicitudSubsidioResolucion,
	) => {
		const subsidioSeleccionado = estaSeleccionado(
			subsidio.solicitudTareaId,
		);

		if (subsidioSeleccionado) {
			const nuevaSeleccion =
				subsidiosSeleccionados.filter(
					(item) =>
						item.solicitudTareaId !==
						subsidio.solicitudTareaId,
				);

			setSubsidiosSeleccionados(
				nuevaSeleccion,
			);

			return;
		}

		setSubsidiosSeleccionados([
			...subsidiosSeleccionados,
			subsidio,
		]);
	};

	const estanTodosSeleccionados =
		subsidios.length > 0 &&
		subsidios.every((subsidio) =>
			estaSeleccionado(
				subsidio.solicitudTareaId,
			),
		);

	const seleccionarTodos = () => {
		if (estanTodosSeleccionados) {
			setSubsidiosSeleccionados([]);
			return;
		}

		setSubsidiosSeleccionados([
			...subsidios,
		]);
	};

	const existenSubsidios =
		subsidios.length > 0;

	const existenSeleccionados =
		subsidiosSeleccionados.length > 0;

	return (
		<CHPaper title="Subsidios">
			<table
				className="credit-table"
				style={{
					margin: '10px',
					width: '99%',
				}}
			>
				<thead>
					<tr>
						<th
							style={{
								textAlign: 'center',
							}}
						>
							<input
								type="checkbox"
								className={
									classes.checkBox
								}
								checked={
									estanTodosSeleccionados
								}
								disabled={
									cargando ||
									!existenSubsidios
								}
								onChange={
									seleccionarTodos
								}
							/>
						</th>

						<th
							style={{
								textAlign: 'center',
							}}
						>
							Documento
						</th>

						<th
							style={{
								textAlign: 'center',
							}}
						>
							Radicado
						</th>

						<th
							style={{
								textAlign: 'center',
								width: '20%',
							}}
						>
							Afiliado
						</th>

						<th
							style={{
								textAlign: 'center',
							}}
						>
							CDP
						</th>

						<th
							style={{
								textAlign: 'center',
								width: '20%',
							}}
						>
							Tercero
						</th>

						<th
							style={{
								textAlign: 'center',
							}}
						>
							Factura
						</th>

						<th
							style={{
								textAlign: 'center',
							}}
						>
							Monto
						</th>

						<th
							style={{
								textAlign: 'center',
								width: '8%',
							}}
						>
							Estado Solicitud
						</th>
					</tr>
				</thead>

				<tbody>
					{cargando && (
						<tr>
							<td
								colSpan={9}
								className={
									classes.contenedorCarga
								}
							>
								<CircularProgress
									size={25}
								/>
							</td>
						</tr>
					)}

					{!cargando &&
						!existenSubsidios && (
							<tr>
								<td
									colSpan={9}
									className={
										classes.mensajeSinResultados
									}
								>
									No se encontraron
									solicitudes de Subsidio
									Secuestrados para Segundo
									Pago o Simultáneo.
								</td>
							</tr>
						)}

					{!cargando &&
						subsidios.map(
							(subsidio) => (
								<tr
									key={
										subsidio.solicitudTareaId
									}
									style={{
										textAlign:
											'center',
									}}
								>
									<td>
										<input
											type="checkbox"
											className={
												classes.checkBox
											}
											checked={estaSeleccionado(
												subsidio.solicitudTareaId,
											)}
											onChange={() =>
												seleccionarSubsidio(
													subsidio,
												)
											}
										/>
									</td>

									<td>
										{
											subsidio.cedulaCliente
										}
									</td>

									<td>
										{obtenerRadicado(
											subsidio,
										)}
									</td>

									<td>
										{
											subsidio.nombreCliente
										}
									</td>

									<td>
										{obtenerCdp(
											subsidio,
										)}
									</td>

									<td>
										{obtenerTercero(
											subsidio,
										)}
									</td>

									<td>
										{obtenerFactura(
											subsidio,
										)}
									</td>

									<td>
										{formatearMoneda(
											subsidio.valorRetirar,
										)}
									</td>

									<td>
										{
											subsidio.estadoSolicitud
										}
									</td>
								</tr>
							),
						)}
				</tbody>
			</table>

			<Grid
				className={
					classes.columnButtonAnular
				}
				item
				xs={12}
				md={12}
			>
				<Button
					className={
						classes.buttonAnular
					}
					variant="outlined"
					style={{
						marginRight: '5px',
					}}
					onClick={anular}
					disabled={
						cargando ||
						!existenSeleccionados
					}
				>
					ANULAR
				</Button>
			</Grid>
		</CHPaper>
	);
};

type Props = {
	subsidios:
		SolicitudSubsidioResolucion[];
	cargando: boolean;
	subsidiosSeleccionados:
		SolicitudSubsidioResolucion[];
	setSubsidiosSeleccionados: (
		subsidios:
			SolicitudSubsidioResolucion[],
	) => void;
	anular: () => void;
};

export default Subsidios;