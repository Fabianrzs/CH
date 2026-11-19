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
} from '@material-ui/core';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import moment from 'moment';
import { toast } from 'react-toastify';
import CHSelect from '../../../components/ch-select/CHSelect';
import {
    InsertarPagoEmbargo, PagoEmbargoDto
} from '../../../types/embargo.model.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import UtilService from '../../../utils/util.service';
import history from '../../../utils/history.router'
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import CatalogApi from '../../../services/api-catalog.service';
import ApiEmbargo from '../../../services/api-embargos.service';
import { ModalArchivoRespuesta } from '../../embargos/components/ModalArchivoRespuesta';
import { LoadingComponent } from '../../../components/loading/loading.component';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import AuthService from '../../../services/auth.service';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'center',
        },
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
    }),
);

export const ModalPagoEmbargos = (props: props) => {

    const {
        handleClose,
        open,
        beneficiarioSeleccionado,
        tiposIdentificacion,
        tipoDocumentoSeleccionado,
        entidadSeleccionada,
        tipoCuentaSeleccionada,
        formularioEmbargos,
        onPagoExitoso,
        nuevoBeneficiario,
        idEntidad,
        tipoCuentaSeleccionada2,
        setVista

    } = props;
    const classes = useStyles();

    

    const { control, handleSubmit } = useForm();
    const [beneficiariosPago, setBeneficiarioPago] = useState<InsertarPagoEmbargo>();
    const [loading, setLoading] = useState(false);
    const observacion = formularioEmbargos.getValues("observacionesAdicionales") ?? "";
    const valorOporcentaje = nuevoBeneficiario[0]?.valorPorcentaje;
    const catalogApi = CatalogApi();
    const API = ApiEmbargo();
    const handleOnClickRegresar = async () => history.push('/PagoEmbargosInicio');

    


    const BeneficiarioPago = async () => {
        try {
            const valor = beneficiarioSeleccionado?.valor.replace("$", "").trim();
            const ClienteTipoIdentificacion = tipoDocumentoSeleccionado?.id;
            const ClienteIdentificacion = beneficiarioSeleccionado?.numeroIdentificacion;
            const NombreRazonSocial = beneficiarioSeleccionado?.nombreRazonSocial;
            const Valor = parseFloat(valor);
            const TipoRetencionId = beneficiarioSeleccionado?.valorPorcentaje;
            const IdEmargo = await formularioEmbargos.getValues('id');
            const Fecha = new Date();

            setBeneficiarioPago({
                ClienteTipoIdentificacion,
                ClienteIdentificacion,
                NombreRazonSocial,
                Valor,
                TipoRetencionId,
                IdEmargo,
                Fecha,
            });
        } catch (error) {
            console.error("Error in BeneficiarioPago:", error);
        }
    };


    const user = AuthService.getUserInfo();

    const InsertarBDP = async () => {
        try {
            setLoading(true);

            const beneficiario = nuevoBeneficiario[0];

            const payload: PagoEmbargoDto = {
                IdEmargo: formularioEmbargos.getValues("id"),
                Fecha: new Date(),

                ClienteTipoIdentificacion: tipoDocumentoSeleccionado.id,
                ClienteIdentificacion: beneficiario.numeroIdentificacion,
                NombreRazonSocial: beneficiario.nombreRazonSocial,
                NumeroCuenta: beneficiario.numeroCuenta,
                Valor: Number(beneficiario.valor),
                Entidad: idEntidad,
                Aprobado: true,
                Observacion: observacion,
                TipoEmbargo: valorOporcentaje,

                CreadoPor: user.USR_ID ?? "SISTEMA",
                TipoRetencionId: tipoCuentaSeleccionada2 ?? null,
            };

            const response = await API.InsertarPagoEmbargo(payload);

            if (response?.isSuccess) {
                await handleOnClickRespuesta();
                toast("El pago fue ejecutado");
            } else {
                toast("El pago no fue ejecutado");
            }

        } catch (error) {
            console.error("Error en InsertarBDP:", error);
            toast("El pago no fue ejecutado");
        } finally {
            setLoading(false);
        }
    };




    const [modalOpenpdf, setModalOpenpdf] = useState(false);
    const [documentoURL, setDocumentoURL] = useState('');
    const [resolver, setResolver] = useState<((value: boolean | null) => void) | null>(null);

    const handleOnClickRespuesta = async () => {
        const solicitudId = formularioEmbargos.getValues('id');
        try {
            const response = await API.postObtenerRespuesta(solicitudId);
            const mimeType = response.type;
            if (mimeType) {
                const file = new Blob([response], { type: mimeType });
                const fileURL = URL.createObjectURL(file);
                setDocumentoURL(fileURL);
                await new Promise((resolve) => {
                    setModalOpenpdf(true);
                    setResolver(() => resolve);
                });
                if (onPagoExitoso) {
                    onPagoExitoso();
                }
            }
        } catch (error) {
            toast('Error al generar respuesta');
        }
    };



    const handleCloseModalRespuesta = async () => {
        setModalOpenpdf(false);
        setDocumentoURL('');
        handleClose();
        await handleOnClickRegresar();
    };
    const onCloseModalRespuesta = async () => {
        setModalOpenpdf(false);
        setDocumentoURL('');
        setLoading(false);
    };

    const moduloPago = 'PAGO EMBARGOS';


    return (

        <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="form-dialog-title"
            maxWidth="md"
        >
            <DialogContent style={{ width: '600px' }}>
                <Grid item>
                    <br />
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        RESUMEN DE PAGO
                    </Typography>
                </Grid>
                <br />
                <Grid container>
                    <Grid container>
                        <Grid item md={10}>
                            <Controller
                                name="tipoDocumento"
                                control={control}
                                rules={{
                                    required: 'Este campo es requerido'
                                }}
                                render={({ field, fieldState: { error } }) => (
                                    <CHTextField
                                        label="Tipo Documento *"
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        value={tipoDocumentoSeleccionado.descripcion}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        inputProps={{ pattern: '^[A-Za-z0-9]*$' }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item md={10}>
                        <Controller
                            name='numeroDocumento'
                            control={control}
                            rules={{
                                pattern: {
                                    value: /^[0-9]+$/g,
                                    message: 'Este campo es requerido y debe contener solo números'
                                },
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido y debe contener solo números',
                                },
                                minLength: {
                                    value: 5,
                                    message:
                                        'La longitud mínima del campo es de 5 caracteres',
                                },
                                maxLength: {
                                    value: 12,
                                    message:
                                        'La longitud máxima del campo es de 12 caracteres',
                                },
                                min: {
                                    value: 1,
                                    message: 'Número de cédula inválido'
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Número de Identificación *"
                                    // value={beneficiarioSeleccionado ? beneficiarioSeleccionado.numeroIdentificacion : ''}
                                    value={nuevoBeneficiario[0]?.numeroIdentificacion}
                                    numberMask

                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item md={10}>
                        <Controller
                            name="nombreRazonSocial"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido'
                            }}
                            render={({ field, fieldState: { error } }) => (
                                <CHTextField
                                    label="Nombre / Razon Social *"
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    // value={beneficiarioSeleccionado ? beneficiarioSeleccionado.nombreRazonSocial : ''}
                                    value={nuevoBeneficiario[0]?.nombreRazonSocial}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    inputProps={{ pattern: '^[A-Za-z0-9]*$' }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid item md={10}>
                    <Controller
                        name="entidad"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido'
                        }}
                        render={({ field, fieldState: { error } }) => (
                            <CHTextField
                                label="Entidad *"
                                isReadOnly
                                InputLabelProps={{ shrink: true }}
                                value={entidadSeleccionada}
                                error={!!error}
                                helperText={error ? error.message : null}
                                inputProps={{ pattern: '^[A-Za-z0-9]*$' }}
                            />
                        )}
                    />
                </Grid>
                <Grid item md={10}>
                    <Controller
                        name="tipoCuenta"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Tipo de Cuenta *"
                                isReadOnly
                                InputLabelProps={{ shrink: true }}
                                value={tipoCuentaSeleccionada}
                                error={!!error}
                                helperText={error ? error.message : null}
                                inputProps={{ pattern: '^[A-Za-z0-9]*$' }}
                            />
                        )}
                    />
                </Grid>
                <Grid container>
                    <Grid item md={10}>
                        <Controller
                            name="numeroCuenta"
                            control={control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                },

                                maxLength: {
                                    value: 21,
                                    message:
                                        'La longitud máxima del campo es de 21 caracteres',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="No. Cuenta *"
                                    // value={beneficiarioSeleccionado ? beneficiarioSeleccionado.numeroCuenta : ''}
                                    value={nuevoBeneficiario[0]?.numeroCuenta}
                                    onChange={onChange}
                                    onKeyPress={(event) => {
                                        const keyCode = event.keyCode || event.which;
                                        const keyValue = String.fromCharCode(keyCode);
                                        const regex = /^[0-9]*$/;
                                        if (!regex.test(keyValue)) {
                                            event.preventDefault();
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item md={10}>
                        <Controller
                            name="valor"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => {
                                const formatCurrency = (val: any) => {
                                    if (val === null || val === undefined || val === '') return '';
                                    return new Intl.NumberFormat("es-CO", {
                                        style: "currency",
                                        currency: "COP",
                                        minimumFractionDigits: 0,
                                    }).format(val);
                                };

                                const formattedValue = formatCurrency(
                                    nuevoBeneficiario[0]?.valor || value
                                );

                                return (
                                    <CHTextField
                                        label="Valor a pagar"
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        value={formattedValue}
                                        onChange={(e) => {

                                            const rawValue = e.target.value.replace(/\D/g, "");
                                            onChange(rawValue);
                                        }}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                    />
                                );
                            }}
                        />
                    </Grid>

                </Grid>
                <Grid container>
                    <Grid item md={10}>
                        <CHCheckbox
                            color='primary'
                            label='APROBAR PAGO'
                        />
                    </Grid>
                </Grid>
                <ModalArchivoRespuesta
                    open={modalOpenpdf}
                    documentoURL={documentoURL}
                    handleClose={handleCloseModalRespuesta}
                    onClose={onCloseModalRespuesta}
                    resolver={resolver}
                    moduloPago={moduloPago}
                    setVista={setVista}
                />
                <LoadingComponent loading={loading} />
                <Grid container>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="primary"
                            style={{ width: '200px' }}
                            onClick={() => InsertarBDP()}
                        >
                            EJECUTAR
                        </Button>
                        <Button
                            className={classes.button}
                            variant="contained"
                            color="default"
                            style={{ width: '200px' }}
                            onClick={handleClose}
                        >
                            CANCELAR
                        </Button>

                    </Grid>
                </Grid>
            </DialogContent>

        </Dialog>
    )
}

interface LinkMapDispatchToProps {
    tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
    handleClose: any;
    open: any;
    valor: any;
    beneficiarioSeleccionado: any;
    tipoDocumentoSeleccionado: any;
    entidadSeleccionada: any;
    tipoCuentaSeleccionada: any;
    formularioEmbargos: any;
    onPagoExitoso: any;
    nuevoBeneficiario: any;
    idEntidad: any;
    tipoCuentaSeleccionada2: any;
    setVista: () => void;
}

type props = LinkMapDispatchToProps;