import { Grid, Container } from '@material-ui/core';
import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { startGetState } from '../../actions/state.action';
import { AppState } from '../../store/rootReducer';
import { LoginState } from '../../reducers/login.reducer';
import { StateState } from '../../reducers/state.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';
import StateTable from './components/stateTable.component';

const State = (props: Props) => {
    const { stateState, loginState, startGetState } = props;
    const load = () => {
        startGetState(loginState.claims);
    };

    useEffect(() => {
        load();
    }, []);
    return (
        <div>
            <Container>
                <LoadingComponent loading={stateState?.loading} />
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <StateTable load={load} />
                    </Grid>
                </Grid>
            </Container>
        </div>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    stateState: StateState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    stateState: state.state,
});

interface LinkMapDispatchToProps {
    startGetState: (props: string) => void;
}

const mapDispatchToProps = {
    startGetState,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(State);
