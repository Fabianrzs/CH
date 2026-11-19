import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    TablePagination,
} from '@material-ui/core';
import { GetApp } from '@material-ui/icons';
import './RegistroNovedades.scss';
import { toast } from 'react-toastify';
import AuthService from '../../services/auth.service';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import ContainerTitle from '../../components/containe-title/container-title';
import CHPaperNew from '../../components/ch-paper-new/CHPaperNew';
import { Auditoria, ConsultaAuditoria, PaginationTable } from '../../types/Auditoria.model.type';
import { startGetLogAuditoria } from '../../actions/auditoria.action';
import ApiAuditoriasNovedades from '../../services/api-registronovedades.service';
import { ConsultarAuditoriaNovedadesDto } from '../../types/registro-novedades.model.type';
import { User } from '../../types/user.type';
import UtilService from '../../utils/util.service';
import CatalogApi from '../../services/api-catalog.service';
import AuditoriaApi from '../../services/api-auditorias.servise'

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
            display: 'flex'
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0px',
        },
        accountButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 1px',
            marginLeft: '.5rem',
            width: '100px'
        },
        icon: {
            color: '#42b7ef',
            paddingRight: '10px',
        },
        alineationRight: {
            margin: '0 5rem 0 60rem',
            position: 'relative',
            display: 'flex'
        },
        actions: {
            margin: '8px 1px',
            width: '120px'
        },
        iconRed: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#FF0000',
            margin: '10px'
        },
        root: {
            flexGrow: 1,
        },
        downloadIcon: {
            display: 'flex',
            justifyContent: 'flex-end',
            alignItems: 'center',
            marginBottom: '10px'
        }
    }),
);

function TableRegistroNovedades({
    users,
    changePagination,
}: {
    users: User[];
    changePagination: (data: PaginationTable) => void
}) {
    const classes = useStyles();
    const api = ApiAuditoriasNovedades();
    const user = AuthService.getUserInfo();

    const [data, setData] = useState<ConsultarAuditoriaNovedadesDto[]>([]);
    const [resultados, setResultados] = useState<ConsultarAuditoriaNovedadesDto[]>([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);

    const fetchData = async () => {
        try {
            const result = await api.getAuditoriaNovedades(page, rowsPerPage);

            if (Array.isArray(result.data)) {
                result.data.sort((a: { fechaModificacion: string | number | Date; }, b: { fechaModificacion: string | number | Date; }) => new Date(b.fechaModificacion).getTime() - new Date(a.fechaModificacion).getTime());
                setData(result.data);
            } else if (result) {
                setData([result.data]);
            } else {
                setData([]);
            }
        } catch (error) {
            console.error('Error al obtener datos', error);
        }
    };


    useEffect(() => {
        fetchData();
    }, [page, rowsPerPage]);

    const [busquedaLog, setBusquedaLog] = useState()

    const handleChangeBuscar = (event: any) => {
        const valorBusqueda = event.target.value.toLowerCase();
        setBusquedaLog(valorBusqueda);

        const resultadosBusqueda = valorBusqueda === '' ? [] : data.filter((row) => {
            const detalleModificacion = (row.detalleModificacion || '').toLowerCase();
            const numeroIdentificacion = (row.nDocumento || '').toString().toLowerCase();
            const fechaHora = (row.fechaModificacion || '').toString().toLowerCase();
            const subModulo = (row.modulo || '').toLowerCase();
            const tipoIdentificacionAfiliado = (String(row.tipoDocumento) || '').toLowerCase();
            const unidadEjecutora = (row.nombreUnidadEjecutora || '').toLowerCase();
            const fechaInicio = (row.fechaInicio || '').toString().toLowerCase();
            const fechaFin = (row.fechaFin || '').toString().toLowerCase();
            const usuarioModificacion = (row.usuarioModificacion || '').toLowerCase();

            return detalleModificacion.includes(valorBusqueda) ||
                numeroIdentificacion.includes(valorBusqueda) ||
                fechaHora.includes(valorBusqueda) ||
                subModulo.includes(valorBusqueda) ||
                tipoIdentificacionAfiliado.includes(valorBusqueda) ||
                unidadEjecutora.includes(valorBusqueda) ||
                fechaInicio.includes(valorBusqueda) ||
                fechaFin.includes(valorBusqueda) ||
                usuarioModificacion.includes(valorBusqueda);
        });
        setResultados(resultadosBusqueda);
    };

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

    const downloadExcel = async () => {
        const api = ApiAuditoriasNovedades();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;
    
        const auditoriaApi = AuditoriaApi();
        try {
            const result = await api.getObtenerReporteNovedades();
            if (result) {

                const byteCharacters = atob(result.data.archivoB64);
                const byteNumbers = new Array(byteCharacters.length);
                for (let i = 0; i < byteCharacters.length; i += 1) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                const byteArray = new Uint8Array(byteNumbers);
                const blob = new Blob([byteArray], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        
                const excelURL = URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = excelURL;
                link.download = result.data.nameFile;
                link.click();
                URL.revokeObjectURL(excelURL);
        
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
                        modulo: 'Log de auditoria administración de novedades',
                        hostname: document.location.host,
                        tipoAccion: 'DESCARGAS',
                        idTipoAccion: 5,
                        descripcion: 'Descarga de Auditoria administración de novedades'
        
                    } as Auditoria;
        
                    auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        }catch (error: any) {
            const descripcionError = `Error en descarga Excel.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;
    
            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Log de auditoria administración de novedades',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };
    
            auditoriaApi.InsertAuditoria(requestAuditoriaError);
    
            toast(`Error al generar reporte excel auditoria administración de novedades: ${error?.message}`, {
                type: toast.TYPE.ERROR
            });
        }
    }

    const handleChangePage = (event: any, newPage: React.SetStateAction<number>) => {
        setPage(newPage);
    };


    const handleChangeRowsPerPage = (event: any) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    return (

        <Grid>
            <ContainerTitle
                title="Log de Auditoria Administración de Novedades" />

            <Grid className="container-24">
                <input type="search" placeholder="Buscar..." onChange={handleChangeBuscar} />
            </Grid>
            <Grid container>

                <Grid item xs={12} className={classes.downloadIcon}>
                    <Button variant='contained' color='primary' startIcon={<GetApp />} onClick={() => downloadExcel()}>
                        Descargar Excel
                    </Button>
                </Grid>
            </Grid>
            {busquedaLog !== '' && resultados.length > 0 && (
                <div>
                    <h2>Resultados de la búsqueda:</h2>
                    <ul>
                        {resultados.map((row, index) => (
                            <li key={`resultado-${index}`}>{row.detalleModificacion} de novedad al documento {row.nDocumento}</li>
                        ))}
                    </ul>
                </div>
            )}

            {busquedaLog !== '' && resultados.length === 0 && (
                <p>No se encontraron resultados con la búsqueda realizada</p>
            )}
            <br/>
            <br/>

            {busquedaLog === '' || resultados.length > 0 ? (
                <Grid className="container-17">
                    <table className="table">
                        <thead>
                            <tr>
                                <th className="tab-space-01">Fecha / Hora</th>
                                <th className="tab-space-01">Submódulo</th>
                                <th className="tab-space-01">Tipo Identificación Afiliado</th>
                                <th className="tab-space-01">Número Identificación Afiliado</th>
                                <th className="tab-space-01">Número Radicado</th>
                                <th className="tab-space-01">Unidad Ejecutora</th>
                                <th className="tab-space-01">Detalle Modificación</th>
                                <th className="tab-space-01">Fecha Inicio</th>
                                <th className="tab-space-01">Fecha Fin</th>
                                <th className="tab-space-01">Fecha Modificación</th>
                                <th className="tab-space-01">Usuario Modificación</th>
                            </tr>
                        </thead>
                        <tbody>
                            {data.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row, index) => (
                                <tr key={index}>
                                    <td data-label="Fecha-Corrección" className="tab-space-01">{UtilService.getFullDate(row.fechaModificacion)}</td>
                                    <td>{row.modulo}</td>
                                    <td>{row.tipoDocumento}</td>
                                    <td>{row.nDocumento}</td>
                                    <td>.</td>
                                    <td>{row.nombreUnidadEjecutora}</td>
                                    <td data-label="Modificacion-Realizada" className="tab-space-01">{row.detalleModificacion}</td>
                                    <td>{UtilService.getFullDate(row.fechaInicio)}</td>
                                    <td>{UtilService.getFullDate(row.fechaFin)}</td>
                                    <td>{UtilService.getFullDate(row.fechaModificacion)}</td>
                                    <td>{`${user.USR_NOMBRE} ${user.USR_SEGUNDONOMBRE} ${user.USR_APELLIDO} ${user.USR_SEGUNDOAPELLIDO}`}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    <TablePagination
                        rowsPerPageOptions={[10, 20, 30, 40]}
                        component="div"
                        count={data.length}
                        rowsPerPage={rowsPerPage}
                        page={page}
                        onPageChange={handleChangePage}
                        onRowsPerPageChange={handleChangeRowsPerPage}
                        labelRowsPerPage="Filas por página"
                    />
                </Grid>
            ) : (
                <Grid className="container-17">
                    <table className="table">
                        <thead>
                            <tr>
                                <th className="tab-space-01">Fecha / Hora</th>
                                <th className="tab-space-01">Submódulo</th>
                                <th className="tab-space-01">Tipo Identificación Afiliado</th>
                                <th className="tab-space-01">Número Identificación Afiliado</th>
                                <th className="tab-space-01">Número Radicado</th>
                                <th className="tab-space-01">Unidad Ejecutora</th>
                                <th className="tab-space-01">Detalle Modificación</th>
                                <th className="tab-space-01">Fecha Inicio</th>
                                <th className="tab-space-01">Fecha Fin</th>
                                <th className="tab-space-01">Fecha Modificación</th>
                                <th className="tab-space-01">Usuario Modificación</th>
                            </tr>
                        </thead>
                        <tbody>
                            {data.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row, index) => (
                                <tr key={index}>
                                    <td data-label="Fecha-Corrección" className="tab-space-01">{UtilService.getFullDate(row.fechaModificacion)}</td>
                                    <td>{row.modulo}</td>
                                    <td>{row.tipoDocumento}</td>
                                    <td>{row.nDocumento}</td>
                                    <td>.</td>
                                    <td>{row.nombreUnidadEjecutora}</td>
                                    <td data-label="Modificacion-Realizada" className="tab-space-01">{row.detalleModificacion}</td>
                                    <td>{UtilService.getFullDate(row.fechaInicio)}</td>
                                    <td>{UtilService.getFullDate(row.fechaFin)}</td>
                                    <td>{UtilService.getFullDate(row.fechaModificacion)}</td>
                                    <td>{`${user.USR_NOMBRE} ${user.USR_SEGUNDONOMBRE} ${user.USR_APELLIDO} ${user.USR_SEGUNDOAPELLIDO}`}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    <TablePagination
                        rowsPerPageOptions={[10, 20, 30, 40]}
                        component="div"
                        count={data.length}
                        rowsPerPage={rowsPerPage}
                        page={page}
                        onPageChange={handleChangePage}
                        onRowsPerPageChange={handleChangeRowsPerPage}
                        labelRowsPerPage="Filas por página"
                    />
                </Grid>
            )}
        </Grid>
    )
}


const RegistroNovedades = () => {

    const classes = useStyles();

    const changePaginationComponente = (data: PaginationTable) => {

        const request = {
            VerDetalle: false,
            PageNum: data.PageNum,
            PageSize: data.Pagesize,
        } as ConsultaAuditoria;

        startGetLogAuditoria(request);

    }


    return (
        <>

            <div className={classes.root}>
                <IconTitle
                    title='Log de Auditoria'
                    subTitle='Administración de Novedades'
                    icon='fch-Administracion-Grupos-Usuarios-0-1'
                />
            </div>

            <CHPaperNew>
                <TableRegistroNovedades
                    changePagination={changePaginationComponente}
                    users={[]}

                />
            </CHPaperNew>
        </>
    );
}

export default (RegistroNovedades);
