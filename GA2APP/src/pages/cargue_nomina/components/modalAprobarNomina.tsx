import { Box, Button, Card, CardContent, Container, createStyles, Dialog, DialogContent, Grid, makeStyles, MenuItem, Paper, Tab, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Tabs, TextareaAutosize, TextField, Theme, Typography } from "@material-ui/core";
import MUIDataTable from "mui-datatables";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { connect } from "react-redux";
import { toast } from "react-toastify";
import swal from "sweetalert";
import { startActualizarSolicitudNomina } from '../../../actions/cargue-nomina.action';
import CHTextField from "../../../components/ch-text-field/CHTextField";
import ContainerTitle from "../../../components/containe-title/container-title";
import IconTitle from "../../../components/page-title-icon/page-title-icon";
import { CargueNominaState } from "../../../reducers/cargue-nomina.reducer";
import { CatalogState } from "../../../reducers/catalog.reducer";
import ApiCargueNomina from "../../../services/api-carguenomina.service";
import CreditApi from "../../../services/api-credit.service";
import NotificationService from "../../../services/notification.service";
import { AppState } from "../../../store/rootReducer";
import UtilService from "../../../utils/util.service";
import TablaCargueComprobante from "./tablaCargueComprobante";


const useStyles = makeStyles((theme: Theme) =>
    createStyles({
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
        columnButton: {
            marginTop: 'auto',
            display: 'flex',
            // justifyContent: 'flex-end',

        },
        button: {
            width: '120px',
            margin: theme.spacing(1),
            marginBottom: '1rem',
        },
        textarea: {
            width: '100%',
            minHeight: '100px',
            padding: '10px',
            fontSize: '20px',
            border: '1px solid #ccc',
            borderRadius: '4px',
            resize: 'vertical',
            '&:focus': {
                outline: 'visible',

                borderColor: "fec917",
            },
        },
        columnButtonAdd: {
            textAlign: 'right',
        },

        errorTextarea: {

        },
        title: {
            marginRight: "30px",
            fontSize: "15px",
            color: "fec917",
            textAlign: "center",
            marginTop: "20px",
        },

        fixedTabs: {
            position: 'sticky',
            top: 0,
            zIndex: 10, // asegúrate que esté por encima del contenido
            backgroundColor: 'white',
            paddingTop: 8,
        },

    }),
);


function TabPanel(props: any) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`simple-tabpanel-${index}`}
            aria-labelledby={`simple-tab-${index}`}
            {...other}
        >
            {value === index && (
                <Box p={3}>
                    <Typography>{children}</Typography>
                </Box>
            )}
        </div>
    );
}

const ModalAprobarNomina = (props: any) => {
    const {
        open,
        handleClose,
        cargueNominaState,
        startActualizarSolicitudNomina,
        catalogState,
    } = props;

    const apiCargueNomina = ApiCargueNomina()

    const classes = useStyles();

    const [tabIndex, setTabIndex] = useState(0);
    const [estado, setEstado] = useState(false);
    const [estadoAprobar, setEstadoAprobar] = useState(false);

    const formularioAprobarNomina = useForm({
        defaultValues: {
            numeroSolicitud: "",
            fechaInicio: "",
            fechaFin: "",
            numeroRadicado: "",
            monto: "",
            periodo: "",
            tipoArchivo: "",
            observaciones: "",
            autorizadoPor: "",
            fechaInicialDocumento: "",
            observacion: "",
            numeroComprobante: "",

        }
    })

    const formularioSeven = useForm({
        defaultValues: {
            numeroComprobante: "",
        }
    })

    const registros = cargueNominaState.documentosCarga?.registros?.toString() || '0';
    const monto = UtilService.formatCurrency(cargueNominaState.documentosCarga?.monto?.toString() || '0');

    // Por defecto, asignamos los valores a 'No Cargados'
    let cargadosReg = '0';
    let cargadosMonto = '0';
    let noCargadosReg = registros;
    let noCargadosMonto = monto;

    // Si la condición se cumple, movemos los datos a 'Cargados'
    if (estado === false && estadoAprobar === true) {
        cargadosReg = registros;
        cargadosMonto = monto;
        noCargadosReg = '0';
        noCargadosMonto = '0';
    }

    const scrollableData = [
        ['Total', registros, monto],
        ['Cargados', cargadosReg, cargadosMonto],
        ['No Cargados', noCargadosReg, noCargadosMonto],
        ['Con Inconsistencias', '0', '0'],
    ];

    // const scrollableData = [
    //     ['Total', cargueNominaState.documentosCarga?.registros?.toString() || '0',
    //     UtilService.formatCurrency(cargueNominaState.documentosCarga?.monto?.toString() || '0') ],
    //     ['Cargados', '0', '0'],
    //     [
    //       'No Cargados',
    //       cargueNominaState.documentosCarga?.registros?.toString() || '0',
    //       UtilService.formatCurrency(cargueNominaState.documentosCarga?.monto?.toString() || '0') 
    //     ],
    //     ['Con Inconsistencias', '0', '0'],
    //   ];

    const columns2 = [
        {
            name: 'Item',
            label: 'Item',
            // options: {
            //     display: 'excluded', // Excluimos esta columna del cuerpo de la tabla
            // },
        },
        { name: 'Registros', label: 'Registros' },
        { name: 'Monto', label: 'Monto' }
    ];

    useEffect(() => {

        const estadoNuevo = cargueNominaState.consultaArchivosCargadosN.some((x: any) => x.estado === "PENDIENTE POR PROCESAR")

        if (estadoNuevo) {
            setEstado(true)
            setEstadoAprobar(false)
        } else {
            setEstadoAprobar(true)
            setEstado(false)
        }


    }, [open])


    const estadoDocumentoAprobar = cargueNominaState.documentosCarga
    if (estadoDocumentoAprobar) {
        formularioAprobarNomina.setValue('numeroSolicitud', estadoDocumentoAprobar.numeroSolicitud)
        formularioAprobarNomina.setValue('numeroRadicado', estadoDocumentoAprobar?.numeroRadicado)
        formularioAprobarNomina.setValue('fechaInicio', UtilService.getFullDateWithTimeDirection(new Date(estadoDocumentoAprobar?.fechaInicialDocumento)))
        formularioAprobarNomina.setValue('fechaFin', UtilService.getFullDateWithTimeDirection(estadoDocumentoAprobar.fechaFinalDocumento))
        formularioAprobarNomina.setValue('monto', estadoDocumentoAprobar.monto)
        formularioAprobarNomina.setValue('periodo', estadoDocumentoAprobar.periodo)
        formularioAprobarNomina.setValue('tipoArchivo', estadoDocumentoAprobar.tipoArchivo)
        formularioAprobarNomina.setValue('autorizadoPor', estadoDocumentoAprobar.autorizadoPor)
        formularioAprobarNomina.setValue('fechaInicialDocumento', UtilService.getFullDateWithTimeDirection(estadoDocumentoAprobar.fechaInicialDocumento))
        formularioAprobarNomina.setValue('observacion', estadoDocumentoAprobar.observacion)
    }

    useEffect(() => {
        if (!open) {
            formularioAprobarNomina.reset();
        }
    }, [open]);

    const handleBuscarComprobante = async () => {

        const datosFormulario = formularioAprobarNomina.getValues();
        if (estado &&
            ((datosFormulario?.numeroRadicado === "" || datosFormulario?.numeroRadicado === null)
                || (datosFormulario?.observacion === "" || datosFormulario?.observacion === null))) {
            await NotificationService.genericAlert(
                'Información',
                `Se debe diligenciar el número de radicado y observación`,
                'warning'
            );
            return;
        }


        try {
            const aprobarIdDocumento = cargueNominaState?.consultaArchivosCargadosN.filter((x: any) => x?.estado === "PENDIENTE POR APROBAR")

            if (aprobarIdDocumento.length !== 0) {
                const documentoId = aprobarIdDocumento[0].idDocumento
                const response = await apiCargueNomina.PostAprobarDocumentoCargueNominaSolicitud(documentoId)
                if (response.isSuccess) {
                    if (estado) {
                        await swal({ text: 'Cargue de nómina procesado', icon: 'success' });

                        handleClose(false)
                    } else if (estadoAprobar) {
                        await swal({ text: 'Cargue de nómina Aprobado', icon: 'success' });

                        handleClose(false)
                    }

                }
            } else {
                const documentoId = cargueNominaState?.consultaArchivosCargadosN[0]?.idDocumento
                const response = await apiCargueNomina.PostAprobarDocumentoCargueNominaSolicitud(documentoId)
                if (response.isSuccess) {
                    if (estado) {
                        await swal({ text: 'Cargue de nómina procesado', icon: 'success' });

                        handleClose(false)
                    } else if (estadoAprobar) {
                        await swal({ text: 'Cargue de nómina Aprobado', icon: 'success' });

                        handleClose(false)
                    }

                }
            }

        } catch (error) {
            toast('Error, al procesar cargue de nómina', { type: toast.TYPE.ERROR })
        }
    }

    const handleActualizarSolicitud = (datos: any) => {


        const request = {
            numeroSolicitud: formularioAprobarNomina.getValues('numeroSolicitud'),
            observacion: formularioAprobarNomina.getValues('observacion'),
            numeroRadicado: formularioAprobarNomina.getValues('numeroRadicado')
        }

        startActualizarSolicitudNomina(request)

    }
    const handleTabChange = (event: any, newValue: any) => {
        setTabIndex(newValue);
    };

    return (
        <>
            <Dialog fullWidth maxWidth="lg" open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
                <IconTitle title='APROBAR' subTitle='CARGUE NÓMINA' icon='fch-Check-1-3' />
                <DialogContent style={{ maxHeight: '80vh', overflowY: 'auto' }}>
                    <Tabs
                        className={classes.fixedTabs}
                        value={tabIndex}
                        onChange={handleTabChange}
                    >
                        <Tab label="Aprobar Carga" />
                        <Tab label="Validar Carga" />
                    </Tabs>
                    <TabPanel value={tabIndex} index={0}>
                        <ContainerTitle title="INFORMACIÓN SOLICITUD" />
                        <Grid container spacing={3}>
                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="numeroSolicitud"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            label="Número  Solicitud"
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="numeroRadicado"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField

                                            label="Número  Radicado*"
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="fechaInicio"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField

                                            label="Fecha Inicio"
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="fechaFin"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField

                                            label="Fecha Fin"
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="monto"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            currencyMask
                                            isReadOnly
                                            label="Monto"
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                        </Grid>

                        {/* <Grid container spacing={3}>

                                <Grid item xs={12} md={12}>
                                    <TextField
                                    fullWidth
                                    label="Campo de texto"
                                    variant="outlined"
                                    inputProps={{
                                        style: {
                                            width: '280px'
                                        }
                                    }} 
                                    />
                                </Grid>

                            </Grid> */}

                        <Grid className={classes.column} item xs={12} md={12}>
                            <Typography variant="h6" className={classes.title}>
                                Observaciones*
                            </Typography>
                            <Controller
                                name="observacion"
                                control={formularioAprobarNomina.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <>
                                        <TextareaAutosize
                                            className={error ? classes.errorTextarea : classes.textarea}
                                            value={value}
                                            onChange={onChange}
                                            style={{ textTransform: 'uppercase' }}
                                        />
                                        {error && <Typography variant="body2" color="error">{error.message}</Typography>}
                                    </>
                                )}
                            />
                        </Grid>
                        <Grid item md={12} className={classes.columnButton} alignItems="flex-start" justifyContent="flex-start" >
                            <Grid item xs={12} md={4}>
                                <Controller
                                    name="numeroComprobante"
                                    control={formularioAprobarNomina.control}
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
                                            label="Número de comprobante"
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
                            </Grid>
                            <Grid item >
                                <Button
                                    className={classes.button}
                                    style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                                    variant="outlined"
                                    color="primary"
                                // onClick={handleActualizarSolicitud} 
                                // onClick={}
                                >
                                    AGREGAR COMPROBANTE
                                </Button>
                            </Grid>
                        </Grid>

                        <Grid container justifyContent="flex-end" spacing={2}>


                            {estadoDocumentoAprobar.observacion === null ? <Grid item>
                                <Button
                                    className={classes.button}
                                    style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                                    variant="outlined"
                                    color="primary"
                                    // onClick={handleActualizarSolicitud} 
                                    onClick={formularioAprobarNomina.handleSubmit(handleActualizarSolicitud)}
                                >
                                    Actualizar
                                </Button>
                            </Grid> : null}
                        </Grid>


                        <ContainerTitle title="CARGAR COMPROBANTE" />


                        <Grid container spacing={3}>
                            <TableContainer component={Paper}>
                                <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                                    <Table aria-label="simple table">
                                        <TableHead>
                                            <TableRow>
                                                <TableCell>Unidad ejecutora</TableCell>
                                                <TableCell>Numero comprobante</TableCell>
                                                <TableCell>Área de Negocio</TableCell>
                                                <TableCell>Vigencia</TableCell>
                                                <TableCell>Clase de Aporte</TableCell>
                                                <TableCell>Tipo de Operación</TableCell>
                                                <TableCell>Producto</TableCell>
                                                <TableCell>Vigencia</TableCell>
                                                <TableCell>Monto</TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {cargueNominaState.consultaArchivosCargadosN.map(
                                                (option: any) => (
                                                    <TableRow key={option.id}>
                                                        <TableCell>{option.idDocumento}</TableCell>
                                                        <TableCell>
                                                            {
                                                                (() => {
                                                                    const unidadEjecutora = catalogState.ExecutingUnit?.find(
                                                                        (item: any) => item.id === option.idUnidadEjecutora
                                                                    );
                                                                    return unidadEjecutora ? unidadEjecutora.nombre : '';
                                                                })()
                                                            }
                                                        </TableCell>
                                                        <TableCell>{UtilService.formatCurrency(option.monto)}</TableCell>
                                                        <TableCell>
                                                            {
                                                                (() => {
                                                                    const tipoArchivo = catalogState.ExecutingUnit?.find(
                                                                        (item: any) => item.id === option.idTipoArchivo
                                                                    );
                                                                    return tipoArchivo ? tipoArchivo.nombre : '';
                                                                })()
                                                            }
                                                        </TableCell>
                                                        <TableCell>{option.documentoNombre}</TableCell>
                                                        <TableCell>{option.periodo}</TableCell>
                                                        <TableCell>{option.fechaCarga}</TableCell>
                                                        <TableCell>{option.estado}</TableCell>
                                                        <TableCell>{option.registros}</TableCell>



                                                        {/* <TableCell>{option.usuario}</TableCell> */}
                                                    </TableRow>
                                                )
                                            )}

                                            <TableRow style={{ borderTop: '1px solid #000' }}>

                                                <TableCell colSpan={7} />


                                                <TableCell align="right" style={{ fontWeight: 400 }}>
                                                    Total
                                                </TableCell>


                                                <TableCell align="right">
                                                    <span style={{ fontWeight: 700 }}>
                                                        $&nbsp;35.192.024
                                                    </span>
                                                </TableCell>
                                            </TableRow>

                                        </TableBody>
                                    </Table>
                                </Grid>
                            </TableContainer>

                        </Grid>


                    </TabPanel>

                    <TabPanel value={tabIndex} index={1}>
                        <ContainerTitle title="INFORMACIÓN DE CARGA" />

                        <Grid container spacing={3}>

                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="numeroSolicitud"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            label="Número Solicitud"
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="periodo"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField

                                            label="Período"
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="tipoArchivo"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField

                                            label="Tipo Archivo"
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                            {/* <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="numeroRadicado"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                    required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        
                                        label="Nuemero Radicado"
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        InputLabelProps={{ shrink: true }}
                                    />
                                    )}
                                />
                                </Grid> */}

                            <Grid item md={2} className={classes.column}>
                                <Controller
                                    name="monto"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField

                                            label="Monto"
                                            currencyMask
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                        </Grid>


                        <MUIDataTable
                            title=""
                            data={scrollableData}
                            columns={columns2}
                            options={{
                                pagination: false,
                                selectableRows: 'none',
                                search: false,
                                filter: false,
                                print: false,
                                download: false,
                                elevation: 0,
                                fixedHeader: false,
                                fixedSelectColumn: false,
                                tableBodyHeight: 'auto',
                                viewColumns: false, // Esto oculta el botón "Mostrar Columnas"
                                textLabels: {
                                    body: {
                                        noMatch: 'No se encontraron resultados',
                                    },
                                },
                            }}
                        />
                        <ContainerTitle title="INFORMACIÓN DE AUTORIZACIÓN" />

                        <Grid container spacing={3}>

                            <Grid item md={6} className={classes.column}>
                                <Controller
                                    name="autorizadoPor"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            label="Usuario"
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>

                            <Grid item md={6} className={classes.column}>
                                <Controller
                                    name="fechaInicialDocumento"
                                    control={formularioAprobarNomina.control}
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField

                                            label="Fecha de Autorización"
                                            isReadOnly
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                        />
                                    )}
                                />
                            </Grid>



                        </Grid>

                        <ContainerTitle title="INFORMACIÓN POR UNIDAD EJECUTORA" />
                        <Grid container spacing={3}>

                            <TableContainer component={Paper}>
                                <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                                    <Table aria-label="simple table">
                                        <TableHead>
                                            <TableRow>
                                                {/* <TableCell>Id Documento</TableCell>
                                                <TableCell>Id Unidad Ejecutora</TableCell>
                                                <TableCell>Monto</TableCell>
                                                <TableCell>Id TipoArchivo</TableCell>
                                                <TableCell>Nombre del Archivo</TableCell>
                                                <TableCell>Período</TableCell>
                                                <TableCell>Fecha de Cargue</TableCell>
                                                <TableCell>Estado</TableCell>
                                                <TableCell>Registros</TableCell>
                                                <TableCell>Usuario</TableCell> */}

                                                <TableCell>Unidad Ejecutora</TableCell>
                                                <TableCell>Total Registros</TableCell>
                                                <TableCell>Total Cargado</TableCell>
                                                <TableCell>Total No Cargado</TableCell>
                                                <TableCell>Monto Total</TableCell>
                                                <TableCell>Monto Cargado</TableCell>
                                                <TableCell>Monto No Cargado</TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {cargueNominaState.consultaArchivosCargadosN.map(
                                                (option: any) => (
                                                    <TableRow key={option.id}>
                                                        <TableCell>
                                                            {
                                                                (() => {
                                                                    const unidadEjecutora = catalogState.ExecutingUnit?.find(
                                                                        (item: any) => item.id === option.idUnidadEjecutora
                                                                    );
                                                                    return unidadEjecutora ? unidadEjecutora.nombre : '';
                                                                })()
                                                            }
                                                        </TableCell>
                                                        <TableCell>{option.idDocumento}</TableCell>
                                                        <TableCell>
                                                            {
                                                                (() => {
                                                                    const tipoArchivo = catalogState.ExecutingUnit?.find(
                                                                        (item: any) => item.id === option.idTipoArchivo
                                                                    );
                                                                    return tipoArchivo ? tipoArchivo.nombre : '';
                                                                })()
                                                            }
                                                        </TableCell>

                                                        <TableCell>{option.documentoNombre}</TableCell>
                                                        <TableCell>{UtilService.formatCurrency(option.monto)}</TableCell>
                                                        <TableCell>{option.periodo}</TableCell>
                                                        <TableCell>{option.fechaCarga}</TableCell>
                                                        {/* <TableCell>{option.estado}</TableCell>
                                                        <TableCell>{option.registros}</TableCell>
                                                        <TableCell>{option.usuario}</TableCell> */}


                                                    </TableRow>
                                                )
                                            )}
                                        </TableBody>
                                    </Table>
                                </Grid>
                            </TableContainer>

                            {/* <MUIDataTable
                                title=""
                                data={[]}
                                columns={[
                                    { name: 'Item', label: 'Unidad Ejecutora' },
                                    { name: 'Registros', label: 'Total Registros' },
                                    { name: 'Monto', label: 'Total Cargados' },
                                    { name: 'Monto', label: 'Total No Cargados' },
                                    { name: 'Monto', label: 'Monto Total' },
                                    { name: 'Monto', label: 'Monto Cargado' },
                                    { name: 'Monto', label: 'Monto No Cargado' }
                                ]}
                                options={{
                                    pagination: false,
                                    selectableRows: 'none',
                                    search: false,
                                    filter: false,
                                    print: false,
                                    download: false,
                                    elevation: 0,
                                    fixedHeader: false,
                                    fixedSelectColumn: false,
                                    tableBodyHeight: 'auto',
                                    viewColumns: false, // Esto oculta el botón "Mostrar Columnas"
                                    textLabels: {
                                        body: {
                                            noMatch: 'No se encontraron resultados',
                                        },
                                    },
                                }}
                            /> */}
                        </Grid>

                    </TabPanel>


                    <Grid item md={12} className={classes.columnButton} alignItems="flex-end" justifyContent="flex-end" >

                        <Button
                            className={classes.button}
                            style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                            variant="outlined"
                            color="primary"
                            onClick={handleClose} >
                            Cancelar
                        </Button>

                        {estado && !estadoAprobar ? <Button
                            className={classes.button}
                            style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                            variant="outlined"
                            color="primary"
                            onClick={handleBuscarComprobante} >
                            Procesar
                        </Button> : <Button
                            className={classes.button}
                            style={{ backgroundColor: '#1C3F8B', color: 'white' }}
                            variant="outlined"
                            color="primary"
                            onClick={handleBuscarComprobante} >
                            Aprobar
                        </Button>}


                    </Grid>

                </DialogContent>
            </Dialog>
        </>
    )
}
interface LinkMapStateToProps {
    catalogState: CatalogState;
    cargueNominaState: CargueNominaState;
}
interface LinkMapDispatchProps {
    startActualizarSolicitudNomina: (request: any[],) => void;
}

const mapDispatchToProps = {
    startActualizarSolicitudNomina,
};


const mapStateToProps = (state: AppState) => ({
    cargueNominaState: state.cargueNomina,
    catalogState: state.catalog,
});

type Props = LinkMapStateToProps & LinkMapDispatchProps;


// export default ModalAprobarNomina;
export default connect(mapStateToProps, mapDispatchToProps)(ModalAprobarNomina);




