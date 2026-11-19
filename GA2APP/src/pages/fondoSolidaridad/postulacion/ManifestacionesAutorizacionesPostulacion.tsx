import React from 'react';
import { Grid, makeStyles, createStyles } from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import '../../solicitudes/solicitudes.styles.scss';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

function ManifestacionesAutorizacionesPostulacion(props: props) {
    const classes = useStyles();
    const {formularioPostulacion, camposSolicitudPostulacionSoloLectura} = props;

    return (
        <>
            <CHPaper title="Manifestación y autorizaciones">
                <p className="paragraphText">
                    Declaro que la información consignada es veraz exacta,
                    completa y actualizada y, así mismo, autorizo a CAJA HONOR
                    para que realice en cualquier tiempo la verificación
                    integral de la misma <br />
                    <br />
                    Expresamente autorizo a CAJA HONOR a: <br />
                    <br />- Consultar y verificar en las bases de datos
                    pertinentes toda mi información, referente a propiedades.
                    <br />- Que en el evento de ser adjudicatario (a) dentro de
                    la presente convocatoria, gire de la cuenta individual con
                    destino al Modelo Héroes los valores a que haya lugar, para
                    efectos de completar el valor de mi solución de vivienda.
                    <br />- Como beneficiario del Modelo héroes autorizo
                    mediante este documento a CAJA HONOR para modificar, revocar
                    o ajustar el acto administrativo por medio del cual se me
                    reconoce el beneficio a través de este modelo, en los casos
                    que la situación administrativa y/o legal así lo amerite.
                    <br />- Es responsabilidad del postulante mantener
                    actualizados los datos de contacto para facilitar los
                    procesos propios de la adjudicación de vivienda.
                    <br /> - Acepto y reconozco los tiempos y modos para elegir
                    el proyecto de vivienda; de acuerdo a lo establecido en el
                    Acuerdo 001 de 2011, Artículo 50 y 51.
                    <br /> - De lo contrario se pierde el beneficio logrado. -
                    Comprendo y acepto que una vez se publique el beneficio,
                    tengo tres (3) meses para hacer efectiva mi solución de
                    vivienda de acuerdo a los proyectos ofrecidos.
                    <br /> - Recuerde que las postulaciones que no cumplan con
                    los requisitos exigidos o aquellas que presenten
                    inconsistencias serán rechazadas.
                    <br /> - Los resultados del proceso serán publicados en la
                    página www.cajahonor.gov.co y puntos de atención al
                    afiliado.
                </p>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="manifestacionAutorizacion"
                        control={formularioPostulacion.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHCheckbox
                                label="Acepto *"
                                onChange={onChange}
                                checked={value}
                                error={!!error}
                                helperText={error ? error.message : null}
                                disabled={camposSolicitudPostulacionSoloLectura}
                            />
                        )}
                    />
                </Grid>
            </CHPaper>
        </>
    );
}

interface LinkMapDispatchToProps {
    formularioPostulacion: any;
    camposSolicitudPostulacionSoloLectura: boolean;
}

type props = LinkMapDispatchToProps;

export default ManifestacionesAutorizacionesPostulacion;
