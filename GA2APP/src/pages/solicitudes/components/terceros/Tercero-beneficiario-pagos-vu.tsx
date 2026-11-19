import React, { useEffect, useState } from 'react';
import {
	Button,
	createStyles,
	Grid,
	IconButton,
	makeStyles,
	MenuItem,
	Paper,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
} from '@material-ui/core';
import { Delete, Edit } from '@material-ui/icons';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import UtilService from '../../../../utils/util.service';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import { BeneficiarioModel, TerceroBeneficiarioModel } from '../../../../models/tercero-beneficiario.model';
import NotificationService from '../../../../services/notification.service';
import CreditoApi from '../../../../services/api-credit.service';
import { ENTIDADBANCARIA, IDENTIFICATIONTYPEDTO, VALORCATALOGO } from '../../../../types/catalog.type';
import GenerarGuid from '../funciones/guid.funtions';
import { FORMS_NAMES, PE_ORIGEN, PE_PORCENTAJE, PE_TIPO_VERIFICACION, USER_SARLAFT_FOUND } from '../../../../constants/constants';
import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import { TerceroApoderadoModel } from '../../../../models/tercero-apoderado.model';
import ApiEntidadesEducativas from '../../../../services/api-entidadesEducativas.service';
import { Fidecomiso } from '../../../../types/educational.entities.type';
import { PropietarioMatriculaModel } from '../../../../models/propietario-matricula.model';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
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
	}),
);

function TercerBeneficiariPagosVu({
	listaValorCatalogo,
	listaBeneficiario,
	listaApoderados,
	listaEntidadBancaria,
	listaTipoDocumentos,
	responseSolicitud,
	beneficiarios,
	formulariosSolicitud,
	tercerosBeneficiarioAdicionados,
	obtenerTipoCuenta,
	validarRetiro,
	siEsAprobadaSolcitud,
	listaEntidadesEducativas,
	beneficiarioAfiliado,
	entidadAhorroProgramadoSeguroEducativo,
	campoSoloLecturaRadicacionAprobada,
	propietarioMatriculaModel,
}: {
	listaValorCatalogo: VALORCATALOGO[];
	listaBeneficiario: TerceroBeneficiarioModel[];
	listaEntidadBancaria: ENTIDADBANCARIA[];
	listaTipoDocumentos: IDENTIFICATIONTYPEDTO[];
	listaApoderados: TerceroApoderadoModel[];
	responseSolicitud: ResponseSeveranceModel | undefined;
	beneficiarios: any[];
	formulariosSolicitud: any[];
	tercerosBeneficiarioAdicionados: (terceros: TerceroBeneficiarioModel[]) => void;
	obtenerTipoCuenta: (catalogoId: number) => void;
	validarRetiro: any;
	siEsAprobadaSolcitud: boolean;
	listaEntidadesEducativas: any[];
	beneficiarioAfiliado: BeneficiarioModel[];
	entidadAhorroProgramadoSeguroEducativo: any;
	campoSoloLecturaRadicacionAprobada: any;
	propietarioMatriculaModel: PropietarioMatriculaModel[];
}) {

	const classes = useStyles();
	const apiCredito = CreditoApi();
	const apiEntidadesEducativas = ApiEntidadesEducativas();
	const [terceroBeneficiarioModel, setTerceroBeneficiarioModel] = useState<TerceroBeneficiarioModel[]>([]);
	const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR BENEFICIARIO');
	const [numDocAnterior, setNumDocAnterior] = useState<string>('');
	const [entidadRegistro, setEntidadRegistro] = useState<ENTIDADBANCARIA | null>(null);
	const [validarValorTipo, setValidarValorType] = useState<any>("");
	const [loading, setLoading] = useState(false);
	const [nullEntidadRegistro, setNullEntidadRegistro] = React.useState(false);
	const [nuevoArray, setNuevoArray] = React.useState<any>(beneficiarios);
	const [beneficiarioEdicionTemporal, setBeneficiarioEdicionTemporal] =
		useState<any | null>(null);

	const controlBeneficiario = useForm({
		defaultValues: {
			id: '',
			tipoDocObj: '',
			numDoc: '',
			razonsocial: '',
			entidadObj: '',
			tipoCuentaObj: '',
			numeroCuenta: '',
			ConfirmarnumeroCuenta: '',
			apoderado: '',
			valorRetirarDos: '',
		},
	});

	// useEffect(() => {
	// 	obtenerTipoCuenta(67);
	// 	if (listaBeneficiario) {
	// 		if (listaBeneficiario.length > 0) {
	// 			listaBeneficiario.forEach(x => {
	// 				x.entidad =
	// 					x.entidad === '00000000-0000-0000-0000-000000000000'
	// 						? null
	// 						: x.entidad;
	// 			});
	// 			setTerceroBeneficiarioModel(listaBeneficiario);
	// 		}
	// 	}
	// }, []);

	useEffect(() => {
		obtenerTipoCuenta(67);
	}, []);

	useEffect(() => {
		const listaNormalizada = Array.isArray(listaBeneficiario)
			? listaBeneficiario.map((x: any) => ({
				...x,
				entidad:
					x.entidad === '00000000-0000-0000-0000-000000000000'
						? null
						: x.entidad,
			}))
			: [];

		setTerceroBeneficiarioModel(listaNormalizada);
	}, [listaBeneficiario]);

	useEffect(() => {

		const cargarLista = async () => {

			const ENTIDAD_ACREDITADA = 479;
			const x: any[] = [];
			if (responseSolicitud?.timCodigo === "REPAES") {
				if (listaEntidadesEducativas?.length) {

					const existenEntidadesAcreditadas = listaEntidadesEducativas.some(entidad => entidad?.acreditada === ENTIDAD_ACREDITADA);

					if (existenEntidadesAcreditadas) {
						listaEntidadesEducativas.forEach((element: any) => {
							if (element.acreditada === ENTIDAD_ACREDITADA) {
								x.push({
									...element,
									tipo: "Entidad",
									numDoc: element.numDoc,
									tipoDoc: "2",
									razonSocial: element.razonSocial.split('_')[2],
								});
							}
						});

						const respuestas = await Promise.all(
							listaEntidadesEducativas.map((entidad) => entidad?.acreditada === ENTIDAD_ACREDITADA && apiEntidadesEducativas.GetCuentas(entidad.idEntidad)));

						respuestas.forEach((response: any) => {
							if (response?.data?.length) {
								response.data.forEach((fideicomiso: Fidecomiso) => {
									x.push({
										...fideicomiso,
										tipo: "Fideicomiso",
										numDoc: fideicomiso.nroIdentificacion,
										tipoDoc: fideicomiso.idTipoIdentificacion,
										razonSocial: fideicomiso.nombreRazonSocial,
										esAutorizado: true,
									});
								});
							}
						});
					}
					else if (beneficiarioAfiliado?.length) {
						const afiliado = beneficiarioAfiliado.find((e: any) => e.tipo === "Afiliado");
						if (afiliado) {
							x.push(afiliado);
						}
					}
				}
			}

			if (responseSolicitud?.timCodigo === 'CONSNUEVA' ||
				responseSolicitud?.timCodigo === 'REPARVIV' ||
				responseSolicitud?.timCodigo === 'ADQUITER' ||
				responseSolicitud?.timCodigo === 'VIUSADA' ||
				responseSolicitud?.timCodigo === 'DSV' ||
				responseSolicitud?.timCodigo === 'DAVAV' ||
				responseSolicitud?.timCodigo === 'DAVAC') {
				if (beneficiarioAfiliado?.length) {
					const afiliado = beneficiarioAfiliado.find((e: any) => e.tipo === "Afiliado");
					if (afiliado) {
						x.push(afiliado);
					}
				}
			}

			if (responseSolicitud?.timCodigo === "AHOPROEDU") {
				if (entidadAhorroProgramadoSeguroEducativo?.length) {
					entidadAhorroProgramadoSeguroEducativo.forEach((element: any) => {
						x.push({
							...element,
							tipo: "Ahorro Programado",
							numDoc: element.nit || '',
							tipoDoc: element.tipoIdentificacion,
						});
					});
				}
			}

			if (listaApoderados?.length) {
				listaApoderados.forEach((element: any) => {
					if (element.esAutorizado) {
						x.push({
							...element,
							tipo: "Apoderado Autorizado",
						});
					}
				});
			}

			if (responseSolicitud?.timCodigo === 'VINUEVA') {

				if (beneficiarioAfiliado?.length) {
					const afiliado = beneficiarioAfiliado.find((e: any) => e.tipo === "Afiliado");
					if (afiliado) {
						x.push(afiliado);
					}
				}

				if (propietarioMatriculaModel?.length) {
					propietarioMatriculaModel.forEach((element: any) => {
						if (element?.razonSocial && element.tipoIdentificacion.id === 2) {
							const propietarioNuevo = {
								...element,
								tipo: "Propietario",
								numDoc: element.numeroIdentificacion,
								tipoDoc: element.tipoIdentificacion.id,
								razonSocial: element?.razonSocial.toUpperCase()
							}
							x.push(propietarioNuevo)
						}

					});
				}
			}

			if(responseSolicitud?.timCodigo === 'AVAC' || responseSolicitud?.timCodigo === 'AVAV'){
				if (beneficiarioAfiliado?.length) {
					const afiliado = beneficiarioAfiliado.find((e: any) => e.tipo === "Afiliado");
					if (afiliado) {
						x.push(afiliado);
					}
				}
			}

			if (formulariosSolicitud.some((item: any) => item === FORMS_NAMES.PROPIETARIO_MATRICULA)
				&& responseSolicitud?.timCodigo !== 'ADQUITER'
				&& responseSolicitud?.timCodigo !== 'VINUEVA'
				&& responseSolicitud?.timCodigo !== 'VIUSADA') {

				if (propietarioMatriculaModel?.length) {
					propietarioMatriculaModel.forEach((element: any) => {
						const propietarioNuevo = {
							...element,
							tipo: "Propietario",
							numDoc: element.numeroIdentificacion,
							tipoDoc: element.tipoIdentificacion.id,
							razonSocial: element?.razonSocial ? element?.razonSocial.toUpperCase() : `${element?.primerNombre || ''} ${element?.primerApellido || ''}`
						}
						x.push(propietarioNuevo)
					});
				}
			}

			setNuevoArray(x);
		};

		cargarLista();

	}, [
		responseSolicitud,
		beneficiarioAfiliado,
		entidadAhorroProgramadoSeguroEducativo,
		listaApoderados,
		propietarioMatriculaModel,
		listaEntidadesEducativas,
		beneficiarios
	]);

	const cancelarTerceroBeneficiario = async () => {
		setDescEstadoBoton('AGREGAR BENEFICIARIO');
		setBeneficiarioEdicionTemporal(null);
		controlBeneficiario.reset()
		setEntidadRegistro(null)
	};

	const agregarTerceroBeneficiario = async (data: any) => {

		const numeroDocumento = data.apoderado.split('_')[2] || '';
		const datoEncontrado = terceroBeneficiarioModel.findIndex(
			item =>
				item.numDoc === numeroDocumento &&
				item.numDoc !== numDocAnterior,
		);

		if (datoEncontrado === -1) {
			await agregaRegistroTerceroBeneficiario(data);
		} else {
			toast('Ya existe un Beneficiario con el mismo número de documento en la lista', {
				type: toast.TYPE.ERROR,
			});
		}
	};

	const agregaRegistroTerceroBeneficiario = async (data: any) => {

		setLoading(true);

		let canContinue = true;
		try {

			const sarlaftResponse = await apiCredito.PostVerificacionTerc({
				pe_Origen: PE_ORIGEN,
				pe_Dato: data?.apoderado.split('_')[1],
				pe_TipoVerificacion: PE_TIPO_VERIFICACION,
				pe_Porcentaje: PE_PORCENTAJE
			})
			if (sarlaftResponse?.isSuccess && sarlaftResponse?.data) {
				canContinue = !((sarlaftResponse?.data?.encontrado as string) === USER_SARLAFT_FOUND.SI);
				setLoading(false)
			}
		} catch (_) {

			setLoading(false)

		}

		if (canContinue) {

			const quitatPesos = UtilService.cleanCurrencyValue(data.valorRetirarDos);

			if (data.numeroCuenta !== data.ConfirmarnumeroCuenta) {
				toast('Número de Cuenta y Confirmación de Número deben ser iguales.', {
					type: toast.TYPE.ERROR,
				});
				data.numeroCuenta = "";
				controlBeneficiario.setValue('ConfirmarnumeroCuenta', '');
				setLoading(false);
				return;
			}

			if (quitatPesos <= validarRetiro) {

				let terceros: TerceroBeneficiarioModel[] = [];

				const idBeneficiarioActual = controlBeneficiario.getValues('id');
				const guid = GenerarGuid();

				const idBeneficiario =
					descEstadoBoton === 'EDITAR BENEFICIARIOS' && idBeneficiarioActual
						? idBeneficiarioActual
						: guid.newGuid();

				let terceroNuevo: any = {}

				const tipoBeneficiario = data.apoderado.split('_')[0];
				const entidadId =
					data.entidadObj !== '' ? data.entidadObj.split('_')[0] : null;

				const tipoDocumento = listaTipoDocumentos.find(
					item => item.id === Number(data.apoderado.split('_')[1])
				);

				terceroNuevo = {
					id: idBeneficiario,
					tipo: tipoBeneficiario,
					tipoDoc: data.apoderado.split('_')[1],
					numDoc: data.apoderado.split('_')[2],
					razonsocial: data.apoderado.split('_')[3],
					tipoDocObj: {
						id: data.apoderado.split('_')[1],
						descripcion: tipoDocumento?.descripcion,
					},
					esAutorizado: true,
					entidad: entidadId === '00000000-0000-0000-0000-000000000000' ? null : entidadId,
					entidadObj: {
						id: entidadId === '00000000-0000-0000-0000-000000000000' ? null : entidadId,
						descripcion: data.entidadObj !== '' ? data.entidadObj.split('_')[1] : null,
					},
					tipoCuenta: data.tipoCuentaObj.split('_')[0],
					tipoCuentaObj: {
						id: data.tipoCuentaObj.split('_')[0],
						descripcion: data.tipoCuentaObj.split('_')[1],
					},
					numeroCuenta: data.numeroCuenta,
					ConfirmarnumeroCuenta: data.ConfirmarnumeroCuenta,
					valorRetirarDos: String(quitatPesos),
				} as TerceroBeneficiarioModel

				if (descEstadoBoton === 'AGREGAR BENEFICIARIO') {


					terceros = [...terceroBeneficiarioModel];
					if (terceros?.length === 0 && quitatPesos > validarRetiro) {
						toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR });
						return;
					}

					if (terceros && terceros?.length > 0) {
						let valorTotalBeneficiados = 0;
						terceros.forEach((element: any) => {
							valorTotalBeneficiados += Number(element.valorRetirarDos);
						})
						const total = valorTotalBeneficiados + quitatPesos;
						if (total > validarRetiro) {
							toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR });
							return;
						}
					}

					setTerceroBeneficiarioModel(oldArray => [
						...oldArray,
						terceroNuevo,
					]);

					terceros.push(terceroNuevo);

					// se elimina fila tipo
					const arrayFinal = terceros.map(({ tipo, ...array }) => array);

					tercerosBeneficiarioAdicionados(arrayFinal);
					cancelarTerceroBeneficiario();
					setLoading(false);

				} else {
					const temp = [...terceroBeneficiarioModel];
					const posicionActual = temp.findIndex(
						x => x.numDoc === numDocAnterior
					);

					if (temp && temp?.length > 0) {
						let valorTotalBeneficiados = 0;
						temp.forEach((element: any, index: any) => {
							if (index !== posicionActual) {
								valorTotalBeneficiados += Number(element.valorRetirarDos);
							}
						})
						const total = valorTotalBeneficiados + Number(terceroNuevo?.valorRetirarDos);
						if (total > validarRetiro) {
							toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR });
							return;
						}
					}

					temp.splice(posicionActual, 1);
					temp.splice(posicionActual, 0, terceroNuevo);
					setTerceroBeneficiarioModel(temp);
					// se elimina fila tipo
					const arrayFinal = temp.map(({ tipo, ...array }) => array);
					tercerosBeneficiarioAdicionados(arrayFinal);
					setNumDocAnterior('');
					cancelarTerceroBeneficiario();
					setLoading(false);
				}
			} else {
				toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR });
			}

		} else {
			console.log('.');
			setLoading(false);
		}
	}

	const eliminarRegistroTercero = async (
		item: string,
	) => {

		const confirmAlert = await NotificationService.confirmAlert(
			'¿Eliminando registro?',
		);
		if (confirmAlert) {
			const temp = [...terceroBeneficiarioModel];
			temp.splice(
				temp.findIndex(x => x.id === item),
				1,
			);
			setTerceroBeneficiarioModel(temp);

			// se elimina fila tipo
			const arrayFinal = temp.map(({ tipo, ...array }) => array);
			tercerosBeneficiarioAdicionados(arrayFinal);
			cancelarTerceroBeneficiario();
		}
	};

	// const seleccionarRegistro = async (item: TerceroBeneficiarioModel) => {

	// 	setNumDocAnterior(item.numDoc);
	// 	setDescEstadoBoton('EDITAR BENEFICIARIOS');

	// 	const newValores: ENTIDADBANCARIA = {

	// 		idEntidadBancaria: item.entidadObj.id,
	// 		numeroIdentificacion: '',
	// 		nombreRazonSocial: item.entidadObj.descripcion,
	// 		codigoBanco: 0,
	// 		estado: true,
	// 		creadoPor: '',
	// 		fechaCreacion: '',
	// 		modificadoPor: '',
	// 		fechaModificacion: '',
	// 	}

	// 	setEntidadRegistro(newValores || null)

	// 	setNullEntidadRegistro(false);

	// 	controlBeneficiario.setValue(
	// 		'entidadObj',
	// 		`${item?.entidadObj?.id}_${item?.entidadObj?.descripcion}`,
	// 	);

	// 	if (item.tipo === 'Entidad' || item.tipo === 'Ahorro Programado') {
	// 		controlBeneficiario.setValue(
	// 			'apoderado',
	// 			`${item.tipo}_${item.tipoDoc}_${item.numDoc}_${item.razonsocial}_Principal`,
	// 		);
	// 	} else {
	// 		controlBeneficiario.setValue(
	// 			'apoderado',
	// 			`${item.tipo}_${item.tipoDoc}_${item.numDoc}_${item.razonsocial}_`,
	// 		);

	// 	}

	// 	controlBeneficiario.setValue(
	// 		'tipoCuentaObj',
	// 		`${item.tipoCuentaObj.id}_${item.tipoCuentaObj.descripcion}`,
	// 	);

	// 	controlBeneficiario.setValue(
	// 		'entidadObj',
	// 		`${item?.entidadObj?.id}_${item?.entidadObj?.descripcion}`,
	// 	);
	// 	controlBeneficiario.setValue('id', item?.id);

	// 	controlBeneficiario.setValue('valorRetirarDos', item.valorRetirarDos || '');

	// 	if (item.tipoCuenta.toString() === '322') {
	// 		item.tipoCuentaObj.descripcion = '';
	// 		controlBeneficiario.setValue('numeroCuenta', '');
	// 	} else {
	// 		controlBeneficiario.setValue('numeroCuenta', item.numeroCuenta);
	// 	};
	// };

	const construirValueBeneficiario = (option: any): string => {
		const tipo = option?.tipo || 'Afiliado';
		const tipoDoc = option?.tipoDoc || option?.tipoDocObj?.id || '';
		const numDoc = option?.numDoc || option?.numeroIdentificacion || '';

		const nombreCompleto = `${option?.primerNombre || ''} ${option?.segundoNombre || ''} ${option?.primerApellido || ''} ${option?.segundoApellido || ''}`.trim();

		const razonSocial =
			option?.razonSocial ||
			option?.razonsocial ||
			nombreCompleto;

		const nombreParaValue =
			tipoDoc?.toString() !== '2' && tipo !== 'Fideicomiso'
				? nombreCompleto || razonSocial
				: razonSocial;

		const principal = option?.esAutorizado ? '' : 'Principal';

		return `${tipo}_${tipoDoc}_${numDoc}_${nombreParaValue}_${principal}`;
	};

	const obtenerTipoDocumentoBeneficiario = (persona: any): string =>
		String(
			persona?.tipoDoc || persona?.tipoDocObj?.id || '',
		).trim();


	const obtenerNumeroDocumentoBeneficiario = (persona: any): string =>
		String(
			persona?.numDoc || persona?.numeroIdentificacion || '',
		).trim()

	const esMismoBeneficiario = (primero: any, segundo: any): boolean =>
	(
		obtenerTipoDocumentoBeneficiario(primero) ===
		obtenerTipoDocumentoBeneficiario(segundo) &&
		obtenerNumeroDocumentoBeneficiario(primero) ===
		obtenerNumeroDocumentoBeneficiario(segundo)
	);


	const opcionesBeneficiario = [...nuevoArray];

	if (beneficiarioEdicionTemporal) {
		const yaExisteEnLista = opcionesBeneficiario.some((option: any) =>
			esMismoBeneficiario(option, beneficiarioEdicionTemporal),
		);

		if (!yaExisteEnLista) {
			opcionesBeneficiario.push(beneficiarioEdicionTemporal);
		}
	}

	const seleccionarRegistro = async (item: TerceroBeneficiarioModel) => {
		setNumDocAnterior(item.numDoc);
		setDescEstadoBoton('EDITAR BENEFICIARIOS');

		const tipoCuentaId = item?.tipoCuentaObj?.id || item?.tipoCuenta;
		const tipoCuentaDescripcion = item?.tipoCuentaObj?.descripcion || '';
		const tipoCuentaValue = `${tipoCuentaId}_${tipoCuentaDescripcion}`;

		const newValores: ENTIDADBANCARIA = {
			idEntidadBancaria: item?.entidadObj?.id,
			numeroIdentificacion: '',
			nombreRazonSocial: item?.entidadObj?.descripcion,
			codigoBanco: 0,
			estado: true,
			creadoPor: '',
			fechaCreacion: '',
			modificadoPor: '',
			fechaModificacion: '',
		};

		setEntidadRegistro(newValores || null);
		setNullEntidadRegistro(false);

		controlBeneficiario.setValue(
			'entidadObj',
			`${item?.entidadObj?.id}_${item?.entidadObj?.descripcion}`,
		);

		const beneficiarioSeleccionado = nuevoArray.find((option: any) =>
			esMismoBeneficiario(option, item),
		);

		let beneficiarioParaEditar = beneficiarioSeleccionado;

		if (!beneficiarioParaEditar) {
			const apoderadoRelacionado = listaApoderados?.find((apoderado: any) =>
				esMismoBeneficiario(apoderado, item),
			);

			const afiliadoRelacionado = beneficiarioAfiliado?.find((afiliado: any) =>
				esMismoBeneficiario(afiliado, item),
			);

			let tipoBeneficiario = item?.tipo || 'Beneficiario registrado';

			if (apoderadoRelacionado?.esAutorizado) {
				tipoBeneficiario = 'Apoderado Autorizado';
			} else if (afiliadoRelacionado) {
				tipoBeneficiario = 'Afiliado';
			}

			beneficiarioParaEditar = {
				...item,
				tipo: tipoBeneficiario,
				tipoDoc: obtenerTipoDocumentoBeneficiario(item),
				numDoc: obtenerNumeroDocumentoBeneficiario(item),
				razonSocial: item?.razonsocial || item?.razonSocial || '',
				esAutorizado: true,
			};

			setBeneficiarioEdicionTemporal(beneficiarioParaEditar);
		} else {
			setBeneficiarioEdicionTemporal(null);
		}

		const valueBeneficiario = construirValueBeneficiario(
			beneficiarioParaEditar,
		);

		controlBeneficiario.setValue('apoderado', valueBeneficiario);

		controlBeneficiario.setValue('tipoCuentaObj', tipoCuentaValue);

		// Ajuste clave: esto permite que el formulario sepa si es CHEQUE o cuenta normal
		setValidarValorType(tipoCuentaValue);

		controlBeneficiario.setValue(
			'entidadObj',
			`${item?.entidadObj?.id}_${item?.entidadObj?.descripcion}`,
		);

		controlBeneficiario.setValue('id', item?.id);
		controlBeneficiario.setValue('valorRetirarDos', item.valorRetirarDos || '');

		if (String(tipoCuentaId) === '322') {
			// No mutar item.tipoCuentaObj.descripcion
			controlBeneficiario.setValue('numeroCuenta', item.numeroCuenta || '');
			controlBeneficiario.setValue('ConfirmarnumeroCuenta', item.numeroCuenta || '');
		} else {
			controlBeneficiario.setValue('numeroCuenta', item.numeroCuenta || '');

			// Ajuste clave: al editar, también carga la confirmación
			controlBeneficiario.setValue(
				'ConfirmarnumeroCuenta',
				item.numeroCuenta || '',
			);
		}
	};

	const onChangeEntidad = (_: any, newValue: ENTIDADBANCARIA) => {
		setNullEntidadRegistro(newValue === null);
		setEntidadRegistro(newValue);

		if (newValue !== null) {
			const dato = `${newValue.idEntidadBancaria}_${newValue.nombreRazonSocial}`;
			controlBeneficiario.setValue('entidadObj', dato);
		}
	};

	if (loading) {
		return <LoadingComponent loading={loading} />
	}

	const onKeyPressNoNumbers = (event: React.KeyboardEvent<HTMLInputElement>) => {
		const keyCode = event.which || event.keyCode;
		if (keyCode >= 48 && keyCode <= 57) {
			event.preventDefault();
		}
	};

	return (
		<>
			<CHPaper title="Información Beneficiario de Pago Nuevo">
				{
					((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
						<Grid container>
							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="apoderado"
									control={controlBeneficiario.control}
									rules={{
										required: 'Este campo es requerido.',
									}}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											label="Beneficiario *"
											value={value}
											helperText={error ? error.message : null}
											onChange={e => {
												onChange(e)
											}}
											style={{ borderBottom: 'none' }}
										>
											{opcionesBeneficiario.map((option: any) => {
												let apoderado = '';

												const tipoDocumento = String(
													option?.tipoDoc || option?.tipoDocObj?.id || '',
												).trim();

												const esNit = tipoDocumento === '2';

												const nombreCompleto = [
													option?.primerNombre,
													option?.segundoNombre,
													option?.primerApellido,
													option?.segundoApellido,
												]
													.filter(Boolean)
													.join(' ')
													.toUpperCase();

												const nombreRazonSocial = String(
													option?.razonSocial || option?.razonsocial || '',
												)
													.trim()
													.toUpperCase();

												// Personas naturales: cédula, apoderado con cédula o propietario natural
												if (
													!esNit &&
													(
														option?.tipo === 'Apoderado Autorizado' ||
														option?.tipo === 'Afiliado' ||
														option?.tipo === 'Propietario'
													)
												) {
													apoderado = `
														${option?.tipo || 'Beneficiario'} -
														${option?.numDoc || option?.numeroIdentificacion || ''} -
														${nombreCompleto || nombreRazonSocial}
														${option?.esAutorizado || option?.tipo === 'Apoderado Autorizado'
																										? ''
																										: ' Principal'}
													`;
												}

												// Personas jurídicas: NIT, entidad, fideicomiso, ahorro programado
												else if (
													esNit ||
													option?.tipo === 'Entidad' ||
													option?.tipo === 'Ahorro Programado' ||
													option?.tipo === 'Fideicomiso'
												) {
													apoderado = `
														${option?.tipo || option?.tipoIdentificacion || 'Beneficiario'} -
														${option?.numDoc || option?.numeroIdentificacion || ''} -
														${nombreRazonSocial}
														${option?.esAutorizado ? '' : ' Principal'}
													`;
												}

												// Respaldo para cualquier otro tipo de beneficiario
												else {
													const nombreBeneficiario =
														nombreRazonSocial || nombreCompleto;

													apoderado = `
														${option?.tipo || 'Beneficiario registrado'} -
														${option?.numDoc || option?.numeroIdentificacion || ''} -
														${nombreBeneficiario}
													`;
												}

												return (
													<MenuItem
														key={`item-${tipoDocumento}-${option?.numDoc || option?.numeroIdentificacion}`}
														value={construirValueBeneficiario(option)}
													>
														{apoderado}
													</MenuItem>
												);
											})}
										</CHSelect>
									)}
								/>
							</Grid>
							<Grid className={classes.column} item xs={12} md={6} lg={3}>
								<Controller
									name="tipoCuentaObj"
									control={controlBeneficiario.control}
									render={({
										field: { onChange, value },
										fieldState: { error },
									}) => (
										<CHSelect
											label="Tipo de Cuenta *"
											value={value}
											onChange={e => {
												onChange(e);
												setValidarValorType(e.target.value)
											}}
										>
											{listaValorCatalogo?.map(
												(option: any) => {
													const dato = `${option.idValor}_${option.descripcion}`;
													return (
														<MenuItem
															key={option.idValor}
															value={dato}
														>
															{option.descripcion}
														</MenuItem>
													);
												},
											)}
										</CHSelect>
									)}
								/>
							</Grid>

							{validarValorTipo === "322_CHEQUE" && (
								<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="entidadObj"
										control={controlBeneficiario.control}
										rules={{
											required: false,
										}}
										render={({ field: { onChange, value } }) => (
											<CHAutoComplete
												value={nullEntidadRegistro ? null : entidadRegistro}
												onChange={onChangeEntidad}
												options={listaEntidadBancaria}
												getOptionLabel={option => option?.nombreRazonSocial}
												renderInput={params => (
													<CHTextField
														value={value}
														{...params}
														label="Entidad"
														onChange={onChange}
														onKeyPress={onKeyPressNoNumbers}
													/>
												)}
											/>
										)}
									/>
								</Grid>
							)
							}

							{validarValorTipo !== "322_CHEQUE" &&
								(<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="entidadObj"
										rules={{
											required: 'Este campo es requerido.',
										}}
										control={controlBeneficiario.control}
										render={(
											{
												field: { onChange, value },
												fieldState: { error },
											}) => (
											<CHAutoComplete
												value={nullEntidadRegistro ? null : entidadRegistro}
												onChange={onChangeEntidad}
												options={listaEntidadBancaria}
												getOptionLabel={option => option?.nombreRazonSocial}
												renderInput={(params) =>
													<CHTextField
														value={value}
														isReadOnly={false}
														error={!!error}
														helperText={error ? error.message : null}
														{...params}
														label="Entidad Bancaria *"
														onChange={onChange}
														InputLabelProps={{ shrink: true }}
														onKeyPress={onKeyPressNoNumbers}
													/>

												}
											/>
										)}
									/>
								</Grid>)
							}

							{validarValorTipo !== "322_CHEQUE" ?
								<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="numeroCuenta"
										control={controlBeneficiario.control}
										rules={{
											maxLength: {
												value: 21,
												message: 'La longitud máxima del campo es de 21 caracteres',
											},
											required: true,
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												onPasteCapture={e => {
													e.preventDefault();
													const clipboardText = e.clipboardData.getData('text/plain');
													const sanitizedText = clipboardText.replace(/[^0-9]/g, '');
													onChange(sanitizedText);
												}}
												numberOnlyMask
												error={!!error}
												helperText={error ? error.message : null}
												label="Número de Cuenta"
												required
												value={value}
												InputLabelProps={{ shrink: true }}
												onKeyDown={e => {
													const allowedKeys = [8, 9, 35, 36, 37, 39, 46]; // Teclas de navegación y borrado
													if (
														(e.key >= '0' && e.key <= '9') ||
														allowedKeys.includes(e.keyCode)
													) {
														return;
													}
													e.preventDefault();
												}}
												onChange={(e: any) => {
													const sanitizedText = e.target.value.replace(/[^0-9]/g, '');
													if (sanitizedText.length <= 21) {
														onChange(sanitizedText)
													}
												}}
											/>


										)}
									/>
								</Grid> : <Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="numeroCuenta"
										control={controlBeneficiario.control}
										rules={{
											maxLength: {
												value: 21,
												message: 'La longitud máxima del campo es de 21 caracteres',
											},
											required: false,
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												onPasteCapture={e => {
													e.preventDefault();
													const clipboardText = e.clipboardData.getData('text/plain');
													const sanitizedText = clipboardText.replace(/[^0-9]/g, '');
													onChange(sanitizedText);
												}}
												numberOnlyMask
												label="Número de Obligación"
												value={value}
												InputLabelProps={{ shrink: true }}
												onKeyDown={e => {
													const allowedKeys = [8, 9, 35, 36, 37, 39, 46]; // Teclas de navegación y borrado
													if (
														(e.key >= '0' && e.key <= '9') ||
														allowedKeys.includes(e.keyCode)
													) {
														return;
													}
													e.preventDefault();
												}}
												onChange={(e: any) => {
													const sanitizedText = e.target.value.replace(/[^0-9]/g, '');
													if (sanitizedText.length <= 21) {
														onChange(sanitizedText)
													}
												}}
											/>
										)}
									/>
								</Grid>}

							{validarValorTipo !== "322_CHEQUE" ?
								<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="ConfirmarnumeroCuenta"
										control={controlBeneficiario.control}
										rules={{

											maxLength: {
												value: 21,
												message: 'La longitud máxima del campo es de 21 caracteres',
											},
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												onCopy={e => e.preventDefault()}
												onPaste={e => e.preventDefault()}
												onContextMenu={e => e.preventDefault()}
												onPasteCapture={e => {
													e.preventDefault();
													const clipboardText = e.clipboardData.getData('text/plain');
													const sanitizedText = clipboardText.replace(/[^0-9]/g, '');
													onChange(sanitizedText);
												}}
												error={!!error}
												helperText={error ? error.message : null}
												label="Confirmación Número de Cuenta"
												numberOnlyMask
												value={value}
												InputLabelProps={{ shrink: true }}
												onKeyDown={e => {
													const allowedKeys = [8, 9, 35, 36, 37, 39, 46]; // Teclas de navegación y borrado
													if (
														(e.key >= '0' && e.key <= '9') ||
														allowedKeys.includes(e.keyCode)
													) {
														return;
													}
													e.preventDefault();
												}}
												onChange={e => {
													const sanitizedText = e.target.value.replace(/[^0-9]/g, '');
													if (sanitizedText.length <= 21) {
														onChange(sanitizedText)
													}
												}}
											/>
										)}
									/>
								</Grid> :
								<Grid className={classes.column} item xs={12} md={6} lg={3}>
									<Controller
										name="ConfirmarnumeroCuenta"
										control={controlBeneficiario.control}
										rules={{
											maxLength: {
												value: 21,
												message: 'La longitud máxima del campo es de 21 caracteres',
											},
										}}
										render={({
											field: { onChange, value },
											fieldState: { error },
										}) => (
											<CHTextField
												onCopy={e => e.preventDefault()}
												onPaste={e => e.preventDefault()}
												onContextMenu={e => e.preventDefault()}
												onPasteCapture={e => {
													e.preventDefault();
												}}
												error={!!error}
												helperText={error ? error.message : null}
												label="Confirmación Número de Obligación"
												numberOnlyMask
												value={value}
												InputLabelProps={{ shrink: true }}
												onKeyDown={e => {
													const allowedKeys = [8, 9, 35, 36, 37, 39, 46]; // Teclas de navegación y borrado
													if (
														(e.key >= '0' && e.key <= '9') ||
														allowedKeys.includes(e.keyCode)
													) {
														return;
													}
													e.preventDefault();
												}}
												onChange={e => {
													const sanitizedText = e.target.value.replace(/[^0-9]/g, '');
													if (sanitizedText.length <= 21) {
														onChange(sanitizedText)
													}
												}}
											/>
										)}
									/>
								</Grid>

							}
							<Grid className={classes.column2} item xs={12} md={6} lg={3}>
								<Controller
									name="valorRetirarDos"
									control={controlBeneficiario.control}
									rules={{
										required: 'Este campo es requerido.',
										validate: (value) => {
											const valueNumber = UtilService.cleanCurrencyValue(value);
											return valueNumber < 1 ? 'Este valor no es aceptado' : true;
										}
									}}
									render={({
										field: { onChange, value, name },
										fieldState: { error },
									}) => (
										<CHTextField
											currencyMask
											error={!!error}
											helperText={error ? error.message : null}
											inputProps={{
												style: {
													textAlign: 'right',
												},
											}}
											label="Valor a Retirar *"
											value={value}
											InputLabelProps={{ shrink: true }}
											onChange={e => {
												onChange(e)
												controlBeneficiario.trigger(name);
											}}
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
								<Button
									variant="outlined"
									onClick={controlBeneficiario.handleSubmit(
										agregarTerceroBeneficiario
									)}
								>
									{descEstadoBoton}
								</Button>
								<Button
									className={classes.buttonAdd}
									variant="outlined"

									onClick={cancelarTerceroBeneficiario}
								>
									CANCELAR
								</Button>
							</Grid>
						</Grid>
				}

				<h3>Lista de Beneficiarios</h3>
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
									<TableCell>Tipo de Identificación</TableCell>
									<TableCell>Número de Identificación</TableCell>
									<TableCell>Nombre / Razón Social</TableCell>
									<TableCell>Tipo de Cuenta</TableCell>
									<TableCell>Entidad Bancaria</TableCell>
									{
										validarValorTipo !== "322_CHEQUE" ?
											<TableCell>Número de Cuenta</TableCell> :
											<TableCell>Número de Obligación</TableCell>
									}

									<TableCell>Valor a retirar</TableCell>
									{
										((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? null :
											<TableCell align="center">
												Acciones
											</TableCell>
									}
								</TableRow>
							</TableHead>
							<TableBody>
								{terceroBeneficiarioModel.map(
									(option: any) => (
										<TableRow key={option.tipoDoc}>
											<TableCell>
												{option.tipoDocObj.descripcion}
											</TableCell>
											<TableCell>
												{option.numDoc}
											</TableCell>
											<TableCell>
												{option?.razonsocial}
											</TableCell>
											<TableCell>
												{option.tipoCuentaObj.descripcion}
											</TableCell>
											<TableCell>
												{option.entidadObj.descripcion !== null || option.entidadObj.descripcion === 'null'
													? option.entidadObj
														.descripcion
													: ''}
											</TableCell>
											<TableCell>
												{option.numeroCuenta}
											</TableCell>
											<TableCell>
												{UtilService.formatCurrency(Number(option.valorRetirarDos))}
											</TableCell>
											{
												((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :

													<TableCell align="center">
														<IconButton
															aria-label="Editar"
															color="primary"
															onClick={() => {
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
															onClick={() => {
																eliminarRegistroTercero(
																	option.id,
																);
															}}
														>
															<Delete />
														</IconButton>
													</TableCell>
											}

										</TableRow>
									),
								)}
							</TableBody>
						</Table>
					</Grid>
				</TableContainer>
			</CHPaper>
		</>
	);
}

export default TercerBeneficiariPagosVu;