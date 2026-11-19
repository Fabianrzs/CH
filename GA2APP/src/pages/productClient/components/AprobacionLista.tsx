import React, { useEffect, useState } from 'react';
import ReactDOM from 'react-dom';
import { Button, CircularProgress, createStyles, Grid, makeStyles } from '@material-ui/core';
import swal from 'sweetalert';
import moment from 'moment';
import { toast } from 'react-toastify';
import { Alert, AlertTitle } from '@material-ui/lab';
import { addMonths, endOfMonth } from 'date-fns';
import NotificationService from '../../../services/notification.service';
import TableMassivePayAplicationList from './TableMassivePayAplicationList';
import { ModalAplicacionMasivaPagosList } from './modalMassivePayAplication/ModalAplicacionMasivaPagosList';
import { procesoListarAplicacionMasivaPago } from '../../../types/producto.model.type';
import { LoadingComponent } from '../../../components/loading/loading.component';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import apiProducto from '../../../services/api-producto.service';
import CatalogApi from '../../../services/api-catalog.service';
import CreditApi from '../../../services/api-credit.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import CarteraApi from '../../../services/api-cartera-service';


enum EstadoAplicacionPago {
    PENDIENTE = "PENDIENTE",
    APROBADA = "APROBADA",
    RECHAZADA = "RECHAZADA",
    ANULADA = "ANULADA"
}

interface accionLista {
    accion: 'INSERTAR' | 'ACTUALIZAR',
    data?: any;
}

const defaultSwalOptionsButtons = { 
    confirm: { text: "Sí", value: true, visible: true },
    cancel: { text: "No", value: false, visible: true }
};

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%'
        },
        groupButtonLoader: {
            display: 'flex',
            gap: '1.5rem',
            alignItems: 'center',
            padding: '1rem 0',
            flexWrap: 'wrap'
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr 3fr',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        }
    }),
);



const MensajeListaExistente = (props: any) =>
{
    const element = document.createElement('div');
    ReactDOM.render(
        <>
            <div>
                Ya existe una lista en estado <strong>{props.proceso.estado}</strong> para el periodo <strong>{props.proceso.periodo} </strong>
                que contiene los siguientes documentos: 
            </div>
            <div style={{maxHeight: "300px", overflowY: "auto", marginTop: "1.5rem"}}>
                <ul>
                    {
                        props.listado.map((x: any) => 
                            <li key={x.nroDocumento} style={{textAlign: "left"}}>{x.nroDocumento}</li>
                        )
                    }
                </ul>
            </div>
            
        </>
    , element);

    return {
        content: {
            element
        },
        buttons: { ok: { text: 'Ok', value: false, visible: true }},
        icon: 'warning'
    };
}

const InfoPanel = ({data, classes}: any) => (data
        ? <Grid container>
            <Grid style={{'margin': '1rem 0'}} item xs={12}>
                <Alert severity="info">
                    <AlertTitle><strong>Información de Lista</strong></AlertTitle>
                    <div className={classes}>
                        <strong>Fecha Aplicación:</strong>
                        <span>{data.fechaAplicacion}</span>
                        <strong>Fecha Pago:</strong>
                        <span>{data.fechaProceso}</span>
                        <strong>Unidad Ejecutora:</strong>
                        <span>{data.unidadEjecutora}</span>
                        <strong>Tipo Crédito: </strong>
                        <span>{data.tipoCredito}</span>
                    </div>
                </Alert>
            </Grid>          
        </Grid>
        : <></>);

const AprobacionListas = (props: any) => {

    const classes = useStyles();
    const api = apiProducto();
    const carteraApi = CarteraApi();

    const catalogApi = CatalogApi();
    const creditApi = CreditApi();
    const [lista, setLista] = useState<any[]>([]);
    const [openModalList, setOpenModalList] = useState(false);
    const [selectedRows, setSelectedRows] = useState<any[]|undefined>([]);
    const [tiposCredito, setTiposCredito] = useState<any[]>([]);
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    const [formState, setFormState] = useState<procesoListarAplicacionMasivaPago|null>(null);
    const [procesoAprobacion, setProcesoAprobacion] = useState<'INICIAL'|'PROCESANDO'|'COMPLETADO'>('INICIAL');
    const [fechaUltimoCierreMensual, setFechaUltimoCierreMensual] = useState<any>();


    useEffect(
        () => {
            cargarUnidadesEjecutoras();
            cargarTiposCredito();
        }, []
    );

    useEffect(() => {  
        carteraApi
        .getUltimosCierres()
        .then(async (response: any) => {
            if (response) {
                if (response.data != null && response.isSuccess) {
                    const fechaUltimoCierre:any = response.data.filter((value: any) => value.idTipoCierre === 2);
    
                    if (fechaUltimoCierre.length > 0) {
                        // Suponiendo que la fecha sea un string en formato ISO y necesitas convertirla a un objeto Date
                        const fechaOriginal = new Date(fechaUltimoCierre[0].fechaCierre); // Asegúrate de que 'fecha' sea el campo correcto
                        const nuevaFecha = endOfMonth(addMonths(fechaOriginal, 1)); // Suma un mes y toma el último día del mes
                        setFechaUltimoCierreMensual(nuevaFecha);

    
                    } else {
                        toast(`No se encontraron cierres de cartera`, { type: toast.TYPE.WARNING });
                    }
                }
            }
        })
    }, []);

    const cargarUnidadesEjecutoras = () => {
        catalogApi.getAllEXECUTINGUNITSRequest()
        .then(x => {
            if(x.isSuccess) {
                setUnidadesEjecutoras(x.data)
            }
        })
        .catch(_ => setUnidadesEjecutoras([]));
    }

    const cargarTiposCredito = () => {
        catalogApi.getCreditType()
        .then(x => {
            if(x.isSuccess) {
                setTiposCredito(x.data.map((x: any) => ({idTipoCredito: x.tcR_ID, descripcion: x.tcR_DESCRIPCION})))
            }
        })
        .catch(_ => setTiposCredito([]));
    }

    const validaProcesoExistente = (procesos: any[]) =>
        procesos.map((procesoLista: any) => {
            if(procesoLista.estado === EstadoAplicacionPago.APROBADA || procesoLista.estado === EstadoAplicacionPago.PENDIENTE) {
                const cruceLista = procesoLista.pagos?.filter((jp: any) => lista.find(ls => ls.nroDocumento === jp.nroDocumento)) ?? [];
                if(cruceLista.length) {
                    return {
                        proceso: procesoLista,
                        listado: cruceLista
                    };
                }
            }
            return null
        }).find((proceso: any) => proceso);

        const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    
        const isValidIPv4 = (ip: string) => {
            const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            return ipv4Regex.test(ip);
        };
    
        const isValidIPv6 = (ip: string) => {
            const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
            return ipv6Regex.test(ip);
        };
    
        const enviarIp = async (ip:any) => {
            const  convertIp = ip.split('.')[0]
           const  hexadecimal = convertIp
            await catalogApi 
                .getObtenerIpLocal(hexadecimal)
                .then(async (response: any) => {
        
                    if (response) {
                        if (response.isSuccess) {
                            setPublicIpAddress(response.data);
                        }
                    }
                })
                .catch(e => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        
        }
    
        useEffect(() => {
            const fetchLocalIPAddress = () => {
                const peerConnection = new RTCPeerConnection({ iceServers: [] });
                peerConnection.createDataChannel('');
                peerConnection.createOffer()
                    .then((offer) => peerConnection.setLocalDescription(offer))
                    .catch((error) => console.error('Error creating offer:', error));
    
                peerConnection.onicecandidate = (event) => {
                    if (event.candidate) {
                        const ip: any = event.candidate.address;
                        if (isValidIPv4(ip) || isValidIPv6(ip)) {
                            enviarIp(ip);
                        } else {
                            enviarIp(ip);
                        }
                    }
                };
    
                return () => {
                    peerConnection.close();
                };
            };
            fetchLocalIPAddress();
    
        }, []);

    const validarLista = async () : Promise<accionLista | null> => {
        const auditoriaApi=AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
                
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIpAddress,
            modulo:'Generar listas',
            hostname:document.location.host
            } as Auditoria
        const periodo = formState?.fechaAplicacion ? moment(formState.fechaAplicacion).format('YYYYMM') : undefined;
        const response = await creditApi.obtenerProcesoAplicacionPagosPorUnidadEjecutora(
            formState?.unidadEjecutora, formState?.tipoCredito, periodo
        );
        
        if(response.isSuccess) {
            requestAuditoria.tipoAccion='CONSULTAS';
            requestAuditoria.idTipoAccion=3;
            requestAuditoria.descripcion=`obtener Proceso Aplicacion Pagos Por Unidad Ejecutora parámetros:
            \n Unidad ejecutora:  ${ unidadesEjecutoras.find(x=>x.id===formState?.unidadEjecutora).sigla }
            \n Tipo de crédito:  ${ tiposCredito.find(x=>x.idTipoCredito===formState?.tipoCredito).descripcion}
            \n Periodo:  ${periodo}`
            await auditoriaApi.InsertAuditoria(requestAuditoria);
            const procesoExistente = validaProcesoExistente(response.data);

            if(procesoExistente) {
                const result = await swal(MensajeListaExistente(procesoExistente));
                requestAuditoria.tipoAccion='VALIDACION';
                    requestAuditoria.idTipoAccion=9;
                    requestAuditoria.descripcion=`Ya existe una lista en estado ${props.proceso.estado} para el periodo ${props.proceso.periodo}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
            } else {
                const result = await swal({ text: '¿Está seguro que desea aprobar la lista?', buttons: defaultSwalOptionsButtons, icon: 'warning' });
                return result ? { accion: 'INSERTAR' } : null; 
            }

            return null;
        }

        throw response;
    };

    const insertarLista = () => creditApi.insertarProcesoAplicacionPago({
        Fecha_Pago_Cuota: moment(formState?.fechaAplicacion).format(),
        Fecha_Pago: moment(formState?.fechaProceso).format(),
        periodo: moment(formState?.fechaAplicacion).format("YYYYMM"),
        unidadEjecutoraId: formState?.unidadEjecutora,
        tipoCreditoId: formState?.tipoCredito,
        estado: EstadoAplicacionPago.PENDIENTE,
        pagos: lista
    });

    const handleOpenModalList = () => {
        setOpenModalList(true);
    };

    const handleCloseModalList = () => {
        setOpenModalList(false);
    };


    const handleRowsDelete = (row: any, data: any[]): false => {
        swal({
            text: '¿Está seguro de eliminar los registros seleccionados?',
            icon: 'warning',
            buttons: { 
                confirm: {
                    text: "Sí",
                    value: true,
                    visible: true,
                },
                cancel: {
                    text: "No",
                    value: false,
                    visible: true,
                }
             },
        }).then((result) => {
            if(result) {
                const auditoriaApi=AuditoriaApi();
                const user = AuthService.getUserInfo();
                const nav =(window.navigator as any);
                const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
                        
                const requestAuditoria={
                    navegador:usrAg,
                    sistemaOperativo:nav.userAgentData.platform,
                    nombreUsuario:user.USR_USERNAME, 
                    usurioId:user.USR_ID,
                    ip:publicIpAddress,
                    modulo:'Generar Listas',
                    hostname:document.location.host
                    } as Auditoria
                const indices = row.data.map((x: any) => x.dataIndex);
                const data = lista.filter((__, index, _) => !indices.includes(index));
                setLista(data);
                setSelectedRows([]);
                requestAuditoria.tipoAccion='TRANSACCIONES';
                requestAuditoria.idTipoAccion=2;
                requestAuditoria.descripcion=`Eliminar registros de tabla de aplicación masiva de pagos en pestaña Aprobación de Lista`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        });
        return false;
    };

    const handleSelectedRow = (__: any, _: any, rowsSelected: any) => {
        setSelectedRows(rowsSelected);
    }

    const handleAprobarLista = async () => {
        
        setProcesoAprobacion('PROCESANDO');
        const auditoriaApi=AuditoriaApi();
                    const user = AuthService.getUserInfo();
                    const nav =(window.navigator as any);
                    const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
                            
                    const requestAuditoria={
                        navegador:usrAg,
                        sistemaOperativo:nav.userAgentData.platform,
                        nombreUsuario:user.USR_USERNAME, 
                        usurioId:user.USR_ID,
                        ip: publicIpAddress,
                        modulo:'Generar Lista',
                        hostname:document.location.host
                        } as Auditoria
        try {
            const result = await validarLista();            
            if(result?.accion === 'INSERTAR') {
                console.log(result);
                const response = await insertarLista();
                if (response.isSuccess) {
                   
                    setProcesoAprobacion('COMPLETADO');
                    setLista([]);
                    swal({ text: 'La lista se ha aprobado correctamente',  icon: 'success' });
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Generar Lista parámetros:
                         \n fecha pago cuota:  ${ moment(formState?.fechaAplicacion).format("DD-MM-YYYY")}
                         \n fecha pago:  ${moment(formState?.fechaProceso).format("DD-MM-YYYY")}
                         \n periodo: ${moment(formState?.fechaAplicacion).format("YYYYMM")}
                         `
                    setFormState(null);
                     await auditoriaApi.InsertAuditoria(requestAuditoria);
                    return;
                }
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=5;
                requestAuditoria.descripcion=`Generar Lista parámetros:
                         \n fecha pago cuota:  ${ moment(formState?.fechaAplicacion).format("DD-MM-YYYY")}
                         \n fecha pago:  ${moment(formState?.fechaProceso).format("DD-MM-YYYY")}
                         \n periodo: ${moment(formState?.fechaAplicacion).format("YYYYMM")}
                \n returnApiMessage: ${response?.data?.returnMessage}`
                 await auditoriaApi.InsertAuditoria(requestAuditoria);
                throw response;
            }

        } catch (error: any) {
            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion=5;
            requestAuditoria.descripcion=`Generar Lista parámetros:
            \n fecha pago cuota:  ${ moment(formState?.fechaAplicacion).format("DD-MM-YYYY")}
            \n fecha pago:  ${moment(formState?.fechaProceso).format("DD-MM-YYYY")}
            \n periodo: ${moment(formState?.fechaAplicacion).format("YYYYMM")}
            \n Error:  ${error}
            \n returnApiMessage: ${error?.returnMessage}`
            await auditoriaApi.InsertAuditoria(requestAuditoria);
            toast(error?.returnMessage ?? "Ha ocurrido un error al ejecutar la petición", { type: toast.TYPE.ERROR });
        }

        setProcesoAprobacion('INICIAL');
    }

    const obtenerMesAnio = (fecha: string | Date): string => {
        const date = new Date(fecha);
        const mes = (date.getMonth() + 1).toString().padStart(2, '0'); // Asegura dos dígitos en el mes
        return `${date.getFullYear()}-${mes}`; // Formato YYYY-MM
    };

    const obtenerMesSiguiente = (fecha: string | Date): string => {
        const date = new Date(fecha);
        date.setMonth(date.getMonth() + 1); // Avanza un mes
        const mes = (date.getMonth() + 1).toString().padStart(2, '0');
        return `${date.getFullYear()}-${mes}`;
    };

    const handleOnClickList = async(data: any) => {
        
        const mesAnioAplicacion = obtenerMesAnio(data.fechaAplicacion);
        const mesAnioSiguienteCierre = obtenerMesAnio(fechaUltimoCierreMensual);
        // console.log('📅 Fecha de Aplicación:', data.fechaAplicacion);
        // console.log('📅 Fecha Último Cierre:', fechaUltimoCierreMensual);
        // console.log('🧮 Mes y Año de Aplicación:', mesAnioAplicacion);
        // console.log('🧮 Mes Siguiente al Cierre:', mesAnioSiguienteCierre);

        try{
           
            const response = await creditApi.obtenerProcesoAplicacionPagos(
               {unidadEjecutora: data?.unidadEjecutora, 
                estado: 'APROBADA', 
                fechaCreacion: new Date(data.fechaAplicacion),
                tipoCredito: data?.tipoCredito
               }
            );
               
  
                const confirmAlert = NotificationService.confirmAlert(
                  `¿Está seguro que desea continuar con el proceso?`
                );
                 
                if(await confirmAlert){
                    setFormState(data);
                    generarReporteLista(data.unidadEjecutora, data.tipoCredito, data.fechaAplicacion);
                    handleCloseModalList();
                }
                
         
        }catch(error: any){
            toast('Error al consultar lista', { type: toast.TYPE.ERROR });
       }
    
    };

    const generarReporteLista = async (
        unidadEjecutora: number, tipoCredito: number, fechaAplicacion: Date
    ) => {
        setLoading(true);
        const auditoriaApi=AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
                
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIpAddress,
            modulo:'Generar Lista',
            hostname:document.location.host
            } as Auditoria
        await api
            .generarListaAplicacionDePago(unidadEjecutora, tipoCredito, fechaAplicacion)
            .then(async response => {
                const array = (response.data || []).filter((item: any) =>
                    item.idProceso === null ||
                    item.idProceso === 0 ||
                    item.idProceso === '0'
                );
            
                if (response.isSuccess && array.length > 0) {
                    setLista(array);
                    setProcesoAprobacion('INICIAL');
            
                    requestAuditoria.tipoAccion = 'TRANSACCIONES';
                    requestAuditoria.idTipoAccion = 2;
                    requestAuditoria.descripcion = `Generar Lista aplicación pagos parámetros:
                     \n Unidad ejecutora:  ${unidadesEjecutoras.find(x => x.id === unidadEjecutora)?.sigla}
                     \n Tipo de crédito:  ${tiposCredito.find(x => x.idTipoCredito === tipoCredito)?.descripcion}
                     \n Fecha aplicación: ${moment(fechaAplicacion).format("DD-MM-YYYY")}
                     `;
            
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                } else {
                    setLista([]);
            
                    toast(
                        array.length === 0
                            ? 'No hay registros pendientes por aprobar. Los registros encontrados ya tienen proceso de pago.'
                            : response.returnMessage,
                        { type: toast.TYPE.WARNING }
                    );
            
                    requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion = 5;
                    requestAuditoria.descripcion = `Generar Lista aplicación pagos parámetros:
                    \n Unidad ejecutora:  ${unidadesEjecutoras.find(x => x.id === unidadEjecutora)?.sigla}
                    \n Tipo de crédito:  ${tiposCredito.find(x => x.idTipoCredito === tipoCredito)?.descripcion}
                    \n Fecha aplicación: ${moment(fechaAplicacion).format("DD-MM-YYYY")}
                    \n returnApiMessage: ${response.returnMessage}`;
            
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            })
            .catch((error:any) => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=5;
                requestAuditoria.descripcion=`Generar Lista aplicación pagos parámetros:
                \n Unidad ejecutora:  ${unidadesEjecutoras.find(x=>x.id===unidadEjecutora).sigla}
                \n Tipo de crédito:  ${tiposCredito.find(x=>x.idTipoCredito===tipoCredito).descripcion}
                \n Fecha aplicación: ${moment(fechaAplicacion).format("DD-MM-YYYY")}
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            })
            .finally(() => {
                setLoading(false);
            });
    };
    
    const getListInfo = () => {
        if(formState) {
            return {
                fechaAplicacion: moment(formState.fechaAplicacion).format('DD/MM/YYYY'),
                fechaProceso: moment(formState.fechaProceso).format('DD/MM/YYYY'),
                unidadEjecutora: unidadesEjecutoras.find((x: any) => x.id === formState.unidadEjecutora)?.nombre ?? '',
                tipoCredito: tiposCredito.find(x => x.idTipoCredito === formState.tipoCredito)?.descripcion ?? '',
                periodo: moment(formState.fechaAplicacion).format('YYYYMM')
            }
        }
        return null;
    }

    const calcTotal = (lista: number[]) =>
    { 
        const result = lista?.reduce((prev, curr, _) => prev + curr, 0) ?? 0;
        const formatter = new Intl.NumberFormat('en-EN', {
            style: 'currency',
            currency: 'USD',
            minimumFractionDigits: 0,
            maximumFractionDigits: 2,
        });

        return formatter.format(result);
    };

    const handleDeletePago = (data: any) => {
        // Aquí tu lógica para eliminar un pago individual
        console.log("Eliminar pago con ID:", data);
      };

    if (loading) {
		return <CHLoadingProcess />;
	}

    return <div>
        <LoadingComponent key="loader" loading={loading} />

        <Grid container>
            <Grid className={classes.column} item xs={12} md={2}>
                <Button
                    variant="contained"
                    className={classes.button}
                    color="primary"
                    component="label"
                    onClick={handleOpenModalList}
                    disabled={procesoAprobacion==='PROCESANDO'}
                >
                    Crear Lista
                </Button>
            </Grid>
        </Grid>

        <Grid container>

            <InfoPanel data={getListInfo()} classes={classes.infoPanel}/>

            {
                lista.some(x => x.observaciones) && 
                <span style={{fontWeight: 'bold', fontSize: '16px', marginTop: '1rem', display: 'block'}}>
                    Observaciones: {lista.filter(x => x.observaciones).length}
                </span>
            }

<TableMassivePayAplicationList
    listState={lista}
    setListState={setLista}
    onRowsDelete={handleRowsDelete}
    selectableRows={procesoAprobacion !== 'INICIAL' ? 'none' : 'multiple'}
    rowsSelected={selectedRows}
    onRowSelectionChange={handleSelectedRow}
    listInfo={getListInfo()}
    onDeletePago={handleDeletePago}
/>



            <div className={classes.groupButtonLoader}>
                <Button variant="contained" color="primary" component="label" disabled={!lista.length || procesoAprobacion !== 'INICIAL'}
                    onClick={handleAprobarLista}>
                        APROBAR LISTA
                </Button>
                <span style={{fontWeight: 'bold', fontSize: '18px'}}>Total: {calcTotal(lista.map(x => x.valorCuota))}</span>
                {
                    procesoAprobacion === 'PROCESANDO' && <CircularProgress  size='1.5rem' />
                }
            </div>
        </Grid>

        

        <ModalAplicacionMasivaPagosList
            open={openModalList}
            unidadesEjecutoras={unidadesEjecutoras}
            handleClose={handleCloseModalList}
            handleOnClick={handleOnClickList}
            formState={formState}
        />

    </div>


};

export default AprobacionListas;