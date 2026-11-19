import { Button, createStyles, Grid, IconButton, makeStyles, Table, TableBody, TableCell, TableHead, TableRow } from "@material-ui/core";
import { Edit } from "@material-ui/icons";
import { format } from "date-fns";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import CHCheckbox from "../../components/ch-checkbox/CHCheckbox";
import CHDatePicker from "../../components/ch-datepicker/CHDatepicker";
import CHPaper from "../../components/ch-paper/CHPaper";
import CHPrivateContainer from "../../components/ch-private-container/CHPrivateContainer";
import CHTextField from "../../components/ch-text-field/CHTextField";
import CreditApi from "../../services/api-credit.service";
import Prospeccion from "../../services/api-prospeccion.service";
import NotificationService from "../../services/notification.service";
import UtilService from "../../utils/util.service";
import ModalEditarParametrizacionLeasing from "./ModalEditarParametrizacionLeasing";
import AuditoriaApi from '../../services/api-auditorias.servise'
import CatalogApi from "../../services/api-catalog.service";
import AuthService from "../../services/auth.service";
import { Auditoria } from "../../types/Auditoria.model.type";

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '20px',

            alignItems: 'flex-end',

            display: 'flex',
            minWidth: '110px',
        },

        cenButtonUno: {
            width: '30%',
            // marginLeft: '10px',
            // MozMarginEnd: '10px',
            // color: '#1706EA ',
            marginTop: '40px',
            marginBottom: '40px',

        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        buttonAdd: {
            marginTop: '7px',
            width: '200px',
            marginLeft: '.5rem',
            marginBottom: '2rem',

        },


    }),
);

const ParametrizacionLeasing = () => {

    const classes = useStyles();
    const apiProspeccion = Prospeccion()

    const [valoresCategoria, setValoresCategoria] = useState<any[]>([]);
    const [openModalEditar, setModalEditar] = useState(false);
    const [desactivarSalario, setDesactivarSalario] = useState(false);


    const { control, handleSubmit, setValue, getValues, watch } = useForm({
        defaultValues: {

            porcentajeInflacion: 0,
            crg_Id: 0,
            crg_Descripcion: '',
            ctg_Id: 0,
            crg_Factor_Subs: 0,
            crg_Anios_Ascenso: 0,
            cra_Salario_Basico: 0,
            cra_Salario_Total: 0,
            cra_Anio: new Date('2024/02/01'),
            crg_ProyectaSM: false,
            crg_ProyectaSubsidio: false

        },
    });

    const formularioDesactivarSalario = useForm({
        defaultValues: {
            desactivarSalario: false
        }
    })



    useEffect(() => {
        ObtenerValCategoria()
        // ObtenerAnioSalaria()
    }, []);


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

    const fechaConsulta = watch('cra_Anio');
    const procentajeConsulta = watch('porcentajeInflacion');

    const ObtenerValCategoria: any = async () => {

        let year = format(getValues('cra_Anio'), 'yyyy');
        if (year === '1969') {
            year = String(getValues('cra_Anio'))
        }

        const request = {
            anio: Number(year),
        }

        const firstDayOfYear = new Date(request.anio, 0, 1);
        setValue('cra_Anio', firstDayOfYear);

        await apiProspeccion
            .postObtenerValoresCategoria(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setValoresCategoria(response.data)
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

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
                modulo: 'Parametrización Salarial',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta de Datos parametricos de salario con los siguientes filtros:
                - Fecha: ${fechaConsulta || 'No especificada'}
                - Valor IPC anual %: ${procentajeConsulta || 'No especificado'}`

            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);


    };

    const crearValorCategoria = async (data: any) => {

        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro?',
        );
        if (confirmAlert) {

            const year = format(data.cra_Anio, 'yyyy');
            const request = {
                "cra_Anio": Number(year),
                "porc_Aumento": Number(data.porcentajeInflacion)
            }
            await apiProspeccion
                .postCrearValoresCategoria(request)
                .then(async (response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                            setValoresCategoria(response.data)
                            toast('Creacion de registro exitosa', { type: toast.TYPE.SUCCESS });
                        }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });

        }
    }

    const handleOpenModalEditar = (rowData: any) => {

        setValue('crg_Id', rowData.crg_Id);
        setValue('ctg_Id', rowData.ctg_Id);
        setValue('crg_Descripcion', rowData.crg_Descripcion);
        setValue('crg_Factor_Subs', rowData.crg_Factor_Subs);
        setValue('crg_Anios_Ascenso', rowData.crg_Anios_Ascenso);
        setValue('cra_Salario_Basico', rowData.cra_Salario_Basico);
        setValue('cra_Salario_Total', rowData.cra_Salario_Total);
        setValue('cra_Anio', rowData.cra_Anio);
        setValue('crg_ProyectaSM', rowData.crg_ProyectaSM);
        setValue('crg_ProyectaSubsidio', rowData.crg_ProyectaSubsidio);

        setModalEditar(true);
    };



    const handleOnClickEditar = async (data: any) => {
        const request = {
            "crg_Descripcion": getValues('crg_Descripcion'),
            "ctg_Id": getValues('ctg_Id'),
            "crg_Id": getValues('crg_Id'),
            "crg_Factor_Subs": getValues('crg_Factor_Subs'),
            "crg_Anios_Ascenso": getValues('crg_Anios_Ascenso'),
            "cra_Salario_Basico": UtilService.cleanCurrencyValue(getValues('cra_Salario_Basico')),
            "cra_Salario_Total": UtilService.cleanCurrencyValue(getValues('cra_Salario_Total')),
            "cra_Anio": getValues('cra_Anio'),
            "crg_ProyectaSM": getValues('crg_ProyectaSM'),
            "crg_ProyectaSubsidio": getValues('crg_ProyectaSubsidio')
        }
        await apiProspeccion
            .postActualizarValoresCategoria(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setValue('cra_Anio', response.data.cra_Anio)
                        ObtenerValCategoria()
                        toast('Actualizacion de registro exitosa', { type: toast.TYPE.SUCCESS });
                        setModalEditar(false);

                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleCloseModalEditar = () => {
        setModalEditar(false);
    };

    const onChangeDesactivarSalario = async (e: any) => {

        setDesactivarSalario(e.target.checked);
        formularioDesactivarSalario.setValue('desactivarSalario', e.target.checked);
        setDesactivarSalario(e.target.checked)

        const request = {
            salarioAnioAnterior: e.target.checked
        }

        await apiProspeccion
            .postActualizarAnioSalarial(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        toast('Se actualizó el año salarial', { type: toast.TYPE.SUCCESS });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const ObtenerAnioSalaria = async () => {
        await apiProspeccion
            .getObtenerAnioSalarial()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        formularioDesactivarSalario.setValue('desactivarSalario', response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    return (
        <>


            <CHPrivateContainer>
                <CHPaper title="Datos Parametricos de Salario">
                    <Grid container>
                        <Grid item md={6} >
                            <Controller
                                name="cra_Anio"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePicker
                                        views={['year']}
                                        format="yyyy"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Fecha"
                                        onChange={onChange}
                                        value={value}
                                    />
                                )}
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <Controller
                                name="porcentajeInflacion"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido',
                                    min: {
                                        value: 1,
                                        message:
                                            'El valor debe ser mayor a 0',
                                    },
                                    max: {
                                        value: 100,
                                        message:
                                            'El valor no puede ser mayor a 100',
                                    },
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
                                        isReadOnly={false}
                                        label="Valor IPC anual (%)"
                                        style={{
                                            marginLeft: '10px',
                                        }}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                        {/* <Grid  item xs={12} md={3}>
                            <Controller
                                name="desactivarSalario"
                                control={formularioDesactivarSalario.control}
                                render={({
                                    field: { onChange, value },
                                }) => (
                                    <CHCheckbox
                                        label="Activar salario año pasado"
                                        onChange={onChangeDesactivarSalario}
                                        checked={desactivarSalario}
                                    />
                                )}
                            />
                        </Grid> */}
                    </Grid>

                    <Grid container item xs={12} >

                        <Grid item xs={12} md={12}
                            container
                            alignItems="flex-end"
                            justifyContent="flex-end"
                        >
                            <Button
                                className={classes.buttonAdd}
                                type="submit"
                                variant="outlined"
                                onClick={handleSubmit(crearValorCategoria)}
                            >
                                CREAR
                            </Button>

                            <Button
                                className={classes.buttonAdd}
                                variant="outlined"
                                onClick={ObtenerValCategoria}
                            >
                                CONSULTAR
                            </Button>
                        </Grid>
                    </Grid>
                </CHPaper>

                <CHPaper title="Tabla Parametrica de Salarios">
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell style={{ width: "11%" }}>Periodo</TableCell>
                                <TableCell style={{ width: "13%" }}>Descripcion</TableCell>
                                <TableCell style={{ width: "13%" }}>Salario Basico</TableCell>
                                <TableCell style={{ width: "13%" }}>Salario Total</TableCell>
                                <TableCell style={{ width: "13%" }}>Años Ascenso</TableCell>
                                <TableCell style={{ width: "13%" }}>Factor Subsidio</TableCell>
                                <TableCell style={{ width: "13%" }}>Proyección SMLV</TableCell>
                                <TableCell style={{ width: "13%" }}>Proyección Subsidio</TableCell>
                                <TableCell style={{ width: "8%" }}>Acción</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {valoresCategoria.map((dato: any) => (

                                <TableRow key={`row-${dato.crg_Id}`}>

                                    <TableCell>{dato.cra_Anio}</TableCell>
                                    <TableCell>{dato.crg_Descripcion}</TableCell>
                                    <TableCell>{UtilService.formatCurrency(dato.cra_Salario_Basico)}</TableCell>
                                    <TableCell>{UtilService.formatCurrency(dato.cra_Salario_Total)}</TableCell>
                                    <TableCell>{dato.crg_Anios_Ascenso}</TableCell>
                                    <TableCell>{dato.crg_Factor_Subs}</TableCell>
                                    <TableCell>{dato.crg_ProyectaSM === true ? 'Activo' : 'Inactivo'}</TableCell>
                                    <TableCell>{dato.crg_ProyectaSubsidio === true ? 'Activo' : 'Inactivo'}</TableCell>
                                    <TableCell>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            size="small"
                                            onClick={() => {
                                                handleOpenModalEditar(dato)
                                            }}
                                        >
                                            <Edit />
                                        </IconButton>
                                    </TableCell>

                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </CHPaper>
            </CHPrivateContainer>

            <ModalEditarParametrizacionLeasing
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                handleOnClickEditar={handleOnClickEditar}
                handleSubmit={handleSubmit}
                control={control}

            />

        </>
    )

}

export default ParametrizacionLeasing;

