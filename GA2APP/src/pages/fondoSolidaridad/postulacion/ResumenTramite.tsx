import { createStyles, Grid, makeStyles } from '@material-ui/core';
import React, { useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { TIPO_SUBMODALIDAD } from '../constants/fondoSolidaridad.constants';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

const ResumenTramite = (props: props) => {
    const classes = useStyles();
    const {catalogState, datosSolicitudPostulacion} = props;

    const formularioTramite = useForm({
        defaultValues: {
            nombreFirma: '',
            apellidoFirma: '',
            firmaSolicitud: '',
            numeroDocumentoFirma: '',
            numeroCelular: '',
            representanteLegal: false,
        },
    });

    useEffect(() => {
        const tipoDocumento = catalogState.IdentificationType.find(item => item.id === datosSolicitudPostulacion?.firmaSolicitud);
        formularioTramite.setValue('nombreFirma', datosSolicitudPostulacion?.nombreFirma);
        formularioTramite.setValue('apellidoFirma', datosSolicitudPostulacion?.apellidoFirma);
        formularioTramite.setValue('firmaSolicitud', tipoDocumento?.descripcion || '');
        formularioTramite.setValue('numeroDocumentoFirma',datosSolicitudPostulacion?.numeroDocumentoFirma);
        formularioTramite.setValue('numeroCelular',datosSolicitudPostulacion?.numeroCelular);
        formularioTramite.setValue('representanteLegal',datosSolicitudPostulacion?.representanteLegal);
     },[datosSolicitudPostulacion])
 
    return (
        <>
         <CHPaper title="Persona que presenta el trámite">
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreFirma"
                            control={formularioTramite.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombres"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    isReadOnly
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="apellidoFirma"
                            control={formularioTramite.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Apellidos"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="firmaSolicitud"
                            control={formularioTramite.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                label="Tipo Documento"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                isReadOnly
                                inputProps={{
                                    style: {
                                        textTransform: 'uppercase',
                                    },
                                }}
                            />
                        )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroDocumentoFirma"
                            control={formularioTramite.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                minLength: {
                                    value: 4,
                                    message:
                                        'La longitud mínima del campo es de 4 caracteres',
                                },
                                maxLength: {
                                    value: 12,
                                    message:
                                        'La longitud máxima del campo es de 12 caracteres',
                                },
                            }}
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly
                                    label="Número documento"
                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroCelular"
                            control={formularioTramite.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                pattern:
                                    /\(?([3-3]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                            }}
                            render={({
                                field: { value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    error={!!error}
                                    helperText={
                                        error
                                            ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos'
                                            : null
                                    }
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly
                                    label="Número celular"
                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                    { datosSolicitudPostulacion?.idSubmodalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA && (
                        <Grid className={classes.column} item xs={12} md={4}>
                         <Controller
                                name="representanteLegal"
                                control={formularioTramite.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHCheckbox
                                        label="¿Representante Legal?"
                                        onChange={onChange}
                                        checked={value}
                                        disabled
                                    />
                                )}
                            />
                        </Grid>
                     )}
                </Grid>
            </CHPaper>
        </>
    )
}

type props = {
    catalogState: CatalogState;
    datosSolicitudPostulacion: any;         
}

export default ResumenTramite;