import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
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
    Table,
    TableHead,
    TableCell,
    TableRow,
    TableBody,
    TableContainer
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import { ClientState } from '../../../../reducers/client.reducer';
import ApiClients from '../../../../services/api-clients.service';
import { startGetMovementsAccountsClient } from '../../../../actions/client.action';
import { AppState } from '../../../../store/rootReducer';
import CatalogApi from '../../../../services/api-catalog.service';
import { LoadingComponent } from '../../../../components/loading/loading.component';




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
        paddingRight: '100px',
        width: '100px',

    },
    camposValores: {
        width: '100px',
    },
    camposValoresDos: {
        width: '100px',
        paddingLeft: '2rem',
    },
    headerCell: {
        textAlign: 'center',
        color: '#1C3F6B'
    }
}));

const api = ApiClients();

function ModalTransferenciaCuentas({
    open,
    onClose,
    responseAdminCuenta,
    saldoCuentas,
    catalogState,
}: {
    open: any;
    onClose: any;
    responseAdminCuenta: any;
    saldoCuentas: any;
    catalogState: any;
}): any {

    const classes = useStyles();
    const catalogApi = CatalogApi();
    const { control, handleSubmit, setValue, getValues, watch } = useForm({
        defaultValues: {
            tipoDocumentoId: '',
            numeroDocumento: '',
            typerequest: '',
            cuentaDestino: '',
            cuentaDestinoNo: '',
            valorTotal: 0,
            radicado: '',
            tipoSolicitud: '',
            modalidad: '',
            comprobante: '',
            tipoIdentificacion: '',
            numeroIdentificacion: 0,
            primerNombre: '',
            segundoNombre: '',
            primerApellido: '',
            segundoApellido: '',
            cuentaOrigen: '',
            cuentaNo: 0,
            fuerza: '',
            unidadEjecutora: '',
            grado: '',
            saldoDisponible: 0,
            montoTotal: 0,
            motivoTraslado: '',
            tipoCuentas: '',
            saldo: 0,
            monto: 0,
            montoDestino: 0,
            tipoCuentaDestino: '',
            observaciones: '',
            descripcionConcepto: ''
        },
    });


    const [tipoSolicitud, setTipoSolicitud] = useState('');
    const [cuentaOrigen, setCuentaOrigen] = useState('');
    const [motivoTraslado, setMotivoTraslado] = useState('');
    const [obtenerTipoDocumento, setObtenerTipoDocumento] = useState<any[]>([]);
    const [selectedCuenta, setSelectedCuenta] = useState('');
    const [selectedSaldo, setSelectedSaldo] = useState('');
    const [saldoDisponible, setSaldoDisponible] = useState('');
    const [cuentaSeleccionada, setCuentaSeleccionada] = useState('');
    const [cuentaSeleccionadaDestino, setCuentaSeleccionadaDestino] = useState('');
    const [searchResults, setSearchResults] = useState<TableRows[]>([]);

    interface TableRows {
        selected: boolean;
        primerNombre: string;
        primerApellido: string;
        numeroCuenta: string;
        nombreCuenta: string;
        estadoCuenta: string;
        fechaCreacion: string | null;
    }

    const obtenerSaldo = (cuenta: string) => {
        const cuentaSeleccionada = responseAdminCuenta.cuentas.find((c: { numeroCuenta: any; }) => c.numeroCuenta === cuenta);
        return { saldoDisponibleCuenta: cuentaSeleccionada ? cuentaSeleccionada.saldoDisponible : '' };
    };

    const [loading, setLoading] = useState<boolean>(false);
    const [opcionesCuentas, setOpcionesCuentas] = useState<Cuenta[]>([]);
    const [tiposCuentas, setTiposCuentas] = useState<Cuenta[]>([]);
    const [numeroCuenta, setNumeroCuenta] = useState('');
    const [fuerzaAfiliado, setFuerzaAfiliado] = useState('');
    const [unidadEjecutoraAfiliado, setUnidadEjecutoraAfiliado] = useState('');
    const [gradoAfiliado, setGradoAfiliado] = useState('');
    const [estadoCuenta, setEstadoCuenta] = useState<{ [key: string]: string }>({});
    const [valorTotal, setValorTotal] = useState(0);
    const [valorTotalDestino, setValorTotalDestino] = useState(0);
    const saldosPorConcepto: Record<string, number> = {};
    const identificationType = watch('tipoDocumentoId');

    useEffect(() => {
        calcularValorTotalDestino();
    }, []);


    interface Concepto {
        descripcionConcepto: string;
        saldo: number;
        monto: number;
    }

    const saldos: number[] = Object.values(saldosPorConcepto);


    const searchAdministracionCuentas = async () => {
        try {
            setLoading(true);
            const formData = getValues();
            const response = await api.getAdministracionCuentasAfiliado({
                numeroDocumento: formData.numeroDocumento,
                tipoDocumentoId: parseInt(formData.tipoDocumentoId, 10)
            })

            const cuentas = response.data.cuentas || [];
            const fuerza = response.data.fuerza || '';
            const unidadEjecutora = response.data.unidadEjecutora || '';
            const grado = response.data.grado || '';
            const estadosCuenta: { [key: string]: string } = {};

            cuentas.forEach((cuenta: any) => {
                estadosCuenta[cuenta.nombreCuenta] = cuenta.estadoCuenta || '';
            });

            setOpcionesCuentas(cuentas);
            setFuerzaAfiliado(fuerza);
            setUnidadEjecutoraAfiliado(unidadEjecutora);
            setGradoAfiliado(grado);
            setEstadoCuenta(estadosCuenta);
            setTiposCuentas(cuentas);

            setSearchResults(response.data);

            toast('Afiliado encontrado, seleccione una cuenta de destino');
        } catch (error: any) {
            toast.error(`Error al buscar datos: ${error.message}`, { type: toast.TYPE.ERROR });
        }
        setLoading(false);
    };

    interface Cuenta {
        id: number;
        nombreCuenta: string;
        numeroCuenta: string;
        conceptos?: Concepto[];
    }

    interface Concepto {
        id?: number;
        descripcionConcepto: string;
    }

    const handleCuentaDestinoChange = (event: any) => {
        const selectedCuentaDestino = event.target.value;

        setValue('cuentaDestino', selectedCuentaDestino);


        const cuentaSeleccionada = opcionesCuentas.find((cuenta) => cuenta.nombreCuenta === selectedCuentaDestino);

        setNumeroCuenta(cuentaSeleccionada ? cuentaSeleccionada.numeroCuenta : '');
    };


    useEffect(() => {
        if (saldoCuentas && selectedCuenta) {
            const saldo = saldoCuentas[selectedCuenta] || 0;
            setSelectedSaldo(saldo);
        }


    }, [saldoCuentas, selectedCuenta]);

    useEffect(() => {
        const saldoResponse = obtenerSaldo(selectedCuenta);
        setSaldoDisponible(saldoResponse.saldoDisponibleCuenta);
    }, [selectedCuenta]);


    useEffect(() => { datosCatalogo() }, []);


    const datosCatalogo = async () => {
        await catalogApi
            .getAllTypeDocuments()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setObtenerTipoDocumento(response.data);
                    }
                }
            })
            .catch(() => {
                toast.error('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleTransferenciaCuentaChange = (event: any) => {
        const selectedTipoSolicitud = event.target.value as string;
        setTipoSolicitud(selectedTipoSolicitud);
    };

    const handleCuentaOrigenChange = (event: React.ChangeEvent<{ name?: string | undefined; value: unknown; }>) => {
        const selectedValue = event.target.value as string;
        setSelectedCuenta(selectedValue);
    };

    const motivosTraslado = [
        'SELECCIONE...',
        'Pago Manuales No Registrados',
        'Inconsistencia de Cargue de Nominas',
    ];

    const calcularValorTotal = () => {
        let total = 0;
        const montos = getValues('monto');
        Object.values(montos).forEach(monto => {
            total += parseFloat(monto) || 0;
        });
        return total;
    };


    const calcularValorTotalDestino = () => {
        let total = 0;
        const montos = getValues("montoDestino");
        Object.values(montos).forEach(monto => {
            total += parseFloat(monto) || 0;
        });
        return total;
    };

    useEffect(() => {
        setValorTotal(0);
        setValorTotalDestino(0);
    }, [cuentaSeleccionada]);

    const actualizarValorTotal = () => {
        const nuevoTotal = calcularValorTotal();
        setValorTotal(nuevoTotal);
    };

    const actualizarValorTotalDestino = () => {
        const nuevoTotalDestino = calcularValorTotalDestino();
        setValorTotalDestino(nuevoTotalDestino);
    };


    const montos: number[] = [];
    const nombresConceptos: string | string[] | undefined = [];
    interface MontosPorConcepto {
        [key: string]: number;
    }

    const montosPorConcepto: MontosPorConcepto = {};


    return (
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
                        TRANSFERENCIA ENTRE CUENTAS
                    </Typography>
                </Grid>
                <br />
                <Grid container spacing={2} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                    <Grid item xs={6}>
                        <Controller
                            name="radicado"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (
                                <CHTextField label="Radicado" />
                            )}
                        />
                    </Grid>
                </Grid>

                <Grid container spacing={2}>
                    <Grid item xs={6}>
                        <Controller
                            name="tipoSolicitud"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <FormControl className={classes.select} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Tipo de Solicitud</InputLabel>
                                    <Select
                                        value={tipoSolicitud}
                                        onChange={handleTransferenciaCuentaChange}
                                    >
                                        <MenuItem value="reactivacion">Tranferencia entre Cuentas</MenuItem>
                                    </Select>
                                </FormControl>
                            )}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Controller
                            name="modalidad"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <FormControl className={classes.select} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Modalidad</InputLabel>
                                    <Select
                                        value={cuentaOrigen}
                                        onChange={(event) => setCuentaOrigen(event.target.value as string)}
                                    >
                                        <MenuItem value="fondo-mecanismos-esp">FONDO MECANISMOS ESPECIALES</MenuItem>
                                        <MenuItem value="fondo-nuevas-conv">FONDO NUEVAS CONVOCATORIAS</MenuItem>
                                        <MenuItem value="acreditacion-v8">NO ACREDITACIÓN V8</MenuItem>
                                        <MenuItem value="reintegro-intereses">REINTEGRO INTERESES</MenuItem>
                                        <MenuItem value="reintegro-vivienda8">REINTEGRO VIVIENDA 8</MenuItem>
                                        <MenuItem value="resolucion-traslados">RESOLUCIÓN TRASLADOS SALDOS 624 DEL 2018</MenuItem>
                                        <MenuItem value="solicitud-interna">SOLICITUD INTERNA</MenuItem>
                                        <MenuItem value="transfe-avac">Transferencia AVAC</MenuItem>
                                        <MenuItem value="transfe-avav">Transferencia AVAV</MenuItem>
                                        <MenuItem value="transfe-avc">Transferencia AVC</MenuItem>
                                    </Select>
                                </FormControl>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <Controller
                            name="comprobante"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <CHTextField label="Comprobante" fullWidth />
                            )}
                        />

                    </Grid>
                </Grid>

                <br />
                <br />
                <br />
                <br />
                <Grid container justify="center">
                    <Grid item xs={12}>
                        <Typography className={classes.titleform} variant="h5">
                            Información del Afiliado Origen
                        </Typography>
                    </Grid>
                </Grid>

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
                </Grid>
                <br />
                <br />
                <Grid container justify="center">
                    <Grid item xs={12}>
                        <Typography className={classes.titleform} variant="h5">
                            Información Origen
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                    <Grid container spacing={2}>
                        <Grid item xs={6}>
                            <Controller
                                name="cuentaOrigen"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <FormControl className={classes.select} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                                        <InputLabel>Cuenta Origen</InputLabel>
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
                                        isReadOnly
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
                                name="fuerza"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Fuerza"
                                        isReadOnly
                                        value={responseAdminCuenta.fuerza}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="unidadEjecutora"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Unidad Ejecutora"
                                        isReadOnly
                                        value={responseAdminCuenta.unidadEjecutora}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="grado"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Grado"
                                        isReadOnly
                                        value={responseAdminCuenta.grado}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="saldoDisponible"
                                control={control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Saldo Disponible"
                                        value={saldoDisponible}
                                        isReadOnly
                                        currencyMask
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                    />
                                )}
                            />
                        </Grid>

                        <Grid item xs={6}>
                            <Controller
                                name="montoTotal"
                                control={control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Monto Total"
                                        currencyMask
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="motivoTraslado"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (

                                    <FormControl className={classes.select} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                                        <InputLabel>Motivo Traslado</InputLabel>
                                        <Select
                                            value={motivoTraslado}
                                            onChange={(event) => setMotivoTraslado(event.target.value as string)}
                                        >
                                            {motivosTraslado.map((option: any) => (
                                                <MenuItem key={option} value={option}>
                                                    {option}
                                                </MenuItem>
                                            ))}

                                        </Select>
                                    </FormControl>
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
                            Movimientos Origen
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                    <Grid container spacing={2}>
                        <Grid item xs={6}>
                            <Controller
                                name="tipoCuentas"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <FormControl className={classes.select} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                                        <InputLabel>Tipo de Cuenta</InputLabel>
                                        <Select
                                            value={value}
                                            onChange={(e: React.ChangeEvent<{ value: unknown }>) => {
                                                onChange(e);
                                                setCuentaSeleccionada(e.target.value as string);
                                            }}
                                        >
                                            <MenuItem value="">Seleccione una opción...</MenuItem>
                                            {responseAdminCuenta && responseAdminCuenta.cuentas.length > 0 && (
                                                responseAdminCuenta.cuentas.map((cuenta: any) => (
                                                    <MenuItem key={cuenta.nombreCuenta} value={cuenta.nombreCuenta}>
                                                        {cuenta.nombreCuenta}
                                                    </MenuItem>
                                                ))
                                            )}

                                        </Select>
                                    </FormControl>
                                )}
                            />
                        </Grid>
                        <TableContainer>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell className={classes.headerCell}>CONCEPTO</TableCell>
                                        <TableCell className={classes.headerCell}>SALDO</TableCell>
                                        <TableCell className={classes.headerCell}>MONTO</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {responseAdminCuenta?.cuentas.forEach((cuenta: any) => {
                                        if (cuenta.nombreCuenta === cuentaSeleccionada) {
                                            cuenta.conceptos.forEach(({ saldo, monto }: any) => {
                                                saldos.push(parseFloat(saldo));
                                                montos.push(parseFloat(monto));
                                            });
                                        }
                                    })}
                                    {responseAdminCuenta && responseAdminCuenta.cuentas.length > 0 && (
                                        responseAdminCuenta.cuentas.map((cuenta: any) => (
                                            cuenta.nombreCuenta === cuentaSeleccionada && (
                                                cuenta.conceptos.map(({ descripcionConcepto, saldo, index }: any) => {
                                                    saldosPorConcepto[descripcionConcepto] = saldo;

                                                    nombresConceptos[index] = descripcionConcepto;

                                                    const montoConcepto = montosPorConcepto[descripcionConcepto];

                                                    montosPorConcepto[descripcionConcepto] = montoConcepto;


                                                    return (

                                                        <React.Fragment key={descripcionConcepto}>
                                                            <TableRow>
                                                                <TableCell>
                                                                    <Controller
                                                                        name={`concepto_${descripcionConcepto}`}
                                                                        control={control}
                                                                        render={({ field: { onChange } }) => (
                                                                            <CHTextField
                                                                                label="Concepto"
                                                                                isReadOnly
                                                                                value={descripcionConcepto}
                                                                                onChange={onChange}
                                                                            />
                                                                        )}
                                                                    />
                                                                </TableCell>
                                                                <TableCell>
                                                                    <Controller
                                                                        name={`saldo.${descripcionConcepto}`}
                                                                        control={control}
                                                                        render={({ field: { onChange } }) => (
                                                                            <CHTextField
                                                                                label="Saldo"
                                                                                currencyMask
                                                                                isReadOnly
                                                                                value={parseFloat(saldo)}
                                                                                onChange={(e) => {
                                                                                    const newValue = parseFloat(e.target.value);
                                                                                    onChange(newValue);
                                                                                }}
                                                                            />
                                                                        )}
                                                                    />
                                                                </TableCell>
                                                                <TableCell key={descripcionConcepto}>
                                                                    <Controller
                                                                        name={`monto.${descripcionConcepto}`}
                                                                        control={control}
                                                                        render={({ field: { onChange, value, onBlur } }) => (
                                                                            <CHTextField
                                                                                label="Monto"
                                                                                value={value}
                                                                                currencyMask
                                                                                onChange={(e) => {
                                                                                    const inputValue = e.target.value;
                                                                                    const cleanedValue = inputValue.replace(/[^0-9.]/g, '');
                                                                                    let newValue = parseFloat(cleanedValue)
                                                                                    if (Number.isNaN(newValue)) {
                                                                                        newValue = 0;
                                                                                    }
                                                                                    onChange(newValue, index);
                                                                                    actualizarValorTotal();
                                                                                }}
                                                                                onBlur={() => {
                                                                                    const saldoConcepto = parseFloat(saldo);
                                                                                    const montoIngresado = typeof value === 'string' ? parseFloat(value) : value as number;
                                                                                    if (montoIngresado > saldoConcepto) {
                                                                                        toast(`El monto es mayor a el saldo de ${descripcionConcepto}`);
                                                                                    } else if (montoIngresado === saldoConcepto) {
                                                                                        toast(`El monto es igual a el saldo de ${descripcionConcepto}`);
                                                                                    } else {
                                                                                        toast(`El monto es menor a el saldo de ${descripcionConcepto}`);
                                                                                    }
                                                                                }}
                                                                            />
                                                                        )}
                                                                    />
                                                                </TableCell>
                                                            </TableRow>
                                                        </React.Fragment>
                                                    )
                                                })
                                            )
                                        ))
                                    )}
                                </TableBody>
                            </Table>
                        </TableContainer>

                        <Grid item xs={6}>
                            <Controller
                                name="valorTotal"
                                control={control}
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Valor Total:"
                                        fullWidth
                                        currencyMask
                                        isReadOnly
                                        value={valorTotal}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                </Grid>
                <br />
                <br />
                <Grid container justify="center" alignItems="center">
                    <Grid item xs={12}>
                        <Typography className={classes.titleform} variant="h5">
                            Información del Afiliado Destino
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                    <Grid container spacing={2}>
                        <Grid item xs={6} >
                            <Controller
                                name="tipoDocumentoId"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Tipo Documento"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                    >
                                        {catalogState.IdentificationType?.map(
                                            (option: IDENTIFICATIONTYPEDTO) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </Grid>

                        <Grid item xs={6}>
                            {
                                (parseInt(identificationType, 10) === 1 || parseInt(identificationType, 10) === 2) ? (

                                    <Controller
                                        name="numeroDocumento"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                            minLength: {
                                                value: 4,
                                                message: 'La longitud mínima del campo es de 4 caracteres',
                                            },
                                            maxLength: {
                                                value: 12,
                                                message: 'La longitud máxima del campo es de 12 caracteres',
                                            },
                                            pattern: /^[0-9]+$/g
                                        }}
                                        defaultValue=""
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? "El número de identificación no debe contener letras" : null}
                                                label="Número de Documento"
                                                value={value}
                                                onChange={onChange}
                                                type="number"
                                            />
                                        )}
                                    />
                                ) : (
                                    <Controller
                                        name="numeroDocumento"
                                        control={control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                isReadOnly={false}
                                                label="Número de Documento"
                                                value={value}
                                                onChange={onChange}
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}
                                            />
                                        )}
                                    />
                                )
                            }
                        </Grid>

                        <Grid item>
                            <Button
                                className={classes.buttonprincipal}

                                onClick={handleSubmit(searchAdministracionCuentas)}
                            >
                                BUSCAR
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
                <br />
                <br />
                <Grid container justify="center">
                    <LoadingComponent key="loader" loading={loading} />
                    <Grid item xs={12}>
                        <Typography className={classes.titleform} variant="h5">
                            Información Destino
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                    <Grid container spacing={2}>
                        <Grid item xs={6}>
                            <Controller
                                name="cuentaDestino"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (

                                    <FormControl className={classes.select} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                                        <InputLabel>Cuenta Destino</InputLabel>
                                        <Select
                                            value={value}
                                            onChange={(event) => {
                                                onChange(event);
                                                handleCuentaDestinoChange(event);
                                                setCuentaSeleccionadaDestino(event.target.value as string);

                                            }}
                                        >
                                            <MenuItem value="select">SELECCIONE...</MenuItem>
                                            {opcionesCuentas.map((cuenta: any) => (
                                                <MenuItem key={cuenta.id} value={cuenta.nombreCuenta}>
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
                                name="cuentaDestinoNo"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Cuenta No:"
                                        value={numeroCuenta}
                                        onChange={(event) => {
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
                                name="fuerza"
                                control={control}
                                defaultValue=""

                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Fuerza"
                                        isReadOnly
                                        value={value || fuerzaAfiliado}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="unidadEjecutora"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Unidad Ejecutora"
                                        isReadOnly
                                        value={value || unidadEjecutoraAfiliado}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="grado"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Grado"
                                        isReadOnly
                                        value={value || gradoAfiliado}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="estado"
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Estado cuenta"
                                        isReadOnly
                                        value={estadoCuenta[cuentaSeleccionadaDestino] || ''}
                                        onChange={onChange}
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
                            Movimientos Destino
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                    <Grid container spacing={2}>
                        <TableContainer>
                            <Table>
                                <TableHead>
                                    <TableRow>

                                        <TableCell className={classes.headerCell}>CONCEPTO</TableCell>
                                        <TableCell className={classes.headerCell}>SALDO DESTINO</TableCell>
                                        <TableCell className={classes.headerCell}>MONTO</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {tiposCuentas.map((cuenta: any) => (
                                        cuenta.nombreCuenta === cuentaSeleccionadaDestino && (
                                            cuenta.conceptos.map(({ descripcionConcepto, saldo }: any, index: number) => (
                                                <TableRow key={index}>
                                                    <TableCell>
                                                        <Controller
                                                            name={`concepto_${descripcionConcepto}`}
                                                            control={control}
                                                            render={({ field: { onChange } }) => (
                                                                <CHTextField
                                                                    label="Concepto"
                                                                    isReadOnly
                                                                    value={descripcionConcepto}
                                                                    onChange={onChange}
                                                                />
                                                            )}
                                                        />
                                                    </TableCell>
                                                    <TableCell>
                                                        <Controller
                                                            name={`saldo_${descripcionConcepto}`}
                                                            control={control}
                                                            render={({ field: { onChange } }) => (
                                                                <CHTextField
                                                                    label="saldo"
                                                                    isReadOnly
                                                                    currencyMask
                                                                    value={parseFloat(saldo)}
                                                                    onChange={onChange}
                                                                />
                                                            )}
                                                        />
                                                    </TableCell>
                                                    <TableCell>
                                                        <Controller
                                                            name={`montoDestino.${descripcionConcepto}`}
                                                            control={control}
                                                            render={({ field: { onChange, value } }) => (
                                                                <CHTextField
                                                                    label="Monto"
                                                                    currencyMask
                                                                    value={value}
                                                                    onChange={(e) => {
                                                                        const inputValue = e.target.value;
                                                                        const cleanedValue = inputValue.replace(/[^0-9.]/g, '');
                                                                        let newValue = parseFloat(cleanedValue)
                                                                        if (Number.isNaN(newValue)) {
                                                                            newValue = 0;
                                                                        }
                                                                        onChange(newValue);
                                                                        actualizarValorTotalDestino();

                                                                    }}
                                                                    onBlur={(e) => {
                                                                        const saldoConceptoDos = parseFloat(saldo);
                                                                        const montoIngresadoDos = typeof value === 'string' ? parseFloat(value) : value as number;


                                                                        if (montoIngresadoDos > saldoConceptoDos) {
                                                                            toast(`El monto es mayor a el saldo de ${descripcionConcepto}`);
                                                                        } else if (montoIngresadoDos === saldoConceptoDos) {
                                                                            toast(`El saldo es igual a el monto de ${descripcionConcepto}`);
                                                                        } else {
                                                                            toast(`El saldo es menor a el monto de ${descripcionConcepto}`);
                                                                        }
                                                                    }}
                                                                />
                                                            )}
                                                        />
                                                    </TableCell>
                                                </TableRow>
                                            ))
                                        )
                                    ))}
                                </TableBody>

                            </Table>
                        </TableContainer>
                        <Grid item xs={6}>
                            <Controller
                                name="valorTotal"
                                control={control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        label="Valor Total:"
                                        fullWidth
                                        value={valorTotalDestino}
                                        currencyMask
                                        disabled
                                    />
                                )}
                            />
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
    )
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
});
const mapDispatchToProps = {
    startGetMovementsAccountsClient,
};
export default connect(mapStateToProps, mapDispatchToProps)(ModalTransferenciaCuentas);