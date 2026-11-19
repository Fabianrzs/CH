import React, { useEffect, useMemo, useState } from "react";
import {
  Box,
  Button,
  CircularProgress,
  Grid,
  Typography,
} from "@material-ui/core";
import { createMuiTheme, MuiThemeProvider } from "@material-ui/core/styles";
import MUIDataTable from "mui-datatables";
import * as XLSX from "xlsx";
import CarteraApi from "../../../services/api-cartera-service";
import ModalHistorialSaldosDiarios from "./ModalHistorialSaldosDiarios";

const SaldosDiariosCartera = ({
  producto,
  clienteRequest,
}: {
  producto: any;
  clienteRequest: any;
}) => {
  const carteraApi = CarteraApi();

  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [openHistorial, setOpenHistorial] = useState(false);

  const getMuiTheme = () =>
    createMuiTheme({
      overrides: {
        MUIDataTable: {
          paper: {
            boxShadow: "0px 2px 6px rgba(0,0,0,0.25)",
            borderRadius: 4,
          },
        },
        MUIDataTableToolbar: {
          root: {
            paddingLeft: 16,
            paddingRight: 16,
          },
          titleText: {
            fontSize: 18,
            fontWeight: 500,
          },
        },
        MUIDataTableHeadCell: {
          root: {
            fontWeight: 600,
            whiteSpace: "normal",
            textAlign: "center",
            fontSize: 13,
          },
        },
        MUIDataTableBodyCell: {
          root: {
            whiteSpace: "nowrap",
            fontSize: 13,
          },
        },
      },
    });

  useEffect(() => {
    if (producto?.numeroCredito) {
      obtenerSaldosCartera();
    }
  }, [producto?.numeroCredito]);

  const obtenerSaldosCartera = async () => {
    try {
      setLoading(true);

      const request = {
        numeroCredito: Number(producto?.numeroCredito ?? 0),
        tipoCredito: Number(
          producto?.tipoCredito ?? producto?.idTipoCredito ?? 1
        ),
      };

      const response = await carteraApi.postObtenerSaldosCartera(request);

      if (Array.isArray(response?.data)) {
        setData(response.data);
      } else {
        setData([]);
      }
    } catch (error) {
      console.log("Error obteniendo saldos cartera:", error);
      setData([]);
    } finally {
      setLoading(false);
    }
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

    // 🔥 columnas a ocultar
    const excluded = [
      "fechaCreacion",
      "fechaUltimaActualizacion",
    ];

    return Array.from(keys)
      .filter((key) => !excluded.includes(key))
      .map((key) => ({
        name: key,
        label: formatHeader(key),
        options: {
          filter: true,
          sort: true,
          customBodyRender: (value: any) => {
            if (key === "numeroCredito") {
              return value;
            }
          
            return formatValue(value);
          },
          setCellHeaderProps: () => ({
            style: {
              minWidth: 130,
              textAlign: "center",
            },
          }),
          setCellProps: () => ({
            style: {
              minWidth: 130,
            },
          }),
        },
      }));
  }, [data]);
  

  const exportToExcel = () => {
    const rows = data.map((item) => {
      const row: any = {};

      Object.keys(item || {}).forEach((key) => {
        const value = item[key];

        if (key === "numeroCredito") {
          row[formatHeader(key)] = String(value);
        } else if (
          typeof value === "string" &&
          /^\d{4}-\d{2}-\d{2}T/.test(value)
        ) {
          row[formatHeader(key)] = new Date(value).toLocaleDateString("es-CO");
        } else {
          row[formatHeader(key)] = value;
        }
      });

      return row;
    });

    const worksheet = XLSX.utils.json_to_sheet(rows);

    Object.keys(worksheet).forEach((cell) => {
      if (cell.startsWith("!")) return;

      const c: any = worksheet[cell];

      if (c.t === "n") {
        c.z = Number.isInteger(c.v) ? "#,##0" : "#,##0.00";
      }
    });

    const workbook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(workbook, worksheet, "Saldos diarios");

    XLSX.writeFile(
      workbook,
      `saldos_diarios_CC.${producto?.identificacion ?? "identificacion"}_CR.${producto?.numeroCredito ?? "crédito"}.xlsx`
    );
  };

  const options: any = {
    filterType: "dropdown",
    responsive: "scrollMaxHeight",
    tableBodyHeight: "430px",
    tableBodyMaxHeight: "430px",
    selectableRows: "none",
    print: false,
    download: true,
    downloadOptions: {
      filename: `saldos_diarios_${producto?.numeroCredito ?? "crédito"}.csv`,
      separator: ";",
    },
    onDownload: () => {
      exportToExcel();
      return false;
    },
    viewColumns: true,
    search: true,
    filter: false,
    rowsPerPage: 10,
    rowsPerPageOptions: [10, 25, 50, 100],
    setTableProps: () => ({
      style: {
        minWidth: "1600px",
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
        print: "Imprimir",
        viewColumns: "Ver columnas",
        filterTable: "Filtrar tabla",
      },
      viewColumns: {
        title: "Mostrar columnas",
        titleAria: "Mostrar/Ocultar columnas",
      },
    },
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" padding={4}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box style={{ padding: "12px 16px" }}>
      <Button
        variant="contained"
        color="primary"
        style={{ marginBottom: '60px' }}
        onClick={() => setOpenHistorial(true)}
      >
        Ver historial
      </Button>
      <MuiThemeProvider theme={getMuiTheme()}>
        <MUIDataTable
          title={
            <Box>
              <Typography variant="h6">Saldos diarios de cartera</Typography>
              <Typography variant="body2" color="textSecondary">
                Crédito: {producto?.numeroCredito ?? "N/A"}
              </Typography>
            </Box>
          }
          data={data}
          columns={columns}
          options={options}
        />
      </MuiThemeProvider>

      <ModalHistorialSaldosDiarios
        open={openHistorial}
        onClose={() => setOpenHistorial(false)}
        producto={producto}
      />


    </Box>


  );
};

export default SaldosDiariosCartera;