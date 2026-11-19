import { Grid, Container } from '@material-ui/core';
import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { startGetLock } from '../../actions/lock.action';
import { AppState } from '../../store/rootReducer';
import { LoginState } from '../../reducers/login.reducer';
import { LockState } from '../../reducers/lock.reducer';
import { LoadingComponent } from '../../components/loading/loading.component';
import LocksTable from './components/LocksTable';

const Locks = (props: Props) => {
    const { startGetLock, lockState, loginState } = props;
    const load = () => {
        startGetLock(loginState.claims);
    };

    useEffect(() => {
        load();
    }, []);
    return (
        <div>
            <Container>
                <LoadingComponent loading={lockState?.loading} />
                <Grid container spacing={3}>
                    <Grid item xs={6} sm={12}>
                        <LocksTable load={load} />
                    </Grid>
                </Grid>
            </Container>
        </div>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    lockState: LockState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    lockState: state.lock,
});

interface LinkMapDispatchToProps {
    startGetLock: (props: string) => void;
}

const mapDispatchToProps = {
    startGetLock,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Locks);
