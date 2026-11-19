import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { createStyles, Grid, MenuItem, makeStyles } from '@material-ui/core';
import { toast } from 'react-toastify';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { MatriculaInmobiliariaModel } from '../../../../models/matricula-inmobiliaria.model';
import ApiClientes from '../../../../services/api-clients.service';
import { ID_SUBMODALIDAD_SEGUNDO_PAGO, ID_SUBMODALIDAD_SIMULTANEO } from '../../../../constants/constants';


const useStyles = makeStyles(() =>
  createStyles({
    column: {
      padding: '10px',
      textAlign: 'center'
    },
    columnLabel: {
      padding: '10px'
    }
  })
);

const InformacionDeAdjudicacion = ({
  formulario,
  responseSolicitud,
  matriculas
}: {
  formulario: UseFormReturn<any>,
  responseSolicitud: any,
  matriculas: MatriculaInmobiliariaModel[];
}) => {
  const classes = useStyles();
  const api = ApiClientes();
  const [disabledDetalle, setdisabledDetalle] = useState<boolean>(false);

  useEffect(() => {
    if (responseSolicitud?.solicitudAdjudicacion !== undefined && responseSolicitud?.solicitudAdjudicacion !== null) {
      formulario.setValue('tipoDeVivienda', responseSolicitud?.solicitudAdjudicacion.siA_TIPO_VIVIENDA);
      formulario.setValue('ciudadIA', responseSolicitud?.solicitudAdjudicacion?.siA_CIUDAD);
      formulario.setValue('direccionIA', responseSolicitud?.solicitudAdjudicacion.siA_DIRECCION);
      formulario.setValue('numeroDeMatrículaIA', responseSolicitud?.solicitudAdjudicacion.siA_MATRICULA);
      formulario.setValue('valorDelSubsidioIA', responseSolicitud?.solicitudAdjudicacion.siA_VALOR_SUBSIDIADO);
      setdisabledDetalle(true);
      obtenerValorSubsidio();
    }
  }, []);

  useEffect(() => {
    obtenerValorSubsidio();
  }, [responseSolicitud]);

  const buscarMatricula = () => {
    if (!Array.isArray(matriculas) || matriculas.length === 0) {
      return;
    }
  
    const matricula = matriculas[0];
  
    let ciudad = '';
  
    if (matricula?.ciudadInmueble?.descripcion) {
      ciudad = matricula.ciudadInmueble.descripcion;
    } else if (matricula?.ciudad?.descripcion) {
      ciudad = matricula.ciudad.descripcion;
    }
  
    formulario.setValue('ciudadIA', ciudad);
    formulario.setValue('direccionIA', matricula?.direccion || '');
    formulario.setValue(
      'numeroDeMatrículaIA',
      matricula?.numeroMatricula || '',
    );
  };

  const obtenerValorSubsidio = async () => {
    const identificationType = Number(responseSolicitud?.tipoIdentificacion || '');
    const identification = responseSolicitud?.identificacionCliente || '';
    await api
      .getClientByIdentificationTypeIdentificationNumberRequest(identificationType,
        identification)
      .then((response: any) => {
        if (response.isSuccess) {
          if ((ID_SUBMODALIDAD_SEGUNDO_PAGO.some((item: any) => item === responseSolicitud?.tipoSubModalidadId) ||
            ID_SUBMODALIDAD_SIMULTANEO.some((item: any) => item === responseSolicitud?.tipoSubModalidadId))
            && response?.data?.valorSubsidio > 0) {
            formulario.setValue('valorDelSubsidioIA', response?.data?.valorSubsidio);
          }
        }
      }
      )
      .catch(() => {
        toast('Error al consultar información en formulario adjudicación', { type: toast.TYPE.ERROR });
      });
  }

  const normalizarTexto = (texto: any) =>
    String(texto || '')
      .trim()
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '');

  const esSegundoPago = ID_SUBMODALIDAD_SEGUNDO_PAGO.some(
    (item: any) => item === responseSolicitud?.tipoSubModalidadId,
  );

  const descripcionModalidad = normalizarTexto(
    responseSolicitud?.descripcionModalidad,
  );

  const esViviendaNueva =
    esSegundoPago &&
    descripcionModalidad.includes('vivienda nueva');

  const esViviendaUsada =
    esSegundoPago &&
    descripcionModalidad.includes('vivienda usada');

  useEffect(() => {
    if (!esSegundoPago) {
      return;
    }

    if (esViviendaNueva) {
      formulario.setValue('tipoDeVivienda', 'nueva', {
        shouldValidate: true,
        shouldDirty: true,
      });
      return;
    }

    if (esViviendaUsada) {
      formulario.setValue('tipoDeVivienda', 'usada', {
        shouldValidate: true,
        shouldDirty: true,
      });
    }
  }, [
    esSegundoPago,
    esViviendaNueva,
    esViviendaUsada,
    formulario,
  ]);

  useEffect(() => {
    if (!esSegundoPago) {
      return;
    }

    if (!esViviendaNueva && !esViviendaUsada) {
      return;
    }

    if (!Array.isArray(matriculas) || matriculas.length === 0) {
      return;
    }

    buscarMatricula();
  }, [
    esSegundoPago,
    esViviendaNueva,
    esViviendaUsada,
    matriculas,
  ]);

  return (
    <>
      <CHPaper title='Información de Adjudicación'>
        <Grid container>
          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name="tipoDeVivienda"
              control={formulario.control}
              rules={{
                required: 'Este campo es requerido.',
              }}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHSelect
                  label="Tipo de vivienda *"
                  value={value}
                  disabled={disabledDetalle || esSegundoPago}
                  onChange={(newValue) => {
                    const selectedValue = newValue.target.value as string;

                    onChange(selectedValue);
                    buscarMatricula();
                  }}
                  error={!!error}
                  helperText={error ? error.message : null}
                >
                  <MenuItem
                    value="nueva"
                    disabled={esViviendaUsada}
                  >
                    Nueva
                  </MenuItem>

                  <MenuItem
                    value="usada"
                    disabled={esViviendaNueva}
                  >
                    Usada
                  </MenuItem>
                </CHSelect>
              )}
            />
          </Grid>
          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name='ciudadIA'
              control={formulario.control}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (

                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly
                  label='Ciudad'
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name='direccionIA'
              control={formulario.control}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly
                  label='Dirección'
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name='numeroDeMatrículaIA'
              control={formulario.control}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  error={!!error}
                  helperText={error ? error.message : null}
                  isReadOnly
                  label='Número de Matrícula *'
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
          <Grid className={classes.column} item xs={12} md={6} lg={3}>
            <Controller
              name='valorDelSubsidioIA'
              control={formulario.control}
              render={({
                field: { onChange, value },
                fieldState: { error },
              }) => (
                <CHTextField
                  currencyMask
                  error={!!error}
                  helperText={error ? error.message : null}
                  InputLabelProps={{ shrink: true }}
                  disabled
                  isReadOnly={disabledDetalle
                    || (ID_SUBMODALIDAD_SIMULTANEO.some(item => item === responseSolicitud?.tipoSubModalidadId) && value > 0)
                    || (ID_SUBMODALIDAD_SEGUNDO_PAGO.some(item => item === responseSolicitud?.tipoSubModalidadId) && value > 0)}
                  label='Valor del subsidio *'
                  value={value}
                  onChange={onChange}
                />
              )}
            />
          </Grid>
        </Grid>
      </CHPaper>
    </>
  )
}

export default InformacionDeAdjudicacion;