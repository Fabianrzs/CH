import React, { ForwardedRef, forwardRef, useEffect, useImperativeHandle, useRef, useState } from "react";
import ReactDOM from "react-dom";
import { IconButton, LinearProgress, createStyles, makeStyles, Tooltip, Button } from "@material-ui/core";
import { CancelOutlined, CloudDownloadOutlined, DeleteOutlined, InfoOutlined } from "@material-ui/icons";
import { toast } from "react-toastify";
import swal from 'sweetalert';
import ApiproductoClients from "../../../../../services/api-producto.service";
import { EstadoReporte } from "../types/EstadoReporte";
import ProcesoReporte from "../types/ProcesoReporte";
import { TipoNombreReporte, TipoReporte } from "../types/TipoReporte";
import AuditoriaApi from '../../../../../services/api-auditorias.servise';
import AuthService from '../../../../../services/auth.service';
import { Auditoria } from '../../../../../types/Auditoria.model.type';
import NotificationService from "../../../../../services/notification.service";

const useStyles = makeStyles(() =>
    createStyles({
        container: {
            marginTop: '2rem',
        }
    }),
);

const mensajeEstado = (estado: EstadoReporte, progreso = 0) => {
    switch(estado) {
        case 'PENDIENTE':   return progreso ? 'Se esta generando el reporte ...' : 'Espere mientras se genera el reporte ...';
        case 'COMPLETADO':  return 'Reporte generado';
        case 'FALLIDO':     return 'Ocurrio un error al generar el reporte ...';
        case 'CANCELANDO':  return 'Cancelando ...';
        case 'ELIMINANDO':  return 'Eliminando ...';
        default: return '';
    }
}

const ProcesoGeneracionReportes = forwardRef((props: {tipoReporte: TipoReporte, tipoCreditoNombre: string, idProceso: string | null}, ref: ForwardedRef<any>) => {
    
    const { tipoReporte, tipoCreditoNombre, idProceso } = props;
    const classes = useStyles();
    const apiProductos = ApiproductoClients();
    const [procesos, setProcesos] = useState<ProcesoReporte[]>([]);
    const consultando = useRef(false);
    const finalizar = useRef(false);

    useImperativeHandle(ref, () => ({
        consultarProcesos
    }));

    useEffect(() => {
        consultarProcesos();
        return () => { finalizar.current = true; }
    }, []);

    const consultarProcesos = () => {

        if(consultando.current || finalizar.current) {
            return;
        }

        consultando.current = true;

       let request;
       
        if( tipoReporte === TipoReporte.RETEFUENTE_ANIO_GRAVABLE_MASIVO &&
            tipoCreditoNombre === TipoNombreReporte.CONSULTA_TOTAL_CARTERA){
            request = {
                tipoReporte: TipoReporte.RETEFUENTE_ANIO_GRAVABLE_MASIVO,
                tipoCredito: 0,
                idProceso: idProceso
            }
        }else if( tipoReporte === TipoReporte.RETEFUENTE_ANIO_GRAVABLE_MASIVO &&
                  tipoCreditoNombre === TipoNombreReporte.LEASING_HABITACIONAL){
            request = {
                tipoReporte: TipoReporte.RETEFUENTE_ANIO_GRAVABLE_MASIVO,
                tipoCredito: 1,
                idProceso: idProceso
            }
        }else if( tipoReporte === TipoReporte.RETEFUENTE_ANIO_GRAVABLE_MASIVO &&
                  tipoCreditoNombre === TipoNombreReporte.HIPOTECARIO){
            request = {
                tipoReporte: TipoReporte.RETEFUENTE_ANIO_GRAVABLE_MASIVO,
                tipoCredito: 2,
                idProceso: idProceso
            }
        }

        apiProductos.consultaProcesoGeneracionReporte(request)
        .then((response: any) => {
            if (response?.isSuccess) {
                setProcesos(response.data);
                if(response?.data?.some((x: any) => x.estado === "PENDIENTE") && !finalizar.current) {
                    setTimeout(() => {
                        consultando.current = false;
                        consultarProcesos();
                    }, 5000);
                }
                else {
                    consultando.current = false;
                }
            }
            return response;
        }).catch(e => {
            setTimeout(() => {
                consultando.current = false;
                consultarProcesos();
            }, 5000);
        });
    }

    const cancelarProceso = async (idProceso: string) => {

        const result = await swal({ text: '¿Está seguro que desea cancelar?', buttons: { 
            confirm: { text: "Sí", value: true, visible: true },
            cancel: { text: "No", value: false, visible: true }
        }, icon: 'warning' });

        const proceso = procesos?.find(x => x.idProceso === idProceso);

        if(result && proceso)
        {
            proceso.estado = "CANCELANDO";
            setProcesos([...procesos]);
            apiProductos.cancelarProcesoGeneracionReporte(idProceso)
            .then((response: any) => {
                if (!response?.isSuccess) {
                    toast(response.returnMessage || 'fallo al cancelar', {type: 'error', hideProgressBar: true});
                }
            }).catch(e => {
                toast('fallo al cancelar', {type: 'error', hideProgressBar: true});
            })
            .finally(() => setTimeout(consultarProcesos, 0));
        }
    }

    const eliminarProceso = async (idProceso: string) => {

        const result = await swal({ text: '¿Está seguro que desea eliminar?', buttons: { 
            confirm: { text: "Sí", value: true, visible: true },
            cancel: { text: "No", value: false, visible: true }
        }, icon: 'warning' });

        const proceso = procesos?.find(x => x.idProceso === idProceso);

        if(result && proceso)
        {
            proceso.estado = "ELIMINANDO";
            setProcesos([...procesos]);
            apiProductos.eliminarProcesoGeneracionReporte(idProceso)
            .then((response: any) => {
                if (!response?.isSuccess) {
                    toast(response.returnMessage || 'fallo al eliminar', {type: 'error', hideProgressBar: true});
                }
            }).catch(e => {
                toast('fallo al eliminar', {type: 'error', hideProgressBar: true});
            })
            .finally(() => setTimeout(consultarProcesos, 0));
        }
    }


    const descargarReporte = (proceso: ProcesoReporte) => {
        toast('Se ha iniciado la descarga, espere mientras finaliza ...', {type: 'success', hideProgressBar: true});
        const auditoriaApi= AuditoriaApi();     
        const user = AuthService.getUserInfo();                          
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;
        const requestAuditoria={
                    navegador:usrAg,
                    sistemaOperativo:nav.userAgentData.platform,
                    nombreUsuario:user.USR_USERNAME, 
                    usurioId:user.USR_ID,
                    ip:'192.168.217.19',
                    modulo:'Certificado de Ingresos y Retenciones',
                     hostname:document.location.host
                } as Auditoria;
        apiProductos.generarTokenReporteMasivo(proceso.idProceso)
            .then(response => {
                
                if (response?.isSuccess && response?.data) {
                    const url = apiProductos.obtenerUriReporteMasivo(proceso.idProceso, response?.data);
                    window.open(url);
                    requestAuditoria.tipoAccion='DESCARGAS';
                    requestAuditoria.idTipoAccion=7;
                    requestAuditoria.descripcion=`Descarga de archivos  ${proceso.tipoReporte} parámetros:
                        \n tipo reporte: ${proceso.tipoReporte}
                        \n archivo: ${proceso.nombreReporte}`;
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }
                
            }).catch((error:any) => {
                toast('fallo al descargar', {type: 'error', hideProgressBar: true});
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=5;
                requestAuditoria.descripcion=`Descarga de archivos  ${proceso.tipoReporte} parámetros:
                \n tipo reporte: ${proceso.tipoReporte}
                \n archivo: ${proceso.nombreReporte}               
                        \n Error:  ${error}
                        \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            });
    }

    const detalle = (props: { detalleProceso?: string }) => {
        const element = document.createElement('div');
        const listado = props.detalleProceso?.trim()?.split(/\r?\n/) ?? [];
        ReactDOM.render(
            <>
                <div style={{maxHeight: "300px", overflowY: "auto", marginTop: "1.5rem"}}>
                    <ul>
                        {
                            listado.map((detalle: string, index: number) => 
                                <li key={index} style={{textAlign: "left", marginTop: '1rem'}}>{detalle}</li>
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

    const verDetalle = async (proceso: ProcesoReporte) => {
        await swal(detalle({ detalleProceso: proceso.detalleProceso}));
    };

    return (
        <div className={classes.container}>
        {procesos
          .filter(p => !p.estado.includes('ELIMINADO') && !p.estado.includes('CANCELADO'))
          .map((proceso: ProcesoReporte) => (
            <div
              style={{ display: 'flex', columnGap: '12px', marginTop: '1rem' }}
              key={proceso.idProceso}
            >
              <div style={{ flexShrink: '0' }}>
                {proceso.estado === "COMPLETADO" && (
                  <>
                    <Tooltip title="Descargar">
                      <IconButton onClick={() => descargarReporte(proceso)}>
                        <CloudDownloadOutlined />
                      </IconButton>
                    </Tooltip>
                    <Tooltip title="Eliminar">
                      <IconButton onClick={() => eliminarProceso(proceso.idProceso)}>
                        <DeleteOutlined />
                      </IconButton>
                    </Tooltip>
                  </>
                )}
                {proceso.estado === "FALLIDO" && (
                  <>
                    <Tooltip title="Eliminar">
                      <IconButton onClick={() => eliminarProceso(proceso.idProceso)}>
                        <DeleteOutlined />
                      </IconButton>
                    </Tooltip>
                  </>
                )}
                {proceso.estado === "PENDIENTE" && (
                  <Tooltip title="Cancelar">
                    <IconButton onClick={() => cancelarProceso(proceso.idProceso)}>
                      <CancelOutlined />
                    </IconButton>
                  </Tooltip>
                )}
              </div>
             
            <div style={{ flexGrow: 1 }}>
                <div>
                  <span style={{ display: 'block' }}>{proceso.nombreReporte}</span>
                  <span style={{ display: 'block' }}>
                    {mensajeEstado(proceso.estado, proceso.progreso)}
                  </span>
                </div>
                <div>
                  {proceso.estado === "PENDIENTE" && (
                        <>
                            <LinearProgress
                                value={proceso.progreso}
                                variant="determinate"
                            />
                            <div
                                style={{
                                    marginTop: 4,
                                    fontSize: 12,
                                    textAlign: 'right',
                                    color: '#555'
                                }}
                            >
                                {`${proceso.progreso}%`}
                            </div>
                        </>
                        )}
                        </div>
                    </div>
            </div>
          ))}
      </div>
    );
});

export default ProcesoGeneracionReportes;