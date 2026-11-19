import React from 'react';
import { Backdrop, CircularProgress, makeStyles } from '@material-ui/core';

const useStyles = makeStyles(theme => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    },
    root: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
}));

interface Prop {
    loading: boolean;
    tittle: string;
}

export const LoadingV2Component = (props: Prop) => {
    const { loading = false, tittle = '' } = props;
    const classes = useStyles();
    const refWrapper = React.createRef();

    return (
        <Backdrop ref={refWrapper} className={classes.backdrop} open={loading}>
            <div className={classes.root}>
                <div>
                    <CircularProgress color="inherit" />
                </div>
                <br />
                <div>
                    <h3>
                        {tittle}
                    </h3>
                </div>
            </div>
        </Backdrop>
    );
};
