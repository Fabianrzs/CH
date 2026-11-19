import { Button, Grid, MenuItem } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';

import { startPostConsultCredit } from '../../actions/credit.actions';
import { CreditConsultCreditRequestModel } from '../../models/requests/credit-consult-credit-request.model';
import { CATALOG } from '../../constants/constants';
import afiliadoApi from '../../services/api-afiliado.service';
import CreditApi from '../../services/api-credit.service';
import ApiCatalog from '../../services/api-catalog.service';
import NotificationService from '../../services/notification.service';
import { AppState } from '../../store/rootReducer';
import { VALORCATALOGO } from '../../types/catalog.type';

import CHDatePicker from '../ch-datepicker/CHDatepicker';
import CHPaper from '../ch-paper/CHPaper';
import CHSelect from '../ch-select/CHSelect';
import CHTextField from '../ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
    }),
);

function DatosAliadosEstrategicos({
    procedureRequest,
    insurancesList,
    startPostConsultCredit,
}: {
    procedureRequest: any;
    insurancesList: any[];
    startPostConsultCredit: (
        request: CreditConsultCreditRequestModel,
    ) => void;
}) {
    const classes = useStyles();

    const apiEstrategico = afiliadoApi();
    const creditApi = CreditApi();
    const apiCatalog = ApiCatalog();

    const formularioDatosAliadosEstrategicos = useForm({
        defaultValues: {
            nombreAliadoEstrategico: '',
            fechaPagoEstudioTitulos: new Date(),
            FechaRecepcionEstudioTtitulos: new Date(),
            concepto: '',
            tipoParqueadero: '',
            percentageExtraBonus: 0,
            talla: 0,
            peso: 0,

            dealInsurance: '',
            dealInsuranceHome: '',

            insuranceID: '',
            homeInsuranceID: '',

            imc: '',
        },
    });

    const {
        watch,
        setValue,
        control,
        getValues,
    } = formularioDatosAliadosEstrategicos;

    const [
        obtenerAliadoActualizar,
        setObtenerAliadoActualizar,
    ] = useState(0);

    const [
        manejaPolizaVida,
        setManejaPolizaVida,
    ] = useState<any[]>([]);

    const [
        manejaPolizaHogar,
        setManejaPolizaHogar,
    ] = useState<any[]>([]);

    const [
        aliadosEstrategicos,
        setAliadosEstrategicos,
    ] = useState<any[]>([]);

    const [
        datosSolicitudCredito,
        setDatosSolicitudCredito,
    ] = useState<any>(null);

    /*
     * Si Redux todavía no ha cargado la lista,
     * evitamos error en .map().
     */
    const listaAseguradoras =
        Array.isArray(insurancesList)
            ? insurancesList
            : [];

    /*
     * =========================================================
     * UTILIDADES
     * =========================================================
     */

    const normalizarTexto = (valor: any): string =>
        String(valor ?? '')
            .trim()
            .toUpperCase();

    const esValorSi = (valor: any): boolean => {
        const texto = normalizarTexto(valor);

        return (
            texto === 'S' ||
            texto === 'SI' ||
            texto === 'SÍ' ||
            texto === 'TRUE' ||
            texto === '1'
        );
    };

    const esValorNo = (valor: any): boolean => {
        const texto = normalizarTexto(valor);

        return (
            texto === 'N' ||
            texto === 'NO' ||
            texto === 'FALSE' ||
            texto === '0'
        );
    };

    const obtenerCodigoCatalogoConvenio = (
        lista: any[],
        valorBackend: any,
    ): any => {
        if (
            !Array.isArray(lista) ||
            lista.length === 0
        ) {
            return '';
        }

        const textoBackend =
            normalizarTexto(valorBackend);

        const porCodigoExacto =
            lista.find(
                (item: any) =>
                    normalizarTexto(
                        item?.codigo,
                    ) === textoBackend,
            );

        if (porCodigoExacto) {
            return porCodigoExacto.codigo;
        }

        if (esValorSi(valorBackend)) {
            const opcionSi =
                lista.find(
                    (item: any) => {
                        const codigo =
                            normalizarTexto(
                                item?.codigo,
                            );

                        const descripcion =
                            normalizarTexto(
                                item?.descripcion,
                            );

                        return (
                            codigo === 'S' ||
                            codigo === 'SI' ||
                            codigo === '1' ||
                            descripcion === 'SI' ||
                            descripcion === 'SÍ'
                        );
                    },
                );

            return opcionSi
                ? opcionSi.codigo
                : '';
        }

        if (esValorNo(valorBackend)) {
            const opcionNo =
                lista.find(
                    (item: any) => {
                        const codigo =
                            normalizarTexto(
                                item?.codigo,
                            );

                        const descripcion =
                            normalizarTexto(
                                item?.descripcion,
                            );

                        return (
                            codigo === 'N' ||
                            codigo === 'NO' ||
                            codigo === '0' ||
                            descripcion === 'NO'
                        );
                    },
                );

            return opcionNo
                ? opcionNo.codigo
                : '';
        }

        return '';
    };

    /*
     * =========================================================
     * CATÁLOGO VIDA
     * =========================================================
     */

    const obtenerManejaPolizaVida = async () => {
        await apiCatalog
            .getAllCatalogoPorId(
                CATALOG.MANEJA_POLIZA_VIDA,
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setManejaPolizaVida(
                            response.data || [],
                        );
                    } else {
                        toast(
                            `Error: ${response.returnMessage}`,
                            {
                                type:
                                    toast.TYPE.ERROR,
                            },
                        );
                    }
                }
            })
            .catch(() => {
                toast(
                    'Error del sistema',
                    {
                        type:
                            toast.TYPE.ERROR,
                    },
                );
            });
    };

    /*
     * =========================================================
     * CATÁLOGO HOGAR
     * =========================================================
     */

    const obtenerManejaPolizaHogar = async () => {
        await apiCatalog
            .getAllCatalogoPorId(
                CATALOG.MANEJA_POLIZA_HOGAR,
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setManejaPolizaHogar(
                            response.data || [],
                        );
                    } else {
                        toast(
                            `Error: ${response.returnMessage}`,
                            {
                                type:
                                    toast.TYPE.ERROR,
                            },
                        );
                    }
                }
            })
            .catch(() => {
                toast(
                    'Error del sistema',
                    {
                        type:
                            toast.TYPE.ERROR,
                    },
                );
            });
    };

    /*
     * =========================================================
     * CONSULTAR SOLICITUD
     * =========================================================
     */

    const consultarSolicitudCredito = async () => {
        try {
            const request = {
                estado: '0',

                idSolicitudCredito:
                    procedureRequest?.requestID,

                identificacion:
                    `${procedureRequest?.clientIdentificationNumber}`,
            };

            const response =
                await creditApi
                    .postConsultCreditData(
                        request,
                    );

            if (
                !response ||
                !response.isSuccess ||
                !response.data
            ) {
                console.log(
                    'consultarSolicitudCredito sin información válida',
                    response,
                );

                return;
            }

            console.log(
                'DATOS SOLICITUD CRÉDITO',
                response.data,
            );

            setDatosSolicitudCredito(
                response.data,
            );
        } catch (error) {
            console.log(
                'Error consultando solicitud crédito',
                error,
            );
        }
    };

    /*
     * =========================================================
     * CARGAR SEGUROS DESDE SOLICITUD
     * =========================================================
     */

    useEffect(() => {
        if (!datosSolicitudCredito) {
            return;
        }

        /*
         * =========================
         * CONVENIO VIDA
         * =========================
         */

        const convenioVida =
            datosSolicitudCredito
                .convenioAseguradora ??
            datosSolicitudCredito
                .convenioVida;

        if (
            manejaPolizaVida.length > 0
        ) {
            const codigoConvenioVida =
                obtenerCodigoCatalogoConvenio(
                    manejaPolizaVida,
                    convenioVida,
                );

            if (
                codigoConvenioVida !== ''
            ) {
                setValue(
                    'dealInsurance',
                    codigoConvenioVida,
                );
            }
        }

        /*
         * =========================
         * ASEGURADORA VIDA
         * =========================
         */

        const {idAseguradora} =
            datosSolicitudCredito;

        if (
            idAseguradora !== null &&
            idAseguradora !== undefined &&
            Number(idAseguradora) !== 0
        ) {
            setValue(
                'insuranceID',
                idAseguradora,
            );
        } else {
            setValue(
                'insuranceID',
                '',
            );
        }

        /*
         * =========================
         * CONVENIO HOGAR
         * =========================
         */

        const convenioHogar =
            datosSolicitudCredito
                .convenioAseguradoraHogar ??
            datosSolicitudCredito
                .convenioHogar;

        if (
            manejaPolizaHogar.length > 0
        ) {
            const codigoConvenioHogar =
                obtenerCodigoCatalogoConvenio(
                    manejaPolizaHogar,
                    convenioHogar,
                );

            if (
                codigoConvenioHogar !== ''
            ) {
                setValue(
                    'dealInsuranceHome',
                    codigoConvenioHogar,
                );
            }
        }

        /*
         * =========================
         * ASEGURADORA HOGAR
         * =========================
         */

        const {idAseguradoraHogar} =
            datosSolicitudCredito;

        if (
            idAseguradoraHogar !== null &&
            idAseguradoraHogar !== undefined &&
            Number(
                idAseguradoraHogar,
            ) !== 0
        ) {
            setValue(
                'homeInsuranceID',
                idAseguradoraHogar,
            );
        } else {
            setValue(
                'homeInsuranceID',
                '',
            );
        }
    }, [
        datosSolicitudCredito,
        manejaPolizaVida,
        manejaPolizaHogar,
        setValue,
    ]);

    /*
     * =========================================================
     * PESO / TALLA
     * =========================================================
     */

    const obtenerPesoYTalla = async () => {
        const request =
            procedureRequest?.requestID;

        await creditApi
            .postObtenerPesoYTalla(
                request,
            )
            .then(async (response: any) => {
                if (
                    response &&
                    response.isSuccess
                ) {
                    if (
                        response.data === null
                    ) {
                        return;
                    }

                    const aliados =
                        response.data;

                    setValue(
                        'peso',
                        aliados?.soC_PESO,
                    );

                    setValue(
                        'talla',
                        aliados?.soC_TALLA,
                    );
                }
            })
            .catch(() => {
                toast(
                    'Error del sistema',
                    {
                        type:
                            toast.TYPE.ERROR,
                    },
                );
            });
    };

    /*
     * =========================================================
     * IMC
     * =========================================================
     */

    const peso =
        watch('peso');

    const talla =
        watch('talla');

    useEffect(() => {
        const pesoNumero =
            Number(peso);

        const tallaNumero =
            Number(talla);

        if (
            pesoNumero > 0 &&
            tallaNumero > 0
        ) {
            const tallaMetros =
                tallaNumero / 100;

            const imc =
                pesoNumero /
                (
                    tallaMetros *
                    tallaMetros
                );

            setValue(
                'imc',
                imc.toFixed(2),
            );
        } else {
            setValue(
                'imc',
                '',
            );
        }
    }, [
        peso,
        talla,
        setValue,
    ]);

    /*
     * =========================================================
     * ALIADOS ESTRATÉGICOS
     * =========================================================
     */

    const consultarAliadosEstrategicos =
        async () => {
            try {
                const response =
                    await creditApi
                        .getConsultarAliadosEstrategicos();

                if (
                    response &&
                    response.data
                ) {
                    setAliadosEstrategicos(
                        response.data,
                    );
                }
            } catch (error) {
                console.log(
                    'Error consultando aliados',
                    error,
                );
            }
        };

    /*
     * =========================================================
     * DATOS ALIADO GUARDADOS
     * =========================================================
     */

    const obtenerDatosAliadosEstrategicos =
        async () => {
            const request = {
                soc_Id:
                    procedureRequest?.requestID,
            };

            await apiEstrategico
                .postObtenerAliadosEstrategicos(
                    request,
                )
                .then(async (response: any) => {
                    if (
                        !response ||
                        !response.isSuccess ||
                        response.data === null
                    ) {
                        return;
                    }

                    const aliados =
                        response.data;

                    setObtenerAliadoActualizar(
                        aliados.dat_Id,
                    );

                    setValue(
                        'nombreAliadoEstrategico',
                        aliados.nombre_Aliado ?? '',
                    );

                    setValue(
                        'fechaPagoEstudioTitulos',
                        aliados.fecha_Pago_Estudio_Titulo,
                    );

                    setValue(
                        'FechaRecepcionEstudioTtitulos',
                        aliados.fecha_Recepcion_Estudio_Titulo,
                    );

                    setValue(
                        'concepto',
                        aliados.concepto ?? '',
                    );

                    setValue(
                        'tipoParqueadero',
                        aliados.tipo_Parqueadero ?? '',
                    );

                    setValue(
                        'percentageExtraBonus',
                        aliados.percentageExtraBonus ?? 0,
                    );

                    if (
                        aliados.peso !== null &&
                        aliados.peso !== undefined
                    ) {
                        setValue(
                            'peso',
                            aliados.peso,
                        );
                    }

                    if (
                        aliados.talla !== null &&
                        aliados.talla !== undefined
                    ) {
                        setValue(
                            'talla',
                            aliados.talla,
                        );
                    }

                    /*
                     * Si todavía no ha llegado
                     * consultarSolicitudCredito,
                     * usamos datos del aliado.
                     */

                    if (
                        !datosSolicitudCredito
                    ) {
                        if (
                            aliados.dealInsurance !== null &&
                            aliados.dealInsurance !== undefined
                        ) {
                            setValue(
                                'dealInsurance',
                                aliados.dealInsurance,
                            );
                        }

                        if (
                            aliados.dealInsuranceHome !== null &&
                            aliados.dealInsuranceHome !== undefined
                        ) {
                            setValue(
                                'dealInsuranceHome',
                                aliados.dealInsuranceHome,
                            );
                        }

                        if (
                            aliados.insuranceID !== null &&
                            aliados.insuranceID !== undefined &&
                            Number(
                                aliados.insuranceID,
                            ) !== 0
                        ) {
                            setValue(
                                'insuranceID',
                                aliados.insuranceID,
                            );
                        }

                        if (
                            aliados.homeInsuranceID !== '' &&
                            aliados.homeInsuranceID !== '' &&
                            Number(
                                aliados.homeInsuranceID,
                            ) !== 0
                        ) {
                            setValue(
                                'homeInsuranceID',
                                aliados.homeInsuranceID,
                            );
                        }
                    }
                })
                .catch(() => {
                    toast(
                        'Error del sistema',
                        {
                            type:
                                toast.TYPE.ERROR,
                        },
                    );
                });
        };

    /*
     * =========================================================
     * INICIAL
     * =========================================================
     */

    useEffect(() => {
        obtenerManejaPolizaVida();
        obtenerManejaPolizaHogar();

        obtenerDatosAliadosEstrategicos();

        obtenerPesoYTalla();

        consultarSolicitudCredito();

        /*
         * Este llamado también se hace en el componente de seguros
         * que ya funciona en el proyecto. Ayuda a garantizar que
         * Redux tenga cargada la información necesaria de crédito,
         * incluida la lista usada por los selects de aseguradoras.
         */
        const dataConsultCreditRequest:
            CreditConsultCreditRequestModel = {
                estado: '0',
                idSolicitudCredito:
                    procedureRequest?.requestID,
                identificacion:
                    `${procedureRequest?.clientIdentificationNumber}`,
            };

        startPostConsultCredit(
            dataConsultCreditRequest,
        );

        consultarAliadosEstrategicos();
    }, []);

    /*
     * =========================================================
     * LISTA DE ASEGURADORAS DESDE REDUX
     * =========================================================
     */

    useEffect(() => {
        console.log(
            'ASEGURADORAS DATOS ALIADOS',
            insurancesList,
        );
    }, [insurancesList]);

    /*
     * =========================================================
     * CAMBIOS DE CONVENIO
     * =========================================================
     *
     * IMPORTANTE:
     * SI y NO permiten mostrar aseguradora.
     *
     * No deshabilitamos el listado.
     */

    const cambiarConvenioVida = (
        event: any,
        onChange: any,
    ) => {
        const { value: valor } = event.target;
    
        setValue(
            'insuranceID',
            '',
        );
    
        onChange(event);
    
        console.log(
            'Convenio Vida:',
            valor,
        );
    };
    
    const cambiarConvenioHogar = (
        event: any,
        onChange: any,
    ) => {
        const { value: valor } = event.target;
    
        setValue(
            'homeInsuranceID',
            '',
        );
    
        onChange(event);
    
        console.log(
            'Convenio Hogar:',
            valor,
        );
    };

    /*
     * =========================================================
     * GUARDAR
     * =========================================================
     */

    const agregarEstratigico =
        async (data: any) => {
            const response = {
                dat_Id: 0,

                soc_Id:
                    procedureRequest?.requestID,

                nombre_Aliado:
                    data.nombreAliadoEstrategico,

                fecha_Pago_Estudio_Titulo:
                    data.fechaPagoEstudioTitulos,

                fecha_Recepcion_Estudio_Titulo:
                    data.FechaRecepcionEstudioTtitulos,

                concepto:
                    data.concepto,

                tipo_Parqueadero:
                    data.tipoParqueadero,

                percentageExtraBonus:
                    Number(
                        data.percentageExtraBonus,
                    ),

                talla:
                    data.talla,

                peso:
                    data.peso,

                dealInsurance:
                    data.dealInsurance,

                dealInsuranceHome:
                    data.dealInsuranceHome,

                /*
                 * VACÍO => 0
                 */
                insuranceID:
                    Number(
                        data.insuranceID,
                    ) || 0,

                homeInsuranceID:
                    String(
                        data.homeInsuranceID,
                    ) || '',

                imc:
                    data.imc,
            };

            console.log(
                'REQUEST GUARDAR',
                response,
            );

            await apiEstrategico
                .postAliadosEstrategicos(
                    response,
                )
                .then(async (result: any) => {
                    if (
                        result &&
                        result.isSuccess
                    ) {
                        setObtenerAliadoActualizar(
                            result.data.dat_Id,
                        );

                        await NotificationService.genericAlert(
                            'Guardado',
                            'guardado con éxito.',
                        );
                    }
                })
                .catch(() => {
                    toast(
                        'Error del sistema',
                        {
                            type:
                                toast.TYPE.ERROR,
                        },
                    );
                });
        };

    /*
     * =========================================================
     * ACTUALIZAR
     * =========================================================
     */

    const actulizarDatosEstrategicos =
        async (data: any) => {
            const response = {
                dat_Id:
                    obtenerAliadoActualizar,

                soc_Id:
                    procedureRequest?.requestID,

                nombre_Aliado:
                    data.nombreAliadoEstrategico,

                fecha_Pago_Estudio_Titulo:
                    data.fechaPagoEstudioTitulos,

                fecha_Recepcion_Estudio_Titulo:
                    data.FechaRecepcionEstudioTtitulos,

                concepto:
                    data.concepto,

                tipo_Parqueadero:
                    data.tipoParqueadero,

                percentageExtraBonus:
                    Number(
                        data.percentageExtraBonus,
                    ),

                talla:
                    data.talla,

                peso:
                    data.peso,

                dealInsurance:
                    data.dealInsurance,

                dealInsuranceHome:
                    data.dealInsuranceHome,

                insuranceID:
                    Number(
                        data.insuranceID,
                    ) || 0,

                homeInsuranceID:
                    String(
                        data.homeInsuranceID,
                    ) || '',

                imc:
                    data.imc,
            };

            console.log(
                'REQUEST ACTUALIZAR',
                response,
            );

            await apiEstrategico
                .postActulaizarAliadoEstrategico(
                    response,
                )
                .then(async (result: any) => {
                    if (
                        result &&
                        result.isSuccess
                    ) {
                        await NotificationService.genericAlert(
                            'Actualizado',
                            'guardado con éxito.',
                        );
                    }
                })
                .catch(() => {
                    toast(
                        'Error del sistema',
                        {
                            type:
                                toast.TYPE.ERROR,
                        },
                    );
                });
        };

    /*
     * =========================================================
     * RENDER
     * =========================================================
     */

    return (
        <>
            <CHPaper title="Datos Aliados Estratégicos">
                <Grid container>
                    {/* ALIADO */}

                    <Grid
                        className={
                            classes.column
                        }
                        item
                        xs={12}
                        md={3}
                    >
                        <Controller
                            name="nombreAliadoEstrategico"
                            control={
                                control
                            }
                            rules={{
                                required:
                                    'Este campo es requerido.',
                            }}
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHTextField
                                    select
                                    label="Nombre Aliado Estratégico"
                                    value={
                                        value
                                    }
                                    onChange={
                                        onChange
                                    }
                                    error={
                                        !!error
                                    }
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                >
                                    <MenuItem value="">
                                        <em>
                                            Seleccione...
                                        </em>
                                    </MenuItem>

                                    {aliadosEstrategicos
                                        .filter(
                                            (
                                                aliado: any,
                                            ) =>
                                                aliado.alI_ESTADO,
                                        )
                                        .map(
                                            (
                                                aliado: any,
                                            ) => (
                                                <MenuItem
                                                    key={
                                                        aliado.alI_ID
                                                    }
                                                    value={
                                                        aliado.alI_ID
                                                    }
                                                >
                                                    {
                                                        aliado.alI_RAZON_SOCIAL
                                                    }
                                                </MenuItem>
                                            ),
                                        )}
                                </CHTextField>
                            )}
                        />
                    </Grid>

                    {/* FECHA PAGO */}

                    <Grid
                        className={
                            classes.column
                        }
                        item
                        xs={12}
                        md={3}
                    >
                        <Controller
                            name="fechaPagoEstudioTitulos"
                            rules={{
                                required:
                                    'Este campo es requerido',
                            }}
                            control={
                                control
                            }
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                            }) => (
                                <CHDatePicker
                                    label="Fecha de Pago Estudio Títulos "
                                    inputProps={{
                                        readOnly:
                                            true,
                                    }}
                                    onChange={
                                        onChange
                                    }
                                    value={
                                        value
                                    }
                                    autoOk
                                />
                            )}
                        />
                    </Grid>

                    {/* FECHA RECEPCION */}

                    <Grid
                        className={
                            classes.column
                        }
                        item
                        xs={12}
                        md={3}
                    >
                        <Controller
                            name="FechaRecepcionEstudioTtitulos"
                            control={
                                control
                            }
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Recepcion Estudio Títulos "
                                    inputProps={{
                                        readOnly:
                                            true,
                                    }}
                                    onChange={
                                        onChange
                                    }
                                    value={
                                        value
                                    }
                                    autoOk
                                />
                            )}
                        />
                    </Grid>

                    {/* CONCEPTO */}

                    <Grid
                        className={
                            classes.column
                        }
                        item
                        xs={12}
                        md={3}
                    >
                        <Controller
                            name="concepto"
                            control={
                                control
                            }
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                            }) => (
                                <CHTextField
                                    label="Concepto "
                                    value={
                                        value
                                    }
                                    onChange={
                                        onChange
                                    }
                                />
                            )}
                        />
                    </Grid>

                    {/* EXTRAPRIMA */}

                    <Grid
                        item
                        md={4}
                        className={
                            classes.column
                        }
                    >
                        <Controller
                            name="percentageExtraBonus"
                            control={
                                control
                            }
                            rules={{
                                required:
                                    'Este campo es requerido.',

                                pattern: {
                                    value:
                                        /^\d*\.?\d*$/,
                                    message:
                                        'Solo se aceptan caracteres numericos',
                                },

                                min: {
                                    value: 0,
                                    message:
                                        'El valor a ingresar es de 0 a 500',
                                },

                                max: {
                                    value: 500,
                                    message:
                                        'El valor no puede ser mayor a 500',
                                },
                            }}
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHTextField
                                    error={
                                        !!error
                                    }
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                    isReadOnly={
                                        procedureRequest
                                            .stateActivityID ===
                                        4
                                    }
                                    label="Porcentaje Extraprima(%) *"
                                    value={
                                        value
                                    }
                                    inputProps={{
                                        style: {
                                            textAlign:
                                                'right',
                                        },
                                    }}
                                    onChange={e => {
                                        if (
                                            e.target
                                                .value
                                                .length <=
                                            3
                                        ) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[a-zA-ZñÑáéíóúÁÉÍÓÚ/@=+:{}#%$()*&¡!¿?[|`"~,<>^_-]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    {/* ==================================================
                        CONVENIO VIDA
                    ================================================== */}

                    <Grid
                        item
                        md={4}
                        className={
                            classes.column
                        }
                    >
                        <Controller
                            name="dealInsurance"
                            control={
                                control
                            }
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHSelect
                                    error={
                                        !!error
                                    }
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                    label="Convenio Seguro Vida"
                                    value={
                                        value
                                    }
                                    onChange={event =>
                                        cambiarConvenioVida(
                                            event,
                                            onChange,
                                        )
                                    }
                                >
                                    {manejaPolizaVida.map(
                                        (
                                            option: VALORCATALOGO,
                                        ) => (
                                            <MenuItem
                                                key={
                                                    option.codigo
                                                }
                                                value={
                                                    option.codigo
                                                }
                                            >
                                                {String(
                                                    option.descripcion ||
                                                        '',
                                                ).toUpperCase()}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    {/* ==================================================
                        ASEGURADORA VIDA
                        
                        IMPORTANTE:
                        NO depende de SI/NO.
                        SIEMPRE SE MUESTRA.
                    ================================================== */}

                    <Grid
                        item
                        md={4}
                        className={
                            classes.column
                        }
                    >
                        <Controller
                            name="insuranceID"
                            control={
                                control
                            }
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHSelect
                                    error={
                                        !!error
                                    }
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                    label="Aseguradora Seguro Vida"
                                    value={
                                        value
                                    }
                                    onChange={
                                        onChange
                                    }
                                >
                                    <MenuItem value="">
                                        NO SELECCIONADO
                                    </MenuItem>

                                    {listaAseguradoras.map(
                                        (
                                            insurance: any,
                                        ) => (
                                            <MenuItem
                                                key={`insurance-${insurance.id}`}
                                                value={
                                                    insurance.id
                                                }
                                            >
                                                {String(
                                                    insurance.razonSocial ||
                                                        '',
                                                ).toUpperCase()}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    {/* ==================================================
                        CONVENIO HOGAR
                    ================================================== */}

                    <Grid
                        item
                        md={4}
                        className={
                            classes.column
                        }
                    >
                        <Controller
                            name="dealInsuranceHome"
                            control={
                                control
                            }
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHSelect
                                    error={
                                        !!error
                                    }
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                    label="Convenio Seguro Hogar"
                                    value={
                                        value
                                    }
                                    onChange={event =>
                                        cambiarConvenioHogar(
                                            event,
                                            onChange,
                                        )
                                    }
                                >
                                    {manejaPolizaHogar.map(
                                        (
                                            option: VALORCATALOGO,
                                        ) => (
                                            <MenuItem
                                                key={
                                                    option.codigo
                                                }
                                                value={
                                                    option.codigo
                                                }
                                            >
                                                {
                                                    option.descripcion
                                                }
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    {/* ==================================================
                        ASEGURADORA HOGAR

                        TAMBIÉN SE MUESTRA SI ES SI O NO
                    ================================================== */}

                    <Grid
                        item
                        md={4}
                        className={
                            classes.column
                        }
                    >
                        <Controller
                            name="homeInsuranceID"
                            control={
                                control
                            }
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHSelect
                                    error={
                                        !!error
                                    }
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                    label="Aseguradora Seguro Hogar"
                                    value={
                                        value
                                    }
                                    onChange={
                                        onChange
                                    }
                                >
                                    <MenuItem value="">
                                        NO SELECCIONADO
                                    </MenuItem>

                                    {listaAseguradoras.map(
                                        (
                                            insurance: any,
                                        ) => (
                                            <MenuItem
                                                key={`insurance-home-${insurance.id}`}
                                                value={
                                                    insurance.id
                                                }
                                            >
                                                {String(
                                                    insurance.razonSocial ||
                                                        '',
                                                ).toUpperCase()}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>

                    {/* PESO */}

                    <Grid
                        item
                        md={4}
                        className={
                            classes.column
                        }
                    >
                        <Controller
                            name="peso"
                            control={
                                control
                            }
                            rules={{
                                required:
                                    'Este campo es requerido.',

                                pattern: {
                                    value:
                                        /^\d*\.?\d*$/,
                                    message:
                                        'Solo se aceptan caracteres numericos',
                                },

                                min: {
                                    value: 0,
                                    message:
                                        'El valor a ingresar es de 0 a 999',
                                },

                                max: {
                                    value: 999,
                                    message:
                                        'El valor no puede ser mayor a 999',
                                },
                            }}
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHTextField
                                    error={
                                        !!error
                                    }
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                    label="Peso (kg)*"
                                    value={
                                        value
                                    }
                                    inputProps={{
                                        style: {
                                            textAlign:
                                                'right',
                                        },
                                    }}
                                    onChange={e => {
                                        if (
                                            e.target
                                                .value
                                                .length <=
                                            3
                                        ) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[a-zA-ZñÑáéíóúÁÉÍÓÚ/@=+:{}#%$()*&¡!¿?[|`"~,<>^_-]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    {/* TALLA */}

                    <Grid
                        item
                        md={4}
                        className={
                            classes.column
                        }
                    >
                        <Controller
                            name="talla"
                            control={
                                control
                            }
                            rules={{
                                required:
                                    'Este campo es requerido.',

                                pattern: {
                                    value:
                                        /^\d*\.?\d*$/,
                                    message:
                                        'Solo se aceptan caracteres numericos',
                                },

                                min: {
                                    value: 0,
                                    message:
                                        'El valor a ingresar es de 0 a 999',
                                },

                                max: {
                                    value: 999,
                                    message:
                                        'El valor no puede ser mayor a 999',
                                },
                            }}
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHTextField
                                    error={
                                        !!error
                                    }
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                    label="Estatura (cm)*"
                                    value={
                                        value
                                    }
                                    inputProps={{
                                        style: {
                                            textAlign:
                                                'right',
                                        },
                                    }}
                                    onChange={e => {
                                        if (
                                            e.target
                                                .value
                                                .length <=
                                            3
                                        ) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[a-zA-ZñÑáéíóúÁÉÍÓÚ/@=+:{}#%$()*&¡!¿?[|`"~,<>^_-]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    {/* IMC */}

                    <Grid
                        className={
                            classes.column
                        }
                        item
                        xs={12}
                        md={4}
                    >
                        <Controller
                            name="imc"
                            control={
                                control
                            }
                            render={({
                                field: {
                                    value,
                                },
                            }) => (
                                <CHTextField
                                    label="IMC"
                                    value={
                                        value
                                    }
                                    isReadOnly
                                />
                            )}
                        />
                    </Grid>
                </Grid>

                {/* BOTON */}

                {obtenerAliadoActualizar !==
                0 ? (
                    <Grid
                        item
                        xs={12}
                        md={3}
                    >
                        <Button
                            variant="contained"
                            type="button"
                            style={{
                                backgroundColor:
                                    '#1C3F8B',
                                color:
                                    'white',
                            }}
                            onClick={formularioDatosAliadosEstrategicos.handleSubmit(
                                actulizarDatosEstrategicos,
                            )}
                        >
                            Actualizar
                        </Button>
                    </Grid>
                ) : (
                    <Grid
                        item
                        xs={12}
                        md={3}
                    >
                        <Button
                            variant="contained"
                            type="button"
                            style={{
                                backgroundColor:
                                    '#1C3F8B',
                                color:
                                    'white',
                            }}
                            onClick={formularioDatosAliadosEstrategicos.handleSubmit(
                                agregarEstratigico,
                            )}
                        >
                            Guardar
                        </Button>
                    </Grid>
                )}
            </CHPaper>
        </>
    );
}

const mapStateToProps = (
    state: AppState,
) => ({
    insurancesList:
        state.credit.insurancesList,
});

const mapDispatchToProps = {
    startPostConsultCredit,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(DatosAliadosEstrategicos);
