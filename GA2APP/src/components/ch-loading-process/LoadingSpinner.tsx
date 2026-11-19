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
            padding: '2rem 0',
        },
    }),
);

function LoadingSpinner(props: {size?: number}) {
    const classes = useStyle();
    const {size} = props;

    return (
        <div className={classes.container}>
            <CircularProgress size={size ?? 30} />
        </div>
    );
}

export default LoadingSpinner;
