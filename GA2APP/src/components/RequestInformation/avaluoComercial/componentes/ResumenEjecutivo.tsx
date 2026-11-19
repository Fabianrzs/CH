import React from "react";
import {
  Box,
  CircularProgress,
  Grid,
  Typography,
} from "@material-ui/core";

import CampoAvaluo from "./CampoAvaluo";
import useAvaluoComercialStyles from "../styles/avaluoComercial.styles";
import { AvaluoSectionProps } from "../types/avaluoComercial.types";

type Props = AvaluoSectionProps & {
  cargandoCliente: boolean;
};

function ResumenEjecutivo({
  values,
  errors,
  cargandoCliente,
  onChange,
}: Props) {
  const classes = useAvaluoComercialStyles();

  return (
    <>
      {cargandoCliente && (
        <Box className={classes.loadingClient}>
          <CircularProgress size={20} />

          <Typography
            variant="body2"
            className={classes.loadingText}
          >
            Consultando información del cliente...
          </Typography>
        </Box>
      )}

      <Grid container spacing={4}>
        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="cliente"
            label="Cliente"
            value={values.cliente}
            error={errors.cliente}
            readOnly
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="cedulaCliente"
            label="Cédula Cliente"
            value={values.cedulaCliente}
            error={errors.cedulaCliente}
            readOnly
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="direccion"
            label="Dirección"
            value={values.direccion}
            error={errors.direccion}
            readOnly
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="sector"
            label="Sector"
            value={values.sector}
            error={errors.sector}
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="barrio"
            label="Barrio"
            value={values.barrio}
            error={errors.barrio}
            readOnly
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="ciudad"
            label="Ciudad"
            value={values.ciudad}
            error={errors.ciudad}
            readOnly
            onChange={onChange}
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <CampoAvaluo
            name="departamento"
            label="Departamento"
            value={values.departamento}
            error={errors.departamento}
            readOnly
            onChange={onChange}
          />
        </Grid>
      </Grid>
    </>
  );
}

export default ResumenEjecutivo;