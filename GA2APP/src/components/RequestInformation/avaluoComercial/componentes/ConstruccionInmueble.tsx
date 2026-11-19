import React from "react";
import { Grid } from "@material-ui/core";
import { AvaluoSectionProps } from "../types/avaluoComercial.types";

import CampoAvaluo from "./CampoAvaluo";
import TituloSeccion from "./TituloSeccion";

function ConstruccionInmueble({
  values,
  errors,
  onChange,
}: AvaluoSectionProps) {
  return (
    <>
      <TituloSeccion>
        Construcción
      </TituloSeccion>

      <Grid container spacing={4}>
        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="salaAreaConstruida"
            label="Sala Área Construida"
            value={values.salaAreaConstruida}
            error={errors.salaAreaConstruida}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="areaPrivada"
            label="Área Privada"
            value={values.areaPrivada}
            error={errors.areaPrivada}
            onChange={onChange}
          />
        </Grid>
      </Grid>
    </>
  );
}

export default ConstruccionInmueble;