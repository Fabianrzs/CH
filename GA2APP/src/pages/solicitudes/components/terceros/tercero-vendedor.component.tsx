import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import { Controller, useForm } from 'react-hook-form';
import { Delete, Edit } from '@material-ui/icons';
import { toast } from 'react-toastify';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHPaper from "../../../../components/ch-paper/CHPaper";
import { CITYDTO, DEPARTAMENTODTO, IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import { TerceroVendedorModel } from '../../../../models/tercero-vendedor.model';
import GenerarGuid from '../funciones/guid.funtions';
import NotificationService from '../../../../services/notification.service';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '120px',
			marginLeft: '.5rem',
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
		columnLabel: {
			padding: '10px',
		},
	}),
);

function TerceroVendedor({
	listaTipoDocumentos,
	listaDepartamentos,
	listaCiudades,
	listaVendedor,
	obtenerCiudadPorVendedorId,
	tercerosVendedorAdicionados
}: {
	listaTipoDocumentos: IDENTIFICATIONTYPEDTO[];
	listaDepartamentos: DEPARTAMENTODTO[];
	listaCiudades: CITYDTO[];
	listaVendedor: TerceroVendedorModel[];
	obtenerCiudadPorVendedorId: (stateID?: string) => void;
	tercerosVendedorAdicionados: (terceros: TerceroVendedorModel[]) => void
}) {
	const classes = useStyles();
	const [terceroVendedorModel, setTerceroVendedorModel] = useState<TerceroVendedorModel[]>([]);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR VENDEDOR');
	const [numDocAnterior, setNumDocAnterior] = useState<string>('');
	const [idAnterior, setIdAnterior] = useState<string>('');
	const [ciudades, setCiudades] = useState<boolean>(true);

	useEffect(() => {
		if (listaVendedor !== null) {
			if (listaVendedor.length > 0) {
				setTerceroVendedorModel(listaVendedor);
			}
		}
	}, []);

	const controlVendedor = useForm({
		defaultValues: {
			tipoDocObj: '',
			numDoc: '',
			razonsocial: '',
			direccion: '',
			departamentoVendedor: '',
			ciudadObj: '',
			correo: '',
			telefono: '',
		},
	});

	const cancelarTerceroVendedor = async () => {
		setDescEstadoBoton('AGREGAR VENDEDOR');
		controlVendedor.setValue("tipoDocObj", '');
		controlVendedor.setValue("numDoc", '');
		controlVendedor.setValue("razonsocial", '');
		controlVendedor.setValue("direccion", '');
		controlVendedor.setValue("departamentoVendedor", '');
		controlVendedor.setValue("ciudadObj", '');
		controlVendedor.setValue("correo", '');
		controlVendedor.setValue("telefono", '');
		controlVendedor.reset();
	}

	const agregarTerceroVendedor = async (data: any) => {

		const datoEncontrado = terceroVendedorModel.findIndex(x => x.numDoc === data.numDoc && x.numDoc !== numDocAnterior);
		if (datoEncontrado === -1) {

			const guid = GenerarGuid();

			const terceroNuevo = {
				id: guid.newGuid(),
				tipoDoc: data.tipoDocObj.split('_')[0],
				tipoDocObj: {
					id: data.tipoDocObj.split('_')[0],
					descripcion: data.tipoDocObj.split('_')[1]
				},
				numDoc: data.numDoc,
				razonSocial: data.razonsocial,
				direccion: data.direccion,
				departamentoVendedor: {
					id: data.departamentoVendedor.split('_')[0],
					descripcion: data.departamentoVendedor.split('_')[1]
				},
				ciudad: data.ciudadObj.split('_')[0],
				ciudadObj: {
					id: data.ciudadObj.split('_')[0],
					descripcion: data.ciudadObj.split('_')[1]
				},
				correo: data.correo,
				telefono: data.telefono,
			};

			let terceros: TerceroVendedorModel[] = [];
			if (descEstadoBoton === 'AGREGAR VENDEDOR') {
				setTerceroVendedorModel(oldArray => [...oldArray, terceroNuevo]);
				terceros = [...terceroVendedorModel];
				terceros.push(terceroNuevo);
			} else {
				const temp = [...terceroVendedorModel];
				const posicionActual = temp.findIndex(x => x.numDoc === numDocAnterior);
				temp.splice(posicionActual, 1);
				terceroNuevo.id = idAnterior;
				temp.splice(posicionActual, 0, terceroNuevo);
				terceros = temp;
				setTerceroVendedorModel(temp);
			}

			tercerosVendedorAdicionados(terceros);
			setNumDocAnterior('');
			setIdAnterior('');
			await cancelarTerceroVendedor();

		} else {
			toast('Ya existe un vendedor con este mismo número de documento en la lista', { type: toast.TYPE.ERROR });
		}
	}


	const eliminarRegistroTercero = async (item: string) => {
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);
		if (confirmAlert) {
			const temp = [...terceroVendedorModel];
			temp.splice(temp.findIndex(x => x.id === item), 1);
			setTerceroVendedorModel(temp);
			tercerosVendedorAdicionados(temp);
		}
	}

	const seleccionarRegistroTercero = async (item: TerceroVendedorModel) => {
		setNumDocAnterior(item.numDoc);
		setIdAnterior(item.id);
		controlVendedor.setValue('ciudadObj', '');
		obtenerCiudadPorVendedorId(item.departamentoVendedor.id.toString());
		controlVendedor.setValue("tipoDocObj", `${item.tipoDocObj.id}_${item.tipoDocObj.descripcion}`);
		controlVendedor.setValue("numDoc", item.numDoc);
		controlVendedor.setValue("razonsocial", item.razonSocial);
		controlVendedor.setValue("direccion", item.direccion);
		controlVendedor.setValue("departamentoVendedor", `${item.departamentoVendedor.id}_${item.departamentoVendedor.descripcion}`);
		controlVendedor.setValue("ciudadObj", `${item.ciudadObj.id}_${item.ciudadObj.descripcion}`);
		controlVendedor.setValue("correo", item.correo);
		controlVendedor.setValue("telefono", item.telefono);
		setDescEstadoBoton('EDITAR VENDEDOR');
	}



	return (
		<>
			<CHPaper title="Información vendedor del inmueble">
				<Grid container>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="tipoDocObj"
							control={controlVendedor.control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									error={!!error}
									label="Tipo de Documento Vendedor"
									value={value}
									helperText={error ? error.message : null}
									onChange={(value) => {
										onChange(value);
									}}
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
							name="numDoc"
							control={controlVendedor.control}
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
									onChange={onChange}
								/>
							)}
						/>
					</Grid>

					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="razonsocial"
							control={controlVendedor.control}
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
									onChange={onChange}
								/>
							)}
						/>
					</Grid>

					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="direccion"
							control={controlVendedor.control}
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
									label="Dirección"
									value={value}
									onChange={onChange}
								/>
							)}
						/>
					</Grid>

					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="departamentoVendedor"
							control={controlVendedor.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									error={!!error}
									label="Departamento *"
									value={value}
									required
									helperText={error ? error.message : null}
									onChange={(value) => {
										const dato = value.target.value as string;
										const idDepartamento = dato.split('_')[0];
										controlVendedor.setValue('ciudadObj', '');
										if (idDepartamento !== "0") {
											obtenerCiudadPorVendedorId(idDepartamento);
											setCiudades(true);
										} else {
											setCiudades(false);
										}
										onChange(value);
									}}
								>
									{listaDepartamentos?.map(
										(option: DEPARTAMENTODTO) => {
											const dato = `${option.id}_${option.nombre}`;
											return (
												<MenuItem
													key={option.id}
													value={dato}
												>
													{option.nombre}
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
							name="ciudadObj"
							control={controlVendedor.control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									error={!!error}
									label="Ciudad"
									value={value}
									helperText={error ? error.message : null}
									onChange={(value) => {
										onChange(value);
									}}
								>
									{ciudades && listaCiudades?.map(
										(option: CITYDTO) => {
											const dato = `${option.id}_${option.nombre}`;
											return (
												<MenuItem
													key={option.id}
													value={dato}
												>
													{option.nombre}
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
							name="correo"
							control={controlVendedor.control}
							rules={{
								required: 'Este campo es requerido.',
								pattern: {
									value: /^(([^<>()\\[\]\\.,;:\s@"]+(\.[^<>()\\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
									message: 'Correo inválido'
								}
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									isReadOnly={false}
									label="Correo Electrónico"
									value={value}
									onChange={onChange}
								/>
							)}
						/>
					</Grid>

					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="telefono"
							control={controlVendedor.control}
							defaultValue=""
							rules={{
								required: 'Este campo es requerido.',
								pattern: /\(?([3|6]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? 'El celular o teléfono debe comenzar por 3 o 6 y tener una longitud de 10 dígitos' : null}
									isReadOnly={false}
									label="Teléfono"
									value={value}
									onChange={onChange}
									cellPhoneMask
								/>
							)}
						/>
					</Grid>
				</Grid>

				<Grid container>
					<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
						<Button
							className={classes.buttonAdd}
							variant="outlined"
							onClick={controlVendedor.handleSubmit(agregarTerceroVendedor)}
						>
							{descEstadoBoton}
						</Button>

						<Button
							className={classes.buttonAdd}
							variant="outlined"
							onClick={cancelarTerceroVendedor}
						>
							CANCELAR
						</Button>
					</Grid>
				</Grid>

				<h3>Lista de vendedores</h3>

				<TableContainer component={Paper}>
					<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
						<Table aria-label="simple table">
							<TableHead>
								<TableRow>
									<TableCell>Tipo de Identificación</TableCell>
									<TableCell>Número de Identificación</TableCell>
									<TableCell>Nombre / Razón Social</TableCell>
									<TableCell>Departamento</TableCell>
									<TableCell>Ciudad</TableCell>
									<TableCell>Correo Electrónico</TableCell>
									<TableCell>Teléfono</TableCell>
									<TableCell align="center">Acciones</TableCell>
								</TableRow>
							</TableHead>
							<TableBody>
								{terceroVendedorModel.map(
									(option: TerceroVendedorModel) => (
										<TableRow key={option.id}>
											<TableCell>{option.tipoDocObj.descripcion}</TableCell>
											<TableCell>{option.numDoc}</TableCell>
											<TableCell>{option.razonSocial}</TableCell>
											<TableCell>{option.departamentoVendedor.descripcion}</TableCell>
											<TableCell>{option.ciudadObj.descripcion}</TableCell>
											<TableCell>{option.correo}</TableCell>
											<TableCell>{option.telefono}</TableCell>
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

export default TerceroVendedor;
