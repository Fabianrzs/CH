import { Box, Button, Card, CardContent, CircularProgress, Container, createStyles, Dialog, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, TextField, Tooltip, Typography } from "@material-ui/core";
import { CloudDownload, Delete, Edit } from "@material-ui/icons";
import { Alert, AlertTitle } from "@material-ui/lab";
import MUIDataTable, { MUIDataTableColumn } from "mui-datatables";
import React, { useEffect, useRef, useState } from "react";
import { connect } from "react-redux";
import { toast } from "react-toastify";
import swal from "sweetalert";
import { startPostActualizarMovimientoComprobante } from "../../../actions/producto.action";
import { lstMovimientoComprobante } from "../../../models/cartera-comprobantes.model";
import { ProductState } from "../../../reducers/producto.reducer";
import CarteraApi from "../../../services/api-cartera-service";
import ApiReportes from "../../../services/api-reportes.service";
import { AppState } from "../../../store/rootReducer";
import { LoadingComponent } from "../../../components/loading/loading.component";
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import CatalogApi from "../../../services/api-catalog.service";



const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '2rem',
        },
        dialogContent: {
            width: '80%', // Ajusta el ancho aquí
            maxWidth: '1000px', // Ancho máximo opcional
            margin: 'auto',
        },
        columnDate: {
            minWidth: '110px',
        },
        summaryRow: {
            backgroundColor: '#f1f1f1'
        },
        summaryCell: {
            textAlign: 'right',
            fontWeight: 'bold',
        },
        tableStickyChecks: {
            '& .MuiTableCell-root:first-child': {
                position: 'sticky',
                left: 0,
                zIndex: 1200,
                backgroundColor: '#fff',
                minWidth: '48px',
                maxWidth: '48px',
                width: '48px',
            },

            '& .MuiTableHead-root .MuiTableCell-root:first-child': {
                position: 'sticky',
                left: 0,
                zIndex: 1400,
                backgroundColor: '#fff',
            },

            '& .MuiTableBody-root .MuiTableCell-root:first-child': {
                position: 'sticky',
                left: 0,
                zIndex: 1100,
                backgroundColor: '#fff',
            },
        },
    }),
);
type UpdateData = {
    Cuenta_ContableId: string;
    cuenta_Contable: string;
    debito: number;
    credito: number;
    editado?: boolean;
    agregado?: boolean;
    eliminado?: boolean;

};
const ModalEditarMovimientoCartera = (props: any) => {

    const classes = useStyles();
    const apiReportes = ApiReportes();
    const apiCartera = CarteraApi();


    const {
        open,
        handleClose,
        handleActualizarMovimiento,
        handleAgregarMovimiento,
        formularioActualizar,
        handleActualizarMovimientoPre,
        opciones,
        dataLista,
        setDataLista,
        resumen,
        productState,
        startPostActualizarMovimientoComprobante,
        tipoComprobanteCartera
    } = props;

    const defaultOptions = {
        setCellHeaderProps: () => ({
            className: 'filas'
        }),

        customBodyRender: (value: any) => {
            if (!value) return '';

            const date = new Date(value);

            // validar fecha inválida
            if (Number.isNaN(date.getTime())) {
                return value;
            }

            // formatear DD-MM-YYYY
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const year = date.getFullYear();

            return `${day}-${month}-${year}`;
        },
    };

    const defaultOptionsFixed = {
        setCellHeaderProps: () => ({
            className: 'filas'
        }),

        customBodyRender: (value: any) => value,
    };

    const formatDateDDMMYYYY = (value: any) => {
        if (!value) return '';

        const valueString = String(value).trim();

        // Caso: 2025-10-03T16:26:00 o 2025-10-03
        const isoMatch = valueString.match(/^(\d{4})-(\d{2})-(\d{2})/);
        if (isoMatch) {
            const [, year, month, day] = isoMatch;
            return `${day}-${month}-${year}`;
        }

        // Caso: 03-10-2025
        const ddmmyyyyMatch = valueString.match(/^(\d{2})-(\d{2})-(\d{4})$/);
        if (ddmmyyyyMatch) {
            return valueString;
        }

        // Caso: 03/10/2025
        const slashMatch = valueString.match(/^(\d{2})\/(\d{2})\/(\d{4})$/);
        if (slashMatch) {
            const [, day, month, year] = slashMatch;
            return `${day}-${month}-${year}`;
        }

        return valueString;
    };

    const formatFechaDentroDetalle = (detalle: string) => {
        if (!detalle) return '';

        const match = detalle.match(
            /\d{1,2}[/-]\d{1,2}[/-]\d{4} \d{1,2}:\d{2}:\d{2}( (AM|PM))?/i
        );

        if (!match) return detalle;

        const fechaOriginal = match[0];

        const partes = fechaOriginal.split(' ');
        const fechaParte = partes[0];
        const horaParte = partes[1];
        const ampm = partes[2];

        const separador = fechaParte.includes('/') ? '/' : '-';
        const [mes, dia, anio] = fechaParte.split(separador).map(Number);

        const horaSplit = horaParte.split(':').map(Number);
        let hora = horaSplit[0];
        const minutos = horaSplit[1];
        const segundos = horaSplit[2];

        if (ampm?.toUpperCase() === 'PM' && hora < 12) hora += 12;
        if (ampm?.toUpperCase() === 'AM' && hora === 12) hora = 0;

        const fechaFormateada =
            `${String(dia).padStart(2, '0')}-${String(mes).padStart(2, '0')}-${anio} ` +
            `${String(hora).padStart(2, '0')}:${String(minutos).padStart(2, '0')}:${String(segundos).padStart(2, '0')}`;

        return detalle.replace(fechaOriginal, fechaFormateada);
    };

    const formatMoneyEsCO = (value: any) => {
        if (value === null || value === undefined || value === '') return '';

        const numberValue = Number(value);

        if (Number.isNaN(numberValue)) return '';

        return new Intl.NumberFormat('es-CO', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        }).format(numberValue);
    };

    const parseMoneyEsCO = (value: string) => {
        if (!value) return 0;

        const cleanedValue = value
            .replace(/\s/g, '')
            .replace(/\$/g, '')
            .replace(/\./g, '')
            .replace(',', '.');

        const numberValue = Number(cleanedValue);

        return Number.isNaN(numberValue) ? 0 : numberValue;
    };

    const columns: MUIDataTableColumn[] = [

        {
            name: 'tipO_OPERACION',
            label: 'Tipo Operación',
            options: defaultOptionsFixed
        },
        {
            name: 'cuenta_Contable',
            label: 'Cuenta contable',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas',
                }),
                setCellProps: () => ({
                    style: {
                        backgroundColor: 'white',
                    }
                }),
                customBodyRender: (value: any) => {
                    const cuentaId = String(value ?? '');
                    const editedRow = updateData.find((item) => item.Cuenta_ContableId === cuentaId);
                    const editado = editedRow?.editado;
                    const editable = dataLista.tipo_Comprobante === "Pagos ventanilla"
                        || dataLista.tipo_Comprobante === "Provision de Cartera";

                    return (
                        <TextField
                            value={editedRow?.cuenta_Contable ?? cuentaId}
                            onChange={(e) => handleInputChange(e.target.value, cuentaId, 'cuenta_Contable')}
                            style={{
                                backgroundColor: editado ? 'yellow' : 'white',
                                width: '120px'
                            }}
                            InputProps={{
                                style: {
                                    color: 'black'
                                }
                            }}
                            disabled={!editable}
                        />
                    );
                }
            },
        },

        {
            name: 'descripcion_Cuenta',
            label: 'Descripcion de cuenta',
            options: defaultOptions
        },
        {
            name: 'numero_Identificacion',
            label: 'Numero Identificacion',
            options: defaultOptions
        },
        {
            name: 'nombre_Tercero',
            label: 'Nombre',
            options: defaultOptionsFixed
        },
        {
            name: 'fecha_Comprobante',
            label: 'Fecha Creación',
            options: {
                ...defaultOptions,
                customBodyRender: (value: any) => formatDateDDMMYYYY(value),
            }
        },
        {
            name: 'fecha_Transaccion',
            label: 'Fecha Transaccion',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => formatDateDDMMYYYY(value),
            }
        },
        {
            name: 'tipo_Comprobante',
            label: 'Tipo Comprobante',
            options: defaultOptions
        },
        {
            name: 'detalle',
            label: 'Detalle',
            options: {
                ...defaultOptions,
                customBodyRender: (value: any) => formatFechaDentroDetalle(value),
            }
        },
        {
            name: 'comprobante_Id',
            label: 'Comprobante Interno',
            options: defaultOptionsFixed
        },
        {
            name: 'comprobante_Seven_Id',
            label: 'Comprobante Seven',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => value === null || value === undefined ? 'N/A' : value
            }
        },
        {
            name: 'numero_Credito',
            label: 'Numero Credito',
            options: defaultOptions
        },
        {
            name: 'tipo_Credito',
            label: 'Tipo Credito',
            options: defaultOptions
        },
        {
            name: 'centro_Costo_Credito',
            label: 'Centro de costo',
            options: defaultOptions,
        },
        {
            name: 'centro_Costo',
            label: 'Area ',
            options: defaultOptionsFixed,
        },
        {
            name: 'debito',
            label: 'Débito',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas',
                    style: { width: '300px' }

                }),
                customBodyRender: (value: any, tableMeta: any) => {
                    const cuenta = tableMeta.rowData[1] ?? '';
                    const editedRow = updateData.find((item) => item.Cuenta_ContableId === tableMeta.rowData[1]);
                    const editado = editedRow?.editado;
                    // const editable = dataLista.tipo_Comprobante === "Pagos ventanilla";
                    const editable = dataLista.tipo_Comprobante === "Pagos ventanilla"
                        || dataLista.tipo_Comprobante === "Provision de Cartera";

                    // const editedRow = updateData.find((item) => item.Cuenta_ContableId === value);
                    // const editado = editedRow?.editado;
                    return (
                        // <TextField
                        //     value={new Intl.NumberFormat('es-CO', {
                        //         minimumFractionDigits: 2,
                        //         maximumFractionDigits: 2
                        //     }).format(Number(editedRow?.debito ?? value ?? 0))}
                        //     onChange={(e) => {
                        //         const rawValue = e.target.value
                        //             .replace(/\./g, '')   // quita puntos de miles
                        //             .replace(',', '.');   // convierte coma decimal en punto

                        //         handleInputChange(Number(rawValue), cuenta, 'debito');
                        //     }}
                        //     style={{
                        //         backgroundColor: editado ? 'yellow' : 'white',
                        //         width: '120px' // Ajusta este valor según lo necesites
                        //     }}
                        //     InputProps={{
                        //         style: {
                        //             color: 'black' // Mantener el texto negro incluso cuando está deshabilitado
                        //         }
                        //     }}
                        //     disabled={!editable}
                        // />
                        <TextField
                            value={
                                inputValues[`${cuenta}-debito`] ??
                                formatMoneyEsCO(editedRow?.debito ?? value ?? 0)
                            }
                            onChange={(e) => {
                                const inputValue = e.target.value;

                                setInputValues((prev) => ({
                                    ...prev,
                                    [`${cuenta}-debito`]: inputValue,
                                }));

                                handleInputChange(parseMoneyEsCO(inputValue), cuenta, 'debito');
                            }}
                            onBlur={() => {
                                const currentValue = inputValues[`${cuenta}-debito`];

                                if (currentValue !== undefined) {
                                    const parsedValue = parseMoneyEsCO(currentValue);

                                    setInputValues((prev) => {
                                        const newValues = { ...prev };
                                        delete newValues[`${cuenta}-debito`];
                                        return newValues;
                                    });

                                    handleInputChange(parsedValue, cuenta, 'debito');
                                }
                            }}
                            style={{
                                backgroundColor: editado ? 'yellow' : 'white',
                                width: '120px'
                            }}
                            InputProps={{
                                style: {
                                    color: 'black',

                                },
                                inputMode: 'decimal'
                            }}
                            disabled={!editable}
                        />
                    );
                },
            },
        },
        {
            name: 'credito',
            label: 'Crédito',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas',
                    style: { width: '300px' }

                }),
                customBodyRender: (value: any, tableMeta: any) => {
                    const cuenta = tableMeta.rowData[1] ?? '';
                    const editedRow = updateData.find((item) => item.Cuenta_ContableId === tableMeta.rowData[1]);
                    const editado = editedRow?.editado;
                    // const editable = dataLista.tipo_Comprobante === "Pagos ventanilla";
                    const editable = dataLista.tipo_Comprobante === "Pagos ventanilla"
                        || dataLista.tipo_Comprobante === "Provision de Cartera";

                    return (
                        // <TextField
                        //     value={new Intl.NumberFormat('es-CO', {
                        //         minimumFractionDigits: 2,
                        //         maximumFractionDigits: 2
                        //     }).format(Number(editedRow?.credito ?? value ?? 0))}

                        //     onChange={(e) => {
                        //         const rawValue = e.target.value
                        //             .replace(/\./g, '')   // quita puntos de miles
                        //             .replace(',', '.');   // convierte coma decimal en punto

                        //         handleInputChange(Number(rawValue), cuenta, 'credito');
                        //     }}
                        //     style={{
                        //         backgroundColor: editado ? 'yellow' : 'white',
                        //         width: '120px' // Ajusta este valor según lo necesites
                        //     }}
                        //     InputProps={{
                        //         style: {
                        //             color: 'black' // Mantener el texto negro incluso cuando está deshabilitado
                        //         }
                        //     }}
                        //     disabled={!editable}
                        // />
                        <TextField
                            value={
                                inputValues[`${cuenta}-credito`] ??
                                formatMoneyEsCO(editedRow?.credito ?? value ?? 0)
                            }
                            onChange={(e) => {
                                const inputValue = e.target.value;

                                setInputValues((prev) => ({
                                    ...prev,
                                    [`${cuenta}-credito`]: inputValue,
                                }));

                                handleInputChange(parseMoneyEsCO(inputValue), cuenta, 'credito');
                            }}
                            onBlur={() => {
                                const currentValue = inputValues[`${cuenta}-credito`];

                                if (currentValue !== undefined) {
                                    const parsedValue = parseMoneyEsCO(currentValue);

                                    setInputValues((prev) => {
                                        const newValues = { ...prev };
                                        delete newValues[`${cuenta}-credito`];
                                        return newValues;
                                    });

                                    handleInputChange(parsedValue, cuenta, 'credito');
                                }
                            }}
                            style={{
                                backgroundColor: editado ? 'yellow' : 'white',
                                width: '120px'
                            }}
                            InputProps={{
                                style: {
                                    color: 'black'
                                },
                                inputMode: 'decimal'
                            }}
                            disabled={!editable}
                        />
                    );
                },
            },
        },
        {
            name: 'acciones',
            label: 'Acciones',
            options: {
                filter: false,
                sort: false,
                download: false,
                print: false,
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRenderLite: (dataIndex: number) => {
                    const hayEliminacionMultiple = rowsSelected.length >= 2;
                    const filaSeleccionada = rowsSelected.includes(dataIndex);
                    const bloquearCanecaIndividual = hayEliminacionMultiple && filaSeleccionada;

                    return (
                        <Tooltip
                            title={
                                bloquearCanecaIndividual
                                    ? "Use la papelera superior para eliminar las filas seleccionadas"
                                    : "Eliminar movimiento"
                            }
                        >
                            <span>
                                <IconButton
                                    size="small"
                                    disabled={bloquearCanecaIndividual}
                                    onClick={() => {
                                        if (bloquearCanecaIndividual) return;
                                        handleEliminarFila(dataIndex);
                                    }}
                                >
                                    <Delete />
                                </IconButton>
                            </span>
                        </Tooltip>
                    );
                },
            },
        },
    ];

    const [downloading, setDownloading] = useState(false);
    const [rowsPerPage, setRowsPerPage] = React.useState(500);
    const [isProccessing, setProcessing] = useState(false);
    const [data, setData] = useState<any[]>([]);
    const [totalRegistros, setTotalRegistros] = useState(0);
    const [totals, setTotals] = useState<{ dataWithHeaderDates: any[] }>({ dataWithHeaderDates: [] });
    const [rowsSelected, setRowsSelected] = useState<number[]>([]);
    const [updateData, setUpdateData] = useState<UpdateData[]>([]);
    const [cuentaContableCounter, setCuentaContableCounter] = useState(1);
    const [inputValues, setInputValues] = useState<{ [key: string]: string }>({});
    const disableProps = { disabled: true };

    // useEffect(() => {
    //     setUpdateData([])
    //     if (dataLista) {

    //         const fecha_Comprobante = moment(dataLista.fecha_Comprobante).format('YYYY-MM-DD');
    //         const fecha_Transaccion = moment(dataLista.fecha_Transaccion).format('YYYY-MM-DD');
    //         const comprobante_Id = dataLista.comprobante_Id?.toString();
    //         const comprobante_Seven_Id = dataLista.comprobante_Seven_Id?.toString();
    //         const tipo_Comprobante = dataLista.tipo_Comprobante?.toString();

    //         const dataWithHeaderDates = (dataLista.lstMovimientoComprobante ?? []).map((row: any) => ({
    //             ...row,
    //             fecha_Comprobante, 
    //             fecha_Transaccion, 
    //             comprobante_Id,
    //             comprobante_Seven_Id,  
    //             tipo_Comprobante,
    //         }));

    //         setTotals({

    //             dataWithHeaderDates
    //         });
    //     }
    // }, [dataLista]); 

    const normalizarTexto = (value: any): string =>
        String(value ?? '')
            .trim()
            .toUpperCase()
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '');

    const comprobanteEstaCancelado = (estado: any): boolean =>
        normalizarTexto(estado) === 'CANCELADO';

    useEffect(() => {
        setUpdateData([]);
        setRowsSelected([]);

        if (!dataLista) {
            setTotals({
                dataWithHeaderDates: [],
            });
            return;
        }

        if (comprobanteEstaCancelado(dataLista.estado)) {
            setTotals({
                dataWithHeaderDates: [],
            });

            setTotalRegistros(0);
            return;
        }

        const fecha_Comprobante = formatDateDDMMYYYY(
            dataLista.fecha_Comprobante
        );

        const fecha_Transaccion = formatDateDDMMYYYY(
            dataLista.fecha_Transaccion
        );

        const comprobante_Id =
            dataLista.comprobante_Id?.toString();

        const comprobante_Seven_Id =
            dataLista.comprobante_Seven_Id != null
                ? dataLista.comprobante_Seven_Id.toString()
                : 'N/A';

        const tipo_Comprobante =
            dataLista.tipo_Comprobante?.toString();

        const dataWithHeaderDates = (
            dataLista.lstMovimientoComprobante ?? []
        ).map((row: any) => {
            let detalleSinFechaYHora = row.detalle;

            if (typeof detalleSinFechaYHora === 'string') {
                detalleSinFechaYHora =
                    detalleSinFechaYHora.replace(
                        /\b\d{4}[-/]\d{1,2}[-/]\d{1,2}([T\s]\d{1,2}:\d{2}(:\d{2})?)?\b|\b\d{1,2}[-/]\d{1,2}[-/]\d{4}([T\s]\d{1,2}:\d{2}(:\d{2})?)?\b|(\bAM\b|\bPM\b)/gi,
                        ''
                    );
            }

            return {
                ...row,
                detalle: detalleSinFechaYHora,
                fecha_Comprobante,
                fecha_Transaccion,
                comprobante_Id,
                comprobante_Seven_Id,
                tipo_Comprobante,
            };
        });

        setTotals({
            dataWithHeaderDates,
        });

        setTotalRegistros(
            dataWithHeaderDates[0]?.total_Registros ??
            dataWithHeaderDates.length
        );
    }, [dataLista]);


    const handleInputChange = (value: any, cuentaId: string, columnName: keyof UpdateData) => {

        setUpdateData((prevUpdateData) => {
            const existingIndex = prevUpdateData.findIndex((item) => item.Cuenta_ContableId === cuentaId);
            const dataPre = totals.dataWithHeaderDates.find((item: any) => item.cuenta_Contable === cuentaId) as any;

            if (existingIndex >= 0) {
                const updatedItem = { ...prevUpdateData[existingIndex] };

                if (columnName === 'cuenta_Contable') {
                    updatedItem.cuenta_Contable = value as string;
                } else if (columnName === 'debito') {
                    updatedItem.debito = value as number;
                } else if (columnName === 'credito') {
                    updatedItem.credito = value as number;
                }

                updatedItem.editado = true;

                return [
                    ...prevUpdateData.slice(0, existingIndex),
                    updatedItem,
                    ...prevUpdateData.slice(existingIndex + 1),
                ];
            }

            return [
                ...prevUpdateData,
                {
                    Cuenta_ContableId: cuentaId,
                    cuenta_Contable: columnName === 'cuenta_Contable' ? value as string : cuentaId,
                    debito: columnName === 'debito' ? value as number : dataPre?.debito ?? 0,
                    credito: columnName === 'credito' ? value as number : dataPre?.credito ?? 0,
                    editado: true,
                    agregado: false

                },
            ];
        });
    };

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
    const [localIpAddress, setLocalIpAddress] = useState<string | null>(null);
    const catalogApi = CatalogApi();

    const isValidIPv4 = (ip: string) => {
        const ipv4Regex = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
        return ipv4Regex.test(ip);
    };

    const isValidIPv6 = (ip: string) => {
        const ipv6Regex = /([a-f0-9:]+:+)+[a-f0-9]+/i;
        return ipv6Regex.test(ip);
    };


    const enviarIp = async (ip: any) => {
        const convertIp = ip.split('.')[0]

        const hexadecimal = convertIp
        await catalogApi
            .getObtenerIpLocal(hexadecimal)
            .then(async (response: any) => {

                if (response) {
                    if (response.isSuccess) {
                        setPublicIpAddress(response.data);
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }

    useEffect(() => {
        const fetchLocalIPAddress = () => {
            const peerConnection = new RTCPeerConnection({ iceServers: [] });
            peerConnection.createDataChannel('');
            peerConnection.createOffer()
                .then((offer) => peerConnection.setLocalDescription(offer))
                .catch((error) => console.error('Error creating offer:', error));

            peerConnection.onicecandidate = (event) => {
                if (event.candidate) {
                    const ip: any = event.candidate.address;
                    
                    if (isValidIPv4(ip) || isValidIPv6(ip)) {
                        setLocalIpAddress(ip);
                        enviarIp(ip);
                    } else {
                        setLocalIpAddress(ip);
                        enviarIp(ip);
                    }
                }
            };

            return () => {
                peerConnection.close();
            };
        };
        fetchLocalIPAddress();

    }, []);

    const handleAgregar = () => {

        const originalData = totals.dataWithHeaderDates[0];

        if (originalData && typeof originalData === 'object' && !Array.isArray(originalData)) {
            const newDataRow = {
                ...(originalData as any),
                cuenta_Contable: '',
                descripcion_Cuenta: "Agrega Detalle",
                debito: 0,
                credito: 0,
                editado: false,
                agregado: true,
            };

            setCuentaContableCounter(prevCounter => prevCounter + 1);
            setTotals((prevTotals: any) => ({
                ...prevTotals,
                dataWithHeaderDates: [...prevTotals.dataWithHeaderDates, newDataRow],
            }));

            setUpdateData((prevUpdateData) => [
                ...prevUpdateData,
                {
                    Cuenta_ContableId: `nuevo-${cuentaContableCounter}`,
                    cuenta_Contable: '',
                    debito: 0,
                    credito: 0,
                    editado: false,
                    agregado: true
                }
            ]);


            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = window.navigator as any;
            const usrAg = nav.userAgentData.brands.find(
                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand;

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Agregar Detalle Contable',
                hostname: document.location.host,
                tipoAccion: 'INSERCIÓN',
                idTipoAccion: 2,
                descripcion: `Se agregó un nuevo detalle contable con los siguientes datos:
                \n Cuenta Contable: ${newDataRow.cuenta_Contable}
                \n Descripción: ${newDataRow.descripcion_Cuenta}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } else {
            console.warn("No hay datos originales válidos para copiar.");
        }
    };


    const handleActualizarMovimientos = async () => {

        const CodigoMovimiento = tipoComprobanteCartera.find(
            (item: any) => item.tipoComprobante === dataLista.tipo_Comprobante
        );

        // const movimientosSinEditado = updateData.map(({ editado, ...resto }) => resto);

        const respuesta = await swal({
            text: 'Este proceso actualizará los datos. ¿Desea continuar?',
            icon: 'warning',
            buttons: {
                confirm: {
                    text: "Sí",
                    value: true,
                    visible: true,
                },
                cancel: {
                    text: "No",
                    value: false,
                    visible: true,
                },
            },
        });


        if (respuesta) {
            const request = {
                Comprobante_Id: dataLista.comprobante_Id,
                Tipo_Credito: dataLista.lstMovimientoComprobante[0].tipo_Credito,
                CodigoProceso: CodigoMovimiento.codigo,
                Movimientos: updateData,
            };

            await startPostActualizarMovimientoComprobante(request);
            setUpdateData([]);
            changePage(1, 500)

            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = (window.navigator as any);
            const usrAg = nav.userAgentData.brands.find(
                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand || 'Desconocido';

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Actualización de Movimientos',
                hostname: document.location.host,
                tipoAccion: 'ACTUALIZACION',
                idTipoAccion: 5,
                descripcion: `Se han actualizado movimientos con los siguientes detalles:
            \n ID Comprobante: ${dataLista.comprobante_Id}
            \n Código de Movimiento: ${CodigoMovimiento.codigo}
            \n Número de Movimientos: ${updateData.length}`
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        }
    };

    const handleClickBorrar = async () => {
        setRowsSelected([]);

        setTotals((prevTotals: any) => ({
            ...prevTotals,
            dataWithHeaderDates: prevTotals.dataWithHeaderDates.map((row: any) => {
                if (row.agregado) {
                    return {
                        ...row,
                        cuenta_Contable: '',
                        descripcion_Cuenta: 'Agrega Detalle',
                        debito: 0,
                        credito: 0,
                        editado: false,
                    };
                }

                return row;
            }),
        }));

        setUpdateData((prevUpdateData) =>
            prevUpdateData
                .filter((item) => item.agregado)
                .map((item) => ({
                    ...item,
                    cuenta_Contable: '',
                    debito: 0,
                    credito: 0,
                    editado: false,
                }))
        );
    };

    const registrarMovimientoEliminado = (row: any) => {
        if (!row) return;

        setUpdateData((prevUpdateData) => {
            const existingIndex = prevUpdateData.findIndex(
                (item) => item.Cuenta_ContableId === row.cuenta_Contable
            );

            if (existingIndex >= 0) {
                const existingItem = prevUpdateData[existingIndex];

                if (existingItem.agregado) {
                    return prevUpdateData.filter(
                        (item) => item.Cuenta_ContableId !== row.cuenta_Contable
                    );
                }

                const updatedItem = {
                    ...existingItem,
                    eliminado: true,
                    editado: true,
                };

                return [
                    ...prevUpdateData.slice(0, existingIndex),
                    updatedItem,
                    ...prevUpdateData.slice(existingIndex + 1),
                ];
            }

            return [
                ...prevUpdateData,
                {
                    Cuenta_ContableId: row.cuenta_Contable,
                    cuenta_Contable: row.cuenta_Contable,
                    debito: row.debito ?? 0,
                    credito: row.credito ?? 0,
                    editado: true,
                    agregado: false,
                    eliminado: true,
                },
            ];
        });
    };

    const obtenerReferenciaMovimiento = (row: any): string | null => {
        const referencia =
            row?.dmc_refe ??
            row?.dmc_Refe ??
            row?.dmc_REFE ??
            row?.DMC_REFE ??
            row?.numero_Identificacion ??
            row?.numeroIdentificacion ??
            null;

        const referenciaNormalizada = String(referencia ?? '').trim();

        if (
            !referenciaNormalizada ||
            referenciaNormalizada === '0' ||
            referenciaNormalizada.toLowerCase() === 'null' ||
            referenciaNormalizada.toLowerCase() === 'undefined'
        ) {
            return null;
        }

        return referenciaNormalizada;
    };


    const handleEliminarFila = async (dataIndex: number) => {
        const rowEliminar = totals.dataWithHeaderDates[dataIndex];

        if (!rowEliminar) return;

        if (dataLista.estado !== 'Pendiente') {
            toast('Solo se pueden eliminar afiliados cuando el comprobante está en estado Pendiente', {
                type: toast.TYPE.WARNING,
            });
            return;
        }

        

        const referenciaEliminar = obtenerReferenciaMovimiento(rowEliminar);


        if (!referenciaEliminar) {
            toast('No se encontró la referencia del afiliado dentro del comprobante', {
                type: toast.TYPE.ERROR,
            });
            return;
        }

        const respuesta = await swal({
            text: `¿Desea eliminar esta fila del comprobante?`,
            icon: 'warning',
            buttons: {
                confirm: {
                    text: "Sí",
                    value: true,
                    visible: true,
                },
                cancel: {
                    text: "No",
                    value: false,
                    visible: true,
                },
            },
        });

        if (!respuesta) return;

        try {
            setProcessing(true);

            const user = AuthService.getUserInfo();

            const request = {
                cC_ID: Number(dataLista.comprobante_Id),
                dmc_refe: [referenciaEliminar],
                modificadoPor: user.USR_ID,
            };


            const response = await apiCartera.postEliminarAfiliadoDelComprobante(request);


            if (response?.isSuccess) {
                toast(
                    response.returnMessage ||
                    'Afiliado eliminado correctamente',
                    {
                        type: toast.TYPE.SUCCESS,
                    }
                );

                setDataLista((prevData: any) => ({
                    ...prevData,
                    estado: 'Cancelado',
                    lstMovimientoComprobante: [],
                    total_Registros: 0,
                }));

                setTotals({
                    dataWithHeaderDates: [],
                });

                setTotalRegistros(0);
                setRowsSelected([]);
                setUpdateData([]);
            } else {
                toast(
                    response?.returnMessage ||
                    'No fue posible eliminar el afiliado',
                    {
                        type: toast.TYPE.ERROR,
                    }
                );
            }
        } catch (error) {
            console.error(error);
            toast('Error al eliminar la fila del comprobante', {
                type: toast.TYPE.ERROR,
            });
        } finally {
            setProcessing(false);
        }
    };


    const handleEliminarFilasSeleccionadas = async (selectedRowsData: any) => {
        const cantidad = selectedRowsData?.data?.length ?? 0;

        if (cantidad === 0) return;

        if (dataLista.estado !== 'Pendiente') {
            toast('Solo se pueden eliminar movimientos cuando el comprobante está en estado Pendiente', {
                type: toast.TYPE.WARNING,
            });
            return;
        }

        const indicesEliminar = selectedRowsData.data.map(
            (row: any) => row.dataIndex
        );

        const filasSeleccionadas = indicesEliminar.map(
            (dataIndex: number) => totals.dataWithHeaderDates[dataIndex]
        );

        const filasConReferencia = filasSeleccionadas.map((row: any) => ({
            row,
            referencia: obtenerReferenciaMovimiento(row),
        }));


        const filasSinReferencia = filasConReferencia.filter(
            (item: any) => !item.referencia
        );

        if (filasSinReferencia.length > 0) {
            console.error(
                'Filas sin una referencia válida para eliminar:',
                filasSinReferencia
            );

            toast(
                `No fue posible identificar ${filasSinReferencia.length} fila(s) seleccionada(s)`,
                {
                    type: toast.TYPE.ERROR,
                }
            );
            return;
        }

        const referenciasEliminar = Array.from(
            new Set(
                filasConReferencia.map(
                    (item: any) => item.referencia as string
                )
            )
        );

        const respuesta = await swal({
            text: `¿Desea eliminar ${referenciasEliminar.length} afiliado(s) del comprobante?`,
            icon: 'warning',
            buttons: {
                confirm: {
                    text: 'Sí',
                    value: true,
                    visible: true,
                },
                cancel: {
                    text: 'No',
                    value: false,
                    visible: true,
                },
            },
        });

        if (!respuesta) return;

        try {
            setProcessing(true);

            const user = AuthService.getUserInfo();

            const request = {
                cC_ID: Number(dataLista.comprobante_Id),
                dmc_refe: referenciasEliminar,
                modificadoPor: user.USR_ID,
            };


            const response = await apiCartera.postEliminarAfiliadoDelComprobante(request);

            if (response?.isSuccess) {
                toast(response.returnMessage || `${cantidad} fila(s) eliminada(s) correctamente`, {
                    type: toast.TYPE.SUCCESS,
                });

                const nuevaDataLista = response.data;

                setDataLista(nuevaDataLista);

                const fecha_Comprobante = formatDateDDMMYYYY(nuevaDataLista.fecha_Comprobante);
                const fecha_Transaccion = formatDateDDMMYYYY(nuevaDataLista.fecha_Transaccion);

                const dataWithHeaderDates = (nuevaDataLista.lstMovimientoComprobante ?? []).map((row: any) => ({
                    ...row,
                    fecha_Comprobante,
                    fecha_Transaccion,
                    comprobante_Id: nuevaDataLista.comprobante_Id?.toString(),
                    comprobante_Seven_Id: nuevaDataLista.comprobante_Seven_Id?.toString(),
                    tipo_Comprobante: nuevaDataLista.tipo_Comprobante?.toString(),
                }));

                setTotals({
                    dataWithHeaderDates,
                });

                setRowsSelected([]);
                setUpdateData([]);
            } else {
                toast(response?.returnMessage || 'No fue posible eliminar las filas seleccionadas', {
                    type: toast.TYPE.ERROR,
                });
            }
        } catch (error) {
            console.error(error);
            toast('Error al eliminar las filas seleccionadas del comprobante', {
                type: toast.TYPE.ERROR,
            });
        } finally {
            setProcessing(false);
        }
    };

    // const handleEliminarFilasSeleccionadas = async (selectedRowsData: any) => {
    //     const cantidad = selectedRowsData?.data?.length ?? 0;

    //     if (cantidad === 0) return;

    //     if (dataLista.estado !== 'Pendiente') {
    //         toast('Solo se pueden eliminar movimientos cuando el comprobante está en estado Pendiente', {
    //             type: toast.TYPE.WARNING,
    //         });
    //         return;
    //     }

    //     const indicesEliminar = selectedRowsData.data.map(
    //         (row: any) => row.dataIndex
    //     );

    //     const respuesta = await swal({
    //         text: `¿Desea eliminar ${cantidad} fila(s) seleccionada(s) del comprobante?`,
    //         icon: 'warning',
    //         buttons: {
    //             confirm: {
    //                 text: "Sí",
    //                 value: true,
    //                 visible: true,
    //             },
    //             cancel: {
    //                 text: "No",
    //                 value: false,
    //                 visible: true,
    //             },
    //         },
    //     });

    //     if (!respuesta) return;

    //     setTotals((prevTotals: any) => ({
    //         ...prevTotals,
    //         dataWithHeaderDates: prevTotals.dataWithHeaderDates.filter(
    //             (_item: any, index: number) => !indicesEliminar.includes(index)
    //         ),
    //     }));

    //     setRowsSelected([]);

    //     toast(`${cantidad} fila(s) eliminada(s) visualmente`, {
    //         type: toast.TYPE.SUCCESS,
    //     });
    // };


    const handleExportToExcel = async () => {
        try {
            const response = await apiCartera.postGenerarExcelComprobanteDetalle(dataLista.detalle_Movimiento_Id);
            const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `Comprobante ${dataLista.comprobante_Id} ${formatFechaDentroDetalle(dataLista.lstMovimientoComprobante[0].detalle)}.xlsx`;
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(link);

            // Lógica de auditoría
            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = window.navigator as any;
            const usrAg = nav.userAgentData.brands.find(
                (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
            )?.brand;

            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Exportación de Comprobantes',
                hostname: document.location.host,
                tipoAccion: 'EXPORTACIÓN',
                idTipoAccion: 4,
                descripcion: `Exportación de comprobante a Excel:
                \n ID Comprobante: ${dataLista.comprobante_Id}
                \n Detalle: ${dataLista.lstMovimientoComprobante[0].detalle}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);

        } catch (error) {
            toast("Ocurrió un error al intentar exportar a Excel. Por favor, inténtalo de nuevo.");
        }
    };



    const handleClick = async () => {
        try {
            setDownloading(true);
            await handleExportToExcel();
        } finally {
            setDownloading(false);
        }
    }

    const changePage = async (
        page: number,
        rowsPerPageValue: number
    ) => {
        if (comprobanteEstaCancelado(dataLista?.estado)) {
            setTotals({
                dataWithHeaderDates: [],
            });

            setTotalRegistros(0);
            setRowsSelected([]);
            return;
        }

        setProcessing(true);

        try {
            const response =
                await apiCartera.postObtenerMovimientosContableCartera({
                    idComprobante:
                        dataLista.detalle_Movimiento_Id,
                    registrosPorPagina:
                        rowsPerPageValue,
                    numeroPagina: page,
                });

            if (response?.isSuccess) {
                const movimientos =
                    response.data ?? [];

                setDataLista((prevData: any) => ({
                    ...prevData,
                    lstMovimientoComprobante:
                        comprobanteEstaCancelado(
                            prevData?.estado
                        )
                            ? []
                            : movimientos,
                }));

                setTotalRegistros(
                    movimientos[0]?.total_Registros ??
                    movimientos.length
                );
            } else {
                setTotals({
                    dataWithHeaderDates: [],
                });

                setTotalRegistros(0);

                toast(
                    response?.returnMessage ||
                    'No fue posible consultar los movimientos',
                    {
                        type: toast.TYPE.ERROR,
                    }
                );
            }
        } finally {
            setProcessing(false);
        }
    };



    return (
        <>
            <>
                <Dialog
                    open={open}
                    onClose={handleClose}
                    aria-labelledby="form-dialog-title"
                    maxWidth="lg"
                >
                    <LoadingComponent key="loader" loading={productState.loading} />
                    <Container maxWidth="lg">
                        <Card style={{ width: '1200px' }}>
                            <CardContent>
                                {opciones === 1 &&
                                    <Grid item>
                                        <Typography
                                            className={classes.titleform}
                                            variant="h2"
                                            component="h6"
                                        >
                                            DETALLES DEL COMPROBANTE
                                        </Typography>
                                    </Grid>
                                }
                                {opciones === 2 &&
                                    <Grid item>
                                        <Typography
                                            className={classes.titleform}
                                            variant="h5"
                                            component="h6"
                                        >
                                            AGREGAR MOVIMIENTO
                                        </Typography>
                                    </Grid>
                                }

                                <Grid container style={{ margin: '1rem 0' }} item xs={12}>
                                    <Alert severity="info" elevation={2}>
                                        <AlertTitle><strong>DETALLE</strong></AlertTitle>
                                        {/* Descripción como cabecera */}
                                        <Typography variant="body1" style={{ marginBottom: '1rem' }}>

                                            {dataLista.tipo_Comprobante
                                                ? `${dataLista.tipo_Comprobante} : ${formatFechaDentroDetalle(dataLista.lstMovimientoComprobante[0].detalle)} `
                                                : 'No hay detalles disponibles.'}
                                            <br />
                                            {`Comprobante Interno No. ${dataLista.comprobante_Id}.`}
                                            <br />
                                            {`Comprobante Seven No. ${dataLista.comprobante_Seven_Id ?? 'N/A'}.`}
                                        </Typography>
                                        <div style={{ display: 'flex', flexWrap: 'wrap', gap: '6rem' }}>
                                            <div>
                                                <strong>Total Diferencia:</strong>
                                                <span>{dataLista.total_Comprobante_Credito - dataLista.total_Comprobante_Debito}</span>
                                            </div>
                                            <div>
                                                <strong>Total Crédito:</strong>
                                                <span>
                                                    {new Intl.NumberFormat('es-CO', {
                                                        style: 'currency',
                                                        currency: 'COP',
                                                    }).format(dataLista.total_Comprobante_Credito || 0)}
                                                </span>
                                            </div>
                                            <div>
                                                <strong>Total Débito:</strong>
                                                <span>
                                                    {new Intl.NumberFormat('es-CO', {
                                                        style: 'currency',
                                                        currency: 'COP',
                                                    }).format(dataLista.total_Comprobante_Debito || 0)}
                                                </span>
                                            </div>

                                        </div>
                                    </Alert>
                                </Grid>


                                <div className={classes.tableStickyChecks}>

                                    {dataLista.tipo_Comprobante === "Pagos ventanilla" && dataLista.estado === "Pendiente" && (
                                        <Grid className="container-23" style={{ display: 'flex', justifyContent: 'flex-end', paddingLeft: '70%' }}>
                                            <Grid>
                                                <button type="button" className="bgcian button-i blue flex" onClick={handleAgregar}>
                                                    <span className="ico fch-Agregar-0-1 icoMid-2 separador-marg-6" />
                                                    AGREGAR
                                                </button>
                                            </Grid>
                                        </Grid>
                                    )}
                                    <MUIDataTable
                                        title=""
                                        data={totals.dataWithHeaderDates ?? []}
                                        columns={columns}
                                        options={{
                                            onChangePage(currentPage: any) {
                                                changePage(currentPage + 1, rowsPerPage);
                                            },

                                            selectableRows: 'multiple',
                                            selectableRowsHeader: true,
                                            rowsSelected,

                                            onRowSelectionChange: (
                                                currentRowsSelected: any,
                                                allRowsSelected: any,
                                                rowsSelectedData: number[]
                                            ) => {
                                                setRowsSelected(rowsSelectedData);
                                            },

                                            customToolbarSelect: (selectedRows: any) => {
                                                const cantidadSeleccionada = selectedRows?.data?.length ?? 0;

                                                if (cantidadSeleccionada < 2) {
                                                    return null;
                                                }

                                                return (
                                                    <Tooltip title="Eliminar seleccionados">
                                                        <IconButton
                                                            onClick={() => handleEliminarFilasSeleccionadas(selectedRows)}
                                                        >
                                                            <Delete />
                                                        </IconButton>
                                                    </Tooltip>
                                                );
                                            },

                                            onRowsDelete: () => false,

                                            filter: false,
                                            print: false,
                                            search: true,
                                            elevation: 2,
                                            filterType: 'dropdown',
                                            responsive: 'vertical',
                                            fixedHeader: true,
                                            fixedSelectColumn: true,
                                            tableBodyHeight: '500px',

                                            download: false,
                                            customToolbar: () => (
                                                <Tooltip title="Descargar Excel">
                                                    {downloading ? (
                                                        <CircularProgress
                                                            size={20}
                                                            style={{
                                                                verticalAlign: 'middle',
                                                                marginLeft: '14px',
                                                                marginRight: '14px',
                                                            }}
                                                        />
                                                    ) : (
                                                        <IconButton onClick={handleClick}>
                                                            <CloudDownload />
                                                        </IconButton>
                                                    )}
                                                </Tooltip>
                                            ),

                                            textLabels: {
                                                body: {
                                                    noMatch: 'No se encontraron resultados',
                                                },
                                                pagination: {
                                                    rowsPerPage: 'Filas por página',
                                                    jumpToPage: '',
                                                },
                                                toolbar: {
                                                    downloadCsv: 'Descargar CSV',
                                                    search: 'Buscar',
                                                    viewColumns: 'Mostrar Columnas',
                                                },
                                                viewColumns: {
                                                    title: 'Mostrar Columnas',
                                                },
                                                selectedRows: {
                                                    text: 'fila(s) seleccionadas',
                                                    delete: 'Eliminar',
                                                    deleteAria: 'Eliminar filas seleccionadas',
                                                },
                                            },

                                            customFooter: (
                                                rowCount: number,
                                                page: number,
                                                rowsPerPageTable: number,
                                                changeRowsPerPage: (page: string | number) => void,
                                                changePageFooter: (newPage: number) => void
                                            ) => (
                                                <TablePagination
                                                    rowsPerPageOptions={[100, 300, 500]}
                                                    colSpan={3}
                                                    count={(totals.dataWithHeaderDates as lstMovimientoComprobante[])?.[0]?.total_Registros || 0}
                                                    rowsPerPage={rowsPerPageTable}
                                                    page={page}
                                                    labelRowsPerPage="Filas por página"
                                                    onPageChange={(_: any, pageChange: number) => changePageFooter(pageChange)}
                                                    onRowsPerPageChange={(event) => {
                                                        const value = parseInt(event.target.value, 10);
                                                        changeRowsPerPage(value);
                                                        setRowsPerPage(value);
                                                        changePageFooter(0);
                                                    }}
                                                    backIconButtonProps={isProccessing ? disableProps : undefined}
                                                    nextIconButtonProps={isProccessing ? disableProps : undefined}
                                                    SelectProps={isProccessing ? disableProps : undefined}
                                                />
                                            ),
                                        } as any}
                                    />
                                </div>
                                <Grid container spacing={3}>
                                    <Grid item xs={12}>
                                        <Button
                                            size="small"
                                            variant="contained"
                                            color="default"
                                            onClick={handleClose}
                                            style={{ marginLeft: "10px", marginTop: "20px" }}
                                        >
                                            Salir
                                        </Button>

                                        {(dataLista.tipo_Comprobante === "Pagos ventanilla" || dataLista.tipo_Comprobante === "Provision de Cartera")
                                            && dataLista.estado === "Pendiente" && (
                                                <>
                                                    <Button
                                                        size="small"
                                                        variant="contained"
                                                        color="default"
                                                        style={{ marginLeft: "10px", marginTop: "20px" }}
                                                        onClick={handleClickBorrar}
                                                    >
                                                        Limpiar
                                                    </Button>

                                                    <Button
                                                        size="small"
                                                        variant="contained"
                                                        color="default"
                                                        style={{ marginLeft: "10px", marginTop: "20px" }}
                                                        onClick={handleActualizarMovimientos}
                                                    >
                                                        Actualizar
                                                    </Button>
                                                </>
                                            )}
                                    </Grid>
                                </Grid>

                            </CardContent>
                        </Card>
                    </Container>
                </Dialog>
            </>

        </>
    )
}
interface LinkMapStateToProps {

    productState: ProductState;

}
const mapStateToProps = (state: AppState) => ({
    productState: state.producto,
});

const mapDispatchToProps = {
    startPostActualizarMovimientoComprobante
};
interface LinkMapDispatchToProps {

    startPostActualizarMovimientoComprobante: (data: any) => void;

}

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ModalEditarMovimientoCartera);

