import React, { useEffect, useMemo, useState } from 'react';
import { connect } from 'react-redux';
import {
    Box,
    Button,
    createStyles,
    Dialog,
    DialogContent,
    DialogTitle,
    Fade,
    Grid,
    IconButton,
    makeStyles,
    MenuItem,
    Modal,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Typography,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { sortBy } from 'lodash';
import { toast } from 'react-toastify';
import moment from 'moment';
import { useHistory } from 'react-router-dom';
import { Edit } from '@material-ui/icons';
import swal from 'sweetalert';


const useStyles = makeStyles((theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '400px',
            marginLeft: '.5rem',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 8px',
        },
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        alineationButtons: {
            display: 'flex',
            justifyContent: 'center',
            marginTop: theme.spacing(2),
        },
        modal: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            backgroundColor: 'rgba(0, 0, 0, 0.5)',
            zIndex: 9999,
            '& .MuiPaper-root': {
                margin: 'auto',
                outline: 'none',
                borderRadius: 8,
                width: '50%',
                height: '50%',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                [theme.breakpoints.down('sm')]: {
                    width: '80%',
                    height: '80%',
                },
            },
        },
        paper: {
            backgroundColor: theme.palette.background.paper,
            border: '2px solid #000',
            boxShadow: theme.shadows[5],
            padding: theme.spacing(5, 3, 2),
            width: '70%',
            height: '90%',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            borderRadius: '3%',
        },
        header: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            marginBottom: '1rem',
        },
        title: {
            color: '#1C3F8B',
            textAlign: 'center',
            fontSize: '0.9rem',
            fontWeight: 'bold',
        },
        actions: {
            marginLeft: '2rem',
            width: '20%',
        },
        customCheckbox: {
            color: 'gray',
            '&$checked': {
                color: 'gray',
            },
        },
        alineationHorizontal: {
            display: 'flex',
            justifyContent: 'center',
        },
        espaciado: {
            paddingInline: '70px',
            paddingBottom: '10px',
        },
        encabezados: {
            textAlign: 'center',
            position: 'sticky',
            top: 0,
            background: '#FEC917',
            zIndex: 1,
        },
    }),
);

const DatosAuditoria = ({
    open,
    handleClose,
    selectedData
}: {
    open: boolean;
    handleClose: () => void;
    selectedData: any;

}) => {
    const classes = useStyles();

    if (!selectedData?.camposActualizados) return null;

    let cambiosAuditoria: { Field: string; OldValue: string; NewValue: string }[] = [];

    try {
        cambiosAuditoria = JSON.parse(selectedData.camposActualizados);
    } catch (error) {
        console.error("Error al parsear camposActualizados:", error);
    }


    return (
        <Modal className={classes.modal} open={open} onClose={handleClose}>
            <Fade in={open}>
                <>
                    <div className={classes.paper}>
                        <Box mb={0.2}>
                            <Typography variant="h6" align="center" className={classes.title}>
                                DETALLE AUDITORIA
                            </Typography>
                        </Box>
                        <br />
                        <Grid container alignItems="center" justify="center">
                            <Grid item xs={12}>
                                <div style={{ maxHeight: '70vh', overflow: 'auto', width: '100%' }}>
                                    <table className="client-table" style={{ width: '100%' }}>
                                        <thead>
                                            <tr>

                                                <th className={classes.encabezados}>Campo</th>
                                                <th className={classes.encabezados}>Valor Anterior</th>
                                                <th className={classes.encabezados}>Valor Nuevo</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {cambiosAuditoria.map((cambio, index) => (
                                                <TableRow key={index}>
                                                    <TableCell>{cambio.Field}</TableCell>
                                                    <TableCell>{cambio.OldValue}</TableCell>
                                                    <TableCell>{cambio.NewValue}</TableCell>
                                                </TableRow>
                                            ))}
                                        </tbody>
                                    </table>
                                </div>
                            </Grid>
                        </Grid>
                    </div>
                </>
            </Fade>
        </Modal>
    )
}

export default DatosAuditoria;