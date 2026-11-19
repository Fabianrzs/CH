import React, { useEffect } from "react";
import { useDispatch, useSelector } from 'react-redux';
import { Container, Grid, MenuItem } from "@material-ui/core";
import { Controller, useForm } from "react-hook-form";

import { AppState } from "../../store/rootReducer";

import {
  startGetAllTypeRequest,
  startGetAllTypeModality
} from '../../actions/catalog.action';

import CHPageTitleV2 from "../../components/ch-page-title/CHPageTitleV2";
import CHPrivateContainer from "../../components/ch-private-container/CHPrivateContainer";
import TituloIcono from "../AdministracionCuentas/components/titulo-icono/tituloIcono.component";
import CHSelect from "../../components/ch-select/CHSelect";
import { TYPEMODALITY, TYPEREQUEST } from "../../types/catalog.type";

const Solicitudes = () => {
  const dispatch = useDispatch();
  const catalogState = useSelector((state: AppState) => state.catalog);

  useEffect(() => {
    dispatch(startGetAllTypeRequest(2))
  }, []);

  const {
    control,
    setValue,
    register,
  } = useForm({
    defaultValues: {
      tipoDocumento: '',
      numeroDocumento: '',
      typerequest: '',
      typemodality: '',
      typesubmodality: '',
    },
  });

  return (
    <CHPrivateContainer>
      <CHPageTitleV2
        topText="Parametrización de solicitudes"
        title1="Parametrización"
        title2="Solicitudes"
        icono="settings"
      />
      <Container>
        <Grid container justifyContent="space-around">
          <Grid item xs={3}>
            <TituloIcono
              title1="Tipo"
              subtitle="de solicitud"
              icono="event_available"
            />
          </Grid>
          <Grid item xs={6}>
            <Controller
              name="typerequest"
              control={control}
              rules={{
                required:
                  'Este campo es requerido.',
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHSelect
                  error={!!error}
                  label="TIPO SOLICITUD"
                  value={value}
                  helperText={
                    error ? error.message : null
                  }
                  onChange={value => {
                    const idTipoSolicitud = value.target.value as string;
                    dispatch(startGetAllTypeModality(idTipoSolicitud));
                    setValue('typemodality', '');
                    register('typemodality', {
                      required:
                        'Este campo es requerido.',
                    });
                    onChange(value);
                  }}
                >
                  {catalogState.typeRequest?.map(
                    (option: TYPEREQUEST) => (
                      <MenuItem
                        key={option.idTipoSolicitud}
                        value={option.idTipoSolicitud}
                      >
                        {
                          option.descripcionTipoSolicitud
                        }
                      </MenuItem>
                    ),
                  )}
                </CHSelect>
              )}
            />
          </Grid>
        </Grid>
        <Grid container justifyContent="space-around">
          <Grid item xs={3}>
            <TituloIcono
              title1="Modalidad"
              subtitle=""
              icono="today"
            />
          </Grid>
          <Grid item xs={6}>
            <Controller
              name="typemodality"
              control={control}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHSelect
                  error={!!error}
                  label="TIPO MODALIDAD"
                  value={value}
                  helperText={
                    error ? error.message : null
                  }
                  onChange={value => {
                    // const dato = value.target
                    //   .value as string;
                    // const codigoModalidad =
                    //   dato.split('_')[0];
                    // const idTipoModalidad =
                    //   dato.split('_')[1];
                    // setModalidadData(
                    //   codigoModalidad,
                    // );
                    // startSubModality(
                    //   idTipoModalidad.toString(),
                    // );
                    // setValue(
                    //   'typesubmodality',
                    //   '',
                    // );
                    // register(
                    //   'typesubmodality',
                    //   {
                    //     required:
                    //       'Este campo es requerido.',
                    //   },
                    // );
                    onChange(value);
                  }}
                  disabled={catalogState.typeModality?.length === undefined || catalogState.typeModality?.length === 0}
                >
                  {catalogState.typeModality?.map(
                    (option: TYPEMODALITY) => {
                      const dato = `${option.codigoModalidad}_${option.idTipoModalidad}`;
                      return (
                        <MenuItem
                          key={option.idTipoModalidad}
                          value={dato}
                        >
                          {
                            option.descripcionTipoModalidad
                          }
                        </MenuItem>
                      );
                    },
                  )}
                </CHSelect>
              )}
            />
          </Grid>
        </Grid>
      </Container>
    </CHPrivateContainer>
  );
}

export default Solicitudes;

/**
 * Retiro parcial cesantias
 * - VINUEVA
 * -- Component: CompraViviendaNueva - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - PropietarioMatricula - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK
 * - VIUSADA
 * -- Component: CompraViviendaUsada - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - PropietarioMatricula - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK
 * - CONSNUEVA
 * -- Component: ConstruccionLotePropio - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - InformacionInmueble - InformacionConyugue - TerceroApoderado - TerceroBeneficiario - ObservacionesSolicitud - OK
 * - REPARVIV
 * -- Component: AmpliacionReparacionVivienda - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - InformacionInmueble - InformacionConyugue - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK -- ACTUALIZADO
 * - LIBERGRAV
 * -- Component: LiberacionGravamenes - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - InformacionInmueble - InformacionConyugue - TerceroBeneficiarioPagosVu - TerceroApoderado - ObservacionesSolicitud - OK -- ACTUALIZADO
 * - REPAES
 * -- Component: RetiroParcialParaEstudio - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - EntidadEducativa - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK -- ACTUALIZADO
 * - AHOPROEDU
 * -- Component: AhorroProgramadoSeguroEducativo - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - TerceroEntidadSeguroEducativo - TerceroBeneficiarioPagosVu - TerceroApoderado - TerceroBeneficiarioEstudio - ObservacionesSolicitud - OK -- ACTUALIZADO
 * - CAEXLEHA
 * -- Component: CanonesLeasingHabitacional - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK -- ACTUALIZADO
 * - READACPROES
 * -- Component: RetiroParaAdquirirAccionesEstado - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - TerceroTenedorAcciones - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK -- ACTUALIZADO
 * - ADQUITER
 * -- Component: AdquisicionTerrenoLote - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - ComponentHarcodeReview=InformacionInmueble - TerceroVendedor - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK -- ACTUALIZADO
 * - PAGOPREDIAL
 * -- Component: No se encontro componente
 * --- Forms:
 */

/**
 * Desafiliación solo aportes
 * - DESAFAPO
 * -- Component: DesafiliacionAportes - Eventos de Botonera Ok
 * --- Forms: DatosPersonalesValor - TerceroBeneficiarioDesafiliaciones - ObservacionesSolicitud - OK
 */

/**
 * Desafiliación con cesantias
 * - DESAFCESA
 * -- Component: DesafiliacionesCesantias - Eventos de Botonera Ok
 * --- Forms: DatosPersonalesDesafiliacionCesantias - TerceroBeneficiarioDesafiliaciones - ObservacionesSolicitud - OK
 */

/**
 * Vivienda 8
 * - VINUEVAPP
 * -- Component: ComptaVivendaNuevaPPSP - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - PropietarioMatricula - TerceroApoderado - TerceroBeneficiarioPagos - ObservacionesSolicitud - OK
 * - VIUSADAPP
 * -- Component: CompraViviendaUsada - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - PropietarioMatricula - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK
 * - CAEXLEHAPP
 * -- Component: CanonesLeasingHabitacional - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - TerceroApoderado - TerceroBeneficiario - ObservacionesSolicitud - OK
 */

/**
 * Vivienda 14
 * - VINUEVASP
 * -- Component: CompraViviendaNueva - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - PropietarioMatricula - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK
 * - VIUSADASP
 * -- Component: CompraViviendaUsada - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - PropietarioMatricula - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK
 * - CONSNUEVASP
 * -- Component: ConstruccionLotePropio - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - InformacionInmueble - InformacionConyugue - TerceroApoderado - TerceroBeneficiarioPagosVu - ObservacionesSolicitud - OK -- ACTUALIZADO
 * - CAEXLEHASP
 * -- Component: CanonesLeasingHabitacional - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - TerceroApoderado - TerceroBeneficiario - ObservacionesSolicitud - OK
 * - PACRECAHO
 * -- Component: PagoCreditoHipotecario - Eventos de Botonera Ok
 * --- Forms: DatosPersonales - MatriculaInmobiliaria - TerceroBeneficiario - ObservacionesSolicitud - OK
 */

/**
 * Postulacion fondo de solidaridad - Estos van con el parametro withButtons en false
 * - RETODES
 * -- Component: RetiroDesvinculacionCapacidadPsicofisica - Eventos de Botonera no aplican para este formulario
 * --- Forms: InformacionSolicitud - IdentificacionPostulantesNucleoFamiliar - ManifestacionesAutorizaciones - CiudadPreferenciaParaRecibirVivienda - FirmaSolicitud - OK
 * - BENEFALL
 * -- Component: BeneficiarioPorFallecimientoDelAfiliado - Eventos de Botonera no aplican para este formulario
 * --- Forms: InformacionSolicitud - IdentificacionPostulantesNucleoFamiliar - ManifestacionesAutorizaciones - CiudadPreferenciaParaRecibirVivienda - FirmaSolicitud - OK
 * - MECANES
 * -- Component: MecanismosEspeciales - Eventos de Botonera no aplican para este formulario
 * --- Forms: InformacionSolicitud - IdentificacionPostulantesNucleoFamiliar - ManifestacionesAutorizaciones - CiudadPreferenciaParaRecibirVivienda - FirmaSolicitud - OK
 */

/**
 * Retiro definitivo de cesantias
 * - REDEFIN
 * -- Component: RetiroDefinitivoCesantias - Eventos de Botonera OK
 * --- Forms: DatosPersonales - TerceroBeneficiario - OK
 */

/**
 * Objetos DB
 * ObtenerFomulariosSolicitud
 * ConsultarExisteSolicitud
 * ConsultarSolicitudPorActividad
 * ObtenerTipoSubModalidad
 */
