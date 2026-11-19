import { Button, createStyles, Grid, makeStyles, Theme } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import CreditApi from "../../services/api-credit.service";
import NotificationService from "../../services/notification.service";
import CHLoadingProcess from "../ch-loading-process/CHLoadingProcess";
import CHPaper from "../ch-paper/CHPaper";
import CHPrivateContainer from "../ch-private-container/CHPrivateContainer";
import TablaCpdInformacion from "./TablaCdpInformacion";

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        column: {
            padding: '10px',
           
        },
        button: {
          width:'200px',
            marginLeft: '.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        root: {
            minWidth: '100%',
            margin: '20px',
        },
        bullet: {
            display: 'inline-block',
            margin: '0 2px',
            transform: 'scale(0.8)',
        },
        title: {
            fontSize: 14,
        },
        card: {
            margin: '30px',
        },

        rootDos: {
            flexGrow: 1,
        },
        paper: {
            padding: theme.spacing(2),
            textAlign: 'center',
            color: theme.palette.text.secondary,
        },
    }),
);

const api = CreditApi();


function BotoneraPagosDesembolso ({
    procedureRequest,
    creditProductData,
    dataPrimerPago,
    datosCuotaInicial,
    datosDesembolso
}: {
        procedureRequest: any;
        creditProductData: any;
        dataPrimerPago: any;
        datosCuotaInicial: any;
        datosDesembolso: any
}): any {
    const classes = useStyles();

    const [isLoading, setIsLoading] = useState (false);
    const [verNuemeroCDP, setVerNuemeroCDP] = useState(false);
    const [numeroCDP, setNumeroCDP] = useState("");
    const [cdpDatos, setCdpDatos] = useState<any>({});

    const mostarPrimerPago = procedureRequest.flowActivityID === 16;
    const mostarSegundoPago = procedureRequest.flowActivityID === 18;

    // const consultaSeven = dataPrimerPago.length > 0 ? true: false ;

    const consultaSeven = async () => {
        // await CrearTerceroSevenCredito(dataPrimerPago)
        // await CrearCDPPrimerPago(dataPrimerPago)
        // await  CrearProveedorSeven()
        // await  CrearFacturaSevenPrimerPago(dataPrimerPago)
        generarPrimerPago()
    }



    useEffect(  ()  =>  {
        if(dataPrimerPago.length !== 0){
            consultaSeven()
        }
    }, [dataPrimerPago]);


    const obternerCdpConsecutivoMostrar = () => {
        
        setIsLoading(true)
        const request = procedureRequest.simulationID
        api
            .postConsultarNotificacionesSolicitudesCredito(request)
            .then(response => {
                if (response) {
                    if (response.isSuccess) {
                        setCdpDatos(response.data)
                        setVerNuemeroCDP(true)
                        setIsLoading(false)
                        
                    }else{
                        setVerNuemeroCDP(true) 
                        setIsLoading(false)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
                setIsLoading(false)
            });
    }

    const solicitudCdp = async(data: any) => {
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID,
        }
            await api
            
            .postCrearCDPSeven(response)
            .then(async response => {
                setIsLoading(false)
                if (response) {
                    if (response.isSuccess) {

                const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                const numCDP = response.returnMessage.match(/<MPR_CONT>(.*?)<\/MPR_CONT>/);
                setNumeroCDP(numCDP[1])

                if(retornoExitoso[1] !== "0"){
                    setVerNuemeroCDP(true)
                    NotificationService.genericAlertError(
                        
                        'CONSULTA CDP',
                        'No hay disponibilidad de recursos',
                         'warning',
                    ); 
                }else{
                    setVerNuemeroCDP(true)
                    NotificationService.genericAlertError(
                        
                        'CONSULTA CDP',
                        ` El numero de CDP asignado es: ${(numCDP[1])}`,  
                        'success',
                    );
                }
            }}
            })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }

  

    const generarPrimerPago = async() => {
        setIsLoading(true)
        const request = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await api
            .DesmbolsoCreditoMomento1(request)
            .then(async response => {
                if(response){
                    if(response.isSuccess){ 
                        setIsLoading(false)
 
                            NotificationService.genericAlertError(
                                
                    
                                'Desembolso Primer Pago',
                                `${response.returnMessage}`,
                                'warning',
                            )
                          
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
        
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        
    }

    // const generarSegundoPago = async(data: any) => {
    //     setIsLoading(true)
    //     const response = {
    

    //         idSolicitudCredito: procedureRequest.requestID,
    //         tipoIdentificacion: creditProductData.tidID,
    //         numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
    //         afl_Id: procedureRequest?.flowActivityID,

    //     }
    //         await api
    //         .DesmbolsoCreditoMomento2(response)
    //         .then(async response => {
    //             if(response){
    //                 if(response.isSuccess){ 
    //                     setIsLoading(false)
    //                     const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
    //                     const numCDP = response.returnMessage.match(/<MPR_CONT>(.*?)<\/MPR_CONT>/);
    //                     setNumeroCDP(numCDP[1])

    //                     if(retornoExitoso[1] !== "0"){
    //                         setVerNuemeroCDP(true)
    //                         NotificationService.genericAlertError(
                                
    //                             'CONSULTA CDP',
    //                             'No hay disponibilidad de recursos',
    //                             'warning',
    //                         ); 
    //                     }else{
    //                         setVerNuemeroCDP(true)
    //                         NotificationService.genericAlertError(
                                
    //                             'CONSULTA CDP',
    //                             ` El numero de CDP asignado es: ${(numCDP[1])}`,  
    //                             'success',
    //                         );
    //                     }
    //                 }else {
    //                     setIsLoading(false)
    //                     toast(`Error: ${response.returnMessage}`, {
    //                         type: toast.TYPE.ERROR,
    //                     });
        
    //             }}
    //           })


    //          .catch(() => {
    //             setIsLoading(false)
    //              toast('Error del sistema', { type: toast.TYPE.ERROR });
    //         });
        


    // }
    const CrearCDPPrimerPago = async() => {
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await api
            .postCrearTerceroSevenCredito(response)
            .then(async response => {
                setIsLoading(false)
                if(response){
                    if(response.isSuccess){  
                        
                        const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                        const retornoMensaje = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                       
                        if(retornoExitoso[1] !== "0"){
                            // const nuevoMensaje = retornoMensaje[1].replace('Tercero', 'Afiliado');
                            // CrearProveedorSeven()
                            guardarCdp(retornoExitoso[1])
                           
                        }
                        else{
                            
                            NotificationService.genericAlertError(
                                
                                'VERIFICACIÓN DEL AFILIADO',
                                `Exitoso`,  
                                'success',
                            );
                        }
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        


    }


    const guardarCdp = async(cdp:any) => {
        const request = {
            simulationID: procedureRequest?.simulationID,
           numeroSolicitud: procedureRequest?.requestNumber
        }
        await api
        .postInsertarCdp(request)
        .then(async response => {
            obternerCdpConsecutivoMostrar()
          })
         .catch(() => {
           
             toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
    }





    const CrearProveedorSeven = async() => {
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await api
            .postCrearProveedorSeven(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){
                    
                        setIsLoading(false)
                        
                        const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                        const retornoMensaje = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
                    
                        if(retornoExitoso[1] !== "0"){
                        
                            guardarDatosInformacion()
                            
                        }
                        else{
                            
                            NotificationService.genericAlertError(
                                
                                'CREAR BENEFICIARIO',
                                ` ${retornoMensaje[1]}`,  
                                'success',
                            );
                            guardarDatosInformacion()
                            setVerNuemeroCDP(true)
                        }
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
            }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast("Error del sistema", { type: toast.TYPE.ERROR });
            });
        


    }
    const CrearCDPPrimerPagoFuncional = async() => {
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await api
            .postCrearCDPPrimerPago(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){  
                setIsLoading(false)
                const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                const numCDP = response.returnMessage.match(/<MPR_CONT>(.*?)<\/MPR_CONT>/);
                const retornoFallido = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);

                setNumeroCDP(numCDP[1])

                if(retornoExitoso[1] !== "0"){
                    setVerNuemeroCDP(true)


                    NotificationService.genericAlertError(
                        
                        'SOLICITUD CDP',
                        'No hay disponibilidad de recursos',
                 
                         'warning',
                    ); 
                    // guardarDatosInformacion(numCDP)
                }else{
                    setVerNuemeroCDP(true)
                    NotificationService.genericAlertError(
                        
                        'SOLICITUD CDP',
                        // ` El numero de CDP asignado es: ${(numCDP[1])}`,  
                        ` Solicitud enviada con éxito,`,  
                        'success',
                    );
                    // guardarDatosInformacion(numCDP)
                }
            }else {
                setIsLoading(false)
                toast(`Error: ${response.returnMessage}`, {
                    type: toast.TYPE.ERROR,
                });

            }}
              })
              

             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        


    }
    const CrearCDPSegundoPago = async(data: any) => {
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await api
            .postCrearCDPSegundoPago(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){  
                        setIsLoading(false)
                        const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                        const numCDP = response.returnMessage.match(/<MPR_CONT>(.*?)<\/MPR_CONT>/);
                        setNumeroCDP(numCDP[1])

                        if(retornoExitoso[1] !== "0"){
                            setVerNuemeroCDP(true)
                            NotificationService.genericAlertError(
                                
                                'CONSULTA CDP',
                                'No hay disponibilidad de recursos',
                                'warning',
                            ); 
                        }else{
                            setVerNuemeroCDP(true)
                            NotificationService.genericAlertError(
                                
                                'CONSULTA CDP',
                                ` El numero de CDP asignado es: ${(numCDP[1])}`,  
                                'success',
                            );
                        }
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                    });
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        


    }

    console.log({procedureRequest})

    const guardarDatosInformacion = () => {
        const request = {
            
            simulationID: procedureRequest.simulationID,
            NumeroSolicitud:procedureRequest.requestNumber
        };
        api
        .postGuardarCdp(request)
        .then(response => {
            if (response) {
                if (response.isSuccess) {
                    NotificationService.genericAlertError(  
                        'Se guardo Solicitud y notificaciones',
                        `Exitoso`,  
                        'success',
                    );
                }
            }
        })
        .catch(() => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
    
       
    };
    // const CrearFacturaSevenPrimerPago = async(data: any) => {
    //     setIsLoading(true)
    //     const response = {
    //         idSolicitudCredito: procedureRequest.requestID,
    //         tipoIdentificacion: creditProductData.tidID,
    //         numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
    //         afl_Id: procedureRequest?.flowActivityID
    //     }
    //         await api
    //         .postCrearFacturaSevenPrimerPago(response)
    //         .then(async response => {
    //             if(response){
    //                 if(response.isSuccess){  
    //                     setIsLoading(false)
    //                     const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);

    //                     if(retornoExitoso[1] !== "0"){
    //                         NotificationService.genericAlertError(
                                
    //                             'FACTURA PRIMER PAGO',
    //                             `Rechazado intente nuevamente`,
    //                             'warning',
    //                         ); 
    //                     }else{
    //                         NotificationService.genericAlertError(
                                

    //                             'FACTURA PRIMER PAGO',
    //                             'Creada con éxito', 
    //                             'success',
    //                         );
    //                     }
    //                 }else {
    //                     setIsLoading(false)
    //                     toast(`Error: ${response.returnMessage}`, {
    //                         type: toast.TYPE.ERROR,
    //                     });
        
    //             }}
    //           })


    //          .catch(() => {
    //             setIsLoading(false)
    //              toast('Error del sistema', { type: toast.TYPE.ERROR });
    //         });
        


    // }
    const CrearFacturaSevenSegundoPago = async(data: any) => {
        setIsLoading(true)
        const response = {
            idSolicitudCredito: procedureRequest.requestID,
            tipoIdentificacion: creditProductData.tidID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
            afl_Id: procedureRequest?.flowActivityID
        }
            await api
            .postCrearFacturaSevenSegundoPago(response)
            .then(async response => {
                if(response){
                    if(response.isSuccess){ 
                        setIsLoading(false)

                        const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                        if(retornoExitoso[1] !== "0"){
                            NotificationService.genericAlertError(
                                
                                'FACTURA SEGUNDO PAGO',
                                `Rechazado intente nuevamente`,
                                'warning',
                            ); 
                        }else{
                            NotificationService.genericAlertError(
                                

                                'FACTURA SEGUNDO PAGO',
                                'Creada con éxito', 
                                'success',
                            );
                        }
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    const ConsultaCDP = async() => {
        setIsLoading(true)
        

        const request = {
            idSolicitudCredito: procedureRequest.requestID,
            numeroDocumentoCliente: String(procedureRequest.clientIdentificationNumber),
        }
            await api
            .postObtenerValorCdp(request)
            .then(async response => {
                if(response){
                    if(response.isSuccess){ 
                        setIsLoading(false)
 
                            NotificationService.genericAlert(
                                'Creación CDP',
                                `El consecutivo del certificado de disponibilidad es:${response.data[0].mpR_CONT}`,
                                'success',
                            )
                          
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
        
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        
    }

    const ConsultaFactura = async() => {
        setIsLoading(true)

        const request = {
            numerO_RADICADO: procedureRequest.requestID,
            identificacion: String(procedureRequest.clientIdentificationNumber),
        }

            await api
            .postObtenerValorFactura(request)
            .then(async response => {
                if(response){
                    if(response.isSuccess){ 
                        setIsLoading(false)
 
                            NotificationService.genericAlert(
                                'Consulta Factura',
                                `Factura creada con éxito, el numero de factura es:${response.data[0].mpR_CONT}`,
                                'success',
                            )
                          
                    }else {
                        setIsLoading(false)
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
        
                }}
              })


             .catch(() => {
                setIsLoading(false)
                 toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
        
    }




    if (isLoading) {
        return (
            <CHPrivateContainer>
                <CHLoadingProcess />
            </CHPrivateContainer>
        );
    }

    return(
        <>


            {mostarPrimerPago &&
            <CHPaper title="Información Segundo Pago">
                <Grid container>

                        {  procedureRequest?.stateActivity === "PENDIENTE" ||  datosDesembolso.length !== 0?
                            <Grid item xs={12} md={3} className={classes.column}>
                                <Button

                                    onClick={()=> CrearCDPPrimerPago()}
                                    className={classes.button}
                                    variant="outlined"
                                >
                                    SOLICITAR CDP
                                </Button>
                            </Grid>: null
                        }

                       
                </Grid>

                    {verNuemeroCDP &&
                        <TablaCpdInformacion
                            procedureRequest={procedureRequest}
                            numeroCDP={cdpDatos?.noT_CDP} />
                    }

                    { procedureRequest?.activity ==="LEGALIZACIÓN" && procedureRequest?.stateActivity === "FINALIZADA" &&
                        <TablaCpdInformacion
                            procedureRequest={procedureRequest}
                            numeroCDP={cdpDatos?.noT_CDP} />
                    }
            </CHPaper>
            }
            {mostarSegundoPago &&
             <CHPaper title="Información Segundo Pago ">
             <Grid container>
             
                 
             <Grid item xs={12} md={3} className={classes.column}>
                     <Button
                         
                         onClick={CrearCDPPrimerPago}
                         className={classes.button}
                         variant="outlined"
                     >
                        1. VALIDAR AFILIADO
                     </Button>
             </Grid>

             <Grid item xs={12} md={3} className={classes.column}>
                     <Button
                       
                         onClick={CrearProveedorSeven}
                         className={classes.button}
                         variant="outlined"
                     >
                         2. CREAR BENEFICIARIO 
                     </Button>

                   
                     
                </Grid>

             <Grid item xs={12} md={3} className={classes.column}>
                     <Button
                         
                         onClick={CrearCDPSegundoPago}
                         className={classes.button}
                         variant="outlined"
                     >
                         3. SOLICITAR CDP 
                     </Button>
             </Grid>
             <Grid item xs={12} md={3} className={classes.column}>
                     <Button
                         
                         onClick={ConsultaCDP}
                         className={classes.button}
                         variant="outlined"
                     >
                         4. CREAR CDP 
                     </Button>
             </Grid>
             <Grid container>

          
                <Grid item xs={12} md={3} className={classes.column}>
                     <Button
                         
                         onClick={CrearFacturaSevenSegundoPago}
                         className={classes.button}
                         variant="outlined"
                     >
                         5. CREAR FACTURA 
                     </Button>
                 </Grid>

                <Grid item xs={12} md={3} className={classes.column}>
                     <Button
                         
                         onClick={ConsultaFactura}
                         className={classes.button}
                         variant="outlined"
                     >
                     6. CONSULTAR FACTURA 
                     </Button>
                 </Grid>


             </Grid>
             </Grid>
 
         </CHPaper>
            // <CHPaper title="Información Segundo Pago ">
            //     <Grid container>
                
                    
            //     <Grid item xs={12} md={3} className={classes.column}>
            //             <Button
                            
            //                 onClick={CrearTerceroSevenCredito}
            //                 className={classes.button}
            //                 variant="outlined"
            //             >
            //                CREAR TERCERO 
            //             </Button>
            //     </Grid>
              

            //     <Grid item xs={12} md={3} className={classes.column}>
            //             <Button
                            
            //                 onClick={CrearCDPSegundoPago}
            //                 className={classes.button}
            //                 variant="outlined"
            //             >
            //                 SOLICITUD CDP 
            //             </Button>
            //     </Grid>
                
            //     <Grid item xs={12} md={3} className={classes.column}>
            //             <Button
                          
            //                 onClick={CrearProveedorSeven}
            //                 className={classes.button}
            //                 variant="outlined"
            //             >
            //                 CREAR BENEFICIARIO
            //             </Button>

            //             {/* {
            //                 verNuemeroCDP ?
            //                 <Grid item xs={12} md={6}>
                                
            //                             <CHTextField
            //                                 isReadOnly
            //                                 label="Nuemero CDP"
            //                                 value={numeroCDP}                                      
            //                                 InputLabelProps={{shrink: true, }}
            //                                 inputProps={{
            //                                     style: {
            //                                         textTransform: 'uppercase',
            //                                     },
            //                                 }}
            //                             />
                                 
                                
            //                 </Grid>

                                                       
                                
            //                     :[]
            //                } */}
                        
            //        </Grid>
            //        <Grid item xs={12} md={3} className={classes.column}>
            //             <Button
                            
            //                 onClick={CrearFacturaSevenSegundoPago}
            //                 className={classes.button}
            //                 variant="outlined"
            //             >
            //                 CREAR FACTURA 
            //             </Button>
            //     </Grid>


            //     </Grid>
            // </CHPaper>
            }
        
        </>
        
        );
}
    

export default BotoneraPagosDesembolso;