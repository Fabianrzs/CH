import { Button, createStyles, Grid, makeStyles, MenuItem } from "@material-ui/core"
import React, { useEffect, useState } from "react"
import swal from 'sweetalert'
import { Controller, useForm } from "react-hook-form"
import { toast } from "react-toastify"
import { useLocation } from "react-router-dom"
import CHPageTitle from "../../../components/ch-page-title/CHPageTitle"
import CHPaper from "../../../components/ch-paper/CHPaper"
import CHPrivateContainer from "../../../components/ch-private-container/CHPrivateContainer"
import CHSelect from "../../../components/ch-select/CHSelect"
import history from '../../../utils/history.router';
import CHTextField from "../../../components/ch-text-field/CHTextField"
import { LoadingComponent } from "../../../components/loading/loading.component"
import CatalogApi from "../../../services/api-catalog.service"
import ModalBusquedaNombreRazonSocial from "./ModalBusquedaNombreRazonSocial"
import TablaIntervinienteBuscar from "./TablaIntervinienteBuscar"
import SolicitudApi from "../../../services/api-solicitud.service"
import NotificationService from "../../../services/notification.service"
import ApiUsuarios from "../../../services/api-grupoUsuarios.service"
import AuthService from "../../../services/auth.service"
import AuditoriaApi from '../../../services/api-auditorias.servise'
import { Auditoria } from "../../../types/Auditoria.model.type"

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '400px',
			marginLeft: '.5rem',
			backgroundColor: '#1C3F8B',
			color: '#FFFFFF',
			marginTop: '1%',
			'&:hover': {
				backgroundColor: '#FFFFFF !important',
				color: 'black !important'
			},
		},
		container: {
			maxHeight: 440,
		},
	}));

const IntervinienteInico = () => {
	const apiSolicitud = SolicitudApi();
	const apiCatalogo = CatalogApi();
	const apiUsuario = ApiUsuarios();
	const location:any = useLocation();
	const id = location?.state?.item === undefined ? location?.state?.data: location?.state?.item ;
	const usuario = AuthService.getUserInfo();
	
	const formularioInterviniente = useForm({
		defaultValues: {
			tipoDocumento: '',
			numeroDocumento: '',
			nombre: '',
			radicado: '',
			numeroSolicitud: '',
			tipoActor: '',
		}
	})

	const [mostrarTabla, setMostrarTabla] = React.useState(false)
	const [listaTipoIdentificacion, setListaTipoIdentificacion] = React.useState([])
	const [openUsuario, setOpenUsuario] = React.useState(false);
	const [loading, setLoading] = React.useState(false);
	const [datas, setDatas] = React.useState<any[]>([]);
	const [listaTipoActores, setListaTipoActores] = React.useState([])
	const [mensaje, setMensaje] = React.useState('')
	const [infoGrupoUsuario, setInfoGrupoUsuario] = React.useState<any[]>([])
	const [page, setPage] = useState(0);

	useEffect(() => {
		obtenerGrupoUsuarioPorId(usuario?.USR_ID);
		if(datas.length>0){
			setDatas(datas)
		}else{
			setMostrarTabla(false)
		}
	}, [ ])

	useEffect(() => {
		apiCatalogo
			.getAllTypeDocuments()
			.then(async response => {
				if (response) {
					if (response.data !== null) {
						setListaTipoIdentificacion(response.data)
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});

		apiSolicitud
			.GetConsultarIntervinientesActores()
			.then(async response => {
				if (response) {
					if (response.data !== null) {
						setListaTipoActores(response?.data)
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	}, [])

	useEffect(() => {
		
		const request = {
			tipoDocumento: 0,
			numeroDocumento: id?.itR_NUMERO_DOCUMENTO === undefined ? id?.numeroDocumento: id?.itR_NUMERO_DOCUMENTO,
			nombre: '',
			actorId: "00000000-0000-0000-0000-000000000000"
		}
		if((id && Object.entries(id).length === 0) || id === undefined){
			setMostrarTabla(false)	
		}else {
			obtenerPorFiltroActores(request)
			formularioInterviniente.setValue('numeroDocumento', id?.itR_NUMERO_DOCUMENTO === undefined?  id?.numeroDocumento: id?.itR_NUMERO_DOCUMENTO)
		}
	}, [])


	const obtenerGrupoUsuarioPorId = (idUsuario: string) => {
		apiUsuario
			.ObtenerGruposUsuarioPorId(idUsuario)
			.then(async response => {
				if (response) {
					if (response.data !== null) {
						setInfoGrupoUsuario(response?.data);
					}
				}
			})
			.catch(() => {
				toast('Error al consultar grupos de usuario', { type: toast.TYPE.ERROR });
			});
	}

	const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
    const catalogApi  = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip:any) => {
        const  convertIp = ip.split('.')[0]
       const  hexadecimal = convertIp
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

	const obtenerPorFiltroActores = async (data: any) => {
		const request = {
			tipoDocumento: data?.tipoDocumento === '' || data?.tipoDocumento === undefined ? 0 : data.tipoDocumento,
			numeroDocumento: data?.numeroDocumento === '' || data?.numeroDocumento === undefined ? '' : data?.numeroDocumento,
			nombre: data?.nombre === '' || data?.nombre === undefined ? '' : data?.nombre,
			radicado: data?.radicado || '',
			numSolicitud: Number(String(data?.numeroSolicitud).trim()) || undefined,
			actorId: data?.tipoActor || "00000000-0000-0000-0000-000000000000"
		};
	
		const auditoriaApi = AuditoriaApi();
		const user = AuthService.getUserInfo();
		const nav = window.navigator as any;
		const usrAg = nav.userAgentData.brands.find(
			(x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
		)?.brand;
	
		let descripcionTipoDoc = 'No especificado';
	
		switch (request.tipoDocumento) {
			case 1: descripcionTipoDoc = 'CÉDULA DE CIUDADANÍA'; break;
			case 2: descripcionTipoDoc = 'NÚMERO DE IDENTIFICACIÓN TRIBUTARIA'; break;
			case 3: descripcionTipoDoc = 'CÉDULA DE EXTRANJERÍA'; break;
			case 4: descripcionTipoDoc = 'TARJETA DE IDENTIDAD'; break;
			case 5: descripcionTipoDoc = 'NÚMERO UNICO DE IDENTIFICACIÓN'; break;
			case 6: descripcionTipoDoc = 'PASAPORTE'; break;
			case 7: descripcionTipoDoc = 'REGISTRO CIVIL'; break;
			case 8: descripcionTipoDoc = 'CÓDIGO JUZGADO'; break;
			case 9: descripcionTipoDoc = 'CÓDIGO MILITAR'; break;
			default: descripcionTipoDoc = 'No especificado'; break;
		}
	
		try {
			const response = await apiSolicitud.PostBuscarIntervinientesActores(request);
	
			if (response?.data?.length > 0) {
				setDatas(response.data);
				setMostrarTabla(true);
				setPage(0);
			} else {
				setMostrarTabla(false);
				setMensaje('No se encontraron resultados');
			}
	
			
			const requestAuditoria: Auditoria = {
				navegador: usrAg,
				sistemaOperativo: nav.userAgentData.platform,
				nombreUsuario: user.USR_USERNAME,
				usurioId: user.USR_ID,
				ip: publicIpAddress,
				modulo: 'Gestión de Intervinientes',
				hostname: document.location.host,
				tipoAccion: 'CONSULTAS',
				idTipoAccion: 3,
				descripcion: `Consulta de intervinientes con los siguientes filtros:
				- Tipo de documento: ${descripcionTipoDoc}
				- No. Documento: ${request.numeroDocumento || 'No especificado'}
				- Nombre/Razón Social: ${request.nombre || 'No especificado'}
				- Radicado: ${request.radicado || 'No especificado'}
				- Número Solicitud: ${request.numSolicitud || 'No especificado'}
				- Tipo Actor: ${request.actorId || 'No especificado'}`
			};
	
			auditoriaApi.InsertAuditoria(requestAuditoria);
	
		} catch (error: any) {
			
			const descripcionError = `Error al consultar intervinientes.
			- Error: ${error?.message || 'Error desconocido'}
			- Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
			- Código HTTP: ${error?.response?.status || 'No disponible'}`;
	
			const requestAuditoriaError: Auditoria = {
				navegador: usrAg,
				sistemaOperativo: nav.userAgentData?.platform,
				nombreUsuario: user?.USR_USERNAME,
				usurioId: user?.USR_ID,
				ip: publicIpAddress,
				modulo: 'Gestión de Intervinientes',
				hostname: document.location.host,
				tipoAccion: 'ERRORES Y EXCEPCIONES',
				idTipoAccion: 8,
				descripcion: descripcionError
			};
	
			auditoriaApi.InsertAuditoria(requestAuditoriaError);
			toast('Error al consultar intervinientes', { type: toast.TYPE.ERROR });
			setMostrarTabla(false);
		}
	};
	

	const eliminarInterviniente = (id: string) => {
		swal({
            text: '¿Está seguro que desea eliminar el registro?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                eliminarIntervinientePorId(id);
            }
        });
	}

	const eliminarIntervinientePorId = async (id: string) => {
		const request =
		{
			intervinientesId: id
		}

		await apiSolicitud
			.PostEliminarIntervinientePorId(request)
			.then(async response => {
				if (response?.isSuccess === true && response?.returnMessage === 'Registro eliminado con éxito') {
					const requestConsultarIntervinientes = {
						tipoDocumento: 0,
						numeroDocumento: '',
						nombre: '',
						actorId: "00000000-0000-0000-0000-000000000000"
					}
					await obtenerPorFiltroActores(requestConsultarIntervinientes);
					await NotificationService.genericAlert(
						'Exitoso!',
						'Se realizó la petición con éxito.',
					);
				} 
			})
			.catch(() => {
				toast('Error al eliminar interviniente', { type: toast.TYPE.ERROR });
			});
	}

	const openModalUsuarioIngreso = () => {
		setOpenUsuario(true);
		formularioInterviniente.setValue('nombre', '')
	};

	const buscarUsuarioIngreso = (datos: any) => {
		setLoading(true)
		if (datos.nombreS_USUARIO !== undefined && datos.apellidoS_USUARIO !== undefined) {
			formularioInterviniente.setValue('nombre', `${datos?.nombreS_USUARIO} ${datos?.apellidoS_USUARIO}`)
			setLoading(false)
			setOpenUsuario(false);
		} else {
			formularioInterviniente.setValue('nombre', "")
			setLoading(false)
			setOpenUsuario(false);
		}
	}

	const handleCloseUsuario = () => {
		setOpenUsuario(false);
	};

	const limpiarCampos = () => {
		formularioInterviniente.reset()
		setMostrarTabla(false)
		setDatas([])
	}
	const classes = useStyles();

	return (
		<>
			<CHPrivateContainer>
				<LoadingComponent key="loader" loading={loading} />
				<CHPageTitle
					topText="Intervinientes"
					subText=""
					title1="Gestión de"
					title2="Intervinientes "
				/>
				<CHPaper title="Información Interviniente">
					<Grid container>
						<Grid className={classes.column} item xs={12} md={4}>
							<Controller
								name="tipoDocumento"
								control={formularioInterviniente.control}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHSelect
										label="Tipo de Documento"
										value={value}
										onChange={value => {
											onChange(value)
										}}
									>
										{listaTipoIdentificacion?.map(
											(option: any) => (
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
						<Grid className={classes.column} item xs={12} md={4}>
							<Controller
								name="numeroDocumento"
								control={formularioInterviniente.control}
								rules={{
									minLength: {
										value: 4,
										message:
											'La longitud mínima del campo es de 4 caracteres',
									},
									maxLength: {
										value: 30,
										message:
											'La longitud máxima del campo es de 30 caracteres',
									},
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										error={!!error}
										helperText={error ? error.message : null}
										isReadOnly={false}
										InputLabelProps={{ shrink: true }}
										label="No. Documento"
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
								name="nombre"
								control={formularioInterviniente.control}
								rules={{
									minLength: {
										value: 4,
										message:
											'La longitud mínima del campo es de 4 caracteres',
									},
									maxLength: {
										value: 30,
										message:
											'La longitud máxima del campo es de 30 caracteres',
									},
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										error={!!error}
										helperText={error ? error.message : null}
										InputLabelProps={{ shrink: true }}
										label="Nombre/Razon Social"
										value={value}
										onChange={e => {
											if(e.target.value.length <= 30){
												onChange(e)
											}
										}}
									/>
								)}
							/>
						</Grid>
						<Grid className={classes.column} item xs={12} md={4}>
							<Controller
								name="radicado"
								control={formularioInterviniente.control}
								rules={{
									minLength: {
										value: 4,
										message:
											'La longitud mínima del campo es de 4 caracteres',
									},
									maxLength: {
										value: 30,
										message:
											'La longitud máxima del campo es de 30 caracteres',
									},
								}}
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
										inputProps={{
											style: {
												textTransform: 'uppercase',
											},
										}}
										onChange={e => {
											onChange(e)
										}}
									/>
								)}
							/>
						</Grid>

						<Grid className={classes.column} item xs={12} md={4}>
							<Controller
								name="numeroSolicitud"
								control={formularioInterviniente.control}
								rules={{
									minLength: {
										value: 4,
										message:
											'La longitud mínima del campo es de 4 caracteres',
									},
									maxLength: {
										value: 30,
										message:
											'La longitud máxima del campo es de 30 caracteres',
									},
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										error={!!error}
										helperText={error ? error.message : null}
										isReadOnly={false}
										InputLabelProps={{ shrink: true }}
										label="No. Solicitud"
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
								name="tipoActor"
								control={formularioInterviniente.control}
								render={({
									field: { onChange, value },
								}) => (
									<CHSelect
										onChange={onChange}
										value={value}
										label="Tipo Actor"
									>
										{listaTipoActores?.map(
											(tipoActor: any) => (
												<MenuItem
													key={
														tipoActor?.itA_ID
													}
													value={
														tipoActor?.itA_ID
													}
												>
													{
														tipoActor?.itA_NOMBRE
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
						onClick={formularioInterviniente.handleSubmit(obtenerPorFiltroActores)}
					>
						CONSULTAR
					</Button>

					<Button
						className={classes.button}
						variant="outlined"
						onClick={() => history.push('/AgregarNuevoActor')}
					>
						Nuevo Actor
					</Button>

					<Button
						className={classes.button}
						variant="outlined"
						onClick={limpiarCampos}
					>
						Limpiar Campos
					</Button>
				</div>
				{
					mostrarTabla  ? 
					<TablaIntervinienteBuscar 
						data={datas} 
						listaTipoIdentificacion={listaTipoIdentificacion} 
						listaTipoActores={listaTipoActores} 
						infoGrupoUsuario={infoGrupoUsuario}
						eliminarInterviniente={eliminarInterviniente}
						page={page}
						setPage={setPage}
					/> : <span style={{marginTop:'30px'}}>{mensaje}</span>
				}
			</CHPrivateContainer>
			<ModalBusquedaNombreRazonSocial
				open={openUsuario}
				onClose={handleCloseUsuario}
				buscarUsuarioIngreso={buscarUsuarioIngreso} />
		</>
	)
}
export default IntervinienteInico