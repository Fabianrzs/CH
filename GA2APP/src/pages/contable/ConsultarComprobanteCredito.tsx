import { Button, Checkbox, CircularProgress, createStyles, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableFooter, TableHead, TablePagination, TableRow, Tooltip, Typography } from '@material-ui/core';
import { Alert, AlertTitle } from '@material-ui/lab';
import React, { useEffect, useMemo, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import MUIDataTable from 'mui-datatables';
import { CloudDownload, Delete, Edit, Search, Share, ShareOutlined } from '@material-ui/icons';
import { CatalogState } from '../../reducers/catalog.reducer';
import { ProductState } from '../../reducers/producto.reducer';
import { CreditState } from '../../reducers/credit.reducer';

import {
    startGetAllIdentificationTypes,
    startGetAllCities,
    startGetAllDepartments,
    startGetConceptosCartera,
    startGetTipoComprobante
} from '../../actions/catalog.action';
import {
    startGetProductoByIdentification,
    startPostCrearComprobanteCartera,
    startPostObtenerComprobanteCartera,
    startPostActualizarMovimientoComprobante,
    startPostObtenerValorCdp,

} from '../../actions/producto.action';
import {
    startObtenerComprobantesCredito

} from '../../actions/credit.actions';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import { LoginState } from '../../reducers/login.reducer';
import { AppState } from '../../store/rootReducer';
import { CITYDTO, DEPARTAMENTODTO, IDENTIFICATIONTYPEDTO, MOVIMIENTOS } from '../../types/catalog.type';
import ContainerTitle from '../../components/containe-title/container-title';
import ModalEditarMovimientoCartera from './components/ModalEditarMovimiento';
import { LoadingComponent } from '../../components/loading/loading.component';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import ApiReportes from '../../services/api-reportes.service';
import AuthService from '../../services/auth.service';
import { Auditoria } from '../../types/Auditoria.model.type';
import AuditoriaApi from '../../services/api-auditorias.servise';
import CatalogApi from '../../services/api-catalog.service';



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
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        totales: {
            textAlign: 'right'
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr auto',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
            marginBottom: '2rem',

        },
        buttonContainer: {
            display: 'flex',
            justifyContent: 'center', // Centra los botones horizontalmente
            marginTop: '1rem'
        }

    }),
);

const ConsultarComprobanteCredito = (props: Props) => {

    const classes = useStyles();
    const apiReportes = ApiReportes();

    const {
        catalogState,
        loginState,
        productState,
        creditState,
        startGetAllIdentificationTypes,
        startGetProductoByIdentification,
        startGetAllCities,
        startGetAllDepartments,
        startPostCrearComprobanteCartera,
        startPostActualizarMovimientoComprobante,
        startPostObtenerComprobanteCartera,
        startGetConceptosCartera,
        startGetTipoComprobante,
        startGetClientByIdentificationTypeIdentificationNumber,
        startPostObtenerValorCdp,
        startObtenerComprobantesCredito
    } = props;

    const columns = [
        {
            name: 'cuentA_CONTABLE',
            label: 'Cuenta Contable',
            options: {
                display: creditState.comprobanteCredito[0]?.toP_CODI === 9077
                    || creditState.comprobanteCredito[0]?.toP_CODI === 9006,
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'toP_CODI',
            label: 'Tipo de Operación',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'identificacion',
            label: 'Numero Documento',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'nombrE_AFILIADO',
            label: 'Nombre',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'fechA_CREACION',
            label: 'Fecha Comprobante',
            options: {
                customBodyRender: (value: any) => new Date(value).toISOString().split('T')[0],
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'caD_MCO_DESC',
            label: 'Descripcion',
            options: {
                display: creditState.comprobanteCredito[0]?.toP_CODI === 9077
                    || creditState.comprobanteCredito[0]?.toP_CODI === 9006,
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'dfA_VALO',
            label: 'Valor Factura',
            options: {
                display: creditState.comprobanteCredito[0]?.toP_CODI === 4133,
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'soC_DESEMBOLSO_APLICADO',
            label: 'Valor Desembolso',
            options: {
                display: creditState.comprobanteCredito[0]?.toP_CODI === 3015,
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'mpR_CONT',
            label: creditState.comprobanteCredito?.[0]?.toP_CODI === 3015 ? 'Numero de CDP' : 'Numero Comprobante',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'debito',
            label: 'Debito',
            options: {
                display: creditState.comprobanteCredito[0]?.toP_CODI === 9077
                    || creditState.comprobanteCredito[0]?.toP_CODI === 9006,
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'credito',
            label: 'Credito',
            options: {
                display: creditState.comprobanteCredito[0]?.toP_CODI === 9077
                    || creditState.comprobanteCredito[0]?.toP_CODI === 9006,
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
    ];


    const [downloading, setDownloading] = useState(false);
    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [opciones, setOpciones] = useState(0);



    const { control, clearErrors, register, getValues, setValue, handleSubmit, reset, ...rest } = useForm({
        defaultValues: {
            numeroComprobante: 0,
            fechaComprobante: new Date(),
            tipoComprobante: '',
            departamento: '',
            ciudad: '',
            concepto: '',
            beneficiario: '',
            tipoIdentificacion: '',
            identificacion: 0,
            NombreBeneficiario: '',
            fechaInicio: new Date(),
            fechaFinal: new Date(),

        }
    });

    const formularioActualizarMovimiento = useForm({
        defaultValues: {
            movimiento_Comprobante_CarteraId: 0,
            comprobante_CarteraId: 0,
            cuenta_Contable: 0,
            descripcion_Cuenta: '',
            centro_Costo: 0,
            detalle: 0,
            tipo: '',
            monto: 0
        }
    })

    const [data, setData] = useState<any[]>([]);
    const [dataLista, setDataLista] = useState<any[]>([]);
    const [listasContables, setListasContables] = useState<any>({ items: [] });
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);
    const [selectedItems, setSelectedItems] = useState<number[]>([]);

    useEffect(() => {
        startGetTipoComprobante()
    }, []);

    const tipoComprobanteCredito = catalogState.tipoComprobante.filter(item => item.tipoContabilidad === 2)

    const dataTabla = creditState.comprobanteCredito.map((option: any) => ({
        cuentA_CONTABLE: option.cuentA_CONTABLE,
        toP_CODI: option.toP_CODI,
        identificacion: option.identificacion,
        nombrE_AFILIADO: option.nombrE_AFILIADO,
        fechA_CREACION: option.fechA_CREACION,
        caD_MCO_DESC: option.caD_MCO_DESC,
        dfA_VALO: option.dfA_VALO,
        soC_DESEMBOLSO_APLICADO: option.soC_DESEMBOLSO_APLICADO,
        mpR_CONT: option.mpR_CONT,
        debito: option.debito,
        credito: option.credito,
    }));


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
        console.log({ convertIp })
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



    const buscarComprobante = (data: any) => {
        setListasContables([])

        const request = {

            fechA_FIN: moment(data.fechaFinal).format("YYYY-MM-DD"),
            fechA_INICIO: moment(data.fechaInicio).format('YYYY-MM-DD'),
            tcC_CODIGO: data.tipoComprobante,
        }
        startObtenerComprobantesCredito(request)

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
            modulo: 'Búsqueda de Comprobantes',
            hostname: document.location.host,
            tipoAccion: 'CONSULTAS',
            idTipoAccion: 3,
            descripcion: `Consulta de comprobantes con los siguientes filtros:
            \n Fecha Inicio: ${request.fechA_INICIO || 'No especificada'}
            \n Fecha Final: ${request.fechA_FIN || 'No especificada'}
            \n Tipo de Comprobante: ${request.tcC_CODIGO || 'No especificado'}`,
        } as Auditoria;

        auditoriaApi.InsertAuditoria(requestAuditoria);

    };
    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };

    const handleActualizarMovimiento = async (data: any) => {
        startPostActualizarMovimientoComprobante(data);
        handleCloseModalAgregar();
    };


    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleExportToExcel();
        } finally {
            setDownloading(false);
        }
    }

    const handleExportToExcel = async () => {

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;
        
        try {
            const columnDataList = dataTabla?.map((row: any) => ({
                data: columns.map(x => row[x.name]?.toString() ?? '')
            }));


            if (columnDataList) {
                const request = {
                    columnData: columnDataList.map((item: any) => item.data),
                    columnsInfo: columns.map(x => ({
                        name: x.label,
                    }))
                };

                const response = await apiReportes.GenerarExcelInvariantCulture(request);
                const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.download = `Comprobante credito.xlsx`;
                document.body.appendChild(link);
                link.click();
                window.URL.revokeObjectURL(url);
                document.body.removeChild(link);

                const requestAuditoria = {
                    navegador: usrAg,
                    sistemaOperativo: nav.userAgentData.platform,
                    nombreUsuario: user.USR_USERNAME,
                    usurioId: user.USR_ID,
                    ip: publicIpAddress,
                    modulo: 'Consulta Comprobantes Credito',
                    hostname: document.location.host,
                    tipoAccion: 'EXPORTACIÓN',
                    idTipoAccion: 4,
                    descripcion: `Se exportó un archivo Excel de comprobantes.`,
                } as Auditoria;

                auditoriaApi.InsertAuditoria(requestAuditoria);

            }
        } catch (error: any) {

            const descripcionError = `Error al consultar comporbantes.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta Comprobantes Credito',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast("Ocurrió un error al intentar exportar a Excel. Por favor, inténtalo de nuevo.");
        }
    };

    return (
        <CHPrivateContainer>
            <PageIndicators
                topTextNivel1='Comprobantes'
                topTextNivel2='Contabilidad Credito'
            />
            <LoadingComponent key="loader" loading={creditState.loading} />

            <IconTitle
                title='Consulta'
                subTitle='Comprobantes Credito'
                icon='fch-Aprobar-Cargue-Nomina-0-1'
            />

            <CHPaper title="Datos de Comprobante">

                <ContainerTitle title="CONSULTAR COMPROBANTE" />

                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="tipoComprobante"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Tipo de comprobante*"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {tipoComprobanteCredito?.map(
                                        (option: any) => (
                                            <MenuItem
                                                key={option.tipoComprobanteId}
                                                value={option.codigo}
                                            >
                                                {option.tipoComprobante}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="fechaInicio"
                            // rules={{
                            //     required: 'Este campo es requerido',

                            // }}
                            control={control}
                            render={({
                                field: { onChange, value },

                            }) => (
                                <CHDatePicker
                                    label="Fecha Inicial"
                                    inputProps={{ readOnly: true }}
                                    onChange={onChange}
                                    value={value}
                                // autoOk
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="fechaFinal"
                            // rules={{
                            //     required: 'Este campo es requerido',

                            // }}
                            control={control}
                            render={({
                                field: { onChange, value },

                            }) => (
                                <CHDatePicker
                                    label="Fecha Final"
                                    inputProps={{ readOnly: true }}
                                    onChange={onChange}
                                    value={value}
                                // autoOk
                                />
                            )}
                        />
                    </Grid>

                    <Grid container justifyContent="center" className={classes.buttonContainer}>
                        <Button
                            className={classes.buttonAdd}
                            type="submit"
                            variant="outlined"
                            onClick={handleSubmit(buscarComprobante)}
                            startIcon={<Search />}
                            style={{ backgroundColor: '#FFD33A', color: 'black' }} // Amarillo pálido y oscuro
                        >
                            BUSCAR
                        </Button>
                    </Grid>

                </Grid>

                <ContainerTitle title="MOVIMIENTOS DE COMPROBANTE" />


                <Grid container>
                    <MUIDataTable
                        title="Comprobante Detalles"
                        data={dataTabla}
                        columns={columns}
                        options={{
                            selectableRows: 'none',
                            filter: false,
                            print: false,
                            search: true,
                            elevation: 2,
                            filterType: 'dropdown',
                            responsive: 'vertical',
                            fixedHeader: true,
                            tableBodyHeight: '500px',
                            textLabels: {
                                body: {
                                    noMatch: 'No se encontraron resultados',
                                },
                                pagination: {
                                    rowsPerPage: 'Filas por página',
                                },
                                toolbar: {
                                    downloadCsv: 'Descargar CSV',
                                    search: 'Buscar',
                                    viewColumns: 'Mostrar Columnas',
                                },
                                viewColumns: {
                                    title: 'Mostrar Columnas',
                                },
                            },
                            download: false,
                            customToolbar: () => (
                                <Tooltip title="Descargar Excel">
                                    {downloading ? (
                                        <CircularProgress size={20} style={{ verticalAlign: 'middle', marginLeft: '14px', marginRight: '14px' }} />
                                    ) : (
                                        <IconButton onClick={handleClick}>
                                            <CloudDownload />
                                        </IconButton>
                                    )}
                                </Tooltip>
                            ),

                            pagination: false,
                        }}
                    />
                </Grid>

                <ModalEditarMovimientoCartera
                    opciones={opciones}
                    open={openModalAgregar}
                    handleClose={handleCloseModalAgregar}
                    handleActualizarMovimiento={handleActualizarMovimiento}
                    formularioActualizar={formularioActualizarMovimiento}
                    dataLista={dataLista}
                />

            </CHPaper>

        </CHPrivateContainer>
    )

}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
    productState: ProductState;
    creditState: CreditState

}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    productState: state.producto,
    creditState: state.credit,


});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetProductoByIdentification: (identificationType: number, idNumber: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void; startGetAllDepartments: () => void;
    startPostCrearComprobanteCartera: (data: any) => void;
    startPostActualizarMovimientoComprobante: (data: any) => void;
    startPostObtenerComprobanteCartera: (data: any) => void;
    startPostObtenerValorCdp: (data: any) => void;
    startObtenerComprobantesCredito: (data: any) => void;
    startGetAllCities: (stateID?: string) => void;
    startGetConceptosCartera: () => void;
    startGetTipoComprobante: () => void;

}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetProductoByIdentification,
    startGetAllDepartments,
    startPostCrearComprobanteCartera,
    startPostObtenerComprobanteCartera,
    startPostObtenerValorCdp,
    startObtenerComprobantesCredito,
    startGetAllCities,
    startGetConceptosCartera,
    startGetTipoComprobante,
    startPostActualizarMovimientoComprobante,
    startGetClientByIdentificationTypeIdentificationNumber
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ConsultarComprobanteCredito);




