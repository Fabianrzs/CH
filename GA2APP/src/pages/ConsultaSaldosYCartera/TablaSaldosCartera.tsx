import React, { useMemo, useState } from 'react';
import {
  Checkbox,
  FormControlLabel,
  IconButton,
  InputAdornment,
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
  Button,
  makeStyles,
} from '@material-ui/core';
import SearchIcon from '@material-ui/icons/Search';
import ViewColumnIcon from '@material-ui/icons/ViewColumn';

const useStyles = makeStyles((theme) => ({
  wrapper: {
    width: '100%',
  },
  toolbar: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    gap: theme.spacing(2),
    marginBottom: theme.spacing(2),
    flexWrap: 'wrap',
  },
  searchBox: {
    minWidth: 280,
    flex: 1,
    maxWidth: 420,
  },
  tableContainer: {
    width: '100%',
    overflowX: 'auto',
  },
  headCell: {
    fontWeight: 700,
    whiteSpace: 'nowrap',
  },
  bodyCell: {
    whiteSpace: 'nowrap',
  },
}));

interface Props {
  data: any[];
  watch: any;
  UtilService: any;
}

type ColumnKey =
  | 'numeroObligacion'
  | 'identificacion'
  | 'nombre'
  | 'saldoTotal'
  | 'tipoCredito'
  | 'provision';

const TablaSaldosCartera = ({ data = [], watch, UtilService }: Props) => {
  const classes = useStyles();

  const [search, setSearch] = useState('');
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const [visibleColumns, setVisibleColumns] = useState<Record<ColumnKey, boolean>>({
    numeroObligacion: true,
    identificacion: true,
    nombre: true,
    saldoTotal: true,
    tipoCredito: true,
    provision: true,
  });

  const getSaldoTotal = (element: any) => {
    if (watch('tipoCredito') === 1) return element.saldoLeasing ?? 0;
    if (watch('tipoCredito') === 2) return element.saldoHipotecario ?? 0;
    return (element.saldoHipotecario ?? 0) + (element.saldoLeasing ?? 0);
  };

  const columns = [
    {
      key: 'numeroObligacion' as ColumnKey,
      label: 'Número De Obligación',
      getValue: (row: any) => row.numeroObligacion ?? '',
    },
    {
      key: 'identificacion' as ColumnKey,
      label: 'Cédula Del Afiliado',
      getValue: (row: any) => row.identificacion ?? '',
    },
    {
      key: 'nombre' as ColumnKey,
      label: 'Nombre Del Afiliado',
      getValue: (row: any) => row.nombre ?? '',
    },
    {
      key: 'saldoTotal' as ColumnKey,
      label: 'Saldo Total',
      getValue: (row: any) => getSaldoTotal(row),
      render: (row: any) => UtilService.formatCurrency(getSaldoTotal(row)),
    },
    {
        key: 'tipoCredito' as ColumnKey,
        label: 'Tipo Crédito',
        getValue: (row: any) => row.tipoCredito,
        
      },
    {
      key: 'provision' as ColumnKey,
      label: 'Provisión',
      getValue: (row: any) => row.provision ?? 0,
      render: (row: any) => UtilService.formatCurrency(row.provision ?? 0),
    },
  ];

  const visibleColumnList = columns.filter((col) => visibleColumns[col.key]);

  const filteredData = useMemo(() => {
    const term = search.trim().toLowerCase();

    if (!term) return data || [];

    return (data || []).filter((row: any) =>
      columns.some((col) => {
        const value = col.getValue(row);
        return String(value ?? '').toLowerCase().includes(term);
      })
    );
  }, [data, search, watch]);

  const paginatedData = useMemo(
    () => filteredData.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage),
    [filteredData, page, rowsPerPage]
  );

  const handleChangePage = (_event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleOpenColumnsMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleCloseColumnsMenu = () => {
    setAnchorEl(null);
  };

  const handleToggleColumn = (key: ColumnKey) => {
    setVisibleColumns((prev) => ({
      ...prev,
      [key]: !prev[key],
    }));
  };

  return (
    <div style={{marginTop: '80px'}} className={classes.wrapper}>
      <div className={classes.toolbar}>
        <TextField
          className={classes.searchBox}
          variant="outlined"
          size="small"
          placeholder="Buscar por obligación, cédula, nombre, saldo o provisión"
          value={search}
          onChange={(e) => {
            setSearch(e.target.value);
            setPage(0);
          }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
          }}
        />

        <Button
          variant="outlined"
          color="primary"
          startIcon={<ViewColumnIcon />}
          onClick={handleOpenColumnsMenu}
        >
          Columnas
        </Button>

        <Menu
          anchorEl={anchorEl}
          keepMounted
          open={Boolean(anchorEl)}
          onClose={handleCloseColumnsMenu}
        >
          {columns.map((col) => (
            <MenuItem key={col.key} onClick={() => handleToggleColumn(col.key)}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={visibleColumns[col.key]}
                    color="primary"
                  />
                }
                label={col.label}
              />
            </MenuItem>
          ))}
        </Menu>
      </div>

      <TableContainer component={Paper} className={classes.tableContainer}>
        <Table>
          <TableHead style={{backgroundColor: '#fec917'}}>
            <TableRow>
              {visibleColumnList.map((col) => (
                <TableCell key={col.key} className={classes.headCell}>
                  {col.label}
                </TableCell>
              ))}
            </TableRow>
          </TableHead>

          <TableBody>
            {paginatedData.length > 0 ? (
              paginatedData.map((row: any, index: number) => (
                <TableRow key={`table-${index}`}>
                  {visibleColumnList.map((col) => (
                    <TableCell key={col.key} className={classes.bodyCell}>
                      {col.render ? col.render(row) : col.getValue(row)}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={visibleColumnList.length || 1} align="center">
                  No se encontraron resultados
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>

        <TablePagination
          component="div"
          count={filteredData.length}
          page={page}
          onPageChange={handleChangePage}
          rowsPerPage={rowsPerPage}
          onRowsPerPageChange={handleChangeRowsPerPage}
          rowsPerPageOptions={[5, 10, 20, 50]}
          labelRowsPerPage="Filas por página"
        />
      </TableContainer>
    </div>
  );
};

export default TablaSaldosCartera;