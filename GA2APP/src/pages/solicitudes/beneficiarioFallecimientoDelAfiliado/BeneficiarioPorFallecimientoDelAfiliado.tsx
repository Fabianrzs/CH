import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CiudadPreferenciaParaRecibirVivienda from '../ciudadPreferenciaParaRecibirVivienda/CiudadPreferenciaParaRecibirVivienda';
import FirmaSolicitud from '../firmaSolicitud/FirmaSolicitud';
import IdentificacionPostulantesNucleoFamiliar from '../identificacionPostulantesNucleoFamiliar/IdentificacionPostulantesNucleoFamiliar';
import ManifestacionesAutorizaciones from '../manifestacionesAutorizaciones/ManifestacionesAutorizaciones';
import InformacionSolicitud from '../informacionSolicitud/InformacionSolicitud';
import { ResponseSeveranceModel } from '../../../models/requests/response-severance.model';
import { AppState } from '../../../store/rootReducer';
import {
	startGetAllDepartments,
	startGetAllTypesForces,
	startGetCategory,
	startGetAllCivilState,
	startGetPhoneTypes,
} from '../../../actions/catalog.action';

const BeneficiarioPorFallecimientoDelAfiliado = (props: propsType) => {
	const dispatch = useDispatch();
	const catalogState = useSelector((state: AppState) => state.catalog);
	const { responseSolicitud } = props;

	useEffect(() => {
		dispatch(startGetAllTypesForces());
		dispatch(startGetCategory());
		dispatch(startGetAllDepartments());
		dispatch(startGetPhoneTypes());
	}, []);

	const obtenerEstadosCiviles = (civilId: string) =>  dispatch(startGetAllCivilState(civilId));

	return (
		<>
			<InformacionSolicitud
				listaForce={catalogState.Force}
				listaGrade={catalogState.Grade}
				listaCategorias={catalogState.categories}
				listaEstadoCivil={catalogState.civilstates}
				listaDepartamentos={catalogState.departments}
				listaTipoTelefonos={catalogState.phoneTypes}
				obtenerEstadosCiviles={(props: string) => obtenerEstadosCiviles(props)}
				responseSolicitud={responseSolicitud}
			/>
			<IdentificacionPostulantesNucleoFamiliar />
			<ManifestacionesAutorizaciones />
			<CiudadPreferenciaParaRecibirVivienda />
			<FirmaSolicitud />
		</>
	);
};

type propsType = {
    responseSolicitud: ResponseSeveranceModel | undefined;
};

export default BeneficiarioPorFallecimientoDelAfiliado;
