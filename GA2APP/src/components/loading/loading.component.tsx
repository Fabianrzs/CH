import React from 'react';
import { Backdrop, CircularProgress, makeStyles } from '@material-ui/core';

const useStyles = makeStyles((theme: any) => ({
    backdrop: {
        position: 'fixed',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
        backgroundColor: 'rgba(0, 0, 0, 0.45)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
    },
}));

interface Prop {
    loading: boolean;
}

export const LoadingComponent = ({ loading = false }: Prop) => {
    const classes = useStyles();

    if (!loading) {
        return null;
    }

    return (
        <div className={classes.backdrop}>
            <CircularProgress color="inherit" />
        </div>
    );
};
