import React, { useEffect, useState } from "react";
import { makeStyles, createStyles } from "@material-ui/core";
import MUIDataTable from "mui-datatables";
import { toast } from "react-toastify";
import moment from "moment";
import UtilService from "../../../utils/util.service";
import ApiReportes from "../../../services/api-reportes.service";
import ApiproductoClients from "../../../services/api-producto.service";
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from "../../../services/auth.service";
import CatalogApi from "../../../services/api-catalog.service";
import { useIp } from '../../../shared/context/IpContext';

const useStyles = makeStyles(() =>
    createStyles({
        columnDate: {
            minWidth: '100px',
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


const getColumnsTable = () => {

    const classes = useStyles();
    const columns = [
        {
            name: 'fechaPago',
            label: 'Fecha de pago',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                    style: {
                        textAlign: 'left',
                    },
                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },
        {
            name: 'fechaAplicacion',
            label: 'Fecha de Aplicación',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },

                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },
        {
            name: 'tipoPago',
            label: 'Tipo Pago',
            options: {
                setCellProps: () => ({
                    style: {
                        textAlign: 'left',
                    },
                    className: classes.columnDate,



                }),
                customBodyRender: (value: string) => (value),
            },
        },
        {
            name: 'valorPago',
            label: 'Valor Pagado',
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
            name: 'abonoCapital',
            label: 'Capital',
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
            name: 'abonoInteresCorriente',
            label: 'Interes Corriente',
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
            name: 'abonoInteresMora',
            label: 'Interes Mora',
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
            name: 'abonoSeguroVida',
            label: 'Seguro de Vida',
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
            name: 'abonoSeguroHogar',
            label: 'Seguro de Hogar',
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
            name: 'capitalRemanente',
            label: 'Capital Remanente',
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
            name: 'interesRemanente',
            label: 'Interés Remanente',
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
    ];

    return columns;
}



const CuotasAdelantadas = (props: any) => {

    const apiReportes = ApiReportes();
    const api = ApiproductoClients();

    const [cuotasAdelantadas, setCuotasAdelantadas] = useState<any[]>([]);
    const columns = getColumnsTable();

    const { publicIp } = useIp();


   
    useEffect(() => {
        obtenerCuotasAdelantadas();
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIp,
            modulo: 'Consulta de cartera',
            hostname: document.location.host
        } as Auditoria
        requestAuditoria.tipoAccion = 'CONSULTAS';
        requestAuditoria.idTipoAccion = 3;
        requestAuditoria.descripcion = `Consultó pestaña Cuotas adelantadas en botón Detalle  parámetros:            
    \n Identificación cliente:  ${props.clienteRequest.identificacion}`
        auditoriaApi.InsertAuditoria(requestAuditoria);
    }, []);

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
                toast('Error del sistema');
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
        const columnDataList = cuotasAdelantadas.map((row) => ({
            data: [
                moment(row.fechaPago).format('DD/MM/YYYY') || '',
                moment(row.fechaAplicacion).format('DD/MM/YYYY') || '',
                row.tipoPago || '',
                String(row.valorPago) || '',
                String(row.abonoCapital) || '',
                String(row.abonoInteresCorriente) || '',
                String(row.abonoInteresMora) || '',
                String(row.abonoSeguroVida) || '',
                String(row.abonoSeguroHogar) || '',
                String(row.capitalRemanente) || '',
                String(row.interesRemanente) || ''
            ]
        }));


        if (columnDataList) {
            const request = {
                columnDataList: columnDataList.map(item => ({
                    data: item.data
                })),
                columnName: ["Fecha de pago", "Fecha de Aplicación",
                    "Tipo Pago", "Valor Pagado",
                    "Capital", "Interes Corriente", "Interes Mora",
                    "Seguro de Vida", "Seguro de Hogar", "Capital Remanente", "Interés Remanente"]
            };
            const response = await apiReportes.GenerarExcels(request);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Cuotas adelantadas ${props.producto.nombreCliente} - Fecha Reporte ${moment().format('YYYY-MM-DD HH_mm_ss')}.xlsx`;
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);

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
                    modulo: 'Consulta Cartera',
                    hostname: document.location.host,
                    tipoAccion: 'DESCARGAS',
                    idTipoAccion: 5,
                    descripcion: 'Descarga de cuotas adelantadas'

                } as Auditoria;

                auditoriaApi.InsertAuditoria(requestAuditoria);
        }
    }

    const obtenerCuotasAdelantadas = () => {
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;

        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIp,
            modulo: 'Consulta de cartera',
            hostname: document.location.host
        } as Auditoria
        api.getCuotasAdelantadas(props.producto.idCredito)
            .then(async response => {
                if (response.isSuccess) {
                    setCuotasAdelantadas(response.data);
                    requestAuditoria.tipoAccion = 'CONSULTAS';
                    requestAuditoria.idTipoAccion = 3;
                    requestAuditoria.descripcion = `Se consulto la cuotas adelantadas parámetros:            
                \n Identificación cliente:  ${props.clienteRequest.identificacion}
                \n Cliente:  ${props.producto.nombreCliente}`
                    await auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            })
            .catch((_) => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    return (

        <div style={{ display: 'table', tableLayout: 'fixed', width: '100%' }}>

            <MUIDataTable title="CUOTAS ADELANTADAS" data={cuotasAdelantadas} columns={columns}
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
                    downloadOptions: {
                        filename: `Cuotas adelantadas ${props.producto.nombreCliente} - Fecha Reporte ${moment().format('YYYY-MM-DD HH_mm_ss')}.csv`,
                        separator: ",",
                    },
                    customToolbar: () =>
                        <button onClick={() => handleExportToExcel()}
                            style={{
                                backgroundColor: 'green',
                                padding: '10px',
                                color: 'white',
                                border: 'none',
                                marginRight: '10px',
                                cursor: 'pointer',
                                borderRadius: '5px',

                            }}
                        >EXCEL</button>
                }}
            />
        </div>

    )

}

export default CuotasAdelantadas;