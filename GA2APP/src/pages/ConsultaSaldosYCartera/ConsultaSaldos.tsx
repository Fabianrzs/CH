import React, {useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem
} from '@material-ui/core';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CarteraApi from '../../services/api-cartera-service';
import UtilService from '../../utils/util.service';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import { LoadingComponent } from '../../components/loading/loading.component';
import ApiReportes from '../../services/api-reportes.service';
import CatalogApi from '../../services/api-catalog.service';
import AuthService from '../../services/auth.service';
import AuditoriaApi from '../../services/api-auditorias.servise'
import { Auditoria } from '../../types/Auditoria.model.type';
import TablaSaldosCartera from './TablaSaldosCartera';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        button2: {
            width: '100%',
            marginTop: '1.5rem',
            color: '#fff',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px px',
        },
        accountButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 8px',
        },

        buttonNoObl: {
            width: '17rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
        buttonTotalSaldos: {
            width: '17rem',
            marginLeft: '31rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
        buttonTotalProvision: {
            width: '17rem',
            marginLeft: '1.5rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
    }),
);

const tipoCredito = [{ id: 1, desc: 'LEASING' }, { id: 2, desc: 'HIPOTECARIO' }, { id: 0, desc: 'CONSULTA TOTAL CARTERA' }]
const meses = [{ id: '1', desc: 'ENERO' }
    , { id: '2', desc: 'FEBRERO' }
    , { id: '3', desc: 'MARZO' }
    , { id: '4', desc: 'ABRIL' }
    , { id: '5', desc: 'MAYO' }
    , { id: '6', desc: 'JUNIO' }
    , { id: '7', desc: 'JULIO' }
    , { id: '8', desc: 'AGOSTO' }
    , { id: '9', desc: 'SEPTIEMBRE' }
    , { id: '10', desc: 'OCTUBRE' }
    , { id: '11', desc: 'NOVIEMBRE' }
    , { id: '12', desc: 'DICIEMBRE' }
]

const api = CarteraApi();
const apiReportes = ApiReportes();

const sumTotalValue = (dataInt: any, tipoCredito: any) => {
    let total = 0;
    dataInt.forEach((element: any) => {
        if (tipoCredito === 1) {
            total += element.saldoLeasing
        } else if (tipoCredito === 2) {
            total += element.saldoHipotecario
        } else {
            total += element.saldoLeasing + element.saldoHipotecario
        }
    });
    return total;
}
const sumTotalValuePro = (dataInt: any) => {
    let total = 0;
    dataInt.forEach((element: any) => {
        total += element.provision
    });
    return total;
}

function ConsultaSaldosTable({
    data,
    watch
}: {
    data: any | null;
    watch: any
}) {
    const classes = useStyles();

    return (
        <div>
            {data != null ? (
                <div>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.buttonNoObl}
                    >
                        {data != null ? (
                            <p>{`TOTALES CRÉDITOS ${data.length}`}</p>
                        ) : null}
                    </Button>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.buttonTotalSaldos}
                    >
                        {data != null ? (
                            <p>{`TOTAL DE LOS SALDOS ${UtilService.formatCurrency(sumTotalValue(data, watch('tipoCredito')))}`}</p>
                        ) : null}
                    </Button>
                    <Button
                        variant="outlined"
                        color="primary"
                        className={classes.buttonTotalProvision}
                    >
                        {data != null ? (
                            <p>{`TOTAL PROVISIONES ${UtilService.formatCurrency(sumTotalValuePro(data))}`}</p>
                        ) : null}
                    </Button>
                    <TablaSaldosCartera
                        data={data}
                        watch={watch}
                        UtilService={UtilService}
/>
                </div>

            ) : null}
        </div>

    );
}

const ConsultaSaldos = () => {
    const classes = useStyles();

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);

    const { control, handleSubmit, watch, setValue } = useForm<any>({
        defaultValues: {
            tipoCredito: 0,
            numeroObligacion: '',
            anio: '',
            mes: '',
            periodoActual: false
        }
    });

    const tipodeCredito = watch('tipoCredito');
    const numerodeObligacion = watch('numeroObligacion');
    const mesCartera = watch('mes');
    const anioCartera = watch('anio');
    const periodoActual = watch('periodoActual');


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


    // const handleSearchButton = async (datos: any) => {
    //     setLoading(true)
    //     api.getConsultaSaldos(datos)
    //         .then((response) => {
    //             if (response.data != null && response.data.length !== 0) {
    //                 setData(response.data)

    //                 const auditoriaApi = AuditoriaApi();
    //                 const user = AuthService.getUserInfo();
    //                 const nav = window.navigator as any;
    //                 const usrAg = nav.userAgentData.brands.find(
    //                     (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    //                 )?.brand;

    //                 let descripcionTipoCredito = 'No especificado';
    //                 let descripcionPeriodo = 'No especificado';

    //                 if (tipodeCredito === 0) {
    //                     descripcionTipoCredito = 'CONSULTA TOTAL CARTERA'
    //                 } else if (tipodeCredito === 1) {
    //                     descripcionTipoCredito = 'LEASING'
    //                 } else if (tipodeCredito === 2) {
    //                     descripcionTipoCredito = 'HIPOTECARIO'
    //                 }

    //                 if (periodoActual === true) {
    //                     descripcionPeriodo = 'Periodo actual marcado'
    //                 } else if (periodoActual === false) {
    //                     descripcionPeriodo = 'Periodo actual no marcado'
    //                 }



    //                 const requestAuditoria = {
    //                     navegador: usrAg,
    //                     sistemaOperativo: nav.userAgentData.platform,
    //                     nombreUsuario: user.USR_USERNAME,
    //                     usurioId: user.USR_ID,
    //                     ip: publicIpAddress,
    //                     modulo: 'Saldos de cartera',
    //                     hostname: document.location.host,
    //                     tipoAccion: 'CONSULTAS',
    //                     idTipoAccion: 3,
    //                     descripcion: `Consulta de saldos con los siguientes filtros:
    //                           - Tipo de crédito: ${descripcionTipoCredito || 'No especificada'}
    //                           - Número obligación: ${numerodeObligacion || 'No especificado'}
    //                           - Mes: ${mesCartera || 'No especificada'}
    //                           - Año: ${anioCartera || 'No especificado'}
    //                           - Periodo Actual: ${descripcionPeriodo || 'No especificada'}`,
    //                 } as Auditoria;

    //                 auditoriaApi.InsertAuditoria(requestAuditoria);


    //             } else if (datos.numeroObligacion !== '') {
    //                 toast('No se encontraron datos para este número de obligación')
    //             } else if ((datos.mes !== '' || datos.anio !== '') && !datos.periodoActual) {
    //                 toast('No se encontraron datos para este periodo')
    //             } else if (datos.periodoActual) {
    //                 toast('No se encontraron datos para el periodo actual')
    //             }
    //         })
    //         .catch(() => {
    //             setLoading(false)
    //             toast('Error en la consulta')
    //         })
    //     setLoading(false)
    // };

    const handleSearchButton = async (datos: any) => {
        setLoading(true);

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const response = await api.getConsultaSaldos(datos);

            if (response.data != null && response.data.length !== 0) {
                setData(response.data);

                let descripcionTipoCredito = 'No especificado';
                let descripcionPeriodo = 'No especificado';

                if (tipodeCredito === 0) {
                    descripcionTipoCredito = 'CONSULTA TOTAL CARTERA'
                } else if (tipodeCredito === 1) {
                    descripcionTipoCredito = 'LEASING'
                } else if (tipodeCredito === 2) {
                    descripcionTipoCredito = 'HIPOTECARIO'
                }

                if (periodoActual === true) {
                    descripcionPeriodo = 'Periodo actual marcado'
                } else if (periodoActual === false) {
                    descripcionPeriodo = 'Periodo actual no marcado'
                }

                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Saldos de cartera',
                    hostname: document.location.host,
                    tipoAccion: 'CONSULTAS',
                    idTipoAccion: 3,
                    descripcion: `Consulta de saldos con los siguientes filtros:
                          - Tipo de crédito: ${descripcionTipoCredito}
                          - Número obligación: ${numerodeObligacion || 'No especificado'}
                          - Mes: ${mesCartera || 'No especificada'}
                          - Año: ${anioCartera || 'No especificado'}
                          - Periodo Actual: ${descripcionPeriodo}`,
                } as Auditoria;

                auditoriaApi.InsertAuditoria(requestAuditoria);

            } else if (datos.numeroObligacion !== '') {
                toast('No se encontraron datos para este número de obligación');
            } else if ((datos.mes !== '' || datos.anio !== '') && !datos.periodoActual) {
                toast('No se encontraron datos para este periodo');
            } else if (datos.periodoActual) {
                toast('No se encontraron datos para el periodo actual');
            }
        } catch (error: any) {
            const descripcionError = `Error al consultar saldos de cartera.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Saldos de cartera',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error en la consulta');
        } finally {
            setLoading(false);
        }
    };


    const GenerarExcel = async () => {
        const columnDataList = data?.map((row: any) => {

            let saldoTotal = '';

            switch (watch('tipoCredito')) {
                case 1:
                    saldoTotal = String(row.saldoLeasing ?? '');
                    break;

                case 2:
                    saldoTotal = String(row.saldoHipotecario ?? '');
                    break;

                default:
                    saldoTotal = String(
                        Number(row.saldoHipotecario ?? 0) +
                        Number(row.saldoLeasing ?? 0)
                    );
                    break;
            }

            return {
                data: [
                    String(row.numeroObligacion ?? ''),
                    String(row.identificacion ?? ''),
                    String(row.nombre ?? ''),
                    String(row.tipoCredito ?? ''),
                    saldoTotal,
                    String(row.provision ?? '')
                ]
            };
        });

        const request = {
            columnDataList: columnDataList.map(item => ({
                data: item.data
            })),
            columnName: [
                "Número De Obligación", "Cédula Del Afiliado",
                "Nombre Del Afiliado", "Tipo Crédito", "Saldo Total", "Provisión"
            ]
        };
        const response = await apiReportes.GenerarExcels(request);
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `Saldos-Cartera:${moment().format()}.xlsx`;
        document.body.appendChild(link);
        link.click();
        window.URL.revokeObjectURL(url);


        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Saldos de Cartera',
                hostname: document.location.host,
                tipoAccion: 'DESCARGAS',
                idTipoAccion: 5,
                descripcion: 'Descarga de archivo Excel saldos de cartera'

            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error al descargar Excel.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Saldos de Cartera',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
        }
    }

    const [isperiodoActualChecked, setIsperiodoActualChecked] = useState(false);
    const handlePeriodoChange = (event: any) => {
        setIsperiodoActualChecked(event.target.checked);
    };

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Saldos de Cartera"
                subText=""
                title1="Saldos de Cartera"
                title2=""
            />
            <CHPaper title="Consulta saldos ss">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo de Crédito"
                                    value={value}
                                    onChange={(e) => {
                                        onChange(e);
                                        setData(null || [])
                                        setValue('numeroObligacion', '')
                                        setValue('mes', '')
                                        setValue('anio', '')
                                    }}
                                >
                                    {tipoCredito.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.desc}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="tipoCredito"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={2}>
                        <Controller
                            control={control}
                            name="numeroObligacion"
                            defaultValue="0"
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Número obligacion"
                                    value={value}
                                    onChange={onChange}
                                    type="number"
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={2} >
                        <Controller
                            control={control}
                            name="mes"
                            rules={{
                                validate: () => !isperiodoActualChecked && (watch('mes') < 1 || watch('mes') == null) ? 'El mes es obligatorio' : true
                            }}
                            defaultValue="0"
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Mes*"
                                    disabled={isperiodoActualChecked}
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {meses.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.desc}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={2}>
                        <Controller
                            control={control}
                            name="anio"
                            rules={{
                                required: {
                                    value: !isperiodoActualChecked,
                                    message: 'Este campo es requerido'
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Año*"
                                    disabled={isperiodoActualChecked}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    value={value}
                                    onChange={onChange}
                                    type="number"
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={2}>
                        <Controller
                            name="periodoActual"
                            control={control}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <CHCheckbox
                                    label="Periodo actual"
                                    onChange={(e) => {
                                        handlePeriodoChange(e)
                                        setValue('mes', '')
                                        setValue('anio', '')
                                        onChange(e)
                                    }}
                                    checked={value}
                                />
                            )}
                        />
                    </Grid>
                    {/* <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            name="activo"
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHCheckboxV2
                                    label="Activo"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid> */}
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={handleSubmit(handleSearchButton)}
                        >
                            CONSULTAR SALDOS
                        </Button>
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={GenerarExcel}
                        >
                            GENERAR EXCEL
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
            <ConsultaSaldosTable
                data={data}
                watch={watch}
            />
        </CHPrivateContainer>
    );
};

export default (ConsultaSaldos);
