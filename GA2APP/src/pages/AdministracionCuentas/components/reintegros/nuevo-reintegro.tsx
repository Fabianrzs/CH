import {
    Button,
    Card,
    CardContent,
    Container,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
    Theme
} from '@material-ui/core';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { Controller, FieldError, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import AttachFileIcon from '@material-ui/icons/AttachFile';
import CHPageTitleV2 from '../../../../components/ch-page-title/CHPageTitleV2';
import CHPrivateContainer from '../../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import ApiCliente from '../../../../services/api-clients.service';
import ApiReintegro from '../../../../services/api-reintegros.service';
import CatalogApi from '../../../../services/api-catalog.service';
import { AfiliadoModel } from '../../../../models/requests/reintegros-request-request.model';
import { LoadingComponent } from '../../../../components/loading/loading.component';
import NotificationService from '../../../../services/notification.service';
import AplicarReintegro from './aplicar-reintegros';
import { TIPO_REINTEGRO } from './constants/reintegros-constants';
import ApiSolicitud from '../../../../services/api-solicitud.service';
import TituloIconoV2 from '../titulo-icono/tituloIcono.componentV2';


const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            backgroundColor: '#1C3F8B',
            color: '#FFFFFF',
        },
        wrapper: {
            margin: theme.spacing(1),
            position: 'relative',
        },
        buttonAgregarAfiliado: {
            width: '150px',
            marginLeft: '40%',
            backgroundColor: '#1C3F8B',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
            marginBottom: '5px',
            marginTop: '10px',
        },
        iconRed: {
            textAlign: 'inherit',
            color: '#FF0000',
        },
        iconoNumeroRadicado: {
            marginLeft: '6%',
        },
        campoRadicado: {
            marginLeft: '12%',
        },
        iconoDodoRadicado: {
            marginTop: '3%',
        },
        enlaceDodo: {
            cursor: 'pointer',
        }
    }),
);

function AfiliadosTable(
    {
        afiliados,
        tiposReintegros,
        eliminarAfiliado,
    }: {
        afiliados: AfiliadoModel[],
        tiposReintegros: any,
        eliminarAfiliado: (numeroDocumento: string) => void,
    }) {
    const classes = useStyles();

    const obtenerDescripcionReintegro = (idTipoReintegro: number) => {
        const reintegro = tiposReintegros.find((item: any) => item.idTipoReintegro === idTipoReintegro);
        return reintegro.descripcion;
    }

    return (
        <>
            <Grid item xs={12}>
                <Card>
                    <CardContent>
                        <table className="table"
                            style={{
                                margin: '20px',
                                width: '95%',
                            }}>
                            <thead>
                                <tr>
                                    <th>Número Documento</th>
                                    <th>Nombres</th>
                                    <th>Apellidos</th>
                                    <th>Tipo Reintegro</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                {afiliados?.map((row, index) => (
                                    <tr key={`table-${index}`}>
                                        <td data-label="numeroDocumento" className="tab-space-01">{row.numeroDocumento}</td>
                                        <td data-label="Nombres" className="tab-space-01">{row.nombres}</td>
                                        <td data-label="Apellidos" className="tab-space-01"> {row.apellidos}</td>
                                        <td data-label="tipoReintegro" className="tab-space-01">{obtenerDescripcionReintegro(row.id_Tipo_Reintegro)}</td>
                                        <td data-label="acciones" className="tab-space-01 actions">
                                            <Grid>
                                                <span aria-label="Eliminar"
                                                    onClick={() => eliminarAfiliado(row.numeroDocumento)}
                                                >
                                                    <span className={`ico fch-Eliminar-0-1 icoMid-2 ${classes.iconRed}`} /></span>
                                            </Grid>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>

                    </CardContent>
                </Card>
            </Grid>
        </>
    );
}

const NuevoReintegro = (props: propsType) => {
    const classes = useStyles();
    const apiCliente = ApiCliente();
    const apiReintegro = ApiReintegro();
    const apiSolicitud = ApiSolicitud();
    const catalogApi = CatalogApi();
    const { aplicarReintegro,
        formularioInformacionSolicitud,
        loadingInformacionSolicitud,
        numeroRadicadoRespuesta,
        setAplicarReintegro,
        setAgregarNuevoReintegro,
        obtenerListadoReintegros,
    } = props;
    const [tiposReintegros, setTiposReintegros] = useState<any[]>([]);
    const [afiliadosModel, setAfiliadosModel] = useState<AfiliadoModel[]>([]);
    const [unidadesEjecutoras, setUnidadesEjecutoras] = useState<any[]>([]);
    const [listaUnidadesEjecutoras, setListaUnidadesEjecutoras] = useState<any[]>([]);
    const [informacionReintegro, setInformacionReintegro] = useState(null);
    const [linkDodo, setLinkDodo] = useState('');
    const [loading, setLoading] = useState(false);
    const valor = '95ca1858-b89d-4631-9893-22932c925184';

    const formularioNuevoReintegro = useForm({
        defaultValues: {
            idReintegro: '',
            numeroRadicado: '',
            idUnidadEjecutora: 0,
            nitUnidadEjecutora: '',
            nombreUnidadEjecutora: '',
            tipoReintegro: '',
            numeroDocumento: '',
            observacion: '',
        }
    })

    useEffect(() => {
        obtenerTiposReintegros();
        obtenerUnidadesEjecutoras();
    }, [])

    const obtenerUnidadesEjecutoras = async () => {
        await catalogApi
            .getAllEXECUTINGUNITSRequest()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setUnidadesEjecutoras(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error unidades ejecutoras nuevo Reintegro', { type: toast.TYPE.ERROR });
            });
    }

    const obtenerUnidadEjecutoraPorIdentificacion = async (identificacion: string) => {
        const arrayUnidadesEjecutoras = unidadesEjecutoras.filter(item => item.identificacion === identificacion);
        setListaUnidadesEjecutoras(arrayUnidadesEjecutoras);
        if (arrayUnidadesEjecutoras.length === 1) {
            const unidadEjecutoraSeleccionada = unidadesEjecutoras.find(item => item.identificacion === identificacion);
            formularioNuevoReintegro.setValue('idUnidadEjecutora', unidadEjecutoraSeleccionada?.id || 0);
            formularioNuevoReintegro.setValue('nombreUnidadEjecutora', unidadEjecutoraSeleccionada?.nombre || '');
        }
    }

    const tieneNumerosSeparadosPorComas = (observacion: string) => {
        const patron = /^\d+(,\d+)*$/;
        let numerosSeparadosPorComas: boolean;

        if (patron.test(observacion)) {
            numerosSeparadosPorComas = true;
        } else {
            numerosSeparadosPorComas = false;
        }
        return numerosSeparadosPorComas;
    }

    const obtenerTiposReintegros = async () => {
        await apiReintegro
            .getTipoReintegro()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setTiposReintegros(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });

                    }
                }
            })
            .catch(() => {
                toast('Error Tipo de Reintegros', { type: toast.TYPE.ERROR });

            });
    }

    const obtenerUrlDodo = async () => {
        const radicado = formularioNuevoReintegro.getValues('numeroRadicado');

        if (radicado === '') {
            toast('Debe diligenciar el Número de Radicado', { type: toast.TYPE.ERROR });
            return;
        }

        if (radicado) {
            await apiSolicitud
                .GetBoteneEnlaceDodo(String(radicado))
                .then(async respuesta => {
                    if (respuesta.data) {
                        setLinkDodo(respuesta.data);
                    } else {
                        setLinkDodo('');
                        toast('Número de Radicado no encontrado en dodo', { type: toast.TYPE.ERROR });
                    }
                })
                .catch(() => {
                    toast('Error del sistema al obtener url de Dodo', { type: toast.TYPE.ERROR });
                });
        }
    }

    const obtenerUnidadEjecutoraPorRadicado = async (radicado: string) => {
        if (radicado === '') {
            toast(`Ingrese un número de radicado para realizar la consulta`, { type: toast.TYPE.ERROR });
            return;
        }

        const patron = /^446-01-20/;

        if (patron.test(radicado)) {
            await apiReintegro
                .postConsultarUnidadEjecutoraPorRadicado(radicado)
                .then(async (response: any) => {
                    if (response.data) {
                        if (response?.data?.unidadEjecutora) {

                            let unidadEjecutora;
                            if (response?.data?.unidadEjecutora === 'JUSTICIA PENAL MILITAR') {
                                unidadEjecutora = unidadesEjecutoras.find(item => item?.id === 21);
                            }

                            if (response?.data?.unidadEjecutora === 'FUERZA AEREA DE COLOMBIA') {
                                unidadEjecutora = unidadesEjecutoras.find(item => item?.id === 11);
                            }

                            if (response?.data?.unidadEjecutora === 'DIRECCION CENTRO DE REHABILITACION INCLUSIVA') {
                                unidadEjecutora = unidadesEjecutoras.find(item => item?.id === 24);
                            }

                            if (response?.data?.unidadEjecutora !== 'JUSTICIA PENAL MILITAR' &&
                                response?.data?.unidadEjecutora !== 'FUERZA AEREA DE COLOMBIA' &&
                                response?.data?.unidadEjecutora !== 'DIRECCION CENTRO DE REHABILITACION INCLUSIVA') {
                                unidadEjecutora = unidadesEjecutoras.find(item => item?.nombre.normalize("NFD").replace(/[\u0300-\u036f]/g, "") === response?.data?.unidadEjecutora);
                            }

                            if (unidadEjecutora) {
                                formularioNuevoReintegro.setValue('idUnidadEjecutora', unidadEjecutora?.id);
                                formularioNuevoReintegro.setValue('nombreUnidadEjecutora', unidadEjecutora?.nombre);
                                formularioNuevoReintegro.setValue('nitUnidadEjecutora', unidadEjecutora?.identificacion);
                                setListaUnidadesEjecutoras([unidadEjecutora]);
                            }

                            if (response?.data?.identificacion) {
                                formularioNuevoReintegro.setValue('numeroDocumento', response?.data?.identificacion);
                            }

                        } else if (response?.data?.unidadEjecutora === null && response?.data?.identificacion === null) {
                            formularioNuevoReintegro.setValue('idUnidadEjecutora', 0);
                            formularioNuevoReintegro.setValue('nombreUnidadEjecutora', '');
                            formularioNuevoReintegro.setValue('nitUnidadEjecutora', '');
                            toast(`Número de Radicado No Encontrado`, { type: toast.TYPE.ERROR });
                        }

                        // if(response.data.identificacion === null && response.data.observaciones === null){
                        //     toast(`Número de Radicado No Encontrado`,{ type: toast.TYPE.ERROR });
                        //     formularioNuevoReintegro.setValue('idUnidadEjecutora',0);
                        //     formularioNuevoReintegro.setValue('nombreUnidadEjecutora','');
                        //     formularioNuevoReintegro.setValue('nitUnidadEjecutora','');
                        // }

                        // if(response.data.identificacion !== null){
                        //     formularioNuevoReintegro.setValue('nitUnidadEjecutora',response.data.identificacion);
                        //     const arrayUnidadesEjecutoras = unidadesEjecutoras.filter(item => item.identificacion === response?.data?.identificacion);
                        //     setListaUnidadesEjecutoras(arrayUnidadesEjecutoras);
                        //     if(arrayUnidadesEjecutoras.length === 1){
                        //         const unidadEjecutoraSeleccionada = unidadesEjecutoras.find(item => item.identificacion === response?.data?.identificacion);
                        //         formularioNuevoReintegro.setValue('idUnidadEjecutora',unidadEjecutoraSeleccionada?.id || 0);
                        //         formularioNuevoReintegro.setValue('nombreUnidadEjecutora',unidadEjecutoraSeleccionada?.nombre || '');
                        //     }else if(arrayUnidadesEjecutoras.length === 0){
                        //         toast(`La Unidad Ejecutora no se encuentra registrada`, { type: toast.TYPE.ERROR });
                        //     }
                        // }

                        // if(response.data.observaciones !== null && tieneNumerosSeparadosPorComas(response.data.observaciones)){
                        //     formularioNuevoReintegro.setValue('numeroDocumento',response?.data?.observaciones);
                        // }else if(response.data.observaciones !== null && tieneNumerosSeparadosPorComas(response.data.observaciones) === false){
                        //     toast(`No hay números de documentos registrados`,{ type: toast.TYPE.ERROR });
                        //     formularioNuevoReintegro.setValue('numeroDocumento','');
                        // }                
                    }
                })
                .catch((e) => {
                    formularioNuevoReintegro.setValue('idUnidadEjecutora', 0);
                    formularioNuevoReintegro.setValue('nombreUnidadEjecutora', '');
                    formularioNuevoReintegro.setValue('nitUnidadEjecutora', '');
                    toast(`Error al consultar Unidad Ejecutora por Radicado ${e.message}`, { type: toast.TYPE.ERROR });
                });
        } else {
            toast(`El Número de Radicado debe iniciar en 446-01-20`, { type: toast.TYPE.ERROR });
        }
    }

    const onSubmit = (data: any) => {
        if (afiliadosModel.length === 0) {
            toast(`Se debe registrar al menos un afiliado`, { type: toast.TYPE.ERROR });
            return;
        }

        const request = {
            numero_Radicado: data.numeroRadicado,
            id_Unidad_Ejecutora: data.idUnidadEjecutora,
            unidad_Ejecutora: data.nombreUnidadEjecutora,
            id_Tipo_Reintegro: Number(data.tipoReintegro),
            observacion: data.observacion,
            afiliados: afiliadosModel,
        }
        crearNuevoReintegro(request);
    }

    const crearNuevoReintegro = async (request: any) => {
        setLoading(true);

        await apiReintegro
            .postCrearReintegro(request)
            .then(async response => {

                if (response?.returnMessage === 'No se pudo guardar ningun afiliado, valide por favor si no se encuentra registrados') {
                    setLoading(false);
                    NotificationService.genericAlert(
                        'Atención',
                        'No se puede guardar la información. Número de cédula ya se encuentra registrado',
                        'warning',
                    );
                } else if (response?.data === null &&
                    response?.isSuccess === false) {
                    setLoading(false);
                    NotificationService.genericAlert(
                        'Atención',
                        'No es posible crear Reintegro. Tiene Reintegros que no se encuentran en estado Procesado',
                        'warning',
                    );
                }
                else if (response?.data) {
                    formularioInformacionSolicitud.setValue('numeroRadicado', response?.data.numero_Radicado);
                    formularioInformacionSolicitud.setValue('fechaInicio', moment(response?.data?.fechaCreacion).format('YYYY-MM-DD h:mmA'));
                    const fecha = response?.data?.afiliados[0].fechaFin;
                    if (fecha === '0001-01-01T00:00:00') {
                        formularioInformacionSolicitud.setValue('fechaFinal', '');
                    } else {
                        formularioInformacionSolicitud.setValue('fechaFinal', moment(fecha).format('YYYY-MM-DD h:mmA'));
                    }
                    setInformacionReintegro(response?.data);
                    const datosComunicaciones = {
                        radicado: response?.data?.numero_Radicado,
                        idReintegro: response?.data?.id_Reintegro,
                    }
                    apiReintegro.postInsertarComunicacionesReintegros(datosComunicaciones)
                        .then(async response => {
                            if (response.returnMessage
                                && String(response.returnMessage).includes('Formulario guardado exitoamente ')) {
                                const radicadoRespuesta = String(response.returnMessage.replace('Formulario guardado exitoamente ', ''));
                                formularioInformacionSolicitud.setValue('numeroRadicadoRespuesta', radicadoRespuesta);
                            }
                            setLoading(false);
                            setAplicarReintegro(true);
                            if (String(response.returnMessage).includes('Por favor verifique algunos afiliados ya se encuentra registrados')) {
                                NotificationService.genericAlert(
                                    'Atención',
                                    'No es posible crear algunos afiliados debido a que ya se encuentran registrados. Por favor verificar',
                                    'warning',
                                );
                            } else {
                                await NotificationService.genericAlert(
                                    'Guardado',
                                    'Se ha guardado correctamente la información',
                                );
                            }
                        }).catch(() => {
                            setLoading(false);
                            toast(`Error al insertar comunicaciones flujo 03`, { type: toast.TYPE.ERROR });
                        })
                }
            })
            .catch(() => {
                setLoading(false);
                toast('Error al crear reintegro', { type: toast.TYPE.ERROR });
            });
    }

    const agregarAfiliado = async (data: any) => {

        setLoading(true);

        let existeRadicado = false;
        const busqueda = {
            numeroRadicado: String(data.numeroRadicado),
            idUnidadEjecutora: 0,
            idTipoReintegro: 0,
        }

        await apiReintegro
            .getConsultarListaReintegros(busqueda)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null) {
                        existeRadicado = true;
                    } else if (response.data === null) {
                        existeRadicado = false;
                    }
                }
            })
            .catch((e) => {
                toast(`Error consultar radicado al crear solicitud ${e.message}`, { type: toast.TYPE.ERROR });
            });

        if (existeRadicado) {
            setLoading(false);
            toast("No se puede agregar afiliados. El número de radicado ya existe", { type: toast.TYPE.ERROR });
            return;
        }

        const text = data?.numeroDocumento;
        if (text.slice(-1) === ',') {
            setLoading(false);
            toast(`El campo no puede terminar con el carácter ','`, { type: toast.TYPE.ERROR });
            return;
        }

        const arrayAfiliados = data?.numeroDocumento.split(',');
        const CEDULA_CIUDADANIA = 1;
        const duplicados: any[] = [];

        for (let i = 0; i < arrayAfiliados.length; i += 1) {
            for (let j = i + 1; j < arrayAfiliados.length; j += 1) {
                if (arrayAfiliados[i] === arrayAfiliados[j]) {
                    duplicados.push(arrayAfiliados[i]);
                }
            }
        }

        let arrayDuplicados: string[] = [];
        if (duplicados.length > 0) {
            const array: Set<string> = new Set(duplicados);
            arrayDuplicados = Array.from(array);
        }

        if (arrayDuplicados.length === 1) {
            setLoading(false);
            toast(`La cédula ${arrayDuplicados.toString()} se encuentra repetida`, { type: toast.TYPE.ERROR });
            return;
        }

        if (arrayDuplicados.length > 1) {
            setLoading(false);
            let cedulas = '';
            arrayDuplicados.reverse().forEach((cedula) => {
                cedulas += `${cedula} <br />`;
            });
            
            toast.info(
                <div>
                    <div>Las cédulas:</div>
                    <div>{cedulas}</div>
                    <br />
                    <div>se encuentran repetidas.</div>
                </div>
            );
            return;
        }

        if (afiliadosModel) {
            const duplicadosEnTabla: any[] = [];
            const duplicadosCedulasEnTabla: any[] = [];
            for (let i = 0; i < arrayAfiliados.length; i += 1) {
                for (let j = 0; j < afiliadosModel.length; j += 1) {
                    if (arrayAfiliados[i] === afiliadosModel[j].numeroDocumento &&
                        data.tipoReintegro === afiliadosModel[j].id_Tipo_Reintegro) {
                        duplicadosEnTabla.push(arrayAfiliados[i]);
                    }

                    // Validación cedulas inconsistentes
                    if (arrayAfiliados[i] === afiliadosModel[j].numeroDocumento &&
                        afiliadosModel[j].id_Tipo_Reintegro === TIPO_REINTEGRO.INCONSISTENTES) {
                        duplicadosCedulasEnTabla.push(arrayAfiliados[i]);
                    }
                }
            }

            if (duplicadosEnTabla.length === 1) {
                setLoading(false);
                toast(`La cédula ${duplicadosEnTabla.toString()} no se puede agregar con el mismo tipo de reintegro`, { type: toast.TYPE.ERROR });
                return;
            }

            if (duplicadosEnTabla.length > 1) {
                setLoading(false);
                let cedulas = '';
                duplicadosEnTabla.reverse().forEach((cedula) => {
                    cedulas += `${cedula} <br />`;
                });
                
                toast.info(
                    <div>
                        <div>Las cédulas:</div>
                        <div>{cedulas}</div>
                        <br />
                        <div>no se pueden agregar con el mismo tipo de reintegro.</div>
                    </div>
                );
                return;
            }

            // Validación cedulas inconsistentes
            if (duplicadosCedulasEnTabla.length === 1) {
                setLoading(false);
                toast(`La cédula ${duplicadosCedulasEnTabla.toString()} ya existe en la tabla de afiliados de Tipo Inconsistentes`, { type: toast.TYPE.ERROR });
                return;
            }

            if (duplicadosCedulasEnTabla.length > 1) {
                setLoading(false);
                let cedulas = '';
                duplicadosCedulasEnTabla.reverse().forEach((cedula) => {
                    cedulas += `${cedula} <br />`;
                });
                
                toast.info(
                    <div>
                        <div>Las cédulas:</div>
                        <div>{cedulas}</div>
                        <br />
                        <div>ya existen en la tabla de afiliados de Tipo Inconsistentes.</div>
                    </div>
                );
                return;
            }
        }

        if (arrayAfiliados[0].trim() === "") {
            setLoading(false);
            toast(`Ingrese el campo número de documento`, { type: toast.TYPE.ERROR });
            return;
        }

        if (arrayAfiliados) {
            let cantidadDocumentos = 0;
            const cedulasNoEncontradasEnSistema: any[] = [];
            const cedulasNoCorrespondenUnidadEjecutora: any[] = [];
            const cedulasSinBloqueoParaDesbloquear: any[] = [];
            const cedulasSaldoDisponibleEnCero: any[] = [];
            const cedulasTienenEmbargo: any[] = [];
            const cedulasSolicitudesPendientes: any[] = [];
            const cedulasEstadoAfiliadoBloqueado: any[] = [];
            const cedulasDevolucionSeisMeses: any[] = [];
            const afiliadosParaCrearReintegro: any[] = [];

            await arrayAfiliados.forEach(async (item: any) => {

                await apiCliente
                    .getClientByIdentificationTypeIdentificationNumberRequest(
                        CEDULA_CIUDADANIA,
                        item
                    )
                    .then(async response => {

                        if (response?.data) {

                            // Verificar si todos los saldos de las cuentas estan en cero
                            let totalSaldoDisponible = 0;
                            let totalEmbargos = 0;
                            let totalBloqueosEmbargos = 0;
                            let valorSolicitado = 0;
                            let tieneEmbargos = false;
                            let saldoDisponibleEnCero = false;
                            let tieneSolicitudesPendientes = false;
                            let tienePagosUltimosSeisMeses = false;

                            if (response?.data?.cuentas?.length > 0) {
                                response?.data?.cuentas.forEach((cuenta: any) => {
                                    const movimientos = response?.data?.cuentas.filter((item: any) => item.numeroCuenta === cuenta.numeroCuenta);
                                    if (movimientos !== undefined && movimientos.length > 0) {
                                        movimientos?.forEach((item: any) => {
                                            if (item.valorBloqueoEmbargo > 0) {
                                                valorSolicitado += item.valorBloqueoEmbargo;
                                            }
                                        })
                                    }

                                    if (cuenta.conceptosSaldosBloqueos.length > 0) {
                                        cuenta.conceptosSaldosBloqueos?.forEach((item: any) => {
                                            totalSaldoDisponible += item.saldoConceptoCuenta;
                                        })
                                    }

                                    if (cuenta.conceptosEmbargosBloqueos.length > 0) {
                                        cuenta.conceptosEmbargosBloqueos?.forEach((item: any) => {
                                            totalBloqueosEmbargos += item.valorAcumulado;
                                        })
                                    }

                                    if (cuenta.embargos.length > 0) {
                                        cuenta.embargos?.forEach((elementEmbargo: any) => {
                                            totalEmbargos += elementEmbargo.tipoEmbargoId === valor ? elementEmbargo.valor : (cuenta.conceptos?.find((x: any) => x.id === elementEmbargo.conceptoId).saldo) * (elementEmbargo.valor / 100)
                                        });
                                    }
                                })

                                totalSaldoDisponible -= totalEmbargos;
                                totalEmbargos += totalBloqueosEmbargos;
                                totalSaldoDisponible -= totalBloqueosEmbargos;
                                totalSaldoDisponible -= valorSolicitado;

                                if (totalSaldoDisponible <= 0 && totalEmbargos > 0) {
                                    tieneEmbargos = true;
                                } else if (totalSaldoDisponible <= 0) {
                                    saldoDisponibleEnCero = true;
                                }
                            }

                            // Validacion si corresponde a la misma unidad ejecutora
                            if (response?.data?.idUnidadEjecutora === data.idUnidadEjecutora) {

                                // Validación si existe bloqueo para ser desbloqueado
                                if (data.tipoReintegro === TIPO_REINTEGRO.DESBLOQUEOS) {
                                    let sinBloqueo = false;
                                    const request = {
                                        numeroDocumento: response?.data?.identificacion,
                                        tipoDocumento: response?.data?.idTipoIdentificacion,
                                    }
                                    await apiReintegro
                                        .getConsultarBloqueoReintegro(request)
                                        .then(async (responseBloqueo: any) => {
                                            if (responseBloqueo.data === null || (responseBloqueo?.data && responseBloqueo?.data.length === 0)) {
                                                sinBloqueo = true;
                                            }
                                        })
                                        .catch(() => {
                                            toast('Error al consultar bloqueo por Reintegro', { type: toast.TYPE.ERROR });

                                        });

                                    if (sinBloqueo) {
                                        cedulasSinBloqueoParaDesbloquear.push(response?.data?.identificacion);
                                    }
                                }

                                if (response?.data?.solicitudes.length > 0) {
                                    // Validación solicitudes en estado pendiente
                                    response?.data?.solicitudes.forEach((solicitud: any) => {
                                        if (solicitud.estadoSolicitud === 'PENDIENTE' &&
                                            (solicitud.tipoSolicitud === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS' ||
                                                solicitud.tipoSolicitud === 'DESAFILIACIÓN' ||
                                                solicitud.tipoSolicitud === 'VERIFICACION DE DERECHOS Y ABONOS DE INTERESES' ||
                                                solicitud.tipoSolicitud === 'DESAFILIACIÓN SÓLO APORTES' ||
                                                solicitud.tipoSolicitud === 'DESAFILIACIÓN CON CESANTÍAS' ||
                                                solicitud.tipoSolicitud === 'DESAFILIACIONES' ||
                                                solicitud.tipoSolicitud === 'CRÉDITO HIPOTECARIO' ||
                                                solicitud.tipoSolicitud === 'SOLUCIÓN DE VIVIENDA' ||
                                                solicitud.tipoSolicitud === 'DEVOLUCIÓN PARCIAL DE CESANTÍAS' ||
                                                solicitud.tipoSolicitud === 'RETIRO DEFINITIVO DE LA INSTITUCIÓN' ||
                                                solicitud.tipoSolicitud === 'SOLICITUD DE SUBSIDIO' ||
                                                solicitud.tipoSolicitud === 'ABONOS DE INTERESES' ||
                                                solicitud.tipoSolicitud === 'POSTULACIÓN FONDO DE SOLIDARIDAD' ||
                                                solicitud.tipoSolicitud === 'RETIRO PARCIAL DE CESANTÍAS')) {
                                            tieneSolicitudesPendientes = true;
                                        }
                                    })

                                    // Validación Reintegros devolución de recursos dentro de los ultimos 6 meses
                                    const fechaActual = new Date();
                                    const seisMesesAtras = new Date();
                                    seisMesesAtras.setMonth(seisMesesAtras.getMonth() - 6);
                                    response?.data?.solicitudes.forEach((solicitud: any) => {
                                        if (solicitud.tipoSolicitud === 'REINTEGROS UNIDADES EJECUTORAS' &&
                                            solicitud.modalidad === 'DEVOLUCIÓN DE RECURSOS') {
                                            if (new Date(solicitud.fechaFinSolicitud).getTime() >= seisMesesAtras.getTime()
                                                && new Date(solicitud.fechaFinSolicitud).getTime() <= fechaActual.getTime()) {
                                                tienePagosUltimosSeisMeses = true;
                                            }
                                        }
                                    })
                                }

                                // Validación afiliados en estado Bloqueado
                                if (response?.data?.estadoCliente === 'BLOQUEADO') {
                                    cedulasEstadoAfiliadoBloqueado.push(response?.data?.identificacion);
                                }

                                if (saldoDisponibleEnCero) {
                                    cedulasSaldoDisponibleEnCero.push(response?.data?.identificacion);
                                }

                                if (tieneEmbargos) {
                                    cedulasTienenEmbargo.push(response?.data?.identificacion);
                                }

                                if (tieneSolicitudesPendientes) {
                                    cedulasSolicitudesPendientes.push(response?.data?.identificacion);
                                }

                                if (tienePagosUltimosSeisMeses) {
                                    cedulasDevolucionSeisMeses.push(response?.data?.identificacion);
                                }

                                let noCumpleCondiciones = false;

                                if (cedulasNoCorrespondenUnidadEjecutora && cedulasNoCorrespondenUnidadEjecutora.some(documento => documento === response?.data?.identificacion === true)) {
                                    noCumpleCondiciones = true;
                                }

                                if (cedulasSaldoDisponibleEnCero && cedulasSaldoDisponibleEnCero.some(documento => documento === response?.data?.identificacion === true)) {
                                    noCumpleCondiciones = true;
                                }

                                if (cedulasSinBloqueoParaDesbloquear && cedulasSinBloqueoParaDesbloquear.some(documento => documento === response?.data?.identificacion === true)) {
                                    noCumpleCondiciones = true;
                                }

                                if (noCumpleCondiciones === false || saldoDisponibleEnCero) {
                                    const afiliadoNuevo: AfiliadoModel = {
                                        tipoDocumentoId: response?.data?.idTipoIdentificacion,
                                        numeroDocumento: response?.data?.identificacion,
                                        nombres: `${response?.data?.primerNombre} ${response?.data?.segundoNombre}`,
                                        apellidos: `${response?.data?.primerApellido} ${response?.data?.segundoApellido}`,
                                        id_Tipo_Reintegro: tieneEmbargos || saldoDisponibleEnCero ? TIPO_REINTEGRO.INCONSISTENTES : data.tipoReintegro,
                                    }

                                    afiliadosParaCrearReintegro.push(afiliadoNuevo);
                                }
                            } else {
                                cedulasNoCorrespondenUnidadEjecutora.push(item);
                            }
                        }
                        else {
                            const afiliadoNuevo: AfiliadoModel = {
                                tipoDocumentoId: "0",
                                numeroDocumento: item,
                                nombres: '',
                                apellidos: '',
                                id_Tipo_Reintegro: TIPO_REINTEGRO.INCONSISTENTES,
                            }

                            afiliadosParaCrearReintegro.push(afiliadoNuevo);
                            cedulasNoEncontradasEnSistema.push(item);
                        }
                    })
                    .catch((e) => {
                        setLoading(false);
                        toast(`Error al consultar información del afiliado ${e.message}`, { type: toast.TYPE.ERROR });
                    });

                cantidadDocumentos += 1;
                if (cantidadDocumentos === arrayAfiliados.length) {

                    let mensaje = '';

                    if (cedulasNoEncontradasEnSistema.length === 1) {
                        mensaje = `La cédula ${cedulasNoEncontradasEnSistema.reverse().toString()} no se encuentra en el sistema.<br />`;
                    }

                    if (cedulasNoCorrespondenUnidadEjecutora.length === 1) {
                        mensaje += `La cédula ${cedulasNoCorrespondenUnidadEjecutora.reverse().toString()} no corresponde la unidad ejecutora.<br /><br />`;
                    }

                    if (cedulasSaldoDisponibleEnCero.length === 1) {
                        mensaje += `La cédula ${cedulasSaldoDisponibleEnCero.reverse().toString()} no tiene saldo disponible.<br /><br />`;
                    }

                    if (cedulasNoEncontradasEnSistema.length > 1) {
                        let cedulas = '';
                        cedulasNoEncontradasEnSistema.reverse().forEach((cedula) => {
                            cedulas += `${cedula} <br />`;
                        });
                        mensaje += `Las cédulas: <br /> ${cedulas} no se encuentran en el sistema. <br /><br />`;
                    }

                    if (cedulasNoCorrespondenUnidadEjecutora.length > 1) {
                        let cedulas = '';
                        cedulasNoCorrespondenUnidadEjecutora.reverse().forEach((cedula) => {
                            cedulas += `${cedula} <br />`;
                        });
                        mensaje += `Las cédulas: <br /> ${cedulas} no corresponden a la unidad ejecutora. <br /><br />`;
                    }

                    if (cedulasSinBloqueoParaDesbloquear.length === 1) {
                        mensaje += `La cédula ${cedulasSinBloqueoParaDesbloquear.reverse().toString()} no tiene bloqueos.<br /><br />`;
                    }

                    if (cedulasTienenEmbargo.length === 1) {
                        mensaje += `La cédula ${cedulasTienenEmbargo.reverse().toString()} se encuentra embargada y tienen saldo disponible en cero. Debe generarse la actividad en Embargos.<br /><br />`;
                    }

                    if (cedulasSolicitudesPendientes.length === 1) {
                        mensaje += `La cédula ${cedulasSolicitudesPendientes.reverse().toString()} tiene solicitudes en trámite.<br /><br />`;
                    }

                    if (cedulasDevolucionSeisMeses.length === 1) {
                        mensaje += `La cédula ${cedulasDevolucionSeisMeses.reverse().toString()} registra pagos por Devolución de Recursos en los últimos 6 meses.<br /><br />`;
                    }

                    if (cedulasEstadoAfiliadoBloqueado.length === 1) {
                        mensaje += `La cédula ${cedulasEstadoAfiliadoBloqueado.reverse().toString()} se encuentra en estado BLOQUEADO.<br /><br />`;
                    }

                    if (cedulasSinBloqueoParaDesbloquear.length > 1) {
                        let cedulas = '';
                        cedulasSinBloqueoParaDesbloquear.reverse().forEach((cedula) => {
                            cedulas += `${cedula} <br />`;
                        });
                        mensaje += `Las cédulas: <br /> ${cedulas} no tienen bloqueos. <br /><br />`;
                    }

                    if (cedulasSaldoDisponibleEnCero.length > 1) {
                        let cedulas = '';
                        cedulasSaldoDisponibleEnCero.reverse().forEach((cedula) => {
                            cedulas += `${cedula} <br />`;
                        });
                        mensaje += `Las cédulas: <br /> ${cedulas} no tienen saldo disponible. <br /><br />`;
                    }

                    if (cedulasTienenEmbargo.length > 1) {
                        let cedulas = '';
                        cedulasTienenEmbargo.reverse().forEach((cedula) => {
                            cedulas += `${cedula} <br />`;
                        });
                        mensaje += `Las cédulas: <br /> ${cedulas} se encuentran embargadas y tienen saldo disponible en cero. Debe generarse la actividad en Embargos. <br /><br />`;
                    }

                    if (cedulasSolicitudesPendientes.length > 1) {
                        let cedulas = '';
                        cedulasSolicitudesPendientes.reverse().forEach((cedula) => {
                            cedulas += `${cedula} <br />`;
                        });
                        mensaje += `Las cédulas: <br /> ${cedulas} tienen solicitudes en trámite. <br /><br />`;
                    }

                    if (cedulasDevolucionSeisMeses.length > 1) {
                        let cedulas = '';
                        cedulasDevolucionSeisMeses.reverse().forEach((cedula) => {
                            cedulas += `${cedula} <br />`;
                        });
                        mensaje += `Las cédulas: <br /> ${cedulas} registran pagos por Devolución de Recursos en los últimos 6 meses. <br /><br />`;
                    }

                    if (cedulasEstadoAfiliadoBloqueado.length > 1) {
                        let cedulas = '';
                        cedulasEstadoAfiliadoBloqueado.reverse().forEach((cedula) => {
                            cedulas += `${cedula} <br />`;
                        });
                        mensaje += `Las cédulas: <br /> ${cedulas} se encuentran en estado BLOQUEADO. <br /><br />`;
                    }

                    if (mensaje) {
                        toast.info(mensaje, { type: toast.TYPE.ERROR });
                    }

                    if (afiliadosParaCrearReintegro.length > 0) {
                        setAfiliadosModel((oldArray) => [...oldArray, ...afiliadosParaCrearReintegro]);
                        let cantidadRegistros = 0;
                        afiliadosParaCrearReintegro.forEach((afiliado: any) => {
                            const index = arrayAfiliados.indexOf(afiliado.numeroDocumento);
                            if (index > -1) {
                                arrayAfiliados.splice(index, 1);
                            }
                            cantidadRegistros += 1;
                            if (cantidadRegistros === afiliadosParaCrearReintegro.length) {
                                formularioNuevoReintegro.setValue('numeroDocumento', arrayAfiliados.toString());
                            }
                        })
                    }
                    setLoading(false);
                }
            })
        }

    }

    const eliminarAfiliado = async (numeroDocumento: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );

        if (confirmAlert) {
            const afiliados = afiliadosModel;
            afiliados.splice(
                afiliados.findIndex(item => item.numeroDocumento === numeroDocumento),
                1,
            );

            setAfiliadosModel(oldArray => [
                ...afiliados,
            ]
            );
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
        }
    }

    const validarErrorCampoRadicado = (error: FieldError | undefined) => {
        let mensaje = null;
        if (error !== undefined && error.type === 'required') {
            mensaje = 'Este campo es requerido.';
        } else if (error !== undefined && error.type === 'pattern') {
            mensaje = 'El Número de Radicado debe iniciar en 446-01-20';
        }
        return mensaje;
    }

    const onClickCancelar = () => {
        setAgregarNuevoReintegro(false);
        setAplicarReintegro(false);
    }

    if (aplicarReintegro && informacionReintegro) {
        return <AplicarReintegro
            informacionReintegro={informacionReintegro}
            numeroRadicadoRespuesta={numeroRadicadoRespuesta}
            formularioInformacionSolicitud={formularioInformacionSolicitud}
            loadingInformacionSolicitud={loadingInformacionSolicitud}
            setAgregarNuevoReintegro={setAgregarNuevoReintegro}
            setAplicarReintegro={setAplicarReintegro}
            obtenerListadoReintegros={obtenerListadoReintegros}
            unidadesEjecutoras={unidadesEjecutoras}
        />
    }

    if (loading) {
        return <LoadingComponent loading={loading} />
    }

    return (
        <CHPrivateContainer>
            <CHPageTitleV2
                topText="Gestión Reintegros"
                title1="Reintegros"
                title2='Unidades Ejecutoras'
                icono="fact_check"
            />
            <Container>
                <Grid item xs={12}>
                    <Card>
                        <CardContent>
                            <Grid container >
                                <Grid item xs={3} className={classes.iconoNumeroRadicado}>
                                    <TituloIconoV2
                                        title1="Número de"
                                        subtitle="Radicado"
                                        icono="event_available"
                                    />
                                </Grid>
                                <Grid item xs={6} className={classes.campoRadicado}>
                                    <Controller
                                        name="numeroRadicado"
                                        control={formularioNuevoReintegro.control}
                                        rules={{
                                            pattern:
                                                /^446-01-20/,
                                            required: 'Este campo es requerido',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={validarErrorCampoRadicado(error)}
                                                isReadOnly={false}
                                                value={value}
                                                label={' '}
                                                onChange={e => {
                                                    if (e.target.value.length <= 20) {
                                                        onChange(
                                                            e.target.value.replace(
                                                                /[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`]/g,
                                                                '',
                                                            ),
                                                        );
                                                    }
                                                }}
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item className={classes.iconoDodoRadicado}>
                                    <a href={linkDodo || undefined} onClick={obtenerUrlDodo} className={classes.enlaceDodo} target="_blank" rel="noopener noreferrer">
                                        <AttachFileIcon />
                                    </a>
                                </Grid>
                            </Grid>
                            <Grid container justifyContent="space-around">
                                <Button
                                    className={classes.buttonAgregarAfiliado}
                                    onClick={() => obtenerUnidadEjecutoraPorRadicado(formularioNuevoReintegro.watch('numeroRadicado'))}
                                >
                                    CONSULTAR
                                </Button>
                            </Grid>
                            <br />
                            <br />
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIconoV2
                                        title1="NIT Unidad"
                                        subtitle="Ejecutora"
                                        icono="event_available"
                                    />
                                </Grid>
                                <Grid item xs={6}>
                                    <Controller
                                        name="nitUnidadEjecutora"
                                        control={formularioNuevoReintegro.control}
                                        rules={{
                                            required:
                                                'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                numberMask
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                isReadOnly
                                                value={value}
                                                label={' '}
                                                onBlur={
                                                    e => {
                                                        obtenerUnidadEjecutoraPorIdentificacion(e.target.value);
                                                    }
                                                }
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <br />
                            <br />
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIconoV2
                                        title1="Unidad"
                                        subtitle="Ejecutora"
                                        icono="today"
                                    />
                                </Grid>
                                <Grid item xs={6}>
                                    <Controller
                                        name="idUnidadEjecutora"
                                        control={formularioNuevoReintegro.control}
                                        rules={{
                                            required:
                                                'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                readOnly
                                                error={!!error}
                                                label=""
                                                value={value}
                                                helperText={error ? error.message : null}
                                                onChange={e => {
                                                    const unidadEjecutoraSeleccionada = unidadesEjecutoras.find(item => item.id === e.target.value);
                                                    formularioNuevoReintegro.setValue('nombreUnidadEjecutora', unidadEjecutoraSeleccionada?.nombre || '');
                                                    onChange(e);
                                                }}
                                            >
                                                {listaUnidadesEjecutoras?.map(
                                                    (option: any) => (
                                                        <MenuItem key={option.id}
                                                            value={option.id}
                                                        >
                                                            {option.nombre}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <br />
                            <br />
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIconoV2
                                        title1="Tipo"
                                        subtitle="Solicitud"
                                        icono="date_range"
                                    />
                                </Grid>
                                <Grid item xs={6}>
                                    <Controller
                                        name="tipoReintegro"
                                        control={formularioNuevoReintegro.control}
                                        rules={{
                                            required:
                                                'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                label=""
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            >
                                                {tiposReintegros?.map(
                                                    (option: any) => (
                                                        <MenuItem key={option.idTipoReintegro}
                                                            value={option.idTipoReintegro}
                                                        >
                                                            {option.descripcion}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <br />
                            <br />
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIconoV2
                                        title1="Número"
                                        subtitle="Documento"
                                        icono="date_range"
                                    />
                                </Grid>
                                <Grid item xs={6}>
                                    <Controller
                                        name="numeroDocumento"
                                        control={formularioNuevoReintegro.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                value={value}
                                                multiline
                                                rows={3}
                                                label=''
                                                onChange={e => {
                                                    onChange(
                                                        e.target.value.replace(
                                                            /[a-zA-Z#./?=!@$%^&*^)(+=-_}{":;><|~` ]/g,
                                                            '',
                                                        ),
                                                    );
                                                }}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container justifyContent="space-around">
                                <Button
                                    className={classes.buttonAgregarAfiliado}
                                    onClick={formularioNuevoReintegro.handleSubmit(
                                        agregarAfiliado)
                                    }
                                >
                                    AGREGAR AFILIADO
                                </Button>
                            </Grid>
                            <Grid container justifyContent="space-around">
                                {afiliadosModel && (
                                    <AfiliadosTable
                                        afiliados={afiliadosModel}
                                        tiposReintegros={tiposReintegros}
                                        eliminarAfiliado={eliminarAfiliado}
                                    />
                                )}
                            </Grid>
                            <br />
                            <br />
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <TituloIconoV2
                                        title1="Observaciones"
                                        subtitle=""
                                        icono="date_range"
                                    />
                                </Grid>
                                <Grid item xs={6}>
                                    <Controller
                                        name="observacion"
                                        control={formularioNuevoReintegro.control}
                                        rules={{
                                            required: {
                                                value: afiliadosModel.some(item => item.id_Tipo_Reintegro === TIPO_REINTEGRO.INCONSISTENTES),
                                                message: 'Este campo es requerido.',
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
                                                inputProps={{
                                                    style: {
                                                        textTransform:
                                                            'uppercase',
                                                    },
                                                }}
                                                value={value}
                                                multiline
                                                rows={3}
                                                label=''
                                                onChange={onChange}
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        </CardContent>
                    </Card>
                </Grid>
                <div className="buttonssolicitud">
                    <Button
                        className={classes.buttonsecundario}
                        variant="outlined"
                        onClick={onClickCancelar}
                    >
                        CANCELAR
                    </Button>
                    <div className={classes.wrapper}>
                        <Button
                            className={classes.buttonprincipal}
                            variant="outlined"
                            onClick={formularioNuevoReintegro.handleSubmit(onSubmit)}
                        >
                            ACEPTAR
                        </Button>
                    </div>
                </div>
            </Container>
        </CHPrivateContainer>
    );
}

type propsType = {
    aplicarReintegro: boolean;
    numeroRadicadoRespuesta: any;
    formularioInformacionSolicitud: any;
    loadingInformacionSolicitud: boolean;
    setAplicarReintegro: (value: boolean) => void;
    setAgregarNuevoReintegro: (value: boolean) => void;
    obtenerListadoReintegros: (busqueda: any) => void;
}

export default NuevoReintegro;