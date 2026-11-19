import { Button, createStyles, Grid, makeStyles, MenuItem } from "@material-ui/core";
import React, { useState, useEffect } from "react";
import { Controller, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import CatalogApi from "../../services/api-catalog.service";
import ApiClients from "../../services/api-clients.service";
import CreditApi from "../../services/api-credit.service";
import ApiParametrization from "../../services/api-parametrization.service";
import Prospeccion from "../../services/api-prospeccion.service";
import AuthService from "../../services/auth.service";
import NotificationService from "../../services/notification.service";
import { Auditoria } from "../../types/Auditoria.model.type";
import CHPageTitle from "../ch-page-title/CHPageTitle";
import CHPaper from "../ch-paper/CHPaper";
import CHPrivateContainer from "../ch-private-container/CHPrivateContainer";
import CHSelect from "../ch-select/CHSelect";
import CHTextField from "../ch-text-field/CHTextField";
import { LoadingComponent } from "../loading/loading.component";
import AcordionProspeccionTablas from "./AcordionProspeccionTablas";
import AuditoriaApi from '../../services/api-auditorias.servise'

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
        },
        button: {
            width: '100%',
            backgroundColor: '#1C3F8B',
            color: '#FFFFFF',
            marginTop: '10px',
            '&:hover': {
                backgroundColor: '#FFFFFF !important',
                color: 'black !important'
            },
        },
        container: {
            maxHeight: 440,
        },
        formControl: {
            width: '100%',
        },
    })
);



const RequestProspeccion = () => {
    const apiCatalogo = CatalogApi();
    const creditoApi = CreditApi();
    const apiProspeccion = Prospeccion();
    const apiParametrization = ApiParametrization();
    const apiClients = ApiClients();
    const formularioProspeccion = useForm({
        defaultValues: {
            tipoDocumento: '1',
            numeroDocumento: '',
        }
    });
    const datoTipoDocumento = formularioProspeccion.watch('tipoDocumento');
    const datoNumDocumento = formularioProspeccion.watch('numeroDocumento');

    const [listaTipoIdentificacion, setListaTipoIdentificacion] = useState<any[]>([]);
    const [datosCartera, setDatosCartera] = useState<any[]>([]);
    const [searchResults, setSearchResults] = useState<any[]>([]);
    const [searchResults1, setSearchResults1] = useState<any[]>([]);
    const [loading, setLoading] = useState(false)
    const [mostrarPdf, setMostrarPdf] = useState(false)
    const [listaDatosProspeccion, setListaDatosProspeccion] = useState<any[]>([])
    const [url, setUrl] = useState('')



    useEffect(() => {
        const urlActual = window.location.href;
        setUrl(urlActual)
            console.log('entro')
     
    }, []);


    const handleNavigation = () => {
        if (url === 'https://cpvmp-az-251t:89/ConsultaProspeccion') {
            window.location.href = 'https://cpvmp-az-251t:86/login';
        } else if (url === 'http://localhost:3000/ConsultaProspeccion') {
            window.location.href = `http://localhost:3001/login`;
        } else if (url === 'https://cpvmp-az-251t:89/ConsultaProspeccion') {
            window.location.href = 'https://cpvmp-az-250d:86/login';
        } else if (url === 'https://cpvmp-az-251p:89/ConsultaProspeccion') {
            window.location.href = 'https://cpvmp-az-251p:86/login';
        }
    };

    useEffect(() => {
        apiCatalogo
            .getAllTypeDocuments()
            .then(async response => {
                if (response?.data) {
                    setListaTipoIdentificacion(response.data);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

        apiParametrization
            .GetAllParametrizationSeguroRegion()
            .then(async response => {
                if (response?.data) {
                    setDatosCartera(response.data);
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }, []);


    const volverEnviarBusqueda = (item: any) => {
        const data = {
            idType: 1,
            idNumber: item.olN_NUMERO_DOCUMENTO
        }
        enviarBusqueda(data)
        ConsultarLineaCredito(data)
    }

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

    const enviarBusqueda = (data: any) => {
       
        setSearchResults([]);
        ConsultarLineaCredito(data)
        setLoading(true);
        DatosClienteHV(data);
        obtenerDatosProspeccionAfiliado(data);

        const auditoriaApi = AuditoriaApi();
        const user = AuthService.getUserInfo();
        const nav = window.navigator as any;
        const usrAg = nav.userAgentData.brands.find(
            (x: { brand: string }) => x.brand === 'Google Chrome' || x.brand === 'Microsoft Edge'
        )?.brand;

        let descripcionTipoDoc = 'No especificado';

        if (parseInt(datoTipoDocumento, 10) === 1) {
            descripcionTipoDoc = 'CÉDULA DE CIUDADANÍA';
        } else if (parseInt(datoTipoDocumento, 10) === 3) {
            descripcionTipoDoc = 'CÉDULA DE EXTRANJERÍA';
        } else if (parseInt(datoTipoDocumento, 10) === 8) {
            descripcionTipoDoc = 'CÓDIGO JUZGADO';
        } else if (parseInt(datoTipoDocumento, 10) === 9) {
            descripcionTipoDoc = 'CÓDIGO MILITAR';
        } else if (parseInt(datoTipoDocumento, 10) === 2) {
            descripcionTipoDoc = 'NÚMERO DE IDENTIFICACIÓN TRIBUTARIA';
        } else if (parseInt(datoTipoDocumento, 10) === 5) {
            descripcionTipoDoc = 'NÚMERO ÚNICO DE IDENTIFICACIÓN';
        } else if (parseInt(datoTipoDocumento, 10) === 6) {
            descripcionTipoDoc = 'PASAPORTE';
        } else if (parseInt(datoTipoDocumento, 10) === 7) {
            descripcionTipoDoc = 'REGISTRO CIVIL';
        } else if (parseInt(datoTipoDocumento, 10) === 4) {
            descripcionTipoDoc = 'TARJETA DE IDENTIDAD';
        }

        try {
            const requestAuditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData.platform,
                nombreUsuario: user.USR_USERNAME,
                usurioId: user.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta Prospección',
                hostname: document.location.host,
                tipoAccion: 'CONSULTAS',
                idTipoAccion: 3,
                descripcion: `Consulta prospección con los siguientes filtros:
                      - Tipo de documento: ${descripcionTipoDoc || 'No especificada'}
                      - Número documento: ${datoNumDocumento || 'No especificado'}`,
            } as Auditoria;

            auditoriaApi.InsertAuditoria(requestAuditoria);
        } catch (error: any) {
            const descripcionError = `Error en consulta de información crédito
            - Tipo de documento: ${descripcionTipoDoc || 'No especificada'}
            - Número documento: ${datoNumDocumento || 'No especificado'}`;

            const requestAuditoriaError: Auditoria = {
                navegador: usrAg,
                sistemaOperativo: nav.userAgentData?.platform,
                nombreUsuario: user?.USR_USERNAME,
                usurioId: user?.USR_ID,
                ip: publicIpAddress,
                modulo: 'Consulta Prospección',
                hostname: document.location.host,
                tipoAccion: 'ERRORES Y EXCEPCIONES',
                idTipoAccion: 8,
                descripcion: descripcionError
            };

            auditoriaApi.InsertAuditoria(requestAuditoriaError);

            toast("Ocurrió un error al realizar la consulta. Inténtalo de nuevo.");
        }



    }

    const DatosClienteHV = (data: any) => {

        const idType = data.tipoDocumento === undefined ? data.idType : data.tipoDocumento;
        const idNumber = data.numeroDocumento === undefined ? data.idNumber : data.numeroDocumento;

        apiClients
            .getClientByIdentificationTypeIdentificationNumberRequest(idType, idNumber)
            .then(async response => {
                if (response?.data) {
                    valoresProspeccion(response.data, data)
                    // datosSeguros(response.data, data);
                } else {
                    setLoading(false);
                    setSearchResults([]);
                    NotificationService.genericAlwarning(
                        'Consulta Afiliado',
                        'No se encontro afiliado.',
                    );
                }
            })
            .catch(() => {
                toast('Ingrese el tipo de documento');
                setLoading(false);


            });
    }


    const consultaValoresSimulacionCreditoReclasificiacion = async (
        valoresAnteriores: any,
        valoresProspeccion: any,
        DatosHV: any,
        data: any,
    ) => {
        const numeroDocumento =
            data.numeroDocumento === undefined
                ? data.idNumber
                : data.numeroDocumento;
    
        try {
            const responseNueva =
                await creditoApi
                    .getConsultaValoresSimulacionCreditoReclasificiacion(
                        numeroDocumento,
                    );
    
            if (responseNueva?.data) {
               
                const valoresSimulacion = {
                    ...valoresAnteriores,
                    ...responseNueva.data,
                };
    
                const combinedResults = [
                    valoresSimulacion,  // data[0]
                    DatosHV,            // data[1]
                    valoresProspeccion, // data[2]
                    data,               // data[3]
                ];
    
                setSearchResults(combinedResults);
    
                return;
            }
    
           
            const combinedResults = [
                valoresAnteriores,
                DatosHV,
                valoresProspeccion,
                data,
            ];
    
            setSearchResults(combinedResults);
    
            NotificationService.genericAlwarning(
                'Consulta Afiliado',
                `No se encontraron valores nuevos de simulación. ${
                    responseNueva?.returnMessage || ''
                }`,
            );
        } catch (error) {
           
            const combinedResults = [
                valoresAnteriores,
                DatosHV,
                valoresProspeccion,
                data,
            ];
    
            setSearchResults(combinedResults);
    
            toast(
                'No se pudieron consultar los nuevos valores de simulación. Se mostrarán los valores anteriores.',
                {
                    type: toast.TYPE.WARNING,
                },
            );
        } finally {
            setLoading(false);
        }
    };

    const valoresProspeccion = async (DatosHV: any, data: any) => {

        const request = {
            tipoDocumento: data.tipoDocumento === undefined ? data.idType : data.tipoDocumento,
            numeroDocumento: data.numeroDocumento === undefined ? data.idNumber : data.numeroDocumento
        }

        try {
            const response = await apiProspeccion.searchProspeccion(request);
            if (response?.data) {

               

                datosSeguros(response.data, DatosHV, request);
            } else {
                setSearchResults([]);
                NotificationService.genericAlwarning(
                    'Consulta Afiliado',
                    `No se encontro afiliado. ${response.returnMessage}`,
                );
            }
        } catch (error) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
            setSearchResults([]);
        } finally {
            setLoading(false);
        }
    };

    const obtenerDatosProspeccionAfiliado = async (data: any) => {
        try {
            const request = data.numeroDocumento === undefined ? data.idNumber : data.numeroDocumento
            const response = await apiProspeccion.getConsultarDatosProspeccionByNumeroDocumento(request);
            if (response?.data) {
                setListaDatosProspeccion(response.data)
            }
        } catch (error) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        } finally {
            setLoading(false);
        }
    }
    const datosSeguros = async (
        valoresProspeccion: any,
        DatosHV: any,
        data: any,
    ) => {
        try {
          
            const responseAnterior =
                await apiProspeccion.postValoresProspeccion(data);
    
           
    
            await consultaValoresSimulacionCreditoReclasificiacion(
                responseAnterior.data,
                valoresProspeccion,
                DatosHV,
                data,
            );
        } catch (error) {
            toast('Error del sistema', {
                type: toast.TYPE.ERROR,
            });
    
            setSearchResults([]);
            setLoading(false);
        }
    };

    const ConsultarLineaCredito = async (data: any) => {
        try {


            const request = {
                tipoDocumento: data.tipoDocumento === undefined ? data.idType : data.tipoDocumento,
                numeroDocumento: data.numeroDocumento === undefined ? data.idNumber : data.numeroDocumento
            }
            const response = await apiProspeccion.postConsultarLineaCredito(request);
            if (response?.data) {

                setSearchResults1(response.data);
                const valorBoleano = response?.data.some((x: any) => x.olN_ESTADO_RECALCULAR === false)
                if (valorBoleano) {
                    setMostrarPdf(true)
                } else {
                    setMostrarPdf(false)
                }

            } else {
                setSearchResults1([]);
                NotificationService.genericAlwarning(
                    'Consulta Afiliado3 ',
                    `No se encontro afiliado. ${response.returnMessage}`,
                );
            }
        } catch (error) {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
            setSearchResults([]);
        }
    };


    const classes = useStyles();

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Reclasificación "
                subText=""
                title1="Reclasificación "
                title2="de Clientes"
            />
            <CHPaper title="Reclasificación  de Clientes">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container spacing={2}>
                    <Grid item xs={12} md={4}>
                        <div className={classes.formControl}>
                            <Controller
                                name="tipoDocumento"
                                control={formularioProspeccion.control}
                                rules={{ required: 'El tipo de documento es obligatorio' }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Tipo de Documento *"
                                        value={value}
                                       
                                        onChange={onChange}
                                    >
                                        {listaTipoIdentificacion.map(
                                            (option: any) => (
                                                <MenuItem
                                                    key={option.id}
                                                    value={option.id}
                                                >
                                                    {option.descripcion}
                                                </MenuItem>
                                            ),
                                        )}
                                    </CHSelect>
                                )}
                            />
                        </div>
                    </Grid>

                    <Grid item xs={12} md={4}>
                        <div className={classes.formControl}>
                            <Controller
                                name="numeroDocumento"
                                control={formularioProspeccion.control}
                                rules={{
                                    required: 'El número de documento es obligatorio',
                                    minLength: {
                                        value: 4,
                                        message: 'La longitud mínima del campo es de 4 caracteres',
                                    },
                                    maxLength: {
                                        value: 14,
                                        message: 'La longitud máxima del campo es de 14 caracteres',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        numberMask
                                        InputLabelProps={{ shrink: true }}
                                        label="Número Documento *"
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </div>
                    </Grid>

                    <Grid item xs={12} md={4}>
                        <Button
                            className={classes.button}
                            variant="contained"
                            onClick={formularioProspeccion.handleSubmit(enviarBusqueda)}
                        >
                            Buscar Prospección
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>


            <Grid>
                <Button onClick={() => handleNavigation()}>IR AL PORTAL</Button>
            </Grid>
            {searchResults.length !== 0 &&
                <AcordionProspeccionTablas
                    data={searchResults}
                    data1={searchResults1}
                    datosCartera={datosCartera}
                    volverEnviarBusqueda={volverEnviarBusqueda}
                    mostrarPdf={mostrarPdf}
                    listaDatosProspeccion={listaDatosProspeccion}
                />
            }
        </CHPrivateContainer>
    );
}

export default RequestProspeccion;
