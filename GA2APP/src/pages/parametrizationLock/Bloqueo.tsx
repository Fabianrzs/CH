import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { Grid } from '@material-ui/core';
import { AppState } from '../../store/rootReducer';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import TableBloqueo from './TableBloqueo';
import { startGetLock } from '../../actions/lock.action';

const Bloqueo = (props: props) => {
    const { startGetLock } = props;

    const load = async () => {
        await startGetLock();
    };

    useEffect(() => {
        load();
    }, []);

    return (
        <CHPrivateContainer>
            <CHPaper title="Módulo Bloqueos">
                <Grid container>
                    <TableBloqueo load={load} />
                </Grid>
            </CHPaper>
        </CHPrivateContainer>
    );
};

const mapStateToProps = (state: AppState) => ({
    lockState: state.lock,
});

interface LinkMapDispatchToProps {
    startGetLock: () => void;
}

const mapDispatchToProps = {
    startGetLock,
};

type props = LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Bloqueo);
