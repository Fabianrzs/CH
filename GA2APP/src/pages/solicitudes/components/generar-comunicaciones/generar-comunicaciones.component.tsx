import React, { useEffect, useState } from 'react';
import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import { AppState } from '../../../../store/rootReducer';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import {
    startGetAllGroupsInconsistencies,
    startGetAllPointsAttention,
    startGetAllToolsInconsistencies,
} from '../../../../actions/catalog.action';
import { startLoadUsers } from '../../../../actions/user.action';
import apiSolicitud from '../../../../services/api-solicitud.service';
import ModalComunicacion from './ModalComunicacion';

const comunicaciones = [
    { id: 0, descripcion: 'NINGUNA' },
    { id: 1, descripcion: 'TRAMITE INTERRUMPIDO POR VERIFICACION DE IDENTIDAD DE LAS PARTES' },
    { id: 2, descripcion: 'TRAMITE INTERRUMPIDO POR VERIFICACION DEL INMUEBLE' },
    { id: 3, descripcion: 'SOLICITUD RATIFICACION  NEGOCIO JURIDICO' },
    { id: 4, descripcion: 'INCONSISTENCIA AVALUO' }
]

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
        buttonSubmit: {
            marginLeft: '.5rem',
            background: '#1C3F8B',
            color: '#FFFFFF',
        },
    }),
);

const api = apiSolicitud();

function GenerarComunicaciones({
    idSolicitud,
    responseSolicitud,
    formularioInformacionComunicacion,
    trazabilidad
}: {
    idSolicitud: string
    responseSolicitud: any;
    formularioInformacionComunicacion: UseFormReturn<any>;
    trazabilidad: any[];
}) {
    const classes = useStyles();
    const [deshabilitarCampos, setDeshabilitarCampos] = useState(false);
    const [mostrarCampoRadicado, setMostrarCampoRadicado] = useState(false);
    const [mostrarCampoRadicado2, setMostrarCampoRadicado2] = useState(false);
    const [mostrarInfoComunicacion2, setMostrarInfoComunicacion2] = useState(false);
    const [informacionComunicaciones, setInformacionComunicaciones] = useState<any[]>([]);
    const [openComunicacion, setOpenComunicacion] = useState(false);

    const formularioCartaComunicacion = useForm({
        defaultValues: {
            asunto: '',
            descripcion: '',
            radicado: '',
        },
    });

    useEffect(() => {
        consultarComunicacionSolicitud();
    }, [responseSolicitud])

    const consultarComunicacionSolicitud = () => {
        api.ObtenerComunicacionesSolicitud(idSolicitud)
            .then(async (result) => {
                if (result.isSuccess && result.data != null) {

                    setInformacionComunicaciones(result.data);
                    if (result.data && result.data.length === 1) {
                        formularioInformacionComunicacion.setValue('idComunicacion', result.data[0]?.idComunicacion);
                        formularioInformacionComunicacion.setValue('idSolicitud', result.data[0]?.idSolicitud);
                        formularioInformacionComunicacion.setValue('idCausalComunicacion', result.data[0]?.idCausalComunicacion);
                        formularioInformacionComunicacion.setValue('textoIncluir', '');
                        formularioInformacionComunicacion.setValue('firmaDigital', result.data[0]?.firmaDigital);
                        formularioInformacionComunicacion.setValue('radicado', result.data[0]?.radicadoRelacionado);
                        formularioInformacionComunicacion.setValue('radicadoRelacionado', result.data[0]?.radicado);
                        setMostrarCampoRadicado(result.data[0]?.radicadoRelacionado);
                    } else if (result.data && result.data.length === 2) {
                        formularioInformacionComunicacion.setValue('idComunicacion', result.data[0]?.idComunicacion);
                        formularioInformacionComunicacion.setValue('idSolicitud', result.data[0]?.idSolicitud);
                        formularioInformacionComunicacion.setValue('idCausalComunicacion', result.data[0]?.idCausalComunicacion);
                        formularioInformacionComunicacion.setValue('textoIncluir', '');
                        formularioInformacionComunicacion.setValue('firmaDigital', result.data[0]?.firmaDigital);
                        formularioInformacionComunicacion.setValue('radicado', result.data[0]?.radicadoRelacionado);
                        formularioInformacionComunicacion.setValue('radicadoRelacionado', result.data[0]?.radicado);
                        setMostrarCampoRadicado(result.data[0]?.radicadoRelacionado);

                        formularioInformacionComunicacion.setValue('idComunicacion2', result.data[1]?.idComunicacion);
                        formularioInformacionComunicacion.setValue('idCausalComunicacion2', result.data[1]?.idCausalComunicacion);
                        formularioInformacionComunicacion.setValue('textoIncluir2', '');
                        formularioInformacionComunicacion.setValue('firmaDigital2', result.data[1]?.firmaDigital);
                        formularioInformacionComunicacion.setValue('radicado2', result.data[1]?.radicadoRelacionado);
                        formularioInformacionComunicacion.setValue('radicadoRelacionado2', result.data[1]?.radicado);
                        setMostrarCampoRadicado2(result.data[1]?.radicadoRelacionado);
                    }

                    if (!responseSolicitud?.nombreTarea.includes('Seguridad documental grupo 1') &&
                        result.data && result.data.length >= 1) {
                        setMostrarInfoComunicacion2(true);
                    }
                }
            }).catch((e) => {
                toast(`Error al consultar comunicaciones ${e.message}`, { type: toast.TYPE.ERROR });
            });
    }

    const handleOpenModal = () => {
        if (responseSolicitud) {
            formularioCartaComunicacion.setValue('asunto', responseSolicitud?.numeroRadicadoSG || '');
        }
        setOpenComunicacion(true);
    };

    const handleCloseModal = () => {
        setOpenComunicacion(false);
    };


    useEffect(() => {
        const validacionVerificacion =
            String(
                responseSolicitud?.nombreTarea ?? '',
            ).trim();

        const tareasDeshabilitadas = [
            'Trámite de pago retiro parcial',
            'Trámite de pago V 8',
            'Trámite de pago V 14',
            'Verificación cuenta individual',
            'Verificación cuenta individual retiro parcial',
        ];

        const debeDeshabilitarCampos =
            tareasDeshabilitadas.includes(
                validacionVerificacion,
            );

        setDeshabilitarCampos(
            debeDeshabilitarCampos,
        );
    }, [
        responseSolicitud?.nombreTarea,
    ]);

    return (
        <>
            <CHPaper title="Generar comunicaciones">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="idCausalComunicacion"
                            control={formularioInformacionComunicacion.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido.',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de comunicación"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    readOnly={deshabilitarCampos}
                                >
                                    {comunicaciones?.map(
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
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="textoIncluir"
                            control={formularioInformacionComunicacion.control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido.',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Texto a incluir"
                                    value={value}
                                    onChange={onChange}
                                    onClick={
                                        deshabilitarCampos ||
                                            mostrarInfoComunicacion2
                                            ? undefined
                                            : handleOpenModal
                                    }
                                    isReadOnly={
                                        mostrarInfoComunicacion2 ||
                                        deshabilitarCampos
                                    }
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    disabled={deshabilitarCampos}
                                />
                            )}
                        />
                    </Grid>
                    {
                        mostrarCampoRadicado && (
                            <Grid className={classes.column} item xs={12} md={3}>
                                <Controller
                                    name="radicado"
                                    control={formularioInformacionComunicacion.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            label="No. Radicado Creado"
                                            value={value}
                                            onChange={onChange}
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            InputLabelProps={{ shrink: true }}
                                            isReadOnly
                                        />
                                    )}
                                />
                            </Grid>
                        )
                    }
                </Grid>
                {mostrarInfoComunicacion2 && (
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={4}>
                            <Controller
                                name="idCausalComunicacion2"
                                control={formularioInformacionComunicacion.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido.',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Tipo de comunicación"
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                        readOnly={String(responseSolicitud?.nombreTarea).includes('Verificación cuenta individual') || deshabilitarCampos}
                                    >
                                        {comunicaciones?.map(
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
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="textoIncluir2"
                                control={formularioInformacionComunicacion.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido.',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        label="Texto a incluir"
                                        value={value}
                                        onChange={onChange}
                                        onClick={
                                            !responseSolicitud?.nombreTarea.includes("Verificación cuenta individual") &&
                                                !responseSolicitud?.nombreTarea.includes("Trámite de pago") ?
                                                () => handleOpenModal() : undefined
                                        }
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        InputLabelProps={{ shrink: true }}
                                        disabled={deshabilitarCampos}
                                    />
                                )}
                            />
                        </Grid>
                        {
                            mostrarCampoRadicado2 && (
                                <Grid className={classes.column} item xs={12} md={3}>
                                    <Controller
                                        name="radicado2"
                                        control={formularioInformacionComunicacion.control}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHTextField
                                                label="No. Radicado Creado"
                                                value={value}
                                                onChange={onChange}
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                InputLabelProps={{ shrink: true }}
                                                isReadOnly
                                            />
                                        )}
                                    />
                                </Grid>
                            )
                        }
                    </Grid>
                )}
                <ModalComunicacion
                    open={openComunicacion}
                    onClose={handleCloseModal}
                    responseSolicitud={responseSolicitud}
                    formularioCartaComunicacion={formularioCartaComunicacion}
                    setMostrarCampoRadicado={setMostrarCampoRadicado}
                    setMostrarCampoRadicado2={setMostrarCampoRadicado2}
                    informacionComunicaciones={informacionComunicaciones}
                    formularioInformacionComunicacion={formularioInformacionComunicacion}
                />
            </CHPaper>
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    userState: state.users,
});

const mapDispatchToProps = (dispatch: any) => ({
    obtenerGrupos: () => dispatch(startGetAllGroupsInconsistencies()),
    obtenerHerramientas: () => dispatch(startGetAllToolsInconsistencies()),
    obtenerPuntosAtencion: () => dispatch(startGetAllPointsAttention()),
    obtenerUsuarios: (token: string) => dispatch(startLoadUsers(token)),
});

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(GenerarComunicaciones);