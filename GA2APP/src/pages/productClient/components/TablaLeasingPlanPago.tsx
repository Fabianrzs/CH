import { createStyles, IconButton, makeStyles, Tooltip } from '@material-ui/core';
import moment from 'moment';
import MUIDataTable from 'mui-datatables';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { CloudDownload } from '@material-ui/icons';
import ApiproductoClients from '../../../services/api-producto.service';
import ApiReportes from '../../../services/api-reportes.service';
import NotificationService from '../../../services/notification.service';
import UtilService from '../../../utils/util.service';
import { ColumnFormatType } from '../../../enums/ColumnFormatType';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import CatalogApi from '../../../services/api-catalog.service';
import { LoadingComponent } from '../../../components/loading/loading.component';
import PdfToolbar from "./reportes/componentes/PdfToolbar";

const useStyles = makeStyles(() =>
    createStyles({
        columnDate: {
            minWidth: '110px',
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

function TablaLeasingPlanPago(props: any) {
    const apiReportes = ApiReportes();
    const classes = useStyles();
    const api = ApiproductoClients()
    const {
        producto,
        clienteRequest
    } = props;

    const ESTADOS: Record<string, string> = {
        P: 'P.T',
        ND: 'No descontó',
        NC: 'P.P'
    }

    useEffect(() => {
        obtnerPlanDePagoLeasing();

    }, []);

    const [datosLeasing, setDatosLeasing] = useState<any[]>([]);
    const [planPagoResponse, setPlanPagoResponse] = useState<any>(null);
    const [productoClienteResponse, setProductoClienteResponse] = useState<any>(null);
    const [abonosResponse, setAbonosResponse] = useState<any[]>([]);

    const [mostrarDatosAbonos, setMostrarDatosAbonos] = useState(false);
    const [loading, setLoading] = useState(false);

    const obtnerPlanDePagoLeasing = () => {

        setLoading(true);
        const request = {
            nro_Credito: producto.numeroCredito,
            tcr_Id: producto.idTipoCredito
        }
        api
            .getPlanDePagoLeasing(request)
            .then(async (response) => {
                if (response) {
                    if (response.isSuccess) {
                        setPlanPagoResponse(response.data);
                        const productoRequest = {
                            tipoIdentificacion: producto.tipoIdentificacion,
                            identificacion: producto.identificacion,
                            idCredito: producto.idCredito
                        };

                        const productoResponse =
                            await api.ObtenerProductoByidentificacionRequest(productoRequest);

                        if (productoResponse.isSuccess) {
                            setProductoClienteResponse(productoResponse.data[0]);
                        }
                        await api.PostObtenerEstadoAbonoCredito(request)
                            .then(async (responseAbono) => {
                                setAbonosResponse(responseAbono.data ?? []);
                                if (responseAbono.data && responseAbono.data.length > 0) {

                                    const array: any[] = [];
                                    response.data.planPago.forEach((itemPlanPago: any, index: any) => {

                                        const abonoCuota = responseAbono.data.find((element: any) => element?.numeroCuota === itemPlanPago?.cuota);
                                        if (abonoCuota && response.data.planPago[index]?.cuota === response.data.planPago[index - 1]?.cuota) {
                                            const filaAbono = {
                                                ...itemPlanPago,
                                                estadoCapital: ESTADOS[abonoCuota?.estadoCapital ?? ''] ?? '',
                                                estadoInteres: ESTADOS[abonoCuota?.estadoInteres ?? ''] ?? '',
                                                estadoSeguroVida: ESTADOS[abonoCuota?.estadoSeguroVida ?? ''] ?? '',
                                                estadoSeguroHogar: ESTADOS[abonoCuota?.estadoSeguroHogar ?? ''] ?? '',
                                                estadoCuota: 'ABONO'
                                            }
                                            array.push(filaAbono);
                                        } else {
                                            array.push(itemPlanPago);
                                        }
                                    })
                                    setMostrarDatosAbonos(true);
                                    setDatosLeasing(array);
                                    setLoading(false);

                                } else {
                                    setDatosLeasing(response.data.planPago);
                                    setMostrarDatosAbonos(false);
                                    setLoading(false);
                                }

                            }).catch((e: any) => {
                                toast(`Error al consultar abonos ${e.message}`, { type: toast.TYPE.ERROR });
                                setLoading(false);
                            });
                    } else {
                        NotificationService.genericAlert(
                            'Error en actualización',
                            `${response.returnMessage}`,
                            'error',
                        );
                        setLoading(false);
                    }
                }
            }).catch(() => {
                toast("Error al consultar plan de pago", { type: toast.TYPE.ERROR });
                setLoading(false);
            });

    }

    const columns = [
        {
            name: 'cuota',
            label: 'Cuota',
            formatType: ColumnFormatType.Numerico,
        },
        {
            name: 'fecha',
            label: 'Fecha',
            formatType: ColumnFormatType.Fecha,
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,

                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },
        {
            name: 'saldoInicial',
            label: 'Saldo inicial',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },

                }),
            },
        },
        {
            name: 'capitalReducido',
            label: 'Capital',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'estadoCapital',
            label: 'Estado Capital',
            options: {
                display: mostrarDatosAbonos,
            },
        },
        {
            name: 'capitalPorPagar',
            label: 'Capital por Pagar',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'interesReducido',
            label: 'Intereses',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'interesPorPagar',
            label: 'Interés por Pagar',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'estadoInteres',
            label: 'Estado Interés',
            options: {
                display: mostrarDatosAbonos,
            },
        },
        {
            name: 'seguroVida',
            label: 'Seguro Vida',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'estadoSeguroVida',
            label: 'Estado Seguro Vida',
            options: {
                display: mostrarDatosAbonos,
            },
        },
        {
            name: 'seguroHogar',
            label: 'Seguro Hogar',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'estadoSeguroHogar',
            label: 'Estado Seguro Hogar',
            options: {
                display: mostrarDatosAbonos,
            },
        },
        {
            name: 'cuotaUnidadPagadora',
            label: 'Cuota a pagar',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'saldoCapital',
            label: 'Saldo a Capital',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'deudaRemanente',
            label: 'Deuda Remanente',
            formatType: ColumnFormatType.Decimal,
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
                setCellProps: () => ({
                    style: {
                        textAlign: 'right',
                    },
                }),
            },
        },
        {
            name: 'estadoCuota',
            label: 'Estado de pago',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'center',
                    },
                }),
            },
        }
    ];

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

    const handleExportToExcel = async () => {
        const columnDataList = datosLeasing.map((row: any) => ({
            data: columns.map(x => {

                if (x.formatType === ColumnFormatType.Fecha && row[x.name]) {
                    const parseDate = moment(row[x.name]);
                    return parseDate.isValid()
                        ? parseDate.format("YYYY-MM-DD")
                        : '';
                }

                return row[x.name]?.toString() ?? ''
            })
        }));

        if (columnDataList) {
            const request = {
                columnData: columnDataList.map(item => item.data),
                columnsInfo: columns.map(x => ({
                    name: x.label,
                    formatType: x.formatType
                }))
            };
            const response = await apiReportes.GenerarExcelInvariantCulture(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Plan pagos leasing - ${clienteRequest.identificacion} - ${clienteRequest.primerNombre} ${clienteRequest.primerApellido} - ${moment().format('YYYY-MM-DD HH_mm_ss')}.xlsx`;
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(link);
            const user = AuthService.getUserInfo();
            const auditoriaApi = AuditoriaApi();
            const nav = (window.navigator as any);
            const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta de cartera',
                hostname: document.location.host
            } as Auditoria;
            requestAuditoria.tipoAccion = 'DESCARGAS';
            requestAuditoria.idTipoAccion = 5;
            requestAuditoria.descripcion = `Descarga de reporte plan de pagos leasing en formato excel:
                         \n archivo: ${link.download}`;
            await auditoriaApi.InsertAuditoria(requestAuditoria);
        }
    }

    const generarPDF = async () => {

        if (!planPagoResponse || !productoClienteResponse) {
            toast("No existe información para generar el PDF", {
                type: toast.TYPE.WARNING
            });
            return;
        }

        const requestReporte = {
            ...productoClienteResponse,
            planPago: []
        };

        try {

            requestReporte.planPago = planPagoResponse.planPago.map((item: any) => {

                const abono = abonosResponse.find(
                    (x: any) => x.numeroCuota === item.cuota
                );

                return {

                    Fecha: item.fecha,
                    Cuota: item.cuota,

                    SeguroVida: item.seguroVida,
                    SeguroHogar: item.seguroHogar,

                    CuotaTradicional: item.cuota,
                    CuotaUnidadPagadora: item.cuotaUnidadPagadora ?? 0,

                    CapitalTradicional: item.capitalTradicional ?? 0,
                    InteresTradicional: item.interesTradicional ?? 0,
                    SaldoTradicional: item.saldoTradicional ?? 0,

                    CapitalReducido: item.capitalReducido ?? 0,
                    InteresReducido: item.interesReducido ?? 0,
                    SaldoReducido: item.saldoReducido ?? 0,

                    CapitalPorPagar: item.capitalPorPagar ?? 0,
                    SaldoCapital: item.saldoCapital ?? 0,
                    InteresPorPagar: item.interesPorPagar ?? 0,

                    SaldoTotal: item.saldoTotal ?? 0,

                    Saldo: item.saldo,

                    Interes: item.interes,

                    Capital: item.capital,

                    SaldoInicial: item.saldoInicial ?? item.saldo,

                    DeudaRemanente: item.deudaRemanente ?? 0,

                    EstadoCuota: abono ? "ABONO" : "",

                    AbonosCuota: abono?.valorPago ?? 0
                };
            });

            const response = await apiReportes.reporteInformacionPlanPago(requestReporte);

            const binary = atob(response.data);

            const bytes = new Uint8Array(binary.length);

            for (let i = 0; i < binary.length; i += 1) {
                bytes[i] = binary.charCodeAt(i);
            }

            const pdfBlob = new Blob([bytes], {
                type: "application/pdf"
            });

            const url = URL.createObjectURL(pdfBlob);

            const link = document.createElement("a");

            link.href = url;

            link.download = `Plan pagos leasing - ${clienteRequest.identificacion} - ${clienteRequest.primerNombre} ${clienteRequest.primerApellido} - ${moment().format('YYYY-MM-DD HH_mm_ss')}.pdf`;

            link.click();

            URL.revokeObjectURL(url);

        } catch (e) {

            toast("Error al generar el PDF", {
                type: toast.TYPE.ERROR
            });

        }
    };


    return (
        <div
            style={{
                display: 'table',
                tableLayout: 'fixed',
                width: '100%',

            }}>


            <div className='MuiTableCell-stickyHeader; jss86'>
                <LoadingComponent loading={loading} />
                <MUIDataTable
                    title="LEASING"
                    data={datosLeasing}
                    columns={columns}
                    options={{
                        selectableRows: 'none',
                        filter: false,
                        print: false,
                        elevation: 2,
                        filterType: 'dropdown',
                        responsive: 'vertical',
                        fixedHeader: true,
                        fixedSelectColumn: true,
                        tableBodyHeight: '430px',
                        rowsPerPageOptions: [5, 15, 20, 100, 200, 500],
                        textLabels: {
                            body: {
                                noMatch: 'No se encontraron resultados',
                            },
                            pagination: {

                                rowsPerPage: 'Filas por página',
                                jumpToPage: '',
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
                            <>
                                <Tooltip title="Descargar Excel">
                                    <IconButton onClick={() => handleExportToExcel()} >
                                        <CloudDownload />
                                    </IconButton>
                                </Tooltip>

                                <PdfToolbar pdfAction={generarPDF} />
                            </>
                        )
                    }}
                />
            </div>
        </div>
    );
}

export default TablaLeasingPlanPago;