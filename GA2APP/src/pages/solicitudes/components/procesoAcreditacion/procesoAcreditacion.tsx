import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Controller, FormProvider, useForm, UseFormHandleSubmit, UseFormReturn, useWatch } from 'react-hook-form';
import { toast } from 'react-toastify';
import {
    Button,
    Card,
    CardContent,
    CircularProgress,
    Container,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
    Tab,
    Tabs,
    Theme,
} from '@material-ui/core';
import InformacionAcreditacion from '../InformacionAcreditacion/InformacionAcreditacion';
import DatosPersonales from '../datos-personales/datosPersonales.component';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';
import CargueDocumentosSolicitud from '../cargue-documentos-solicitud/cargue-documentos-solicitud.component';
import ObservacionesSolicitud from '../observaciones/observaciones';
import ObservacionesInicioSolicitud from '../observaciones/observaciones-inicio-solicitud.component';
import BotoneraInformacionSolicitud from '../botonera/botonera-informacion-solicitud';
import BotoneraSolicitud from '../botonera/botonera-solicitud.component';
import { SolicitudTitleType } from '../../../../types/types';
import CHPageTitleV2 from '../../../../components/ch-page-title/CHPageTitleV2';
import SolicitudApi from '../../../../services/api-solicitud.service';
import NotificationService from '../../../../services/notification.service';
import { LoadingV2Component } from '../../../../components/loading/loadingV2.component';

const TABS_INDEX = {
    RESUMENSOLICITUD: 'resumen',
    CARGUEDOCUMENTOSACREDITACION: 'carguededocumentosacreditacion',
    // CARGUEDOCUMENTOSV14: 'carguededocumentosv14',
    // CARGUEDOCUMENTOSV8: 'carguededocumentosv8',
    // CARGUEDOCUMENTOSDESAFILIACION: 'carguededocumentosdesafiliaciones',
    // VALIDACIONDOCUMENTOS: 'validaciondedocumentos',
    // CARGAARCHIVOS: 'cargaarchivos_2',
    // VERARCHIVOS: 'verarchivos_2',
    // FORMULARIO_INCONSISTENCIAS: 'formulario',
    // VERDETALLESOLICITUD: 'Detalle Solicitud',
    // VERIFICACIONCUENTAINDIVIDUAL: '10',
    // VERIFICACIONESDECUENTA: '11',
    // HISTORIALINCONSISTENCIA: '7',
    // SEGURIDADDOCUMENTAL: '8',
};



function ProcesoAcreditacion({
    responseSolicitud,
    control,
    withButtons = true,
    onItemGoBack,
    onSubmit,
    onSubmitAprobar,
    onSubmitRechazar,
    // onSubmitAnular,
    tipoAccion,
    handleSubmit,
    tipoAccionDetalleProcesar,
    tipoSolicitud,
    cambiarEstadoTramite,
    trazabilidad,
    setValidarRetiro,
    setMostrarMensaje,
    formularioAcreditacion,
    mostrarBotonera,
    dynamicTitle,
    hideTitle = false,
    matriculaAcre,
    vrRetiroAcre,
    obtnerBoleano,
    isloading
}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    control: UseFormReturn<any>;
    withButtons?: boolean,
    onItemGoBack: () => void;
    onSubmit: () => void;
    onSubmitAprobar: () => void;
    onSubmitRechazar: () => void;
    // onSubmitAnular: () => void;
    tipoAccion: number;
    handleSubmit: UseFormHandleSubmit<any> | undefined;
    tipoAccionDetalleProcesar: boolean;
    tipoSolicitud: any;
    cambiarEstadoTramite: any;
    trazabilidad: any;
    setValidarRetiro: React.Dispatch<React.SetStateAction<number>>;
    setMostrarMensaje: (value: boolean) => void;
    formularioAcreditacion: UseFormReturn<any>;
    mostrarBotonera: any;
    dynamicTitle: SolicitudTitleType;
    hideTitle: boolean;
    matriculaAcre: string;
    vrRetiroAcre: number;
    obtnerBoleano: any | undefined;
    isloading: boolean;
}) {


    const useStyles = makeStyles(() =>
        createStyles({
            centrar: {
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                textAlign: 'center',
                width: '100%',
            },

        }),
    );


    const classes = useStyles();
    const api = SolicitudApi();

    const [documentosAguardar, setDocumentosAguardar] = useState<any>([]);
    const documentosAgregados = (
        documentos: any,
    ): void => {
        setDocumentosAguardar(documentos);
    };


    const methods = useForm({
        defaultValues: {
            nombresCliente: responseSolicitud?.nombresCliente ?? '',
            apellidosCliente: responseSolicitud?.apellidosCliente ?? '',
            acuerdo: "Acuerdo 02 de 2020",
            beneficiarioPago: `${responseSolicitud?.nombresCliente} ${responseSolicitud?.apellidosCliente}` ?? "",
            cumpleRequisito: '',
            documentoBeneficiarioPago: responseSolicitud?.identificacionCliente ?? "",
            fechaGiro: new Date(),
            fechaProximoSeguimiento: new Date(),
            fechaSeguimiento: new Date(),
            fecha_Firma_Escritura: new Date(),
            noRadicado: responseSolicitud?.numeroRadicadoSG ?? "",
            numeroMatricula: matriculaAcre ?? "",
            numero_Folio: responseSolicitud?.numero_Folio || 0,
            observacion: responseSolicitud?.observacion || '',
            valorGirado: vrRetiroAcre ?? "",
            valorSolicitud: vrRetiroAcre ?? "",
            causal: '',
            accionRealizar: '',
            tipoVivienda: '',
        }
    });


    useEffect(() => {
        if (responseSolicitud || matriculaAcre || vrRetiroAcre) {
            methods.reset({
                nombresCliente: responseSolicitud?.nombresCliente ?? '',
                apellidosCliente: responseSolicitud?.apellidosCliente ?? '',
                acuerdo: "Acuerdo 02 de 2020",
                beneficiarioPago: `${responseSolicitud?.nombresCliente} ${responseSolicitud?.apellidosCliente}` ?? "",
                cumpleRequisito: '',
                documentoBeneficiarioPago: responseSolicitud?.identificacionCliente ?? "",
                fechaGiro: new Date(),
                fechaProximoSeguimiento: new Date(),
                fechaSeguimiento: new Date(),
                fecha_Firma_Escritura: new Date(),
                noRadicado: responseSolicitud?.numeroRadicadoSG ?? "",
                numeroMatricula: matriculaAcre ?? "",
                numero_Folio: responseSolicitud?.numero_Folio || 0,
                observacion: responseSolicitud?.observacion || '',
                valorGirado: vrRetiroAcre ?? "",
                valorSolicitud: vrRetiroAcre ?? "",
                causal: '',
                accionRealizar: '',
            });
        }
    }, [responseSolicitud, matriculaAcre, vrRetiroAcre, methods]);



    const tabsToShow3 = [
        { label: 'Resumen', value: TABS_INDEX.RESUMENSOLICITUD },
        { label: 'Cargue de documentos', value: TABS_INDEX.CARGUEDOCUMENTOSACREDITACION },

    ];

    const [currentTab, setCurrentTab] = useState(tabsToShow3[0].value);

    const handleTabChange = (_event: React.SyntheticEvent, newValue: any) => {
        setCurrentTab(newValue);
    };

    const onSubmitAnular = async () => {
        const confirmAlert = await NotificationService.confirmAlert('¿Está seguro que desea continuar?');

        if (confirmAlert) {
            const solicitudTareaDto = {
                solicitudTareaId: responseSolicitud?.solicitudTareaId as string
            };
            try {
                const responseAnular = await api.
                    PostAnularSolicitudTarea(solicitudTareaDto);
                if (responseAnular && responseAnular?.isSuccess) {
                    onItemGoBack()
                    obtnerBoleano(true)
                    toast('Se anulo la solicitud correctamente.', { type: toast.TYPE.SUCCESS });
                }
            }
            catch (e) {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            }
        }
    }



    return (
        <FormProvider {...methods}>
            <LoadingV2Component key="loader" loading={isloading} tittle='' />
            <Grid container>
                {!hideTitle && (
                    <>
                        <Grid item xs={12}>
                            <CHPageTitleV2 topText={dynamicTitle?.topText ?? ""}
                                title1={dynamicTitle?.title ?? "Título por defecto"}
                                icono={dynamicTitle?.icon ?? "default_icon"}
                            />
                        </Grid>
                    </>
                )}

                <Card>
                    <CardContent>
                        <Tabs value={currentTab} onChange={handleTabChange} aria-label="Formulario Tabs">
                            {tabsToShow3.map((tab) => (
                                <Tab key={tab.value} label={tab.label} value={tab.value} />
                            ))}
                        </Tabs>

                        <Grid container spacing={2}>
                            {currentTab === TABS_INDEX.RESUMENSOLICITUD && (
                                <>
                                    <Grid item xs={12}>
                                        <DatosPersonales
                                            responseSolicitud={responseSolicitud}
                                            control={methods}
                                            setValidarRetiro={setValidarRetiro}
                                            setMostrarMensaje={setMostrarMensaje}
                                            formularioAcreditacion={formularioAcreditacion}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <InformacionAcreditacion
                                            responseSolicitud={responseSolicitud}
                                            matriculaAcre={matriculaAcre}
                                            vrRetiroAcre={vrRetiroAcre}
                                            control={methods}
                                            formularioAcreditacion={formularioAcreditacion}
                                        />
                                    </Grid>

                                    <Grid item xs={12}>
                                        <ObservacionesInicioSolicitud control={methods} formularioAcreditacion={formularioAcreditacion} />
                                    </Grid>
                                </>
                            )}

                            {currentTab === TABS_INDEX.CARGUEDOCUMENTOSACREDITACION && (
                                <>
                                    <Grid item xs={12}>
                                        <CargueDocumentosSolicitud
                                            responseSolicitud={responseSolicitud}
                                            documentosAgregados={documentosAgregados}
                                            siEsAprobadaSolcitud={undefined}
                                        />
                                    </Grid>
                                </>
                            )}
                        </Grid>

                        {withButtons && true && (
                            <BotoneraSolicitud
                                onItemGoBack={onItemGoBack}
                                tipoAccion={tipoAccion}
                                onSubmit={onSubmit}
                                handleSubmit={methods.handleSubmit}
                                tipoAccionDetalleProcesar={tipoAccionDetalleProcesar}
                                tipoSolicitud={tipoSolicitud}
                                cambiarEstadoTramite={cambiarEstadoTramite}
                                onSubmitAnular={onSubmitAnular}
                                onSubmitAprobar={onSubmitAprobar}
                            />
                        )}


                    </CardContent>
                </Card>
            </Grid>
        </FormProvider>
    );

}

export default ProcesoAcreditacion;

