import { Button, createStyles, FormControlLabel, Grid, makeStyles, MenuItem, TablePagination, TablePaginationProps, Typography } from '@material-ui/core';
import { CloudDownload } from '@material-ui/icons';
import moment from 'moment';
import React, { useState, useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CatalogApi from '../../../services/api-catalog.service';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import ApiReportes from '../../../services/api-reportes.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import { EXECUTINGUNITDTO, FORCEDTO } from '../../../types/catalog.type';
import UtilService from '../../../utils/util.service';
import { TIPO_SOLICITUD } from '../constants/fondoSolidaridad.constants';
import AuditoriaApi from '../../../services/api-auditorias.servise';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		columnButton: {
			padding: '10px',
			textAlign: 'right',
		},
		title: {
			textAlign: 'center',
		},
		text: {
			marginTop: '50px',
			marginLeft: '10px',
			marginRight: '10px',
			color: '#808080'
		},
		checkBox: {
			color: '#808080'
		}
	}),
);

function TableReporteModeloFS({
	data,
	page,
	setPage,
	generarExcel
}: {
	data: any;
	page: number;
	setPage: (value: number) => void;
	generarExcel: () => void;
}) {

	const classes = useStyles();
	const rowsPerPageOptions = [10, 20, 50, 100];
	const [rowsPerPage, setRowsPerPage] = useState(rowsPerPageOptions[0]);

	const handleChangePage = (
		event: React.MouseEvent<HTMLButtonElement> | null,
		newPage: number,
	) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (
		event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
	) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

	const labelDisplayedRows: TablePaginationProps['labelDisplayedRows'] = ({ from, to, count }) => `${from}-${to} de ${count}`;

	return (
		<div>
			{data && data.length > 0 && (
				<table className="client-table">
					<thead>
						<tr>
							<th colSpan={15} style={{ backgroundColor: '#f1f2f6', textAlign: 'right' }}>
								<Button variant="contained" color="primary"
									onClick={generarExcel}
									style={{ width: '15%', margin: '10px', marginRight: '15px' }} startIcon={<CloudDownload />}
								>
									Generar Reporte
								</Button>
							</th>
						</tr>
						<tr>
							<th className={classes.title}>Tipo Identificación</th>
							<th className={classes.title}>Identificación</th>
							<th className={classes.title}>Nombre y apellidos</th>
							<th className={classes.title}>Modelo</th>
							<th className={classes.title}>Fuerza</th>
							<th className={classes.title}>Grado</th>
							<th className={classes.title}>Unidad Ejecutora</th>
							<th className={classes.title}>Fecha Primer Aporte</th>
							<th className={classes.title}>Fecha Ultimo Aporte</th>
							<th className={classes.title}>Número Aporte</th>
							<th className={classes.title}>Fecha Solicitud</th>
							<th className={classes.title}>Porcentaje Participación</th>
							<th className={classes.title}>Saldo</th>
							<th className={classes.title}>Identificación Beneficiario</th>
							<th className={classes.title}>Nombre Beneficiario</th>
						</tr>
					</thead>
					<tbody>
						{data?.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)?.map((row: any, index: any) => (
							<tr key={`tr${index}`}>
								<td className="tab-space-01">{row?.tipoIdentificacionAfiliado}</td>
								<td className="tab-space-01">{row?.numeroDocumentoAfiliado}</td>
								<td className="tab-space-01">{row?.nombreAfiliado}</td>
								<td className="tab-space-01">{row?.modelo}</td>
								<td className="tab-space-01">{row?.fuerza}</td>
								<td className="tab-space-01">{row?.grado}</td>
								<td className="tab-space-01">{row?.unidadEjecutora}</td>
								<td className="tab-space-01">{UtilService.getDate(row?.fechaPrimerAporte)}</td>
								<td className="tab-space-01">{UtilService.getDate(row?.fechaUltimoAporte)}</td>
								<td className="tab-space-01">{row?.numeroAporte}</td>
								<td className="tab-space-01">{UtilService.getDate(row?.fechaSolicitud)}</td>
								<td className="tab-space-01">{row?.porcentajeParticipacion}</td>
								<td className="tab-space-01">{UtilService.formatCurrency(row?.saldo)}</td>
								<td className="tab-space-01">{row?.identificacionBeneficiario}</td>
								<td className="tab-space-01">{row?.nombreBeneficiario}</td>
							</tr>
						))}
					</tbody>
				</table>
			)}

			{
				data && data.length > 0 && (
					<TablePagination
						component="div"
						count={data.length}
						page={page}
						onPageChange={handleChangePage}
						rowsPerPage={rowsPerPage}
						labelRowsPerPage="Filas por página"
						labelDisplayedRows={labelDisplayedRows}
						onRowsPerPageChange={handleChangeRowsPerPage}
					/>
				)
			}
		</div>

	);
}

const ReporteModeloFS = (props: Props) => {

	const classes = useStyles();
	const apiFondoSolidaridad = ApiFondoSolidaridad();
	const apiReportes = ApiReportes();
	const { catalogState } = props;
	const [tipoModalidad, setTipoModalidad] = useState<any[]>([]);
	const [informacion, setInformacion] = useState<any>(null);
	const [loading, setLoading] = useState(false);
	const [page, setPage] = useState(0);
	const formularioReporteModeloFS = useForm({
		defaultValues: {
			modelo: '',
			fuerza: '',
			unidadEjecutora: '',
			fechaPostulacionInicial: null,
			fechaPostulacionFinal: null,
			fechaInicialPrimerAporteDesde: null,
			fechaInicialPrimerAporteHasta: null,
			fechaUltimoAporteDesde: null,
			fechaUltimoAporteHasta: null,
			sinAportarAlaFecha: false,
		}
	})

	const modeloSeleccionadoId = formularioReporteModeloFS.getValues("modelo");
	const fuerzaSeleccionadaDesc = formularioReporteModeloFS.getValues("fuerza");
	const unidadEjecutoraSeleccionadaNombre = formularioReporteModeloFS.getValues("unidadEjecutora");

	const modeloSeleccionadoNombre = tipoModalidad.find(
		(item) => item.id === modeloSeleccionadoId
	)?.nombre || "";

	const fuerzaNombre = fuerzaSeleccionadaDesc || "";
	const unidadEjecutoraNombre = unidadEjecutoraSeleccionadaNombre || "";

	useEffect(() => {
		consultarTipoModalidad(TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD);
	}, []);

	const consultarTipoModalidad = async (idTipoSolicitud: number) => {
		await apiFondoSolidaridad
			.getConsultarTipoModalidadPostulante(idTipoSolicitud)
			.then(async response => {
				if (response.data) {
					setTipoModalidad(response.data);
				}
			})
			.catch((e) => {
				toast(`Error consultar tipo de modalidad ${e.message}`, { type: toast.TYPE.ERROR });
			});
	};

	// const generarExcel = async () => {

	// 	setLoading(false);
	// 	const columnDataList = informacion?.map((row: any) => ({
	// 		data: [
	// 			row?.tipoIdentificacionAfiliado,
	// 			row?.numeroDocumentoAfiliado,
	// 			row?.nombreAfiliado,
	// 			row?.modelo,
	// 			row?.fuerza,
	// 			row?.grado,
	// 			row?.unidadEjecutora,
	// 			UtilService.getDate(row?.fechaPrimerAporte),
	// 			UtilService.getDate(row?.fechaUltimoAporte),
	// 			String(row?.numeroAporte),
	// 			UtilService.getDate(row?.fechaSolicitud),
	// 			String(row?.porcentajeParticipacion),
	// 			UtilService.formatCurrency(row?.saldo),
	// 			row?.identificacionBeneficiario,
	// 			row?.nombreBeneficiario
	// 		]

	// 	}));
	// 	const request = {
	// 		columnDataList: columnDataList.map((item: any) => ({
	// 			data: item.data
	// 		})),
	// 		columnName: [
	// 			"Tipo de Identificación",
	// 			"Número de Identificación",
	// 			"Nombres y Apellidos",
	// 			"Modelo",
	// 			"Fuerza",
	// 			"Grado",
	// 			"Unidad Ejecutora",
	// 			"Fecha Primer Aporte",
	// 			"Fecha Ultimo Aporte",
	// 			"Número Aporte",
	// 			"Fecha Solicitud",
	// 			"Porcentaje Participación",
	// 			"Saldo",
	// 			"Identificación Beneficiario",
	// 			"Nombre Beneficiario",
	// 		]

	// 	}
	// 	await apiReportes.GenerarExcels(request)
	// 		.then(response => {
	// 			if (response) {
	// 				const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
	// 				const url = window.URL.createObjectURL(blob);
	// 				const link = document.createElement('a');
	// 				link.href = url;
	// 				link.download = `ReporteFS-Fecha:${moment().format()}.csv`;
	// 				document.body.appendChild(link);
	// 				link.click();
	// 				window.URL.revokeObjectURL(url);

	// 				const auditoriaApi = AuditoriaApi();
	// 				const user = AuthService.getUserInfo();
	// 				const nav = window.navigator as any;
	// 				const usrAg = nav.userAgentData.brands.find(
	// 					(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
	// 				)?.brand;

	// 				const requestAuditoria = {
	// 					navegador: usrAg,
	// 					sistemaOperativo: nav.userAgentData.platform,
	// 					nombreUsuario: user.USR_USERNAME,
	// 					usurioId: user.USR_ID,
	// 					ip: publicIpAddress,
	// 					modulo: 'Reportes Fondo Solidaridad',
	// 					hostname: document.location.host,
	// 					tipoAccion: 'DESCARGAS',
	// 					idTipoAccion: 5,
	// 					descripcion: 'Descarga de reporte'

	// 				} as Auditoria;

	// 				auditoriaApi.InsertAuditoria(requestAuditoria);
	// 				setLoading(false);
	// 			}
	// 		})
	// 		.catch(() => {
	// 			setLoading(false);
	// 			toast('Error al generar reporte', { type: toast.TYPE.ERROR });
	// 		})
	// }

	const generarExcel = async () => {
		setLoading(true);

		const columnDataList = informacion?.map((row: any) => ({
			data: [
				row?.tipoIdentificacionAfiliado,
				row?.numeroDocumentoAfiliado,
				row?.nombreAfiliado,
				row?.modelo,
				row?.fuerza,
				row?.grado,
				row?.unidadEjecutora,
				UtilService.getDate(row?.fechaPrimerAporte),
				UtilService.getDate(row?.fechaUltimoAporte),
				String(row?.numeroAporte),
				UtilService.getDate(row?.fechaSolicitud),
				String(row?.porcentajeParticipacion),
				UtilService.formatCurrency(row?.saldo),
				row?.identificacionBeneficiario,
				row?.nombreBeneficiario
			]
		}));

		const request = {
			columnDataList: columnDataList.map((item: any) => ({
				data: item.data
			})),
			columnName: [
				"Tipo de Identificación",
				"Número de Identificación",
				"Nombres y Apellidos",
				"Modelo",
				"Fuerza",
				"Grado",
				"Unidad Ejecutora",
				"Fecha Primer Aporte",
				"Fecha Ultimo Aporte",
				"Número Aporte",
				"Fecha Solicitud",
				"Porcentaje Participación",
				"Saldo",
				"Identificación Beneficiario",
				"Nombre Beneficiario",
			]
		};

		const auditoriaApi = AuditoriaApi();
		const user = AuthService.getUserInfo();
		const nav = window.navigator as any;
		const usrAg = nav.userAgentData?.brands?.find(
			(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
		)?.brand;

		try {
			const response = await apiReportes.GenerarExcels(request);

			if (response) {
				const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
				const url = window.URL.createObjectURL(blob);
				const link = document.createElement('a');
				link.href = url;
				link.download = `ReporteFS-Fecha:${moment().format('YYYY-MM-DD_HH-mm')}.xlsx`;
				document.body.appendChild(link);
				link.click();
				window.URL.revokeObjectURL(url);

				const requestAuditoria: Auditoria = {
					navegador: usrAg,
					sistemaOperativo: nav.userAgentData?.platform,
					nombreUsuario: user?.USR_USERNAME,
					usurioId: user?.USR_ID,
					ip: publicIpAddress,
					modulo: 'Reportes Fondo Solidaridad',
					hostname: document.location.host,
					tipoAccion: 'DESCARGAS',
					idTipoAccion: 5,
					descripcion: 'Descarga de reporte'
				};

				auditoriaApi.InsertAuditoria(requestAuditoria);
			} else {
				toast('La respuesta del servidor fue vacía', { type: toast.TYPE.ERROR });
			}
		} catch (error: any) {

			const descripcionError = `Error al generar archivo Excel de reporte fondo solidaridad.
			- Error: ${error?.message || 'Error desconocido'}
			- Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
			- Código HTTP: ${error?.response?.status || 'No disponible'}`;

			const requestAuditoriaError: Auditoria = {
				navegador: usrAg,
				sistemaOperativo: nav.userAgentData?.platform,
				nombreUsuario: user?.USR_USERNAME,
				usurioId: user?.USR_ID,
				ip: publicIpAddress,
				modulo: 'Reportes Fondo Solidaridad',
				hostname: document.location.host,
				tipoAccion: 'ERRORES Y EXCEPCIONES',
				idTipoAccion: 8,
				descripcion: descripcionError
			};

			auditoriaApi.InsertAuditoria(requestAuditoriaError);
			toast('Error al generar reporte', { type: toast.TYPE.ERROR });
		} finally {
			setLoading(false);
		}
	};


	const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
	const catalogApi = CatalogApi();

	const isValidIPv4 = (ip: string) => {
		const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
		return ipv4Regex.test(ip);
	};

	const isValidIPv6 = (ip: string) => {
		const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
		return ipv6Regex.test(ip);
	};

	const enviarIp = async (ip: any) => {
		const convertIp = ip.split('.')[0]

		const hexadecimal = convertIp
		await catalogApi
			.getObtenerIpLocal(hexadecimal)
			.then(async (response: any) => {

				if (response) {
					if (response.isSuccess) {
						setPublicIpAddress(response.data);
					}
				}
			})
			.catch(e => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});

	}

	useEffect(() => {
		const fetchLocalIPAddress = () => {
			const peerConnection = new RTCPeerConnection({ iceServers: [] });
			peerConnection.createDataChannel('');
			peerConnection.createOffer()
				.then((offer) => peerConnection.setLocalDescription(offer))
				.catch((error) => console.error('Error creating offer:', error));

			peerConnection.onicecandidate = (event) => {
				if (event.candidate) {
					const ip: any = event.candidate.address;
					if (isValidIPv4(ip) || isValidIPv6(ip)) {
						enviarIp(ip);
					} else {
						enviarIp(ip);
					}
				}
			};

			return () => {
				peerConnection.close();
			};
		};
		fetchLocalIPAddress();

	}, []);

	// const consultarInformacion = async (data: any) => {

	// 	setLoading(true);

	// 	const request = {
	// 		reportModel: {
	// 			modelo: data.modelo === '' || data.modelo === 0 ? null : data.modelo,
	// 			fuerza: data.fuerza === '' || data.fuerza === 0 ? null : data.fuerza,
	// 			unidadEjecutora: data.unidadEjecutora === '' || data.unidadEjecutora === 0 ? null : data.unidadEjecutora,
	// 			fechaPostulacionFinal: data.fechaPostulacionFinal,
	// 			fechaPostulacionInicial: data.fechaPostulacionInicial,
	// 			fechaInicialPrimerAporteDesde: data.fechaInicialPrimerAporteDesde,
	// 			fechaInicialPrimerAporteHasta: data.fechaInicialPrimerAporteHasta,
	// 			fchaUltimoAporteDesde: data.fechaUltimoAporteDesde,
	// 			fchaUltimoAporteHasta: data.fechaUltimoAporteHasta,
	// 			sinAportarAlaFecha: data.sinAportarAlaFecha,
	// 		},
	// 		reportStatus: null
	// 	}

	// 	await apiFondoSolidaridad
	// 		.postReportesFondoSolidaridad(request)
	// 		.then(async response => {
	// 			if (response.data) {
	// 				setPage(0);
	// 				setInformacion(response.data);

	// 				const auditoriaApi = AuditoriaApi();
	// 				const user = AuthService.getUserInfo();
	// 				const nav = window.navigator as any;
	// 				const usrAg = nav.userAgentData.brands.find(
	// 					(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
	// 				)?.brand;

	// 				const requestAuditoria = {
	// 					navegador: usrAg,
	// 					sistemaOperativo: nav.userAgentData.platform,
	// 					nombreUsuario: user.USR_USERNAME,
	// 					usurioId: user.USR_ID,
	// 					ip: publicIpAddress,
	// 					modulo: 'Consulta de Información Modelo Fondo Solidaridad',
	// 					hostname: document.location.host,
	// 					tipoAccion: 'CONSULTAS',
	// 					idTipoAccion: 3,
	// 					descripcion: `Consulta de Información Modelo Fondo Solidaridad realizada con los siguientes filtros:
	//                 - Modelo: ${modeloSeleccionadoNombre}
	//                 - Fuerza: ${fuerzaNombre}
	//                 - Unidad Ejecutora: ${unidadEjecutoraNombre}
	// 				- Fecha Inicial Postulación: ${request.reportModel.fechaPostulacionInicial}
	// 				- Fecha Final Postulación: ${request.reportModel.fechaPostulacionFinal}`

	// 				} as Auditoria;

	// 				auditoriaApi.InsertAuditoria(requestAuditoria);

	// 				setLoading(false);
	// 			} else {
	// 				setLoading(false);
	// 				toast("No se encontraron resultados");
	// 			}
	// 		})
	// 		.catch((e) => {
	// 			toast(`Error consultar información ${e.message}`, { type: toast.TYPE.ERROR });
	// 			setLoading(false);
	// 		});
	// }

	const consultarInformacion = async (data: any) => {
		setLoading(true);

		const request = {
			reportModel: {
				modelo: data.modelo === '' || data.modelo === 0 ? null : data.modelo,
				fuerza: data.fuerza === '' || data.fuerza === 0 ? null : data.fuerza,
				unidadEjecutora: data.unidadEjecutora === '' || data.unidadEjecutora === 0 ? null : data.unidadEjecutora,
				fechaPostulacionFinal: data.fechaPostulacionFinal,
				fechaPostulacionInicial: data.fechaPostulacionInicial,
				fechaInicialPrimerAporteDesde: data.fechaInicialPrimerAporteDesde,
				fechaInicialPrimerAporteHasta: data.fechaInicialPrimerAporteHasta,
				fchaUltimoAporteDesde: data.fechaUltimoAporteDesde,
				fchaUltimoAporteHasta: data.fechaUltimoAporteHasta,
				sinAportarAlaFecha: data.sinAportarAlaFecha,
			},
			reportStatus: null
		};

		const auditoriaApi = AuditoriaApi();
		const user = AuthService.getUserInfo();
		const nav = window.navigator as any;
		const usrAg = nav.userAgentData?.brands?.find(
			(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
		)?.brand;

		try {
			const response = await apiFondoSolidaridad.postReportesFondoSolidaridad(request);

			if (response?.data) {
				setPage(0);
				setInformacion(response.data);

				const requestAuditoria: Auditoria = {
					navegador: usrAg,
					sistemaOperativo: nav.userAgentData?.platform,
					nombreUsuario: user?.USR_USERNAME,
					usurioId: user?.USR_ID,
					ip: publicIpAddress,
					modulo: 'Consulta de Información Modelo Fondo Solidaridad',
					hostname: document.location.host,
					tipoAccion: 'CONSULTAS',
					idTipoAccion: 3,
					descripcion: `Consulta realizada con los siguientes filtros:
					- Modelo: ${modeloSeleccionadoNombre}
					- Fuerza: ${fuerzaNombre}
					- Unidad Ejecutora: ${unidadEjecutoraNombre}
					- Fecha Inicial Postulación: ${request.reportModel.fechaPostulacionInicial}
					- Fecha Final Postulación: ${request.reportModel.fechaPostulacionFinal}`
				};

				auditoriaApi.InsertAuditoria(requestAuditoria);
			} else {
				toast("No se encontraron resultados");
			}
		} catch (error: any) {
			console.error("Error al consultar información:", error);

			const descripcionError = `Error al consultar información modelo fondo solidaridad.
			- Error: ${error?.message || 'Error desconocido'}
			- Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
			- Código HTTP: ${error?.response?.status || 'No disponible'}`;

			const requestAuditoriaError: Auditoria = {
				navegador: usrAg,
				sistemaOperativo: nav.userAgentData?.platform,
				nombreUsuario: user?.USR_USERNAME,
				usurioId: user?.USR_ID,
				ip: publicIpAddress,
				modulo: 'Consulta de Información Modelo Fondo Solidaridad',
				hostname: document.location.host,
				tipoAccion: 'ERRORES Y EXCEPCIONES',
				idTipoAccion: 8,
				descripcion: descripcionError
			};

			auditoriaApi.InsertAuditoria(requestAuditoriaError);
			toast(`Error al consultar información`, { type: toast.TYPE.ERROR });
		} finally {
			setLoading(false);
		}
	};


	const cancelar = () => {
		setInformacion([]);
		formularioReporteModeloFS.reset();
	}

	return (
		<>
			<LoadingComponent loading={loading} />
			<CHPaper title="Consulta Información Modelo Fondo Solidaridad">
				<Grid container>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="modelo"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
									defaultValue="0"
									label="Modelo"
									value={value}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>
									{tipoModalidad?.map(
										(option: any) => (
											<MenuItem
												key={option.id}
												value={option.id}
											>
												{
													option.nombre
												}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fuerza"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
									defaultValue="0"
									label="Fuerza"
									value={value}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>
									{catalogState?.Force.map(
										(option: FORCEDTO) => (
											<MenuItem
												key={option.id}
												value={option.descripcion}
											>
												{
													option.descripcion
												}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="unidadEjecutora"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
									defaultValue="0"
									label="Unidad Ejecutora"
									value={value}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>
									{catalogState?.ExecutingUnit.map(
										(option: EXECUTINGUNITDTO) => (
											<MenuItem
												key={option.id}
												value={option.nombre}
											>
												{
													option.nombre
												}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fechaPostulacionInicial"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHDatePicker
									label="Fecha Inicial Postulación"
									onChange={onChange}
									value={value}
									inputProps={{ readOnly: false }}
									InputLabelProps={{ shrink: true }}
									autoOk
								/>
							)}
						/>
					</Grid>
					<p className={classes.text}>{"  "}</p>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fechaPostulacionFinal"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHDatePicker
									label="Fecha Final Postulación"
									onChange={onChange}
									value={value}
									inputProps={{ readOnly: false }}
									InputLabelProps={{ shrink: true }}
									autoOk
								/>
							)}
						/>
					</Grid>
				</Grid>
				<Grid container>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fechaInicialPrimerAporteDesde"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHDatePicker
									label="Fecha Inicial Primer Aporte"
									onChange={onChange}
									value={value}
									inputProps={{ readOnly: false }}
									InputLabelProps={{ shrink: true }}
									autoOk
								/>
							)}
						/>
					</Grid>
					<p className={classes.text}>Y</p>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fechaInicialPrimerAporteHasta"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHDatePicker
									label=" "
									onChange={onChange}
									value={value}
									inputProps={{ readOnly: false }}
									InputLabelProps={{ shrink: true }}
									autoOk
								/>
							)}
						/>
					</Grid>
				</Grid>
				<Grid container>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fechaUltimoAporteDesde"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHDatePicker
									label="Fecha Final Primer Aporte"
									onChange={onChange}
									value={value}
									inputProps={{ readOnly: false }}
									InputLabelProps={{ shrink: true }}
									autoOk
								/>
							)}
						/>
					</Grid>
					<p className={classes.text}>Y</p>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fechaUltimoAporteHasta"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<CHDatePicker
									label=" "
									onChange={onChange}
									value={value}
									inputProps={{ readOnly: false }}
									InputLabelProps={{ shrink: true }}
									autoOk
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="sinAportarAlaFecha"
							control={formularioReporteModeloFS.control}
							render={({ field: { onChange, value } }) => (
								<FormControlLabel
									control={
										<CHCheckbox
											checked={value}
											onChange={onChange}
											color="primary" label=''
										/>
									}
									label={
										<Typography style={{ color: '#808080', whiteSpace: 'nowrap', }}>
											Sin aportar a la fecha
										</Typography>
									}
									style={{ color: '#808080' }}
								/>
							)}
						/>
					</Grid>
				</Grid>
				<Grid container>
					<Grid className={classes.columnButton} item xs={12} md={12}>
						<Button variant="contained" color="primary" style={{ width: '15%' }} onClick={formularioReporteModeloFS.handleSubmit(consultarInformacion)}>
							Consultar
						</Button>
						<Button variant="contained" color="primary" style={{ width: '15%', marginLeft: '10px' }} onClick={cancelar}>
							Cancelar
						</Button>
					</Grid>
				</Grid>
			</CHPaper>
			<TableReporteModeloFS
				data={informacion}
				page={page}
				setPage={setPage}
				generarExcel={generarExcel}
			/>
		</>
	)

}


type Props = {
	catalogState: CatalogState;
}

export default ReporteModeloFS;