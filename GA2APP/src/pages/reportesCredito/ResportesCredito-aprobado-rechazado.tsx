import React, { useState } from 'react';
import moment from 'moment';
import { toast } from 'react-toastify';
import {
  Button,
  Checkbox,
  FormControlLabel,
  Grid,
  IconButton,
  makeStyles,
  Menu,
  MenuItem,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  TextField,
} from '@material-ui/core';
import {
  FilterList as FilterListIcon,
  SaveAlt as SaveAltIcon,
} from '@material-ui/icons';
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';

import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import { LoadingV2Component } from '../../components/loading/loadingV2.component';
import ApiReportes from '../../services/api-reportes.service';
import UtilService from '../../utils/util.service';

interface ReporteCreditoAprobadoRechazado {
  cedula: string;
  nombreAfiliado: string;
  fechaComite: string;
  actaAprobacion: string;
  fuerza: string;
  gradoCategoria: string;
  tipoSolucionVivienda: string;
  compraCarteraEntidad: string;
  puntoRecepcion: string;
  tipoVivienda: string;
  inmueble: string;
  vetustez: string;
  ciudadInmueble: string;
  departamento: string;
  tipoCredito: string;
  antiguedadAfiliado: string;
  sexo: string;
  estadoCivil: string;
  edad: number;
  scoring: string;
  ingresoTotal: number;
  nivelEndeudamiento: string;
  resultadoAGMGO: string;
  fechaScoring: string;
  plazoMeses: number;
  tasa: number;
  seguroPositiva: string;
  cuota: number;
  valorCredito: number;
  subsidio: number;
  cuentaIndividual: number;
  valorRecursosPropios: number;
  valorVenta: number;
  valorAvaluo: number;
  tipoViviendaVis: string;
  seguro: string;
  porcentajeExtraPrima: number;
  diferenciaPorcentaje: number;
  observaciones: string;
}

interface TipoCredito {
  id: number;
  descripcion: string;
}

const useStyles = makeStyles({
  tableHeader: {
    backgroundColor: '#FEC917',
    fontWeight: 'bold',
  },
  tableContainer: {
    marginTop: '50px',
    overflowX: 'auto',
  },
  table: {
    minWidth: 5000,
  },
  tableCell: {
    whiteSpace: 'nowrap',
  },
});

const columnasDisponibles = [
  { id: 'cedula', label: 'Cédula' },
  { id: 'nombreAfiliado', label: 'Nombre del Afiliado' },
  { id: 'fechaComite', label: 'Fecha de Comité' },
  { id: 'actaAprobacion', label: 'Acta de Aprobación' },
  { id: 'fuerza', label: 'Fuerza' },
  { id: 'gradoCategoria', label: 'Grado Categoría' },
  {
    id: 'tipoSolucionVivienda',
    label: 'Tipo de Solución Vivienda',
  },
  {
    id: 'compraCarteraEntidad',
    label: 'Compra de Cartera / Entidad',
  },
  { id: 'puntoRecepcion', label: 'Punto de Recepción' },
  { id: 'tipoVivienda', label: 'Tipo Vivienda' },
  { id: 'inmueble', label: 'Inmueble' },
  { id: 'vetustez', label: 'Vetustez' },
  {
    id: 'ciudadInmueble',
    label: 'Ciudad del Inmueble',
  },
  { id: 'departamento', label: 'Departamento' },
  { id: 'tipoCredito', label: 'Tipo de Crédito' },
  {
    id: 'antiguedadAfiliado',
    label: 'Antigüedad Afiliado',
  },
  { id: 'sexo', label: 'Sexo' },
  { id: 'estadoCivil', label: 'Estado Civil' },
  { id: 'edad', label: 'Edad' },
  { id: 'scoring', label: 'Scoring' },
  { id: 'ingresoTotal', label: 'Ingreso Total' },
  {
    id: 'nivelEndeudamiento',
    label: 'Nivel de Endeudamiento',
  },
  { id: 'resultadoAGMGO', label: 'Resultado AGMGO' },
  { id: 'fechaScoring', label: 'Fecha de Scoring' },
  { id: 'plazoMeses', label: 'Plazo (Meses)' },
  { id: 'tasa', label: 'Tasa %' },
  { id: 'seguroPositiva', label: 'Seguro Positiva' },
  { id: 'cuota', label: 'Cuota' },
  { id: 'valorCredito', label: 'Valor del Crédito' },
  { id: 'subsidio', label: 'Subsidio' },
  { id: 'cuentaIndividual', label: 'Cuenta Individual' },
  {
    id: 'valorRecursosPropios',
    label: 'Valor Recursos Propios',
  },
  { id: 'valorVenta', label: 'Valor Venta' },
  { id: 'valorAvaluo', label: 'Valor Avalúo' },
  {
    id: 'tipoViviendaVis',
    label: 'Tipo de Vivienda (VIS - NO VIS - VIP)',
  },
  {
    id: 'seguro',
    label: 'Seguro (Positiva - Externo)',
  },
  {
    id: 'porcentajeExtraPrima',
    label: '% de Extra Prima',
  },
  {
    id: 'diferenciaPorcentaje',
    label: 'Diferencia %',
  },
  { id: 'observaciones', label: 'Observaciones' },
];

const ResportesCreditoAprobadoRechazado = () => {
  const classes = useStyles();
  const apiReportes = ApiReportes();

  const [cedula, setCedula] = useState<string>('');
  const [fechaComite, setFechaComite] = useState<Date | null>(
    null,
  );
  const [tipoCredito, setTipoCredito] = useState<number>(0);

  const [listaTipoCredito] = useState<TipoCredito[]>([
    {
      id: 0,
      descripcion: 'Todos',
    },
    {
      id: 1,
      descripcion: 'Hipotecario',
    },
    {
      id: 2,
      descripcion: 'Leasing Habitacional',
    },
  ]);

  const [data, setData] = useState<
    ReporteCreditoAprobadoRechazado[]
  >([]);

  const [page, setPage] = useState<number>(0);
  const [rowsPerPage, setRowsPerPage] =
    useState<number>(10);

  const [isLoading, setIsLoading] =
    useState<boolean>(false);

  const [columnasVisibles, setColumnasVisibles] =
    useState<string[]>(
      columnasDisponibles.map(columna => columna.id),
    );

  const [anchorEl, setAnchorEl] =
    useState<null | HTMLElement>(null);

  const handleOpenMenu = (
    event: React.MouseEvent<HTMLElement>,
  ) => {
    setAnchorEl(event.currentTarget);
  };

  const handleCloseMenu = () => {
    setAnchorEl(null);
  };

  const limpiarFiltros = () => {
    setCedula('');
    setFechaComite(null);
    setTipoCredito(0);
    setData([]);
    setPage(0);
  };

  const handleBuscar = async () => {
    setIsLoading(true);

    const request = {
      cedula:
        cedula.trim() !== ''
          ? cedula.trim()
          : null,
      fechaComite: fechaComite
        ? fechaComite.toISOString().split('T')[0]
        : null,
      tipoCredito:
        tipoCredito !== 0
          ? tipoCredito
          : null,
    };

    console.log(
      'Filtros reporte aprobados/rechazados:',
      request,
    );

    try {
      /*
       * TODO:
       *
       * Cuando BACK entregue el endpoint,
       * reemplazar este bloque.
       *
       * Ejemplo:
       *
       * const response =
       *   await apiReportes
       *     .ObtenerReporteCreditoAprobadoRechazado(
       *       request,
       *     );
       *
       * if (response && response.isSuccess) {
       *   setData(response.data || []);
       *   setPage(0);
       * } else {
       *   setData([]);
       *
       *   toast(
       *     'No hay información para los filtros seleccionados',
       *     {
       *       type: toast.TYPE.WARNING,
       *     },
       *   );
       * }
       */

      console.log(
        'Pendiente conectar servicio del BACK',
      );

      setData([]);
      setPage(0);
    } catch (error) {
      console.error(
        'Error consultando reporte:',
        error,
      );

      setData([]);

      toast(
        'Ocurrió un error al realizar la consulta.',
        {
          type: toast.TYPE.ERROR,
        },
      );
    } finally {
      setIsLoading(false);
    }
  };

  const exportToExcel = async () => {
    if (data.length === 0) {
      toast('No hay información para exportar', {
        type: toast.TYPE.WARNING,
      });

      return;
    }

    const columnDataList = data.map(row => ({
      data: [
        String(row.cedula || ''),
        String(row.nombreAfiliado || ''),
        String(row.fechaComite || ''),
        String(row.actaAprobacion || ''),
        String(row.fuerza || ''),
        String(row.gradoCategoria || ''),
        String(row.tipoSolucionVivienda || ''),
        String(row.compraCarteraEntidad || ''),
        String(row.puntoRecepcion || ''),
        String(row.tipoVivienda || ''),
        String(row.inmueble || ''),
        String(row.vetustez || ''),
        String(row.ciudadInmueble || ''),
        String(row.departamento || ''),
        String(row.tipoCredito || ''),
        String(row.antiguedadAfiliado || ''),
        String(row.sexo || ''),
        String(row.estadoCivil || ''),
        String(row.edad || ''),
        String(row.scoring || ''),
        String(row.ingresoTotal || ''),
        String(row.nivelEndeudamiento || ''),
        String(row.resultadoAGMGO || ''),
        String(row.fechaScoring || ''),
        String(row.plazoMeses || ''),
        String(row.tasa || ''),
        String(row.seguroPositiva || ''),
        String(row.cuota || ''),
        String(row.valorCredito || ''),
        String(row.subsidio || ''),
        String(row.cuentaIndividual || ''),
        String(row.valorRecursosPropios || ''),
        String(row.valorVenta || ''),
        String(row.valorAvaluo || ''),
        String(row.tipoViviendaVis || ''),
        String(row.seguro || ''),
        String(row.porcentajeExtraPrima || ''),
        String(row.diferenciaPorcentaje || ''),
        String(row.observaciones || ''),
      ],
    }));

    const request = {
      columnDataList: columnDataList.map(item => ({
        data: item.data,
      })),
      columnName: columnasDisponibles.map(
        columna => columna.label,
      ),
    };

    try {
      const response = await apiReportes.GenerarExcels(
        request,
      );

      const blob = new Blob([response], {
        type:
          'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      });

      const url =
        window.URL.createObjectURL(blob);

      const link =
        document.createElement('a');

      link.href = url;

      link.download =
        `Reporte_creditos_aprobados_rechazados_${moment().format(
          'YYYYMMDD_HHmmss',
        )}.xlsx`;

      document.body.appendChild(link);

      link.click();

      document.body.removeChild(link);

      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error(
        'Error exportando Excel:',
        error,
      );

      toast(
        'No se pudo generar el archivo Excel.',
        {
          type: toast.TYPE.ERROR,
        },
      );
    }
  };

  const handleChangePage = (
    event: unknown,
    newPage: number,
  ) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement>,
  ) => {
    setRowsPerPage(
      parseInt(event.target.value, 10),
    );

    setPage(0);
  };

  const toggleColumna = (
    idColumna: string,
  ) => {
    setColumnasVisibles(prev => {
      if (prev.includes(idColumna)) {
        return prev.filter(
          id => id !== idColumna,
        );
      }

      return [
        ...prev,
        idColumna,
      ];
    });
  };

  return (
    <div>
      <LoadingV2Component
        key="loader"
        loading={isLoading}
        tittle="Consultando..."
      />

      <PageIndicators
        topTextNivel1="Reportes Crédito"
        topTextNivel2="Aprobados / Rechazados"
      />

      <IconTitle
        title="Reporte Solicitudes Aprobadas / Rechazadas"
        subTitle=""
        icon="fch-Vigencia-GMF-1-2"
      />

      <MuiPickersUtilsProvider
        utils={DateFnsUtils}
      >
        <Paper
          style={{
            padding: 24,
            marginTop: 20,
          }}
        >
          <Grid
            container
            spacing={3}
            alignItems="center"
          >
            <Grid
              item
              xs={12}
              sm={4}
            >
              <TextField
                label="Cédula"
                value={cedula}
                onChange={event =>
                  setCedula(
                    event.target.value.replace(
                      /[^0-9]/g,
                      '',
                    ),
                  )
                }
                fullWidth
                variant="standard"
                type="number"
                InputLabelProps={{
                  shrink: true,
                }}
              />
            </Grid>

            <Grid
              item
              xs={12}
              sm={4}
            >
              <CHDatePicker
                label="Fecha Comité"
                format="yyyy-MM-dd"
                value={fechaComite}
                onChange={setFechaComite}
                fullWidth
                InputLabelProps={{
                  shrink: true,
                }}
              />
            </Grid>

            <Grid
              item
              xs={12}
              sm={4}
            >
              <TextField
                label="Tipo de Crédito"
                select
                value={tipoCredito}
                onChange={event =>
                  setTipoCredito(
                    Number(event.target.value),
                  )
                }
                fullWidth
                variant="standard"
                InputLabelProps={{
                  shrink: true,
                }}
              >
                {listaTipoCredito.map(
                  option => (
                    <MenuItem
                      key={option.id}
                      value={option.id}
                    >
                      {option.descripcion}
                    </MenuItem>
                  ),
                )}
              </TextField>
            </Grid>

            <Grid
              item
              xs={6}
              sm={2}
            >
              <Button
                variant="contained"
                color="primary"
                onClick={handleBuscar}
                fullWidth
              >
                Consultar
              </Button>
            </Grid>

            <Grid
              item
              xs={6}
              sm={2}
            >
              <Button
                variant="outlined"
                color="secondary"
                onClick={limpiarFiltros}
                fullWidth
              >
                Limpiar
              </Button>
            </Grid>
          </Grid>

          {data.length > 0 && (
            <TableContainer
              component={Paper}
              className={
                classes.tableContainer
              }
            >
              <Table
                className={classes.table}
              >
                <TableHead>
                  <TableRow>
                    <TableCell
                      colSpan={
                        columnasVisibles.length
                      }
                      align="right"
                    >
                      <Button
                        startIcon={
                          <SaveAltIcon />
                        }
                        variant="outlined"
                        color="secondary"
                        onClick={exportToExcel}
                        style={{
                          marginRight: 8,
                        }}
                      >
                        Descargar Excel
                      </Button>

                      <IconButton
                        onClick={
                          handleOpenMenu
                        }
                        aria-label="Filtrar columnas"
                      >
                        <FilterListIcon />
                      </IconButton>

                      <Menu
                        anchorEl={anchorEl}
                        keepMounted
                        open={Boolean(
                          anchorEl,
                        )}
                        onClose={
                          handleCloseMenu
                        }
                      >
                        {columnasDisponibles.map(
                          columna => (
                            <MenuItem
                              key={
                                columna.id
                              }
                            >
                              <FormControlLabel
                                control={
                                  <Checkbox
                                    id={`chk-${columna.id}`}
                                    checked={columnasVisibles.includes(
                                      columna.id,
                                    )}
                                    onChange={() =>
                                      toggleColumna(
                                        columna.id,
                                      )
                                    }
                                  />
                                }
                                label={
                                  columna.label
                                }
                              />
                            </MenuItem>
                          ),
                        )}
                      </Menu>
                    </TableCell>
                  </TableRow>

                  <TableRow
                    className={
                      classes.tableHeader
                    }
                  >
                    {columnasDisponibles
                      .filter(columna =>
                        columnasVisibles.includes(
                          columna.id,
                        ),
                      )
                      .map(columna => (
                        <TableCell
                          key={columna.id}
                          className={
                            classes.tableCell
                          }
                        >
                          {columna.label}
                        </TableCell>
                      ))}
                  </TableRow>
                </TableHead>

                <TableBody>
                  {data
                    .slice(
                      page *
                        rowsPerPage,
                      page *
                        rowsPerPage +
                        rowsPerPage,
                    )
                    .map(
                      (
                        row,
                        index,
                      ) => (
                        <TableRow
                          key={`${row.cedula}-${index}`}
                        >
                          {columnasVisibles.includes(
                            'cedula',
                          ) && (
                            <TableCell>
                              {row.cedula}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'nombreAfiliado',
                          ) && (
                            <TableCell>
                              {
                                row.nombreAfiliado
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'fechaComite',
                          ) && (
                            <TableCell>
                              {row.fechaComite
                                ? new Date(
                                    row.fechaComite,
                                  ).toLocaleDateString()
                                : ''}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'actaAprobacion',
                          ) && (
                            <TableCell>
                              {
                                row.actaAprobacion
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'fuerza',
                          ) && (
                            <TableCell>
                              {row.fuerza}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'gradoCategoria',
                          ) && (
                            <TableCell>
                              {
                                row.gradoCategoria
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'tipoSolucionVivienda',
                          ) && (
                            <TableCell>
                              {
                                row.tipoSolucionVivienda
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'compraCarteraEntidad',
                          ) && (
                            <TableCell>
                              {
                                row.compraCarteraEntidad
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'puntoRecepcion',
                          ) && (
                            <TableCell>
                              {
                                row.puntoRecepcion
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'tipoVivienda',
                          ) && (
                            <TableCell>
                              {
                                row.tipoVivienda
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'inmueble',
                          ) && (
                            <TableCell>
                              {row.inmueble}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'vetustez',
                          ) && (
                            <TableCell>
                              {row.vetustez}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'ciudadInmueble',
                          ) && (
                            <TableCell>
                              {
                                row.ciudadInmueble
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'departamento',
                          ) && (
                            <TableCell>
                              {
                                row.departamento
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'tipoCredito',
                          ) && (
                            <TableCell>
                              {
                                row.tipoCredito
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'antiguedadAfiliado',
                          ) && (
                            <TableCell>
                              {
                                row.antiguedadAfiliado
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'sexo',
                          ) && (
                            <TableCell>
                              {row.sexo}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'estadoCivil',
                          ) && (
                            <TableCell>
                              {
                                row.estadoCivil
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'edad',
                          ) && (
                            <TableCell>
                              {row.edad}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'scoring',
                          ) && (
                            <TableCell>
                              {row.scoring}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'ingresoTotal',
                          ) && (
                            <TableCell>
                              {UtilService.formatCurrency(
                                row.ingresoTotal,
                              )}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'nivelEndeudamiento',
                          ) && (
                            <TableCell>
                              {
                                row.nivelEndeudamiento
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'resultadoAGMGO',
                          ) && (
                            <TableCell>
                              {
                                row.resultadoAGMGO
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'fechaScoring',
                          ) && (
                            <TableCell>
                              {row.fechaScoring
                                ? new Date(
                                    row.fechaScoring,
                                  ).toLocaleDateString()
                                : ''}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'plazoMeses',
                          ) && (
                            <TableCell>
                              {
                                row.plazoMeses
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'tasa',
                          ) && (
                            <TableCell>
                              {row.tasa}
                              %
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'seguroPositiva',
                          ) && (
                            <TableCell>
                              {
                                row.seguroPositiva
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'cuota',
                          ) && (
                            <TableCell>
                              {UtilService.formatCurrency(
                                row.cuota,
                              )}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'valorCredito',
                          ) && (
                            <TableCell>
                              {UtilService.formatCurrency(
                                row.valorCredito,
                              )}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'subsidio',
                          ) && (
                            <TableCell>
                              {UtilService.formatCurrency(
                                row.subsidio,
                              )}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'cuentaIndividual',
                          ) && (
                            <TableCell>
                              {UtilService.formatCurrency(
                                row.cuentaIndividual,
                              )}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'valorRecursosPropios',
                          ) && (
                            <TableCell>
                              {UtilService.formatCurrency(
                                row.valorRecursosPropios,
                              )}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'valorVenta',
                          ) && (
                            <TableCell>
                              {UtilService.formatCurrency(
                                row.valorVenta,
                              )}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'valorAvaluo',
                          ) && (
                            <TableCell>
                              {UtilService.formatCurrency(
                                row.valorAvaluo,
                              )}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'tipoViviendaVis',
                          ) && (
                            <TableCell>
                              {
                                row.tipoViviendaVis
                              }
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'seguro',
                          ) && (
                            <TableCell>
                              {row.seguro}
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'porcentajeExtraPrima',
                          ) && (
                            <TableCell>
                              {
                                row.porcentajeExtraPrima
                              }
                              %
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'diferenciaPorcentaje',
                          ) && (
                            <TableCell>
                              {
                                row.diferenciaPorcentaje
                              }
                              %
                            </TableCell>
                          )}

                          {columnasVisibles.includes(
                            'observaciones',
                          ) && (
                            <TableCell>
                              {
                                row.observaciones
                              }
                            </TableCell>
                          )}
                        </TableRow>
                      ),
                    )}
                </TableBody>
              </Table>

              <TablePagination
                component="div"
                count={data.length}
                page={page}
                onPageChange={
                  handleChangePage
                }
                rowsPerPage={
                  rowsPerPage
                }
                onRowsPerPageChange={
                  handleChangeRowsPerPage
                }
                labelRowsPerPage="Filas por página"
              />
            </TableContainer>
          )}
        </Paper>
      </MuiPickersUtilsProvider>
    </div>
  );
};

export default ResportesCreditoAprobadoRechazado;