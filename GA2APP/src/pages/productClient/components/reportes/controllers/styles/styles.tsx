import { createStyles, makeStyles } from '@material-ui/core';

const controllerStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        }
    }),
);

export default controllerStyles;