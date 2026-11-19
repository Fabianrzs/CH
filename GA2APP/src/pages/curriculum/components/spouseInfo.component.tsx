import React from 'react';
import { connect } from 'react-redux';
import { Grid, MenuItem, makeStyles, createStyles } from '@material-ui/core';
import { Controller } from 'react-hook-form';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

function SpouseInformation({
    catalogState,
    control,
    estadoCivil,
    idTipoIdentificacionConyuge,
}: {
    catalogState: CatalogState;
    control: any;
    estadoCivil: string;
    idTipoIdentificacionConyuge: number | null;
}) {
    const classes = useStyles();

    const enableSpouse = estadoCivil === 'C' || estadoCivil === 'U';
    const cedulaCiudadania = catalogState.IdentificationType.find(
        (item: any) => item.descripcion === 'CÉDULA DE CIUDADANÍA'
    );

    const documentoGenerico = cedulaCiudadania?.descripcion;



    return (

        <CHPaper title="Información de cónyuge">
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        rules={{
                            required: {
                                value: enableSpouse,
                                message: 'Este campo es requerido',
                            },
                        }}
                        control={control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            // <CHSelect
                            //     error={!!error}
                            //     helperText={error ? error.message : null}
                            //     readOnly
                            //     defaultValue={documentoGenerico}
                            //     label="Tipo de documento"
                            //     value={documentoGenerico}
                            //     onChange={onChange}
                            // >
                            //     {catalogState.IdentificationType.map(
                            //         (option: any) => (
                            //             <MenuItem
                            //                 key={option.id}
                            //                 value={option.id}
                            //             >
                            //                 {option.descripcion}
                            //             </MenuItem>
                            //         ),
                            //     )}
                            // </CHSelect>
                            <CHTextField

                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly
                                label="Tipo de documento"
                                value={documentoGenerico}
                                onChange={onChange}
                            />
                        )}
                        name="idTipoIdentificacionConyuge"
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    {idTipoIdentificacionConyuge === 1 || idTipoIdentificacionConyuge === 2 ? (
                        <Controller
                            name="identificacionConyuge"
                            rules={{
                                required: {
                                    value: enableSpouse,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            control={control}
                            defaultValue=""
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField

                                    error={enableSpouse && !!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Identificación cónyuge"
                                    value={value}
                                    onChange={onChange}
                                    type="number"
                                />
                            )}
                        />
                    ) : (
                        <Controller
                            name="identificacionConyuge"
                            rules={{
                                required: {
                                    value: enableSpouse,
                                    message: 'Este campo es requerido',
                                },
                            }}
                            control={control}
                            defaultValue=""
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField

                                    error={enableSpouse && !!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    label="Identificación cónyuge"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    )}
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="primerNombreConyuge"
                        rules={{
                            required: {
                                value: enableSpouse,
                                message: 'Este campo es requerido',
                            },
                            pattern: /^[ A-zÀ-ú ]+$/g
                        }}
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField

                                error={enableSpouse && !!error}
                                helperText={error ? 'El nombre solo debe contener letras' : null}
                                isReadOnly
                                label="Primer nombre cónyuge"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="segundoNombreConyuge"
                        rules={{
                            pattern: /^[ A-zÀ-ú ]+$/g
                        }}
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField

                                error={enableSpouse && !!error}
                                helperText={error ? 'El nombre debe contener solo letras' : null}
                                isReadOnly
                                label="Segundo nombre cónyuge"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="primerApellidoConyuge"
                        rules={{
                            required: {
                                value: enableSpouse,
                                message: 'Este campo es requerido',
                            },
                            pattern: /^[ A-zÀ-ú ]+$/g
                        }}
                        control={control}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField

                                error={enableSpouse && !!error}
                                helperText={error ? 'El apellido debe contener solo letras' : null}
                                isReadOnly
                                label="Primer apellido cónyuge"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="segundoApellidoConyuge"
                        control={control}
                        rules={{
                            pattern: /^[ A-zÀ-ú ]+$/g
                        }}
                        defaultValue=""
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField

                                error={enableSpouse && !!error}
                                helperText={error ? 'El apellido debe contener solo letras' : null}
                                isReadOnly
                                label="Segundo apellido cónyuge"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
            </Grid>
        </CHPaper>

    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
});

export default connect(mapStateToProps)(SpouseInformation);
