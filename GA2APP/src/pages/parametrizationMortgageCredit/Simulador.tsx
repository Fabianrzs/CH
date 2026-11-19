import { Container, Grid } from '@material-ui/core';
import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { startGetAllSimuladorTasas } from '../../actions/simulador.action';
import { AppState } from '../../store/rootReducer';
import { LoginState } from '../../reducers/login.reducer';
import { SimuladorState } from '../../reducers/simulador.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';
import ParametrizationMortgageCredit from './components/ParametrizationMortgageCredit';

const Simulador = (props: Props) => {
const {simuladorState, loginState, startGetAllSimuladorTasas} = props;
const load = () => {
    startGetAllSimuladorTasas(loginState.claims);
};

useEffect(() => {
    load();
}, []);

    return (
        <div>
            <Container>
                <LoadingComponent loading={simuladorState?.loading} />
                <Grid container spacing={3}>
                    <Grid item xs={6} sm={12}>
                        <ParametrizationMortgageCredit load={load} />
                    </Grid>
                </Grid>
            </Container> 
        </div>
    )
}
interface LinkMapStateToProps {
    loginState: LoginState;
    simuladorState: SimuladorState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    simuladorState: state.simulador,
});

interface LinkMapDispatchToProps {
    startGetAllSimuladorTasas: (props: string) => void;
}

const mapDispatchToProps = {
    startGetAllSimuladorTasas,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Simulador);


