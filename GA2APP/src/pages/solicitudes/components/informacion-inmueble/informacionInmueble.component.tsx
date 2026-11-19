import React, { useState, useEffect, useRef } from 'react';
import { toast } from 'react-toastify';
import { Search } from '@material-ui/icons';
import { Controller, useForm } from 'react-hook-form';
import {
  Button, createStyles, Grid, MenuItem, RadioGroup, Radio, FormLabel, makeStyles,
} from '@material-ui/core';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import { CITYDTO, DEPARTAMENTODTO, OFICINASREGISTRODTO } from '../../../../types/catalog.type';
import { Address } from '../../../../types/address.type';
import { setFormDataAddres } from '../../../../components/modals/address/constants/address.components.constants';
import AddressComponentV2 from '../../../../components/modals/address/addressV2.component'
import ApiSolicitudes from '../../../../services/api-solicitud.service';
import NotificationService from '../../../../services/notification.service';
import { MatriculaInmobiliariaModel } from '../../../../models/matricula-inmobiliaria.model';
import GenerarGuid from '../funciones/guid.funtions';
import CHCheckboxV2 from '../../../../components/ch-checkbox/CHCheckboxV2';
import { LoadingV2Component } from '../../../../components/loading/loadingV2.component';
import TablaMatriculasInformacionInmueble from './TablaMatriculasInformacionInmueble';

const useStyles = makeStyles(() =>
  createStyles({
    buttonAdd: {
      width: '200px',
      marginLeft: '.5rem',
    },
    columnButtonAdd: {
      textAlign: 'right',
    },
    column: {
      padding: '10px',
      textAlign: 'center'
    },
    columnLabel: {
      padding: '10px'
    }
  })
);

const InformacionInmueble = ({
  oficinasRegistro,
  listaMatriculas,
  listaDepartamentos,
  listaCiudades,
  listaOficinas,
  oficinaRegistro,
  matriculasAdicionadas,
  siEsAprobadaSolcitud,
  responseSolicitud,
  campoSoloLecturaRadicacionAprobada
}: {
  oficinasRegistro: OFICINASREGISTRODTO[],
  listaMatriculas: MatriculaInmobiliariaModel[];
  listaDepartamentos: DEPARTAMENTODTO[],
  listaCiudades: CITYDTO[],
  listaOficinas: OFICINASREGISTRODTO[],
  oficinaRegistro: OFICINASREGISTRODTO | null,
  siEsAprobadaSolcitud: boolean;
  matriculasAdicionadas: (matriculas: MatriculaInmobiliariaModel[]) => void;
  responseSolicitud: any;
  campoSoloLecturaRadicacionAprobada: any;
}) => {

  const classes = useStyles();
  const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR MATRÍCULA');
  const [oficinaRegistros, setOficinaRegistro] = useState<OFICINASREGISTRODTO | null>(null);
  const [ciudadRegistro, setCiudadRegistro] = useState<CITYDTO | null>(null);
  const [esBloqueada, setEsBloqueada] = useState(false);
  const apiSolicitud = ApiSolicitudes();
  const [readOnly, setReadOnly] = useState(true);
  const [departamentos, setDepartamentos] = useState<DEPARTAMENTODTO[]>([]);
  const [ciudadesFilter, setCiudadesFilter] = useState<CITYDTO[]>([]) || undefined;
  const [disabled, setDisabled] = useState(true);
  const [open, setOpen] = useState(false);
  const [mostrarConyuge, setMostrarConyuge] = useState(false);
  const [loading, setLoading] = useState(false)
  const [isLoading, setIsLoading] = useState(false)
  const [mostrarPrincipal, setMostrarPrincipal] = useState(false)
  const [matriculaAnterior, setmatriculaAnterior] = useState<string>('');
  const [idAnterior, setIdAnterior] = useState<string>('');
  const [matriculasInmobiliariasModel, setMatriculasInmobiliariasModel] = useState<MatriculaInmobiliariaModel[] | any[]>([]);
  const [mostrarColumnasConyuge, setMostrarColumnasConyuge] = useState(false)

  const matriculasInicializadasRef = useRef(false);
  const matriculasPrimeraRadicacionRef = useRef<Set<string>>(new Set());

  // useEffect(() => {
  //   console.log('responseSolicitud', responseSolicitud
  //   );

  // }, []);

  const obtenerClaveMatricula = (matricula: any) =>
    String(matricula?.id || matricula?.numeroMatricula || '');

  const esSegundaRadicacionRechazada =
    responseSolicitud?.soL_RECHAZADA === '1';

  const puedeEditarRegistro = (matricula: any) => {
    // En cualquier radicación normal se permite editar.
    if (!esSegundaRadicacionRechazada) {
      return true;
    }

    // En la segunda radicación rechazada, no permite editar
    // matrículas que ya venían de la primera radicación.
    return !matriculasPrimeraRadicacionRef.current.has(
      obtenerClaveMatricula(matricula),
    );
  };

  const matriculasControl = useForm({
    defaultValues: {
      id: '',
      departamentoInmueble: '',
      ciudadInmueble: '',
      numeroMatricula: '',
      direccion: '',
      esPrincipal: false,
      bloqueo: false,
      causalId: '',
      tipoOperacionId: '',
      numeroOficinaBucar: '',
      numeroMatriculaBuscar: '',
      oficinaInmueble: '',
      codigoOficina: '',
      propietario: '',
    },
  });

  const departamentoSeleccionado = matriculasControl.watch(
    'departamentoInmueble',
  );

  const conyugeFormulario = useForm({
    defaultValues: {
      tipoDocumento: '',
      documento: '',
      primerNombre: '',
      segundoNombre: '',
      primerApellido: '',
      segundoApellido: '',
      conyugeActivo: false
    },
  });

  useEffect(() => {
    if (
      matriculasInicializadasRef.current ||
      !Array.isArray(listaMatriculas)
    ) {
      return;
    }

    const matriculasIniciales = listaMatriculas.filter(
      (x: any) => x.numeroMatricula !== '-',
    );

    setMatriculasInmobiliariasModel(matriculasIniciales);

    matriculasPrimeraRadicacionRef.current = new Set(
      matriculasIniciales.map(obtenerClaveMatricula),
    );

    matriculasInicializadasRef.current = true;
  }, [listaMatriculas]);

  const agregarNuevoValor = () => {
    const esConyuge = matriculasControl.getValues('propietario');
    const existenConyugeActivos = matriculasInmobiliariasModel?.some((x: any) => x?.conyugeActivo === true);
    const conyugeActivoEnRespuesta = responseSolicitud?.informacionConyugeDto?.conyugeActivo === true;
    if ((esConyuge === '137' || esConyuge === '') && conyugeActivoEnRespuesta && existenConyugeActivos) {
      const nuevosValores = matriculasInmobiliariasModel.map(item => {
        if (item.conyugeActivo) {
          return {
            ...item,
            tipoDocumento: responseSolicitud?.informacionConyugeDto.nombreTipoIdentificacionConyuge,
            documento: responseSolicitud?.informacionConyugeDto.identificacionConyuge,
            primerNombre: responseSolicitud?.informacionConyugeDto.primerNombreConyuge,
            segundoNombre: responseSolicitud?.informacionConyugeDto.segundoNombreConyuge,
            primerApellido: responseSolicitud?.informacionConyugeDto.primerApellidoConyuge,
            segundoApellido: responseSolicitud?.informacionConyugeDto.segundoApellidoConyuge,
          };
        }
        return item;
      });
      setMostrarColumnasConyuge(true);
      setMatriculasInmobiliariasModel(nuevosValores);
    } else if ((esConyuge === '137' || esConyuge === '') && existenConyugeActivos) {
      setMostrarColumnasConyuge(true);
    }
    else {
      setMostrarColumnasConyuge(false);
    }
  }

  const esSubsidioSecuestradosDeudaHipotecaria =
    responseSolicitud?.descripcionProceso?.trim().toUpperCase() ===
    'SUBSIDIO SECUESTRADOS' &&
    responseSolicitud?.descripcionModalidad?.trim().toUpperCase() ===
    'ABONO O CANCELACIÓN DE DEUDA HIPOTECARIA';

  useEffect(() => {
    agregarNuevoValor()
  }, [matriculasInmobiliariasModel.length !== 0])

  useEffect(() => {
    if (responseSolicitud?.nombreTarea === "Radicación V 14") {
      setMostrarPrincipal(true)
    } else {
      setMostrarPrincipal(true)
    }
  }, [responseSolicitud])


  const handleOpenAddress = () => {
    setOpen(true);
  };

  const onCloseAddress = (value: Address) => {
    if (value) {
      const result = setFormDataAddres(value);
      matriculasControl.setValue('direccion', result);
    }
    setOpen(false);
  };


  const getCitys = (idDepartamento: any) => {
    const departamentoId = String(
      idDepartamento?.id || idDepartamento || '',
    );

    const ciudades = (listaCiudades || []).filter(
      ciudad => String(ciudad.idDepartamento) === departamentoId,
    );

    setCiudadesFilter(ciudades);
  };

  useEffect(() => {
    const departamentoId = String(departamentoSeleccionado || '');

    if (!departamentoId) {
      setCiudadesFilter([]);
      return;
    }

    const ciudadesFiltradas = (listaCiudades || []).filter(
      ciudad => String(ciudad.idDepartamento) === departamentoId,
    );

    setCiudadesFilter(ciudadesFiltradas);
  }, [listaCiudades, departamentoSeleccionado]);

  useEffect(() => {
    if (!esSubsidioSecuestradosDeudaHipotecaria) {
      return;
    }
  
    matriculasControl.setValue('propietario', '87');
  
    conyugeFormulario.reset({
      tipoDocumento: '',
      documento: '',
      primerNombre: '',
      segundoNombre: '',
      primerApellido: '',
      segundoApellido: '',
      conyugeActivo: false,
    });
  
    setMostrarConyuge(false);
    setMostrarColumnasConyuge(false);
  }, [
    esSubsidioSecuestradosDeudaHipotecaria,
    matriculasControl,
    conyugeFormulario,
  ]);

  const resetearMatricula = () => {
    setIsLoading(true)
    setTimeout(() => {
      setIsLoading(false)
      conyugeFormulario.setValue('conyugeActivo', false)
      matriculasControl.setValue('propietario', '87')
    }, 100);
    setDescEstadoBoton('AGREGAR MATRÍCULA');
    setOficinaRegistro(null);
    setCiudadRegistro(null);
    setEsBloqueada(false);
    matriculasControl.reset();
    conyugeFormulario.reset();
    setMostrarConyuge(false)
    conyugeFormulario.setValue('conyugeActivo', false)
    matriculasControl.setValue('propietario', '87')
    matriculasControl.clearErrors()
  };

  const onchangeOffice = (_: any, newValue: OFICINASREGISTRODTO) => {

    setCiudadRegistro(null);
    setOficinaRegistro(newValue);
    matriculasControl.setValue('oficinaInmueble', newValue?.idOficina.toString());
    matriculasControl.setValue('codigoOficina', newValue?.codigoOficina);
    matriculasControl.resetField('departamentoInmueble');

    const cundinamarca = newValue?.codigoDepartamento === 25
    if (cundinamarca && (newValue?.codigoOficina === "50N" || newValue?.codigoOficina === "50C" || newValue?.codigoOficina === "50S"))
      setDepartamentos(listaDepartamentos.filter(item => item.id === newValue?.codigoDepartamento ||
        item.id === 11));
    else {
      setDepartamentos(listaDepartamentos.filter(item => item.id === newValue?.codigoDepartamento));
    }
    setDisabled(false);
  };

  const buscarMatricula = async (codigomatricula: any, numeroMatricu: string) => {
    const numeroMatricula = `${codigomatricula}-${numeroMatricu}`
    await apiSolicitud
      .GetConsultarExisteMatricula(numeroMatricula)
      .then(async response => {
        if (response) {
          if (response?.isSuccess) {
            if (response?.data?.maiEstado) {

              setEsBloqueada(response?.data?.maiBloqueo);
              setReadOnly(true);

              const departamentoId = response?.data?.dpdId;

              const departamento = listaDepartamentos?.find(
                (item: any) => item?.id === departamentoId,
              );

              if (departamento) {
                setDepartamentos([departamento]);
              }

              matriculasControl.setValue(
                'departamentoInmueble',
                `${departamentoId || ''}`,
              );

              getCitys(departamentoId);
              matriculasControl.setValue('direccion', response?.data?.maiDireccion);
              if (response?.data?.maiNumeroMatricula?.indexOf('-') > 0) {
                const numMatricula = response?.data?.maiNumeroMatricula.split('-');
                matriculasControl.setValue('numeroMatricula', numMatricula[1]);
              } else {
                matriculasControl.setValue('numeroMatricula', response?.data?.maiNumeroMatricula);
              }
              const office = listaOficinas?.find(o => o.codigoOficina === response?.data?.ofcCodigo);
              if (office) {
                setOficinaRegistro(office);
                matriculasControl.setValue('oficinaInmueble', office?.idOficina.toString());
                matriculasControl.setValue('codigoOficina', office?.codigoOficina);
              }
              const ciudadInmueble = listaCiudades.find(x => x.id.toString() === response?.data?.dpcId.toString());
              setCiudadRegistro(ciudadInmueble as CITYDTO);
              matriculasControl.setValue('ciudadInmueble', `${response?.data?.dpcId}_${response?.data?.dpcNombre}`);
              setDisabled(true);
            } else {
              NotificationService.genericAlwarning(
                'Alerta',
                `Advertencia: Matricula inactiva`,
                'warning')
            }
          } else {
            NotificationService.genericAlwarning(
              'Alerta',
              `No existe la matricula, diligencie los campos`,
              'warning')
            matriculasControl.setValue('oficinaInmueble', '');
            setCiudadRegistro(null);
            setOficinaRegistro(null);
            resetearMatricula();
            setReadOnly(false);
            setDisabled(false);
          }
        }
      })
      .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });
      });
  };

  const verificarSiPrincipal = async (data: any) => {
    if (responseSolicitud?.nombreTarea === "Radicación V 14" ||
      responseSolicitud?.timCodigo === "LIBERGRAV" ||
      responseSolicitud?.timCodigo === 'REPARVIV' ||
      responseSolicitud?.timCodigo === 'CONSNUEVA' ||
      responseSolicitud?.timCodigo === 'PAGOPREDIAL' ||
      responseSolicitud?.timCodigo === "ADQUITER") {
      const datoEncontrado = matriculasInmobiliariasModel.findIndex(
        x =>
          x.id === data.id &&
          x.id !== matriculaAnterior,
      );
      if (datoEncontrado === -1) {
        const existePrincipal = matriculasInmobiliariasModel.some(
          x =>
            x.esPrincipal === true &&
            x.id !== matriculaAnterior,
        );

        if ((datoEncontrado && descEstadoBoton === 'CORREGIR DATO')) {
          agregarMatricula(data);
        }
        else if (!existePrincipal && data.esPrincipal) {
          agregarMatricula(data);
        } else if (!matriculasInmobiliariasModel.length && data.esPrincipal) {
          agregarMatricula(data);
        } else if (responseSolicitud?.descripcionProceso === "RETIRO PARCIAL DE CESANTÍAS" &&
          matriculasInmobiliariasModel.length === 1 &&
          (responseSolicitud?.timCodigo === 'REPARVIV' ||
            responseSolicitud?.timCodigo === 'LIBERGRAV' ||
            responseSolicitud?.timCodigo === 'ADQUITER' ||
            responseSolicitud?.timCodigo === 'CONSNUEVA')) {
          toast('Para esta solicitud solo se puede ingresar una Matrícula Inmobiliaria', { type: toast.TYPE.ERROR });
        } else if (matriculasInmobiliariasModel.length === 1 && !data.esPrincipal) {
          toast('Solo se puede ingresar una matrícula y debe ser principal', { type: toast.TYPE.ERROR });
        } else if (!data.esPrincipal) {
          toast('La matrícula debe ser principal', { type: toast.TYPE.ERROR });
        } else {
          toast('Ya existe una matrícula como principal', { type: toast.TYPE.ERROR });
        }
      } else {
        toast('El número de matrícula ya existe en la lista', { type: toast.TYPE.ERROR });
      }
      setLoading(false);
    }
    else {
      const datoEncontrado = matriculasInmobiliariasModel.findIndex(
        x =>
          x.numeroMatricula.split('-')[1] === data.numeroMatricula &&
          x.numeroMatricula !== matriculaAnterior,
      );
      if (datoEncontrado === -1) {
        const existePrincipal = matriculasInmobiliariasModel.findIndex(
          x =>
            x.esPrincipal === true &&
            data.esPrincipal === true &&
            x.numeroMatricula !== matriculaAnterior,
        );
        if (existePrincipal === -1 || descEstadoBoton === 'CORREGIR DATO') {
          if (matriculasInmobiliariasModel.length === 0) {
            if (data.esPrincipal === false) {
              toast(
                'El primer registro debe ser matrícula principal',
                { type: toast.TYPE.ERROR },
              );
              setLoading(false)
            } else {
              agregarMatricula(data);
              setLoading(false)
            }
          } else {
            agregarMatricula(data);
            setLoading(false)
          }
        } else {
          toast('Ya existe una matrícula como principal', {
            type: toast.TYPE.ERROR,
          });
          setLoading(false)
        }
      } else {
        toast('El número de matrícula ya existe en la lista', {
          type: toast.TYPE.ERROR,
        });
        setLoading(false)
      }
    }
  };

  const agregarMatricula = async (data: any) => {
    const ciudad = data.ciudadInmueble.split('_')

    const nombreDepartamento = listaDepartamentos?.find((item: DEPARTAMENTODTO) => item.id === Number(data.departamentoInmueble));
    const nombreCiudad = listaCiudades.find(item => item.id === Number(data.ciudadInmueble));
    const guid = GenerarGuid();
    const esConyuge =
      matriculasControl.getValues('propietario') === '137' &&
      !esSubsidioSecuestradosDeudaHipotecaria;
    const matriculaNueva: any = {
      id: guid.newGuid(),
      esPrincipal: data.esPrincipal,
      numeroMatricula: `${data.codigoOficina}-${data.numeroMatricula}`,
      direccion: data.direccion,
      ciudadId: Number(ciudad[0]),
      oficinaRegistroId: Number(data.oficinaInmueble),
      departamentoInmueble: {
        id: data.departamentoInmueble,
        descripcion: nombreDepartamento?.nombre
      },
      ciudadInmueble: {
        id: Number(ciudad[0]),
        descripcion: ciudad[1] || nombreCiudad?.nombre
      },
      departamentoId: Number(data.departamentoInmueble),
      bloqueo: false,
      causalId: '98efdd13-2459-42fc-8787-6034f9935c35',
      tipoOperacionId: '38d1812e-eff8-48ea-9cc7-cdd0c159d0dd',
      esBloqueada: esBloqueada,
      tipoDocumento: esConyuge ? conyugeFormulario.getValues('tipoDocumento') : '',
      documento: esConyuge ? conyugeFormulario.getValues('documento') : '',
      primerNombre: esConyuge ? conyugeFormulario.getValues('primerNombre') : '',
      segundoNombre: esConyuge ? conyugeFormulario.getValues('segundoNombre') : '',
      primerApellido: esConyuge ? conyugeFormulario.getValues('primerApellido') : '',
      segundoApellido: esConyuge ? conyugeFormulario.getValues('segundoApellido') : '',
      conyugeActivo: esConyuge
    };
    let matriculas: MatriculaInmobiliariaModel[] = [];
    if (descEstadoBoton === 'AGREGAR MATRÍCULA') {
      setMatriculasInmobiliariasModel(oldArray => [
        ...oldArray,
        matriculaNueva,
      ]);
      matriculas = [...matriculasInmobiliariasModel];
      matriculas.push(matriculaNueva);
    } else {
      const temp = [...matriculasInmobiliariasModel];
      const posicionActual = temp.findIndex(
        x => x.id === matriculaAnterior,
      );
      temp.splice(posicionActual, 1);
      matriculaNueva.id = idAnterior;
      temp.splice(posicionActual, 0, matriculaNueva);
      matriculas = temp;
      setMatriculasInmobiliariasModel(temp);
    }
    matriculasAdicionadas(matriculas);
    setmatriculaAnterior('');
    resetearMatricula();
  };

  const limpiarDatosConyuge = () => {
    conyugeFormulario.reset({
      tipoDocumento: '',
      documento: '',
      primerNombre: '',
      segundoNombre: '',
      primerApellido: '',
      segundoApellido: '',
      conyugeActivo: false,
    });

    setMostrarConyuge(false);
  };

  const onChangePropietarios = async (item: any) => {
    if (
      esSubsidioSecuestradosDeudaHipotecaria &&
      item === '137'
    ) {
      matriculasControl.setValue('propietario', '87');
      limpiarDatosConyuge();
      setMostrarColumnasConyuge(false);
      return;
    }
    if (item === '137') {
      setLoading(true)
      setMostrarColumnasConyuge(true)
      setMostrarConyuge(true)
      conyugeFormulario.setValue('conyugeActivo', true)
      const request = {
        tipoIdentificacion: responseSolicitud?.tipoIdentificacion,
        identificacion: responseSolicitud?.identificacionCliente,
      }
      await apiSolicitud.PostInfoConyuge(request)
        .then(async response => {
          if (response) {
            if (response?.returnMessage === "El afiliado no tiene conyuge registrado") {
              await NotificationService.genericAlwarning(
                'Advertencia',
                'No tiene cónyuge.',
                'warning'
              );
              matriculasControl.setValue('propietario', '87')
              conyugeFormulario.setValue('conyugeActivo', false)
              setMostrarColumnasConyuge(false)
              setMostrarConyuge(false)
              setLoading(false)
            }
            else if (response?.isSuccess
              && response?.data?.identificacionConyuge
              && response?.data?.primerNombreConyuge
              && response?.data?.primerApellidoConyuge) {

              conyugeFormulario.setValue('tipoDocumento', response?.data?.nombreTipoIdentificacionConyuge)
              conyugeFormulario.setValue('documento', response?.data?.identificacionConyuge)
              conyugeFormulario.setValue('primerNombre', response?.data?.primerNombreConyuge)
              conyugeFormulario.setValue('segundoNombre', response?.data?.segundoNombreConyuge)
              conyugeFormulario.setValue('primerApellido', response?.data?.primerApellidoConyuge)
              conyugeFormulario.setValue('segundoApellido', response?.data?.segundoApellidoConyuge)
              matriculasControl.setValue('propietario', '137')
              conyugeFormulario.setValue('conyugeActivo', response?.data?.conyugeActivo)
              setLoading(false)
            } else {
              matriculasControl.setValue('propietario', '87');

              conyugeFormulario.reset({
                tipoDocumento: '',
                documento: '',
                primerNombre: '',
                segundoNombre: '',
                primerApellido: '',
                segundoApellido: '',
                conyugeActivo: false,
              });

              setMostrarConyuge(false);

              const existenConyugeActivos = matriculasInmobiliariasModel?.some(
                (x: any) => x?.conyugeActivo === true
              );

              setMostrarColumnasConyuge(existenConyugeActivos);
            }
          }
        })
        .catch(() => {
          toast('Error al consultar información conyugé', { type: toast.TYPE.ERROR });
          setLoading(false)
        });
    } else {
      setMostrarConyuge(false)
    }
  }

  const onChangeCiudad = (_: any, newValue: CITYDTO) => {
    matriculasControl.setValue('ciudadInmueble', `${newValue?.id.toString()}_${newValue?.nombre}`);
    setCiudadRegistro(newValue);
  };

  const seleccionarRegistro = async (item: any) => {

    if (!puedeEditarRegistro(item)) {
      toast(
        'Esta matrícula fue registrada en la primera radicación y no puede modificarse. Puede eliminarla y crear una nueva.',
        { type: toast.TYPE.WARNING },
      );
      return;
    }

    const departamentoId =
      item?.departamentoId || item?.departamentoInmueble?.id;

    setIdAnterior(item.id);
    getCitys(item.departamentoId);
    if (item?.numeroMatricula?.indexOf('-') > 0) {
      const numMatricula = item.numeroMatricula.split('-');
      matriculasControl.setValue('numeroMatricula', numMatricula[1]);

      const office = listaOficinas?.find(o => o.codigoOficina === numMatricula[0]);
      if (office) {
        setOficinaRegistro(office);
        matriculasControl.setValue('oficinaInmueble', office?.idOficina.toString());
        matriculasControl.setValue('codigoOficina', office?.codigoOficina);
      }
    } else {
      matriculasControl.setValue('numeroMatricula', item.numeroMatricula);
    }

    matriculasControl.setValue('direccion', item.direccion);
    matriculasControl.setValue('id', item.id);
    matriculasControl.setValue('esPrincipal', item.esPrincipal);
    matriculasControl.setValue(
      'departamentoInmueble',
      departamentoId?.toString() || '',
    );
    matriculasControl.setValue('ciudadInmueble', `${item.ciudadId}_${item.ciudadInmueble?.descripcion}`);

    const ciudad = listaCiudades.find(x => x.id.toString() === item.ciudadId.toString());
    setCiudadRegistro(ciudad as CITYDTO);

    setDescEstadoBoton('CORREGIR DATO');
    setmatriculaAnterior(item.id);

    // NUEVO: cargar o limpiar datos según sea Cónyuge o Afiliado
    const puedeAsignarConyuge =
      item?.conyugeActivo === true &&
      !esSubsidioSecuestradosDeudaHipotecaria;

    if (puedeAsignarConyuge) {
      matriculasControl.setValue('propietario', '137');
      setMostrarConyuge(true);
      setMostrarColumnasConyuge(true);

      conyugeFormulario.setValue('conyugeActivo', true);
      conyugeFormulario.setValue(
        'tipoDocumento',
        item?.tipoDocumento || '',
      );
      conyugeFormulario.setValue(
        'documento',
        item?.documento || '',
      );
      conyugeFormulario.setValue(
        'primerNombre',
        item?.primerNombre || '',
      );
      conyugeFormulario.setValue(
        'segundoNombre',
        item?.segundoNombre || '',
      );
      conyugeFormulario.setValue(
        'primerApellido',
        item?.primerApellido || '',
      );
      conyugeFormulario.setValue(
        'segundoApellido',
        item?.segundoApellido || '',
      );
    } else {
      matriculasControl.setValue('propietario', '87');

      conyugeFormulario.reset({
        tipoDocumento: '',
        documento: '',
        primerNombre: '',
        segundoNombre: '',
        primerApellido: '',
        segundoApellido: '',
        conyugeActivo: false,
      });

      setMostrarConyuge(false);

      const existenConyugeActivos =
        !esSubsidioSecuestradosDeudaHipotecaria &&
        matriculasInmobiliariasModel?.some(
          (x: any) =>
            x?.conyugeActivo === true &&
            x?.id !== item?.id,
        );

      setMostrarColumnasConyuge(existenConyugeActivos);
    }
  };

  const eliminarRegistro = async (item: string) => {
    const confirmAlert = await NotificationService.confirmAlert(
      '¿Eliminando registro?',
    );
    if (confirmAlert) {
      const temp = [...matriculasInmobiliariasModel];
      temp.splice(
        temp.findIndex(x => x.numeroMatricula === item),
        1,
      );
      resetearMatricula();
      setMatriculasInmobiliariasModel(temp);

      matriculasAdicionadas(temp);
    }
  };

  const formularioMatriculaBloqueado =
    (
      (
        siEsAprobadaSolcitud ||
        responseSolicitud?.secciones?.includes('INCONSISTENCIA')
      ) &&
      responseSolicitud?.soL_RECHAZADA !== '1'
    ) ||
    campoSoloLecturaRadicacionAprobada;

  if (loading) {
    return <LoadingV2Component key="loader" loading={loading} tittle='Validando cónyuge' />
  }

  if (isLoading) {
    return <LoadingV2Component key="loader" loading={isLoading} tittle='...' />
  }

  return (
    <div>
      <CHPaper title='Información inmueble'>
        {
          formularioMatriculaBloqueado ?
            '' :
            <Grid container>
              <Grid className={classes.column} item xs={12} md={1}>
                <Controller
                  name="numeroOficinaBucar"
                  control={matriculasControl.control}
                  defaultValue=""
                  render={({ field }) => (
                    <CHTextField
                      {...field}
                      type="text"
                      placeholder="Ejemplo: 123"
                      value={field.value.toUpperCase() || ''}
                      onChange={({ target: { value } }) => {
                        if (value.length <= 3) {
                          field.onChange(value);
                        }
                        if (value.length === 3) {
                          const nextField = document.getElementById('numeroMatriculaBuscar');
                          if (nextField) {
                            nextField.focus();
                          }
                        }
                      }}
                    />
                  )}
                />
              </Grid>
              <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                  name="numeroMatriculaBuscar"
                  control={matriculasControl.control}
                  render={({ field }) => (
                    <CHTextField
                      {...field}
                      id="numeroMatriculaBuscar"
                      type="text"
                      placeholder="Ejemplo: 456789..."
                      inputProps={{ maxLength: 12 }}
                      onChange={e => {
                        const value = e.target.value.replace(/\D/g, '');
                        field.onChange(value);
                      }}
                    />
                  )}
                />
              </Grid>
              <Button
                aria-label="Eliminar"
                color="secondary"
                onClick={() => {
                  buscarMatricula(matriculasControl.getValues('numeroOficinaBucar'), matriculasControl.getValues('numeroMatriculaBuscar'));
                }}
              >
                <Search />
              </Button>
            </Grid>
        }

        {
          formularioMatriculaBloqueado ?
            null :
            <Grid container>
              <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                  name='oficinaInmueble'
                  rules={{
                    required: 'Este campo es requerido',
                  }}
                  control={matriculasControl.control}
                  render={(
                    {
                      fieldState: { error },
                    }) => (
                    <CHAutoComplete
                      value={oficinaRegistros === null ? oficinaRegistro : oficinaRegistros}
                      options={oficinasRegistro}
                      getOptionLabel={option => option?.descripcionOficina}
                      onChange={onchangeOffice}
                      disabled={siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA') || readOnly}
                      renderInput={params =>
                        <CHTextField
                          {...params}
                          label='Oficina del inmueble'
                          InputLabelProps={{ shrink: true }}
                          error={!!error}
                          required
                          isReadOnly={formularioMatriculaBloqueado || readOnly}
                          helperText={
                            error ? error.message : null
                          }
                        />
                      }
                    />
                  )}
                />
              </Grid>
              <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                  name='codigoOficina'
                  control={matriculasControl.control}
                  rules={{
                    required: 'Este campo es requerido.',
                  }}
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHTextField
                      error={!!error}
                      helperText={error ? error.message : null}
                      isReadOnly
                      required
                      label='Código oficina'
                      value={value}
                      onChange={onChange}
                      InputLabelProps={{ shrink: true }}
                    />
                  )}
                />
              </Grid>
              <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                  name='numeroMatricula'
                  control={matriculasControl.control}
                  rules={{
                    required: 'Este campo es requerido.',
                  }}
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHTextField
                      onCopy={e => e.preventDefault()}
                      onPaste={e => e.preventDefault()}
                      onCut={e => e.preventDefault()}
                      error={!!error}
                      isReadOnly={formularioMatriculaBloqueado || readOnly}
                      helperText={error ? error.message : null}
                      inputProps={{
                        maxLength: 11,
                      }}
                      label="Matrícula Inmobiliaria"
                      value={value}
                      required
                      InputLabelProps={{ shrink: true }}
                      onChange={e => {
                        onChange(
                          e.target.value.replace(
                            /[^A-Z\d]/g,
                            '',
                          ),
                        );
                      }}
                    />
                  )}
                />
              </Grid>
              <Grid className={classes.column} item xs={12} md={6} lg={3}>
                <Controller
                  name="direccion"
                  control={matriculasControl.control}
                  rules={{
                    required: 'Este campo es requerido.',
                  }}
                  defaultValue=""
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHTextField
                      InputLabelProps={{ shrink: true }}
                      label="Dirección "
                      error={!!error}
                      helperText={
                        error ? error.message : null
                      }
                      isReadOnly
                      disabled={disabled}
                      value={value}
                      onClick={handleOpenAddress}
                      onChange={(onChange)}
                    />

                  )}
                />
              </Grid>
              <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                  name='departamentoInmueble'
                  control={matriculasControl.control}
                  rules={{
                    required: 'Este campo es requerido.',
                  }}
                  render={({
                    field: { onChange, value },
                    fieldState: { error },
                  }) => (
                    <CHSelect
                      error={!!error}
                      label="Departamento del Inmueble *"
                      value={value}
                      required
                      readOnly={readOnly}
                      disabled={disabled}
                      helperText={error ? error.message : null}
                      onChange={value => {
                        if (value.target.value !== 0) {
                          getCitys(value.target.value);
                        }
                        onChange(value);
                        setCiudadRegistro(null);
                        matriculasControl.setValue('ciudadInmueble', '');
                      }}
                    >
                      {departamentos?.map(
                        (option: DEPARTAMENTODTO) => (
                          <MenuItem
                            key={option.id}
                            value={option.id}
                          >
                            {option.nombre}
                          </MenuItem>
                        ),
                      )}
                    </CHSelect>
                  )}
                />
              </Grid>
              <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                  name="ciudadInmueble"
                  control={matriculasControl.control}
                  rules={{
                    required: 'Este campo es requerido',
                  }}
                  render={(
                    {
                      fieldState: { error },
                    }) => (
                    <CHAutoComplete
                      value={ciudadRegistro}
                      options={ciudadesFilter}
                      getOptionLabel={option => option?.nombre}
                      onChange={onChangeCiudad}
                      disabled={readOnly}
                      renderInput={(params) =>
                        <CHTextField
                          {...params}
                          error={!!error}
                          isReadOnly={readOnly}
                          InputLabelProps={{ shrink: true }}
                          helperText={error ? error.message : null}
                          required
                          label="Ciudad del Inmueble"
                        />
                      }
                    />
                  )}
                />
              </Grid>
              {mostrarConyuge ?
                <>
                  <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                      name="tipoDocumento"
                      control={conyugeFormulario.control}
                      rules={{
                        required: 'Este campo es requerido',
                      }}
                      render={(
                        {
                          field: { value },
                          fieldState: { error },
                        }) => (
                        <CHTextField
                          error={!!error}
                          value={value}
                          InputLabelProps={{ shrink: true }}
                          isReadOnly={formularioMatriculaBloqueado || readOnly}
                          helperText={error ? error.message : null}
                          required
                          label="Tipo Documento"
                        />
                      )}
                    />
                  </Grid>
                  <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                      name="documento"
                      control={conyugeFormulario.control}
                      rules={{
                        required: 'Este campo es requerido',
                      }}
                      render={(
                        {
                          field: { value },
                          fieldState: { error },
                        }) => (
                        <CHTextField
                          InputLabelProps={{ shrink: true }}
                          error={!!error}
                          isReadOnly={formularioMatriculaBloqueado || readOnly}
                          helperText={error ? error.message : null}
                          value={value}
                          required
                          label="Documento"
                        />
                      )}
                    />
                  </Grid>
                  <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                      name="primerNombre"
                      control={conyugeFormulario.control}
                      rules={{
                        required: 'Este campo es requerido',
                      }}
                      render={(
                        {
                          field: { value },
                          fieldState: { error },
                        }) => (
                        <CHTextField
                          error={!!error}
                          isReadOnly={formularioMatriculaBloqueado || readOnly}
                          helperText={error ? error.message : null}
                          value={value}
                          required
                          label="Primer Nombre Conyuge"
                          InputLabelProps={{ shrink: true }}
                        />
                      )}
                    />
                  </Grid>
                  <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                      name="segundoNombre"
                      control={conyugeFormulario.control}
                      rules={{
                        required: 'Este campo es requerido',
                      }}
                      render={(
                        {
                          field: { value },
                          fieldState: { error },
                        }) => (

                        <CHTextField
                          error={!!error}
                          isReadOnly={formularioMatriculaBloqueado || readOnly}
                          helperText={error ? error.message : null}
                          value={value}
                          label="Segundo Nombre Conyuge"
                          InputLabelProps={{ shrink: true }}
                        />
                      )}
                    />
                  </Grid>
                  <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                      name="primerApellido"
                      control={conyugeFormulario.control}
                      rules={{
                        required: 'Este campo es requerido',
                      }}
                      render={(
                        {
                          field: { value },
                          fieldState: { error },
                        }) => (
                        <CHTextField
                          error={!!error}
                          isReadOnly={formularioMatriculaBloqueado || readOnly}
                          helperText={error ? error.message : null}
                          value={value}
                          required
                          label="Primer Apellido Conyuge"
                          InputLabelProps={{ shrink: true }}
                        />
                      )}
                    />
                  </Grid>
                  <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                      name="segundoApellido"
                      control={conyugeFormulario.control}
                      rules={{
                        required: 'Este campo es requerido',
                      }}
                      render={(
                        {
                          field: { value },
                          fieldState: { error },
                        }) => (
                        <CHTextField
                          error={!!error}
                          isReadOnly={formularioMatriculaBloqueado || readOnly}
                          helperText={error ? error.message : null}
                          value={value}
                          label="segundo Apellido Conyuge"
                          InputLabelProps={{ shrink: true }}
                        />
                      )}
                    />
                  </Grid>
                </> : null
              }
              <Grid container>
                <Grid className={classes.columnLabel} item xs={12} md={3}>
                  <FormLabel component='legend'>Propietario</FormLabel>
                  <Controller
                    name='propietario'
                    control={matriculasControl.control}
                    render={({ field }) => (
                      <RadioGroup
                        value={field.value || '87'}
                        onChange={(event) => {
                          const id = event.target.value as string;

                          field.onChange(id);
                          matriculasControl.setValue('propietario', id);

                          onChangePropietarios(id);
                        }}
                        row
                        aria-label='propietario'
                        name='row-radio-buttons-group'
                      >
                        <FormControlLabel
                          value='87'
                          control={<Radio />}
                          label='Afiliado'
                          disabled={formularioMatriculaBloqueado}
                        />

                        <FormControlLabel
                          value='137'
                          control={<Radio />}
                          label='Cónyuge'
                          disabled={
                            formularioMatriculaBloqueado ||
                            esSubsidioSecuestradosDeudaHipotecaria
                          }
                        />
                      </RadioGroup>
                    )}
                  />
                </Grid>
              </Grid>
              {
                mostrarPrincipal ? <Grid className={classes.columnLabel} item xs={12} md={6} lg={3}>
                  <Controller
                    name="esPrincipal"
                    control={matriculasControl.control}
                    render={({ field: { onChange, value } }) => (
                      <CHCheckboxV2
                        label="Principal / Relacionado"
                        onChange={onChange}
                        checked={value}
                      />
                    )}
                  />
                </Grid> : null
              }
              <Grid container>
                <Grid
                  className={classes.columnButtonAdd}
                  item
                  xs={12}
                  md={12}>
                  <Button
                    className={classes.buttonAdd}
                    variant="outlined"
                    onClick={matriculasControl.handleSubmit(
                      verificarSiPrincipal,
                    )}
                  >
                    {descEstadoBoton}
                  </Button>

                  <Button
                    className={classes.buttonAdd}
                    variant="outlined"
                    onClick={resetearMatricula}
                  >
                    Cancelar
                  </Button>
                </Grid>
              </Grid>
            </Grid>
        }
        <AddressComponentV2
          onOpen={open}
          onClose={onCloseAddress}
          tipoDireccion={1 === 1}
        />
        <TablaMatriculasInformacionInmueble
          mostrarColumnasConyuge={mostrarColumnasConyuge}
          matriculasInmobiliariasModel={matriculasInmobiliariasModel}
          siEsAprobadaSolcitud={siEsAprobadaSolcitud}
          responseSolicitud={responseSolicitud}
          campoSoloLecturaRadicacionAprobada={campoSoloLecturaRadicacionAprobada}
          seleccionarRegistro={seleccionarRegistro}
          eliminarRegistro={eliminarRegistro}
          puedeEditarRegistro={puedeEditarRegistro}
        />
      </CHPaper>
    </div>
  )
}
export default InformacionInmueble;