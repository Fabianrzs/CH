import React, { useEffect, useState } from 'react';
import { Grid } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { AppState } from '../../../store/rootReducer';
import TableUpdates from './TableUpdates';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { ProductoDto } from '../../../types/producto.model.type';
import AuthService from '../../../services/auth.service';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import { Auditoria } from '../../../types/Auditoria.model.type';
import { ClienteDto } from '../../../types/clienteUnidadEjecutora.type';
import { ProductState } from '../../../reducers/producto.reducer';
import CatalogApi from '../../../services/api-catalog.service';

const ProductUpdates = (props: props) => {

    const { producto, clienteRequest, productoState } = props;
    const productoData = productoState.productos.find((objeto: any) => objeto.idCredito === producto.idCredito);

    const idCredito = productoData?.idCredito;
    const estadoCredito = productoData?.estadoCredito;
    const idEstadoCredito = productoData?.idEstadoCredito;

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

    useEffect(() => {
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
        requestAuditoria.descripcion = `Consultó pestaña Novedades de cartera en botón Detalle  parámetros:            
            \n Identificación cliente:  ${clienteRequest.identificacion}`
        auditoriaApi.InsertAuditoria(requestAuditoria);
    })
    return (
        <CHPrivateContainer>
            <CHPaper title="Novedades de Cartera">
                <Grid container>
                    <TableUpdates
                        producto={producto}
                        idCredito={idCredito}
                        estadoCredito={estadoCredito}
                        idEstadoCredito={idEstadoCredito}
                        productoData={productoData}
                        clienteRequest={clienteRequest}
                    />
                </Grid>
            </CHPaper>
        </CHPrivateContainer>
    )
}

interface LinkMapDispatchToProps {

    producto: ProductoDto;
    clienteRequest: ClienteDto
    productoState: ProductState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    catalogState: state.catalog,
    simuladorState: state.simulador,
    productoState: state.producto,
});

type props = LinkMapDispatchToProps;
export default connect(mapStateToProps)(ProductUpdates);