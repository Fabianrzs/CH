import { Alert, Color } from '@material-ui/lab';
import { Snackbar } from '@material-ui/core';
import React from 'react';

function CHSnackBar({
    isOpen,
    handleClose,
    message,
    type,
}: {
    message: string;
    type: Color;
    isOpen: boolean;
    handleClose: () => void;
}) {
    return (
        <Snackbar open={isOpen} autoHideDuration={6000} onClose={handleClose}>
            <Alert onClose={handleClose} severity={type}>
                {message}
            </Alert>
        </Snackbar>
    );
}

export default CHSnackBar;
