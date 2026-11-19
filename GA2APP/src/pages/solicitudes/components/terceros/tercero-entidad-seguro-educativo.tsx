import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, IconButton, makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import { Controller, useForm} from 'react-hook-form';
import { toast } from 'react-toastify';
import { Delete, Edit } from '@material-ui/icons';
import CHPaper from "../../../../components/ch-paper/CHPaper";
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import { ENTIDADSEGUROEDUCATIVO } from '../../../../types/catalog.type';
import {startGetAllEntidadSeguroEducativo}  from '../../../../actions/catalog.action';
import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import AuthService from '../../../../services/auth.service';
import ApiCatalog from '../../../../services/api-catalog.service';
import NotificationService from '../../../../services/notification.service';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
		columnButtonAdd: {
			textAlign: 'right',
		},
	}),
);

function TerceroEntidadSeguroEducativo({
	responseSolicitud,
	listaEntidadSeguroEducativo,
	setEntidadAhorroProgramadoSeguroEducativo,
	siEsAprobadaSolcitud,
	onChangeEntidad,
	campoSoloLecturaRadicacionAprobada,
}: {
	responseSolicitud: ResponseSeveranceModel | undefined;
	listaEntidadSeguroEducativo: ENTIDADSEGUROEDUCATIVO[];
	setEntidadAhorroProgramadoSeguroEducativo: any;
	siEsAprobadaSolcitud: boolean;
	onChangeEntidad: (entidad: ENTIDADSEGUROEDUCATIVO) => void;
	campoSoloLecturaRadicacionAprobada: any;
}) {
	
	const classes = useStyles();
	const apiCatalogo = ApiCatalog();
	const [entidadRegistro, setEntidadRegistro] = useState<ENTIDADSEGUROEDUCATIVO | null>(null);
	const [entidadesEducativas, setEntidadesEducativas] = useState<any[]>([]);
	const [estadoBoton, setEstadoBoton] = useState<string>('AGREGAR');
	const [funcionCrear, setFuncionCrear] = useState<boolean>(false);
	const [deshabilitarBotonCrear, setDeshabilitarBotonCrear] = useState<boolean>(false);
	const [campoSoloLectura, setCampoSoloLectura] = useState<boolean>(true);
	const user = AuthService.getUserInfo();

	const formualarioEntidad = useForm({
		defaultValues: {
			entidadSeguroEducativo: '',
			numeroPolizaSeguro: '',
			numeroIdentificacion: '',
			nombreRazonSocial: '',
			nit: '',
		},
	})

	useEffect(() => {

		if (responseSolicitud?.solicitudTerceroEntidadSeguroEducativoDto !== null) {

			const registro = listaEntidadSeguroEducativo.find(x => x.idEntidadSeguroEducativo === responseSolicitud?.solicitudTerceroEntidadSeguroEducativoDto.entidadSeguroEducativoId);

			const data = responseSolicitud?.solicitudTerceroEntidadSeguroEducativoDto
			setEntidadesEducativas([{
				tipoIdentificacion: data?.tipoDocId,
				numeroIdentificacion: data?.numDoc,
				razonSocial: data?.razonSocialAseguradora,
				idEntidadSeguroEducativo: data?.entidadSeguroEducativoId,
				numeroPoliza: data?.numeroPoliza,
				nit: registro?.nit || ''
			}])
			setEntidadAhorroProgramadoSeguroEducativo([{
				tipoIdentificacion: data?.tipoDocId,
				numeroIdentificacion: data?.numDoc,
				razonSocial: data?.razonSocialAseguradora,
				idEntidadSeguroEducativo: data?.entidadSeguroEducativoId,
				numeroPoliza: data?.numeroPoliza,
				nit: registro?.nit || ''
			}])
		}
	}, []);

	const habilitarFuncionCrear = () => {
		cancelarEntidad();
		setFuncionCrear(true);
		setCampoSoloLectura(false);
		setDeshabilitarBotonCrear(true);
	}

	const onchangeEntidadSeguroEducativo = (_: any, newValue: any) => {

		if (newValue !== null) {
			const dato = `${newValue?.tipoIdentificacion}_${newValue?.numeroIdentificacion}_${newValue?.nombreRazonSocial}_${newValue?.idEntidadSeguroEducativo}`;
			formualarioEntidad.setValue("entidadSeguroEducativo", dato);
			formualarioEntidad.clearErrors("entidadSeguroEducativo");
			setDeshabilitarBotonCrear(true);
			if(newValue?.nit){
				setCampoSoloLectura(true);
				formualarioEntidad.setValue("nit", newValue?.nit);
			}else if(newValue?.nit !== undefined){
				formualarioEntidad.setValue("nit", '');
			}

		} else {
			setCampoSoloLectura(false);
			formualarioEntidad.setValue("entidadSeguroEducativo", '');
		}
		setEntidadRegistro(newValue);
		onChangeEntidad(newValue);
	};

	const agregarEntidad = async(value: any) => {
		

		setEstadoBoton('AGREGAR');

		if(funcionCrear){

			if(value?.nombreRazonSocial === ''){
				formualarioEntidad.setError('nombreRazonSocial', {
					type: 'custom',
					message: 'Este campo es requerido.',
				});
				return;
			}

			if(value?.nit === ''){
				formualarioEntidad.setError('nit', {
					type: 'custom',
					message: 'Este campo es requerido.',
				});
				return;
			}

			if(value?.numeroPolizaSeguro === ''){
				formualarioEntidad.setError('numeroPolizaSeguro', {
					type: 'custom',
					message: 'Este campo es requerido.',
				});
				return;
			}

			const entidadNueva = {
				tipoIdentificacion: 2,
				numeroIdentificacion: '',
				nit: value.nit,
				nombreRazonSocial: value.nombreRazonSocial.toUpperCase(),
				creadoPor: user?.USR_ID
			}
			await apiCatalogo.postInsertEntidaSeguroEducativo(entidadNueva)
			.then(async (response)=> {
				if(response.isSuccess){

					const request = [{
						tipoIdentificacion: 2,
						numeroIdentificacion: value.numeroIdentificacion,
						razonSocial: value.nombreRazonSocial.toUpperCase(),
						idEntidadSeguroEducativo: response?.data?.idEntidadSeguroEducativo,
						numeroPoliza: value.numeroPolizaSeguro,
						nit: value.nit
					}];
	
					setEntidadAhorroProgramadoSeguroEducativo(request);
					setEntidadesEducativas(request);
					cancelarEntidad();
				}
			})
			.catch((error: any)=> {
				if(error?.response?.data?.returnMessage === 'El NIT ya se encuentra registrado'){
					toast(`${error?.response?.data?.returnMessage}`, {
						type: toast.TYPE.ERROR,
					});
				}else{
					toast(`Error al agregar seguro educativo: ${error?.message}`, {
						type: toast.TYPE.ERROR,
					});
				}
			})

		}else if(!funcionCrear && value && value.entidadSeguroEducativo){

			if(value?.numeroPolizaSeguro === ''){
				formualarioEntidad.setError('numeroPolizaSeguro', {
					type: 'custom',
					message: 'Este campo es requerido.',
				});
				return;
			}

			const parts = value.entidadSeguroEducativo.split('_');

			if (parts.length >= 4) {

				const request = [{
					tipoIdentificacion: parts[0],
					numeroIdentificacion: parts[1],
					razonSocial: parts[2],
					idEntidadSeguroEducativo: parts[3],
					numeroPoliza: value.numeroPolizaSeguro,
					nit: value.nit
				}];

				setEntidadAhorroProgramadoSeguroEducativo(request)
				setEntidadesEducativas(request);
				cancelarEntidad();
			}
		}

	};

	const cancelarEntidad = () => {
		setEstadoBoton('AGREGAR');
		formualarioEntidad.reset();
		setEntidadRegistro(null);
		setFuncionCrear(false);
		setCampoSoloLectura(true);
		setDeshabilitarBotonCrear(false);
		formualarioEntidad.clearErrors();
	}

	const editar = (indice: any) => {

		setEstadoBoton('EDITAR');
		setDeshabilitarBotonCrear(true);
		const entidad = {
			tipoIdentificacion: indice.tipoIdentificacion,
			numeroIdentificacion: indice.numeroIdentificacion,
			nombreRazonSocial: indice.razonSocial,
			idEntidadSeguroEducativo: indice.idEntidadSeguroEducativo,

		};
		
		formualarioEntidad.setValue("numeroPolizaSeguro", String(indice?.numeroPoliza));
		formualarioEntidad.setValue("nit", indice?.nit);
		onchangeEntidadSeguroEducativo('_', entidad);
	}

	const eliminar = async(indice: any) => {
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);
		if(confirmAlert){
			setEstadoBoton('AGREGAR');
			const nuevasEntidades = [...entidadesEducativas];
			nuevasEntidades.splice(indice, 1);
			setEntidadesEducativas(nuevasEntidades);
			setDeshabilitarBotonCrear(false);
			formualarioEntidad.clearErrors();
			formualarioEntidad.reset();
			setEntidadRegistro(null);
		}
	};

	return (
		<>
			<CHPaper title="Entidad de ahorro programado o seguro educativo">
				{
					(siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) || campoSoloLecturaRadicacionAprobada ? '' :
						<Grid container>

							{!funcionCrear && (
								<Grid className={classes.column} item xs={12} md={3}>
									<Controller
										name="entidadSeguroEducativo"
										rules={{
											required: 'Este campo es requerido',
										}}
										control={formualarioEntidad.control}
										render={(
											{
												fieldState: { error },
											}) => (
											<CHAutoComplete
												value={entidadRegistro}
												options={listaEntidadSeguroEducativo}
												getOptionLabel={option => `${option?.nombreRazonSocial} ${option.numeroIdentificacion? ` - (${option.numeroIdentificacion})` : ''}`}
												onChange={onchangeEntidadSeguroEducativo}
												renderInput={(params) =>
													<CHTextField
														{...params}
														error={!!error}
														helperText={error ? error.message : null}
														label="Entidad de Seguro"
														InputLabelProps={{ shrink: true }}
													/>
												}
											/>
										)}
									/>
								</Grid>
							)}

							{funcionCrear && (
								<Grid className={classes.column} item xs={12} md={3}>
									<Controller
										name="nombreRazonSocial"
										control={formualarioEntidad.control}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												error={!!error}
												helperText={error ? error.message : null}
												isReadOnly={false}
												label="Nombre Razón Social *"
												value={value}
												InputLabelProps={{ shrink: true }}
												onChange={onChange}
												inputProps={{
													style: {
														textTransform: 'uppercase',
													},
													maxLength: 50
												}}
											/>
										)}
									/>
								</Grid>
							)}

							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="nit"
									control={formualarioEntidad.control}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={campoSoloLectura}
											label={estadoBoton === 'AGREGAR' ? "NIT *" : "NIT"} 
											value={value}
											InputLabelProps={{ shrink: true }}
											onChange={onChange}
											inputProps={{
												style: {
													textTransform: 'uppercase',
												},
												maxLength: 12
											}}
										/>
									)}
								/>
							</Grid>

							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="numeroPolizaSeguro"
									control={formualarioEntidad.control}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											required
											helperText={error ? error.message : null}
											isReadOnly={false}
											label="Número Póliza de Seguro"
											value={value}
											inputProps={{maxLength: 25}}
											InputLabelProps={{ shrink: true }}
											onChange={e => {
												onChange(
													e.target.value.replace(
														/[^a-zA-Z0-9_-]/g,
														'',
													)
												)

											}
											}
										/>
									)}
								/>
							</Grid>

							{
								(siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) || campoSoloLecturaRadicacionAprobada ? null :
									<Grid container>
										<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
											<Button
												className={classes.buttonAdd}
												variant="outlined"
												onClick={habilitarFuncionCrear}
												disabled={deshabilitarBotonCrear}
											>
												CREAR
											</Button>
											<Button
												className={classes.buttonAdd}
												variant="outlined"
												onClick={formualarioEntidad.handleSubmit(agregarEntidad)}
											>
												{estadoBoton}
											</Button>
											<Button
												className={classes.buttonAdd}
												variant="outlined"
												onClick={cancelarEntidad}
											>
												CANCELAR
											</Button>
										</Grid>
									</Grid>
							}
						</Grid>
				}

				<h3>Entidad de seguro educativo</h3>
				<TableContainer component={Paper}>
					<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
						<Table aria-label="simple table">
							<TableHead>
								<TableRow>
									<TableCell>Entidad de seguro educativo</TableCell>
									<TableCell>NIT</TableCell>
									<TableCell>Número de póliza</TableCell>
									{
										((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
											<TableCell align="center">Acciones</TableCell>
									}

								</TableRow>
							</TableHead>
							<TableBody>
								{entidadesEducativas.map(
									(option: any) => (
										<TableRow key={option.razonSocial}>
											<TableCell>{option.razonSocial}</TableCell>
											<TableCell>{option?.nit}</TableCell>
											<TableCell>{option.numeroPoliza}</TableCell>
											{
												((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
													<TableCell align="center">
														<IconButton aria-label="Editar" color="primary" onClick={() => {
															editar(option);
														}}>
															<Edit />
														</IconButton>
														<IconButton aria-label="Eliminar" color="secondary" onClick={() => {
															eliminar(option.id);
														}}>
															<Delete />
														</IconButton>
													</TableCell>
											}

										</TableRow>
									)
								)}
							</TableBody>
						</Table>
					</Grid>
				</TableContainer>

			</CHPaper>
		</>
	)
}

export default TerceroEntidadSeguroEducativo;