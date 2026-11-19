import React, { useEffect, useMemo, useState } from "react";
import {
    Box,
    Button,
    Checkbox,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Grid,
    IconButton,
    InputAdornment,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TextField,
    Typography,
    Chip,
    makeStyles,
} from "@material-ui/core";
import SearchIcon from "@material-ui/icons/Search";
import ClearIcon from "@material-ui/icons/Clear";
import AddIcon from "@material-ui/icons/Add";
import EditIcon from "@material-ui/icons/Edit";
import CatalogApi from "../../services/api-catalog.service";
import AuthService from "../../services/auth.service";

interface ConceptoFinanciero {
    id: number;
    descripcion: string;
    sigla: string;
    estado: string;
    seDebeTenerEnCuenta: boolean;
}
const useStyles = makeStyles(() => ({
    root: {
      padding: "24px 32px",
      backgroundColor: "#f3f5f9",
      minHeight: "100vh",
    },
    header: {
      marginBottom: 18,
      padding: "0 0 12px 0",
    },
    title: {
      display: "inline-block",
      fontWeight: 700,
      fontSize: 16,
      color: "#ffffff",
      backgroundColor: "#2f5597",
      padding: "3px 6px",
    },
    subtitle: {
      marginTop: 8,
      borderBottom: "1px solid #9e9e9e",
      paddingBottom: 8,
    },
    addButton: {
      backgroundColor: "#244b96",
      color: "#fff",
      fontWeight: 500,
      minWidth: 160,
      height: 38,
      borderRadius: 3,
      boxShadow: "0 3px 6px rgba(0,0,0,0.25)",
      "&:hover": {
        backgroundColor: "#1d3f7f",
      },
    },
    mainPaper: {
      padding: 16,
      borderRadius: 12,
      backgroundColor: "#ffffff",
      border: "1px solid #ddd",
      boxShadow: "0 2px 6px rgba(0,0,0,0.18)",
    },
    tablePaper: {
      padding: 14,
      borderRadius: 12,
      backgroundColor: "#ffffff",
      boxShadow: "0 1px 6px rgba(0,0,0,0.12)",
    },
    searchBox: {
      marginBottom: 14,
      "& .MuiInputBase-root": {
        fontSize: 13,
      },
    },
    tableContainer: {
      borderRadius: 6,
      overflow: "hidden",
      border: "1px solid #ffffff",
    },
    tableHead: {
      backgroundColor: "#ffc914",
    },
    headCell: {
      fontWeight: 500,
      textAlign: "center",
      color: "#34495e",
      fontSize: 12,
      padding: "16px 10px",
      borderRight: "1px solid #ffffff",
    },
    bodyCell: {
      textAlign: "center",
      fontSize: 12,
      color: "#34495e",
      padding: "12px 10px",
      borderRight: "1px solid #ffffff",
      borderBottom: "1px solid #ffffff",
    },
    rowGray: {
      backgroundColor: "#bfbfbf",
    },
    rowLight: {
      backgroundColor: "#f1f2f5",
    },
    activo: {
      color: "#1fae4b",
      fontWeight: 700,
      backgroundColor: "transparent",
    },
    inactivo: {
      color: "#d32f2f",
      fontWeight: 700,
      backgroundColor: "transparent",
    },
    actions: {
      display: "flex",
      justifyContent: "center",
    },
    actionButton: {
      color: "#000000",
    },
    dialogTitle: {
      fontWeight: 700,
      textAlign: "center",
    },
    dialogPaper: {
        borderRadius: 14,
      },
      
      dialogTitleBox: {
        backgroundColor: "#244b96",
        color: "#ffffff",
        padding: "12px 20px",
      },
      
      cancelButton: {
        color: "#555",
        fontWeight: 700,
      },
      
      saveButton: {
        backgroundColor: "#244b96",
        color: "#fff",
        fontWeight: 700,
        "&:hover": {
          backgroundColor: "#1d3f7f",
        },
      },
  }));

function ParametrizacionFinanciera() {
    const classes = useStyles();
    const apiCatalogo = CatalogApi();

    const [conceptos, setConceptos] = useState<ConceptoFinanciero[]>([]);
    const [buscar, setBuscar] = useState("");
    const [open, setOpen] = useState(false);
    const [editando, setEditando] = useState<ConceptoFinanciero | null>(null);
    const [descripcion, setDescripcion] = useState("");
    const [seDebeTenerEnCuenta, setSeDebeTenerEnCuenta] = useState(false);

    const conceptosFiltrados = useMemo(
        () =>
            conceptos.filter(
                (item) =>
                    item.descripcion.toLowerCase().includes(buscar.toLowerCase()) ||
                    item.sigla.toLowerCase().includes(buscar.toLowerCase())
            ),
        [conceptos, buscar]
    );

    const obtenerConceptosNomina = async () => {
        try {
            const response = await apiCatalogo.getObtenerConceptoNominaParam();

            const data = response?.data || [];

            const conceptosMapeados = data.map((item: any) => ({
                id: item.id,
                descripcion: item.descripcion || "",
                sigla: item.sigla || "",
                estado: item.estado || "0",
                seDebeTenerEnCuenta: String(item.estado) === "1",
            }));

            setConceptos(conceptosMapeados);
        } catch (error) {
            console.log("Error consultando conceptos de nómina", error);
        }
    };




    useEffect(() => {
        obtenerConceptosNomina();
    }, []);

    const abrirAgregar = () => {
        setEditando(null);
        setDescripcion("");
        setSeDebeTenerEnCuenta(false);
        setOpen(true);
    };

    const abrirEditar = (item: ConceptoFinanciero) => {
        setEditando(item);
        setDescripcion(item.descripcion);
        setSeDebeTenerEnCuenta(item.seDebeTenerEnCuenta);
        setOpen(true);
    };

    const guardar = async () => {
        if (!descripcion.trim()) return;

        const user = AuthService.getUserInfo();

        const request = {
            ...(editando && { id: editando.id }),
            descripcion: descripcion.trim(),
            sigla: descripcion.trim(),
            estado: String(seDebeTenerEnCuenta),
            creadoPor: user?.USR_ID,
        };

        try {
            const response = editando
                ? await apiCatalogo.putActualizarConceptoNomina(request)
                : await apiCatalogo.postInsertarConceptoNomina(request);

            if (response?.isSuccess || response?.data?.isSuccess) {
                await obtenerConceptosNomina();
                setOpen(false);
            } else {
                console.log(response?.returnMessage || response?.data?.returnMessage);
            }
        } catch (error) {
            console.log("Error guardando concepto de nómina", error);
        }
    };



    useEffect(() => {
        const User = AuthService.getUserInfo();
        console.log({ User })
    }, []);

    return (
        <Box className={classes.root}>
          <Paper className={classes.mainPaper}>
            <Box className={classes.header}>
              <Typography className={classes.title}>
                Parametrización financiera
              </Typography>
      
              <Typography
                variant="body2"
                color="textSecondary"
                className={classes.subtitle}
              >
                Administración de conceptos financieros
              </Typography>
            </Box>
      
            <Button
              className={classes.addButton}
              variant="contained"
              startIcon={<AddIcon />}
              onClick={abrirAgregar}
            >
              Agregar
            </Button>
      
            <Paper elevation={0} className={classes.tablePaper} style={{ marginTop: 18 }}>
              <TextField
                className={classes.searchBox}
                fullWidth
                placeholder="Buscar por concepto o sigla"
                value={buscar}
                onChange={(e) => setBuscar(e.target.value)}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <SearchIcon />
                    </InputAdornment>
                  ),
                  endAdornment: buscar ? (
                    <InputAdornment position="end">
                      <IconButton size="small" onClick={() => setBuscar("")}>
                        <ClearIcon />
                      </IconButton>
                    </InputAdornment>
                  ) : null,
                }}
              />
      
              <Paper variant="outlined" className={classes.tableContainer}>
                <Table size="small">
                  <TableHead className={classes.tableHead}>
                    <TableRow>
                      <TableCell className={classes.headCell}>Concepto</TableCell>
                      <TableCell className={classes.headCell}>Sigla</TableCell>
                      <TableCell className={classes.headCell}>Estado</TableCell>
                      <TableCell className={classes.headCell}>Acciones</TableCell>
                    </TableRow>
                  </TableHead>
      
                  <TableBody>
                    {conceptosFiltrados.map((item, index) => (
                      <TableRow
                        hover
                        key={item.id}
                        className={index % 2 === 0 ? classes.rowGray : classes.rowLight}
                      >
                        <TableCell className={classes.bodyCell}>
                          {item.descripcion}
                        </TableCell>
      
                        <TableCell className={classes.bodyCell}>
                          {item.sigla}
                        </TableCell>
      
                        <TableCell className={classes.bodyCell}>
                          <Chip
                            size="small"
                            label={item.seDebeTenerEnCuenta ? "ACTIVO" : "INACTIVO"}
                            className={
                              item.seDebeTenerEnCuenta
                                ? classes.activo
                                : classes.inactivo
                            }
                          />
                        </TableCell>
      
                        <TableCell className={classes.bodyCell}>
                          <Box className={classes.actions}>
                            <IconButton
                              size="small"
                              className={classes.actionButton}
                              onClick={() => abrirEditar(item)}
                            >
                              <EditIcon />
                            </IconButton>
                          </Box>
                        </TableCell>
                      </TableRow>
                    ))}
      
                    {conceptosFiltrados.length === 0 && (
                      <TableRow>
                        <TableCell colSpan={4} align="center">
                          No se encontraron conceptos
                        </TableCell>
                      </TableRow>
                    )}
                  </TableBody>
                </Table>
              </Paper>
            </Paper>
          </Paper>
      
          <Dialog
            open={open}
            onClose={() => setOpen(false)}
            maxWidth="xs"
            fullWidth
            classes={{ paper: classes.dialogPaper }}
          >
            <DialogTitle className={classes.dialogTitleBox}>
              <Typography className={classes.dialogTitle}>
                {editando ? "Editar Concepto" : "Agregar Concepto"}
              </Typography>
            </DialogTitle>
      
            <DialogContent>
              <TextField
                fullWidth
                label="Descripción"
                required
                value={descripcion}
                onChange={(e) => setDescripcion(e.target.value)}
                margin="normal"
              />
      
              <Box display="flex" alignItems="center" mt={2}>
                <Typography>Estado</Typography>
                <Checkbox
                  checked={seDebeTenerEnCuenta}
                  onChange={(e) => setSeDebeTenerEnCuenta(e.target.checked)}
                  color="primary"
                />
              </Box>
            </DialogContent>
      
            <DialogActions>
              <Button className={classes.cancelButton} onClick={() => setOpen(false)}>
                Cancelar
              </Button>
      
              <Button
                variant="contained"
                className={classes.saveButton}
                onClick={guardar}
                disabled={!descripcion.trim()}
              >
                {editando ? "Guardar" : "Agregar"}
              </Button>
            </DialogActions>
          </Dialog>
        </Box>
      );
}

export default ParametrizacionFinanciera;