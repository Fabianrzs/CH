import React, { useEffect, useState } from 'react';
import { Box, Button, Grid, makeStyles, Tab, Tabs, Typography, CircularProgress, Paper, Table, TableCell, TableContainer, TableRow } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { Link, useHistory } from 'react-router-dom';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';
import UtilService from '../../../../utils/util.service';
import ApiVerificacionDerecho from '../../../../services/api-verificacion.service';
import CHTextFieldV2 from '../../../../components/ch-text-field/CHTextFieldV2';
import { ParametrosAbonosInteresDto } from '../../../../types/verificacion.model.type';
import AuthService from '../../../../services/auth.service';

function CierreTable1({
    data, mostrarTabla
}:
    {
        data: any[],
        mostrarTabla: any,

    }) {
    const columns = [

        {
            name: 'tipoCuenta',
            label: 'TIPO CUENTA',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'concepto',
            label: 'CONCEPTO',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'totalBase',
            label: 'TOTAL BASE',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
        {
            name: 'abonadoPeriodo',
            label: 'INTERÉS ABONADOS EN EL PERIODO',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
        {
            name: 'noAbonadoPeriodo',
            label: 'INTERÉS NO ABONADO EN EL PERIODO',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
        {
            name: 'totalReconocidoPeriodo',
            label: 'TOTAL INTERESES RECONOCIDOS EN EL PERIODO',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
        {
            name: 'noAbonadosSaldoInsuficiente',
            label: 'INTERÉS NO ABONADOS POR SALDO INSUFICIENTE (IPC NEGATIVO)',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
    ];

    const sumas = {
        tipoCuenta: '',
        concepto: 'TOTAL',
        totalBase: 0,
        abonadoPeriodo: 0,
        noAbonadoPeriodo: 0,
        totalReconocidoPeriodo: 0,
        noAbonadosSaldoInsuficiente: 0,
    };

    data.forEach((row) => {
        sumas.totalBase += row.totalBase || 0;
        sumas.abonadoPeriodo += row.abonadoPeriodo || 0;
        sumas.noAbonadoPeriodo += row.noAbonadoPeriodo || 0;
        sumas.totalReconocidoPeriodo += row.totalReconocidoPeriodo || 0;
        sumas.noAbonadosSaldoInsuficiente += row.noAbonadosSaldoInsuficiente || 0;
    });

    sumas.totalBase = parseFloat(sumas.totalBase.toFixed(2));
    sumas.abonadoPeriodo = parseFloat(sumas.abonadoPeriodo.toFixed(2));
    sumas.noAbonadoPeriodo = parseFloat(sumas.noAbonadoPeriodo.toFixed(2));
    sumas.totalReconocidoPeriodo = parseFloat(sumas.totalReconocidoPeriodo.toFixed(2));
    sumas.noAbonadosSaldoInsuficiente = parseFloat(sumas.noAbonadosSaldoInsuficiente.toFixed(2));
    const dataConSumas = [...data, sumas];

    return (

        mostrarTabla && (
            <MUIDataTable
                title={
                    <h6 style={{ textAlign: 'right' }}>
                        CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICÍA <br />
                        GRUPO DE ADMINISTRACIÓN DE CUENTAS INDIVIDUALES Y CESANTÍAS <br />
                        LIQUIDACIÓN DE INTERESES <br />
                        ABONAR INTERESES SOLUCIÓN DE VIVIENDA
                    </h6>
                }
                data={dataConSumas}
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
                    customFooter: () => {
                        const sums = [
                            null,
                            data.reduce((acc, row) => acc + parseFloat(row.totalBase), 0).toFixed(2),
                            data.reduce((acc, row) => acc + parseFloat(row.abonadoPeriodo), 0).toFixed(2),
                            data.reduce((acc, row) => acc + parseFloat(row.noAbonadoPeriodo), 0).toFixed(2),
                            data.reduce((acc, row) => acc + parseFloat(row.totalReconocidoPeriodo), 0).toFixed(2),
                            data.reduce((acc, row) => acc + parseFloat(row.noAbonadosSaldoInsuficiente), 0).toFixed(2),
                        ];

                        return [

                        ];
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
            name: 'unidadEjecutora',
            label: 'UNIDAD EJECUTORA',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'totalSolucionVivienda',
            label: 'ABONAR INTERESES SOLUCIÓN DE VIVIENDA (MONTO)',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {

                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
        {
            name: 'totaAdministracionCesantias',
            label: 'ABONAR INTERESES CESANTÍAS(MONTO)',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
        {
            name: 'totaAVAV',
            label: 'ABONAR INTERESES AVAV(MONTO)',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
        {
            name: 'totaAVC',
            label: 'ABONAR INTERESES AVC(MONTO)',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
        {
            name: 'totaAVAC',
            label: 'ABONAR INTERESES AVAC(MONTO)',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    const formattedValue = parseFloat(value).toLocaleString('es-CO');
                    const ctaNumeroModificado = `$${formattedValue}`;
                    return ctaNumeroModificado;
                }
            },
        },
    ];

    const sumas = {
        unidadEjecutora: 'TOTAL',
        totalSolucionVivienda: 0.00,
        totaAdministracionCesantias: 0.00,
        totaAVAV: 0.00,
        totaAVC: 0.00,
        totaAVAC: 0.00,
    };

    data.forEach((row) => {
        sumas.totalSolucionVivienda += row.totalSolucionVivienda || 0;
        sumas.totaAdministracionCesantias += row.totaAdministracionCesantias || 0;
        sumas.totaAVAV += row.totaAVAV || 0;
        sumas.totaAVC += row.totaAVC || 0;
        sumas.totaAVAC += row.totaAVAC || 0;
    });

    sumas.totalSolucionVivienda = parseFloat(sumas.totalSolucionVivienda.toFixed(2));
    sumas.totaAdministracionCesantias = parseFloat(sumas.totaAdministracionCesantias.toFixed(2));
    sumas.totaAVAV = parseFloat(sumas.totaAVAV.toFixed(2));
    sumas.totaAVC = parseFloat(sumas.totaAVC.toFixed(2));
    sumas.totaAVAC = parseFloat(sumas.totaAVAC.toFixed(2));

    const dataConSumas = [...data, sumas];

    return (

        mostrarTabla && (
            <MUIDataTable
                title=""
                data={dataConSumas}
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
                        toolbar: {
                            downloadCsv: 'Descargar CSV',
                            search: 'Buscar',
                            viewColumns: 'Mostrar Columnas',
                        },

                        viewColumns: {
                            title: 'Mostrar Columnas',
                        },
                    },

                    customFooter: () => {
                        const sums = [
                            null,
                            data.reduce((acc, row) => acc + parseFloat(row.interesSolucionVivienda), 0).toFixed(2),
                            data.reduce((acc, row) => acc + parseFloat(row.interesSesantias), 0).toFixed(2),
                            data.reduce((acc, row) => acc + parseFloat(row.interesAVAV), 0).toFixed(2),
                            data.reduce((acc, row) => acc + parseFloat(row.interesAVC), 0).toFixed(2),
                            data.reduce((acc, row) => acc + parseFloat(row.interesAVAC), 0).toFixed(2),
                        ];

                        return [

                        ];
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

const abonoInteresesDetalles = (props: any) => {

    const classes = useStyles();
    const Api = ApiVerificacionDerecho();
    const [value, setValue] = React.useState(0);
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [dataCierreMesInfo, setDataCierreMesInfo] = useState<any[]>([]);
    const [dataCierreMesResult, setDataCierreMesResult] = useState<any[]>([]);
    const [mostrarTablaInfo, setMostrarTablaInfo] = useState(false);
    const [mostrarTablaResult, setMostrarTablaResult] = useState(false);
    const [isButtonDisabled, setIsButtonDisabled] = useState(false);

    const user = AuthService.getUserInfo();
    const { rowData } = props.location.state;
    const history = useHistory();

    const aprobar = {
        aprobado: rowData.aprobado
    }
    const procesar = {
        estado: rowData.estado
    }

    interface Datos {
        tipoCuenta: string;
        concepto: string;
        totalBase: number;
        abonadoPeriodo: number;
        noAbonadoPeriodo: number;
        totalReconocidoPeriodo: number;
        noAbonadosSaldoInsuficiente: number;
    }

    const showDataInfo = async () => {

        const request = {
            anio: rowData.anio,
            mes: rowData.mes,
            UsuarioId: user.USR_ID,
            UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`
        };

        await Api
            .ObtenerConsolidadoFiltroByPeriodoRequest(request)
            .then(async (response: any) => {
                if (response && response.data !== null && response.isSuccess) {
                    const valoresInteres: Datos[] = response.data.map((item: any) => {

                        if (item.id === 1) {
                            return {
                                tipoCuenta: "ABONAR INTERESES AVAC",
                                concepto: "INTERESES AHORRO AVAC",
                                totalBase: item.totalBase,
                                abonadoPeriodo: item.totaInteresAbonadoPeriodo,
                                noAbonadoPeriodo: item.totaInteresNoAbonadoPeriodo,
                                totalReconocidoPeriodo: item.totaInteresReconocidoPeriodo,
                                noAbonadosSaldoInsuficiente: item.totaInteresIPCNegativo,
                            };
                        }
                        if (item.id === 2) {
                            return {
                                tipoCuenta: "ABONAR INTERESES AVAV",
                                concepto: "INTERESES AHORRO AVAV",
                                totalBase: item.totalBase,
                                abonadoPeriodo: item.totaInteresAbonadoPeriodo,
                                noAbonadoPeriodo: item.totaInteresNoAbonadoPeriodo,
                                totalReconocidoPeriodo: item.totaInteresReconocidoPeriodo,
                                noAbonadosSaldoInsuficiente: item.totaInteresIPCNegativo,
                            };
                        }
                        if (item.id === 3) {
                            return {
                                tipoCuenta: "ABONAR INTERESES CESANTIAS",
                                concepto: "INTERESES CESANTIAS",
                                totalBase: item.totalBase,
                                abonadoPeriodo: item.totaInteresAbonadoPeriodo,
                                noAbonadoPeriodo: item.totaInteresNoAbonadoPeriodo,
                                totalReconocidoPeriodo: item.totaInteresReconocidoPeriodo,
                                noAbonadosSaldoInsuficiente: item.totaInteresIPCNegativo,
                            };
                        }
                        if (item.id === 4) {
                            return {
                                tipoCuenta: "ABONAR INTERESES SOLUCION DE VIVIENDA",
                                concepto: "INTERESES APORTES",
                                totalBase: item.totalBase,
                                abonadoPeriodo: item.totaInteresAbonadoPeriodo,
                                noAbonadoPeriodo: item.totaInteresNoAbonadoPeriodo,
                                totalReconocidoPeriodo: item.totaInteresReconocidoPeriodo,
                                noAbonadosSaldoInsuficiente: item.totaInteresIPCNegativo,
                            };
                        }
                        if (item.id === 5) {
                            return {
                                tipoCuenta: "ABONAR INTERESES SOLUCION DE VIVIENDA",
                                concepto: "INTERESES CESANTIAS",
                                totalBase: item.totalBase,
                                abonadoPeriodo: item.totaInteresAbonadoPeriodo,
                                noAbonadoPeriodo: item.totaInteresNoAbonadoPeriodo,
                                totalReconocidoPeriodo: item.totaInteresReconocidoPeriodo,
                                noAbonadosSaldoInsuficiente: item.totaInteresIPCNegativo,
                            };
                        }
                        if (item.id === 6) {
                            return {
                                tipoCuenta: "ABONAR INTERESES AVC",
                                concepto: "INTERESES AHORRO AVC",
                                totalBase: item.totalBase,
                                abonadoPeriodo: item.totaInteresAbonadoPeriodo,
                                noAbonadoPeriodo: item.totaInteresNoAbonadoPeriodo,
                                totalReconocidoPeriodo: item.totaInteresReconocidoPeriodo,
                                noAbonadosSaldoInsuficiente: item.totaInteresIPCNegativo,
                            };
                        }
                        return null;
                    });

                    const valoresInteresFiltered = valoresInteres.filter((item: Datos | null) => item !== null);

                    setDataCierreMesInfo(valoresInteresFiltered);
                    setMostrarTablaInfo(true);
                    setIsLocalLoading(false);
                } else {
                    toast("No se encontraron resultados", { type: toast.TYPE.ERROR });
                    setMostrarTablaInfo(false);
                    setIsLocalLoading(false);
                }
            })
            .catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });
    }

    const showVerificacionDerechos = async () => {

        const request = {
            anio: rowData.anio,
            mes: rowData.mes,
            UsuarioId: user.USR_ID,
            UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`
        };

        await Api
            .ObtenerTodoVerificacionDerechoByPeriodoRequest(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.data !== null) {
                        if (response.data.aprobado === null) {
                            toast(`La solicitud de Verificación de Derechos para el periodo está pendiente por aprobación`, { type: toast.TYPE.ERROR });
                            setIsLocalLoading(false);
                            setIsButtonDisabled(true);
                        }
                        else {

                            toast(`La solicitud de Verificación de Derechos para el periodo ya fue aprobada`, { type: toast.TYPE.SUCCESS });
                            setIsLocalLoading(false);
                        }
                    } else {

                        toast(`La solicitud de Verificación de Derechos para el periodo ya fue aprobada`, { type: toast.TYPE.SUCCESS });
                        setIsLocalLoading(false);
                    }
                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });
    }

    const showDataResult = async () => {

        interface AbonosInteresesDatos {
            unidadEjecutora: string;
            totalSolucionVivienda: number;
            totaAdministracionCesantias: number;
            totaAVAV: number;
            totaAVC: number;
            totaAVAC: number;
        }

        const request = {
            anio: rowData.anio,
            mes: rowData.mes,
            UsuarioId: user.USR_ID,
            UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`
        };
        await Api
            .ObtenerInformacionAbonoInteresUnidadEjecutoraFiltroByPeriodoRequest(request)
            .then(async (response: any) => {
                if (response && response.data !== null && response.isSuccess) {

                    const valoresInteresDatos: AbonosInteresesDatos[] = response.data.map((item: any) => {

                        if (item.id === 10) {
                            return {
                                unidadEjecutora: "ARMADA NACIONAL",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 17) {
                            return {
                                unidadEjecutora: "CAJA DE RETIRO DE LAS FFMM",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 2) {
                            return {
                                unidadEjecutora: "CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICÍA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 12) {
                            return {
                                unidadEjecutora: "CAJA SUELDOS DE RETIRO POLICÍA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 9) {
                            return {
                                unidadEjecutora: "COMANDO GENERAL FFMM",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 24) {
                            return {
                                unidadEjecutora: "DIRECCIÓN DE VETERANOS Y REHABILITACIÓN INCLUSIVA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 21) {
                            return {
                                unidadEjecutora: "DIRECCIÓN EJECUTIVA DE LA JUSTICIA PENAL MILITAR",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 20) {
                            return {
                                unidadEjecutora: "DIRECCIÓN GENERAL MARÍTIMA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 15) {
                            return {
                                unidadEjecutora: "DIRECCIÓN GENERAL SANIDAD ARMADA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }

                        if (item.id === 14) {
                            return {
                                unidadEjecutora: "DIRECCIÓN GENERAL SANIDAD EJÉRCITO",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 16) {
                            return {
                                unidadEjecutora: "DIRECCIÓN GENERAL SANIDAD FUERZA AÉREA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 13) {
                            return {
                                unidadEjecutora: "DIRECCIÓN GENERAL SANIDAD MILITAR",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 6) {
                            return {
                                unidadEjecutora: "DIRECCIÓN SANIDAD POLICÍA NACIONAL",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 1) {
                            return {
                                unidadEjecutora: "EJÉRCITO NACIONAL DE COLOMBIA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 11) {
                            return {
                                unidadEjecutora: "FUERZA ÁEREA DE COLOMBIA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }

                        if (item.id === 7) {
                            return {
                                unidadEjecutora: "GABINETE",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 8) {
                            return {
                                unidadEjecutora: "PENSIONADOS FFMM",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 5) {
                            return {
                                unidadEjecutora: "PENSIONADOS POLICÍA NACIONAL",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 22) {
                            return {
                                unidadEjecutora: "POLICÍA BIENESTAR",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 3) {
                            return {
                                unidadEjecutora: "POLICÍA NACIONAL",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 19) {
                            return {
                                unidadEjecutora: "SOLDADOS ARMADA NACIONAL",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }
                        if (item.id === 18) {
                            return {
                                unidadEjecutora: "SOLDADOS EJÉRCITO NACIONAL DE COLOMBIA",
                                totalSolucionVivienda: item.totalSolucionVivienda,
                                totaAdministracionCesantias: item.totaAdministracionCesantias,
                                totaAVAV: item.totaAVAV,
                                totaAVC: item.totaAVC,
                                totaAVAC: item.totaAVAC,
                            };
                        }

                        return null;
                    });

                    const valoresInteresFiltered = valoresInteresDatos.filter((item: AbonosInteresesDatos | null) => item !== null);
                    const dataConSumasOrdenada = valoresInteresFiltered.sort((a, b) => a.unidadEjecutora.localeCompare(b.unidadEjecutora));

                    setDataCierreMesResult(dataConSumasOrdenada);
                    setMostrarTablaResult(true);
                    setIsLocalLoading(false);
                } else {
                    toast("No se encontraron resultados", { type: toast.TYPE.ERROR });
                    setMostrarTablaResult(false);
                    setIsLocalLoading(false);
                }
            })
            .catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });
    }

    const formularioParametrosGenerales = useForm<ParametrosAbonosInteresDto>({
        defaultValues: {
            anio: rowData.anio,
            mes: rowData.mes,
            fechaAprobacion: rowData.fechaAprobacion,
            fechaRecepcion: rowData.fechaRecepcion,
            valorIpc: rowData.valorIpc,
            porcentajeIpc: rowData.porcentajeIpc,
            totalInteresesAbonados: rowData.totalInteresesAbonados,
            comprobanteSolucionViviendaAportes: rowData.comprobanteSolucionViviendaAportes,
            comprobanteSolucionViviendaCesantias: rowData.comprobanteSolucionViviendaCesantias,
            comprobanteSolucionAdministracionCesantias: rowData.comprobanteSolucionAdministracionCesantias,
            comprobanteAVAC: rowData.comprobanteAVAC,
            comprobanteAVAV: rowData.comprobanteAVAV,
            comprobanteAVC: rowData.comprobanteAVC,
            numeroRadicado: rowData.numeroRadicado
        },
    });

    useEffect(() => {
        showVerificacionDerechos()
        showDataInfo()
        showDataResult()
    }, [])

    if (isLocalLoading) {
        return <CHLoadingProcess />;
    }

    const handleChange = (event: any, newValue: any) => {
        setValue(newValue);
    };

    const handleClick = async () => {

        setIsLocalLoading(true);

        const InsertarMovimientos = {
            anio: rowData.anio,
            mes: rowData.mes,
            fechaRecepcion: new Date(),
            fechaAprobacion: new Date(),
            valorIpc: 0,
            porcentajeIpc: 0,
            totalInteresesAbonados: 0,
            comprobanteSolucionViviendaAportes: "",
            comprobanteSolucionViviendaCesantias: "",
            comprobanteSolucionAdministracionCesantias: "",
            comprobanteAVAC: "",
            comprobanteAVAV: "",
            comprobanteAVC: "",
            numeroRadicado: rowData.numeroRadicado,
            aprobado: 1,
            UsuarioId: user.USR_ID,
            UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`,
            estado: 2,
            nombreEstado: "Pendiente por aprobar",
        };

        let mostrarMensaje = false;
        await Api
            .ActualizarParametrosInteresAbonadosMovimientosRequest(InsertarMovimientos)
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {
                        if (response.data.estado === 2 && response.data.aprobado === null) {
                            mostrarMensaje = false;
                            toast(`El procesamiento de abono de intereses fue satisfactoria`, { type: toast.TYPE.SUCCESS });
                            setIsLocalLoading(false);
                            setIsButtonDisabled(true);
                        }
                        else if (response.data.aprobado === null) {
                            mostrarMensaje = false;
                            toast(`La aplicación de los movimientos no fue satisfactoria`, { type: toast.TYPE.ERROR });
                            setIsLocalLoading(false);
                            setIsButtonDisabled(true);
                        }
                        else {
                            mostrarMensaje = true;
                            toast(`La aplicación de los movimientos fue satisfactoria, Abonos de intereses aprobado`, { type: toast.TYPE.SUCCESS });
                            setIsLocalLoading(false);

                        }
                    } else {
                        toast(`La aplicación de los movimientos no fue satisfactoria`, { type: toast.TYPE.SUCCESS });
                        setIsLocalLoading(false);
                    }

                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });

        history.push('./abonoIntereses', {
            showSuccessMessage: mostrarMensaje
        });
    };

    return (
        <div
            style={{
                display: 'table',
                tableLayout: 'fixed',
                width: '100%',

            }}>

            <div className={classes.root}>
                <Tabs value={value} onChange={handleChange} aria-label="simple tabs example">
                    <Tab label="INFORMACIÓN ABONOS DE INTERÉS" {...a11yProps(0)} />
                    <Tab label="REPORTE CONSOLIDADO POR CUENTAS" {...a11yProps(1)} />
                    <Tab label="REPORTE CONSOLIDADO POR UE" {...a11yProps(1)} />
                </Tabs>
                <TabPanel value={value} index={0}>
                    Información de Entrada
                    <div className='MuiTableCell-stickyHeader; jss86'>
                        <Grid container>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="fechaRecepcion"
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
                                            label="Fecha de Recepción"
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
                                    name="valorIpc"
                                    control={formularioParametrosGenerales.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextFieldV2
                                            isReadOnly
                                            label="Valor I.P.C."
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
                                    name="porcentajeIpc"
                                    control={formularioParametrosGenerales.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextFieldV2
                                            percentMask
                                            isReadOnly
                                            label="Porcentaje Abono de Interés"
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
                                    name="anio"
                                    control={formularioParametrosGenerales.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextFieldV2
                                            numberMask
                                            isReadOnly
                                            label="Periodo Año"
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
                                    name="mes"
                                    control={formularioParametrosGenerales.control}
                                    render={({
                                        field: { onChange, value },
                                    }) => (
                                        <CHTextFieldV2
                                            numberMask
                                            isReadOnly
                                            label="Periodo Mes"
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
                                    name="comprobanteSolucionViviendaAportes"
                                    control={formularioParametrosGenerales.control}
                                    render={({ field: { onChange, value } }) => (
                                        <Grid container justify="space-between">
                                            <Grid item>
                                                <CHTextFieldV2
                                                    isReadOnly
                                                    label="Comprobante (Solución de Vivienda)"
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
                                                    onChange={onChange}
                                                />
                                            </Grid>
                                        </Grid>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="comprobanteSolucionAdministracionCesantias"
                                    control={formularioParametrosGenerales.control}
                                    render={({ field: { onChange, value } }) => (
                                        <Grid container justify="space-between">
                                            <Grid item>
                                                <CHTextFieldV2
                                                    isReadOnly
                                                    label="Comprobante (Administración de Cesantías)"
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
                                                    onChange={onChange}
                                                />
                                            </Grid>
                                        </Grid>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="comprobanteAVAC"
                                    control={formularioParametrosGenerales.control}
                                    render={({ field: { onChange, value } }) => (
                                        <Grid container justify="space-between">
                                            <Grid item>
                                                <CHTextFieldV2
                                                    isReadOnly
                                                    label="Comprobante (AVAC)"
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
                                                    onChange={onChange}
                                                />
                                            </Grid>
                                        </Grid>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="comprobanteAVAV"
                                    control={formularioParametrosGenerales.control}
                                    render={({ field: { onChange, value } }) => (
                                        <Grid container justify="space-between">
                                            <Grid item>
                                                <CHTextFieldV2
                                                    isReadOnly
                                                    label="Comprobante (AVAV)"
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
                                                    onChange={onChange}
                                                />
                                            </Grid>
                                        </Grid>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="comprobanteAVC"
                                    control={formularioParametrosGenerales.control}
                                    render={({ field: { onChange, value } }) => (
                                        <Grid container justify="space-between">
                                            <Grid item>
                                                <CHTextFieldV2
                                                    isReadOnly
                                                    label="Comprobante (AVC)"
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
                                                    onChange={onChange}
                                                />
                                            </Grid>
                                        </Grid>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="numeroRadicado"
                                    control={formularioParametrosGenerales.control}
                                    render={({ field: { onChange, value } }) => (
                                        <Grid container justify="space-between">
                                            <Grid item>
                                                <CHTextFieldV2
                                                    isReadOnly
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
                                                    onChange={onChange}
                                                />
                                            </Grid>
                                        </Grid>
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </div>
                    <Grid className="buttonssolicitud">
                        <Link to={{ pathname: './abonoIntereses' }}>
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
                    <div className='MuiTableCell-stickyHeader; jss86'>
                        <CierreTable1
                            data={dataCierreMesInfo}
                            mostrarTabla={mostrarTablaInfo}
                        />
                    </div>
                    <Grid className="buttonssolicitud">
                        <Link to={{ pathname: './abonoIntereses' }}>
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
                        <Link to={{ pathname: './abonoIntereses' }}>
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
    );
};

export default abonoInteresesDetalles;