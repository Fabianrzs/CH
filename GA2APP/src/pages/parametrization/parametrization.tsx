import { Grid, Container } from '@material-ui/core';
import { connect } from 'react-redux';
import React, { useEffect } from 'react';
import Insurance from './components/insurance.component';
import Rate from './components/rate.component';
import Term from './components/term.component';
import Settlement from './components/settlement.component';
import {
    startGetRateParametrization,
    startGetInsuranceParametrization,
    startGetTermParametrization,
    startObtenerParametrosTransaccion,
    startGetSettlementParametrization,
} from '../../actions/parametrization.action';
import { AppState } from '../../store/rootReducer';
import { LoginState } from '../../reducers/login.reducer';
import { ParametrizacionState } from '../../reducers/parametrization.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';

const Parametrization = (props: Props) => {
    const { parametrizationState,
        loginState,
        startGetRateParametrization,
        startGetInsuranceParametrization,
        startGetTermParametrization,
        startObtenerParametrosTransaccion,
        startGetSettlementParametrization,
    
    
    } = props;
    const load = () => {
        startGetRateParametrization(loginState.claims);
        startGetInsuranceParametrization(loginState.claims);
        startGetTermParametrization(loginState.claims);
        startObtenerParametrosTransaccion(loginState.claims);
        startGetSettlementParametrization(loginState.claims);
    };

    useEffect(() => {
        load();
    }, []);

    return (
        <div>
            <Container>
                <LoadingComponent
                    key="loader"
                    loading={parametrizationState.loading}
                />
                <Grid container spacing={3}>
                    <Grid item xs={6}>
                        <Rate />
                    </Grid>
                    <Grid item xs={6}>
                        <Term />
                    </Grid>
                    <Grid item xs={6}>
                        <Insurance />
                    </Grid>
                    <Grid item xs={6}>
                        <Settlement />
                    </Grid>
                </Grid>
            </Container>
        </div>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    parametrizationState: ParametrizacionState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    parametrizationState: state.parametrization,
});

const mapDispatchToProps = {
    startGetRateParametrization,
    startGetInsuranceParametrization,
    startGetTermParametrization,
    startObtenerParametrosTransaccion,
    startGetSettlementParametrization,
};
interface LinkMapDispatchToProps {
    startGetInsuranceParametrization: (props: string) => void;
    startGetRateParametrization: (props: string) => void;
    startGetTermParametrization: (props: string) => void;
    startGetSettlementParametrization: (props: string) => void;
    startObtenerParametrosTransaccion: (props: string) => void;
}

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Parametrization);
