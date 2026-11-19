import React, { useEffect, useState } from 'react';
import {
	Button,
	createStyles,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	Grid,
	makeStyles,
	MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';

import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { LoadingV2Component } from '../../../../components/loading/loadingV2.component';
import {
	ENTIDADEDUCATIVA,
	NIVELEDUCATIVO,
	PROGRAMAPORENTIDADDTO,
	VALORCATALOGO,
} from '../../../../types/catalog.type';
import ApiCatalog from '../../../../services/api-catalog.service';
import ApiEntidadesEducativas from '../../../../services/api-entidadesEducativas.service';
import AuthService from '../../../../services/auth.service';
import NotificationService from '../../../../services/notification.service';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
	}),
);

interface FormularioPrograma {
	idTipoPrograma: string;
	programa: string;
	nivelEducativoId: string;
}

export interface ProgramaEducativoCreadoResult {
	entidad: ENTIDADEDUCATIVA;
	programaCreado: PROGRAMAPORENTIDADDTO;
	programasActualizados: PROGRAMAPORENTIDADDTO[];
}

interface ModalProgramaEducativoProps {
	open: boolean;
	entidad: ENTIDADEDUCATIVA | null;
	modoEdicion?: boolean;
	programaInicial?: any | null;
	onClose: () => void;
	onProgramaGuardado: (
		resultado: ProgramaEducativoCreadoResult,
	) => void | Promise<void>;
}

const valoresIniciales: FormularioPrograma = {
	idTipoPrograma: '',
	programa: '',
	nivelEducativoId: '',
};

function ModalProgramaEducativo({
	open,
	entidad,
	modoEdicion = false,
	programaInicial = null,
	onClose,
	onProgramaGuardado,
}: ModalProgramaEducativoProps) {
	const classes = useStyles();
	const api = ApiCatalog();
	const apiEntidadesEducativas =
		ApiEntidadesEducativas();
	const user = AuthService.getUserInfo();

	const [tiposPrograma, setTiposPrograma] =
		useState<VALORCATALOGO[]>([]);

	const [nivelesPrograma, setNivelesPrograma] =
		useState<NIVELEDUCATIVO[]>([]);

	const [loading, setLoading] =
		useState(false);

	const formularioPrograma =
		useForm<FormularioPrograma>({
			defaultValues: valoresIniciales,
		});

	const normalizarTexto = (
		valor: unknown,
	): string =>
		String(valor ?? '')
			.trim()
			.toUpperCase()
			.normalize('NFD')
			.replace(/[\u0300-\u036f]/g, '');

	const obtenerIdTipoProgramaInicial = (
		tiposCatalogo: VALORCATALOGO[],
	): string => {
		const idDirecto = [
			programaInicial?.idTipoPrograma,
			programaInicial?.tipoProgramaId,
			programaInicial?.idValorTipoPrograma,
			programaInicial?.catalogoValorId,
			programaInicial?.tipoPrograma?.idValor,
			programaInicial?.tipoPrograma?.id,
			programaInicial?.idTipo,
		].find(
			valor =>
				valor !== null &&
				valor !== undefined &&
				String(valor).trim() !== '',
		);

		if (
			idDirecto !== null &&
			idDirecto !== undefined &&
			String(idDirecto).trim()
		) {
			const tipoPorId =
				tiposCatalogo.find(
					tipo =>
						String(tipo?.idValor) ===
						String(idDirecto),
				);

			if (tipoPorId) {
				return String(
					tipoPorId.idValor,
				);
			}
		}

		const descripcionTipo =
			normalizarTexto(
				programaInicial
					?.tipoProgramaDescripcion ??
				programaInicial
					?.descripcionTipoPrograma ??
				programaInicial
					?.catalogoValorDescripcion ??
				programaInicial
					?.tipoPrograma
					?.descripcion ??
				programaInicial
					?.tipoPrograma,
			);

		if (!descripcionTipo) {
			return '';
		}

		const tipoEncontrado =
			tiposCatalogo.find(
				tipo =>
					normalizarTexto(
						tipo?.descripcion,
					) === descripcionTipo,
			);

		return String(
			tipoEncontrado?.idValor ?? '',
		);
	};

	useEffect(() => {
		let componenteActivo = true;

		const cargarCatalogos = async () => {
			setLoading(true);

			try {
				const [
					responseNiveles,
					responseTiposPrograma,
				] = await Promise.all([
					api.getAllNivelEducativo(),
					api.getValoresCatalogoPorNombre(
						'TIPO PROGRAMA',
					),
				]);

				if (!componenteActivo) {
					return;
				}

				if (
					!responseNiveles?.isSuccess ||
					!responseTiposPrograma?.isSuccess
				) {
					toast(
						'No fue posible cargar la información del programa educativo.',
						{
							type: toast.TYPE.ERROR,
						},
					);

					onClose();
					return;
				}

				let niveles: NIVELEDUCATIVO[] = [];

				if (
					Array.isArray(
						responseNiveles.data,
					)
				) {
					niveles =
						responseNiveles.data;
				}

				let tipos: VALORCATALOGO[] = [];

				if (
					Array.isArray(
						responseTiposPrograma.data,
					)
				) {
					tipos =
						responseTiposPrograma.data;
				}

				setNivelesPrograma(niveles);
				setTiposPrograma(tipos);

				if (
					modoEdicion &&
					programaInicial
				) {
					const idTipoProgramaInicial =
						obtenerIdTipoProgramaInicial(tipos);


					formularioPrograma.reset({
						idTipoPrograma:
							idTipoProgramaInicial,
						programa: String(
							programaInicial?.programa ?? '',
						),
						nivelEducativoId: String(
							programaInicial?.nivelEducativoId ?? '',
						),
					});

					return;
				}

				formularioPrograma.reset(
					valoresIniciales,
				);
			} catch (error) {
				if (!componenteActivo) {
					return;
				}

				console.error(
					'Error cargando catálogos del programa:',
					error,
				);

				toast(
					'Error al consultar los catálogos del programa educativo.',
					{
						type: toast.TYPE.ERROR,
					},
				);

				onClose();
			} finally {
				if (componenteActivo) {
					setLoading(false);
				}
			}
		};

		if (open) {
			cargarCatalogos();
		}

		return () => {
			componenteActivo = false;
		};
	}, [
		open,
		modoEdicion,
		programaInicial,
	]);

	const cerrarModal = () => {
		if (loading) {
			return;
		}

		formularioPrograma.reset(
			valoresIniciales,
		);

		onClose();
	};

	const validarCambiosEdicion = (
		data: FormularioPrograma,
	): boolean => {
		if (
			!modoEdicion ||
			!programaInicial
		) {
			return true;
		}

		const programaActual =
			normalizarTexto(
				programaInicial?.programa,
			);

		const programaNuevo =
			normalizarTexto(data.programa);

		const nivelActual = String(
			programaInicial
				?.nivelEducativoId || '',
		);

		const nivelNuevo = String(
			data.nivelEducativoId || '',
		);

		const tipoActual =
			obtenerIdTipoProgramaInicial(
				tiposPrograma,
			);

		const tipoNuevo = String(
			data.idTipoPrograma || '',
		);

		const mismoPrograma =
			programaActual === programaNuevo;

		const mismoNivel =
			nivelActual === nivelNuevo;

		const mismoTipo =
			tipoActual === tipoNuevo;

		if (
			mismoPrograma &&
			mismoNivel &&
			mismoTipo
		) {
			toast(
				'No se realizaron cambios en el programa educativo.',
				{
					type: toast.TYPE.INFO,
				},
			);

			return false;
		}

		return true;
	};

	const buscarProgramaGuardado = (
		programasActualizados:
			PROGRAMAPORENTIDADDTO[],
		programa: string,
		nivelEducativoId: string,
	): PROGRAMAPORENTIDADDTO | undefined => {
		const programasCoincidentes =
			programasActualizados.filter(
				(
					item:
						PROGRAMAPORENTIDADDTO,
				) =>
					normalizarTexto(
						item.programa,
					) ===
					normalizarTexto(
						programa,
					) &&
					String(
						item.nivelEducativoId,
					) ===
					String(
						nivelEducativoId,
					),
			);

		if (
			programasCoincidentes.length === 0
		) {
			return undefined;
		}

		/*
		 * Cuando se crea una nueva combinación,
		 * tomamos el último registro coincidente.
		 */
		return programasCoincidentes[
			programasCoincidentes.length - 1
		];
	};

	const guardarProgramaEducativo = async (
		data: FormularioPrograma,
	) => {
		const idEntidad =
			entidad?.idEntidad;

		const programa = String(
			data.programa ?? '',
		).trim();

		const idTipoPrograma = Number(
			data.idTipoPrograma,
		);

		const nivelEducativoId = String(
			data.nivelEducativoId ?? '',
		).trim();

		if (!idEntidad) {
			toast(
				'No fue posible identificar la entidad educativa.',
				{
					type: toast.TYPE.ERROR,
				},
			);

			return;
		}

		if (
			!programa ||
			!idTipoPrograma ||
			!nivelEducativoId
		) {
			toast(
				'Debe seleccionar tipo de programa, nivel educativo e ingresar el programa.',
				{
					type: toast.TYPE.ERROR,
				},
			);

			return;
		}

		const puedeGuardar =
			validarCambiosEdicion(data);

		if (!puedeGuardar) {
			return;
		}

		const payloadPrograma = {
			idEntidad,
			idTipoPrograma,
			programa,
			nivelEducativoId,
			creadoPor: user?.USR_ID,
			estado: 1,
		};

		setLoading(true);

		try {
			const responseGuardarPrograma =
				await apiEntidadesEducativas
					.AddProgram(
						payloadPrograma as any,
					);

			if (
				!responseGuardarPrograma
					?.isSuccess
			) {
				await NotificationService
					.genericAlertError(
						'Error',
						responseGuardarPrograma
							?.returnMessage ||
						'No fue posible guardar el programa educativo.',
					);

				return;
			}

			const responseProgramas =
				await api
					.getAllProgramaEducativoPorEntidad(
						idEntidad,
					);

			let programasActualizados:
				PROGRAMAPORENTIDADDTO[] = [];

			if (
				responseProgramas?.isSuccess &&
				Array.isArray(
					responseProgramas.data,
				)
			) {
				programasActualizados =
					responseProgramas.data;
			}

			const programaGuardado =
				buscarProgramaGuardado(
					programasActualizados,
					programa,
					nivelEducativoId,
				);

			if (
				!programaGuardado
					?.idProgramaEntidad ||
				!programaGuardado
					?.nivelEducativoId
			) {
				await NotificationService
					.genericAlertError(
						'Error',
						'El programa fue guardado, pero no fue posible recuperarlo para asociarlo a la solicitud.',
					);

				return;
			}

			await onProgramaGuardado({
				entidad,
				programaCreado:
					programaGuardado,
				programasActualizados,
			});

			formularioPrograma.reset(
				valoresIniciales,
			);
		} catch (error) {
			console.error(
				'Error guardando programa educativo:',
				error,
			);

			toast(
				'Error al guardar el programa educativo.',
				{
					type: toast.TYPE.ERROR,
				},
			);
		} finally {
			setLoading(false);
		}
	};

	const tituloModal = modoEdicion
		? 'Editar programa educativo'
		: 'Crear programa educativo';

	const textoCarga = modoEdicion
		? 'Actualizando programa'
		: 'Guardando programa';

	const textoBoton = modoEdicion
		? 'ACTUALIZAR PROGRAMA'
		: 'GUARDAR PROGRAMA';

	return (
		<Dialog
			open={open}
			fullWidth
			maxWidth="sm"
			disableBackdropClick={loading}
			disableEscapeKeyDown={loading}
			onClose={cerrarModal}
		>
			<LoadingV2Component
				key="loader-programa"
				loading={loading}
				tittle={textoCarga}
			/>

			<DialogTitle>
				{tituloModal}
			</DialogTitle>

			<DialogContent>
				<p>
					<b>Entidad:</b>{' '}
					{entidad?.razonSocial || ''}
				</p>

				{modoEdicion && (
					<p>
						Modifique el programa o el
						nivel educativo. Al guardar,
						la nueva selección se
						asociará al registro que está
						editando.
					</p>
				)}

				<Grid container>
					<Grid
						className={classes.column}
						item
						xs={12}
					>
						<Controller
							name="idTipoPrograma"
							control={
								formularioPrograma.control
							}
							rules={{
								required:
									'Seleccione el tipo de programa.',
							}}
							render={({
								field: {
									onChange,
									value,
								},
								fieldState: {
									error,
								},
							}) => (
								<CHSelect
									label="Tipo de Programa *"
									value={String(value ?? '')}
									error={!!error}
									helperText={
										error
											? error.message
											: null
									}
									onChange={event =>
										onChange(
											String(event.target.value),
										)
									}
								>
									{tiposPrograma.map(option => (
										<MenuItem
											key={String(option.idValor)}
											value={String(option.idValor)}
										>
											{option.descripcion}
										</MenuItem>
									))}
								</CHSelect>
							)}
						/>
					</Grid>

					<Grid
						className={classes.column}
						item
						xs={12}
					>
						<Controller
							name="programa"
							control={
								formularioPrograma.control
							}
							rules={{
								required:
									'Ingrese el programa educativo.',
							}}
							render={({
								field: {
									onChange,
									value,
								},
								fieldState: {
									error,
								},
							}) => (
								<CHTextField
									label="Programa Educativo *"
									InputLabelProps={{
										shrink: true,
									}}
									value={value}
									onChange={(
										event: React.ChangeEvent<HTMLInputElement>,
									) => {
										onChange(
											event.target.value.toUpperCase(),
										);
									}}
									error={
										!!error
									}
									helperText={
										error
											? error.message
											: null
									}
								/>
							)}
						/>
					</Grid>

					<Grid
						className={classes.column}
						item
						xs={12}
					>
						<Controller
							name="nivelEducativoId"
							control={
								formularioPrograma.control
							}
							rules={{
								required:
									'Seleccione el nivel educativo.',
							}}
							render={({
								field: {
									onChange,
									value,
								},
								fieldState: {
									error,
								},
							}) => (
								<CHSelect
									label="Nivel Educativo *"
									value={value}
									error={
										!!error
									}
									helperText={
										error
											? error.message
											: null
									}
									onChange={
										onChange
									}
								>
									{nivelesPrograma.map(
										option => (
											<MenuItem
												key={
													option.id
												}
												value={
													option.id
												}
											>
												{
													option.descripcion
												}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
				</Grid>
			</DialogContent>

			<DialogActions>
				<Button
					disabled={loading}
					onClick={cerrarModal}
				>
					CANCELAR
				</Button>

				<Button
					variant="outlined"
					disabled={loading}
					onClick={
						formularioPrograma
							.handleSubmit(
								guardarProgramaEducativo,
							)
					}
				>
					{textoBoton}
				</Button>
			</DialogActions>
		</Dialog>
	);
}

export default ModalProgramaEducativo;