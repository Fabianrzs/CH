import React, { useEffect, useState } from 'react';
import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller } from 'react-hook-form';
import { sortBy } from 'lodash';
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
import { CatalogState } from '../../../../reducers/catalog.reducer';
import {
    GRUPOSINCONSITENCIASDTO,
    HERRAMIENTAINCONSISTENCIADTO,
    INCONSISTENCIADTO,
    PUNTOSATENCIONDTO,
} from '../../../../types/catalog.type';
import apiCatalog from '../../../../services/api-catalog.service';
import apiSolicitud from '../../../../services/api-solicitud.service';
import { LoginState } from '../../../../reducers/login.reducer';
import { UserState } from '../../../../reducers/user.reducer';
import { User } from '../../../../types/user.type';
// import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import { FormularioInconsistenciasDto } from '../../../../models/requests/solicitud-tarea.model';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';

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

function FormularioInconsistencias({
    catalogState,
    loginState,
    userState,
    formularioInfoInconsistencias,
    idSolicitud,
    obtenerGrupos,
    obtenerHerramientas,
    obtenerPuntosAtencion,
    obtenerUsuarios,
    setEstadoSubsanar,
}: {
    catalogState: CatalogState;
    loginState: LoginState;
    userState: UserState;
    formularioInfoInconsistencias: any;
    idSolicitud: string;
    obtenerGrupos: () => void;
    obtenerHerramientas: () => void;
    obtenerPuntosAtencion: () => void;
    obtenerUsuarios: (token: string) => void;
    setEstadoSubsanar: (value: boolean) => void;
}) {
    const classes = useStyles();
    const api = apiCatalog();
    const apiSolicitudes = apiSolicitud();
    const [inconsistencias, setInconsistencias] = useState<INCONSISTENCIADTO[]>(
        [],
    );
    const [info, setInfo] = useState<FormularioInconsistenciasDto[]>([]);

    useEffect(() => {
        obtenerGrupos();
        obtenerHerramientas();
        obtenerUsuarios(loginState.claims);
        obtenerPuntosAtencion();
        if (
            formularioInfoInconsistencias.getValues('grupoInconsistencia') !== 0
        ) {
            obtenerInconsistencias(
                formularioInfoInconsistencias.getValues('grupoInconsistencia'),
            );
        } 
    }, []);

    useEffect(() => {

        if (info.length > 0) {
            if (
                formularioInfoInconsistencias.getValues(
                    'grupoInconsistencia',
                ) !== 0
            ) {
                obtenerInconsistencias(
                    formularioInfoInconsistencias.getValues(
                        'grupoInconsistencia',
                    ),
                );
            }
            formularioInfoInconsistencias.setValue(
                'grupoInconsistencia',
                info[0].grupoInconsistencia,
            );
            formularioInfoInconsistencias.setValue(
                'listaInconsistenciaId',
                info[0].listaInconsistenciaId,
            );
            formularioInfoInconsistencias.setValue(
                'herramientaId',
                info[0].herramientaId,
            );
            formularioInfoInconsistencias.setValue(
                'puntoAtencionId',
                info[0].puntoAtencionId,
            );
            formularioInfoInconsistencias.setValue(
                'solicitudId',
                info[0].solicitudId,
            );
            formularioInfoInconsistencias.setValue(
                'tecnicoId',
                info[0].tecnicoId,
            );
            formularioInfoInconsistencias.setValue(
                'objetoEstudio',
                info[0].objetoEstudio,
            );
            formularioInfoInconsistencias.setValue(
                'analisis',
                info[0].analisis,
            );
            formularioInfoInconsistencias.setValue(
                'observacion',
                info[0].observacion,
            );
            // setValue('subsanado', info[0].subsanado);
            // setValue('estado', info[0].estado);
        }
    }, [info]);

    const obtenerInconsistencias = async (grupo: number) => {
        await api
            .getInconsistenciesByGroup(grupo)
            .then(async response => {
                const datos = response.data as INCONSISTENCIADTO[];
                setInconsistencias(datos);
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerFormularioInconsistencia = async (idSolicitud: string) => {
        await apiSolicitudes
            .GetConsultarInconsistencia(idSolicitud)
            .then(async response => {
                if (response) {
                    if (response.isSuccess) {
                        setInfo(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    return (
        <>
            <CHPaper title="Formulario de Inconsistencias">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="grupoInconsistencia"
                            control={formularioInfoInconsistencias.control}
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
                                    label="Novedad"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={e => {
                                        obtenerInconsistencias(
                                            Number(e.target.value),
                                        );
                                        onChange(e);
                                    }}
                                >
                                    {catalogState.gruposInconsistencias?.map(
                                        (option: GRUPOSINCONSITENCIASDTO) => (
                                            <MenuItem
                                                key={option.idGrupo}
                                                value={option.grupo}
                                            >
                                                {option.grupo}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="listaInconsistenciaId"
                            control={formularioInfoInconsistencias.control}
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
                                    label="Tipo de Inconsistencia"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {inconsistencias?.map(
                                        (option: INCONSISTENCIADTO) => (
                                            <MenuItem
                                                key={
                                                    option.idTipoInconsistencia
                                                }
                                                value={
                                                    option.idTipoInconsistencia
                                                }
                                            >
                                                {
                                                    option.descripcionInconsistencia
                                                }
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="herramientaId"
                            control={formularioInfoInconsistencias.control}
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
                                    label="Herramienta Utilizada"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {sortBy(
                                        catalogState?.herramientasInconsistencia,
                                        'herramientaDescripcion',
                                    ).map(
                                        (
                                            option: HERRAMIENTAINCONSISTENCIADTO,
                                        ) => (
                                            <MenuItem
                                                key={option.idHerramienta}
                                                value={option.idHerramienta}
                                            >
                                                {option.herramientaDescripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="objetoEstudio"
                            control={formularioInfoInconsistencias.control}
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
                                    label="Objeto de Estudio"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="analisis"
                            control={formularioInfoInconsistencias.control}
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
                                    label="Análisis Realizado"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tecnicoId"
                            control={formularioInfoInconsistencias.control}
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
                                    label="Técnico que reportó"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {sortBy(userState?.users, 'nombre').map(
                                        (option: User) => (
                                            <MenuItem
                                                key={option.id}
                                                value={option.id}
                                            >
                                                {`${option.nombre.toLocaleUpperCase()}
                                              ${
                                                  option.segundoNombre !== null
                                                      ? option.segundoNombre.toLocaleUpperCase()
                                                      : ''
                                              }
                                              ${option.apellido.toLocaleUpperCase()}
                                              ${
                                                  option.segundoApellido !==
                                                  null
                                                      ? option.segundoApellido.toLocaleUpperCase()
                                                      : ''
                                              }`}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="puntoAtencionId"
                            control={formularioInfoInconsistencias.control}
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
                                    label="Punto de Atención"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {catalogState.puntosAtencion?.map(
                                        (option: PUNTOSATENCIONDTO) => (
                                            <MenuItem
                                                key={option.idPuntosAtencion}
                                                value={option.idPuntosAtencion}
                                            >
                                                {option.puntosAtencion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="observacion"
                            control={formularioInfoInconsistencias.control}
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
                                    label="Observación"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="subsanado"
                            control={formularioInfoInconsistencias.control}
                            render={({ field: { onChange, value } }) => (
                                <CHCheckbox
                                    label="Subsanado"
                                    onChange={e => {
                                        if (e.target.checked) {
                                            setEstadoSubsanar(false);
                                        } else {
                                            setEstadoSubsanar(true);
                                        }
                                        onChange(e);
                                    }}
                                    checked={value}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
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
)(FormularioInconsistencias);
