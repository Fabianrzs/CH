import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { connect } from 'react-redux';
import { useForm } from 'react-hook-form';
import { AppState } from '../../../store/rootReducer';
import { ProgramaEducativo, ProgramasEducativos } from '../../../types/educational.entities.type';
import {
	startGetAllNivelEducativo,
	startGetAllValoresCatalogoPorNombre,
} from '../../../actions/catalog.action';
import {
	startAddPrograms,
	startUpdatePrograms,
	startDeleteProgram,
	startGetProgramasEducativos,
} from '../../../actions/educationalEntities.actions';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import CreateProgram from './createProgram';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';
import NotificationService from '../../../services/notification.service';
import { PROGRAMAPORENTIDADDTO } from '../../../types/catalog.type';

const useStyles = makeStyles(() =>
	createStyles({
		iconRed: {
			padding: '8px',
			textAlign: 'inherit',
			color: '#FF0000',
			margin: '10px',
		},
		button: {
			backgroundColor: '#1C3F8B',
			color: '#fff',
			width: '20%',
			marginRight: '1%',
		},
	}),
);

function Programs({
	catalogState,
	idEntidad,
	usuario,
	educationalEntitiesState,
	obtenerTipoPrograma,
	obtenerNivelEducativo,
	obtenerProgramasEducativosPorEntidad,
	guardarProgramaEducativo,
	actualizarProgramaEducativo,
	eliminarProgramaEducativo,
}: {
	catalogState: CatalogState;
	idEntidad: string;
	usuario: any;
	educationalEntitiesState: EducationalEntitiesState;
	obtenerTipoPrograma: (nombreCatalogo: string) => void,
	obtenerNivelEducativo: () => void;
	obtenerProgramasEducativosPorEntidad: (idEntidad: string) => void;
	guardarProgramaEducativo: (data: ProgramasEducativos) => void;
	actualizarProgramaEducativo: (data: ProgramaEducativo) => void;
	eliminarProgramaEducativo: (idProgramaEducativo: string, idEntidad: string, idUsuario: string) => void;
}): any {

	const classes = useStyles();
	const [isOpenCreate, setIsOpenCreate] = useState<boolean>(false);
	const [dataModal, setDataModal] = useState<any>({});
	const formularioPrograma = useForm<ProgramasEducativos>({
		defaultValues: {
			idEntidad: idEntidad,
			idTipoPrograma: 0,
			nivelEducativoId: '',
			programa: '',
			creadoPor: '',
			modificacionPor: '',
			estado: 1,
		},
	});

	useEffect(() => {
		obtenerProgramasEducativosPorEntidad(idEntidad);
	}, []);

	const onClose = () => {
		formularioPrograma.reset();
		setIsOpenCreate(false);
	};

	const openCreateOrUpdateModal = (data: any = null, tipoFuncionalidad: string) => {

		if(tipoFuncionalidad === 'editar'){
			setDataModal(data);
			loadData(data);
		}else {
			setDataModal(null);
		}
	
		setIsOpenCreate(true);
	};

	const loadData = (dataModel: PROGRAMAPORENTIDADDTO) => {
		formularioPrograma.setValue('idTipoPrograma', parseInt(dataModel.catalogoValorId, 10));
		formularioPrograma.setValue('programa', dataModel.programa);
		formularioPrograma.setValue('nivelEducativoId', dataModel.nivelEducativoId);
		formularioPrograma.setValue('estado', dataModel.estado? 1 : 0);
	}

	const eliminarPrograma = async(idProgramaEducativo: string) => {
		const confirmAlert = await NotificationService.confirmAlert(
			`¿Está seguro que desea eliminar el registro?`,
		);

		if (confirmAlert) {
			eliminarProgramaEducativo(idProgramaEducativo,idEntidad, usuario?.USR_ID);
		} 

	}

	return (
		<Grid container>
			<CHPaperNew>
				<table className="table">
					<thead>
						<tr>
							<th className="detailHead">Razón social entidad</th>
							<th className="enrollmentNumberHead">Programa</th>
							<th className="addressHead">Nivel educativo</th>
							<th className="addressHead">Estado</th>
							<th className="actionsHead">Acciones</th>
						</tr>
					</thead>
					<tbody>
						{educationalEntitiesState.programasEducativos?.map(
							(row, index) => (
								<tr
									key={`table-realStateRegistrations${index}`}
								>
									<td className="tab-space-03">
										{row.razonSocialEntidad}
									</td>
									<td className="tab-space-02">
										{row.programa}
									</td>
									<td className="tab-space-01">
										{row.nivelEducativo}
									</td>
									<td className="tab-space-01">
										{row.estado ? 'Activo' : 'Inactivo'}
									</td>
									<td data-label="acciones" className="tab-space-01">
										<span aria-label="Detalle" onClick={() => openCreateOrUpdateModal(row, 'editar')}><span className="ico fch-Hoja-Vida-0-1 icoMid-3" /></span>
										<span aria-label="Eliminar" onClick={() => eliminarPrograma(row.idPrograma!)}><span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} /></span>
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
						onClick={() => openCreateOrUpdateModal(null,'agregar')}
					>
						AGREGAR PROGRAMA
					</Button>
				</div>
			</CHPaperNew>
			<CreateProgram
				isOpen={isOpenCreate}
				onClose={onClose}
				idEntidad={idEntidad}
				listaTipoPrograma={catalogState.valorCatalogo}
				listaNivelEducativo={catalogState.nivelEducativo}
				dataModel={dataModal}
				obtenerTipoPrograma={obtenerTipoPrograma}
				obtenerNivelEducativo={obtenerNivelEducativo}
				guardarProgramaEducativo={guardarProgramaEducativo}
				actualizarProgramaEducativo={actualizarProgramaEducativo}
				formularioPrograma={formularioPrograma}
				obtenerProgramasEducativosPorEntidad={obtenerProgramasEducativosPorEntidad}
			/>
		</Grid>
	);
}

const mapStateToProps = (state: AppState) => ({
	educationalEntitiesState: state.educationalEntities,
	catalogState: state.catalog,
});

const mapDispatchToProps = (dispatch: any) => ({
	obtenerTipoPrograma: (nombreCatalogo: string) =>
		dispatch(startGetAllValoresCatalogoPorNombre(nombreCatalogo)),
	obtenerNivelEducativo: () =>
		dispatch(startGetAllNivelEducativo()),
	obtenerProgramasEducativosPorEntidad: (idEntidad: string) =>
		dispatch(startGetProgramasEducativos(idEntidad)),
	guardarProgramaEducativo: (data: ProgramasEducativos) =>
		dispatch(startAddPrograms(data)),
	actualizarProgramaEducativo: (data: ProgramaEducativo) =>
		dispatch(startUpdatePrograms(data)),
	eliminarProgramaEducativo: (idProgramaEducativo: string, idEntidad: string, idUsuario: string) =>
		dispatch(startDeleteProgram(idProgramaEducativo, idEntidad, idUsuario)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Programs);