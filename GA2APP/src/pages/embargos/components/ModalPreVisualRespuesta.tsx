import React, { useEffect, useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    TextField,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    MenuItem,
    Modal,
} from '@material-ui/core';
import jsPDF from "jspdf";
import { useForm } from 'react-hook-form';

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
        columnButtonAdd: {
            textAlign: 'center',
        },
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
    }),
);

export const ModalPreVisualRespuesta = (props: any) => {
    const classes = useStyles();


    const {
        handleClose,
        open,
        dataFormulario

    } = props;

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="form-dialog-title"
            maxWidth="md"
        >
            <DialogContent style={{ width: '600px' }}>
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <Typography variant="h6" className={classes.titleform}>
                            CAJA HONOR - PRE VISUALIZACIÓN DATOS RESPUESTA
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Identificación: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.iden}
                        </Typography>
                    </Grid>

                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Primer Nombre: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.primNombre}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Segundo Nombre: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.segNombre}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Primer Apellido: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.primApellido}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Segundo Apellido: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.segApellido}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Número Radicado Asociado: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.radicadoFlujo}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Dirección: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.direc}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Fecha Radicado: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.radicadoFecha}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Número Oficio Juzgado: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.juzgadoRadicado}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Fecha Oficio Juzgado: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.fechaOficioJuzg}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Departamento Juzgado: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.departJuzgado}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Municipio Juzgado: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.municipJuzgado}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Juzgado: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.nombreJuzgado}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Cuenta Judicial: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.cuentaJudicial}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Oficina de Destino: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.ofiDestino}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Número Expedientes: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.numExpedientes}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Código Interno: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.codigoInterno}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Correo Electrónico: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.correoElect}
                        </Typography>
                    </Grid>

                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Tipo Proceso: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.tipoProces}
                        </Typography>
                    </Grid>
                    <Grid item md={10}>
                        <Typography variant="subtitle1" style={{ fontWeight: 'bold', display: 'inline' }}>
                            Número Proceso: 
                        </Typography>
                        <Typography variant="subtitle1" style={{ display: 'inline' }}>
                            &nbsp;{dataFormulario.numProceso}
                        </Typography>
                    </Grid>
                </Grid>

            </DialogContent>
        </Dialog>
    );
}