import React, {useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    TablePaginationProps,
    TablePagination,
} from '@material-ui/core';
import moment from 'moment';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import { LoadingComponent } from '../../../components/loading/loading.component';
import ApiClients from '../../../services/api-clients.service';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { startGetAllGrades, startGetAllForces, startGetCategory, startGetAllExecutingUnits, startGetAllIdentificationTypes, startGetAllCivilState } from '../../../actions/catalog.action';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import ApiReportes from '../../../services/api-reportes.service';
import CatalogApi from '../../../services/api-catalog.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import SimuladorApi from '../../../services/api-simulador.service';
import { LoginState } from '../../../reducers/login.reducer';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import { GRADEDTO } from '../../../types/catalog.type';
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

const api = ApiClients();
const apiReportes = ApiReportes();

function ConsultaAfiliadosTable({
    data,
    page,
    setPage,
    catalogState,
}: {
    data: any;
    page: number;
    setPage: (value: number) => void;
    catalogState: CatalogState;
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
                            <th className={classes.textTable}>Número identificación</th>
                            <th className={classes.textTable}>Afiliado</th>
                            <th className={classes.textTable}>Fuerza</th>
                            <th className={classes.textTable}>Unidad Ejecutora</th>
                            <th className={classes.textTable}>Grado</th>
                            <th className={classes.textTable}>Catergoría</th>
                            <th className={classes.textTable}>Régimen</th>
                            <th className={classes.textTable}>Fecha Inscripción</th>
                            <th className={classes.textTable}>Tipo Vinculación</th>
                            <th className={classes.textTable}>Estado Civil</th>
                            <th className={classes.textTable}>Cónyuge</th>
                            <th className={classes.textTable}>Estado Afiliado</th>
                        </tr>
                    </thead>
                    <tbody>
                    {data?.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)?.map((element: any, index: any) => {

                        if(element?.identificacion){
                            return(
                                <tr key={`table-${index}`}>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.identificacion}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.afiliado}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.idFuerza}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.idUnidadEjecutora}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.idGrado}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.idCategoria}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.regimen}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{UtilService.getDate(element.fechaInscripcion)}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.tipoVinculacion}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{catalogState.civilstates.find(item => item.id === element.estadoCivil)?.descripcion}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.conyuge === "   "? "N/A" : element.conyuge}</td>
                                    <td data-label="Número Cuenta" className="tab-space-01">{element.estadoCliente}</td>
                                </tr>
                            )
                         }
                         return null
                        })}
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

const ConsultaAfiliados = (props: Props) => {

    const classes = useStyles();
    const apiSimulador = SimuladorApi();
    const {catalogState,
           loginState,
           startGetAllGrades,
           startGetAllForces,
           startGetCategory,
           startGetAllExecutingUnits,
           startGetAllIdentificationTypes,
           startGetAllCivilState
        } = props;

    useEffect(() => {
        startGetAllGrades(loginState.claims)
        startGetAllForces(loginState.claims)
        startGetCategory(loginState.claims)
        startGetAllExecutingUnits(loginState.claims)
        startGetAllIdentificationTypes(loginState.claims)
        startGetAllCivilState(loginState.claims)
        obtenerTiposVinculacion();
        obtenerEstadosAfiliado();
    }, [])

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [gradoValue, setGradoValue] = useState<GRADEDTO | null>(null);
    const [nullGradoValue, setNullGradoValue] = React.useState(false);
    const grados: any[] = catalogState.Grade? catalogState.Grade?.filter((element:any) => element?.descripcion !== "(SIN IDENTIFICAR)"): [];
    const [tiposVinculacion, setTiposVinculacion] = useState([]);
    const [estadosAfiliado, setEstadosAfiliado] = useState([]);
    const [page, setPage] = useState(0);

    const formularioAfiliados = useForm<any>({
        defaultValues: {
            idTipoIdentificacion: 0,
            identificacion: '',
            idFuerza: 0,
            idUnidadEjecutora: 0,
            estado: 0,
            idCategoria: 0,
            idGrado: 0,
            idTipoVinculacion: 0,
            fechaInscripcionDesde: null,
            fechaInscripcionHasta: null,
            fechaActualizacionDesde: null,
            fechaActualizacionHasta: null,
            regimenDesde: 0,
            regimenHasta: 0,
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

    const obtenerTiposVinculacion = () => { 
        apiSimulador.postAllTipoActor()
            .then((response) => {
                const tiposOrdenados = response.data.sort((a: any, b: any) =>
                    a.descripcionTipoActor.localeCompare(b.descripcionTipoActor)
                );
                setTiposVinculacion(tiposOrdenados);
            })
            .catch((error) => {
                console.error('Error al obtener tipos de vinculación:', error);
            });
    };

    const obtenerEstadosAfiliado = () => { 
        catalogApi.getEstadosCliente()
            .then((response) => {
                if(response?.data){
                    setEstadosAfiliado(response?.data);
                }
            })
            .catch((error) => {
                console.error('Error al obtener estados afiliado', error);
            });
    };

    const handleSearchButton = async (datos: any) => {

        setLoading(true);
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        if(datos.regimenDesde === ""){
            datos.regimenDesde = 0;
        }

        if(datos.regimenHasta === ""){
            datos.regimenHasta = 0;
        }

        try {
            const response = await api.getConsultaAfiliados(datos);
            setPage(0);
            setData(response.data);
           
            if(response.data && response?.data.length === 0){
                toast(`No se encontraron resultados`)
            }
           
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
        setLoading(true);
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData?.brands?.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;
    
        try {
            const columnDataList = data?.map((row: any) => ({
                data: [
                    String(row.identificacion) || '',
                    String(row.afiliado),
                    String(row.idFuerza),
                    String(row.idUnidadEjecutora),
                    String(row.idGrado),
                    String(row.idCategoria),
                    String(row.regimen),
                    UtilService.getDateFormat(row.fechaInscripcion),
                    String(row.tipoVinculacion),
                    String(catalogState.civilstates.find(item => item.id === row.estadoCivil)?.descripcion),
                    String(row.conyuge === "   "? "N/A" : row.conyuge),
                    String(row.estadoCliente)
                ]
            }));
    
            const request = {
                columnDataList: columnDataList.map(item => ({ data: item.data })),
                columnName: [
                    "Número identificación",
                    "Nombre", "Fuerza", "Unidad Ejecutora", "Grado", "Categoría",
                    "Régimen", "Fecha Inscripción", "Tipo Vinculación", "Estado Civil",
                    "Cónyuge", "Estado Afiliado"
                ]
            };

            const response = await apiReportes.GenerarExcels(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Consulta-Afiliados:${moment().format()}.xlsx`;
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);
            setLoading(false);
    
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
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
            setLoading(false);
            toast('Error al generar Excel', { type: toast.TYPE.ERROR });
        }
    };

    const onChangeGrado = (event: any, grado: any) => {
        setNullGradoValue(grado == null);
        setGradoValue(grado);
        if (grado) {
            formularioAfiliados.setValue('idGrado', String(grado.id));
        }else{
            formularioAfiliados.setValue('idGrado', 0);
        }
    };
    
    return (
        <div>
            <CHPaper title="Consulta afiliados">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo Identificacion"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                            SELECCIONE...
                                    </MenuItem>
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
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHTextField
                                    defaultValue=""
                                    label="Identificación"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                            name="identificacion"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo Vinculación"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                            SELECCIONE...
                                    </MenuItem>
                                    {tiposVinculacion?.map((option:any) => {
                                        if(option.descripcionTipoActor !== "FUNCIONARIO AFILIADO SOLUCIÓN VIVIENDA" &&
                                           option.descripcionTipoActor !== "FUNCIONARIO AFILIADO ADMINISTRACIÓN CESANTÍAS"){
                                            return(
                                                <MenuItem key={option.idTipoActor} value={option.idTipoActor}>
                                                    {option.descripcionTipoActor}
                                                </MenuItem>
                                            )
                                        }
                                        return null;
                                    })}
                                </CHSelect>
                            )}
                            name="idTipoVinculacion"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="fuerza"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        SELECCIONE...
                                    </MenuItem>
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
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Unidad Ejecutora"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        SELECCIONE...
                                    </MenuItem>
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
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Estado"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                            SELECCIONE...
                                    </MenuItem>
                                    {estadosAfiliado.map((option: any) => (
                                        <MenuItem key={option.firtsId} value={option.firtsId}>
                                            {option.descriptions}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="estado"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                              name="idGrado"
                              control={formularioAfiliados.control}
                              rules={{
                                  required:
                                      'Este campo es requerido.',
                              }}
                              render={({ fieldState: { error } }) => (
                                  <CHAutoComplete
                                      value={
                                          nullGradoValue
                                              ? null
                                              : gradoValue
                                      }
                                      options={grados}
                                      getOptionLabel={option =>
                                          option.descripcion
                                      }
                                      onChange={(
                                          evento: any,
                                          grado: any,
                                      ) => {
                                        onChangeGrado(
                                              evento,
                                              grado,
                                          );
                                      }}
                                      renderInput={params => (
                                          <CHTextField
                                              {...params}
                                              error={!!error}
                                              helperText={
                                                  error
                                                      ? error.message
                                                      : null
                                              }
                                              InputLabelProps={{
                                                  shrink: true,
                                              }}
                                              label="Grado"
                                          />
                                      )}
                                  />
                              )}
                            />
                        </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Categoria"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>
                                        SELECCIONE...
                                    </MenuItem>
                                    {catalogState.categories.map((option: any) => {
                                        if(option.descripcion !== '(SIN IDENTIFICAR)'){
                                            return(
                                                <MenuItem key={option.id} value={option.id}>
                                                    {option.descripcion}
                                                </MenuItem>
                                            )
                                           
                                        }
                                        return null
                                    })}
                                </CHSelect>
                            )}
                            name="idCategoria"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Inscripción Desde"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                            name="fechaInscripcionDesde"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Inscripción Hasta"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                            name="fechaInscripcionHasta"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Actualización desde"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                            name="fechaActualizacionDesde"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHDatePicker
                                    defaultValue=""
                                    label="Fecha Actualización Hasta"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                            name="fechaActualizacionHasta"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={formularioAfiliados.control}
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
                            control={formularioAfiliados.control}
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
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={formularioAfiliados.handleSubmit(handleSearchButton)}
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
            {data?.length > 0 && (
                 <ConsultaAfiliadosTable
                 data={data}
                 page={page}
                 setPage={setPage}
                 catalogState={catalogState}
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
    startGetAllCivilState: (claims: string) => void;
    startGetAllGrades: (claims: string) => void;
    startGetAllForces: (claims: string) => void;
    startGetCategory: (claims: string) => void;
    startGetAllExecutingUnits: (claims: string) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllCivilState,
    startGetAllGrades,
    startGetAllForces,
    startGetCategory,
    startGetAllExecutingUnits,
    startGetAllIdentificationTypes,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ConsultaAfiliados);