import React from 'react';
import {
    Button,
    Dialog,
    TextField,
    Card,
    CardContent,
    Container,
    Grid,
    Typography,
    makeStyles,
    createStyles,
} from '@material-ui/core';
import AlertDialog from '../../../components/dialog/dialog.component';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

export const ModalEditar = (props: any) => {
    const classes = useStyles();
    const {
        open,
        handleClose,
        handleOnClickSave,
        handleonblurText,
        handleInputChangeText,
        handleonblurNumeric,
        handleInputChangeNumeric,
        formState,
        confirmDialog,
        setConfirmDialog,
    } = props;
    return (
        <div>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="md">
                    <Card>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    ACTUALIZAR ESTADO
                                </Typography>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <Typography>Codigo</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <TextField
                                        type="number"
                                        name="codigo"
                                        onChange={handleInputChangeNumeric}
                                        onBlur={handleonblurNumeric}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        value={formState?.codigo}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <Typography>Concepto</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <TextField
                                        type="text"
                                        name="concepto"
                                        onChange={handleInputChangeText}
                                        onBlur={handleonblurText}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        value={formState?.concepto}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <Typography>
                                        Dias Mora Activa Estado
                                    </Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <TextField
                                        type="text"
                                        name="diasMoraActivaEstado"
                                        onChange={handleInputChangeText}
                                        onBlur={handleonblurText}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        value={formState?.diasMoraActivaEstado}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <Typography>Saldo Mínimo Deuda</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <TextField
                                        type="text"
                                        name="saldoDeuda"
                                        onChange={handleInputChangeText}
                                        onBlur={handleonblurText}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        value={formState?.saldoDeuda}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={handleOnClickSave}
                                    >
                                        Actualizar
                                    </Button>
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={handleClose}
                                        style={{ marginLeft: '10px' }}
                                    >
                                        Cancelar
                                    </Button>
                                </Grid>
                            </Grid>
                        </CardContent>
                    </Card>
                    <AlertDialog
                        confirmDialog={confirmDialog}
                        setConfirmDialog={setConfirmDialog}
                    />
                </Container>
            </Dialog>
        </div>
    );
};
