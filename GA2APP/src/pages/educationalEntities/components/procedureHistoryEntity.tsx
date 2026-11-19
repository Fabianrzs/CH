import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles, } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import ApiReportes from '../../../services/api-reportes.service';
import { startGetTramiteEntity } from '../../../actions/educationalEntities.actions';
import ApiCatalogo from '../../../services/api-catalog.service';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import UtilService from '../../../utils/util.service';
import { EducationalEntitiesState } from '../../../reducers/educationalEntities.reducer';
import { AppState } from '../../../store/rootReducer';

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
function procedureHistoryEntity({
    idEntity,
    educationalEntities,
    obtenerTramite,
}: {
    idEntity: any;
    educationalEntities: EducationalEntitiesState;
    obtenerTramite: (idEntidad: string) => void;
}): any {

    const classes = useStyles();

    useEffect(() => {
        if(idEntity){
            obtenerTramite(idEntity);
        }
    }, [idEntity]);

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const catalogApi = ApiCatalogo();

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
        if(educationalEntities?.tramite && educationalEntities?.tramite.length > 0){

            const columnDataList = educationalEntities?.tramite?.map((row:any) => ({
                data: [
                    row.tipoDocumento,
                    String(row.cedula),
                    row.nombre || '',
                    String(row.radicado) || '',
                    row.modalidad,
                    row.subModalidad || '',
                    UtilService.getFullDateWithTimeDirection(row.fechaCreacion),
                    row.programa || '',
                    row.nivel || '',
                    row.estado || ''
                ]
            }));

            if (columnDataList) {
                const request = {
                    columnDataList: columnDataList.map(item => ({
                        data: item.data
                    })),
                    columnName: [
                        "Tipo de Documento",
                        "N° Documento Afiliado",
                        "Nombres/Razón Social",
                        "N° Radicado",
                        "Tipo Solicitud",
                        "Modalidad",
                        "Fecha Solicitud",
                        "Programa Educativo",
                        "Nivel Educativo",
                        "Estado"
                    ]
                };

                const apiReportes = ApiReportes();
                const response = await apiReportes.GenerarExcels(request);
                const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                const url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = url;
                link.download = 'archivo.xlsx';
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
                    modulo: 'Gestión de entidades',
                    hostname: document.location.host,
                    tipoAccion: 'DESCARGAS',
                    idTipoAccion: 5,
                    descripcion: 'Descarga de reporte Lista de tramites'
        
                } as Auditoria;
        
                auditoriaApi.InsertAuditoria(requestAuditoria);
            }
        }
    };
   
    return (
        <Grid container>
            <CHPaperNew>
                <table className="table">
                    <thead>
                        <tr>
                            <th className='column2'>Tipo de Documento</th>
                            <th className='column2'>N° Documento Afiliado</th>
                            <th className='column2'>Nombres/Razón Social</th>
                            <th className='column2'>N° Radicado</th>
                            <th className='column2'>Tipo Solicitud</th>
                            <th className='column2'>Modalidad</th>
                            <th className='column2'>Beneficiario de Estudio</th>
                            <th className='column2'>Fecha Solicitud</th>
                            <th className='column2'>Programa Educativo</th>
                            <th className='column2'>Nivel Educativo</th>
                            <th className='column2'>Estado</th>
                        </tr>
                    </thead>
                    <tbody>
                {educationalEntities?.tramite && educationalEntities?.tramite.map((row: any, index: number) => ( 
                            <tr key={`table-${index}`}>
                                <td className='tab-space-01'>{row.tipoDocumento}</td>
                                <td className='tab-space-01'>{row.cedula}</td>
                                <td className='tab-space-01'>{row.nombre}</td>
                                <td className='tab-space-01'>{row.radicado}</td>
                                <td className='tab-space-01'>{row.modalidad}</td>
                                <td className='tab-space-01'>{row?.subModalidad}</td>
                                <td className='tab-space-01'>{row?.beneficiario}</td>
                                <td className='tab-space-01'>
                                    {UtilService.getFullDateWithTimeDirection(row.fechaCreacion)}   
                                </td>
                                <td className='tab-space-01'>{row.programa}</td>
                                <td className='tab-space-01'>{row.nivel}</td>
                                <td className='tab-space-01'>{row.estado}</td>
                            </tr>
                        ))}

                        {educationalEntities?.tramite && educationalEntities?.tramite.length === 0 && (
                            <tr>
                                <td colSpan={9} style={{ textAlign: 'center' }}>No hay datos disponibles</td>
                            </tr>
                        )}
                    </tbody>
                </table>
                <div className="buttons">
                    <Button
                        className={classes.button}
                        variant="outlined"
                        type="submit"
                        onClick={() => onClickExcel()}
                    >
                        REPORTE
                    </Button>
                </div>

            </CHPaperNew>
        </Grid>
    );
}

const mapStateToProps = (state: AppState) => ({
    educationalEntities: state.educationalEntities,
});

const mapDispatchToProps = (dispatch: any) => ({
    obtenerTramite: (idEntidad: string) =>
        dispatch(startGetTramiteEntity(idEntidad))
});

export default connect(mapStateToProps, mapDispatchToProps)(procedureHistoryEntity);