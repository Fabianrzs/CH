import React, { useState } from 'react';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    Grid,
    makeStyles,
    MenuItem,
    Typography,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import UtilService from '../../../../utils/util.service';
import AddressComponent from '../../../../components/modals/address/address.component';
import { Address } from '../../../../types/address.type';
import { setFormDataAddres } from '../../../../components/modals/address/constants/address.components.constants';

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
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        element: {
            marginLeft: '40px',
        }
    }),
);

export const ModalGenerarPago = (props: any) => {
    const classes = useStyles();
    const {open,
           onClose,
           tiposCuenta,
           entidadesBancarias,
           formulario,
           ejecutarPago,
           mostrarCamposCheque,
           mostrarCamposCuenta,
           setMostrarCamposCheque,
           setMostrarCamposCuenta
        } = props;

   
    const [openDireccion, setOpenDireccion] = useState(false);

    const onCloseAddress = (value: Address) => {
        if (value !== null) {
            const result = setFormDataAddres(value);
            formulario.setValue('direccion', result);
            setOpenDireccion(false);
        } else {
            formulario.setValue('direccion', '');
            setOpenDireccion(false);
        }
    };

    const handleOpenAddress = () => {
        setOpenDireccion(true);
    };
    
    return (
        <>
        <AddressComponent
            onOpen={openDireccion}
            onClose={onCloseAddress}
            control={formulario.control}
            getValues={formulario.watch}
            tipoDireccion
            setValue={formulario.setValue}
        />
            <Dialog
                fullWidth
                maxWidth="sm"
                open={open}
                onClose={onClose}
                aria-labelledby="form-dialog-title"
            >
                <DialogContent>
                    <Grid item>
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            GENERAR PAGO
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container spacing={3}>
                        <Grid item xs={10} className={classes.element}>
                        <Controller
                                name="nit"
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        numberMask
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="NIT"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={e => {
                                            if(e.target.value.length < 20) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[a-zA-Z#./?=!@$%^&*^)(+=_}{":;><|~`]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        }}
                                />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={3}>
                        <Grid item xs={10} className={classes.element}>
                        <Controller
                                name="unidadEjecutora"
                                control={formulario.control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                    isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Unidad Ejecutora"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={onChange}
                            />
                            )}
                        />
                    </Grid>
                    </Grid>
                    <Grid container spacing={3}>
                        <Grid item xs={10} className={classes.element}>
                        <Controller
                                name="tipoCuenta"
                                control={formulario.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Tipo de Cuenta *"
                                        value={ value }
                                        helperText={
                                            error ? error.message : null
                                        }
                                        onChange={e => {
                                            // Si se selecciona Cheque
                                            if(e.target.value === '3'){
                                                setMostrarCamposCheque(true);
                                                setMostrarCamposCuenta(false);
                                                formulario.setValue('entidad','');
                                                formulario.setValue('numeroCuenta','');
                                            }
                                            // Si se selecciona cuenta
                                            else if(e.target.value === '1' ||
                                                    e.target.value === '2'){
                                                setMostrarCamposCheque(false);
                                                setMostrarCamposCuenta(true);
                                                formulario.setValue('direccion','');
                                            }
                                            onChange(e);
                                        }}
                                    >
                                    {tiposCuenta?.map(
                                        (option: any) => ( 
                                            <MenuItem key={option.codigo} 
                                                        value={option.codigo}
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                            ),
                                    )} 
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    {mostrarCamposCuenta && (
                        <Grid container spacing={3}>
                        <Grid item xs={10} className={classes.element}>
                        <Controller
                                name="entidad"
                                control={formulario.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        label="Entidad *"
                                        value={ value }
                                        helperText={
                                            error ? error.message : null
                                        }
                                        onChange={onChange}
                                    >
                                    {entidadesBancarias?.map(
                                        (option: any) => ( 
                                            <MenuItem key={option.codigoBanco} 
                                                        value={option.codigoBanco}
                                            >
                                                {option.nombreRazonSocial}
                                            </MenuItem>
                                            ),
                                    )} 
                                    </CHSelect>
                                )}
                            />
                        </Grid>
                    </Grid>
                    )}
                    {mostrarCamposCuenta && (
                        <Grid container spacing={3}>
                        <Grid item xs={10} className={classes.element}>
                            <Controller
                                name="numeroCuenta"
                                control={formulario.control}
                                rules={{
                                    required:
                                        'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        label="Número de Cuenta *"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={e => {
                                            if (
                                                e.target.value
                                                    .length <= 21
                                            ) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                            }
                                        }
                                    />
                                )}
                            />
                        </Grid>
                        </Grid>
                    )}
                    {mostrarCamposCheque && (
                          <Grid container spacing={3}>
                          <Grid item xs={10} className={classes.element}>
                          <Controller
                                name="direccion"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        isReadOnly
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        label="Dirección *"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                        onClick={handleOpenAddress}
                                        onChange={onChange}
                                    />
                                )}
                            />
                          </Grid>
                        </Grid>
                    )}
                    <Grid container spacing={3}>
                        <Grid item xs={10} className={classes.element}>
                        <Controller
                                name="valorReintegrar"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                    validate: () => UtilService.cleanCurrencyValue(formulario.watch('valorReintegrar')) <= 0? 
                                                    'El valor debe ser mayor a cero' : true
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        currencyMask
                                        label="Valor a Reintegrar"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <br />
                    <br />
                    <Grid container>
                    <Grid className={classes.column} item xs={12} md={4} />
                    <Grid className={classes.column} item xs={12} md={2}>
                        <Button
                            size="small"
                            variant="contained"
                            color="primary"
                            onClick={formulario.handleSubmit(ejecutarPago)}
                        >
                            Ejecutar
                        </Button>
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={2}>
                        <Button
                            size="small"
                            variant="contained"
                            color="default"
                            onClick={onClose}
                        >
                            Cancelar
                        </Button>
                    </Grid>
                    </Grid>
                </DialogContent>
            </Dialog>
        </>
    );
};