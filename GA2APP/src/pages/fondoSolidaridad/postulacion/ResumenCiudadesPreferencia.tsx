import { createStyles, Grid, makeStyles } from '@material-ui/core';
import React, { useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { CITYDTO } from '../../../types/catalog.type';
import ApiCatalog from '../../../services/api-catalog.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

const ResumenCiudadesPreferencia = (props: props) => {

    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const {catalogState, datosSolicitudPostulacion} = props;

    const formularioResumenCiudad = useForm({
        defaultValues: {
            departamento1: '',
            ciudad1: '',
            departamento2: '',
            ciudad2: '',
            departamento3: '',
            ciudad3: '',
        },
    });

    useEffect(() => {
       const departamento1 = catalogState.departments.find(item => item.id === datosSolicitudPostulacion?.departamento1);
       const departamento2 = catalogState.departments.find(item => item.id === datosSolicitudPostulacion?.departameno2);
       const departamento3 = catalogState.departments.find(item => item.id === datosSolicitudPostulacion?.deparmento3);
       formularioResumenCiudad.setValue('departamento1',departamento1?.nombre || '');
       formularioResumenCiudad.setValue('departamento2',departamento2?.nombre || '');
       formularioResumenCiudad.setValue('departamento3',departamento3?.nombre || '');
       obtenerCiudadesPreferencia1(departamento1?.id || 0);
       obtenerCiudadesPreferencia2(departamento2?.id || 0);
       obtenerCiudadesPreferencia3(departamento3?.id || 0);
    },[datosSolicitudPostulacion])

    const obtenerCiudadesPreferencia1 = async (idDepto: number) => {
		const value = idDepto.toString();
		const response = await apiCatalog.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
            const ciudad = datos.find(item => item.id === datosSolicitudPostulacion?.ciuadad1);
            formularioResumenCiudad.setValue('ciudad1',ciudad?.nombre || '');
		}
	};

    const obtenerCiudadesPreferencia2 = async (idDepto: number) => {
		const value = idDepto.toString();
		const response = await apiCatalog.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
            const ciudad = datos.find(item => item.id === datosSolicitudPostulacion?.ciudad2);
            formularioResumenCiudad.setValue('ciudad2',ciudad?.nombre || '');
		}
	};

    const obtenerCiudadesPreferencia3 = async (idDepto: number) => {
		const value = idDepto.toString();
		const response = await apiCatalog.getCityOfState(value);
		if (response.data !== null) {
			const datos = (await response.data) as CITYDTO[];
            const ciudad = datos.find(item => item.id === datosSolicitudPostulacion?.ciudad3);
            formularioResumenCiudad.setValue('ciudad3',ciudad?.nombre || '');
		}
	};
    
    return (
        <>
          <CHPaper title="Ciudad de preferencia para recibir su vivienda">
                <p className="paragraphText">
                    Manifiesto que en el evento de ser adjuticatorio adentro de
                    la presente convocatoria, mediante el mecanismo de solución
                    de vivienda (adjudicación),{' '}
                    <b>
                        <u>
                            estoy dispuesto a aceptar una de las viviendas de
                            CAJA HONOR tenga disponibles en los diferentes
                            proyectos dirigidos a mi categoría
                        </u>
                    </b>
                    . Si usted llegará a ser beneficiario del fondo de
                    solidaridad ¿En que ciudad le gustaría solucionar su
                    vivienda?. En la casilla 1diligencie la de mayor
                    preferencia.
                </p>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento1"
                            control={formularioResumenCiudad.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                error={!!error}
                                label="Departamento"
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                helperText={error ? error.message : null}
                                isReadOnly
                                onChange={onChange}
                        />
                    )}
                />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="ciudad1"
                            control={formularioResumenCiudad.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                            <CHTextField
                                error={!!error}
                                label="Ciudad 1 (de mayor preferencia)"
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                helperText={error ? error.message : null}
                                isReadOnly
                                onChange={onChange}
                          />
                          )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento2"
                            control={formularioResumenCiudad.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                            <CHTextField
                                error={!!error}
                                label="Departamento"
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                helperText={error ? error.message : null}
                                isReadOnly
                                onChange={onChange}
                          />
                          )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudad2"
                            control={formularioResumenCiudad.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                            <CHTextField
                                error={!!error}
                                label="Ciudad 2"
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                helperText={error ? error.message : null}
                                isReadOnly
                                onChange={onChange}
                          />
                          )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="departamento3"
                            control={formularioResumenCiudad.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                            <CHTextField
                                error={!!error}
                                label="Departamento"
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                helperText={error ? error.message : null}
                                isReadOnly
                                onChange={onChange}
                          />
                          )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="ciudad3"
                            control={formularioResumenCiudad.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                            <CHTextField
                                error={!!error}
                                label="Ciudad 3"
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                helperText={error ? error.message : null}
                                isReadOnly
                                onChange={onChange}
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
    catalogState: CatalogState;  
    datosSolicitudPostulacion: any;         
}

export default ResumenCiudadesPreferencia;