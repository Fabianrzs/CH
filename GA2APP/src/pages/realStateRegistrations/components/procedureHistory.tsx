import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles, } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { AppState } from '../../../store/rootReducer';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { HistorialPropietariosDTO, HistorialTramites } from '../../../types/realStateRegistrations.type';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';
import EditHistory from './editHistory';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CreatePropietary from './createPropietary';
import ApiReportes from '../../../services/api-reportes.service';
import UtilService from '../../../utils/util.service';
import AuditoriaApi from '../../../services/api-auditorias.servise'
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
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

function procedureHistory({
    realStateRegistrationsState,
    catalogState,
    consultar,
}: {
    realStateRegistrationsState: RealStateRegistrationsState;
    catalogState: CatalogState;
    consultar: () => void;
}): any {

    const classes = useStyles();
    const [isOpenEdit, setIsOpenEdit] = useState<boolean>(false);
    const [isOpenCreate, setIsOpenCreate] = useState<boolean>(false);

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

    const onClickExcel = async () => {
        const columnDataList = realStateRegistrationsState?.informacionBasica?.tramites?.map((row) => ({
            data: [
                catalogState.IdentificationType.find(x => x.id.toString() === row.tipoDoc)?.descripcion,
                String(row.nroDoc),
                row.nombreRazonSocial,
                String(row.nroRadicado) || '',
                row.tipoSolicitud || '',
                row.modalidad || '',
                // aqui va el llamado a beneficiario de estudio
                String(UtilService.getFullDate(row.fecSolicitud)) || '',
                String(UtilService.formatCurrency(row.valorSolicitud)) || '',
                row.estado|| '',
            ]
        }));
        
        if (columnDataList) { // Comprobación de nulidad
            const request = {
                columnDataList: columnDataList.map(item => ({
                    data: item.data
                })),
                columnName: ["Tipo de Documento","N° Documento Afiliado","Nombres/Razón Social", "N° Radicado","Tipo Solicitud","Modalidad","Beneficiario de Estudio", "Fecha Solicitud","Valor Solicitud","Estado"]
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
                modulo: 'Matrículas Inmobiliarias / Lista de trámites',
                hostname: document.location.host,
                tipoAccion: 'DESCARGAS',
                idTipoAccion: 5,
                descripcion: 'Descarga de archivo Excel de la Lista de trámites'

            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        }
    }

    const onCloseEdit = () => {
        setIsOpenEdit(false)
        setIsOpenCreate(false)
    };

    return (
        <Grid container>
            <CHPaperNew>
                <table className="table">
                    <thead>
                        <tr>
                            <th className='column2'>Tipo de Documento</th>
                            <th className='column2'>N° Documento</th>
                            <th className='column2'>Afiliado</th>
                            <th className='column2'>N° Radicado</th>
                            <th className='column2'>Tipo Solicitud</th>
                            <th className='column2'>Modalidad</th>
                            <th className='column2'>Fecha Solicitud</th>
                            <th className='column2'>Valor Solicitud</th>
                            <th className='column2'>Estado</th>
                        </tr>
                    </thead>
                    <tbody>
                        {realStateRegistrationsState?.informacionBasica?.tramites?.map((row: HistorialTramites, index: number) => (
                            <tr key={`table-${index}`}>
                                <td className='tab-space-01'>{catalogState.IdentificationType.find(x => x.id.toString() === row.tipoDoc)?.descripcion}</td>
                                <td className='tab-space-01'>{row.nroDoc}</td>
                                <td className='tab-space-01'>{row.nombreRazonSocial}</td>
                                <td className='tab-space-01'>{row.nroRadicado}</td>
                                <td className='tab-space-01'>{row.tipoSolicitud}</td>
                                <td className='tab-space-01'>{row.modalidad}</td>
                                <td className='column4'>{UtilService.getFullDateWithTimeDirection(row.fecSolicitud)}</td>  
                                <td className='tab-space-01'>{UtilService.formatCurrency(row.valorSolicitud)}</td>
                                <td className='tab-space-01'>{row.estado}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
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
                <EditHistory
                    isOpen={isOpenEdit}
                    onClose={onCloseEdit}
                    catalogState={catalogState}
                    data={{} as HistorialPropietariosDTO}
                    consultar= {consultar}
                />
                <CreatePropietary
                    isOpen={isOpenCreate}
                    onClose={onCloseEdit}
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

export default connect(mapStateToProps)(procedureHistory);