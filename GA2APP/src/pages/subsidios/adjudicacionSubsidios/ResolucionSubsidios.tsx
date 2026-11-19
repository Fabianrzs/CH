import {
    Button,
    createStyles,
    makeStyles,
} from '@material-ui/core';

import React, {
    useEffect,
    useState,
} from 'react';

import {
    connect,
    ConnectedProps,
} from 'react-redux';

import swal from 'sweetalert';
import { useForm } from 'react-hook-form';

import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';

import CuadrosAfectados from './CuadrosAfectados';
import InformacionEntrada from './InformacionEntrada';
import Subsidios from './Subsidios';
import SubsidiosAnulados from './SubsidiosAnulados';

import NotificationService from '../../../services/notification.service';

import { AppState } from '../../../store/rootReducer';

import {
    startPostConsultarSolicitudes,
} from '../../../actions/solicitud.action';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            marginTop: '1.5rem',
            marginRight: '.5rem',
        },
        buttonConfirmacion: {
            backgroundColor: '#c4c4c4',
            color: '#595959',
        },
        buttonAnular: {
            backgroundColor: '#c4c4c4',
            color: '#595959',
        },
        buttonCancelar: {
            backgroundColor: '#efefef',
            color: '#595959',
        },
    }),
);

export type SolicitudSubsidioResolucion = {
    numeroSolicitud: number;
    procesoDescripcion: string;
    idTipoIdentificacion: number;
    cedulaCliente: string;
    nombreCliente: string;
    fechaSolicitud: string;
    valorRetirar: number;
    tareaNombre: string;
    estadoSolicitud: string;
    estadoSolicitudId: number;
    solicitudTareaId: string;
    idUsuarioAsignado: string;
    usuarioAsignado: string;
    idCliente: number;
    timCodigo: string;
    tpsSubId: string;
    tpsSubDescripcion: string;
    radicadoGestionDocumental: string | null;
    numeroRadicadoSG: string | null;
    modalidadDescripcion: string;
    cdp?: string | null;
    tercero?: string | null;
    factura?: string | null;
};

const normalizarTexto = (
	valor: unknown,
): string =>
	String(valor ?? '')
		.trim()
		.toUpperCase()
		.normalize('NFD')
		.replace(/[\u0300-\u036f]/g, '');

const filtrarSubsidiosResolucion = (
    solicitudes: any[],
): SolicitudSubsidioResolucion[] => {
    if (!Array.isArray(solicitudes)) {
        return [];
    }

    return solicitudes.filter(
        (solicitud: any) => {
            const proceso = normalizarTexto(
                solicitud?.procesoDescripcion,
            );

            const submodalidad = normalizarTexto(
                solicitud?.tpsSubDescripcion,
            );

            const esSubsidioSecuestrados =
                proceso ===
                'SUBSIDIO SECUESTRADOS';

            const esSegundoPago =
                submodalidad ===
                'SEGUNDO PAGO';

            const esSimultaneo =
                submodalidad ===
                'SIMULTANEO';

            return (
                esSubsidioSecuestrados &&
                (esSegundoPago || esSimultaneo)
            );
        },
    );
};

const mapStateToProps = (
    state: AppState,
) => ({
    solicitudesConsultadas:
        state.solicitud.solicitudes ?? [],
});

const connector = connect(
    mapStateToProps,
    {
        consultarSolicitudes:
            startPostConsultarSolicitudes,
    },
);

type ReduxProps = ConnectedProps<
    typeof connector
>;

type OwnProps = {
    regresar: () => void;
};

type Props = OwnProps & ReduxProps;

const RealizacionSubsidios = ({
    regresar,
    solicitudesConsultadas,
    consultarSolicitudes,
}: Props) => {
    const classes = useStyles();

    const [
        subsidiosSeleccionados,
        setSubsidiosSeleccionados,
    ] = useState<
        SolicitudSubsidioResolucion[]
    >([]);

    const [
        subsidios,
        setSubsidios,
    ] = useState<
        SolicitudSubsidioResolucion[]
    >([]);

    const [
        cargandoSubsidios,
        setCargandoSubsidios,
    ] = useState(false);

    const formularioInformacionEntrada =
        useForm({
            defaultValues: {
                numeroResolucion: '',
                fechaInicioResolucion: '',
                fechaCierreResolucion: '',
                estado: '',
                radicado: '',
                totalAdjudicaciones: '',
                montoTotal: '',
            },
        });

    const formularioCuadrosAfectados =
        useForm({
            defaultValues: {
                totalSubsidios: '',
                totalMonto: '',
                fechaAdjudicacion: '',
            },
        });

    useEffect(() => {
        const consultar = async () => {
            setCargandoSubsidios(true);

            try {
                /*
                    Temporalmente se deja el objeto vacío
                    hasta completar los campos reales de
                    ParametrosBusqueda.
                */
                const parametrosBusqueda: any = {};

                await consultarSolicitudes(
                    parametrosBusqueda,
                    'NO',
                );
            } catch (error) {
                console.error(
                    'Error consultando solicitudes para resolución de subsidios:',
                    error,
                );

                NotificationService.genericAlwarning(
                    'Advertencia',
                    'No fue posible consultar las solicitudes de subsidios.',
                    'warning',
                );
            } finally {
                setCargandoSubsidios(false);
            }
        };

        consultar();
    }, [consultarSolicitudes]);

    useEffect(() => {
        const resultados =
            filtrarSubsidiosResolucion(
                solicitudesConsultadas,
            );

        setSubsidios(resultados);

        setSubsidiosSeleccionados(
            (seleccionadosActuales) =>
                seleccionadosActuales.filter(
                    (seleccionado) =>
                        resultados.some(
                            (resultado) =>
                                resultado.solicitudTareaId ===
                                seleccionado.solicitudTareaId,
                        ),
                ),
        );
    }, [solicitudesConsultadas]);

    const anular = () => {
        if (
            subsidiosSeleccionados.length === 0
        ) {
            NotificationService.genericAlwarning(
                'Advertencia',
                'Debe seleccionar al menos un subsidio.',
                'warning',
            );

            return;
        }

        swal({
            title: '¿Es Subsanable?',
            text: '',
            icon: 'warning',
            buttons: {
                rechazar: {
                    text: 'SI',
                    value: 'SI',
                    className:
                        classes.buttonConfirmacion,
                },
                anular: {
                    text: 'NO',
                    value: 'NO',
                    className:
                        classes.buttonAnular,
                },
                cancelar: {
                    text: 'CANCELAR',
                    value: null,
                    className:
                        classes.buttonCancelar,
                },
            },
            closeOnClickOutside: false,
        }).then((respuesta) => {
            if (respuesta === 'SI') {
                NotificationService.genericAlert(
                    'Exitoso!',
                    'Se realizó la petición con éxito.',
                );

                return;
            }

            if (respuesta === 'NO') {
                NotificationService.genericAlert(
                    'Exitoso!',
                    'Se realizó la petición con éxito.',
                );
            }
        });
    };

    const generarPdf = () => {
        console.log('generarPdf');
    };

    const cerrarResolucion = () => {
        console.log('cerrarResolucion');
    };

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Gestión de subsidios"
                subText=""
                title1="Realización de"
                title2="Resolución de Subsidios"
            />

            <InformacionEntrada
                formularioInformacionEntrada={
                    formularioInformacionEntrada
                }
            />

            <CuadrosAfectados
                formularioCuadrosAfectados={
                    formularioCuadrosAfectados
                }
            />

            <Subsidios
                subsidios={subsidios}
                cargando={cargandoSubsidios}
                subsidiosSeleccionados={
                    subsidiosSeleccionados
                }
                setSubsidiosSeleccionados={
                    setSubsidiosSeleccionados
                }
                anular={anular}
            />

            <SubsidiosAnulados />

            <div className="buttons">
                <Button
                    className={classes.button}
                    variant="outlined"
                    color="primary"
                    onClick={generarPdf}
                >
                    GENERAR PDF
                </Button>

                <Button
                    className={classes.button}
                    variant="outlined"
                    color="primary"
                    onClick={cerrarResolucion}
                >
                    CERRAR RESOLUCIÓN
                </Button>

                <Button
                    className={classes.button}
                    variant="outlined"
                    color="primary"
                    onClick={regresar}
                >
                    REGRESAR
                </Button>
            </div>
        </CHPrivateContainer>
    );
};

export default connector(
    RealizacionSubsidios,
);