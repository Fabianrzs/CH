import React, {
    useEffect,
    useRef,
    useState,
} from 'react';
import {
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Typography,
} from '@material-ui/core';
import { toast } from 'react-toastify';

type ModalPazSalvoProps = {
    open: boolean;
    loading: boolean;
    onClose: () => void;
    onCargar: (archivo: File) => Promise<void>;
};

const ModalPazSalvo = ({
    open,
    loading,
    onClose,
    onCargar,
}: ModalPazSalvoProps) => {
    const [
        archivoSeleccionado,
        setArchivoSeleccionado,
    ] = useState<File | null>(null);

    const inputArchivoRef =
        useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        if (!open) {
            setArchivoSeleccionado(null);

            if (inputArchivoRef.current) {
                inputArchivoRef.current.value = '';
            }
        }
    }, [open]);

    const abrirSelectorArchivo = () => {
        if (loading) {
            return;
        }

        inputArchivoRef.current?.click();
    };

    const seleccionarArchivo = (
        event: React.ChangeEvent<HTMLInputElement>,
    ) => {
        const archivo = event.target.files?.[0];

        if (!archivo) {
            setArchivoSeleccionado(null);
            return;
        }

        const extension = archivo.name
            .split('.')
            .pop()
            ?.toLowerCase();

        if (
            archivo.type !== 'application/pdf' &&
            extension !== 'pdf'
        ) {
            toast(
                'El documento de paz y salvo debe ser un archivo PDF',
                {
                    type: toast.TYPE.WARNING,
                },
            );

            event.target.value = '';
            setArchivoSeleccionado(null);

            return;
        }

        setArchivoSeleccionado(archivo);
    };

    const cargarArchivo = async () => {
        if (!archivoSeleccionado) {
            toast(
                'Debe seleccionar el documento de paz y salvo',
                {
                    type: toast.TYPE.WARNING,
                },
            );

            return;
        }

        await onCargar(archivoSeleccionado);
    };

    const cerrarModal = () => {
        if (loading) {
            return;
        }

        setArchivoSeleccionado(null);

        if (inputArchivoRef.current) {
            inputArchivoRef.current.value = '';
        }

        onClose();
    };

    return (
        <Dialog
            open={open}
            onClose={cerrarModal}
            fullWidth
            maxWidth="sm"
            disableBackdropClick={loading}
            disableEscapeKeyDown={loading}
            aria-labelledby="modal-paz-salvo-titulo"
        >
            <DialogTitle id="modal-paz-salvo-titulo">
                Documento de paz y salvo
            </DialogTitle>

            <DialogContent>
                <Typography
                    style={{
                        marginBottom: 20,
                    }}
                >
                    Por favor adjunte a continuación el
                    documento de paz y salvo de la obligación
                    que desea reclasificar.
                </Typography>

                <input
                    ref={inputArchivoRef}
                    id="archivo-paz-salvo"
                    type="file"
                    accept="application/pdf,.pdf"
                    onChange={seleccionarArchivo}
                    disabled={loading}
                    aria-label="Seleccionar documento de paz y salvo"
                    style={{
                        display: 'none',
                    }}
                />

                <Button
                    variant="outlined"
                    color="primary"
                    onClick={abrirSelectorArchivo}
                    disabled={loading}
                >
                    Seleccionar documento
                </Button>

                {archivoSeleccionado && (
                    <Typography
                        style={{
                            marginTop: 12,
                            fontWeight: 600,
                            wordBreak: 'break-word',
                        }}
                    >
                        Archivo seleccionado:{' '}
                        {archivoSeleccionado.name}
                    </Typography>
                )}
            </DialogContent>

            <DialogActions>
                <Button
                    onClick={cerrarModal}
                    disabled={loading}
                >
                    Cancelar
                </Button>

                <Button
                    onClick={cargarArchivo}
                    disabled={
                        !archivoSeleccionado ||
                        loading
                    }
                    variant="contained"
                    color="primary"
                    startIcon={
                        loading ? (
                            <CircularProgress
                                size={18}
                                color="inherit"
                            />
                        ) : undefined
                    }
                >
                    {loading
                        ? 'Cargando...'
                        : 'Cargar'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ModalPazSalvo;