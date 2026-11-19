import React, { useEffect, useState } from 'react';
import { Button, createStyles, makeStyles } from '@material-ui/core';
import { toast } from 'react-toastify';
import { UseFormHandleSubmit } from 'react-hook-form';
import { BOTONERA_SOLICITUD } from '../../../../constants/constants';
import { useIp } from '../../../../shared/context/IpContext';
import AuditoriaApi from '../../../../services/api-auditorias.servise';
import AuthService from '../../../../services/auth.service';

const useStyles = makeStyles(() =>
	createStyles({
		button: {
			width: '120px',
			marginLeft: '.5rem',
		},
	}),
);

function BotoneraSolicitud({
	onItemGoBack,
	onSubmit,
	onSubmitAprobar,
	onSubmitRechazar,
	onSubmitAnular,
	tipoAccion,
	handleSubmit,
	tipoAccionDetalleProcesar,
	tipoSolicitud,
	cambiarEstadoTramite,
	esRadicacionDevueltaPorRechazo,
	trazabilidad,
	deshabilitarAprobar
}: {
	onItemGoBack: () => void;
	onSubmit: (opciones?: { silencioso?: boolean }) => void | Promise<void>;
	onSubmitAprobar: () => void;
	onSubmitRechazar: () => void;
	onSubmitAnular: () => void;
	tipoAccion: number;
	handleSubmit: UseFormHandleSubmit<any, any>;
	tipoAccionDetalleProcesar: boolean;
	tipoSolicitud: any;
	cambiarEstadoTramite: any;
	esRadicacionDevueltaPorRechazo?: boolean;
	trazabilidad: any;
	deshabilitarAprobar?: boolean;
}) {


	const classes = useStyles();
	const [actualizacionRealizada, setActualizacionRealizada] = useState(false);
	const { publicIp } = useIp();
	const auditoriaApi = AuditoriaApi();
	const user = AuthService.getUserInfo();

	useEffect(() => {
		if (tipoAccion === BOTONERA_SOLICITUD.GUARDAR) {
			setActualizacionRealizada(false);
		}
	}, [tipoAccion]);

	const registrarAuditoriaGuardarActualizar = (accion: string) => {
		if (!publicIp) {
			console.warn("IP aún no disponible");
			return;
		}

		const nav = window.navigator as any;
		const usrAg = nav.userAgentData?.brands.find(
			(x: { brand: string }) => x.brand === "Google Chrome" || x.brand === "Microsoft Edge"
		)?.brand || "Navegador desconocido";

		const requestAuditoria = {
			navegador: usrAg,
			sistemaOperativo: nav.userAgentData?.platform || "Desconocido",
			nombreUsuario: user.USR_USERNAME,
			usurioId: user.USR_ID,
			ip: publicIp,
			modulo: "Solicitudes",
			hostname: document.location.host,
			tipoAccion: "GUARDAR/ACTUALIZAR",
			idTipoAccion: 4,
			descripcion: `El usuario ${user.USR_USERNAME} realizó la acción: ${accion}`,
		};

		auditoriaApi.InsertAuditoria(requestAuditoria);
	};

	const registrarAuditoriaAprobar = (accion: string) => {
		if (!publicIp) {
			console.warn("IP aún no disponible");
			return;
		}

		const nav = window.navigator as any;
		const usrAg = nav.userAgentData?.brands.find(
			(x: { brand: string }) => x.brand === "Google Chrome" || x.brand === "Microsoft Edge"
		)?.brand || "Navegador desconocido";

		const requestAuditoria = {
			navegador: usrAg,
			sistemaOperativo: nav.userAgentData?.platform || "Desconocido",
			nombreUsuario: user.USR_USERNAME,
			usurioId: user.USR_ID,
			ip: publicIp,
			modulo: "Solicitudes",
			hostname: document.location.host,
			tipoAccion: "APROBAR",
			idTipoAccion: 4,
			descripcion: `El usuario ${user.USR_USERNAME} realizó la acción: ${accion}`,
		};

		auditoriaApi.InsertAuditoria(requestAuditoria);
	};

	const registrarAuditoriaAnular = (accion: string) => {
		if (!publicIp) {
			console.warn("IP aún no disponible");
			return;
		}

		const nav = window.navigator as any;
		const usrAg = nav.userAgentData?.brands.find(
			(x: { brand: string }) => x.brand === "Google Chrome" || x.brand === "Microsoft Edge"
		)?.brand || "Navegador desconocido";

		const requestAuditoria = {
			navegador: usrAg,
			sistemaOperativo: nav.userAgentData?.platform || "Desconocido",
			nombreUsuario: user.USR_USERNAME,
			usurioId: user.USR_ID,
			ip: publicIp,
			modulo: "Solicitudes",
			hostname: document.location.host,
			tipoAccion: "ANULAR",
			idTipoAccion: 4,
			descripcion: `El usuario ${user.USR_USERNAME} realizó la acción: ${accion}`,
		};

		auditoriaApi.InsertAuditoria(requestAuditoria);
	};

	const disabledAprobar =
		!actualizacionRealizada ||
		(
			!esRadicacionDevueltaPorRechazo &&
			cambiarEstadoTramite === false &&
			(
				tipoAccionDetalleProcesar ||
				tipoSolicitud === 'PENDIENTE' ||
				tipoSolicitud === undefined
			)
		);

	const obtenerObservacion = (data: any) =>
		String(
			data?.observaciones ??
			data?.observacion ??
			data?.observacionesGrupo1 ??
			data?.observacionesGrupo2 ??
			''
		).trim();


	// const validarYAnular = async (data: any) => {
	// 	const observacion = obtenerObservacion(data);

	// 	if (!observacion) {
	// 		toast('Debe ingresar una observación antes de anular la solicitud.', {
	// 			type: toast.TYPE.ERROR,
	// 		});
	// 		return;
	// 	}

	// 	await onSubmitAnular();
	// 	registrarAuditoriaAnular('ANULAR');
	// };

	const validarYAnular = async (data: any) => {
		const observacion = obtenerObservacion(data);

		if (!observacion) {
			toast('Debe ingresar una observación antes de anular la solicitud.', {
				type: toast.TYPE.ERROR,
			});
			return;
		}

		try {

			await onSubmit();

			if (tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR) {
				setActualizacionRealizada(true);
			}


			await onSubmitAnular();


			registrarAuditoriaAnular('ANULAR');

		} catch (error) {
			console.error('Error al actualizar y anular la solicitud:', error);

			toast('Ocurrió un error al anular la solicitud.', {
				type: toast.TYPE.ERROR,
			});
		}
	};

	const mostrarErrorObservacionAnular = () => {
		toast('Debe ingresar una observación antes de anular la solicitud.', {
			type: toast.TYPE.ERROR,
		});
	};


	return (
		<>
			<div className="buttons">
				{handleSubmit !== undefined ? (
					<Button
						// type="button"
						className={classes.button}
						variant="outlined"
						onClick={async () => {
							await handleSubmit(onSubmit)();

							if (tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR) {
								setActualizacionRealizada(true);
							}

							registrarAuditoriaGuardarActualizar(
								tipoAccion === BOTONERA_SOLICITUD.GUARDAR ? 'GUARDAR' : 'ACTUALIZAR'
							);
						}}
						disabled={tipoAccionDetalleProcesar}
					>
						{tipoAccion === BOTONERA_SOLICITUD.GUARDAR ? 'GUARDAR' : 'ACTUALIZAR'}
					</Button>
				) : (
					<Button
						// type="button"
						className={classes.button}
						variant="outlined"
						onClick={async () => {
							await onSubmit();

							if (tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR) {
								setActualizacionRealizada(true);
							}

							registrarAuditoriaGuardarActualizar(
								tipoAccion === BOTONERA_SOLICITUD.GUARDAR ? 'GUARDAR1' : 'ACTUALIZAR'
							);
						}}
						disabled={tipoAccionDetalleProcesar}
					>
						{tipoAccion === BOTONERA_SOLICITUD.GUARDAR ? 'GUARDAR' : 'ACTUALIZAR'}
					</Button>
				)}

				{tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR ? (
					<Button
						// type="button"
						className={classes.button}
						variant="outlined"
						disabled={deshabilitarAprobar || !cambiarEstadoTramite}
						onClick={onSubmitAprobar}

					>
						APROBAR
					</Button>
				) : null}

				{/* {tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR ? (

					<Button
						type="button"
						className={classes.button}
						variant="outlined"
						onClick={async (e) => {
							e.preventDefault();
							e.stopPropagation();

							if (!handleSubmit) {
								toast('No fue posible validar las observaciones.', {
									type: toast.TYPE.ERROR,
								});
								return;
							}

							await handleSubmit(
								validarYAnular,
								mostrarErrorObservacionAnular
							)();
						}}
						disabled={
							(
								trazabilidad &&
								trazabilidad.length >= 1 &&
								trazabilidad[1]?.descripcionEstadoSolicitud === 'RECHAZADA'
							) || tipoAccionDetalleProcesar
						}
					>
						ANULAR
					</Button>
				) : null} */}
				{tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR ? (

					<Button
						type="button"
						className={classes.button}
						variant="outlined"
						onClick={async (e) => {
							e.preventDefault();
							e.stopPropagation();

							if (!onSubmitAnular) {
								toast('No fue posible anular la solicitud.', {
									type: toast.TYPE.ERROR,
								});
								return;
							}

							await onSubmitAnular();
						}}
						disabled={
							(
								trazabilidad &&
								trazabilidad.length >= 1 &&
								trazabilidad[1]?.descripcionEstadoSolicitud === 'RECHAZADA'
							) || tipoAccionDetalleProcesar
						}
					>
						ANULAR
					</Button>
				) : null}

				<Button
					className={classes.button}
					variant="outlined"
					onClick={() => {
						onItemGoBack();
					}}
				>
					REGRESAR
				</Button>
			</div>
		</>
	);
}

export default BotoneraSolicitud;