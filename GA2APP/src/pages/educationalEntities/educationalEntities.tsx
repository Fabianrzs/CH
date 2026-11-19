import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import './table-cuentas.scss';
import { Button, Card, CardContent, Grid, MenuItem, TablePagination } from '@material-ui/core';
import WarningIcon from '@material-ui/icons/Warning';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { CatalogState } from '../../reducers/catalog.reducer';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import { AppState } from '../../store/rootReducer';
import CHPaperNew from '../../components/ch-paper/CHPaperV2';
import CHPageTitle2 from '../../components/ch-page-title/CHPageTitleV2';
import { IDENTIFICATIONTYPEDTO } from '../../types/catalog.type';
import ApiReportes from '../../services/api-reportes.service';
import {
	EntidadesEducativasFiltro,
	EntidadesEducativasFiltroRequest,
	EntidadesEducativasFiltroRequestInitial,
} from '../../types/educational.entities.type';
import { startDeleteEntidadEducativa } from '../../actions/educationalEntities.actions';
import { EducationalEntitiesState } from '../../reducers/educationalEntities.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';
import history from '../../utils/history.router';
import {
	startGetAllIdentificationTypes,
	startGetAllCities,
	startGetAllDepartments,
	startGetAllValoresCatalogoPorNombre
} from '../../actions/catalog.action';
import { LoginState } from '../../reducers/login.reducer';
import CHTextField from '../../components/ch-text-field/CHTextField';
import ContainerTitle from '../../components/containe-title/container-title';
import { CATALOG_NAME } from '../../constants/constants';
import NotificationService from '../../services/notification.service';
import ApiEntidadesEducativas from '../../services/api-entidadesEducativas.service';
import CHSelect from '../../components/ch-select/CHSelect';
import GrupoUsers from '../../services/api-grupoUsuarios.service';
import AuthService from '../../services/auth.service';
import AuditoriaApi from '../../services/api-auditorias.servise'
import { Auditoria } from '../../types/Auditoria.model.type';
import CatalogApi from '../../services/api-catalog.service';

const useStyles = makeStyles(() =>
	createStyles({
		marginInput: {
			margin: '.6rem',
		},
		iconRed: {
			padding: '8px',
			textAlign: 'inherit',
			color: '#FF0000',
			margin: '10px'
		},
		button: {
			width: '400px',
			marginLeft: '3rem',
			background: '#1C3F8B',
			color: '#FFFFFF'
		},
	}),
);

function EducationalEntitiesTable({
	data,
	catalogState,
	isDataLoaded,
	startDeleteEntidadEducativa,
	submitEntidadesEducativasFilter,

}: {
	data: EntidadesEducativasFiltro[] | undefined | null;
	catalogState: CatalogState;
	isDataLoaded: boolean;
	startDeleteEntidadEducativa: (idEntidad: string) => void;
	submitEntidadesEducativasFilter: any;
}) {
	const classes = useStyles();
	const [page, setPage] = useState(0);
	const [rowsPerPage, setRowsPerPage] = useState(10);
	const user = AuthService.getUserInfo();
	const ApiUsers = GrupoUsers();
	const [grupo, setGrupo] = useState<string | undefined>();

	const GetGrupo = async (USR_ID: string) => {
		try {
			const grupos = await ApiUsers.ObtenerGruposUsuarioPorId(USR_ID);
			if (grupos.data.length > 0) {
				const grupoObtenido = grupos.data[0]?.grupo;
				setGrupo(grupoObtenido);
			}
			return grupos;
		} catch (error) {
			toast('Error al descargar el reporte. Verifica la respuesta del servidor.', { type: toast.TYPE.ERROR });
			throw error;
		}
	};

	useEffect(() => {
		GetGrupo(user.USR_ID)
	}, []);

	const handleDetalleEntidadEducativa = (itemRow: EntidadesEducativasFiltro) => async (event: React.MouseEvent) => {
		history.push(`/EntidadesEducativasDetalle/${itemRow.idEntidad}`);
		event.preventDefault();
	}

	const handleDeleteEntidadEducativa = (itemRow: EntidadesEducativasFiltro) => async (event: React.MouseEvent) => {
		const confirmAlert = await NotificationService.confirmAlert(
			`¿Está seguro de eliminar la Entidad Educativa ${itemRow.razonSocial}?`,
		);

		if (confirmAlert) {
			await startDeleteEntidadEducativa(itemRow.idEntidad);
			submitEntidadesEducativasFilter({});
		} else {
			toast('No se pudo eliminar la entidad educativa.');
		}
	};

	if ((data == null || data.length === 0) && isDataLoaded) {
		return <div>No se encontraron resultados</div>;
	}

	const handleChangePage = (event: any, newPage: React.SetStateAction<number>) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (event: any) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

	return (
		<>
			<Grid item xs={12}>
				<Card>
					<ContainerTitle
						title="Entidades Educativas"
					/>
					<CardContent>
						< Grid className="container-23" >
							<table className="table">
								<thead>
									<tr>
										<th >
											Tipo de Identificación
										</th>
										<th >Número de Documento</th>
										<th >Razón Social</th>
										<th >Ciudad</th>
										<th >Teléfono</th>
										<th >
											Correo Electrónico
										</th>
										<th >
											Estado
										</th>
										<th >Acciones</th>
										<th >Reporte</th>
									</tr>
								</thead>
								<tbody>
									{data && data.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row, index) => (
										<tr key={`table-${index}`}>
											<td data-label="Identificacion" className="tab-space-01">
												{catalogState.IdentificationType.find(X => X.id === row.tipoIdentificacion)?.descripcion}
											</td>
											<td data-label="Nit" className="tab-space-01">{row.nit}</td>
											<td data-label="razonSocial" className="tab-space-01">{row.razonSocial}</td>
											<td data-label="Ciudad" className="tab-space-01">{row.nombreCiudad}</td>
											<td data-label="telefono" className="tab-space-01">{row.telefono}</td>
											<td data-label="correo" className="tab-space-01">{row.correoElectronico}</td>
											<td data-label="estado" className="tab-space-01">{row.bloqueo ? 'INACTIVA' : 'ACTIVA'}</td>
											<td data-label="acciones" className="tab-space-01 actions">
												<Grid>
													<span aria-label="Detalle" onClick={handleDetalleEntidadEducativa(row)}>
														<span className="ico fch-Hoja-Vida-0-1 icoMid-3" />
													</span>
													{grupo === "GVISP_L" && (
														<span aria-label="Eliminar" onClick={handleDeleteEntidadEducativa(row)}>
															<span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} />
														</span>
													)}
												</Grid>
											</td>
											<td data-label="fecCre" className="tab-space-01">
												{row?.solicitudesProcesadas && (
													<span
														key={`button-${row.idEntidad}`}
														className={`${classes.iconRed}`}
														style={{
															border: 'none',
															background: 'transparent',
															outline: 'none',
															padding: 0,
														}}
													>
														<WarningIcon />
													</span>
												)}
											</td>
										</tr>
									)
									)
									}
								</tbody>
							</table>
							<TablePagination
								rowsPerPageOptions={[10, 20, 30, 40]}
								component="div"
								count={data?.length ?? 0}
								rowsPerPage={rowsPerPage}
								page={page}
								onPageChange={handleChangePage}
								onRowsPerPageChange={handleChangeRowsPerPage}
								labelRowsPerPage="Filas por página"
							/>
						</Grid >
					</CardContent>
				</Card>
			</Grid>
		</>
	);
}

function educationalEntities({
	catalogState,
	loginState,
	educationalEntitiesState,
	startGetAllIdentificationTypes,
	startGetAllCities,
	startGetAllDepartments,
	startGetAllValoresCatalogoPorNombre,
	startDeleteEntidadEducativa,
}: {
	catalogState: CatalogState;
	loginState: LoginState;
	educationalEntitiesState: EducationalEntitiesState;
	startGetAllIdentificationTypes: (props: string) => void;
	startGetAllCities: () => void;
	startGetAllDepartments: () => void;
	startGetAllValoresCatalogoPorNombre: (nombreCatalogo: string) => void;
	startDeleteEntidadEducativa: (idEntidad: string) => void;
}): any {

	useEffect(() => {
		startGetAllIdentificationTypes(loginState.claims);
		startGetAllCities();
		startGetAllDepartments();
		startGetAllValoresCatalogoPorNombre(CATALOG_NAME.CUENTA_ACREDITADA);
		startGetAllValoresCatalogoPorNombre(CATALOG_NAME.TIPO_PERSONA);
	}, []);

	const classes = useStyles();
	const [isDataLoaded, setIsDataLoaded] = useState(false);

	const { control, handleSubmit, reset } = useForm<EntidadesEducativasFiltroRequest>(
		{
			defaultValues: EntidadesEducativasFiltroRequestInitial,
		},
	);

	const [educationalEntitiess, setEducationalEntitiess] = useState<any[]>([]);
	const api = ApiEntidadesEducativas();

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
				.catch((error) => toast('Error creating offer', { type: toast.TYPE.ERROR }));

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

	// const submitEntidadesEducativasFilter = async (
	// 	entidadEducativa: EntidadesEducativasFiltroRequest,
	// ) => {
	// 	setIsDataLoaded(true);
	// 	try {
	// 		const entidadesFiltradas = await api.getEntidadesEducativasFiltroRequest(entidadEducativa);

	// 		if (entidadesFiltradas?.data?.length > 0) {
	// 			setEducationalEntitiess(entidadesFiltradas.data);
	// 			const auditoriaApi = AuditoriaApi();

	// 			const user = AuthService.getUserInfo();
	// 			const nav = window.navigator as any;
	// 			const usrAg = nav.userAgentData.brands.find(
	// 				(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
	// 			)?.brand;

	// 			const request = {

	// 				tipoIdentificacion: entidadEducativa.tipoIdentificacion,
	// 				nit: entidadEducativa.nit,
	// 				razonSocial: entidadEducativa.razonSocial,

	// 			}
	// 			const requestAuditoria = {
	// 				navegador: usrAg,
	// 				sistemaOperativo: nav.userAgentData.platform,
	// 				nombreUsuario: user.USR_USERNAME,
	// 				usurioId: user.USR_ID,
	// 				ip: publicIpAddress,
	// 				modulo: 'Entidades Educativas',
	// 				hostname: document.location.host,
	// 				tipoAccion: 'CONSULTAS',
	// 				idTipoAccion: 3,
	// 				descripcion: `Consulta de entidades educativas con los siguientes filtros:
	// 				- Tipo de documento: ${request.tipoIdentificacion || 'No especificada'}
	// 				- Número de documento: ${request.nit || 'No especificado'}
	// 				- Nombre de entidad educativa: ${request.razonSocial || 'No especificado'}`,
	// 			} as Auditoria;

	// 			auditoriaApi.InsertAuditoria(requestAuditoria);

	// 		} else {
	// 			toast('No se encontraron entidades educativas con el filtro aplicado.', { type: toast.TYPE.WARNING });
	// 		}

	// 	} catch (error) {
	// 		toast('Error al aplicar el filtro y obtener datos.', { type: toast.TYPE.ERROR });
	// 	} finally {
	// 		setIsDataLoaded(false);
	// 	}
	// };

	const submitEntidadesEducativasFilter = async (
		entidadEducativa: EntidadesEducativasFiltroRequest,
	) => {

		if(String(entidadEducativa?.nit).trim() === "" &&
		   String(entidadEducativa?.razonSocial).trim() === "" &&
		   entidadEducativa?.tipoIdentificacion === 0){
			toast('Se debe ingresar por lo menos un filtro', { type: toast.TYPE.ERROR});
			return;
		}

		setIsDataLoaded(true);

		const auditoriaApi = AuditoriaApi();
		const user = AuthService.getUserInfo();
		const nav = window.navigator as any;
		const usrAg = nav.userAgentData?.brands?.find(
			(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
		)?.brand;

		const request = {
			tipoIdentificacion: entidadEducativa.tipoIdentificacion,
			nit: entidadEducativa.nit,
			razonSocial: entidadEducativa.razonSocial,
		};

		const requestAuditoriaBase = {
			navegador: usrAg,
			sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
			nombreUsuario: user?.USR_USERNAME || 'Desconocido',
			usurioId: user?.USR_ID || 0,
			ip: publicIpAddress,
			hostname: document.location.host,
			modulo: 'Entidades Educativas',
		};

		const descripcionFiltros = `Consulta de entidades educativas con los siguientes filtros:
	- Tipo de documento: ${request.tipoIdentificacion || 'No especificada'}
	- Número de documento: ${request.nit || 'No especificado'}
	- Nombre de entidad educativa: ${request.razonSocial || 'No especificado'}`;

		try {
			const entidadesFiltradas = await api.getEntidadesEducativasFiltroRequest(entidadEducativa);

			if (entidadesFiltradas?.data?.length > 0) {
				setEducationalEntitiess(entidadesFiltradas.data);

				const requestAuditoriaConsulta: Auditoria = {
					...requestAuditoriaBase,
					tipoAccion: 'CONSULTAS',
					idTipoAccion: 3,
					descripcion: descripcionFiltros,
				};

				auditoriaApi.InsertAuditoria(requestAuditoriaConsulta);
			} else {
				toast('No se encontraron entidades educativas con el filtro aplicado.', { type: toast.TYPE.WARNING });
			}
		} catch (error: any) {
			const descripcionError = `Se produjo un error al intentar consultar entidades educativas con los siguientes filtros:
	- Tipo de documento: ${request.tipoIdentificacion || 'No especificada'}
	- Número de documento: ${request.nit || 'No especificado'}
	- Nombre de entidad educativa: ${request.razonSocial || 'No especificado'}
	- Error: ${error?.message || 'Error desconocido'}
	- Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
	- Código HTTP: ${error?.response?.status || 'No disponible'}`;

			const requestAuditoriaError: Auditoria = {
				...requestAuditoriaBase,
				tipoAccion: 'ERRORES Y EXCEPCIONES',
				idTipoAccion: 8,
				descripcion: descripcionError,
			};

			auditoriaApi.InsertAuditoria(requestAuditoriaError);
			toast('Error al aplicar el filtro y obtener datos.', { type: toast.TYPE.ERROR });
		} finally {
			setIsDataLoaded(false);
		}
	};


	const handleClear = () => {
		setIsDataLoaded(false);
		reset({
			tipoIdentificacion: 0,
			nit: '',
			razonSocial: '',
		});

	};

	const onClickExcel = async () => {
		if (!educationalEntitiess || educationalEntitiess.length === 0) {
			toast('No hay datos cargados para descargar.', { type: toast.TYPE.WARNING });
			return;
		}

		const columnDataList = educationalEntitiess.map((row) => ({
			data: [
				catalogState?.IdentificationType?.find(x => x.id === row.tipoIdentificacion)?.descripcion || 'N/A',
				row.nit || '',
				row.razonSocial || '',
				row.nombreCiudad || '',
				row.telefono || '',
				row.correoElectronico || '',
				row.estado ? 'ACTIVO' : 'INACTIVO',
			]
		}));

		const request = {
			columnDataList: columnDataList.map(item => ({
				data: item.data
			})),
			columnName: [
				"Tipo de Identificación",
				"Número de Identificación",
				"Razón Social",
				"Ciudad",
				"Teléfono",
				"Correo Electrónico",
				"Estado"
			]
		};

		const apiReportes = ApiReportes();

		const user = AuthService.getUserInfo();
		const nav = window.navigator as any;
		const usrAg = nav.userAgentData.brands.find(
			(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
		)?.brand;
		const auditoriaApi = AuditoriaApi();

		const requestAuditoriaBase = {
			navegador: usrAg,
			sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
			nombreUsuario: user?.USR_USERNAME || 'Desconocido',
			usurioId: user?.USR_ID || 0,
			ip: publicIpAddress,
			hostname: document.location.host,
			modulo: 'Entidades Educativas',
		};


		try {
			const response = await apiReportes.GenerarExcels(request);

			const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
			const url = window.URL.createObjectURL(blob);
			const a = document.createElement('a');
			a.href = url;
			a.download = 'archivo.xlsx';
			document.body.appendChild(a);
			a.click();
			document.body.removeChild(a);
			window.URL.revokeObjectURL(url);

			const descripcionAuditoria = `Descarga de reporte de entidades educativas. Columnas exportadas:
				- ${request.columnName.join(' | ')}
				Cantidad de registros: ${request.columnDataList.length}`;

			const requestAuditoria: Auditoria = {
				...requestAuditoriaBase,
				tipoAccion: 'DESCARGA',
				idTipoAccion: 5,
				descripcion: descripcionAuditoria,
			};

			auditoriaApi.InsertAuditoria(requestAuditoria);

		} catch (error) {
			const descripcionError = `Error al descargar el reporte de entidades educativas.
				- Error: ${error?.message || 'Error desconocido'}
				- Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
				- Código HTTP: ${error?.response?.status || 'No disponible'}`;

			const requestAuditoriaError: Auditoria = {
				...requestAuditoriaBase,
				tipoAccion: 'ERRORES Y EXCEPCIONES',
				idTipoAccion: 8,
				descripcion: descripcionError,
			};

			auditoriaApi.InsertAuditoria(requestAuditoriaError);
			toast('Error al descargar el reporte. Verifica la respuesta del servidor.', { type: toast.TYPE.ERROR });
		}
	};

	return (
		<CHPrivateContainer>
			<LoadingComponent
				key="loader"
				loading={educationalEntitiesState.loading}
			/>
			<CHPageTitle2
				topText="Gestión de afiliados"
				topText2="Gestión de entidades"
				title1="Gestión de entidades"
				title2="educativas"
				icono="school"
			/>
			<CHPaperNew>
				<Grid container>
					<Grid
						className={`item-containerForm ${classes.marginInput}`}
						md={3}
					>
						<Grid className="subcontainerForm" item>
							<Grid container>
								<Controller
									name="tipoIdentificacion"
									control={control}
									render={({ field: { onChange, value } }) => (
										<CHSelect
											defaultValue="0"
											label="Tipo de Identificación"
											value={value}
											onChange={onChange}
										>
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
						</Grid>
					</Grid>
					<Grid
						className={`item-containerForm ${classes.marginInput}`}
						md={3}
					>
						<Grid className="subcontainerForm">
							<Controller
								name="nit"
								control={control}
								rules={{
									maxLength: {
										value: 12,
										message:
											'La longitud máxima del campo es de 12 caracteres',
									},
									pattern: {
										value: /(^[0-9]+-?[0-9]{1})$/g,
										message: 'Ingrese un número de documento válido',
									},
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										InputLabelProps={{ shrink: true }}
										error={!!error}
										helperText={error ? error.message : null}
										label='Numero de documento'
										value={value}
										onChange={(e) => {
											let newValue = e.target.value.replace(/\D/g, '');
											if (newValue.length > 11) {
												newValue = newValue.slice(0, 11);
											}
											onChange(newValue);

										}} />
								)}
							/>
						</Grid>
					</Grid>
					<Grid
						className={`item-containerForm ${classes.marginInput}`}
						md={3}
					>
						<Grid className="subcontainerForm">
							<Controller
								name="razonSocial"
								control={control}
								render={({ field: { onChange, value } }) => (
									<CHTextField
										label="Nombre entidad educativa"
										onChange={onChange}
										value={value}
										InputLabelProps={{ shrink: true }}
									/>
								)}
							/>
						</Grid>
					</Grid>
					<Grid className="buttons" md={12}>
						<Grid className="" md={3}>
							<Button
								className={classes.button}
								onClick={handleSubmit(
									submitEntidadesEducativasFilter,
								)}
								variant="outlined">
								CONSULTAR
							</Button>
						</Grid>
						<Grid className="" md={3}>
							<Button
								className={classes.button}
								onClick={() =>
									history.push('/AgregarEntidadEducativa')
								}
								variant="outlined"
							>
								AGREGAR ENTIDAD
							</Button>
						</Grid>
						<Grid md={3}>
							<Button className={classes.button} onClick={handleSubmit(onClickExcel)} variant="outlined" >
								REPORTE
							</Button>
						</Grid>
						<Grid className="" md={3}>
							<Button
								className={classes.button}
								onClick={handleSubmit(
									handleClear,
								)}
								variant="outlined">
								LIMPIAR
							</Button>
						</Grid>
					</Grid>
				</Grid>
			</CHPaperNew>
			<EducationalEntitiesTable
				data={educationalEntitiess}
				catalogState={catalogState}
				isDataLoaded={isDataLoaded}
				startDeleteEntidadEducativa={startDeleteEntidadEducativa}
				submitEntidadesEducativasFilter={submitEntidadesEducativasFilter}
			/>
		</CHPrivateContainer>
	);
}

const mapStateToProps = (state: AppState) => ({
	catalogState: state.catalog,
	loginState: state.login,
	educationalEntitiesState: state.educationalEntities,
});

const mapDispatchToProps = (dispatch: any) => ({
	startGetAllIdentificationTypes: (props: string) =>
		dispatch(startGetAllIdentificationTypes(props)),
	startGetAllDepartments: () =>
		dispatch(startGetAllDepartments()),
	startGetAllCities: () =>
		dispatch(startGetAllCities()),
	startGetAllValoresCatalogoPorNombre: (nombreCatalogo: string) =>
		dispatch(startGetAllValoresCatalogoPorNombre(nombreCatalogo)),
	startDeleteEntidadEducativa: (idEntidad: string) =>
		dispatch(startDeleteEntidadEducativa(idEntidad)),
});

export default connect(mapStateToProps, mapDispatchToProps)(educationalEntities);