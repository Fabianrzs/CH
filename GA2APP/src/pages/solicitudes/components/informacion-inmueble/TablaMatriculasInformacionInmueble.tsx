import { createStyles, Grid, IconButton, makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import { Delete, Edit } from "@material-ui/icons";
import React from "react"


const useStyles = makeStyles(() =>
  createStyles({
    columnButtonAdd: {
      textAlign: 'right',
    },
  })
);
const TablaMatriculasInformacionInmueble = ({
  mostrarColumnasConyuge,
  matriculasInmobiliariasModel,
  campoSoloLecturaRadicacionAprobada,
  siEsAprobadaSolcitud,
  responseSolicitud,
  seleccionarRegistro,
  eliminarRegistro,
  puedeEditarRegistro
}:
  {
    mostrarColumnasConyuge: any;
    matriculasInmobiliariasModel: any;
    campoSoloLecturaRadicacionAprobada: boolean;
    siEsAprobadaSolcitud: any;
    responseSolicitud: any;
    seleccionarRegistro: any;
    eliminarRegistro: any;
    puedeEditarRegistro: (registro: any) => boolean;
  }) => {
  const classes = useStyles()

  const accionesBloqueadas =
    ((siEsAprobadaSolcitud ||
      responseSolicitud?.secciones?.includes('INCONSISTENCIA')) &&
      responseSolicitud?.soL_RECHAZADA !== '1') ||
    campoSoloLecturaRadicacionAprobada;

  const mostrarAcciones = !accionesBloqueadas;


  let matriculasParaMostrar: any[] = [];

  if (
    Array.isArray(matriculasInmobiliariasModel) &&
    matriculasInmobiliariasModel.length > 0
  ) {
    matriculasParaMostrar = matriculasInmobiliariasModel;
  } else if (
    Array.isArray(responseSolicitud?.solicitudMatriculaDto)
  ) {
    matriculasParaMostrar =
      responseSolicitud.solicitudMatriculaDto;
  }

  return (

    <> <h3>Lista de matrículas inmobiliarias</h3>
      <TableContainer component={Paper}>
        <Grid
          className={classes.columnButtonAdd}
          item
          xs={12}
          md={12}
        >
          <Table aria-label="simple table">
            <TableHead>
              <TableRow>
                <TableCell>Número Matrícula</TableCell>
                <TableCell>Estado</TableCell>
                <TableCell>Dirección</TableCell>
                <TableCell>Departamento</TableCell>
                <TableCell>Ciudad</TableCell>
                {
                  mostrarColumnasConyuge ? <>
                    <TableCell>Tipo Documento </TableCell>
                    <TableCell> Documento </TableCell>
                    <TableCell> Nombres/Apellidos</TableCell>
                  </> : null
                }
                {mostrarAcciones && (
                  <TableCell align="center">
                    Acciones
                  </TableCell>
                )}
              </TableRow>
            </TableHead>
            <TableBody>

              {matriculasParaMostrar?.map(
                (option: any) => (
                  <TableRow key={option?.numeroMatricula}>
                    <TableCell>
                      <span>{option?.numeroMatricula}</span>
                    </TableCell>
                    <TableCell>
                      {option?.esPrincipal ? 'Principal' : 'Relacionado'}
                    </TableCell>
                    <TableCell>
                      {option?.direccion}
                    </TableCell>
                    <TableCell>
                      {option.departamentoInmueble?.descripcion || option?.departamento?.descripcion}
                    </TableCell>
                    <TableCell>
                      {option.ciudadInmueble?.descripcion || option?.ciudad?.descripcion}
                    </TableCell>
                    {
                      mostrarColumnasConyuge ? <>
                        <TableCell>
                          {option?.conyugeActivo ? option?.tipoDocumento : ''}
                        </TableCell>
                        <TableCell>
                          {option?.conyugeActivo ? option?.documento : ''}
                        </TableCell>
                        <TableCell>
                          {option?.conyugeActivo
                            ? `${option?.primerNombre || ''} ${option?.segundoNombre || ''} ${option?.primerApellido || ''} ${option?.segundoApellido || ''}`
                            : ''}
                        </TableCell>
                      </> : null
                    }
                    {mostrarAcciones && (
                      <TableCell align="center">
                        {puedeEditarRegistro(option) && (
                          <IconButton
                            aria-label="Editar"
                            color="primary"
                            onClick={() => {
                              seleccionarRegistro(option);
                            }}
                          >
                            <Edit />
                          </IconButton>
                        )}

                        <IconButton
                          aria-label="Eliminar"
                          color="secondary"
                          onClick={() => {
                            eliminarRegistro(option.numeroMatricula);
                          }}
                        >
                          <Delete />
                        </IconButton>
                      </TableCell>
                    )}
                  </TableRow>
                ),
              )}
            </TableBody>
          </Table>
        </Grid>
      </TableContainer></>
  );
}

export default TablaMatriculasInformacionInmueble