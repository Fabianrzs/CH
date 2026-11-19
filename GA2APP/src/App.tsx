import { createTheme, CssBaseline, ThemeProvider } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { toast, ToastContainer } from 'react-toastify';
import { connect } from 'react-redux';
import swal from 'sweetalert';
import RouterAppRoot from './router/AppRouting';
import { AppState } from './store/rootReducer';
import './App.css';
import 'react-toastify/dist/ReactToastify.css';
import history from './utils/history.router';
import AuthService from './services/auth.service';
import { Auditoria } from './types/Auditoria.model.type';
import AuditoriaApi from './services/api-auditorias.servise';
import Api from './services/api.core.service';
import ApiTransaccion from './services/api-parametrization.service'
import StorageService from './services/storage.service';
import CatalogApi from './services/api-catalog.service';
import { IpProvider, useIp } from './shared/context/IpContext';


export const App = () => {
    const OpenSans = {
        fontFamily: 'Open Sans',
        fontStyle: 'normal',
        fontDisplay: 'swap',
        fontWeight: 400,
        unicodeRange:
            'U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF',
    } as any;

    const theme = createTheme({
        typography: {
            fontFamily: 'OpenSans-Regular',
            fontSize: 14,
        },
        palette: {
            primary: {
                main: '#1C3F8B',
            },
            background: {
                default: '#f1f2f6'
            }
        },
        overrides: {
            MuiCssBaseline: {
                '@global': {
                    '@font-face': [OpenSans],
                },
            },
        },
    });
    const obtenerTiempo = async function () {
        const api = ApiTransaccion();
        const result = await api.ObtenerParametroTransaccionRequest();
        if (result.isSuccess && result.data.length > 0) {

            result.data.forEach((element: { proceso: string; codigo: number; }) => {
                if (element.proceso === 'TIEMPOSESION')
                    tiempoSesion = element.codigo
            });
        }
    }

    let tiempoSesion: any;
    let t: NodeJS.Timeout;
    const reiniciarTiempo = () => {
        if (tiempoSesion) {
            clearTimeout(t);
            const tiempo = tiempoSesion * 60 * 1000;
            t = setTimeout(tiempoExcedido, tiempo);
        }
    };
    const handleCloseSesion = () => {
        StorageService.removeStorageFromLogout();
        history.push('/');
    };

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



    const ipData = useIp() || {};
    const { publicIp } = ipData;


    const tiempoExcedido = () => {
        const user = AuthService.getUserInfo();
        if (user !== null) {

            const api = Api();
            api.Logout(user.USR_ID);
            const auditoriaApi = AuditoriaApi();
            const nav = (window.navigator as any);
            let usrAg = 'Unknown';
            if (nav.userAgentData && nav.userAgentData.brands) {
                usrAg = nav.userAgentData.brands.filter(
                    (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
                )[0]?.brand || 'Unknown';
            }
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIp,
                modulo: 'Logout',
                hostname: document.location.host
            } as Auditoria;

            window.localStorage.clear()
            handleCloseSesion();
            requestAuditoria.tipoAccion = 'INICIO DE SESIÓN';
            requestAuditoria.idTipoAccion = 1;
            requestAuditoria.descripcion = `Cierre de sesión por inactividad:
                             \n Usuario:  ${user.USR_USERNAME}`;
            auditoriaApi.InsertAuditoria(requestAuditoria);
            swal({
                text: 'Por seguridad, la sesión de usuario ha sido cerrada por inactividad',
                icon: 'warning',
            })
        }
    }

    useEffect(() => {
        obtenerTiempo();


        return () => {
            window.removeEventListener("mousemove", reiniciarTiempo);
            window.removeEventListener("mousedown", reiniciarTiempo);
            window.removeEventListener("keypress", reiniciarTiempo);
            window.removeEventListener("DOMMouseScroll", reiniciarTiempo);
            window.removeEventListener("mousewheel", reiniciarTiempo);
            window.removeEventListener("touchmove", reiniciarTiempo);
            window.removeEventListener("MSPointerMove", reiniciarTiempo);
            window.removeEventListener("load", reiniciarTiempo);
        };
    }, []);
    return (
        <div>
            <ToastContainer
                position="bottom-right"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
            />
            <ThemeProvider theme={theme}>
                <CssBaseline />
                <IpProvider>
                    <RouterAppRoot />
                </IpProvider>
            </ThemeProvider>
        </div>
    );
};

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
});

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(App);
