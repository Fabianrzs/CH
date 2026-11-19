import { Button, Checkbox, createStyles, FormControlLabel, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableFooter, TableHead, TablePagination, TableRow, Tooltip, Typography } from '@material-ui/core';
import { Alert, AlertTitle } from '@material-ui/lab';
import MUIDataTable from 'mui-datatables';
import React, { useEffect, useMemo, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { connect } from 'react-redux';
import { Cancel, CancelSharp, Delete, Edit, Search, Share, ShareOutlined, UndoTwoTone, UnfoldLess, Visibility } from '@material-ui/icons';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import { CatalogState } from '../../reducers/catalog.reducer';
import { ProductState } from '../../reducers/producto.reducer';
import {
    startGetAllIdentificationTypes,
    startGetAllCities,
    startGetAllDepartments,
    startGetBeneficiarioCartera,
    startGetConceptosCartera,
    startGetTipoComprobante
} from '../../actions/catalog.action';
import {
    startPostCrearComprobanteCartera,
    startPostObtenerComprobanteCartera,
    startPostActualizarMovimientoComprobante,
    startPostObtenerComprobantePorCodigoProceso,
    startPostGenerarComprobanteSevenPorProceso,
    resetComprobante,
    startPostObtenerMovimientosContableCartera,
    startPostAnularMovimientoContable,
} from '../../actions/producto.action';
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
import NotificationService from '../../services/notification.service';
import CarteraApi from '../../services/api-cartera-service';
import { CarteraComprobanrtesDataModel } from '../../models/cartera-comprobantes.model';

import { Auditoria } from '../../types/Auditoria.model.type';
import AuditoriaApi from '../../services/api-auditorias.servise';
import AuthService from '../../services/auth.service';
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

const ConsultarComprobanteCartera = (props: Props) => {
    const classes = useStyles();
    const formatMoney = (value: any) =>
    value != null
        ? new Intl.NumberFormat('es-CO', {
              style: 'currency',
              currency: 'COP',
          }).format(value)
        : '$0';
    const columns = [
        {
            name: 'aprobado',
            label: 'Aprobados',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any, tableMeta: any) => {
                    const rowData = dataComprobantes[tableMeta.rowIndex];

                    const rowDatas = tableMeta.tableData;

                    // const isAprobados = rowDatas[tableMeta.rowIndex].Array[8]?.estado === 'Pendiente';
                    const isAprobado = tableMeta.rowData[9] === 'Pendiente';
                    const id = tableMeta.rowData[6];


                    // const isAprobado = productState?.comprobanteCartera[tableMeta.rowIndex]?.estado === 'Pendiente';
                    // const id = productState?.comprobanteCartera[tableMeta.rowIndex]?.comprobante_Id;

                    return (
                        <Checkbox
                            checked={selectedItems.includes(id)} // Verifica si está seleccionado
                            onChange={() => handleCheckboxChange(id)}
                            disabled={!isAprobado} // Asegúrate de que sea booleano
                        />
                    );
                },
            },
        },

        {
            name: 'tipo_Operacion',
            label: 'Tipo Operacion',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },

        {
            name: 'identificacion',
            label: 'Numero Identificación',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'fecha_Comprobante',
            label: 'Fecha Creación',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => {
                    if (!value) return '';
        
                    const date = new Date(value);
        
                    const day = String(date.getDate()).padStart(2, '0');
                    const month = String(date.getMonth() + 1).padStart(2, '0');
                    const year = date.getFullYear();
        
                    return `${day}/${month}/${year}`;
                },
            },
        },
        {
            name: 'fecha_Transaccion',
            label: 'Fecha Transacción',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) =>
                    value
                        ? new Date(value).toLocaleDateString('es-CO')
                        : '',
            },
        },
        {
            name: 'tipo_Comprobante',
            label: 'Tipo Comprobante',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'comprobante_Id',
            label: 'Comprobante Interno',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'comprobante_Seven_Id',
            label: 'Comprobante Seven',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) =>  value ?? 'N/A'
            },
        },
        {
            name: 'total_Comprobante_Credito',
            label: 'Total Comprobante',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas',
                }),
                customBodyRender: formatMoney
            },
        },
        {
            name: 'estado',
            label: 'Estado',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'acciones',
            label: 'Acciones',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any, tableMeta: any) => (
                    <>
                        <Tooltip title="Abrir">
                            <IconButton color="secondary" onClick={() => handleEdit(tableMeta.rowData)}>
                                {/* <IconButton color="secondary" onClick={() => handleEdit(productState.comprobanteCartera[tableMeta.rowIndex])}> */}
                                <Visibility />
                            </IconButton>
                        </Tooltip>


                        {(
                            tableMeta.rowData[8] === 'Reversado') && (
                                <Tooltip title="Anular">
                                    <IconButton color="secondary" onClick={() => handleCancel(tableMeta.rowData)}>
                                        <CancelSharp />
                                    </IconButton>
                                </Tooltip>
                            )}

                    </>
                ),
            },
        }

    ];



    const {
        catalogState,

        loginState,
        productState,
        startGetAllIdentificationTypes,
        startGetAllCities,
        startGetAllDepartments,
        startPostCrearComprobanteCartera,
        startPostActualizarMovimientoComprobante,
        startPostObtenerComprobanteCartera,
        resetComprobante,
        startGetBeneficiarioCartera,
        startGetConceptosCartera,
        startGetTipoComprobante,
        startGetClientByIdentificationTypeIdentificationNumber,
        startPostObtenerComprobantePorCodigoProceso,
        startPostGenerarComprobanteSevenPorProceso,
        startPostObtenerMovimientosContableCartera,
        startPostAnularMovimientoContable
    } = props;

    const apiCartera = CarteraApi();
    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [opciones, setOpciones] = useState(0);



    const { control, clearErrors, register, getValues, setValue, handleSubmit, reset, ...rest } = useForm({
        defaultValues: {
            numeroComprobante: 0,
            fechaComprobante: new Date(),
            tipoComprobante: null,
            departamento: '',
            ciudad: '',
            concepto: '',
            beneficiario: '',
            tipoIdentificacion: '',
            identificacion: 0,
            NombreBeneficiario: '',
            fechaInicio: null,
            fechaFinal: null,
            comprobanteId: null,
            comprobanteSvnId: null,
            tipoOperacion: null,
            estado: false

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
    const [dataComprobantes, setDataComprobantes] = useState<any[]>([]);
    const [dataLista, setDataLista] = useState<any[]>([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);
    const [selectedItems, setSelectedItems] = useState<any[]>([]);
    const [comprobantesSvn, setComprobantesSvn] = useState<number[]>([]);
    const [datosConsulta, setDatosConsulta] = useState<any>(null);
    const [loading, setLoading] = useState<any>(false);
    


    // const paginatedData = productState.comprobanteCartera.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);
    // const tipoComprobanteCartera = catalogState.tipoComprobante.filter((mov: any) => mov.tipo === 'D')

    useEffect(() => {
        startGetTipoComprobante()
        resetComprobante()
    }, []);

    useEffect(() => {
        const dataComprobante = Array.isArray(productState?.comprobanteCartera)
            ? productState.comprobanteCartera.map((item) => ({
                ...item,
                aprobado: item.aprobado ?? false,
                tipo_Operacion: item.tipo_Operacion ?? 'N/A',
                fecha_Comprobante: item.fecha_Comprobante ?? null,
                fecha_Transaccion: item.fecha_Transaccion ?? null,
                tipo_Comprobante: item.tipo_Comprobante ?? 'N/A',
                comprobante_Id: item.comprobante_Id ?? 'N/A',
                comprobante_Seven_Id: item.comprobante_Seven_Id,
                total_Comprobante_Credito: item.total_Comprobante_Credito ?? 0,
                estado: item.estado ?? 'N/A',
            }))
            : [];

        setDataComprobantes(dataComprobante)
    }, [productState?.comprobanteCartera]);

    const tipoComprobanteCartera = catalogState.tipoComprobante.filter(item => item.tipoContabilidad === 1)


    const handleChangePage = (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
        setPage(newPage);
    };

    const handleCheckboxChange = (id: any) => {
        setSelectedItems((prevSelectedItems) => {
            if (prevSelectedItems.includes(id)) {
                return prevSelectedItems.filter(item => item !== id);
            }
            return [...prevSelectedItems, id];
        });
    };



    const [selectedRows, setSelectedRows] = useState([]);
    const options = {
        selectableRows: "multiple", // "single" si solo quieres seleccionar una fila a la vez
        onRowSelectionChange: (rowsSelected: any, allRows: any) => {
            const selectedIds = allRows.map((row: any) => data[row.dataIndex]?.id);
            setSelectedRows(selectedIds);
        },
        selectToolbarPlacement: "none", // Para ocultar la barra de selección si no se necesita
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

    const handleInsertarSeven = async () => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro?',
            'Los comprobantes seleccionados serán aprobados y enviados al módulo de contabilidad.'
        );

        if (confirmAlert) {
            if (!Array.isArray(productState.comprobanteCartera) || productState.comprobanteCartera.length === 0) {
                toast.error('Error: No hay comprobantes en la cartera para la búsqueda.');
                return;
            }

            const comprobantesFiltrados = productState.comprobanteCartera.filter(carteraItem =>
                selectedItems.includes(carteraItem.comprobante_Id)
            );

            if (comprobantesFiltrados.length === 0) {
                toast.error('Error: No se encontraron comprobantes filtrados.');
                return;
            }

            const tipoComprobanteReferencia = comprobantesFiltrados[0].tipo_Comprobante;

            if (tipoComprobanteReferencia == null) {
                toast.error('Error: El tipo de comprobante es inválido.');
                return;
            }

            const todosTienenMismoTipo = comprobantesFiltrados.every(item =>
                item.tipo_Comprobante === tipoComprobanteReferencia
            );

            if (!todosTienenMismoTipo) {
                toast.error('Error: Los comprobantes seleccionados no pertenecen al mismo tipo.');
                return;
            }

            if (comprobantesFiltrados.length !== selectedItems.length) {
                toast.error('Error: No todos los comprobantes seleccionados se encuentran en la cartera.');
                return;
            }

            if (selectedItems.length === 0) {
                toast.error('Error: No se han seleccionado comprobantes.');
                return;
            }

            const CodigoMovimiento = tipoComprobanteCartera.find(item => item.tipoComprobante === tipoComprobanteReferencia)

            if (!CodigoMovimiento) {
                toast.error('Error: No se encontró un código de movimiento para el tipo de comprobante especificado.');
                return;
            }
            const request = {
                codigoProceso: CodigoMovimiento.codigo,
                comprobantes: selectedItems.map((id: number) => ({
                    comprobante_Id: id
                }))
            };

            startPostGenerarComprobanteSevenPorProceso(request, datosConsulta);
            setSelectedItems([]);

            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = (window.navigator as any);
            const usrAg = nav.userAgentData.brands.find(
                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand || 'Desconocido';

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Inserción de Comprobantes en Seven',
                hostname: document.location.host,
                tipoAccion: 'ENVIO DE DATOS',
                idTipoAccion: 4,
                descripcion: `Se han enviado comprobantes los comprobantes a contabilidad:
            \n Tipo de Comprobante: ${tipoComprobanteReferencia}
            \n Código de Movimiento: ${CodigoMovimiento.codigo}
            \n Comprobantes Aprobados: ${selectedItems.length}`
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        }

    };


    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const handleDelete = (id: number) => {
        const newData = data.filter((item: any) => item.id !== id);
        setData(newData);
    };


    const handleCancel = async (data: any) => {
        const comprobanteFiltrado: any = productState.comprobanteCartera?.filter(
            (comprobante: any) => comprobante.comprobante_Id === data[6]
        );
        const comprobanteSeleccionado = comprobanteFiltrado?.length > 0 ? comprobanteFiltrado[0] : null;

        const request = comprobanteSeleccionado ? {
            Cc_Id: comprobanteSeleccionado.comprobante_Id,
            Comprobante_Seven_Id: comprobanteSeleccionado.comprobante_Seven_Id,
            Consecutivo_Seven_Id: comprobanteSeleccionado.consecutivo_Seven_Id,
            Top_codi: comprobanteSeleccionado.tipo_Operacion
        } : null;

        if (request) {
            // Aquí ponemos el swal como tú quieres
            const confirmacion = await swal({
                title: 'Anular Comprobante',
                text: '¿Está seguro (S/N)?',
                icon: 'warning',
                buttons: ['No', 'Sí'],
            });

            if (confirmacion) {
                await startPostAnularMovimientoContable(request);
                swal('¡Anulado!', 'El comprobante ha sido anulado con éxito.', 'success');
            } else {
                swal('Cancelado', 'La anulación ha sido cancelada.', 'info');
            }
        }
    };

    const handleEdit = async (rowData: any) => {

        const comprobanteFiltrado: any = productState.comprobanteCartera?.filter((comprobante: any) => comprobante.comprobante_Id === rowData[6]);
        const comprobanteSeleccionado = comprobanteFiltrado?.length > 0 ? comprobanteFiltrado[0] : null;

        setLoading(true);

        try {
            // await startPostObtenerMovimientosContableCartera(rowData.detalle_Movimiento_Id)
            const response = await apiCartera.postObtenerMovimientosContableCartera({
                idComprobante: comprobanteSeleccionado.detalle_Movimiento_Id,
                registrosPorPagina: 500,
                numeroPagina: 1
            })
            if (response?.isSuccess && response.data?.length > 0) {
                const updatedData = {
                    ...comprobanteSeleccionado,
                    lstMovimientoComprobante: response.data
                };
                setOpciones(1)
                setDataLista(updatedData)
                setOpenModalAgregar(true);
            } else {
                toast('No se encontraron resultados para el comprobante.', { type: toast.TYPE.WARNING, autoClose: 5000 });
            }

        } catch (error) {
            toast('Error del sistema. Por favor, intenta más tarde.', { type: toast.TYPE.ERROR });
        }
        finally {
            setLoading(false);
        }

    };



    const buscarComprobante = (data: any) => {
        setSelectedItems([]);

        const request = {
            fechaFinal: data.fechaFinal ? moment(data.fechaFinal).format("YYYY-MM-DD") : null,
            fechaInicio: data.fechaInicio ? moment(data.fechaInicio).format('YYYY-MM-DD') : null,
            codigoProceso: data.tipoComprobante,
            comprobanteId: data.comprobanteId,
            comprobanteSevenId: data.comprobanteSvnId,
            tipoOperacion: data.tipoOperacion,
            estado: data.estado ? 1 : null
        };

        setDatosConsulta(request);
        startPostObtenerComprobantePorCodigoProceso(request);


        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        let estadoDescripcion = 'No especificado';

        if (request.estado === 1) {
            estadoDescripcion = 'Activo';
        } else if (request.estado === 0) {
            estadoDescripcion = 'Inactivo';
        }

        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de Comprobantes Cartera',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de comprobantes con los siguientes filtros:
                - Tipo de Comprobante: ${request.codigoProceso || 'No especificado'}
                - Fecha Inicio: ${request.fechaInicio || 'No especificada'}
                - Fecha Final: ${request.fechaFinal || 'No especificada'}
                - Número Comprobante Interno: ${request.comprobanteId || 'No especificado'}
                - Número Comprobante Seven: ${request.comprobanteSevenId || 'No especificado'}
                - Tipo de Operación: ${request.tipoOperacion || 'No especificado'}
                - Estado: ${estadoDescripcion}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error al consultar Comprobantes Cartera.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de Comprobantes Cartera',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al consultar Comprobantes Cartera', { type: toast.TYPE.ERROR });
        }


    };


    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    
        if (datosConsulta) {
            setSelectedItems([]);
            startPostObtenerComprobantePorCodigoProceso(datosConsulta);
        }
    };

    const handleActualizarMovimiento = async (data: any) => {
        startPostActualizarMovimientoComprobante(data);
        handleCloseModalAgregar();
    };

    return (
        <CHPrivateContainer>
            <PageIndicators
                topTextNivel1='Comprobantes'
                topTextNivel2='Contabilidad Cartera'
            />
            <LoadingComponent key="loader" loading={productState.loading || loading} />

            <IconTitle
                title='Consulta'
                subTitle='Comprobantes Cartera'
                icon='fch-Aprobar-Cargue-Nomina-0-1'
            />

            <CHPaper title="Datos de Comprobante">

                <ContainerTitle title="CONSULTAR COMPROBANTE" />

                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="tipoComprobante"
                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHSelect
                                    label="Tipo de comprobante*"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={(e) => {
                                        const selectedValue = e.target.value === '' ? null : e.target.value; // Asignar null si el valor está vacío
                                        onChange(selectedValue);
                                    }}
                                >
                                    <MenuItem value="">
                                        <em>Ninguna...</em>
                                    </MenuItem>
                                    {tipoComprobanteCartera?.map((option: any) => (
                                        <MenuItem key={option.tipoComprobanteId} value={option.codigo}>
                                            {option.tipoComprobante}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="fechaInicio"
                            control={control}
                            defaultValue={null}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Inicial"
                                    onChange={(date) => onChange(date || null)}
                                    value={value || null}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="fechaFinal"
                            control={control}
                            defaultValue={null}
                            render={({ field: { onChange, value } }) => (
                                <CHDatePicker
                                    label="Fecha Final"
                                    onChange={(date) => onChange(date || null)}
                                    value={value || null}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="comprobanteId"
                            control={control}
                            defaultValue={null}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Numero Comprobante Interno"
                                    value={value}
                                    onChange={(e) => {
                                        let newValue = e.target.value.replace(/\D/g, '');
                                        if (newValue.length > 10) {
                                            newValue = newValue.slice(0, 10);
                                        }
                                        onChange(newValue.length > 0 ? newValue : null);
                                    }}
                                    InputLabelProps={{ shrink: true, }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="comprobanteSvnId"
                            control={control}
                            defaultValue={null}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Numero Comprobante Seven"
                                    value={value}
                                    onChange={(e) => {
                                        let newValue = e.target.value.replace(/\D/g, '');
                                        if (newValue.length > 10) {
                                            newValue = newValue.slice(0, 10);
                                        }
                                        onChange(newValue.length > 0 ? newValue : null);
                                    }}
                                    InputLabelProps={{ shrink: true, }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="tipoOperacion"
                            control={control}
                            defaultValue={null}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    label="Tipo de Operación"
                                    value={value}
                                    onChange={(e) => {
                                        let newValue = e.target.value.replace(/\D/g, '');
                                        if (newValue.length > 10) {
                                            newValue = newValue.slice(0, 10);
                                        }
                                        onChange(newValue.length > 0 ? newValue : null);
                                    }}
                                    InputLabelProps={{ shrink: true, }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="estado"
                            control={control}
                            defaultValue={false}
                            render={({ field: { onChange, value } }) => (
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            checked={value}
                                            onChange={(e) => onChange(e.target.checked)}
                                        />
                                    }
                                    label="Pendientes"
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

                <ContainerTitle title="COMPROBANTES" />

                <Grid container>
                    <MUIDataTable
                        columns={columns}
                        data={dataComprobantes}
                        title=""
                        options={{
                            selectableRows: 'none',
                            responsive: 'vertical',
                            filter: false,
                            search: true,
                            download: false,
                            viewColumns: false,
                            print: false,
                            elevation: 0,
                            tableBodyHeight: '600px',
                            // tableBodyMaxHeight: '600px',
                            selectableRowsHeader: false,
                            textLabels: {
                                body: {
                                    noMatch: 'Ningun Resultado',
                                },
                                pagination: {
                                    displayRows: 'de',
                                    next: 'Siguiente',
                                    previous: 'Anterior',
                                    rowsPerPage: 'Filas por página',
                                    jumpToPage: '',
                                },
                            },
                            setRowProps: (row, dataIndex, rowIndex) => {
                                const rowData = dataComprobantes[rowIndex]; // Accede al objeto original de la data
                                if (rowData.total_Comprobante_Credito !== rowData.total_Comprobante_Debito) { // Validación usando la data original
                                    return {
                                        style: { backgroundColor: 'rgba(255, 0, 0, 0.1)' },
                                    };
                                }
                                return {};
                            },

                            onColumnSortChange: (columnName, direction) => {
                                // Lógica para ordenar los datos según columnName y direction
                                const sortedData = [...dataComprobantes].sort((a, b) =>
                                    (a[columnName] > b[columnName] ? 1 : -1) * (direction === 'asc' ? 1 : -1)
                                );
                                setDataComprobantes(sortedData);
                            }

                        }}
                    />
                </Grid>

                <Grid container>
                    <Grid container className={classes.buttonContainer}>
                        <Button
                            className={classes.buttonAdd}
                            type="submit"
                            variant="outlined"
                            onClick={handleInsertarSeven}
                        >
                            ENVIAR COMPROBANTES A CONTABILIDAD
                        </Button>
                    </Grid>
                </Grid>

            </CHPaper>
            <ModalEditarMovimientoCartera
                opciones={opciones}
                open={openModalAgregar}
                handleClose={handleCloseModalAgregar}
                handleActualizarMovimiento={handleActualizarMovimiento}
                formularioActualizar={formularioActualizarMovimiento}
                dataLista={dataLista}
                setDataLista={setDataLista}
                tipoComprobanteCartera={tipoComprobanteCartera}
            />

        </CHPrivateContainer>
    )

}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
    productState: ProductState;

}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    productState: state.producto,

});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void; startGetAllDepartments: () => void;
    startPostCrearComprobanteCartera: (data: any) => void;
    startPostActualizarMovimientoComprobante: (data: any) => void;
    startPostObtenerComprobanteCartera: (data: any) => void;
    resetComprobante: () => void;
    startPostObtenerComprobantePorCodigoProceso: (data: any) => void;
    startPostGenerarComprobanteSevenPorProceso: (dataG: any, dataC: any) => void;
    startPostObtenerMovimientosContableCartera: (dataG: any) => void;
    startPostAnularMovimientoContable: (dataG: any) => void;
    startGetAllCities: (stateID?: string) => void;
    startGetBeneficiarioCartera: () => void;
    startGetConceptosCartera: () => void;
    startGetTipoComprobante: () => void;

}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetAllDepartments,
    startPostCrearComprobanteCartera,
    resetComprobante,
    startPostObtenerComprobanteCartera,
    startPostObtenerComprobantePorCodigoProceso,
    startPostGenerarComprobanteSevenPorProceso,
    startPostAnularMovimientoContable,
    startGetAllCities,
    startGetBeneficiarioCartera,
    startGetConceptosCartera,
    startGetTipoComprobante,
    startPostActualizarMovimientoComprobante,
    startGetClientByIdentificationTypeIdentificationNumber,
    startPostObtenerMovimientosContableCartera
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ConsultarComprobanteCartera);




