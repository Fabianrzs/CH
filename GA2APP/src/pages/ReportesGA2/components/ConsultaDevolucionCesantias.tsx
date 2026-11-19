import React, { useState, useEffect} from 'react';
import { Grid, createStyles,  makeStyles, MenuItem, Button, TablePaginationProps, TablePagination } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { AppState } from '../../../store/rootReducer';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import ApiCatalgo from '../../../services/api-catalog.service';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import AuthService from '../../../services/auth.service';
import ReportesApi from '../../../services/api-reportes.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import { LoadingComponent } from '../../../components/loading/loading.component';
import UtilService from '../../../utils/util.service';

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
        textTable: {
			textAlign: 'center'
		}
    }),
);

function ConsultaDevolucionCesantiasTable({
    data,
    page,
    setPage,
}: {
    data: any;
    page: number;
    setPage: (value: number) => void;
}) {

    const classes = useStyles();
    const rowsPerPageOptions = [10, 20, 50, 100];
	const [rowsPerPage, setRowsPerPage] = React.useState(rowsPerPageOptions[0]);
    const handleChangePage = (
		event: React.MouseEvent<HTMLButtonElement> | null,
		newPage: number,
	) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (
		event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
	) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

	const labelDisplayedRows: TablePaginationProps['labelDisplayedRows'] = ({ from, to, count }) => `${from}-${to} de ${count}`;

    return (
        <div>
            {data.length > 0 ? (
                <table className="client-table">
                    <thead>
                        <tr>
                            <th className={classes.textTable}>Fuerza</th>
                            <th className={classes.textTable}>Unidad Ejecutora</th>
                            <th className={classes.textTable}>Catergoría</th>
                            <th className={classes.textTable}>Grado</th>
                            <th className={classes.textTable}>Tipo Actor</th>
                            <th className={classes.textTable}>Cesantías</th>
                            <th className={classes.textTable}>Tipo Solicitud</th>
                            <th className={classes.textTable}>Tipo Modalidad</th>
                            <th className={classes.textTable}>NEP</th>
                            <th className={classes.textTable}>TOTAL AFILIADOS</th>
                        </tr>
                    </thead>
                    <tbody>
                    {data?.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)?.map((element: any, index: any) => (
                            <tr key={`table-${index}`}>
                                <td data-label="Número Cuenta" className="tab-space-01">{element?.idFuerza}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{element?.idUnidadEjecutora}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{element?.idCategoria}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{element?.idGrado}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{element?.tipoActor}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{UtilService.formatCurrency(element?.caC_VALOR_DESCONTADO)}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{element?.tipoSolicitud}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{element?.tipoModalidad}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{' '}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{' '}</td>

                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : null}
            {
				data.length > 0 && (
					<TablePagination
						component="div"
						count={data.length}
						page={page}
						onPageChange={handleChangePage}
						rowsPerPage={rowsPerPage}
						labelRowsPerPage="Filas por página"
						labelDisplayedRows={labelDisplayedRows}
						onRowsPerPageChange={handleChangeRowsPerPage}
					/>
				)
			}
        </div>

    );
}

const ConsultaAfiliadosDevolucionCesantias = () => {

    const classes = useStyles();
    const apiCatalogo = ApiCatalgo();
    const apiReportes = ReportesApi();
    const [tipoCuenta,setTipoCuenta] = useState([]);
    const [page, setPage] = useState(0);
    const [loading, setLoading] = useState(false);
    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [informacionReporte, setInformacionReporte] = useState([]);
   
    const formulario = useForm<any>({
        defaultValues:{
            tipoCuenta: 'CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA',
            fechaRadicacionDesde: null,
            fechaRadicacionHasta: null,
        }
    })

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
        await apiCatalogo
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

    useEffect(() => {
        obtenerTipoCuenta();
    }, [])

    const obtenerTipoCuenta = async() => {
        await apiCatalogo.getAllTipoCuenta().
            then((response: any)  => {
                if(response?.data){
                    setTipoCuenta(response?.data);
                }
            }).catch(e => {
                toast('Error al consultar tipo cuenta', { type: toast.TYPE.ERROR });
            });
    }

    const handleSearchButton = async (data: any) => {

        setLoading(true);

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        const request = {
            tipoCuenta: data.tipoCuenta === 'SELECCIONE...'? null : data.tipoCuenta,
            fechaRadicacionInicial: data.fechaRadicacionDesde,
            fechaRadicacionFinal: data.fechaRadicacionHasta
        }

        await apiReportes.reporteAfiliadoDevolucionCesantias(request).
        then((response: any)  => {
            if(response?.data?.afiliados && response?.data?.afiliados.length > 0){
                setInformacionReporte(response?.data?.afiliados);
                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Generación de Reportes GA2',
                    hostname: document.location.host,
                    tipoAccion: 'CONSULTAS',
                    idTipoAccion: 3
                } as Auditoria;
        
                auditoriaApi.InsertAuditoria(requestAuditoria);
                setLoading(false);
            }else{
                setInformacionReporte([]);
                toast(`No se encontraron resultados`);
                setLoading(false);
            }
        }).catch((error: any) => {
            const descripcionError = `Error al consultar afiliados con devolución de cesantías.
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
            toast('Error al consultar reporte afiliados con devolución cesantías', { type: toast.TYPE.ERROR });
        });
        
    }

    const GenerarExcel = async () => {
       
        if(!informacionReporte || informacionReporte?.length === 0){
            toast(`No se encontraron resultados`);
            return;
       }

        setLoading(true);
        const columnDataList = informacionReporte?.map((row: any) => ({
            data: [
                String(row.idFuerza) || '',
                String(row.idUnidadEjecutora),
                String(row.idCategoria),
                String(row.idGrado),
                String(row.tipoActor),
                String(UtilService.formatCurrency(row?.caC_VALOR_DESCONTADO)),
                String(row.tipoSolicitud),
                String(row.tipoModalidad),
                "",
                "",
             ]
        }));

        const request = {
            columnDataList: columnDataList.map((item:any) => ({ data: item.data })),
            columnName: [
                "Fuerza", "Unidad Ejecutora",
                "Categoría", "Grado", "Tipo Actor", "Censatías", "Tipo Solicitud",
                "Tipo Modalidad", "Nep", "Total Afiliados"
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
            link.download = `Devolución cesantías:${moment().format()}.xlsx`;
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
            toast('Error al generar reporte devolución cesantías', { type: toast.TYPE.ERROR });
       }
    }

    return(
        <>
        <CHPaper title="Consulta Afiliados con Devolución de Cesantías">
             <LoadingComponent key="loader" loading={loading} />
             <Grid container>
                <Grid className={classes.column} item xs={12} md={4}>
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
                                    label="Tipo Cuenta"
                                    value={value}
                                    onChange={onChange}
                                >
                                    {tipoCuenta.map((option: any) => {
                                            if(option.idTipoCuenta !== 7 && option.idTipoCuenta !== 9){
                                                return(
                                                    <MenuItem key={option.idTipoCuenta} value={option.descripcionTipoCuenta}>
                                                        {option.descripcionTipoCuenta}
                                                    </MenuItem>
                                                )
                                            }
                                            return null
                                        }
                                        
                                    )}
                                </CHSelect>
                            )}
                            name="tipoCuenta"
                        />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formulario.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Radicación Desde"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                            name="fechaRadicacionDesde"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formulario.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Radicación Hasta"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                            name="fechaRadicacionHasta"
                        />
                    </Grid>
             </Grid>
             <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={formulario.handleSubmit(handleSearchButton)}
                        >
                            CONSULTAR
                        </Button>
                    </Grid>
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
        </CHPaper>
        <ConsultaDevolucionCesantiasTable 
            data={informacionReporte} 
            page={page} 
            setPage={setPage}
        />
    </>
    )
}


export default ConsultaAfiliadosDevolucionCesantias;