import React, { useEffect, useMemo, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { toast } from 'react-toastify';
import moment from 'moment';
import Search from '@material-ui/icons/Search';
import { Paper, Grid, List, ListItem, ListItemText, ListItemAvatar, Avatar, Typography, TextField, Button, Tabs, Tab, Divider, Tooltip, TablePagination, TablePaginationProps, Box } from '@material-ui/core';
import { Email } from '@material-ui/icons';
import { format } from 'date-fns';
import MailOutlineIcon from '@material-ui/icons/MailOutline';
import DraftsIcon from '@material-ui/icons/Drafts';
import { useHistory } from 'react-router-dom';
import asigTareas from '../../services/api-asignacionTareas.service';
import AuthService from '../../services/auth.service';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    height: '100vh',
    backgroundColor: '#f9f9f9',
    padding: theme.spacing(2),
    alignItems: 'center',
  },
  header: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    padding: theme.spacing(1),
    backgroundColor: '#1c3f8b',
    color: '#fff',
    borderRadius: 8,
    marginBottom: theme.spacing(1),
    textAlign: 'center',
    width: '100%',
  },
  headerIcon: {
    marginRight: theme.spacing(1),
    fontSize: 30,
  },
  searchContainer: {
    display: 'flex',
    alignItems: 'center',
    width: '100%',
    margin: '0 auto',
    padding: theme.spacing(1),
    borderRadius: 8,
    backgroundColor: '#fff',
    boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
    marginBottom: theme.spacing(2),
  },
  searchIcon: {
    marginRight: theme.spacing(1),
    color: '#3f51b5',
  },
  searchInput: {
    flex: 1,
    border: 'none',
    outline: 'none',
    fontSize: '1rem',
  },
  content: {
    display: 'flex',
    flex: 1,
    [theme.breakpoints.down('sm')]: {
      flexDirection: 'column',
    },
    width: '100%',
    margin: '0 auto',
    '& > div': {
      marginBottom: theme.spacing(2),
    },
  },
  inboxContainer: {
    padding: theme.spacing(2),
    backgroundColor: '#fff',
    boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
    borderRadius: 8,
    maxHeight: '70vh',
    overflow: 'auto',
  },
  closeButton: {
    marginTop: theme.spacing(2),
    backgroundColor: '#1c3f8b',
    color: 'white',
  },
  column: {
    textAlign: 'center',
    width: '100%',
  },
}));

interface TabPanelProps {
  children: React.ReactNode;
  value: number;
  index: number;
}

const TabPanel: React.FC<TabPanelProps> = ({ children, value, index }) => (
  <div role="tabpanel" hidden={value !== index}>
    {value === index && <Typography>{children}</Typography>}
  </div>
);

const HomePage = () => {

  const [value, setValue] = useState(0);

  const handleChange = (event: any, newValue: any) => {
    setValue(newValue);
  };
  const apiAsignarTarea = asigTareas();

  const classes = useStyles();
  const [searchTerm, setSearchTerm] = useState('');
  const [tareasAsignadas, setTareasAsignadas] = useState<any[]>([]);
  const [loading, setLoading] = useState(false)
  const history = useHistory();
  const user = AuthService.getUserInfo();

  // console.log('userTareas', user);
  // Paginación Sin leer
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  // Paginación Leídos
  const [pageLeidos, setPageLeidos] = useState(0);
  const [rowsPerPageLeidos, setRowsPerPageLeidos] = useState(10);

  // Paginación Sin leer
  const handleChangePage = (event: any, newPage: any) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: any) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  // Paginación Leídos
  const handleChangePageLeidos = (event: any, newPage: any) => {
    setPageLeidos(newPage);
  };

  const handleChangeRowsPerPageLeidos = (event: any) => {
    setRowsPerPageLeidos(parseInt(event.target.value, 10));
    setPageLeidos(0);
  };

  const labelDisplayedRows: TablePaginationProps['labelDisplayedRows'] = ({ from, to, count }) => `${from}-${to} de ${count}`;

  useEffect(() => {
    obtenerTareasTotales();
  }, []);

  const obtenerTareasTotales = async () => {
    setLoading(true);
    try {
      const request = {
        idUsuario: user.USR_ID,
      };

      const responseTareas = await apiAsignarTarea.getObtenerAsignacionTareas(request);
      let tareas = [];
      if (responseTareas && responseTareas.isSuccess) {
        tareas = responseTareas?.data.map((objeto: any) => ({
          ...objeto,
          nombreUsuario: user.USR_USERNAME,
          estado_Tarea: objeto.estado_Tarea ?? objeto.estado,
        }));
      }


      if (user.USR_ID === '16ee93d8-d06d-40d9-bf5f-500656929ec5') {
        const responseNomina = await apiAsignarTarea.getConsultaNotificacionesCargueNomina(user?.USR_ID);

        if (responseNomina && responseNomina.isSuccess) {
          const tareasNomina = responseNomina?.data.map((item: any) => ({
            ...item,
            nombreUsuario: user.USR_USERNAME,
            estado_Tarea: item.estado,
            descripcion: item.nrN_DESCRIPCION,
            idUsuario: item.usR_USUARIO,
            modulo: item.nrN_MODULO,
            radicado: null,
            usR_ENVIA: 'Unidad Ejecutora',
            tipoModalidad: 'Cargue nómima UEJ',
            tipoSolicitud: 'Cargue nómima UEJ',
            tipoSubModalidad: 'Cargue nómima UEJ',
            trA_NOMBRE: 'Cargue Nómina',
            asT_FECHA_REGISTRO: item.nrN_FECHA_REGISTRO,
          }));

          tareas = [...tareas, ...tareasNomina];
        }
      }
      tareas.sort((a: any, b: any) => new Date(b.asT_FECHA_REGISTRO).getTime() - new Date(a.asT_FECHA_REGISTRO).getTime());
      setTareasAsignadas(tareas);
    } catch (error) {
      toast('Error del sistema', { type: toast.TYPE.ERROR });
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value);
  };

  const irTarea = (tarea: any) => {

    if (tarea.modulo === "Solicitud" && tarea.radicado !== null) {
      history.push(`/GestionSolicitudes/${tarea.radicado}`);
    }
    else if (tarea.modulo === 'Cargue Nomina') {
      history.push({
        pathname: `/CargueNomina`,
        state: { tarea }
      });
    }
    else if (tarea.radicado === null && tarea.modulo === "Solicitud") {
      toast('No existe número de radicado para esta tarea', { type: toast.TYPE.WARNING })
    }
    else if (tarea.link === "/verificacionDerechos" || tarea.link === "/AbonoIntereses") {
      history.push(tarea.link);
    }

    else if (tarea.radicado !== null && tarea.modulo === "Fondo De Solidaridad") {
      let radicado = '';
      if (tarea.radicado === 'N/A') {
        radicado = 'N';
        history.push(`/Postulacion/${radicado}/${tarea.identificacionAfiliado}`);
      }
      if (tarea.radicado !== 'N/A' && tarea.radicado !== null) {
        radicado = 'S';
        history.push(`/Postulacion/${radicado}/${tarea.radicado}`);
      }
    }
    else if (tarea.radicado === null && tarea.modulo === "Fondo De Solidaridad") {
      toast('No existe número de radicado para esta tarea', { type: toast.TYPE.WARNING })
    }
    else if (tarea.radicado !== null && tarea.radicado?.startsWith("13")) {
      if (tarea.modulo === 'Levantamiento o Desembargo ') {
        history.push(`/DesembargoInicio/${tarea.radicado}`);

      }
      else if (tarea.modulo === 'Pago bloqueo y/o embargo') {
        history.push(`/PagoEmbargosInicio/${tarea.radicado}`);

      }
      else if (tarea.modulo === 'Embargo' || tarea.modulo === 'EMBARGO') {
        history.push(`/EmbargosInicio/${tarea.radicado}`);
      }
    }
  };

  const calcularFechaVencimiento = (fechaInicio: any, diasHabiles: any) => {
    const fecha = moment(fechaInicio);
    let diasRestantes = diasHabiles;

    while (diasRestantes > 0) {
      fecha.add(1, 'days');
      if (fecha.day() >= 1 && fecha.day() <= 5) {
        diasRestantes -= 1;
      }
    }

    return fecha.format('YYYY-MM-DD');
  }

  const safeLowerIncludes = (value: any, term: string) => {
    if (!value) return false;
    return value.toString().toLowerCase().includes(term);
  };

  const normalizarEstado = (estado?: string) =>
    String(estado ?? '')
      .trim()
      .toUpperCase();

  const esTareaSinLeer = (message: any) => {
    const estado = normalizarEstado(message?.estado_Tarea);

    return [
      'PENDIENTE',
      'INCONSISTENTE',
      'BLOQUEADO',
      'EN TRÁMITE',
      'EN VERIFICACION',
      'PENDIENTE POR PROCESAR',
      'DISPONIBLE',
      'PENDIENTE POR APROBAR',
      'CORREGIDO',
    ].includes(estado);
  };

  const esTareaLeida = (message: any) => {
    const estado = normalizarEstado(message?.estado_Tarea);

    return [
      'APROBADO',
      'APROBADA',
      'RECHAZADO',
      'RECHAZADA',
    ].includes(estado);
  };

  const filter = useMemo(() => {
    const term = searchTerm?.toLowerCase();

    return tareasAsignadas?.filter((tarea: any) =>
      safeLowerIncludes(tarea?.nombreUsuario, term)
      || safeLowerIncludes(tarea?.estado, term)
      || safeLowerIncludes(tarea?.idUsuario, term)
      || safeLowerIncludes(tarea?.modulo === null ? 'SinModulo' : tarea?.modulo, term)
      || safeLowerIncludes(tarea?.radicado === null ? '' : tarea?.radicado, term)
      || safeLowerIncludes(tarea?.asT_FECHA_REGISTRO === null ? '' : moment(tarea?.asT_FECHA_REGISTRO).format('DD/MM/YYYY'), term)
      || safeLowerIncludes(tarea?.asT_FECHA_REGISTRO === null ? '' : moment(tarea?.asT_FECHA_REGISTRO).format('DD-MM-YYYY'), term)
      || safeLowerIncludes(tarea?.asT_FECHA_REGISTRO === null ? '' : moment(tarea?.asT_FECHA_REGISTRO).format('DDMMYYYY'), term)
      || safeLowerIncludes(tarea?.usR_ENVIA === null ? '' : tarea?.usR_ENVIA, term)
      || safeLowerIncludes(tarea?.estado_Tarea, term)
      || safeLowerIncludes(tarea?.tipoModalidad, term)
      || safeLowerIncludes(tarea?.tipoSolicitud, term)
      || safeLowerIncludes(tarea?.tipoSubModalidad, term)
      || safeLowerIncludes(tarea?.trA_NOMBRE, term)
      || safeLowerIncludes(tarea?.identificacionAfiliado === null ? 'No existe identificación' : tarea?.identificacionAfiliado, term)
    );
  }, [searchTerm, tareasAsignadas]);

  const mostrarDescripcion = (descripcion: string, estado: string) => {

    let texto;
    if (descripcion) {
      const terminaConEstado = descripcion.endsWith('se encuentra en estado');
      if (terminaConEstado) {
        texto = `${descripcion} ${estado}`;
      } else {
        texto = descripcion;
      }
    }
    return texto;

  }

  return (
    <>
      <div>
        <LoadingV2Component key="loader" loading={loading} tittle='Consultando tareas asignadas' />
        <div style={{ display: 'flex', justifyContent: 'center' }}>
          <Tabs value={value}
            onChange={handleChange}
            TabIndicatorProps={{
              style: {
                backgroundColor: '#ffd800',
                height: '4px',
              },
            }}
          >
            <Tab
              label="Sin leer"
              style={{
                borderBottom: value === 0 ? '2px solid yellow' : 'none',
                fontSize: '19px',
                fontFamily: 'Tahoma',
                color: '#525252'
              }}
            />
            <Tab
              label="Leídos"
              style={{
                borderBottom: value === 1 ? '2px solid yellow' : 'none',
                fontSize: '19px',
                fontFamily: 'Tahoma',
                color: '#525252'
              }}
            />
          </Tabs>
        </div>

        <br />
        <TabPanel value={value} index={0}>
          <div className={classes.root}>
            <Paper className={classes.header} elevation={3}>
              <MailOutlineIcon className={classes.headerIcon} />
              <Typography variant="h5" style={{ fontFamily: 'Tahoma' }}>Tareas Asignadas</Typography>
            </Paper>
            <div className={classes.searchContainer}>
              <TextField
                className={classes.searchInput}
                placeholder="Buscar tareas"
                value={searchTerm}
                onChange={handleSearch}
                variant="outlined"
                fullWidth
                InputProps={{
                  startAdornment: <Search className={classes.searchIcon} />,
                }}
              />
            </div>
            <div className={classes.content}>
              <Grid container>
                <Grid className={classes.column} item xs={12} md={12}>
                  {filter.length !== 0 && (
                    <Paper elevation={3} className={classes.inboxContainer}>
                      <TablePagination
                        rowsPerPageOptions={[10, 20, 50]}
                        component="div"
                        count={filter
                          .filter(message => value === 0 ?
                            message.estado_Tarea === 'PENDIENTE'
                            || message.estado_Tarea === 'INCONSISTENTE'
                            || message.estado_Tarea === 'BLOQUEADO'
                            || message.estado_Tarea === 'EN TRÁMITE'
                            || message.estado_Tarea === 'EN VERIFICACION'
                            || message.estado_Tarea === 'Pendiente por procesar'
                            || message.estado_Tarea === 'Disponible'
                            || message.estado_Tarea === 'Pendiente por aprobar'
                            || message.estado_Tarea === 'CORREGIDO'
                            : '').length}
                        rowsPerPage={rowsPerPage}
                        page={page}
                        onPageChange={handleChangePage}
                        onRowsPerPageChange={handleChangeRowsPerPage}
                        labelRowsPerPage="Filas por página:"
                        labelDisplayedRows={labelDisplayedRows}
                      />
                      <List>
                        {filter
                          .filter(message => value === 0 ?
                            message.estado_Tarea === 'PENDIENTE'
                            || message.estado_Tarea === 'INCONSISTENTE'
                            || message.estado_Tarea === 'BLOQUEADO'
                            || message.estado_Tarea === 'EN TRÁMITE'
                            || message.estado_Tarea === 'EN VERIFICACION'
                            || message.estado_Tarea === 'Pendiente por procesar'
                            || message.estado_Tarea === 'Disponible'
                            || message.estado_Tarea === 'Pendiente por aprobar'
                            || message.estado_Tarea === 'CORREGIDO'
                            : '')
                          .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                          .map((message) => {
                            const formattedDate = format(new Date(message.asT_FECHA_REGISTRO), 'dd MMM yyyy');
                            const formattedTime = format(new Date(message.asT_FECHA_REGISTRO), 'HH:mm:ss');
                            const fechaVencimiento = calcularFechaVencimiento(message.asT_FECHA_REGISTRO, 3);
                            const fechaActual = moment();
                            let color = '';
                            let mensaje = '';
                            if (fechaActual.isBefore(fechaVencimiento, 'day')) {
                              color = 'green';
                              mensaje = ` Te encuentras al dia con tu asignación ${fechaVencimiento}`

                            } else if (fechaActual.isSameOrBefore(fechaVencimiento, 'day')) {
                              color = 'yellow';
                              mensaje = ` Tienes menos de 3 días para completar tu asignación ${fechaVencimiento}`

                            } else {
                              color = 'red';
                              mensaje = ` Tienes menos de 2 días para completar tu asignación ${fechaVencimiento}`
                            }
                            const primerNombre = message?.usR_ENVIA?.split('.') || [];
                            const primerApellid = (primerNombre[1]?.split('@')) || [];

                            return (
                              <ListItem key={message.id}>
                                <Tooltip title={mensaje} arrow>
                                  <ListItemAvatar>
                                    <Avatar>
                                      <Email style={{ color: color }} />
                                    </Avatar>
                                  </ListItemAvatar>
                                </Tooltip>
                                <ListItemText
                                  primary={
                                    <>
                                      {message?.usR_ENVIA && (
                                        <>
                                          <Divider
                                            style={{
                                              height: 0,
                                              border: 'none',
                                              borderTop: '2px solid #808080',
                                            }}
                                          />
                                          <Typography variant="body2" color="textSecondary" style={{ textAlign: 'left', marginTop: '5px', marginBottom: '5px' }}>
                                            <strong> Enviado por: </strong> {primerNombre[0]?.toUpperCase() || ''} {primerApellid[0]?.toUpperCase() || ''}
                                          </Typography>
                                          <Divider />
                                        </>
                                      )}

                                    </>
                                  }
                                  secondary={
                                    <Grid container spacing={1}>

                                      <Grid item xs={4}>
                                        <strong>Módulo:</strong> {message.modulo.toUpperCase()} <br />
                                        <strong>Tarea:</strong> {message.trA_NOMBRE?.toUpperCase()} <br />
                                        <strong>Tipo Modalidad:</strong> {message.tipoModalidad} <br />
                                        <strong>Tipo Solicitud:</strong> {message.tipoSolicitud} <br />
                                        <strong>Tipo SubModalidad:</strong> {message.tipoSubModalidad}
                                      </Grid>

                                      <Grid item xs={4}>
                                        <strong>Estado:</strong> {message.estado_Tarea.toUpperCase()} <br />
                                        <strong>No. Radicado:</strong> {message.radicado} <br />
                                        {(message.modulo !== 'Verificación de Derechos' &&
                                          message.modulo !== 'Abono de intereses') && (
                                            <>
                                              <strong>Identificación:</strong> {message.identificacionAfiliado} <br />
                                            </>
                                          )}
                                        <strong>Fecha:</strong> {formattedDate} <br />
                                        <strong>Hora:</strong> {formattedTime}
                                      </Grid>

                                      <Grid item xs={4}>
                                        <strong>Descripción:</strong>
                                        <div style={{
                                          marginTop: 4,
                                          textAlign: 'justify',
                                          whiteSpace: 'pre-line',
                                        }}>
                                          {mostrarDescripcion(message.descripcion, message.estado_Tarea.toUpperCase()) || 'N/A'}
                                          {/* {message.descripcion} */}
                                        </div>
                                      </Grid>

                                      <Grid item xs={12}>
                                        <Button
                                          onClick={() => irTarea(message)}
                                          className={classes.closeButton}
                                          style={{ marginTop: '8px' }}
                                        >
                                          Ir a la tarea
                                        </Button>
                                      </Grid>

                                    </Grid>
                                  }
                                />
                              </ListItem>
                            );
                          })}
                      </List>
                    </Paper>
                  )}
                </Grid>
              </Grid>
            </div>
          </div>

        </TabPanel>
        <TabPanel value={value} index={1}>
          <div className={classes.root}>
            <Paper className={classes.header} elevation={3}>
              <DraftsIcon className={classes.headerIcon} />
              <Typography variant="h5" style={{ fontFamily: 'Tahoma' }}>Tareas Finalizadas</Typography>
            </Paper>
            <div className={classes.searchContainer}>
              <TextField
                className={classes.searchInput}
                placeholder="Buscar tareas finalizadas"
                value={searchTerm}
                onChange={handleSearch}
                variant="outlined"
                fullWidth
                InputProps={{
                  startAdornment: <Search className={classes.searchIcon} />,
                }}
              />
            </div>
            <div className={classes.content}>
              <Grid container spacing={2}>
                <Grid item xs={12} sm={12}>
                  {filter.length !== 0 && (
                    <Paper elevation={3} className={classes.inboxContainer}>
                      <TablePagination
                        rowsPerPageOptions={[10, 20, 50]}
                        component="div"
                        count={filter
                          .filter(message => value === 1 ?
                            message?.estado_Tarea === 'APROBADO' || message?.estado_Tarea === 'APROBADA' || message?.estado_Tarea === 'Aprobado' || message?.estado_Tarea === 'RECHAZADO' || message?.estado_Tarea === 'RECHAZADA' :
                            '').length}
                        rowsPerPage={rowsPerPageLeidos}
                        page={pageLeidos}
                        onPageChange={handleChangePageLeidos}
                        onRowsPerPageChange={handleChangeRowsPerPageLeidos}
                        labelRowsPerPage="Filas por página:"
                        labelDisplayedRows={labelDisplayedRows}
                      />
                      <List>
                        {filter
                          .filter(message => value === 1 ?
                            message?.estado_Tarea === 'APROBADO' || message?.estado_Tarea === 'APROBADA' || message?.estado_Tarea === 'Aprobado' || message?.estado_Tarea === 'RECHAZADO' || message?.estado_Tarea === 'RECHAZADA' :
                            '')
                          .slice(pageLeidos * rowsPerPageLeidos, pageLeidos * rowsPerPageLeidos + rowsPerPageLeidos)
                          .map((message) => {
                            const formattedDate = format(new Date(message.asT_FECHA_REGISTRO), 'dd MMM yyyy');
                            const formattedTime = format(new Date(message.asT_FECHA_REGISTRO), 'HH:mm:ss');
                            const primerNombre: any = message?.usR_ENVIA.split('.')
                            const primerApellid: any = primerNombre[1]?.split('@') === undefined ? '' : primerNombre[1]?.split('@')

                            return (
                              <ListItem key={message.id}>
                                <ListItemAvatar>
                                  <Avatar>
                                    <Email style={{ color: '#3f51b5' }} />
                                  </Avatar>
                                </ListItemAvatar>
                                <ListItemText
                                  primary={
                                    <>
                                      {message?.usR_ENVIA && (
                                        <>
                                          <Divider
                                            style={{
                                              height: 0,
                                              border: 'none',
                                              borderTop: '2px solid #808080',
                                            }}
                                          />
                                          <Typography variant="body2" color="textSecondary" style={{ textAlign: 'left' }}>
                                            <strong> Enviado por: </strong> {primerNombre[0]?.toUpperCase()} {primerApellid[0]?.toUpperCase()}
                                          </Typography>
                                          <Divider />
                                        </>
                                      )}

                                    </>
                                  }
                                  secondary={
                                    <Grid container spacing={1}>
                                      <Grid item xs={6}>
                                        {
                                          message.modulo.toUpperCase() === 'CARGUE NOMINA'
                                            ? <strong>Módulo:</strong>
                                            : <strong>Módulo:</strong>
                                        }
                                        {' '}
                                        {message.modulo.toUpperCase() === 'CARGUE NOMINA'
                                          ? 'CARGUE NÓMINA'
                                          : message.modulo.toUpperCase()
                                        }
                                        <br />

                                        <strong>Tarea:</strong> {message?.trA_NOMBRE?.toUpperCase()} <br />
                                        <strong>Tipo Modalidad:</strong> {message.tipoModalidad} <br />
                                        <strong>Tipo Solicitud:</strong> {message.tipoSolicitud} <br />
                                        <strong>Tipo SubModalidad:</strong> {message.tipoSubModalidad}
                                      </Grid>

                                      <Grid item xs={6}>
                                        <strong>Estado:</strong> {message.estado_Tarea.toUpperCase()} <br />
                                        <strong>No. Radicado: </strong>
                                        <span
                                          style={{ color: 'blue', textDecoration: 'underline', cursor: 'pointer' }}
                                          onClick={() => irTarea(message)}
                                        >
                                          {message.radicado}
                                        </span>
                                        <br />
                                        <strong>Fecha:</strong> {formattedDate} <br />
                                        <strong>Hora:</strong> {formattedTime}
                                      </Grid>
                                    </Grid>
                                  }
                                />
                              </ListItem>
                            );
                          })}
                      </List>
                    </Paper>
                  )}
                </Grid>
              </Grid>
            </div>
          </div>
        </TabPanel>
      </div>
    </>
  );
};

export default HomePage;