import React, { useEffect, useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHDatePickerV3 from '../../../../components/ch-datepicker/CHDatepickerV3';
import ApiParametrization from '../../../../services/api-parametrization.service';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';
import UtilService from '../../../../utils/util.service';
import { ModalIPC } from './modalIPC';
import { IPCDto } from '../../../../types/parametrization.model.type';
import AuthService from '../../../../services/auth.service';
import { Auditoria } from '../../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../../services/api-auditorias.servise'
import CatalogApi from '../../../../services/api-catalog.service';

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

        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

function CierreTable({
    data, mostrarTabla, handleOpenModalEdit, infoUsuario
}:
{
    infoUsuario: any
    data: any[],
    mostrarTabla: any,
    handleOpenModalEdit: any
}) {

    const columns = [

        {
            name: "Procesar",
            options: {
                customBodyRender: (value: any, tableMeta: any,) => {
                    const fecha1 = new Date();
                    fecha1.setMonth(fecha1.getMonth() - 1);
                    const fecha2 = new Date(data[tableMeta.rowIndex].anio, data[tableMeta.rowIndex].mes, 1);

                    if (fecha1 <= fecha2) {
                        return (
                            <button
                                disabled={(infoUsuario !== "88")}
                                onClick={() => handleOpenModalEdit(data[tableMeta.rowIndex], "Actualizar")}>
                                CONTINUAR
                            </button>
                        );
                    }
                    return null;
                },
                setCellHeaderProps: () => ({
                    className: "filas",
                }),
            },
        },
        {
            name: 'anio',
            label: 'Año',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'mes',
            label: 'Mes',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'porcentajeIPC',
            label: '%  de Abono Intereses',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'valorIPC',
            label: 'Valor I.P.C',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'fechaActualizacion',
            label: 'Fecha de Actualización',
            options: {
                customBodyRender: (value: Date) => UtilService.getFullDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
    ];

    return (

        mostrarTabla && (
            <MUIDataTable
                title="Historial IPC"
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

const IPC = () => {

    const user = AuthService.getUserInfo();
    const Api = ApiParametrization();
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [dataCierreMes, setDataCierreMes] = useState<any[]>([]);
    const [mostrarTabla, setMostrarTabla] = useState(false);
    const [openModalAdd, setOpenModalAdd] = useState(false);
    const classes = useStyles();
    const [buttonText, setButtonText] = useState("");
    const [idConnection, setIdConnection] = useState('');

    const showData = async () => {

        await Api
            .ObtenerTodoReporteIPCRequest()
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {

                            setDataCierreMes(response.data)
                            setMostrarTabla(true)
                            setIsLocalLoading(false);

                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTabla(false)
                        setIsLocalLoading(false);
                    }

                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });
    }

    useEffect(() => {
        showData();
        const id = localStorage.getItem('connectionId');
        setIdConnection(id || '');
    }, [])

    const formularioIPC = useForm({
        'mode': 'onChange',
        defaultValues: {

            fechaInicial: null,
            fechaFinal: moment(new Date()).add(1, 'months').format('YYYY-MM'),
        },

    });

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0]

        const hexadecimal = convertIp
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

    const fechaInicialIndice = formularioIPC.watch('fechaInicial');
    const fechaFinalIndice = formularioIPC.watch('fechaFinal');

    const onSubmit = async (data: any) => {

        if (!data.fechaInicial) {
            data.fechaInicial = moment('1910-12').format('YYYY-MM');
        }

        if (!data.fechaFinal) {
            data.fechaFinal = moment(new Date()).format('YYYY-MM');
        }

        const request = {
            id: 0,
            anio: 0,
            mes: 0,
            FechaActualizacion: new Date(),
            ValorIPC: 0,
            PorcentajeIPC: 0,
            Desde_Anio: moment(data.fechaInicial).year(),
            Desde_Mes: moment(data.fechaInicial).month() + 1,
            Hasta_Anio: moment(data.fechaFinal).year(),
            Hasta_Mes: moment(data.fechaFinal).month() + 1,
            UsuarioId: user.USR_ID,
            UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`
        }

        await Api
            .ObtenerReporteIPCRequest(request)
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {

                            setDataCierreMes(response.data)
                            setMostrarTabla(true)
                            setIsLocalLoading(false);

                            const auditoriaApi = AuditoriaApi();
                            const user = AuthService.getUserInfo();
                            const nav = window.navigator as any;
                            const usrAg = nav.userAgentData.brands.find(
                                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                            )?.brand;

                            const requestAuditoria = {
                                navegador: usrAg,
                                sistemaOperativo: nav.userAgentData.platform,
                                nombreUsuario: user.USR_USERNAME,
                                usurioId: user.USR_ID,
                                ip: publicIpAddress,
                                modulo: 'Reintegro I.P.C',
                                hostname: document.location.host,
                                tipoAccion: 'CONSULTAS',
                                idTipoAccion: 3,
                                descripcion: `Consulta de Índice con los siguientes filtros:
                                    - Periodo Inicial: ${fechaInicialIndice || 'No especificada'}
                                    - Periodo Final: ${fechaFinalIndice || 'No especificada'}`,
                            } as Auditoria;

                            auditoriaApi.InsertAuditoria(requestAuditoria);
                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTabla(false)
                        setIsLocalLoading(false);
                    }
                }
            }).catch((error: any) => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });


                setIsLocalLoading(false);
            });
    };

    if (isLocalLoading) {
        return <CHLoadingProcess />;
    }

    const formulario = useForm<IPCDto>({
        defaultValues: {
            Desde_Anio: moment(new Date()).year(),
            Desde_Mes: moment(new Date()).month(),
            Hasta_Anio: moment(new Date()).year(),
            Hasta_Mes: moment(new Date()).month(),
            id: 0,
            anio: 0,
            mes: 0,
            FechaActualizacion: new Date(),
            ValorIPC: 0,
            PorcentajeIPC: 100.0,
            UsuarioId: user.USR_ID,
            UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`
        },
    });

    const handleOpenModalEdit = (rowData: any, buttonText: string) => {
        const fechaPeriodo = new Date(rowData.anio, rowData.mes - 1, 1);

        const request = {
            id: rowData.id,
            anio: rowData.anio,
            mes: rowData.mes,
            FechaActualizacion: fechaPeriodo,
            ValorIPC: rowData.valorIPC,
            PorcentajeIPC: rowData.porcentajeIPC,
            Desde_Anio: moment(new Date()).year(),
            Desde_Mes: moment(new Date()).month(),
            Hasta_Anio: moment(new Date()).year(),
            Hasta_Mes: moment(new Date()).month(),
            UsuarioId: user.USR_ID,
            UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`
        }

        formulario.reset(request);
        setButtonText(buttonText);
        setOpenModalAdd(true);
    };

    const handleOpenModalAdd = (buttonText: string) => {

        const request = {
            Desde_Anio: moment(new Date()).year(),
            Desde_Mes: moment(new Date()).month(),
            Hasta_Anio: moment(new Date()).year(),
            Hasta_Mes: moment(new Date()).month(),
            id: 0,
            anio: 0,
            mes: 0,
            FechaActualizacion: new Date(),
            ValorIPC: 0,
            PorcentajeIPC: 100.0,
            UsuarioId: user.USR_ID,
            UsuarioNombre: `${user.USR_NOMBRE} ${user.USR_APELLIDO}`
        }

        formulario.reset(request);
        setButtonText(buttonText);
        setOpenModalAdd(true);
    };

    const handleCloseModalAdd = async () => {
        setOpenModalAdd(false);
    };

    const handleAdd = async (data: IPCDto) => {

        let valorId = 0;
        if (data.id === 0) {
            valorId = 0;
        }
        else {
            valorId = data.id;

        }
        const valoresBusqueda: IPCDto = {
            Desde_Anio: moment(new Date()).year(),
            Desde_Mes: moment(new Date()).month(),
            Hasta_Anio: moment(new Date()).year(),
            Hasta_Mes: moment(new Date()).month(),
            id: valorId,
            anio: moment(data.FechaActualizacion).year(),
            mes: moment(data.FechaActualizacion).month() + 1,
            FechaActualizacion: new Date(),
            ValorIPC: data.ValorIPC,
            PorcentajeIPC: data.PorcentajeIPC,
            UsuarioId: user.USR_ID,
            UsuarioNombre: buttonText === "Sincronizar" ? idConnection : `${user.USR_NOMBRE} ${user.USR_APELLIDO}`

        };

        if (buttonText === "Sincronizar") {
            try {
                await Api.PostSincronizarCuentas(valoresBusqueda).then(async (ipcResponse) => {
                    if (ipcResponse && ipcResponse.mensaje && ipcResponse.mensaje.includes('Tarea de sincronizacion')) {
                        toast('Sincronización de cuentas iniciada', { type: toast.TYPE.SUCCESS });
                        setIsLocalLoading(false);
                        handleCloseModalAdd();
                    } else {
                        setIsLocalLoading(false);
                        toast('Error al sincronizar', { type: toast.TYPE.ERROR });
                    }
                }).catch(() => {
                    setIsLocalLoading(false);
                    toast('Error al sincronizar', { type: toast.TYPE.ERROR });
                });

            } catch (error) {
                toast('Error en sincronizar:', { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            }

        } else {
            try {
                const crearIPCPromise = Api.CrearIPCRequest(valoresBusqueda).then(async (ipcResponse) => {
                    if (ipcResponse && ipcResponse.isSuccess) {
                        toast('Agregado con éxito', { type: toast.TYPE.SUCCESS });
                        setIsLocalLoading(false);
                        handleCloseModalAdd();
                        try {
                            const response = await Api.ObtenerTodoReporteIPCRequest();
                            if (response && response.isSuccess && response.data) {
                                setDataCierreMes(response.data);
                                setMostrarTabla(true);
                            } else {
                                toast('No se encontraron resultados', { type: toast.TYPE.ERROR });
                                setMostrarTabla(false);
                            }
                        } catch {
                            toast('Error del sistema al obtener el reporte', { type: toast.TYPE.ERROR });
                        }
                    } else {
                        toast('Error al agregar IPC', { type: toast.TYPE.ERROR });
                    }
                }).catch(() => {
                    toast('Error al procesar CrearIPC', { type: toast.TYPE.ERROR });
                });

                const crearProcesosIPCPromise = Api.CrearProcesosIPCRequest(valoresBusqueda).catch(() => {
                    toast('Error al procesar CrearProcesosIPC', { type: toast.TYPE.ERROR });
                });

                await Promise.allSettled([crearIPCPromise, crearProcesosIPCPromise]);
                setIsLocalLoading(false);
                handleCloseModalAdd();
            } catch (error) {
                console.error('Error en la ejecución de las promesas:', error);
                toast('Error en el sistema', { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            }
        }
    };

    return (
        <div
            style={{
                display: 'table',
                tableLayout: 'fixed',
                width: '100%',

            }}>
            <PageIndicators
                topTextNivel1="Administración Cuentas"
                topTextNivel2="Registro I.P.C"
            />
            <IconTitle
                title="Índice de Precios"
                subTitle="al Consumidor"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid>
                <Grid item xs={12}>
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <CHContainerSubtitle
                                        subTitle="Consulta de Índice"
                                        title="Precios al Consumidor "
                                        icon="fch-Procedimientos-solicitudes-0-1"
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaInicial"
                                        control={formularioIPC.control}
                                        rules={{
                                            validate: (value) => {

                                                if (!value) {
                                                    return true;
                                                }
                                                const startDateValue = moment(formularioIPC.getValues('fechaInicial'), 'YYYY/MM');
                                                const endDateValue = moment(formularioIPC.getValues('fechaFinal'), 'YYYY/MM');

                                                const isStartDateValid = startDateValue.isValid();
                                                const isEndDateValid = endDateValue.isValid();

                                                if (!isStartDateValid) {
                                                    return 'Fecha inicial no válida';
                                                }

                                                if (isEndDateValid) {
                                                    const isStartDateBeforeEndDate = startDateValue.isBefore(endDateValue);
                                                    if (!isStartDateBeforeEndDate) {
                                                        return 'Periodo inicial debe ser menor al periodo final';
                                                    }
                                                }

                                                const isAfterOrEqualMinDate = startDateValue.isSameOrAfter(moment('1910-12'), 'month');

                                                if (!isAfterOrEqualMinDate) {
                                                    return 'Fecha inicial no válida';
                                                }

                                                return true;
                                            },
                                        }}
                                        render={({ field, fieldState }) => (
                                            <CHDatePickerV3
                                                views={['year', 'month']}
                                                invalidDateMessage=""
                                                maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                                                minDateMessage='La fecha no debe ser anterior a la fecha mínima'
                                                minDate={moment('1910-12').format('YYYY-MM')}
                                                label="Periodo Inicial *"
                                                value={field.value}
                                                onChange={(date) => field.onChange(date)}
                                                onBlur={field.onBlur}
                                                error={!!fieldState.error}
                                                helperText={fieldState.error ? fieldState.error.message : null}
                                            />
                                        )}
                                    />
                                    <br />
                                    <Controller
                                        name="fechaFinal"
                                        control={formularioIPC.control}
                                        rules={{
                                            validate: (value) => {

                                                if (!value) {
                                                    return true;
                                                }

                                                const isAfterOrEqualMinDate = moment(value, 'YYYY/MM').isSameOrAfter(moment('1910-12'), 'month');

                                                if (!isAfterOrEqualMinDate) {
                                                    return 'Fecha no válida';
                                                }

                                                return true;
                                            },
                                        }}
                                        render={({ field, fieldState }) => (
                                            <CHDatePickerV3
                                                views={['year', 'month']}
                                                invalidDateMessage=""
                                                maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                                                minDateMessage='La fecha no debe ser anterior a la fecha mínima'
                                                minDate={moment('1910-12').format('YYYY-MM')}
                                                label="Periodo Final *"
                                                value={field.value}
                                                onChange={(date) => field.onChange(date)}
                                                onBlur={field.onBlur}
                                                error={!!fieldState.error}
                                                helperText={fieldState.error ? fieldState.error.message : null}
                                            />
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
                    onClick={formularioIPC.handleSubmit(onSubmit)}
                >
                    Consultar
                </Button>

                <Button
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={() => handleOpenModalAdd("Agregar")}
                >
                    Agregar I.P.C.
                </Button>
                <Button
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={() => handleOpenModalAdd("Sincronizar")}
                >
                    Sincronizar Cuentas
                </Button>
            </Grid>
            <br />
            <div className='MuiTableCell-stickyHeader; jss86'>
                <CierreTable
                    data={dataCierreMes}
                    mostrarTabla={mostrarTabla}
                    handleOpenModalEdit={handleOpenModalEdit}
                    infoUsuario={user.GRP_ID}
                />
            </div>
            <ModalIPC
                open={openModalAdd}
                formulario={formulario}
                onClose={handleCloseModalAdd}
                handleAdd={handleAdd}
                buttonText={buttonText}
            />
        </div>
    );
};

export default IPC;
