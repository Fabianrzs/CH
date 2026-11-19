import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { AppState } from '../../../store/rootReducer';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { HistorialCambios, HistorialPropietariosDTO, MatriculasInmobiliariasDTO } from '../../../types/realStateRegistrations.type';
import { startUpdatePropietaryHistory, startGetBasicInformation } from '../../../actions/realStateRegistrations.action';
import EditHistory from './editHistory';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CreatePropietary from './createPropietary';
import UtilService from '../../../utils/util.service';
import ApiReportes from '../../../services/api-reportes.service';
import CatalogApi from '../../../services/api-catalog.service';
import AuditoriaApi from '../../../services/api-auditorias.servise'
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';

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

const PropietaryHistory = ({
  dataActualizada,
  catalogState,
  consultar,
}: {
  dataActualizada: MatriculasInmobiliariasDTO;
  catalogState: CatalogState;
  consultar: () => void;
}): JSX.Element => {

  const classes = useStyles();
  const [isOpenCreate, setIsOpenCreate] = useState<boolean>(false);
  const [isOpenEdit, setIsOpenEdit] = useState<boolean>(false);

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

    const columnDataList = dataActualizada?.cambios?.map((row) => ({

      data: [
        String(row.matcPestaña),
        String(row.matcCampo),
        String(row.matcVlrAnterior) || '',
        String(row.matcVlrNuevo) || '',
        String(row.match_user),
        UtilService.getFullDate(row.matc_fecmood) || '',
        String(row.usuarioBloqueado)
      ]
    }));

    if (columnDataList) { // Comprobación de nulidad
      const request = {
        columnDataList: columnDataList.map(item => ({
          data: item.data
        })),
        columnName: ["Pestaña", "Campo", "Valor Anterior", "Valor Actualizacion", "Modulo", "Fecha Modificacion", "Usuario"]
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
        modulo: 'Matrículas Inmobiliarias / Historial de cambios',
        hostname: document.location.host,
        tipoAccion: 'DESCARGAS',
        idTipoAccion: 5,
        descripcion: 'Descarga de archivo Excel del historial de cambios'

      } as Auditoria;

      auditoriaApi.InsertAuditoria(requestAuditoria);
    }
  }

  useEffect(() => {
    consultar();
  }, []);

  const onCloseEdit = () => {
    setIsOpenEdit(false);
    setIsOpenCreate(false);
  };

  return (
    <Grid container>
      <CHPaperNew>
        <table className="table">
          <thead>
            <tr>
              <th className='column2'>Pestaña</th>
              <th className='column2'>Campo</th>
              <th className='column2'>Valor Anterior</th>
              <th className='column2'>Valor Actualizacion</th>
              <th className='column2'>Modulo</th>
              <th className='column2'>Fecha Modificacion</th>
              <th className='column2'>Usuario</th>
            </tr>
          </thead>
          <tbody>

            {dataActualizada?.cambios?.map((row: HistorialCambios, index: number) => (
              <tr key={`table-${index}`}>
                <td className='tab-space-01'>{row.matcPestaña}</td>
                <td className='tab-space-01'>{row.matcCampo}</td>
                <td className='tab-space-01'>{row.matcVlrAnterior}</td>
                <td className='tab-space-01'>{row.matcVlrNuevo}</td>
                <td className='tab-space-01'>{row.match_user}</td>
                <td className='column4'>{UtilService.getFullDateWithTimeDirection(row.matc_fecmood)}</td>
                <td className='tab-space-01'>{row.usuarioBloqueado}</td>

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
          consultar={consultar}
        />
        <CreatePropietary
          isOpen={isOpenCreate}
          onClose={onCloseEdit}
          catalogState={catalogState}
        />
      </CHPaperNew>
    </Grid>
  );
};

const mapStateToProps = (state: AppState) => ({
  catalogState: state.catalog,
  loginState: state.login,
  informacionBasica: state.informacionBasica
});

const mapDispatchToProps = {
  startUpdatePropietaryHistory,
  startGetBasicInformation
};

export default connect(mapStateToProps, mapDispatchToProps)(PropietaryHistory);