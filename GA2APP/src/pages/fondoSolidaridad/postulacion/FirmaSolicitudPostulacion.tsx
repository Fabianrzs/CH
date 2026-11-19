import React from 'react';
import {
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { Controller} from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { TIPO_SUBMODALIDAD } from '../constants/fondoSolidaridad.constants';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        buttonAdd: {
            width: '200px',
            marginRight: '15px',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
    }),
);

function FirmaSolicitudPostulacion(props: props) {
    const classes = useStyles();
    const {catalogState, 
           formularioPostulacion, 
           cliente,
           tipoSubModalidad,
           camposSolicitudPostulacionSoloLectura} = props;

    return (
        <>
            <CHPaper title="Persona que presenta el trámite">
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreFirma"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombres *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    isReadOnly={camposSolicitudPostulacionSoloLectura}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '').substring(0,25)
                                        
                                        );
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="apellidoFirma"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Apellidos *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly={camposSolicitudPostulacionSoloLectura}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '').substring(0,25)
                                        
                                        );
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="firmaSolicitud"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                label="Tipo documento *"
                                value={value}
                                onChange={onChange}
                                error={!!error}
                                readOnly={camposSolicitudPostulacionSoloLectura}
                                helperText={
                                    error ? error.message : null
                                }
                            >
                              {catalogState.IdentificationType?.map(
                                    (option: IDENTIFICATIONTYPEDTO) => (
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
                            name="numeroDocumentoFirma"
                            control={formularioPostulacion.control}
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
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    isReadOnly={camposSolicitudPostulacionSoloLectura}
                                    label="Número documento *"
                                    value={value}
                                    onChange={e => {
										if(e.target.value.length <= 12){
											onChange(e);
										}
									}}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroCelular"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                                pattern:
                                    /\(?([3-3]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    error={!!error}
                                    type="number"
                                    helperText={
                                        error
                                            ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos'
                                            : null
                                    }
                                    isReadOnly={camposSolicitudPostulacionSoloLectura}
                                    label="Número celular *"
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value.length <= 10
                                        ) {
                                            onChange(e);
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    { tipoSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA && (
                         <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="representanteLegal"
                                control={formularioPostulacion.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHCheckbox
                                        label="¿Representante Legal?"
                                        onChange={onChange}
                                        checked={value}
                                        disabled={camposSolicitudPostulacionSoloLectura}
                                    />
                                )}
                            />
                        </Grid>
                    )} 
                    { tipoSubModalidad !== TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA && (
                     <Grid className={classes.column} item xs={12} md={3}>
                         <Controller
                                name="mismoAfiliado"
                                control={formularioPostulacion.control}
                                render={({ field: { onChange, value } }) => (
                                    <CHCheckbox
                                        label="Mismo Afiliado"
                                        onChange={e => {
                                            if(e.target.checked){
                                                formularioPostulacion.setValue('nombreFirma', `${cliente?.primerNombre} ${cliente?.segundoNombre}`);
                                                formularioPostulacion.setValue('apellidoFirma', `${cliente?.primerApellido} ${cliente?.segundoApellido}`);
                                                formularioPostulacion.setValue('firmaSolicitud', cliente?.idTipoIdentificacion);
                                                formularioPostulacion.setValue('numeroDocumentoFirma', cliente?.identificacion);
                                            }else{
                                                formularioPostulacion.setValue('nombreFirma', '');
                                                formularioPostulacion.setValue('apellidoFirma', '');
                                                formularioPostulacion.setValue('firmaSolicitud', '');
                                                formularioPostulacion.setValue('numeroDocumentoFirma', '');
                                            }
                                            onChange(e);
                                        }}
                                        checked={value}
                                        disabled={camposSolicitudPostulacionSoloLectura}
                                    />
                                )}
                            />
                        </Grid>
                     )} 
                </Grid>
            </CHPaper>
        </>
    );
}

interface LinkMapDispatchToProps {
    catalogState: CatalogState;
    formularioPostulacion: any;
    cliente: any;
    tipoSubModalidad: number;
    camposSolicitudPostulacionSoloLectura: boolean;
}

type props = LinkMapDispatchToProps;

export default FirmaSolicitudPostulacion;