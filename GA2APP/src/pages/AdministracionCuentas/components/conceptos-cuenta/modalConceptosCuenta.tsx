import React, { useState } from 'react';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    Typography,
    FormControl,
    InputLabel,
    MenuItem,
    Select
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ConceptoCuenta } from '../types';


const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
    },
    buttonprincipal: {
        width: '250px',
        height: '50px',
        marginLeft: '1rem',
        background: '#FEC917',
    },
    buttonsecundario: {
        width: '250px',
        height: '50px',
        marginLeft: '1rem',
        background: '#43b5f1',
    },
    buttonadd: {
        width: '250px',
        height: '50px',
        marginRight: '1rem',
        background: '#43b5f1',
    },
    titleform: {
        width: '100%',
        textAlign: 'center',
        color: '#1C3F6B',
        padding: 1,
        fontWeight: 'bold',
        fontSize: '1rem',
    },
    column: {
        padding: '10px',
        textAlign: 'center'
    },
    select: {

        textAlign: 'left',
        paddingRight: '100px'

    },
    tableCellBlueText: {
        color: 'blue',
    },
}));

export const ModalConceptosCuenta = (props: any) => {

    const classes = useStyles();
    const {
        open,
        onClose,
        motivoTrasladoSeleccionado,
    } = props;

    const { handleSubmit, control, getValues } = useForm();

    let conceptOptions: any[] = [];

    if (motivoTrasladoSeleccionado === 'Cuenta individual solucion de vivienda') {
        conceptOptions = [
            'AHORROS OBLIGATORIOS',
            'AHORROS RETROACTIVOS',
            'AHORROS VOLUNTARIOS',
            'CESANTÍAS',
            'CESANTÍAS RETROACTIVOS',
            'COMPENSACIONES',
            'CONCILIACIONES',
            'EXCEDENTES FINANCIEROS',
            'FONDO SOLIDARIDAD',
            'INTERESES APORTES',
            'INTERESES CESANTÍAS',
            'POR LEGALIZAR',
            'RETROACTIVOS FONDO SOLIDARIDAD',
            'SUBSIDIO',
        ];
    } else if (
        motivoTrasladoSeleccionado === 'Cuenta individual administración de cesantías'
    ) {
        conceptOptions = [
            'CESANTÍAS',
            'CESANTÍAS DOCEVAS',
            'CESANTÍAS RETROACTIVOS',
            'INTERESES CESANTÍAS',
            'RENDIMIENTOS CESANTÍAS',
        ];
    } else if (
        motivoTrasladoSeleccionado === 'Ahorro Voluntario Afiliado Cesantias'
    ) {
        conceptOptions = [
            'AHORRO AVAC',
            'INTERESES AHORRO AVAC',
        ];
    } else if (
        motivoTrasladoSeleccionado === 'Ahorro Voluntario Afiliado Veterano'
    ) {
        conceptOptions = [
            'AHORRO VOLUNTARIO AVAV',
            'INTERESES AHORRO AVAV',
        ];
    } else if (
        motivoTrasladoSeleccionado === 'Aporte Voluntario Contractual (AVC)'
    ) {
        conceptOptions = [
            'APORTE VOLUNTARIO CONTRACTUAL AVC',
            'INTERESES APORTE VOLUNTARIO CONTRACTUAL AVC',
        ];
    }


    const [conceptosList, setConceptosList] = useState<ConceptoCuenta[]>([]);

    const handleSave = () => {
        const nuevoConcepto: ConceptoCuenta = {
            concepto: formData.concepto,
            monto: formData.monto,
            cuotas: Number(formData.cuotas),
            selected: false,
            id: conceptosList.length + 1
        };


        props.onAddConcepto(nuevoConcepto);


        setFormData({
            concepto: '',
            monto: '',
            cuotas: ''
        });

        props.onClose();

    };


    const [formData, setFormData] = useState({
        concepto: '',
        monto: '',
        cuotas: ''
    });

    return (
        <Dialog
            fullWidth
            maxWidth="sm"
            open={open}
            onClose={onClose}
            aria-labelledby="form-dialog-title"
        >
            <DialogContent>
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <Typography variant="h5" component="h6">
                            CONCEPTOS CUENTA
                        </Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <Controller
                            name="concepto"
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value } }) => (
                                <FormControl className={classes.select} style={{ width: '110%', paddingRight: '100px' }}>
                                    <InputLabel>Concepto</InputLabel>
                                    <Select
                                        value={value}
                                        onChange={onChange}
                                    >
                                        {conceptOptions.map((option) => (
                                            <MenuItem key={option} value={option}>
                                                {option}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            )}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <Controller
                            name="monto"
                            control={control}
                            defaultValue=""
                            render={({ field }) => (

                                <CHTextField
                                    label="Monto"
                                    fullWidth
                                    currencyMask
                                    {...field}
                                    value={formData.monto}
                                    onChange={(event) => setFormData({ ...formData, monto: event.target.value })}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <Controller
                            name="cuotas"
                            rules={{
                                pattern: {
                                    value: /^[0-9]*$/,
                                    message: ''
                                },
                            }}
                            control={control}
                            defaultValue=""
                            render={({ field }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    label="Cuotas"
                                    type="number"
                                    {...field}
                                    value={formData.cuotas}
                                    onChange={(event) => setFormData({ ...formData, cuotas: event.target.value })}
                                />
                            )}
                        />
                    </Grid>
                    <Grid container justify="flex-end" spacing={2}>
                        <Grid item>
                            <Button variant="outlined" color="primary" onClick={handleSave}>
                                Agregar
                            </Button>
                        </Grid>
                        <Grid item>
                            <Button variant="outlined" color="default" onClick={onClose}>
                                Cancelar
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}