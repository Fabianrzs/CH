import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import AttachFileIcon from '@material-ui/icons/AttachFile';
import { createStyles, Grid, makeStyles } from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import ApiSolicitud from '../../../../services/api-solicitud.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        enlaceDodo:{
            cursor: 'pointer',
            marginTop: '48px',
        }
    }),
);

const InformacionSolicitudReintegro = (props: props) => {
    const classes = useStyles();
    const apiSolicitud = ApiSolicitud();
    const {informacionReintegro,numeroRadicadoRespuesta,formularioInformacionSolicitud} = props;
    const [linkDodo, setLinkDodo] = useState('');
    const [linkDodoRadicadoRespuesta, setLinkDodoRadicadoRespuesta] = useState('');

    useEffect(() => {
        obtenerUrlDodo();
        obtenerUrlDodoRadicadoRespuesta();
    }, [informacionReintegro,numeroRadicadoRespuesta]);

    const obtenerUrlDodo = async() => {
        const radicado = informacionReintegro?.numero_Radicado;
        if(radicado){
            await apiSolicitud
            .GetBoteneEnlaceDodo(String(radicado))
            .then(async respuesta => {
                if(respuesta?.data){
                    setLinkDodo(respuesta.data);
                }
            })
            .catch(() => {
                toast('Error del sistema al obtener url de Dodo en Información Solicitud Reintegros', { type: toast.TYPE.ERROR });
            });
        }
	}

    const obtenerUrlDodoRadicadoRespuesta = async() => {
        const radicado = numeroRadicadoRespuesta;
        if(radicado){
            await apiSolicitud
            .GetBoteneEnlaceDodo(String(radicado))
            .then(async respuesta => {
                if(respuesta?.data){
                    setLinkDodoRadicadoRespuesta(respuesta.data);
                }
            })
            .catch(() => {
                toast('Error del sistema al obtener url de Dodo en Información Solicitud Reintegros Radicado Respuesta', { type: toast.TYPE.ERROR });
            });
        }
	}

    return (
        <>
            <CHPaper title="Información Solicitud">
                    <Grid container>
                        <Grid className={classes.column} item xs={12} md={2}>
                            <Controller
                                name="numeroRadicado"
                                control={formularioInformacionSolicitud.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        isReadOnly
                                        value={value}
                                        onChange={onChange}
                                        label="Número Radicado"
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>
                            <a  href={linkDodo} className={classes.enlaceDodo}  target="_blank" rel="noopener noreferrer">
                                    <AttachFileIcon />
                            </a>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="numeroRadicadoRespuesta"
                                control={formularioInformacionSolicitud.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        isReadOnly
                                        value={value}
                                        onChange={onChange}
                                        label="No. Radicado Respuesta"
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>
                            <a  href={linkDodoRadicadoRespuesta} className={classes.enlaceDodo}  target="_blank" rel="noopener noreferrer">
                                    <AttachFileIcon />
                            </a>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaInicio"
                                control={formularioInformacionSolicitud.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        isReadOnly
                                        value={value}
                                        onChange={onChange}
                                        label="Fecha Inicio"
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaFinal"
                                control={formularioInformacionSolicitud.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHTextField
                                        isReadOnly
                                        value={value}
                                        onChange={onChange}
                                        label="Fecha Final"
                                        InputLabelProps={{ shrink: true }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
            </CHPaper>
        </>
    )
}

type props = {
    informacionReintegro: any;
    formularioInformacionSolicitud: any;
    numeroRadicadoRespuesta: any;
}

export default InformacionSolicitudReintegro;