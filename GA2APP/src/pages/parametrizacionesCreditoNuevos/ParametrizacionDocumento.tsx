import React, {
  useEffect,
  useState,
} from 'react';

import {
  Box,
  Button,
  Checkbox,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
  IconButton,
  MenuItem,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  TextField,
  Tooltip,
  Typography,
  makeStyles,
} from '@material-ui/core';

import Autocomplete from '@material-ui/lab/Autocomplete';

import SearchIcon from '@material-ui/icons/Search';
import AddIcon from '@material-ui/icons/Add';
import EditIcon from '@material-ui/icons/Edit';
import DeleteIcon from '@material-ui/icons/Delete';

import swal from 'sweetalert';

import CreditApi from '../../services/api-credit.service';

interface TipoActividad {
  id: number;
  descripcion: string;
  estado: number;
  creadoPor?: string;
  fechaCreacion?: string;
  modificadoPor?: string;
  fechaModificacion?: string;
}

interface DocumentoActividad {
  tdC_ID: string;
  iD_ACTIVIDAD_FLUJO: number;
  actividad: string;
  tipO_DOCUMENTO: string;
  obligatorio: string;
  carga: string;
  visualiza: string;
  visualizacliente?: string;
  docpreaprobado?: string;
  orden: number;
  rutA_IMAGEN: string | null;
  dcS_APROBACION: number;
}

interface TipoDocumento {
  tdC_ID: number;
  tdC_DESCRIPCION: string;
  tdC_ESTADO: number;
  tdC_CREADO_POR?: string;
  tdC_FECHA_CREACION?: string;
  tdC_MODIFICADO_POR?: string;
  tdC_FECHA_MODIFICACION?: string;
  tdC_COD_TABLA_RETENCION?: number;
  tdC_NOMBRE_EQUIVALENTE?: string;
  tdC_COD_DOCUMENTO?: number;
}

interface PayloadGuardarDocumento {
  taC_ID: number;
  tdC_ID: number;
  dcA_OBLIGATORIO: string;
  dcA_CARGA: string;
  dcA_VISUALIZA: string;
}

interface PayloadEliminarDocumento {
  taC_ID: number;
  tdC_ID: number;
}

interface ErrorServicio {
  message?: string;

  response?: {
      data?: {
          returnMessage?: string;
          message?: string;
          title?: string;
      };
  };
}

const useStyles = makeStyles(() => ({
  root: {
      padding: '24px 32px',
      backgroundColor: '#f3f5f9',
      minHeight: '100vh',
  },

  mainPaper: {
      padding: 16,
      borderRadius: 12,
      backgroundColor: '#ffffff',
      border: '1px solid #ddd',
      boxShadow: '0 2px 6px rgba(0,0,0,0.18)',
  },

  header: {
      marginBottom: 18,
      paddingBottom: 12,
  },

  title: {
      display: 'inline-block',
      fontWeight: 700,
      fontSize: 16,
      color: '#ffffff',
      backgroundColor: '#2f5597',
      padding: '3px 6px',
  },

  subtitle: {
      marginTop: 8,
      borderBottom: '1px solid #9e9e9e',
      paddingBottom: 8,
  },

  addButton: {
      backgroundColor: '#244b96',
      color: '#ffffff',
      fontWeight: 500,
      minWidth: 160,
      height: 38,
      borderRadius: 3,

      '&:hover': {
          backgroundColor: '#1d3f7f',
      },

      '&:disabled': {
          backgroundColor: '#bdbdbd',
          color: '#ffffff',
      },
  },

  tablePaper: {
      padding: 14,
      borderRadius: 12,
      backgroundColor: '#ffffff',
      boxShadow: '0 1px 6px rgba(0,0,0,0.12)',
  },

  filtrosContainer: {
      marginBottom: 18,
  },

  selectActividad: {
      backgroundColor: '#ffffff',

      '& .MuiOutlinedInput-root': {
          borderRadius: 5,
      },
  },

  searchButton: {
      height: 40,
      backgroundColor: '#244b96',
      color: '#ffffff',
      fontWeight: 600,
      minWidth: 130,

      '&:hover': {
          backgroundColor: '#1d3f7f',
      },

      '&:disabled': {
          backgroundColor: '#bdbdbd',
          color: '#ffffff',
      },
  },

  tableContainer: {
      borderRadius: 6,
      overflowX: 'auto',
      border: '1px solid #ffffff',
  },

  table: {
      minWidth: 850,
  },

  tableHead: {
      backgroundColor: '#ffc914',
  },

  headCell: {
      fontWeight: 600,
      textAlign: 'center',
      color: '#34495e',
      fontSize: 12,
      padding: '14px 10px',
      borderRight: '1px solid #ffffff',
      whiteSpace: 'nowrap',
  },

  bodyCell: {
      textAlign: 'center',
      fontSize: 12,
      color: '#34495e',
      padding: '12px 10px',
      borderRight: '1px solid #ffffff',
      borderBottom: '1px solid #ffffff',
  },

  documentCell: {
      textAlign: 'left',
      fontSize: 12,
      color: '#34495e',
      padding: '12px 10px',
      borderRight: '1px solid #ffffff',
      borderBottom: '1px solid #ffffff',
  },

  activityCell: {
      textAlign: 'left',
      fontSize: 12,
      color: '#34495e',
      padding: '12px 10px',
      borderRight: '1px solid #ffffff',
      borderBottom: '1px solid #ffffff',
  },

  rowGray: {
      backgroundColor: '#bfbfbf',
  },

  rowLight: {
      backgroundColor: '#f1f2f5',
  },

  optionYes: {
      color: '#1fae4b',
      fontWeight: 700,
  },

  optionNo: {
      color: '#d32f2f',
      fontWeight: 700,
  },

  actions: {
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      gap: 4,
  },

  editButton: {
      color: '#244b96',
  },

  deleteButton: {
      color: '#d32f2f',
  },

  loadingBox: {
      padding: 30,
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
  },

  emptyBox: {
      padding: 30,
      textAlign: 'center',
  },

  dialogPaper: {
      borderRadius: 12,
  },

  dialogTitleBox: {
      backgroundColor: '#244b96',
      color: '#ffffff',
      padding: '12px 20px',
  },

  dialogTitle: {
      fontWeight: 700,
      textAlign: 'center',
  },

  dialogContent: {
      paddingTop: 20,
  },

  checksContainer: {
      marginTop: 18,
      padding: 12,
      borderRadius: 8,
      border: '1px solid #e0e0e0',
      backgroundColor: '#f8f9fb',
  },

  checkBoxItem: {
      display: 'flex',
      alignItems: 'center',
  },

  cancelButton: {
      color: '#555555',
      fontWeight: 600,
  },

  saveButton: {
      backgroundColor: '#244b96',
      color: '#ffffff',
      fontWeight: 600,

      '&:hover': {
          backgroundColor: '#1d3f7f',
      },

      '&:disabled': {
          backgroundColor: '#bdbdbd',
          color: '#ffffff',
      },
  },
}));

const apiCredito = CreditApi();

function ParametrizacionDocumento() {
  const classes = useStyles();

  const [
      tiposActividad,
      setTiposActividad,
  ] = useState<TipoActividad[]>([]);

  const [
      tipoActividadSeleccionado,
      setTipoActividadSeleccionado,
  ] = useState('');

  const [
      cargandoActividades,
      setCargandoActividades,
  ] = useState(false);

  const [
      documentos,
      setDocumentos,
  ] = useState<DocumentoActividad[]>([]);

  const [
      buscandoDocumentos,
      setBuscandoDocumentos,
  ] = useState(false);

  const [
      busquedaRealizada,
      setBusquedaRealizada,
  ] = useState(false);

  const [
      openAgregar,
      setOpenAgregar,
  ] = useState(false);

  const [
      documentoEditando,
      setDocumentoEditando,
  ] = useState<DocumentoActividad | null>(
      null,
  );

  const [
      actividadAgregar,
      setActividadAgregar,
  ] = useState('');

  const [
      tiposDocumento,
      setTiposDocumento,
  ] = useState<TipoDocumento[]>([]);

  const [
      documentosDisponibles,
      setDocumentosDisponibles,
  ] = useState<TipoDocumento[]>([]);

  const [
      documentoSeleccionado,
      setDocumentoSeleccionado,
  ] = useState<TipoDocumento | null>(
      null,
  );

  const [
      obligatorio,
      setObligatorio,
  ] = useState(false);

  const [
      carga,
      setCarga,
  ] = useState(false);

  const [
      visualiza,
      setVisualiza,
  ] = useState(false);

  const [
      cargandoTiposDocumento,
      setCargandoTiposDocumento,
  ] = useState(false);

  const [
      guardandoDocumento,
      setGuardandoDocumento,
  ] = useState(false);

  const [
      eliminandoDocumento,
      setEliminandoDocumento,
  ] = useState(false);

  const mostrarAlerta = async (
    title: string,
    text: string,
    icon: 'success' | 'error' | 'warning' | 'info',
) => {
    await swal({
        title,
        text,
        icon,
        buttons: {
            confirm: {
                text: 'Aceptar',
                value: true,
                visible: true,
                closeModal: true,
            },
        },
        closeOnClickOutside: false,
        closeOnEsc: false,
    });
};

  const obtenerTituloModal = () => {
      if (documentoEditando) {
          return 'Actualizar documento';
      }

      return 'Agregar documento';
  };

  const obtenerTextoBotonGuardar = () => {
      if (guardandoDocumento) {
          if (documentoEditando) {
              return 'Actualizando...';
          }

          return 'Guardando...';
      }

      if (documentoEditando) {
          return 'Actualizar';
      }

      return 'Guardar';
  };

  const obtenerHelperDocumento = () => {
      if (documentoEditando) {
          return 'El documento no puede cambiarse durante la actualización';
      }

      if (!actividadAgregar) {
          return 'Primero seleccione un tipo de actividad';
      }

      return 'Escriba el nombre del documento para buscar';
  };

  const obtenerTituloConfirmacion = () => {
      if (documentoEditando) {
          return '¿Estás seguro de actualizar este documento?';
      }

      return '¿Estás seguro de agregar este documento?';
  };

  const obtenerTituloExito = (
      esEdicion: boolean,
  ) => {
      if (esEdicion) {
          return 'Documento actualizado';
      }

      return 'Documento agregado';
  };

  const obtenerMensajeExito = (
      esEdicion: boolean,
  ) => {
      if (esEdicion) {
          return 'El documento fue actualizado correctamente.';
      }

      return 'El documento fue agregado correctamente.';
  };

  const obtenerMensajeErrorGuardar = (
      esEdicion: boolean,
  ) => {
      if (esEdicion) {
          return 'No fue posible actualizar el documento.';
      }

      return 'No fue posible guardar el documento.';
  };

  const obtenerMensajeErrorServicio = (
      error: unknown,
      mensajePredeterminado: string,
  ) => {
      const errorServicio =
          error as ErrorServicio;

      if (
          errorServicio.response &&
          errorServicio.response.data
      ) {
          const dataError =
              errorServicio.response.data;

          if (dataError.returnMessage) {
              return dataError.returnMessage;
          }

          if (dataError.message) {
              return dataError.message;
          }

          if (dataError.title) {
              return dataError.title;
          }
      }

      if (errorServicio.message) {
          return errorServicio.message;
      }

      return mensajePredeterminado;
  };

  const obtenerTiposActividad =
      async () => {
          try {
              setCargandoActividades(
                  true,
              );

              const response =
                  await apiCredito
                      .getObtenertipoactividadporid(
                          '',
                      );

              if (
                  response &&
                  response.isSuccess &&
                  Array.isArray(
                      response.data,
                  )
              ) {
                  setTiposActividad(
                      response.data,
                  );

                  return;
              }

              setTiposActividad([]);
          } catch (error) {
              setTiposActividad([]);

              await mostrarAlerta(
                  'Error',
                  'No fue posible consultar los tipos de actividad.',
                  'error',
              );
          } finally {
              setCargandoActividades(
                  false,
              );
          }
      };

  const obtenerTiposDocumento =
      async (): Promise<TipoDocumento[]> => {
          try {
              setCargandoTiposDocumento(
                  true,
              );

              const response =
                  await apiCredito
                      .GetObtenerTiposDocumento();

              if (
                  response &&
                  response.isSuccess &&
                  Array.isArray(
                      response.data,
                  )
              ) {
                  const activos =
                      response.data.filter(
                          (
                              item: TipoDocumento,
                          ) =>
                              Number(
                                  item.tdC_ESTADO,
                              ) === 1,
                      );

                  setTiposDocumento(
                      activos,
                  );

                  return activos;
              }

              setTiposDocumento([]);

              return [];
          } catch (error) {
              setTiposDocumento([]);

              await mostrarAlerta(
                  'Error',
                  'No fue posible consultar los tipos de documento.',
                  'error',
              );

              return [];
          } finally {
              setCargandoTiposDocumento(
                  false,
              );
          }
      };

  const obtenerDocumentosPorActividad =
      async (
          actividadId: string,
      ): Promise<DocumentoActividad[]> => {
          if (!actividadId) {
              return [];
          }

          const payload = {
              taC_ID: Number(
                  actividadId,
              ),
          };

          const response =
              await apiCredito
                  .PostObtenerDocumentoPorTipoActividad(
                      payload,
                  );

          if (
              response &&
              response.isSuccess &&
              Array.isArray(
                  response.data,
              )
          ) {
              return response.data;
          }

          return [];
      };

  const recargarDocumentos =
      async (
          actividadId: string,
          mostrarError = true,
      ) => {
          if (!actividadId) {
              setDocumentos([]);

              return;
          }

          try {
              setBuscandoDocumentos(
                  true,
              );

              setBusquedaRealizada(
                  true,
              );

              const resultado =
                  await obtenerDocumentosPorActividad(
                      actividadId,
                  );

              setDocumentos(
                  resultado,
              );
          } catch (error) {
              setDocumentos([]);

              if (mostrarError) {
                  await mostrarAlerta(
                      'Error',
                      'No fue posible consultar los documentos.',
                      'error',
                  );
              }
          } finally {
              setBuscandoDocumentos(
                  false,
              );
          }
      };

  const buscarDocumentos =
      async () => {
          if (
              !tipoActividadSeleccionado
          ) {
              return;
          }

          await recargarDocumentos(
              tipoActividadSeleccionado,
          );
      };

  const obtenerDocumentosDisponibles =
      async (
          actividadId: string,
      ) => {
          if (!actividadId) {
              setDocumentosDisponibles(
                  [],
              );

              return;
          }

          try {
              setDocumentoSeleccionado(
                  null,
              );

              let catalogo =
                  tiposDocumento;

              if (
                  catalogo.length === 0
              ) {
                  catalogo =
                      await obtenerTiposDocumento();
              }

              const relacionados =
                  await obtenerDocumentosPorActividad(
                      actividadId,
                  );

              const idsRelacionados =
                  relacionados.map(
                      item =>
                          String(
                              item.tdC_ID,
                          ),
                  );

              const disponibles =
                  catalogo.filter(
                      item =>
                          !idsRelacionados.includes(
                              String(
                                  item.tdC_ID,
                              ),
                          ),
                  );

              setDocumentosDisponibles(
                  disponibles,
              );
          } catch (error) {
              setDocumentosDisponibles(
                  [],
              );

              await mostrarAlerta(
                  'Error',
                  'No fue posible consultar los documentos disponibles.',
                  'error',
              );
          }
      };

  useEffect(() => {
      obtenerTiposActividad();

      obtenerTiposDocumento();
  }, []);

  const cambiarTipoActividad = (
      event: React.ChangeEvent<HTMLInputElement>,
  ) => {
      setTipoActividadSeleccionado(
          event.target.value,
      );

      setDocumentos([]);

      setBusquedaRealizada(false);
  };

  const limpiarModal = () => {
      setDocumentoEditando(null);

      setActividadAgregar('');

      setDocumentoSeleccionado(
          null,
      );

      setDocumentosDisponibles(
          [],
      );

      setObligatorio(false);

      setCarga(false);

      setVisualiza(false);
  };

  const abrirAgregar =
      async () => {
          limpiarModal();

          const actividadInicial =
              tipoActividadSeleccionado ||
              '';

          setActividadAgregar(
              actividadInicial,
          );

          setOpenAgregar(true);

          if (actividadInicial) {
              await obtenerDocumentosDisponibles(
                  actividadInicial,
              );
          }
      };

  const abrirEditar = (
      item: DocumentoActividad,
  ) => {
      if (
          !tipoActividadSeleccionado
      ) {
          return;
      }

      const documentoCatalogo =
          tiposDocumento.find(
              documento =>
                  String(
                      documento.tdC_ID,
                  ) ===
                  String(
                      item.tdC_ID,
                  ),
          );

      let documentoActual:
          TipoDocumento;

      if (documentoCatalogo) {
          documentoActual =
              documentoCatalogo;
      } else {
          documentoActual = {
              tdC_ID: Number(
                  item.tdC_ID,
              ),
              tdC_DESCRIPCION:
                  item.tipO_DOCUMENTO,
              tdC_ESTADO: 1,
          };
      }

      setDocumentoEditando(
          item,
      );

      setActividadAgregar(
          tipoActividadSeleccionado,
      );

      setDocumentoSeleccionado(
          documentoActual,
      );

      setDocumentosDisponibles([
          documentoActual,
      ]);

      setObligatorio(
          item.obligatorio === 'S',
      );

      setCarga(
          item.carga === 'S',
      );

      setVisualiza(
          item.visualiza === 'S',
      );

      setOpenAgregar(true);
  };

  const cerrarAgregar = () => {
      if (guardandoDocumento) {
          return;
      }

      setOpenAgregar(false);

      limpiarModal();
  };

  const cambiarActividadAgregar =
      async (
          event: React.ChangeEvent<HTMLInputElement>,
      ) => {
          const actividadId =
              event.target.value;

          setActividadAgregar(
              actividadId,
          );

          setDocumentoSeleccionado(
              null,
          );

          setDocumentosDisponibles(
              [],
          );

          if (!actividadId) {
              return;
          }

          await obtenerDocumentosDisponibles(
              actividadId,
          );
      };

  const guardarDocumento =
      async () => {
          if (!actividadAgregar) {
              await mostrarAlerta(
                  'Información',
                  'Debe seleccionar un tipo de actividad.',
                  'info',
              );

              return;
          }

          if (!documentoSeleccionado) {
              await mostrarAlerta(
                  'Información',
                  'Debe seleccionar un documento.',
                  'info',
              );

              return;
          }

          const esEdicion =
              documentoEditando !== null;

          const confirmar =
              await swal({
                  title:
                      obtenerTituloConfirmacion(),
                  text:
                      documentoSeleccionado
                          .tdC_DESCRIPCION,
                  icon: 'info',
                  buttons: {
                      cancel: {
                          text: 'No',
                          value: null,
                          visible: true,
                          closeModal: true,
                      },

                      confirm: {
                          text: 'Sí',
                          value: true,
                          visible: true,
                          closeModal: true,
                      },
                  },
                  closeOnClickOutside: false,
                  closeOnEsc: false,
              });

          if (!confirmar) {
              return;
          }

          const actividadId =
              actividadAgregar;

          try {
              setGuardandoDocumento(
                  true,
              );

              const payload:
                  PayloadGuardarDocumento = {
                      taC_ID: Number(
                          actividadId,
                      ),

                      tdC_ID: Number(
                          documentoSeleccionado
                              .tdC_ID,
                      ),

                      dcA_OBLIGATORIO:
                          obligatorio
                              ? 'S'
                              : 'N',

                      dcA_CARGA:
                          carga
                              ? 'S'
                              : 'N',

                      dcA_VISUALIZA:
                          visualiza
                              ? 'S'
                              : 'N',
                  };

              const response =
                  await apiCredito
                      .PostInsertarDocumentoCreditoPorActividad(
                          payload,
                      );

              if (
                  !response ||
                  !response.isSuccess
              ) {
                  let mensajeError =
                      obtenerMensajeErrorGuardar(
                          esEdicion,
                      );

                  if (
                      response &&
                      response.returnMessage
                  ) {
                      mensajeError =
                          response.returnMessage;
                  }

                  setGuardandoDocumento(
                      false,
                  );

                  await mostrarAlerta(
                      'Error',
                      mensajeError,
                      'error',
                  );

                  return;
              }

              let mensajeExito =
                  obtenerMensajeExito(
                      esEdicion,
                  );

              if (
                  response.returnMessage
              ) {
                  mensajeExito =
                      response.returnMessage;
              }

              /*
               * Primero se detiene el spinner.
               */
              setGuardandoDocumento(
                  false,
              );

              /*
               * Luego se cierra el modal.
               */
              setOpenAgregar(false);

              limpiarModal();

              setTipoActividadSeleccionado(
                  actividadId,
              );

              /*
               * Después se actualiza la tabla.
               */
              await recargarDocumentos(
                  actividadId,
                  false,
              );

              /*
               * Finalmente se muestra el éxito
               * con botón Aceptar.
               */
              await mostrarAlerta(
                  obtenerTituloExito(
                      esEdicion,
                  ),
                  mensajeExito,
                  'success',
              );
          } catch (error) {
              setGuardandoDocumento(
                  false,
              );

              const mensajeError =
                  obtenerMensajeErrorServicio(
                      error,
                      obtenerMensajeErrorGuardar(
                          esEdicion,
                      ),
                  );

              await mostrarAlerta(
                  'Error',
                  mensajeError,
                  'error',
              );
          } finally {
              setGuardandoDocumento(
                  false,
              );
          }
      };

  const eliminarDocumento =
      async (
          item: DocumentoActividad,
      ) => {
          if (
              !tipoActividadSeleccionado
          ) {
              await mostrarAlerta(
                  'Información',
                  'No fue posible identificar la actividad seleccionada.',
                  'info',
              );

              return;
          }

          const confirmar =
              await swal({
                  title: '¿Estás seguro de eliminar este documento?',
                  text:
                      item.tipO_DOCUMENTO,
                  icon: 'warning',
                  buttons: {
                      cancel: {
                          text: 'No',
                          value: null,
                          visible: true,
                          closeModal: true,
                      },

                      confirm: {
                          text: 'Sí',
                          value: true,
                          visible: true,
                          closeModal: true,
                      },
                  },
                  dangerMode: true,
                  closeOnClickOutside: false,
                  closeOnEsc: false,
              });

          if (!confirmar) {
              return;
          }

          const actividadId =
              tipoActividadSeleccionado;

          try {
              setEliminandoDocumento(
                  true,
              );

              const payload:
                  PayloadEliminarDocumento = {
                      taC_ID: Number(
                          actividadId,
                      ),

                      tdC_ID: Number(
                          item.tdC_ID,
                      ),
                  };

              const response =
                  await apiCredito
                      .PostEliminarDocumentoActividadCredito(
                          payload,
                      );

              if (
                  !response ||
                  !response.isSuccess
              ) {
                  let mensajeError =
                      'No fue posible eliminar el documento.';

                  if (
                      response &&
                      response.returnMessage
                  ) {
                      mensajeError =
                          response.returnMessage;
                  }

                  setEliminandoDocumento(
                      false,
                  );

                  await mostrarAlerta(
                      'Error',
                      mensajeError,
                      'error',
                  );

                  return;
              }

              let mensajeExito =
                  'El documento fue eliminado correctamente.';

              if (
                  response.returnMessage
              ) {
                  mensajeExito =
                      response.returnMessage;
              }

              /*
               * Se quita el estado de eliminación
               * antes de abrir el SweetAlert.
               */
              setEliminandoDocumento(
                  false,
              );

              /*
               * Se actualiza la tabla.
               */
              await recargarDocumentos(
                  actividadId,
                  false,
              );

              /*
               * Se muestra el éxito con Aceptar.
               */
              await mostrarAlerta(
                  'Documento eliminado',
                  mensajeExito,
                  'success',
              );
          } catch (error) {
              setEliminandoDocumento(
                  false,
              );

              const mensajeError =
                  obtenerMensajeErrorServicio(
                      error,
                      'No fue posible eliminar el documento.',
                  );

              await mostrarAlerta(
                  'Error',
                  mensajeError,
                  'error',
              );
          } finally {
              setEliminandoDocumento(
                  false,
              );
          }
      };

  const renderSiNo = (
      valor: string,
  ) => (
      <Typography
          component="span"
          className={
              valor === 'S'
                  ? classes.optionYes
                  : classes.optionNo
          }
      >
          {valor === 'S'
              ? 'Sí'
              : 'No'}
      </Typography>
  );

  const renderIconoGuardar = () => {
      if (!guardandoDocumento) {
          return undefined;
      }

      return (
          <CircularProgress
              size={17}
              color="inherit"
          />
      );
  };

  return (
      <Box
          className={
              classes.root
          }
      >
          <Paper
              className={
                  classes.mainPaper
              }
          >
              <Box
                  className={
                      classes.header
                  }
              >
                  <Typography
                      className={
                          classes.title
                      }
                  >
                      Parametrización documentos
                  </Typography>

                  <Typography
                      variant="body2"
                      color="textSecondary"
                      className={
                          classes.subtitle
                      }
                  >
                      Administración de documentos requeridos
                  </Typography>
              </Box>

              <Button
                  className={
                      classes.addButton
                  }
                  variant="contained"
                  startIcon={
                      <AddIcon />
                  }
                  onClick={
                      abrirAgregar
                  }
                  disabled={
                      guardandoDocumento ||
                      eliminandoDocumento
                  }
              >
                  Agregar
              </Button>

              <Paper
                  elevation={0}
                  className={
                      classes.tablePaper
                  }
                  style={{
                      marginTop: 18,
                  }}
              >
                  <Grid
                      container
                      spacing={2}
                      alignItems="center"
                      className={
                          classes.filtrosContainer
                      }
                  >
                      <Grid
                          item
                          xs={12}
                          sm={9}
                          md={10}
                      >
                          <TextField
                              select
                              fullWidth
                              variant="outlined"
                              size="small"
                              label="Tipo de actividad"
                              value={
                                  tipoActividadSeleccionado
                              }
                              onChange={
                                  cambiarTipoActividad
                              }
                              disabled={
                                  cargandoActividades ||
                                  guardandoDocumento ||
                                  eliminandoDocumento
                              }
                              className={
                                  classes.selectActividad
                              }
                          >
                              <MenuItem value="">
                                  <em>
                                      Seleccione una actividad
                                  </em>
                              </MenuItem>

                              {tiposActividad.map(
                                  actividad => (
                                      <MenuItem
                                          key={
                                              actividad.id
                                          }
                                          value={
                                              actividad.id
                                          }
                                      >
                                          {
                                              actividad.descripcion
                                          }
                                      </MenuItem>
                                  ),
                              )}
                          </TextField>
                      </Grid>

                      <Grid
                          item
                          xs={12}
                          sm={3}
                          md={2}
                      >
                          <Button
                              fullWidth
                              variant="contained"
                              className={
                                  classes.searchButton
                              }
                              startIcon={
                                  buscandoDocumentos ? (
                                      <CircularProgress
                                          size={18}
                                          color="inherit"
                                      />
                                  ) : (
                                      <SearchIcon />
                                  )
                              }
                              disabled={
                                  !tipoActividadSeleccionado ||
                                  buscandoDocumentos ||
                                  guardandoDocumento ||
                                  eliminandoDocumento
                              }
                              onClick={
                                  buscarDocumentos
                              }
                          >
                              {buscandoDocumentos
                                  ? 'Buscando'
                                  : 'Buscar'}
                          </Button>
                      </Grid>
                  </Grid>

                  <Paper
                      variant="outlined"
                      className={
                          classes.tableContainer
                      }
                  >
                      <Table
                          size="small"
                          className={
                              classes.table
                          }
                      >
                          <TableHead
                              className={
                                  classes.tableHead
                              }
                          >
                              <TableRow>
                                  <TableCell
                                      className={
                                          classes.headCell
                                      }
                                  >
                                      Documento
                                  </TableCell>

                                  <TableCell
                                      className={
                                          classes.headCell
                                      }
                                  >
                                      Actividad
                                  </TableCell>

                                  <TableCell
                                      className={
                                          classes.headCell
                                      }
                                  >
                                      Obligatorio
                                  </TableCell>

                                  <TableCell
                                      className={
                                          classes.headCell
                                      }
                                  >
                                      Carga
                                  </TableCell>

                                  <TableCell
                                      className={
                                          classes.headCell
                                      }
                                  >
                                      Visualiza
                                  </TableCell>

                                  <TableCell
                                      className={
                                          classes.headCell
                                      }
                                  >
                                      Acciones
                                  </TableCell>
                              </TableRow>
                          </TableHead>

                          <TableBody>
                              {!buscandoDocumentos &&
                                  documentos.map(
                                      (
                                          item,
                                          index,
                                      ) => (
                                          <TableRow
                                              hover
                                              key={`${item.tdC_ID}-${index}`}
                                              className={
                                                  index %
                                                      2 ===
                                                  0
                                                      ? classes.rowGray
                                                      : classes.rowLight
                                              }
                                          >
                                              <TableCell
                                                  className={
                                                      classes.documentCell
                                                  }
                                              >
                                                  {
                                                      item.tipO_DOCUMENTO
                                                  }
                                              </TableCell>

                                              <TableCell
                                                  className={
                                                      classes.activityCell
                                                  }
                                              >
                                                  {
                                                      item.actividad
                                                  }
                                              </TableCell>

                                              <TableCell
                                                  className={
                                                      classes.bodyCell
                                                  }
                                              >
                                                  {renderSiNo(
                                                      item.obligatorio,
                                                  )}
                                              </TableCell>

                                              <TableCell
                                                  className={
                                                      classes.bodyCell
                                                  }
                                              >
                                                  {renderSiNo(
                                                      item.carga,
                                                  )}
                                              </TableCell>

                                              <TableCell
                                                  className={
                                                      classes.bodyCell
                                                  }
                                              >
                                                  {renderSiNo(
                                                      item.visualiza,
                                                  )}
                                              </TableCell>

                                              <TableCell
                                                  className={
                                                      classes.bodyCell
                                                  }
                                              >
                                                  <Box
                                                      className={
                                                          classes.actions
                                                      }
                                                  >
                                                      <Tooltip title="Editar documento">
                                                          <span>
                                                              <IconButton
                                                                  size="small"
                                                                  className={
                                                                      classes.editButton
                                                                  }
                                                                  disabled={
                                                                      eliminandoDocumento ||
                                                                      guardandoDocumento
                                                                  }
                                                                  onClick={() =>
                                                                      abrirEditar(
                                                                          item,
                                                                      )
                                                                  }
                                                              >
                                                                  <EditIcon />
                                                              </IconButton>
                                                          </span>
                                                      </Tooltip>

                                                      <Tooltip title="Eliminar documento">
                                                          <span>
                                                              <IconButton
                                                                  size="small"
                                                                  className={
                                                                      classes.deleteButton
                                                                  }
                                                                  disabled={
                                                                      eliminandoDocumento ||
                                                                      guardandoDocumento
                                                                  }
                                                                  onClick={() =>
                                                                      eliminarDocumento(
                                                                          item,
                                                                      )
                                                                  }
                                                              >
                                                                  <DeleteIcon />
                                                              </IconButton>
                                                          </span>
                                                      </Tooltip>
                                                  </Box>
                                              </TableCell>
                                          </TableRow>
                                      ),
                                  )}

                              {buscandoDocumentos && (
                                  <TableRow>
                                      <TableCell
                                          colSpan={6}
                                      >
                                          <Box
                                              className={
                                                  classes.loadingBox
                                              }
                                          >
                                              <CircularProgress
                                                  size={28}
                                              />
                                          </Box>
                                      </TableCell>
                                  </TableRow>
                              )}

                              {!buscandoDocumentos &&
                                  !busquedaRealizada && (
                                      <TableRow>
                                          <TableCell
                                              colSpan={6}
                                          >
                                              <Box
                                                  className={
                                                      classes.emptyBox
                                                  }
                                              >
                                                  <Typography
                                                      variant="body2"
                                                      color="textSecondary"
                                                  >
                                                      Seleccione un tipo de actividad y presione Buscar
                                                  </Typography>
                                              </Box>
                                          </TableCell>
                                      </TableRow>
                                  )}

                              {!buscandoDocumentos &&
                                  busquedaRealizada &&
                                  documentos.length ===
                                      0 && (
                                      <TableRow>
                                          <TableCell
                                              colSpan={6}
                                          >
                                              <Box
                                                  className={
                                                      classes.emptyBox
                                                  }
                                              >
                                                  <Typography
                                                      variant="body2"
                                                      color="textSecondary"
                                                  >
                                                      No se encontraron documentos para la actividad seleccionada
                                                  </Typography>
                                              </Box>
                                          </TableCell>
                                      </TableRow>
                                  )}
                          </TableBody>
                      </Table>
                  </Paper>
              </Paper>
          </Paper>

          <Dialog
              open={
                  openAgregar
              }
              onClose={
                  cerrarAgregar
              }
              maxWidth="sm"
              fullWidth
              disableBackdropClick={
                  guardandoDocumento
              }
              disableEscapeKeyDown={
                  guardandoDocumento
              }
              classes={{
                  paper:
                      classes.dialogPaper,
              }}
          >
              <DialogTitle
                  className={
                      classes.dialogTitleBox
                  }
              >
                  <Typography
                      className={
                          classes.dialogTitle
                      }
                  >
                      {obtenerTituloModal()}
                  </Typography>
              </DialogTitle>

              <DialogContent
                  className={
                      classes.dialogContent
                  }
              >
                  <TextField
                      select
                      fullWidth
                      variant="outlined"
                      size="small"
                      label="Tipo de actividad"
                      value={
                          actividadAgregar
                      }
                      onChange={
                          cambiarActividadAgregar
                      }
                      disabled={
                          guardandoDocumento ||
                          documentoEditando !==
                              null
                      }
                      margin="normal"
                  >
                      <MenuItem value="">
                          <em>
                              Seleccione una actividad
                          </em>
                      </MenuItem>

                      {tiposActividad.map(
                          actividad => (
                              <MenuItem
                                  key={
                                      actividad.id
                                  }
                                  value={
                                      actividad.id
                                  }
                              >
                                  {
                                      actividad.descripcion
                                  }
                              </MenuItem>
                          ),
                      )}
                  </TextField>

                  <Autocomplete
                      options={
                          documentosDisponibles
                      }
                      value={
                          documentoSeleccionado
                      }
                      getOptionLabel={option =>
                          option?.tdC_DESCRIPCION ||
                          ''
                      }
                      getOptionSelected={(
                          option,
                          value,
                      ) =>
                          option.tdC_ID ===
                          value.tdC_ID
                      }
                      onChange={(
                          _event,
                          value,
                      ) =>
                          setDocumentoSeleccionado(
                              value,
                          )
                      }
                      disabled={
                          documentoEditando !==
                              null ||
                          !actividadAgregar ||
                          cargandoTiposDocumento ||
                          guardandoDocumento
                      }
                      noOptionsText="No hay documentos disponibles"
                      renderInput={params => (
                          <TextField
                              {...params}
                              fullWidth
                              variant="outlined"
                              size="small"
                              label="Documento"
                              margin="normal"
                              placeholder="Escriba para buscar..."
                              helperText={
                                  obtenerHelperDocumento()
                              }
                              InputProps={{
                                  ...params.InputProps,

                                  endAdornment: (
                                      <>
                                          {cargandoTiposDocumento && (
                                              <CircularProgress
                                                  size={18}
                                              />
                                          )}

                                          {
                                              params
                                                  .InputProps
                                                  .endAdornment
                                          }
                                      </>
                                  ),
                              }}
                          />
                      )}
                  />

                  <Box
                      className={
                          classes.checksContainer
                      }
                  >
                      <Grid
                          container
                          spacing={2}
                      >
                          <Grid
                              item
                              xs={12}
                              sm={4}
                          >
                              <Box
                                  className={
                                      classes.checkBoxItem
                                  }
                              >
                                  <Checkbox
                                      checked={
                                          obligatorio
                                      }
                                      onChange={event =>
                                          setObligatorio(
                                              event
                                                  .target
                                                  .checked,
                                          )
                                      }
                                      color="primary"
                                      disabled={
                                          guardandoDocumento
                                      }
                                  />

                                  <Typography>
                                      Obligatorio
                                  </Typography>
                              </Box>
                          </Grid>

                          <Grid
                              item
                              xs={12}
                              sm={4}
                          >
                              <Box
                                  className={
                                      classes.checkBoxItem
                                  }
                              >
                                  <Checkbox
                                      checked={
                                          carga
                                      }
                                      onChange={event =>
                                          setCarga(
                                              event
                                                  .target
                                                  .checked,
                                          )
                                      }
                                      color="primary"
                                      disabled={
                                          guardandoDocumento
                                      }
                                  />

                                  <Typography>
                                      Carga
                                  </Typography>
                              </Box>
                          </Grid>

                          <Grid
                              item
                              xs={12}
                              sm={4}
                          >
                              <Box
                                  className={
                                      classes.checkBoxItem
                                  }
                              >
                                  <Checkbox
                                      checked={
                                          visualiza
                                      }
                                      onChange={event =>
                                          setVisualiza(
                                              event
                                                  .target
                                                  .checked,
                                          )
                                      }
                                      color="primary"
                                      disabled={
                                          guardandoDocumento
                                      }
                                  />

                                  <Typography>
                                      Visualiza
                                  </Typography>
                              </Box>
                          </Grid>
                      </Grid>
                  </Box>
              </DialogContent>

              <DialogActions>
                  <Button
                      className={
                          classes.cancelButton
                      }
                      onClick={
                          cerrarAgregar
                      }
                      disabled={
                          guardandoDocumento
                      }
                  >
                      Cancelar
                  </Button>

                  <Button
                      variant="contained"
                      className={
                          classes.saveButton
                      }
                      disabled={
                          !actividadAgregar ||
                          !documentoSeleccionado ||
                          guardandoDocumento
                      }
                      onClick={
                          guardarDocumento
                      }
                      startIcon={
                          renderIconoGuardar()
                      }
                  >
                      {obtenerTextoBotonGuardar()}
                  </Button>
              </DialogActions>
          </Dialog>
      </Box>
  );
}

export default ParametrizacionDocumento;