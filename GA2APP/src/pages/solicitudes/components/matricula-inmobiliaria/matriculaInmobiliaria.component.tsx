import React, { useState, useEffect, useMemo } from 'react';
import { connect } from 'react-redux';
import {
	createStyles,
	Grid,
	makeStyles,
	Button,
	MenuItem,
} from '@material-ui/core';
import { Search } from '@material-ui/icons';
import { toast } from 'react-toastify';
import { Controller, useForm, UseFormReturn, useWatch } from 'react-hook-form';
import { MatriculaInmobiliariaModel } from '../../../../models/matricula-inmobiliaria.model';
import NotificationService from '../../../../services/notification.service';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import { DEPARTAMENTODTO, CITYDTO, OFICINASREGISTRODTO } from '../../../../types/catalog.type';
import { Address } from '../../../../types/address.type';
import { AppState } from '../../../../store/rootReducer';
import { RealStateRegistrationsState } from '../../../../reducers/realStateRegistrations.reducer';
import GenerarGuid from '../funciones/guid.funtions';
import { ModalMatriculaInmobiliaria } from './modal-matriculaInmobiliaria-bloqueo-desbloqueo';
import {
	startGetOperacionesBloqueos,
	startGetBloqueosById,
} from '../../../../actions/realStateRegistrations.action';
import ApiRealState from '../../../../services/api-realstateregistrations.service';
import ApiSolicitudes from '../../../../services/api-solicitud.service';
import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import { setFormDataAddres } from '../../../../components/modals/address/constants/address.components.constants';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';
import AddressComponentV2 from '../../../../components/modals/address/addressV2.component'
import TablaInformacionDelInmueble from './TablaInformacionDelInmueble';
import AuthService from '../../../../services/auth.service';

const useStyles = makeStyles(() =>
	createStyles({
		columnButtonAdd: {
			textAlign: 'right',
		},
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
		columnLabel: {
			padding: '10px',
		},
	}),
);

function MatriculaInmobiliaria({
	title,
	listaMatriculas,
	listaDepartamentos,
	listaCiudades,
	listaOficinas,
	realState,
	matriculasAdicionadas,
	matriculasPendientesPorEliminar,
	responseSolicitud,
	siEsAprobadaSolcitud,
	campoSoloLecturaRadicacionAprobada,
	esRadicacionDevueltaPorRechazo,
	propietariosMatricula,
	propietariosMatriculaActualizados,
	cargarCiudadesPorDepartamento
}: {
	title: string;
	listaMatriculas: MatriculaInmobiliariaModel[];
	listaDepartamentos: DEPARTAMENTODTO[],
	listaCiudades: CITYDTO[],
	listaOficinas: OFICINASREGISTRODTO[],
	realState: RealStateRegistrationsState;
	matriculasAdicionadas: (matriculas: MatriculaInmobiliariaModel[]) => void;
	matriculasPendientesPorEliminar?: (matriculas: MatriculaInmobiliariaModel[]) => void;
	responseSolicitud: ResponseSeveranceModel | undefined;
	siEsAprobadaSolcitud: boolean;
	campoSoloLecturaRadicacionAprobada: any;
	esRadicacionDevueltaPorRechazo?: boolean;
	propietariosMatricula: any[];
	propietariosMatriculaActualizados?: (propietarios: any[]) => void;
	cargarCiudadesPorDepartamento: (
		idDepartamento: number,
	) => Promise<CITYDTO[]>;
}) {
	const classes = useStyles();
	const api = ApiRealState();
	const apiSolicitud = ApiSolicitudes();
	const [matriculaInmobiliariaModel, setMatriculaInmobiliariaModel] = useState<MatriculaInmobiliariaModel[]>([]);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR MATRÍCULA');
	const [matriculaAnterior, setmatriculaAnterior] = useState<string>('');
	const [idAnterior, setIdAnterior] = useState<string>('');
	const [departamentos, setDepartamentos] = useState<DEPARTAMENTODTO[]>([]);
	// const [ciudadesFilter, setCiudadesFilter] = useState<CITYDTO[]>([]);
	const [openModal, setOpenModal] = useState(false);
	const [opcion, setOpcion] = useState(false);
	const [causal, setCausal] = useState('');
	const [readOnly, setReadOnly] = useState(true);
	const [disabledOffice, setDisabledOffice] = useState(true);
	const [disabled, setDisabled] = useState(true);
	const [oficinaRegistro, setOficinaRegistro] = useState<OFICINASREGISTRODTO | null>(null);
	const [ciudadRegistro, setCiudadRegistro] = useState<CITYDTO | null>(null);
	const [esBloqueada, setEsBloqueada] = useState(false);
	const [loading, setLoading] = useState(false)
	const [open, setOpen] = useState(false);
	const [matriculaBloqueo, setMatriculaBloqueo] = useState({
		id: '',
		numeroMatricula: '',
		esPrincipal: false,
		direccion: '',
		ciudadId: 0,
		departamentoId: 0,
		departamento: '',
		ciudad: '',
		bloqueo: false,
		causalId: '',
		tipoOperacionId: '',
	});
	const [buttonDisabled, setButtonDisabled] = useState<boolean>(false);
	const [matriculaEncontradaApi, setMatriculaEncontradaApi] = useState<boolean>(false);

	const [matriculasPendientesEliminar, setMatriculasPendientesEliminar] = useState<MatriculaInmobiliariaModel[]>([]);

	const matriculasControl = useForm({
		defaultValues: {
			id: '',
			departamentoInmueble: '',
			ciudadInmueble: '',
			numeroMatricula: '',
			direccion: '',
			esPrincipal: true,
			bloqueo: false,
			causalId: '',
			tipoOperacionId: '',
			numeroOficinaBucar: '',
			numeroMatriculaBuscar: '',
			oficinaInmueble: '',
			codigoOficina: ''
		},
	});

	const departamentoInmuebleSeleccionado = useWatch({
		control: matriculasControl.control,
		name: 'departamentoInmueble',
	});

	const ciudadesDelDepartamento = useMemo(() => {
		const idDepartamento = String(
			departamentoInmuebleSeleccionado ?? '',
		)
			.split('_')[0]
			.trim();

		if (!idDepartamento) {
			return [];
		}

		return (listaCiudades ?? []).filter((ciudad: CITYDTO) => {
			const idDepartamentoCiudad =
				ciudad?.idDepartamento ??
				(ciudad as any)?.departamentoId ??
				(ciudad as any)?.dpdId ??
				(ciudad as any)?.departamento?.id;

			return String(idDepartamentoCiudad ?? '') === idDepartamento;
		});
	}, [departamentoInmuebleSeleccionado, listaCiudades]);


	// useEffect(() => {
	// 	console.log(
	// 		'[Matrícula] Departamento seleccionado:',
	// 		departamentoInmuebleSeleccionado,
	// 	);

	// 	console.log(
	// 		'[Matrícula] Ciudades que se están renderizando:',
	// 		ciudadesDelDepartamento.length,
	// 	);

	// 	console.table(ciudadesDelDepartamento);
	// }, [departamentoInmuebleSeleccionado, ciudadesDelDepartamento]);


	const { reset } = useForm({
		defaultValues: {
			causalId: '',
		},
	});


	const normalizarBloqueoEdicion = (valor: any): boolean =>
		valor === true ||
		valor === 1 ||
		['true', '1'].includes(
			String(valor ?? '').trim().toLowerCase(),
		);

	// useEffect(() => {
	// 	const matriculasGuardadas = (listaMatriculas ?? []).map((item: any) => ({
	// 		...item,


	// 		bloquearEdicion:
	// 			item?.bloquearEdicion !== undefined &&
	// 				item?.bloquearEdicion !== null
	// 				? normalizarBloqueoEdicion(item.bloquearEdicion)
	// 				: true,
	// 	}));

	// 	setMatriculaInmobiliariaModel(matriculasGuardadas);
	// }, [listaMatriculas]);

	useEffect(() => {

		const matriculasGuardadas = (
			listaMatriculas ?? []
		).map((item: any) => ({
			...item,
			bloquearEdicion:
				item?.bloquearEdicion !== undefined &&
					item?.bloquearEdicion !== null
					? normalizarBloqueoEdicion(
						item.bloquearEdicion,
					)
					: true,
		}));

		setMatriculaInmobiliariaModel(
			matriculasGuardadas,
		);
	}, [listaMatriculas, responseSolicitud]);

	const handleOpenAddress = () => {
		if (readOnly) {
			setOpen(false);
		} else {
			setOpen(true);
		}
	};

	const onCloseAddress = (value: Address) => {
		if (value) {
			const result = setFormDataAddres(value);
			matriculasControl.setValue('direccion', result);
		}
		setOpen(false);
	};

	const seleccionarRegistro = async (item: MatriculaInmobiliariaModel | any) => {
		if (normalizarBloqueoEdicion(item?.bloquearEdicion)) {
			return;
		}

		setMatriculaEncontradaApi(false);
		setReadOnly(false);
		setDisabledOffice(false);
		setDisabled(false);

		setIdAnterior(item.id);

		const departamentoId = item.departamentoId ?? item.departamento?.id;
		const ciudadId = item.ciudadId ?? item.ciudad?.id;

		const departamento = listaDepartamentos.find(
			x => x.id.toString() === departamentoId?.toString()
		);

		const ciudad = listaCiudades.find(
			x => x.id.toString() === ciudadId?.toString()
		);

		if (departamento) {
			setDepartamentos([departamento]);

			matriculasControl.setValue(
				'departamentoInmueble',
				`${departamento.id}_${departamento.nombre}`,
			);
		}

		if (ciudad) {
			setCiudadRegistro(ciudad);
			matriculasControl.setValue(
				'ciudadInmueble',
				`${ciudad.id}_${ciudad.nombre}`,
			);
		}

		if (item?.numeroMatricula?.indexOf('-') > 0) {
			const numMatricula = item.numeroMatricula.split('-');

			matriculasControl.setValue('numeroMatricula', numMatricula[1]);

			const office = listaOficinas?.find(
				o => o.codigoOficina === numMatricula[0],
			);

			if (office) {
				setOficinaRegistro(office);
				matriculasControl.setValue('oficinaInmueble', office.idOficina.toString());
				matriculasControl.setValue('codigoOficina', office.codigoOficina);
			}
		} else {
			matriculasControl.setValue('numeroMatricula', item.numeroMatricula);
		}

		matriculasControl.setValue('direccion', item.direccion);
		matriculasControl.setValue('id', item.id);
		matriculasControl.setValue('esPrincipal', item.esPrincipal);

		setDescEstadoBoton('CORREGIR DATO');
		setmatriculaAnterior(item.id);
	};



	const obtenerMensajeErrorEliminarMatricula = (error: any) => {
		const mensajeError =
			error?.returnMessage ||
			error?.message ||
			error?.response?.data?.returnMessage ||
			'';

		if (
			mensajeError.includes('Subquery returned more than 1 value')
		) {
			return 'No fue posible eliminar la matrícula porque se encontraron múltiples registros asociados. Por favor valide la información relacionada antes de intentar eliminarla nuevamente.';
		}

		return (
			mensajeError ||
			'Ocurrió un error al intentar eliminar la matrícula, intente de nuevo.'
		);
	};

	const obtenerIdMatricula = (matricula: any) =>
		String(matricula?.maiId ?? matricula?.id ?? '');

	const obtenerNumeroMatricula = (matricula: any) =>
		String(
			matricula?.maiNumeroMatricula ??
			matricula?.numeroMatricula ??
			''
		).trim();

	const obtenerPropietariosAsociadosAMatricula = (matricula: any) => {
		const idMatricula = obtenerIdMatricula(matricula);
		const numeroMatricula = obtenerNumeroMatricula(matricula);

		return propietariosMatricula?.filter((propietario: any) =>
			propietario?.matricula?.some((matriculaPropietario: any) => {
				const idMatriculaPropietario = String(
					matriculaPropietario?.id ??
					matriculaPropietario?.maiId ??
					''
				);

				const numeroMatriculaPropietario = String(
					matriculaPropietario?.numeroMatricula ??
					matriculaPropietario?.maiNumeroMatricula ??
					''
				).trim();

				return (
					(idMatricula && idMatriculaPropietario === idMatricula) ||
					(numeroMatricula && numeroMatriculaPropietario === numeroMatricula)
				);
			})
		) || [];
	};


	const eliminarMatriculaEnBackend = async (matricula: any) => {
		const codigoOficina =
			matricula?.codigoOficina ??
			(matricula?.numeroMatricula?.includes('-')
				? matricula.numeroMatricula.split('-')[0]
				: '');

		const user = AuthService.getUserInfo();

		const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';

		const isGuid = (value: any) =>
			typeof value === 'string' &&
			/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);

		const idUsuario = isGuid(user?.USR_ID)
			? user.USR_ID
			: EMPTY_GUID;

		const request = {
			maiId: matricula?.maiId ?? matricula?.id,
			maiNumeroMatricula: matricula?.maiNumeroMatricula ?? matricula?.numeroMatricula,
			maiDireccion: matricula?.maiDireccion ?? matricula?.direccion ?? '',
			maiFechaRegistro: matricula?.maiFechaRegistro ?? new Date().toISOString(),

			prpTipoIdentificacion: '',
			prpNumeroIdentificacion: '',
			prpNombreRazonSocial: '',
			prpPrimerNombre: '',
			prpSegundoNombre: '',
			prpPrimerApellido: '',
			prpSegundoApellido: '',

			dcpId: Number(matricula?.dcpId ?? matricula?.ciudad?.id ?? matricula?.ciudadId ?? 0),
			dpdId: Number(matricula?.dpdId ?? matricula?.departamento?.id ?? matricula?.departamentoId ?? 0),
			idOficinaRegistro: Number(matricula?.idOficinaRegistro ?? matricula?.oficinaRegistroId ?? 0),

			estado: matricula?.estado ?? (matricula?.esPrincipal ? 'Principal' : 'Relacionado'),

			nombreUsuario: user?.USR_USERNAME ?? '',
			idUsuario,
			correo: user?.USR_EMAIL ?? '',
			celular: user?.USR_CELULAR ?? '',

			nroCelular: '',
			tipoIdentificacion: '',
			mail: '',
			codigoOficina,

			propietarios: [],
			novedades: [],
			tramites: [],
			cambios: [],
			causales: [],
		};

		const response = await api.DeleteRealStateRegistrations(request as any);

		if (response?.isSuccess === false) {
			throw new Error(response?.returnMessage || 'No fue posible eliminar la matrícula');
		}

		return response;
	};

	const desvincularMatriculaDePropietarios = (matriculaEliminada: any) => {
		const idMatricula = obtenerIdMatricula(matriculaEliminada);
		const numeroMatricula = obtenerNumeroMatricula(matriculaEliminada);

		const propietariosActualizados = (propietariosMatricula ?? []).map((propietario: any) => {
			const matriculasActualizadas = (propietario?.matricula ?? []).filter((matriculaPropietario: any) => {
				const idMatriculaPropietario = String(
					matriculaPropietario?.id ??
					matriculaPropietario?.maiId ??
					''
				);

				const numeroMatriculaPropietario = String(
					matriculaPropietario?.numeroMatricula ??
					matriculaPropietario?.maiNumeroMatricula ??
					''
				).trim();

				const esLaMismaMatricula =
					(idMatricula && idMatriculaPropietario === idMatricula) ||
					(numeroMatricula && numeroMatriculaPropietario === numeroMatricula);

				return !esLaMismaMatricula;
			});

			return {
				...propietario,
				matricula: matriculasActualizadas,
			};
		});

		propietariosMatriculaActualizados?.(propietariosActualizados);

		return propietariosActualizados;
	};


	const eliminarRegistro = async (matricula: MatriculaInmobiliariaModel) => {
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Desea eliminar esta matrícula?',
		);

		if (!confirmAlert) return;

		try {
			setLoading(true);

			const codigoOficina =
				(matricula as any).codigoOficina ??
				(matricula.numeroMatricula?.includes('-')
					? matricula.numeroMatricula.split('-')[0]
					: '');

			const user = AuthService.getUserInfo();

			const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';

			const isGuid = (value: any) =>
				typeof value === 'string' &&
				/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(value);

			const idUsuario = isGuid(user?.USR_ID)
				? user.USR_ID
				: EMPTY_GUID;

			const propietariosAsociados =
				obtenerPropietariosAsociadosAMatricula(matricula);


			if (propietariosAsociados.length > 0) {
				const temp = matriculaInmobiliariaModel.filter(
					(x: any) =>
						String(x?.id ?? x?.maiId) !== String((matricula as any)?.id ?? (matricula as any)?.maiId) &&
						String(x?.numeroMatricula ?? x?.maiNumeroMatricula) !==
						String((matricula as any)?.numeroMatricula ?? (matricula as any)?.maiNumeroMatricula),
				);

				setMatriculaInmobiliariaModel(temp);
				matriculasAdicionadas(temp);

				desvincularMatriculaDePropietarios(matricula);

				setMatriculasPendientesEliminar(prev => {
					const yaExiste = prev.some(
						(x: any) =>
							String(x?.id ?? x?.maiId) === String((matricula as any)?.id ?? (matricula as any)?.maiId) ||
							String(x?.numeroMatricula ?? x?.maiNumeroMatricula) ===
							String((matricula as any)?.numeroMatricula ?? (matricula as any)?.maiNumeroMatricula),
					);

					if (yaExiste) return prev;

					const nuevasPendientes = [...prev, matricula];

					matriculasPendientesPorEliminar?.(nuevasPendientes);

					return nuevasPendientes;
				});

				setDescEstadoBoton('AGREGAR MATRÍCULA');
				setOficinaRegistro(null);
				setCiudadRegistro(null);
				setEsBloqueada(false);
				setMatriculaEncontradaApi(false);
				matriculasControl.reset();

				await NotificationService.genericAlert(
					'Matrícula pendiente por eliminar',
					'La matrícula fue retirada y desvinculada del propietario en el formulario. Ahora debe editar el propietario y seleccionar la nueva matrícula antes de guardar.',
					'warning',
				);

				return;
			}

			const request = {
				maiId: (matricula as any).maiId ?? matricula.id,
				maiNumeroMatricula: (matricula as any).maiNumeroMatricula ?? matricula.numeroMatricula,
				maiDireccion: (matricula as any).maiDireccion ?? matricula.direccion ?? '',
				maiFechaRegistro: (matricula as any).maiFechaRegistro ?? new Date().toISOString(),

				prpTipoIdentificacion: '',
				prpNumeroIdentificacion: '',
				prpNombreRazonSocial: '',
				prpPrimerNombre: '',
				prpSegundoNombre: '',
				prpPrimerApellido: '',
				prpSegundoApellido: '',

				dcpId: Number((matricula as any).dcpId ?? matricula.ciudad?.id ?? matricula.ciudadId ?? 0),
				dpdId: Number((matricula as any).dpdId ?? matricula.departamento?.id ?? matricula.departamentoId ?? 0),
				idOficinaRegistro: Number((matricula as any).idOficinaRegistro ?? (matricula as any).oficinaRegistroId ?? 0),

				estado: (matricula as any).estado ?? (matricula.esPrincipal ? 'Principal' : 'Relacionado'),

				nombreUsuario: user?.USR_USERNAME ?? '',
				idUsuario: idUsuario,
				correo: user?.USR_EMAIL ?? '',
				celular: user?.USR_CELULAR ?? '',


				nroCelular: '',
				tipoIdentificacion: '',
				mail: '',
				codigoOficina: codigoOficina,

				propietarios: [],
				novedades: [],
				tramites: [],
				cambios: [],
				causales: [],
			};

			const response = await eliminarMatriculaEnBackend(matricula);

			if (response?.isSuccess === false) {
				const mensajeError =
					response?.returnMessage || 'No fue posible eliminar la matrícula';

				console.error('Error API EliminarMatriculaInmobiliaria:', response);

				await NotificationService.genericAlert(
					'Error en la eliminación',
					mensajeError,
					'error',
				);

				return;
			}

			const temp = matriculaInmobiliariaModel.filter(
				x => x.numeroMatricula !== matricula.numeroMatricula,
			);

			setMatriculaInmobiliariaModel(temp);
			matriculasAdicionadas(temp);

			setDescEstadoBoton('AGREGAR MATRÍCULA');
			setOficinaRegistro(null);
			setCiudadRegistro(null);
			setEsBloqueada(false);
			setMatriculaEncontradaApi(false);
			matriculasControl.reset();

			NotificationService.genericAlert(
				'Eliminado',
				'La matrícula fue eliminada correctamente.',
			);

		} catch (error: any) {
			console.error('Error al eliminar matrícula inmobiliaria:', error);

			await NotificationService.genericAlert(
				'Error en la eliminación',
				error?.message ||
				'Ocurrió un error al intentar eliminar la matrícula, intente de nuevo.',
				'error',
			);
		} finally {
			setLoading(false);
		}
	};

	const resetearMatricula = () => {
		setDescEstadoBoton('AGREGAR MATRÍCULA');
		setmatriculaAnterior('');
		setIdAnterior('');

		setOficinaRegistro(null);
		setCiudadRegistro(null);

		setDepartamentos([]);

		setEsBloqueada(false);
		setMatriculaEncontradaApi(false);

		setReadOnly(true);
		setDisabledOffice(true);
		setDisabled(true);

		matriculasControl.reset();
	};

	const esSolicitudVivienda8 = String(
		responseSolicitud?.nombreTarea ?? '',
	)
		.toUpperCase()
		.includes('V 8');

	// const agregarMatricula = async (data: any) => {

	// 	setLoading(true)

	// 	const verificarEsPrincipal = data?.esPrincipal !== true

	// 	if (verificarEsPrincipal) {
	// 		toast('La matrícula debe ser principal', {
	// 			type: toast.TYPE.ERROR,
	// 		});
	// 		setLoading(false)
	// 		return
	// 	}

	// 	const datoEncontrado = matriculaInmobiliariaModel.findIndex(
	// 		x =>
	// 			x.id === data.id &&
	// 			x.id !== matriculaAnterior,
	// 	);
	// 	if (datoEncontrado === -1) {

	// 		const existePrincipal = matriculaInmobiliariaModel.findIndex(
	// 			x =>
	// 				x.esPrincipal === true &&
	// 				x.id !== matriculaAnterior,
	// 		);

	// 		if (!existePrincipal) {
	// 			toast('Ya existe una matrícula ', {
	// 				type: toast.TYPE.ERROR,
	// 			});
	// 			setLoading(false)
	// 		} else {
	// 			agregarRegistromatricula(data);
	// 			setLoading(false)
	// 		}
	// 	}
	// };

	const agregarMatricula = async (data: any) => {
		setLoading(true);

		try {
			const esPrincipal = data?.esPrincipal === true;

			const numeroMatriculaCompleta = `${data?.codigoOficina ?? ''}-${data?.numeroMatricula ?? ''}`
				.trim()
				.toUpperCase();

			// Al editar, no se valida contra el mismo registro.
			const matriculasAValidar = matriculaInmobiliariaModel.filter(
				(matricula: MatriculaInmobiliariaModel) =>
					!matriculaAnterior ||
					String(matricula?.id ?? '') !==
					String(matriculaAnterior),
			);

			const existeMismaMatricula = matriculasAValidar.some(
				(matricula: MatriculaInmobiliariaModel) =>
					String(matricula?.numeroMatricula ?? '')
						.trim()
						.toUpperCase() === numeroMatriculaCompleta,
			);

			if (existeMismaMatricula) {
				toast('La matrícula ya se encuentra agregada.', {
					type: toast.TYPE.ERROR,
				});
				return;
			}

			const existePrincipal = matriculasAValidar.some(
				(matricula: MatriculaInmobiliariaModel) =>
					matricula.esPrincipal === true,
			);

			const existeRelacionado = matriculasAValidar.some(
				(matricula: MatriculaInmobiliariaModel) =>
					matricula.esPrincipal === false,
			);

			// Modalidades diferentes a Vivienda 8:
			// solo pueden tener un inmueble principal.
			if (!esSolicitudVivienda8 && !esPrincipal) {
				toast(
					'Para esta modalidad la matrícula debe registrarse como inmueble principal.',
					{ type: toast.TYPE.ERROR },
				);
				return;
			}

			// En cualquier modalidad no se permiten dos principales.
			if (esPrincipal && existePrincipal) {
				toast('Solo se permite una matrícula principal.', {
					type: toast.TYPE.ERROR,
				});
				return;
			}

			// Solo Vivienda 8 puede tener un inmueble relacionado.
			if (!esPrincipal) {
				if (!esSolicitudVivienda8) {
					toast(
						'Esta modalidad no permite agregar inmuebles relacionados.',
						{ type: toast.TYPE.ERROR },
					);
					return;
				}

				if (!existePrincipal) {
					toast(
						'Primero debe registrar el inmueble principal antes de agregar uno relacionado.',
						{ type: toast.TYPE.ERROR },
					);
					return;
				}

				if (existeRelacionado) {
					toast(
						'Para Vivienda 8 solo se permite un inmueble relacionado.',
						{ type: toast.TYPE.ERROR },
					);
					return;
				}
			}

			await agregarRegistromatricula(data);
		} finally {
			setLoading(false);
		}
	};

	const agregarRegistromatriculaBloqueo = async (
		data: any,
		causal: string,
	) => {
		const array = matriculaInmobiliariaModel.map(item => {
			if (item.id === data.id) {
				return {
					...item,
					bloqueo: data.bloqueo as boolean,
					causalId: causal,
					tipoOperacionId: data.tipoOperacionId as string,
				};
			}
			return item;
		});
		setMatriculaInmobiliariaModel(array);
		const request = {
			matriculaId: data.id as string,
			tipoOperacionId: data.tipoOperacionId as string,
			causalId: causal as string,
		};
		await insertarNovedadMatricula(request);
	};

	const insertarNovedadMatricula = async (request: any) => {
		await api
			.AddNoveltyBlocks(request)
			.then(async response => {

				if (response && response.isSuccess) {
					NotificationService.genericAlert(
						'Guardado',
						'Se ha guardado correctamente la novedad de matricula.',
					);
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};

	const agregarRegistromatricula = async (data: any) => {

		const guid = GenerarGuid();
		const matriculaAnteriorEnLista =
			descEstadoBoton === 'CORREGIR DATO'
				? matriculaInmobiliariaModel.find(
					item => String(item.id) === String(matriculaAnterior),
				)
				: null;
		const matriculaNueva = {
			id: guid.newGuid(),
			esPrincipal: data.esPrincipal,
			numeroMatricula: `${data.codigoOficina}-${data.numeroMatricula}`,
			direccion: data.direccion,
			ciudadId: data.ciudadInmueble.split('_')[0],
			departamentoId: data.departamentoInmueble.split('_')[0],
			departamento: {
				id: data.departamentoInmueble.split('_')[0],
				descripcion: data.departamentoInmueble.split('_')[1],
			},
			ciudad: {
				id: data.ciudadInmueble.split('_')[0],
				descripcion: data.ciudadInmueble.split('_')[1],
			},
			bloqueo: false,
			causalId: '98efdd13-2459-42fc-8787-6034f9935c35',
			tipoOperacionId: '38d1812e-eff8-48ea-9cc7-cdd0c159d0dd',
			oficinaRegistroId: data.oficinaInmueble,
			esBloqueada: esBloqueada,
			bloquearEdicion:
				descEstadoBoton === 'CORREGIR DATO'
					? normalizarBloqueoEdicion(
						matriculaAnteriorEnLista?.bloquearEdicion,
					)
					: matriculaEncontradaApi,
		};
		let matriculas: MatriculaInmobiliariaModel[] = [];
		if (descEstadoBoton === 'AGREGAR MATRÍCULA') {
			setMatriculaInmobiliariaModel(oldArray => [
				...oldArray,
				matriculaNueva,
			]);
			matriculas = [...matriculaInmobiliariaModel];
			matriculas.push(matriculaNueva);
		} else {
			const temp = [...matriculaInmobiliariaModel];
			const posicionActual = temp.findIndex(
				x => x.id === matriculaAnterior,
			);
			temp.splice(posicionActual, 1);
			matriculaNueva.id = idAnterior;
			temp.splice(posicionActual, 0, matriculaNueva);
			matriculas = temp;
			setMatriculaInmobiliariaModel(temp);
		}
		matriculasAdicionadas(matriculas);
		setmatriculaAnterior('');
		resetearMatricula();
	};
	const handleCloseModal = () => {
		setOpenModal(false);
		setOpcion(false);
		reset();
		setMatriculaBloqueo({
			id: '',
			numeroMatricula: '',
			esPrincipal: false,
			direccion: '',
			ciudadId: 0,
			departamentoId: 0,
			departamento: '',
			ciudad: '',
			bloqueo: false,
			causalId: '',
			tipoOperacionId: '',
		});
	};

	const hadlOnChangeCausal = (value: any) => {
		const causal = value as string;
		setCausal(causal);
	};
	const handleInsertarNovedad = () => {
		agregarRegistromatriculaBloqueo(matriculaBloqueo, causal);
		handleCloseModal();
	};

	const buscarMatricula = async (codigomatricula: any, numeroMatricu: string) => {
		const numeroMatricula = `${codigomatricula}-${numeroMatricu}`
		resetearMatricula();
		await apiSolicitud
			.GetConsultarExisteMatricula(numeroMatricula)
			.then(async response => {
				if (response) {
					if (response?.isSuccess) {
						if (response?.data?.maiEstado) {
							setMatriculaEncontradaApi(true);
							setEsBloqueada(response?.data?.maiBloqueo);
							setReadOnly(true);
							setDisabledOffice(true);

							const departamento = listaDepartamentos.find(
								x => x.id.toString() === response?.data?.dpdId?.toString()
							);

							if (departamento) {
								setDepartamentos([departamento]);

								matriculasControl.setValue(
									'departamentoInmueble',
									`${departamento.id}_${departamento.nombre}`,
								);
							}

							matriculasControl.setValue('direccion', response?.data?.maiDireccion);
							if (response?.data?.maiNumeroMatricula?.indexOf('-') > 0) {
								const numMatricula = response?.data?.maiNumeroMatricula.split('-');
								matriculasControl.setValue('numeroMatricula', numMatricula[1]);
							} else {
								matriculasControl.setValue('numeroMatricula', response?.data?.maiNumeroMatricula);
							}
							const office = listaOficinas?.find(o => o.codigoOficina === response?.data?.ofcCodigo);
							if (office) {
								setOficinaRegistro(office);
								matriculasControl.setValue('oficinaInmueble', office?.idOficina.toString());
								matriculasControl.setValue('codigoOficina', office?.codigoOficina);
							}

							const ciudad = listaCiudades.find(
								x => x.id.toString() === response?.data?.dpcId?.toString()
							);

							if (ciudad) {
								setCiudadRegistro(ciudad);
								matriculasControl.setValue(
									'ciudadInmueble',
									`${ciudad.id}_${ciudad.nombre}`,
								);
							}
						} else {

							NotificationService.genericAlwarning(
								'Alerta',
								`Advertencia: Matricula inactiva`,
								'warning')
						}
					} else {

						NotificationService.genericAlwarning(
							'Alerta',
							`No existe la matricula, diligencie los campos`,
							'warning')

						setMatriculaEncontradaApi(false);
						setOficinaRegistro(null);
						resetearMatricula();
						setReadOnly(false);
						setDisabledOffice(false);
						matriculasControl.setValue('numeroOficinaBucar', '')
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};



	const onchangeOffice = (
		_: any,
		newValue: OFICINASREGISTRODTO | null,
	) => {
		setOficinaRegistro(newValue);
		setCiudadRegistro(null);

		matriculasControl.setValue('ciudadInmueble', '');
		matriculasControl.setValue('departamentoInmueble', '');

		if (!newValue) {
			setDepartamentos([]);
			setDisabled(true);
			return;
		}

		matriculasControl.setValue(
			'oficinaInmueble',
			newValue.idOficina.toString(),
		);

		matriculasControl.setValue(
			'codigoOficina',
			newValue.codigoOficina,
		);

		const esCundinamarca = newValue.codigoDepartamento === 25;

		const departamentosDisponibles =
			esCundinamarca &&
				(newValue.codigoOficina === '50N' ||
					newValue.codigoOficina === '50C' ||
					newValue.codigoOficina === '50S')
				? listaDepartamentos.filter(
					item =>
						item.id === newValue.codigoDepartamento ||
						item.id === 11,
				)
				: listaDepartamentos.filter(
					item => item.id === newValue.codigoDepartamento,
				);

		setDepartamentos(departamentosDisponibles);
		setDisabled(false);
	};


	const onchangeCiudad = (_: any, newValue: CITYDTO | null) => {
		setCiudadRegistro(newValue);

		matriculasControl.setValue(
			'ciudadInmueble',
			newValue
				? `${newValue.id}_${newValue.nombre}`
				: '',
		);
	};

	if (loading) {
		return <CHLoadingProcess />
	}

	const normalizarTexto = (valor?: string) =>
		String(valor ?? '')
			.normalize('NFD')
			.replace(/[\u0300-\u036f]/g, '')
			.trim()
			.toUpperCase();

	const esActividadRadicacion =
		normalizarTexto(responseSolicitud?.nombreTarea)
			.startsWith('RADICACION');

	const solicitudDevueltaPorRechazo =
		esRadicacionDevueltaPorRechazo ||
		String(responseSolicitud?.soL_RECHAZADA ?? '') === '1';

	const tieneSeccionInconsistencia =
		responseSolicitud?.secciones?.includes('INCONSISTENCIA');

	const ocultarFormularioMatricula =
		!esActividadRadicacion ||
		(
			!solicitudDevueltaPorRechazo &&
			(
				((siEsAprobadaSolcitud || tieneSeccionInconsistencia) &&
					String(responseSolicitud?.soL_RECHAZADA ?? '') !== '1') ||
				campoSoloLecturaRadicacionAprobada
			)
		);


	return (
		<>
			<CHPaper title={title}>
				{
					ocultarFormularioMatricula ?
						'' :
						<Grid container>
							<Grid className={classes.column} item xs={12} md={1}>
								<Controller
									name="numeroOficinaBucar"
									control={matriculasControl.control}
									defaultValue=""
									render={({ field }) => (
										<CHTextField
											{...field}
											type="text"
											placeholder="Ejemplo: 123"
											value={field.value.toUpperCase() || ''}
											onChange={({ target: { value } }) => {
												if (value.length <= 3) {
													field.onChange(value);
												}
												if (value.length === 3) {
													const nextField = document.getElementById('numeroMatriculaBuscar');
													if (nextField) {
														nextField.focus();
													}
												}
											}}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="numeroMatriculaBuscar"
									control={matriculasControl.control}
									render={({ field }) => (
										<CHTextField
											{...field}
											id="numeroMatriculaBuscar"
											type="text"
											placeholder="Ejemplo: 0456789..."
											inputProps={{ inputMode: 'numeric', maxLength: 12 }}
											onChange={e => {
												const value = e.target.value.replace(/\D/g, '');
												field.onChange(value);
											}}
										/>
									)}
								/>
							</Grid>
							<Button
								aria-label="Eliminar"
								color="secondary"
								onClick={() => {
									buscarMatricula(matriculasControl.getValues('numeroOficinaBucar'), matriculasControl.getValues('numeroMatriculaBuscar'));
								}}
							>
								<Search />
							</Button>
						</Grid>
				}
				{
					ocultarFormularioMatricula ?
						null :
						<Grid container>
							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="oficinaInmueble"
									rules={{
										required: 'Este campo es requerido',
									}}
									control={matriculasControl.control}
									render={(
										{
											field: { onChange, value },
											fieldState: { error },
										}) => (
										<CHAutoComplete
											disabled={disabledOffice}
											value={oficinaRegistro}
											options={listaOficinas}
											getOptionLabel={option => option?.descripcionOficina}
											onChange={onchangeOffice}
											renderInput={(params) =>

												<CHTextField
													error={!!error}
													helperText={error ? error.message : null}
													{...params}
													required
													label="Oficina del inmueble"
												/>

											}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="codigoOficina"
									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (

										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											disabled
											label="Código oficina"
											required
											value={value}
										/>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="numeroMatricula"
									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
										maxLength: 13
									}}
									render={({ field: { onChange, value }, fieldState: { error } }) => (
										<CHTextField
											error={!!error}
											helperText={error ? error.message : null}
											inputProps={{
												maxLength: 11,
												onPaste: (e) => {
													e.preventDefault(); // Prevent the default paste behavior
												}
											}}
											isReadOnly={readOnly}
											disabled={disabled}
											label="Matrícula Inmobiliaria"
											value={value}
											required
											onChange={(e) => {
												onChange(e.target.value.replace(/[^A-Z\d]/g, ''));
											}}
											InputLabelProps={{ shrink: true }}
										/>
									)}
								/>
							</Grid>

							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="direccion"
									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									defaultValue=""
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											InputLabelProps={{ shrink: true }}
											label="Dirección "
											error={!!error}
											helperText={
												error ? error.message : null
											}
											isReadOnly
											disabled={disabled}
											value={value}
											onClick={handleOpenAddress}
											onChange={(onChange)}
										/>

									)}
								/>
							</Grid>

							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="departamentoInmueble"
									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (

										<CHSelect
											error={!!error}
											label="Departamento del Inmueble *"
											value={value}
											required
											readOnly={readOnly}
											disabled={disabled}
											helperText={error ? error.message : null}
											onChange={event => {
												setCiudadRegistro(null);
												matriculasControl.setValue('ciudadInmueble', '');

												onChange(event);
											}}
										>
											{departamentos
												.filter(item => item.id !== 0)
												?.map((option: DEPARTAMENTODTO) => {
													const dato = `${option.id}_${option.nombre}`;
													return (
														<MenuItem
															key={option.id}
															value={dato}
														>
															{option.nombre}
														</MenuItem>
													);
												})}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="ciudadInmueble"

									control={matriculasControl.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (

										<CHAutoComplete
											value={ciudadRegistro}
											options={ciudadesDelDepartamento}
											onChange={onchangeCiudad}
											disabled={readOnly || !departamentoInmuebleSeleccionado}
											getOptionLabel={option => option?.nombre ?? ''}
											getOptionSelected={(option, value) =>
												String(option?.id) === String(value?.id)
											}
											renderInput={(params) => (
												<CHTextField
													{...params}
													error={!!error}
													helperText={error ? error.message : null}
													required
													label="Ciudad del Inmueble"
													isReadOnly={readOnly}
													InputLabelProps={{ shrink: true }}
												/>
											)}
										/>
									)}
								/>
							</Grid>
						</Grid>
				}
				{
					ocultarFormularioMatricula ?
						null :
						<Grid container>
							<Grid className={classes.columnLabel} item xs={12} md={6} lg={3}>
								<Controller
									name="esPrincipal"
									control={matriculasControl.control}
									render={({ field: { onChange, value } }) => (
										<CHCheckbox
											label="Principal / Relacionado"
											onChange={onChange}
											checked={value}
											disabled={campoSoloLecturaRadicacionAprobada}
										/>
									)}
								/>
							</Grid>
						</Grid>
				}
				{ocultarFormularioMatricula ?
					'' :
					<Grid container>
						<Grid
							className={classes.columnButtonAdd}
							item
							xs={12}
							md={12}
						>

							<Button
								className={classes.buttonAdd}
								variant="outlined"
								disabled={buttonDisabled}
								onClick={matriculasControl.handleSubmit(
									agregarMatricula,
								)}
							>
								{descEstadoBoton}
							</Button>
							<Button
								className={classes.buttonAdd}
								variant="outlined"
								disabled={buttonDisabled}
								onClick={resetearMatricula}
							>
								CANCELAR
							</Button>
						</Grid>
					</Grid>
				}


				<TablaInformacionDelInmueble
					matriculaInmobiliariaModel={matriculaInmobiliariaModel}
					siEsAprobadaSolcitud={siEsAprobadaSolcitud}
					responseSolicitud={responseSolicitud}
					seleccionarRegistro={seleccionarRegistro}
					eliminarRegistro={eliminarRegistro}
					campoSoloLecturaRadicacionAprobada={ocultarFormularioMatricula}
					actividadActual={responseSolicitud?.nombreTarea}
				/>
			</CHPaper>
			<ModalMatriculaInmobiliaria
				open={openModal}
				onClose={handleCloseModal}
				control={matriculasControl.control}
				opcion={opcion}
				listaCausales={realState.causalesPortipo}
				handleInsertarNovedad={handleInsertarNovedad}
				hadlOnChangeCausal={hadlOnChangeCausal}
				handleSubmit={matriculasControl.handleSubmit}
			/>
			<AddressComponentV2
				onOpen={open}
				onClose={onCloseAddress}
				tipoDireccion={1 === 1}
			/>
		</>
	);
}
const mapStateToProps = (state: AppState) => ({
	realState: state.realStateRegistrations,
});
const mapDispatchToProps = (dispatch: any) => ({
	obtenerOperacionesBloqueos: () => dispatch(startGetOperacionesBloqueos()),
	obtenerCausalesPorId: (typeIdOperation: string) => dispatch(startGetBloqueosById(typeIdOperation)),
});
export default connect(mapStateToProps, mapDispatchToProps,)(MatriculaInmobiliaria);
