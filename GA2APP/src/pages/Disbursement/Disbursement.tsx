import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { connect } from 'react-redux';
import { useForm, Controller } from 'react-hook-form';
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    Grid,
    MenuItem,
} from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import MUIDataTable from 'mui-datatables';
import moment from 'moment';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../components/ch-paper/CHPaper';
import NotificationService from '../../services/notification.service';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHSelect from '../../components/ch-select/CHSelect';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import { CreateDesembolsoModel } from '../../models/requests/credit-desembolso-model';
import {
    startPostCreateDesembolsoRequestSuccess,
    ClearDesembolsoData,
    startDesembolsoDesestimiento,
    starPostDesistirDesembolso
} from '../../actions/credit.actions';
import { AppState } from '../../store/rootReducer';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CreditApi from '../../services/api-credit.service';
import UtilService from '../../utils/util.service';
import CatalogApi from '../../services/api-catalog.service';
import AuthService from '../../services/auth.service';
import AuditoriaApi from '../../services/api-auditorias.servise'
import { Auditoria } from '../../types/Auditoria.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
            margin: '8px 0',
        },
        dialoTamaño: {
            width: '100%',
        },
        columnDate: {
            minWidth: '110px',
        },


    }),
);



function GrillaDatos({
    startDesembolso,
    desembolsoData,
    causalDesistimiento,
    starDesistirDesembolso,
    ocultarBtnAcciones,
    ocultarBtnAccionesDesistimiento,
    obtenerDatosCliente

}: {
    startDesembolso: (request: any) => void;
    desembolsoData: any[];
    causalDesistimiento: any;
    starDesistirDesembolso: (request: any) => void;
    ocultarBtnAcciones: any;
    ocultarBtnAccionesDesistimiento: any;
    obtenerDatosCliente: any;


}) {
    const api = CreditApi();
    const [open, setOpen] = React.useState(false);
    const [selectedData, setSelectedData] = React.useState<any | null>(null);
    const [mosotrarFechaAnulacion, setMosotrarFechaAnulacion] = React.useState(false);
    const [datoDesistiemiento, setDatoDesistimiento] = React.useState<any>('');




    const { control, handleSubmit, setValue } = useForm({
        defaultValues: {
            observacionAnulacion: '',
            idCdd: '',
            idSolicitudDesembolso: '',
        },
    });

    useEffect(() => {
        fechaAnuladoDesembolso()

    }, [desembolsoData]);
    useEffect(() => {

        startDesembolso(obtenerDatosCliente)
    }, []);

    const fechaAnuladoDesembolso = () => {

        const fechaAnulacion = desembolsoData.find(item => item.anulado === 'S');
        if (fechaAnulacion) {
            setMosotrarFechaAnulacion(true)

        } else {
            setMosotrarFechaAnulacion(false)
        }

    }

    const handleClickOpen = async (data: any) => {
        console.log("data________________________", data)

        if (data.anulado === "S") {
            await NotificationService.genericAlert(
                'Operacion Cancela',
                'Desembolso esta anulado.',
                'warning'
            );
            return;

        }
        setOpen(true);
        setDatoDesistimiento(data)
    };


    const onSubmit = async (data: any) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro?',
        );

        if (confirmAlert) {
            const request = {
                observacionAnulacion: data.observacionAnulacion,
                idCdd: data.idCdd,
                IdSolicitudDesistimiento: datoDesistiemiento?.idSolicitudDesembolso,

            };
            starDesistirDesembolso(request);
            setOpen(false);
            startDesembolso(obtenerDatosCliente);

            setValue('observacionAnulacion', '');


        }

    }


    const handleApprove = (datoItem: any) => async () => {

        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro ?',
        );

        if (confirmAlert) {
            const dato = desembolsoData.find(item => item.idDesembolso === datoItem.idDesembolso);
            const datos: any = {
                solicitudDesembolsoId: dato.idSolicitudDesembolso,
                aplicadoPor: dato.aplicadoPor,
                fechaAplicacion: moment(new Date()).format('YYYY-MM-DD'),
                solictudId: dato.idSolicitudCredito,
            }

            await api
                .patchDesembolso(datos)
                .then(async (response) => {
                    if (response) {
                        if (response.isSuccess) {
                            toast('Se aprobó el Desembolso correctamente.', { type: toast.TYPE.SUCCESS });
                            setSelectedData(response.data);
                            startDesembolso(obtenerDatosCliente);

                        } else {
                            toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.ERROR });
                        }
                    }
                }).catch(() => {
                    toast("Error del sistema", { type: toast.TYPE.ERROR });
                });

        }
    };




    const handleClose = () => {
        setOpen(false);
    };


    const columns = [
        {
            name: 'numeroSolicitud',
            label: 'Número de Solicitud',

        },
        {
            name: 'tipoCredito',
            label: 'Tipo de Crédito',
        },
        {
            name: 'documentoCliente',
            label: 'Documento',
        },
        {
            name: 'nombreCliente',
            label: 'Nombre',
        },
        {
            name: 'fechaDesembolso',
            label: 'Fecha Desembolso',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },
        {
            name: 'valorDesembolso',
            label: 'Valor Desembolso',
            options: {
                customBodyRender: (value: number | bigint) =>
                    UtilService.formatCurrency(value),
            },
        },
        {
            name: 'aplicado',
            label: 'Aplicado',
        },
        {
            name: 'fechaAplicacion',
            label: 'Fecha de Aplicación',
            options: {
                setCellProps: () => ({
                    className: classes.columnDate,
                }),
                customBodyRender: (value: string) => UtilService.getDate(value),
            },
        },
        {
            name: 'anulado',
            label: 'Anulado',
        },
        mosotrarFechaAnulacion ?
            {

                name: 'fechaAnulacion',
                label: 'Fecha de Anulación',
                options: {
                    setCellProps: () => ({
                        className: classes.columnDate,
                    }),
                    customBodyRender: (value: string) => UtilService.getDate(value),
                },
            } : '',

        !ocultarBtnAccionesDesistimiento ?

            {

                name: 'aciones',
                label: 'Acciones ',
                options: {
                    customBodyRender: () =>
                        ocultarBtnAcciones && <Button style={{
                            marginRight: '1rem',
                        }}
                            onClick={handleApprove(desembolsoData.map(item => item))}
                            variant="contained"
                            color="primary">
                            Aprobar
                        </Button>,
                },

            } : '',

        ocultarBtnAccionesDesistimiento ?


            {

                name: 'aciones',
                label: 'Acciones',
                options: {
                    customBodyRender: (value: any, tableMeta: any, rowData: any) => (



                        <Button

                            style={{
                                marginRight: '1rem',
                            }}


                            onClick={() => handleClickOpen(desembolsoData[tableMeta.rowIndex],)}
                            variant="contained"
                            color="primary"
                        >




                            Desistimiento
                        </Button>
                    ),
                },

            } : '',

    ];

    const classes = useStyles();

    if (desembolsoData.length > 0) {
        return (
            <CHPaper title="">
                <MUIDataTable
                    columns={columns}
                    data={desembolsoData}
                    title=""
                    options={{
                        responsive: 'vertical',
                        filter: false,
                        selectableRows: 'none',
                        elevation: 0,

                        textLabels: {
                            pagination: {
                                displayRows: 'de',
                                next: 'Siguiente',
                                previous: 'Anterior',
                                rowsPerPage: 'Filas por página',
                                jumpToPage: '',
                            },
                            toolbar: {
                                downloadCsv: 'Descargar CSV',
                                search: 'Buscar',
                                viewColumns: 'Mostrar Columnas',
                            },

                        },
                    }}
                />

                <div>

                    <Dialog open={open} onClose={handleClose}>
                        <DialogTitle  >DESISTIMIENTO</DialogTitle>
                        <DialogContent  >
                            <DialogContentText>
                                Campo texto con la observación para el desistimiento.
                                Campo obligatorio para poder realizar el desistimiento.
                            </DialogContentText>
                            <Grid className={classes.column} item xs={12} md={12} >
                                <Controller
                                    name="idCdd"
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
                                            label="Motivo de desistimiento *"
                                            value={value}
                                            helperText={error ? error.message : null}
                                            onChange={onChange}
                                        >
                                            {causalDesistimiento?.map((data: any) => (
                                                <MenuItem

                                                    key={data.causalDesistimientoId}
                                                    value={data.causalDesistimientoId}
                                                >
                                                    {data.descripcion}
                                                </MenuItem>
                                            ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <Controller
                                    name="observacionAnulacion"
                                    control={control}

                                    rules={{
                                        required:
                                            'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            label="Observación *"
                                            value={value}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            </Grid>
                        </DialogContent>
                        <DialogActions>
                            <Button onClick={handleClose}>Cancelar</Button>
                            <Button onClick={handleSubmit(onSubmit)}>Enviar</Button>
                        </DialogActions>
                    </Dialog>
                </div>
            </CHPaper>

        );
    }
    return (
        <></>


    )


}

function RequestInformationLegalizationData(
    {
        startDesembolso,
        desembolsoData,
        isLoading,
        clearData,
        causalDesistimiento,
        startcausalDesistimiento,
        starDesistirDesembolso,

    }: {
        startDesembolso: (request: any) => void;
        desembolsoData: any;
        isLoading: boolean;
        clearData: () => void;
        causalDesistimiento: any;
        startcausalDesistimiento: () => void;
        starDesistirDesembolso: (request: any) => void;


    }): any {
    const classes = useStyles();



    useEffect(() => {
        clearData();


    }, []);

    useEffect(() => {
        startcausalDesistimiento();
    }, [desembolsoData]);

    const { control, handleSubmit } = useForm<CreateDesembolsoModel>({
        defaultValues: {
            fechaDesembolso: new Date(),
            documentoCliente: '0',
            aplicado: 'n',
        },
    });

    const [ocultarBtnAcciones, setOcultarBtnAcciones] = React.useState<boolean | null>(true);
    const [ocultarBtnAccionesDesistimiento, setOcultarBtnAccionesDesistimiento] = React.useState<boolean | null>(false);
    const [obtenerDatosCliente, setObtenerDatosCliente] = React.useState<any>({});

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

    const onSubmit = async (datosDesembolso: any) => {

        if (datosDesembolso.aplicado === 'S') {
            setOcultarBtnAcciones(false);
            setOcultarBtnAccionesDesistimiento(true)
        } else if (datosDesembolso.aplicado === 'N') {
            setOcultarBtnAccionesDesistimiento(false)
            setOcultarBtnAcciones(true);
        }

        const request = {
            fechaDesembolso: moment(`${datosDesembolso.fechaDesembolso}`).format('YYYY-MM-DD'),
            documentoCliente: `${datosDesembolso.documentoCliente}`,
            aplicado: `${datosDesembolso.aplicado}`,
        }

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        let descripcionAplicado = 'No especificado';

        if (request.aplicado === 'S') {
            descripcionAplicado = 'Si'
        } else if (request.aplicado === 'N') {
            descripcionAplicado = 'No'
        }

        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Desembolsos',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de desembolsos con los siguientes filtros:
                      - Documento Cliente: ${request.documentoCliente || 'No especificada'}
                      - Fecha de Desembolso: ${request.fechaDesembolso || 'No especificado'}
                      - Aplicado: ${descripcionAplicado || 'No especificada'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);


            startDesembolso(request);
            setObtenerDatosCliente(request)
        } catch (error: any) {
            const descripcionError = `Error al consultar desembolsos.
            - Error: ${error?.message || 'Error desconocido'}
            - Mensaje API: ${error?.response?.data?.returnMessage || 'No disponible'}
            - Código HTTP: ${error?.response?.status || 'No disponible'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Desembolsos',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);
            toast('Error al consultar desembolsos', { type: toast.TYPE.ERROR });
        }
    };


    if (isLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <CHPrivateContainer>

            <CHPageTitle
                topText="Credito"
                subText=""
                title1="Desembolsos"
                title2=""
            />
            <CHPaper title="">
                <Grid container alignItems="center">
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="documentoCliente"
                            control={control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },

                            }}

                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Documento Cliente"
                                    onChange={onChange}
                                    value={value}

                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="fechaDesembolso"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha de Desembolso"
                                    onChange={onChange}
                                    value={value}

                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={4} className={classes.column}>
                        <Controller
                            name="aplicado"
                            control={control}
                            render={({ field: { onChange, value } }) => (
                                <CHSelect
                                    label="Aplicado"
                                    onChange={onChange}
                                    value={value}
                                >
                                    <MenuItem value={0}>Seleccione</MenuItem>
                                    <MenuItem value="S">Si</MenuItem>
                                    <MenuItem value="N">No</MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={2} className={classes.column}>
                        <Button
                            onClick={handleSubmit(onSubmit)}
                            className={classes.button}
                            variant="outlined"
                        >

                            Consultar
                        </Button>
                    </Grid>


                </Grid>
            </CHPaper>
            <GrillaDatos
                startDesembolso={startDesembolso}
                desembolsoData={desembolsoData}
                causalDesistimiento={causalDesistimiento}
                starDesistirDesembolso={starDesistirDesembolso}
                ocultarBtnAcciones={ocultarBtnAcciones}
                ocultarBtnAccionesDesistimiento={ocultarBtnAccionesDesistimiento}
                obtenerDatosCliente={obtenerDatosCliente}


            />
        </CHPrivateContainer>
    );
}



const mapStateToProps = (state: AppState) => ({
    desembolsoData: state.credit.desembolsoData,
    desembolsoAprovado: state.credit.desembolsoAprovado,
    causalDesistimiento: state.credit.causalDesistimiento,
    desistirDesembolso: state.credit.desistirDesembolso

});

const mapDispatchToProps = (dispatch: any) => ({
    clearData: () =>
        dispatch(ClearDesembolsoData()),
    startDesembolso: (request: any) =>
        dispatch(startPostCreateDesembolsoRequestSuccess(request),
        ),
    startcausalDesistimiento: () =>
        dispatch(startDesembolsoDesestimiento()),

    starDesistirDesembolso: (request: any) =>
        dispatch(starPostDesistirDesembolso(request)),

});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)
    (RequestInformationLegalizationData);
