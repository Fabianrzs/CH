import React, { useEffect, useState } from "react";
import {
    Button,
    Grid,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@material-ui/core';
import { connect } from "react-redux";
import { toast } from "react-toastify";
import { startGetAllAplicacionPago } from '../../../actions/producto.action';
import CHPaper from "../../../components/ch-paper/CHPaper";
import UtilService from "../../../utils/util.service";
import CreditApi from "../../../services/api-credit.service";
import NotificationService from "../../../services/notification.service";
import { CreditFlowDocumentsRequestModel } from "../../../models/requests/credit-flow-documents-request.model";
import CHLoadingProcess from "../../../components/ch-loading-process/CHLoadingProcess";
import { ProductoDto } from "../../../types/producto.model.type";
import { ClienteDto } from "../../../types/client.model.type";
import { LoginState } from "../../../reducers/login.reducer";
import { CatalogState } from "../../../reducers/catalog.reducer";
import { SimuladorState } from "../../../reducers/simulador.reducer";
import { ProductState } from "../../../reducers/producto.reducer";
import { AppState } from "../../../store/rootReducer";
import ApiproductoClients from "../../../services/api-producto.service";
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuthService from "../../../services/auth.service";
import CatalogApi from "../../../services/api-catalog.service";

function MostrarArchivos(props: any) {

    const api = CreditApi();
    const [listasArchivosMostrar, setListasArchivosMostrar] = useState<any[]>([]);
    const [isLoadingFile, setIsLoadingFile] = useState<boolean>(false);

    const {
        procedureRequest,
        clienteRequest
    } = props;

    useEffect(() => {

        const mensaje = "sin datos"
        const listarArchivos = async () => {

            const request: any = {

                tcR_ID: procedureRequest.idTipoCredito,
                soC_ID: procedureRequest.idCredito,
                afL_ID: 30
            };
            await api
                .postCreditFiles(request)
                .then(async response => {
                    if (response.isSuccess) {
                        setListasArchivosMostrar(response.data)
                    } else {
                        await NotificationService.genericAlert(
                            'Error en actualización',
                            'Ocurrio un error al intentar actualizar, intente de nuevo.',
                            'error',
                        );
                    }
                })
                .catch(() => {
                    // setLoading(false);
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
        }
        const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
        const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
        const catalogApi  = CatalogApi();
        
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
                            setLocalIpAddress((prevIp) => {
                                if (prevIp !== ip) {
                                    enviarIp(ip);
                                    return ip;
                                }
                                return prevIp;
                            });
                        }
                    }
                };

                return () => peerConnection.close();
            };

            fetchLocalIPAddress();
        }, []);

        listarArchivos();
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
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
        } as Auditoria
        requestAuditoria.tipoAccion = 'CONSULTAS';
        requestAuditoria.idTipoAccion = 3;
        requestAuditoria.descripcion = `Consultó pestaña Visualizar archivos en botón Detalle  parámetros:            
                    \n Identificación cliente:  ${clienteRequest.identificacion}`
        auditoriaApi.InsertAuditoria(requestAuditoria);
    }, [])



    const getFileS = (path: string) => async () => {

        const data = await api.postCreditDownloadFileS(path);
        const ext = UtilService.getFileExtension(path);
        if (ext) {
            const mimeType = UtilService.getMimeTypeFromExtension(ext);
            if (mimeType) {
                const file = new Blob([data], { type: mimeType });
                const fileURL = URL.createObjectURL(file);
                window.open(fileURL);
            }
        }

        setIsLoadingFile(false);
    }

    if (isLoadingFile) {
        return <CHLoadingProcess />;
    }

    return (
        <>

            <CHPaper title="Archivos Solicitudes">
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell component="th">
                                <b>Tipo Documento</b>
                            </TableCell>
                            <TableCell component="th">
                                <b>Acción</b>
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {listasArchivosMostrar.map((file, key) => (
                            <TableRow key={`file-${key}`}>
                                <TableCell>{file.tipO_DOCUMENTO}</TableCell>

                                <TableCell>
                                    {
                                        file.rutA_IMAGEN !== null && (
                                            <Button onClick={getFileS(file.rutA_IMAGEN)}>
                                                Ver
                                            </Button>)
                                    }
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </CHPaper>

        </>
    )

}

export default MostrarArchivos