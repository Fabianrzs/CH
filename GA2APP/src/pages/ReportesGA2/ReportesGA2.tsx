import React, { useEffect, useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    MenuItem
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../components/ch-select/CHSelect';
import { LoadingComponent } from '../../components/loading/loading.component';
import CatalogApi from '../../services/api-catalog.service';
import ConsultaAfiliados from './components/ConsultaAfiliados';
import ConsultaAfiliadosPorFuerza from './components/ConsultaAfiliadosPorFuerza';
import ConsultaDocumentos from './components/ConsultaDocumentos';
import ConsultaDatosBasicosAfiliado from './components/ConsultaDatosBasicosAfiliado';
import ConsolidadoFuerza from './components/ConsolidadoFuerza';
import Solicitudes from './components/Solicitudes';
import Inconsistencias from './components/Inconsistencias';
import SolicitudesDiarias from './components/SolicitudesDiarias';
import SaldoCuentas from './components/SaldoCuentas';
import NovedadesUnidadesEjecutoras from './components/NovedadesUnidadesEjecutoras';
import Movimientos from './components/Movimientos';
import ReporteRCredito from '../productClient/components/reportes/reporteRCredito/ReporteRCredito';
import ConsultaAfiliadosDevolucionCesantias from './components/ConsultaDevolucionCesantias';

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
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        button2: {
            width: '100%',
            marginTop: '1.5rem',
            color: '#fff',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px px',
        },
        accountButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 8px',
        },

        buttonNoObl: {
            width: '17rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
        buttonTotalSaldos: {
            width: '17rem',
            marginLeft: '31rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
        buttonTotalProvision: {
            width: '17rem',
            marginLeft: '1.5rem',
            marginTop: '1.5rem',
            color: '#1C3F8B',
        },
    }),
);

const apiCatalogs = CatalogApi();

const ConsultaReportesGA2 = () => {
    const classes = useStyles();

    useEffect(() => {
        getReportType();
    }, [])

    const [reportType, setReportType] = useState([]);
    const [loading, setLoading] = useState(false);

    const { control, handleSubmit, watch } = useForm<any>({
        defaultValues: {
            reportTypeId: 0,
        }
    });

    const getReportType = () => {
        apiCatalogs.ObtenerTipoReporteGA2()
            .then((response) => {
                setReportType(response.data)
            })
    }

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Reportes GA2"
                subText=""
                title1="Generación de Reportes"
                title2="GA2"
            />
            <CHPaper title="Generación reportes GA2">
                <LoadingComponent key="loader" loading={loading} />
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo Reporte"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem key={0} value={0}>Seleccione el tipo de reporte</MenuItem>
                                    {reportType.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="reportTypeId"
                        />
                    </Grid>
                </Grid>
            </CHPaper>
            {watch('reportTypeId') === 1 ? (
                <ConsultaAfiliados />
            ): null}
            {watch('reportTypeId') === 2 ? (
                <ConsultaAfiliadosPorFuerza />
            ): null}
            {watch('reportTypeId') === 3 ? (
                <ConsultaDatosBasicosAfiliado />
            ): null}
            {watch('reportTypeId') === 4 ? (
                <ConsultaDocumentos />
            ): null}
            {watch('reportTypeId') === 5 ? (
                <ConsolidadoFuerza />
            ): null}
             {watch('reportTypeId') === 6 ? (
                <ConsultaAfiliadosDevolucionCesantias />
            ): null}
           
            {watch('reportTypeId') === 7 ? (
                <SolicitudesDiarias />
            ): null}
            {watch('reportTypeId') === 8 ? (
                <SaldoCuentas />
            ): null}
            {watch('reportTypeId') === 9 ? (
                 <Solicitudes />
            ): null}
            {watch('reportTypeId') === 10 ? (
                <NovedadesUnidadesEjecutoras />
            ): null}
            {watch('reportTypeId') === 11 ? (
                <Movimientos />
            ): null}
            {watch('reportTypeId') === 12 ? (
                <ReporteRCredito />
            ): null}
             {watch('reportTypeId') === 13 ? (
                <Inconsistencias />
            ): null}
           
        </CHPrivateContainer>
    );
};

export default ConsultaReportesGA2;
