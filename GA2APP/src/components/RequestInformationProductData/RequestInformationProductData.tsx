import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { Button, Grid, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { toast } from 'react-toastify';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import { CreditProductRequestModel, Matricula } from '../../models/requests/credit-product-request.model';
import { CreditProductModel } from '../../models/credit-product.model';
import ApiCredit from '../../services/api-credit.service';
import CHSelect from '../ch-select/CHSelect';
import { ESTADOVIVIENDA, IDENTIFICATIONTYPEDTO, TIPOINMUEBLE, TIVIVIENDA, VALORCATALOGO } from '../../types/catalog.type';
import CatalogApi from '../../services/api-catalog.service';
import Prospeccion from '../../services/api-prospeccion.service';
import { Address } from '../../types/address.type';
import { setFormDataAddres } from '../modals/address/constants/address.components.constants';
import AddressComponentV2 from '../../pages/productClient/components/address.component.v2';
import NotificationService from '../../services/notification.service';
import GenerarGuid from '../../pages/AdministracionCuentas/components/funciones/guid.funtions';
import { CatalogState } from '../../reducers/catalog.reducer';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
			textAlign: 'right',
		},
    }),
);

function RequestInformationProductData({
    startPostCreditProductData,
    creditProductData,
    procedureRequest,
    isLoading,
    catalogState

}: {
    startPostCreditProductData: (data: CreditProductRequestModel) => void;
    creditProductData: CreditProductModel;
    procedureRequest: ProcedureRequestModel;
    isLoading: boolean;
    catalogState: CatalogState;

}) {

    const classes = useStyles();
    const apiCredit = ApiCredit();
    const apiCatalog = CatalogApi();
    const apiProspeccion = Prospeccion()

    const isFinalizedActivity = procedureRequest.stateActivityID === 4;

    const [mostrarMatricula1, setMostrarMatricula1] = useState(false);
    const [mostrarMatricula2, setMostrarMatricula2] = useState(false);
    const [mostrarMatricula3, setMostrarMatricula3] = useState(false);

    const [verGarajeEditar, setVerGarajeEditar] = useState(false);
    const [verDepositoEditar, setVerDepositoEditar] = useState(false);
    const [tipoDepositoCatalogo, setTipoDepositoCatalogo] = useState([]);
    const [tipoGarajeCatalogo, setTipoGarajeCatalogo] = useState([]);
    const [garaje, setGaraje] = useState<VALORCATALOGO[]>([]);
    const [conjuntoCerrado, setConjuntoCerrado] = useState<VALORCATALOGO[]>([]);
    const [tipoInmuebles, setTipoInmuebles] = useState<TIPOINMUEBLE[]>([]);
    const [tipoMatriculas, setTipoMatriculas] = useState<any[]>([]);
    const [matriculasCredito, setMatriculasCredito] = useState<any[]>([]);
    const [estadoViviendaCatalogo, setEstadoViviendaCatalogo] = useState<[]>([]);
    const [tipoViviendaCatalogo, setTipoViviendaCatalogo] = useState<[]>([]);
    const [isLoadings, setIsLoadings] = useState(isLoading);
    const [dataSolicitudCredito, setDataSolicitudCredito] = useState<any>({});
    const [informacionVendedores, setInformacionVendedores] = useState<any [] | null>(null);
    const [verAscensor, setVerAscensor] = useState(false);
    const [openDireccionDetalleProducto, setOpenDireccionDetalleProducto] = useState(false);
    const [direccionValue, setDireccionValue] = useState(true);
    const [deshabilitarCampos, setDeshabilitarCampos] = useState(false);

    const formularioGarantia = useForm<any>({
        defaultValues: {
            tipoDeposito: "",
            tipoGaraje: "",
            garaje: " ",
            deposito: " ",
            tipoInmueble: "",
            poseeAsensor: "",
            pisoInmueble: "",
            conjuntoCerrado: "",
            estadoVivienda: "",
            añosConstruccion: 0,
            direccionInmueble: "",
            direccion: '',
            localidadInmueble: "",
            barrioInmueble: "",
            propiedadHorizontal: "",
            tipoVivienda: "",
            numeroMatricula1: "",
            numeroMatricula2: "",
            numeroMatricula3: "",
            tipoMatricula1: 0,
            tipoMatricula2: 0,
            tipoMatricula3: 0,
            areaConstruidaMatricula1: 0,
            areaConstruidaMatricula2: 0,
            areaConstruidaMatricula3: 0,
            valorInmuebleMatricula1: 0,
            valorInmuebleMatricula2: 0,
            valorInmuebleMatricula3: 0,
            valorAvaluo: 0,
        },
    })

    useEffect(() => {

        if (procedureRequest.flowActivityID === 21) {
            setDeshabilitarCampos(true)
        }

        const dataRequest: CreditProductRequestModel = {
            ind: 2,
            soC_ID: procedureRequest.requestID,
        };
        startPostCreditProductData(dataRequest);

        obtenerMatriculasCreditoPorSolicitud();
        obtenerTipoParqueadero();
        obtenerGaraje();
        cargarTipoInmueble();
        obtenerConjuntoCerrado();
        obtenerCatalogoTipoMatricula();
        obtenerCatalogoEstadoVivienda();
        obtenerCatalogoTipoVivienda();
        datosInfoSolicitud();
        datosSimulacion();
        datosInfoVendedores();
    }, []);

    const guardar = async (data: any) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro ?',
        );
        if (confirmAlert) {

            ActualizarMatricula(data)

            const request = {

                clI_IDENTIFICACION: dataSolicitudCredito.clI_IDENTIFICACION,
                dpC_ID: dataSolicitudCredito.dpC_ID,
                dpC_ID_INMUEBLE: dataSolicitudCredito.dpC_ID_INMUEBLE,
                dpC_ID_VENDEDOR: dataSolicitudCredito.dpC_ID_VENDEDOR,
                dpD_ID: dataSolicitudCredito.dpD_ID,
                dpD_ID_INMUEBLE: dataSolicitudCredito.dpD_ID_INMUEBLE,
                dpD_ID_VENDEDOR: dataSolicitudCredito.dpD_ID_VENDEDOR,
                slS_ID: dataSolicitudCredito.slS_ID,
                soC_AUTORIZA_CONSULTA_CENTRALES: dataSolicitudCredito.soC_AUTORIZA_CONSULTA_CENTRALES,
                soC_AUTORIZA_USO_DATOS: dataSolicitudCredito.soC_AUTORIZA_USO_DATOS,
                soC_CAPACIDAD_ENDEUDAMIENTO: dataSolicitudCredito.soC_CAPACIDAD_ENDEUDAMIENTO,
                soC_DECLARACION_ASEGURABILIDAD: dataSolicitudCredito.soC_DECLARACION_ASEGURABILIDAD,
                soC_DECLARACION_ORIGEN_FONDOS: dataSolicitudCredito.soC_DECLARACION_ORIGEN_FONDOS,
                soC_ESTADO: dataSolicitudCredito.soC_ESTADO,
                soC_NIVEL_ENDEUDAMIENTO: dataSolicitudCredito.soC_NIVEL_ENDEUDAMIENTO,
                soC_NIVEL_ENDEUDAMIENTO_CUOTA: dataSolicitudCredito.soC_NIVEL_ENDEUDAMIENTO_CUOTA,
                soC_PORC_EXTRAPRIMA: dataSolicitudCredito.soC_PORC_EXTRAPRIMA,
                soP_CORREO_VENDEDOR: dataSolicitudCredito.soP_CORREO_VENDEDOR,
                soP_DIRECCION_VENDEDOR: dataSolicitudCredito.soP_DIRECCION_VENDEDOR,
                soP_FECHA_ENTREGA_INMUEBLE: dataSolicitudCredito.soP_FECHA_ENTREGA_INMUEBLE,
                soP_MATRICULA_INMOBILIARIA1: dataSolicitudCredito.soP_MATRICULA_INMOBILIARIA1,
                soP_MATRICULA_INMOBILIARIA2: dataSolicitudCredito.soP_MATRICULA_INMOBILIARIA2,
                soP_MATRICULA_INMOBILIARIA3: dataSolicitudCredito.soP_MATRICULA_INMOBILIARIA3,
                soP_NOMBRE_VENDEDOR: dataSolicitudCredito.soP_NOMBRE_VENDEDOR,
                soP_NUMERO_DOCUMENTO_VENDEDOR: dataSolicitudCredito.soP_NUMERO_DOCUMENTO_VENDEDOR,
                soP_PLAZO_FINANCIACION: dataSolicitudCredito.soP_PLAZO_FINANCIACION,
                soP_PORC_FINANCIACION: dataSolicitudCredito.soP_PORC_FINANCIACION,
                soP_TELEFONO_VENDEDOR: dataSolicitudCredito.soP_TELEFONO_VENDEDOR,

                soP_VALOR_CREDITO: dataSolicitudCredito.soP_VALOR_CREDITO,
                soP_VALOR_INMUEBLE: dataSolicitudCredito.soP_VALOR_INMUEBLE,
                tcR_ID: dataSolicitudCredito.tcR_ID,
                tiD_ID: dataSolicitudCredito.tiD_ID,
                tiD_ID_VENDEDOR: dataSolicitudCredito.tiD_ID_VENDEDOR,

                tvV_ID: (data.estadoVivienda),
                soP_ESTADO_INMUEBLE: data.tipoVivienda,
                soP_AÑOS_CONSTRUCCION: data.añosConstruccion,
                soP_BARRIO_INMUEBLE: data.barrioInmueble,
                soP_CONJUNTO_CERRADO: data.conjuntoCerrado,
                soP_DIRECCION_INMUEBLE: data.direccionInmueble,
                soP_LOCALIDAD_INMUEBLE: data.localidadInmueble,
                soP_PISO_INMUEBLE: data.pisoInmueble,
                soP_POSEE_ASENSOR: data.poseeAsensor,
                soP_PROPIEDAD_HORIZONTAL: data.propiedadHorizontal,
                soP_TIENE_DEPOSITO: data.deposito,
                soP_TIENE_GARAJE: data.garaje,
                soP_TIPO_DEPOSITO: data.tipoDeposito,
                soP_TIPO_GARAGE: data.tipoGaraje,
                soP_TIPO_INMUEBLE: data.tipoInmueble,
            }
            await apiProspeccion
                .postCrearSolicitudCredito(request)
                .then((response: any) => {
                    if (response) {
                        if (response.isSuccess) {
                            datosInfoSolicitud();
                        }
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                });
            obtenerMatriculasCreditoPorSolicitud();
        }

    }

    const ActualizarMatricula = async (data: any) => {
        const guid = GenerarGuid();
        const matriculas: any[] = [];

        for (let i = 0; i < matriculasCredito.length; i += 1) {
            const matricula = {
                numeroMatricula: data[`numeroMatricula${i + 1}`],
                fechaMatricula: matriculasCredito[i].fechaMatricula,
                chip: matriculasCredito[i].chip,
                cedulaCatastral: matriculasCredito[i].cedulaCatastral,
                tipo_Matricula: data[`tipoMatricula${i + 1}`],
                areaPrivadaConstruida: matriculasCredito[i].areaPrivadaConstruida,
                areaAvaluoComercial: matriculasCredito[i].areaAvaluoComercial,
                areaAvaluoCatastral: matriculasCredito[i].areaAvaluoCatastral,
            };

            matriculas.push(matricula);
        }

        try {
            await Promise.all(
                matriculasCredito.map((item) =>
                    apiCredit.deleteMatriculaCredito(item.idMatricula)
                )
            );

            await Promise.all(
                matriculas.map(async (item) => {
                    const matricula: Matricula = {
                        idMatricula: guid.newGuid(),
                        numeroMatricula: item.numeroMatricula,
                        fechaMatricula: item.fechaMatricula,
                        chip: item.chip,
                        cedulaCatastral: item.cedulaCatastral,
                        tipo_Matricula: item.tipo_Matricula,
                        areaPrivadaConstruida: item.areaPrivadaConstruida,
                        areaAvaluoComercial: item.areaAvaluoComercial,
                        areaAvaluoCatastral: item.areaAvaluoCatastral,
                        socId: procedureRequest.requestID,
                    };

                    await apiCredit.postCrearMatriculaCredito(matricula);

                    await NotificationService.genericAlert(
                        'Guardado',
                        'Información almacenada'
                    );
                })
            );
        } catch (error) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        }
    };

    const obtenerMatriculasCreditoPorSolicitud = async () => {
        setIsLoadings(true)
        await apiCredit
            .postObtenerMatriculasCreditoPorSolicitud(
                procedureRequest.requestID
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null) {
                        setMatriculasCredito(response.data);
                        if (response.data[0]) {
                            formularioGarantia.setValue("numeroMatricula1", response.data[0].numeroMatricula)
                            formularioGarantia.setValue("tipoMatricula1", response.data[0].tipo_Matricula)
                            formularioGarantia.setValue("areaConstruidaMatricula1", response.data[0].areaPrivadaConstruida)
                            formularioGarantia.setValue("valorInmuebleMatricula1", response.data[0].areaAvaluoComercial)
                            setMostrarMatricula1(true)

                        }

                        if (response.data[1]) {
                            formularioGarantia.setValue("numeroMatricula2", response.data[1].numeroMatricula)
                            formularioGarantia.setValue("tipoMatricula2", response.data[1].tipo_Matricula)
                            formularioGarantia.setValue("areaConstruidaMatricula2", response.data[1].areaPrivadaConstruida)
                            formularioGarantia.setValue("valorInmuebleMatricula2", response.data[1].areaAvaluoComercial)
                            setMostrarMatricula2(true)

                        }

                        if (response.data[2]) {
                            formularioGarantia.setValue("numeroMatricula3", response.data[2].numeroMatricula)
                            formularioGarantia.setValue("tipoMatricula3", response.data[2].tipo_Matricula)
                            formularioGarantia.setValue("areaConstruidaMatricula3", response.data[2].areaPrivadaConstruida)
                            formularioGarantia.setValue("valorInmuebleMatricula3", response.data[2].areaAvaluoComercial)
                            setMostrarMatricula3(true)

                        }
                    }
                }
                setIsLoadings(false)
            })
            .catch((e) => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
                setIsLoadings(false)
            });
    };


    const datosInfoSolicitud = async () => {

        await apiCredit
            .getObtenerSolicitudCreditoTipoNumeroIdentificacion(

                1,
                String(procedureRequest.clientIdentificationNumber)
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null) {
                        setDataSolicitudCredito(response.data)

                        formularioGarantia.setValue("garaje", response.data.soP_TIENE_GARAJE)
                        formularioGarantia.setValue("deposito", response.data.soP_TIENE_DEPOSITO)
                        formularioGarantia.setValue("tipoInmueble", response.data.soP_TIPO_INMUEBLE)
                        formularioGarantia.setValue("conjuntoCerrado", response.data.soP_CONJUNTO_CERRADO)
                        formularioGarantia.setValue("pisoInmueble", creditProductData.propertyType)
                        formularioGarantia.setValue("añosConstruccion", response.data.soP_AÑOS_CONSTRUCCION)
                        formularioGarantia.setValue("direccionInmueble", response.data.soP_DIRECCION_INMUEBLE)
                        formularioGarantia.setValue("localidadInmueble", response.data.soP_LOCALIDAD_INMUEBLE)
                        formularioGarantia.setValue("barrioInmueble", response.data.soP_BARRIO_INMUEBLE)
                        formularioGarantia.setValue("propiedadHorizontal", response.data.soP_PROPIEDAD_HORIZONTAL)
                        formularioGarantia.setValue("poseeAsensor", response.data.soP_POSEE_ASENSOR)
                        formularioGarantia.setValue("pisoInmueble", response.data.soP_PISO_INMUEBLE)
                        formularioGarantia.setValue("tipoDeposito", response.data.soP_TIPO_DEPOSITO)
                        formularioGarantia.setValue("tipoGaraje", response.data.soP_TIPO_GARAGE)
                        formularioGarantia.setValue("estadoVivienda", response.data.tvV_ID)
                        formularioGarantia.setValue("tipoVivienda", response.data.soP_ESTADO_INMUEBLE)

                        if (response.data.soP_TIENE_GARAJE === "SI") {
                            setVerGarajeEditar(true)
                        } else {
                            setVerGarajeEditar(false)
                        }

                        if (response.data.soP_TIENE_DEPOSITO === "SI") {
                            setVerDepositoEditar(true)
                        } else {
                            setVerDepositoEditar(false)
                        }

                        setDataSolicitudCredito(response.data)

                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    };

    const datosSimulacion = async () => {

        await apiCredit
            .getObtenerSimulacionPersonales(
                String(procedureRequest.clientIdentificationNumber)
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null) {
                        formularioGarantia.setValue("valorInmueble", response?.data?.valorInmueble)
                        formularioGarantia.setValue("valorAvaluo", response?.data?.avaluoInmueble)
                    }
                }
            })
            .catch(e => {
                toast('Error al consultar datos simulación', { type: toast.TYPE.ERROR });
            });

    };

    const datosInfoVendedores = async () => {

        const request = {
            Soc_Id: procedureRequest.requestID,
            Sls_Id: '00000000-0000-0000-0000-000000000000'
        }
        await apiCredit
            .postObtenerVendedor(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null && response.data?.length > 0) {
                        setInformacionVendedores(response?.data);
                    }
                }
            })
            .catch(e => {
                toast('Error al consultar datos simulación', { type: toast.TYPE.ERROR });
            });

    };

    const cargarTipoInmueble = async () => {
        await apiCatalog
            .getObtenerValoresCatalogosPorIdCatalogo(43)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setTipoInmuebles(response.data);

                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };
    const obtenerTipoParqueadero = async () => {
        await apiCatalog
            .getAllCatalogoPorId(82)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setTipoDepositoCatalogo(response.data);
                        setTipoGarajeCatalogo(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerGaraje = async () => {
        await apiCatalog
            .getObtenerValoresCatalogosPorIdCatalogo(76)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setGaraje(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerConjuntoCerrado = async () => {
        await apiCatalog
            .getObtenerValoresCatalogosPorIdCatalogo(78)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setConjuntoCerrado(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerCatalogoEstadoVivienda = async () => {
        await apiCatalog
            .getEstadoVivienda()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        setEstadoViviendaCatalogo(response.data);

                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };
    const obtenerCatalogoTipoVivienda = async () => {
        await apiCatalog
            .getVivienda()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        setTipoViviendaCatalogo(response.data);

                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };
    const obtenerCatalogoTipoMatricula = async () => {
        await apiCatalog
            .getObtenerValoresCatalogosPorIdCatalogo(90)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {

                        setTipoMatriculas(response.data);

                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };



    const resetAddress = () => {
        formularioGarantia.setValue('idTipoCalle', '');
        formularioGarantia.setValue('idLetra1', '');
        formularioGarantia.setValue('numeroCalle1', '');
        formularioGarantia.setValue('direccionBis1', '');
        formularioGarantia.setValue('idCardinal1', '');
        formularioGarantia.setValue('numeroCalle2', '');
        formularioGarantia.setValue('idLetra2', '');
        formularioGarantia.setValue('idCardinal2', '');
        formularioGarantia.setValue('numeroCalle3', '');
        formularioGarantia.setValue('idLetra3', '');
        formularioGarantia.setValue('idCardinal3', '');
        formularioGarantia.setValue('direccionComplemento', '');
    };

    const handleOpenAddressDetalleProducto = () => {
        resetAddress();
        setOpenDireccionDetalleProducto(true);
    };

    const onCloseAddressDetalleProducto = (value: Address) => {
        if (value !== null) {
            const result = setFormDataAddres(value);
            formularioGarantia.setValue('direccionInmueble', result);
            setOpenDireccionDetalleProducto(false);
            setDireccionValue(false);
            formularioGarantia.clearErrors('direccionInmueble');
        } else {
            formularioGarantia.setValue('direccionInmueble', creditProductData.sopPropertyAddress);
            setOpenDireccionDetalleProducto(false);
            setDireccionValue(true);
        }
    };

    const handleKeyPressDetalleProducto = (e: any) => {
        if (e.key === 'Enter') {
            handleOpenAddressDetalleProducto();
        }
    };

    if (isLoadings) {
        return <CHLoadingProcess />;
    }

    return (
        <>
            <AddressComponentV2
                onOpen={openDireccionDetalleProducto}
                onClose={onCloseAddressDetalleProducto}
                control={formularioGarantia.control}
                getValues={formularioGarantia.watch}
                tipoDireccion={false}
                setValue={formularioGarantia.setValue}
            />
            <CHPaper title="Detalle del Producto">
                <Grid container>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="tipoVivienda"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo vivienda *"
                                    readOnly={deshabilitarCampos}
                                    value={value}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={onChange}

                                >
                                    {tipoViviendaCatalogo?.map(
                                        (option: TIVIVIENDA) => (
                                            <MenuItem
                                                key={
                                                    option.tiV_VIVIENDA_ID
                                                }
                                                value={
                                                    option.tiV_VIVIENDA_DESCRIPCION
                                                }
                                            >
                                                {
                                                    option.tiV_VIVIENDA_DESCRIPCION
                                                }
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="tipoInmueble"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo Inmueble *"
                                    value={value}
                                    readOnly={deshabilitarCampos}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={e => {

                                        const tipoInmue = e.target.value
                                        if (tipoInmue === 'APARTAMENTO') {
                                            setVerAscensor(false)
                                        } else {
                                            setVerAscensor(true)
                                        }
                                        onChange(e)
                                    }}
                                >
                                    {tipoInmuebles?.map(
                                        (option: TIPOINMUEBLE) => (
                                            <MenuItem
                                                key={option.descripcion}
                                                value={
                                                    option.descripcion
                                                }
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>


                    {
                        !verAscensor &&
                        <Grid item md={3} className={classes.column} >
                            <Controller
                                name="poseeAsensor"
                                control={formularioGarantia.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (

                                    <CHSelect
                                        label="Posee Ascensor"
                                        value={value}
                                        readOnly={deshabilitarCampos}
                                        onChange={onChange}
                                    >
                                        <MenuItem key="0" value="0">
                                            SI
                                        </MenuItem>
                                        <MenuItem key="1" value="1">
                                            NO
                                        </MenuItem>
                                    </CHSelect>
                                )}
                            />

                        </Grid>

                    }

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="pisoInmueble"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    label="Piso del Inmueble "
                                    numberAniosMask
                                    isReadOnly={deshabilitarCampos}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="conjuntoCerrado"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Conjunto cerrado *"
                                    value={value}
                                    readOnly={deshabilitarCampos}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={onChange}
                                >
                                    {conjuntoCerrado?.map(
                                        (item: VALORCATALOGO) => (
                                            <MenuItem
                                                key={item.codigo}
                                                value={item.codigo}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="estadoVivienda"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Estado Construcción"
                                    value={value}
                                    readOnly={deshabilitarCampos}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={onChange}

                                >
                                    {estadoViviendaCatalogo.map(
                                        (option: ESTADOVIVIENDA) => (
                                            <MenuItem
                                                key={option.esV_ID + 2}
                                                value={option.esV_ID + 2}
                                            >
                                                {option.esV_DESCRIPCION}
                                            </MenuItem>
                                        ),
                                    )}

                                </CHSelect>
                            )}
                        />
                    </Grid>

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="añosConstruccion"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido',
                                min: {
                                    value: 1,
                                    message:
                                        'El valor debe ser mayor a cero',
                                },
                                max: {
                                    value: 50,
                                    message:
                                        'El valor debe ser menor a 50',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberAniosMask
                                    label="Años construcción *"
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={deshabilitarCampos}
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value.length <= 2
                                        ) {
                                            onChange(e);
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="direccionInmueble"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                value: direccionValue,
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    label="Dirección del inmueble *"
                                    error={!!error}
                                    disabled={deshabilitarCampos}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                        onClick:
                                            handleOpenAddressDetalleProducto,
                                    }}
                                    onKeyDown={
                                        handleKeyPressDetalleProducto
                                    }

                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly={deshabilitarCampos}
                            label="Departamento del Inmueble"
                            value={creditProductData.propertyState || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly={deshabilitarCampos}
                            label="Ciudad del Inmueble"
                            value={creditProductData.propertyCity || ''}
                        />
                    </Grid>

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="localidadInmueble"
                            control={formularioGarantia.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{ style: { textTransform: 'uppercase', }, }}

                                    InputLabelProps={{ shrink: true }}
                                    label="Localidad del Inmueble"
                                    error={!!error}
                                    isReadOnly={deshabilitarCampos}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="barrioInmueble"
                            control={formularioGarantia.control}

                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    inputProps={{ style: { textTransform: 'uppercase', }, }}


                                    InputLabelProps={{ shrink: true }}
                                    label="Barrio del Inmueble "
                                    error={!!error}
                                    isReadOnly={deshabilitarCampos}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="propiedadHorizontal"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField

                                    inputProps={{
                                        style: { textTransform: 'uppercase', },

                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    label="Nombre propiedad horizontal* "
                                    error={!!error}
                                    isReadOnly={deshabilitarCampos}
                                    value={value}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={onChange}

                                />
                            )}
                        />
                    </Grid>

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="deposito"
                            control={formularioGarantia.control}
                            defaultValue=""
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Depósito *"
                                    value={value}
                                    readOnly={deshabilitarCampos}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={e => {
                                        const tipoGaraj = e.target.value
                                        if (tipoGaraj === "SI") {
                                            setVerDepositoEditar(true)
                                        } else {
                                            setVerDepositoEditar(false)

                                        }
                                        onChange(e)
                                    }}
                                >
                                    {garaje?.map(
                                        (item: VALORCATALOGO) => (
                                            <MenuItem

                                                key={item.codigo}
                                                value={item.codigo}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    {
                        verDepositoEditar ?

                            <Grid item md={3} className={classes.column}>
                                <Controller
                                    name="tipoDeposito"
                                    control={formularioGarantia.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            defaultValue=""
                                            error={!!error}
                                            readOnly={deshabilitarCampos}
                                            label="Tipo Depósito *"
                                            value={value}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                            {tipoDepositoCatalogo.map(
                                                (item: VALORCATALOGO) => (
                                                    <MenuItem
                                                        key={item.codigo}
                                                        value={item.codigo}
                                                    >
                                                        {item.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                            : []
                    }

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="garaje"
                            control={formularioGarantia.control}
                            defaultValue=""
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Garaje *"
                                    value={value}
                                    readOnly={deshabilitarCampos}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={e => {
                                        const tipoGaraj = e.target.value
                                        if (tipoGaraj === "SI") {
                                            setVerGarajeEditar(true)
                                        } else {
                                            setVerGarajeEditar(false)

                                        }
                                        onChange(e)
                                    }}

                                >
                                    {garaje?.map(
                                        (item: VALORCATALOGO) => (
                                            <MenuItem
                                                key={item.codigo}
                                                value={item.codigo}
                                            >
                                                {item.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    {
                        verGarajeEditar ?
                            <Grid item md={3} className={classes.column}>
                                <Controller
                                    name="tipoGaraje"
                                    control={formularioGarantia.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo Garaje *"
                                            value={value}
                                            readOnly={deshabilitarCampos}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                            {tipoGarajeCatalogo.map(

                                                (item: VALORCATALOGO) => (
                                                    <MenuItem
                                                        key={item.codigo}
                                                        value={item.codigo}
                                                    >
                                                        {item.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                            : []

                    }

                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="valorAvaluo"
                            control={formularioGarantia.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    inputProps={{
                                        style: { textTransform: 'uppercase',textAlign: 'right', },

                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    label="Valor Avalúo"
                                    error={!!error}
                                    isReadOnly
                                    value={value}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={12}>
                        <Typography className={classes.titleform} variant="h5" component="h6">Información Matrículas</Typography>
                    </Grid>

                        {mostrarMatricula1 &&
                            <Grid container>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="numeroMatricula1"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Número Matrícula 1 *"
                                                error={!!error}
                                                isReadOnly={deshabilitarCampos}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}

                                                value={value}
                                                onChange={onChange}

                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="tipoMatricula1"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                label="Tipo de Matricula 1"
                                                value={value}
                                                readOnly={deshabilitarCampos}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            >
                                                {tipoMatriculas.map(
                                                    (item: VALORCATALOGO) => (
                                                        <MenuItem
                                                            key={item.idValor}
                                                            value={item.idValor}
                                                        >
                                                            {item.descripcion}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="areaConstruidaMatricula1"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Área Construida"
                                                error={!!error}
                                                isReadOnly
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}

                                                value={value}
                                                onChange={onChange}

                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="valorInmuebleMatricula1"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                currencyMask
                                                InputLabelProps={{ shrink: true }}
                                                label="Valor Inmueble"
                                                error={!!error}
                                                isReadOnly
                                                helperText={
                                                    error ? error.message : null
                                                }
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
                            </Grid>
                        }
                        
                        {mostrarMatricula2 &&
                            <Grid container>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="numeroMatricula2"
                                        control={formularioGarantia.control}

                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
                                            label="Número Matrícula 2 *"
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            inputProps={{
                                                style: {
                                                    textTransform: 'uppercase',
                                                },
                                            }}
                                            isReadOnly={deshabilitarCampos}
                                            value={value}
                                            onChange={onChange}

                                        />
                                    )}
                                />
                        </Grid>
                        <Grid item md={3} className={classes.column}>
                                <Controller
                                    name="tipoMatricula2"
                                    control={formularioGarantia.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo de Matricula 2"
                                            value={value}
                                            readOnly={deshabilitarCampos}
                                            helperText={
                                                error ? error.message : null
                                            }
                                            onChange={onChange}
                                        >
                                            {tipoMatriculas.map(
                                                (item: VALORCATALOGO) => (
                                                    <MenuItem
                                                        key={item.idValor}
                                                        value={item.idValor}
                                                    >
                                                        {item.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="areaConstruidaMatricula2"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Área Construida"
                                                error={!!error}
                                                isReadOnly
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}

                                                value={value}
                                                onChange={onChange}

                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="valorInmuebleMatricula2"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                currencyMask
                                                InputLabelProps={{ shrink: true }}
                                                label="Valor Inmueble"
                                                error={!!error}
                                                isReadOnly
                                                helperText={
                                                    error ? error.message : null
                                                }
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
                        </Grid>
                    }
                        {mostrarMatricula3 &&
                            <Grid container>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="numeroMatricula3"
                                        control={formularioGarantia.control}

                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Número Matrícula 3 *"
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}
                                                isReadOnly={deshabilitarCampos}
                                                value={value}
                                                onChange={onChange}

                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="tipoMatricula3"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                label="Tipo de Matricula 3"
                                                value={value}
                                                readOnly={deshabilitarCampos}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            >
                                                {tipoMatriculas.map(
                                                    (item: VALORCATALOGO) => (
                                                        <MenuItem
                                                            key={item.idValor}
                                                            value={item.idValor}
                                                        >
                                                            {item.descripcion}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="areaConstruidaMatricula3"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Área Construida"
                                                error={!!error}
                                                isReadOnly
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                inputProps={{
                                                    style: {
                                                        textTransform: 'uppercase',
                                                    },
                                                }}

                                                value={value}
                                                onChange={onChange}

                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item md={3} className={classes.column}>
                                    <Controller
                                        name="valorInmuebleMatricula3"
                                        control={formularioGarantia.control}
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                currencyMask
                                                InputLabelProps={{ shrink: true }}
                                                label="Valor Inmueble"
                                                error={!!error}
                                                isReadOnly
                                                helperText={
                                                    error ? error.message : null
                                                }
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
                            </Grid>
                        }
                {informacionVendedores && informacionVendedores?.length > 0 && (
                     <Grid container>
                        {informacionVendedores && informacionVendedores?.length > 0 && (
                        <Grid className={classes.column} item xs={12} md={12}>
                            <Typography className={classes.titleform} variant="h5" component="h6">Información Vendedores</Typography>
                        </Grid>
                        )}
                        <TableContainer component={Paper}>
                        <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                            <Table aria-label="simple table">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Tipo de Identificación</TableCell>
                                        <TableCell>Número de Identificación</TableCell>
                                        <TableCell>Nombre / Razón Social</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {informacionVendedores.map(
                                        (option: any) => (
                                            <TableRow key={option.id}>
                                                <TableCell>{catalogState?.IdentificationType?.find((item: IDENTIFICATIONTYPEDTO) => item?.id === option?.tipo_Doc)?.descripcion || ''}</TableCell>
                                                <TableCell>{option?.numero_Doc}</TableCell>
                                                <TableCell>{option?.nombre_Vendedor}</TableCell>
                                            </TableRow>
                                        )
                                    )}
                                </TableBody>
                            </Table>
                        </Grid>
                    </TableContainer>
                 </Grid>
               )}

                {deshabilitarCampos ? null :
                    <Grid item xs={12} md={3}>
                        <Button
                            onClick={formularioGarantia.handleSubmit(guardar)}
                            variant="contained"
                            style={{ marginTop: '20px', backgroundColor: '#1C3F8B', color: 'white' }}
                        >
                            Guardar
                        </Button>
                    </Grid>
                }

            </CHPaper>
        </>
    );
}
export default RequestInformationProductData;
