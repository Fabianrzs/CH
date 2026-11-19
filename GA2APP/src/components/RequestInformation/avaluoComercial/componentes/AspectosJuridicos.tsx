import React from "react";
import {
  CircularProgress,
  Grid,
  TextField,
} from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import {
  FormErrors,
  FormValues,
} from "../types/avaluoComercial.types";
import CampoAvaluo from "./CampoAvaluo";
import TituloSeccion from "./TituloSeccion";
import CampoAutocomplete from "./CampoAutocomplete";

interface Props {
  values: FormValues;
  errors: FormErrors;
  departamentos: any[];
  ciudadesEscritura: any[];
  cargandoCiudadesEscritura: boolean;

  onChange: (
    field: keyof FormValues,
    value: string
  ) => void;

  onDepartamentoChange: (
    departamento: any | null
  ) => void;

  onCiudadChange: (
    ciudad: any | null
  ) => void;
}

const obtenerIdCatalogo = (
  item: any
): string =>
  String(
    item?.codigo ??
      item?.id ??
      item?.departamentoId ??
      item?.ciudadId ??
      item?.idDepartamento ??
      item?.idCiudad ??
      ""
  );

const obtenerNombreCatalogo = (
  item: any
): string =>
  item?.nombre ||
  item?.descripcion ||
  item?.label ||
  item?.nombreDepartamento ||
  item?.nombreCiudad ||
  "";

function AspectosJuridicos({
  values,
  errors,
  departamentos,
  ciudadesEscritura,
  cargandoCiudadesEscritura,
  onChange,
  onDepartamentoChange,
  onCiudadChange,
}: Props) {
  const departamentoSeleccionado =
    departamentos.find(
      (departamento: any) =>
        obtenerIdCatalogo(departamento) ===
        String(
          values.departamentoEscrituraId
        )
    ) || null;

  const ciudadSeleccionada =
    ciudadesEscritura.find(
      (ciudad: any) =>
        obtenerIdCatalogo(ciudad) ===
        String(values.ciudadEscrituraId)
    ) || null;

  return (
    <>
      <TituloSeccion>
        Aspectos Jurídicos
      </TituloSeccion>

      <Grid container spacing={4}>
        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="propietario"
            label="Propietario"
            value={values.propietario}
            error={errors.propietario}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="escritura"
            label="Escritura"
            value={values.escritura}
            error={errors.escritura}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="notaria"
            label="Notaría"
            value={values.notaria}
            error={errors.notaria}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="fechaEscritura"
            label="Fecha Escritura"
            type="date"
            value={values.fechaEscritura}
            error={errors.fechaEscritura}
            onChange={onChange}
          />
        </Grid>

        {/* DEPARTAMENTO ESCRITURA */}
        <Grid item xs={12} md={4}>
        <CampoAutocomplete
                    label="Departamento Escritura"
                    required
                    value={departamentoSeleccionado}
                    options={departamentos}
                    getOptionLabel={item => item.nombre}
                    error={errors.departamentoEscritura}
                    onChange={onDepartamentoChange}
                />


        </Grid>

        {/* CIUDAD ESCRITURA */}
        <Grid item xs={12} md={4}>
        <CampoAutocomplete
                    label="Ciudad Escritura"
                    required
                    value={ciudadSeleccionada}
                    options={ciudadesEscritura}
                    loading={cargandoCiudadesEscritura}
                    disabled={!values.departamentoEscrituraId}
                    getOptionLabel={item => item.nombre}
                    error={errors.ciudadEscritura}
                    onChange={onCiudadChange}
                />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="cedulaCatastral"
            label="Cédula Catastral"
            value={values.cedulaCatastral}
            error={errors.cedulaCatastral}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="licenciaConstruccion"
            label="Licencia de Construcción"
            value={
              values.licenciaConstruccion
            }
            error={
              errors.licenciaConstruccion
            }
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="nombreConjunto"
            label="Nombre del Conjunto"
            value={values.nombreConjunto}
            error={errors.nombreConjunto}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="coeficienteCopropiedad"
            label="Coeficiente Copropiedad"
            value={
              values.coeficienteCopropiedad
            }
            error={
              errors.coeficienteCopropiedad
            }
            onChange={onChange}
          />
        </Grid>
      </Grid>
    </>
  );
}

export default AspectosJuridicos;