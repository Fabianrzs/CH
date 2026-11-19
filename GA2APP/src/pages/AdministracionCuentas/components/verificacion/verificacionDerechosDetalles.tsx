import React, { useEffect, useState } from 'react';
import { Box, Button, Grid, makeStyles, Tab, Tabs, Typography, CircularProgress } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { Link, useHistory } from 'react-router-dom';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import UtilService from '../../../../utils/util.service';
import ApiVerificacionDerecho from '../../../../services/api-verificacion.service';
import CHTextFieldV2 from '../../../../components/ch-text-field/CHTextFieldV2';
import { ParametrosVerificacionDerechoDto } from '../../../../types/verificacion.model.type';
import AuthService from '../../../../services/auth.service';
import { LoadingComponent } from '../../../../components/loading/loading.component';

function CierreTable1({
    data, mostrarTabla
}:
    {
        data: any[],
        mostrarTabla: any,

    }) {
    const columns = [

        {
            name: 'estadoFinal',
            label: 'Estado Inicial',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'estadoInicial',
            label: 'Estado Final',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'motivoCambio',
            label: 'Motivo de Cambio',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'cuentaAfectada',
            label: 'Cuentas Afectadas',
            options: {
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
                    pagination: true,
                    rowsPerPage: 50,
                    rowsPerPageOptions: [50],
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

function CierreTableResult({
    data, mostrarTabla
}:

    {
        data: any[],
        mostrarTabla: any,

    }) {
    const columns = [

        {
            name: 'ctaNumero',
            label: 'CUENTA',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const ctaNumeroModificado = `${value}-0`;
                    return ctaNumeroModificado;
                }
            }
        },
        {
            name: 'identificacion',
            label: 'IDENTIFICACIÓN',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'estadoInicial',
            label: 'ESTADO INICIAL',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'estadoFinal',
            label: 'ESTADO FINAL',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'ctaCuotas',
            label: 'CUOTAS APORTADAS',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'cuotasPendientes',
            label: 'CUOTAS PENDIENTES',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'ctaFechaPrimerAporte',
            label: 'PRIMER APORTE',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),

            },
        },
        {
            name: 'fechaUltimoMovimientoCredito',
            label: 'ULTIMO APORTE',

            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),

            },
        },
        {
            name: 'nombreUnidadEjecutora',
            label: 'UNIDAD EJECUTORA',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'cambiarEstado',
            label: 'CAMBIA ESTADO',
            options: {
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

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
    },
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
    titleform: {
        width: '100%',
        textAlign: 'center',
        color: '#1C3F6B',
        padding: 1,
        fontWeight: 'bold',
        fontSize: '1rem',
    },
    column: {
        padding: '10px',
        textAlign: 'center'
    },
}));

function TabPanel(props: any) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`simple-tabpanel-${index}`}
            aria-labelledby={`simple-tab-${index}`}
            {...other}
        >
            {value === index && (
                <Box p={3}>
                    <Typography>{children}</Typography>
                </Box>
            )}
        </div>
    );
}

function a11yProps(index: any) {
    return {
        id: `simple-tab-${index}`,
        'aria-controls': `simple-tabpanel-${index}`,
    };
}

const VerificacionDetalles = (props: any) => {

    const Api = ApiVerificacionDerecho();
    const [value, setValue] = React.useState(0);
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const { rowData } = props.location.state;
    const [numeroRadicado, setNumeroRadicado] = useState(rowData.numeroRadicado || '');
    const [dataCierreMesInfo, setDataCierreMesInfo] = useState<any[]>([]);
    const [dataCierreMesResult, setDataCierreMesResult] = useState<any[]>([]);
    const [mostrarTablaInfo, setMostrarTablaInfo] = useState(false);
    const [mostrarTablaResult, setMostrarTablaResult] = useState(false);
    const [isButtonDisabled, setIsButtonDisabled] = useState(false);
    const history = useHistory();
    const user = AuthService.getUserInfo();
    const classes = useStyles();

    const aprobar = {
        aprobado: rowData.aprobado
    }
    const procesar = {
        estado: rowData.estado
    }

    const request = {
        anio: rowData.anio,
        mes: rowData.mes,
        UsuarioId: user.USR_ID,
        UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`,
        numeroRadicado: numeroRadicado,

    };

    const formularioParametrosGenerales = useForm<ParametrosVerificacionDerechoDto>({
        defaultValues: {
            id: rowData.id,
            anio: rowData.anio,
            mes: rowData.mes,
            fechaAprobacion: rowData.fechaAprobacion,
            fechaOperacion: rowData.fechaOperacion,
            totalCuentasVerificadas: 0,
            totalCambiosEstado: 0,
            usuario: rowData.usuario,
            aprobado: rowData.aprobado,
            numeroRadicado: rowData.numeroRadicado
        },
    });

    useEffect(() => {
        showDataInfo()
        showDataResult()
    }, [])

    const showDataInfo = async () => {
        setIsLocalLoading(true);
        await Api
            .ObtenerTodoVerificacionDerechosInfoByPeriodoFiltroRequest(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {
                            formularioParametrosGenerales.setValue('totalCambiosEstado', response?.data?.totalCuentasAfectadas);
                            const datos = [
                                { estadoFinal: 'VIGENTE', estadoInicial: 'MOROSA', motivoCambio: 'UNO O DOS MESES PENDIENTES DE APORTAR', cuentaAfectada: response.data.vigenteMorosa },
                                { estadoFinal: 'VIGENTE', estadoInicial: 'INACTIVA', motivoCambio: '', cuentaAfectada: response.data.vigenteInactiva },
                                { estadoFinal: 'VIGENTE', estadoInicial: 'PRECANCELADA', motivoCambio: '', cuentaAfectada: response.data.vigentePrecancelada },
                                { estadoFinal: 'VIGENTE', estadoInicial: 'CANCELADA', motivoCambio: 'CUENTA EN SALDO $0.00 Y MAS DE 12 MESES SIN APORTAR', cuentaAfectada: response.data.vigenteCancelada },
                                { estadoFinal: 'MOROSA', estadoInicial: 'VIGENTE', motivoCambio: '', cuentaAfectada: response.data.morosaVigente },
                                { estadoFinal: 'MOROSA', estadoInicial: 'INACTIVA', motivoCambio: 'MAS DE 3 MESES SIN APORTAR, HASTA 11 MESES', cuentaAfectada: response.data.morosaInactiva },
                                { estadoFinal: 'MOROSA', estadoInicial: 'PRECANCELADA', motivoCambio: '', cuentaAfectada: response.data.morosaPrecancelada },
                                { estadoFinal: 'MOROSA', estadoInicial: 'CANCELADA', motivoCambio: '', cuentaAfectada: response.data.morosaCancelada },
                                { estadoFinal: 'INACTIVA', estadoInicial: 'VIGENTE', motivoCambio: '', cuentaAfectada: response.data.inactivaVigente },
                                { estadoFinal: 'INACTIVA', estadoInicial: 'MOROSA', motivoCambio: '', cuentaAfectada: response.data.inactivaMorosa },
                                { estadoFinal: 'INACTIVA', estadoInicial: 'PRECANCELADA', motivoCambio: 'MAS DE 12 MESES SIN APORTAR', cuentaAfectada: response.data.inactivaPrecancelada },
                                { estadoFinal: 'INACTIVA', estadoInicial: 'CANCELADA', motivoCambio: '', cuentaAfectada: response.data.inactivaCancelada },
                                { estadoFinal: 'PRECANCELADA', estadoInicial: 'CANCELADA', motivoCambio: '', cuentaAfectada: response.data.suspendidaCancelada },

                            ];
                            setDataCierreMesInfo(datos)
                            setMostrarTablaInfo(true);
                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTablaInfo(false)
                        setIsLocalLoading(false);
                    }

                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });
    }

    const showDataResult = async () => {
        setIsLocalLoading(true);
        await Api
            .ObtenerTodoDatosVerificacionDerechosByPeriodosRequest(request)
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {
                            formularioParametrosGenerales.setValue('totalCuentasVerificadas', response?.data?.[0].totalCuenta);
                            const formattedData = response.data.map((item: any) => ({
                                ...item,
                                fechaUltimoMovimientoCredito: formatDateWithoutTime(
                                    item.fechaUltimoMovimientoCredito
                                ),
                                ctaFechaPrimerAporte: formatDateWithoutTime(
                                    item.ctaFechaPrimerAporte
                                ),
                            }));
                            setDataCierreMesResult(formattedData);
                            setMostrarTablaResult(true);
                            setIsLocalLoading(false);
                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTablaResult(false)
                        setIsLocalLoading(false);
                    }
                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });
    }

    const handleChange = (event: any, newValue: any) => {
        setValue(newValue);
    };

    const handleClick = async () => {
        setIsLocalLoading(true);
        await Api
            .ActualizarTodoEctCuentasRequest(request)
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {

                        if (response.isSuccess && response.data.estado === 2) {
                            toast(`Verificación de Derechos Procesada con éxito`, { type: toast.TYPE.SUCCESS });
                            setIsLocalLoading(false);
                            setIsButtonDisabled(true);
                            history.push('./verificacionDerechos');
                        }
                        if (response.isSuccess && response.data.estado === 3) {
                            toast(`Verificación de Derechos Aprobada con éxito`, { type: toast.TYPE.SUCCESS });
                            setIsLocalLoading(false);
                            setIsButtonDisabled(true);
                            history.push('./verificacionDerechos');
                        }
                    }
                    else {
                        toast(`Número de Radicado no Ingresado`, { type: toast.TYPE.ERROR });
                        setIsLocalLoading(false);
                    }

                }
            }).catch(() => {
                toast(`Error al procesar Verificación de derechos`, { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
                setIsButtonDisabled(true);
                history.push('./verificacionDerechos');
            });

        setIsLocalLoading(false);
    };

    function formatDateWithoutTime(date: any) {
        const dateObj = new Date(date);
        const year = dateObj.getFullYear().toString().padStart(4, '0');
        const month = (dateObj.getMonth() + 1).toString().padStart(2, '0');
        const day = dateObj.getDate().toString().padStart(2, '0');
        return `${day}-${month}-${year}`;
    }

    return (
        <>
            <LoadingComponent loading={isLocalLoading} />
            <div
                style={{
                    display: 'table',
                    tableLayout: 'fixed',
                    width: '100%',

                }}>
                <div className={classes.root}>
                    <Tabs value={value} onChange={handleChange} aria-label="simple tabs example">
                        <Tab label="Información de Entrada" {...a11yProps(0)} />
                        <Tab label="Información Resultado" {...a11yProps(1)} />
                        <Tab label="Reportes" {...a11yProps(2)} />
                    </Tabs>
                    <TabPanel value={value} index={0}>
                        Información de Entrada
                        <div className='MuiTableCell-stickyHeader; jss86'>
                            <Grid container>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="fechaOperacion"
                                        control={formularioParametrosGenerales.control}
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextFieldV2
                                                datatype=''
                                                isReadOnly
                                                label="Fecha de Operación"
                                                style={{
                                                    marginLeft: '10px',
                                                }}
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                value={UtilService.getFullDate(value)}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="fechaAprobacion"
                                        control={formularioParametrosGenerales.control}
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextFieldV2
                                                datatype=''
                                                isReadOnly
                                                label="Fecha de Aprobación"
                                                style={{
                                                    marginLeft: '10px',
                                                }}
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                value={UtilService.getFullDate(value)}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="totalCuentasVerificadas"
                                        control={formularioParametrosGenerales.control}
                                        render={({
                                            field: { onChange, value },
                                        }) => (
                                            <CHTextFieldV2
                                                numberMask
                                                isReadOnly
                                                label="Total Cuentas Verificadas"
                                                style={{
                                                    marginLeft: '10px',
                                                }}
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                value={value}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="totalCambiosEstado"
                                        control={formularioParametrosGenerales.control}
                                        render={({
                                            field: { onChange, value },
                                        }) => (
                                            <CHTextFieldV2
                                                numberMask
                                                isReadOnly
                                                label="Total Cambios de Estado"
                                                style={{
                                                    marginLeft: '10px',
                                                }}
                                                InputLabelProps={{ shrink: true }}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                                value={value}
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="numeroRadicado"
                                        control={formularioParametrosGenerales.control}
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <Grid container justify="space-between">
                                                <Grid item>
                                                    <CHTextFieldV2
                                                        isReadOnly={false}
                                                        label="Número Radicado"
                                                        style={{
                                                            marginLeft: '10px',
                                                        }}
                                                        InputLabelProps={{ shrink: true }}
                                                        inputProps={{
                                                            style: {
                                                                textAlign: 'left',
                                                            },
                                                        }}
                                                        value={value}

                                                        onChange={(e) => {
                                                            const inputValue = e.target.value;
                                                            const formatoCorrectoRegex = /^(\d+-){0,2}\d*$/;

                                                            if (formatoCorrectoRegex.test(inputValue)) {
                                                                onChange(e);
                                                                setNumeroRadicado(inputValue);
                                                            }
                                                        }}
                                                    />
                                                </Grid>
                                            </Grid>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        </div>
                        <Grid className="buttonssolicitud">
                            <Link to={{ pathname: './verificacionDerechos' }}>
                                <Button
                                    className={classes.buttonprincipal}
                                    variant="outlined"
                                >
                                    REGRESAR
                                </Button>
                            </Link>
                        </Grid>
                    </TabPanel>
                    <TabPanel value={value} index={1}>
                        Información Verificación Derechos
                        <div className='MuiTableCell-stickyHeader; jss86'>
                            <CierreTable1
                                data={dataCierreMesInfo}
                                mostrarTabla={mostrarTablaInfo}
                            />
                        </div>
                        <Grid className="buttonssolicitud">
                            <Link to={{ pathname: './verificacionDerechos' }}>
                                <Button
                                    className={classes.buttonprincipal}
                                    variant="outlined"
                                >
                                    REGRESAR
                                </Button>
                            </Link>
                        </Grid>
                    </TabPanel>
                    <TabPanel value={value} index={2}>
                        Reportes Verificación Derechos
                        <div className='MuiTableCell-stickyHeader; jss86'>
                            <CierreTableResult
                                data={dataCierreMesResult}
                                mostrarTabla={mostrarTablaResult}
                            />
                        </div>
                        <Grid className="buttonssolicitud">
                            <Button
                                className={classes.buttonprincipal}
                                disabled={isButtonDisabled || aprobar.aprobado !== null}
                                onClick={handleClick}
                            >
                                {procesar.estado === 1 ? 'PROCESAR' : 'APROBAR'}
                            </Button>
                            <Link to={{ pathname: './verificacionDerechos' }}>
                                <Button
                                    className={classes.buttonprincipal}
                                    variant="outlined"
                                >
                                    REGRESAR
                                </Button>
                            </Link>
                        </Grid>
                    </TabPanel>
                </div>
            </div>
        </>
    );
};

export default VerificacionDetalles;