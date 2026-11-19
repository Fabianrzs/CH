import React, { useEffect, useState } from 'react';
import * as XLSX from 'xlsx';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import { Button, createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import moment from 'moment';
import AuthService from '../../services/auth.service';
import { AppState } from '../../store/rootReducer';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../components/ch-paper-new/CHPaperNew';
import history from '../../utils/history.router';
import { startGetTablasAuditoria, startGetLogAuditoria, SetLogAuditoriaDetalle } from '../../actions/auditoria.action';
import { ConsultaAuditoria, Auditoria, PaginationTable, TipoAccion, AuditoriaExcel } from '../../types/Auditoria.model.type';
import { AuditoriaState } from '../../reducers/auditoria.reducer';
import TableAuditoria from './components/tablaauditoria';
import { UserState } from '../../reducers/user.reducer';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHSelect from '../../components/ch-select/CHSelect';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import ApiUser from '../../services/api-login.service';
import CHAutoComplete from '../../components/ch-auto-complete/CHAutoComplete';
import { startGetAllUsers } from '../../actions/catalog.action';
import { LoginState } from '../../reducers/login.reducer';
import { CatalogState } from '../../reducers/catalog.reducer';
import CatalogApi from '../../services/api-catalog.service';
import AuditoriaApi from '../../services/api-auditorias.servise';

const useStyles = makeStyles(() =>
	createStyles({

		root: {
			flexGrow: 1,
		},
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '400px',
			marginLeft: '.5rem',
		},
		buttonRegresar: {
			width: '120px',
		},
		processButton: {
			backgroundColor: '#1C3F8B',
			color: '#fff',
			textAlign: 'right',
			marginRight: '-100px'
		},
		margin: {
			marginLeft: '-100px',
			marginRight: '-90px'
		},
		marginBotonRegresar: {
			textAlign: 'end',
			marginLeft: '-90px',
			marginRight: '-90px'
		}
	}),
);


const LogAuditoria = (props: Props) => {

	const classes = useStyles();
	const { auditoriaState, startGetAllUsers, startGetTablasAuditoria, startGetLogAuditoria, SetLogAuditoriaDetalle, usersState, catalogState, loginState } = props;
	const [userValue, setUserValue] = React.useState<any | null>(null);

	const acciones: TipoAccion[] = [
		{
			id: 0,
			nombre: 'TODAS'
		},
		{
			id: 1,
			nombre: 'INICIO DE SESIÓN'
		},
		{
			id: 2,
			nombre: 'TRANSACCIONES'
		},
		{
			id: 3,
			nombre: 'CONSULTAS'
		},
		{
			id: 4,
			nombre: 'MODIFICACIONES'
		},
		{
			id: 5,
			nombre: 'DESCARGAS'
		},
		{
			id: 6,
			nombre: 'GUARDAR'
		},
		{
			id: 7,
			nombre: 'ELIMINAR'
		},
		{
			id: 8,
			nombre: 'ERRORES Y EXCEPCIONES'
		},
		{
			id: 9,
			nombre: 'VALIDACION'
		},
	];

	useEffect(() => {
		if (catalogState.users.length === 0)
			startGetAllUsers(loginState.claims);
	});
	useEffect(() => {
		setLoading(false);
	}, []);

	const onChangeUser = (event: any, newValue: any | null) => {
		setUserValue(newValue);
	};

	const changePaginationComponente = (data: PaginationTable) => {

		const request = {
			VerDetalle: false,
			PageNum: data.PageNum,
			PageSize: data.Pagesize,
		} as ConsultaAuditoria;

		startGetLogAuditoria(request);

	}

	const setDetalle = (auditortia: Auditoria) => {
		SetLogAuditoriaDetalle(auditortia);
		history.push("/DetalleAuditoria");
	};

	const { control, handleSubmit, setValue, getValues, watch } = useForm({
		defaultValues: {
			nombreUsuario: '',
			tipoAccion: '',
			fechaInicio: null,
			fechaFin: null,
			descripcion: '',
			modulo: ''
		},
	});

	const handleCleanFilters = () => {
		setUserValue(null);
		setValue('modulo', '');
		setValue('descripcion', '');
		setValue('tipoAccion', '');
		setValue('fechaInicio', null);
		setValue('fechaFin', null);
	}

	const user = AuthService.getUserInfo();
	const [mostrarTabla, setMostrarTabla] = useState<boolean>(false);
	const [isLoading, setLoading] = useState(false);

	const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
	const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
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
						setLocalIpAddress(ip);
						enviarIp(ip);
					} else {
						setLocalIpAddress(ip);
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

	const descripcionData = watch('descripcion');
	const moduloData = watch('modulo');

	const handleSearch = (data: any) => {
		const usuario = userValue === null ? '' : userValue.nombreUsuario;
		const modulo = getValues('modulo');
		const descripcion = getValues('descripcion');
		const rawTipo = getValues('tipoAccion');
		const tipoAccion = rawTipo === '0' ? null : String(rawTipo);
		const fechaIni = getValues('fechaInicio') === '' ? null : getValues('fechaInicio');
		const fechaFin = getValues('fechaFin') === null ? null : getValues('fechaFin');
		const request = {
			VerDetalle: false,
			PageNum: data.PageNum,
			PageSize: data.Pagesize,
			Usuario: usuario,
			TipoAccion: tipoAccion,
			FechaInicio: fechaIni,
			FechaFin: fechaFin,
			descripcion: descripcion,
			modulo: modulo
		} as unknown as ConsultaAuditoria;
		console.log('request', request);

		const auditoriaApi = AuditoriaApi();
		const user = AuthService.getUserInfo();
		const nav = window.navigator as any;
		const usrAg = nav.userAgentData.brands.find(
			(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
		)?.brand;

		let descripcionTipoAccion = 'No especificado';

		if (request.TipoAccion === '1') {
			descripcionTipoAccion = 'INICIO DE SESIÓN'
		} else if (request.TipoAccion === '2') {
			descripcionTipoAccion = 'TRANSACCIONES'
		} else if (request.TipoAccion === '3') {
			descripcionTipoAccion = 'CONSULTAS'
		} else if (request.TipoAccion === '4') {
			descripcionTipoAccion = 'MODIFICACIONES'
		} else if (request.TipoAccion === '5') {
			descripcionTipoAccion = 'DESCARGAS'
		} else if (request.TipoAccion === '6') {
			descripcionTipoAccion = 'GUARDAR'
		} else if (request.TipoAccion === '7') {
			descripcionTipoAccion = 'ELIMINAR'
		}

		const requestAuditoria = {
			navegador: usrAg,
			sistemaOperativo: nav.userAgentData.platform,
			nombreUsuario: user.USR_USERNAME,
			usurioId: user.USR_ID,
			ip: publicIpAddress,
			modulo: 'Auditoria',
			hostname: document.location.host,
			tipoAccion: 'CONSULTAS',
			idTipoAccion: 3,
			descripcion: `Consulta de Auditoria con los siguientes filtros:
            - Usuario: ${request.Usuario || 'No especificado'}
            - Tipo Acción: ${descripcionTipoAccion || 'No especificada'}
            - Fecha Inicial: ${request.FechaInicio || 'No especificada'}
			- Fecha Final: ${request.FechaFin || 'No especificada'}
			- Descripción: ${descripcionData || 'No especificada'}
			- Módulo: ${moduloData || 'No especificado'}`,
		} as Auditoria;

		auditoriaApi.InsertAuditoria(requestAuditoria);



		setLoading(true)
		setTimeout(() => {
			startGetLogAuditoria(request);
			setLoading(false);
			setMostrarTabla(true)
		}, 2500);
	};

	const aExcel = () => {
		const usuario = userValue === null ? '' : userValue.nombreUsuario;
		const modulo = getValues('modulo');
		const descripcion = getValues('descripcion');
		const tipoAccion = getValues('tipoAccion');
		const fechaIni = getValues('fechaInicio') === '' ? null : getValues('fechaInicio');
		const fechaFin = getValues('fechaFin') === null ? null : getValues('fechaFin');
		const request = {
			VerDetalle: false,
			PageNum: 10,
			PageSize: 10,
			Usuario: usuario,
			TipoAccion: tipoAccion,
			FechaInicio: fechaIni,
			FechaFin: fechaFin,
			descripcion: descripcion,
			modulo: modulo
		} as unknown as ConsultaAuditoria;
		startGetLogAuditoria(request);
		const auditoriaExcel: any = [];
		auditoriaState.logs.auditorias.forEach((item) => {
			const a = {
				IdEvento: item.id,
				Fecha: moment(item.fechaRegistro).format("DD-MM-YYYY HH:MM:ss"),
				Accion: item.tipoAccion,
				Descripcion: item.descripcion,
				Modulo: item.modulo,
				Campo: item.nombreColumna,
				ValorAnterior: item.valorAnterior,
				ValorNuevo: item.valorNuevo,
				Usuario: item.nombreUsuario,
				Hostname: item.hostname,
				Ip: item.ip,
				Navegador: item.navegador,
				SistemaOperativo: item.sistemaOperativo
			} as AuditoriaExcel
			auditoriaExcel.push(a);
		})
		const tipoArchivo = 'application/vnd.openxmlformats-officedocument-spreadsheetml.sheet;charset-UTF-8';
		const ws = XLSX.utils.json_to_sheet(auditoriaExcel);
		const wb = { Sheets: { 'data': ws }, SheetNames: ['data'] };
		const excelBuffer = XLSX.write(wb, { bookType: 'xlsx', type: 'array' })

		const blob = new Blob([excelBuffer], { type: tipoArchivo })
		const url = URL.createObjectURL(blob);
		const link = document.createElement('a');
		link.download = "Log audot.xlsx"
		link.href = url;
		link.click();
	};
	const aTxt = async () => {
		const usuario = userValue === null ? '' : userValue.nombreUsuario;
		const modulo = getValues('modulo');
		const descripcion = getValues('descripcion');
		const tipoAccion = getValues('tipoAccion');
		const fechaIni = getValues('fechaInicio') === '' ? null : getValues('fechaInicio');
		const fechaFin = getValues('fechaFin') === null ? null : getValues('fechaFin');
		const request = {
			VerDetalle: false,
			PageNum: 10,
			PageSize: 10,
			Usuario: usuario,
			TipoAccion: tipoAccion,
			FechaInicio: fechaIni,
			FechaFin: fechaFin,
			descripcion: descripcion,
			modulo: modulo
		} as unknown as ConsultaAuditoria;
		await startGetLogAuditoria(request);
		const tipoArchivo = 'text/plain';
		const fileData = JSON.stringify(auditoriaState.logs.auditorias);
		const blob = new Blob([fileData], { type: tipoArchivo })
		const url = URL.createObjectURL(blob);
		const link = document.createElement('a');
		link.download = "consulta.json"
		link.href = url;
		link.click();
	}

	return (
		<div className={classes.root}>
			<LoadingV2Component key="loader" loading={isLoading} tittle='Consultando...' />
			<PageIndicators
				topTextNivel1='Auditoria'
				topTextNivel2='Log Auditoria' />

			<IconTitle
				subTitle='Auditoria'
				title='Log'
				icon='fch-Administracion-Grupos-Usuarios-0-1'
			/>

			<CHPaper title="Filtros">
				<Grid container>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="nombreUsuario"
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHAutoComplete
									value={userValue}
									options={catalogState.users}
									getOptionLabel={option => option.nombreUsuario}
									onChange={onChangeUser}
									renderInput={params => (
										<CHTextField
											{...params}
											label="Usuario"
											error={!!error}
											helperText={
												error ? error.message : null
											}
										/>
									)}
								/>
							)}
						/>
					</Grid>

					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="tipoAccion"
							control={control}
							render={({ field: { onChange, value } }) => (
								<CHSelect
									defaultValue="0"
									label="Tipo Acción"
									value={value}
									onChange={(e) => onChange(String(e.target.value))}
								>
									{
										acciones.map((item: TipoAccion) => (
											<MenuItem key={item.id} value={item.id}>
												{item.nombre}
											</MenuItem>
										))
									}
								</CHSelect>
							)}
						/>
					</Grid>

					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fechaInicio"
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHDatePicker

									error={!!error}
									InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
									helperText={error ? error.message : null}
									label="Fecha Inicial"
									value={value}
									onChange={onChange}

								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="fechaFin"
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHDatePicker
									InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
									error={!!error}
									helperText={error ? error.message : null}
									label="Fecha Final"
									value={value}
									onChange={onChange}

								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="descripcion"
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly={false}
									InputLabelProps={{ shrink: true }}
									label="Descripción"
									value={value}
									onChange={e => {
										onChange(e)
									}}
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="modulo"
							control={control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly={false}
									InputLabelProps={{ shrink: true }}
									label="Módulo"
									value={value}
									onChange={e => {
										onChange(e)
									}}
								/>
							)}
						/>
					</Grid>

				</Grid>

				<Button
					className={classes.processButton}
					variant="outlined"
					onClick={handleSubmit(handleCleanFilters)}
				>
					QUITAR FILTROS
				</Button>
			</CHPaper>
			<div className="buttons">
				<Button
					className={classes.button}
					variant="outlined"
					onClick={handleSubmit(handleSearch)}
				>
					CONSULTAR
				</Button>
				<Button
					className={classes.button}
					variant="outlined"
					onClick={handleSubmit(aTxt)}
				>
					EXPORTAR A TXT
				</Button>
				<Button
					className={classes.button}
					variant="outlined"
					onClick={handleSubmit(aExcel)}
				>
					EXPORTAR A EXCEL
				</Button>
			</div>
			<br />

			{
				mostrarTabla &&
				<CHPaperNew>
					<TableAuditoria
						setDetalle={setDetalle}
						log={auditoriaState.logs}
						changePagination={changePaginationComponente}
						usersState={usersState}
					/>
				</CHPaperNew>

			}
		</div>

	);
}


interface LinkMapStateToProps {
	auditoriaState: AuditoriaState;
	usersState: UserState;
	loginState: LoginState;
	catalogState: CatalogState;
}

const mapStateToProps = (state: AppState) => ({
	auditoriaState: state.auditoria,
	catalogState: state.catalog,
	usersState: state.users,
	loginState: state.login,
});

interface LinkMapDispatchToProps {
	startGetTablasAuditoria: () => void;
	startGetLogAuditoria: (auditoria: ConsultaAuditoria) => void;
	SetLogAuditoriaDetalle: (auditoria: Auditoria) => void;
	startGetAllUsers: (claims: string) => void;
}

const mapDispatchToProps = {
	startGetTablasAuditoria,
	startGetLogAuditoria,
	SetLogAuditoriaDetalle,
	startGetAllUsers
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(LogAuditoria);