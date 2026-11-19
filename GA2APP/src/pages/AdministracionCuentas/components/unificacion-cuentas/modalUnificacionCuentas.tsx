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
    TableContainer,
    Table,
    TableHead,
    TableRow,
    TableCell,
    TableBody,
    Checkbox,
    Paper
} from '@material-ui/core';
import swal from 'sweetalert';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import ApiClients from '../../../../services/api-clients.service';
import CHSelect from '../../../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../../../types/catalog.type';
import { startGetMovementsAccountsClient } from '../../../../actions/client.action';
import CatalogApi from '../../../../services/api-catalog.service';
import { ClientState } from '../../../../reducers/client.reducer';
import { AppState } from '../../../../store/rootReducer';
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
        paddingRight: '100px'

    },
    tableCellBlueText: {
        color: 'blue',
    },
    headerCell: {
        textAlign: 'center',
    },
    h2: {
        color: '#1C3F6B',
    }
}));


function ModalUnificacionCuentas({
    open,
    onClose,
    clientState,
    catalogState
}: {
    open: any;
    onClose: any;
    clientState: ClientState;
    catalogState: any;
}): any {

    const classes = useStyles();
    const api = ApiClients();

    const catalogApi = CatalogApi();
    const [obtenerTipoDocumento, setObtenerTipoDocumento] = useState<any[]>([]);
    const [tipoSolicitud, setTipoSolicitud] = useState('');
    const [selectAll, setSelectAll] = useState(false);
    const [checkAll, setCheckAll] = useState(false);
    const [checkAllDos, setCheckAllDos] = useState(false);
    const [selectedRows, setSelectedRows] = useState<any[]>([]);
    const [selectedRowsDos, setSelectedRowsDos] = useState<any[]>([]);
    const [selectedFilas, setSelectedFilas] = useState<number[]>([]);
    const [selectedModalidad, setSelectedModalidad] = useState('');
    const [tableData, setTableData] = useState<{ requisito: string; descripcion: string; selected?: boolean }[]>(
        []
    );

    const [loading, setLoading] = useState<boolean>(false);
    const [opcionesCuentas, setOpcionesCuentas] = useState<Cuenta[]>([]);
    const [opcionesCuentasUno, setOpcionesCuentasUno] = useState<Cuenta[]>([]);
    const [opcionesCuentasDos, setOpcionesCuentasDos] = useState<CuentaSaldo[]>([]);
    const [conceptosCuentas, setConceptosCuentas] = useState<Cuenta[]>([]);
    const [tiposCuentas, setTiposCuentas] = useState<Cuenta[]>([]);
    const [numeroCuenta, setNumeroCuenta] = useState('');
    const [numeroCuentaSaldo, setNumeroCuentaSaldo] = useState('');
    const [fuerzaAfiliado, setFuerzaAfiliado] = useState('');
    const [unidadEjecutoraAfiliado, setUnidadEjecutoraAfiliado] = useState('');
    const [gradoAfiliado, setGradoAfiliado] = useState('');
    const [cuotas, setCuotas] = useState<any[]>([]);
    const [cuotasSaldo, setCuotasSaldo] = useState<any[]>([]);
    const [priNomAfiliado, setPriNomAfiliado] = useState('');
    const [segNomAfiliado, setSegNomAfiliado] = useState('');
    const [priApeAfiliado, setPriApeAfiliado] = useState('');
    const [segApeAfiliado, setSegApeAfiliado] = useState('');
    const [cuentaDestAfiliado, setCuentaDestAfiliado] = useState<any>();
    const [cuentaDestAfiliadoSaldo, setCuentaDestAfiliadoSaldo] = useState<any>();
    const [dataFormCompleta, setDataFormCompleta] = useState('');
    const [dataFuerza, setDataFuerza] = useState('');
    const [dataGrado, setDataGrado] = useState('');
    const [dataUnidadEjecutora, setDataUnidadEjecutora] = useState('');
    const [priNomOrigen, setPriNomOrigen] = useState('');
    const [segNomOrigen, setSegNomOrigen] = useState('');
    const [priApeOrigen, setPriApeOrigen] = useState('');
    const [segApeOrigen, setSegApeOrigen] = useState('');
    const [searchResults, setSearchResults] = useState<TableRows[]>([]);
    const [cuentaSeleccionada, setCuentaSeleccionada] = useState<Cuenta | null>(null);


    const { control: controlUno, handleSubmit: handleSubmitUno, setValue: setValueUno, getValues: getValuesUno, watch } = useForm({
        defaultValues: {
            tipoDocumentoIdUno: '',
            numeroDocumentoUno: '',
            primerNombreUno: '',
            segundoNombreUno: '',
            primerApellidoUno: '',
            segundoApellidoUno: '',
            cuentaDestinoUno: '',
            cuentaDestinoNoUno: '',
            fuerzaUno: '',
            unidadEjecutoraUno: '',
            gradoUno: '',
            cuotasUno: 0,
            valorTrasladoUno: 0,
            valorTotalUno: 0,
        },
        mode: 'onBlur'

    });

    const { control: controlDos, handleSubmit: handleSubmitDos, setValue: setValueDos, getValues: getValuesDos, watch: watchDos } = useForm({
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
            tipoCuentaDestino: '',
            observaciones: '',
            cuotas: 0,
            valorTraslado: 0,
            tipoCuenta: '',
        },
        mode: 'onBlur'
    });

    const identificationTypeUno = watch('tipoDocumentoIdUno');
    const identificationTypeDos = watchDos('tipoDocumentoId');

    const { control: controlDestino, setValue: setValueDestino, } = useForm({
        defaultValues: {
            tipoCuentaUno: '',
        },
    });

    interface Cuenta {
        id: number;
        nombreCuenta: string;
        numeroCuenta: string;
        conceptos?: Concepto[];
        primerNombre: string;
        estadoCuenta: string;
        fechaCreacion: Date;
        selected: boolean;
        saldo: any;
        cueotas: number;
        bloqueos: any[];
    }

    interface CuentaSaldo {
        id: number;
        nombreCuenta: string;
        numeroCuenta: string;
        conceptos?: Concepto[];
        primerNombre: string;
        estadoCuenta: string;
        fechaCreacion: Date;
        selected: boolean;
        saldo: any;
        cueotas: number;
        bloqueos: any[];
    }

    interface Concepto {
        descripcionConcepto: string;
        saldo: any;
    }

    const searchDatosAfiliadoDestino = async () => {
        try {
            setLoading(true);
            const formData = getValuesUno();
            const response = await api.getAdministracionCuentasAfiliado({
                numeroDocumento: formData.numeroDocumentoUno,
                tipoDocumentoId: parseInt(formData.tipoDocumentoIdUno, 10)
            })

            const cuentas = response.data.cuentas || [];
            const conceptos = response.data.cuentas.conceptos || [];
            const fuerza = response.data.fuerza || '';
            const unidadEjecutora = response.data.unidadEjecutora || '';
            const grado = response.data.grado || '';

            const priNombreAfiliado = response.data.primerNombre || '';
            const segNombreAfiliado = response.data.segundoNombre || '';
            const priApellidoAfiliado = response.data.primerApellido || '';
            const segApellidoAfiliado = response.data.segundoApellido || '';
            const cuotasDestinoAfiliado = response.data?.cuentas || [];

            const cuotasDestino = cuotasDestinoAfiliado.length > 0 ? cuotasDestinoAfiliado.flatMap((cuenta: any) => cuenta.cueotas || []) : [];


            setOpcionesCuentasUno(cuentas);
            setOpcionesCuentasDos(cuentas);
            setConceptosCuentas(conceptos);

            setFuerzaAfiliado(fuerza);
            setUnidadEjecutoraAfiliado(unidadEjecutora);
            setGradoAfiliado(grado);
            setTiposCuentas(cuentas);
            setCuotas(cuotasDestino);

            setPriNomAfiliado(priNombreAfiliado);
            setSegNomAfiliado(segNombreAfiliado);
            setPriApeAfiliado(priApellidoAfiliado);
            setSegApeAfiliado(segApellidoAfiliado);

            setCuentaDestAfiliado(cuotasDestinoAfiliado);
            setCuentaDestAfiliadoSaldo(cuotasDestinoAfiliado);

            setSearchResults(response.data);

            toast('Afiliado encontrado, seleccione una cuenta de destino');

        } catch (error: any) {

            let errorMessage = 'Error al buscar datos';

            if (error.response) {
                errorMessage += `: ${error.response.data.message || error.response.statusText}`;
            } else if (error.request) {
                errorMessage += ': No hay respuesta del servidor';
            } else {
                errorMessage += `: ${error.message}`;
            }

            toast(errorMessage, { type: toast.TYPE.ERROR });
        }
        setLoading(false);
    };

    const searchAdministracionCuentas = async () => {
        try {
            setLoading(true);
            const formData = getValuesDos();

            const response = await api.getAdministracionCuentasAfiliado({
                numeroDocumento: formData.numeroDocumento,
                tipoDocumentoId: parseInt(formData.tipoDocumentoId, 10)
            })


            const priNombreAfiliado = response.data.primerNombre || '';
            const segNombreAfiliado = response.data.segundoNombre || '';
            const priApellidoAfiliado = response.data.primerApellido || '';
            const segApellidoAfiliado = response.data.segundoApellido || '';
            const cuotasOrigenAfiliado = response.data?.cuentas || [];
            const cuentas = response.data.cuentas || [];
            const nombresCompletos = `${priNombreAfiliado} ${segNombreAfiliado} ${priApellidoAfiliado} ${segApellidoAfiliado}`.trim();
            const fuerzaAfiliado = response.data.fuerza;
            const gradoAfiliado = response.data.grado;
            const unidadEjeAfiliado = response.data.unidadEjecutora;

            setDataFormCompleta(nombresCompletos);
            setDataFuerza(fuerzaAfiliado);
            setDataGrado(gradoAfiliado);
            setDataUnidadEjecutora(unidadEjeAfiliado);

            setPriNomOrigen(priNombreAfiliado);
            setSegNomOrigen(segNombreAfiliado);
            setPriApeOrigen(priApellidoAfiliado);
            setSegApeOrigen(segApellidoAfiliado);
            setCuentaDestAfiliado(cuotasOrigenAfiliado);
            setOpcionesCuentas(cuentas);


            setSearchResults(response.data);

            toast('Afiliado encontrado');

        } catch (error) {
            console.error('Error al buscar datos:', error);
        }
        setLoading(false);
    };


    const handleSaldoDestino = (event: any) => {
        const selectedCuentaDestinoSaldo = event.target.value;
        const cuotasDestinoAfiliadoSaldo = cuentaDestAfiliadoSaldo;


        setNumeroCuentaSaldo('');
        setCuotasSaldo([]);

        setValueDestino('tipoCuentaUno', selectedCuentaDestinoSaldo);

        const cuentaSeleccionadaSaldo = opcionesCuentasUno.find((cuenta) => cuenta.nombreCuenta === selectedCuentaDestinoSaldo);

        setCuentaSeleccionada(cuentaSeleccionadaSaldo || null);

        if (cuentaSeleccionadaSaldo) {
            setNumeroCuentaSaldo(cuentaSeleccionadaSaldo.numeroCuenta);

            const cuotasFiltradasSaldo = cuotasDestinoAfiliadoSaldo
                ? cuotasDestinoAfiliadoSaldo
                    .filter((cuenta: any) => cuenta.nombreCuenta === selectedCuentaDestinoSaldo)
                    .flatMap((cuenta: any) => cuenta.cueotas || [])
                : [];


            setCuotasSaldo(cuotasFiltradasSaldo);
        }
    };

    const handleCuentaDestinoChange = (event: any) => {
        const selectedCuentaDestino = event.target.value;
        const cuotasDestinoAfiliado = cuentaDestAfiliado;


        setNumeroCuenta('');
        setCuotas([]);

        setValueUno('cuentaDestinoUno', selectedCuentaDestino);

        const cuentaSeleccionada = opcionesCuentasDos.find((cuenta) => cuenta.nombreCuenta === selectedCuentaDestino);

        if (cuentaSeleccionada) {
            setNumeroCuenta(cuentaSeleccionada.numeroCuenta);

            const cuotasFiltradas = cuotasDestinoAfiliado
                .filter((cuenta: any) => cuenta.nombreCuenta === selectedCuentaDestino)
                .flatMap((cuenta: any) => cuenta.cueotas || []);

            setCuotas(cuotasFiltradas);
        }
    };


    interface TableRows {
        primerNombre: string;
        primerApellido: string;
    }


    useEffect(() => { datosCatalogo() }, []);

    useEffect(() => {

        const updatedSelectedRowsUno = clientState?.admincuentas.cuentas.map((row) => ({
            ...row,
            selected: false,
        }));
        setSelectedRows(updatedSelectedRowsUno);
        setCheckAll(false);

    }, [open]);

    useEffect(() => {

        const updatedSelectedRowsDos = clientState?.admincuentas.cuentas.map((row) => ({
            ...row,
            selected: false,
        }));
        setSelectedRowsDos(updatedSelectedRowsDos);
        setCheckAllDos(false);

    }, [open]);

    useEffect(() => {
        if (cuentaSeleccionada && cuentaSeleccionada.conceptos) {
            const total = cuentaSeleccionada.conceptos.reduce((acc, concepto) => acc + concepto.saldo, 0);

            setValueUno('valorTotalUno', total);
        }
    }, [cuentaSeleccionada]);

    const handleSelectAll = () => {
        if (selectAll) {
            setSelectedFilas([]);
        } else {
            setSelectedFilas(tableData.map((_, index) => index));
        }
        setSelectAll(!selectAll);
    };

    const handleRowSelect = (index: any) => {
        const updatedSelectedFilas = [...selectedFilas];

        const rowIndex = updatedSelectedFilas.indexOf(index);

        if (rowIndex === -1) {
            updatedSelectedFilas.push(index);
        } else {
            updatedSelectedFilas.splice(rowIndex, 1);
        }
        setSelectedFilas(updatedSelectedFilas);
    };

    const handleTransferenciaCuentaChange = (event: any) => {
        const selectedTipoSolicitud = event.target.value as string;
        setTipoSolicitud(selectedTipoSolicitud);
    };

    const MODALIDADES = {
        CAMBIO_CATEGORIA: 'CAMBIO DE CATEGORIA',
        CAMBIO_NOMBRE: 'CAMBIO DE NOMBRE',
        CORRECCION_NOMBRE: 'CORRECIÓN DEL NOMBRE',
        DOCUMENTO_IDENTIFICACION: 'DOCUMENTO DE IDENTIFICACIÓN',
        MUERTE_AFILIADO: 'MUERTE DEL AFILIADO',
    };

    const modalidadesUniCuentas = [
        'SELECCIONE...',
        MODALIDADES.CAMBIO_CATEGORIA,
        MODALIDADES.CAMBIO_NOMBRE,
        MODALIDADES.CORRECCION_NOMBRE,
        MODALIDADES.DOCUMENTO_IDENTIFICACION,
        MODALIDADES.MUERTE_AFILIADO,
    ];

    const CERTIFICACION_FUERZA = {
        requisito: 'CERTIFICACIÓN DE LA FUERZA',
        descripcion: 'CERTIFICACIÓN DONDE ACLARE EL NOMBRE Y EL NÚMERO DE IDENTIFICACIÓN',
    };

    const FOTOCOPIA_CEDULA = {
        requisito: 'FOTOCOPIA AUTENTICADA DE LA CEDULA DEL AFILIADO',
        descripcion: 'FOTOCOPIA AUNTENTICADA DE LA CEDULA DEL AFILIADO',
    };

    const handleModalidadChange = (event: any) => {
        const selectedOption = event.target.value;
        setSelectedModalidad(selectedOption);

        switch (selectedOption) {
            case MODALIDADES.CAMBIO_CATEGORIA:
                setTableData([
                    { requisito: 'RESOLUCIÓN DE NOMBRAMIENTO', descripcion: 'FOTOCOPIA SIMPLE', selected: false },
                    CERTIFICACION_FUERZA,
                ]);
                break;
            case MODALIDADES.CAMBIO_NOMBRE:
                setTableData([
                    { requisito: 'COPIA DE LA ESCRITURA PÚBLICA CON LA QUE LA NOTARIA', descripcion: 'CON LA QUE LA NOTARIA LE OTORGO EL NUEVO NOMBRE' },
                    CERTIFICACION_FUERZA,
                    FOTOCOPIA_CEDULA,
                ]);
                break;
            case MODALIDADES.CORRECCION_NOMBRE:
            case MODALIDADES.DOCUMENTO_IDENTIFICACION:
                setTableData([
                    CERTIFICACION_FUERZA,
                    FOTOCOPIA_CEDULA,
                ]);
                break;
            case MODALIDADES.MUERTE_AFILIADO:
                setTableData([
                    { requisito: 'RESOLUCIÓN PENSIONAL DE BENEFICIARIOS', descripcion: 'EN EL EVENTO EN QUE LOS BENEFICIARIOS DEL CAUSANTE QUEDAN PENSIONADOS, CON SELLO DE EJECUTORIA' },
                ]);
                break;
            default:
                setTableData([]);
        }
    };





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
            .catch((error: any) => {
                toast.error(`Error del sistema: ${error.message}`, { type: toast.TYPE.ERROR });
            });
    }


    const [cuentasSeleccionadas, setCuentasSeleccionadas] = useState<number[]>([]);
    const [cuentasSeleccionadasDos, setCuentasSeleccionadasDos] = useState<number[]>([]);
    const [cuentasSeleccionadasSegundaTabla, setCuentasSeleccionadasSegundaTabla] = useState<Cuenta[]>([]);

    const [checkboxState, setCheckboxState] = useState<{ [key: number]: boolean }>({});
    const [checkboxStateTwo, setCheckboxStateTwo] = useState<{ [key: number]: boolean }>({});
    const [disabledState, setDisabledState] = useState<{ [key: number]: boolean }>({});


    const handleElegirCuenta = () => {
        const cuentasSeleccionadasPrimeraTabla = cuentasSeleccionadas.map((index) => opcionesCuentas[index]);
    
        if (cuentasSeleccionadasPrimeraTabla.length === 0) {
          swal({
            icon: 'info',
            title: 'Oops...',
            text: 'Debes volver a seleccionar al menos una cuenta.',
          });
          return;
        }
    
        const nombresCuentas = cuentasSeleccionadasPrimeraTabla.map(cuenta => cuenta.nombreCuenta);
        const cuentasSeleccionadasTexto = nombresCuentas.join(', ');
    
        swal({
          text: `LA(S) CUENTA(S) SELECCIONADA(S): ${cuentasSeleccionadasTexto}`,
          icon: 'warning',
        });
    
        setCuentasSeleccionadasSegundaTabla([...cuentasSeleccionadasSegundaTabla, ...cuentasSeleccionadasPrimeraTabla]);
        setCuentasSeleccionadas([]);
        setCheckboxState({});
        setDisabledState(cuentasSeleccionadas.reduce((acc, index) => ({ ...acc, [index]: true }), disabledState));
      };
    
      const handleSelectFilaUno = (event: React.ChangeEvent<HTMLInputElement>, index: number) => {
        const isChecked = event.target.checked;
    
        setCheckboxState({
          ...checkboxState,
          [index]: isChecked,
        });
    
        if (isChecked) {
          setCuentasSeleccionadas([...cuentasSeleccionadas, index]);
        } else {
          const nuevasCuentasSeleccionadas = cuentasSeleccionadas.filter((item) => item !== index);
          setCuentasSeleccionadas(nuevasCuentasSeleccionadas);
        }
      };
  
      const handleSelectFilaDos = (event: React.ChangeEvent<HTMLInputElement>, index: number) => {
          const isChecked = event.target.checked;
      
          setCheckboxStateTwo({
            ...checkboxStateTwo,
            [index]: isChecked,
          });
      
          if (isChecked) {
            setCuentasSeleccionadasDos([...cuentasSeleccionadasDos, index]);
          } else {
            const nuevasCuentasSeleccionadas = cuentasSeleccionadasDos.filter((item) => item !== index);
            setCuentasSeleccionadasDos(nuevasCuentasSeleccionadas);
          }
        };
  
      const handleTransferirCuentas = () => {
  
          if (cuentasSeleccionadasDos.length === 0) {
              swal({
                  icon: 'info',
                  title: 'Oops...',
                  text: 'Debes seleccionar al menos una cuenta.',
              })
  
          } else {
              const nuevasCuentas = cuentasSeleccionadasSegundaTabla.filter((row, index) => !cuentasSeleccionadasDos.includes(index));
              setCuentasSeleccionadasSegundaTabla(nuevasCuentas);
              setCuentasSeleccionadasDos([]);
              setDisabledState(cuentasSeleccionadasDos.reduce((acc, index) => {
                const cuentaOriginalIndex = opcionesCuentas.findIndex(cuenta => cuenta.numeroCuenta === cuentasSeleccionadasSegundaTabla[index].numeroCuenta);
                return { ...acc, [cuentaOriginalIndex]: false };
            }, disabledState));
          }
  
        setCheckboxStateTwo({});
  
      };




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
                        UNIFICACIÓN DE CUENTAS
                    </Typography>
                </Grid>
                <br />
                <Grid container spacing={2}>
                    <Grid item xs={6}>
                        <Controller
                            name="radicado"
                            control={controlDos}
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
                            control={controlDos}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <FormControl className={classes.select} style={{ width: '100%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Tipo de Solicitud</InputLabel>
                                    <Select
                                        value={tipoSolicitud}
                                        onChange={handleTransferenciaCuentaChange}
                                    >
                                        <MenuItem value="reactivacion">Unificación de Cuentas</MenuItem>
                                    </Select>
                                </FormControl>
                            )}
                        />
                    </Grid>

                    <Grid item xs={6}>
                        <Controller
                            name="modalidad"
                            control={controlDos}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <FormControl className={classes.select} style={{ width: '100%', paddingRight: '100px', marginBottom: '20px' }}>
                                    <InputLabel>Modalidad</InputLabel>
                                    <Select
                                        value={selectedModalidad}
                                        onChange={handleModalidadChange}
                                    >
                                        {modalidadesUniCuentas.map((option: any) => (
                                            <MenuItem key={option} value={option}>
                                                {option}
                                            </MenuItem>
                                        ))}

                                    </Select>
                                </FormControl>
                            )}
                        />
                    </Grid>
                    {selectedModalidad !== 'SELECCIONE...' && (
                        <TableContainer>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Seleccionar
                                            <Checkbox
                                                checked={selectAll}
                                                onChange={handleSelectAll}
                                            />
                                        </TableCell>
                                        <TableCell className={classes.tableCellBlueText}>Requisito</TableCell>
                                        <TableCell className={classes.tableCellBlueText}>Descripción</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {tableData.map((rowData, index) => (
                                        <TableRow key={index}>
                                            <TableCell>
                                                <Checkbox
                                                    checked={selectedFilas.includes(index)}
                                                    onChange={() => handleRowSelect(index)}
                                                />
                                            </TableCell>
                                            <TableCell>{rowData.requisito}</TableCell>
                                            <TableCell>{rowData.descripcion}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    )}
                </Grid>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <Controller
                            name="comprobante"
                            control={controlDos}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <CHTextField label="Comprobante" fullWidth />
                            )}
                        />

                    </Grid>
                </Grid>
                <br />
                <br />
                <Grid container justify="center">
                    <LoadingComponent key="loader" loading={loading} />
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
                                name="tipoDocumentoIdUno"
                                control={controlUno}
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
                                (parseInt(identificationTypeUno, 10) === 1 || parseInt(identificationTypeUno, 10) === 2) ? (

                                    <Controller
                                        name="numeroDocumentoUno"
                                        control={controlUno}
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
                                        name="numeroDocumentoUno"
                                        control={controlUno}
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

                        <Grid container spacing={2}>
                            <Grid item xs={6}>
                                <Button
                                    className={classes.buttonprincipal}

                                    onClick={handleSubmitUno(searchDatosAfiliadoDestino)}
                                >
                                    BUSCAR
                                </Button>
                            </Grid>
                        </Grid>

                        <Grid item xs={6}>
                            <Controller
                                name="primerNombreUno"
                                control={controlUno}
                                defaultValue=""
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Primer Nombre"
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        value={value || priNomAfiliado}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="segundoNombreUno"
                                control={controlUno}
                                defaultValue=""
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Segundo Nombre"
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        value={value || segNomAfiliado}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="primerApellidoUno"
                                control={controlUno}
                                defaultValue=""
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Primer Apellido"
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        value={value || priApeAfiliado}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="segundoApellidoUno"
                                control={controlUno}
                                defaultValue=""
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Segundo Apellido"
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        value={value || segApeAfiliado}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <br />

                </Grid>
                <br />
                <br />
                <Grid container justify="center">
                    <Grid item xs={12}>
                        <Typography className={classes.titleform} variant="h5">
                            INFORMACIÓN CUENTA DESTINO
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                    <Grid container spacing={2}>
                        <Grid item xs={6}>
                            <Controller
                                name="cuentaDestinoUno"
                                control={controlUno}
                                defaultValue=""
                                render={({ field: { onChange, value } }) => (

                                    <FormControl className={classes.select} style={{ width: '80%', paddingRight: '100px', marginBottom: '20px' }}>
                                        <InputLabel>Cuenta Destino</InputLabel>
                                        <Select
                                            value={value}
                                            onChange={(event) => {
                                                onChange(event);
                                                handleCuentaDestinoChange(event);
                                                handleSaldoDestino(event);
                                            }}
                                        >
                                            <MenuItem value="select">Seleccione una opción</MenuItem>
                                            {opcionesCuentasUno.map((cuenta: any) => (
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
                                name="cuentaDestinoNoUno"
                                control={controlUno}
                                defaultValue=""
                                render={({ field: { onChange } }) => (
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
                                name="fuerzaUno"
                                control={controlUno}
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
                                name="unidadEjecutoraUno"
                                control={controlUno}
                                defaultValue=""
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Unidad Ejecutora"
                                        isReadOnly
                                        value={value || unidadEjecutoraAfiliado}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="gradoUno"
                                control={controlUno}
                                defaultValue=""
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Grado"
                                        isReadOnly
                                        value={value || gradoAfiliado}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="cuotasUno"
                                control={controlUno}
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Cuotas"
                                        isReadOnly
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        value={value || cuotas}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <Controller
                                name="valorTrasladoUno"
                                control={controlUno}
                                render={({ field: { value } }) => (
                                    <CHTextField
                                        label="Valor Traslado"
                                        // value={value}
                                        currencyMask
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
                            SALDO POR CONCEPTO CUENTA DE DESTINO
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                </Grid>
                <TableContainer>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell className={classes.headerCell}>CONCEPTO</TableCell>
                                <TableCell className={classes.headerCell}>SALDO</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {cuentaSeleccionada?.conceptos && cuentaSeleccionada.conceptos.length > 0 && (
                                cuentaSeleccionada.conceptos.map((concepto: Concepto, index: number) => (
                                    <TableRow key={index}>
                                        <TableCell>{concepto.descripcionConcepto}</TableCell>
                                        <TableCell>
                                            <CHTextField
                                                label="SALDO"
                                                currencyMask
                                                isReadOnly
                                                value={concepto.saldo}
                                            />
                                        </TableCell>
                                    </TableRow>
                                ))
                            )}
                        </TableBody>
                    </Table>

                </TableContainer>

                <Grid item xs={12} md={6}>
                    <Controller
                        name="valorTotalUno"
                        control={controlUno}
                        render={({ field: { onChange, value } }) => (

                            <CHTextField
                                label="Valor Total:"
                                fullWidth
                                currencyMask
                                value={value}
                                onChange={onChange}
                                isReadOnly
                            />
                        )}
                    />
                </Grid>

                <br />
                <br />
                <Grid container justify="center">
                    <Grid item xs={12}>
                        <Typography className={classes.titleform} variant="h5">
                            Información del Afiliado Origen
                        </Typography>
                    </Grid>
                    <br />
                    <br />
                </Grid>
                <Grid container spacing={2}>

                    <Grid item xs={6} >
                        <Controller
                            name="tipoDocumentoId"
                            control={controlDos}
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
                            (parseInt(identificationTypeDos, 10) === 1 || parseInt(identificationTypeDos, 10) === 2) ? (

                                <Controller
                                    name="numeroDocumento"
                                    control={controlDos}
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
                                    control={controlDos}
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

                            onClick={handleSubmitDos(searchAdministracionCuentas)}
                        >
                            BUSCAR
                        </Button>
                    </Grid>

                </Grid>
                <Grid container>
                    <h2 className={classes.h2}>Información de Cuentas</h2>
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell className={classes.tableCellBlueText}>
                                        Acción
                                    </TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Nombre</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Número de Cuenta</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Tipo de Cuenta</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Estado de Cuenta</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Fecha de Inscripción</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {opcionesCuentas.map((row, index) => (
                                    <TableRow key={index}>
                                        <TableCell>
                                            <Checkbox
                                                checked={checkboxState[index] || false}
                                                onChange={(event) => handleSelectFilaUno(event, index)}
                                                disabled={disabledState[index] || false}
                                            />

                                        </TableCell>
                                        <TableCell>
                                            {dataFormCompleta}
                                        </TableCell>
                                        <TableCell>
                                            {row.numeroCuenta}
                                        </TableCell>
                                        <TableCell> {row.nombreCuenta}</TableCell>
                                        <TableCell>{row.estadoCuenta}</TableCell>
                                        <TableCell>
                                            {row.fechaCreacion ?
                                                new Date(row.fechaCreacion).toLocaleDateString('es-ES', { day: '2-digit', month: '2-digit', year: 'numeric' })
                                                : ''
                                            }
                                        </TableCell>

                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </Grid>
                <Grid item spacing={1}>
                    <Button
                        style={{ backgroundColor: '#a9a9a9', color: 'white', marginTop: "20px", marginLeft: "55rem", width: "20%" }}
                        onClick={handleElegirCuenta}
                    >
                        Elegir Cuenta
                    </Button>
                </Grid>
                <Grid container>
                    <h2 className={classes.h2}>Información Cuentas Origen</h2>
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell className={classes.tableCellBlueText}>
                                        Acción
                                    </TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Número</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Tipo de Cuenta</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Fecha Creación</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Saldo</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Cuentahabiente</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Fuerza</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Unidad Ejecutora</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Grado</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Cuotas</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Bloqueado</TableCell>
                                    <TableCell className={classes.tableCellBlueText}>Estado</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {cuentasSeleccionadasSegundaTabla.map((row, index) => (
                                    <TableRow key={index}>
                                        <TableCell>
                                            <Checkbox
                                                checked={checkboxStateTwo[index] || false}
                                                onChange={(event) => handleSelectFilaDos(event, index)}
                                            />
                                        </TableCell>
                                        <TableCell>{row.numeroCuenta}</TableCell>
                                        <TableCell>{row.nombreCuenta}</TableCell>
                                        <TableCell>
                                            {row.fechaCreacion ?
                                                new Date(row.fechaCreacion).toLocaleDateString('es-ES', { day: '2-digit', month: '2-digit', year: 'numeric' })
                                                : ''
                                            }
                                        </TableCell>

                                        <TableCell>
                                            {new Intl.NumberFormat('es-CO', {
                                                style: 'currency',
                                                currency: 'COP',
                                            }).format(row.saldo).replace('COP', '')}
                                        </TableCell>
                                        <TableCell>{dataFormCompleta}</TableCell>
                                        <TableCell>{dataFuerza}</TableCell>
                                        <TableCell>{dataUnidadEjecutora}</TableCell>
                                        <TableCell>{dataGrado}</TableCell>
                                        <TableCell>{row.cueotas}</TableCell>
                                        <TableCell>{row.bloqueos.length}</TableCell>
                                        <TableCell>{row.estadoCuenta}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                    <Grid item spacing={1}>
                        <Button
                            style={{ backgroundColor: '#a9a9a9', color: 'white', marginTop: "20px", marginLeft: "55rem", width: "20%" }}
                            onClick={handleTransferirCuentas}>Deseleccionar cuenta o cuentas
                        </Button>
                    </Grid>

                    <Grid item xs={6}>
                        <Controller
                            name="observaciones"
                            control={controlDos}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (

                                <CHTextField label="Observaciones" fullWidth multiline rows={4} />
                            )}
                        />
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
export default connect(mapStateToProps, mapDispatchToProps)(ModalUnificacionCuentas);
