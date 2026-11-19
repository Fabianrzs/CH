import { Grid, Container } from '@material-ui/core';
import React from 'react';
import { connect } from 'react-redux';
import { startGetNotification } from '../../actions/notification.action';
import { AppState } from '../../store/rootReducer';
import NotificationsTable from './components/NotificationsTable';
import { LoadingComponent } from '../../components/loading/loading.component';

const Notifications = (props?: any) => {
    const { notificationState } = props;
    return (
        <div>
            <Container>
                <LoadingComponent loading={notificationState?.loading} />
                <Grid item xs={6} sm={12}>
                    <NotificationsTable /* load={load} */ />
                </Grid>
            </Container>
        </div>
    );
};

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    notificationState: state.notification,
});

const mapDispatchToProps = {
    startGetNotification,
};

export default connect(mapStateToProps, mapDispatchToProps)(Notifications);
