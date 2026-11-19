import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { connect } from 'react-redux';
import { AppState } from '../../../store/rootReducer';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';
import { startGetAllValoresCatalogoPorNombre } from '../../../actions/catalog.action';
import { startGetBloqueos, startGetEducationalEntitiesDetail, startLockOrUnlockEntidades } from '../../../actions/educationalEntities.actions';
import { CatalogState } from '../../../reducers/catalog.reducer';
import LockEntities from './modal.lock.educationalEntities';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { BloqueosEntidadesEducativas } from '../../../types/educational.entities.type';
import UtilService from '../../../utils/util.service';

const bloqueos = [
	{ id: 74, descripcion: 'BLOQUEO' },
	{ id: 75, descripcion: 'DESBLOQUEO' },
];

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '50px',
			textAlign: 'left',
			color: '#42b7ef',
			borderColor: 'fec917',
		},
		processButton: {
			backgroundColor: '#42b7ef',
			color: '#fff',
			width: '12%',
			marginLeft: '69%',
		},
		processButton2: {
			backgroundColor: '#fec917',
			color: '#1c3f8b',
			width: '12%',
			marginLeft: '1%',
		},
		icon: {
			padding: '8px',
			textAlign: 'inherit',
			color: '#42b7ef',
			margin: '10px',
		},
		iconAmarillo: {
			padding: '8px',
			textAlign: 'inherit',
			color: '#1c3f8b',
			margin: '10px',
		},
		iconRed: {
			padding: '8px',
			textAlign: 'inherit',
			color: '#FF0000',
			margin: '10px',
		},
		marginInput: {
			margin: '2%',
			marginLeft: '5%',
		},
		button: {
			backgroundColor: '#42b7ef',
			color: '#fff',
			width: '20%',
			marginRight: '1%',
		},
		observaciones: {
            textAlign: 'justify',
            whiteSpace: 'pre-line',
            paddingBottom: '20px',
            paddingRight: '12px',
        }
	}),
);

function BlocksEntities({
	idEntidad,
	educationalEntitiesState,
	catalogState,
	obtenerCausales,
	obtenerBloqueosEntidad,
	bloquearDesbloquearEntidad,
	GetEducationalEntitiesDetail,
}: {
	idEntidad: string,
	educationalEntitiesState: EducationalEntitiesState;
	catalogState: CatalogState;
	obtenerCausales: (nombreCatalogo: string) => void;
	obtenerBloqueosEntidad: (idEntidad: string) => void;
	bloquearDesbloquearEntidad: (data: BloqueosEntidadesEducativas, lock: boolean) => void;
	GetEducationalEntitiesDetail:(idEntidad:string) => void;
}): any {
	const [lastOperationText, setLastOperationText] = useState('');
	const [open, setOpen] = useState(false);
	const classes = useStyles();

	useEffect(() => {
		obtenerBloqueosEntidad(idEntidad);
	}, []);

	useEffect(() => {
		const bloqueosClone = [...educationalEntitiesState.bloqueos];
		const ultimoRegistro = bloqueosClone[0];
		if (ultimoRegistro !== null && ultimoRegistro?.nombreOperacion === 'BLOQUEO') {
			setLastOperationText('DESBLOQUEO');
			GetEducationalEntitiesDetail(idEntidad);
		} else {
			setLastOperationText('BLOQUEO');
			GetEducationalEntitiesDetail(idEntidad);
		}
	}, [educationalEntitiesState.bloqueos])

	const handleOpenAddress = () => {
		setOpen(true);
	};

	const onClose = () => {
		GetEducationalEntitiesDetail(idEntidad);
		setOpen(false);
		GetEducationalEntitiesDetail(idEntidad);
	};

	return (
		<Grid container>
			<CHPaperNew>
				<table className="table">
					<thead>
						<tr>
							<th className="head">Tipo operación</th>
							<th className="tipoOperacion1">Causal bloqueo</th>
							<th className="tipoOperacion1">Observaciones</th>
							<th className="column2">Funcionario bloqueo</th>
							<th className="column2">Fecha</th>
						</tr>
					</thead>
					<tbody>
						{educationalEntitiesState.bloqueos?.map((row, index) => (
							<tr key={`table-realStateRegistrations${index}`}>
								<td className="tab-space-01">
									{row.nombreOperacion}
								</td>
								<td className="tab-space-01">
									{row.nombreCausal}
								</td>
								<td className={classes.observaciones}>
									{row.observaciones}
								</td>
								<td className="tab-space-01">
									{row.userName}
								</td>
								<td className="tab-space-01">
									{UtilService.getFullDateWithTimeDirection(new Date(String(row?.fechaBloqueo).toString()))}
								</td>
							</tr>
						))}
					</tbody>
				</table>
				<div className="buttons">
					<Button
						className={classes.button}
						variant="outlined"
						onClick={handleOpenAddress}
					>
						{lastOperationText}
					</Button>
				</div>
			</CHPaperNew>
			<LockEntities
				idEntidad={idEntidad}
				tiposOperacion={bloqueos}
				causalesBloqueo={catalogState.valorCatalogo}
				tipoOperacion={lastOperationText}
				isOpen={open}
				onClose={onClose}
				obtenerCausales={obtenerCausales}
				bloquearDesbloquearEntidad={bloquearDesbloquearEntidad}
				getEducationalEntitiesDetail = {GetEducationalEntitiesDetail} />
		</Grid>
	);
}

const mapStateToProps = (state: AppState) => ({
	educationalEntitiesState: state.educationalEntities,
	catalogState: state.catalog
});

const mapDispatchToProps = (dispatch: any) => ({
	obtenerBloqueosEntidad: (idEntidad: string) =>
		dispatch(startGetBloqueos(idEntidad)),
	obtenerCausales: (nombreCatalogo: string) =>
		dispatch(startGetAllValoresCatalogoPorNombre(nombreCatalogo)),
	bloquearDesbloquearEntidad: (data: BloqueosEntidadesEducativas, lock: boolean) =>
		dispatch(startLockOrUnlockEntidades(data, lock)),
		GetEducationalEntitiesDetail: (idEntidad: string) =>
		dispatch(startGetEducationalEntitiesDetail(idEntidad))

});

export default connect(mapStateToProps, mapDispatchToProps)(BlocksEntities);