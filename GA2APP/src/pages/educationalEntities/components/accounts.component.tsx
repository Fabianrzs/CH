import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { AppState } from '../../../store/rootReducer';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';
import { startGetAllEntidadBancaria, startGetAllValoresCatalogoPorNombre } from '../../../actions/catalog.action';
import CreateAccount from './createAccount';
import { CatalogState } from '../../../reducers/catalog.reducer';
import {
	startGetCuentas,
	startAddCuenta,
	startDeleteCuenta,
	startUpdateFideicomisos,
} from '../../../actions/educationalEntities.actions';
import { CuentaEntidadesEducativas, Fidecomiso, FidecomisoInitial } from '../../../types/educational.entities.type';
import UtilService from '../../../utils/util.service';
import NotificationService from '../../../services/notification.service';

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

function Accounts({
	idEntidad,
	educationalEntitiesState,
	catalogState,
	usuario,
	obtenerCuentasEntidad,
	actualizarCuenta,
	borrarCuenta
}: {
	idEntidad: string;
	educationalEntitiesState: EducationalEntitiesState;
	catalogState: CatalogState;
	usuario: any;
	obtenerCuentasEntidad: (idEntidad: string) => void;
	actualizarCuenta: (cuentaEntidad: Fidecomiso, idEntidad: string) => void;
	borrarCuenta: (idCuentaBancaria: string, idEntidad: string, idUsuario: string) => void;
}): any {
	const classes = useStyles();
	const [isOpenCreate, setIsOpenCreate] = useState<boolean>(false);
	const [dataModal, setDataModal] = useState<any>({});

	const formularioFidecomiso = useForm<Fidecomiso>({
		defaultValues: FidecomisoInitial,
	});

	const datos = () => {
		obtenerCuentasEntidad(idEntidad);
	}

	useEffect(() => {
		console.log(educationalEntitiesState)
		obtenerCuentasEntidad(idEntidad);

	}, []);

	const onClose = () => {
		formularioFidecomiso.reset();
		setIsOpenCreate(false);
	};

	const openCreateOrUpdateModal = (data: any = null, tipoFuncion: string) => {
		if(tipoFuncion === 'editar'){
			setDataModal(data);
			loadData(data);
		}else{
			setDataModal(null);
		}
		setIsOpenCreate(true);
	};

	
	const loadData = (dataModel: any) => {
		formularioFidecomiso.setValue('idTipoIdentificacion', dataModel.idTipoIdentificacion);
		formularioFidecomiso.setValue('nroIdentificacion', dataModel.nroIdentificacion);
		formularioFidecomiso.setValue('nombreRazonSocial', dataModel.nombreRazonSocial);
	}

	const DeleteFidecomiso = async(data: any) => {
		const confirmAlert = await NotificationService.confirmAlert(
			`¿Está seguro que desea eliminar el registro?`,
		);

		if (confirmAlert) {
			borrarCuenta(data, idEntidad, usuario?.USR_ID);
		}
	}

	return (
		<Grid container>
			<CHPaperNew>
				<table className="table">
					<thead>
						<tr>
							<th className="head">Tipo de Identificacion</th>
							<th className="head">N° Identificación</th>
							<th className="head">Razón Social</th>
							<th className="head">Fecha Creacion</th>
						</tr>
					</thead>
					<tbody>
					{educationalEntitiesState?.cuentas?.map((row: any, index: any) => (
							<tr key={`table-realStateRegistrations${index}`}>
								<td className="tab-space-02">
									{row?.tipoDocumento}
								</td>
								<td className="tab-space-02">
									{row?.nroIdentificacion}
								</td>
								<td className="tab-space-02">
									{row?.nombreRazonSocial}
								</td>
								<td className="tab-space-01">
								    {UtilService.getFullDateWithTimeDirection(new Date(String(row?.fecCreacion).toString()))}
								</td>
								<td data-label="acciones" className="tab-space-01">
									<span aria-label="Detalle" onClick={() => openCreateOrUpdateModal(row, 'editar')}><span className="ico fch-Hoja-Vida-0-1 icoMid-3" /></span>
									<span aria-label="Eliminar" onClick={() => DeleteFidecomiso(row.enE_FIDEICOMISO_ID!)}><span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} /></span>
								</td>
							</tr>
						))}
					</tbody>
				</table>
				<div className="buttons">
					<Button
						className={classes.button}
						variant="outlined"
						onClick={() => openCreateOrUpdateModal(null,'agregar')}
					>
						AGREGAR FIDEICOMISO
					</Button>
				</div>
			</CHPaperNew>
			<CreateAccount
				isOpen={isOpenCreate}
				dataModel={dataModal}
				catalogState={catalogState}
				onClose={onClose}
				Fidecomiso={datos}
				idEntidad={idEntidad}
				formularioFidecomiso={formularioFidecomiso}
				actualizarCuenta={actualizarCuenta}
			/>
		</Grid>
	);
}

const mapStateToProps = (state: AppState) => ({
	catalogState: state.catalog,
	educationalEntitiesState: state.educationalEntities,
});

const mapDispatchToProps = (dispatch: any) => ({
	obtenerCuentasEntidad: (idEntidad: string) =>
		dispatch(startGetCuentas(idEntidad)),
	obtenerEntidadesBancarias: () =>
		dispatch(startGetAllEntidadBancaria()),
	obtenerTipoCuenta: (nombreCatalogo: string) =>
		dispatch(startGetAllValoresCatalogoPorNombre(nombreCatalogo)),
	crearCuenta: (cuentaEntidad: CuentaEntidadesEducativas) =>
		dispatch(startAddCuenta(cuentaEntidad)),
	actualizarCuenta: (cuentaEntidad: Fidecomiso, idEntidad: string) =>
		dispatch(startUpdateFideicomisos(cuentaEntidad, idEntidad)),
	borrarCuenta: (idCuentaBancaria: string, idEntidad: string, idUsuario: string) =>
		dispatch(startDeleteCuenta(idCuentaBancaria, idEntidad, idUsuario)),
});

export default connect(mapStateToProps, mapDispatchToProps)(Accounts);