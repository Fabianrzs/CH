import React, { useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Button, Card, CardContent, createStyles, Grid, makeStyles} from '@material-ui/core';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHDatePickerV3 from '../../../../components/ch-datepicker/CHDatepickerV3';
import NotificationService from '../../../../services/notification.service';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';
import UtilService from '../../../../utils/util.service';
import AuthService from '../../../../services/auth.service';
import ApiVerificacionDerecho from '../../../../services/api-verificacion.service';
import CatalogApi from '../../../../services/api-catalog.service';
import { Auditoria } from '../../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../../services/api-auditorias.servise'

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
    data, mostrarTabla, infoUsuario
}:
    {
        data: any[],
        mostrarTabla: any,
        infoUsuario: any
    }) {
    const columns = [

        {
            name: "Procesar",
            options: {
                customBodyRender: (value: any, tableMeta: any,) => {
                    const aprobado = data[tableMeta.rowIndex].estado;
                    let buttonText = "DETALLES";

                    if(infoUsuario === "60" && aprobado === 2){
                        buttonText = "DETALLES";
                    }else if(aprobado === null || aprobado === 1 || aprobado === 2){
                        buttonText = "CONTINUAR";
                    }

                    return (
                        <Link to={{ pathname: './abonoInteresesDetalles', state: { rowData: data[tableMeta.rowIndex] } }}>
                            <button disabled={infoUsuario === "88" && aprobado === 1}>
                                {buttonText}
                            </button>
                        </Link>
                    );
                },
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'numeroSolicitud',
            label: '# Solicitud',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
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
            name: 'totalInteresesAbonados',
            label: 'Intereses Abonados',
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
            name: 'usuarioNombre',
            label: 'Nombre Usuario',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'fechaAprobacion',
            label: 'Fecha de Aprobación',
            options: {
                customBodyRender: (value: Date) => UtilService.getFullDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'nombreEstado',
            label: 'ESTADO',
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
                title="Solicitudes Abono de Intereses"
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

const abonoIntereses = () => {

    const user = AuthService.getUserInfo();
    const classes = useStyles();
    const Api = ApiVerificacionDerecho();
    const location = useLocation();
    const state = location.state as any;
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [dataCierreMes, setDataCierreMes] = useState<any[]>([]);
    const [mostrarTabla, setMostrarTabla] = useState(false);

    useEffect(() => {
        if (state?.showSuccessMessage) {
            NotificationService.genericAlert(
                '¡Exitoso!',
                'Solicitud aprobada con éxito.'
            );
        }
    }, [location.state]);


    const showData = async () => {

        await Api
            .ObtenerTodoInteresAbonoRequest()
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
        showData()
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
            DesdeAnio: moment(data.fechaInicial).year(),
            DesdeMes: moment(data.fechaInicial).month() + 1,
            HastaAnio: moment(data.fechaFinal).year(),
            HastaMes: moment(data.fechaFinal).month() + 1,
        }

        await Api
            .ObtenerTodoAbonoInteresFiltroRequest(request)
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
                                modulo: 'Abono de Intereses',
                                hostname: document.location.host,
                                tipoAccion: 'CONSULTAS',
                                idTipoAccion: 3,
                                descripcion: `Consulta Abono de intereses con los siguientes filtros:
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
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });
    };

    if (isLocalLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <div
            style={{
                display: 'table',
                tableLayout: 'fixed',
                width: '100%',

            }}>
            <PageIndicators
                topTextNivel1="Administración Cuentas"
                topTextNivel2="Abono de Intereses"
            />
            <IconTitle
                title="Abono"
                subTitle="de Intereses"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid>
                <Grid item xs={12}>
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <CHContainerSubtitle
                                        subTitle="Abono de intereses"
                                        title="Consulta "
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
            </Grid>
            <br />
            <div className='MuiTableCell-stickyHeader; jss86'>
                <CierreTable
                    data={dataCierreMes}
                    mostrarTabla={mostrarTabla}
                    infoUsuario={user.GRP_ID}
                />
            </div>
        </div>
    );
};

export default abonoIntereses;