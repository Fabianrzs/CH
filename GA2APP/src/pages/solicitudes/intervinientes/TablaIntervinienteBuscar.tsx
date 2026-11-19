import {
  Button,
  createStyles,
  Grid,
  makeStyles,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  Tooltip
} from "@material-ui/core";
import { CloudDownload } from "@material-ui/icons";
import React, { useEffect, useState } from "react";
import { toast } from 'react-toastify';
import moment from 'moment';
import { LoadingComponent } from "../../../components/loading/loading.component";
import history from '../../../utils/history.router';
import ApiReportes from '../../../services/api-reportes.service';
import { Auditoria } from "../../../types/Auditoria.model.type";
import CatalogApi from "../../../services/api-catalog.service";
import AuthService from "../../../services/auth.service";
import AuditoriaApi from '../../../services/api-auditorias.servise'

const useStyles = makeStyles((theme) =>
  createStyles({
    column: {
      padding: '10px',
      textAlign: 'center',
    },
    button: {
      width: '400px',
      marginLeft: '.5rem',
      '&:hover': {
        backgroundColor: '#FFFFFF !important',
        color: 'black !important'
      },
    },
    buttonRegresar: {
      width: '120px',
    },
    processButton: {
      backgroundColor: '#1C3F8B',
      color: '#fff',
    },
    margin: {
      marginLeft: '-100px',
      marginRight: '-90px'
    },
    marginBotonRegresar: {
      textAlign: 'end',
      marginLeft: '-90px',
      marginRight: '-90px'
    },
    root: {
      width: '100%',
      marginTop: theme.spacing(3),
      overflowX: 'auto',
    },
    container: {
      maxHeight: 800,
    },
    tableHeader: {
      backgroundColor: '#FEC917',
      borderBottom: '1px solid #e0e0e0',
    },
    alternateRow: {
      backgroundColor: '#D3D3D3',
    },
    icon: {
      padding: '8px',
      textAlign: 'inherit',
      color: '#42b7ef',
      margin: '10px',
      cursor: 'pointer'

    },
    iconRed: {
      padding: '8px',
      textAlign: 'inherit',
      color: '#FF0000',
      margin: '10px',
      cursor: 'pointer'
    },
  })
);

const TablaIntervinienteBuscar = ({ data, listaTipoIdentificacion, listaTipoActores, infoGrupoUsuario, page, setPage, eliminarInterviniente }: { data: any; listaTipoIdentificacion: any; listaTipoActores: any; infoGrupoUsuario: any[]; page: number; setPage: (value: number) => void; eliminarInterviniente: (id: string) => void; }) => {
  const classes = useStyles();
  const apiReportes = ApiReportes();
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [loading, setLoading] = React.useState(false);

  const handleChangePage = (event: any, newPage: any) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: any) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleClick = (row: any) => {
    history.push({
      pathname: '/DetalleActualizacionesInterviniente',
      state: { row }
    });
  };

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

  const handleDownloadExcel = async () => {
    setLoading(true);

    const columnDataList = data.map((row: any) => ({
      data: [
        listaTipoIdentificacion
          .filter((tipo: any) => tipo.id === row.itR_TIPO_DOCUMENTO)
          .map((tipo: any) => tipo.descripcion.toUpperCase())
          .shift() || '',
        String(row.itR_NUMERO_DOCUMENTO) || '',
        String(row.itR_NOMBRE).includes('_') ? row.itR_NOMBRE.split('_')[2] : String(row.itR_NOMBRE).toLocaleUpperCase(),
        String(row.soL_RADICADO_GD) || '',
        String(row.soL_NUMERO_SOLICITUD) || '',
        listaTipoActores
          .filter((actor: any) => actor.itA_ID === row.itA_ID_FK)
          .map((actor: any) => actor.itA_NOMBRE.toUpperCase())
          .shift() || '',
      ]
    }));

    const columnNames = [
      "Tipo Documento",
      "No. Documento",
      "Nombres/Razón Social",
      "Radicado",
      "Número Solicitud",
      "Tipo Actor"
    ];

    const informacionReporte = {
      columnDataList: columnDataList,
      columnName: columnNames
    };

    const auditoriaApi = AuditoriaApi();
    const user = AuthService.getUserInfo();
    const nav = window.navigator as any;
    const usrAg = nav.userAgentData?.brands?.find(
      (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    )?.brand;

    try {
      const response = await apiReportes.GenerarExcels(informacionReporte);

      if (response) {
        const blob = new Blob([response], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `Intervinientes-Fecha:${moment().format()}.csv`;
        document.body.appendChild(link);
        link.click();
        window.URL.revokeObjectURL(url);


        const requestAuditoria: Auditoria = {
          navegador: usrAg,
          sistemaOperativo: nav.userAgentData?.platform,
          nombreUsuario: user?.USR_USERNAME,
          usurioId: user?.USR_ID,
          ip: publicIpAddress,
          modulo: 'Gestión de intervinientes',
          hostname: document.location.host,
          tipoAccion: 'DESCARGAS',
          idTipoAccion: 5,
          descripcion: 'Descarga de archivo Excel intervinientes'
        };

        auditoriaApi.InsertAuditoria(requestAuditoria);
      }

    } catch (error: any) {
      toast('Error al generar reporte intervinientes', { type: toast.TYPE.ERROR });


      const descripcionError = `Error al descargar el archivo Excel de intervinientes.
        - Error: ${error?.message || 'Error desconocido'}
        - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
        - Código HTTP: ${error?.response?.status || 'No disponible'}`;

      const requestAuditoriaError: Auditoria = {
        navegador: usrAg,
        sistemaOperativo: nav.userAgentData?.platform,
        nombreUsuario: user?.USR_USERNAME,
        usurioId: user?.USR_ID,
        ip: publicIpAddress,
        modulo: 'Gestión de intervinientes',
        hostname: document.location.host,
        tipoAccion: 'ERRORES Y EXCEPCIONES',
        idTipoAccion: 8,
        descripcion: descripcionError
      };

      auditoriaApi.InsertAuditoria(requestAuditoriaError);
    } finally {
      setLoading(false);
    }
  };


  const mostrarNombre = (primerNombre: string, segundoNombre: string, primerApellido: string, segundoApellido: string) => {
    let nombre = '';
    if(primerNombre){
      const razonSocial = primerNombre.split('_')[2] || '';
      if (razonSocial) {
        nombre = razonSocial;
      } else {
        nombre = `${primerNombre} ${segundoNombre} ${primerApellido} ${segundoApellido}`;
      }
    }
    return nombre.toUpperCase();
  }

  return (
    <Paper className={classes.root}>
      <LoadingComponent key="loader" loading={loading} />
      <Grid container justifyContent="flex-end" style={{ padding: '20px', backgroundColor: '#FEC917' }}>
        <Button
          variant="contained"
          color="primary"
          onClick={handleDownloadExcel}
          startIcon={<CloudDownload />}
        >
          Descargar Excel
        </Button>
      </Grid>
      <TableContainer className={classes.container}>
        <Table stickyHeader aria-label="sticky table">
          <TableHead>
            <TableRow>
              <TableCell className={classes.tableHeader}>Tipo Documento</TableCell>
              <TableCell className={classes.tableHeader}>No. Documento</TableCell>
              <TableCell className={classes.tableHeader}>Nombres/Razón Social</TableCell>
              <TableCell className={classes.tableHeader}>Radicado</TableCell>
              <TableCell className={classes.tableHeader}>Número Solicitud</TableCell>
              <TableCell className={classes.tableHeader}>Tipo Actor</TableCell>
              <TableCell className={classes.tableHeader}>Acciones</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {(rowsPerPage > 0
              ? data.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
              : data
            ).map((row: any, index: any) => (
              <TableRow key={index} className={index % 2 === 0 ? classes.alternateRow : ''}>
                <TableCell>
                  {
                    listaTipoIdentificacion
                      .filter((tipo: any) => tipo.id === row.itR_TIPO_DOCUMENTO)
                      .map((tipo: any) => tipo.descripcion.toUpperCase())
                      .shift() || 'Descripción no encontrada'
                  }
                </TableCell>
                <TableCell>{row.itR_NUMERO_DOCUMENTO}</TableCell>
                <TableCell>{mostrarNombre(row.itR_NOMBRE, row.itR_SEGUNDONOMBRE, row.itR_PRIMERAPELLIDO, row.itR_SEGUNDOAPELLIDO)}</TableCell>
                <TableCell>{row.soL_RADICADO_GD === 'Sin Tipo Proceso' ? '' : row.soL_RADICADO_GD}</TableCell>
                <TableCell>{row.soL_NUMERO_SOLICITUD === '0' ? '' : row.soL_NUMERO_SOLICITUD}</TableCell>
                <TableCell>{listaTipoActores.filter((actor: any) => actor.itA_ID === row.itA_ID_FK)
                  .map((actor: any) => actor.itA_NOMBRE.toUpperCase())
                  .shift() || 'Descripción no encontrada'}</TableCell>
                <TableCell>
                  <Grid>
                    <Tooltip title="Detalle/Actualizar">
                      <span aria-label="Detalle">
                        <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`}
                          onClick={() => handleClick(row)} />
                      </span>
                    </Tooltip>
                    {infoGrupoUsuario &&
                      infoGrupoUsuario.length > 0 &&
                      infoGrupoUsuario[0]?.grupo === 'GVISP_L' &&
                      infoGrupoUsuario[0]?.estado === "ACTIVO" && (
                        <Tooltip title="Eliminar">
                          <span aria-label="Eliminar">
                            <span className={`ico fch-Eliminar-0-1 icoMid-3 ${classes.iconRed}`}
                              onClick={() => eliminarInterviniente(row?.itR_ID)}
                            />
                          </span>
                        </Tooltip>
                      )}
                  </Grid>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <TablePagination
        rowsPerPageOptions={[10, 25, 50]}
        component="div"
        count={data.length}
        rowsPerPage={rowsPerPage}
        page={page}
        onPageChange={handleChangePage}
        onRowsPerPageChange={handleChangeRowsPerPage}
        labelRowsPerPage="Filas por página:"
      />

    </Paper>
  )
}

export default TablaIntervinienteBuscar;
