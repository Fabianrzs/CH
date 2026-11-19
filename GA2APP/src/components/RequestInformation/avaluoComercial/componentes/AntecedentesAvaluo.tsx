import React from "react";
import { Grid } from "@material-ui/core";
import { AvaluoSectionProps } from "../types/avaluoComercial.types";
import CampoAvaluo from "./CampoAvaluo";
import TituloSeccion from "./TituloSeccion";

function AntecedentesAvaluo({
  values,
  errors,
  onChange,
}: AvaluoSectionProps) {
  return (
    <>
      <TituloSeccion>
        Antecedentes
      </TituloSeccion>

      <Grid container spacing={4}>
        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="fechaInforme"
            label="Fecha del Informe"
            type="date"
            value={values.fechaInforme}
            error={errors.fechaInforme}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="vetustez"
            label="Vetustez"
            value={values.vetustez}
            error={errors.vetustez}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="fechaPagoAvaluo"
            label="Fecha de Pago Avalúo"
            type="date"
            value={values.fechaPagoAvaluo}
            error={errors.fechaPagoAvaluo}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="fechaVisita"
            label="Fecha de Visita"
            type="date"
            value={values.fechaVisita}
            error={errors.fechaVisita}
            onChange={onChange}
          />
        </Grid>
      </Grid>
    </>
  );
}

export default AntecedentesAvaluo;