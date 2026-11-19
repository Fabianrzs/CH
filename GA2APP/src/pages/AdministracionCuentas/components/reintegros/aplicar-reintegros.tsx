import { Button, makeStyles, createStyles } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import { AppState } from '../../../../store/rootReducer';
import CHPageTitle from '../../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../../components/ch-private-container/CHPrivateContainer';
import { CatalogState } from '../../../../reducers/catalog.reducer';
import { LoginState } from '../../../../reducers/login.reducer';
import Bloqueos from './bloqueos';
import Desbloqueos from './desbloqueos';
import DevolucionRecursos from './devolucion-recursos';
import Inconsistentes from './inconsistentes';
import {startGetAllIdentificationTypes} from '../../../../actions/catalog.action';
import { ModalGenerarPago } from './modal-generar-pago';
import NotificationService from '../../../../services/notification.service';
import ApiCatalog from '../../../../services/api-catalog.service';
import ApiReintegro from '../../../../services/api-reintegros.service';
import ApiClient from '../../../../services/api-clients.service';
import { TIPO_REINTEGRO } from './constants/reintegros-constants';
import UtilService from '../../../../utils/util.service';
import { CuentaModelDto } from '../../../../models/requests/reintegros-request-request.model';
import { ModalAgregarReintegro } from './modal-agregar-reintegro';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import InformacionSolicitudReintegro from './informacionSolicitudReintegro';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },   
        columnRight: {
            padding: '10px',
            textAlign: 'right',
        },
        button: {
            marginTop: '1.5rem',
            marginRight: '.5rem',
        },
    }),
);

const AplicarReintegro = (props: Props) => {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiReintegro = ApiReintegro();
    const apiCliente = ApiClient();
    const {loginState,
           catalogState,
           informacionReintegro,
           numeroRadicadoRespuesta,
           formularioInformacionSolicitud,
           unidadesEjecutoras,
           loadingInformacionSolicitud,
           setAplicarReintegro,
           setAgregarNuevoReintegro,
           obtenerListadoReintegros,
           startGetAllIdentificationTypes} = props;
    const [idReintegro,setIdReintegro] = useState('');
    const [listaDevolucionRecursos,setListaDevolucionRecursos] = useState<any []>([]);
    const [listaDevolucionRecursosSeleccionada,setListaDevolucionRecursosSeleccionada] = useState<any []>([]);
    const [listaBloqueos,setListaBloqueos] = useState<any []>([]);
    const [listaDesbloqueos,setListaDesbloqueos] = useState<any []>([]);
    const [listaInconsistentes,setListaInconsitentes] = useState<any []>([]);
    const [tiposCuenta,setTiposCuenta] = useState<any []>([]);
    const [entidadesBancarias,setEntidadesBancarias] = useState<any []>([]);
    const [cuentas,setCuentas] = useState<CuentaModelDto[]>([]);
    const [tipoValor,setTipoValor] = useState('');
    const [deshabilitarBoton,setDeshabilitarBoton] = useState(true);
    const [mostrarCampoObservacion,setMostrarCampoObservacion] = useState(false);
    const [openModalGenerarPago,setOpenModalGenerarPago] = useState(false);
    const [deshabilitarBotonProcesar,setDeshabilitarBotonProcesar] = useState(false);
    const [deshabilitarBotonGenerarRespuesta,setDeshabilitarGenerarRespuesta] = useState(true);
    const [mostrarCamposCheque,setMostrarCamposCheque]= useState(false);
    const [mostrarCamposCuenta,setMostrarCamposCuenta]= useState(false);
    const [loading,setLoading]= useState(false);
    const [open,setOpen]= useState(false);

    const formularioGenerarPago =  useForm({
        defaultValues:{
            nit: '',
            unidadEjecutora: '',
            tipoCuenta: '',
            direccion: '',
            entidad: '',
            numeroCuenta: '',
            valorReintegrar: '',
        }
    })

    const formularioAgregarReintegro = useForm({
        defaultValues: {
            id: '',
            idReintegro: '',
            tipoDocumento: '',
            numeroDocumento: '',
            tipoReintegro: '',
            nombres: '',
            apellidos: '',
            unidadEjecutora: '',
            tipoValor: '',
            valorSolicitado:'',
            estado: '',
            observationAfiliado: '',
        },
    });

    useEffect(() => {
        eliminarRegistrosTablas();
        startGetAllIdentificationTypes(loginState.claims);
        consultarTiposCuenta();
        consultarEntidadesBancarias();
        if(informacionReintegro){
            generarListas(informacionReintegro?.afiliados, false);
            setIdReintegro(informacionReintegro?.id_Reintegro);
        }
    },[informacionReintegro])

    useEffect(() => {
        if(listaDevolucionRecursosSeleccionada.length > 0 &&
           listaDevolucionRecursosSeleccionada.every((item: any) => item.estadoAfiliao === 'EN EVALUACION')){
             setDeshabilitarBoton(false);
         }else{
             setDeshabilitarBoton(true);
         }
       
    },[listaDevolucionRecursosSeleccionada])

    const handleOpen = async(row: any) => {
        setLoading(true);
        setOpen(true);
        if(row){
            formularioAgregarReintegro.setValue('id',row?.id_Afiliado);
            formularioAgregarReintegro.setValue('idReintegro',row?.idReintegro);
            formularioAgregarReintegro.setValue('tipoDocumento',row?.tipoDocumentoId);
            formularioAgregarReintegro.setValue('numeroDocumento',row?.numeroDocumento);
            formularioAgregarReintegro.setValue('tipoReintegro',row?.id_Tipo_Reintegro);
            formularioAgregarReintegro.setValue('nombres',row?.nombres);
            formularioAgregarReintegro.setValue('apellidos',row?.apellidos);
            formularioAgregarReintegro.setValue('unidadEjecutora',informacionReintegro?.unidad_Ejecutora);
            setTipoValor(row?.tipoValor);
            if(UtilService.cleanCurrencyValue(row?.valorSolicitado) === 0 && (row?.tipoValor === '' || row?.tipoValor === 'VALOR')){
                formularioAgregarReintegro.setValue('valorSolicitado','');
                formularioAgregarReintegro.setValue('tipoValor','');
            }

            if(UtilService.cleanCurrencyValue(row?.valorSolicitado) !== 0){
                if(row?.tipoValor === 'VALOR'){
                    formularioAgregarReintegro.setValue('tipoValor',row?.tipoValor);
                    formularioAgregarReintegro.setValue('valorSolicitado',row?.valorSolicitado);
                }
                else if(row?.tipoValor === 'PORCENTAJE'){
                    formularioAgregarReintegro.setValue('valorSolicitado','');
                    formularioAgregarReintegro.setValue('tipoValor',row?.tipoValor);
                }
            }
            formularioAgregarReintegro.setValue('estado',row?.estadoAfiliao);
            
            await apiCliente
            .getCuentasConceptosPorTipoNumeroIdentificacion(
                row?.tipoDocumentoId,
                String(row?.numeroDocumento),
            )
            .then(async response => {
                if (response.data) {
                    const array = response.data?.cuentas;
                    if (response.isSuccess && array) {
                        setCuentas(array);
                    }
                    setLoading(false);    
                }else if(response.data === null){
                    NotificationService.genericAlertError(
                        '',
                        `${response.returnMessage}`,
                        'warning',
                    ); 
                    setLoading(false);    
                }
            })
            .catch(() => {
                setLoading(false);  
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
       } else{
            setLoading(false);  
            toast('No se encuentra información del afiliado', { type: toast.TYPE.ERROR });  
       }
    }

    const handleDeclararInconsistente = () =>  {
        if(mostrarCampoObservacion === true){
            formularioAgregarReintegro.setValue('observationAfiliado','');
            setMostrarCampoObservacion(false);
        }else{
            setMostrarCampoObservacion(true);
        }
    } 

    const consultarCuentaConcepto = async (tipoDocumento: number, documento: string) => {
        await apiCliente
            .getCuentasConceptosPorTipoNumeroIdentificacion(
                tipoDocumento,
                documento,
            )
            .then(async response => {
                if (response) {
                    const array = response.data?.cuentas;
                    if (response.isSuccess && array) {
                        setCuentas(array);
                    }
                }
            })
            .catch((e) => {
                toast(`Error en consultar cuenta concepto ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };
   
    const obtenerReintegrosPorId = async (idReintegro: string) => {
        eliminarRegistrosTablas();
        await apiReintegro
            .getObtenerListadoReintegrosPorId(idReintegro)
            .then(async response => {
                if (response.data) {
                    const fecha = response?.data?.afiliados[0].fechaFin;
                    if(fecha === '0001-01-01T00:00:00'){
                        formularioInformacionSolicitud.setValue('fechaFinal','');
                    }else{
                        formularioInformacionSolicitud.setValue('fechaFinal',moment(fecha).format('YYYY-MM-DD h:mmA'));
                    }
                    generarListas(response.data?.afiliados,true);
                }
            })
            .catch((e) => {
                toast(`Error obtener reintegros por Id ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const consultarTiposCuenta = async() => {
       await apiCatalog.getObtenerValoresCatalogosPorIdCatalogo(67)
            .then(response => {
                if(response.data){
                    setTiposCuenta(response.data);
                }
            }).catch(() => {
                toast(`Error Tipos de Cuenta`, {type: toast.TYPE.ERROR});
            })
    }

    const consultarEntidadesBancarias = async() => {
        await apiCatalog.getAllEntidadBancaria()
             .then(response => {
                 if(response.data){
                     setEntidadesBancarias(response.data);
                 }
             }).catch(() => {
                 toast(`Error Entidades Bancarias`, {type: toast.TYPE.ERROR});
             })
    }

    const handleOpenModalGenerarPago = () => {

        let valorSolicitadoEnCero = false;
        listaDevolucionRecursosSeleccionada.forEach(item => {
            if(item.valorSolicitado === 0){
                valorSolicitadoEnCero = true;
            }
         })

        if(valorSolicitadoEnCero){
            toast('El Valor Solicitado debe ser mayor a cero', { type: toast.TYPE.ERROR });
            return;
        }

        let total = 0;
        listaDevolucionRecursosSeleccionada.forEach(item => {
           if(item.valorSolicitado){
              total += item.valorSolicitado;
           }
        })

        let nitUnidadEjecutora;
        if(unidadesEjecutoras){
            nitUnidadEjecutora = unidadesEjecutoras?.find(item => item.id === informacionReintegro?.id_Unidad_Ejecutora)
        }

        formularioGenerarPago.setValue('nit', String(nitUnidadEjecutora?.identificacion));
        formularioGenerarPago.setValue('unidadEjecutora', informacionReintegro?.unidad_Ejecutora);
        formularioGenerarPago.setValue('valorReintegrar',UtilService.formatCurrency(total));
        formularioGenerarPago.setValue('tipoCuenta','');
        formularioGenerarPago.setValue('entidad','');
        formularioGenerarPago.setValue('numeroCuenta','');
        setOpenModalGenerarPago(true);
    };

    const handleCloseModalGenerarPago = () => {
        setOpenModalGenerarPago(false);
        setMostrarCamposCheque(false);
        setMostrarCamposCuenta(false);
    }

    const generarListas = (afiliados: any[], modoActualizar: boolean) => {
        let afiliadosEstadoProcesado = 0;
        let afiliadosBloqueos = 0;
        let afiliadosDesbloqueos = 0;
        let afiliadosDevolucionRecursos = 0;
        let afiliadosInconsistentes = 0;
        for(let i=0; i<afiliados.length;i +=1 ){
            if(afiliados[i].id_Tipo_Reintegro === TIPO_REINTEGRO.DEVOLUCION_DE_RECURSOS){
                setListaDevolucionRecursos(oldArray => [
                    ...oldArray,
                    afiliados[i],
                ]);
                if(afiliados[i].estadoAfiliao === 'PROCESADO'){
                    afiliadosEstadoProcesado += 1;
                }
                afiliadosDevolucionRecursos += 1;
            }else if(afiliados[i].id_Tipo_Reintegro === TIPO_REINTEGRO.BLOQUEOS){
                setListaBloqueos(oldArray => [
                    ...oldArray,
                    afiliados[i],
                ]);
                if(afiliados[i].estadoAfiliao === 'PROCESADO'){
                    afiliadosEstadoProcesado += 1;
                }
                afiliadosBloqueos += 1;
            }else if(afiliados[i].id_Tipo_Reintegro === TIPO_REINTEGRO.DESBLOQUEOS){
                setListaDesbloqueos(oldArray => [
                    ...oldArray,
                    afiliados[i],
                ]);
                if(afiliados[i].estadoAfiliao === 'PROCESADO'){
                    afiliadosEstadoProcesado += 1;
                }
                afiliadosDesbloqueos += 1;
            }
            else if(afiliados[i].id_Tipo_Reintegro === TIPO_REINTEGRO.INCONSISTENTES){
                afiliadosInconsistentes += 1;
                if(modoActualizar){
                    if(afiliados[i].observationAfiliado){
                        setListaInconsitentes(oldArray => [
                            ...oldArray,
                            afiliados[i],
                        ]);
                    }else{
                        const nuevoItemInconsistente = {
                            ...afiliados[i],
                            observationAfiliado: informacionReintegro?.observacion
                        }
                        setListaInconsitentes(oldArray => [
                            ...oldArray,
                            nuevoItemInconsistente,
                        ]);
                    }
                    if(afiliados[i].estadoAfiliao === 'PROCESADO'){
                        afiliadosEstadoProcesado += 1;
                    }
                   
                }else{
                    const nuevoItemInconsistente = {
                        ...afiliados[i],
                        observationAfiliado: informacionReintegro?.observacion || afiliados[i].observationAfiliado
                    }
                    setListaInconsitentes(oldArray => [
                        ...oldArray,
                        nuevoItemInconsistente,
                    ]);
                    if(afiliados[i].estadoAfiliao === 'PROCESADO'){
                        afiliadosEstadoProcesado += 1;
                    }
                }
            }
        }

        // Validación Deshabilitar boton PROCESAR
        if(afiliadosBloqueos === 0 && 
           afiliadosDesbloqueos === 0 && 
           afiliadosDevolucionRecursos === 0 &&
           afiliadosInconsistentes > 0){
            setDeshabilitarGenerarRespuesta(false);
            setDeshabilitarBotonProcesar(true);
        }
        else if(afiliadosEstadoProcesado === afiliados.length){
            setDeshabilitarBotonProcesar(true);
             // Validación habilitar boton GENERAR RESPUESTA
            setDeshabilitarGenerarRespuesta(false);
        }     
        else{
            setDeshabilitarBotonProcesar(false);
        }
    }

    const procesar = async() => {

        if(listaDevolucionRecursos.every(item => item.valorSolicitado === 0) === true &&
           listaBloqueos.every(item => item.valorSolicitado===  0) === true &&
           listaDesbloqueos.every(item => item.valorSolicitado === 0) === true){
           toast(`El valor solicitado es cero. No se puede cambiar a estado Procesado`, {type: toast.TYPE.ERROR});
           return;
        }

        swal({
            text: "¿Está seguro que desea realizar esta acción?",
            icon: "warning",
            buttons: ["Sí", "No"]
        }).then(async respuesta => {
            if (!respuesta) {

        const arrayAfiliados: any[] = [];

        listaDevolucionRecursos.forEach(item => {
            if(item.valorSolicitado > 0)
            {
                const afiliadoDevolucionRecursos = {
                    idAfiliadoReintegro: item.id_Afiliado,
                    numeroDocumento: item.numeroDocumento,
                    nombres: item.nombres,
                    tipoDocumentoId: item.tipoDocumentoId,
                    apellidos: item.apellidos,
                    idReintegro: idReintegro,
                    id_Tipo_Reintegro: item.id_Tipo_Reintegro,
                    valorSolicitado: item.valorSolicitado,
                    estadoAfiliao: item.estadoAfiliao,
                }
                arrayAfiliados.push(afiliadoDevolucionRecursos);
           }
        })

        listaBloqueos.forEach(item => {
            if(item.valorSolicitado > 0)
            {
                const afiliadoBloqueo = {
                    idAfiliadoReintegro: item.id_Afiliado,
                    numeroDocumento: item.numeroDocumento,
                    nombres: item.nombres,
                    tipoDocumentoId: item.tipoDocumentoId,
                    apellidos: item.apellidos,
                    idReintegro: idReintegro,
                    id_Tipo_Reintegro: item.id_Tipo_Reintegro,
                    valorSolicitado: item.valorSolicitado,
                    estadoAfiliao: 'PROCESADO',
                }
                arrayAfiliados.push(afiliadoBloqueo);
            }
        })

        listaDesbloqueos.forEach(item => {
            if(item.valorSolicitado > 0)
            {
                const afiliadoDesbloqueo = {
                    idAfiliadoReintegro: item.id_Afiliado,
                    numeroDocumento: item.numeroDocumento,
                    nombres: item.nombres,
                    tipoDocumentoId: item.tipoDocumentoId,
                    apellidos: item.apellidos,
                    idReintegro: idReintegro,
                    id_Tipo_Reintegro: item.id_Tipo_Reintegro,
                    valorSolicitado: item.valorSolicitado,
                    estadoAfiliao: 'PROCESADO',
                }
                arrayAfiliados.push(afiliadoDesbloqueo);
            }
        })

        listaInconsistentes.forEach(item => {
            const afiliadoInconsistente = {
                idAfiliadoReintegro: item.id_Afiliado,
                numeroDocumento: item.numeroDocumento,
                nombres: item.nombres,
                tipoDocumentoId: item.tipoDocumentoId,
                apellidos: item.apellidos,
                idReintegro: idReintegro,
                id_Tipo_Reintegro: item.id_Tipo_Reintegro,
                valorSolicitado: item.valorSolicitado,
                estadoAfiliao: item.estadoAfiliao,
            }
            arrayAfiliados.push(afiliadoInconsistente);
        })

        await apiReintegro.postGuardarAfiliadosReintegro(arrayAfiliados)
        .catch(() => {
            toast(`Error al guardar en procesar`, {type: toast.TYPE.ERROR});
        })

        arrayAfiliados.forEach((afiliado: any) => {
            if(afiliado.id_Tipo_Reintegro === TIPO_REINTEGRO.BLOQUEOS ||
               afiliado.id_Tipo_Reintegro === TIPO_REINTEGRO.DESBLOQUEOS){

                apiCliente
                .getCuentasConceptosPorTipoNumeroIdentificacion(
                    Number(afiliado.tipoDocumentoId),
                    String(afiliado.numeroDocumento),
                )
                .then(async response => {
                    if (response) {
                        const array = response.data?.cuentas;
                        const arrayConceptos: any[] =[];
                        if (response.isSuccess && array) {
                            array.forEach((cuenta:any) => {
                                if(cuenta?.saldo !== 0 && cuenta?.conceptos.length !== 0 && cuenta?.estadoCuenta !== 'CANCELADA'){
                                    cuenta.conceptos.forEach((concepto: any) => {
                                        const item = {
                                            conceptoId: concepto.conceptoId,
                                            cuentaId: cuenta.cuentaId,
                                            valorBloque: concepto.valorBloqueoEmbargo,
                                            valorSaldo: concepto.valorNewSaldo,
                                            idTipoReintegro: afiliado.id_Tipo_Reintegro
                                        }
                                        arrayConceptos.push(item);
                                    })
                                }
                            })
                        }
                        await apiReintegro.postCrearMovimientoCuenta(arrayConceptos)
                        .catch(() => {
                            toast(`Error al crear movimientos cuenta`, {type: toast.TYPE.ERROR});
                        })
                    }
                })
                .catch(() => {
                    toast('Error cuentas conceptos procesar', { type: toast.TYPE.ERROR });
                });

            }
        })
        obtenerReintegrosPorId(idReintegro);
           }
        })
    }

    const guardarInformacionGenerarPagoReintegros = async (request: any) => {
        await apiReintegro
            .postGenerarPagoReintegro(request)
            .then()
            .catch((e) => {
                toast(`Error al guardar información generar pagos ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    const ejecutarPago = async(data: any) => {
        handleCloseModalGenerarPago();
        setLoading(true);
        let cantidadAfiliados = 0;
        let numeroPagosExistosos = 0;
        let noHayDisponibilidadRecursos = 0;
        let pagosRechazados = 0;
        let numeroCDP: any;
        let numeroFactura: any;
        const arrayAfiliados: any[] = [];

        listaDevolucionRecursosSeleccionada.forEach((item: any, index: any) => {
            apiCliente.getObtenerInformacionCliente(item.numeroDocumento)
            .then(response => {
                if(response){
                    if(response.data){
                        const request = {
                            idReintegros: idReintegro,
                            idCliente: response.data.clI_ID,
                        }
                        apiReintegro.postPagosReintegrosSeven(request)
                        .then(response => {
                            if(response.data){
                                numeroCDP = JSON.parse(response.data.crearCDPResponse);
                                numeroFactura = JSON.parse(response.data.insertarFacturaPResponse);

                                if(numeroCDP?.NewDataSet?.PG_MPRES?.RETORNO !== "0"){
                                    noHayDisponibilidadRecursos +=1;
                                }else if(numeroFactura?.NewDataSet?.PO_FACTU?.RETORNO !== "0"){
                                    pagosRechazados += 1;
                                }else{
                                    numeroPagosExistosos += 1;
                                }

                                cantidadAfiliados += 1;
                                if(cantidadAfiliados === listaDevolucionRecursosSeleccionada.length){
                                    if(noHayDisponibilidadRecursos > 0){
                                        setLoading(false);
                                        NotificationService.genericAlertError(
                                            'CONSULTA CDP',
                                            'No hay disponibilidad de recursos',
                                            'warning',
                                        ); 
                                    }
                            
                                    if(pagosRechazados > 0){
                                        setLoading(false);
                                        NotificationService.genericAlertError(
                                            'FACTURA PAGO',
                                            `Rechazado intente nuevamente`,
                                            'warning',
                                        ); 
                                    }
                                }

                                if(numeroPagosExistosos === listaDevolucionRecursosSeleccionada.length &&
                                    noHayDisponibilidadRecursos === 0 &&
                                    pagosRechazados === 0){
                                     listaDevolucionRecursosSeleccionada.forEach(item => {
                                         const afiliadoDevolucionRecursos = {
                                             id_Afiliado: item.id_Afiliado,
                                             numeroDocumento: item.numeroDocumento,
                                             nombres: item.nombres,
                                             tipoDocumentoId: item.tipoDocumentoId,
                                             apellidos: item.apellidos,
                                             idReintegro: idReintegro,
                                             id_Tipo_Reintegro: item.id_Tipo_Reintegro,
                                             valorSolicitado: item.valorSolicitado,
                                             estadoAfiliao: 'PROCESADO',
                                             observationAfiliado: item.observationAfiliado,
                                             tipoValor: item.tipoValor,
                                         }
                                         arrayAfiliados.push(afiliadoDevolucionRecursos);
                                     })
                             
                                     listaBloqueos.forEach(item => {
                                         const afiliadoBloqueo = {
                                             id_Afiliado: item.id_Afiliado,
                                             numeroDocumento: item.numeroDocumento,
                                             nombres: item.nombres,
                                             tipoDocumentoId: item.tipoDocumentoId,
                                             apellidos: item.apellidos,
                                             idReintegro: idReintegro,
                                             id_Tipo_Reintegro: item.id_Tipo_Reintegro,
                                             valorSolicitado: item.valorSolicitado,
                                             estadoAfiliao: item.estadoAfiliao,
                                             observationAfiliado: item.observationAfiliado,
                                             tipoValor: item.tipoValor,
                                         }
                                         arrayAfiliados.push(afiliadoBloqueo);
                                     })
                             
                                     listaDesbloqueos.forEach(item => {
                                         const afiliadoDesbloqueo = {
                                             id_Afiliado: item.id_Afiliado,
                                             numeroDocumento: item.numeroDocumento,
                                             nombres: item.nombres,
                                             tipoDocumentoId: item.tipoDocumentoId,
                                             apellidos: item.apellidos,
                                             idReintegro: idReintegro,
                                             id_Tipo_Reintegro: item.id_Tipo_Reintegro,
                                             valorSolicitado: item.valorSolicitado,
                                             estadoAfiliao: item.estadoAfiliao,
                                             observationAfiliado: item.observationAfiliado,
                                             tipoValor: item.tipoValor,
                                         }
                                         arrayAfiliados.push(afiliadoDesbloqueo);
                                     })
                             
                                     listaInconsistentes.forEach(item => {
                                         const afiliadoBloqueo = {
                                             id_Afiliado: item.id_Afiliado,
                                             numeroDocumento: item.numeroDocumento,
                                             nombres: item.nombres,
                                             tipoDocumentoId: item.tipoDocumentoId,
                                             apellidos: item.apellidos,
                                             idReintegro: idReintegro,
                                             id_Tipo_Reintegro: item.id_Tipo_Reintegro,
                                             valorSolicitado: item.valorSolicitado,
                                             estadoAfiliao: item.estadoAfiliao,
                                             observationAfiliado: item.observationAfiliado,
                                             tipoValor: item.tipoValor,
                                         }
                                         arrayAfiliados.push(afiliadoBloqueo);
                                     })
                             
                                     apiReintegro.postActualizarListaAfiliados(arrayAfiliados)
                                     .then().catch(() => {
                                         setLoading(false);
                                         toast(`Error al actualizar afiliados en generar pago`, {type: toast.TYPE.ERROR});
                                     })
                                     arrayAfiliados.forEach((afiliado: any) => {
                                         if(afiliado.id_Tipo_Reintegro === TIPO_REINTEGRO.DEVOLUCION_DE_RECURSOS){
                                             apiCliente
                                             .getCuentasConceptosPorTipoNumeroIdentificacion(
                                                 Number(afiliado.tipoDocumentoId),
                                                 String(afiliado.numeroDocumento),
                                             )
                                             .then(async response => {
                                                 if (response) {
                                                     const array = response.data?.cuentas;
                                                     const arrayConceptos: any[] =[];
                                                     const arrayConceptosProcesar: any[] = [];
                                                     if (response.isSuccess && array) {
                                                         array.forEach((cuenta:any) => {
                                                             if(cuenta?.saldo !== 0 && cuenta?.conceptos.length !== 0 && cuenta?.estadoCuenta !== 'CANCELADA'){
                                                                 cuenta.conceptos.forEach((concepto: any) => {
                                                                     const item = {
                                                                         conceptoId: concepto.conceptoId,
                                                                         cuentaId: cuenta.cuentaId,
                                                                         valorBloque: concepto.valorBloqueoEmbargo,
                                                                         valorSaldo: concepto.valorNewSaldo,
                                                                         idTipoReintegro: afiliado.id_Tipo_Reintegro
                                                                     }
                                                                     arrayConceptos.push(item);
                                                                     // Conceptos para procesar en hoja de vida
                                                                     if(concepto.valorNewSaldo > 0){
                                                                        const conceptoProcesar = {
                                                                            cuentaId: concepto.cuentaId,
                                                                            conceptoId: concepto.conceptoId,
                                                                            saldo: concepto.valorNewSaldo,
                                                                            valorDevolver: concepto.valorDevolver
                                                                        }
                                                                        arrayConceptosProcesar.push(conceptoProcesar);
                                                                     }
                                                                 })
                                                             }
                                                         })
                                                     }
                                                     await apiReintegro.postCrearMovimientoCuenta(arrayConceptos)
                                                     .catch(() => {
                                                         setLoading(false);
                                                         toast(`Error al crear movimientos cuenta en generar pago`, {type: toast.TYPE.ERROR});
                                                     })
                                                     // Generar fecha final
                                                     await apiReintegro.getUpdateFechaFinal(idReintegro)
                                                     .catch(() => {
                                                         setLoading(false);
                                                         toast(`Error al actualizar fecha final`, {type: toast.TYPE.ERROR});
                                                     })
                                                    // Conceptos para procesar en hoja de vida
                                                     await apiReintegro.postProcesarSaldoConceptoCuenta(arrayConceptosProcesar)
                                                     .catch(() => {
                                                         setLoading(false);
                                                         toast(`Error al procesar saldo concepto cuenta`, {type: toast.TYPE.ERROR});
                                                     })
                                                     obtenerReintegrosPorId(idReintegro);
                                                     const request = {
                                                         idReintegro: idReintegro,
                                                         codigoBanco: data.entidad,
                                                         tipoCuenta: Number(data.tipoCuenta),
                                                         numeroCuenta: Number(data.numeroCuenta),
                                                         direccion: data.direccion
                                                     }
                                                     guardarInformacionGenerarPagoReintegros(request);
                                                     setListaDevolucionRecursosSeleccionada([]);
                                                     setDeshabilitarBoton(true);
                                                     setLoading(false);
                                                     NotificationService.genericAlert(
                                                        `Planilla generada con éxito`,
                                                        `Se generó la planilla de pago con éxito \n CDP Generado: ${numeroCDP?.NewDataSet?.PG_MPRES?.MPR_NUME} \n Factura Generada: ${numeroFactura?.NewDataSet?.PO_FACTU?.FAC_NUME}`,
                                                     );
                                                     // Generar PDF planilla de pago
                                                     try {
                                                        const datosUsuario =
                                                        {
                                                            identificacion:  String(afiliado.numeroDocumento),
                                                            tipoIdentificacion: Number(afiliado.tipoDocumentoId),
                                                            cdp: String(numeroCDP?.NewDataSet?.PG_MPRES?.MPR_NUME)
                                                        }
                                                        const response = await apiReintegro.postPlanillaPago(datosUsuario);
                                            
                                                        const mimeType = response.type;
                                                        const file = new Blob([response], { type: mimeType });
                                                        const fileURL = URL.createObjectURL(file);
                                                        window.open(fileURL, '_blank');
                                                    } catch (e) {
                                            
                                                        await NotificationService.genericAlert(
                                                            'Error',
                                                            'Error al obtener el archivo. Intente nuevamente.',
                                                            'error',
                                                        );
                                                    }
                                                 }
                                             })
                                             .catch(() => {
                                                 setLoading(false);
                                                 toast('Error cuentas conceptos procesar', { type: toast.TYPE.ERROR });
                                             });
                                         }
                                     })
                                 }
                            }
                        }).catch((e) => {
                            setLoading(false);
                            toast(`Error pago reintegros seven  ${e.message}`, {type: toast.TYPE.ERROR});
                        })
                    }
                }
            }).catch((e) => {
                setLoading(false);
                toast(`Error consultar información cliente en generar pago ${e.message}`, {type: toast.TYPE.ERROR});
            })

          }
        )
    }

    const onClickCancelar = () => {
        setAgregarNuevoReintegro(false);
        setAplicarReintegro(false);
        eliminarRegistrosTablas();
        const busqueda = {
            numeroRadicado: '',
            unidadEjecutora: 0,
            tipoReintegro: 0,
        }
        obtenerListadoReintegros(busqueda);
    }

    const eliminarRegistrosTablas = () => {
        setListaDevolucionRecursos([]);
        setListaBloqueos([]);
        setListaDesbloqueos([]);
        setListaInconsitentes([]);
    }

    const generarRespuesta = async() => {
        if(informacionReintegro?.numero_Radicado){
            setLoading(true);
            apiReintegro
                .getGenerarOficio(informacionReintegro?.numero_Radicado)
                .then(async response => {
                    if (response?.type === "application/pdf") {
                        const url = URL.createObjectURL(response);
                        window.open(url);
                    }
                })
                .catch(async (e) => {
                    if(e.response) {
                        const result = JSON.parse(await e.response.data.text());
                        toast(result.returnMessage, { type: toast.TYPE.ERROR });
                    } else {
                        toast('No se pudo generar el reporte oficio Reintegro', { type: toast.TYPE.ERROR });
                    }
                }).finally(() => setLoading(false));
        }
    }

    return (
        <>
            <LoadingComponent loading={loadingInformacionSolicitud} />
            <LoadingComponent loading={loading} />
            <CHPrivateContainer>
                <CHPageTitle
                    topText="Reintegros"
                    subText=""
                    title1="Reintegros"
                    title2="Unidades Ejecutoras"
                />
                <InformacionSolicitudReintegro 
                    informacionReintegro={informacionReintegro}
                    numeroRadicadoRespuesta={numeroRadicadoRespuesta}
                    formularioInformacionSolicitud={formularioInformacionSolicitud}
                />
                <CHPaper title="Devolución de Recursos">
                <DevolucionRecursos
                    listaDevolucionRecursos={listaDevolucionRecursos}
                    listaDevolucionRecursosSeleccionada={listaDevolucionRecursosSeleccionada}
                    handleOpen={handleOpen}
                    setListaDevolucionRecursosSeleccionada={setListaDevolucionRecursosSeleccionada} 
                />
                </CHPaper>
                <CHPaper title="Bloqueos">
                <Bloqueos
                    listaBloqueos={listaBloqueos}
                    handleOpen={handleOpen}
                />
                </CHPaper>
                <CHPaper title="Desbloqueos">
                <Desbloqueos
                    listaDesbloqueos={listaDesbloqueos}
                    handleOpen={handleOpen}
                />
                </CHPaper>
                <CHPaper title="Inconsistentes">
                <Inconsistentes
                    listaInconsistentes={listaInconsistentes}                
                />
                </CHPaper>
                <div className="buttons">
                    <Button
                        className={classes.button}
                        variant="outlined"
                        color="primary"
                        disabled={deshabilitarBotonGenerarRespuesta}
                        onClick={generarRespuesta}
                
                    >
                        GENERAR RESPUESTA
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        disabled={deshabilitarBoton}
                        onClick={handleOpenModalGenerarPago}
                    >
                        GENERAR PAGO
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        disabled={deshabilitarBotonProcesar || 
                                  (listaDevolucionRecursos.length > 0 
                                    && listaBloqueos.length === 0 
                                    && listaDesbloqueos.length === 0)}
                        onClick={procesar}
                    >
                        PROCESAR
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={onClickCancelar}
                    >
                        REGRESAR
                    </Button>
                </div>
                <ModalAgregarReintegro 
                    open={open}
                    cuentas={cuentas}
                    loadingAplicarReintegros={loading}
                    tipoValor={tipoValor}
                    idReintegro={idReintegro}
                    catalogState={catalogState}
                    formulario={formularioAgregarReintegro}
                    mostrarCampoObservacion={mostrarCampoObservacion}
                    setMostrarCampoObservacion={setMostrarCampoObservacion}
                    setOpen={setOpen}
                    setTipoValor={setTipoValor}
                    obtenerReintegrosPorId={obtenerReintegrosPorId}
                    eliminarRegistrosTablas={eliminarRegistrosTablas}
                    consultarCuentaConcepto={consultarCuentaConcepto}
                    handleDeclararInconsistente={handleDeclararInconsistente}
                    setListaDevolucionRecursosSeleccionada={setListaDevolucionRecursosSeleccionada} 
                />
                <ModalGenerarPago
                    open={openModalGenerarPago}
                    tiposCuenta={tiposCuenta}
                    formulario={formularioGenerarPago}
                    entidadesBancarias={entidadesBancarias}
                    ejecutarPago={ejecutarPago}
                    onClose={handleCloseModalGenerarPago}
                    mostrarCamposCheque={mostrarCamposCheque}
                    setMostrarCamposCheque={setMostrarCamposCheque}
                    mostrarCamposCuenta={mostrarCamposCuenta}
                    setMostrarCamposCuenta={setMostrarCamposCuenta}
                />
            </CHPrivateContainer> 
        </>
    );
}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    informacionReintegro: any;
    unidadesEjecutoras: any[];
    numeroRadicadoRespuesta: any;
    formularioInformacionSolicitud: any;
    loadingInformacionSolicitud: boolean;
    setAplicarReintegro: (value: boolean) => void;
    setAgregarNuevoReintegro: (value: boolean) => void;
    obtenerListadoReintegros: (busqueda: any) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(AplicarReintegro);