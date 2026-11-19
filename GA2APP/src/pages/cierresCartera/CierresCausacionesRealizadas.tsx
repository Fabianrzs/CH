import { Button, Card, CardContent, Checkbox, createStyles, Grid, ListItemText, makeStyles, MenuItem, OutlinedInput, Select } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable from 'mui-datatables';
import moment from 'moment';
import { toast } from 'react-toastify';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../components/ch-select/CHSelect';
import Header from '../../components/header/Header';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CreditApi from '../../services/api-credit.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import UtilService from '../../utils/util.service';  
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import AuthService from '../../services/auth.service';
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
        columnDate: {
            minWidth: '110px',
        },

        tablaCierre: {
            overflow: 'auto !important'
        },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#43b5f1',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
        },
    }),
);



function CierreTable({data, mostrarTabla}: {data: any[], mostrarTabla: any}) {

    const classes = useStyles();

    const columns = [
        {
            name: 'fechaCierre',
            label: 'Fecha Cierre',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },
                    
                }),
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
               
            },
        
        },
    
        {
            name: 'fechaEjecucion',
            label: 'Fecha Ejecución',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },
                    
                }),
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
               
            },
        
        },

        {
            name: 'nombreTipoCierre',
            label: 'Tipo Cierre',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
    ];

    return mostrarTabla && (
        <MUIDataTable
            title=""
            data={data}
            columns={columns}

            options={{
                selectableRows: 'none',
                filter: false,
                print: false,
                elevation: 2,
                filterType: 'dropdown',
                responsive: 'vertical',
                fixedHeader: true,
                fixedSelectColumn: true,
                tableBodyHeight: '470px',
                rowsPerPageOptions: [5, 15, 20, 100, 200, 500 ],
                textLabels: {
                    body: {
                        noMatch: 'No se encontraron resultados',
                    },
                    pagination: {
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
    );
}

const CierresCausacionesRealizadas = () => {

    const classes = useStyles();
    const creditApi = CreditApi();
    const auditoriaApi = AuditoriaApi();
    const [tiposCierre, setTipoCierre] = useState<any[]>([]);
    const [isProccessing, setProcessing] = useState(false);
    const [tableVisible, setTableVisible] = useState(false);
    const [data, setData] = useState([]);
    console.log(new Date());

    useEffect(() => {
        creditApi.getObtenerTiposDeCierreCartera().then(x => {
            setTipoCierre(x.data);
            console.log(tiposCierre);
        });
    }, []);
    
    const defaultValues: {
        fechaCierreInicial: Date,
        fechaCierreFinal: Date,
        tiposCierre: string[]
    } = {
        fechaCierreInicial:  new Date(),
        fechaCierreFinal: new Date(),
        tiposCierre: []
    };

    const formControls = useForm({
        'mode': 'onChange',
        defaultValues: defaultValues,
    });

    const { publicIp } = useIp();

    const onSubmit = (request: any) => {
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
        const fechaCierreInicial = moment(request.fechaCierreInicial);
        const fechaCierreFinal = moment(request.fechaCierreFinal);

        const invalidFechaInicial = request.fechaCierreInicial != null && !fechaCierreInicial.isValid();
        const invalidFechaFinal = request.fechaCierreFinal != null && !fechaCierreFinal.isValid();

        if(invalidFechaInicial || invalidFechaFinal) {
            return;
        }
        

        setProcessing(true);
        setTableVisible(false);
        const payload = {
            tiposCierre: request.tiposCierre,
            fechaCierreInicial: fechaCierreInicial.isValid() ? fechaCierreInicial.startOf('day').toISOString() : null,
            fechaCierreFinal: fechaCierreFinal.isValid() ? fechaCierreFinal.startOf('day').toISOString() : null
        }
        console.log(payload);
        debugger
        const tiposCierres= ()=>
            payload.tiposCierre.map((item: any)=>(`${tiposCierre.find((y:any)=>y.idTipo===item).nombre}`)
        );
        creditApi.ObtenerCierresCausacionesRealizadas(payload)
        .then(x => {
            requestAuditoria.tipoAccion='CONSULTAS';
            requestAuditoria.idTipoAccion=3;
            requestAuditoria.descripcion=`Obtener cierres causiones realizadas parámetros: 
            \n Tipo de cierre:  ${tiposCierres()}
            \n Fecha inicial:  ${moment(payload.fechaCierreInicial).format("DD/MM/YYYY HH:mm")}
            \n Fecha final:  ${moment(payload.fechaCierreFinal).format("DD/MM/YYYY HH:mm")}`
            auditoriaApi.InsertAuditoria(requestAuditoria);
            setData(x.data);
            setTableVisible(true);
        })
        .finally(() => setProcessing(false)).catch((error:any)=>{
            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion=8;
            requestAuditoria.descripcion=`Obtener cierres causiones realizadas parámetros: 
            \n Tipo de cierre:  ${tiposCierres()}          
            \n Fecha inicial:  ${moment(payload.fechaCierreInicial).format("DD/MM/YYYY HH:mm")}
            \n Fecha final:  ${moment(payload.fechaCierreFinal).format("DD/MM/YYYY HH:mm")}
            \n Error:  ${error}
            \n returnApiMessage: ${error?.response?.data?.returnMessage}`
            auditoriaApi.InsertAuditoria(requestAuditoria);
        });
    };

    return (<div  
        style={{
            display: 'table',
            tableLayout: 'fixed',
            width: '100%',
            
        }}>
         <Header />
         <CHPrivateContainer>
            <PageIndicators
                topTextNivel1="Cierres Cartera"
                topTextNivel2="Cierres y causaciones realizadas"
            />
            <IconTitle
                title="Cierres y causaciones"
                subTitle="realizadas"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid container>
                <Grid item xs={12} >
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaCierreInicial"
                                        control={formControls.control}
                                        rules={{
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                error={!!error}
                                                label="Fecha de cierre inicial"
                                                value={value}
                                                onChange={onChange}
                                            />                                          
                                        )}                                        
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaCierreFinal"
                                        control={formControls.control}
                                        rules={{
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                error={!!error}
                                                label="Fecha de cierre final"
                                                value={value}
                                                onChange={onChange}
                                            />                                          
                                        )}                                        
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="tiposCierre"
                                        control={formControls.control}
                                        rules={{
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                            label="Tipo de cierres"
                                            value={value}
                                            onChange={onChange}
                                            renderValue={(selected: any) => selected.map((value: any) => tiposCierre.find(x => x.idTipo === value).nombre).join(', ')}
                                            multiple>
                                             {
                                                tiposCierre.map((item) => (
                                                    <MenuItem key={item.idTipo} value={item.idTipo}>
                                                        <Checkbox checked={value.indexOf(item.idTipo) > -1} color="primary" />
                                                        <ListItemText primary={item.nombre} />
                                                    </MenuItem>
                                                ))
                                            }
                                        </CHSelect>
                                                                                   
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
                    type='submit'
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={formControls.handleSubmit(onSubmit)}>
                    Buscar
                </Button>
            </Grid>
            <br />
            {
                isProccessing && <CHLoadingProcess />
            }
            <div className='MuiTableCell-stickyHeader; jss86'>
                <CierreTable data={data} mostrarTabla={tableVisible}/>
            </div>
            </CHPrivateContainer>
        </div>)
}

export default CierresCausacionesRealizadas;