import React, { useEffect, useState } from 'react';
import {
	Button,
	createStyles,
	Grid,
	makeStyles,
	MenuItem,
	TablePagination,
	TablePaginationProps
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { sortBy } from 'lodash';
import { toast } from 'react-toastify';
import { useParams } from 'react-router-dom';
import moment from 'moment';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import { AppState } from '../../../store/rootReducer';
import { LoginState } from '../../../reducers/login.reducer';
import { startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import SolicitudPostulacion from './SolicitudPostulacion';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import { ACTIVIDAD_POSTULACION, ESTADO_POSTULACION, TIPO_SOLICITUD } from '../constants/fondoSolidaridad.constants';
import { SolicitudDto } from '../../../models/requests/solicitud-tarea.model';
import { LoadingComponent } from '../../../components/loading/loading.component';
import AuthService from '../../../services/auth.service';
import CatalogApi from '../../../services/api-catalog.service';
import AuditoriaApi from '../../../services/api-auditorias.servise'
import { Auditoria } from '../../../types/Auditoria.model.type';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '400px',
			marginLeft: '.5rem',
		},
		processButton: {
			backgroundColor: '#1C3F8B',
			color: '#fff',
			margin: '8px 0',
		},
		processButtonDeshabilitado: {
			backgroundColor: 'transparent',
			color: '#676767',
			margin: '8px 0',
		},
		textTable: {
			textAlign: 'center'
		}
	}),
);

function PostulacionesTable({
	data,
	onClickItem,
	page,
	setPage,
}: {
	data: SolicitudDto[] | null;
	onClickItem: (data: SolicitudDto | null) => void;
	page: number;
	setPage: (value: number) => void;
}) {
	const classes = useStyles();
	const [loading, setLoading] = useState(false);
	const usuario = AuthService.getUserInfo();
	const rowsPerPageOptions = [10, 20, 50, 100];
	const [rowsPerPage, setRowsPerPage] = React.useState(rowsPerPageOptions[0]);

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

	return data !== null ? (
		<>
			<LoadingComponent loading={loading} />
			<table className="credit-table">
				<thead>
					<tr>
						<th className={classes.textTable}>Procesar</th>
						<th className={classes.textTable}>No. Radicado</th>
						<th className={classes.textTable}>Tipo Sub-Modalidad</th>
						<th className={classes.textTable}>No. Documento</th>
						<th className={classes.textTable}>Nombres y Apellidos</th>
						<th className={classes.textTable}>Fecha Solicitud</th>
						<th className={classes.textTable}>Valor Solicitud</th>
						<th className={classes.textTable}>Usuario</th>
						<th className={classes.textTable}>Actividad Actual</th>
						<th className={classes.textTable}>Estado</th>
					</tr>
				</thead>
				<tbody>
					{data?.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)?.map((row, index) => (
						<tr key={`table-${index}`}>
							<td>
								{
									row.estadoProceso !== ESTADO_POSTULACION.RECHAZADO ? (
										<Button
											className={
												(row.estadoProceso === ESTADO_POSTULACION.ANULADO ||
													(row.estadoProceso === ESTADO_POSTULACION.APROBADO && row.actividad === ACTIVIDAD_POSTULACION.CALIFICACION)) ||
													((row.estadoProceso === ESTADO_POSTULACION.PENDIENTE ||
														row.estadoProceso === ESTADO_POSTULACION.BLOQUEADO ||
														row.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) &&
														(usuario !== null && row.userId === usuario?.USR_ID)) ?
													classes.processButton : classes.processButtonDeshabilitado
											}
											disabled={!((row.estadoProceso === ESTADO_POSTULACION.ANULADO ||
												(row.estadoProceso === ESTADO_POSTULACION.APROBADO && row.actividad === ACTIVIDAD_POSTULACION.CALIFICACION)) ||
												((row.estadoProceso === ESTADO_POSTULACION.PENDIENTE ||
													row.estadoProceso === ESTADO_POSTULACION.BLOQUEADO ||
													row.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) &&
													(usuario !== null && row.userId === usuario?.USR_ID)))}
											onClick={() => {
												if ((row.estadoProceso === ESTADO_POSTULACION.ANULADO ||
													(row.estadoProceso === ESTADO_POSTULACION.APROBADO && row.actividad === ACTIVIDAD_POSTULACION.CALIFICACION)) ||
													((row.estadoProceso === ESTADO_POSTULACION.PENDIENTE ||
														row.estadoProceso === ESTADO_POSTULACION.BLOQUEADO ||
														row.estadoProceso === ESTADO_POSTULACION.INCONSISTENTE) &&
														(usuario !== null && row.userId === usuario?.USR_ID))) {
													onClickItem(row);
													setLoading(false);
												}
											}}
										>
											{row.estadoProceso === ESTADO_POSTULACION.ANULADO ||
												(row.estadoProceso === ESTADO_POSTULACION.APROBADO && row.actividad === ACTIVIDAD_POSTULACION.CALIFICACION) ? 'DETALLES' : 'PROCESAR'}
										</Button>
									) : null
								}
							</td>
							<td>{row.noRadicado}</td>
							<td>{row.tipoSubModalidadNombre}</td>
							<td>{row.numeroDocumeto}</td>
							<td>{row.nombreCompletoAfiliado}</td>
							<td>{moment(row.fechaCreacion).format('YYYY-MM-DD h:mmA')}</td>
							<td>
								{" "}
							</td>
							<td>{row.userName}</td>
							<td>{row.actividad}</td>
							<td>{row.estadoProceso}</td>
						</tr>
					))}
				</tbody>
			</table>
			{
				data.length > 0 && (
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
		</>
	) : null;
};

const PostulacionInicio = (props: Props) => {
	const {
		loginState,
		startGetAllIdentificationTypes
	} = props;
	const classes = useStyles();
	const apiFondoSolidaridad = ApiFondoSolidaridad();
	const [page, setPage] = useState(0);
	const [tipoModalidad, setTipoModalidad] = useState<any[]>([]);
	const [solicitudes, setSolicitudes] = useState<SolicitudDto[] | null>([]);
	const [actividad, setActividad] = useState<any[]>([]);
	const [selectedData, setSelectedData] = useState<SolicitudDto | null>(null);
	const [agregarSolicitudPostulacion, setAgregarSolicitudPostulacion] = useState(false);
	const [loading, setLoading] = useState(false);
	const [actividadSolicitud, setActividadSolicitud] = useState('');
	const [convocatorias, setConvocatorias] = useState<any[]>([]);
	const [estadoSolicitud, setEstadoSolicitud] = useState('');
	const [datosSolicitudPostulacion, setDatosSolicitudPostulacion] = useState<any>(null);
	const { radicado, numero }: any = useParams();

	const formularioConsulta = useForm({
		defaultValues: {
			radicado: '',
			numeroConvocatoria: 0,
			identificacion: '',
			actividad: 0,
			tipoModalidad: 0,
		},
	});

	useEffect(() => {
		startGetAllIdentificationTypes(loginState.claims);
		consultarActividad();
		consultarTipoModalidad(TIPO_SOLICITUD.SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD);
		const busqueda = {
			radicado: '',
			numeroConvocatoria: '',
			estado: '',
			fechaDesde: '',
			fechaHasta: '',
			fechaPublicacion: '',
		}
		consultarConvocatorias(busqueda);

		if (radicado === undefined && numero === undefined) {
			formularioConsulta.setValue('identificacion', '');
			formularioConsulta.setValue('radicado', '');
		}

		// validacion numero !== "null"
		if (radicado === 'N' && numero !== "null") {
			formularioConsulta.setValue('identificacion', numero);
			const request = {
				radicado: '',
				idEtapa: '',

				identificacion: numero,
				tipoModalidad: '',
				numeroConvocatoria: '',
			}
			consultarSolicitudes(request);
		}

		if (radicado === 'S' && numero !== "null") {
			formularioConsulta.setValue('radicado', numero);
			const request = {
				radicado: numero,
				idEtapa: '',
				identificacion: '',
				tipoModalidad: '',
				numeroConvocatoria: '',
			}
			consultarSolicitudes(request);
		}
	}, []);

	const agregarNuevaPostulacion = () => {
		setAgregarSolicitudPostulacion(true);
	}

	const onItemIsSelected = (data: SolicitudDto | null) => {
		setSelectedData(data);
		setActividadSolicitud(data?.actividad || '');
		setEstadoSolicitud(data?.estadoProceso || '');
		setDatosSolicitudPostulacion(data);
	};

	const consultarActividad = async () => {
		await apiFondoSolidaridad
			.getConsultarActividad()
			.then(async response => {
				if (response.data) {
					const datosOrdenados = sortBy(response.data, 'nombre');
					setActividad(datosOrdenados);
				}
			})
			.catch((e) => {
				toast(`Error consultar actividades ${e.message}`, { type: toast.TYPE.ERROR });
			});
	};

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

	const consultarConvocatorias = async (busqueda: any) => {
		await apiFondoSolidaridad
			.getObtenerConvocatorias(busqueda)
			.then(async response => {
				if (response.data) {
					const arrayNumeroConvocatoria = response.data?.map((item: any) => item.numeroConvocatoria);
					const array: Set<number> = new Set(arrayNumeroConvocatoria);
					const arraySinDuplicados: number[] = Array.from(array);
					setConvocatorias(arraySinDuplicados);
				}
			})
			.catch((e) => {
				setLoading(false);
				toast(`Error consultar convocatorias ${e.message}`, { type: toast.TYPE.ERROR });
			});
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

	const handleSearch = (data: any) => {
		setLoading(true);
		const request = {
			radicado: data.radicado,
			idEtapa: data.actividad === 0 ? '' : data.actividad,
			identificacion: data.identificacion,
			tipoModalidad: data.tipoModalidad === 0 ? '' : data.tipoModalidad,
			numeroConvocatoria: data.numeroConvocatoria === 0 ? '' : data.numeroConvocatoria,
		}
		consultarSolicitudes(request);

		let descripcionEtapa = 'No especificado';
		let descripcionModalidad = 'No especificado';

		if (request.idEtapa === 4) {
			descripcionEtapa = 'CALIFICACIÓN';
		} else if (request.idEtapa === 2) {
			descripcionEtapa = 'CARGUE DE DOCUMENTOS FONDO SOLIDARIDAD';
		} else if (request.idEtapa === 3) {
			descripcionEtapa = 'ENTREVISTA';
		} else if (request.idEtapa === 1) {
			descripcionEtapa = 'SOLICITUD POSTULACIÓN FONDO SOLIDARIDAD';
		} else if (request.idEtapa === 7) {
			descripcionEtapa = 'VERIFICACIÓN DE CUENTA INDIVIDUAL';
		} else if (request.idEtapa === 6) {
			descripcionEtapa = 'VERIFICACIÓN DE EMBARGOS';
		} else if (request.idEtapa === 5) {
			descripcionEtapa = 'VERIFICACIÓN DE NOVEDADES';
		}

		if (request.tipoModalidad === 1) {
			descripcionModalidad = 'HÉROES';
		} else if (request.tipoModalidad === 3) {
			descripcionModalidad = 'PROYECTOS Y MECANISMOS ESPECIALES';
		}


		const auditoriaApi = AuditoriaApi();
		const user = AuthService.getUserInfo();
		const nav = window.navigator as any;
		const usrAg = nav.userAgentData.brands.find(
			(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
		)?.brand;

		try {
			const requestAuditoria = {
				navegador: usrAg,
				sistemaOperativo: nav.userAgentData.platform,
				nombreUsuario: user.USR_USERNAME,
				usurioId: user.USR_ID,
				ip: publicIpAddress,
				modulo: 'Fondo Solidaridad / Postulación',
				hostname: document.location.host,
				tipoAccion: 'CONSULTAS',
				idTipoAccion: 3,
				descripcion: `Consulta postulación fondo solidaridad con los siguientes filtros:
					  - Radicado: ${request.radicado || 'No especificada'}
					  - No. Identificación: ${request.identificacion || 'No especificado'}
					  - Número Convocatoria: ${request.numeroConvocatoria || 'No especificada'}
					  - Etapa del Proceso: ${descripcionEtapa || 'No especificado'}
					  - Tipo de Modalidad: ${descripcionModalidad || 'No especificado'}`,
			} as Auditoria;

			auditoriaApi.InsertAuditoria(requestAuditoria);
		} catch (error: any) {
			const descripcionError = `Error en consulta de información crédito
            - Radicado: ${request.radicado || 'No especificada'}
			- No. Identificación: ${request.identificacion || 'No especificado'}
			- Número Convocatoria: ${request.numeroConvocatoria || 'No especificada'}
			- Etapa del Proceso: ${descripcionEtapa || 'No especificado'}
			- Tipo de Modalidad: ${descripcionModalidad || 'No especificado'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Fondo Solidaridad / Postulación',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);

            toast("Ocurrió un error al realizar la consulta. Inténtalo de nuevo.");
		}


	}

	const consultarSolicitudes = async (request: any) => {
		await apiFondoSolidaridad
			.getConsultarSolicitudes(request)
			.then(async response => {
				if (response?.data && response?.data?.length > 0) {
					setSolicitudes(response.data);
					setPage(0);
					setLoading(false);
				} else {
					setSolicitudes(null);
					setLoading(false);
					toast("No se encontraron resultados");
				}
			})
			.catch((e) => {
				toast(`Error consultar solicitudes postulación ${e.message}`, { type: toast.TYPE.ERROR });
				setLoading(false);
			});
	};

	if (agregarSolicitudPostulacion || selectedData) {
		return (
			<SolicitudPostulacion
				actividadSolicitud={actividadSolicitud}
				estadoSolicitud={estadoSolicitud}
				setActividadSolicitud={setActividadSolicitud}
				setEstadoSolicitud={setEstadoSolicitud}
				datosSolicitudPostulacion={datosSolicitudPostulacion}
				setDatosSolicitudPostulacion={setDatosSolicitudPostulacion}
				setAgregarSolicitudPostulacion={setAgregarSolicitudPostulacion}
				selectedData={selectedData}
				setSelectedData={setSelectedData}
				consultarSolicitudes={consultarSolicitudes}
			/>
		)
	}

	return (
		<CHPrivateContainer>
			<CHPageTitle
				topText="Gestión de postulaciones"
				subText=""
				title1="Postulación"
				title2="Fondo de Solidaridad"
			/>
			<CHPaper title="Información Solicitud">
				<Grid container>
					<Grid className={classes.column} item xs={12} md={2}>
						<Controller
							name="radicado"
							control={formularioConsulta.control}
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
					<Grid className={classes.column} item xs={12} md={2}>
						<Controller
							name="identificacion"
							control={formularioConsulta.control}
							rules={{
								minLength: {
									value: 4,
									message:
										'La longitud mínima del campo es de 4 caracteres',
								},
								maxLength: {
									value: 12,
									message:
										'La longitud máxima del campo es de 12 caracteres',
								},
							}}
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
					<Grid className={classes.column} item xs={12} md={2}>
						<Controller
							name="numeroConvocatoria"
							control={formularioConsulta.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									defaultValue=""
									label="Número Convocatoria"
									value={value}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>
									{convocatorias?.map(
										(option: any) => (
											<MenuItem
												key={option}
												value={option}
											>
												{option}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="actividad"
							control={formularioConsulta.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
									defaultValue=""
									label="Etapa del Proceso"
									value={value}
									onChange={onChange}
								>
									<MenuItem key={0} value={0} >
										SELECCIONE...
									</MenuItem>
									{actividad?.map(
										(option: any) => (
											<MenuItem
												key={option.id}
												value={option.id}
											>
												{option.nombre}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="tipoModalidad"
							control={formularioConsulta.control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
									defaultValue="0"
									label="Tipo de Modalidad"
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
				</Grid>
			</CHPaper>
			<div className="buttons">
				<Button
					className={classes.button}
					variant="outlined"
					onClick={formularioConsulta.handleSubmit(handleSearch)}
				>
					CONSULTAR
				</Button>
				<Button
					className={classes.button}
					variant="outlined"
					onClick={agregarNuevaPostulacion}
				>
					AGREGAR POSTULACIÓN
				</Button>
			</div>
			{
				solicitudes && (
					<>
						<LoadingComponent loading={loading} />
						<PostulacionesTable
							data={solicitudes}
							onClickItem={onItemIsSelected}
							page={page}
							setPage={setPage}
						/>
					</>
				)
			}

		</CHPrivateContainer>
	);
};

interface LinkMapStateToProps {
	loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
	productoState: state.producto,
	loginState: state.login,
});

interface LinkMapDispatchToProps {
	startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
	startGetAllIdentificationTypes,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(PostulacionInicio);