import {
    Box,
    Button,
    CardActions,
    CardContent,
    CardHeader,
    Paper,
} from '@material-ui/core';
import React from 'react';
import './container.application.main.scss';

export const ContainerApplicationHeader = (props?: any) => {
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

export const ContainerApplication = (props?: any) => {
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
