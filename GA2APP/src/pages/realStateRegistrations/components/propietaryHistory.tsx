import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles, } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { AppState } from '../../../store/rootReducer';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { HistorialPropietariosDTO } from '../../../types/realStateRegistrations.type';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';
import NotificationService from '../../../services/notification.service';
import { startDeletePropietaryHistory } from '../../../actions/realStateRegistrations.action';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CreatePropietary from './createPropietary';
import UtilService from '../../../utils/util.service';
import ApiReportes from '../../../services/api-reportes.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise'
import CatalogApi from '../../../services/api-catalog.service';

const useStyles = makeStyles(() =>
    createStyles({

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

        button: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            width: '18%',
            marginLeft: '80%',
        },

    }),
);

function propietaryHistory({
    realStateRegistrationsState,
    startDeletePropietaryHistory,
    catalogState,
    dataActualizada,
    consultar,
}: {
    realStateRegistrationsState: RealStateRegistrationsState;
    startDeletePropietaryHistory: (historiaPropietariosDelete: string) => void;
    catalogState: CatalogState;
    dataActualizada:any
    consultar: () => void;
}): any {
    const classes = useStyles();
    const [isOpenCreate, setIsOpenCreate] = useState<boolean>(false);

    useEffect(() => {
        consultar();
     }, [realStateRegistrationsState?.informacionBasica]);

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
                .catch((error: any) => console.error('Error creating offer:', error));

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

    const onClickExcel = async () => {

        const columnDataList = dataActualizada?.propietarios?.map((row: any) => ({
            data: [
                row.prpTipoIdentificacion,
                String(row.prpNumeroIdentificacion),
                String(row.prpNombreRazonSocial).toUpperCase() || `${row?.prpPrimerNombre || ''} ${row?.prSegundoNombre || ''} ${row?.prpPrimerApellido || ''} ${row?.prpSegundoApellido || ''}`,
                row.telefono,
                row.correo,
                realStateRegistrationsState?.informacionBasica?.maiNumeroMatricula|| 'N/A',
                UtilService.getFullDate(row.pmaFecCre)  
            ]
        }));
        
        if (columnDataList) { // Comprobación de nulidad
            const request = {
                columnDataList: columnDataList.map((item: any) => ({
                    data: item.data
                })),
                columnName: ["Tipo de Documento","N° Documento Afiliado","Nombres/Razón Social", "Teléfono", "Correo Electrónico", "N° Matricula","Fecha Solicitud"]
            };
            
            const apiReportes = ApiReportes();
           
            const response = await apiReportes.GenerarExcels(request);

            // Crear un blob con los datos recibidos
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

            // Crear un objeto URL para el blob
            const url = window.URL.createObjectURL(blob);

            // Crear un enlace temporal y hacer clic en él para iniciar la descarga
            const link = document.createElement('a');
            link.href = url;
            link.download = 'archivo.xlsx';
            document.body.appendChild(link);
            link.click();

            // Liberar el objeto URL
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
                modulo: 'Matrículas Inmobiliarias / Historial de propietarios',
                hostname: document.location.host,
                tipoAccion: 'DESCARGAS',
                idTipoAccion: 5,
                descripcion: 'Descarga de archivo Excel del historial de propietarios'

            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        }
    }

    const onClose= () => {
        setIsOpenCreate(false)
        consultar();
    };

    return (
        <Grid container>
            <CHPaperNew>
                <table className="table">
                    <thead>
                        <tr>
                            <th className='column2'>Tipo de Identificación</th>
                            <th className='column2'>N° Identificación</th>
                            <th className='column2'>Razón Social</th>
                            <th className='column2'>Teléfono</th>
                            <th className='column2'>Correo Electrónico</th>
                            <th className='column2'>Matrícula Inmobiliaria</th>
                            <th className='column2'>Fecha Creacion</th>
                        </tr>
                    </thead>
                    <tbody>
                    {dataActualizada?.propietarios?.sort((a: any,b: any) => new Date(b.pmaFecCre).getTime() - new Date(a.pmaFecCre).getTime()).map((row: HistorialPropietariosDTO, index: number) => (
                            <tr key={`table-${index}`}>
                                <td className='tab-space-01'>
                                    {catalogState.IdentificationType.find(
                                        x => x.id === row.prpIdTipoIdentificacion
                                    )?.descripcion}
                                </td>
                                <td className='column3'>{row.prpNumeroIdentificacion}</td>
                                <td className='tab-space-01'>
                                    {row.prpNombreRazonSocial ? String(row.prpNombreRazonSocial).toLocaleUpperCase() : `${String(row.prpPrimerNombre).toLocaleUpperCase()} ${String(row.prSegundoNombre).toLocaleUpperCase()} ${String(row.prpPrimerApellido).toLocaleUpperCase()} ${String(row.prpSegundoApellido).toLocaleUpperCase()}`}
                                </td>
                                <td className='tab-space-01'>
                                    {row.telefono}
                                </td>
                                <td className='tab-space-01'>
                                    {row?.correo}
                                </td>
                                <td className='tab-space-01'>
                                    {realStateRegistrationsState?.informacionBasica?.maiNumeroMatricula}
                                </td>
                                <td className="tab-space-01">
                                    {UtilService.getFullDateWithTimeDirection(row.pmaFecCre)}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <div className="buttons">
                    <Button
                        className={classes.button}
                        variant="outlined"
                        type="submit"
                        onClick={() => setIsOpenCreate(true)}
                    >
                        AGREGAR PROPIETARIO
                    </Button>
                </div>
                <div className="buttons">
                    <Button
                        className={classes.button}
                        variant="outlined"
                        type="submit"
                        onClick={() => onClickExcel()}
                    >
                        EXCEL
                    </Button>
                </div>
                <CreatePropietary
                    isOpen={isOpenCreate}
                    onClose={onClose}
                    catalogState={catalogState}
                />
            </CHPaperNew>
        </Grid>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    realStateRegistrationsState: state.realStateRegistrations,
});

const mapDispatchToProps = {
    startDeletePropietaryHistory,
};


export default connect(mapStateToProps, mapDispatchToProps)(propietaryHistory);
