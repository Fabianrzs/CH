import React, { useEffect, useState } from 'react';
import {
	Grid,
	makeStyles,
	createStyles,
	Dialog,
	DialogContent,
	DialogTitle,
	Button,
	MuiThemeProvider,
	createTheme,
} from '@material-ui/core';
import { Controller, UseFormReturn } from 'react-hook-form';
import { SedeEntidadEducativa } from '../../../types/educational.entities.type';
import { CITYDTO } from '../../../types/catalog.type';
import AuthService from '../../../services/auth.service';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';

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

const formLabelsTheme = createTheme({
	overrides: {
		MuiFormLabel: {
			asterisk: {
				color: '#db3131',
				'&$error': {
					color: '#db3131',
				},
			},
		},
	},
});

function CreateCampus({
	isOpen,
	listaCiudades,
	dataModel = null,
	formularioSede,
	ciudadValue,
	onClose,
	guardarSede,
	actualizarSede,
	setCiudadValue
}: {
	isOpen: boolean;
	listaCiudades: CITYDTO[];
	dataModel: SedeEntidadEducativa | null;
	formularioSede: UseFormReturn<any>;
	ciudadValue: CITYDTO | null;
	onClose: () => void;
	guardarSede: (data: SedeEntidadEducativa) => void;
	actualizarSede: (data: SedeEntidadEducativa) => void;
	setCiudadValue: (data: CITYDTO | null) => void;
}) {
	
	const classes = useStyles();
	const [titleText, setTitleText] = useState<string>('');

	useEffect(() => {
		if (dataModel !== null) {
			setTitleText('ACTUALIZAR SEDE');
		} else {
			setTitleText('AGREGAR SEDE');
			formularioSede.reset();
		}
	}, [dataModel]);

	const onChangeCity = (event: any, newValue: CITYDTO | null) => {
		setCiudadValue(newValue);
		formularioSede.setValue('ciudadId', newValue?.id?.toString() || '');
	};

	const handleClose = () => {
		onClose();
	};

	const CreateProgramHandleSubmit = (data: SedeEntidadEducativa) => {
		const user = AuthService.getUserInfo();
		if (dataModel !== null) {
			const modifyData: SedeEntidadEducativa = {
				idSedeEntidad: dataModel.idSedeEntidad,
				nombreSede: data.nombreSede,
				ciudadId: data.ciudadId,
				estado: true,
				idEntidad: dataModel.idEntidad,
				modificadoPor: user.USR_ID,
			};
			actualizarSede(modifyData);
		} else {
			data.creadoPor = user.USR_ID;
			guardarSede(data);
		}
		handleClose();
		formularioSede.reset();
	};

	return (
		<Dialog
			fullWidth
			open={isOpen}
			maxWidth="md"
			aria-labelledby="form-dialog-title"
		>
			<DialogTitle id="form-dialog-title">
				{titleText}
			</DialogTitle>
			<DialogContent>
				<Grid item container>
					<Grid container md={12}>
						<Grid className="item-containerModal" md={5}>
							<Grid className="subcontainerModal">
								<Controller
									name="nombreSede"
									control={formularioSede.control}
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
											label="Sede"
											value={value}
											required
											onChange={onChange}
										/>
									)}
								/>
							</Grid>
						</Grid>
						<Grid className="item-containerModal" md={5}>
							<Grid className="subcontainerModal">
								<Controller
									name="ciudadId"
									control={formularioSede.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										fieldState: { error },
									}) => (
										<CHAutoComplete
											value={ciudadValue}
											options={listaCiudades}
											getOptionLabel={option => option.nombre}
											onChange={onChangeCity}
											renderInput={params => (
												<MuiThemeProvider theme={formLabelsTheme}>
													<CHTextField
														error={!!error}
														helperText={error ? error.message : null}
														{...params}
														InputLabelProps={{ shrink: true }}
														required
														label="Ciudad"
													/>
												</MuiThemeProvider>
											)}
										/>
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
						onClick={formularioSede.handleSubmit(CreateProgramHandleSubmit)}
					>
						{titleText}
					</Button>
				</div>
			</DialogContent>
		</Dialog>
	);
}

export default CreateCampus;
