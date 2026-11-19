import React, { useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    MenuItem,
    FormControlLabel,
    Checkbox,
    TextField,
    Modal,
    Fade,
    Box
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { getDate } from 'date-fns';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import { connect } from 'react-redux';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import {
    TipoEmbargoDto,
    TipoRetencionDto,
} from '../../../types/embargo.model.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import UtilService from '../../../utils/util.service';
import history from '../../../utils/history.router'
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import ApiEmbargo from '../../../services/api-embargos.service';
import { startGetAllTypeDocuments } from '../../../actions/catalog.action';



const useStyles = makeStyles((theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        alineationButtons: {
            display: 'flex',
            justifyContent: 'center',
            marginTop: theme.spacing(2),

        },
        modal: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            backgroundColor: 'rgba(0, 0, 0, 0.5)',
            zIndex: 9999,
            '& .MuiPaper-root': {
                margin: 'auto',
                outline: 'none',
                borderRadius: 8,
                width: '50%',
                height: '50%',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                [theme.breakpoints.down('sm')]: {
                    width: '80%',
                    height: '80%',
                },
            },
        },
        paper: {
            backgroundColor: theme.palette.background.paper,
            border: '2px solid #000',
            boxShadow: theme.shadows[5],
            padding: theme.spacing(5, 3, 2),
            width: '70%',
            height: '90%',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            borderRadius: '3%',
        },
        header: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            marginBottom: '1rem',
        },
        title: {
            color: '#1C3F8B',
            textAlign: 'center',
            fontSize: '0.9rem',
            fontWeight: 'bold',
        },
        actions: {
            marginLeft: '2rem',
            width: '20%',
        },
        customCheckbox: {
            color: 'gray',
            '&$checked': {
                color: 'gray',
            },
        },
        alineationHorizontal: {
            display: 'flex',
            justifyContent: "center"
        },
        espaciado: {
            paddingInline: '70px',
            paddingBottom: '10px'
        },
        encabezados: {
            textAlign: 'center',
            position: 'sticky',
            top: 0,
            background: '#FEC917',
            zIndex: 1
        }
    }),
);

const GenerarArchivo = (props: any) => {
    const {
        open,
        handleClose,
        embargo,
    } = props;
    const [data, setData] = useState<any>([]);
    const classes = useStyles();
    const API = ApiEmbargo();



    const ObtenerPagoEmbargo = async (fecha: Date) => {
        try {
            const resp = await API.ObtenerPagoEmbargo(fecha);


            if (!resp?.isSuccess) {
                setData([]);
                toast(resp?.returnMessage ?? 'Error consultando pagos');
                return;
            }

            if (!resp?.data) {
                setData([]);
                toast('No se encontraron pagos de embargo para la fecha consultada');
                return;
            }

            setData(Array.isArray(resp.data) ? resp.data : [resp.data]);
        } catch (e) {
            setData([]);
            toast('Error consultando pagos');
        }
    };


    useEffect(() => {
        const Fecha = new Date();
        ObtenerPagoEmbargo(Fecha);
        startGetAllTypeDocuments();
    }, []);

    const handleCvs = () => {
        console.log("Descargar archivo cvs");

    };

    const handleTxt = () => {
        console.log("Descargar archivo txt");

    };

    const handleAnular = () => {
        console.log("proceso anulado");

    };

    return (
        <Modal
            className={classes.modal}
            open={open}
            onClose={() => handleClose()}
        >
            <Fade in={open}>
                <div className={classes.paper}>
                    <Box mb={0.2}>
                        <Typography variant="h6" align="center">GENERAR ARCHIVO</Typography>
                    </Box>
                    <Grid container alignItems="center" justify="center">
                        <Grid item xs={12}>
                            <div style={{ maxHeight: '70vh', overflow: 'auto', width: '100%' }}>
                                <table className="client-table" style={{ width: '100%' }}>
                                    <thead>
                                        <tr>
                                            <th className={classes.encabezados}>FECHA</th>
                                            <th className={classes.encabezados}>No. PLANILLA</th>
                                            <th className={classes.encabezados}>TIPO DE EMBARGO</th>
                                            <th className={classes.encabezados}>FUNCIONARIO</th>
                                            <th className={classes.encabezados}>RADICADO</th>
                                            <th className={classes.encabezados}>CODIGO JUZGADO</th>
                                            <th className={classes.encabezados}>NOMBRE JUZGADO</th>
                                            <th className={classes.encabezados}>CONCEPTO</th>
                                            <th className={classes.encabezados}>No. PROCESO</th>
                                            <th className={classes.encabezados}>TIPO ID DEMANDANTE</th>
                                            <th className={classes.encabezados}>IDENTIFICACION DEMANDANTE</th>
                                            <th className={classes.encabezados}>RAZON SOCIAL/NOMBRE DEMANDANTE</th>
                                            <th className={classes.encabezados}>TIPO ID DEMANDANDO</th>
                                            <th className={classes.encabezados}>IDENTIFICACION DEMANDANDO</th>
                                            <th className={classes.encabezados}>RAZON SOCIAL/NOMBRE DEMANDANDO</th>
                                            <th className={classes.encabezados}>VALOR A PAGAR</th>
                                            <th className={classes.encabezados}>FOLIOS</th>
                                            <th className={classes.encabezados}>ACCIONES</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        {data.length > 0 ? (
                                            data.map((item: any) => (
                                                <tr key={item.idEmargo ?? item.id ?? item.tpE_ID}>
                                                    <td>{item.fecha}</td>
                                                    <td>{0}</td>
                                                    <td>{item.tipoEmbargo}</td>
                                                    <td>{item.ebA_CREADOPOR_NOMBRE} {item.ebA_APELLIDO_CREADOPOR}</td>
                                                    <td>{item.ebA_RADICADO_WORK_MANAGER}</td>
                                                    <td>{item.codigO_INTERNO}</td>
                                                    <td>{item.nombreRazonSocial}</td>
                                                    <td>{item.tpE_NOMBRE}</td>
                                                    <td>{item.codigO_INTERNO}{item.ebA_EXPEDIENTE_BANCO_AGRARIO}</td>
                                                    <td>{item.clienteTipoIdentificacion}</td>
                                                    <td>{item.clienteIdentificacion}</td>
                                                    <td>{item.nombre}</td>
                                                    <td>{0}</td>
                                                    <td>{item.ebA_CLI_IDENTIFICACION}</td>
                                                    <td>{item.ebA_NOMBRE_DEMANDADO}</td>
                                                    <td>{item.valor}</td>
                                                    <td>{0}</td>
                                                    <td>
                                                        <Button variant="outlined" color="primary" onClick={handleAnular}>
                                                            ANULAR
                                                        </Button>
                                                    </td>
                                                </tr>
                                            ))
                                        ) : (
                                            <tr>
                                                <td colSpan={18}>No data available</td>
                                            </tr>
                                        )}
                                    </tbody>

                                </table>
                            </div>
                        </Grid>
                        <Grid item xs={12} className={classes.alineationHorizontal}>
                            <div className={classes.espaciado}>
                                <Button
                                    variant="outlined"
                                    color="primary"
                                    onClick={() => handleCvs()}
                                >
                                    Descargar cvs
                                </Button>
                            </div>
                            <div className={classes.espaciado}>
                                <Button
                                    variant="outlined"
                                    color="primary"
                                    onClick={() => handleTxt()}
                                >
                                    Descargar txt
                                </Button>
                            </div>
                            <div className={classes.espaciado}>
                                <Button
                                    variant="outlined"
                                    color="primary"
                                    onClick={() => handleClose()}
                                >
                                    Cerrar
                                </Button>
                            </div>
                        </Grid>

                    </Grid>
                </div>
            </Fade>
        </Modal >

    );
}

export default GenerarArchivo;