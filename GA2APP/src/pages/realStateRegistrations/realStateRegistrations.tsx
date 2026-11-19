import React, { useEffect, useState } from 'react';
import { Button, Grid, MenuItem, RadioGroup, FormControlLabel, Radio, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, TablePagination } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { CloudDownload } from '@material-ui/icons';
import { CatalogState } from '../../reducers/catalog.reducer';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import ApiReportes from '../../services/api-reportes.service';
import { AppState } from '../../store/rootReducer';
import CHPaperNew from '../../components/ch-paper/CHPaperV2';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import { IDENTIFICATIONTYPEDTO } from '../../types/catalog.type';
import './realStateRegistrations.scss';
import { MatriculasInmobiliariasFilterDTO, MatriculasInmobiliariasDTO } from '../../types/realStateRegistrations.type';
import { startDeleteRealStateRegistrations, startGetBasicInformation, startGetRealStateRegistrations } from '../../actions/realStateRegistrations.action';
import { startGetAllDepartments, startGetAllCities, startGetRegistrationOffice, startGetAllTypeDocuments } from '../../actions/catalog.action';
import { RealStateRegistrationsState } from '../../reducers/realStateRegistrations.reducer'
import history from '../../utils/history.router';
import { LoadingComponent } from '../../components/loading/loading.component';
import UtilService from '../../utils/util.service';
import ContainerTitle from '../../components/containe-title/container-title';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import NotificationService from '../../services/notification.service';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHAutoComplete from '../../components/ch-auto-complete/CHAutoComplete';
import CatalogApi from '../../services/api-catalog.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import ApiGrupoUsuarios from  '../../services/api-grupoUsuarios.service';

const useStyles = makeStyles((theme) =>
    createStyles({
        root: {
            width: '100%',
            marginTop: theme.spacing(3),
            overflowX: 'auto',
        },
        icon: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#42b7ef',
            margin: '10px'
        },
        iconRed: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#FF0000',
            margin: '10px'
        },
        tableRow: {
            '&:nth-of-type(odd)': {
                backgroundColor: '#D3D3D3',
            },
            '&:nth-of-type(even)': {
                backgroundColor: '#ffffff',
            },
        },
        tableCell: {
            borderBottom: 'none',
        },
        headerCell: {
            backgroundColor: '#fec917',
            borderBottom: 'none',
        },
        downloadButtonContainer: {
            padding: '20px',
            backgroundColor: '#fec917',
        }, container: {
            maxHeight: 800,
        },
        tableHeader: {
            backgroundColor: '#FEC917',
            borderBottom: '1px solid #e0e0e0',
        },
        alternateRow: {
            backgroundColor: '#D3D3D3',
        },
    }),
);

function realStateRegistrations({
    catalogState,
    startGetAllDepartments,
    startGetAllCities,
    startGetRegistrationOffice,
    realStateRegistrationsState,
    startGetRealStateRegistrations,
    startGetBasicInformation,
    startGetAllTypeDocuments,
    startDeleteRealStateRegistrations,
}: {
    catalogState: CatalogState;
    startGetRegistrationOffice: () => void;
    startGetAllDepartments: () => void;
    startGetAllCities: () => void;
    realStateRegistrationsState: RealStateRegistrationsState;
    startGetRealStateRegistrations: (matriculas: MatriculasInmobiliariasFilterDTO) => void;
    startGetBasicInformation: (idMatricula: string) => void;
    startGetAllTypeDocuments: () => void;
    startDeleteRealStateRegistrations: (matricula: MatriculasInmobiliariasDTO) => void;
}): any {

    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const { control, handleSubmit, reset } = useForm<MatriculasInmobiliariasFilterDTO>({
        defaultValues: {
            numeroIdentificacion: '',
            numeroMatricula: '',
            tipoIdentificacion: 0,
            fechaCreacion: null,
            fechaFin: null,
            bloqueo: 3,
            nomOfc: '',
        }
    })

    const tieneAlgunFiltro = (data: MatriculasInmobiliariasFilterDTO): boolean =>
        Boolean(
            data.numeroIdentificacion?.trim() ||
            data.numeroMatricula?.trim() ||
            (data.tipoIdentificacion && data.tipoIdentificacion !== 0) ||
            data.fechaCreacion ||
            data.fechaFin ||
            data.nomOfc?.trim() ||
            (data.bloqueo !== undefined && data.bloqueo !== 3)
        );



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

    const consultarMatriculasinmobiliarias = async (consultaMatriculas: MatriculasInmobiliariasFilterDTO) => {

        if (!tieneAlgunFiltro(consultaMatriculas)) {
            toast.warn('Debe seleccionar al menos un filtro para realizar la consulta');
            return;
        }

        const auditoriaApi = AuditoriaApi();

        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        const request = {

            bloqueo: consultaMatriculas.bloqueo,
            fechaCreacion: consultaMatriculas.fechaCreacion,
            fechaFin: consultaMatriculas.fechaFin,
            nomOfc: consultaMatriculas.nomOfc,
            numeroIdentificacion: consultaMatriculas.numeroIdentificacion,
            numeroMatricula: consultaMatriculas.numeroMatricula,
            tipoIdentificacion: consultaMatriculas.tipoIdentificacion
        }

        try {

            startGetRealStateRegistrations(consultaMatriculas);



            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Matrículas Inmobiliarias',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de Matrículas Inmobiliarias con los siguientes filtros:
                - Tipo de identificación: ${request.tipoIdentificacion || 'No especificada'}
                - Número de identificación: ${request.numeroIdentificacion || 'No especificado'}
                - Número matrícula: ${request.numeroMatricula || 'No especificado'}
                - Fecha Inicio: ${request.fechaCreacion || 'No especificado'}
                - Fecha Fin: ${request.fechaFin || 'No especificado'}
                - Oficina Registro: ${request.nomOfc || 'No especificado'}
                - Check bloqueada o activa: ${request.bloqueo || 'No especificado'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);

            setIsDataLoaded(true);

        } catch (error: any) {

            const descripcionError = `Error en consulta de hoja de vida:
                - Tipo de Identificacion: ${request.tipoIdentificacion || 'No especificado'}
                - Número de ID: ${request.numeroIdentificacion || 'No especificado'}
                - Número de identificación: ${request.numeroIdentificacion || 'No especificado'}
                - Número matrícula: ${request.numeroMatricula || 'No especificado'}
                - Fecha Inicio: ${request.fechaCreacion || 'No especificado'}
                - Fecha Fin: ${request.fechaFin || 'No especificado'}
                - Oficina Registro: ${request.nomOfc || 'No especificado'}
                - Check bloqueada o activa: ${request.bloqueo || 'No especificado'}
                - Error: ${error?.message || 'Error desconocido'}
                - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform || 'No disponible',
                nombreUsuario: user?.USR_USERNAME || 'Desconocido',
                usurioId: user?.USR_ID || 0,
                ip: publicIpAddress || 'IP no disponible',
                modulo: 'Consulta de hoja de vida',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            await auditoriaApi.InsertAuditoria(requestAuditoriaError);

            toast("Ocurrió un error al realizar la consulta. Inténtalo de nuevo.");
        }

    }

    useEffect(() => {
        startGetAllTypeDocuments();
        startGetAllDepartments();
        startGetAllCities();
        startGetRegistrationOffice();

    }, []);

    const onClickExcel = async () => {
        const columnDataList = realStateRegistrationsState?.matriculasInmobiliarias?.map((row) => ({
            data: [
                row.maiNumeroMatricula,
                row.maiDireccion,
                UtilService.getFullDate(row.maiFechaRegistro),
                row.prpTipoIdentificacion ?? 'MATRICULA SIN PROPIETARIO',
                row.prpNumeroIdentificacion ?? 'MATRICULA SIN PROPIETARIO',
                row.prpNombreRazonSocial === '' || row.prpNombreRazonSocial === null ? `${row?.prpPrimerNombre} ${row?.prpSegundoNombre} ${row?.prpPrimerApellido} ${row?.prpSegundoApellido}` : row.prpNombreRazonSocial,
                row.estado || '',
                catalogState.oficinasRegistro.find(x => row.idOficinaRegistro === x.idOficina)?.descripcionOficina || '',
                catalogState.City.find(x => row.dcpId === x.id)?.nombre || '',
                catalogState.departments.find(x => row.dpdId === x.id)?.nombre || '',
            ]
        }));

        if (columnDataList) {
            const request = {
                columnDataList: columnDataList.map(item => ({
                    data: item.data
                })),
                columnName: ["N° Matrícula", "Dirección", "Fecha de Registro", "Tipo de Identificación", "Número de Identificación", "Razón Social / Nombre", "Estado", "Oficina Registro", "Ciudad", "Departamento"]
            };

            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = window.navigator as any;
            const usrAg = nav.userAgentData.brands.find(
                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand;

            try {
                const apiReportes = ApiReportes();
                const response = await apiReportes.GenerarExcels(request);
                const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.download = 'archivo.xlsx';
                document.body.appendChild(link);
                link.click();
                window.URL.revokeObjectURL(url);



                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData.platform,
                    nombreUsuario: user.USR_USERNAME,
                    usurioId: user.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Matrículas Inmobiliarias',
                    hostname: document.location.host,
                    tipoAccion: 'DESCARGAS',
                    idTipoAccion: 5,
                    descripcion: 'Descarga de archivo Excel de matrículas inmobiliarias'

                } as Auditoria;

                auditoriaApi.InsertAuditoria(requestAuditoria);
            } catch (error: any) {
                const descripcionError = `Error en descarga de archivo}.
                - Error: ${error?.message || 'Error desconocido'}
                - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
                - Código HTTP: ${error?.response?.status || 'No disponible'}`;

                const requestAuditoriaError: Auditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData?.platform,
                    nombreUsuario: user?.USR_USERNAME,
                    usurioId: user?.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Matrículas Inmobiliarias',
                    hostname: document.location.host,
                    tipoAccion: 'ERRORES Y EXCEPCIONES',
                    idTipoAccion: 8,
                    descripcion: descripcionError
                };

                auditoriaApi.InsertAuditoria(requestAuditoriaError);

                NotificationService.genericAlert(
                    'Error',
                    'No se pudo generar el archivo.',
                    'error',
                );
            }


        }
    }

    const onClickClean = () => {
        reset();
    }

    return (
        <CHPrivateContainer>
            <LoadingComponent key="loader" loading={realStateRegistrationsState.loading} />
            <PageIndicators topTextNivel1="Matrículas" topTextNivel2="Matrículas Inmobiliarias" />
            <IconTitle title="Gestión De Matrículas" subTitle="Inmobiliarias" icon="fch-Inicio-0-1" />
            <CHPaperNew>
                <Grid container spacing={2}>
                    <Grid item xs={12} sm={6} md={3}>
                        <Controller
                            control={control}
                            name="tipoIdentificacion"
                            render={({ field: { onChange, value } }) => (
                                <CHSelect label="Tipo de Identificación" value={value} onChange={onChange}>
                                    {catalogState.IdentificationType?.map((option: IDENTIFICATIONTYPEDTO) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} sm={6} md={3}>
                        <Controller
                            name="numeroIdentificacion"
                            control={control}
                            rules={{
                                pattern: { value: /^[0-9]+$/g, message: 'Este campo es requerido y debe contener solo números' },
                                minLength: { value: 4, message: 'La longitud mínima del campo es de 4 caracteres' },
                                min: { value: 1, message: 'Número de cédula inválido' },
                            }}
                            defaultValue=""
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    numberMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número de identificación"
                                    onChange={onChange}
                                    value={value}
                                    inputProps={{
                                        maxLength: 12
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} sm={6} md={3}>
                        <Controller
                            name="numeroMatricula"
                            control={control}
                            defaultValue=""
                            render={({ field }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    label="Número de matrícula"
                                    value={field.value}
                                    onChange={(event) => {
                                        const newValue = event.target.value.replace(/\D/g, ''); // Elimina caracteres no numéricos
                                        field.onChange(newValue);
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} sm={6} md={3}>
                        <Controller
                            name="fechaCreacion"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Inicio"
                                    InputLabelProps={{ shrink: true }}
                                    onChange={onChange}
                                    value={value}
                                    disableFuture
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} sm={6} md={3}>
                        <Controller
                            name="fechaFin"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Fin"
                                    InputLabelProps={{ shrink: true }}
                                    onChange={onChange}
                                    value={value}
                                    disableFuture
                                />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} sm={6} md={3}>
                        <Controller
                            name="nomOfc"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <CHAutoComplete
                                    value={
                                        // Busca el objeto correspondiente a partir de la descripción almacenada en 'value'
                                        catalogState.oficinasRegistro.find(option => option.descripcionOficina === value) || null
                                    }
                                    onChange={(event, newValue) => {
                                        onChange(newValue ? newValue.descripcionOficina : '');
                                    }}
                                    options={catalogState.oficinasRegistro}
                                    getOptionLabel={(option) => `${option.descripcionOficina} (${option.codigoOficina})`}
                                    renderInput={(params) => (
                                        <CHTextField
                                            {...params}
                                            label="Oficinas Registro *"
                                        />
                                    )}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} sm={6} md={3}>
                        <Controller
                            name="bloqueo"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <RadioGroup
                                    value={value}
                                    onChange={onChange}
                                    row
                                    aria-label="actualización"
                                    defaultValue="3"
                                    name="row-radio-buttons-group"
                                >
                                    <FormControlLabel value="1" control={<Radio color="primary" />} label="BLOQUEADA" />
                                    <FormControlLabel value="0" control={<Radio color="primary" />} label="ACTIVA" />
                                </RadioGroup>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container justifyContent="flex-end" spacing={2} style={{ marginTop: '20px' }}>
                    <Grid item>
                        <Button variant="contained" color="primary" onClick={handleSubmit(consultarMatriculasinmobiliarias)}>
                            CONSULTAR
                        </Button>
                    </Grid>
                    <Grid item>
                        <Button variant="contained" color="primary" onClick={() => history.push('/AgregarNuevaMatricula')}>
                            AGREGAR
                        </Button>
                    </Grid>
                    <Grid item>
                        <Button variant="contained" onClick={handleSubmit(onClickClean)}>
                            LIMPIAR
                        </Button>
                    </Grid>
                </Grid>
            </CHPaperNew>
            <RealStateRegistrationsTable
                data={realStateRegistrationsState?.matriculasInmobiliarias}
                startGetBasicInformation={startGetBasicInformation}
                isDataLoaded={isDataLoaded}
                catalogState={catalogState}
                handleSubmit={handleSubmit}
                onClickExcel={onClickExcel}
                startDeleteRealStateRegistrations={startDeleteRealStateRegistrations}
            />
        </CHPrivateContainer>
    );
};

function RealStateRegistrationsTable({
    data,
    startGetBasicInformation,
    isDataLoaded,
    catalogState,
    handleSubmit,
    onClickExcel,
    startDeleteRealStateRegistrations,
}: {
    data: any;
    startGetBasicInformation: (idMatricula: string) => void;
    isDataLoaded: boolean;
    catalogState: CatalogState;
    handleSubmit: any;
    onClickExcel: any;
    startDeleteRealStateRegistrations: (matricula: MatriculasInmobiliariasDTO) => void;

}): any {

    const apiGrupoUsuarios = ApiGrupoUsuarios();
    const [matriculaEliminar, setMatriculaEliminar] = React.useState({} as MatriculasInmobiliariasDTO);
    const [gruposUsuario, setGruposUsuario] = useState<any[]>([]);
    const user = AuthService.getUserInfo();

    const obtenerGrupoUsuario = async () => {
        try {
            const grupos = await apiGrupoUsuarios.ObtenerGruposUsuarioPorId(user.USR_ID);
            if (grupos?.data?.length > 0) {
                setGruposUsuario(grupos.data[0]?.grupo);
            } else {
                setGruposUsuario([]);
            }
        } catch (error) {
            toast('Error al obtener grupos de usuario', { type: toast.TYPE.ERROR })
            setGruposUsuario([]);
        }
    };
   
    useEffect(() => {
        async function confirmAlertDelete(matriculaEliminar: any) {
            if (matriculaEliminar.maiId != null) {
                const confirmAlert = await NotificationService.confirmAlert(
                    '¿Desea eliminar esta matrícula?',
                );
                try {
                    if (confirmAlert) {
                        startDeleteRealStateRegistrations(matriculaEliminar);
                    }
                } catch (e) {
                    await NotificationService.genericAlert(
                        'Error en la eliminación',
                        'Ocurrio un error al intentar eliminar, intente de nuevo.',
                        'error',
                    );
                }
            }
        }
        confirmAlertDelete(matriculaEliminar);
    }, [matriculaEliminar]);

    useEffect(() => {
        obtenerGrupoUsuario();
    }, [user.USR_ID]);

    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);

    const handleChangePage = (event: any, newPage: any) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: any) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const emptyRows = rowsPerPage - Math.min(rowsPerPage, data?.length - page * rowsPerPage);
    const classes = useStyles();

    if (!isDataLoaded) {
        return null;
    }

    if ((data == null || data.length === 0) && isDataLoaded) {
        return <div>No se encontraron resultados</div>;
    }

    const onClickDetalle = (matricula: any) => (event: any) => {
        event.preventDefault();
        startGetBasicInformation(matricula.maiId);
    }

    const onClickEliminar = (matricula: any) => (event: any) => {
        event.preventDefault();
        setMatriculaEliminar(matricula);
    }

    

    return (
        <Paper className={classes.root}>
            <CHPaperNew>
                <Grid>
                    <ContainerTitle title="Matrículas Inmobiliarias" />
                </Grid>
                <Grid container justifyContent="flex-end" className={classes.downloadButtonContainer}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={handleSubmit(onClickExcel)}
                        startIcon={<CloudDownload />}
                    >
                        Descargar Excel
                    </Button>
                </Grid>
                <Grid>
                    <TableContainer className={classes.container}>
                        <Table stickyHeader aria-label="sticky table">
                            <TableHead>
                                <TableRow>
                                    <TableCell className={classes.tableHeader}>N° Matrícula</TableCell>
                                    <TableCell className={classes.tableHeader}>Dirección</TableCell>
                                    <TableCell className={classes.tableHeader}>Fecha de Registro</TableCell>
                                    <TableCell className={classes.tableHeader}>Tipo de Identificación</TableCell>
                                    <TableCell className={classes.tableHeader}>No. Identificación</TableCell>
                                    <TableCell className={classes.tableHeader}>Razón Social / Nombre</TableCell>
                                    <TableCell className={classes.tableHeader}>Estado</TableCell>
                                    <TableCell className={classes.tableHeader}>Oficina Registro</TableCell>
                                    <TableCell className={classes.tableHeader}>Ciudad</TableCell>
                                    <TableCell className={classes.tableHeader}>Departamento</TableCell>
                                    <TableCell className={classes.tableHeader}>Acciones</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {data.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row: any, index: any) => (
                                    <TableRow key={index} className={index % 2 === 0 ? classes.alternateRow : ''}>
                                        <TableCell >{row.maiNumeroMatricula}</TableCell>
                                        <TableCell >{row.maiDireccion}</TableCell>
                                        <TableCell >{UtilService.getFullDate(row.maiFechaRegistro)}</TableCell>
                                        <TableCell >{row.prpTipoIdentificacion === "" || row.prpTipoIdentificacion == null ? "MATRÍCULA SIN PROPIETARIO" : row.prpTipoIdentificacion}</TableCell>
                                        <TableCell >{row.prpNumeroIdentificacion === "" || row.prpNumeroIdentificacion == null ? "MATRÍCULA SIN PROPIETARIO" : row.prpNumeroIdentificacion}</TableCell>
                                        <TableCell >{row.prpNombreRazonSocial === "" || row.prpNombreRazonSocial == null ? `${String(row.prpPrimerNombre).toUpperCase() ?? 'MATRICULA SIN PROPIETARIO'} ${String(row.prpSegundoNombre).toUpperCase() ?? ''} ${String(row.prpPrimerApellido).toUpperCase() ?? ''} ${String(row.prpSegundoApellido).toUpperCase() ?? ''}` : String(row.prpNombreRazonSocial).toUpperCase() ?? ''}</TableCell>
                                        <TableCell >{row.estado}</TableCell>
                                        <TableCell >{catalogState.oficinasRegistro.find(x => row.idOficinaRegistro === x.idOficina)?.descripcionOficina}</TableCell>
                                        <TableCell >{catalogState.City.find(x => row.dcpId === x.id)?.nombre}</TableCell>
                                        <TableCell >{catalogState.departments.find(x => row.dpdId === x.id)?.nombre}</TableCell>
                                        <TableCell >
                                            <Grid>
                                               <span aria-label="Detalle" onClick={onClickDetalle(row)}><span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} /> </span>
                                               {gruposUsuario && gruposUsuario.length > 0 && gruposUsuario.includes('GVISP_L') ? <span aria-label="Eliminar" onClick={onClickEliminar(row)}><span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`} /></span> : null} 
                                            </Grid>
                                        </TableCell>
                                    </TableRow>
                                ))}
                                {emptyRows > 0 && (
                                    <TableRow style={{ height: 53 * emptyRows }}>
                                        <TableCell colSpan={11} />
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    <TablePagination
                        rowsPerPageOptions={[10, 25, 50]}
                        component="div"
                        count={data.length}
                        rowsPerPage={rowsPerPage}
                        page={page}
                        onPageChange={handleChangePage}
                        onRowsPerPageChange={handleChangeRowsPerPage}
                        labelRowsPerPage="Filas por página:"
                    />
                </Grid>
            </CHPaperNew>
        </Paper>
    );
};

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    realStateRegistrationsState: state.realStateRegistrations,

});

const mapDispatchToProps = {
    startGetRealStateRegistrations,
    startGetBasicInformation,
    startGetAllTypeDocuments,
    startDeleteRealStateRegistrations,
    startGetAllDepartments,
    startGetAllCities,
    startGetRegistrationOffice,
}

export default connect(mapStateToProps, mapDispatchToProps)(realStateRegistrations);