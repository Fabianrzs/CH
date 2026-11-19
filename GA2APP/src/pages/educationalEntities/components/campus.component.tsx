import React, { useState, useEffect } from 'react';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { connect } from 'react-redux';
import { useForm } from 'react-hook-form';
import { AppState } from '../../../store/rootReducer';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import {
	startAddCampus,
	startGetSedesEntidadEducativa,
	startUpdateCampus,
	startDeleteCampus,
} from '../../../actions/educationalEntities.actions';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import CreateCampus from './createCampus';
import { SedeEntidadEducativa } from '../../../types/educational.entities.type';
import { CITYDTO } from '../../../types/catalog.type';
import NotificationService from '../../../services/notification.service';

const useStyles = makeStyles(() =>
	createStyles({
		iconRed: {
			padding: '8px',
			textAlign: 'inherit',
			color: '#FF0000',
			margin: '10px'
		},
		button: {
			backgroundColor: '#1C3F8B',
			color: '#fff',
			width: '20%',
			marginRight: '1%',
		},
	}),
);

function Campus({
	idEntidad,
	educationalEntitiesState,
	usuario,
	catalogState,
	obtenerSedesPorEntidad,
	guardarSede,
	actualizarSede,
	eliminarSede,
}: {
	idEntidad: string;
	educationalEntitiesState: EducationalEntitiesState;
	usuario: any;
	catalogState: CatalogState;
	obtenerSedesPorEntidad: (idEntidad: string) => void;
	guardarSede: (data: SedeEntidadEducativa) => void;
	actualizarSede: (data: SedeEntidadEducativa) => void;
	eliminarSede: (idSede: string, idEntidad: string, idUsuario: string) => void;
}): any {
	const classes = useStyles();
	const [isOpenCreate, setIsOpenCreate] = useState<boolean>(false);
	const [dataModal, setDataModal] = useState<any>(null);
	const [ciudadValue, setCiudadValue] = React.useState<CITYDTO | null>(null);

	const formularioSede = useForm<SedeEntidadEducativa>({
		defaultValues: {
			idEntidad: idEntidad,
			ciudadId: '',
			nombreSede: '',
			creadoPor: ''
		},
	})

	useEffect(() => {
		obtenerSedesPorEntidad(idEntidad);
	}, []);

	const onClose = () => {
		formularioSede.reset();
		setIsOpenCreate(false);
	};

	const openCreateOrUpdateModal = (data: any = null, tipoFuncion: string) => {
		if(tipoFuncion === 'editar'){
			setDataModal(data);
			loadData(data);
		}else{
			setDataModal(null);
			setCiudadValue(null);
		}
		
		setIsOpenCreate(true);
	};

	const loadData = (dataModel: SedeEntidadEducativa) => {
		formularioSede.setValue('nombreSede', dataModel.nombreSede);
		const city = catalogState.City?.find((o: any) => o.id === parseInt(dataModel.ciudadId, 10));
		if (city) {
			setCiudadValue(city);
			formularioSede.setValue('ciudadId', dataModel.ciudadId);
		}
	}

	const eliminarRegistroSede = async(idSede: string) => {
		const confirmAlert = await NotificationService.confirmAlert(
			`¿Está seguro que desea eliminar el registro?`,
		);

		if (confirmAlert) {
			eliminarSede(idSede, idEntidad, usuario?.USR_ID);
		} 

	}

	return (
		<Grid container>
			<CHPaperNew>
				<table className="table">
					<thead>
						<tr>
							<th className="detailHead"> Nombre sede</th>
							<th className="enrollmentNumberHead">Ciudad</th>
							<th className="actionsHead">Acciones</th>
						</tr>
					</thead>
					<tbody>
						{educationalEntitiesState.sedesEntidadEducativa?.map(
							(row, index) => (
								<tr key={`table-campus${index}`}>
									<td className="enrollmentNumberBody tab-space-02">
										{row.nombreSede}
									</td>
									<td className="addressBody tab-space-01">
										{row.nombreCiudad}
									</td>
									<td data-label="acciones" className="tab-space-01">
										<span aria-label="Detalle" onClick={() => openCreateOrUpdateModal(row,'editar')}><span className="ico fch-Hoja-Vida-0-1 icoMid-3" /></span>
										<span aria-label="Eliminar" onClick={() => eliminarRegistroSede(row.idSedeEntidad)}><span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} /></span>
									</td>
								</tr>
							),
						)}
					</tbody>
				</table>
				<div className="buttons">
					<Button
						className={classes.button}
						variant="outlined"
						type="submit"
						onClick={() => openCreateOrUpdateModal(null, 'agregar')}
					>
						AGREGAR SEDES
					</Button>
				</div>
				<CreateCampus
					isOpen={isOpenCreate}
					dataModel={dataModal}
					ciudadValue={ciudadValue}
					formularioSede={formularioSede}
					listaCiudades={catalogState.City}
					onClose={onClose}
					guardarSede={guardarSede}
					actualizarSede={actualizarSede}
					setCiudadValue={setCiudadValue}
				/>
			</CHPaperNew>
		</Grid>
	);
}

const mapStateToProps = (state: AppState) => ({
	catalogState: state.catalog,
	educationalEntitiesState: state.educationalEntities,
});

const mapDispatchToProps = (dispatch: any) => ({
	obtenerSedesPorEntidad: (idEntidad: string) =>
		dispatch(startGetSedesEntidadEducativa(idEntidad)),
	guardarSede: (dataSede: SedeEntidadEducativa) =>
		dispatch(startAddCampus(dataSede)),
	actualizarSede: (dataSede: SedeEntidadEducativa) =>
		dispatch(startUpdateCampus(dataSede)),
	eliminarSede: (idSede: string, idEntidad: string, idUsuario: string) =>
		dispatch(startDeleteCampus(idSede, idEntidad, idUsuario)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Campus);