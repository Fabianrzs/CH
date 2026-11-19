import { React, useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem
} from '@material-ui/core';
import moment from 'moment';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { startGetAllGrades, startGetAllForces, startGetCategory, startGetAllExecutingUnits, startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { CatalogState } from '../../../reducers/catalog.reducer';
import ApiClients from '../../../services/api-clients.service';
import ApiReportes from '../../../services/api-reportes.service';
import { AppState } from '../../../store/rootReducer';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import CatalogApi from '../../../services/api-catalog.service';

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

const api = ApiClients();
const apiReportes = ApiReportes();

function ConsultaDocumentosAfiliadosTable({
    data,
    watch,
    catalogState
}: {
    data: any | null;
    watch: any;
    catalogState: CatalogState;
}) {
    return (
        <div>
            {data != null ? (
                <table className="client-table">
                    <thead>
                        <tr>
                            <th>Tipo Identificación</th>
                            <th>Número identificación</th>
                            <th>Nombre</th>
                            <th>Fuerza</th>
                            <th>Unidad Ejecutora</th>
                            <th>Grado</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data?.map((element: any, index: any) => (
                            <tr key={`table-${index}`}>
                                <td data-label="Número Cuenta" className="tab-space-01">{catalogState.IdentificationType.find(x => x.id === element.idTipoIdentificacion)?.descripcion}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{element.identificacion}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{`${element.primerNombre} ${element.primerApellido}`}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{catalogState.Force.find(x => x.id === element.idFuerza)?.descripcion}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{catalogState.ExecutingUnit.find(x => x.id === element.idUnidadEjecutora)?.nombre}</td>
                                <td data-label="Número Cuenta" className="tab-space-01">{catalogState.Grade.find(x => x.id === element.idGrado)?.descripcion}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : null}
        </div>

    );
}

const ConsultaAfiliados = ({
    catalogState,
    startGetAllGrades,
    startGetAllForces,
    startGetCategory,
    startGetAllExecutingUnits,
    startGetAllIdentificationTypes
}: {
    catalogState: CatalogState;
    startGetAllGrades: () => void;
    startGetAllForces: () => void;
    startGetCategory: () => void;
    startGetAllExecutingUnits: () => void;
    startGetAllIdentificationTypes: () => void;
}) => {
    const classes = useStyles();

    useEffect(() => {
        startGetAllGrades()
        startGetAllForces()
        startGetCategory()
        startGetAllExecutingUnits()
        startGetAllIdentificationTypes()
    }, [])

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);

    const { control, handleSubmit, watch } = useForm<any>({
        defaultValues: {
            idTipoIdentificacion: 0,
            identificacion: '',
            idFuerza: 0,
            idUnidadEjecutora: 0,
            idCategoria: 0,
            estado: 0,
            idGrado: 0,
            unidad: 0,
            fechaInscripcionDesde: Date,
            fechaInscripcionHasta: Date,
            fechaActualizacionDesde: Date,
            fechaActualizacionHasta: Date,
            regimenDesde: 0,
            regimenHasta: 0,
            idTipoAfiliacion: 0
        }
    });

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
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
                        setLocalIpAddress(ip);
                        enviarIp(ip);
                    } else {
                        setLocalIpAddress(ip);
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

    const handleSearchButton = async (datos: any) => {
        setLoading(true)
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const response = await api.getConsultaAfiliados(datos);
            setData(response.data);

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Generación de Reportes GA2',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);

        } catch (error: any) {
            const descripcionError = `Error al consultar afiliados.
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
            toast('Error al consultar afiliados', { type: toast.TYPE.ERROR });
        } finally {
            setLoading(false);
        }
    };

    const GenerarExcel = async () => {
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        try {
            const columnDataList = data?.map((row: any) => ({
                data: [
                    String(catalogState?.IdentificationType.find(x => x.id === row.idTipoIdentificacion)?.descripcion) || '',
                    String(row.identificacion) || '',
                    String(`${row.primerNombre} ${row.primerApellido}`) || '',
                    String(catalogState.Force.find(x => x.id === row.idFuerza)?.descripcion) || '',
                    String(catalogState.ExecutingUnit.find(x => x.id === row.idUnidadEjecutora)?.nombre) || '',
                    String(catalogState.Grade.find(x => x.id === row.idGrado)?.descripcion) || ''
                ]

            }));

            const request = {
                columnDataList: columnDataList.map(item => ({
                    data: item.data
                })),
                columnName: [
                    "Tipo Identificación", "Número identificación",
                    "Nombre", "Fuerza", "Unidad Ejecutora", "Grado"
                ]
            };
            const response = await apiReportes.GenerarExcels(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Consulta-Afiliados:${moment().format()}.csv`;
            document.body.appendChild(link);
            link.click();
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
        } catch (error: any) {
            const descripcionError = `Error al generar Excel.
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
            toast('Error al generar Excel', { type: toast.TYPE.ERROR });
        }

    }

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Consulta de afiliados"
                subText=""
                title1="Reporte de"
                title2="Afiliados"
            />
            <CHPaper title="Consulta afiliados">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo Identificacion"
                                    value={value}
                                    onChange={onChange}
                                >
                                    {catalogState.IdentificationType.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="idTipoIdentificacion"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHTextField
                                    defaultValue=""
                                    label="Identificacion"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="identificacion"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="fuerza"
                                    value={value}
                                    onChange={onChange}
                                >
                                    {catalogState.Force.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="idFuerza"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Unidad Ejecutora"
                                    value={value}
                                    onChange={onChange}
                                >
                                    {catalogState.ExecutingUnit.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.nombre}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="idUnidadEjecutora"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Categoria"
                                    value={value}
                                    onChange={onChange}
                                >
                                    {catalogState.categories.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="idCategoria"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHTextField
                                    defaultValue=""
                                    label="Unidad"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="unidad"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Inscripción Desde"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="fechaInscripcionDesde"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Inscripcion Hasta"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="fechaInscripcionHasta"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Actualización desde"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="fechaActualizacionDesde"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHTextField
                                    defaultValue=""
                                    label="Regimen Desde"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="regimenDesde"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHTextField
                                    defaultValue=""
                                    label="Regimen Hasta"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="regimenHasta"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={handleSubmit(handleSearchButton)}
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
                            GENERAR EXCEL
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
            <ConsultaDocumentosAfiliadosTable
                data={data}
                watch={watch}
                catalogState={catalogState}
            />
        </CHPrivateContainer>
    );
};

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
    loginState: state.login,
});

const mapDispatchToProps = {
    startGetAllGrades,
    startGetAllForces,
    startGetCategory,
    startGetAllExecutingUnits,
    startGetAllIdentificationTypes
};

export default connect(mapStateToProps, mapDispatchToProps)(ConsultaAfiliados);
