import { Button, createStyles, Grid, makeStyles } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import moment from 'moment';
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import { ProcedureRequestModel } from "../../models/procedure-request.model";
import CreditApi from "../../services/api-credit.service";
import CHLoadingProcess from "../ch-loading-process/CHLoadingProcess";
import CHPaper from "../ch-paper/CHPaper";
import CHTextField from "../ch-text-field/CHTextField";
import ApiReportes from "../../services/api-reportes.service";
import Prospeccion from "../../services/api-prospeccion.service";
import NotificationService from "../../services/notification.service";
import UtilService from "../../utils/util.service";


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '10px',
            marginLeft: '.5rem',
      
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
    }),
);


function CentralDeRiesgo({
    procedureRequest,
    isLoading,
}: {
    procedureRequest: ProcedureRequestModel;
    isLoading:boolean;
}) {

    const classes = useStyles();
    const CreditoApi = CreditApi();
    const reportesApi = ApiReportes();
    const apiProspeccion = Prospeccion();

   
    const [responseReport, setResponseReport] = useState<any>([]);
    const [isLoadingDos, setIsLoadingDos] = useState(false);
    const [obtenerRutaImagen, setObtenerRutaImagen] = useState('');
    const formularioCentralDeRiesgo = useForm({
        defaultValues: {

            salarioTotal: 0,
            descuentos: 0,
            salarioNeto: 0,
            capacidaPago: 0,
            canonCuota: 0,
            disponibleLibranza: 0,
            controlCapacidaPago: " ",
            cuotaFinanciero: 0,
            cuotaReal: 0,
            totalObligaciones: 0,
            cuotaSinCanon: "",
            cuotaConCanon: "0",
            ControlEndeudamiento: "",
            scoring: 0,
            decisionHomologada: "",
        },
    })



    useEffect(() => {
        DocumentosActSimulacionAfiliado();
        ObtenerDatosSimulacion();
        obtenerSolicitud();
        obtenerDatosProspeccionAfiliado();
    }, []);


    const DocumentosActSimulacionAfiliado = async () => {
        
        setIsLoadingDos(true);
        const request = {
            slS_ID: String(procedureRequest.simulationID),
            afL_ID: 1,
            tcR_ID: 1,
        };

        await CreditoApi
            .postDocumentosActSimulacion(request)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        const { data } = response;
                        const filtroData = data.filter((item:any) => item.tipO_DOCUMENTO === "Registro consulta centrales de riesgos")

                        if(filtroData && filtroData[0].rutA_IMAGEN !== undefined){
                            setObtenerRutaImagen(filtroData[0].rutA_IMAGEN)
                        }else{
                            await NotificationService.genericAlert(
                                'Warning',
                                'No se puede generar archivo para este Usuario.',
                                'warning',
                            );
                        }
                        setIsLoadingDos(false);
                    }
                       
                    
                }
            })
            .catch(() => {
                setIsLoadingDos(false);
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const ObtenerDatosSimulacion = async () => {
        const request = {
            smc_id: procedureRequest.simulationID,
        };
        await CreditoApi
            .getObtenerDatosSimulacion(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        formularioCentralDeRiesgo.setValue('canonCuota', response?.data[0]?.totalCuota)
                        // obtenerSalarioMensual(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };



    const obtenerDatosProspeccionAfiliado = async() => {      
            console.log({procedureRequest})
             const request = String(procedureRequest?.clientIdentificationNumber)
           
             await apiProspeccion
            .getConsultarDatosProspeccionByNumeroDocumento(request)
            .then((response:any)=>{
                if (response?.data) {
                    
                    formularioCentralDeRiesgo.setValue('salarioTotal', response.data.edP_SALARIO_TOTAL)
                    formularioCentralDeRiesgo.setValue('descuentos', response.data.edP_DEDUCCIONES)
                    formularioCentralDeRiesgo.setValue('salarioNeto', response.data.edP_SALARIO_NETO)
                    formularioCentralDeRiesgo.setValue('capacidaPago', response.data.edP_CAPACIDAD_PAGO)
                    formularioCentralDeRiesgo.setValue('controlCapacidaPago', response.data.edP_CONTROL_NIVEL_ENDUEDAMIENTO_CUOTA_PORC)
                    formularioCentralDeRiesgo.setValue('cuotaFinanciero', response.data.edP_SECTOR_FINANCIERO)
                    formularioCentralDeRiesgo.setValue('cuotaReal', response.data.edP_SECTOR_REAL)
                    formularioCentralDeRiesgo.setValue('totalObligaciones', response.data.edP_SECTOR_FINANCIERO + response.data.edP_SECTOR_REAL )
                    formularioCentralDeRiesgo.setValue(
                        'cuotaSinCanon',
                        Number(response.data.edP_ENDUEDAMIENTO_ACTUAL_CUOTA_PORC).toFixed(2)
                    );
                    
                    formularioCentralDeRiesgo.setValue(
                        'cuotaConCanon',
                        Number(response.data.edP_ENDUEDAMIENTO_ACTUAL_SIN_CUOTA_PORC).toFixed(2)
                    );            
                    formularioCentralDeRiesgo.setValue('ControlEndeudamiento', response.data.edP_CONTROL_NIVEL_ENDUEDAMIENTO_CUOTA_PORC)
                    formularioCentralDeRiesgo.setValue('scoring', response.data.edP_SCORE)
                    formularioCentralDeRiesgo.setValue('decisionHomologada', response.data.edP_DECISION_HOMOLOGADA)
                    formularioCentralDeRiesgo.setValue('disponibleLibranza', response.data.edP_DISPONIBLE_LIBRANZA)
                }
            })
            
        .catch (() => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        }) 
    }

    const obtenerSolicitud = async () => {
        const response = {
            ind: 2,
            soC_ID: procedureRequest?.requestID,
        }
        await
            CreditoApi
                .postCreditBasicData(response)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                            setResponseReport(response.data);
                        }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });

                });
    }

    const verArchivos = async () => {
        try {
            const item = obtenerRutaImagen
            setIsLoadingDos(true);
            const data = await CreditoApi.postVerArchivos(item);
            const ext = UtilService.getFileExtension(item);
            if (ext) {
                const mimeType = UtilService.getMimeTypeFromExtension(ext);
                if (mimeType) {
                    const file = new Blob([data], { type: mimeType });
                    const fileURL = URL.createObjectURL(file);
                    window.open(fileURL);
                }
            }
            setIsLoadingDos(false);
        } catch (e) {
            await NotificationService.genericAlert(
                'Warning',
                'No se puede generar archivo para este Usuario.',
                'warning',
            );
            setIsLoadingDos(false);
        }
    };


    

    // const obtenerProspeccionTransUnion = async (parametroObtenerSolicitud: any, dataSalario: any) => {
    //     
    //     const response = {
    //         SLS_ID: procedureRequest?.simulationID,
    //         tipoIdentificacion: parametroObtenerSolicitud.tiD_ID,
    //         identificacion: String(procedureRequest?.clientIdentificationNumber),
    //         telefono: procedureRequest?.cellphoneNumber.toString().replace("(", "").replace(") ", "").replace("-", ""),
    //         fechaExpedicionIdentificacion:  moment(parametroObtenerSolicitud?.soB_FECHA_EXPEDICION).format('YYYY-MM-DD') ,
    //         apellidos: parametroObtenerSolicitud?.soB_PRIMER_APELLIDO,
    //         salarioMensual: dataSalario,
    //         traerHistorialCrediticio: "Y"
    //     }

    //     await
    //         CreditoApi
    //             .postProspeccionTransUnion(response)
    //             .then(async (response: any) => {
    //                 if (response) {
    //                     if (response.isSuccess) {
    //                         setConsultaTransUnion(response.data);
    //                         setResponseReport(response.data.response);
    //                     }
    //                 }
    //             })
    //             .catch(() => {
    //                 toast('Error del sistema', { type: toast.TYPE.ERROR });
    //                 setIsLoadingDos(false)
    //             });
    // };

    const generarPdf = async () => {
        setIsLoadingDos(true);
        const request = {
            slS_ID: procedureRequest?.simulationID,
            tipoIdentificacion: responseReport.tiD_ID,
            identificacion: String(procedureRequest?.clientIdentificationNumber)
           }
        try {
            const response = await reportesApi.postReporteInformacionComercial(request);
            const mimeType = response.type;
            const file = new Blob([response], { type: mimeType });
            const fileURL = URL.createObjectURL(file);
            window.open(fileURL, '_blank');
            setIsLoadingDos(false);
        } catch (e) {
            console.log(e)
            setIsLoadingDos(false);
        }
    }


    if (isLoadingDos || isLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <>
            <CHPaper title="Control Endeudamiento Global">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="salarioTotal"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',

                                        },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Salario Total"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="descuentos"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',

                                        },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Descuentos"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4} >
                        <Controller
                            name="salarioNeto"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Salario Neto"
                                    value={value}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4} >
                        <Controller
                            name="capacidaPago"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                       },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Capacidad de Pago"
                                    value={value}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4} >
                        <Controller
                            name="canonCuota"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Canon / Cuota Mensual"
                                    value={value}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4} >
                        <Controller
                            name="disponibleLibranza"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Disponible Libranza"
                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4} >
                        <Controller
                            name="controlCapacidaPago"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    label="Control Capacidad de Pago"
                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                </Grid>

            </CHPaper>
            <CHPaper title="Control Endeudamineto Nómina">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}   >
                        <Controller
                            name="cuotaFinanciero"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Cuotas Sector Financiero/Solidario/Asegurador"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}   >
                        <Controller
                            name="cuotaReal"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Cuotas Sector Real"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}   >
                        <Controller
                            name="totalObligaciones"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    currencyMask
                                    label="Total Obligaciones"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}   >
                        <Controller
                            name="cuotaConCanon"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    percentMask
                                    label="Endeudamiento Global Sin Canon / Cuota"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}   >
                        <Controller
                            name="cuotaSinCanon"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    percentMask
                                    label="Endeudamiento Global Con Canon / Cuota"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}   >
                        <Controller
                            name="ControlEndeudamiento"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                  
                                    isReadOnly
                                    label="Control Nivel de Endeudamiento"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}   >
                        <Controller
                            name="scoring"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    label="Score"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}   >
                        <Controller
                            name="decisionHomologada"
                            control={formularioCentralDeRiesgo.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{
                                        style: {
                                            padding: '10px',
                                            textAlign: 'right',
                                        },
                                    }}
                                    isReadOnly
                                    label="decisión Homologada"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                </Grid>

            </CHPaper>

            <Grid item xs={12} md={4} style={{ marginBottom: "30px", marginTop: "20px" }} >
                <br />
                <Button
                    variant="contained"
                    size='large'
                    type="submit"
                    onClick={() => generarPdf()}
                
                    style={{ backgroundColor: '#a9a9a9', color: 'white', width: "100%" }}
                >
                    INFORMACIÓN COMERCIAL
                </Button>

                <br />
                <Button
                    variant="contained"
                    type="submit"
                    size='large'
                    onClick={()=>verArchivos()}
                    style={{ backgroundColor: '#a9a9a9', color: 'white', width: "100%", marginTop: "20px" }}
                >
                    REPORTE 80%.
                </Button>
            </Grid>

        </>
    )
}



export default CentralDeRiesgo
