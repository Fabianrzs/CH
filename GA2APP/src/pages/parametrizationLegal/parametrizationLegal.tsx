import { Grid, Container } from '@material-ui/core';
import { connect } from 'react-redux';
import React, { useEffect } from 'react';
import Table from './components/table.component';
import {
    startGetLegalReliefParametrization,
    startGetLegalRateParametrization,
    startGetLegalGmfParametrization,
} from '../../actions/parametrizationLegal.action';
import { AppState } from '../../store/rootReducer';
import { LoginState } from '../../reducers/login.reducer';
import { ParametrizacionLegalState } from '../../reducers/parametrizationLegal.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';

const ParametrizationLegal = (props: Props) => {
    const {loginState, parametrizationLegalState, startGetLegalGmfParametrization, startGetLegalRateParametrization, startGetLegalReliefParametrization } = props;
    const load = () => {
        startGetLegalReliefParametrization(loginState.claims);
        startGetLegalRateParametrization(loginState.claims);
        startGetLegalGmfParametrization(loginState.claims);
    };

    useEffect(() => {
        load();
    }, []);

    return (
        <div>
            <Container>
                <LoadingComponent
                    key="loader"
                    loading={parametrizationLegalState.loading}
                />
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <Table load={load} />
                    </Grid>
                </Grid>
            </Container>
        </div>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    parametrizationLegalState: ParametrizacionLegalState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    parametrizationLegalState: state.parametrizationLegal,
});

const mapDispatchToProps = {
    startGetLegalReliefParametrization,
    startGetLegalRateParametrization,
    startGetLegalGmfParametrization,
};
interface LinkMapDispatchToProps {
    startGetLegalReliefParametrization: (props: string) => void;
    startGetLegalRateParametrization: (props: string) => void;
    startGetLegalGmfParametrization: (props: string) => void;
}

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ParametrizationLegal);
