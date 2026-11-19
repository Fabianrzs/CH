import React, { useEffect, useState } from 'react';
import {
	Grid,
	createStyles,
	makeStyles,
	TableRow,
	Typography,
	Button,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { format } from 'date-fns';
import { AppState } from '../../../store/rootReducer';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';
import { CreateAnotacion } from '../../../types/educational.entities.type';
import { startGetArchivosPorEntidad } from '../../../actions/educationalEntities.actions';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ApiEntidadesEducativas from '../../../services/api-entidadesEducativas.service';
import AuthService from '../../../services/auth.service';
import UtilService from '../../../utils/util.service';

const useStyles = makeStyles(() =>
	createStyles({
		aliniarIcono: {
			marginTop: '1rem',
		},
		aliniarTexto: {
			color: 'Black',
			padding: '10px',
		},
		columnaUpload: {
			paddingLeft: '15%',
		},
		button: {
			backgroundColor: '#42b7ef',
			color: '#fff',
			width: '20%',
			marginRight: '1%',
		},
	}),
);

interface TableRow {
	fecCreacion: Date;
	observacion: string;
	usuario: string;
	idEntidad: string;
}


function FilesEntities({
	idEntidad,
	educationalEntitiesState,
}: {
	idEntidad: string,
	educationalEntitiesState: EducationalEntitiesState;
}): any {

	const api = ApiEntidadesEducativas();
	const [loading, setLoading] = useState(false);
	const [tableData, setTableData] = useState<TableRow[]>([]);
	const user = AuthService.getUserInfo();

	const formularioAnotaciones = useForm({
		defaultValues: {
			observaciones: '',
		},
	});
	const observacion = formularioAnotaciones.watch('observaciones');


	const onClickAddnew = async () => {

		const creadoPorr = user.USR_ID;
		try {

			const newAnotacion: CreateAnotacion = {
				idEntidad: idEntidad,
				anotacion: observacion,
				creadoPor: creadoPorr,
				fechaCreacion: new Date(),
			};

			const response = await api.PostCrearAnotacion(newAnotacion);

			if (response.isSuccess) {
				setLoading(true);
				try {
					const response = await api.PostConsultarAnotacion(idEntidad);
			
					if (response?.isSuccess && Array.isArray(response.data)) {
				
						const formattedData = response.data.map((annotation: { fechaCreacion: string | number | Date; anotacion: any; creadoPor: any; }) => ({
							fecCreacion: new Date(annotation.fechaCreacion),
							observacion: annotation.anotacion,
							usuario: annotation.creadoPor,
							idEntidad: idEntidad,
						}));
			
						setTableData(formattedData);
					} else {
						setTableData([]);
					}
				} catch (error) {
					toast('Error al consultar las anotaciones.', { type: toast.TYPE.ERROR });
				} finally {
					setLoading(false);
				}
				formularioAnotaciones.reset();
				toast('Registro agregado exitosamente.', { type: toast.TYPE.SUCCESS });
			} else {
				toast('Error al agregar el registro en la API.', { type: toast.TYPE.ERROR });
			}
		} catch (error) {
			console.error('Error al realizar la llamada a la API:', error);
			toast('Error al conectar con la API.', { type: toast.TYPE.ERROR });
		}
	};

	const fetchAnnotations = async () => { 
		setLoading(true);
		try {
			const response = await api.PostConsultarAnotacion(idEntidad);
	
			if (response?.isSuccess && Array.isArray(response.data)) {
		
				const formattedData = response.data.map((annotation: { fechaCreacion: string | number | Date; anotacion: any; creadoPor: any; }) => ({
					fecCreacion: new Date(annotation.fechaCreacion),
					observacion: annotation.anotacion,
					usuario: annotation.creadoPor,
					idEntidad: idEntidad,
				}));
	
				setTableData(formattedData);
				toast('Se encontraron anotaciones.', { type: toast.TYPE.SUCCESS });
			} else {
				setTableData([]);
				toast('No se encontraron anotaciones para esta entidad.', { type: toast.TYPE.INFO });
			}
		} catch (error) {
			toast('Error al consultar las anotaciones.', { type: toast.TYPE.ERROR });
		} finally {
			setLoading(false);
		}
	};
	
	useEffect(() => {
		fetchAnnotations();
	}, [idEntidad]);

	const onClickCancel = () => {
		formularioAnotaciones.reset();
	}

	return (
		<>
			<CHPaper title="Anotaciones">
				<LoadingComponent key="loader" loading={loading} />
				<Grid container spacing={2}>
					<Grid item xs={6}>
						<Controller
							name="observaciones"
							control={formularioAnotaciones.control}
							defaultValue=""
							rules={{
								required: 'Este campo es requerido',
							}}
							render={({ field: { onChange, value }, fieldState: { error } }) => (
								<CHTextField
									InputLabelProps={{ shrink: true }}
									label="Observaciones *"
									fullWidth
									multiline
									value={value}
									onChange={onChange}
									inputProps={{
										style: {
											textTransform: 'uppercase',
										},
									}}
									error={!!error}
									helperText={error ? error.message : null}
								/>
							)}
						/>
					</Grid>
					<Grid container justifyContent="flex-end">
						<Button
							onClick={formularioAnotaciones.handleSubmit(onClickAddnew)}
							variant="contained"
							color='primary'
						>
							AGREGAR ANOTACIÓN
						</Button>
						<Button
							onClick={onClickCancel}
							variant="contained"
							color='default'
							style={{ marginLeft: '10px' }}
						>
							CANCELAR
						</Button>
					</Grid>
				</Grid>
				<Grid>
					<Typography variant="h5" component="h5" gutterBottom>
						Lista de anotaciones
					</Typography>
					<br />
					<br />
					<table className="table">
						<thead>
							<tr>
								<th className="head">Fecha de anotación</th>
								<th className="head">Observación</th>
								<th className="head">Usuario</th>
							</tr>
						</thead>
						<tbody>
							{tableData.length > 0 ? (
								tableData.map((row: any, index: number) => (
									<tr key={`table-realStateRegistrations-${index}`}>
										<td className="tab-space-01">
											{UtilService.getFullDateWithTimeDirection(new Date(String(row?.fecCreacion).toString()))}
										</td>
										<td className="tab-space-02" style={{ textTransform: 'uppercase' }}>
											{row.observacion}
										</td>
										<td className="tab-space-02" style={{ textTransform: 'uppercase' }}>
											{row.usuario}
										</td>
									</tr>
								))
							) : (
								<tr>
									<td colSpan={3} style={{ textAlign: 'center' }}>
										No hay anotaciones.
									</td>
								</tr>
							)}
						</tbody>
					</table>
				</Grid>
			</CHPaper>
		</>
	);
}

const mapStateToProps = (state: AppState) => ({
	educationalEntitiesState: state.educationalEntities,
});

const mapDispatchToProps = (dispatch: any) => ({
	obtenerArchivosPorEntidad: (idEntidad: string) =>
		dispatch(startGetArchivosPorEntidad(idEntidad))
});

export default connect(mapStateToProps, mapDispatchToProps)(FilesEntities);