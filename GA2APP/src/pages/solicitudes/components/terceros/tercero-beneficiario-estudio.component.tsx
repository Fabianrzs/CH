import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import { Controller, useForm } from 'react-hook-form';
import { Delete, Edit } from '@material-ui/icons';
import { toast } from 'react-toastify';
import SearchIcon from '@material-ui/icons/Search';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHPaper from "../../../../components/ch-paper/CHPaper";
import GenerarGuid from '../funciones/guid.funtions';
import NotificationService from '../../../../services/notification.service';
import CreditoApi from '../../../../services/api-credit.service';
import { IDENTIFICATIONTYPEDTO, LISTABENEFICIARIOS, VALORCATALOGO } from '../../../../types/catalog.type';
import { TerceroBeneficiarioEstudioModel } from '../../../../models/tercero-beneficiario-estudio.model';
import { PE_ORIGEN, PE_PORCENTAJE, PE_TIPO_VERIFICACION, USER_SARLAFT_FOUND } from '../../../../constants/constants';
import ApiClients from '../../../../services/api-clients.service';
import ApiCatalogo from '../../../../services/api-catalog.service';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import ApiSolicitud from '../../../../services/api-solicitud.service';
import AuthService from '../../../../services/auth.service';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		column2: {
			padding: '10px',
			textAlign: 'left',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
		columnButtonAdd: {
			textAlign: 'right',
		},
		icon: {
			marginTop: '15px'
		}
	}),
);

function TerceroBeneficiarioEstudio({
	listaTipoDocumentos,
	listaParentesco,
	listaBeneficiarioEstudio,
	tercerosBeneficiarioEstudioAdicionados,
	obtenerParentesco,
	siEsAprobadaSolcitud,
	responseSolicitud,
	campoSoloLecturaRadicacionAprobada,
	onBeneficiariosChange,

}: {
	listaTipoDocumentos: IDENTIFICATIONTYPEDTO[];
	listaParentesco: VALORCATALOGO[];
	listaBeneficiarioEstudio: TerceroBeneficiarioEstudioModel[];
	tercerosBeneficiarioEstudioAdicionados: (terceros: any[]) => void;
	obtenerParentesco: (catalogoId: number) => void;
	siEsAprobadaSolcitud: boolean;
	responseSolicitud: any;
	campoSoloLecturaRadicacionAprobada: any;
	onBeneficiariosChange: (listaActualizada: TerceroBeneficiarioEstudioModel[]) => void;
}) {
	const classes = useStyles();
	const apiCredito = CreditoApi();
	const apiClientes = ApiClients();
	const apiCatalogo = ApiCatalogo();
	const apiSolicitud = ApiSolicitud();
	const [terceroBeneficiarioEstudioModel, setTerceroBeneficiarioEstudioModel] = useState<TerceroBeneficiarioEstudioModel[]>([]);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR BENEFICIARIO');
	const [numDocAnterior, setNumDocAnterior] = useState<string>('');
	const [idAnterior, setIdAnterior] = useState<string>('');
	const [tipoDeBeneficiario, setTipoDeBeneficiario] = useState('');
	const [listadoBeneficiariosEstudio, setListadoBeneficiariosEstudio] = useState<any[]>([]);
	const [listaParentescos, setListaParentescos] = useState<VALORCATALOGO[]>([]);
	const [idValor, setIdValor] = useState(listaParentescos[0]?.idValor || null);
	const [descripcion, setDescripcion] = useState(listaParentescos[0]?.descripcion || '');
	const [deshabilitarLupaBusqueda, setDeshabilitarLupaBusqueda] = useState(true);
	const [deshabilitarNombresBeneficiario, setDeshabilitarNombresBeneficiario] = useState(true);
	const user = AuthService.getUserInfo();

	const controlApoderado = useForm({
		defaultValues: {
			parentescoEstudioObj: '',
			tipoDocEstudioObj: '',
			numDocEstudio: '',
			primerNombreBeneficiario:'',
			segundoNombreBeneficiario:'',
			primerApellidoBeneficiario: '',
			segundoApellidoBeneficiario: '',
			razonsocialEstudio: '',
			listaBeneficiarios: '',
			identificacionConyuge: '',
			primerNombreConyuge: '',
			segundoNombreConyuge: '',
			primerApellidoConyuge: '',
			segundoApellidoConyuge: '',
			tipoDocHijo: '',
			numeroDocHijo: '',
			nombreHijo: '',
		},
	});

	useEffect(() => {
		consultarListaParentesco();
	}, []);

	const consultarListaParentesco = async () => {
		const PARENTESCO = 70;
		try {
			const response = await apiCatalogo.getObtenerValoresCatalogosPorIdCatalogo(PARENTESCO);
			if (response.data) {

				const listaParentescos = response.data.map((item: any) => ({
					idValor: item.idValor,
					descripcion: item.descripcion
				}));
				setListaParentescos(listaParentescos);
			}
		} catch (error) {
			toast('Error del sistema', { type: toast.TYPE.ERROR });
		}
	}

	useEffect(() => {
		obtenerParentesco(3);
		if (listaBeneficiarioEstudio !== null && listaBeneficiarioEstudio !== undefined) {
			if (listaBeneficiarioEstudio.length > 0) {
				setTerceroBeneficiarioEstudioModel(listaBeneficiarioEstudio);
				setListadoBeneficiariosEstudio(listaBeneficiarioEstudio);
			}
		}
	}, []);

	const cancelarTerceroApoderado = async () => {
		setDescEstadoBoton('AGREGAR BENEFICIARIO');
		controlApoderado.reset();
	}

	const limpiarBeneficiario = () => {
		controlApoderado.reset();
		setBeneficiarioEditando(null);
		setDescEstadoBoton('AGREGAR BENEFICIARIO');
		setTipoDeBeneficiario('');
	};

	const agregarTerceroApoderado = async (data: any) => {
		/**
		 * Verificamos en vigia-sarlaft si el usuario existe en ese lugar
		 */
		let canContinue = true;
		try {
			const sarlaftResponse = await apiCredito.PostVerificacionTerc({
				pe_Origen: PE_ORIGEN,
				pe_Dato: data.numDocEstudio.toString(),
				pe_TipoVerificacion: PE_TIPO_VERIFICACION,
				pe_Porcentaje: PE_PORCENTAJE
			})
			if (sarlaftResponse?.isSuccess && sarlaftResponse?.data) {
				canContinue = !((sarlaftResponse?.data?.encontrado as string) === USER_SARLAFT_FOUND.SI);
			}
		} catch (_) {
			toast('API-ERROR api de sarlaft no disponible', { type: toast.TYPE.ERROR });
		}
		if (canContinue) {
			const datoEncontrado = terceroBeneficiarioEstudioModel.findIndex(x => x.numDocEstudio === data.numDocEstudio && x.numDocEstudio !== numDocAnterior);
			if (datoEncontrado === -1) {
				const guid = GenerarGuid();
				
				const terceroNuevo = {
					id: guid.newGuid(),
					tipoDocEstudio: data.tipoDocEstudioObj.split('_')[0],
					tipoDocEstudioObj: {
						id: data.tipoDocEstudioObj.split('_')[0],
						descripcion: data.tipoDocEstudioObj.split('_')[1]
					},
					tipoIdentificacion: {
						id: data.tipoDocEstudioObj.split('_')[0],
						descripcion: data.tipoDocEstudioObj.split('_')[1]
					},
					parentescoEstudio: data.parentescoEstudioObj.split('_')[0] === '' ? null : data.parentescoEstudioObj.split('_')[0],
					parentescoEstudioObj: {
						id: data.parentescoEstudioObj.split('_')[0],
						descripcion: data.parentescoEstudioObj.split('_')[1]
					},
					numDocEstudio: data.numDocEstudio,
					numDoc: data.numDocEstudio,
					primerNombre: data.primerNombre || data.razonsocialEstudio || "",
					segundoNombre: data.segundoNombre || "",
					primerApellido: data.primerApellido || "",
					segundoApellido: data.segundoApellido || "",
					razonsocialEstudio: data.razonsocialEstudio,
					esAutorizado: data.esAutorizado,
					tipoBeneficiario: data.tipoBeneficiario
				};

				let terceros: TerceroBeneficiarioEstudioModel[] = [];
				if (descEstadoBoton === 'AGREGAR BENEFICIARIO') {
					setTerceroBeneficiarioEstudioModel(oldArray => [...oldArray, terceroNuevo]);
					terceros = [...terceroBeneficiarioEstudioModel];
					terceros.push(terceroNuevo);
					
				} else {
					const temp = [...terceroBeneficiarioEstudioModel];
					const posicionActual = temp.findIndex(x => x.numDocEstudio === numDocAnterior);
					temp.splice(posicionActual, 1);
					terceroNuevo.id = idAnterior;
					temp.splice(posicionActual, 0, terceroNuevo);
					terceros = temp;
					setTerceroBeneficiarioEstudioModel(temp);
				}
				tercerosBeneficiarioEstudioAdicionados(terceros);
				onBeneficiariosChange(terceros);
				setNumDocAnterior('');
				setIdAnterior('');
				await cancelarTerceroApoderado();
			} else {
				toast('Ya existe un apoderado con este mismo número de documento en la lista', { type: toast.TYPE.ERROR });
			}
		} else {
			toast(`La identificación ${data.numDocEstudio.toString()} se encuentra en lista de Sarlaft`, {
				type: toast.TYPE.ERROR,
			});
		}

	
	}

	const eliminarRegistroTercero = async (item: string) => {
		const confirmAlert = await NotificationService.confirmAlert('¿Eliminando registro?');
		if (confirmAlert) {
			const temp = [...terceroBeneficiarioEstudioModel];
			temp.splice(temp.findIndex(x => x.id === item), 1);
			setTerceroBeneficiarioEstudioModel(temp);
			tercerosBeneficiarioEstudioAdicionados(temp);
			controlApoderado.reset();
		}
	}

	const seleccionarRegistroTercero = async (item: TerceroBeneficiarioEstudioModel) => {
		setNumDocAnterior(item.numDocEstudio);
		setIdAnterior(item.id);
		controlApoderado.setValue("tipoDocEstudioObj", `${item.tipoDocEstudioObj.id}_${item.tipoDocEstudioObj.descripcion}`);
		if (item.parentescoEstudio != null) {
			controlApoderado.setValue("parentescoEstudioObj", `${item.parentescoEstudioObj.id}_${item.parentescoEstudioObj.descripcion}`);
		}
		controlApoderado.setValue("numDocEstudio", item.numDocEstudio);
		controlApoderado.setValue("razonsocialEstudio", item.razonsocialEstudio);
		setDescEstadoBoton('EDITAR BENEFICIARIO');
	}

	const [isLoading, setIsLoading] = useState(false);
	const [beneficiarioEditando, setBeneficiarioEditando] = useState<number | null>(null);

	const handleBeneficiaryChange = async (value: any) => {

		if (typeof value !== 'string' || !value.includes('_')) {
			return;
		}

		const [idValor, descripcion] = value.split('_');
		setIdValor(parseInt(idValor, 10));
		setDescripcion(descripcion);
		setIsLoading(true);
		setTipoDeBeneficiario(descripcion);

			if (descripcion === 'AFILIADO') {

				try {
					const response = await apiClientes.getClientByIdentificationTypeIdentificationNumberRequest(
						responseSolicitud?.tipoIdentificacion as unknown as number,
						responseSolicitud?.identificacionCliente,
		
					);
					
				const {
					idTipoIdentificacion,
					nombreTipoIdentificacion,
					identificacion,
					primerNombre,
					segundoNombre,
					primerApellido,
					segundoApellido
				} = response.data;
				const valorTipoDoc = `${idTipoIdentificacion}_${nombreTipoIdentificacion}`;
				setDeshabilitarNombresBeneficiario(true);
				controlApoderado.setValue('tipoDocEstudioObj', valorTipoDoc);
				controlApoderado.setValue('numDocEstudio', identificacion);
				controlApoderado.setValue('razonsocialEstudio', `${primerNombre || ''} ${segundoNombre || ''} ${primerApellido || ''} ${segundoApellido || ''}`);
				controlApoderado.setValue('primerNombreBeneficiario',primerNombre);
				controlApoderado.setValue('segundoNombreBeneficiario',segundoNombre || '');
				controlApoderado.setValue('primerApellidoBeneficiario',primerApellido);
				controlApoderado.setValue('segundoApellidoBeneficiario',segundoApellido || '');
				setIsLoading(false);	
			 }
			catch (error: any) {
				setIsLoading(false);	
				toast(`Error en la consulta de afiliado: ${error.message}`, { type: toast.TYPE.ERROR });
			}

			} else if (descripcion === 'CÓNYUGUE') {

				try {
					const response = await apiClientes.getClientByIdentificationTypeIdentificationNumberRequest(
						responseSolicitud?.tipoIdentificacion as unknown as number,
						responseSolicitud?.identificacionCliente,
		
					);

					setDeshabilitarNombresBeneficiario(true);
				if ((responseSolicitud?.timCodigo === 'REPAES' ||
					responseSolicitud?.timCodigo === 'AHOPROEDU') &&
					(response?.data?.identificacionConyuge === '' ||
					response?.data?.primerNombreConyuge === '' ||
					response?.data?.primerApellidoConyuge === '')) {
					controlApoderado.setValue('listaBeneficiarios','');
					NotificationService.genericAlert(
						'Advertencia',
						`Afiliado no tiene cónyuge registrado.`,
						'warning'
					);
					setIsLoading(false);
					return;
				}

				if (response?.data?.idTipoIdentificacionConyuge === 0) {
					controlApoderado.setValue('tipoDocEstudioObj', '1_CÉDULA DE CIUDADANÍA');
				} else if (response?.data?.idTipoIdentificacionConyuge !== 0) {
					const documento = listaTipoDocumentos.find(item => item?.id === response?.data?.idTipoIdentificacionConyuge);
					controlApoderado.setValue('tipoDocEstudioObj', `${documento?.id}_${documento?.descripcion}`);
				}
				controlApoderado.setValue('numDocEstudio', response?.data?.identificacionConyuge);
				controlApoderado.setValue('razonsocialEstudio', `${response?.data?.primerNombreConyuge} ${response?.data?.segundoNombreConyuge || ''} ${response?.data?.primerApellidoConyuge} ${response?.data?.segundoApellidoConyuge || ''}`);
				controlApoderado.setValue('primerNombreBeneficiario',response?.data?.primerNombreConyuge);
				controlApoderado.setValue('segundoNombreBeneficiario',response?.data?.segundoNombreConyuge || '');
				controlApoderado.setValue('primerApellidoBeneficiario',response?.data?.primerApellidoConyuge);
				controlApoderado.setValue('segundoApellidoBeneficiario',response?.data?.segundoApellidoConyuge || '');
				setIsLoading(false);	
				}
				catch (error: any) {
					setIsLoading(false);	
					toast(`Error en la consulta de conyugé: ${error.message}`, { type: toast.TYPE.ERROR });
				}

			} else if (descripcion === 'HIJO/HIJA') {

				setDeshabilitarNombresBeneficiario(true);
				setIsLoading(false);	
			}
	};


	const agregarBeneficiario = async(data: any) => {

		const TIPO_TERCERO_BENEFICIARIO_ESTUDIO = 401;
		const tipoDoc = listaTipoDocumentos.find(doc => doc.id === parseInt(data.tipoDocEstudioObj, 10));
		const tipoDocObj = tipoDoc
			? { id: String(tipoDoc.id), descripcion: tipoDoc.descripcion }
			: { id: 'N/A', descripcion: 'N/A' };

		const guid = GenerarGuid();

		if(data.primerNombreBeneficiario !== ''  && data.primerApellidoBeneficiario !== ''){
			data.razonsocialEstudio = `${data.primerNombreBeneficiario} ${data.segundoNombreBeneficiario || ''} ${data.primerApellidoBeneficiario} ${data.segundoApellidoBeneficiario}`
		}

		const nuevoBeneficiario: any = {
			id: guid.newGuid(),
			tipoBeneficiario: TIPO_TERCERO_BENEFICIARIO_ESTUDIO,
			tipoDocEstudioObj: tipoDocObj,
			numDocEstudio: data.numDocEstudio || 'N/A',
			razonsocialEstudio: data.razonsocialEstudio,
			parentescoEstudio: String(idValor),
			tipoDeBeneficiario: descripcion,
			primerNombre: data.primerNombreBeneficiario || '',
			segundoNombre: data.segundoNombreBeneficiario || '',
			primerApellido: data.primerApellidoBeneficiario || '',
			segundoApellido: data.segundoApellidoBeneficiario || ''
		};

		if (beneficiarioEditando !== null) {

			const listadoActualizado = listadoBeneficiariosEstudio.map(b =>
				b.id === beneficiarioEditando
					? { ...b, ...nuevoBeneficiario, id: beneficiarioEditando }
					: b
			);
			
			setListadoBeneficiariosEstudio(listadoActualizado);
			tercerosBeneficiarioEstudioAdicionados(listadoActualizado);
			setBeneficiarioEditando(null);
			limpiarBeneficiario();

		} else {

			const beneficiarios: any[] = [];
			beneficiarios.push(nuevoBeneficiario);
			tercerosBeneficiarioEstudioAdicionados(beneficiarios);
			setListadoBeneficiariosEstudio(beneficiarios);
			limpiarBeneficiario();
		}

		controlApoderado.reset();

	};

	const [formValid, setFormValid] = useState(false);

	useEffect(() => {
		const isValid =
			!controlApoderado.getValues('listaBeneficiarios') &&
			!controlApoderado.getValues('tipoDocEstudioObj') &&
			!controlApoderado.getValues('numDocEstudio') &&
			!controlApoderado.getValues('razonsocialEstudio');

		setFormValid(isValid);
	}, [
		controlApoderado.getValues('listaBeneficiarios'),
		controlApoderado.getValues('tipoDocEstudioObj'),
		controlApoderado.getValues('numDocEstudio'),
		controlApoderado.getValues('razonsocialEstudio'),
	]);

	const eliminarRegistroBeneficiario = async (id: string) => {
		const confirmAlert = await NotificationService.confirmAlert('¿Eliminar registro?');
		if (confirmAlert) {
			const temp = [...listadoBeneficiariosEstudio];

			const indexToRemove = temp.findIndex(b => b.id === id);
			if (indexToRemove !== -1) {
				temp.splice(indexToRemove, 1);
			} else {
				toast(`No se encontró el beneficiario con ID: ${id}`, { type: toast.TYPE.ERROR });
			}

			setListadoBeneficiariosEstudio(temp);
			tercerosBeneficiarioEstudioAdicionados(temp);
			limpiarBeneficiario();
		}
	};

	const seleccionarRegistroBeneficiario = async (item: LISTABENEFICIARIOS) => {

		setDeshabilitarNombresBeneficiario(false);
		setNumDocAnterior(item.numDocEstudio || item.numeroDocHijo);
		setIdAnterior(item.id);
		const beneficiario = listaParentescos.find((option: any) => option.idValor === Number(item.parentescoEstudio));
		if (beneficiario) {
			controlApoderado.setValue("listaBeneficiarios", `${item.parentescoEstudio}_${beneficiario?.descripcion}`);
		}
		controlApoderado.setValue("tipoDocEstudioObj", `${item?.tipoDocEstudioObj?.id}_${item?.tipoDocEstudioObj?.descripcion}`);
		controlApoderado.setValue("numDocEstudio", item?.numDocEstudio);
		controlApoderado.setValue("razonsocialEstudio", item?.razonsocialEstudio);

		controlApoderado.setValue("primerNombreBeneficiario", item?.primerNombre || '');
		controlApoderado.setValue("segundoNombreBeneficiario", item?.segundoNombre || '');
		controlApoderado.setValue("primerApellidoBeneficiario", item?.primerApellido || '');
		controlApoderado.setValue("segundoApellidoBeneficiario", item?.segundoApellido || '');

		setDescEstadoBoton('EDITAR BENEFICIARIO');
		setBeneficiarioEditando(item.id);
	};

	const handleSearchClick = async() => {

		const request = {
			tipoDocumentoAfiliado: responseSolicitud?.tipoIdentificacion,
			numeroDocumentoAfiliado: responseSolicitud?.identificacionCliente
		}

		try {
			const response = await apiSolicitud.PostConsultarIntervinientePorDocumentoAfiliado(request);
			if(response.data){
				const numeroDocumentoBeneficiario = controlApoderado.watch('numDocEstudio');
				const encontrado = response.data.find((item: any) => item?.itR_NUMERO_DOCUMENTO === numeroDocumentoBeneficiario);
				
				if(encontrado){
					setDeshabilitarNombresBeneficiario(true);
					controlApoderado.setValue('primerNombreBeneficiario',encontrado?.itR_NOMBRE);
					controlApoderado.setValue('segundoNombreBeneficiario',encontrado?.itR_SEGUNDONOMBRE);
					controlApoderado.setValue('primerApellidoBeneficiario',encontrado?.itR_PRIMERAPELLIDO);
					controlApoderado.setValue('segundoApellidoBeneficiario',encontrado?.itR_SEGUNDOAPELLIDO);
					
				}else{
					setDeshabilitarNombresBeneficiario(false);
					NotificationService.genericAlwarning(
						'',
						`Número de documento no se encontró. El beneficiario no existe`,
						'warning',
					);
				}
				
			}else{
				setDeshabilitarNombresBeneficiario(false);
				NotificationService.genericAlwarning(
					'',
					`Número de documento no se encontró. El beneficiario no existe`,
					'warning',
				);
			}

		}catch (error: any) {
			setIsLoading(false);	
			toast(`Error en la consulta de beneficiario de estudio en intervinientes: ${error.message}`, { type: toast.TYPE.ERROR });
		}
	}

	return (
		responseSolicitud?.timCodigo === "RETINST" ?
			<CHPaper title="Beneficiarios">

				{
					siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA') ? '' :
						<Grid container>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="tipoDocEstudioObj"
									control={controlApoderado.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											error={!!error}
											label="Tipo de Documento"
											value={value}
											helperText={error ? error.message : null}
											onChange={onChange}
										>
											{listaTipoDocumentos?.map(
												(option: IDENTIFICATIONTYPEDTO) => {
													const dato = `${option.id}_${option.descripcion}`;
													return (
														<MenuItem
															key={option.id}
															value={dato}
														>
															{option.descripcion}
														</MenuItem>
													);
												}
											)}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="numDocEstudio"
									control={controlApoderado.control}
									rules={{
										required: 'Este campo es requerido.',
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
											label="Número de Documento"
											value={value}
											onChange={(value) => {
												onChange(value);
											}}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="razonsocialEstudio"
									control={controlApoderado.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={false}
											label="Nombre / Razón Social"
											value={value}
											onChange={(value) => {
												onChange(value);
											}}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="parentescoEstudioObj"
									control={controlApoderado.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											error={!!error}
											label="Parentesco"
											value={value}
											helperText={error ? error.message : null}
											onChange={(value) => {
												onChange(value);
											}}
										>
											{listaParentesco?.map(
												(option: VALORCATALOGO) => {
													const dato = `${option.idValor}_${option.descripcion}`;
													return (
														<MenuItem
															key={option.idValor}
															value={dato}

														>
															{option.descripcion}
														</MenuItem>
													);
												}
											)}
										</CHSelect>
									)}
								/>
							</Grid>
						</Grid>
				}

				<Grid container>
					<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
						<Button
							className={classes.buttonAdd}
							variant="outlined"
							onClick={controlApoderado.handleSubmit(agregarTerceroApoderado)}
						>
							{descEstadoBoton}
						</Button>
						<Button
							className={classes.buttonAdd}
							variant="outlined"
							onClick={cancelarTerceroApoderado}
						>
							CANCELAR
						</Button>
					</Grid>
				</Grid>
				<h3>Lista de Apoderados</h3>
				<TableContainer component={Paper}>
					<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
						<Table aria-label="simple table">
							<TableHead>
								<TableRow>
									<TableCell>Tipo de Identificación</TableCell>
									<TableCell>Número de Identificación</TableCell>
									<TableCell>Nombre / Razón Social</TableCell>
									<TableCell>Parentesco</TableCell>
									{
										((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') ? '' :
											<TableCell align="center">Acciones</TableCell>
									}
								</TableRow>
							</TableHead>
							<TableBody>
								{terceroBeneficiarioEstudioModel.map(
									(option: TerceroBeneficiarioEstudioModel) => (
										<TableRow key={option.id}>
											<TableCell>{option.tipoDocEstudioObj.descripcion}</TableCell>
											<TableCell>{option.numDocEstudio}</TableCell>
											<TableCell>{option.razonsocialEstudio}</TableCell>
											<TableCell>{option.parentescoEstudioObj.id === undefined ? 'N/A' : option.parentescoEstudioObj.descripcion}</TableCell>
											{
												((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') ? '' :
													<TableCell align="center">
														<IconButton aria-label="Editar" color="primary" onClick={() => {
															seleccionarRegistroTercero(option);
														}}>
															<Edit />
														</IconButton>
														<IconButton aria-label="Eliminar" color="secondary" onClick={() => {
															eliminarRegistroTercero(option.id);
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
			</CHPaper> :

			<CHPaper title="Beneficiario de estudio">
				<LoadingComponent key="loader" loading={isLoading} />
				{
					siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA') || campoSoloLecturaRadicacionAprobada ? null :
						<Grid container>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="listaBeneficiarios"
									control={controlApoderado.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											error={!!error}
											label="Beneficiario"
											value={value}
											helperText={error ? error.message : null}
											disabled={descEstadoBoton === 'EDITAR BENEFICIARIO'}
											onChange={(event) => {
												const selectedValue = event.target.value;

												if (typeof selectedValue !== 'string') {
													return;
												}
												onChange(selectedValue);
												controlApoderado.setValue('tipoDocEstudioObj', '');
												controlApoderado.setValue('numDocEstudio', '');
												controlApoderado.setValue('razonsocialEstudio', '');
												controlApoderado.setValue('primerNombreBeneficiario', '');
												controlApoderado.setValue('segundoNombreBeneficiario', '');
												controlApoderado.setValue('primerApellidoBeneficiario', '');
												controlApoderado.setValue('segundoApellidoBeneficiario', '');
												handleBeneficiaryChange(selectedValue);
											}}
										>
											{listaParentescos.map((option: VALORCATALOGO) => (
												<MenuItem
													key={option.idValor}
													value={`${option.idValor}_${option.descripcion}`}
												>
													{option.descripcion}
												</MenuItem>
											))}
										</CHSelect>

									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="tipoDocEstudioObj"
									control={controlApoderado.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => {
										const selectedOption = listaTipoDocumentos?.find(
											(option: any) => `${option.id}_${option.descripcion}` === value
										);
										return (
											<CHSelect
												error={!!error}
												label="Tipo de Documento *"
												value={selectedOption ? `${selectedOption.id}_${selectedOption.descripcion}` : ''}
												helperText={error ? error.message : null}
												onChange={onChange}
												disabled={tipoDeBeneficiario !== "HIJO/HIJA" || descEstadoBoton === 'EDITAR BENEFICIARIO'}
											>
												{listaTipoDocumentos?.map((option: IDENTIFICATIONTYPEDTO) => {
													if (option?.id && option?.descripcion) {
														const dato = `${option.id}_${option.descripcion}`;

														return (
															<MenuItem key={option.id} value={dato}>
																{option.descripcion}
															</MenuItem>
														);
													}
													return null;
												})}
											</CHSelect>
										);
									}}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="numDocEstudio"
									control={controlApoderado.control}
									rules={{
										required: 'Este campo es requerido.',
										minLength: {
											value: 4,
											message: 'La longitud mínima del campo es de 4 caracteres',
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
											label="Número de Documento *"
											value={value}
											onChange={(e) => {
												const value = e.target.value.replace(/[^0-9]/g, '');
												onChange(value);
												if(value.length < 5){
													setDeshabilitarLupaBusqueda(true);
												}else if(value.length >= 5){
													setDeshabilitarLupaBusqueda(false);
												}
											}}
											InputLabelProps={{ shrink: true }}
											disabled={tipoDeBeneficiario !== "HIJO/HIJA" || descEstadoBoton === 'EDITAR BENEFICIARIO'}
											inputProps={{
												maxLength: 12
											}}
										/>
									)}
								/>
							</Grid>

							{tipoDeBeneficiario === 'HIJO/HIJA' && (
								<Grid item xs="auto">
									<IconButton
										onClick={handleSearchClick}
										className={classes.icon}
										disabled={deshabilitarLupaBusqueda}
									>
										<SearchIcon />
									</IconButton>
								</Grid>
							)}
									<Grid className={classes.column2} item xs>
									<Controller
										name="primerNombreBeneficiario"
										control={controlApoderado.control}
										rules={{
											required: 'Este campo es requerido.',
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												error={!!error}
												helperText={error ? error.message : null}
												isReadOnly={false}
												label="Primer Nombre *"
												value={value}
												onChange={onChange}
												inputProps={{
													style: {
														textTransform: 'uppercase',
													},
													maxLength: 50
												}}
												InputLabelProps={{ shrink: true }}
												disabled={deshabilitarNombresBeneficiario}
											/>
										)}
									/>
								</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="segundoNombreBeneficiario"
									control={controlApoderado.control}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={false}
											label="Segundo Nombre"
											value={value}
											onChange={onChange}
											inputProps={{
												style: {
													textTransform: 'uppercase',
												},
												maxLength: 50
											}}
											InputLabelProps={{ shrink: true }}
											disabled={deshabilitarNombresBeneficiario}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="primerApellidoBeneficiario"
									control={controlApoderado.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={false}
											label="Primer Apellido *"
											value={value}
											onChange={onChange}
											inputProps={{
												style: {
													textTransform: 'uppercase',
												},
												maxLength: 50
											}}
											InputLabelProps={{ shrink: true }}
											disabled={deshabilitarNombresBeneficiario}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="segundoApellidoBeneficiario"
									control={controlApoderado.control}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={false}
											label="Segundo Apellido"
											value={value}
											onChange={onChange}
											inputProps={{
												style: {
													textTransform: 'uppercase',
												},
												maxLength: 50
											}}
											InputLabelProps={{ shrink: true }}
											disabled={deshabilitarNombresBeneficiario}
										/>
									)}
								/>
							</Grid>
						</Grid>
				}
				{
					siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA') || campoSoloLecturaRadicacionAprobada ? null :
						<Grid container>
							<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
								<Button className={classes.buttonAdd} variant="outlined" onClick={controlApoderado.handleSubmit(agregarBeneficiario)} disabled={formValid}>
									{descEstadoBoton}
								</Button>

								<Button className={classes.buttonAdd} variant="outlined" onClick={limpiarBeneficiario}>
									CANCELAR
								</Button>
							</Grid>
						</Grid>
				}
				<h3>Lista de beneficiarios de estudio</h3>
				<TableContainer component={Paper}>
					<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
						<Table aria-label="simple table">
							<TableHead>
								<TableRow>
									<TableCell>Beneficiario</TableCell>
									<TableCell>Tipo de Identificación</TableCell>
									<TableCell>Número de Identificación</TableCell>
									<TableCell>Nombre / Razón Social</TableCell>
									{
										siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA') || campoSoloLecturaRadicacionAprobada ? null :
											<TableCell align="center">Acciones</TableCell>
									}
								</TableRow>
							</TableHead>
							<TableBody>
								{
									listadoBeneficiariosEstudio.length > 0 && listadoBeneficiariosEstudio.map(
										(beneficiario: LISTABENEFICIARIOS) => 
										
											 (
												<TableRow key={beneficiario.id}>
													<TableCell>
														{beneficiario.tipoDeBeneficiario || ''}
													</TableCell>
													<TableCell>
														{beneficiario.tipoDocEstudioObj?.descripcion || ''}
													</TableCell>
													<TableCell>
														{beneficiario.numDocEstudio || 'N/A'}
													</TableCell>
													<TableCell style={{ textTransform: 'uppercase' }}>
														{beneficiario.razonsocialEstudio || 'N/A'}
													</TableCell>
													{siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA') || campoSoloLecturaRadicacionAprobada ? null :
														<TableCell align="center">
															<IconButton
																aria-label="Editar"
																color="primary"
																onClick={() => seleccionarRegistroBeneficiario(beneficiario)}
																disabled={beneficiario?.tipoDeBeneficiario !== 'HIJO/HIJA'}
															>
																<Edit />
															</IconButton>
															<IconButton
																aria-label="Eliminar"
																color="secondary"
																onClick={() => eliminarRegistroBeneficiario(beneficiario.id)}
															>
																<Delete />
															</IconButton>
														</TableCell>
													}
												</TableRow>
											))
										}
							</TableBody>
						</Table>
					</Grid>
				</TableContainer>
			</CHPaper>
	)
}

export default TerceroBeneficiarioEstudio;