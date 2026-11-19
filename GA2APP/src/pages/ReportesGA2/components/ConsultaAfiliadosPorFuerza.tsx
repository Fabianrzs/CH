import React, {useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Typography
} from '@material-ui/core';
import moment from 'moment';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { startGetAllExecutingUnits} from '../../../actions/catalog.action';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CatalogApi from '../../../services/api-catalog.service';
import ApiClients from '../../../services/api-clients.service';
import ApiReportes from '../../../services/api-reportes.service';
import { AppState } from '../../../store/rootReducer';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise'
import { LoginState } from '../../../reducers/login.reducer';
import UtilService from '../../../utils/util.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginRight: '10px',
            marginLeft: '210%',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        textTable: {
			textAlign: 'center'
		},
        text: {
			fontWeight: 'bold'
		},
        titleform: {
            width: '100%',
            marginLeft: '10px',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

const api = ApiClients();
const apiReportes = ApiReportes();

function ConsultaAfiliadosTable({
    data,
    datosFormulario
}: {
    data: any | null;
    datosFormulario: any
}) {
    const classes = useStyles();

    return (
        <div>
                 <Grid container style={{ marginTop: "40px" }}>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                      {datosFormulario?.nombreFuerza === 'CONTEO GENERAL'? "Conteo por Estado" : `Fuerza: ${data?.nombreFuerza}`} 
                    </Typography>
                </Grid>
                <table className="client-table">
                    <thead>
                        <tr>
                            <th className={classes.textTable}>Estado</th>
                            <th className={classes.textTable}>Total Afiliados</th>
                        </tr>
                    </thead>
                    <tbody>
                    {data?.estados?.map((element: any, index: any) => (
                            <tr key={`table-${index}`}>
                                <td className="tab-space-01">{element.estado}</td>
                                <td className="tab-space-01">{UtilService.formatNumber(element.total)}</td>
                            </tr>
                    ))}
                      <tr className={classes.text}>
                            <td className="tab-space-01"> {datosFormulario?.nombreFuerza === 'CONTEO GENERAL'? "TOTAL AFILIADOS POR ESTADO" : "TOTAL AFILIADOS POR FUERZA"}</td>
                            <td className="tab-space-01">{datosFormulario?.nombreFuerza === 'CONTEO GENERAL'? UtilService.formatNumber(data?.totalGeneral) : UtilService.formatNumber(data?.totalFuerza)}</td>
                      </tr>
                    </tbody>
                </table>
        </div>
    );
}

const ConsultaAfiliadosPorFuerza = (props: Props) => {

    const classes = useStyles();
    const {loginState, catalogState,startGetAllExecutingUnits} = props;
    const [informacionConteo, setInformacionConteo] = useState<any>([]);
    const [fuerzaEncontrada, setFuerzaEncontrada] = useState<any>(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        startGetAllExecutingUnits(loginState?.claims)
        consultarAfiliadosPorFuerza();
    }, [])

    const formularioAfiliadosPorFuerza = useForm<any>({
        defaultValues: {
            nombreFuerza: 'SELECCIONE...',
        }
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


    const consultarAfiliadosPorFuerza = async () => {

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        const FUERZAS = '0';
        await api.getConsultaAfiliadosPorFuerza(FUERZAS)
            .then(response => {
                if(response?.data){
                    setInformacionConteo(response?.data);
                    // Auditoría de consulta exitosa
                    const requestAuditoria: Auditoria = {
                        navegador: usrAg,
                        sistemaOperativo: nav.userAgentData.platform,
                        nombreUsuario: user.USR_USERNAME,
                        usurioId: user.USR_ID,
                        ip: publicIpAddress,
                        modulo: 'Generación de Reportes GA2',
                        hostname: document.location.host,
                        tipoAccion: 'CONSULTAS',
                        idTipoAccion: 3,
                        descripcion: `Consulta de conteo afiliados por estado y fuerza`
                    };

                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }else {
                    toast('No se encuentra información');
                }
            })
            .catch((error: any) => {
                // Auditoría de error funcional
                const requestAuditoriaError: Auditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Generación de Reportes GA2',
                    hostname: document.location.host,
                    tipoAccion: 'ERRORES Y EXCEPCIONES',
                    idTipoAccion: 8,
                    descripcion: `Error en la consulta de conteo de afiliados por estado y fuerza.
                                    - Mensaje API: ${error?.returnMessage || 'No disponible'}`
                };

                auditoriaApi.InsertAuditoria(requestAuditoriaError);
                toast('Error en consulta afiliados por fuerza', { type: toast.TYPE.ERROR });
            })
    }

    const GenerarExcel = async () => {

        setLoading(true);
        const arrayReporte: any[]= [];

        const tituloEstados = {
            columna1: "ESTADO",
            columna2: "TOTAL"
        }

        const espacio ={
            columna1: " ",
            columna2: " "
        }

        if(informacionConteo){
            informacionConteo.fuerzas?.forEach((item: any) => {

                const tituloFuerza = {
                    columna1: "FUERZA:",
                    columna2: item?.nombreFuerza
                }
                arrayReporte.push(tituloFuerza);
                arrayReporte.push(tituloEstados);

                if(item?.estados){
                    item?.estados?.forEach((estadoFuerza: any) => {
                        const infoEstados ={
                            columna1: estadoFuerza?.estado,
                            columna2:  UtilService.formatNumber(estadoFuerza?.total)
                        }
                        arrayReporte.push(infoEstados);
                    })
                }

                const totalFuerza = {
                    columna1: "TOTAL FUERZA",
                    columna2:  UtilService.formatNumber(item?.totalFuerza)
                }
                arrayReporte.push(totalFuerza);
                arrayReporte.push(espacio);
           })

           const tituloConteoPorEstado = {
                columna1: "CONTEO POR ESTADO",
                columna2: " "
           }

           arrayReporte.push(tituloConteoPorEstado);
           arrayReporte.push(tituloEstados);

           informacionConteo?.conteoPorEstado?.forEach((conteo: any) => {
                const conteoGeneral = {
                    columna1: conteo?.estado,
                    columna2:  UtilService.formatNumber(conteo?.total)
                }
                arrayReporte.push(conteoGeneral);
           })

           const totalAfiliados = {
                columna1: "TOTAL AFILIADOS",
                columna2: UtilService.formatNumber(informacionConteo?.totalGeneral) 
           }
           arrayReporte.push(totalAfiliados);
        }

        const columnDataList = arrayReporte?.map((row: any) => ({
            data: [
                String(row?.columna1),
                String(row?.columna2)
            ]

        }));
        
        const request = {
            columnDataList: columnDataList.map(item => ({
                data: item.data
            })),
            columnName: [
                "Conteo de Afiliados", " ",
            ]
        };

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

       try{
            const response = await apiReportes.GenerarExcels(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Conteo Afiliados Por Fuerza:${moment().format()}.xlsx`;
            document.body.appendChild(link);
            link.click();
            setLoading(false);
            window.URL.revokeObjectURL(url);

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Generación de Reportes GA2',
                hostname: document.location.host,
                tipoAccion: 'DESCARGAS',
                idTipoAccion: 5,
                descripcion: 'Descarga de archivo Excel reporte GA2'

            } as Auditoria;
            auditoriaApi.InsertAuditoria(requestAuditoria);
           
       }catch(error: any){
            const descripcionError = `Error al generar reporte conteo afiliados.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Generación de Reportes GA2',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            setLoading(false);
            toast('Error al generar reporte conteo afiliados', { type: toast.TYPE.ERROR });
       }
    }

    return (
        <div>
            <CHPaper title="Consulta conteo afiliados">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            control={formularioAfiliadosPorFuerza.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Seleccione para consultar información conteo de afiliados"
                                    value={value}
                                    onChange={e => {
                                        setLoading(true);
                                        onChange(e);
                                        if(informacionConteo){

                                            if(e.target.value !== "CONTEO GENERAL"){
                                                const fuerzaEncontrada = informacionConteo.fuerzas?.find((item:any) => item?.nombreFuerza === e.target.value);
                                                if(fuerzaEncontrada){
                                                    setFuerzaEncontrada(fuerzaEncontrada);
                                                    setLoading(false);
                                                }else{
                                                    setFuerzaEncontrada(null);
                                                    setLoading(false);
                                                    toast(`No se encontraron resultados`);
                                                }
                                            }else if(e.target.value === "CONTEO GENERAL"){
                                                const conteoGeneral = {
                                                    estados: informacionConteo?.conteoPorEstado,
                                                    totalGeneral: informacionConteo?.totalGeneral,
                                                }
                                                setFuerzaEncontrada(conteoGeneral);
                                                setLoading(false);
                                            }
                                        }
                                    }}
                                >
                                    {catalogState.Force.map((option: any) => (
                                        <MenuItem key={option.id} value={option.descripcion}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                      <MenuItem key={0} value="CONTEO GENERAL">
                                           CONTEO GENERAL POR ESTADO
                                      </MenuItem>
                                </CHSelect>
                            )}
                            name="nombreFuerza"
                        />
                    </Grid>
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Button
                                variant="contained"
                                color="primary"
                                className={classes.button}
                                onClick={GenerarExcel}
                            >
                                GENERAR REPORTE
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </CHPaper>
            {fuerzaEncontrada && (
                <ConsultaAfiliadosTable 
                    data={fuerzaEncontrada}
                    datosFormulario={formularioAfiliadosPorFuerza.getValues()} 
                />
            )}
        </div>
    );
};

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    startGetAllExecutingUnits: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllExecutingUnits,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ConsultaAfiliadosPorFuerza);