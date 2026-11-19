import React, { ReactFragment } from 'react';
import { Box, CardContent, Paper } from '@material-ui/core';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';
import './container.application.main.scss';

const ContainerApplication = (props: Props) => {
    const { contentMain } = props;

    return (
        <Box display="flex" justifyContent="center" p={1} m={1}>
            <Box p={1}>
                <Paper elevation={8}>
                    <CardContent>{contentMain.children}</CardContent>
                </Paper>
            </Box>
        </Box>
    );
};

interface ContentMain {
    children: ReactFragment;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
});

const mapDispatchToProps = {};

type Props = {
    contentMain: ContentMain;
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ContainerApplication);
