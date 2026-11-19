import { Button, createStyles, Grid, makeStyles, MenuItem, TablePagination, TablePaginationProps } from '@material-ui/core';
import { CloudDownload } from '@material-ui/icons';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { CATEGORIES, EXECUTINGUNITDTO, FORCEDTO, IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import ApiReportes from '../../../services/api-reportes.service';
import UtilService from '../../../utils/util.service';
import CatalogApi from '../../../services/api-catalog.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from '../../../services/auth.service';
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
	}),
);

function TableReporteFSPorEstadoTramite({
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
							<th colSpan={12} style={{ backgroundColor: '#f1f2f6', textAlign: 'right' }}>
								<Button variant="contained" color="primary"
									onClick={generarExcel}
									style={{ width: '15%', margin: '10px', marginRight: '15px' }} startIcon={<CloudDownload />}
								>
									Generar Reporte
								</Button>
							</th>
						</tr>
						<tr>
							<th className={classes.title}>Radicado</th>
							<th className={classes.title}>Tipo Identificación</th>
							<th className={classes.title}>Identificación</th>
							<th className={classes.title}>Nombre y apellidos</th>
							<th className={classes.title}>Ciudad</th>
							<th className={classes.title}>Dirección</th>
							<th className={classes.title}>Teléfono</th>
							<th className={classes.title}>Fuerza</th>
							<th className={classes.title}>Categoría</th>
							<th className={classes.title}>Fecha Solicitud</th>
							<th className={classes.title}>Estado</th>
							<th className={classes.title}>Participación</th>
						</tr>
					</thead>
					<tbody>
						{data?.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)?.map((row: any, index: any) => (
							<tr key={`tr${index}`}>
								<td className="tab-space-01">{row?.radicado}</td>
								<td className="tab-space-01">{row?.cedulaAfiliado}</td>
								<td className="tab-space-01">{row?.identificacionPostulante}</td>
								<td className="tab-space-01">{row?.nombrePostulante}</td>
								<td className="tab-space-01">{row?.ciuedad}</td>
								<td className="tab-space-01">{String(row?.direccion).includes('00000000000') ? '' : row?.direccion}</td>
								<td className="tab-space-01">{row?.telefono}</td>
								<td className="tab-space-01">{row?.fuerza}</td>
								<td className="tab-space-01">{row?.categoria}</td>
								<td className="tab-space-01">{UtilService.getDate(row?.fechaSolicitud)}</td>
								<td className="tab-space-01">{row?.estadoSolicitud}</td>
								<td className="tab-space-01">{row?.participacion}</td>
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

const ReporteFSPorEstadoTramite = (props: Props) => {
	const classes = useStyles();
	const apiFondoSolidaridad = ApiFondoSolidaridad();
	const apiReportes = ApiReportes();
	const { catalogState } = props;
	const [loading, setLoading] = useState(false);
	const [informacion, setInformacion] = useState<any>(null);
	const [page, setPage] = useState(0);
	const estados = ['PENDIENTE', 'APROBADO'];
	const formularioReporteEstadoTramite = useForm({
		defaultValues: {
			radicado: '',
			tipoIdentificacion: '',
			identificacion: '',
			fechaPostulacion: null,
			fuerza: '',
			unidadEjecutora: '',
			categoria: '',
			estado: ''
		}
	})

	const generarReporte = async () => {
		setLoading(true);
		const columnDataList = informacion?.map((row: any) => ({
			data: [
				String(row?.consecutivo),
				row?.radicado,
				row?.cedulaAfiliado,
				row?.identificacionPostulante,
				row?.nombrePostulante,
				row?.ciuedad,
				String(row?.direccion).includes('00000000000') ? '' : row?.direccion,
				row?.telefono,
				row?.fuerza,
				row?.categoria,
				UtilService.getDate(row?.fechaSolicitud),
				row?.estadoSolicitud,
				String(row?.participacion),
			]

		}));
		const request = {
			columnDataList: columnDataList.map((item: any) => ({
				data: item.data
			})),

			columnName: [
				"Consecutivo",
				"Radicado",
				"Tipo de Identificación",
				"Número de Identificación",
				"Nombres y Apellidos",
				"Ciudad",
				"Dirección",
				"Teléfono",
				"Fuerza",
				"Categoría",
				"Fecha Solicitud",
				"Estado",
				"Participación",
			]

		}

		await apiReportes.GenerarExcels(request)
			.then(response => {
				if (response) {
					const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
					const url = window.URL.createObjectURL(blob);
					const link = document.createElement('a');
					link.href = url;
					link.download = `ReporteEstadoFS-Fecha:${moment().format()}.csv`;
					document.body.appendChild(link);
					link.click();
					window.URL.revokeObjectURL(url);

					const auditoriaApi = AuditoriaApi();
					const user = AuthService.getUserInfo();
					const nav = window.navigator as any;
					const usrAg = nav.userAgentData.brands.find(
						(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
					)?.brand;


					const requestAuditoria = {
						navegador: usrAg,
						sistemaOperativo: nav.userAgentData.platform,
						nombreUsuario: user.USR_USERNAME,
						usurioId: user.USR_ID,
						ip: publicIpAddress,
						modulo: 'Reportes Fondo Solidaridad',
						hostname: document.location.host,
						tipoAccion: 'DESCARGAS',
						idTipoAccion: 5,
						descripcion: 'Descarga de reporte'

					} as Auditoria;

					auditoriaApi.InsertAuditoria(requestAuditoria);
					setLoading(false);
				}
			})
			.catch(() => {
				setLoading(false);
				toast('Error al generar reporte', { type: toast.TYPE.ERROR });
			})
	}

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

	const categoriaSeleccion = formularioReporteEstadoTramite.getValues("categoria");
	const fuerzaSeleccionadaDesc = formularioReporteEstadoTramite.getValues("fuerza");
	const unidadEjecutoraSeleccionadaNombre = formularioReporteEstadoTramite.getValues("unidadEjecutora");
	const tipoIdentiSeleccionadaNombre = Number(formularioReporteEstadoTramite.getValues("tipoIdentificacion"));

	const categoriaNombre = categoriaSeleccion || "";
	const fuerzaNombre = fuerzaSeleccionadaDesc || "";
	const unidadEjecutoraNombre = unidadEjecutoraSeleccionadaNombre || "";
	const tipoIdentiNombre = catalogState.IdentificationType?.find(
		(item) => item.id === tipoIdentiSeleccionadaNombre
	)?.descripcion || "";


	const consultarInformacion = async (data: any) => {

		setLoading(true);
		const request = {
			reportModel: null,
			reportStatus: {
				radicado: data.radicado === '' ? null : data.radicado,
				tipoIdentificacion: data.tipoIdentificacion === '' || data.tipoIdentificacion === 0 ? null : String(data.tipoIdentificacion),
				identificacion: data.identificacion === '' ? null : String(data.identificacion),
				fechaPostulacion: data.fechaPostulacion,
				fuerza: data.fuerza === '' || data.fuerza === 0 ? null : data.fuerza,
				unidadEjecutora: data.unidadEjecutora === '' || data.unidadEjecutora === 0 ? null : data.unidadEjecutora,
				categoria: data.categoria === '' || data.categoria === 0 ? null : data.categoria,
				estado: data.estado === '' || data.estado === 0 ? null : data.estado
			},
		}

		await apiFondoSolidaridad
			.postReportesFondoSolidaridad(request)
			.then(async response => {
				if (response.data) {
					setPage(0);
					setInformacion(response.data);

					const auditoriaApi = AuditoriaApi();
					const user = AuthService.getUserInfo();
					const nav = window.navigator as any;
					const usrAg = nav.userAgentData.brands.find(
						(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
					)?.brand;

					const requestAuditoria = {
						navegador: usrAg,
						sistemaOperativo: nav.userAgentData.platform,
						nombreUsuario: user.USR_USERNAME,
						usurioId: user.USR_ID,
						ip: publicIpAddress,
						modulo: 'Consulta de Información Fondo Solidaridad Por Estado De Trámite',
						hostname: document.location.host,
						tipoAccion: 'CONSULTAS',
						idTipoAccion: 3,
						descripcion: `Consulta de Información Fondo Solidaridad Por Estado De Trámite realizada con los siguientes filtros:
                    - Radicado: ${request.reportStatus.radicado}
                    - Tipo Identificación: ${tipoIdentiNombre}
                    - No. Identificación: ${request.reportStatus.identificacion}
					- Fecha Postulación: ${request.reportStatus.fechaPostulacion}
					- Fuerza: ${fuerzaNombre}
					- Unidad Ejecutora: ${unidadEjecutoraNombre}
                    - Categoria: ${categoriaNombre}
                    - Estado: ${request.reportStatus.estado}`

					} as Auditoria;

					auditoriaApi.InsertAuditoria(requestAuditoria);
					setLoading(false);
				} else {
					setLoading(false);
					toast("No se encontraron resultados");
				}
			})
			.catch((e: any) => {
				toast(`Error consultar información ${e.message}`, { type: toast.TYPE.ERROR });
				setLoading(false);
			});
	}

	const cancelar = () => {
		setInformacion([]);
		formularioReporteEstadoTramite.reset();
	}

	return (
		<>
			<LoadingComponent loading={loading} />
			<CHPaper title="Consulta Información Fondo Solidaridad Por Estado De Trámite">
				<Grid container>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="radicado"
							control={formularioReporteEstadoTramite.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly={false}
									InputLabelProps={{ shrink: true }}
									label="Radicado"
									value={value}
									onChange={e => {
										if (e.target.value.length < 30) {
											onChange(
												e.target.value.replace(
													/[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`]/g,
													'',
												),
											);
										}
									}}
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="tipoIdentificacion"
							control={formularioReporteEstadoTramite.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									error={!!error}
									label="Tipo Identificación"
									value={value}
									helperText={
										error ? error.message : null
									}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>
									{catalogState.IdentificationType?.map(
										(
											option: IDENTIFICATIONTYPEDTO,
										) => (
											<MenuItem
												key={option.id}
												value={option.id}
											>
												{option.descripcion}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="identificacion"
							control={formularioReporteEstadoTramite.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									numberMask
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly={false}
									InputLabelProps={{ shrink: true }}
									label="No. Identificación"
									value={value}
									onChange={e => {
										if (e.target.value.length <= 12) {
											onChange(e);
										}
									}}
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="fechaPostulacion"
							control={formularioReporteEstadoTramite.control}
							render={({ field: { onChange, value } }) => (
								<CHDatePicker
									label="Fecha Postulación"
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
							name="fuerza"
							control={formularioReporteEstadoTramite.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
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
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="unidadEjecutora"
							control={formularioReporteEstadoTramite.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
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
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="categoria"
							control={formularioReporteEstadoTramite.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
									label="Categoría"
									value={value}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>

									{catalogState.categories?.map((option: CATEGORIES) => {
										if (option.id !== 0) {
											return (
												<MenuItem
													key={option.id}
													value={option.descripcion}
												>
													{
														option.descripcion
													}
												</MenuItem>
											)
										}
										return null;
									})}

								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="estado"
							control={formularioReporteEstadoTramite.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
									label="Estado"
									value={value}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>

									{estados?.map((option: string) => (
										<MenuItem
											key={option}
											value={option}
										>
											{option}
										</MenuItem>
									))}
								</CHSelect>
							)}
						/>
					</Grid>
				</Grid>
				<Grid container>
					<Grid className={classes.columnButton} item xs={12} md={12}>
						<Button variant="contained" color="primary" style={{ width: '15%' }} onClick={formularioReporteEstadoTramite.handleSubmit(consultarInformacion)}>
							Consultar
						</Button>
						<Button variant="contained" color="primary" style={{ width: '15%', marginLeft: '10px' }} onClick={cancelar}>
							Cancelar
						</Button>
					</Grid>
				</Grid>
			</CHPaper>
			<TableReporteFSPorEstadoTramite
				data={informacion}
				page={page}
				setPage={setPage}
				generarExcel={generarReporte}
			/>
		</>
	)
}

type Props = {
	catalogState: CatalogState;
}

export default ReporteFSPorEstadoTramite;