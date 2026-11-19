import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { startGetAllExecutingUnits, startGetAllIdentificationTypes, startGetAllTypesForces, startGetCategory } from '../../../actions/catalog.action';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import ReporteFSPorEstadoTramite from './ReporteFSPorEstado';
import ReporteModeloFS from './ReporteModeloFondoSolidaridad';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
	}),
);

const ReportesFondoSolidaridad = (props: Props) => {

    const classes = useStyles();
    const {loginState, 
           catalogState, 
           startGetAllTypesForces,
           startGetAllExecutingUnits,
           startGetAllIdentificationTypes,
           startGetCategory} = props;
    const [mostrarReporteModelo, setMostrarReporteModelo] = useState(false);
    const [mostrarReporteTramiteEstado, setMostrarReporteTramiteEstado] = useState(false);
    const formularioReportes = useForm({
        defaultValues: {
			reporte: '',
		},
    })

    useEffect(() => {
        startGetAllTypesForces(loginState.claims);
        startGetAllExecutingUnits(loginState.claims);
        startGetAllIdentificationTypes(loginState.claims);
        startGetCategory(loginState.claims);
    }, []);

    return (
        <>
          <CHPrivateContainer>
                <CHPageTitle
                    topText="Gestión de Reportes"
                    subText=""
                    title1="Reportes"
                    title2="Fondo de Solidaridad"
			    />
                <CHPaper title="Generación Reportes">
                    <Grid className={classes.column} item xs={12} md={4}>
						<Controller
							name="reporte"
							control={formularioReportes.control}
							render={({
								field: { onChange, value },
								fieldState: { error },
							}) => (
								<CHSelect
									defaultValue=""
									label="Tipo Reporte"
									value={value}
									onChange={(e) => {
                                        if(e.target.value === 0){
                                            setMostrarReporteModelo(false);
                                            setMostrarReporteTramiteEstado(false);
                                        }
                                        else if(e.target.value === 1){
                                            setMostrarReporteModelo(true);
                                            setMostrarReporteTramiteEstado(false);
                                        }else if(e.target.value === 2){
                                            setMostrarReporteModelo(false);
                                            setMostrarReporteTramiteEstado(true);
                                        }
                                        onChange(e);
                                    }}
								>
									<MenuItem key={0} value={0} >
                                               SELECCIONE...
                                    </MenuItem>
                                    <MenuItem key={1} value={1} >
                                               MODELO FONDO SOLIDARIDAD
                                    </MenuItem>
                                    <MenuItem key={2} value={2} >
                                               FONDO SOLIDARIDAD POR ESTADO DE TRÁMITE
                                    </MenuItem>
								</CHSelect>
							)}
						 />
					</Grid>
                </CHPaper>
                {mostrarReporteModelo && (
                    <ReporteModeloFS 
                      catalogState={catalogState}
                    />
                )}
                {mostrarReporteTramiteEstado && (
                    <ReporteFSPorEstadoTramite 
                      catalogState={catalogState}
                    />
                )}
          </CHPrivateContainer>
        </>
    )
}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    startGetAllTypesForces: (claims: string) => void;
    startGetAllExecutingUnits: (claims: string) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetCategory: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllTypesForces,
    startGetAllExecutingUnits,
    startGetAllIdentificationTypes,
    startGetCategory
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ReportesFondoSolidaridad);