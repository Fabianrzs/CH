import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import moment from 'moment';
import {
  Button, Grid, Paper, Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, TablePagination,
  TextField, MenuItem, makeStyles, IconButton, Menu, Checkbox, FormControlLabel
} from '@material-ui/core';
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import { SaveAlt as SaveAltIcon, FilterList as FilterListIcon } from '@material-ui/icons';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import UtilService from '../../utils/util.service';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import CreditApi from '../../services/api-credit.service';
import ApiReportes from '../../services/api-reportes.service';
import CatalogApi from '../../services/api-catalog.service';
import AuthService from '../../services/auth.service';
import AuditoriaApi from '../../services/api-auditorias.servise';
import { Auditoria } from '../../types/Auditoria.model.type';
import NotificationService from '../../services/notification.service';

interface ReporteCredito {
  fechA_SOLICITUD: string;
  numerO_DOCUMENTO: string;
  nombrE_COMPLETO: string;
  estadoCredito: string;
  departamentO_ID: string;
  ciudaD_ID: string;
  puntO_ATENCION: string;
  tipO_CREDITO: string;
  valoR_INMUBLE: number;
  valoR_AVALUO: number;
  valoR_PRESTAMO: number;
  consecutivo: number;
  descripcionTarea: string;
  correoAsesor: string;
  nombreCompletoAsesor: string;
}

const useStyles = makeStyles({
  tableHeader: {
    backgroundColor: '#FEC917',
    fontWeight: 'bold',
  },
});

const columnasDisponibles = [
  { id: 'fechA_SOLICITUD', label: 'Fecha Solicitud' },
  { id: 'numerO_DOCUMENTO', label: 'Documento' },
  { id: 'nombrE_COMPLETO', label: 'Nombre' },
  { id: 'estadoCredito', label: 'Estado Crédito' },
  { id: 'departamentO_ID', label: 'Departamento' },
  { id: 'ciudaD_ID', label: 'Ciudad' },
  { id: 'puntO_ATENCION', label: 'Punto Atención' },
  { id: 'tipO_CREDITO', label: 'Tipo Crédito' },
  { id: 'valoR_INMUBLE', label: 'Valor Inmueble' },
  { id: 'valoR_AVALUO', label: 'Valor Avalúo' },
  { id: 'valoR_PRESTAMO', label: 'Valor Préstamo' },
  { id: 'descripcionTarea', label: 'Descripción Tarea' },
  { id: 'correoAsesor', label: 'Correo Asesor' },
  { id: 'nombreCompletoAsesor', label: 'Nombre Completo Asesor' },
];

const ReporteCredito = () => {
  const apiCredit = CreditApi();
  const apiReportes = ApiReportes();
  const catalogApi = CatalogApi();
  const classes = useStyles()
  const [fechaInicio, setFechaInicio] = useState<Date | null>(null);
  const [fechaFinal, setFechaFinal] = useState<Date | null>(null);
  const [numeroDocumento, setNumeroDocumento] = useState<string>('');
  const [consecutivoSolicitud, setConsecutivoSolicitud] = useState<string>('');
  const [tipoTarea, setTipoTarea] = useState<any>(0);
  const [estadoTarea, setEstadoTarea] = useState<any>(0);
  const [data, setData] = useState<ReporteCredito[]>([]);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [isLoading, setIsLoading] = useState(false);
  const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
  const [listaTipoTareas, setListaTipoTareas] = useState<any[]>([]);
  const [listaEstadoTareas, setListaEstadoTareas] = useState<any[]>([]);
  const [columnasVisibles, setColumnasVisibles] = useState<string[]>(columnasDisponibles.map(c => c.id));
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const handleOpenMenu = (event: React.MouseEvent<HTMLElement>) => setAnchorEl(event.currentTarget);
  const handleCloseMenu = () => setAnchorEl(null);

  // prueba

  const limpiarFiltros = () => {
    setFechaInicio(null);
    setFechaFinal(null);
    setNumeroDocumento('');
    setConsecutivoSolicitud('');
    setTipoTarea(0);
    setEstadoTarea(0);
  };

  useEffect(() => {
    obtenerTiposTareas();
    obtenerEstadosTareas();
  }, [])

  const obtenerTiposTareas = async () => {
    try {
      const response = await apiCredit.getObtenertipoactividadporid('')
      if (response.isSuccess) {
        setListaTipoTareas(response.data)
      }

    } catch (error) {
      console.log({ error })
    }
  }
  const obtenerEstadosTareas = async () => {
    try {
      const response:any = await catalogApi.getActivityStatus()
      if (response.isSuccess) {
        setListaEstadoTareas(response.data)
      }

    } catch (error) {
      console.log({ error })
    }
  }

  const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);

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
            setLocalIpAddress(ip);
            enviarIp(ip);
          } else {
            setLocalIpAddress(ip);
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

  const handleBuscar = async () => {
    setIsLoading(true);
    const request = {
      fechaInicio: fechaInicio ? fechaInicio.toISOString().split('T')[0] : null,
      fechaFinal: fechaFinal ? fechaFinal.toISOString().split('T')[0] : null,
      numeroDocumento: numeroDocumento.trim() !== '' ? numeroDocumento : null,
      consecutivoSolicitud: consecutivoSolicitud.trim() !== '' ? Number(consecutivoSolicitud) : null,
      tipoTarea: typeof tipoTarea === 'number' && tipoTarea !== 0 ? tipoTarea : null,
      estadoCredito: typeof estadoTarea === 'number' && estadoTarea !== 0 ? estadoTarea : null,

    };

    const auditoriaApi = AuditoriaApi();
    const user = AuthService.getUserInfo();
    const nav = window.navigator as any;
    const usrAg = nav.userAgentData?.brands?.find(
      (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    )?.brand;

    try {
      const response = await apiCredit.postObteberExcelCredito(request);
      if (response?.isSuccess) {
        setData(response.data);
        setPage(0);

        const requestAuditoria = {
          navegador: usrAg,
          sistemaOperativo: nav.userAgentData?.platform,
          nombreUsuario: user.USR_USERNAME,
          usurioId: user.USR_ID,
          ip: publicIpAddress,
          modulo: 'Reportes de crédito',
          hostname: document.location.host,
          tipoAccion: 'CONSULTAS',
          idTipoAccion: 3,
          descripcion: `Consulta de Reportes de crédito con filtros: 
          Número de documento: ${request.numeroDocumento}
          Consecutivo Solicitud: ${request.consecutivoSolicitud}
          Estado tarea: ${request.estadoCredito}
          Tipos Tareas: ${request.tipoTarea}
          Fecha inicio: ${request.fechaInicio}
          Fecha final: ${request.fechaFinal}`,
        } as Auditoria;
        setIsLoading(false);
        auditoriaApi.InsertAuditoria(requestAuditoria);
      } else {
        toast('No hay créditos para estos filtros', { type: toast.TYPE.WARNING });
        setIsLoading(false);
      }
    } catch (error: any) {

      const descripcionError = `Error en consulta de información crédito
      Consulta de Reportes de crédito con filtros: 
          Número de documento: ${request.numeroDocumento}
          Consecutivo Solicitud: ${request.consecutivoSolicitud}
          Estado tarea: ${request.estadoCredito}
          Tipos Tareas: ${request.tipoTarea}
          Fecha inicio: ${request.fechaInicio}
          Fecha final: ${request.fechaFinal}`;

      const requestAuditoriaError: Auditoria = {
        navegador: usrAg,
        sistemaOperativo: nav.userAgentData?.platform,
        nombreUsuario: user?.USR_USERNAME,
        usurioId: user?.USR_ID,
        ip: publicIpAddress,
        modulo: 'Reportes de crédito',
        hostname: document.location.host,
        tipoAccion: 'ERRORES Y EXCEPCIONES',
        idTipoAccion: 8,
        descripcion: descripcionError
      };

      auditoriaApi.InsertAuditoria(requestAuditoriaError);

      toast("Ocurrió un error al realizar la consulta. Inténtalo de nuevo.");

      toast('Error del sistema', { type: toast.TYPE.ERROR });

    } finally {
      setIsLoading(false);
    }
  };

  const exportToExcel = async () => {
    const columnDataList = data.map((row: any) => ({
      data: [
        String(row.fechA_SOLICITUD) || '',
        String(row.numerO_DOCUMENTO) || '',
        String(row.nombrE_COMPLETO) || '',
        String(row.estadoCredito) || '',
        String(row.departamentO_ID) || '',
        String(row.ciudaD_ID) || '',
        String(row.puntO_ATENCION) || '',
        String(row.tipO_CREDITO) || '',
        String(row.valoR_INMUBLE) || '',
        String(row.valoR_AVALUO) || '',
        String(row.valoR_PRESTAMO) || '',
        String(row.descripcionTarea) || '',
        String(row.correoAsesor) || '',
        String(row.nombreCompletoAsesor) || '',
      ]
    }));

    const request = {
      columnDataList: columnDataList.map(item => ({ data: item.data })),
      columnName: [
        'Fecha Solicitud', 'Documento', 'Nombre', 'Estado Crédito', 'Departamento', 'Ciudad', 'Punto Atención', 'Tipo Crédito', 'Valor Inmueble', 'Valor Avalúo', 'Valor Préstamo', 'Descripción Tarea', 'Correo Asesor', 'Nombre Completo Asesor'
      ]
    };

    const auditoriaApi = AuditoriaApi();
    const user = AuthService.getUserInfo();
    const nav = window.navigator as any;
    const usrAg = nav.userAgentData.brands.find(
      (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
    )?.brand;

    try {
      const response = await apiReportes.GenerarExcels(request);
      const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `Reportes_credito_${moment().format()}.xlsx`;
      document.body.appendChild(link);
      link.click();
      window.URL.revokeObjectURL(url);


      const requestAuditoria = {
        navegador: usrAg,
        sistemaOperativo: nav.userAgentData.platform,
        nombreUsuario: user.USR_USERNAME,
        usurioId: user.USR_ID,
        ip: publicIpAddress,
        modulo: 'Reportes Crédito',
        hostname: document.location.host,
        tipoAccion: 'DESCARGAS',
        idTipoAccion: 5,
        descripcion: 'Descarga de archivo Excel reportes crédito'

      } as Auditoria;

      auditoriaApi.InsertAuditoria(requestAuditoria);
    } catch (error: any) {
      const descripcionError = `Error en descarga de archivo}.
      - Error: ${error?.message || 'Error desconocido'}
      - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
      - Código HTTP: ${error?.response?.status || 'No disponible'}`;

      const requestAuditoriaError: Auditoria = {
        navegador: usrAg,
        sistemaOperativo: nav.userAgentData?.platform,
        nombreUsuario: user?.USR_USERNAME,
        usurioId: user?.USR_ID,
        ip: publicIpAddress,
        modulo: 'Reportes Crédito',
        hostname: document.location.host,
        tipoAccion: 'ERRORES Y EXCEPCIONES',
        idTipoAccion: 8,
        descripcion: descripcionError
      };

      auditoriaApi.InsertAuditoria(requestAuditoriaError);

      NotificationService.genericAlert(
        'Error',
        'No se pudo generar el archivo.',
        'error',
      );
    }


  };

  const handleChangePage = (event: unknown, newPage: number) => setPage(newPage);
  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };




  return (
    <div>
      <LoadingV2Component key="loader" loading={isLoading} tittle='Consultando...' />
      <PageIndicators topTextNivel1='Reportes Crédito' topTextNivel2='Reporte' />
      <IconTitle title='Reportes' subTitle='' icon='fch-Vigencia-GMF-1-2' />

      <MuiPickersUtilsProvider utils={DateFnsUtils}>
        <Paper style={{ padding: 24, marginTop: 20 }}>
          <Grid container spacing={3} alignItems="center">
            <Grid item xs={12} sm={4}>
              <TextField
                label="Número Documento"
                value={numeroDocumento}
                onChange={(e) => setNumeroDocumento(e.target.value.replace(/[^0-9]/g, ''))}
                fullWidth
                variant="standard"
                type="number"
                InputLabelProps={{ shrink: true }}
              />
            </Grid>

            <Grid item xs={12} sm={4}>
              <TextField
                label="Consecutivo Solicitud"
                value={consecutivoSolicitud}
                onChange={(e) => setConsecutivoSolicitud(e.target.value.replace(/[^0-9]/g, ''))}
                fullWidth
                variant="standard"
                type="number"
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item xs={12} sm={4}>
              <TextField label="Estado Tarea" select value={estadoTarea} onChange={(e) => setEstadoTarea(e.target.value)} fullWidth variant="standard" InputLabelProps={{ shrink: true }}>
                {listaEstadoTareas.map((option) => (
                  <MenuItem key={option.id} value={option.id}>
                    {option.descripcion}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} sm={4}>
              <TextField label="Tipos Tareas" select value={tipoTarea} onChange={(e) => setTipoTarea(e.target.value)} fullWidth variant="standard" InputLabelProps={{ shrink: true }}>
                {listaTipoTareas.map((option) => (
                  <MenuItem key={option.id} value={option.id}>
                    {option.descripcion}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>

            <Grid item xs={12} sm={4}>
              <CHDatePicker label="Fecha Inicio" format="yyyy-MM-dd" value={fechaInicio} onChange={setFechaInicio} fullWidth InputLabelProps={{ shrink: true }} />
            </Grid>
            <Grid item xs={12} sm={4}>
              <CHDatePicker label="Fecha Final" format="yyyy-MM-dd" value={fechaFinal} onChange={setFechaFinal} fullWidth InputLabelProps={{ shrink: true }} />
            </Grid>
            <Grid item xs={6} sm={2}>
              <Button variant="contained" color="primary" onClick={handleBuscar} fullWidth>Buscar</Button>
            </Grid>
            <Grid item xs={6} sm={2}>
              <Button variant="outlined" color="secondary" onClick={limpiarFiltros} fullWidth>Limpiar</Button>
            </Grid>

          </Grid>

          {data.length > 0 && (
            <>

              <TableContainer component={Paper} style={{ marginTop: '50px' }}>
                <Table>
                  <TableHead>
                    <TableRow >
                      <TableCell colSpan={columnasVisibles.length} align="right">
                        <Button
                          startIcon={<SaveAltIcon />}
                          variant="outlined"
                          color="secondary"
                          onClick={exportToExcel}
                          style={{ marginRight: 8 }}
                        >
                          Descargar Excel
                        </Button>

                        <IconButton onClick={handleOpenMenu} aria-label="Filtrar columnas">
                          <FilterListIcon />
                        </IconButton>

                        <Menu anchorEl={anchorEl} keepMounted open={Boolean(anchorEl)} onClose={handleCloseMenu}>
                          {columnasDisponibles.map((col) => (
                            <MenuItem key={col.id}>
                              <FormControlLabel
                                control={
                                  <Checkbox
                                    id={`chk-${col.id}`}
                                    checked={columnasVisibles.includes(col.id)}
                                    onChange={() => {
                                      setColumnasVisibles((prev) =>
                                        prev.includes(col.id)
                                          ? prev.filter((id) => id !== col.id)
                                          : [...prev, col.id]
                                      );
                                    }}
                                  />
                                }
                                label={col.label}
                              />
                            </MenuItem>
                          ))}
                        </Menu>
                      </TableCell>
                    </TableRow>

                    <TableRow className={classes.tableHeader}>
                      {columnasDisponibles
                        .filter((col) => columnasVisibles.includes(col.id))
                        .map((col) => (
                          <TableCell key={col.id}>{col.label}</TableCell>
                        ))}
                    </TableRow>
                  </TableHead>

                  <TableBody>
                    {data.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row, idx) => (
                      <TableRow key={idx}>
                        {columnasVisibles.includes('fechA_SOLICITUD') && <TableCell>{new Date(row.fechA_SOLICITUD).toLocaleDateString()}</TableCell>}
                        {columnasVisibles.includes('numerO_DOCUMENTO') && <TableCell>{row.numerO_DOCUMENTO}</TableCell>}
                        {columnasVisibles.includes('nombrE_COMPLETO') && <TableCell>{row.nombrE_COMPLETO}</TableCell>}
                        {columnasVisibles.includes('estadoCredito') && <TableCell>{row.estadoCredito}</TableCell>}
                        {columnasVisibles.includes('departamentO_ID') && <TableCell>{row.departamentO_ID}</TableCell>}
                        {columnasVisibles.includes('ciudaD_ID') && <TableCell>{row.ciudaD_ID}</TableCell>}
                        {columnasVisibles.includes('puntO_ATENCION') && <TableCell>{row.puntO_ATENCION}</TableCell>}
                        {columnasVisibles.includes('tipO_CREDITO') && <TableCell>{row.tipO_CREDITO}</TableCell>}
                        {columnasVisibles.includes('valoR_INMUBLE') && <TableCell>{UtilService.formatCurrency(row.valoR_INMUBLE)}</TableCell>}
                        {columnasVisibles.includes('valoR_AVALUO') && <TableCell>{UtilService.formatCurrency(row.valoR_AVALUO)}</TableCell>}
                        {columnasVisibles.includes('valoR_PRESTAMO') && <TableCell>{UtilService.formatCurrency(row.valoR_PRESTAMO)}</TableCell>}
                        {columnasVisibles.includes('descripcionTarea') && <TableCell>{row.descripcionTarea}</TableCell>}
                        {columnasVisibles.includes('correoAsesor') && <TableCell>{row.correoAsesor}</TableCell>}
                        {columnasVisibles.includes('nombreCompletoAsesor') && <TableCell>{row.nombreCompletoAsesor}</TableCell>}
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
                <TablePagination
                  component="div"
                  count={data.length}
                  page={page}
                  onPageChange={handleChangePage}
                  rowsPerPage={rowsPerPage}
                  onRowsPerPageChange={handleChangeRowsPerPage}
                  labelRowsPerPage="Filas por página"
                />
              </TableContainer>
            </>
          )}
        </Paper>
      </MuiPickersUtilsProvider>
    </div>
  );
};

export default ReporteCredito;