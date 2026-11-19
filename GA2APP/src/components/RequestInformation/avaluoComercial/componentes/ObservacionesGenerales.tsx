import React from "react";
import { Grid } from "@material-ui/core";
import { AvaluoSectionProps } from "../types/avaluoComercial.types";

import CampoAvaluo from "./CampoAvaluo";
import TituloSeccion from "./TituloSeccion";

function ObservacionesGenerales({
  values,
  errors,
  onChange,
}: AvaluoSectionProps) {
  return (
    <>
      <TituloSeccion>
        Observaciones Generales
      </TituloSeccion>

      <Grid container spacing={4}>
        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="distribucionInmueble"
            label="Distribución del Inmueble"
            value={values.distribucionInmueble}
            error={errors.distribucionInmueble}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="caracteristicasAdicionales"
            label="Características Adicionales"
            value={values.caracteristicasAdicionales}
            error={errors.caracteristicasAdicionales}
            onChange={onChange}
          />
        </Grid>
      </Grid>
    </>
  );
}

export default ObservacionesGenerales;