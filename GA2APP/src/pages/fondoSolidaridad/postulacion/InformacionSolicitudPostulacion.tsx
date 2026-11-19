import React, { useEffect, useState } from 'react';
import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { Controller } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import {
	CATEGORIES,
	ESTADOCIVILDTO,
	CITYDTO,
	DEPARTAMENTODTO,
	FORCEDTO,
	GRADEDTO,
	PHONETYPE,
} from '../../../types/catalog.type';
import apiCatalog from '../../../services/api-catalog.service';
import { CatalogState } from '../../../reducers/catalog.reducer';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import { LoadingComponent } from '../../../components/loading/loading.component';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
	}),
);

const InformacionSolicitudPostulacion = (props: props) => {
	const classes = useStyles();
	const api = apiCatalog();
	const apiFondoSolidaridad  = ApiFondoSolidaridad();
	const [ciudadLugarExpedicion, setCiudadLugarExpedicion] = useState<CITYDTO[]>([]);
	const [ciudad, setCiudad] = useState<CITYDTO[]>([]);
	const [datosTipoSolicitud,setTipoSolicitud] = useState<any>(null);
    const [datosTipoModalidad,setTipoModalidad] = useState<any>(null);
	const [datosSubModalidad,setDatosSubModalidad] = useState<any>(null);
	const [loading, setLoading] = useState(false);
	
	const {
		cliente,
		catalogState,
		tipoSolicitud,
		tipoModalidad,
		tipoSubModalidad,
		formularioPostulacion
	} = props;

	useEffect(() => {
	    setLoading(true);
		if(cliente !== null){
			obtenerCiudadesLugarExpedicion(cliente?.idDepartamentoExpedicionDocumento);
			obtenerCiudadesResidencia(cliente?.idDepartamentoResidencia);
			consultarTipoSolicitud();
			consultarTipoModalidad(tipoSolicitud);
			consultarTipoSubModalidad(tipoModalidad);
			formularioPostulacion.setValue('numeroDocumento', cliente?.identificacion || '');
			formularioPostulacion.setValue('idTipoSolicitud', tipoSolicitud);
			formularioPostulacion.setValue('idModalidad', tipoModalidad);
			formularioPostulacion.setValue('idSubModalidad', tipoSubModalidad);
			// Guardar unidad ejecutora
            const unidadEjecutora = catalogState.ExecutingUnit.find(item => item.id === cliente?.idUnidadEjecutora);
			formularioPostulacion.setValue('unidadEjecutora', unidadEjecutora?.nombre);
			// Guardar categoría
			const categoria = catalogState.categories.find(item => item.id === cliente?.idCategoria);
			formularioPostulacion.setValue('categoria', categoria?.descripcion);
			
			setLoading(false);
		}
	}, [cliente]);

	const obtenerCiudadesLugarExpedicion = async (idDepto: number) => {
		const value = String(idDepto);
		const response = await api.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
			setCiudadLugarExpedicion(datos);
		}
	};

	const obtenerCiudadesResidencia = async (idDepto: number) => {
		const value = String(idDepto);
		const response = await api.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
			setCiudad(datos);
		}
	};

	const consultarTipoSolicitud = async () => {
        await apiFondoSolidaridad
            .getConsultarTipoSolicitudPostulante()
            .then(async response => {
                if(response.data){
                    setTipoSolicitud(response.data);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const consultarTipoModalidad = async (idTipoSolicitud: number) => {
        await apiFondoSolidaridad
            .getConsultarTipoModalidadPostulante(idTipoSolicitud)
            .then(async response => {
                if(response.data){
                    setTipoModalidad(response.data);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

	const consultarTipoSubModalidad = async (idTipoModalidad: number) => {
		await apiFondoSolidaridad
		.getConsultarTipoSubModalidadPostulante(idTipoModalidad)
		.then(async response => {
			if(response.data){
				setDatosSubModalidad(response?.data);
			}
		})
		.catch((e) => {
			toast(`Error al consultar tipo submodalidad ${e.message}`, { type: toast.TYPE.ERROR });
		});
    };

	return (
       <>
	   <LoadingComponent loading={loading} />
		<CHPaper title="Información solicitud">
			<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Nombres"
						InputLabelProps={{ shrink: true }}
						value={`${cliente?.primerNombre} ${cliente?.segundoNombre}`}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Apellidos"
						InputLabelProps={{ shrink: true }}
						value={`${cliente?.primerApellido} ${cliente?.segundoApellido}`}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
						<Controller
                            name="numeroDocumento"
                            control={formularioPostulacion.control}
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
									isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    label="Número Documento"
                                    value={value}
                                />
                            )}
                        />
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idDepartamentoExpedicionDocumento"
						label="Departamento expedición"
						value={
							cliente?.idDepartamentoExpedicionDocumento || ''
						}
						open={false}
						readOnly
					>
						{catalogState?.departments.map((option: DEPARTAMENTODTO) => (
							<MenuItem key={option.id} value={option.codigo}>
								{option.nombre}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
				</Grid>
				<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idCiudadExpedicionDocumento"
						label="Ciudad expedición"
						value={
							cliente?.idCiudadExpedicionDocumento || ''
						}
						open={false}
						readOnly
					>
						{ciudadLugarExpedicion?.map((option: CITYDTO) => (
							<MenuItem key={option.id} value={option.codigo}>
								{option.nombre}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
			
				<Grid className={classes.column} item xs={12} md={3}>
				    <CHSelect
						name="idFuerza"
						label="Fuerza"
						value={
							cliente?.idFuerza || 0
						}
						open={false}
						readOnly
					>
						{catalogState?.Force.map((option: FORCEDTO) => (
							<MenuItem key={option.id} value={option.id}>
								{option.descripcion}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
							name="idGrado"
							label="Grado"
							value={
								cliente?.idGrado || 0
							}
							open={false}
							readOnly
						>
							{catalogState?.Grade.map((option: GRADEDTO) => (
								<MenuItem key={option.id} value={option.id}>
									{option.descripcion}
								</MenuItem>
							))}
					</CHSelect>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
						<CHSelect
							name="idCategoria"
							label="Categoría"
							value={
								cliente?.idCategoria || 0
							}
							open={false}
							readOnly
						>
							{catalogState?.categories.map((option: CATEGORIES) => (
								<MenuItem key={option.id} value={option.id}>
									{option.descripcion}
								</MenuItem>
							))}
					   </CHSelect>
					</Grid>
				</Grid>
				<Grid container>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="estadoCivil"
						label="Estado Civil"
						value={
							cliente?.estadoCivil || 0
						}
						open={false}
						readOnly
					>
						{catalogState?.civilstates.map((option: ESTADOCIVILDTO) => (
							<MenuItem key={option.id} value={option.id}>
								{option.descripcion}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Dirección"
						InputLabelProps={{ shrink: true }}
						value={cliente?.direccion || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Barrio"
						InputLabelProps={{ shrink: true }}
						value={cliente?.barrio || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idDepartamentoResidencia"
						label="Departamento Residencia"
						value={
							cliente?.idDepartamentoResidencia || ''
						}
						open={false}
						readOnly
					>
						{catalogState?.departments.map((option: DEPARTAMENTODTO) => (
							<MenuItem key={option.id} value={option.codigo}>
								{option.nombre}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
				</Grid>
				<Grid container>
				   <Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idCiudadResidencia"
						label="Ciudad Residencia"
						value={
							cliente?.idCiudadResidencia || ''
						}
						open={false}
						readOnly
					>
						{ciudad.map((option: CITYDTO) => (
							<MenuItem key={option.id} value={option.codigo}>
								{option.nombre}
							</MenuItem>
						))}
					</CHSelect>
				  </Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHSelect
						name="idTipoTelefono"
						label="Tipo Teléfono"
						value={
							cliente?.idTipoTelefono || ''
						}
						open={false}
						readOnly
					>
						{catalogState?.phoneTypes.map((option: PHONETYPE) => (
							<MenuItem key={option.idTipoTelefono} value={option.idTipoTelefono}>
								{option.descripcionTipoTelefono}
							</MenuItem>
						))}
					</CHSelect>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Número de teléfono"
						InputLabelProps={{ shrink: true }}
						value={cliente?.telefono || ''}
					/>
				</Grid>
				<Grid className={classes.column} item xs={12} md={3}>
					<CHTextField
						isReadOnly
						label="Correo Electrónico"
						InputLabelProps={{ shrink: true }}
						value={cliente?.correo || ''}
					/>
				</Grid>
			</Grid>
			<Grid container>
				<Grid className={classes.column} item xs={6} md={6}>
					<Controller
                            name="idTipoSolicitud"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { value } }) => (
                                <CHSelect
                                    label="Tipo de Solicitud"
                                    value={value}
									readOnly
                                >
                                    {datosTipoSolicitud?.map(
                                        (option: any) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.id}
                                            >
                                                {option.nombre}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
				</Grid>
				<Grid className={classes.column} item xs={6} md={6}>
						<Controller
                            name="idModalidad"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { value } }) => (
                                <CHSelect
                                    label="Modalidad (Modelo)"
                                    value={value}
									readOnly
                                >
                                    {datosTipoModalidad?.map(
                                        (option: any) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.id}
                                            >
                                                {option.nombre}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
				</Grid>
			</Grid>
			<Grid container>
				<Grid className={classes.column} item xs={12} md={12}>
						<Controller
                            name="idSubModalidad"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({ field: { value } }) => (
                                <CHSelect
                                    label="Sub-Modalidad (Calidad Postulación)"
                                    value={value}
									readOnly
                                >
                                    {datosSubModalidad?.map(
                                        (option: any) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.id}
                                            >
                                                {option.nombre}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
				</Grid>
			</Grid>
		</CHPaper>
		</>
	);
};

interface LinkMapStateToProps {
	catalogState: CatalogState;
	cliente: any;
	tipoSolicitud: number;
	tipoModalidad: number;
	tipoSubModalidad: number;
	formularioPostulacion: any;
}

type props = LinkMapStateToProps;

export default connect(null, null)(InformacionSolicitudPostulacion);
