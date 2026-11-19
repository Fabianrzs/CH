import React, { useEffect, useState } from 'react';
import { Button, createStyles, makeStyles } from '@material-ui/core';
import { UseFormHandleSubmit } from 'react-hook-form';
import { BOTONERA_SOLICITUD } from '../../../../constants/constants';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';


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
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnLabel: {
            padding: '10px',
        },
    }),
);

function BotoneraInformacionSolicitudComponent({
    onItemGoBack,
    onSubmit,
    onSubmitAprobar,
    onSubmitSubsanar,
    onSubmitRechazar,
    onSubmitGenerarPago,
    tipoAccion,
    disabledActualizar,
    disabledAprobar,
    disabledSubsanar,
    disabledRechazar,
    disabledRegresar,
    tipoAccionDetalleProcesar,
    responseSolicitud,
    currentTabValue,
    aprobarBloquear,
    hablitarActualiza,
    deshabilitarBTNRechazarGp2,
    deshalitarBotonesInconsistencia,
    userAsig,
    grupo,
    userIngreso,
    requiereActualizarParaAprobar,
    handleSubmit,
    onActualizacionCompletada

}: {
    onItemGoBack: () => void;
    onSubmit: (data?: any) => Promise<boolean>;
    onSubmitAprobar: () => void;
    onSubmitSubsanar: () => void;
    onSubmitRechazar: () => void;
    onSubmitGenerarPago: any;
    tipoAccion: number;
    disabledActualizar: boolean;
    disabledAprobar: boolean;
    disabledSubsanar: boolean;
    disabledRechazar: boolean;
    disabledRegresar: boolean;
    tipoAccionDetalleProcesar: boolean;
    responseSolicitud: ResponseSeveranceModel | undefined;
    currentTabValue: any;
    aprobarBloquear: any;
    hablitarActualiza: any;
    deshabilitarBTNRechazarGp2: any;
    deshalitarBotonesInconsistencia: boolean;
    userAsig: string | undefined;
    grupo: any;
    userIngreso: string;
    requiereActualizarParaAprobar?: boolean;
    handleSubmit: UseFormHandleSubmit<any>;
    onActualizacionCompletada?: () => void;
}) {

    const classes = useStyles();

    const [
        tipoAccionDetalleProcesarSubsanar,
        settipoAccionDetalleProcesarSubsanar,
    ] = useState<boolean>(false);

    const [
        actualizarHabilitado,
        setActualizarHabilitado,
    ] = useState<boolean>(false);

    const [
        aprobarHabilitado,
        setAprobarHabilitado,
    ] = useState<boolean>(false);

    const [
        actualizacionCompletada,
        setActualizacionCompletada,
    ] = useState<boolean>(false);

    useEffect(() => {
        let deshabilitarSubsanar =
            tipoAccionDetalleProcesar;

        const nombreTarea =
            responseSolicitud?.nombreTarea || '';

        if (
            nombreTarea.includes('Cargue de documentos') ||
            nombreTarea.includes(
                'Seguridad documental grupo',
            )
        ) {
            deshabilitarSubsanar = true;
        }

        settipoAccionDetalleProcesarSubsanar(
            deshabilitarSubsanar,
        );
    }, [
        tipoAccionDetalleProcesar,
        responseSolicitud?.nombreTarea,
    ]);

    useEffect(() => {
        const esVerificacionCuentaIndividual =
            responseSolicitud?.nombreTarea?.includes(
                'Verificación cuenta individual',
            );

        if (esVerificacionCuentaIndividual) {
            setActualizarHabilitado(true);
            setAprobarHabilitado(false);
        }
    }, [responseSolicitud?.nombreTarea]);

    useEffect(() => {
        if (actualizacionCompletada) {
            setAprobarHabilitado(true);
        }
    }, [actualizacionCompletada]);

    useEffect(() => {
        if (
            requiereActualizarParaAprobar &&
            !disabledActualizar
        ) {
            setActualizacionCompletada(false);
            setAprobarHabilitado(false);
        }
    }, [
        disabledActualizar,
        requiereActualizarParaAprobar,
    ]);


    const handleActualizar = async () => {
        try {
            const ejecutarSubmit = handleSubmit(
                async (data: any) => {
                    const actualizacionExitosa =
                        await onSubmit(data);
    
                    if (!actualizacionExitosa) {
                        setActualizacionCompletada(
                            false,
                        );
    
                        setAprobarHabilitado(
                            false,
                        );
    
                        return;
                    }
    
                    setActualizacionCompletada(
                        true,
                    );
    
                    setAprobarHabilitado(
                        true,
                    );
                },
                () => {
                    setActualizacionCompletada(
                        false,
                    );
    
                    setAprobarHabilitado(
                        false,
                    );
                },
            );
    
            await ejecutarSubmit();
        } catch (error) {
            console.error(
                'Error en la actualización:',
                error,
            );
    
            setActualizacionCompletada(
                false,
            );
    
            setAprobarHabilitado(
                false,
            );
        }
    };

    const renderActionButton = () => {
        const tarea = responseSolicitud?.nombreTarea;
        if (tarea?.includes('Verificación cuenta individual') && currentTabValue === 4) {
            return (
                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmitGenerarPago}
                    disabled={tipoAccionDetalleProcesar ? true : disabledAprobar}
                >
                    GENERAR PAGO
                </Button>
            );
        }

        const deshabilitarAprobar =
            tipoAccionDetalleProcesar ||
            deshalitarBotonesInconsistencia ||
            disabledAprobar;

        return (
            <Button
                className={classes.button}
                variant="outlined"
                onClick={onSubmitAprobar}
                disabled={deshabilitarAprobar}
            // no delete
            // disabled={tipoAccionDetalleProcesar || deshalitarBotonesInconsistencia || !aprobarHabilitado}
            // disabled={tipoAccionDetalleProcesar || deshalitarBotonesInconsistencia  || disabledAprobar || !aprobarHabilitado}
            >
                APROBAR
            </Button>
        );

        return null;

    };


    const renderActionButtonRechazar = () => {
        const nombreTarea = responseSolicitud?.nombreTarea || '';

        const esSeguridadDocumentalGrupo1 =
            nombreTarea.includes('Seguridad documental grupo 1');

        const esSeguridadDocumentalGrupo2 =
            nombreTarea.includes('Seguridad documental grupo 2');

        const esVerificacionCuentaIndividual =
            nombreTarea.includes('Verificación cuenta individual');

        const esVerificacionListasRestrictivas =
            nombreTarea.includes('Verificación de listas restrictivas');

        const esVerificacion =
            nombreTarea.includes('Verificación');

        const deshabilitarBase =
            userAsig !== userIngreso ||
            tipoAccionDetalleProcesar ||
            disabledRechazar;

        const renderBoton = (disabled: boolean) => (
            <Button
                className={classes.button}
                variant="outlined"
                onClick={onSubmitRechazar}
                disabled={disabled}
            >
                RECHAZAR
            </Button>
        );

        if (esSeguridadDocumentalGrupo1) {
            return renderBoton(
                deshabilitarBase ||
                deshalitarBotonesInconsistencia,
            );
        }

        if (esSeguridadDocumentalGrupo2) {
            return renderBoton(
                deshabilitarBase ||
                deshabilitarBTNRechazarGp2,
            );
        }

        if (esVerificacionCuentaIndividual) {
            return renderBoton(
                deshabilitarBase ||
                deshabilitarBTNRechazarGp2,
            );
        }

        return renderBoton(
            deshabilitarBase ||
            esVerificacionListasRestrictivas ||
            deshalitarBotonesInconsistencia ||
            esVerificacion,
        );
    };

    const nombreTarea = responseSolicitud?.nombreTarea || '';

    const esVerificacionListasRestrictivas = nombreTarea.includes(
        'Verificación de listas restrictivas'
    );

    const deshabilitarBotonSubsanar =
        userAsig !== userIngreso ||
        esVerificacionListasRestrictivas ||
        tipoAccionDetalleProcesarSubsanar ||
        disabledSubsanar ||
        !!requiereActualizarParaAprobar;


    return tipoAccion === BOTONERA_SOLICITUD.ACTUALIZAR ? (
        <>
            <div className="buttons">
                {/* {
                    responseSolicitud?.nombreTarea?.includes('Seguridad documental grupo') ?
                        <Button
                            className={classes.button}
                            variant="outlined"
                            onClick={onSubmit}
                            disabled={userAsig !== userIngreso || tipoAccionDetalleProcesar ? true : disabledRechazar || hablitarActualiza}

                        >
                            ACTUALIZAR

                        </Button> :

                        <Button
                            className={classes.button}
                            variant="outlined"
                            onClick={handleActualizar}
                            disabled={
                                userAsig !== userIngreso ||
                                tipoAccionDetalleProcesar ||
                                disabledActualizar ||
                                deshalitarBotonesInconsistencia
                            }
                        >
                            ACTUALIZAR
                        </Button>
                } */}

                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={handleActualizar}
                    disabled={
                        userAsig !== userIngreso ||
                        tipoAccionDetalleProcesar ||
                        disabledActualizar ||
                        deshalitarBotonesInconsistencia
                    }
                >
                    ACTUALIZAR
                </Button>


                {renderActionButton()}

                {renderActionButtonRechazar()}

                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onSubmitSubsanar}
                    disabled={deshabilitarBotonSubsanar}
                >
                    SUBSANAR
                </Button>

                <Button
                    className={classes.button}
                    variant="outlined"
                    onClick={onItemGoBack}
                    disabled={disabledRegresar}
                >
                    REGRESAR
                </Button>
            </div>
        </>
    ) : null;
}

export default BotoneraInformacionSolicitudComponent;

