import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    // createMuiTheme,
} from '@material-ui/core';
// import { blue } from '@material-ui/core/colors';
import { GetApp } from '@material-ui/icons';
import './RegistroEmbargos.scss';
import AuthService from '../../services/auth.service';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import ContainerTitle from '../../components/containe-title/container-title';
import CHPaperNew from '../../components/ch-paper-new/CHPaperNew';
import TablePagination from '../../components/table-pagination/TablePagination';
import { ConsultaAuditoria, PaginationTable } from '../../types/Auditoria.model.type';
import { startGetLogAuditoria } from '../../actions/auditoria.action';
import ApiAuditoriasNovedades from '../../services/api-registronovedades.service';
import { ConsultarAuditoriaNovedadesDto } from '../../types/registro-novedades.model.type';
import { User } from '../../types/user.type';
import UtilService from '../../utils/util.service';

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

function TableRegistroEmbargos({
    users,
    log,
    changePagination,
}: {
    users: User[];
    log: any;
    changePagination: (data: PaginationTable) => void
}) {
    const classes = useStyles();
    

    const [items, setItems] = useState<User[]>();
    const [numeroPaginas, setNumeroPaginas] = useState<number>();
    const [numeroActual, setNumeroPaginaActual] = useState<number>();
    const user = AuthService.getUserInfo();
    const apiAuditoriaNovedades = ApiAuditoriasNovedades();
    const [auditoriaNovedad, setAuditoriaNovedad] = useState<ConsultarAuditoriaNovedadesDto | null>(
        null,
    );

    const [data, setData] = useState<ConsultarAuditoriaNovedadesDto[]>([]);
    const [busqueda, setBusqueda] = useState('');
    const [resultados, setResultados] = useState<ConsultarAuditoriaNovedadesDto[]>([]);


    const fetchData = async () => {
        try {
            const api = ApiAuditoriasNovedades();
            const result = await api.getAuditoriaNovedades();

            if (Array.isArray(result.data)) {
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
    }, []);

    const handleChangeBuscar = (event: any) => {

        const valorBusqueda = event.target.value;
        setBusqueda(valorBusqueda);

        if (valorBusqueda === '') {
            setResultados([]);
            return;
        }

        const resultadosBusqueda = data.filter((row) =>
            row.detalleModificacion.toLowerCase().includes(valorBusqueda.toLowerCase())
        );

        setResultados(resultadosBusqueda);

    };

    const filterItems = (value: string) => {
        if (!log || !log.embargos) {
            return [];
        }
        const filter = log.embargos.filter(
            (x: {
                modificacionRealizada: string;
                novedadDatosCorregida: string;
                nombreUsuario: string;
            }) =>
                x.modificacionRealizada?.toUpperCase().includes(value) ||
                x.nombreUsuario?.toUpperCase().includes(value)

        );
        return filter;
    };

    const downloadExcel = async () => {
        debugger
        const api = ApiAuditoriasNovedades();
        const result = await api.getObtenerReporteNovedades();

        const byteCharacters = atob(result.data.archivoB64);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i+=1) {
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
    }


    return (
        
        <Grid>
            <ContainerTitle
                title="Log Despachos Judiciales" />

            <Grid className="container-24">
                <input type="search" placeholder="Buscar.." onChange={handleChangeBuscar} />
            </Grid>
            
            {resultados.length > 0 ? (
                <div>
                    <h2>Resultados de la busqueda:</h2>
                    <ul>
                        {resultados.map((row, index) => (
                            <li key={`resultado-${index}`}>{row.detalleModificacion}</li>
                            
                        ))}
                    </ul>
                </div>
            ) : (
                busqueda !== '' && <p>No se encontraron resultados con la busqueda realizada</p>
            )}
            <Grid className="container-17">
                <table className="table">
                    <thead>
                        <th className="tab-space-01">Fecha / Hora</th>
                        <th className="tab-space-01">Usuario</th>
                    </thead>
                    <tbody>
                        {data
                        ?.sort((a, b) => new Date(b.fechaModificacion).getTime() - new Date(a.fechaModificacion).getTime())
                        .map((row, index) => (
                            <tr key={`table-${index}`}>
                                <td data-label="Fecha-Cprrecion" className="tab-space-01">{new Date(row.fechaModificacion).toLocaleString()}</td>
                                <td>{`${user.USR_NOMBRE} ${user.USR_SEGUNDONOMBRE} ${user.USR_APELLIDO} ${user.USR_SEGUNDOAPELLIDO}`}</td>
                            </tr>
                        ))}

                    </tbody>
                </table>

                <TablePagination
                    paginaActual={numeroActual}
                    numPaginas={numeroPaginas}
                    numFilas={[10, 20, 50, 100]}
                    changePagination={changePagination}

                />
            </Grid>
        </Grid>
    )
}





const RegistroEmbargos = () => {

    const classes = useStyles();
    const [embargos, setEmbargos] = useState();
    
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
                    title='Log'
                    subTitle='de Despachos Judiciales'
                    icon='fch-Administracion-Grupos-Usuarios-0-1'
                />
            </div>

            <CHPaperNew>
                <TableRegistroEmbargos
                    log={setEmbargos}
                    changePagination={changePaginationComponente} 
                    users={[]}                    
                    
                />
            </CHPaperNew>
        </>
    );
}

export default (RegistroEmbargos);
