import {
    Box,
    Button,
    CardActions,
    CardContent,
    CardHeader,
    Paper,
} from '@material-ui/core';
import React, { ReactFragment } from 'react';
import { connect } from 'react-redux';
import { AppState } from '../../store/rootReducer';
import './container.application.main.scss';

const ContainerApplicationHeader = (props: Props) => {
    const { contentMain } = props;
    return (
        <Box display="flex" justifyContent="center" p={1} m={1}>
            <Box p={1}>
                <Paper elevation={8}>
                    <CardHeader
                        title={contentMain.title}
                        className="header-container-application-main"
                    />
                    <CardContent>{contentMain.children}</CardContent>
                    <CardActions>
                        <Button variant="contained" color="primary">
                            Confirmar
                        </Button>
                    </CardActions>
                </Paper>
            </Box>
        </Box>
    );
};

interface ContentMain {
    title: string;
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
)(ContainerApplicationHeader);
