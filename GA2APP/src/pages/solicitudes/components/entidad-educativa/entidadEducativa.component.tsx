import React, { useEffect, useState } from 'react';
import {
	Button,
	Paper,
	createStyles,
	Grid,
	IconButton,
	makeStyles,
	MenuItem,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';

import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { Delete, Edit } from '@material-ui/icons';
import { startAddEducationalEntitiesFromRepaes } from '../../../../actions/educationalEntities.actions';
import { EntidadesEducativasAgregar } from '../../../../types/educational.entities.type';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../../components/ch-select/CHSelect';
import {
	CITYDTO,
	DEPARTAMENTODTO,
	ENTIDADEDUCATIVA,
	NIVELEDUCATIVO,
	PROGRAMAPORENTIDADDTO,
	IDENTIFICATIONTYPEDTO,
	VALORCATALOGO,
} from '../../../../types/catalog.type';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import ApiCatalog from '../../../../services/api-catalog.service';
import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import UtilService from '../../../../utils/util.service';
import CHAutoCompleteV2 from '../../../../components/ch-auto-complete/CHAutoCompleteV2';
import NotificationService from '../../../../services/notification.service';
import { LoadingV2Component } from '../../../../components/loading/loadingV2.component';
import ModalProgramaEducativo, {
	ProgramaEducativoCreadoResult,
} from './ModalProgramaEducativo';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		button: {
			width: '120px',
			marginLeft: '.5rem',
		},
		column2: {
			padding: '10px',
			textAlign: 'left',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
		columnButtonAdd: {
			textAlign: 'right',
		},
		columnLabel: {
			padding: '10px',
		},
	}),
);

function EntidadEducativa({
	listaEntidadEducativa,
	responseSolicitud,
	validarRetiro,
	setValorReciboTotal,
	setEstadoEntidadNueva,
	siEsAprobadaSolcitud,
	campoSoloLecturaRadicacionAprobada,
	listaTiposDocumento,
	startAddEducationalEntitiesFromRepaes,
	listaTipoPersona,
	setIdsEntidadesCreadasFormulario
}: {
	listaEntidadEducativa: ENTIDADEDUCATIVA[];
	responseSolicitud: ResponseSeveranceModel | undefined;
	validarRetiro: any;
	setValorReciboTotal: (value: number) => void;
	setEstadoEntidadNueva: (value: any[]) => void;
	siEsAprobadaSolcitud: boolean;
	campoSoloLecturaRadicacionAprobada: any;
	listaTiposDocumento: IDENTIFICATIONTYPEDTO[];
	startAddEducationalEntitiesFromRepaes: (
		data: EntidadesEducativasAgregar,
	) => any;
	listaTipoPersona: VALORCATALOGO[];
	setIdsEntidadesCreadasFormulario: (
		value: React.SetStateAction<string[]>,
	) => void;
}) {

	const classes = useStyles();
	const api = ApiCatalog();
	const [programas, setProgramas] = useState<any[]>([]);
	const [niveles, setNiveles] = useState<NIVELEDUCATIVO[]>([]);
	const [entidadEducativa, setEntidadEducativa] = useState<any | null>(null);
	const [listaEntidades, setListaEntidades] = useState<any[]>([]);
	const [departamentos, setDepartamentos] = useState<any[]>([]);
	const [departamentoValue, setDepartamentoValue] = React.useState<DEPARTAMENTODTO | null>(null);
	const [ciudadValue, setCiudadValue] = React.useState<CITYDTO | null>(null);
	const [ciudadValues, setCiudadValues] = React.useState<CITYDTO[]>([]);
	const [mostrarCampoHoras, setMostrarCampoHoras] = React.useState(false);
	const [descEstadoBoton, setDescEstadoBoton] = React.useState('AGREGAR ENTIDAD');
	const [entidadAnterior, setEntidadAnterior] = useState<number>(0);
	const [idAnteriorEntidad, setIdAnteriorEntidad] = useState<number>(0);
	const [entidadAcreditada, setEntidadAcreditada] = useState([]);
	const [datosEntidades, setDatosEntidades] = useState<any[]>([]);
	const [loading, setLoading] = useState(false);
	const [modoCrearEntidad, setModoCrearEntidad] = useState(false);

	const [editandoEntidad, setEditandoEntidad] =
		useState(false);

	const [
		editandoEntidadNueva,
		setEditandoEntidadNueva,
	] = useState(false);
	// MODAL PROGRAMA EDUCATIVO

	const [openModalPrograma, setOpenModalPrograma] = useState(false);
	const [modoEdicionPrograma, setModoEdicionPrograma] =
		useState(false);

	const [programaEnEdicion, setProgramaEnEdicion] =
		useState<any | null>(null);

	const [entidadPendientePrograma, setEntidadPendientePrograma] =
		useState<ENTIDADEDUCATIVA | null>(null);

	const [entidadesDisponibles, setEntidadesDisponibles] =
		useState<ENTIDADEDUCATIVA[]>([]);

	const [entidadListaParaAgregar, setEntidadListaParaAgregar] =
		useState(false);

	useEffect(() => {
		setEntidadesDisponibles(anterior => {
			const entidades = [
				...anterior,
				...(listaEntidadEducativa || []),
			];

			const mapa = new Map<string, ENTIDADEDUCATIVA>();

			entidades.forEach(entidad => {
				if (entidad?.idEntidad) {
					mapa.set(String(entidad.idEntidad), entidad);
				}
			});

			return Array.from(mapa.values());
		});
	}, [listaEntidadEducativa]);

	// MODAL PROGRAMA EDUCATIVO

	const valoresInicialesEntidad = {
		id: '',
		razonSocial: '',
		programaentidad: '',
		nivelEducativo: '',
		numReciboPago: '',

		fechaLimitePago: null,
		Fecha_de_inicio: null,
		Fecha_fin: null,

		departamento: 0,
		ciudad: 0,
		modalidad_estudio: '',
		periodoAcademico: '',
		horas: '',
		acreditada: 0,
		tipoDocumentoNuevaEntidad: '',
		numeroDocumentoNuevaEntidad: '',
		valorRecibo: '',
	};

	const formularioEntidad = useForm({
		defaultValues: valoresInicialesEntidad,
	});

	const [
		razonSocialEntidadEdicion,
		setRazonSocialEntidadEdicion,
	] = useState('');

	useEffect(() => {
		if (
			editandoEntidadNueva &&
			razonSocialEntidadEdicion
		) {
			formularioEntidad.setValue(
				'razonSocial',
				razonSocialEntidadEdicion,
				{
					shouldValidate: true,
					shouldDirty: false,
				},
			);
		}
	}, [
		editandoEntidadNueva,
		razonSocialEntidadEdicion,
	]);

	const obtenerNombreRazonSocial = (razonSocial: any) => {

		const partes = razonSocial?.split("_");
		return partes?.slice(-1)[0] || "Nombre no encontrado";
	};

	useEffect(() => {
		obtenerDepatartamentos();
		consultarCatalogoAcreditada();

		if (Array.isArray(responseSolicitud?.solicitudTerceroEntidadEducativaDto)) {
			responseSolicitud?.solicitudTerceroEntidadEducativaDto.forEach(entidad => {
				if (entidad.departamento) {
					obtenerCiudades(entidad.departamento);
				}
			});


			const entidadesMapeadas = responseSolicitud?.solicitudTerceroEntidadEducativaDto.map(entidad => ({
				razonSocial: obtenerNombreRazonSocial(entidad.razonSocial),
				idNivel: entidad.idNivel,
				idProgramaEntidad: entidad.idProgramaEntidad,
				departamento: entidad.departamento,
				ciudad: entidad.ciudad,
				horas: entidad.horas,
				fecha_fin: entidad.fecha_fin,
				numReciboPago: entidad.reciboPago,
				fechalimite: entidad.fechalimite,
				fecha_de_inicio: entidad.fecha_fin,
				valorRecibo: entidad.valorRecibo,
				periodoAcademico: entidad.periodoAcademico,
				idEntidad: entidad.idEntidad,
				programaEducativo: entidad.programaEducativo,
				nivelEducativo: entidad.nivelEducativo,

			}));

			if (entidadesMapeadas) {
				setDatosEntidades(entidadesMapeadas);
			}

		} else {

			setDatosEntidades([]);
		}
	}, [listaEntidades, responseSolicitud]);

	const consultarCatalogoAcreditada = async () => {
		const ACREDITACION = 88;
		await api
			.getObtenerValoresCatalogosPorIdCatalogo(ACREDITACION)
			.then(async (response: any) => {
				if (response) {
					if (response.isSuccess) {
						setEntidadAcreditada(response?.data);
					}
				}
			})
			.catch(() => {
				toast('Error al consultar entidades acreditadas', { type: toast.TYPE.ERROR });
			});
	};

	const onChangeEntidadEducativa = async (
		event: any,
		newValue: ENTIDADEDUCATIVA | null,
	) => {
		setEntidadEducativa(newValue);
		setProgramas([]);
		setNiveles([]);

		formularioEntidad.setValue(
			'programaentidad',
			'',
		);

		formularioEntidad.setValue(
			'nivelEducativo',
			'',
		);

		if (newValue) {
			formularioEntidad.setValue(
				'razonSocial',
				`${newValue.idEntidad}_` +
				`${newValue.nit}_` +
				`${newValue.razonSocial}`,
			);

			await obtenerPrograma(
				newValue.idEntidad,
			);

			formularioEntidad.setValue(
				'acreditada',
				newValue.esAcreditada !== 479
					? 480
					: 479,
			);
			return;
		}

		formularioEntidad.setValue(
			'razonSocial',
			'',
		);
	};

	const onChangeDepartment = (event: any, newValue: DEPARTAMENTODTO) => {
		if (newValue !== null) {
			setDepartamentoValue(newValue)
			setCiudadValue(null)
			obtenerCiudades(String(newValue?.id))
			formularioEntidad.setValue('departamento', newValue.id)
			formularioEntidad.setValue('ciudad', 0)
		}
	};

	const obtenerPrograma = async (entidad: string) => {
		setLoading(true);
		await api.getAllProgramaEducativoPorEntidad(entidad)
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						setProgramas(response.data);
						setLoading(false);
					}
				}
			})
			.catch(() => {
				setLoading(false);
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};

	const obtenerDepatartamentos = async () => {
		await api.getAllStates()
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						setDepartamentos(response?.data?.filter((item: DEPARTAMENTODTO) => item.id !== 0));
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};

	const obtenerCiudades = async (idDepartamento: string) => {
		await api.getCityOfState(idDepartamento)
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						setCiudadValues(response.data);
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};

	const onChangeCity = (event: any, newValue: CITYDTO | null) => {
		setCiudadValue(newValue);
		formularioEntidad.setValue('ciudad', newValue?.id as number)
	};

	const normalizarTexto = (valor: any): string =>
		String(valor ?? '')
			.trim()
			.toUpperCase()
			.normalize('NFD')
			.replace(/[\u0300-\u036f]/g, '');

	const abrirModalPrograma = (
		entidad?: ENTIDADEDUCATIVA | null,
	) => {
		const entidadPrograma =
			entidad ??
			entidadEducativa ??
			entidadPendientePrograma;

		if (!entidadPrograma?.idEntidad) {
			toast(
				'No fue posible identificar la entidad educativa.',
				{ type: toast.TYPE.ERROR },
			);
			return;
		}

		setModoEdicionPrograma(false);
		setProgramaEnEdicion(null);
		const entidadProgramaLimpia = {
			...entidadPrograma,
			razonSocial: limpiarRazonSocialEntidad(
				entidadPrograma.razonSocial,
			),
		};

		setEntidadPendientePrograma(
			entidadProgramaLimpia,
		);

		setOpenModalPrograma(true);
	};

	const abrirModalEdicionPrograma = () => {
		if (!editandoEntidad) {
			toast(
				'Primero debe seleccionar una entidad para editar.',
				{ type: toast.TYPE.INFO },
			);
			return;
		}

		if (!entidadEducativa?.idEntidad) {
			toast(
				'No fue posible identificar la entidad educativa.',
				{ type: toast.TYPE.ERROR },
			);
			return;
		}

		const idProgramaSeleccionado =
			formularioEntidad.getValues(
				'programaentidad',
			);

		const programaSeleccionado =
			programas.find(
				programa =>
					String(
						programa.idProgramaEntidad,
					) ===
					String(
						idProgramaSeleccionado,
					),
			);

		if (!programaSeleccionado) {
			toast(
				'No fue posible identificar el programa educativo seleccionado.',
				{ type: toast.TYPE.ERROR },
			);
			return;
		}

		const entidadEdicionLimpia = {
			...entidadEducativa,
			razonSocial:
				limpiarRazonSocialEntidad(
					entidadEducativa.razonSocial,
				),
		};

		setEntidadPendientePrograma(
			entidadEdicionLimpia,
		);

		const programaParaEdicion = {
			...programaSeleccionado,

			idTipoPrograma:
				programaSeleccionado?.idTipoPrograma ??
				programaSeleccionado?.tipoProgramaId ??
				programaSeleccionado?.idValorTipoPrograma ??
				programaSeleccionado?.tipoPrograma?.idValor ??
				programaSeleccionado?.tipoPrograma?.id ??
				'',

			tipoProgramaDescripcion:
				programaSeleccionado
					?.tipoProgramaDescripcion ??
				programaSeleccionado
					?.descripcionTipoPrograma ??
				programaSeleccionado
					?.tipoPrograma?.descripcion ??
				programaSeleccionado
					?.tipoPrograma ??
				'',
		};

		console.log(
			'[PROGRAMA SELECCIONADO PARA EDITAR]',
			programaParaEdicion,
		);

		setProgramaEnEdicion(
			programaParaEdicion,
		);

		setModoEdicionPrograma(true);
		setOpenModalPrograma(true);
	};

	const asociarPrograma = async ({
		entidad,
		programaCreado,
		programasActualizados,
	}: ProgramaEducativoCreadoResult) => {
		setProgramas(programasActualizados);

		setNiveles([
			{
				id:
					programaCreado
						.nivelEducativoId,
				descripcion:
					programaCreado
						.nivelEducativo,
			},
		]);

		setEntidadEducativa(entidad);

		const razonSocialLimpia =
			limpiarRazonSocialEntidad(
				entidad?.razonSocial,
			);

		formularioEntidad.setValue(
			'razonSocial',
			razonSocialLimpia,
			{
				shouldValidate: true,
				shouldDirty: true,
			},
		);

		formularioEntidad.setValue(
			'programaentidad',
			programaCreado.idProgramaEntidad,
			{
				shouldValidate: true,
				shouldDirty: true,
			},
		);

		formularioEntidad.setValue(
			'nivelEducativo',
			programaCreado.nivelEducativoId,
			{
				shouldValidate: true,
				shouldDirty: true,
			},
		);

		formularioEntidad.setValue(
			'acreditada',
			480,
		);

		const mensaje = modoEdicionPrograma
			? 'El programa educativo fue actualizado correctamente. Presione EDITAR ENTIDAD para aplicar los cambios al registro.'
			: 'El programa educativo fue asociado correctamente. Complete los demás datos y presione AGREGAR ENTIDAD.';

		if (!modoEdicionPrograma) {
			setModoCrearEntidad(false);
			setEntidadListaParaAgregar(true);
		}

		setOpenModalPrograma(false);
		setProgramaEnEdicion(null);
		setModoEdicionPrograma(false);
		setEntidadPendientePrograma(null);

		await NotificationService.genericAlert(
			modoEdicionPrograma
				? 'Programa actualizado'
				: 'Programa creado',
			mensaje,
		);
	};

	const TIPO_PERSONA_NATURAL = 1;

	const crearEntidadEducativaRepaes = async () => {
		const data = formularioEntidad.getValues();

		const razonSocial = String(
			data?.razonSocial ?? '',
		).trim();

		const tipoIdentificacion = Number(
			data?.tipoDocumentoNuevaEntidad,
		);

		const numeroDocumento = String(
			data?.numeroDocumentoNuevaEntidad ?? '',
		).trim();

		const departamento = Number(
			data?.departamento ?? 0,
		);

		const ciudad = Number(
			data?.ciudad ?? 0,
		);

		if (
			!razonSocial ||
			!tipoIdentificacion ||
			!numeroDocumento ||
			!departamento ||
			!ciudad
		) {
			toast(
				'Debe completar entidad educativa, tipo de documento, número de documento, departamento y ciudad.',
				{ type: toast.TYPE.ERROR },
			);

			return;
		}

		const documentoSeleccionado =
			listaTiposDocumento.find(
				(item: IDENTIFICATIONTYPEDTO) =>
					Number(item?.id) ===
					tipoIdentificacion,
			);



		const entidadNueva: EntidadesEducativasAgregar = {
			tipoIdentificacion,
			nit: numeroDocumento,
			razonSocial,
			estado: true,
			esAcreditada: 480,
			tipoPersona: TIPO_PERSONA_NATURAL,
			correoElectronico: '',
			ciudad,
			direccion: '',
			telefono: '',
			contacto: '',
			departamento,
		};

		setLoading(true);

		try {
			const responseCreacion =
				await startAddEducationalEntitiesFromRepaes(
					entidadNueva,
				);

			const mensajeRetorno = String(
				responseCreacion?.returnMessage ?? '',
			).trim();

			if (
				!responseCreacion?.isSuccess ||
				mensajeRetorno
			) {
				await NotificationService
					.genericAlertError(
						'Error',
						mensajeRetorno ||
						'No fue posible crear la entidad educativa.',
					);

				return;
			}

			const entidadCreada =
				responseCreacion?.data;

			const idEntidadCreada =
				entidadCreada?.idEntidad;

			if (!idEntidadCreada) {
				await NotificationService
					.genericAlertError(
						'Error',
						'La entidad fue creada, pero el servicio no retornó su identificador.',
					);

				return;
			}

			setIdsEntidadesCreadasFormulario(
				(idsActuales: string[]) => {
					if (
						idsActuales.includes(
							idEntidadCreada,
						)
					) {
						return idsActuales;
					}

					return [
						...idsActuales,
						idEntidadCreada,
					];
				},
			);

			const entidadNuevaCreada = {
				...entidadCreada,
				idEntidad: idEntidadCreada,
				nit:
					entidadCreada?.nit ||
					numeroDocumento,
				razonSocial:
					entidadCreada?.razonSocial ||
					razonSocial,
				esAcreditada:
					entidadCreada
						?.esAcreditada ?? 480,
				creadaDesdeFormulario: true,
			} as ENTIDADEDUCATIVA & {
				creadaDesdeFormulario: boolean;
			};

			setEntidadEducativa(
				entidadNuevaCreada,
			);

			setEntidadPendientePrograma(
				entidadNuevaCreada,
			);

			setEntidadesDisponibles(
				entidadesActuales => {
					const entidadYaExiste =
						entidadesActuales.some(
							item =>
								String(
									item.idEntidad,
								) ===
								String(
									idEntidadCreada,
								),
						);

					if (entidadYaExiste) {
						return entidadesActuales;
					}

					return [
						...entidadesActuales,
						entidadNuevaCreada,
					];
				},
			);

			setProgramas([]);
			setNiveles([]);

			formularioEntidad.setValue(
				'programaentidad',
				'',
			);

			formularioEntidad.setValue(
				'nivelEducativo',
				'',
			);

			formularioEntidad.setValue(
				'acreditada',
				480,
			);

			await abrirModalPrograma(
				entidadNuevaCreada,
			);
		} catch (error) {
			console.error(
				'Error creando entidad educativa desde REPAES:',
				error,
			);

			toast(
				'Error al crear la entidad educativa.',
				{ type: toast.TYPE.ERROR },
			);
		} finally {
			setLoading(false);
		}
	};

	const agregarEntidad = (data: any) => {

		const datoEncontrado = listaEntidades.findIndex(
			item =>
				item.id === data.id &&
				item.id !== entidadAnterior,
		);

		let valorReciboTotal = 0;
		if (listaEntidades.length === 0) {
			valorReciboTotal = UtilService.cleanCurrencyValue(data?.valorRecibo);
			setValorReciboTotal(valorReciboTotal);
		}
		if (listaEntidades.length > 0) {
			valorReciboTotal = UtilService.cleanCurrencyValue(data?.valorRecibo);
			listaEntidades.forEach(item => {
				if (item) {
					valorReciboTotal += UtilService.cleanCurrencyValue(item.valorRecibo);
				}
			})
			setValorReciboTotal(valorReciboTotal);
		}

		if (datoEncontrado === -1) {
			agregarRegistroEntidad(data);
			if (valorReciboTotal < validarRetiro) {
				toast('El Valor Recibo no puede ser menor al Valor de la Solicitud', {
					type: toast.TYPE.WARNING
				});
			}
		} else {
			toast('La entidad ya existe en la lista', {
				type: toast.TYPE.ERROR,
			});
		}
	};

	const agregarRegistroEntidad = (data: any) => {

		const programa = programas.find(
			item =>
				String(item.idProgramaEntidad) ===
				String(data?.programaentidad),
		);

		if (!programa) {
			toast(
				'Debe seleccionar un programa educativo válido.',
				{ type: toast.TYPE.ERROR },
			);
			return;
		}


		const educativo = programas.find(
			(item) => item.nivelEducativoId === data?.nivelEducativo
		);

		const ciudad = ciudadValues.find(
			(item) => item.id === data?.ciudad
		);

		let idRegistro = 1;

		if (listaEntidades.length > 0) {
			idRegistro =
				Math.max(
					...listaEntidades.map(
						item => Number(item?.id) || 0,
					),
				) + 1;
		}

		const esEntidadCreadaDesdeFormulario =
			entidadEducativa?.creadaDesdeFormulario ===
			true ||
			editandoEntidadNueva;

		let numeroDocumentoEntidad =
			data?.razonSocial?.split('_')[1] || '';

		let razonSocialEntidad =
			data?.razonSocial || '';

		if (esEntidadCreadaDesdeFormulario) {
			numeroDocumentoEntidad = String(
				data?.numeroDocumentoNuevaEntidad ||
				entidadEducativa?.nit ||
				'',
			).trim();

			const nombreEntidad =
				limpiarRazonSocialEntidad(
					data?.razonSocial ||
					entidadEducativa?.razonSocial,
				);

			razonSocialEntidad =
				`${entidadEducativa.idEntidad}_` +
				`${numeroDocumentoEntidad}_` +
				`${nombreEntidad}`;
		}

		const nuevaEntidad = {
			id: idRegistro,

			idEntidad:
				entidadEducativa?.idEntidad || '',

			razonSocial:
				razonSocialEntidad,

			numDoc:
				numeroDocumentoEntidad,

			tipoDocumentoNuevaEntidad:
				data?.tipoDocumentoNuevaEntidad ||
				entidadEducativa?.tipoIdentificacion ||
				'',

			numeroDocumentoNuevaEntidad:
				numeroDocumentoEntidad,

			programaentidad:
				programa?.idProgramaEntidad,

			nombrePrograma:
				programa?.programa,

			nivelEducativo:
				programa?.nivelEducativoId,

			nombreNivelEducativo:
				programa?.nivelEducativo,

			numReciboPago:
				data.numReciboPago,

			fechaLimitePago:
				UtilService.getDateCustom(
					data.fechaLimitePago,
				),

			departamento:
				data?.departamento,

			ciudad:
				data?.ciudad,

			ciudadNombre:
				ciudad?.nombre,

			modalidad_estudio:
				data?.modalidad_estudio,

			periodoAcademico:
				data?.periodoAcademico,

			horas:
				data?.horas,

			Fecha_de_inicio:
				UtilService.getDateCustom(
					data?.Fecha_de_inicio,
				),

			Fecha_fin:
				UtilService.getDateCustom(
					data?.Fecha_fin,
				),

			acreditada:
				Number(data?.acreditada),

			valorRecibo:
				data?.valorRecibo,

			creadaDesdeFormulario:
				esEntidadCreadaDesdeFormulario,
		};

		let entidades: any[] = [];

		if (descEstadoBoton === 'AGREGAR ENTIDAD') {
			setListaEntidades(oldArray => [
				...oldArray,
				nuevaEntidad,
			]);
			entidades = [...listaEntidades];
			entidades.push(nuevaEntidad);
		} else {
			const temp = [...listaEntidades];
			const posicionActual = temp.findIndex(
				item => item.id === entidadAnterior,
			);

			nuevaEntidad.id = temp[posicionActual].id;

			if (
				Number(nuevaEntidad.id) !==
				temp[posicionActual].id
			) {
				temp[posicionActual].id =
					nuevaEntidad.id;
			} else {
				nuevaEntidad.id = idAnteriorEntidad;
			}
			temp.splice(posicionActual, 1);
			temp.splice(posicionActual, 0, nuevaEntidad);
			entidades = temp;
			setListaEntidades(temp);
			let valorReciboTotal = 0;
			if (temp.length > 0) {
				valorReciboTotal = UtilService.cleanCurrencyValue(data?.valorRecibo);
				temp.forEach(item => {
					if (item) {
						valorReciboTotal += UtilService.cleanCurrencyValue(item.valorRecibo);
					}
				})
				setValorReciboTotal(valorReciboTotal);
			}
		}

		setListaEntidades(entidades);
		setEstadoEntidadNueva(entidades);
		setEntidadAnterior(0);
		resetEntidades();
	};

	const seleccionarRegistroEntidad = async (item: any) => {
		await api.getCityOfState(item?.departamento)
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						setCiudadValues(response.data);
						const ciudadSeleccionada = response?.data.find((x: any) => x?.id === item?.ciudad);
						setCiudadValue(ciudadSeleccionada || null);
					}
				}
			})
			.catch(() => {
				toast('Error al consultar ciudades en seleccionar registro', { type: toast.TYPE.ERROR });
			});
		const idEntidad = item?.razonSocial.split("_")[0];
		// const entidad = listaEntidadEducativa.find(x => x.idEntidad === idEntidad);
		const entidad = entidadesDisponibles.find(
			x => x.idEntidad === idEntidad,
		);
		const esEntidadNueva =
			item?.creadaDesdeFormulario === true;

		setEditandoEntidad(true);
		setEditandoEntidadNueva(esEntidadNueva);
		setModoCrearEntidad(false);

		await api.getAllProgramaEducativoPorEntidad(idEntidad)
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						setProgramas(response.data);
						const programa = response?.data.find((x: any) => x.idProgramaEntidad === item?.programaentidad);
						const nivel: NIVELEDUCATIVO[] = [
							{
								id:
									programa?.nivelEducativoId ||
									'',
								descripcion:
									programa?.nivelEducativo ||
									'',
							},
						];
						setNiveles(nivel);

					}
				}
			})
			.catch(() => {
				toast('Error al consultar programas en seleccionar registro', { type: toast.TYPE.ERROR });
			});
		const departamento = departamentos.find(x => x?.id === item?.departamento);
		setEntidadAnterior(item.id);
		setIdAnteriorEntidad(item.id);
		setDepartamentoValue(departamento);
		setEntidadEducativa(entidad);
		if (esEntidadNueva) {
			const razonSocialLimpia =
				limpiarRazonSocialEntidad(
					item?.razonSocial,
				);

			setRazonSocialEntidadEdicion(
				razonSocialLimpia,
			);

			formularioEntidad.setValue(
				'tipoDocumentoNuevaEntidad',
				item?.tipoDocumentoNuevaEntidad ||
				entidad?.tipoIdentificacion ||
				'',
			);

			formularioEntidad.setValue(
				'numeroDocumentoNuevaEntidad',
				item?.numeroDocumentoNuevaEntidad ||
				item?.numDoc ||
				entidad?.nit ||
				'',
			);
		} else {
			setRazonSocialEntidadEdicion('');

			formularioEntidad.setValue(
				'razonSocial',
				item?.razonSocial,
			);
		}

		setMostrarCampoHoras(
			Number(item?.periodoAcademico) === 3,
		);

		formularioEntidad.setValue('razonSocial', item?.razonSocial);
		formularioEntidad.setValue('programaentidad', item?.programaentidad);
		formularioEntidad.setValue('nivelEducativo', item?.nivelEducativo);
		formularioEntidad.setValue('numReciboPago', item?.numReciboPago);
		formularioEntidad.setValue('fechaLimitePago', item?.fechaLimitePago);
		formularioEntidad.setValue('departamento', item?.departamento);
		formularioEntidad.setValue('ciudad', item?.ciudad);
		formularioEntidad.setValue('modalidad_estudio', item?.modalidad_estudio);
		formularioEntidad.setValue('periodoAcademico', item?.periodoAcademico);
		formularioEntidad.setValue('horas', item?.horas);
		formularioEntidad.setValue('Fecha_de_inicio', item?.Fecha_de_inicio);
		formularioEntidad.setValue('Fecha_fin', item?.Fecha_fin);
		formularioEntidad.setValue('acreditada', item?.acreditada);
		formularioEntidad.setValue('valorRecibo', item?.valorRecibo);
		let razonSocialFormulario =
			item?.razonSocial || '';

		if (esEntidadNueva) {
			razonSocialFormulario =
				limpiarRazonSocialEntidad(
					item?.razonSocial,
				);
		}

		formularioEntidad.setValue(
			'razonSocial',
			razonSocialFormulario,
			{
				shouldValidate: true,
				shouldDirty: false,
			},
		);
		setDescEstadoBoton('EDITAR ENTIDAD');
	};

	const resetEntidades = () => {
		setModoCrearEntidad(false);
		setDescEstadoBoton('AGREGAR ENTIDAD');
		setEntidadEducativa(null);
		setProgramas([]);
		setNiveles([]);
		setDepartamentoValue(null);
		setCiudadValues([]);
		setCiudadValue(null);
		formularioEntidad.clearErrors();
		setOpenModalPrograma(false);
		setEntidadPendientePrograma(null);
		setEntidadListaParaAgregar(false);
		setEditandoEntidad(false);
		setEditandoEntidadNueva(false);
		setMostrarCampoHoras(false);
		setRazonSocialEntidadEdicion('');
		formularioEntidad.reset({
			...valoresInicialesEntidad,
			fechaLimitePago: null,
			Fecha_de_inicio: null,
			Fecha_fin: null,
		});
	}

	const activarModoCrearEntidad = () => {
		resetEntidades();

		// 480 corresponde a NO
		formularioEntidad.setValue('acreditada', 480);

		setModoCrearEntidad(true);
	};

	const eliminarEntidad = async (id: number) => {
		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);
		if (confirmAlert) {
			const temp = [...listaEntidades];
			temp.splice(
				temp.findIndex(item => item.id === id),
				1,
			);
			setListaEntidades(temp);
			setEstadoEntidadNueva(temp);
			await NotificationService.genericAlert(
				'Eliminado',
				'Se eliminó con éxito el registro.',
			);
			resetEntidades();
			let valorReciboTotal = 0;
			if (temp.length === 0) {
				setValorReciboTotal(0);
			}
			if (temp.length > 0) {
				temp.forEach(item => {
					if (item) {
						valorReciboTotal += UtilService.cleanCurrencyValue(item.valorRecibo);
					}
				})
				setValorReciboTotal(valorReciboTotal);
			}
		}
	}

	const mostrarDepartamento = (idDepartamento: number) => {
		const departamento = departamentos.find(
			(x: any) => x.id === idDepartamento
		);
		return departamento?.nombre || '';
	}

	const mostrarCiudad = (idCiudad: number) => {
		const ciudad = ciudadValues.find(
			(x: any) => x.id === idCiudad
		);
		return ciudad?.nombre || '';
	}

	const limpiarRazonSocialEntidad = (
		valor: any,
	): string => {
		let texto = String(valor || '').trim();

		const formatoCompuesto =
			/^[^_]+_[^_]+_/;

		/*
		 * Puede venir:
		 * id_documento_nombre
		 * o id_documento_id_documento_nombre
		 */
		while (formatoCompuesto.test(texto)) {
			const partes = texto.split('_');

			if (partes.length < 3) {
				break;
			}

			const primerValor = partes[0];

			const pareceId =
				primerValor.includes('-') ||
				primerValor.length >= 20;

			if (!pareceId) {
				break;
			}

			texto = partes.slice(2).join('_');
		}

		return texto;
	};

	function obtenerNombre(razonSocial: any) {
		const regex = /^[^_]*_[^_]*_(.*)$/;
		const resultado = razonSocial.match(regex);
		return resultado ? resultado[1] : razonSocial;
	}

	const mostrarModalidadEstudio = (modalidadEstudio: number) => {
		let modalidadEstu;
		if (modalidadEstudio === 0) {
			modalidadEstu = 'PRESENCIAL';
		} else if (modalidadEstudio === 1) {
			modalidadEstu = 'VIRTUAL';
		} else {
			modalidadEstu = 'SEMIPRESENCIAL';
		}
		return modalidadEstu;
	}

	const mostrarPeriodoAcademico = (periodoAcademico: number, modalidadEstudio: number) => {
		let nombrePeriodoAcademico;
		if (periodoAcademico === 0) {
			nombrePeriodoAcademico = 'SEMESTRE'
		}
		else if (periodoAcademico === 1) {
			nombrePeriodoAcademico = 'CUATRIMESTRE'
		} else if (modalidadEstudio === 2) {
			nombrePeriodoAcademico = 'TRIMESTRE'
		} else {
			nombrePeriodoAcademico = 'INTENSIDAD HORARIA '
		}
		return nombrePeriodoAcademico || '';
	}

	const formatDate = (dateString: string | number | Date) => {
		if (!dateString) return '';

		const date = new Date(dateString);


		const day = String(date.getDate()).padStart(2, '0');
		const month = String(date.getMonth() + 1).padStart(2, '0');
		const year = date.getFullYear();


		let hours = date.getHours();
		hours = hours % 12 || 12;

		return `${day}/${month}/${year}`;
	};

	const obtenerTextoBotonEntidad = (): string => {
		if (!modoCrearEntidad) {
			return descEstadoBoton;
		}

		if (entidadPendientePrograma?.idEntidad) {
			return 'CREAR PROGRAMA';
		}

		return 'GUARDAR ENTIDAD';
	};

	const obtenerFechaMinimaLimitePago = (): Date => {
		const fechaMinima = new Date();

		fechaMinima.setHours(0, 0, 0, 0);
		fechaMinima.setDate(fechaMinima.getDate() + 5);

		return fechaMinima;
	};

	const validarFechaLimitePago = (fecha: any) => {
		if (!fecha) {
			return 'Este campo es requerido.';
		}

		const fechaSeleccionada = new Date(fecha);
		fechaSeleccionada.setHours(0, 0, 0, 0);

		const fechaMinima = obtenerFechaMinimaLimitePago();

		if (fechaSeleccionada < fechaMinima) {
			return 'La fecha límite de pago debe ser mínimo cinco días después de la fecha actual.';
		}

		return true;
	};

	const mostrarCamposCreacionEntidad =
		modoCrearEntidad ||
		editandoEntidadNueva;


	return (
		<CHPaper title="Información Entidad Educativa">
			<LoadingV2Component key="loader" loading={loading} tittle='Consultando Programas' />
			{
				(siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) || campoSoloLecturaRadicacionAprobada ? '' : <Grid container>

					{mostrarCamposCreacionEntidad ? (
						<Grid className={classes.column} item xs={12} md={3}>
							<Controller
								name="razonSocial"
								control={formularioEntidad.control}
								rules={{
									required: 'Este campo es requerido.',
								}}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										InputLabelProps={{ shrink: true }}
										label="Entidad Educativa o Razón Social *"
										value={value}
										onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
											onChange(event.target.value.toUpperCase());
										}}
										error={!!error}
										helperText={error ? error.message : null}
									/>
								)}
							/>
						</Grid>
					) : (
						<Grid className={classes.column} item xs={12} md={3}>
							<Controller
								name="razonSocial"
								rules={{
									required: 'Este campo es requerido',
								}}
								control={formularioEntidad.control}
								defaultValue=""
								render={({ fieldState: { error } }) => (
									<CHAutoCompleteV2
										value={entidadEducativa}
										// options={listaEntidadEducativa}
										options={entidadesDisponibles}
										getOptionLabel={(option: any) => option.razonSocial}
										onChange={onChangeEntidadEducativa}
										renderInput={params => (
											<CHTextField
												{...params}
												label="Entidad Educativa"
												InputLabelProps={{ shrink: true }}
												error={!!error}
												helperText={error ? error.message : null}
											/>
										)}
									/>
								)}
							/>
						</Grid>
					)}
					{!modoCrearEntidad && (
						<>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="programaentidad"
									control={formularioEntidad.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) =>
										modoCrearEntidad ? (
											<CHTextField
												InputLabelProps={{ shrink: true }}
												label="Programa Educativo *"
												value={value}
												onChange={onChange}
												error={!!error}
												helperText={error ? error.message : null}
											/>
										) : (
											<CHSelect
												error={!!error}
												label="Programa Educativo"
												value={value}
												helperText={error ? error.message : null}
												onChange={value => {
													const dato = value.target.value as string;

													const programa = programas.find(
														item => item.idProgramaEntidad === dato,
													);

													const nivel: NIVELEDUCATIVO[] = [
														{
															id: programa?.nivelEducativoId || '',
															descripcion: programa?.nivelEducativo || '',
														},
													];

													setNiveles(nivel);

													formularioEntidad.setValue(
														'nivelEducativo',
														nivel[0].id,
													);

													onChange(value);
												}}
											>
												{programas?.map(
													(option: PROGRAMAPORENTIDADDTO) => (
														<MenuItem
															key={option.idProgramaEntidad}
															value={option.idProgramaEntidad}
														>
															{option.programa}
														</MenuItem>
													),
												)}
											</CHSelect>
										)
									}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="nivelEducativo"
									control={formularioEntidad.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) =>
										modoCrearEntidad ? (
											<CHTextField
												InputLabelProps={{ shrink: true }}
												label="Nivel Educativo *"
												value={value}
												onChange={onChange}
												error={!!error}
												helperText={error ? error.message : null}
											/>
										) : (
											<CHSelect
												readOnly
												error={!!error}
												label="Nivel Educativo"
												value={value}
												helperText={error ? error.message : null}
												onChange={onChange}
											>
												{niveles?.map((option: NIVELEDUCATIVO) => (
													<MenuItem
														key={option.id}
														value={option.id}
													>
														{option.descripcion}
													</MenuItem>
												))}
											</CHSelect>
										)
									}
								/>
							</Grid>
						</>
					)}


					{mostrarCamposCreacionEntidad && (
						<>
							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="tipoDocumentoNuevaEntidad"
									control={formularioEntidad.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											error={!!error}
											label="Tipo de Documento *"
											value={value}
											helperText={error ? error.message : null}
											onChange={onChange}
										>
											{listaTiposDocumento.map(
												(option: IDENTIFICATIONTYPEDTO) => (
													<MenuItem
														key={option.id}
														value={option.id}
													>
														{option.descripcion}
													</MenuItem>
												),
											)}
										</CHSelect>
									)}
								/>
							</Grid>

							<Grid className={classes.column} item xs={12} md={3}>
								<Controller
									name="numeroDocumentoNuevaEntidad"
									control={formularioEntidad.control}
									rules={{
										required: 'Este campo es requerido.',
										minLength: {
											value: 4,
											message:
												'La longitud mínima es de 4 caracteres.',
										},
										maxLength: {
											value: 12,
											message:
												'La longitud máxima es de 12 caracteres.',
										},
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHTextField
											numberMask
											InputLabelProps={{ shrink: true }}
											label="Número de Documento *"
											value={value}
											onChange={onChange}
											error={!!error}
											helperText={error ? error.message : null}
										/>
									)}
								/>
							</Grid>
						</>
					)}



					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="departamento"
							rules={{
								validate: () => departamentos === null ? "Seleccione un departamento" : true
							}}
							control={formularioEntidad.control}
							render={({ fieldState: { error } }) => (
								<CHAutoComplete
									value={departamentoValue}
									options={departamentos}
									getOptionLabel={(option: any) => option.nombre}
									onChange={onChangeDepartment}
									renderInput={params => (
										<CHTextField
											{...params}
											error={!!error}
											helperText={
												error
													? error.message
													: null
											}
											InputLabelProps={{
												shrink: true,
											}}
											label="Departamento *"
										/>
									)}
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="ciudad"
							rules={{
								validate: () => ciudadValue === null ? "Seleccione una ciudad" : true
							}}
							control={formularioEntidad.control}
							render={({ fieldState: { error } }) => (
								<CHAutoCompleteV2
									value={
										ciudadValue == null
											? null
											: ciudadValue
									}
									options={ciudadValues}
									getOptionLabel={(option: any) => option.nombre}
									onChange={onChangeCity}
									renderInput={params => (
										<CHTextField
											{...params}
											InputLabelProps={{ shrink: true }}
											label="Ciudad *"
											error={!!error}
											helperText={
												error ? error.message : null
											}
										/>
									)}
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="modalidad_estudio"
							control={formularioEntidad.control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									error={!!error}
									label="Modalidad de Estudio"
									value={value}
									helperText={error ? error.message : null}
									onChange={value => {
										onChange(value);
									}}
								>

									<MenuItem key={0} value={0}>
										PRESENCIAL
									</MenuItem>
									<MenuItem key={1} value={1}>
										VIRTUAL
									</MenuItem>
									<MenuItem key={2} value={2}>
										SEMIPRESENCIAL
									</MenuItem>

								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="periodoAcademico"
							control={formularioEntidad.control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									error={!!error}
									label="Periodo Académico"
									value={value}
									helperText={error ? error.message : null}
									onChange={value => {

										if (value.target.value === 3) {
											setMostrarCampoHoras(true)
										} else {
											setMostrarCampoHoras(false)
											formularioEntidad.setValue('horas', '')
										}
										onChange(value.target.value);
									}}
								>

									<MenuItem key={0} value={0}>
										SEMESTRE
									</MenuItem>
									<MenuItem key={1} value={1}>
										CUATRIMESTRE
									</MenuItem>
									<MenuItem key={2} value={2}>
										TRIMESTRE
									</MenuItem>
									<MenuItem key={3} value={3}>
										INTENSIDAD HORARIA
									</MenuItem>

								</CHSelect>
							)}
						/>
					</Grid>
					{
						mostrarCampoHoras ? <Grid className={classes.column} item xs={12} md={3}>
							<Controller
								name="horas"
								rules={{
									required: 'Este campo es requerido.',
								}}
								control={formularioEntidad.control}
								render={({
									field: { onChange, value },
									fieldState: { error },
								}) => (
									<CHTextField
										error={!!error}
										helperText={error ? error.message : null}
										numberOnlyMask
										label="Intensidad Horaria*"
										value={value}
										onChange={onChange}
									/>
								)}
							/>
						</Grid> : null
					}

					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="numReciboPago"
							rules={{
								required: 'Este campo es requerido.',
							}}
							control={formularioEntidad.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									error={!!error}
									helperText={error ? error.message : null}
									numberOnlyMask
									label="Número Recibo De Pago *"
									InputLabelProps={{ shrink: true, }}
									value={value}
									onChange={onChange}
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="fechaLimitePago"
							control={formularioEntidad.control}
							rules={{
								required: 'Este campo es requerido.',
								validate: validarFechaLimitePago,
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHDatePicker
									error={!!error}
									label="Fecha Límite De Pago"
									value={value}
									helperText={error ? error.message : null}
									minDate={obtenerFechaMinimaLimitePago()}
									onChange={onChange}
									allowKeyboardControl={false}
									autoOk
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="Fecha_de_inicio"
							control={formularioEntidad.control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHDatePicker
									error={!!error}
									label="Fecha Inicio"
									value={value}
									helperText={error ? error.message : null}
									onChange={onChange}
									allowKeyboardControl={false}
									autoOk
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="Fecha_fin"
							control={formularioEntidad.control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHDatePicker
									error={!!error}
									label="Fecha Fin"
									value={value}
									helperText={error ? error.message : null}
									onChange={onChange}
									allowKeyboardControl={false}
									autoOk
								/>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="acreditada"
							control={formularioEntidad.control}
							rules={{
								required: 'Este campo es requerido.',
							}}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									disabled={modoCrearEntidad}
									error={!!error}
									label="Acreditada"
									value={value}
									readOnly
									helperText={error ? error.message : null}
									onChange={value => {
										onChange(value);
									}}
								>
									{entidadAcreditada?.map(
										(option: any) => (
											<MenuItem
												key={option.idValor}
												value={option.idValor}
											>
												{option.descripcion}
											</MenuItem>
										),
									)}
								</CHSelect>
							)}
						/>
					</Grid>
					<Grid className={classes.column} item xs={12} md={3}>
						<Controller
							name="valorRecibo"
							rules={{
								required: 'Este campo es requerido.',
							}}
							control={formularioEntidad.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHTextField
									inputProps={{ style: { textAlign: 'right' } }}
									error={!!error}
									helperText={error ? error.message : null}
									InputLabelProps={{ shrink: true }}
									currencyMask
									label="Valor Recibo *"
									value={value}
									onChange={onChange}
								/>
							)}
						/>
					</Grid>


					<Grid
						className={classes.columnButtonAdd}
						item
						xs={12}
						md={12}
					>
						{editandoEntidad && (
							<Button
								className={classes.buttonAdd}
								variant="outlined"
								disabled={loading}
								onClick={
									abrirModalEdicionPrograma
								}
							>
								EDITAR PROGRAMA
							</Button>
						)}

						<Button
							className={classes.buttonAdd}
							variant="outlined"
							disabled={
								modoCrearEntidad ||
								editandoEntidad ||
								Boolean(entidadEducativa) ||
								entidadListaParaAgregar ||
								loading
							}
							onClick={activarModoCrearEntidad}
						>
							CREAR ENTIDAD
						</Button>

						<Button
							className={classes.buttonAdd}
							variant="outlined"
							disabled={loading}
							onClick={() => {
								if (modoCrearEntidad) {
									if (
										entidadPendientePrograma
											?.idEntidad
									) {
										abrirModalPrograma();
										return;
									}

									crearEntidadEducativaRepaes();
									return;
								}

								formularioEntidad
									.handleSubmit(
										agregarEntidad,
									)();
							}}
						>
							{obtenerTextoBotonEntidad()}
						</Button>

						<Button
							className={classes.buttonAdd}
							variant="outlined"
							onClick={resetEntidades}
						>
							CANCELAR
						</Button>
					</Grid>
				</Grid>

			}

			<ModalProgramaEducativo
				open={openModalPrograma}
				entidad={entidadPendientePrograma}
				modoEdicion={modoEdicionPrograma}
				programaInicial={programaEnEdicion}
				onClose={() => {
					setOpenModalPrograma(false);
					setModoEdicionPrograma(false);
					setProgramaEnEdicion(null);
				}}
				onProgramaGuardado={asociarPrograma}
			/>


			<h3>Entidades</h3>
			<TableContainer component={Paper}>
				<Grid
					className={classes.columnButtonAdd}
					item
					xs={12}
					md={12}
				>
					<Table aria-label="simple table">
						<TableHead>
							<TableRow>
								<TableCell>Razón Social</TableCell>
								<TableCell>Programa Educativo</TableCell>
								<TableCell>Nivel Educativo</TableCell>
								<TableCell>Departamento</TableCell>
								<TableCell>Ciudad</TableCell>
								<TableCell>Modalidad de Estudio</TableCell>
								<TableCell>Perido Academico</TableCell>
								<TableCell>intensidad Horaria</TableCell>
								<TableCell>Fecha Inicio</TableCell>
								<TableCell>Fecha Fin</TableCell>
								<TableCell>Acreditada</TableCell>
								<TableCell>Número Recibo De Pago</TableCell>
								<TableCell>Fecha Limite de Pago</TableCell>
								<TableCell>Valor Recibo</TableCell>
								{((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
									<TableCell align="center" colSpan={2}>
										Acción
									</TableCell>
								}

							</TableRow>
						</TableHead>
						<TableBody>
							{(listaEntidades?.length ? listaEntidades : datosEntidades)?.map((option: any) => (
								<TableRow key={option?.razonSocial?.idEntidad}>
									<TableCell>{obtenerNombre(option?.razonSocial) || 'N/A'}</TableCell>
									<TableCell>{option?.nombrePrograma || option?.programaentidad || option?.programaEducativo || 'N/A'}</TableCell>
									<TableCell>{option?.nombreNivelEducativo || option?.nivelEducativo || option?.nivelEducativo || 'N/A'}</TableCell>
									<TableCell>{mostrarDepartamento(option.departamento)}</TableCell>
									<TableCell>{mostrarCiudad(option.ciudad) || option?.ciudadNombre || option?.ciudad}</TableCell>
									<TableCell>{mostrarModalidadEstudio(option.modalidad_estudio)}</TableCell>
									<TableCell>{mostrarPeriodoAcademico(option.periodoAcademico, option.modalidad_estudio) || option.periodoAcademico}</TableCell>
									<TableCell>{option?.horas}</TableCell>
									<TableCell>{option?.inicio || option?.Fecha_de_inicio || formatDate(option?.fecha_de_inicio)}</TableCell>
									<TableCell>{option?.Fecha_fin || formatDate(option?.fecha_fin)}</TableCell>
									<TableCell>{option?.acreditada === 479 ? 'SI' : 'NO'}</TableCell>
									<TableCell>{option?.numReciboPago}</TableCell>
									<TableCell>{option?.fechalimite || option?.fechaLimitePago}</TableCell>
									<TableCell>{option?.valorRecibo}</TableCell>

									{((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
										<>
											<TableCell >
												<IconButton
													aria-label="Editar"
													color="primary"
													onClick={() => {
														seleccionarRegistroEntidad(option);
													}}
												>
													<Edit />
												</IconButton>
											</TableCell>
											<TableCell >
												<IconButton
													aria-label="Eliminar"
													color="secondary"
													onClick={() => {
														eliminarEntidad(option.id);
													}}
												>
													<Delete />
												</IconButton>
											</TableCell>

										</>
									}

								</TableRow>
							))
							}
						</TableBody>
					</Table>
				</Grid>
			</TableContainer>
		</CHPaper>
	);
}

// export default EntidadEducativa;
const mapDispatchToProps = {
	startAddEducationalEntitiesFromRepaes,
};

export default connect(
	null,
	mapDispatchToProps,
)(EntidadEducativa);