import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import moment from 'moment';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import CreditApi from '../../../services/api-credit.service';
import TableMassivePayAplicationList from './TableMassivePayAplicationList';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import TablaListasAprobadas from '../TablaListasAprobadas';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import { UnidadEjecutoraDto } from '../../../types/simulador.model.type';
import { AppState } from '../../../store/rootReducer';
import { SimuladorState } from '../../../reducers/simulador.reducer';
import { startGetAllUnidadEjecutora } from '../../../actions/simulador.action';
import AuditoriaApi from '../../../services/api-auditorias.servise';
import AuthService from '../../../services/auth.service';
import { Auditoria } from '../../../types/Auditoria.model.type';

import CatalogApi from '../../../services/api-catalog.service';
import CarteraApi from '../../../services/api-cartera-service';


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%'
        },
        buttonCompare: {
            // width: '100%',
        },
        groupButtonLoader: {
            display: 'flex',
            gap: '1.5rem',
            alignItems: 'center',
            padding: '1rem 0',
            flexWrap: 'wrap'
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr 3fr',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        }
    }),
);


const ListasAprobadas = (props: any) => {

    const {
        simuladorState,
        startGetAllUnidadEjecutora
    } = props;

    const creditApi = CreditApi();
    const carteraApi = CarteraApi();


    const [listados, setListados] = useState<any[]>([]);
    const [listaSeleccionada, setListaSeleccionada] = useState<any>(null);
    const [loading, setLoading] = useState(false);
    const [selectedRows, setSelectedRows] = useState<any[] | undefined>([]);
    const [procesandoEliminacionLista, setProcesandoEliminacionLista] = useState<boolean>(false);

    const classes = useStyles();

    const formularioConsulta = useForm({
        defaultValues: {
            fechaCreacion: '',
            unidadEjecutora: '',
            estado: 'APROBADA'
        },
    });
    useEffect(
        () => {
            startGetAllUnidadEjecutora();
        }, []
    );

    const [publicIpAddress, setPublicIpAddress] = useState<string | null>(null);
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
                        enviarIp(ip);
                    } else {
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

    const handleSelectedRow = (__: any, _: any, rowsSelected: any) => {
        setSelectedRows(rowsSelected);
        const index = rowsSelected[0];

        if (index !== undefined) {
            setListaSeleccionada(listados[index]);

            const l = listados[index];
            // const l=listaSeleccionada;
            const auditoriaApi = AuditoriaApi();
            const user = AuthService.getUserInfo();
            const nav = (window.navigator as any);
            const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Generar listas',
                hostname: document.location.host
            } as Auditoria;
            requestAuditoria.tipoAccion = 'CONSULTAS';
            requestAuditoria.idTipoAccion = 3;
            requestAuditoria.descripcion = `Buscar pagos listas aprobadas parámetros:           
                            \n Periodo=${l.periodo}
                            \n Tipo Crédito=${l.tipoCredito}
                            \n Unidad ejecutora=${l.unidadEjecutora}`
            auditoriaApi.InsertAuditoria(requestAuditoria);
        } else {
            setListaSeleccionada(null);
        }

    }



    const cargarListasPendientes = (datos: any) => {
        setListaSeleccionada(null);
        setLoading(true);
        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = (window.navigator as any);
        const usrAg = nav.userAgentData.brands.filter((x: { brand: string; }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge')[0].brand;
        const requestAuditoria = {
            navegador: usrAg,
            sistemaOperativo: nav.userAgentData.platform,
            nombreUsuario: user.USR_USERNAME,
            usurioId: user.USR_ID,
            ip: publicIpAddress,
            modulo: 'Generar listas',
            hostname: document.location.host
        } as Auditoria;
        creditApi.obtenerProcesoAplicacionPagos(datos)
            .then(x => {
                if (x.isSuccess) {
                    setListados(x.data ?? []);
                    requestAuditoria.tipoAccion = 'CONSULTAS';
                    requestAuditoria.idTipoAccion = 3;
                    requestAuditoria.descripcion = `Buscar listas aprobadas parámetros:           
                \n Unidad Ejecutora:  ${simuladorState?.simuladorunidadEjecutora?.find((x: { idUnidadEjecutora: any; }) => x.idUnidadEjecutora === datos.unidadEjecutora)?.nombreUnidadEjecutora || 'Unidad Ejecutora no disponible'}
                \n Estado:  ${datos.estado}`
                    auditoriaApi.InsertAuditoria(requestAuditoria);
                }
            })
            .catch((error: any) => {
                setListados([])
                requestAuditoria.tipoAccion = 'ERRORES Y EXCEPCIONES';
                requestAuditoria.idTipoAccion = 8;
                requestAuditoria.descripcion = `Buscar listas aprobadas parámetros:           
                \n Unidad Ejecutora:  ${simuladorState?.simuladorunidadEjecutora?.find((x: { idUnidadEjecutora: any; }) => x.idUnidadEjecutora === datos.unidadEjecutora).nombreUnidadEjecutora}
                
                \n Estado:  ${datos.estado}
            \n Error:  ${error}
            \n returnApiMessage: ${error?.response?.data?.returnMessage}`
                auditoriaApi.InsertAuditoria(requestAuditoria);
            })
            .finally(() => setLoading(false));
    }


    const handleEliminarProcesoAplicacionPago = async (idProceso: number) => {

        if (idProceso) {
            const result = await swal({
                text: '¿Está seguro que desea eliminar la lista?', buttons: {
                    confirm: { text: "Sí", value: true, visible: true },
                    cancel: { text: "No", value: false, visible: true }
                }, icon: 'warning'
            });

            if (result) {
                setProcesandoEliminacionLista(true);
                carteraApi.postReversarPagosLista(idProceso)
                    .then(x => {
                        if (x.isSuccess) {
                            if (idProceso === listaSeleccionada?.idProceso) {
                                setListaSeleccionada(null);
                                setSelectedRows([]);
                            }
                            setListados(listados.filter((x: any) => x.idProceso !== idProceso));
                        } else {
                            toast(x?.returnMessage ?? "Ha ocurrido un error al eliminar la lista", { type: toast.TYPE.ERROR });
                        }
                    })
                    .catch(_ => {
                        toast('Error del sistema', { type: toast.TYPE.ERROR });
                    })
                    .finally(() => setProcesandoEliminacionLista(false));
            }
        }
    }

    const onDeletePago = async (rowData: any) => {

        try {
            // 1. Mostrar el Swal de confirmación primero
            const result = await swal({
                text: '¿Está seguro que desea reversar el pago?',
                buttons: {
                    confirm: { text: "Sí", value: true, visible: true },
                    cancel: { text: "No", value: false, visible: true }
                },
                icon: 'warning'
            });

            // 2. Si el usuario confirma
            if (result) {
                setLoading(true);

                // 3. Ejecutar reversar pago
                const response = await carteraApi.PostReversarPagoUnitarioDeLista({
                    cpgId: rowData
                });

                if (response?.isSuccess && response.data) {
                    const formData = formularioConsulta.getValues();
                    await cargarListasPendientes(formData);
                    toast('Pago reversado correctamente.', { type: toast.TYPE.SUCCESS, autoClose: 5000 });
                } else {
                    toast('No se encontraron resultados para el comprobante.', { type: toast.TYPE.WARNING, autoClose: 5000 });
                }
            } else {
                toast('Operación cancelada.', { type: toast.TYPE.INFO, autoClose: 4000 });
            }

        } catch (error) {
            toast('Error del sistema. Por favor, intenta más tarde.', { type: toast.TYPE.ERROR });
        } finally {
            setLoading(false);
        }
    };


    // const formatTotal = (value?: number) => {
    //     const formatter = new Intl.NumberFormat('en-EN', {
    //         style: 'currency',
    //         currency: 'USD',
    //         minimumFractionDigits: 0,
    //         maximumFractionDigits: 6,
    //     });

    //     return formatter.format(value ?? 0);
    // }

    if (loading) {
        return <CHLoadingProcess />;
    }

    const listInfo = {
        periodo: listaSeleccionada?.periodo ?? '',
        unidadEjecutora: listaSeleccionada?.unidadEjecutora ?? '',
        tipoCredito: listaSeleccionada?.tipoCredito ?? ''
    };

    return (<>

        <Grid container>

            <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="fechaCreacion"
                    control={formularioConsulta.control}
                    render={({
                        field: { onChange, value, onBlur },
                        fieldState: { error },
                    }) => (
                        <CHDatePicker
                            InputLabelProps={{ shrink: true }}
                            label="Periodo*"
                            // onChange={(date, value) => {
                            //     const lastDay = moment(date).endOf('month').startOf('day').toDate();
                            //     onChange(lastDay);
                            // }}                            
                            value={value}
                            maxDate={new Date()}
                            maxDateMessage="La fecha debe encontrarce en el periodo actual"
                            invalidDateMessage="Fecha inválida"
                            views={['year', 'month']}
                            onBlur={onBlur}
                            onChange={onChange}
                            format="yyyy/MM"
                            placeholder='YYYY/MM'
                        />
                    )}
                />
            </Grid>


            <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="unidadEjecutora"
                    control={formularioConsulta.control}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHSelect
                            label="Unidad Ejecutora *"
                            value={value}
                            onChange={onChange}
                        >
                            {
                                simuladorState.simuladorunidadEjecutora?.map(
                                    (option: UnidadEjecutoraDto) => (
                                        <MenuItem
                                            key={option.idUnidadEjecutora}
                                            value={option.idUnidadEjecutora}
                                        >
                                            {option.nombreUnidadEjecutora}
                                        </MenuItem>
                                    ))
                            }
                        </CHSelect>
                    )}
                />
            </Grid>

            <Grid className={classes.column} item xs={12} md={3}>
                <Button

                    variant="contained"
                    className={classes.button}
                    color="primary"
                    component="label"
                    onClick={formularioConsulta.handleSubmit(cargarListasPendientes)}

                >
                    Buscar Lista
                </Button>
            </Grid>

            <Grid item xs={12}>
                <TablaListasAprobadas
                    listState={listados}
                    rowsSelected={selectedRows}
                    onRowSelectionChange={handleSelectedRow}
                    onDeleteProceso={handleEliminarProcesoAplicacionPago}
                    selectableRows='single'
                />
            </Grid>
        </Grid>
        <br />
        {
            ((listaSeleccionada?.pagos?.length ?? 0) > 0) &&
            <Grid container>
                <Grid item xs={12}>
                    <TableMassivePayAplicationList
                        listState={listaSeleccionada?.pagos ?? []}
                        listInfo={listInfo} selectableRows='none'
                        onDeletePago={onDeletePago}
                    />
                </Grid>
            </Grid>
        }

    </>);
}

interface LinkMapStateToProps {
    simuladorState: SimuladorState;
}

const mapStateToProps = (state: AppState) => ({
    simuladorState: state.simulador,
    parametrizationState: state.parametrization,
});

interface LinkMapDispatchToProps {
    startGetAllUnidadEjecutora: () => void;
}

const mapDispatchToProps = {
    startGetAllUnidadEjecutora,

};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

// export default ListasAprobadas;
export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ListasAprobadas);