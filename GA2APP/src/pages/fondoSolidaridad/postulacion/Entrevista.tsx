import { createStyles, 
         Grid, 
         makeStyles,
         MenuItem} from '@material-ui/core';
import React from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { numeroHabitantes, 
         tipoViviendaPostulacionDto, 
         tipo_vivenda_postulacion,
         viviendaDto,
         posee_vivenda} from '../constants/fondoSolidaridad.constants';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

const Entrevista =  (props: propsType) => {

    const classes = useStyles();
    const {
           formularioEntrevista,
           camposEntrevistaSoloLectura} = props;

    return (
        <>
            <CHPaper title="Información Entrevista">
                    <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="efS_VIVIENDA_PROPIA"
                            control={formularioEntrevista.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposEntrevistaSoloLectura}
                                    label="Posee Vivienda Propia? *"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
								
                                >
									  {posee_vivenda?.map(
                                        (option: viviendaDto) => (
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
                     <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="efS_TIPO_VIVIENDA"
                            control={formularioEntrevista.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposEntrevistaSoloLectura}
                                    label="Tipo de Tenencia de Vivienda *"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {
                                       formularioEntrevista.watch('efS_VIVIENDA_PROPIA') === 'SI' && (
                                        tipo_vivenda_postulacion?.map(
                                            (option: tipoViviendaPostulacionDto) => {
                                                if(option.id !== 'ARRIENDO'){
                                                    return (
                                                        <MenuItem
                                                        key={option.id}
                                                        value={option.id}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                    )
                                                }
                                                return null;
                                            }
                                        ))
                                    }

                                    {
                                       formularioEntrevista.watch('efS_VIVIENDA_PROPIA') === 'NO' && (
                                        tipo_vivenda_postulacion?.map(
                                            (option: tipoViviendaPostulacionDto) => {
                                                if(option.id !== 'PROPIA'){
                                                    return (
                                                        <MenuItem
                                                        key={option.id}
                                                        value={option.id}
                                                    >
                                                        {option.descripcion}
                                                    </MenuItem>
                                                    )
                                                }
                                                return null;
                                            }
                                        ))
                                    }
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="efS_HABITANTE"
                            control={formularioEntrevista.control}
                            rules={{
                                value: true,
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={camposEntrevistaSoloLectura}
                                    label="Cuántos Habitan *"
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
								
                                >
                                    {numeroHabitantes?.map(
                                        option => (
                                            <MenuItem
                                                key={option}
                                                value={option}
                                            >
                                                {option}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
                    <Grid container>
                        <Controller
                            name="efS_INFORMATIVO_ADMIN"
                            control={formularioEntrevista.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={camposEntrevistaSoloLectura}
                                    error={!!error}
                                    label="Informativo Administrativo"
                                    InputLabelProps={{ shrink: true }}
                                    multiline
                                    rows={3}
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid container>
                        <Controller
                            name="efS_CONCEPTO_PSICOSOCIAL"
                            control={formularioEntrevista.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={camposEntrevistaSoloLectura}
                                    error={!!error}
                                    label="Concepto Psicosocial *"
                                    InputLabelProps={{ shrink: true }}
                                    multiline
                                    rows={3}
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </CHPaper>
                <CHPaper title="Observaciones">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="efS_OBSERVACION"
                            control={formularioEntrevista.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={camposEntrevistaSoloLectura}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Observaciones Adicionales *"
                                    value={value}
                                    onChange={onChange}
                                    multiline
                                    rows={2}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    )
}

type propsType = {
    formularioEntrevista: UseFormReturn<any>;
    camposEntrevistaSoloLectura: boolean;
}

export default Entrevista;
