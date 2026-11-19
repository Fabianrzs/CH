import React, { ChangeEvent, useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { useForm } from 'react-hook-form';
import './table-cuentas.scss';
import {
  Button,
  Grid,
  makeStyles,
  createStyles,
  CardContent,
  Card
} from '@material-ui/core';
import swal from 'sweetalert';
import { toast } from 'react-toastify';
import moment from 'moment';
import { AdminCuentasAfiliado } from '../../../../types/client.model.type';
import ModalDetalleDeCuenta from '../popup-component/popup-component';
import ModalMoviemientoDeCuenta from '../popup-component/popup-component-movimiento';
import UtilService from '../../../../utils/util.service';
import { CAUSALBLOQUEOCUENTA, IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import ModalBloqueoDeCuenta from '../popup-component/popup-component-bloqueos';
import NotificationService from '../../../../services/notification.service';
import history from '../../../../utils/history.router';
import ContainerTitle from '../../../../components/containe-title/container-title';
import ModalNuevaCuentaUsuario from '../popup-component/popup-component-nueva-cuenta';
import ModalCambioEstado from '../popup-component/popup-component-cambio-estado';
import { startActualizaBloqueoCuentas } from '../../../../actions/catalog.action';
import ModalInclusionSaldos from '../inclusion-saldo/popup-inclusion-saldo';
import ModalLiquidacionCuentas from '../liquidacion-cuentas/popup-liquidacion-cuentas';
import ModalDevolucionSaldo from '../devolucion-saldo/popup-devolucion-saldo';
import ModalOdenRecaudo from '../orden-recaudo/popup-orden-recaudo';
import { startGetAdministracionCuentasAfiliado } from '../../../../actions/client.action';
import ApiClients from '../../../../services/api-clients.service';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import { ModalInteresesNoCausados } from '../intereses-no-causados/modalInteresesNoCausados';
import { ModalSolicitudNovedad } from '../solicitud-novedades/modalSolicitudNovedad';
import ModalTransferenciaCuentas from '../transferencia-cuentas/modalTransferenciaCuentas';
import ModalSolucionInconsistencias from '../solucion-inconsistencias/modalSolucionInconsistencias';
import { InteresNoCausadoDto } from '../../../../types/verificacion.model.type';
import ApiVerificacionDerecho from '../../../../services/api-verificacion.service';
import ModalUnificacionCuentas from '../unificacion-cuentas/modalUnificacionCuentas';
import ApiSchedule from '../../../../services/api-cronograma.service';
import { UnidadEjecutoraCronogramaDTO } from '../../../../types/cronograma.type';
import { showAlert } from '../../../../utils/AlertService';

const useStyles = makeStyles(() =>
  createStyles({

    buttonprincipal: {
      width: '250px',
      height: '50px',
      marginLeft: '1rem',
      background: '#FEC917'
    },
    buttonsecundario: {
      width: '250px',
      height: '50px',
      marginLeft: '1rem',
      background: '#43b5f1'
    },
    root: {
      flexGrow: 1,
    },
    iconRed: {
      padding: '8px',
      textAlign: 'inherit',
      color: '#FF0000',
      margin: '10px'
    },
  }),
);

const api = ApiClients();
const apiCronograma = ApiSchedule();

function TablaDetalleCuentaAfiliado({
  responseAdminCuenta,
  tipoBloqueo,
  startActualizaBloqueoCuentas,
  actualizarObjetoRespuesta,
  tiposIdentificacion,
}: {
  responseAdminCuenta: AdminCuentasAfiliado;
  tipoBloqueo: CAUSALBLOQUEOCUENTA[];
  startActualizaBloqueoCuentas: (cuenta: any) => void;
  actualizarObjetoRespuesta: (data: any) => void;
  tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
}) {


  const ApiVerificacion = ApiVerificacionDerecho();
  const classes = useStyles();
  const valor = '95ca1858-b89d-4631-9893-22932c925184';

  const [openModalAdd, setopenModalAdd] = useState<boolean>();
  const [openModalCambioEstado, setopenModalCambioEstado] = useState<boolean>();
  const [loading, setLoading] = useState<boolean>(false);
  const [totalCuentas, setTotalCuentas] = useState<number>(0);
  const [openModalAddMovimiento, setopenModalAddMovimiento] = useState<boolean>();
  const [cuentaId, setIdCuentaSelect] = useState<number>(0);
  const [cuenta, setCuentaSelect] = useState<any>(0);
  const [tipoCuenta, setTipoCuenta] = useState<string>('');
  const [openModalBloqueoCuentaConcepto, setopenModalBloqueoCuentaConcepto] = useState<boolean>(false);
  const [openModalNuevaCuenta, setopenModalNuevaCuenta] = useState<boolean>(false);
  const [ejecutora, setEjecutora] = useState<boolean>(false);
  const [adminCuentaResponse, setAdminCuentaResponse] = useState<any>(responseAdminCuenta)
  const [items, setItems] = useState<any[]>(responseAdminCuenta.cuentas);
  const [cuentaSeleccionada, setCuentaSeleccionada] = useState<any | null>(null);

  const [openInclusionSaldo, setOpenInclusionSaldo] = useState(false);
  const [openLiquidacionCuentas, setOpenLiquidacionCuentas] = useState(false);
  const [openDevolucionSaldo, setOpenDevolucionSaldo] = useState(false);
  const [openOrdenRecaudo, setOpenOrdenRecaudo] = useState(false);
  const [firstLoad, setFirstLoad] = useState(true);
  const [approbed, setApprobed] = useState(false);

  const [valueCuenta, setValueCuenta] = useState("");
  const [listadoEmbargos, setListadoEmbargos] = useState<any[]>([])
  const [totalBloqueoAcumulado, setBloqueoAcumulado] = useState<any[]>([])

  const [openModalInteresesNoCausados, setOpenModalInteresesNoCausados] = useState(false);
  const [openModalSolicitudNovedad, setOpenModalSolicitudNovedad] = useState(false);
  const [openModalTransferenciaCuentas, setOpenModalTransferenciaCuentas] = useState(false);
  const [openModalUnificacionCuentas, setOpenModalUnificacionCuentas] = useState(false);
  const [openModalSolucionInconsistencias, setOpenModalSolucionInconsistencias] = useState(false);
  const [actualizaDom, setActualizaDom] = useState(false);

  const [unidadEjecutoraInfo, setUnidadEjecutoraInfo] = useState(null);


  useEffect(() => {
    setItems(responseAdminCuenta.cuentas);
    if (responseAdminCuenta.cuentas.length > totalCuentas) {
      setTotalCuentas(responseAdminCuenta.cuentas.length);
      setopenModalNuevaCuenta(false);
    }

    const porcentaje = 'ae7d141a-fd88-4f57-a199-4db5b5301946';
    const variacionPorcentaje = 'f65c4c4a-174e-4406-8aed-32b5b47cf0b3';

    const embargosTotalLista: any[] = []
    const bloqueosTotalLista: any[] = []
    responseAdminCuenta.cuentas.forEach(element => {
      const totalEmbargos = {
        cuenta: element.cuentaId,
        valorEmbargo: 0,
        porcentaje: 0
      }
      const totalBloqueos = {
        cuenta: element.numeroCuenta,
        valorBloqueo: 0,
      }
      element.listaEmbargos?.forEach(elementEmbargo => {
        if (elementEmbargo.estado === 1) {
          if (elementEmbargo.tipoEmbargoId === porcentaje || elementEmbargo.tipoEmbargoId === variacionPorcentaje) {
            totalEmbargos.valorEmbargo += (element?.conceptos?.find(x => +x.id === elementEmbargo?.conceptoId)?.saldo || 0) * (elementEmbargo.valor / 100);
          } else {
            totalEmbargos.valorEmbargo += elementEmbargo.valor
          }
        }
      });
      element.conceptos?.forEach(concepto => {
        totalBloqueos.valorBloqueo += concepto.valorBloqueoEmbargo
      });

      embargosTotalLista.push(totalEmbargos)
      bloqueosTotalLista.push(totalBloqueos)
    });
    setBloqueoAcumulado(bloqueosTotalLista)
    setListadoEmbargos(embargosTotalLista)
  }, [responseAdminCuenta.cuentas]);

  const handleModalCloseAdd = () => {
    setopenModalAdd(false);
  }

  const handleModalCloseCambioEstado = () => {
    setopenModalCambioEstado(false);
  }

  const handleModalCloseAddMovimiento = () => {
    setopenModalAddMovimiento(false);
  }

  const handleModalCloseBloqueoCuenta = () => {
    setopenModalBloqueoCuentaConcepto(false);
    setCuentaSeleccionada(null);
  }

  const handleModalCloseNuevaCuenta = () => {
    setopenModalNuevaCuenta(false);
  }


  const handleModalOpenBloqueoCuentaConcepto = (
    val: boolean,
    idCuenta: number,
    row: any
  ) => async (event: React.MouseEvent) => {


    event.preventDefault();


    if (val === false) {

      setCuentaSeleccionada(row);

      setIdCuentaSelect(idCuenta);
      setopenModalBloqueoCuentaConcepto(true);
      setTipoCuenta('CUENTA');
      setValueCuenta(`Cuenta 0${responseAdminCuenta.tipoCuentaC[0].idTipoCuentaC}-${row.numeroCuenta}-0`);

    } else {

      await NotificationService.genericAlert(
        '¡Atención!',
        'La cuenta se encuentra bloqueada, para desbloquear dé click sobre el ícono de la tabla.',
        'info'
      );
    }
  };

  const handleModalOpenNuevaCuenta = () => async (event: React.MouseEvent) => {
    if (responseAdminCuenta.siglaUnidadEjecutora === 'CASUR' || responseAdminCuenta.siglaUnidadEjecutora === 'CREMIL') {
      setEjecutora(true);
    }
    setopenModalNuevaCuenta(true);
    event.preventDefault();
  }

  const handleModalOpenAddConcepto = (val: boolean, idCuenta: number) => (event: React.MouseEvent) => {
    setIdCuentaSelect(idCuenta);
    setopenModalAdd(val);
    event.preventDefault();
  }

  const handleModalOpenAddMovimiento = (val: boolean, idCuenta: number) => async (event: React.MouseEvent) => {

    setIdCuentaSelect(idCuenta);
    setopenModalAddMovimiento(val);
    event.preventDefault();

  }

  const handleModalOpenCambioEstado = (val: boolean, cuenta: any, cuentaId: number) => (event: React.MouseEvent) => {
    setCuentaSelect(cuenta);
    setIdCuentaSelect(cuentaId);
    setopenModalCambioEstado(val);
    event.preventDefault();
  }

  const handleChangeBuscar = (event: ChangeEvent<HTMLInputElement>) => {

    if (event.target.value.toUpperCase() === '') {
      setItems(responseAdminCuenta.cuentas);
    } else {
      setItems(filterItems(event.target.value));
    }

  }

  const handleModalInclusionSaldo = async (data: any) => {
    setOpenInclusionSaldo(false);
    setLoading(true)
    await api.postInclusionSaldos(data)
    const response = await api.getAdministracionCuentasAfiliado({ numeroDocumento: responseAdminCuenta.identificacion, tipoDocumentoId: responseAdminCuenta.tipoIdentificacionId });
    if (response.isSuccess) {
      setItems(response.data.cuentas)
      setAdminCuentaResponse(response.data)
      actualizarObjetoRespuesta(response.data)
    }

    setLoading(false)
  }

  const handlePostCrearCuenta = async (data: any) => {
    setLoading(true);
    try {
      const responseCrear = await api.postCrearCuenta(data);
      if (responseCrear.isSuccess) {
        setopenModalNuevaCuenta(false);
        const response = await api.getAdministracionCuentasAfiliado({ numeroDocumento: responseAdminCuenta.identificacion, tipoDocumentoId: responseAdminCuenta.tipoIdentificacionId });
        if (response.isSuccess) {
          setItems(response.data.cuentas)
          setAdminCuentaResponse(response.data)
          actualizarObjetoRespuesta(response.data)
        }
      } else {
        swal({
          text: responseCrear.returnMessage,
          icon: "error",
          buttons: ["ok"]
        });
      }

    } catch {
      swal({
        text: 'Verifique los datos de la cuenta.',
        icon: "error",
        buttons: ["ok"]
      });
      setLoading(false);
    }
    setLoading(false);
  }

  const handlePostCambiarEstado = async (data: any) => {
    setLoading(true);
    try {
      const responseCrear = await api.postCambiarEstado(data);
      if (responseCrear.isSuccess) {
        setopenModalCambioEstado(false);
        const response = await api.getAdministracionCuentasAfiliado({ numeroDocumento: responseAdminCuenta.identificacion, tipoDocumentoId: responseAdminCuenta.tipoIdentificacionId });
        if (response.isSuccess) {
          setItems(response.data.cuentas)
          setAdminCuentaResponse(response.data)
          actualizarObjetoRespuesta(response.data)
        }
      } else {
        swal({
          text: responseCrear.returnMessage,
          icon: "error",
          buttons: ["ok"]
        });
      }

    } catch {
      swal({
        text: 'Verifique los datos de la cuenta.',
        icon: "error",
        buttons: ["ok"]
      });
      setLoading(false);
    }
    setLoading(false);
  }

  const handleClose = () => {
    setOpenInclusionSaldo(false)
  }

  const handleCloseLiquidacionCuenta = () => {
    setOpenLiquidacionCuentas(false)
  }

  const handleCloseDevolucionSaldo = () => {
    setOpenDevolucionSaldo(false)
  }

  const handleCloseOrdenRecaudo = () => {
    setOpenOrdenRecaudo(false)
  }

  const [dataInclusionSaldo, setDataInclusionSaldo] = useState<any>();
  const inclusionSaldoOpenModal = (data: any) => {
    console.log(data)
    setDataInclusionSaldo(data);
    setFirstLoad(false)
    setApprobed(data.aprobacion)
    setOpenInclusionSaldo(true)
  }

  const eliminarInclusionSaldo = async (data: any) => {
    const confirmAlert = await NotificationService.confirmAlert(
      '¿Desea eliminar la Orden de Recaudo?',
    );
    if (confirmAlert) {
      setLoading(true)
      const response = await api.eliminarInclusionSaldosEncabezado(data.idInclusionSaldoEncabezado)
      if (response.isSuccess) {
        const responseUpdate = await api.getAdministracionCuentasAfiliado({ numeroDocumento: responseAdminCuenta.identificacion, tipoDocumentoId: responseAdminCuenta.tipoIdentificacionId });
        if (responseUpdate.isSuccess) {
          setItems(responseUpdate.data.cuentas)
          setAdminCuentaResponse(responseUpdate.data)
          actualizarObjetoRespuesta(responseUpdate.data)
        }
      }
    }
    setLoading(false)
  }

  const onCancel = () => {
    history.push('/AdministracionCuentas');
  }

  const firstLoadTrue = () => {
    setFirstLoad(true)
  }

  const filterItems = (value: string) => {
    const filter = responseAdminCuenta.cuentas.filter(
      (x) =>
        x.numeroCuenta.toString().includes(value) ||
        x.saldo.toString().includes(value) ||
        x.saldoCanje.toString().includes(value) ||
        x.saldoDisponible.toString().includes(value)
    );
    return filter;
  };

  let total = 0;
  if (total === 0) {
    items?.forEach(element => {
      total += element.saldoDisponible
    });

    listadoEmbargos?.forEach(embargo => {
      total -= embargo.valorEmbargo
    })
  }

  const desbloquearCuenta = (data: any) => {
    swal({
      text: "¿Está seguro de desbloquear la cuenta?",
      icon: "info",
      buttons: ["No", "Sí"]
    }).then(async respuesta => {
      if (respuesta) {
        startActualizaBloqueoCuentas({
          CausalBloqueoId: data.causalBloqueoId,
          cuentaId: data.cuentaId,
          conceptoId: 0,
          tipoBloqueo: 'DESBLOQUEO',
          monto: false,
          valor: 0,
          estado: false,
        });


      }
    })
  }

  const handleAddInclusionSaldo = () => {
    setDataInclusionSaldo(null)
    setOpenInclusionSaldo(true)
  }

  const formularioInterecesNoCausados = useForm<InteresNoCausadoDto>({
    defaultValues: {
      anio: moment(new Date()).year(),
      usuarioId: 0,
      concepto: 0,
      ctaId: 0,
      tipoCuentaId: 0,
      saldo: 0,
      interesesNoCausados: 0,
      saldoTotalInteresNoCausado: 0
    },
  });

  const { watch } = useForm({
    defaultValues: {
      idTipoIdentificacion: responseAdminCuenta.tipoIdentificacionId,
      identificacion: responseAdminCuenta.identificacion,
      primerNombre: '',
      segundoNombre: '',
      primerApellido: '',
      segundoApellido: '',
      radicado: '',
      periodoNovedad: new Date(),
      conceptoNovedad: '',
      modalidad: '',
      codigoMilitar: '',
      idUnidadEjecutora: 0,
      idConceptoNovedad: 0,
      codigoAlta: 0,
      codigoBaja: 0,
      fechaInicioNovedad: new Date(),
      fechaFinNovedad: new Date(),
      porcentaje: 0,
      valor: 0,
      esAlta: false,
      esBaja: false,
      observaciones: '',
      token: ''
    }
  });

  const handleOpenModalAdd = async () => {
    const primerCuentaId = items[0].cuentaId;
    const primerCuentaIdTipo = items[0].tipoCuentaId;
    const primerCuentaSaldo = total;

    const request = ({
      anio: moment(new Date()).year(),
      usuarioId: responseAdminCuenta.clienteId,
      concepto: 0,
      ctaId: primerCuentaId,
      tipoCuentaId: primerCuentaIdTipo,
      saldo: primerCuentaSaldo,
      interesesNoCausados: 0,
      saldoTotalInteresNoCausado: 0
    });
    await ApiVerificacion
      .ObtenerInteresesNoCausadosRequest(request)
      .then(async (response: any) => {
        const datos = {
          anio: moment(new Date()).year(),
          usuarioId: response.data.usuarioId,
          concepto: 0,
          ctaId: response.ctaId,
          tipoCuentaId: response.data.tipoCuentaId,
          saldo: response.data.saldo,
          interesesNoCausados: response.data.interesesNoCausados,
          saldoTotalInteresNoCausado: response.data.saldoTotalInteresNoCausado
        }
        formularioInterecesNoCausados.reset(datos);
        setActualizaDom(true)

      }).catch((error: any) => {
        toast("Error del sistema", { type: toast.TYPE.ERROR });

      });
    setOpenModalInteresesNoCausados(true);
  };

  const handleCloseModalAdd = async () => {
    setLoading(false);
    setOpenModalInteresesNoCausados(false);
    formularioInterecesNoCausados.reset();
    setActualizaDom(false)
  };

  const [loadingModal, setLoadingModal] = useState(true);
  const [idConceptoParam, setIdConceptoParam] = useState(0);
  const [periodoParam, setPeriodoParam] = useState<UnidadEjecutoraCronogramaDTO[] | undefined | null>(null);

  function findPeriodoNovedad(unidadEjecutoraId: any, data: any[]) {
    return data.find((item: { idUnidadEjecutora: any; }) => item.idUnidadEjecutora === unidadEjecutoraId);
  }

  const handleOpenModalSolicitud = async () => {

    try {
      setLoading(true);

      const unidadEjecutoraId = responseAdminCuenta?.unidadEjecutoraId;

      const periodoNovedad = await apiCronograma.getExecutingUnitsSchedule();
      const unidadEjecutoraEncontrada = findPeriodoNovedad(unidadEjecutoraId, periodoNovedad.data);
      const periodoNov = unidadEjecutoraEncontrada.periodo;

      setPeriodoParam(periodoNov);
      setIdConceptoParam(unidadEjecutoraId);
      setLoadingModal(true);
      setOpenModalSolicitudNovedad(true);

    } catch (error) {
      toast({ type: toast.TYPE.ERROR });
    } finally {
      setLoadingModal(false);
    }
    setLoading(false);
    return null;
  };

  const handleCloseModalSolicitud = async () => {
    setLoading(false);
    setOpenModalSolicitudNovedad(false);
  };

  type SaldoCuentas = { [tipoCuenta: string]: any };
  const [saldoCuentas, setSaldoCuentas] = useState({});


  const handleOpenModalTransferencia = async () => {
    setOpenModalTransferenciaCuentas(true);
    const cuentasAfiliado = await api.getClientByIdentificationTypeIdentificationNumberRequest(
      watch('idTipoIdentificacion'),
      watch('identificacion')
    );

    const saldoDisponible = cuentasAfiliado.data.cuentas[0]?.saldoDisponible;

    if (cuentasAfiliado.data) {

      const cuentas = cuentasAfiliado.data.cuentas || [];
      const saldoCuentas: SaldoCuentas = {};
      cuentas.forEach((cuenta: { tipoCuenta: string | number; saldo: any; }) => {
        saldoCuentas[cuenta.tipoCuenta] = cuenta.saldo;
        setSaldoCuentas(saldoDisponible);
      });
    }

  };

  const handleCloseModalTransferencia = async () => {
    setLoading(false);
    setOpenModalTransferenciaCuentas(false);
  };

  const handleOpenModalUnificacionCuentas = async () => {
    setLoading(true);

    try {
      setLoading(false);

      setOpenModalUnificacionCuentas(true);
    } catch (error) {
      console.error('Error al cargar datos:', error);

      setLoading(false);
    }
  };

  const handleCloseModalUnificacionCuentas = async () => {
    setLoading(false);
    setOpenModalUnificacionCuentas(false);
  };

  const handleOpenModalSolucionInconsistencias = async () => {
    setOpenModalSolucionInconsistencias(true);
  };

  const handleCloseModalSolucionInconsistencias = async () => {
    setLoading(false);
    setOpenModalSolucionInconsistencias(false);
  };

  const obtenerValorBloqueoReintegro = (numeroCuenta: number) => {
    let valorReintegro = 0;
    const cuenta = items?.find(item => item.numeroCuenta === numeroCuenta);
    if (cuenta) {
      cuenta.conceptos.forEach((concepto: any) => {
        if (concepto.valorDevolver > 0) {
          valorReintegro += concepto.valorDevolver;
        }
      })
    }
    return valorReintegro;
  }

  const obtenerValorEmbargo = (conceptoId: number, valor: number, conceptos: any) => {
    const result = (conceptos?.find((x: any) => x.id === conceptoId).saldo) * (valor / 100);
    return result;
  }

  const obtenerValorTotalEmbargo = () => {
    let result = 0;
    if (responseAdminCuenta && responseAdminCuenta.cuentas.length > 0) {
      responseAdminCuenta.cuentas?.forEach((cuenta) => {
        cuenta.listaEmbargos?.forEach((listaEmbargos) => {
          result += listaEmbargos.tipoEmbargoId === valor ?
            listaEmbargos.valor : obtenerValorEmbargo(listaEmbargos.conceptoId, listaEmbargos.valor, cuenta.conceptos)
        })
      })
    }
    return result;
  }

  const onClickAfiliadosFallecidos = () => {
    showAlert(5);
  }

  const [bloqueosManuales, setBloqueosManuales] = useState<{ [key: string]: number }>({});

  // const handleBloqueoExitoso = (numeroCuenta: string, valorBloqueado: number) => {

  //   setBloqueosManuales(prevBloqueos => ({
  //     ...prevBloqueos,
  //     [numeroCuenta]: (prevBloqueos[numeroCuenta] || 0) + valorBloqueado
  //   }));
  // };

  const handleBloqueoExitoso = (
    numeroCuenta: string,
    valorBloqueado: number,
    conceptoId?: string | number,
    porcentaje?: any,
    cuentaIdOrIdConcepto?: number,
    saldoCuenta?: number
  ) => {

    setBloqueosManuales(prevBloqueos => {
      const nuevosBloqueos = { ...prevBloqueos };
      let valorFinal = valorBloqueado;
      let saldoUsar: number | undefined = saldoCuenta;

      if (saldoUsar === undefined || saldoUsar === null) {
        const cuentaEncontrada = items.find(item => item.cuentaId === cuentaIdOrIdConcepto);
        
        if (cuentaEncontrada) {
          saldoUsar = cuentaEncontrada.saldo;
        }
      }
  
      if (porcentaje === "PORCENTAJE" && saldoUsar !== undefined && saldoUsar !== null) {
        valorFinal = (saldoUsar * (valorBloqueado / 100));
      }
  
  
      if (conceptoId && !Number.isNaN(Number(conceptoId))) {
        const claveConcepto = `${numeroCuenta}-${conceptoId}`;
  
        const valorActualConcepto = prevBloqueos[claveConcepto] || 0;

        nuevosBloqueos[claveConcepto] = valorActualConcepto + valorFinal;
  
        const valorTotalCuentaActual = prevBloqueos[numeroCuenta] || 0;

        nuevosBloqueos[numeroCuenta] = valorTotalCuentaActual + valorFinal;
  
      } else {
        nuevosBloqueos[numeroCuenta] = valorFinal;
      }
  
      return nuevosBloqueos;
    });
  };

  const getConceptDescriptionById = (cuenta: any, idConcepto: any) => {
    const concepto = cuenta.conceptos?.find((c: any) => c.id?.toString() === idConcepto.toString());
    return concepto?.descripcionConcepto || `Bloqueo Manual (ID: ${idConcepto})`;
  };

  const generarBloqueosYEmbargos = (cuenta: any) => {
    const numeroCuentaStr = cuenta.numeroCuenta?.toString() || cuenta.cuentaId?.toString() || '';
    const listaCombinada = [];

    if (cuenta.listaEmbargos && cuenta.listaEmbargos.length > 0) {
      listaCombinada.push(...cuenta.listaEmbargos.map((embargo: any) => ({
        tipo: 'Embargo Judicial',
        conceptoDescripcion: embargo.tipoProceso,
        valorBloqueo: embargo.tipoEmbargoId === valor ? embargo.valor : obtenerValorEmbargo(embargo.conceptoId, embargo.valor, cuenta.conceptos),
        estadoCuenta: cuenta.estadoCuenta,
        numeroCuenta: cuenta.numeroCuenta,
      })));
    }

    if (bloqueosManuales && numeroCuentaStr) {
      Object.entries(bloqueosManuales).forEach(([clave, valorBloqueo]) => {

        if (clave.startsWith(`${numeroCuentaStr}-`) && valorBloqueo > 0) {
          const [cuentaId, idConcepto] = clave.split('-');
          const descripcion = getConceptDescriptionById(cuenta, idConcepto);

          listaCombinada.push({
            tipo: 'Bloqueo Manual',
            conceptoDescripcion: descripcion,
            valorBloqueo: valorBloqueo,
            estadoCuenta: cuenta.estadoCuenta,
            numeroCuenta: cuenta.numeroCuenta,
          });
        }
      });
    }

    return listaCombinada;
  };

  const allBloqueosAndEmbargos = responseAdminCuenta?.cuentas?.flatMap((cuenta) =>
    generarBloqueosYEmbargos(cuenta)
  ) || [];

  return (
    <>
      <Grid>
        <LoadingComponent key="loader" loading={loading} />
        <Grid className="container-24">
          <input type="search" placeholder="Buscar.." onChange={handleChangeBuscar} />
        </Grid>

        <Grid className="container-23">
          <Grid>
            <button type="button" className="bgcian button-i blue flex" onClick={handleModalOpenNuevaCuenta()}  >
              <span className="ico fch-Agregar-0-1 icoMid-2 separador-marg-6" />
              AGREGAR
            </button>
          </Grid>
        </Grid>
        <Grid className="container-17">
          <table className="table">
            <thead>
              <th className="tab-space-01">Número Cuenta</th>
              <th className="tab-space-01">Descripción</th>
              <th className="tab-space-01">Fecha Creación</th>
              <th className="tab-space-01">Saldo</th>
              <th className="tab-space-01">Valor Embargo/Bloqueo</th>
              <th className="tab-space-01">Valor Bloqueos Reintegros</th>
              <th className="tab-space-01">Saldo Disponible</th>
              <th className="tab-space-01">Asignación Salarial</th>
              <th className="tab-space-01">Estado Cuenta</th>
              <th className="tab-space-01">Acciones</th>
            </thead>
            <tbody>
              {items?.map((row, index) => {
                const valorEmbargo = obtenerValorTotalEmbargo();
                const valorBloqueoReintegro = obtenerValorBloqueoReintegro(row.numeroCuenta);
                const valorBloqueoManual = bloqueosManuales[row.numeroCuenta] || 0;

                const totalBloqueoAcumuladoTotal = valorEmbargo + valorBloqueoReintegro + valorBloqueoManual;
                const saldoDisponible = row.saldo - totalBloqueoAcumuladoTotal;

                return (
                  <tr key={`table-${index}`}>
                    <td data-label="Número Cuenta" className="tab-space-01">{row.numeroCuenta}</td>
                    <td data-label="Descripción" className="tab-space-01">{row.nombreCuenta}</td>
                    <td data-label="Descripción" className="tab-space-01">{UtilService.getDate(row.fechaCreacion)}</td>
                    <td data-label="Saldo" className="tab-space-01">{UtilService.formatCurrency(row.saldo)}</td>

                    <td data-label="Valor Embargo" className="tab-space-01">
                      {UtilService.formatCurrency(valorEmbargo + valorBloqueoManual)}
                    </td>
                    <td data-label="Valor Bloqueo Reintegro" className="tab-space-01">
                      {UtilService.formatCurrency(valorBloqueoReintegro)}
                    </td>
                    <td data-label="Saldo Disponible" className="tab-space-01">
                      {UtilService.formatCurrency(saldoDisponible)}
                    </td>

                    <td data-label="Asignacion Salarial" className="tab-space-01">
                      {UtilService.formatCurrency(row.asignacionSalarial)}
                    </td>

                    <td data-label="Cuenta Bloqueada" className="tab-space-01">{row.estadoCuenta}
                      <span aria-label="Cambio Estado" onClick={handleModalOpenCambioEstado(true, row, row.cuentaId || 0)} ><span className="ico fch-Hoja-Vida-0-1 icoMid-3" /></span>
                    </td>

                    <td data-label="Estado" className="tab-space-01 actions">
                      <Grid>
                        <span aria-label="Detalle Cuenta" onClick={handleModalOpenAddConcepto(true, row.cuentaId || 0)} ><span className="ico fch-Hoja-Vida-0-1 icoMid-3" /></span>
                        <span aria-label="Movimiento Cuenta" onClick={handleModalOpenAddMovimiento(true, row.cuentaId || 0)} ><span className="ico fch-Excepciones-1-1 icoMid-3" /></span>
                        <span aria-label="Bloqueos" onClick={handleModalOpenBloqueoCuentaConcepto(row.cuentaBloqueo, row.cuentaId || 0, row)} ><span className="ico fch-Modulo-Bloqueos-0-1 icoMid-3" /></span>
                      </Grid>
                    </td>
                  </tr>
                );
              })}

              <tr>
                <td colSpan={2}><b>TOTAL DISPONIBLE</b></td>
                <td><b>{UtilService.formatCurrency(total - obtenerValorTotalEmbargo())}</b></td>
              </tr>
            </tbody>
          </table>
        </Grid>
      </Grid>

      <div>
        <br />
        <br />
        <br />
        <br />
      </div>

      <div className={classes.root}>
        <Grid item xs={12}>
          <Card>
            <ContainerTitle
              title="Orden de Recaudo"
            />
            <CardContent>
              <Grid className="container-23">
                <Grid>
                  <button type="button" className="bgcian button-i blue flex" onClick={() => handleAddInclusionSaldo()}  >
                    <span className="ico fch-Agregar-0-1 icoMid-2 separador-marg-6" />
                    AGREGAR
                  </button>
                </Grid>
              </Grid>

              <Grid className="container-17">
                <table className="table">
                  <thead>
                    <th className="tab-space-01">Tipo de identificación</th>
                    <th className="tab-space-01">Número de identificación</th>
                    <th className="tab-space-01">Concepto</th>
                    <th className="tab-space-03">Número de recaudo</th>
                    <th className="tab-space-02">Fecha inicial</th>
                    <th className="tab-space-04">Fecha final</th>
                    <th className="tab-space-05">Valor</th>
                    <th className="tab-space-01">Acciones</th>
                  </thead>
                  <tbody>
                    {items?.map((row, index) => (
                      row?.inclusionSaldos?.map((rowInclusionSaldos: any, indexBloqueos: any) => (
                        rowInclusionSaldos?.opciones?.map((rowOpciones: any, indexOpciones: any) => (
                          <tr key={`table-${indexBloqueos}`}>
                            <td data-label="Tipo de identificación" className="tab-space-01">{responseAdminCuenta.tipoIdentificacion}</td>
                            <td data-label="Número de identificación" className="tab-space-01">{responseAdminCuenta.identificacion}</td>
                            <td data-label="Concepto" className="tab-space-01">{rowOpciones.concepto}</td>
                            <td data-label="Número de recaudo" className="tab-space-03">{rowInclusionSaldos.numeroReferencia != null ? rowInclusionSaldos.numeroReferencia : 'N/A'}</td>
                            <td data-label="Fecha inicial" className="tab-space-02">{UtilService.getDate(rowOpciones.periodoDesde)}</td>
                            <td data-label="Fecha final" className="tab-space-04">{UtilService.getDate(rowOpciones.periodoHasta)}</td>
                            <td data-label="Valor" className="tab-space-05">{UtilService.formatCurrency(rowOpciones.valor)}</td>
                            <td className="tab-space-01 ">
                              {
                                !rowInclusionSaldos.aprobacion ?
                                  (
                                    <Grid>
                                      <span aria-label="Detalle">
                                        <span aria-label="Bloqueos" onClick={() => inclusionSaldoOpenModal(rowInclusionSaldos)} ><span className="ico fch-Hoja-Vida-0-1 icoMid-3" /></span>
                                        <span aria-label="Eliminar" onClick={() => eliminarInclusionSaldo(rowInclusionSaldos)}>
                                          <span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} />
                                        </span>
                                      </span>
                                    </Grid>
                                  ) : (
                                    <Grid>
                                      <span aria-label="Detalle">
                                        <span aria-label="Bloqueos" onClick={() => inclusionSaldoOpenModal(rowInclusionSaldos)} ><span className="ico fch-Hoja-Vida-0-1 icoMid-3" /></span>
                                      </span>
                                    </Grid>
                                  )
                              }
                            </td>
                          </tr>
                        ))
                      ))
                    ))}
                  </tbody>
                </table>
              </Grid>

            </CardContent>
          </Card>
        </Grid>
      </div>

      <div>
        <br />
        <br />
        <br />
        <br />
      </div>

      <div className={classes.root}>
        <Grid item xs={12}>
          <Card>
            <ContainerTitle
              title="Bloqueos y embargos de la cuenta"


            />
            <CardContent>
              <Grid className="container-17">
                <table className="table">
                  <thead>
                    <th className="tab-space-01">Cuenta</th>
                    <th className="tab-space-01">Concepto</th>
                    <th className="tab-space-01">Valor Embargo/Bloqueo</th>
                    <th className="tab-space-01">Estado Cuenta</th>
                  </thead>
                  <tbody>
                    {allBloqueosAndEmbargos.length > 0 ? (
                      allBloqueosAndEmbargos.map((item, index) => (
                        <tr key={`${item.numeroCuenta}-${item.tipo}-${index}`}>
                          <td>{item.numeroCuenta}</td>
                          <td>{item.conceptoDescripcion} ({item.tipo})</td>
                          <td>
                            {UtilService.formatCurrency(item.valorBloqueo)}
                          </td>
                          <td>{item.estadoCuenta}</td>
                        </tr>
                      ))
                    ) : (
                      <tr>
                        <td colSpan={4}>
                          <p>El afiliado no tiene bloqueos ni embargos.</p>
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>

              </Grid>
            </CardContent>
          </Card>
        </Grid>
      </div>

      <div>
        <br />
        <br />
        <br />
        <br />
      </div>

      <div className={classes.root}>
        <Grid item xs={12}>
          <Card>
            <ContainerTitle
              title="Conjunto Acciones sobre la cuenta"
            />
            <CardContent>

              <Grid>
                <Grid className="container-17">
                  <Button
                    onClick={() => setOpenLiquidacionCuentas(true)}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    LIQUIDACIÓN CUOTAS
                  </Button>
                  <Button
                    onClick={() => setOpenInclusionSaldo(true)}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    INCLUSIÓN SALDO
                  </Button>
                  <Button
                    onClick={() => setOpenDevolucionSaldo(true)}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    DEVOLUCIÓN SALDO
                  </Button>
                  <Button
                    onClick={() => setOpenOrdenRecaudo(true)}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    ORDEN RECAUDO
                  </Button>
                  <Button
                    className={classes.buttonsecundario}
                    variant="outlined"
                    onClick={onClickAfiliadosFallecidos}
                  >
                    AFILIADOS FALLECIDOS
                  </Button>

                  <Button
                    onClick={handleOpenModalAdd}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    INTERESES NO CAUSADOS
                  </Button>

                  <Button
                    onClick={handleOpenModalSolicitud}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    SOLICITUD DE NOVEDAD
                  </Button>

                  <Button
                    onClick={handleOpenModalTransferencia}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    TRANSFERENCIA ENTRE CUENTAS
                  </Button>

                  <Button
                    onClick={handleOpenModalUnificacionCuentas}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    UNIFICACIÓN DE CUENTAS
                  </Button>

                  <Button
                    onClick={handleOpenModalSolucionInconsistencias}
                    className={classes.buttonsecundario}
                    variant="outlined"
                  >
                    SOLUCIÓN INCONSISTENCIAS
                  </Button>

                </Grid>
              </Grid>

            </CardContent>
          </Card>
        </Grid>
      </div>

      <div className="buttonssolicitud">
        <Button
          className={classes.buttonprincipal}
          variant="outlined"
          onClick={onCancel}
        >
          CANCELAR
        </Button>

      </div>

      {
        openModalAdd && <ModalDetalleDeCuenta
          open={openModalAdd}
          handleClose={handleModalCloseAdd}
          responseAdminCuenta={responseAdminCuenta}
          cuentaId={cuentaId}
          bloqueosManuales={bloqueosManuales}
        />
      }

      <ModalMoviemientoDeCuenta
        open={openModalAddMovimiento}
        handleClose={handleModalCloseAddMovimiento}
        responseAdminCuenta={adminCuentaResponse}
        cuentaId={cuentaId} />

      {
        openModalBloqueoCuentaConcepto &&
        <ModalBloqueoDeCuenta
          open={openModalBloqueoCuentaConcepto}
          handleClose={handleModalCloseBloqueoCuenta}
          cuentaIdOrIdConcepto={cuentaId}
          tipoCuenta={tipoCuenta}
          tipoBloqueos={tipoBloqueo}
          valueCuenta={valueCuenta}
          responseAdminCuenta={responseAdminCuenta}
          idCuenta={cuentaId}
          onBloqueoExitoso={handleBloqueoExitoso}
          numeroCuenta={cuentaSeleccionada?.numeroCuenta || ''}
        />
      }

      {
        openModalNuevaCuenta &&
        <ModalNuevaCuentaUsuario
          startCrearCuentas={handlePostCrearCuenta}
          open={openModalNuevaCuenta}
          handleClose={handleModalCloseNuevaCuenta}
          tipoCuentaC={responseAdminCuenta.tipoCuentaC}
          porcentajeDescuento={responseAdminCuenta.porcentajeDescuentos}
          causalEstadoCuentas={responseAdminCuenta.causalEstadoCuenta}
          idUnidadEjecuta={responseAdminCuenta.unidadEjecutoraId}
          idCliente={responseAdminCuenta.clienteId}
          modalidad={responseAdminCuenta.modalidadesAfiliacion}
          ejecutora={ejecutora}
        />
      }

      {
        openModalCambioEstado &&
        <ModalCambioEstado
          startCambioEstado={handlePostCambiarEstado}
          open={openModalCambioEstado}
          handleClose={handleModalCloseCambioEstado}
          tipoCuentaC={responseAdminCuenta.tipoCuentaC}
          porcentajeDescuento={responseAdminCuenta.porcentajeDescuentos}
          causalEstadoCuentas={responseAdminCuenta.causalEstadoCuenta}
          idUnidadEjecuta={responseAdminCuenta.unidadEjecutoraId}
          idCliente={responseAdminCuenta.clienteId}
          estadosCuenta={responseAdminCuenta.estadosCuenta}
          cuenta={cuenta}
          cuentaId={cuentaId}
        />
      }

      <ModalInclusionSaldos
        open={openInclusionSaldo as boolean}
        handleClose={handleModalInclusionSaldo}
        data={dataInclusionSaldo}
        handleCloseReturn={handleClose}
        firstLoad={firstLoad}
        setFirstLoadTrue={firstLoadTrue}
        approbed={approbed}
      />

      <ModalLiquidacionCuentas
        open={openLiquidacionCuentas as boolean}
        dataCuentas={responseAdminCuenta}
        handleCloseReturn={handleCloseLiquidacionCuenta}
        firstLoad={firstLoad}
        approbed={approbed}
      />

      <ModalDevolucionSaldo
        open={openDevolucionSaldo as boolean}
        dataCuentas={responseAdminCuenta}
        handleCloseReturn={handleCloseDevolucionSaldo}
        firstLoad={firstLoad}
        approbed={approbed}
      />

      <ModalOdenRecaudo
        open={openOrdenRecaudo as boolean}
        dataCuentas={responseAdminCuenta}
        handleCloseReturn={handleCloseOrdenRecaudo}
        firstLoad={firstLoad}
        approbed={approbed}
      />

      <ModalInteresesNoCausados
        setLoading={setLoading}
        open={openModalInteresesNoCausados}
        formularioInterecesNoCausados={formularioInterecesNoCausados}
        onClose={handleCloseModalAdd}
        responseAdminCuenta={adminCuentaResponse}
        idCliente={responseAdminCuenta.clienteId}
        cuentas={items}
        total={total}
        actualizaDom={actualizaDom}
        setActualizaDom={setActualizaDom}
      />

      <ModalSolicitudNovedad
        setLoading={setLoading}
        open={openModalSolicitudNovedad}
        onClose={handleCloseModalSolicitud}
        tiposIdentificacion={tiposIdentificacion}
        responseAdminCuenta={responseAdminCuenta}
        loadingModal={loadingModal}
        periodoParam={periodoParam}
        idConceptoParam={idConceptoParam}
      />

      <ModalTransferenciaCuentas
        open={openModalTransferenciaCuentas}
        onClose={handleCloseModalTransferencia}
        responseAdminCuenta={responseAdminCuenta}
        saldoCuentas={saldoCuentas}
      />

      <ModalUnificacionCuentas
        open={openModalUnificacionCuentas}
        onClose={handleCloseModalUnificacionCuentas}
      />

      <ModalSolucionInconsistencias
        open={openModalSolucionInconsistencias}
        onClose={handleCloseModalSolucionInconsistencias}
        responseAdminCuenta={responseAdminCuenta}
      />
    </>
  );

}

const mapDispatchToProps = {
  startActualizaBloqueoCuentas,
  startGetAdministracionCuentasAfiliado
};


export default connect(null, mapDispatchToProps)(TablaDetalleCuentaAfiliado);