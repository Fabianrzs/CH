import React, {
    useCallback,
    useEffect,
    useState,
} from 'react';
import {
    Card,
    CardContent,
    CircularProgress,
    createStyles,
    Grid,
    makeStyles,
    Theme,
    Typography,
} from '@material-ui/core';
import { useSelector } from 'react-redux';

import ApiCredito from '../../services/api-credit.service';
import CatalogApi from '../../services/api-catalog.service';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            width: '100%',
        },
        card: {
            width: '100%',
        },
        title: {
            fontWeight: 600,
            marginBottom: theme.spacing(2),
        },
        address: {
            marginBottom: theme.spacing(3),
            lineHeight: 1.6,
        },
        mapContainer: {
            width: '100%',
            height: '400px',
            borderRadius: '6px',
            overflow: 'hidden',
        },
        iframe: {
            width: '100%',
            height: '100%',
            border: 0,
            display: 'block',
        },
        noData: {
            padding: theme.spacing(3),
            textAlign: 'center',
        },
        loading: {
            padding: theme.spacing(4),
            textAlign: 'center',
        },
        mapLink: {
            display: 'inline-block',
            marginBottom: theme.spacing(2),
            textDecoration: 'none',
        },
    }),
);

interface Props {
    procedureRequest: any;
    isLoading: boolean;
}

interface UbicacionData {
    direccion: string;
    ciudad: string;
    departamento: string;
}

const apiCredito = ApiCredito();
const catalogApi = CatalogApi();

/*
 * Catálogo de departamentos de Colombia.
 *
 * Se utiliza como respaldo cuando el catálogo
 * de departamentos no se encuentra cargado
 * en Redux.
 */
const DEPARTAMENTOS_COLOMBIA: {
    [key: number]: string;
} = {
    5: 'ANTIOQUIA',
    8: 'ATLÁNTICO',
    11: 'BOGOTÁ D.C.',
    13: 'BOLÍVAR',
    15: 'BOYACÁ',
    17: 'CALDAS',
    18: 'CAQUETÁ',
    19: 'CAUCA',
    20: 'CESAR',
    23: 'CÓRDOBA',
    25: 'CUNDINAMARCA',
    27: 'CHOCÓ',
    41: 'HUILA',
    44: 'LA GUAJIRA',
    47: 'MAGDALENA',
    50: 'META',
    52: 'NARIÑO',
    54: 'NORTE DE SANTANDER',
    63: 'QUINDÍO',
    66: 'RISARALDA',
    68: 'SANTANDER',
    70: 'SUCRE',
    73: 'TOLIMA',
    76: 'VALLE DEL CAUCA',
    81: 'ARAUCA',
    85: 'CASANARE',
    86: 'PUTUMAYO',
    88: 'ARCHIPIÉLAGO DE SAN ANDRÉS, PROVIDENCIA Y SANTA CATALINA',
    91: 'AMAZONAS',
    94: 'GUAINÍA',
    95: 'GUAVIARE',
    97: 'VAUPÉS',
    99: 'VICHADA',
};

const normalizarRespuesta = (
    response: any,
): any => {
    if (!response) {
        return {};
    }

    if (
        response.isSuccess !== undefined
    ) {
        return response;
    }

    if (
        response.data &&
        response.data.isSuccess !== undefined
    ) {
        return response.data;
    }

    return response.data || response || {};
};

const obtenerListaRespuesta = (
    response: any,
): any[] => {
    const respuesta =
        normalizarRespuesta(response);

    const lista =
        respuesta.data ||
        respuesta.result ||
        respuesta ||
        [];

    return Array.isArray(lista)
        ? lista
        : [];
};

const obtenerIdCatalogo = (
    item: any,
): any =>
    item?.id ??
    item?.idCiudad ??
    item?.ciudadId ??
    item?.idDepartamento ??
    item?.departamentoId ??
    item?.dpC_ID ??
    item?.dpD_ID ??
    item?.dpT_ID ??
    item?.value ??
    item?.codigo ??
    null;

const obtenerNombreCatalogo = (
    item: any,
): string =>
    String(
        item?.nombre ??
            item?.descripcion ??
            item?.label ??
            item?.nombreCiudad ??
            item?.ciudadNombre ??
            item?.nombreDepartamento ??
            item?.departamentoNombre ??
            '',
    );

const buscarItemPorId = (
    lista: any[],
    idBuscado: any,
): any =>
    lista.find(
        (item: any) =>
            Number(
                obtenerIdCatalogo(item),
            ) === Number(idBuscado),
    );

const Ubicacion: React.FC<Props> = ({
    procedureRequest,
    isLoading,
}) => {
    const classes = useStyles();

    const [loading, setLoading] =
        useState<boolean>(false);

    const [ubicacion, setUbicacion] =
        useState<UbicacionData>({
            direccion: '',
            ciudad: '',
            departamento: '',
        });

    /*
     * Catálogos existentes en Redux.
     */
    const catalogState = useSelector(
        (state: any) =>
            state?.catalog ||
            state?.catalogState ||
            {},
    );

    const departamentos =
        catalogState?.departments ||
        catalogState?.departamentos ||
        catalogState?.department ||
        [];

    /*
     * Número de identificación del cliente.
     */
    const clientIdentificationNumber =
        procedureRequest
            ?.clientIdentificationNumber ||
        procedureRequest?.numeroDocumento ||
        '';

    /*
     * Obtiene el nombre del departamento
     * utilizando el ID.
     *
     * Primero intenta Redux.
     * Si Redux no tiene el catálogo cargado,
     * utiliza el catálogo de respaldo.
     */
    const obtenerNombreDepartamento =
        useCallback(
            (
                departamentoId: number,
            ): string => {
                console.log(
                    '=====================================',
                );

                console.log(
                    'ID DEPARTAMENTO:',
                    departamentoId,
                );

                console.log(
                    'CATÁLOGO DEPARTAMENTOS REDUX:',
                    departamentos,
                );

                const departamentoEncontrado =
                    buscarItemPorId(
                        departamentos,
                        departamentoId,
                    );

                if (departamentoEncontrado) {
                    const nombre =
                        obtenerNombreCatalogo(
                            departamentoEncontrado,
                        );

                    console.log(
                        'DEPARTAMENTO ENCONTRADO EN REDUX:',
                        departamentoEncontrado,
                    );

                    console.log(
                        'NOMBRE DEPARTAMENTO:',
                        nombre,
                    );

                    return nombre;
                }

                const nombreDepartamento =
                    DEPARTAMENTOS_COLOMBIA[
                        departamentoId
                    ] || '';

                console.log(
                    'DEPARTAMENTO DESDE CATÁLOGO RESPALDO:',
                    nombreDepartamento,
                );

                return nombreDepartamento;
            },
            [departamentos],
        );

    /*
     * Consulta las ciudades pertenecientes
     * al departamento.
     */
    const obtenerCiudadesDepartamento =
        useCallback(
            async (
                departamentoId: number,
            ): Promise<any[]> => {
                if (!departamentoId) {
                    return [];
                }

                try {
                    console.log(
                        '=====================================',
                    );

                    console.log(
                        'CONSULTANDO CIUDADES DEL DEPARTAMENTO:',
                        departamentoId,
                    );

                    const response =
                        await catalogApi.getCitiesByDepartment(
                            departamentoId,
                        );

                    console.log(
                        'RESPUESTA getCitiesByDepartment:',
                        response,
                    );

                    const ciudades =
                        obtenerListaRespuesta(
                            response,
                        );

                    console.log(
                        'TOTAL CIUDADES:',
                        ciudades.length,
                    );

                    return ciudades;
                } catch (error) {
                    console.error(
                        'ERROR CONSULTANDO CIUDADES:',
                        error,
                    );

                    return [];
                }
            },
            [],
        );

    /*
     * Consulta principal.
     */
    const consultarUbicacion =
        useCallback(
            async (): Promise<void> => {
                if (
                    !clientIdentificationNumber
                ) {
                    console.log(
                        'No existe clientIdentificationNumber',
                    );

                    return;
                }

                try {
                    setLoading(true);

                    const tipoDocumentoId = 1;

                    console.log(
                        '=====================================',
                    );

                    console.log(
                        'CONSULTANDO UBICACIÓN DEL INMUEBLE',
                    );

                    console.log(
                        'TIPO DOCUMENTO:',
                        tipoDocumentoId,
                    );

                    console.log(
                        'NÚMERO DOCUMENTO:',
                        clientIdentificationNumber,
                    );

                    /*
                     * Consulta:
                     *
                     * ObtenerSolicitudCreditoTipoNumeroIdentificacion
                     */
                    const response: any =
                        await apiCredito.getObtenerSolicitudCreditoTipoNumeroIdentificacion(
                            tipoDocumentoId,
                            String(
                                clientIdentificationNumber,
                            ),
                        );

                    console.log(
                        'RESPUESTA SOLICITUD:',
                        response,
                    );

                    const responseNormalizada =
                        normalizarRespuesta(
                            response,
                        );

                    const datos =
                        responseNormalizada.data ||
                        responseNormalizada;

                    console.log(
                        'DATOS SOLICITUD:',
                        datos,
                    );

                    if (!datos) {
                        setUbicacion({
                            direccion: '',
                            ciudad: '',
                            departamento: '',
                        });

                        return;
                    }

                    /*
                     * Datos del inmueble.
                     */
                    const direccion =
                        datos
                            .soP_DIRECCION_INMUEBLE ||
                        '';

                    const ciudadId =
                        Number(
                            datos
                                .dpC_ID_INMUEBLE ||
                                0,
                        );

                    const departamentoId =
                        Number(
                            datos
                                .dpD_ID_INMUEBLE ||
                                0,
                        );

                    console.log(
                        '=====================================',
                    );

                    console.log(
                        'DATOS UBICACIÓN:',
                        {
                            direccion,
                            ciudadId,
                            departamentoId,
                        },
                    );

                    /*
                     * Obtener nombre departamento.
                     */
                    const nombreDepartamento =
                        obtenerNombreDepartamento(
                            departamentoId,
                        );

                    /*
                     * Obtener ciudades del departamento.
                     */
                    const ciudades =
                        await obtenerCiudadesDepartamento(
                            departamentoId,
                        );

                    console.log(
                        'BUSCANDO CIUDAD ID:',
                        ciudadId,
                    );

                    /*
                     * Tu respuesta real viene así:
                     *
                     * {
                     *   id: 843,
                     *   idDepartamento: 68,
                     *   codigo: 1,
                     *   nombre: "BUCARAMANGA"
                     * }
                     *
                     * Por eso se compara contra id.
                     */
                    const ciudadEncontrada =
                        ciudades.find(
                            (item: any) =>
                                Number(
                                    item?.id ??
                                        item
                                            ?.idCiudad ??
                                        item
                                            ?.ciudadId ??
                                        0,
                                ) ===
                                ciudadId,
                        );

                    console.log(
                        'CIUDAD ENCONTRADA:',
                        ciudadEncontrada,
                    );

                    const nombreCiudad =
                        obtenerNombreCatalogo(
                            ciudadEncontrada,
                        );

                    console.log(
                        '=====================================',
                    );

                    console.log(
                        'RESULTADO UBICACIÓN:',
                        {
                            direccion,
                            ciudad:
                                nombreCiudad,
                            departamento:
                                nombreDepartamento,
                        },
                    );

                    /*
                     * Guardamos nombres.
                     */
                    setUbicacion({
                        direccion,
                        ciudad:
                            nombreCiudad,
                        departamento:
                            nombreDepartamento,
                    });
                } catch (error) {
                    console.error(
                        'ERROR CONSULTANDO UBICACIÓN:',
                        error,
                    );

                    setUbicacion({
                        direccion: '',
                        ciudad: '',
                        departamento: '',
                    });
                } finally {
                    setLoading(false);
                }
            },
            [
                clientIdentificationNumber,
                obtenerCiudadesDepartamento,
                obtenerNombreDepartamento,
            ],
        );

    useEffect(() => {
        consultarUbicacion();
    }, [consultarUbicacion]);

    /*
     * =================================================
     * DIRECCIÓN COMPLETA PARA GOOGLE MAPS
     * =================================================
     *
     * Ejemplo:
     *
     * CL 11 A 14 48,
     * BUCARAMANGA,
     * SANTANDER,
     * Colombia
     */
    const direccionCompleta = [
        ubicacion.direccion,
        ubicacion.ciudad,
        ubicacion.departamento,
        'Colombia',
    ]
        .filter(Boolean)
        .join(', ');

    /*
     * =================================================
     * URL NORMAL DE GOOGLE MAPS
     * =================================================
     *
     * Quedará similar a:
     *
     * https://www.google.com/maps/place/
     * CL+11+A+14+48%2C+BUCARAMANGA%2C+SANTANDER%2C+Colombia
     *
     * Es el mismo formato /maps/place/
     * que quieres utilizar.
     */
    const direccionGoogleMaps =
        encodeURIComponent(
            direccionCompleta,
        ).replace(/%20/g, '+');

    const urlGoogleMaps =
        ubicacion.direccion
            ? `https://www.google.com/maps/place/${direccionGoogleMaps}`
            : '';

    /*
     * =================================================
     * URL PARA EL IFRAME
     * =================================================
     *
     * Para incrustar Google Maps usamos
     * output=embed.
     */
    const urlMapa =
        ubicacion.direccion
            ? `https://www.google.com/maps?q=${encodeURIComponent(
                  direccionCompleta,
              )}&output=embed`
            : '';

    console.log(
        '=====================================',
    );

    console.log(
        'DIRECCIÓN GOOGLE MAPS:',
        direccionCompleta,
    );

    console.log(
        'URL /MAPS/PLACE:',
        urlGoogleMaps,
    );

    console.log(
        'URL IFRAME:',
        urlMapa,
    );

    console.log(
        '=====================================',
    );

    if (
        isLoading ||
        loading
    ) {
        return (
            <Grid
                container
                spacing={2}
                className={
                    classes.root
                }
            >
                <Grid
                    item
                    xs={12}
                >
                    <Card
                        className={
                            classes.card
                        }
                    >
                        <CardContent
                            className={
                                classes.loading
                            }
                        >
                            <CircularProgress />

                            <Typography variant="body1">
                                Consultando ubicación...
                            </Typography>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
        );
    }

    return (
        <Grid
            container
            spacing={2}
            className={
                classes.root
            }
        >
            <Grid
                item
                xs={12}
            >
                <Card
                    className={
                        classes.card
                    }
                >
                    <CardContent>
                        <Typography
                            variant="h6"
                            className={
                                classes.title
                            }
                        >
                            Ubicación
                        </Typography>

                        {ubicacion.direccion ? (
                            <>
                                <Typography
                                    variant="body1"
                                    className={
                                        classes.address
                                    }
                                >
                                    <strong>
                                        Dirección:
                                    </strong>{' '}
                                    {
                                        ubicacion.direccion
                                    }

                                    <br />

                                    <strong>
                                        Ciudad:
                                    </strong>{' '}
                                    {ubicacion.ciudad ||
                                        'No encontrada'}

                                    <br />

                                    <strong>
                                        Departamento:
                                    </strong>{' '}
                                    {ubicacion.departamento ||
                                        'No encontrado'}
                                </Typography>

                                {urlGoogleMaps && (
                                    <a
                                        href={
                                            urlGoogleMaps
                                        }
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className={
                                            classes.mapLink
                                        }
                                    >
                                        Abrir en Google Maps
                                    </a>
                                )}

                                <div
                                    className={
                                        classes.mapContainer
                                    }
                                >
                                    <iframe
                                        title="Ubicación del inmueble"
                                        src={
                                            urlMapa
                                        }
                                        className={
                                            classes.iframe
                                        }
                                        allowFullScreen
                                    />
                                </div>
                            </>
                        ) : (
                            <Typography
                                variant="body1"
                                className={
                                    classes.noData
                                }
                            >
                                No se encontró una
                                dirección para mostrar.
                            </Typography>
                        )}
                    </CardContent>
                </Card>
            </Grid>
        </Grid>
    );
};

export default Ubicacion;