import React from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';

export default function AlertDialog(props: any) {
    const { confirmDialog, setConfirmDialog } = props;
    const handleClose = () => {
        setConfirmDialog({ isOpen: false });
    };

    return (
        <div>
            <Dialog
                open={confirmDialog.isOpen}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title">
                    {confirmDialog.title}
                </DialogTitle>
                <DialogContent>
                    <DialogContentText id="alert-dialog-description">
                        {confirmDialog.bodyText}
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} className="titulo1">
                        Cancelar
                    </Button>
                    <Button
                        onClick={confirmDialog.onConfirm}
                        className="titulo2"
                        autoFocus
                    >
                        Aceptar
                    </Button>
                </DialogActions>
            </Dialog>
        </div>
    );
}
