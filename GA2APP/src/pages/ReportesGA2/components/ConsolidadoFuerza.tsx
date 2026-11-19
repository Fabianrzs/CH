import React, { useEffect, useState } from 'react';
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
import CHPaper from '../../../components/ch-paper/CHPaper';
import UtilService from '../../../utils/util.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { startGetAllExecutingUnits } from '../../../actions/catalog.action';
import ApiReportes from '../../../services/api-reportes.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import CatalgoApi from '../../../services/api-catalog.service';
import { EXECUTINGUNITDTO } from '../../../types/catalog.type';
import { LoginState } from '../../../reducers/login.reducer';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';

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
        titleform: {
            width: '100%',
            marginLeft: '10px',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        textTable: {
			textAlign: 'center'
		},
    }),
);

const apiReportes = ApiReportes();

function ConsolidadoFuerzasTable({
    informacionUEJ,
    catalogState,
}: {
    informacionUEJ: any;
    catalogState: CatalogState;
}) {

    const classes = useStyles();
    const unidadEjecutora = catalogState?.ExecutingUnit.find((item: EXECUTINGUNITDTO) => item?.id === informacionUEJ[0]?.ueJ_ID);

    return (
        <>
            <Grid container style={{ marginTop: "40px" }}>
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                      UNIDAD EJECUTORA: {unidadEjecutora?.nombre} 
                    </Typography>
            </Grid>
            <br/>
            {
                Object.entries(
                    informacionUEJ.reduce((acc: Record<string, any[]>, item: any) => {
                    const key = String(item.conceptoID);
                    if (!acc[key]) acc[key] = [];
                    acc[key].push(item);
                    return acc;
                    }, {})
                ).map(([conceptoID, items]) => {

                    const arrayItems = items as any[];
                    const concepto = arrayItems?.find((arrayItem: any) => arrayItem?.conceptoID === Number(conceptoID))
                        return(
                                <div key={conceptoID}>
                                        <Typography
                                            className={classes.titleform}
                                            variant="h5"
                                            component="h6"
                                        >
                                            CONCEPTO: {concepto?.nombreConcepto}
                                        </Typography>
                                    <table className="client-table">
                                    <thead>
                                        <tr>
                                        <th className={classes.textTable}>Tipo Cuenta</th>
                                        <th className={classes.textTable}>Total Concepto</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {arrayItems.map((option: any, index: any) => (
                                        <tr key={`${conceptoID}-${index}`}>
                                            {option?.tipoCuenta === 'TOTAL CONCEPTO'? 
                                                <b><td className="tab-space-01">{option?.tipoCuenta}</td></b> :
                                                <td className="tab-space-01">{option?.tipoCuenta}</td>
                                            }
                                            <td className="tab-space-01">
                                                {UtilService.formatCurrency(option.totalConcepto || option.totalGeneralConcepto)}
                                            </td>
                                        </tr>
                                        ))}
                                    </tbody>
                                </table>
                                <br/>
                            </div>
                        );
                  })
                }
                <table className="client-table">
                  <tbody>
                      <tr style={{ fontWeight: "bold", backgroundColor: "#C4C4C4" }}>
                      <td className="tab-space-01">TOTAL UNIDAD EJECUTORA {unidadEjecutora?.nombre}</td>
                      <td className="tab-space-01">
                          {UtilService.formatCurrency(
                            informacionUEJ.reduce((acc: number, item: any) => {
                                if (item.tipoCuenta === "TOTAL CONCEPTO") {
                                return acc + (item.totalConcepto || item.totalGeneralConcepto || 0);
                                }
                                return acc;
                            }, 0)
                          )}
                      </td>
                      </tr>
                  </tbody>
              </table>
        </>
    )
}

const ConsolidadoFuerzas = ({
    catalogState,
    loginState,
    startGetAllExecutingUnits
} : {
    catalogState : CatalogState;
    loginState: LoginState;
    startGetAllExecutingUnits: (props: string) => void;
}) => {

    const classes = useStyles();
    const catalogApi = CatalgoApi();
    useEffect(() => {
        startGetAllExecutingUnits(loginState.claims);
        obtenerConsolidadoFuerza();
    }, [])

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [data, setData] = useState([]);
    const [informacionUEJ, setInformacionUEJ] = useState([]);
    const [loading, setLoading] = useState(false);

    const formulario = useForm<any>({
        defaultValues: {
            unidadEjecutora: 'SELECCIONE...',
        }
    });

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

    const obtenerConsolidadoFuerza = () => {
        setLoading(true);

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        apiReportes.getReporteConsolidadoFuerza()
        .then((response) => {
            if(response?.data){
                setData(response.data);
                setLoading(false);

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
                    descripcion: `Consulta consolidado por fuerzas`
                };

                auditoriaApi.InsertAuditoria(requestAuditoria);
            }else{
                setData([]);
                setLoading(false);
                toast('No se encuentra información');
            }
        }).catch((error: any) => {

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
                descripcion: `Error en la consulta consolidado por fuerzas.
                                - Mensaje API: ${error?.returnMessage || 'No disponible'}`
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);

            setLoading(false);
            toast(`Error en consulta consolidado por fuerza:${error}`, { type: toast.TYPE.ERROR });
        })
    }

    const GenerarExcel = async () => {

        setLoading(true);
        const arrayReporte: any[]= [];

        const tituloTipoCuenta = {
            columna1: "TIPO DE CUENTA",
            columna2: "TOTAL CONCEPTO"
        }

        const espacio ={
            columna1: " ",
            columna2: " "
        }

        const fechaReporte ={
            columna1: 'FECHA REPORTE',
            columna2: UtilService.getFullDateWithTime(String(new Date())),
        }

        arrayReporte.push(fechaReporte);
        arrayReporte.push(espacio);

        Object.entries(
            data.reduce((acc: Record<string, any[]>, unidadEjecutora: any) => {
            const key = String(unidadEjecutora.ueJ_ID);
            if (!acc[key]) acc[key] = [];
            acc[key].push(unidadEjecutora);
            return acc;
            }, {})
        ).forEach(([ueJ_ID, listaUnidadesEjecutoras]) => {
           
            let tituloUnidadEjeAgregado = false;
            const unidadEjecutora = catalogState?.ExecutingUnit.find((unidad: EXECUTINGUNITDTO) => unidad?.id === Number(ueJ_ID));

            Object.entries(
                listaUnidadesEjecutoras.reduce((acc: Record<string, any[]>, concepto: any) => {
                const key = String(concepto.conceptoID);
                if (!acc[key]) acc[key] = [];
                acc[key].push(concepto);
                return acc;
                }, {})
            ).forEach(([conceptoID, listaConceptos]) => {

                const listaConceptosFiltrada = listaConceptos?.filter((item: any) => item?.totalConcepto > 0 || (item?.totalConcepto === null && item?.totalGeneralConcepto > 0));
                const concepto = listaConceptos?.find((con: any) => con?.conceptoID === Number(conceptoID));

                if(tituloUnidadEjeAgregado === false && 
                   listaConceptosFiltrada && 
                   listaConceptosFiltrada.length > 0){

                    tituloUnidadEjeAgregado = true;
                    const tituloUnEj ={
                        columna1: "UNIDAD EJECUTORA:",
                        columna2: unidadEjecutora?.nombre || ''
                    }
                    arrayReporte.push(tituloUnEj);

                }

                if(listaConceptosFiltrada && listaConceptosFiltrada.length > 0){

                    const tituloConcepto ={
                        columna1: "CONCEPTO:",
                        columna2: `${concepto?.nombreConcepto}`
                    }
                    arrayReporte.push(tituloConcepto);
                    arrayReporte.push(tituloTipoCuenta);
                }

                listaConceptosFiltrada.forEach((item: any) => {
                        const valorConcepto = item?.totalConcepto || item?.totalGeneralConcepto
                        const tituloConcepto ={
                            columna1: item?.tipoCuenta,
                            columna2: UtilService.formatCurrency(valorConcepto)
                        }
                        arrayReporte.push(tituloConcepto);
                })
                
                if(listaConceptosFiltrada && listaConceptosFiltrada.length > 0){
                    arrayReporte.push(espacio);
                }
            })

            const listaConceptosPorUnidadEJecutora = data?.filter((conceptorPorUnidad:any) => conceptorPorUnidad?.ueJ_ID === Number(ueJ_ID));
           
            let valorTotalConcepto = 0;
            listaConceptosPorUnidadEJecutora.forEach((item: any) => {
                if(item?.tipoCuenta === 'TOTAL CONCEPTO'){
                    valorTotalConcepto += Number(item?.totalGeneralConcepto) || 0;
                }
            })
            
            if(valorTotalConcepto > 0){
                const totalUnidadEj ={
                    columna1: `TOTAL UNIDAD EJECUTORA ${unidadEjecutora?.nombre}`,
                    columna2:  UtilService.formatCurrency(valorTotalConcepto)
                }
                arrayReporte.push(totalUnidadEj);
                arrayReporte.push(espacio);
            }
        })

        data.reduce((acc: number, item: any) => {
            if (item.tipoCuenta === "TOTAL CONCEPTO") {
            return acc + (item.totalConcepto || item.totalGeneralConcepto || 0);
            }
            return acc;
        }, 0)

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
                "Consolidado por Fuerzas", " ",
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
            link.download = `Consolidado por fuerzas:${moment().format()}.xlsx`;
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
            const descripcionError = `Error al generar reporte consolidado por fuerzas.
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
            toast('Error al generar reporte consolidado por fuerzas', { type: toast.TYPE.ERROR });
       }
    }

    return (
        <div>
            <CHPaper title="Consolidado de Fuerzas">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container>
                <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Unidad Ejecutora"
                                    value={value}
                                    onChange={e => {
                                        const informacion = data?.filter((item: any) => item?.ueJ_ID === e.target.value && (item?.totalConcepto > 0 || (item?.totalConcepto === null && item?.totalGeneralConcepto > 0)));
                                        if(informacion && informacion.length > 0){
                                            setInformacionUEJ(informacion);
                                        }else{
                                            setInformacionUEJ([]);
                                            toast('No se encuentra información');
                                        }
                                        onChange(e);
                                    }}
                                >
                                    {catalogState.ExecutingUnit.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.nombre}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="unidadEjecutora"
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
            {informacionUEJ && informacionUEJ?.length > 0 && (
                <ConsolidadoFuerzasTable
                    informacionUEJ={informacionUEJ}
                    catalogState={catalogState}
                />
            )}
        </div>
    );
};

const mapStateToProps = (state: AppState) => ({
	catalogState: state.catalog,
	loginState: state.login,
});

const mapDispatchToProps = {
	startGetAllExecutingUnits,
};

export default connect(mapStateToProps, mapDispatchToProps)(ConsolidadoFuerzas);