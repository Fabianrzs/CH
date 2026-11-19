import React from "react";
import { Grid } from "@material-ui/core";

import CampoAvaluo from "./CampoAvaluo";
import TituloSeccion from "./TituloSeccion";
import { AvaluoSectionProps } from "../types/avaluoComercial.types";
import { garageOptions } from "../constants/avaluoComercial.constants";

function DependenciasInmueble({
  values,
  errors,
  onChange,
}: AvaluoSectionProps) {
  const tieneGaraje =
    String(values.garaje || "")
      .trim()
      .toUpperCase() === "SI";

  return (
    <>
      <TituloSeccion>
        Dependencias
      </TituloSeccion>

      <Grid container spacing={4}>
        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="descripcion"
            label="Descripción"
            value={values.descripcion}
            error={errors.descripcion}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="garaje"
            label="Garaje"
            value={values.garaje}
            error={errors.garaje}
            select
            options={garageOptions}
            onChange={onChange}
          />
        </Grid>

        {tieneGaraje && (
          <Grid item xs={12} md={4}>
            <CampoAvaluo
              name="descripcionGaraje"
              label="Descripción Garaje"
              value={values.descripcionGaraje}
              error={errors.descripcionGaraje}
              onChange={onChange}
            />
          </Grid>
        )}
      </Grid>
    </>
  );
}

export default DependenciasInmueble;