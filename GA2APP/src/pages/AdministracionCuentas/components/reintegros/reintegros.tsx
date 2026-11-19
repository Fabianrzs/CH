import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
    TablePagination,
    TablePaginationProps
} from '@material-ui/core';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { sortBy } from 'lodash';
import CHPageTitle from '../../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CatalogApi from '../../../../services/api-catalog.service';
import ReintegrosApi from '../../../../services/api-reintegros.service';
import UtilService from '../../../../utils/util.service';
import NuevoReintegro from './nuevo-reintegro';
import AplicarReintegro from './aplicar-reintegros';
import AuthService from '../../../../services/auth.service';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import AuditoriaApi from '../../../../services/api-auditorias.servise'
import { Auditoria } from '../../../../types/Auditoria.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
    }),
);

function ReintegrosTable(
    {
        listaReintegros,
        loading,
        onClickItem
    }: {
        listaReintegros: any[],
        loading: boolean,
        onClickItem: (data: any | null) => void;
    }) {
    const classes = useStyles();
    const usuario = AuthService.getUserInfo();
    const rowsPerPageOptions = [10, 20, 50, 100];
    const [page, setPage] = React.useState(0);
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
        <>
            <LoadingComponent loading={loading} />
            <table className="credit-table">
                <thead>
                    <tr>
                        <th style={{ textAlign: 'center' }}>
                            Procesar
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            No. Radicado
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Unidad Ejecutora
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Tipo reintegro
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            No. Documento Afiliado
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Nombres/Apellidos
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Fecha de Solicitud
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Valor Solicitado
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Usuario
                        </th>
                        <th style={{ textAlign: 'center' }}>
                            Estado
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {listaReintegros?.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)?.map((row, index) => (
                        <tr key={`table-${index}`}>
                            <td>
                                <Button
                                    className={classes.processButton}
                                    onClick={() => {
                                        if (row?.estadoReintegro === "PROCESADO") {
                                            onClickItem(row);
                                        } else if (usuario !== null && row.idUsuario === usuario?.USR_ID) {
                                            onClickItem(row);
                                        }
                                    }}
                                >
                                    {row?.estadoReintegro === "PROCESADO" ? "DETALLES" : "CONTINUAR"}
                                </Button>
                            </td>
                            <td>{row.numero_Radicado}</td>
                            <td>{row.unidad_Ejecutora}</td>
                            <td>{row.tipoReintegro}</td>
                            <td>{row.numeroDocumento}</td>
                            <td>{row.nombreAfiliado}</td>
                            <td>{moment(row.fechaCreacion).format('YYYY-MM-DD h:mmA')}</td>
                            <td>{row.valorSueldo === '...' ? row.valorSueldo : UtilService.formatCurrency(row.valorSueldo)}</td>
                            <td>{row.nombreUsuario}</td>
                            <td>{row.estadoReintegro}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            {
                listaReintegros.length > 0 && (
                    <TablePagination
                        component="div"
                        count={listaReintegros.length}
                        page={page}
                        onPageChange={handleChangePage}
                        rowsPerPage={rowsPerPage}
                        labelRowsPerPage="Filas por página"
                        labelDisplayedRows={labelDisplayedRows}
                        onRowsPerPageChange={handleChangeRowsPerPage}
                    />
                )
            }
        </>
    );
}

const Reintegros = () => {
    const classes = useStyles();
    const catalogApi = CatalogApi();
    const reintegrosApi = ReintegrosApi();
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<any[]>([]);
    const [tiposReintegros, setTiposReintegros] = useState<any[]>([]);
    const [listaReintegros, setListaReintegros] = useState<any[]>([]);
    const [selectedData, setSelectedData] = useState<any | null>(null);
    const [informacionReintegro, setInformacionReintegro] = useState(null);
    const [agregarNuevoReintegro, setAgregarNuevoReintegro] = useState(false);
    const [aplicarReintegro, setAplicarReintegro] = useState(false);
    const [loading, setLoading] = useState(false);
    const [numeroRadicadoRespuesta, setNumeroRadicadoRespuesta] = useState('');
    const [loadingInformacionSolicitud, setLoadingInformacionSolicitud] = useState(false);

    const formularioBusquedaReintegros = useForm({
        defaultValues: {
            numeroRadicado: '',
            unidadEjecutora: 0,
            tipoReintegro: 0,
        }
    })

    const formularioInformacionSolicitud = useForm({
        defaultValues: {
            numeroRadicado: '',
            numeroRadicadoRespuesta: '',
            fechaInicio: '',
            fechaFinal: ''
        }
    })

    useEffect(() => {
        obtenerUnidadesEjecutoras();
        obtenerTiposReintegros();
    }, [])

    const agregarNuevoReintegroOnClick = () => setAgregarNuevoReintegro(true);

    const onItemIsSelected = (data: any | null) => {
        setSelectedData(data);
        obtenerReintegrosPorId(data?.id_Reintegro);
        setNumeroRadicadoRespuesta(data?.numero_Radicado_Relacionado);
        formularioInformacionSolicitud.setValue('numeroRadicadoRespuesta', data?.numero_Radicado_Relacionado);
        setAplicarReintegro(true);
    };

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);


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

    const onSubmit = (data: any) => {
        const request = {
            numeroRadicado: String(data.numeroRadicado),
            idUnidadEjecutora: Number(data.unidadEjecutora),
            idTipoReintegro: data.tipoReintegro,
        }
        obtenerListadoReintegros(request);

        let descripcionUnidadEjec = 'No especificada';
        let descripcionTipoReintegro = 'No especificado';

        if (request.idUnidadEjecutora === 10) {
            descripcionUnidadEjec = 'ARMADA NACIONAL'
        } else if (request.idUnidadEjecutora === 17) {
            descripcionUnidadEjec = 'CAJA DE RETIRO DE LAS FFMM'
        } else if (request.idUnidadEjecutora === 2) {
            descripcionUnidadEjec = 'CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICÍA'
        } else if (request.idUnidadEjecutora === 12) {
            descripcionUnidadEjec = 'CAJA SUELDOS DE RETIRO POLICÍA'
        } else if (request.idUnidadEjecutora === 9) {
            descripcionUnidadEjec = 'COMANDO GENERAL FFMM'
        } else if (request.idUnidadEjecutora === 24) {
            descripcionUnidadEjec = 'DIRECCIÓN DE VETERANOS Y REHABILITACIÓN INCLUSIVA'
        } else if (request.idUnidadEjecutora === 20) {
            descripcionUnidadEjec = 'DIRECCIÓN GENERAL MARÍTIMA'
        } else if (request.idUnidadEjecutora === 15) {
            descripcionUnidadEjec = 'DIRECCIÓN GENERAL SANIDAD ARMADA'
        } else if (request.idUnidadEjecutora === 14) {
            descripcionUnidadEjec = 'DIRECCIÓN GENERAL SANIDAD EJÉRCITO'
        } else if (request.idUnidadEjecutora === 16) {
            descripcionUnidadEjec = 'DIRECCIÓN GENERAL SANIDAD FUERZA GENERAL'
        } else if (request.idUnidadEjecutora === 13) {
            descripcionUnidadEjec = 'DIRECCIÓN GENERAL SANIDAD MILITAR'
        } else if (request.idUnidadEjecutora === 6) {
            descripcionUnidadEjec = 'DIRECCIÓN SANIDAD POLICÍA NACIONAL'
        } else if (request.idUnidadEjecutora === 1) {
            descripcionUnidadEjec = 'EJÉRCITO NACIONAL DE COLOMBIA'
        } else if (request.idUnidadEjecutora === 11) {
            descripcionUnidadEjec = 'FUERZA AEROESPACIAL DE COLOMBIA'
        } else if (request.idUnidadEjecutora === 7) {
            descripcionUnidadEjec = 'GABINETE'
        } else if (request.idUnidadEjecutora === 8) {
            descripcionUnidadEjec = 'PENSIONADOS FFMM'
        } else if (request.idUnidadEjecutora === 5) {
            descripcionUnidadEjec = 'PENSIONADOS POLICÍA NACIONAL'
        } else if (request.idUnidadEjecutora === 22) {
            descripcionUnidadEjec = 'POLICÍA BIENESTAR'
        } else if (request.idUnidadEjecutora === 3) {
            descripcionUnidadEjec = 'ARMADA NACIONAL'
        } else if (request.idUnidadEjecutora === 19) {
            descripcionUnidadEjec = 'SOLDADOS ARMADA NACIONAL'
        } else if (request.idUnidadEjecutora === 18) {
            descripcionUnidadEjec = 'SOLDADOS EJERCITO NACIONAL DE COLOMBIA'
        } else if (request.idUnidadEjecutora === 21) {
            descripcionUnidadEjec = 'UNIDAD ADMINISTRATIVA ESPECIAL DE LA JUSTICIA PENAL MILITAR Y POLICIAL'
        }

        if (request.idTipoReintegro === 2) {
            descripcionTipoReintegro = 'BLOQUEOS'
        } else if (request.idTipoReintegro === 3) {
            descripcionTipoReintegro = 'DESBLOQUEOS'
        } else if (request.idTipoReintegro === 1) {
            descripcionTipoReintegro = 'DEVOLUCIÓN DE RECURSOS'
        } else if (request.idTipoReintegro === 4) {
            descripcionTipoReintegro = 'INCONSISTENTES'
        }

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
            modulo: 'Reintegros Unidades Ejecutoras',
            hostname: document.location.host,
            tipoAccion: 'CONSULTAS',
            idTipoAccion: 3,
            descripcion: `Consulta de reintegros con los siguientes filtros:
                  - Número Radicado: ${request.numeroRadicado || 'No especificada'}
                  - Unidad Ejecutora: ${descripcionUnidadEjec || 'No especificado'}
                  - Tipo de Reintegro: ${descripcionTipoReintegro || 'No especificada'}`,
        } as Auditoria;

        auditoriaApi.InsertAuditoria(requestAuditoria);

    };

    const obtenerListadoReintegros = async (request: any) => {
        setLoading(true);
        await reintegrosApi
            .getConsultarListaReintegros(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null) {
                        setListaReintegros(response.data);
                        setLoading(false);
                    } else if (response.data === null) {
                        setListaReintegros([]);
                        setLoading(false);
                        toast("No se encontraron resultados");
                    }
                }
            })
            .catch((e) => {
                setLoading(false);
                toast(`Error consultar listado reintegros ${e.message}`, { type: toast.TYPE.ERROR });
            });
    }

    const obtenerUnidadesEjecutoras = async () => {
        await catalogApi
            .getAllEXECUTINGUNITSRequest()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        const datosOrdenados = sortBy(response.data, 'nombre');
                        setUnidadesEjecutoras(datosOrdenados);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch((e) => {
                toast(`Error consultar unidades ejecutoras ${e.message}`, { type: toast.TYPE.ERROR });

            });
    }

    const obtenerTiposReintegros = async () => {
        await reintegrosApi
            .getTipoReintegro()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setTiposReintegros(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });

                    }
                }
            })
            .catch((e) => {
                toast(`Error consultar tipo de reintegros ${e.message}`, { type: toast.TYPE.ERROR });

            });
    }

    const obtenerReintegrosPorId = async (idReintegro: string) => {
        setLoadingInformacionSolicitud(true);
        await reintegrosApi
            .getObtenerListadoReintegrosPorId(idReintegro)
            .then(async response => {
                if (response.data) {
                    formularioInformacionSolicitud.setValue('numeroRadicado', response?.data.numero_Radicado);
                    formularioInformacionSolicitud.setValue('fechaInicio', moment(response?.data?.fechaCreacion).format('YYYY-MM-DD h:mmA'));
                    const fecha = response?.data?.afiliados[0].fechaFin;
                    if (fecha === '0001-01-01T00:00:00') {
                        formularioInformacionSolicitud.setValue('fechaFinal', '');
                    } else {
                        formularioInformacionSolicitud.setValue('fechaFinal', moment(fecha).format('YYYY-MM-DD h:mmA'));
                    }
                    setInformacionReintegro(response?.data);
                    setLoadingInformacionSolicitud(false);
                }
            })
            .catch((e) => {
                setLoadingInformacionSolicitud(false);
                toast(`Error en consultar reintegros por Id ${e.message}`, { type: toast.TYPE.ERROR });
            });
    };

    if (agregarNuevoReintegro) {
        return (
            <NuevoReintegro
                aplicarReintegro={aplicarReintegro}
                numeroRadicadoRespuesta={numeroRadicadoRespuesta}
                formularioInformacionSolicitud={formularioInformacionSolicitud}
                loadingInformacionSolicitud={loadingInformacionSolicitud}
                setAplicarReintegro={setAplicarReintegro}
                setAgregarNuevoReintegro={setAgregarNuevoReintegro}
                obtenerListadoReintegros={obtenerListadoReintegros}
            />
        )
    }

    if (selectedData && aplicarReintegro) {
        return (
            <AplicarReintegro
                informacionReintegro={informacionReintegro}
                numeroRadicadoRespuesta={numeroRadicadoRespuesta}
                formularioInformacionSolicitud={formularioInformacionSolicitud}
                loadingInformacionSolicitud={loadingInformacionSolicitud}
                setAplicarReintegro={setAplicarReintegro}
                setAgregarNuevoReintegro={setAgregarNuevoReintegro}
                unidadesEjecutoras={unidadesEjecutoras}
                obtenerListadoReintegros={obtenerListadoReintegros}
            />
        )
    }

    return (
        <>
            <CHPrivateContainer>
                <CHPageTitle
                    topText="Reintegros"
                    subText=""
                    title1="Reintegros"
                    title2="Unidades Ejecutoras"
                />
                <CHPaper title="Información Reintegros">
                    <form>
                        <Grid container>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="numeroRadicado"
                                    rules={{
                                        pattern:
                                            /^446-01-20/,
                                    }}
                                    control={formularioBusquedaReintegros.control}
                                    render={({ field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            isReadOnly={false}
                                            error={!!error}
                                            helperText={error ? 'El Número de Radicado debe iniciar en 446-01-20' : null}
                                            value={value}
                                            onChange={e => {
                                                if (e.target.value.length < 30) {
                                                    onChange(
                                                        e.target.value.replace(
                                                            /[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`]/g,
                                                            '',
                                                        ),
                                                    );
                                                }
                                            }}
                                            label="No. Radicado"
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="unidadEjecutora"
                                    control={formularioBusquedaReintegros.control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHSelect
                                            label="Unidad Ejecutora"
                                            value={value}
                                            onChange={onChange}
                                        >
                                            <MenuItem key={0} value={0} >
                                                SELECCIONE...
                                            </MenuItem>
                                            {unidadesEjecutoras?.map((option, key) => (
                                                <MenuItem
                                                    value={option.id}
                                                    key={key}
                                                >
                                                    {option.nombre}
                                                </MenuItem>
                                            ))}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={4}>
                                <Controller
                                    name="tipoReintegro"
                                    control={formularioBusquedaReintegros.control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHSelect
                                            label="Tipo de Reintegro"
                                            value={value}
                                            onChange={onChange}
                                        >
                                            <MenuItem key={0} value={0} >
                                                SELECCIONE...
                                            </MenuItem>
                                            {tiposReintegros?.map((option, key) => (
                                                <MenuItem
                                                    value={option.idTipoReintegro}
                                                    key={key}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ))}

                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </form>
                </CHPaper>
                <div className="buttons">
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={formularioBusquedaReintegros.handleSubmit(onSubmit)}
                    >
                        CONSULTAR
                    </Button>
                    <Button
                        className={classes.button}
                        variant="outlined"
                        onClick={agregarNuevoReintegroOnClick}
                    >
                        AGREGAR REINTEGRO
                    </Button>
                </div>
                <ReintegrosTable
                    listaReintegros={listaReintegros}
                    loading={loading}
                    onClickItem={onItemIsSelected}
                />
            </CHPrivateContainer>
        </>
    );
}

export default Reintegros;
