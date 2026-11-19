import React, { useEffect } from 'react';
import {
	Delete,
	Edit,
} from '@material-ui/icons';
import {
	createStyles,
	Grid,
	IconButton,
	makeStyles,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
} from '@material-ui/core';
import {
	MatriculaInmobiliariaModel,
} from '../../../../models/matricula-inmobiliaria.model';

const useStyles = makeStyles(() =>
	createStyles({
		columnButtonAdd: {
			textAlign: 'right',
		},
		mensajeSinRegistros: {
			padding: '20px',
			textAlign: 'center',
		},
	}),
);

const normalizarTexto = (
	valor?: string,
): string =>
	String(valor ?? '')
		.normalize('NFD')
		.replace(/[\u0300-\u036f]/g, '')
		.trim()
		.toUpperCase();

const obtenerNumeroMatricula = (
	option: any,
): string => {
	if (option?.numeroMatricula) {
		return String(option.numeroMatricula);
	}

	if (option?.numeroMatriculaInmobiliaria) {
		return String(
			option.numeroMatriculaInmobiliaria,
		);
	}

	if (option?.matricula) {
		return String(option.matricula);
	}

	if (option?.numeroDeMatricula) {
		return String(option.numeroDeMatricula);
	}

	return '-';
};

const obtenerDireccion = (
	option: any,
): string => {
	if (option?.direccion) {
		return String(option.direccion);
	}

	if (option?.direccionInmueble) {
		return String(option.direccionInmueble);
	}

	if (option?.ubicacion?.direccion) {
		return String(option.ubicacion.direccion);
	}

	return '';
};

const obtenerDepartamento = (
	option: any,
): string => {
	if (option?.departamento?.descripcion) {
		return String(
			option.departamento.descripcion,
		);
	}

	if (option?.departamentoDescripcion) {
		return String(
			option.departamentoDescripcion,
		);
	}

	if (typeof option?.departamento === 'string') {
		return option.departamento;
	}

	return '';
};

const obtenerCiudad = (
	option: any,
): string => {
	if (option?.ciudad?.descripcion) {
		return String(option.ciudad.descripcion);
	}

	if (option?.ciudadDescripcion) {
		return String(option.ciudadDescripcion);
	}

	if (typeof option?.ciudad === 'string') {
		return option.ciudad;
	}

	return '';
};

const obtenerEstadoMatricula = (
	option: any,
): string => {
	if (option?.esPrincipal === true) {
		return 'Principal';
	}

	if (option?.esPrincipal === false) {
		return 'Relacionado';
	}

	const estado = normalizarTexto(
		option?.estado ??
			option?.tipoMatricula ??
			option?.estadoMatricula,
	);

	if (estado === 'PRINCIPAL') {
		return 'Principal';
	}

	if (
		estado === 'RELACIONADO' ||
		estado === 'RELACIONADA'
	) {
		return 'Relacionado';
	}

	return '';
};

const TablaInformacionDelInmueble = ({
	matriculaInmobiliariaModel,
	siEsAprobadaSolcitud,
	responseSolicitud,
	seleccionarRegistro,
	eliminarRegistro,
	campoSoloLecturaRadicacionAprobada,
	actividadActual,
}: {
	matriculaInmobiliariaModel: any;
	siEsAprobadaSolcitud: any;
	responseSolicitud: any;
	seleccionarRegistro: any;
	eliminarRegistro: any;
	campoSoloLecturaRadicacionAprobada: boolean;
	actividadActual?: string;
}) => {
	const classes = useStyles();

	const listaMatriculas = Array.isArray(
		matriculaInmobiliariaModel,
	)
		? matriculaInmobiliariaModel
		: [];


	const formularioSoloLectura =
		(
			(
				siEsAprobadaSolcitud ||
				responseSolicitud?.secciones?.includes(
					'INCONSISTENCIA',
				)
			) &&
			responseSolicitud?.soL_RECHAZADA !== '1'
		) ||
		campoSoloLecturaRadicacionAprobada;

	const mostrarAcciones =
		!formularioSoloLectura;

	const nombreTarea =
		responseSolicitud?.nombreTarea ?? '';

	const esActividadRadicacion =
		normalizarTexto(nombreTarea).startsWith(
			'RADICACION',
		);

	const puedeEditarRegistro = (
		option: any,
	): boolean =>
		esActividadRadicacion &&
		!formularioSoloLectura &&
		!option?.bloquearEdicion;

	const puedeEliminarRegistro =
		(): boolean =>
			esActividadRadicacion &&
			!formularioSoloLectura;

	return (
		<>
			<h3>
				Lista de matrículas inmobiliarias
			</h3>

			<TableContainer component={Paper}>
				<Grid
					className={
						classes.columnButtonAdd
					}
					item
					xs={12}
					md={12}
				>
					<Table aria-label="simple table">
						<TableHead>
							<TableRow>
								<TableCell>
									Número Matrícula
								</TableCell>

								<TableCell>
									Estado
								</TableCell>

								<TableCell>
									Dirección
								</TableCell>

								<TableCell>
									Departamento
								</TableCell>

								<TableCell>
									Ciudad
								</TableCell>

								{mostrarAcciones && (
									<TableCell align="center">
										Acciones
									</TableCell>
								)}
							</TableRow>
						</TableHead>

						<TableBody>
							{listaMatriculas.length ===
								0 && (
								<TableRow>
									<TableCell
										colSpan={
											mostrarAcciones
												? 6
												: 5
										}
										className={
											classes.mensajeSinRegistros
										}
									>
										No se encontraron
										matrículas
										inmobiliarias.
									</TableCell>
								</TableRow>
							)}

							{listaMatriculas.map(
								(
									option: MatriculaInmobiliariaModel,
									index: number,
								) => {
									const numeroMatricula =
										obtenerNumeroMatricula(
											option,
										);

									const direccion =
										obtenerDireccion(
											option,
										);

									const departamento =
										obtenerDepartamento(
											option,
										);

									const ciudad =
										obtenerCiudad(
											option,
										);

									const estado =
										obtenerEstadoMatricula(
											option,
										);

									return (
										<TableRow
											key={`${numeroMatricula}-${index}`}
										>
											<TableCell>
												{numeroMatricula}
											</TableCell>

											<TableCell>
												{estado}
											</TableCell>

											<TableCell>
												{direccion}
											</TableCell>

											<TableCell>
												{departamento}
											</TableCell>

											<TableCell>
												{ciudad}
											</TableCell>

											{mostrarAcciones && (
												<TableCell align="center">
													<IconButton
														aria-label="Editar"
														color="primary"
														disabled={
															!puedeEditarRegistro(
																option,
															)
														}
														onClick={() => {
															if (
																!puedeEditarRegistro(
																	option,
																)
															) {
																return;
															}

															seleccionarRegistro(
																option,
															);
														}}
													>
														<Edit />
													</IconButton>

													<IconButton
														aria-label="Eliminar"
														color="secondary"
														disabled={
															!puedeEliminarRegistro()
														}
														onClick={() => {
															if (
																!puedeEliminarRegistro()
															) {
																return;
															}

															eliminarRegistro(
																option,
															);
														}}
													>
														<Delete />
													</IconButton>
												</TableCell>
											)}
										</TableRow>
									);
								},
							)}
						</TableBody>
					</Table>
				</Grid>
			</TableContainer>
		</>
	);
};

export default TablaInformacionDelInmueble;