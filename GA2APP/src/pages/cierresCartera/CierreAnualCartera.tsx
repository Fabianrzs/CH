import { Button, Card, CardContent, CircularProgress, createStyles, FormControlLabel, Grid,  makeStyles, Radio, RadioGroup } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import MUIDataTable from 'mui-datatables';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import CreditApi from '../../services/api-credit.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CHContainerSubtitle from '../../components/container-subtittle/container-subtittle';
import UtilService from '../../utils/util.service';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import NotificationService from '../../services/notification.service';
import ApiproductoClients from '../../services/api-producto.service';
import AuthService from '../../services/auth.service';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import { Auditoria } from '../../types/Auditoria.model.type';
import CatalogApi from '../../services/api-catalog.service';
import { useIp } from '../../shared/context/IpContext';

const useStyles = makeStyles(() =>
    createStyles({
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#43b5f1',
        },

        tablaCierre: {
            overflow: 'auto !important',
            maxHeight: '500px'
        },
    }),
);

function CierreTableAnual({
    data, mostrarTabla
}:
    {
        data: any[],
        mostrarTabla: any
    }) {


        const columns = [
            {
                name: 'numeroCredito',
                label: 'Número de Crédito',
                options: {
                    setCellHeaderProps: () => ({
                       className: 'filas'
                    }),
                },
            },
            {
                name: 'tipoIdentificacion',
                label: 'Tipo de Identificación',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'identificacion',
                label: 'Número de Identificación',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'nombreCompleto',
                label: 'Apellidos y nombres',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'unidadEjecutora',
                label: 'Unidad Ejecutora',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            // {
            //     name: 'categoria',
            //     label: 'Categoría',
            //     options: {
            //         setCellHeaderProps: () => ({
            //             className: 'filas'
            //         }),
            //     },
            // },
            // {
            //     name: 'grado',
            //     label: 'Grado',
            //     options: {
            //         setCellHeaderProps: () => ({
            //             className: 'filas'
            //         }),
            //     },
            // },
            {
                name: 'tipoCredito',
                label: 'Tipo Crédito',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'calificacionCartera',
                label: 'Calificación de Cartera',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'diasMora',
                label: 'Días Mora',
                options: {
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'saldoCapital',
                label: 'Saldo Capital',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'saldoCapitalTradicional',
                label: 'Saldo Capital Tradicional',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'acumuladoSeguroHogar',
                label: 'Seguro Hogar por pagar',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'acumuladoSeguroVida',
                label: 'Seguro Vida por pagar',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'provisionGenerica',
                label: 'Provisión Genérica',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'abonoSeguroHogar',
                label: 'Abono Seguro Hogar',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'abonoSeguroVida',
                label: 'Abono Seguro Vida',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'abonoCapital',
                label: 'Abono Capital',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'abonoInteresCorriente',
                label: 'Abono Interés Corriente',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'abonoInteresMora',
                label: 'Abono Interés Mora',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'abonoCapitalRemanente',
                label: 'Abono Capital Remanente',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
            {
                name: 'abonoInteresRemanente',
                label: 'Abono Interés Remanente',
                options: {
                    customBodyRender: (value: number | bigint) => UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
                },
            },
        ];

    return (

        mostrarTabla && (
            <MUIDataTable
                title=""
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    tableBodyHeight: '600px',
                    textLabels: {
                        body: {
                            noMatch: 'No se encontraron resultados',
                          },
                        pagination: {
                            displayRows: 'de',
                            next: 'Siguiente',
                            previous: 'Anterior',
                            rowsPerPage: 'Filas por página',
                            jumpToPage: '',
                        },
                        toolbar: {
                            downloadCsv: 'Descargar CSV',
                            search: 'Buscar',
                            viewColumns: 'Mostrar Columnas',
                        },
                        viewColumns: {
                            title: 'Mostrar Columnas',
                        },
                    },
                }}
            />
        )
    );
}

const CierreAnualCartera = () => {
   
    const creditApi = CreditApi();
    const ApiClients = ApiproductoClients();
    const auditoriaApi = AuditoriaApi();
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [dataCierreAnual, setDataCierreAnual] = useState<any[]>([]);
    const [mostrarTablaAnual, setMostrarTablaAnual] = useState(false);

    const classes = useStyles();
    
    const form = useForm({
        'mode': 'onChange',
        defaultValues: {
            fecha: moment(new Date()).subtract(1, 'year').format(),
            actualizacion: "0"
        },

    });

    const onSubmit = async (data: any) => {
        const inputFecha = moment(data.fecha);
        const user = AuthService.getUserInfo();
       
        if(!inputFecha.isValid()){
            return
        }

        const request = {
            anioCierre: moment(data.fecha).year(),
            actualiza: data.actualizacion === "1"
        }
        const requestSVN = {
            anioCierre: moment(data.fecha).year(),
            actualiza: true,
            creadoPor: user.USR_ID,

        }

        if(request.actualiza) {
            const result = await swal({
                text: 'Se realizará el cierre anual. ¿Desea continuar?',
                icon: 'warning',
                buttons: {
                    confirm: { text: 'SI', value: true, visible: true },
                    cancel: { text: 'NO', value: false, visible: true }
                },
            });

            if(result) {
                await ejecutarCierre(request);
                insertarSevenCierreAnual(requestSVN)
            }

        } else {
            await ejecutarCierre(request);
        }

    };

    const { publicIp } = useIp();


    const ejecutarCierre = async (request: any) => {
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
       
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIp,
            modulo:'Cierre mensual de cartera',
            hostname:document.location.host
        } as Auditoria
        try {
            
            setIsLocalLoading(true);
            const response = await creditApi.postCierreAnualCartera(request);

            if (response) {
                if (response.isSuccess) {
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Cierre anual  de  cartera parámetros:
                         \n Año:  ${request.anioCierre}
                         \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                    setDataCierreAnual(response.data)
                    setMostrarTablaAnual(true);
                } else {
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Cierre anual  de  cartera parámetros:
                         \n Año:  ${request.anioCierre}
                         \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                         \n returnApiMessage: ${response.returnMessage}`
                         await auditoriaApi.InsertAuditoria(requestAuditoria);
                    setMostrarTablaAnual(false)
                    NotificationService.genericAlertError(
                        'Ejecución cierre anual',
                        response?.returnMessage ?? `Error al ejecutar el cierre`
                    );
                }
            }

            return response;
        }
        catch (error:any) {
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion=8;
                requestAuditoria.descripcion=`Cierre anual  de  cartera parámetros:
                \n Año:  ${request.anioCierre}
                \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                \n Error:  ${error}
                \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                await auditoriaApi.InsertAuditoria(requestAuditoria);
                toast("Error del sistema", { type: toast.TYPE.ERROR });
               throw error;
        }
        finally {
            setIsLocalLoading(false);
        }
    }

    const insertarSevenCierreAnual= (data: any) => {
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
       
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIp,
            modulo:'Cierre mensual de cartera',
            hostname:document.location.host
        } as Auditoria
        setIsLocalLoading(true)
        ApiClients
        .postCierreAnualSeven(data)
        .then(response => {
            if(response.isSuccess) {
                const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                const errorTexto = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);

                if(retornoExitoso[1] === "0"){
                    toast('Insercion de movimiento EXITOSA', { type: toast.TYPE.SUCCESS });
                    requestAuditoria.tipoAccion='TRANSACCIONES';
                    requestAuditoria.idTipoAccion=2;
                    requestAuditoria.descripcion=`Inserción movimiento en seven EXITOSA. Cierre anual  de  cartera parámetros:
                         \n Año:  ${data.anioCierre}
                         \n Actualiza Seven:  ${data.actualiza?'Sí':'No'}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }else{
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Inserción movimiento en seven NO EXITOSA. Cierre anual  de  cartera parámetros:
                         \n Año:  ${data.anioCierre}
                         \n Actualiza Seven:  ${data.actualiza?'Sí':'No'}
                         \n Error:  ${errorTexto[1]}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                    toast(`Insercion de movimiento FALLO: ${errorTexto[1]}`, { type: toast.TYPE.ERROR, autoClose: 10000 });
                }
            }
        })
        .catch((error) => {
            if (error.response && error.response.data ) {
                requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                   requestAuditoria.descripcion=`Inserción movimiento en seven. Cierre anual  de  cartera parámetros:
                 \n fecha:  ${moment(data.fechaCierre).format("DD/MM/YYYY HH:mm")}
                 \n Actualiza Seven:  ${data.actualiza?'Sí':'No'}
                 \n Error:  ${error}
                 \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                  auditoriaApi.InsertAuditoria(requestAuditoria);
                toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR, autoClose: 10000 });
            } else {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }
        }).finally(() => setIsLocalLoading(false));
    }
        
    return (
        <div 
        style={{
            display: 'table',
            tableLayout: 'fixed',
            width: '100%',
            
        }}>
            <LoadingV2Component key="loader" loading={isLocalLoading} tittle='Cierre Anual...' />


            <PageIndicators
                topTextNivel1="Cierres Cartera"
                topTextNivel2="Cierre Anual"
            />
            <IconTitle
                title="Cierre"
                subTitle="Anual"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid>
                <Grid item xs={12}>
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <CHContainerSubtitle
                                        subTitle="de Datos"
                                        title="Actualización"
                                        icon="fch-Procedimientos-solicitudes-0-1"
                                    />
                                </Grid>

                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fecha"
                                        control={form.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                views={['year']}
                                                format="yyyy"
                                                label="Año"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={error?.message}
                                                invalidDateMessage="Fecha no válida"
                                            />                                          
                                        )}                                        
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="actualizacion"
                                        control={form.control}
                                        render={({
                                            field: { onChange, value },
                                        }) => (
                                            <RadioGroup
                                                value={value}
                                                onChange={onChange}
                                                row
                                                aria-label="actualización"
                                                defaultValue="0"
                                                name="row-radio-buttons-group"
                                            >
                                                <FormControlLabel
                                                    value="1"
                                                    control={
                                                        <Radio color="primary" />
                                                    }
                                                    label="DEFINITIVO"
                                                />
                                                <FormControlLabel
                                                    value="0"
                                                    control={
                                                        <Radio color="primary" />
                                                    }
                                                    label="SIMULADO"
                                                />
                                            </RadioGroup>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <br />
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <Grid className="buttonssolicitud">
                <Button
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={form.handleSubmit(onSubmit)}>
                    CIERRE
                </Button>
    
            </Grid>
            <br />
            <div className='MuiTableCell-stickyHeader; jss86'>
            <CierreTableAnual
                data={dataCierreAnual}
                mostrarTabla={mostrarTablaAnual} />
            </div>
           
        </div>
    );
};

export default CierreAnualCartera;
