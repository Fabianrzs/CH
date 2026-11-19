import {
    Button,
    Dialog,
    TextField,
    Card,
    CardContent,
    Container,
    Grid,
    Typography,
    Select,
    MenuItem,
    FormControl,
} from '@material-ui/core';
import React from 'react';
import AlertDialog from '../../../components/dialog/dialog.component';

export const ModalAgregar = (props: any) => {
    const {
        open,
        handleClose,
        handleOnClickSave,
        handleonblurText,
        handleInputChangeText,
        handleonblurNumeric,
        handleInputChangeNumeric,
        formLock,
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
                                        value={formLock?.codigo}
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
                                        value={formLock?.concepto}
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
                                        name="diasMora"
                                        onChange={handleInputChangeText}
                                        onBlur={handleonblurText}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        value={formLock?.diasMora}
                                    />
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <Typography>Reversible</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <FormControl fullWidth>
                                        <Select
                                            labelId="demo-simple-select-label"
                                            name="reversible"
                                            value={formLock?.reversible}
                                            onChange={handleInputChangeText}
                                        >
                                            <MenuItem value="true">Si</MenuItem>
                                            <MenuItem value="false">
                                                No
                                            </MenuItem>
                                        </Select>
                                    </FormControl>
                                </Grid>
                            </Grid>
                            <Grid container spacing={3}>
                                <Grid item xs={6}>
                                    <Typography>Aceleracion Deuda</Typography>
                                </Grid>
                                <Grid item xs={6}>
                                    <FormControl fullWidth>
                                        <Select
                                            name="aceleracionDeuda"
                                            value={formLock?.aceleracionDeuda}
                                            onChange={handleInputChangeText}
                                        >
                                            <MenuItem value="true">Si</MenuItem>
                                            <MenuItem value="false">
                                                No
                                            </MenuItem>
                                        </Select>
                                    </FormControl>
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
                                        Agregar
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
