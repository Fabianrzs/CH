import React, { useEffect } from 'react';
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
import NotificationService from '../../../services/notification.service';
import history from '../../../utils/history.router';

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

type ModalArchivoRespuestaProps = {
    open: boolean;
    documentoURL: string;
    handleClose: () => void;
    onClose: () => void;
    resolver: any;
    estado: any;
    modulo?: string;
    moduloPago: string;
    setVista: () => void;
};

export const ModalArchivoRespuesta = (props: ModalArchivoRespuestaProps) => {
    const { open, documentoURL, handleClose, onClose, resolver, estado, modulo, moduloPago, setVista } = props;
    const classes = useStyles();
    const handleGuardar = async () => {
        // await resolver(true);
        await NotificationService.genericAlert(
            'Procesado',
            'Se ha procesado correctamente la información',
        );
        handleClose();
        setVista();
    };

    const handleCancelar = async () => {
        await onClose()
    };

    // useEffect(() => {
    //     console.log('moduloPago', moduloPago);
    // }, [moduloPago]);
    return (
        <Dialog open={open}>
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
                {(estado === '395' || (modulo === 'DESEMBARGO' && estado === '396') || (moduloPago === 'PAGO EMBARGOS')) && (
                    <Button
                        variant="contained"
                        color="primary"
                        style={{ width: '200px' }}
                        onClick={handleGuardar}
                    >
                        Procesar
                    </Button>
                )}

                <Button
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