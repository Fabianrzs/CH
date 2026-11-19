import React from "react";
import {
  Grid,
  TextField,
} from "@material-ui/core";
import { AvaluoSectionProps } from "../types/avaluoComercial.types";
import useAvaluoComercialStyles from "../styles/avaluoComercial.styles";
import TituloSeccion from "./TituloSeccion";

const limpiarValorMoneda = (
  value: string
): string =>
  String(value || "").replace(/\D/g, "");

const formatearValorMoneda = (
  value: string
): string => {
  const valorLimpio =
    limpiarValorMoneda(value);

  if (!valorLimpio) {
    return "";
  }

  const valorNumerico = Number(
    valorLimpio
  );

  return `$ ${valorNumerico.toLocaleString(
    "es-CO"
  )}`;
};

function CuadroValoracion({
  values,
  errors,
  onChange,
}: AvaluoSectionProps) {
  const classes =
    useAvaluoComercialStyles();

  const handleChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const valorLimpio =
      limpiarValorMoneda(
        event.target.value
      );

    onChange(
      "inmueble",
      valorLimpio
    );
  };

  return (
    <>
      <TituloSeccion>
        Cuadro de Valoración
      </TituloSeccion>

      <Grid
        container
        spacing={4}
        alignItems="center"
      >
        <Grid item xs={12} md={4}>
          <TextField
            className={classes.field}
            fullWidth
            required
            label="Inmueble"
            value={formatearValorMoneda(
              values.inmueble
            )}
            error={Boolean(
              errors.inmueble
            )}
            helperText={
              errors.inmueble || ""
            }
            onChange={handleChange}
            inputProps={{
              inputMode: "numeric",
              style: {
                textAlign: "right",
              },
            }}
          />
        </Grid>
      </Grid>
    </>
  );
}

export default CuadroValoracion;