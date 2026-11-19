import React, { useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    Typography,
    FormControl,
    InputLabel,
    MenuItem,
    Select,
    TableContainer,
    Table,
    TableHead,
    TableRow,
    TableCell,
    TableBody,
    Checkbox,
    Paper
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ModalConceptosCuenta } from '../conceptos-cuenta/modalConceptosCuenta';
import { ConceptoCuenta } from '../types';
import ModalInconsistencias from '../inconsistenciasModal/modalInconsistencias';
import ApiClients from '../../../../services/api-clients.service';
import { startGetMovementsAccountsClient } from '../../../../actions/client.action';
import { AppState } from '../../../../store/rootReducer';


const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
    },
    buttonprincipal: {
        width: '250px',
        height: '50px',
        marginLeft: '1rem',
        background: '#FEC917',
    },
    buttonsecundario: {
        width: '250px',
        height: '50px',
        marginLeft: '1rem',
        background: '#43b5f1',
    },
    buttonadd: {
        width: '250px',
        height: '50px',
        marginRight: '1rem',
        background: '#43b5f1',
    },
    titleform: {
        width: '100%',
        textAlign: 'center',
        color: '#1C3F6B',
        padding: 1,
        fontWeight: 'bold',
        fontSize: '1rem',
    },
    column: {
        padding: '10px',
        textAlign: 'center'
    },
    select: {

        textAlign: 'left',
        paddingRight: '100px'

    },
    tableCellBlueText: {
        color: 'blue',
    },
}));



function ModalSolucionInconsistencias({
    open,
    onClose,
    responseAdminCuenta,
}: {
    open: any;
    onClose: any;
    responseAdminCuenta: any;
}): any {
    const classes = useStyles();

    const { control } = useForm({
        defaultValues: {
            radicado: '',
            tipoIdentificacion: '',
            numeroIdentificacion: 0,
            primerNombre: '',
            segundoNombre: '',
            primerApellido: '',
            segundoApellido: '',
            cuenta: '',
            cuentaNo: 0,
            valorTotal: 0,
            total: 0,
            observaciones: '',
        },
    });

    const [motivoTraslado, setMotivoTraslado] = useState('');
    const [conceptosCuentaModalOpen, setConceptosCuentaModalOpen] = useState(false);
    const [inconsistenciasModalOpen, setInconsistenciasModalOpen] = useState(false);
    const [conceptosList, setConceptosList] = useState<ConceptoCuenta[]>([]);
    const [conceptos, setConceptos] = useState<string>('');
    const [monto, setMonto] = useState<number>(0);
    const [cuotas, setCuotas] = useState<number>(0);
    const [selectAll, setSelectAll] = useState(false);
    const [selectedCuenta, setSelectedCuenta] = useState('');
    const [saldoDisponible, setSaldoDisponible] = useState('');
    const [cuentas, setCuentas] = useState<any[]>([]);

    const handleCuentaOrigenChange = (event: React.ChangeEvent<{ name?: string | undefined; value: unknown; }>) => {
        const selectedValue = event.target.value as string;
        setSelectedCuenta(selectedValue);

    };

    const obtenerSaldo = (cuenta: string) => {
        const cuentaSeleccionada = responseAdminCuenta.cuentas.find((c: { numeroCuenta: any; }) => c.numeroCuenta === cuenta);
        return { saldoDisponibleCuenta: cuentaSeleccionada ? cuentaSeleccionada.saldoDisponible : '' };

    };

    useEffect(() => {
        const saldoResponse = obtenerSaldo(selectedCuenta);
        setSaldoDisponible(saldoResponse.saldoDisponibleCuenta);
    }, [selectedCuenta]);

    const handleOpenConceptosCuentaModal = () => {
        setConceptosCuentaModalOpen(true);
    };

    const handleOpenInconsistenciasModal = () => {
        setInconsistenciasModalOpen(true);
    }

    const handleCloseInconsistenciasModal = () => {
        setInconsistenciasModalOpen(false);
    }

    const handleAddConcepto = (nuevoConcepto: ConceptoCuenta) => {
        setConceptosList([...conceptosList, nuevoConcepto]);
    };

    const handleSelectAll = () => {
        const updatedConceptosList = conceptosList.map((item) => ({
            ...item,
            selected: !selectAll,
        }));
        setConceptosList(updatedConceptosList);
        setSelectAll(!selectAll);
    };

    const handleSelect = (id: any) => {
        const updatedConceptosList = conceptosList.map((item) =>
            item.id === id ? { ...item, selected: !item.selected } : item
        );
        setConceptosList(updatedConceptosList);
    };

    interface Row {
        id: number;
        afiliado: string;
        unidadEjecutora: string;
        valor: string;
        interes: string;
        concepto: string;
        vigencia: string;
      }
    const [rows, setRows] = useState<Row[]>([]);

    const addRow = (afiliado: any, unidadEjecutora: any) => {
      const newRow = {
        id: rows.length + 1,
        afiliado,
        unidadEjecutora,
        valor: '',
        interes: '',
        concepto: '',
        vigencia: ''
      };
  
      setRows([...rows, newRow]);
    };

    const handleAddRowFromModal = (afiliado: string, unidadEjecutora: string) => {
        addRow(afiliado, unidadEjecutora);
      };
      


    return (
        <>
            <Dialog
                fullWidth
                maxWidth="lg"
                open={open}
                onClose={onClose}
                aria-labelledby="form-dialog-title"
            >
                <DialogContent>
                    <Grid item>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            SOLUCIÓN INCONSISTENCIAS
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container spacing={2}>
                        <Grid item xs={6}>
                            <Controller
                                name="radicado"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Radicado"
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container justify="center">
                        <Grid item xs={12}>
                            <Typography className={classes.titleform} variant="h5">
                                Información del Afiliado
                            </Typography>
                        </Grid>
                        <br />
                        <br />

                        <Grid container spacing={2}>
                            <Grid item xs={6}>
                                <Controller
                                    name="tipoIdentificacion"
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value } }) => (

                                        <CHTextField
                                            label="Tipo de identificación"
                                            isReadOnly
                                            value={responseAdminCuenta.tipoIdentificacion}

                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <Controller
                                    name="numeroIdentificacion"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (

                                        <CHTextField
                                            label="Numero de identificación"
                                            isReadOnly
                                            value={responseAdminCuenta.identificacion}

                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <Controller
                                    name="primerNombre"
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value } }) => (
                                        <CHTextField
                                            label="Primer Nombre"
                                            isReadOnly
                                            value={responseAdminCuenta.primerNombre}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <Controller
                                    name="segundoNombre"
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value } }) => (
                                        <CHTextField
                                            label="Segundo Nombre"
                                            isReadOnly
                                            value={responseAdminCuenta.segundoNombre}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <Controller
                                    name="primerApellido"
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value } }) => (
                                        <CHTextField
                                            label="Primer Apellido"
                                            isReadOnly
                                            value={responseAdminCuenta.primerApellido}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <Controller
                                    name="segundoApellido"
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value } }) => (
                                        <CHTextField
                                            label="Segundo Apellido"
                                            isReadOnly
                                            value={responseAdminCuenta.segundoApellido}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item xs={7}>
                                <Controller
                                    name="cuenta"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <FormControl className={classes.select} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                                            <InputLabel>Cuenta No.</InputLabel>
                                            <Select
                                                value={selectedCuenta}
                                                onChange={(event) => {
                                                    const selectedValue = String(event.target.value);
                                                    setSelectedCuenta(selectedValue);
                                                    onChange(event);
                                                    handleCuentaOrigenChange(event);

                                                }}
                                            >
                                                {responseAdminCuenta.cuentas.map((cuenta: any) => (
                                                    <MenuItem key={cuenta.numeroCuenta} value={cuenta.numeroCuenta}>
                                                        {cuenta.nombreCuenta}
                                                    </MenuItem>
                                                ))}
                                            </Select>
                                        </FormControl>
                                    )}
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <Controller
                                    name="cuentaNo"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (
                                        <CHTextField
                                            label="Cuenta No:"
                                            value={selectedCuenta}
                                            disabled
                                            onChange={(event: any) => {
                                                const numericValue = event.target.value.replace(/\D/g, '');
                                                onChange(numericValue);
                                            }}
                                            fullWidth
                                            InputProps={{
                                                inputProps: {
                                                    inputMode: 'numeric',
                                                    pattern: '[0-9]*',
                                                },
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <Controller
                                    name="valorTotal"
                                    control={control}
                                    render={({ field: { onChange, value } }) => (

                                        <CHTextField
                                            label="Valor Total:"
                                            currencyMask
                                            isReadOnly
                                            value={saldoDisponible}
                                            onChange={onChange}
                                            InputLabelProps={{
                                                shrink: true,
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <br />
                    <br />
                    <Grid container justify="center">
                        <Grid item xs={12}>
                            <Typography className={classes.titleform} variant="h5">
                                Inconsistencias
                            </Typography>
                        </Grid>
                        <br />
                        <br />
                        <Grid container justify="flex-start" spacing={2}>
                            <Grid item>
                                <Button
                                    className={classes.buttonadd}
                                    onClick={handleOpenInconsistenciasModal}
                                >
                                    Adicionar
                                </Button>
                            </Grid>
                        </Grid>

                        <div className={classes.root}>
                            <TableContainer component={Paper}>
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>
                                                <Checkbox />
                                            </TableCell>
                                            <TableCell>Afiliado</TableCell>
                                            <TableCell>Unidad Ejecutora</TableCell>
                                            <TableCell>Valor</TableCell>
                                            <TableCell>Interés</TableCell>
                                            <TableCell>Concepto</TableCell>
                                            <TableCell>Vigencia</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {rows.map((row) => (
                                            <TableRow key={row.id}>
                                                <TableCell>
                                                    <Checkbox />
                                                </TableCell>
                                                <TableCell>{row.afiliado}</TableCell>
                                                <TableCell>{row.unidadEjecutora}</TableCell>
                                                <TableCell>{row.valor}</TableCell>
                                                <TableCell>{row.interes}</TableCell>
                                                <TableCell>{row.concepto}</TableCell>
                                                <TableCell>{row.vigencia}</TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                            <br />
                        </div>

                    </Grid>
                    <br />
                    <br />
                    <TableContainer>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>
                                        <Checkbox
                                            checked={selectAll}
                                            onChange={handleSelectAll}
                                        />
                                    </TableCell>
                                    <TableCell className={classes.tableCellBlueText}>INRS ID</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>TIPO INCONSISTENCIA</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>AFILIADO</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>VALOR</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>CONCEPTO</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>FECHA SOLICITUD</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>DESCRIPCIÓN</TableCell>
                                </TableRow>
                            </TableHead>
                        </Table>
                    </TableContainer>
                    <br />
                    <br />
                    <Grid container justify="flex-start" spacing={2}>
                        <Grid item>
                            <Button
                                className={classes.buttonadd}
                                onClick={handleOpenConceptosCuentaModal}
                            >
                                Aceptar
                            </Button>
                        </Grid>
                    </Grid>
                    <br />
                    <br />
                    <Grid container justify="center">
                        <Grid item xs={12}>
                            <Typography className={classes.titleform} variant="h5">
                                Destino
                            </Typography>
                        </Grid>
                        <br />
                        <br />
                        <TableContainer>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>
                                            <Checkbox
                                                checked={selectAll}
                                                onChange={handleSelectAll}
                                            />
                                        </TableCell>
                                        <TableCell>Concepto</TableCell>
                                        <TableCell>Monto</TableCell>
                                        <TableCell>Cuotas</TableCell>
                                    </TableRow>
                                </TableHead>
                                <tbody>
                                    {conceptosList.map((item) => (
                                        <TableRow key={item.id}>
                                            <TableCell>
                                                <Checkbox
                                                    checked={item.selected}
                                                    onChange={() => handleSelect(item.id)}
                                                />
                                            </TableCell>
                                            <TableCell>{item.concepto}</TableCell>
                                            <TableCell>{item.monto}</TableCell>
                                            <TableCell>{item.cuotas}</TableCell>
                                        </TableRow>
                                    ))}
                                </tbody>
                            </Table>
                        </TableContainer>
                        <Grid item xs={6}>
                            <Controller
                                name="total"
                                control={control}
                                render={({ field: { onChange, value } }) => (

                                    <CHTextField
                                        label="Total:"
                                        fullWidth
                                        currencyMask
                                    />
                                )}
                            />
                        </Grid>
                        <Grid container justify="flex-start" spacing={2}>
                            <Grid item>
                                <Button
                                    className={classes.buttonadd}
                                    onClick={handleOpenConceptosCuentaModal}
                                >
                                    Adicionar
                                </Button>
                            </Grid>
                        </Grid>

                        <Grid container spacing={2}>
                            <Grid item xs={6}>
                                <Controller
                                    name="observaciones"
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value } }) => (

                                        <CHTextField label="Observaciones" fullWidth multiline rows={4} />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <br />
                    <br />
                    <Grid container justify="flex-end" spacing={2}>
                        <Grid item>
                            <Button className={classes.buttonprincipal} onClick={onClose}>Regresar</Button>
                        </Grid>
                        <Grid item>
                            <Button className={classes.buttonsecundario}>Aceptar</Button>
                        </Grid>
                    </Grid>
                </DialogContent>

            </Dialog>

            <ModalConceptosCuenta
                open={conceptosCuentaModalOpen}
                onClose={() => setConceptosCuentaModalOpen(false)}
                onAddConcepto={handleAddConcepto}
                motivoTrasladoSeleccionado={motivoTraslado}
                setConceptos={setConceptos}
                setMonto={setMonto}
                setCuotas={setCuotas}
            />

            <ModalInconsistencias
                open={inconsistenciasModalOpen}
                onClose={handleCloseInconsistenciasModal}
                responseAdminCuenta={responseAdminCuenta}
                addRow={addRow}

            />
        </>
    )
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
});
const mapDispatchToProps = {
    startGetMovementsAccountsClient,
};
export default connect(mapStateToProps, mapDispatchToProps)(ModalSolucionInconsistencias);