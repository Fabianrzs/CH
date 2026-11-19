import React, { useEffect, useState } from 'react'
import { Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@material-ui/core';
import { Delete, Edit } from '@material-ui/icons';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import { TerceroBeneficiarioModel } from '../../../../models/tercero-beneficiario.model';
import NotificationService from '../../../../services/notification.service';
import { ENTIDADBANCARIA, IDENTIFICATIONTYPEDTO, VALORCATALOGO } from '../../../../types/catalog.type';
import UtilService from '../../../../utils/util.service';
import GenerarGuid from '../funciones/guid.funtions';
import ApiCredito from '../../../../services/api-credit.service';
import { ID_SUBMODALIDAD_SEGUNDO_PAGO, PE_ORIGEN, PE_PORCENTAJE, PE_TIPO_VERIFICACION, USER_SARLAFT_FOUND } from '../../../../constants/constants';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
    }),
);

function TerceroBeneficiarioPago({
    listaBeneficiario,
    responseSolicitud,
    listaValorCatalogo,
    listaEntidadBancaria,
    tercerosBeneficiarioAdicionados,
    obtenerTipoCuenta,
    validarRetiro,
    listaTipoDocumentos,
    siEsAprobadaSolcitud,
    campoSoloLecturaRadicacionAprobada,
}: {
    listaBeneficiario: TerceroBeneficiarioModel[];
    responseSolicitud: ResponseSeveranceModel | undefined;
    listaValorCatalogo: VALORCATALOGO[];
    listaEntidadBancaria: ENTIDADBANCARIA[];
    tercerosBeneficiarioAdicionados: (terceros: TerceroBeneficiarioModel[]) => void;
    obtenerTipoCuenta: (catalogoId: number) => void;
    validarRetiro: any
    listaTipoDocumentos: IDENTIFICATIONTYPEDTO[];
    siEsAprobadaSolcitud: boolean;
    campoSoloLecturaRadicacionAprobada: any;
}) {
    const classes = useStyles();
    const apiCredito = ApiCredito();
    const [terceroBeneficiarioModel, setTerceroBeneficiarioModel] = useState<TerceroBeneficiarioModel[]>([]);
    const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR BENEFICIARIO');
    const [numDocAnterior, setNumDocAnterior] = useState<string>('');
    const [idAnterior, setIdAnterior] = useState<string>('');
    const [readOnlyEntidad, setReadOnlyEntidad] = useState<boolean>(false);
    const [readOnlyNumeroCuenta, setReadOnlyNumeroCuenta] = useState<boolean>(false);
    const [ocualtarNumeroCuenta, setOcultarNumerocunta] = useState<boolean>(false);
    const SUBMODALIDAD_ADQUISICION_LEASING_HABITACIONAL_V8 = 'fe77fa46-7fa9-42c5-abe6-3d8e373e0597';

    useEffect(() => {
        obtenerTipoCuenta(67);
        if (listaBeneficiario !== null) {
            if (listaBeneficiario.length > 0) {

                listaBeneficiario.forEach(x => {
                    x.entidad = x.entidad === '00000000-0000-0000-0000-000000000000' ? null : x.entidad;
                });

                setTerceroBeneficiarioModel(listaBeneficiario);
            }
        }
    }, []);

    const controlBeneficiario = useForm({
        defaultValues: {
            tipoDocObj: '',
            numDoc: '',
            razonsocial: '',
            entidadObj: '',
            tipoCuentaObj: '',
            numeroCuenta: '',
            confirmarnumeroCuenta: '',
            valorRetirarDos: '',
        },
    });

    const cancelarTerceroBeneficiario = async () => {
        setDescEstadoBoton('AGREGAR BENEFICIARIO');
        controlBeneficiario.setValue("tipoDocObj", '');
        controlBeneficiario.setValue("numDoc", '');
        controlBeneficiario.setValue("razonsocial", '');
        controlBeneficiario.setValue("entidadObj", '');
        controlBeneficiario.setValue("tipoCuentaObj", '');
        controlBeneficiario.setValue("numeroCuenta", '');
        controlBeneficiario.reset();
        controlBeneficiario.register("entidadObj", { required: 'Este campo es requerido.' });
        controlBeneficiario.register("numeroCuenta", { required: 'Este campo es requerido.' });
    }

    const agregarTerceroBeneficiario = async (data: any) => {
        const validarSiExiste = terceroBeneficiarioModel?.some((item: any) => item.numDoc === item.numDoc)
        if (validarSiExiste && descEstadoBoton !== "EDITAR BENEFICIARIO" && (responseSolicitud?.timCodigo === 'LIBERGRAV' || responseSolicitud?.timCodigo === 'READACPROES' || responseSolicitud?.timCodigo === 'PAGOPREDIAL')) {
            toast(`Ya existe un benificiario.`, { type: toast.TYPE.ERROR })
            return
        }

        if (data.numeroCuenta !== data.confirmarnumeroCuenta) {
            toast(
                'Número de Cuenta y Confirmación de Número deben ser iguales.',
                { type: toast.TYPE.ERROR },
            );
            return
        }
        const valorNuevo = UtilService.cleanCurrencyValue(data.valorRetirarDos);
        const subModalidadSegundoPago = ID_SUBMODALIDAD_SEGUNDO_PAGO.some(item => item === responseSolicitud?.tipoSubModalidadId);
        if (valorNuevo > validarRetiro && subModalidadSegundoPago === false) {
            toast(`El valor a retirar debe ser igual al Valor de la Solicitud.`, { type: toast.TYPE.ERROR })
            return
        }

        const datoEncontrado = terceroBeneficiarioModel.findIndex(x => x.numDoc === data.numDoc && x.numDoc !== numDocAnterior);
        if (datoEncontrado === -1) {
            let canContinue = true;
            try {
                const sarlaftResponse = await apiCredito.PostVerificacionTerc({
                    pe_Origen: PE_ORIGEN,
                    pe_Dato: data.numDoc,
                    pe_TipoVerificacion: PE_TIPO_VERIFICACION,
                    pe_Porcentaje: PE_PORCENTAJE
                })
                if (sarlaftResponse?.isSuccess && sarlaftResponse?.data) {
                    canContinue = !((sarlaftResponse?.data?.encontrado as string) === USER_SARLAFT_FOUND.SI);
                }
            } catch (_) {
                toast('Error api Verificación Tercero', { type: toast.TYPE.ERROR, });
            }

            if (canContinue) {

                const guid = GenerarGuid();
                const entidadId = data.entidadObj !== "" ? data.entidadObj.split('_')[0] : null;
                const quitatPesos = UtilService.cleanCurrencyValue(
                    data.valorRetirarDos,
                );

                const terceroNuevo = {
                    id: guid.newGuid(),
                    tipoDoc: data.tipoDocObj.split('_')[0],
                    tipoDocObj: {
                        id: data.tipoDocObj.split('_')[0],
                        descripcion: data.tipoDocObj.split('_')[1]
                    },
                    numDoc: data.numDoc,
                    razonsocial: data.razonsocial,
                    entidad: entidadId === '00000000-0000-0000-0000-000000000000' ? null : entidadId,
                    entidadObj: {
                        id: entidadId === '00000000-0000-0000-0000-000000000000' ? null : entidadId,
                        descripcion: data.entidadObj !== "" ? data.entidadObj.split('_')[1] : null
                    },
                    tipoCuenta: data.tipoCuentaObj.split('_')[0],
                    tipoCuentaObj: {
                        id: data.tipoCuentaObj.split('_')[0],
                        descripcion: data.tipoCuentaObj.split('_')[1]
                    },
                    numeroCuenta: data.numeroCuenta,
                    ConfirmarCuenta: data.ConfirmarCuenta,
                    valorRetirarDos: String(quitatPesos)
                };

                let terceros: TerceroBeneficiarioModel[] = [];
                if (descEstadoBoton === 'AGREGAR BENEFICIARIO') {
                    setTerceroBeneficiarioModel(oldArray => [...oldArray, terceroNuevo]);
                    terceros = [...terceroBeneficiarioModel];
                    terceros.push(terceroNuevo);
                } else {
                    const temp = [...terceroBeneficiarioModel];
                    const posicionActual = temp.findIndex(x => x.numDoc === numDocAnterior);
                    temp.splice(posicionActual, 1);
                    terceroNuevo.id = idAnterior;
                    temp.splice(posicionActual, 0, terceroNuevo);
                    terceros = temp;
                    setTerceroBeneficiarioModel(temp);
                }

                tercerosBeneficiarioAdicionados(terceros);
                setNumDocAnterior('');
                setIdAnterior('');
                await cancelarTerceroBeneficiario();

            } else {
                NotificationService.genericAlwarning(
                    'Alerta',
                    `Por favor comunicarse con el oficial de cumplimiento`,
                    'warning')
            }

        } else {
            toast('Ya existe un Beneficiario con este mismo número de documento en la lista', { type: toast.TYPE.ERROR });
        }
    }

    const eliminarRegistroTercero = async (item: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...terceroBeneficiarioModel];
            temp.splice(temp.findIndex(x => x.id === item), 1);
            setTerceroBeneficiarioModel(temp);
            tercerosBeneficiarioAdicionados(temp);
        }
    }


    const seleccionarRegistroTercero = async (
        item: TerceroBeneficiarioModel,
      ) => {
        setNumDocAnterior(item.numDoc);
        setIdAnterior(item.id);
      
        controlBeneficiario.setValue(
          "tipoDocObj",
          `${item.tipoDocObj.id}_${item.tipoDocObj.descripcion}`,
        );
      
        controlBeneficiario.setValue("numDoc", item.numDoc);
        controlBeneficiario.setValue("razonsocial", item.razonsocial);
      
        if (item.entidadObj?.descripcion !== null) {
          controlBeneficiario.setValue(
            "entidadObj",
            `${item.entidadObj?.id}_${item.entidadObj?.descripcion}`,
          );
        }
      
        controlBeneficiario.setValue(
          "tipoCuentaObj",
          `${item.tipoCuentaObj.id}_${item.tipoCuentaObj.descripcion}`,
        );
      
        controlBeneficiario.setValue("numeroCuenta", item.numeroCuenta);
     
        controlBeneficiario.setValue(
          "confirmarnumeroCuenta",
          item.numeroCuenta,
        );
      
        controlBeneficiario.setValue(
          "valorRetirarDos",
          item.valorRetirarDos
            ? UtilService.formatCurrency(Number(item.valorRetirarDos))
            : '',
        );
      
        setDescEstadoBoton('EDITAR BENEFICIARIO');
      };

    return (
        <>
            <CHPaper title="Información Beneficiario de Pago">
                {
                    ((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' :
                        <Grid container>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="tipoDocObj"
                                    control={controlBeneficiario.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo de Documento *"
                                            required
                                            value={value}
                                            helperText={error ? error.message : null}
                                            onChange={e => {

                                                onChange(e);

                                            }}
                                        >

                                            {listaTipoDocumentos?.map(
                                                (option: IDENTIFICATIONTYPEDTO) => {
                                                    if (responseSolicitud?.descripcionModalidad === "ADQUISICIÓN DE ACCIONES DE PROPIEDAD DEL ESTADO" || responseSolicitud?.timCodigo === "LIBERGRAV" ||
                                                        responseSolicitud?.timCodigo === "APTGHGR" ||
                                                        responseSolicitud?.timCodigo === "READACPROES" ||
                                                        (responseSolicitud?.timCodigo === "CAEXLEHAPP" && responseSolicitud?.tipoSubModalidadId !== SUBMODALIDAD_ADQUISICION_LEASING_HABITACIONAL_V8) ||
                                                        responseSolicitud?.timCodigo === "CESOCLEAS" ||
                                                        responseSolicitud?.timCodigo === "EJEOPCOMP") {
                                                        if (option.descripcion === "NÚMERO DE IDENTIFICACIÓN TRIBUTARIA") {
                                                            return (
                                                                <MenuItem
                                                                    key={option.id}
                                                                    value={`${option.id}_${option.descripcion}`}
                                                                >
                                                                    {option.descripcion}
                                                                </MenuItem>
                                                            );
                                                        }

                                                    } else {
                                                        return (
                                                            <MenuItem
                                                                key={option.id}
                                                                value={`${option.id}_${option.descripcion}`}
                                                            >
                                                                {option.descripcion}
                                                            </MenuItem>
                                                        );
                                                    }
                                                    return null;

                                                }
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="numDoc"
                                    control={controlBeneficiario.control}
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
                                            InputLabelProps={{ shrink: true }}
                                            numberOnlyMask
                                            required
                                            label="Número de documento"
                                            value={value}
                                            onChange={e => {
                                                if (e.target.value.length <= 12) {
                                                    onChange(e);
                                                }
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="razonsocial"
                                    control={controlBeneficiario.control}
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
                                            required
                                            label="Nombre / Razón Social"
                                            value={value}
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{ style: { textTransform: 'uppercase', }, maxLength: 50 }}
                                            onChange={onChange}
                                        />
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="tipoCuentaObj"
                                    control={controlBeneficiario.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
                                            error={!!error}
                                            label="Tipo de Cuenta *"
                                            value={value}
                                            required
                                            helperText={error ? error.message : null}
                                            onChange={(value) => {

                                                const dato = value.target.value as string;
                                                const idEntidadBancaria = dato.split('_')[0];
                                                if (idEntidadBancaria === '322') {

                                                    setReadOnlyEntidad(false);
                                                    setReadOnlyNumeroCuenta(true);
                                                    controlBeneficiario.setValue("entidadObj", '');
                                                    controlBeneficiario.setValue("numeroCuenta", '');
                                                    setOcultarNumerocunta(true)

                                                } else {
                                                    setReadOnlyEntidad(false);
                                                    setReadOnlyNumeroCuenta(false);
                                                    setOcultarNumerocunta(false)

                                                    controlBeneficiario.register("entidadObj", {
                                                        required: 'Este campo es requerido.'
                                                    });

                                                    controlBeneficiario.register("numeroCuenta", {
                                                        required: 'Este campo es requerido.'
                                                    });
                                                }
                                                onChange(value);
                                            }}
                                        >
                                            {listaValorCatalogo?.map(
                                                (option: VALORCATALOGO) => {
                                                    const dato = `${option.idValor}_${option.descripcion}`;
                                                    return (
                                                        <MenuItem
                                                            key={option.idValor}
                                                            value={dato}
                                                        >
                                                            {option.descripcion}
                                                        </MenuItem>
                                                    );
                                                }
                                            )}
                                        </CHSelect>
                                    )}
                                />
                            </Grid>
                            {
                                ocualtarNumeroCuenta ?
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="entidadObj"
                                            rules={{
                                                required: false,
                                            }}
                                            control={controlBeneficiario.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHSelect
                                                    label="Entidad "
                                                    readOnly={readOnlyEntidad}
                                                    value={value}
                                                    onChange={(value) => {
                                                        onChange(value);
                                                    }}
                                                >
                                                    {listaEntidadBancaria?.map(
                                                        (option: ENTIDADBANCARIA) => {
                                                            const dato = `${option.idEntidadBancaria}_${option.nombreRazonSocial}`;
                                                            return (
                                                                <MenuItem
                                                                    key={option.idEntidadBancaria}
                                                                    value={dato}
                                                                >
                                                                    {option.nombreRazonSocial}
                                                                </MenuItem>
                                                            );
                                                        }
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid> : <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="entidadObj"
                                            control={controlBeneficiario.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (

                                                <CHSelect
                                                    error={!!error}
                                                    label="Entidad Bancaria *"
                                                    readOnly={readOnlyEntidad}
                                                    value={value}
                                                    required
                                                    helperText={error ? error.message : null}
                                                    onChange={(value) => {
                                                        onChange(value);
                                                    }}
                                                >
                                                    {listaEntidadBancaria?.map(
                                                        (option: ENTIDADBANCARIA) => {
                                                            const dato = `${option.idEntidadBancaria}_${option.nombreRazonSocial}`;
                                                            return (
                                                                <MenuItem
                                                                    key={option.idEntidadBancaria}
                                                                    value={dato}
                                                                >
                                                                    {option.nombreRazonSocial}
                                                                </MenuItem>
                                                            );
                                                        }
                                                    )}
                                                </CHSelect>
                                            )}
                                        />
                                    </Grid>
                            }
                            {
                                !ocualtarNumeroCuenta && (
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="numeroCuenta"
                                            control={controlBeneficiario.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    numberOnlyMask
                                                    required
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    InputLabelProps={{ shrink: true }}
                                                    isReadOnly={readOnlyNumeroCuenta}
                                                    label="Número de Cuenta"
                                                    value={value}
                                                    onChange={onChange}
                                                />
                                            )}
                                        />
                                    </Grid>
                                )
                            }
                            {
                                ocualtarNumeroCuenta && (
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="numeroCuenta"
                                            rules={{
                                                required: false,
                                            }}
                                            control={controlBeneficiario.control}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    numberOnlyMask
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    label="Número de Obligación"
                                                    value={value}
                                                    onChange={onChange}
                                                />
                                            )}
                                        />
                                    </Grid>
                                )
                            }
                            {
                                !ocualtarNumeroCuenta && (
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="confirmarnumeroCuenta"
                                            control={controlBeneficiario.control}
                                            rules={{

                                                maxLength: {
                                                    value: 21,
                                                    message: 'La longitud máxima del campo es de 21 caracteres',
                                                },
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    onPasteCapture={e => { e.preventDefault(); }}
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    InputLabelProps={{ shrink: true }}
                                                    label="Confirmación Número de Cuenta"
                                                    numberOnlyMask
                                                    value={value}
                                                    onChange={e => {
                                                        if (e.target.value.length <= 21) {
                                                            onChange(e)
                                                        }
                                                    }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                )
                            }
                            {
                                ocualtarNumeroCuenta && (
                                    <Grid className={classes.column} item xs={12} md={3}>
                                        <Controller
                                            name="confirmarnumeroCuenta"
                                            control={controlBeneficiario.control}
                                            rules={{

                                                maxLength: {
                                                    value: 21,
                                                    message: 'La longitud máxima del campo es de 21 caracteres',
                                                },
                                            }}
                                            render={({
                                                field: { onChange, value },
                                                fieldState: { error },
                                            }) => (
                                                <CHTextField
                                                    onPasteCapture={e => {
                                                        e.preventDefault();
                                                    }}
                                                    error={!!error}
                                                    helperText={error ? error.message : null}
                                                    InputLabelProps={{ shrink: true }}
                                                    label="Confirmación Número de Obligación"
                                                    numberOnlyMask
                                                    value={value}
                                                    onChange={e => {
                                                        if (e.target.value.length <= 21) {
                                                            onChange(e)
                                                        }
                                                    }}
                                                />
                                            )}
                                        />
                                    </Grid>
                                )
                            }
                            <Grid className={classes.column2} item xs={12} md={3}>
                                <Controller
                                    name="valorRetirarDos"
                                    control={controlBeneficiario.control}
                                    rules={{
                                        required: 'Este campo es requerido.',
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            currencyMask
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            inputProps={{
                                                style: {
                                                    textAlign: 'right',
                                                },
                                            }}
                                            label="Valor a Retirar"
                                            value={value}
                                            onChange={e => {

                                                const valor = e.target.value

                                                onChange(e)
                                            }}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                }
                {
                    ((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? '' : <Grid container>
                        <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                            <Button
                                className={classes.buttonAdd}
                                variant="outlined"
                                onClick={controlBeneficiario.handleSubmit(agregarTerceroBeneficiario)}
                            >
                                {descEstadoBoton}
                            </Button>

                            <Button
                                className={classes.buttonAdd}
                                variant="outlined"
                                onClick={cancelarTerceroBeneficiario}
                            >
                                CANCELAR
                            </Button>
                        </Grid>
                    </Grid>
                }
                <h3>Lista de Beneficiarios</h3>
                <TableContainer component={Paper}>
                    <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                        <Table aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Tipo de Identificación</TableCell>
                                    <TableCell>Número de Identificación</TableCell>
                                    <TableCell>Nombre / Razón Social</TableCell>
                                    <TableCell>Tipo de Cuenta</TableCell>
                                    <TableCell>Entidad Bancaria</TableCell>
                                    <TableCell>Número de Cuenta</TableCell>
                                    <TableCell>Valor a retirar</TableCell>
                                    {
                                        ((siEsAprobadaSolcitud || responseSolicitud?.secciones?.includes('INCONSISTENCIA')) && responseSolicitud?.soL_RECHAZADA !== '1') || campoSoloLecturaRadicacionAprobada ? null :
                                            <TableCell align="center">Acciones</TableCell>
                                    }
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {terceroBeneficiarioModel.map(
                                    (option: TerceroBeneficiarioModel) => (
                                        <TableRow key={option.id}>
                                            <TableCell>{option?.tipoDocObj?.descripcion}</TableCell>
                                            <TableCell>{option.numDoc}</TableCell>
                                            <TableCell>{String(option.razonsocial).toUpperCase()}</TableCell>
                                            <TableCell>{option.tipoCuentaObj.descripcion}</TableCell>
                                            <TableCell>{option.entidadObj.descripcion !== null ? option.entidadObj.descripcion : ''}</TableCell>
                                            <TableCell>{option.numeroCuenta}</TableCell>
                                            <TableCell>
                                                {UtilService.formatCurrency(
                                                    Number(
                                                        option.valorRetirarDos,
                                                    ),
                                                )}
                                            </TableCell>

                                            {
                                                (
                                                    (
                                                        siEsAprobadaSolcitud ||
                                                        responseSolicitud?.secciones?.includes('INCONSISTENCIA')
                                                    ) &&
                                                    responseSolicitud?.soL_RECHAZADA !== '1'
                                                ) ||
                                                    campoSoloLecturaRadicacionAprobada
                                                    ? null
                                                    : (
                                                        <TableCell align="center">
                                                            <IconButton
                                                                aria-label="Editar"
                                                                color="primary"
                                                                onClick={() => {
                                                                    seleccionarRegistroTercero(option);
                                                                }}
                                                            >
                                                                <Edit />
                                                            </IconButton>

                                                            <IconButton
                                                                aria-label="Eliminar"
                                                                color="secondary"
                                                                onClick={() => {
                                                                    eliminarRegistroTercero(option.id);
                                                                }}
                                                            >
                                                                <Delete />
                                                            </IconButton>
                                                        </TableCell>
                                                    )
                                            }

                                        </TableRow>
                                    )
                                )}
                            </TableBody>
                        </Table>
                    </Grid>
                </TableContainer>
            </CHPaper>
        </>
    )
}

export default TerceroBeneficiarioPago;