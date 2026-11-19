import React, { useEffect, useState } from 'react';
import {
	Grid,
	makeStyles,
	createStyles,
	Dialog,
	DialogContent,
	DialogTitle,
	Button,
	MenuItem,
} from '@material-ui/core';
import { Controller, UseFormReturn } from 'react-hook-form';
import {
	ProgramaEducativo,
	ProgramasEducativos,
} from '../../../types/educational.entities.type';
import { CATALOG_NAME } from '../../../constants/constants';
import { PROGRAMAPORENTIDADDTO, NIVELEDUCATIVO, VALORCATALOGO } from '../../../types/catalog.type';
import AuthService from '../../../services/auth.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '120px',
			marginLeft: '.5rem',
			borderRadius: '10%',
			background: '#1C3F8B',
			color: '#FFFFFF',
		},
		restoreButton: {
			width: '250px',
			marginLeft: '.5rem',
			background: '#1C3F8B',
			color: '#FFFFFF',
		},
		column2: {
			padding: '10px',
			textAlign: 'left',
		},
	}),
);

function CreateProgram({
	isOpen,
	idEntidad,
	listaTipoPrograma,
	listaNivelEducativo,
	dataModel = null,
	formularioPrograma,
	onClose,
	obtenerTipoPrograma,
	obtenerNivelEducativo,
	guardarProgramaEducativo,
	actualizarProgramaEducativo,
	obtenerProgramasEducativosPorEntidad,
}: {
	isOpen: boolean,
	idEntidad: string,
	listaTipoPrograma: VALORCATALOGO[],
	listaNivelEducativo: NIVELEDUCATIVO[],
	dataModel: PROGRAMAPORENTIDADDTO | null,
	formularioPrograma: UseFormReturn<any>,
	onClose: () => void,
	obtenerTipoPrograma: (nombreCatalogo: string) => void,
	obtenerNivelEducativo: () => void;
	guardarProgramaEducativo: (data: ProgramasEducativos) => void;
	actualizarProgramaEducativo: (data: ProgramaEducativo) => void;
	obtenerProgramasEducativosPorEntidad:(data: string) => void;
}) {
	const classes = useStyles();
	
	const [title, setTitle] = useState<string>('');
	const [textButton, setTextButton] = useState<string>('');
	const listaEstado =[
	 {code:1,description:'Activo'},
	 {code:0,description:'Inactivo'}
	]
	
	useEffect(() => {
		if (dataModel !== null) {
			setTitle('ACTUALIZAR PROGRAMA EDUCATIVO');
			setTextButton('ACTUALIZAR PROGRAMA');
		} else {
			setTitle('AGREGAR PROGRAMA EDUCATIVO');
			setTextButton('AGREGAR PROGRAMA');
			formularioPrograma.reset();
		}
		obtenerTipoPrograma(CATALOG_NAME.TIPO_PROGRAMA);
		obtenerNivelEducativo();
	}, [dataModel]);

	const handleClose = () => {
		onClose();
	};

	const CreateOrUpdateProgramHandleSubmit = (data: ProgramasEducativos) => {
		const user = AuthService.getUserInfo();
		if (dataModel !== null) {
			const modifyData: ProgramaEducativo = {
				id: dataModel.idPrograma,
				programa: data.programa,
				estado:data?.estado,
				idEntidad: dataModel.idEntidad,
				nivelEducativoId: data.nivelEducativoId,
				idTipoPrograma: data.idTipoPrograma,
				modificacionPor: user.USR_ID,
			};
			actualizarProgramaEducativo(modifyData);
		} else {
			const modifyData: ProgramasEducativos = {
				idEntidad: data.idEntidad,
				idTipoPrograma: data.idTipoPrograma,
				programa: data.programa,
				nivelEducativoId: data.nivelEducativoId,
				creadoPor: user.USR_ID,
				estado:data.estado 
			};
			guardarProgramaEducativo(modifyData);
		}
		formularioPrograma.reset();
		onClose();
	};

	return (
		<Dialog
			fullWidth
			open={isOpen}
			maxWidth="md"
			aria-labelledby="form-dialog-title"
		>
			<DialogTitle id="form-dialog-title">
				{title}
			</DialogTitle>
			<DialogContent>
				<Grid item container>
					<Grid container md={12}>
						<Grid className="item-containerModal" md={5}>
							<Grid className="subcontainerModal">
								<Controller
									name="idTipoPrograma"
									control={formularioPrograma.control}
									rules={{
										validate: () => formularioPrograma.watch('idTipoPrograma') === 0 ? 'Este campo es requerido' : true,
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											defaultValue=""
											error={!!error}
											label="Tipo programa *"
											value={value}
											helperText={error ? error.message : null}
											onChange={onChange}
											disabled={dataModel !== null}
										>
											{listaTipoPrograma.map(
												(option: any) => (
													<MenuItem key={option.idValor} value={option.idValor}>
														{option.descripcion}
													</MenuItem>
												),
											)}
										</CHSelect>
									)}
								/>
							</Grid>
						</Grid>
						<Grid className="item-containerModal" md={5}>
							<Grid className="subcontainerModal">
								<Controller
									name="programa"
									control={formularioPrograma.control}
									defaultValue=""
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField											
											InputLabelProps={{ shrink: true }}
											error={!!error}
											helperText={error ? error.message : null}
											isReadOnly={false}
											label="Programa *"
											value={value}
											onChange={onChange}
										/>
									)}
								/>
							</Grid>
						</Grid>
						<Grid className="item-containerModal" md={5}>
							<Grid className="subcontainerModal">
								<Controller
									name="nivelEducativoId"
									control={formularioPrograma.control}
									defaultValue=""
									rules={{
										validate: () => formularioPrograma.watch('nivelEducativoId') === '' ? 'Este campo es requerido' : true,
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											defaultValue=""
											error={!!error}
											label="Nivel educativo *"
											value={value}
											helperText={error ? error.message : null}
											onChange={onChange}
											disabled={dataModel !== null}
										>
											{listaNivelEducativo.map(
												(option: any) => (
													<MenuItem key={option.id} value={option.id}>
														{option.descripcion}
													</MenuItem>
												),
											)}
										</CHSelect>
									)}
								/>
							</Grid>
						</Grid>
							<Grid className="item-containerModal" md={5}>
								<Grid className="subcontainerModal">
									<Controller
										name="estado"
										control={formularioPrograma.control}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHSelect
											defaultValue=""
											error={!!error}
											label="Estado*"
											value={value}
											helperText={error ? error.message : null}
											onChange={onChange}>
											
											{listaEstado.map(
												(x:any) => (
													<MenuItem
													key={x.code}
													value={x.code}
												>
													{x.description}
												</MenuItem>
												),
											)}
										</CHSelect>
										)}
										/>
								</Grid>
							</Grid>
					</Grid>
				</Grid>
				<div className="buttons">
					<Button
						className={classes.button}
						variant="outlined"
						type="submit"
						onClick={handleClose}
					>
						CERRAR
					</Button>
					<Button
						className={classes.restoreButton}
						variant="outlined"
						onClick={formularioPrograma.handleSubmit(CreateOrUpdateProgramHandleSubmit)}
					>
						{textButton}
					</Button>
				</div>
			</DialogContent>
		</Dialog>
	);
}

export default CreateProgram;