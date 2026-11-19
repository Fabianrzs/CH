import React, { useEffect, useState } from 'react';
import { Theme, makeStyles } from '@material-ui/core/styles';
import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { useLocation } from 'react-router-dom';
import { Button, FormControlLabel, FormLabel, Grid, MenuItem, Radio, RadioGroup } from '@material-ui/core';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import history from '../../../utils/history.router';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import CatalogApi from '../../../services/api-catalog.service';
import SolicitudApi from '../../../services/api-solicitud.service';
import NotificationService from '../../../services/notification.service';
import { Address } from "../../../types/address.type";
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import AddressComponentV2 from '../../../components/modals/address/addressV2.component'
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import { LoadingComponent } from '../../../components/loading/loading.component';
import TramitesIntervinientes from './tramitesIntervinientes';
import HistorialCambiosIntervinientes from './HistorialCambiosIntervinientes';
import InformacionPagoIntervinientes from './InformacionPagoIntervinientes';
import AuthService from '../../../services/auth.service';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: '100%',

    '& .MuiAccordion-root': {
      marginBottom: 0,
    },
  },
  firstAccordionTab: {
    color: '#ffffff',
    backgroundColor: '#1C3F8B',
    fontSize: '100',
    flexDirection: 'row-reverse',
    '&:hover': {
      backgroundColor: '#fec917',
    }
  },
  button: {
    width: '400px',
    backgroundColor: '#1C3F8B',
    color: '#FFFFFF',
    marginLeft: '.5rem',
    '&:hover': {
      backgroundColor: '#FFFFFF !important',
      color: 'black !important'
    },
  },
  column: {
    padding: '10px',
    textAlign: 'center',
  },
  container: {
    maxHeight: 440,
  },
  fiveAccordionTab: {
    color: '#ffffff',
    backgroundColor: '#1C3F8B',
    flexDirection: 'row-reverse',
    '&:hover': {
      backgroundColor: '#fec917',
    },
  },
  heading: {
    fontSize: theme.typography.pxToRem(15),
  }
}));


const DetalleActulizacionesInterviniente = () => {

  const apiCatalogo = CatalogApi();
  const apiSolicitud = SolicitudApi();
  const classes = useStyles();

  const [listaTipoIdentificacion, setListaTipoIdentificacion] = useState([])
  const [nuevoListadoIdenticacion, setNuevoListadoIdenticacion] = useState([])
  const [listaDepartamentos, setListaDepartamentos] = useState<any[]>([])
  const [nullDepartamentoValue, setNullDepartamentoValue] = useState(false)
  const [departamentoValue, setDepartamentoValue] = useState<any | null>(null)
  const [listaEntidadesBancarias, setListaEntidadesBancarias] = useState([])
  const [listaDCiudades, setListaDCiudades] = useState<any[]>([])
  const [listaTipoActores, setListaTipoActores] = useState<any[]>([]);
  const [listaTipoCuentas, setListaTipoCuenta] = useState([])
  const [open, setOpen] = useState(false);
  const [mostrar, setMostrar] = useState(false);
  const [loading, setLoading] = useState(false);
  const [nullCiudadValue, setNullCiudadValue] = useState(false)
  const [ciudadValue, setCiudadValue] = useState<any | null>(null)
  const [nullEntidadValue, setNullEntidadValue] = useState(false)
  const [entidadValue, setEntidadValue] = useState<any[] | null>(null)
  const [datosLlenarTablaPagos, setDatosLlenarTablaPagos] = useState<any>('')
  const [informacionListaTramites, setInformacionListaTramites] = useState<any>(null);
  const user = AuthService.getUserInfo();

  const location: any = useLocation();
  const informacionInterviniente = location?.state?.row;


  const formularioTipoActor = useForm({
    defaultValues: {
      tipoDocumento: '',
      numeroDocumento: '',
      nombre: '',
      personaJuridicaNatural: '',
      celular: '',
      correoElectronico: '',
      departamento: '',
      ciudad: '',
      direccion: '',
      barrio: '',
      telefono: '',
      tipoActor: '',
      tipoCuenta: '',
      numeroCuentaBancaria: '',
      entidadBancaria: 0,
      fechaExpedicionCedula: null,
      constructora: 'no',
      convenioEntidad: 'no',

      tipoDocumentoAfiliado: '',
      nombreAfiliado: '',
      numeroDocumentoAfiliado: '',
    }
  })

  useEffect(() => {
    setLoading(true)
    setTimeout(() => {
      setLoading(false)
    }, 2500);
  }, [])

  useEffect(() => {
    apiCatalogo
      .getAllTypeDocuments()
      .then(async response => {
        if (response) {
          if (response.data !== null) {
            setListaTipoIdentificacion(response?.data)

          }
        }
      })
      .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });

      });

  }, [])

  useEffect(() => {
    if (informacionInterviniente?.itR_ID) {
      obtenerTipoActorById(String(informacionInterviniente?.itR_ID).toUpperCase())
    }
  }, [listaDepartamentos, listaTipoIdentificacion])

  useEffect(() => {

    apiCatalogo.getAllStates()
      .then(response => {
        if (response.data !== null) {
          setListaDepartamentos(response?.data);

        }
      })
      .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });

      });
    const request = {
      tipoDocumento: informacionInterviniente?.itR_TIPO_DOCUMENTO,
      numeroDocumento: informacionInterviniente?.itR_NUMERO_DOCUMENTO
    }
    consultarListaTramites(request);
  }, [informacionInterviniente?.itR_ID, listaTipoIdentificacion]);

  useEffect(() => {
    apiCatalogo.
      getAllCatalogoPorId(67)
      .then(async response => {
        if (response) {
          if (response.data !== null) {
            setListaTipoCuenta(response?.data)
          }
        }
      })
      .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });
      });
  }, [])

  useEffect(() => {
    apiCatalogo.
      getAllEntidadBancaria()
      .then(async response => {
        if (response) {
          if (response.data !== null) {
            setListaEntidadesBancarias(response?.data)
          }
        }
      })
      .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });
      });
  }, [])

  useEffect(() => {
    apiSolicitud
      .GetConsultarIntervinientesActores()
      .then(async response => {
        if (response) {
          if (response.data !== null) {
            setListaTipoActores(response?.data)
          }
        }
      })
      .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });
      });
  }, [])

  const CODIGO_BENEFICIARIO_ESTUDIO =
    'BEST';

  const valorTipoActor: any =
    formularioTipoActor.watch('tipoActor');

  const idTipoActorActual =
    valorTipoActor ||
    informacionInterviniente?.itA_ID_FK ||
    '';

  const tipoActorSeleccionado: any =
    listaTipoActores.find(
      (item: any) =>
        String(item?.itA_ID) ===
        String(idTipoActorActual),
    );

  const normalizarTexto = (valor: any) =>
    String(valor ?? '')
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .trim()
      .replace(/\s+/g, ' ')
      .toUpperCase();

  const esTipoActorBeneficiarioPago =
    tipoActorSeleccionado?.itA_CODIGO ===
    CODIGO_BENEFICIARIO_ESTUDIO ||
    normalizarTexto(
      tipoActorSeleccionado?.itA_NOMBRE,
    ) === 'BENEFICIARIO DE PAGO';

  const filterCiudadesByDepartamento = (departamentoId: any) => {
    if (!departamentoId || departamentoId === undefined) {
      setListaDCiudades([])
    } else {
      apiCatalogo.getAllCities()
        .then(response => {
          if (response.data !== null) {
            const nuevaListaCiudades = response?.data.filter((ciudad: any) => ciudad.idDepartamento === departamentoId)

            setListaDCiudades(nuevaListaCiudades)
          }
        })
        .catch(() => {
          toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
    }
  };

  const estadoPersona = (item: any) => {

    if (item === 'juridica') {
      const tipoIdentificacion = listaTipoIdentificacion.filter((x: any) => x.descripcion === "NÚMERO DE IDENTIFICACIÓN TRIBUTARIA")
      setNuevoListadoIdenticacion(tipoIdentificacion)
    } else {
      setNuevoListadoIdenticacion(listaTipoIdentificacion)
    }
  }

  const estadoConstructora = (item: any) => {
    if (item === 'si') {
      setMostrar(true)
    } else {
      setMostrar(false)
    }

  }

  const cleanPhoneNumber = (number: any) => number.replace(/[()\s-]/g, '');

  const obtenerTipoActorById = async (id: any) => {
    const request = id;

    try {
      const response =
        await apiSolicitud.PostObtenerTipoActorById(
          request,
        );

      const data = response?.data;

      if (!response?.isSuccess || !data) {
        return;
      }

      const idDepartamento =
        data?.itR_DEPARTAMENTO;

      const idCiudad =
        data?.itR_CIUDAD;

      const entidad = Number(
        data?.itR_ENTIDAD_BANCARIA,
      );

      const departamentoEncontrado =
        listaDepartamentos.find(
          (item: any) =>
            item?.id === idDepartamento,
        );

      // const tipoDocumentoAfiliado =
      //   data?.itR_TIPO_DOCUMENTO_AFILIADO ||
      //   data?.tipoDocumentoAfiliado ||
      //   '';

      setDepartamentoValue(
        departamentoEncontrado || null,
      );

      setNullDepartamentoValue(false);

      /*
       * Carga la entidad bancaria correspondiente.
       */
      apiCatalogo
        .getAllEntidadBancaria()
        .then((responseEntidad) => {
          if (!responseEntidad?.data) {
            return;
          }

          const entidadesBancarias =
            responseEntidad.data;

          setListaEntidadesBancarias(
            entidadesBancarias,
          );

          const entidadEncontrada =
            entidadesBancarias.find(
              (item: any) =>
                Number(item?.codigoBanco) ===
                entidad,
            );

          setEntidadValue(
            entidadEncontrada || null,
          );

          setNullEntidadValue(false);

          formularioTipoActor.setValue(
            'entidadBancaria',
            entidad,
          );
        })
        .catch(() => {
          toast('Error del sistema', {
            type: toast.TYPE.ERROR,
          });
        });

      /*
       * Carga las ciudades correspondientes
       * al departamento del interviniente.
       */
      if (!idDepartamento) {
        setListaDCiudades([]);
        setCiudadValue(null);
        setNullCiudadValue(true);
      } else {
        apiCatalogo
          .getAllCities()
          .then((responseCiudades) => {
            if (!responseCiudades?.data) {
              return;
            }

            const ciudades =
              responseCiudades.data;

            const ciudadesDepartamento =
              ciudades.filter(
                (ciudad: any) =>
                  ciudad?.idDepartamento ===
                  idDepartamento,
              );

            const ciudadEncontrada =
              ciudades.find(
                (item: any) =>
                  item?.id === idCiudad,
              );

            setListaDCiudades(
              ciudadesDepartamento,
            );

            setCiudadValue(
              ciudadEncontrada || null,
            );

            setNullCiudadValue(false);

            formularioTipoActor.setValue(
              'ciudad',
              idCiudad || '',
            );
          })
          .catch(() => {
            toast('Error del sistema', {
              type: toast.TYPE.ERROR,
            });
          });
      }

      /*
       * Información básica del beneficiario.
       */
      formularioTipoActor.setValue(
        'tipoDocumento',
        data?.itR_TIPO_DOCUMENTO || '',
      );

      formularioTipoActor.setValue(
        'personaJuridicaNatural',
        data?.itR_TIPO_PERSONA || '',
      );

      estadoPersona(
        data?.itR_TIPO_PERSONA,
      );

      formularioTipoActor.setValue(
        'numeroDocumento',
        data?.itR_NUMERO_DOCUMENTO || '',
      );

      const nombreCompleto = [
        data?.itR_NOMBRE,
        data?.itR_SEGUNDONOMBRE,
        data?.itR_PRIMERAPELLIDO,
        data?.itR_SEGUNDOAPELLIDO,
      ]
        .filter(Boolean)
        .join(' ');

      formularioTipoActor.setValue(
        'nombre',
        nombreCompleto,
      );

      formularioTipoActor.setValue(
        'celular',
        data?.itR_CELULAR || '',
      );

      formularioTipoActor.setValue(
        'correoElectronico',
        data?.itR_CORREO_ELECTRONICO || '',
      );

      formularioTipoActor.setValue(
        'departamento',
        idDepartamento || '',
      );

      formularioTipoActor.setValue(
        'direccion',
        data?.itR_DIRECCION || '',
      );

      formularioTipoActor.setValue(
        'barrio',
        data?.itR_BARRIO || '',
      );

      formularioTipoActor.setValue(
        'telefono',
        data?.itR_TELEFONO || '',
      );

      formularioTipoActor.setValue(
        'tipoCuenta',
        data?.itR_TIPO_CUENTA || '',
      );

      formularioTipoActor.setValue(
        'numeroCuentaBancaria',
        data?.itR_NUMERO_CUENTA_BANCARIA ||
        '',
      );

      formularioTipoActor.setValue(
        'fechaExpedicionCedula',
        data?.itR_FECHA_EXPEDICION || null,
      );

      formularioTipoActor.setValue(
        'constructora',
        data?.itR_CONSTRUCTORA || 'no',
      );

      formularioTipoActor.setValue(
        'convenioEntidad',
        data?.itR_CONVENIO_ENTIDAD || 'no',
      );

      formularioTipoActor.setValue(
        'tipoActor',
        data?.itA_ID_FK || '',
      );

      setMostrar(
        data?.itR_CONSTRUCTORA === 'si',
      );


      // formularioTipoActor.setValue(
      //   'tipoDocumentoAfiliado',
      //   tipoDocumentoAfiliado,
      // );

      setDatosLlenarTablaPagos(data);
    } catch (e: any) {
      toast(
        `Error al consultar el interviniente ${e?.message || ''
        }`,
        {
          type: toast.TYPE.ERROR,
        },
      );
    }
  };

  const onChageDepartamento = (evento: any, departamento: any) => {
    setNullDepartamentoValue(departamento === null)
    setDepartamentoValue(departamento)
    setNullCiudadValue(true)
    setCiudadValue(null)
    filterCiudadesByDepartamento(departamento?.id)
  }

  const onChageCiudad = (evento: any, ciudad: any) => {
    setNullCiudadValue(ciudad === null)
    setCiudadValue(ciudad)
  }

  const onChageEntidad = (evento: any, entidad: any) => {
    setNullCiudadValue(entidad === null)
    setCiudadValue(entidad)
  }

  const actualizarTipoActor = (data: any) => {

    const numeroCelular = cleanPhoneNumber(data.celular);
    const numeroTelefonico = cleanPhoneNumber(data.telefono);
    const request = {
      intervinientesId: informacionInterviniente?.itR_ID,
      celular: numeroCelular,
      correoElectronico: data.correoElectronico,
      departamento: departamentoValue.id,
      ciudad: ciudadValue ? ciudadValue.id : 0,
      direccion: data.direccion,
      barrio: data.barrio,
      telefono: numeroTelefonico,
      modificadoPor: user?.USR_ID,
      modulo: 'Gestión de Intervinientes',
      nombre: informacionInterviniente?.itR_NOMBRE,
      segundoNombre: informacionInterviniente?.itR_SEGUNDONOMBRE,
      primerApellido: informacionInterviniente?.itR_PRIMERAPELLIDO,
      segundoApellido: informacionInterviniente?.itR_SEGUNDOAPELLIDO
    }

    apiSolicitud.
      PosttActualizarIntervinientes(request)
      .then(async response => {
        if (response) {
          if (response.data !== null) {
            const item = response?.data
            await NotificationService.genericAlert(
              'Actualizo con éxito',
              'Se actualizó el actor con éxito.',
            );
            history.push({
              pathname: '/GestionIntervinientes',
              state: { item }
            });
          }
        }
      })
      .catch((e) => {
        toast(`Error al actualizar interviniente ${e.message}`, { type: toast.TYPE.ERROR });
      });
  }


  const RADICADO_EN_REVISION = '21-01-20260513056238';

  const normalizarValor = (valor: any) =>
    String(valor ?? '').trim();

  const esBeneficiarioDePago = (item: any) =>
    normalizarTexto(item?.itA_NOMBRE) === 'BENEFICIARIO DE PAGO';

  const esProcesada = (item: any) =>
    normalizarTexto(item?.esS_DESCRIPCION) === 'PROCESADA';

  const resumirTramite = (item: any) => ({
    radicado: item?.soL_RADICADO_GD,
    numeroSolicitud: item?.soL_NUMERO_SOLICITUD,
    cedulaAfiliado: item?.soL_CEDULA,
    clienteIdAfiliado: item?.clI_ID,
    nombreAfiliado: item?.prP_NOMBRE_RAZON_SOCIAL,
    tipoInterviniente: item?.itA_NOMBRE,
    idInterviniente: item?.itA_ID,
    estado: item?.esS_DESCRIPCION,
  });

  const obtenerRadicadosUnicos = (items: any[]) =>
    Array.from(
      new Set(
        items
          .map((item: any) => normalizarValor(item?.soL_RADICADO_GD))
          .filter(Boolean),
      ),
    );

  const consultarListaTramites = async (request: any) => {
    try {
      const response =
        await apiSolicitud.PostListaTramitesIntervinientes(
          request,
        );

      const tramites = Array.isArray(response?.data)
        ? response.data
        : [];

      const afiliadosRelacionados = tramites
        .filter(
          (item: any) =>
            item?.clI_ID ||
            item?.soL_CEDULA ||
            item?.prP_NOMBRE_RAZON_SOCIAL,
        )
        .map((item: any) => ({
          clienteId: item?.clI_ID || '',
          numeroDocumento: item?.soL_CEDULA || '',
          nombre:
            item?.prP_NOMBRE_RAZON_SOCIAL || '',
        }));

      const afiliadosUnicos =
        afiliadosRelacionados.filter(
          (
            afiliado: any,
            indice: number,
            lista: any[],
          ) =>
            indice ===
            lista.findIndex((item: any) => {
              const mismoCliente =
                normalizarValor(
                  item?.clienteId,
                ) ===
                normalizarValor(
                  afiliado?.clienteId,
                );

              const mismoDocumento =
                normalizarValor(
                  item?.numeroDocumento,
                ) ===
                normalizarValor(
                  afiliado?.numeroDocumento,
                );

              return mismoCliente || mismoDocumento;
            }),
        );

      const afiliadoRelacionado =
        afiliadosUnicos[0];

      if (afiliadoRelacionado) {
        const tipoDocumentoAfiliado =
          listaTipoIdentificacion.find(
            (tipo: any) => tipo?.id === 1,
          );

        formularioTipoActor.setValue(
          'tipoDocumentoAfiliado',
          tipoDocumentoAfiliado?.id || '',
        );

        formularioTipoActor.setValue(
          'nombreAfiliado',
          afiliadoRelacionado?.nombre || '',
        );

        formularioTipoActor.setValue(
          'numeroDocumentoAfiliado',
          afiliadoRelacionado
            ?.numeroDocumento || '',
        );

        
      } else {
        formularioTipoActor.setValue(
          'tipoDocumentoAfiliado',
          '',
        );

        formularioTipoActor.setValue(
          'nombreAfiliado',
          '',
        );

        formularioTipoActor.setValue(
          'numeroDocumentoAfiliado',
          '',
        );
      }


      setInformacionListaTramites(tramites);
    } catch (e: any) {
      toast(
        `Error al consultar lista de trámites ${e.message}`,
        {
          type: toast.TYPE.ERROR,
        },
      );
    }
  };

  const handleOpenAddress = () => {
    setOpen(true);
  };

  const onCloseAddress = (value: Address) => {
    if (value) {
      const result = setFormDataAddres(value);
      formularioTipoActor.setValue('direccion', result);
    }
    setOpen(false);
  };

  return (
    <CHPrivateContainer>
      <LoadingComponent loading={loading} />
      <CHPageTitle
        topText="Intervinientes"
        subText=""
        title1="Gestión de"
        title2="Intervinientes"
      />

      <div className={classes.root} >
        <Accordion >
          <AccordionSummary
            expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
            aria-controls="panel1a-content"
            id="panel1a-header"
            className={classes.fiveAccordionTab}
          >
            <Typography>INFORMACION BASICA</Typography>
          </AccordionSummary>
          <br />
          <br />
          <Grid container style={{ padding: '20px' }}>
            <Grid className={classes.column} item xs={12} md={1}>
              <FormLabel component="legend">Tipo de Persona *</FormLabel>
              <Controller
                name="personaJuridicaNatural"
                control={formularioTipoActor.control}
                render={({ field }) => (
                  <RadioGroup
                    aria-label="personaJuridicaNatural"
                    name="personaJuridicaNatural"
                    value={field.value}
                    onChange={(event) => {
                      field.onChange(event.target.value);
                      estadoPersona(event.target.value);
                    }}
                  >
                    <FormControlLabel
                      value="natural"
                      control={<Radio color="primary" />}
                      label="Natural"
                      disabled
                    />
                    <FormControlLabel
                      value="juridica"
                      control={<Radio color="primary" />}
                      label="Juridica"
                      disabled
                    />
                  </RadioGroup>
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={3}>
              <Controller
                name="tipoDocumento"
                control={formularioTipoActor.control}
                rules={{ required: "Campo obligatorio" }}
                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHSelect
                    label="Tipo de Documento*"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    readOnly
                    helperText={error ? error.message : null}
                  >
                    {nuevoListadoIdenticacion?.map(
                      (option: any) => (
                        <MenuItem
                          key={option.id}
                          value={option.id}
                        >
                          {option.descripcion}
                        </MenuItem>
                      ),
                    )}
                  </CHSelect>
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="numeroDocumento"
                control={formularioTipoActor.control}
                rules={{
                  required: "Campo obligatorio",
                  minLength: {
                    value: 4,
                    message:
                      'La longitud mínima del campo es de 4 caracteres',
                  },
                  maxLength: {
                    value: 30,
                    message:
                      'La longitud máxima del campo es de 30 caracteres',
                  },
                }}
                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHTextField
                    error={!!error}
                    helperText={error ? error.message : null}
                    isReadOnly
                    numberMask
                    InputLabelProps={{ shrink: true }}
                    label="No. Documento*"
                    value={value}
                    onChange={onChange}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="nombre"
                control={formularioTipoActor.control}
                rules={{
                  required: "Campo obligatorio",
                  minLength: {
                    value: 4,
                    message:
                      'La longitud mínima del campo es de 4 caracteres',
                  },
                  maxLength: {
                    value: 30,
                    message:
                      'La longitud máxima del campo es de 30 caracteres',
                  },
                }}
                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHTextField
                    error={!!error}
                    helperText={error ? error.message : null}
                    isReadOnly
                    InputLabelProps={{ shrink: true }}
                    label="Nombre/Razon Social*"
                    value={value}
                    onChange={onChange}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="celular"
                rules={{
                  required: 'Este campo es requerido.',
                  pattern: /\(?([3-3]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                }}
                control={formularioTipoActor.control}
                render={({
                  field: { onChange, value },
                  fieldState: { error }
                }) => (
                  <CHTextField
                    error={!!error}
                    InputLabelProps={{ shrink: true }}
                    label="Celular*"
                    onChange={onChange}
                    value={value}
                    helperText={error ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos' : null}
                    cellPhoneMask
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="correoElectronico"
                control={formularioTipoActor.control}
                render={({
                  field: { onChange, value },
                }) => (
                  <CHTextField
                    isReadOnly={false}
                    InputLabelProps={{ shrink: true }}
                    label="Correo Electrónico"
                    value={value}
                    onChange={onChange}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="departamento"
                control={formularioTipoActor.control}

                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHAutoComplete
                    options={listaDepartamentos}
                    getOptionLabel={(option) => option?.nombre || ''}
                    value={nullDepartamentoValue ? null : departamentoValue}
                    onChange={(evento: any, departamento: any) => {
                      onChageDepartamento(evento, departamento)
                    }}
                    renderInput={(params) => (
                      <CHTextField
                        {...params}
                        label="Departamento"
                      />
                    )}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="ciudad"
                control={formularioTipoActor.control}
                defaultValue=""
                render={({ field: { onChange, value }, fieldState: { error } }) => (
                  <CHAutoComplete
                    options={listaDCiudades}
                    getOptionLabel={(option) => option?.nombre || ''}
                    onChange={(evento: any, ciudad: any) => {
                      onChageCiudad(evento, ciudad)
                    }}
                    value={nullCiudadValue ? null : ciudadValue}
                    renderInput={(params) => (
                      <CHTextField
                        {...params}
                        label="Ciudad"
                        error={!!error}
                      />
                    )}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="direccion"
                control={formularioTipoActor.control}
                defaultValue=""
                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHTextField
                    InputLabelProps={{ shrink: true }}
                    label="Dirección "
                    isReadOnly
                    value={value}
                    onClick={handleOpenAddress}
                    onChange={(onChange)}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="barrio"
                control={formularioTipoActor.control}
                render={({
                  field: { onChange, value },
                }) => (
                  <CHTextField
                    isReadOnly={false}
                    InputLabelProps={{ shrink: true }}
                    label="Barrio"
                    value={value}
                    onChange={onChange}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="telefono"
                rules={{
                  pattern: /\(?([6-6]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                }}
                control={formularioTipoActor.control}
                render={({
                  field: { onChange, value },
                  fieldState: { error }
                }) => (
                  <CHTextField
                    label="Teléfono"
                    InputLabelProps={{ shrink: true }}
                    onChange={onChange}
                    value={value}
                    error={!!error}
                    helperText={error ? 'El Teléfono debe comenzar por 6 y tener una longitud de 10 dígitos' : null}
                    cellPhoneMask
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="tipoActor"
                control={formularioTipoActor.control}
                rules={{ required: "Campo obligatorio" }}
                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHSelect
                    error={!!error}
                    helperText={
                      error
                        ? error.message
                        : null
                    }
                    onChange={onChange}
                    readOnly
                    value={value}
                    label="Tipo Actor*"
                  >
                    {listaTipoActores?.map(
                      (tipoActor: any) => (
                        <MenuItem
                          key={
                            tipoActor?.itA_ID
                          }
                          value={
                            tipoActor?.itA_ID
                          }
                        >
                          {
                            tipoActor?.itA_NOMBRE
                          }
                        </MenuItem>
                      ),
                    )}
                  </CHSelect>
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="tipoCuenta"
                control={formularioTipoActor.control}
                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHSelect
                    label="Tipo de Cuenta"
                    value={value}
                    readOnly
                    onChange={e => {
                      onChange(e);
                    }}
                  >
                    {listaTipoCuentas?.map(
                      (option: any) => {
                        const dato = `${option.idValor}_${option.descripcion}`;
                        return (
                          <MenuItem
                            key={option.idValor}
                            value={dato}
                          >
                            {option.descripcion}
                          </MenuItem>
                        );
                      },
                    )}
                  </CHSelect>

                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="numeroCuentaBancaria"
                control={formularioTipoActor.control}
                render={({
                  field: { onChange, value },
                }) => (
                  <CHTextField
                    isReadOnly
                    InputLabelProps={{ shrink: true }}
                    label="Número de Cuenta Bancaria"
                    value={value}
                    onChange={onChange}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="entidadBancaria"
                control={formularioTipoActor.control}
                render={({
                  field: { onChange, value },
                  fieldState: { error },
                }) => (
                  <CHAutoComplete
                    disabled
                    options={listaEntidadesBancarias}
                    getOptionLabel={(option: any) => option.nombreRazonSocial}
                    onChange={(evento: any, entidad: any) => {
                      onChageEntidad(evento, entidad)
                    }}
                    value={nullEntidadValue ? null : entidadValue}
                    renderInput={(params) => (
                      <CHTextField
                        {...params}
                        label="Entidad Bancaria"
                      />
                    )}
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={4}>
              <Controller
                name="fechaExpedicionCedula"
                control={formularioTipoActor.control}
                render={({
                  field: { onChange, value },
                }) => (
                  <CHDatePicker
                    InputLabelProps={{ shrink: true }}
                    label="Fecha de Expedición de la Cédula"
                    value={value}
                    onChange={onChange}
                    readOnly
                  />
                )}
              />
            </Grid>
            <Grid className={classes.column} item xs={12} md={1}>
              <FormLabel
                component="legend"
                style={{ textAlign: 'left', width: '100%' }}
              >
                Constructora
              </FormLabel>
              <Controller
                name="constructora"
                control={formularioTipoActor.control}
                render={({ field }) => (
                  <RadioGroup
                    style={{ display: 'flex', }}
                    aria-label="constructora"
                    name="constructora"
                    value={field.value}
                    onChange={(event) => {
                      field.onChange(event.target.value);
                      estadoConstructora(event.target.value);
                    }}
                  >
                    <FormControlLabel
                      value="si"
                      control={<Radio color="primary" />}
                      label="Si"
                      disabled
                    />
                    <FormControlLabel
                      value="no"
                      control={<Radio color="primary" />}
                      label="No"
                      disabled
                    />
                  </RadioGroup>
                )}
              />
            </Grid>
            {esTipoActorBeneficiarioPago && (
              <>
                <Grid
                  className={classes.column}
                  item
                  xs={12}
                  md={3}
                >
                  <Controller
                    name="tipoDocumentoAfiliado"
                    control={formularioTipoActor.control}
                    render={({
                      field: {
                        onChange,
                        value,
                      },
                    }) => (
                      <CHSelect
                        label="Tipo de Documento Afiliado"
                        value={value || ''}
                        onChange={onChange}
                        readOnly
                      >
                        {listaTipoIdentificacion.map(
                          (option: any) => (
                            <MenuItem
                              key={option.id}
                              value={option.id}
                            >
                              {option.descripcion}
                            </MenuItem>
                          ),
                        )}
                      </CHSelect>
                    )}
                  />
                </Grid>

                <Grid
                  className={classes.column}
                  item
                  xs={12}
                  md={4}
                >
                  <Controller
                    name="numeroDocumentoAfiliado"
                    control={
                      formularioTipoActor.control
                    }
                    render={({
                      field: {
                        onChange,
                        value,
                      },
                    }) => (
                      <CHTextField
                        InputLabelProps={{
                          shrink: true,
                        }}
                        label="No. Documento Afiliado"
                        value={value}
                        onChange={onChange}
                        isReadOnly
                        numberMask
                      />
                    )}
                  />
                </Grid>
              </>
            )}

            {mostrar && (
              <Grid className={classes.column} item xs={12} md={4}>
                <FormLabel component="legend">Convenio/Entidad</FormLabel>
                <Controller
                  name="convenioEntidad"
                  control={formularioTipoActor.control}
                  render={({ field }) => (
                    <RadioGroup
                      aria-label="convenioEntidad"
                      name="convenioEntidad"
                      value={field.value}
                      onChange={(event) => {
                        field.onChange(event.target.value)
                      }}
                    >
                      <FormControlLabel value="si"
                        control={<Radio color="primary" />}
                        label="Si"
                        disabled
                      />
                      <FormControlLabel
                        value="no"
                        control={<Radio color="primary" />}
                        label="No"
                        disabled
                      />
                    </RadioGroup>
                  )}
                />
              </Grid>
            )}
          </Grid>
          <div style={{ display: 'flex', justifyContent: 'flex-end', padding: '30px' }}>
            <Button
              className={classes.button}
              variant="outlined"
              onClick={formularioTipoActor.handleSubmit(actualizarTipoActor)}
            >
              Actualizar
            </Button>
          </div>
          <br />
          <br />
          <AddressComponentV2
            onOpen={open}
            onClose={onCloseAddress}
            tipoDireccion={1 === 1}
          />
        </Accordion >
        <Accordion  >
          <AccordionSummary
            expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
            aria-controls="panel1a-content"
            id="panel1a-header"
            className={classes.fiveAccordionTab}
          >
            <Typography>INFORMACION DE PAGO</Typography>
          </AccordionSummary>
          <AccordionDetails >
            <InformacionPagoIntervinientes
              datosLlenarTablaPagos={datosLlenarTablaPagos}
              listaEntidadesBancarias={listaEntidadesBancarias}
            />
          </AccordionDetails>
        </Accordion>
        <Accordion >
          <AccordionSummary
            expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
            aria-controls="panel1a-content"
            id="panel1a-header"
            className={classes.firstAccordionTab}
          >
            <Typography className={classes.heading}>LISTA DE TRÁMITES</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <TramitesIntervinientes
              listaTipoIdentificacion={listaTipoIdentificacion}
              data={informacionListaTramites}
            />
          </AccordionDetails>
        </Accordion>
        <Accordion >
          <AccordionSummary
            expandIcon={<ExpandMoreIcon style={{ color: '#b4e3f9' }} />}
            aria-controls="panel1a-content"
            id="panel1a-header"
            className={classes.firstAccordionTab}
          >
            <Typography className={classes.heading}>HISTORIAL DE CAMBIOS</Typography>
          </AccordionSummary>
          <AccordionDetails>
            <HistorialCambiosIntervinientes idInterviniente={informacionInterviniente?.itR_ID} />
          </AccordionDetails>
        </Accordion>
      </div>
      <br />
      <div className="buttons">
        <Button
          className={classes.button}
          variant="outlined"
          onClick={() => history.push('/GestionIntervinientes')}  >Regresar
        </Button>
      </div>
    </CHPrivateContainer>

  );
}

export default DetalleActulizacionesInterviniente;