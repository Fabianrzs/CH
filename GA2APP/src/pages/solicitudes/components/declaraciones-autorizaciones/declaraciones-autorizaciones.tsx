import { createStyles, Grid, makeStyles } from '@material-ui/core';
import React from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import CHPaper from '../../../../components/ch-paper/CHPaper';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        paragraphText: {
            textAlign: 'justify',
            lineHeight: 1.6,
            marginBottom: '16px',
        },
    }),
);

const DeclaracionesAutorizaciones = (props: TypeProps) => {

    const classes = useStyles();
    const {formulario, setAceptarCheckValue, campoSoloLecturaRadicacionAprobada} = props;
   
    return(
        <CHPaper title="Declaraciones y autorizaciones">
            <p className={classes.paragraphText}>
                    Autorizaciones tratamiento de datos<br />
                    <br />
                    En cumplimiento de lo dispuesto en la Ley 1266 de 2008, la Ley 1581 de 2012, el Decreto
                    Reglamentario 1377 de 2013 y la Ley 527 de 1999, autorizo de manera libre, previa,
                    expresa e informada a Caja Honor para realizar el tratamiento de mis datos personales,
                    incluyendo su recolección, almacenamiento, uso, circulación y supresión, con fines
                    relacionados con los servicios y trámites ofrecidos por la entidad.<br />
                    <br />
                    Autorizo expresamente a Caja Honor para consultar mi información en centrales de riesgo,
                    con el fin de verificar datos financieros y crediticios relacionados con los trámites que
                    adelante ante la entidad, garantizando el uso responsable, seguro y confidencial de mis
                    datos personales, conforme a lo establecido en la Ley 1266 de 2008 y la Ley 1581 de 2012.<br />
                    <br />
                    Acepto y autorizo el tratamiento de los datos personales y de la información que suministre
                    en diligenciamiento del Formato Único de Pago (FUP digital) para la radicación de la
                    solicitud del trámite ante la Caja Promotora de Vivienda Militar y de Policía.<br />
                    <br />
                    Declaraciones de la solicitud del trámite, servicio y/o producto financiero <br />
                    <br />
                    Declaro, bajo la gravedad de juramento, que la información y documentación aportada a la
                    solicitud del trámite es veraz, auténticas e integra, al igual que las huellas y firmas en el
                    contenidas, asumiendo plena responsabilidad por su contenido. Autorizo a la Caja
                    Promotora de Vivienda Militar y de Policía a verificar y validar el trámite, incluyendo la
                    destinación de los recursos solicitados (subsidio, aportes y cesantías).<br />
                    <br />
                    Declaración juramentada<br />
                    <br />
                    Declaro, bajo la gravedad de juramento, que la información y documentación aportada es
                    veraz y completa, asumiendo plena responsabilidad por su contenido. Autorizo a Caja
                    Honor a verificar y validar el trámite, incluyendo la destinación de los recursos solicitados
                    (subsidio, aportes y cesantías), conforme a los modelos de solución de vivienda vigentes.<br />
                    <br />
                    Reconozco que esta actuación se enmarca en lo dispuesto por la Ley 1952 de 2019 –
                    Código General Disciplinario, en cuanto a la responsabilidad y deberes de los servidores
                    públicos y beneficiarios de recursos del Estado.<br />
                </p>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="acepta"
                        control={formulario.control}
                        render={({ field: { onChange, value }, }) => (
                            <CHCheckbox
                                label="Acepto *"
                                onChange={(e) => {
                                    setAceptarCheckValue(e.target.checked);
                                    onChange(e.target.checked)
                                }}
                                checked={campoSoloLecturaRadicacionAprobada? true : value}
                                disabled={campoSoloLecturaRadicacionAprobada}
                            />
                        )}
                    />
                </Grid>
        </CHPaper>
    )
}

interface TypeProps {
    formulario: UseFormReturn<any>;
    setAceptarCheckValue: (value: boolean) => void;
    campoSoloLecturaRadicacionAprobada: boolean;
}

export default DeclaracionesAutorizaciones;