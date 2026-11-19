import React from "react";
import { Box } from "@material-ui/core";
import useAvaluoComercialStyles from "../styles/avaluoComercial.styles";


type Props = {
  children: React.ReactNode;
};

function TituloSeccion({ children }: Props) {
  const classes = useAvaluoComercialStyles();

  return (
    <Box className={classes.section}>
      {children}
    </Box>
  );
}

export default TituloSeccion;