import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import {
    Button,
    Card,
    CardContent,
    FormControlLabel,
    Grid,
    Radio,
    RadioGroup,
    makeStyles,
    createStyles,
    Tooltip,
    IconButton,
} from '@material-ui/core';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable, { MUIDataTableColumn } from 'mui-datatables';
import { toast } from 'react-toastify';
import { Delete } from '@material-ui/icons';
import swal from 'sweetalert';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CHContainerSubtitle from '../../components/container-subtittle/container-subtittle';
import { startPostCierreDiarioCartera } from '../../actions/producto.action';
import UtilService from '../../utils/util.service';
import Header from '../../components/header/Header';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import './TablasCierres.scss';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CreditApi from '../../services/api-credit.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import { ProductState } from '../../reducers/producto.reducer';
import { AppState } from '../../store/rootReducer';
import CarteraApi from '../../services/api-cartera-service';
import NotificationService from '../../services/notification.service';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import ApiproductoClients from '../../services/api-producto.service';
import AuthService from '../../services/auth.service';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import { Auditoria } from '../../types/Auditoria.model.type';
import DownloadToolbar from '../productClient/components/reportes/componentes/DownloadToolbar';
import handleExportToExcel from '../Reports/helpers/handleExportToExcel';
import CatalogApi from '../../services/api-catalog.service';
import { useIp } from '../../shared/context/IpContext';
import { ColumnFormatType } from '../../enums/ColumnFormatType';


const useStyles = makeStyles(() =>
    createStyles({
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        columnDate: {
            minWidth: '110px',

        },

        tablaCierre: {
            overflow: 'auto !important'
     
    },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#43b5f1',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
        },
    }),
);


function CierreTable(
    { 
        data, 
        mostrarTabla, 
        fechaCierre,
        esDefinitivo
    }:{ 
        data: any[], 
        mostrarTabla: any,
        fechaCierre: any,
        esDefinitivo: boolean
    }) {

   
            const carteraApi = CarteraApi();
            const user = AuthService.getUserInfo();
    const classes = useStyles();
    const [rowsSelected, setRowsSelected] = useState<any[]>([]);
    const [tableData, setTableData] = useState(data || []);

    useEffect(() => {
        setTableData(data || []);
    }, [data]);

    const obtenerComprobanteInterno = (row: any) => {
        const hipotecario = Number(row.comprobanteInternoHipotecario || 0);
        const leasing = Number(row.comprobanteInternoLeasing || 0);
    
        if (hipotecario > 0) return hipotecario;
        if (leasing > 0) return leasing;
    
        return 0;
    };
    
    const validarMismoComprobante = (registros: any[]) => {
        const comprobantes: number[] = registros.map((row: any) =>
            obtenerComprobanteInterno(row)
        );
    
        const tieneSinComprobante = comprobantes.some((cc_ID: number) => cc_ID <= 0);
    
        if (tieneSinComprobante) {
            return {
                valido: false,
                mensaje: 'Hay registros seleccionados sin comprobante interno válido.',
                cc_ID: 0,
            };
        }
    
        const comprobantesUnicos = Array.from(new Set<number>(comprobantes));
    
        if (comprobantesUnicos.length > 1) {
            return {
                valido: false,
                mensaje:
                    'No se pueden eliminar registros de comprobantes internos diferentes. Seleccione registros del mismo comprobante.',
                cc_ID: 0,
            };
        }
    
        return {
            valido: true,
            mensaje: '',
            cc_ID: comprobantesUnicos[0],
        };
    };

    const handleEliminarFila = async (rowEliminar: any, dataIndex: number) => {
        const respuesta = await swal({
            title: '¿Está seguro?',
            text: 'El registro será eliminado.',
            icon: 'warning',
            buttons: {
                cancel: {
                    text: 'Cancelar',
                    visible: true,
                    value: false,
                },
                confirm: {
                    text: 'Eliminar',
                    visible: true,
                    value: true,
                },
            },
        });
    
        if (!respuesta) return;
    
        try {
            const cc_ID = obtenerComprobanteInterno(rowEliminar);
    
            if (cc_ID <= 0) {
                swal({
                    title: 'Validación',
                    text: 'El registro no tiene comprobante interno válido.',
                    icon: 'warning',
                });
                return;
            }
    
            const request = {
                cc_ID,
                dmc_refe: [rowEliminar.identificacion],
                modificadoPor: user.USR_ID,
            };
    
    
            const response = await carteraApi.postEliminarAfiliadoDelComprobante(request);
    
            if (response) {
                const nuevaData = [...tableData];
                nuevaData.splice(dataIndex, 1);
    
                setTableData(nuevaData);
                setRowsSelected([]);
    
                swal({
                    title: 'Eliminado',
                    text: 'El registro fue eliminado correctamente.',
                    icon: 'success',
                });
            }
        } catch (error) {
            console.log(error);
    
            swal({
                title: 'Error',
                text: 'No fue posible eliminar el registro.',
                icon: 'error',
            });
        }
    };
    
    const handleEliminarFilasSeleccionadas = async (selectedRows: any) => {
       
    
        const indicesEliminar = selectedRows.data.map(
            (row: any) => row.dataIndex
        );
    
    
        const registrosEliminar = indicesEliminar.map(
            (dataIndex: number) => tableData[dataIndex]
        );
    
    
        const validacion = validarMismoComprobante(registrosEliminar);
    
        if (!validacion.valido) {
            swal({
                title: 'Validación',
                text: validacion.mensaje,
                icon: 'warning',
            });
    
            return;
        }
    
        const respuesta = await swal({
            title: '¿Está seguro?',
            text: 'Los registros seleccionados serán eliminados.',
            icon: 'warning',
            buttons: {
                cancel: {
                    text: 'Cancelar',
                    visible: true,
                    value: false,
                },
                confirm: {
                    text: 'Eliminar',
                    visible: true,
                    value: true,
                },
            },
        });
    
        if (!respuesta) return;
    
        try {
            const request = {
                cc_ID: validacion.cc_ID,
                dmc_refe: registrosEliminar.map((row: any) => row.identificacion),
                modificadoPor: user.USR_ID,
            };
    
    
            const response = await carteraApi.postEliminarAfiliadoDelComprobante(request);
    
            if (response) {
                const nuevaData = tableData.filter(
                    (_: any, index: number) => !indicesEliminar.includes(index)
                );
    
                setTableData(nuevaData);
                setRowsSelected([]);
    
                swal({
                    title: 'Eliminado',
                    text: 'Los registros fueron eliminados correctamente.',
                    icon: 'success',
                });
            }
        } catch (error) {
            console.log(error);
    
            swal({
                title: 'Error',
                text: 'No fue posible eliminar los registros seleccionados.',
                icon: 'error',
            });
        }
    };
    
   
    const columns = [
        {
            name: 'numeroCredito',
            label: 'Número de Crédito',
            options: {

                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },
                   
                }),
                setCellHeaderProps: () => ({
         
                   className: 'elemento'
                }),
            
            },
        },
        {
            name: 'tipoIdentificacion',
            label: 'Tipo de Identificación',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'identificacion',
            label: 'Número de Identificación',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'nombres',
            label: 'Apellidos y nombres',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'tipoCredito',
            label: 'Tipo Crédito',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'estadoCredito',
            label: 'Estado Crédito',
            options: {
              customBodyRender: (value: string) => 
                value === null ? 'EN MORA' : value,
              setCellHeaderProps: () => ({
                className: 'filas',
              }),
            },
            formatType: ColumnFormatType.Estado,
        },
        {
            name: 'plazoTotal',
            label: 'Plazo Total',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },       
        {
            name: 'plazoActual',
            label: 'Plazo Actual',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'cuotasCanceladas',
            label: 'Cuotas pagadas',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        // {
        //     name: 'tasaSeguroVida',
        //     label: 'Tasa Seguro Vida',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.roundValue1(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },

        // {
        //     name: 'tasaSeguroHogar',
        //     label: 'Tasa Seguro Hogar',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.roundValue1(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        // },
        {
            name: 'tasaCreditoEA',
            label: 'Tasa Crédito EA',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Porcentaje,
        },
        {
            name: 'tasaPeriodica',
            label: 'Tasa Crédito NMV %',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'tasaCreditoEAMora',
            label: 'Tasa Crédito EA Mora',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Porcentaje,
        },
        {
            name: 'tasaPeriodicaMora',
            label: 'Tasa Periódica Diaria de Mora',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Porcentaje,
        },
        {
            name: 'valorCuota',
            label: 'Valor Cuota',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },
        // {
        //     name: 'saldoCapital',
        //     label: 'Saldo Capital',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //             UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        //     formatType: ColumnFormatType.Decimal,
        // },
        {
            name: 'saldoMora',
            label: 'Saldo Capital Mora',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },
        {
            name: 'diasMora',
            label: 'Días Mora',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },

        // {
        //     name: 'valorDeudaRemanente',
        //     label: 'Valor Deuda Remanente',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        //     formatType: ColumnFormatType.Decimal,
        // },

        {
            name: 'fechaUltimoPago',
            label: 'Fecha Último Pago',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },
                    
                }),
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Fecha,
        },
        {
            name: 'fechaProximoPago',
            label: 'Fecha Próximo Pago',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },
                    
                }),
                customBodyRender: (value: Date) => UtilService.getDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Fecha,
        },

        {
            name: 'diasAcumuladosMora',
            label: 'Días Acumulados Mora',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Numerico,
        },

        {
            name: 'intMoraAnterior',
            label: 'Int. Mora Anterior',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },

        // {
        //     name: 'acumuladoSeguroHogar',
        //     label: 'Acumulado Seguro Hogar',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        //     formatType: ColumnFormatType.Decimal,
        // },


        // {
        //     name: 'acumuladoSeguroVida',
        //     label: 'Acumulado Seguro Vida',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //         UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        //     formatType: ColumnFormatType.Decimal,
        // },

        {
            name: 'totalMora',
            label: 'Total Mora',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },

        {
            name: 'causacionInteresesMora',
            label: 'Causación int. mora',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
            formatType: ColumnFormatType.Decimal,
        },

        // {
        //     name: 'fechaUltimoPagoInteresesMora',
        //     label: 'Fecha Último Pago Interés Mora',
        //     options: {
        //         setCellProps: () => ({
        //             className: classes.columnDate,
        //             style: {
        //                 textAlign: 'left',
        //             },
                    
        //         }),
        //         customBodyRender: (value: Date) => UtilService.getDate(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        //     formatType: ColumnFormatType.Fecha,
        // },
            
        // {
        //     name: 'acumuladoInteresesCorrientes',
        //     label: 'Acumulado Interés Corriente',
        //     options: {
        //         customBodyRender: (value: number | bigint) =>
        //             UtilService.formatCurrency(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        //     formatType: ColumnFormatType.Decimal,
        // },
    
        // {
        //     name: 'fechaUltimoPagoInteresesCorrientes',
        //     label: 'Fecha Último Pago Interés Corriente',
        //     options: {
        //         setCellProps: () => ({
        //             className: classes.columnDate,
        //             style: {
        //                 textAlign: 'left',
        //             },
                    
        //         }),
        //         customBodyRender: (value: Date) => UtilService.getDate(value),
        //         setCellHeaderProps: () => ({
        //             className: 'filas'
        //         }),
        //     },
        //     formatType: ColumnFormatType.Fecha,
        // },
   
        {
            name: 'calificacion',
            label: 'Calificación de Cartera',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'areaNegocio',
            label: 'Unidad Ejecutora',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'provision',
            label: 'Provisión de Cartera',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                    setCellHeaderProps: () => ({
                        className: 'filas'
                    }),
            },
            formatType: ColumnFormatType.Decimal,
        },

        {
            name: 'provisionPorcentaje',
            label: 'Provisión %',
            options: {
              
                setCellHeaderProps: () => ({
                 className:'filas'
                }),
            },
            formatType: ColumnFormatType.PorcentajeDecimal,
        },
     
    ];

    const columnaAcciones = {
        name: 'acciones',
        label: 'Acciones',
        options: {
            filter: false,
            sort: false,
            download: false,
            print: false,
    
            customBodyRenderLite: (dataIndex: number) => {
                const rowData = tableData[dataIndex];
    
                return (
                    <Tooltip title="Eliminar">
                        <IconButton
                            onClick={() => handleEliminarFila(rowData, dataIndex)}
                        >
                            <Delete />
                        </IconButton>
                    </Tooltip>
                );
            },
        },
    };
    
    const columnsFinal = esDefinitivo
        ? [...columns, columnaAcciones]
        : columns;
    

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi  = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };

    const enviarIp = async (ip:any) => {
        const  convertIp = ip.split('.')[0]
       const  hexadecimal = convertIp
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



    const handleDownload = async () => {
        const date = new Date(fechaCierre);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
    
        const formattedDate = `${day}-${month}-${year}`;
        const fileName = `CIERRE MORATORIO ${formattedDate}.xlsx`;
    
        await handleExportToExcel(tableData, columns, fileName, publicIpAddress);
    };

    return mostrarTabla && (
        <MUIDataTable
        title=""
        data={tableData}
        columns={columnsFinal}
        options={{
            selectableRows: esDefinitivo ? 'multiple' : 'none',
            selectableRowsHeader: esDefinitivo,
            rowsSelected: esDefinitivo ? rowsSelected : [],
    
            onRowSelectionChange: esDefinitivo
                ? (
                    currentRowsSelected: any,
                    allRowsSelected: any,
                    rowsSelectedData: number[]
                ) => {
                    setRowsSelected(rowsSelectedData);
                }
                : undefined,
    
            customToolbarSelect: esDefinitivo
                ? (selectedRows: any) => (
                    <Tooltip title="Eliminar seleccionados">
                        <IconButton
                            onClick={() => handleEliminarFilasSeleccionadas(selectedRows)}
                        >
                            <Delete />
                        </IconButton>
                    </Tooltip>
                )
                : undefined,
    
            onRowsDelete: esDefinitivo ? () => false : undefined,
    
            viewColumns: esDefinitivo,
            fixedSelectColumn: esDefinitivo,
    
            responsive: 'vertical',
            filter: false,
            print: false,
            elevation: 0,
            fixedHeader: true,
            tableBodyHeight: '420px',
            download: false,
            rowsPerPageOptions: [5, 15, 20, 100, 200, 500],
    
            customToolbar: () => (
                <DownloadToolbar downloadAction={handleDownload} />
            ),
        } as any}
    />
    );
}

const CierreDiarioCartera = (props: props) => {

    const creditApi = CreditApi();
    const carteraApi = CarteraApi();
    const ApiClients = ApiproductoClients();
    const auditoriaApi = AuditoriaApi();

    let fechaUltimoCierreMensual: any = null;
    let fechaUltimoCierreDiario: any = null;
    let ejecutarProceso = true;

    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [dataCierreDiario, setDataCierreDiario] = useState<any[]>([]);
    const [mostrarTabla, setMostrarTabla] = useState(false);


  
    
    const formularioCierreDiarioCartera = useForm({
        'mode':'onChange',
        defaultValues: {
            fechaCierre: new Date(),
            actualizacion: '0'
        },

    });

    const classes = useStyles();
    const tipoActualizacion = formularioCierreDiarioCartera.watch('actualizacion');
    const esDefinitivo = tipoActualizacion === '1';
    const { productoState, startPostCierreDiarioCartera } = props;

    const { control, handleSubmit, getValues } = useForm({
        defaultValues: {
            actualizacion: '0',
        },
    });

    carteraApi
        .getUltimosCierres()
        .then(async (response: any) => {
            if (response) {
                if (response.data != null && response.isSuccess) {
                    fechaUltimoCierreMensual = response.data.filter((value:any) => value.idTipoCierre === 2)
                    fechaUltimoCierreDiario = response.data.filter((value:any) => value.idTipoCierre === 1)
                } else {
                    toast(`No se encontraron cierres de cartera`, { type: toast.TYPE.WARNING });
                }
            }
        })

        const insertarCierreDiarioMoratorio = (data: any) => {
            setIsLocalLoading(true)
    
            ApiClients
            .postCierreDiarioSevenIM(data)
            .then(response => {
                if(response.isSuccess) {
                    const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
                    const errorTexto = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);

                    if(retornoExitoso[1] === "0"){
                        toast('Insercion de movimiento EXITOSA', { type: toast.TYPE.SUCCESS });
                    }else{
                        toast(`Insercion de movimiento FALLO: ${errorTexto[1]}`, { type: toast.TYPE.ERROR, autoClose: 10000 });
                    }
                }
            })
            .catch((_) => {
                toast('Error del sistema', { type: toast.TYPE.ERROR, autoClose: 10000 });
            }).finally(() => setIsLocalLoading(false));
        }

    const { publicIp } = useIp();

    const onSubmit = async (data: any) => {
        const user = AuthService.getUserInfo();
        const nav =(window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; })=>x.brand==='Google Chrome'||x.brand==='Microsoft Edge')[0].brand;    
       
        const requestAuditoria={
            navegador:usrAg,
            sistemaOperativo:nav.userAgentData.platform,
            nombreUsuario:user.USR_USERNAME, 
            usurioId:user.USR_ID,
            ip: publicIp,
            modulo:'Cierre Diario',
            hostname:document.location.host
        } as Auditoria

        const fechaCierre = moment(data.fechaCierre);
        if(!fechaCierre.isValid()) {
            return;
        }

        ejecutarProceso = true;
        
        const request = {
            fechaCierre: fechaCierre.startOf('day').format(),
            actualiza: data.actualizacion === "1",
        }

        if(request.actualiza) {
            if (fechaUltimoCierreDiario !== null && fechaUltimoCierreDiario.length > 0 && fechaUltimoCierreDiario[0].fechaCierre !== null) {
                const fechaUltimoCierre = new Date(fechaUltimoCierreDiario[0]?.fechaCierre);
                const esfechaIgualAlUltimoCierre = moment(request.fechaCierre).isSame(fechaUltimoCierre, 'day');
                const esFechaAnteriorAlUltimoCierre = moment(request.fechaCierre).isBefore(fechaUltimoCierre,'day');
                const ultimoDiaMes = moment(fechaUltimoCierre).endOf('month').startOf('day');
                const ultimoCierreEsFinMes = moment(fechaUltimoCierre).isSame(ultimoDiaMes.toDate(), 'day');
                const esSuperiorAlMesCierre = (!(ultimoCierreEsFinMes) && moment(request.fechaCierre).isAfter(ultimoDiaMes.toDate(),'day'));
                const debeCerrarMesAnterior = ultimoCierreEsFinMes && moment(request.fechaCierre).isAfter(moment(ultimoDiaMes).add(1, 'months'), 'month');

                if(esfechaIgualAlUltimoCierre){        
                    ejecutarProceso = false;
                    const msg='Ya se ejecutó el cierre para esta fecha'
                    requestAuditoria.tipoAccion='VALIDACION';
                    requestAuditoria.idTipoAccion=9;
                    requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                         \n fecha:  ${moment(request.fechaCierre).format("DD/MM/YYYY HH:mm")}
                         \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                         \n Validación: ${msg}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                    NotificationService.genericAlertError(
                        'Error',               
                        msg,
                        'error',
                    );
                    return
                }
    
                if(esFechaAnteriorAlUltimoCierre){   
                    ejecutarProceso = false;
                    const msg=`No se puede ejecutar un cierre diario con fecha anterior al último cierre: ${moment(fechaUltimoCierre).format('DD/MM/YYYY')}`
                    requestAuditoria.tipoAccion='VALIDACION';
                    requestAuditoria.idTipoAccion=9;
                    requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                         \n fecha:  ${moment(request.fechaCierre).format("DD/MM/YYYY HH:mm")}
                         \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                         \n Validación: ${msg} `
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                    NotificationService.genericAlertError(
                        'Error',               
                         msg,
                        'error',
                    );
                    return
                }
    
                if(esSuperiorAlMesCierre){        
                    ejecutarProceso = false;
                    const msg=`La fecha del cierre diario, no puede ser el siguiente mes. (Primero debe realizar el cierre diario de: ${moment(ultimoDiaMes).format('DD/MM/YYYY')})`
                    requestAuditoria.tipoAccion='VALIDACION';
                    requestAuditoria.idTipoAccion=9;
                    requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                         \n fecha:  ${moment(request.fechaCierre).format("DD/MM/YYYY HH:mm")}
                         \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                         \n Validación: ${msg} `
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                    NotificationService.genericAlertError(
                        'Error',               
                        msg,
                        'error',
                    );
                    return
                }

                if(debeCerrarMesAnterior){        
                    ejecutarProceso = false;
                    const msg=`No se puede realizar el cierre con esta fecha, debe realizarce antes el cierre: ${moment(ultimoDiaMes).add(1, 'months').endOf('month').format('DD/MM/YYYY')}`
                    requestAuditoria.tipoAccion='VALIDACION';
                    requestAuditoria.idTipoAccion=9;
                    requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                         \n fecha:  ${moment(request.fechaCierre).format("DD/MM/YYYY HH:mm")}
                         \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                         \n Validación: ${msg} `
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                    NotificationService.genericAlertError(
                        'Error',               
                        msg,
                        'error',
                    );
                    return
                }
            }  
            

        }


        try {
            
            if (request.actualiza) {
                
                await swal({
                    text: 'Este proceso actualizará los datos. ¿Desea continuar?',
                    icon: 'warning',
                    buttons: { 
                        confirm: {
                            text: "Sí",
                            value: true,
                            visible: true,
                        },
                        cancel: {
                            text: "No",
                            value: false,
                            visible: true,
                        }
                     },
                }).then((respuesta) => {
                    if(!respuesta){
                        ejecutarProceso = false;
                    }
                })
            }

            if(ejecutarProceso){
                setIsLocalLoading(true);
                const response = await creditApi.postCierreDiarioCartera(request);
                const { data, isSuccess } = response;
                
                if (data != null && isSuccess) {
                  setDataCierreDiario(data);
                  setMostrarTabla(true);

                    if(request.actualiza){
                        
                        const fechaUltimoCierre = new Date(fechaUltimoCierreDiario[0]?.fechaCierre);
                        fechaUltimoCierre.setDate(fechaUltimoCierre.getDate() + 1);
                        
                        const datos = {
                            fechaCierre: fechaCierre.startOf('day').format(),
                            actualiza: true,
                            creadoPor: user.USR_ID,
                        }
                        // insertarCierreDiarioMoratorio(datos)
                        requestAuditoria.tipoAccion='TRANSACCIONES';
                        requestAuditoria.idTipoAccion=2;
                        requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                             \n fecha:  ${moment(request.fechaCierre).format("DD/MM/YYYY HH:mm")}
                             \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}`
                        await auditoriaApi.InsertAuditoria(requestAuditoria);
                    }
                    
                } else {
                    const error= response?.returnMessage;
                    requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
                    requestAuditoria.idTipoAccion=8;
                    requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                         \n fecha:  ${moment(request.fechaCierre).format("DD/MM/YYYY HH:mm")}
                         \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                         \n Error:  ${error}
                         \n returnApiMessage: ${response.returnMessage}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                  toast(error, { type: toast.TYPE.ERROR });
                  setMostrarTabla(false);
                }
            }
          } catch (error:any) {
            requestAuditoria.tipoAccion='ERRORES Y EXCEPCIONES';
            requestAuditoria.idTipoAccion=8;
            requestAuditoria.descripcion=`Cierre diario  de  cartera parámetros:
                 \n fecha:  ${moment(request.fechaCierre).format("DD/MM/YYYY HH:mm")}
                 \n Actualiza Seven:  ${request.actualiza?'Sí':'No'}
                 \n Error:  ${error}
                 \n returnApiMessage: ${error?.response?.returnMessage}`
            await auditoriaApi.InsertAuditoria(requestAuditoria);
            toast("Error del sistema", { type: toast.TYPE.ERROR });
          } finally {
            setIsLocalLoading(false);
          }
          

    };

    const construirMovimientosContables = async(request: any) => {
       

        setIsLocalLoading(true)
        formularioCierreDiarioCartera.getValues('fechaCierre')
        const fechaCierreValidar = moment(formularioCierreDiarioCartera.getValues('fechaCierre')).utcOffset('-05:00');
        const user = AuthService.getUserInfo();

        const data : any = {
            fechaCierre: fechaCierreValidar.format(),
            actualiza: false,
            creadoPor: user.USR_ID,
            tipoCierre: 1
        }
        ApiClients
        .postConstruirMovimientosContables(data)
        .then(response => {
            if(response.isSuccess) {

                if (response.isSuccess && response.data.length > 0){         
                    toast('Construcción de Movimientos EXITOSA', { type: toast.TYPE.SUCCESS });
                    const array: any[] = [];
                    response.data.forEach((element: any, index: any) => {
                        const resultados =
                            element.causacionInteresesRemanente +
                            element.causacionSeguroHogar +
                            element.causacionSeguroVida +
                            element.causacionInteresesCorriente;
                        array.push(resultados);
                    });
    
                    const arrayDos = response.data.map((item: any, index: any) => ({
                        ...item,
                        total: array[index],
                    }));
                    setDataCierreDiario(arrayDos);
                    setMostrarTabla(true);




                } else{
                    toast('Construcción de Movimientos FALLO', { type: toast.TYPE.ERROR });
                }

            }
        })
        .catch((error) => {
            if (error.response && error.response.data ) {
                toast(error.response.data.returnMessage, { type: toast.TYPE.ERROR });
            } else {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
            }
        }).finally(() => setIsLocalLoading(false));
    } 

    // if (isLocalLoading ) {
    //     return <CHLoadingProcess />;
    // }

    return (
        <div  
        style={{
            display: 'table',
            tableLayout: 'fixed',
            width: '100%',
            
        }}>
                
        <LoadingV2Component key="loader" loading={isLocalLoading} tittle='Ejecutando Cierre Moratorio...' />

         <Header />
         <CHPrivateContainer>
            <PageIndicators
                topTextNivel1="Cierres Cartera"
                topTextNivel2="Cierre Diario"
            />
            <IconTitle
                title="Cierre Diario"
                subTitle="Interes Moratorio"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid container>
                <Grid item xs={12} >
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <CHContainerSubtitle
                                        subTitle="de Datos"
                                        title="Actualización"
                                        icon="fch-Procedimientos-solicitudes-0-1"
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaCierre"
                                        control={formularioCierreDiarioCartera.control}
                                        rules={{
                                            required: 'Este campo es requerido.'
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHDatePicker
                                                error={!!error}
                                                label="Fecha de cierre diario *"
                                                value={value}
                                                onChange={onChange}
                                            />                                          
                                        )}                                        
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="actualizacion"
                                        control={formularioCierreDiarioCartera.control}
                                        render={({
                                            field: { onChange, value },
                                        }) => (
                                            <RadioGroup
                                                value={value}
                                                onChange={onChange}
                                                row
                                                aria-label="actualización"
                                                defaultValue="0"
                                                name="row-radio-buttons-group"
                                            >
                                                <FormControlLabel
                                                    value="1"
                                                    control={
                                                        <Radio color="primary" />
                                                    }
                                                    label="DEFINITIVO"
                                                />
                                                <FormControlLabel
                                                    value="0"
                                                    control={
                                                        <Radio color="primary" />
                                                    }
                                                    label="SIMULADO"
                                                />
                                            </RadioGroup>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <br />
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <Grid className="buttonssolicitud">
                <Button
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={formularioCierreDiarioCartera.handleSubmit(onSubmit)}
                >
                    CIERRE
                </Button>

                <Button
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={
                        formularioCierreDiarioCartera.handleSubmit(construirMovimientosContables)
                    }
                >
                    SEVEN
                </Button>
            </Grid>

            <br />
            <div className='MuiTableCell-stickyHeader; jss86'>
            <CierreTable 
    data={dataCierreDiario} 
    mostrarTabla={mostrarTabla}
    fechaCierre={formularioCierreDiarioCartera.getValues('fechaCierre')}
    esDefinitivo={esDefinitivo}
/>
            </div>
            
            </CHPrivateContainer>
        </div>
        
    );
};

interface LinkMapStateToProps {
    productoState: ProductState;
}

const mapStateToProps = (state: AppState) => ({
    productoState: state.producto,
});

interface LinkMapDispatchToProps {
    startPostCierreDiarioCartera: (actualizacionDatos: boolean) => void;
}

const mapDispatchToProps = {
    startPostCierreDiarioCartera,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(CierreDiarioCartera);
