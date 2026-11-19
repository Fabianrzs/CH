import React, { useMemo, useState } from "react";
import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
  Typography,
} from "@material-ui/core";
import { Controller, useForm } from "react-hook-form";
import MUIDataTable from "mui-datatables";
import * as XLSX from "xlsx";
import CarteraApi from "../../../services/api-cartera-service";
import CHDatePicker from "../../../components/ch-datepicker/CHDatepicker";


type FormValues = {
  fechaInicial: Date | null;
  fechaFinal: Date | null;
};

const ModalHistorialSaldosDiarios = ({
  open,
  onClose,
  producto,
}: {
  open: boolean;
  onClose: () => void;
  producto: any;
}) => {
  const carteraApi = CarteraApi();

  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const formularioHistorial = useForm<FormValues>({
    defaultValues: {
      fechaInicial: null,
      fechaFinal: null,
    },
  });

  const fechaInicialWatch = formularioHistorial.watch("fechaInicial");

  const buildDate = (date: Date | null, endOfDay = false) => {
    if (!date) return null;

    const fecha = new Date(date);

    if (endOfDay) {
      fecha.setHours(23, 59, 59, 999);
    } else {
      fecha.setHours(0, 0, 0, 0);
    }

    return fecha.toISOString();
  };

  const formatHeader = (key: string) =>
    key
      .replace(/([A-Z])/g, " $1")
      .replace(/^./, (str) => str.toUpperCase());

  const formatValue = (value: any) => {
    if (value === null || value === undefined) return "";

    if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}T/.test(value)) {
      return new Date(value).toLocaleDateString("es-CO");
    }

    if (typeof value === "number") {
      return new Intl.NumberFormat("es-CO").format(value);
    }

    return String(value);
  };

  const columns = useMemo(() => {
    if (!data.length) return [];

    const keys = new Set<string>();

    data.forEach((item) => {
      Object.keys(item || {}).forEach((key) => {
        keys.add(key);
      });
    });

    const excluded = ["fechaCreacion", "fechaUltimaActualizacion"];

    return Array.from(keys)
      .filter((key) => !excluded.includes(key))
      .map((key) => ({
        name: key,
        label: formatHeader(key),
        options: {
          filter: true,
          sort: true,
          customBodyRender: (value: any) => formatValue(value),
          setCellHeaderProps: () => ({
            style: {
              minWidth: 130,
              textAlign: "center",
              fontWeight: 600,
            },
          }),
          setCellProps: () => ({
            style: {
              minWidth: 130,
              whiteSpace: "nowrap",
            },
          }),
        },
      }));
  }, [data]);

  const consultarHistorial = async () => {
    const isValid = await formularioHistorial.trigger();

    if (!isValid) return;

    try {
      setLoading(true);

      const values = formularioHistorial.getValues();

      const request = {
        numeroCredito: Number(producto?.numeroCredito ?? 0),
        tipoCredito: Number(
          producto?.tipoCredito ?? producto?.idTipoCredito ?? 1
        ),
        fechaInicial: buildDate(values.fechaInicial),
        fechaFinal: buildDate(values.fechaFinal, true),
      };

      const response =
        await carteraApi.postObtenerHistorialCausacionInteresCorriente(
          request
        );

      if (Array.isArray(response?.data)) {
        setData(response.data);
      } else {
        setData([]);
      }
    } catch (error) {
      console.log("Error consultando historial:", error);
      setData([]);
    } finally {
      setLoading(false);
    }
  };

  const exportToExcel = () => {
    const rows = data.map((item) => {
      const row: any = {};

      Object.keys(item || {}).forEach((key) => {
        if (key !== "fechaCreacion" && key !== "fechaUltimaActualizacion") {
          row[formatHeader(key)] = formatValue(item[key]);
        }
      });

      return row;
    });

    const worksheet = XLSX.utils.json_to_sheet(rows);
    const workbook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(workbook, worksheet, "Historial saldos");

    XLSX.writeFile(
      workbook,
      `historial_saldos_${producto?.numeroCredito ?? "crédito"}.xlsx`
    );
  };

  const limpiarCerrar = () => {
    formularioHistorial.reset({
      fechaInicial: null,
      fechaFinal: null,
    });

    setData([]);
    onClose();
  };

  const options: any = {
    responsive: "scrollMaxHeight",
    tableBodyHeight: "360px",
    tableBodyMaxHeight: "360px",
    selectableRows: "none",
    print: false,
    download: true,
    viewColumns: true,
    search: true,
    filter: false,
    rowsPerPage: 10,
    rowsPerPageOptions: [10, 25, 50, 100],
    onDownload: () => {
      exportToExcel();
      return false;
    },
    setTableProps: () => ({
      style: {
        minWidth: "1500px",
      },
    }),
    textLabels: {
      body: {
        noMatch: "No hay información para mostrar",
        toolTip: "Ordenar",
      },
      pagination: {
        next: "Siguiente",
        previous: "Anterior",
        rowsPerPage: "Filas por página:",
        displayRows: "de",
      },
      toolbar: {
        search: "Buscar",
        downloadCsv: "Descargar Excel",
        viewColumns: "Ver columnas",
        filterTable: "Filtrar tabla",
      },
      viewColumns: {
        title: "Mostrar columnas",
        titleAria: "Mostrar/Ocultar columnas",
      },
    },
  };

  let content: React.ReactNode;

  if (loading) {
    content = (
      <Box display="flex" justifyContent="center" padding={4}>
        <CircularProgress />
      </Box>
    );
  } else {
    content = (
      <MUIDataTable
        title=""
        data={data}
        columns={columns}
        options={options}
      />
    );
  }

  return (
    <Dialog  open={open}
    onClose={limpiarCerrar}
    maxWidth="lg"
    fullWidth
    fullScreen={false}
    disableEnforceFocus
    PaperProps={{
      style: {
        margin: "auto",
      },
    }}>
      <DialogTitle>Historial de saldos diarios</DialogTitle>

      <DialogContent>
        <Typography variant="body2" color="textSecondary">
          Crédito: {producto?.numeroCredito ?? "N/A"}
        </Typography>

        <Box marginTop={2} marginBottom={2}>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={12} md={4}>
              <Controller
                name="fechaInicial"
                control={formularioHistorial.control}
                rules={{
                  required: "Este campo es requerido",
                }}
                render={({ field: { onChange, value }, fieldState: { error } }) => (
                  <CHDatePicker
                    error={!!error}
                    label="Fecha inicial*"
                    value={value}
                    invalidDateMessage="Fecha no válida"
                    maxDateMessage="La fecha no debe ser posterior a la actual"
                    onChange={onChange}
                    maxDate={new Date()}
                  />
                )}
              />
            </Grid>

            <Grid item xs={12} md={4}>
              <Controller
                name="fechaFinal"
                control={formularioHistorial.control}
                rules={{
                  required: "Este campo es requerido",
                  validate: (value) => {
                    if (!value || !fechaInicialWatch) return true;

                    const fechaFinal = new Date(value);
                    const fechaInicial = new Date(fechaInicialWatch);

                    if (fechaFinal < fechaInicial) {
                      return "La fecha final no puede ser menor a la inicial";
                    }

                    return true;
                  },
                }}
                render={({ field: { onChange, value }, fieldState: { error } }) => (
                  <CHDatePicker
                    error={!!error}
                    label="Fecha final*"
                    value={value}
                    invalidDateMessage="Fecha no válida"
                    minDateMessage="La fecha final no puede ser menor a la inicial"
                    maxDateMessage="La fecha no debe ser posterior a la actual"
                    onChange={onChange}
                    minDate={fechaInicialWatch}
                    maxDate={new Date()}
                  />
                )}
              />
            </Grid>

            <Grid item xs={12} md={4}>
              <Button
                fullWidth
                variant="contained"
                color="primary"
                onClick={consultarHistorial}
                disabled={loading}
              >
                Consultar
              </Button>
            </Grid>
          </Grid>
        </Box>

        {content}
      </DialogContent>

      <DialogActions>
        <Button onClick={limpiarCerrar} color="primary">
          Cerrar
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default ModalHistorialSaldosDiarios;