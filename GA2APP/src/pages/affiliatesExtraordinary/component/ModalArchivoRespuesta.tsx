import React from 'react';
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
} from '@material-ui/core';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

export const ModalArchivoRespuesta = (props: any) => {
    const { open, documentoURL, handleClose, onClose, resolver } = props;
    const classes = useStyles();

    const handleGuardar = async () => {
        await resolver(true);        
       await handleClose();
    };

    const handleCancelar = async () => {
        await resolver(false);
        await onClose()
    };

    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogContent style={{ width: '600px' }}>
                <Grid item>
                    <br />
                    <Typography
                        className={classes.titleform}
                        variant="h5"
                        component="h6"
                    >
                        OFICIO DE RESPUESTA
                    </Typography>
                </Grid>
                <iframe src={documentoURL} width="100%" height="800" title="Documento" />
            </DialogContent>
            <DialogActions>
                <Button className={classes.button}
                    variant="contained"
                    color="primary"
                    style={{ width: '200px' }}
                    onClick={handleGuardar} >
                    Guardar
                </Button>
                <Button className={classes.button}
                    variant="contained"
                    color="primary"
                    style={{ width: '200px' }}
                    onClick={handleCancelar} >
                    Cancelar
                </Button>
            </DialogActions>
        </Dialog>
    );
};
