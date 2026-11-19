import React from 'react';
import { CircularProgress } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';

const useStyle = makeStyles(() =>
    createStyles({
        container: {
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            padding: '10rem 0',
        },
    }),
);

function CHLoadingProcess() {
    const classes = useStyle();
    return (
        <div className={classes.container}>
            <CircularProgress />
        </div>
    );
}

export default CHLoadingProcess;
