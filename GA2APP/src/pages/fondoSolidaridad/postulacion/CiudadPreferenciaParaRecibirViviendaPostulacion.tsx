import React, { useState } from 'react';
import { Grid, makeStyles, createStyles, MenuItem } from '@material-ui/core';
import { Controller} from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { CITYDTO, DEPARTAMENTODTO } from '../../../types/catalog.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

function CiudadPreferenciaParaRecibirViviendaPostulacion(props: props) {
    const classes = useStyles();
    const {catalogState, 
           formularioPostulacion, 
           ciudadPreferencia1,
           ciudadPreferencia2,
           ciudadPreferencia3,
           camposSolicitudPostulacionSoloLectura,
           obtenerCiudadesPreferencia1,
           obtenerCiudadesPreferencia2,
           obtenerCiudadesPreferencia3,
        } = props;

    return (
        <>
            <CHPaper title="Ciudad de preferencia para recibir su vivienda">
                <p className="paragraphText">
                    Manifiesto que en el evento de ser beneficiario(a) dentro de la presente convocatoria, 
                    mediante la modalidad Adjudicación de Vivienda,{' '} 
                    <b>
                        <u>
                            estoy dispuesto a aceptar una de las viviendas de 
                            CAJA HONOR tenga disponibles en los diferentes 
                            proyectos dirigidos a mi categoría
                        </u>
                    </b>
                    . Si usted llegará a ser beneficiario del fondo de 
                    solidaridad ¿En qué ciudad le gustaría solucionar 
                    su vivienda?. En la casilla 1 diligencie la de mayor 
                    preferencia.
                </p>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento1"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Departamento *"
                                    value={value}
                                    onChange={e => {
                                        obtenerCiudadesPreferencia1(Number(e.target.value));
                                        onChange(e);
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    readOnly={camposSolicitudPostulacionSoloLectura}
                            >
                                {catalogState.departments?.map((option: DEPARTAMENTODTO) => {
                                    if (option.id !== 0) {
                                        return (
                                        <MenuItem
                                            key={option.id}
                                            value={option.id}
                                        >
                                            {option.nombre}
                                        </MenuItem>
                                        );
                                    }
                                    return null;
                                })}
    
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="ciudad1"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                label="Ciudad 1 (de mayor preferencia) *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    readOnly={camposSolicitudPostulacionSoloLectura}
                                >
                            {ciudadPreferencia1?.map((option: CITYDTO) => {
                                if (option.id !== 0) {
                                    return (
                                    <MenuItem
                                        key={option.id}
                                        value={option.id}
                                    >
                                        {option.nombre}
                                    </MenuItem>
                                    );
                                }
                                return null;
                            })}
                                </CHSelect>
                            )}
                         />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento2"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Departamento *"
                                    value={value}
                                    onChange={e => {
                                        obtenerCiudadesPreferencia2(Number(e.target.value));
                                        onChange(e);
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    readOnly={camposSolicitudPostulacionSoloLectura}
                            >
                                {catalogState.departments?.map((option: DEPARTAMENTODTO) => {
                                    if (option.id !== 0) {
                                        return (
                                        <MenuItem
                                            key={option.id}
                                            value={option.id}
                                        >
                                            {option.nombre}
                                        </MenuItem>
                                        );
                                    }
                                    return null;
                                })}
    
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudad2"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Ciudad 2 *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    readOnly={camposSolicitudPostulacionSoloLectura}
                                >
                            {ciudadPreferencia2?.map((option: CITYDTO) => {
                                if (option.id !== 0) {
                                    return (
                                    <MenuItem
                                        key={option.id}
                                        value={option.id}
                                    >
                                        {option.nombre}
                                    </MenuItem>
                                    );
                                }
                                return null;
                            })}

                            </CHSelect>
                           )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento3"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Departamento *"
                                    value={value}
                                    onChange={e => {
                                        obtenerCiudadesPreferencia3(Number(e.target.value));
                                        onChange(e);
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    readOnly={camposSolicitudPostulacionSoloLectura}
                            >
                                {catalogState.departments?.map((option: DEPARTAMENTODTO) => {
                                    if (option.id !== 0) {
                                        return (
                                        <MenuItem
                                            key={option.id}
                                            value={option.id}
                                        >
                                            {option.nombre}
                                        </MenuItem>
                                        );
                                    }
                                    return null;
                                })}
    
                            </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudad3"
                            control={formularioPostulacion.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Ciudad 3 *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    readOnly={camposSolicitudPostulacionSoloLectura}
                                >
                            {ciudadPreferencia3?.map((option: CITYDTO) => {
                                if (option.id !== 0) {
                                    return (
                                    <MenuItem
                                        key={option.id}
                                        value={option.id}
                                    >
                                        {option.nombre}
                                    </MenuItem>
                                    );
                                }
                                return null;
                            })}

                            </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    );
}

interface LinkMapDispatchToProps {
    catalogState: CatalogState;
    formularioPostulacion: any;
    ciudadPreferencia1: CITYDTO[];
    ciudadPreferencia2: CITYDTO[];
    ciudadPreferencia3: CITYDTO[];
    camposSolicitudPostulacionSoloLectura: boolean;
    obtenerCiudadesPreferencia1: (departamento: number) => void;
    obtenerCiudadesPreferencia2: (departamento: number) => void;
    obtenerCiudadesPreferencia3: (departamento: number) => void;
}

type props = LinkMapDispatchToProps;


export default CiudadPreferenciaParaRecibirViviendaPostulacion;
