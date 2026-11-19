
import React, { useEffect, useState } from 'react';
import { Controller, useForm, UseFormHandleSubmit } from 'react-hook-form';
import {
    Button,
    createStyles,
    Dialog,
    DialogContent,
    DialogTitle,
    Grid,
    makeStyles,
    createTheme,
    MuiThemeProvider,
    MenuItem,
} from '@material-ui/core';
import CHPaper from '../../../components/ch-paper/CHPaperV2';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';

import { TerceroBeneficiarioModel } from '../../../models/tercero-beneficiario.model';

import NotificationService from '../../../services/notification.service';
import { ENTIDADBANCARIA, VALORCATALOGO } from '../../../types/catalog.type';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        column3: {
            padding: '10px',
            textAlign: 'right',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        columnDate: {
            minWidth: '110px',

        },
        columnTextAdd: {
            textAlign: 'right',
            width: '200px',
        }
    }),
);


export const ModalResumenPago = (props: any) => {

    const {open, handleClose, handleOnClick, 
           listaValorCatalogo, listaEntidadBancaria, responseSolicitud} = props;

    const [terceroBeneficiarioModel, setTerceroBeneficiarioModel] = useState<TerceroBeneficiarioModel>({} as TerceroBeneficiarioModel);

    const classes = useStyles();

    const formLabelsTheme = createTheme({
        overrides: {
            MuiFormLabel: {
                asterisk: {
                    color: '#db3131',
                    '&$error': {
                        color: '#db3131',
                    },
                },
            },
        },
    });

    const formulario = useForm({
		defaultValues: {
			numeroIdentificación: '',
			nombreTercero: '',
			valorSolicitud: '',
			numeroCuenta: '',
			tipoCuentaObj: '',
			entidadObj: '',
		},
	});

    useEffect(() => {
		setTerceroBeneficiarioModel(responseSolicitud.solicitudTerceroBeneficiarioDto.length > 0? responseSolicitud.solicitudTerceroBeneficiarioDto.at(-1): terceroBeneficiarioModel);
	}, [responseSolicitud]);
     
    const onSubmit = async (data:any) => {
        let validate = true;
        const nombreTercero = (responseSolicitud?.nombresCliente || '') + (responseSolicitud?.apellidosCliente || '');

     

        if(responseSolicitud?.identificacionCliente !==  data.numeroIdentificación){
            validate = false;
        }else
        if(nombreTercero !== data.nombreTercero){
            validate = false
        }else
        if(responseSolicitud?.valorRetirar !== data.valorSolicitud){
            validate = false
        }else
        if(responseSolicitud?.numeroCuenta !== data.numeroCuenta){
            validate = false
        }else
        if(responseSolicitud?.tipoCuentaObj !== data.tipoCuentaObj){
            validate = false
        }else
        if(responseSolicitud?.entidadObj !== data.entidadObj){
            validate = false
        }

        if(!validate){
            await NotificationService.genericAlert('Los datos ingresados no coinciden.',"","error");
        }else
        if(handleOnClick){
            handleOnClick();
        }
    }

    
    return (
        <div>
              <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogTitle>RESUMEN DE PAGO</DialogTitle>
                <DialogContent style={{ width: '600px' }}>
                    <Grid container>
                        <Grid item md={6}>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <CHTextField
                                    isReadOnly
                                    label="Número de identificación"
                                    value={
                                        responseSolicitud?.identificacionCliente || ''
                                    }
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <CHTextField
                                    isReadOnly
                                    label="Nombre del tercero"
                                    value={(responseSolicitud?.nombresCliente || '') + (responseSolicitud?.apellidosCliente || '')}
                                />
                            </Grid>
                            <Grid className={classes.column3} item xs={12} md={12}>
                                <CHTextField 
                                    currencyMask
                                    isReadOnly
                                    label="Valor a pagar"
                                    value={responseSolicitud?.valorRetirar || '0'}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <CHTextField
                                    isReadOnly
                                    label="Número de cuenta"
                                    value={
                                        terceroBeneficiarioModel?.numeroCuenta || ''
                                    }
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <CHTextField
                                    isReadOnly
                                    label="Tipo cuenta"
                                    value={
                                        terceroBeneficiarioModel?.tipoCuentaObj?.descripcion || ''
                                    }
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <CHTextField
                                    isReadOnly
                                    label="Entidad"
                                    value={
                                        terceroBeneficiarioModel?.entidadObj?.descripcion || ''
                                    }
                                />
                            </Grid>
                        </Grid>
                        <Grid item md={6}>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <Controller
                                    name="numeroIdentificación"
                                    control={formulario.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <MuiThemeProvider theme={formLabelsTheme}>
                                            <CHTextField
                                                numberMask
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                label="Confirmar Número de identificación"
                                                value={value}
                                                onChange={onChange}
                                            />
                                        </MuiThemeProvider>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <Controller
                                    name="nombreTercero"
                                    control={formulario.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <MuiThemeProvider theme={formLabelsTheme}>
                                            <CHTextField
                                                nameMask
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                label="Confirmar Nombre del tercero"
                                                value={value}
                                                onChange={onChange}
                                            />
                                        </MuiThemeProvider>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column3} item xs={12} md={12}>
                                <Controller
                                    name="valorSolicitud"
                                    control={formulario.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <MuiThemeProvider theme={formLabelsTheme}>
                                            <CHTextField
                                                currencyMask
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                label="Confirmar Valor a pagar"
                                                value={value}
                                                onChange={onChange}
                                                inputProps={{
                                                    style: {
                                                        textAlign: 'right',
                                                    },
                                                }}
                                            />
                                        </MuiThemeProvider>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <Controller
                                    name="numeroCuenta"
                                    control={formulario.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <MuiThemeProvider theme={formLabelsTheme}>
                                            <CHTextField
                                                numberMask
                                                error={!!error}
                                                helperText={error ? error.message : null}
                                                label="Confirmar Número de cuenta"
                                                value={value}
                                                onChange={onChange}
                                            />
                                        </MuiThemeProvider>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <Controller
                                    name="tipoCuentaObj"
                                    control={formulario.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <MuiThemeProvider theme={formLabelsTheme}>
                                            <CHSelect
                                                error={!!error}
                                                label="Confirmar Tipo de Cuenta *"
                                                value={value}
                                                helperText={error ? error.message : null}
                                                onChange={(value) => {

                                                    const dato = value.target.value as string;
                                                    const idEntidadBancaria = dato.split('_')[0];
                                                    onChange(value);
                                                }}
                                            >
                                                {listaValorCatalogo?.map(
                                                    (option:VALORCATALOGO) => {
                                                        const dato = `${option.idValor}_${option.descripcion}`;
                                                        return (
                                                            <MenuItem
                                                                key={option.idValor}
                                                                value={dato}
                                                            >
                                                                {option.descripcion}
                                                            </MenuItem>
                                                        );
                                                    }
                                                )}
                                            </CHSelect>
                                        </MuiThemeProvider>
                                    )}
                                />
                            </Grid>
                            <Grid className={classes.column} item xs={12} md={12}>
                                <Controller
                                    name="entidadObj"
                                    control={formulario.control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <MuiThemeProvider theme={formLabelsTheme}>
                                            <CHSelect
                                                error={!!error}
                                                label="Confirmar Entidad"
                                                value={value}
                                                helperText={error ? error.message : null}
                                                onChange={(value) => {
                                                    onChange(value);
                                                }}
                                            >
                                                {listaEntidadBancaria?.map(
                                                    (option: ENTIDADBANCARIA) => {
                                                        const dato = `${option.idEntidadBancaria}_${option.nombreRazonSocial}`;
                                                        return (
                                                            <MenuItem
                                                                key={option.idEntidadBancaria}
                                                                value={dato}
                                                            >
                                                                {option.nombreRazonSocial}
                                                            </MenuItem>
                                                        );
                                                    }
                                                )}
                                            </CHSelect>
                                        </MuiThemeProvider>
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>

                    <br />
                    <Grid container>
                        <Grid item>
                            <Button
                                size="small"
                                variant="contained"
                                color="primary"
                                onClick={formulario.handleSubmit(onSubmit)}
                            >
                                CONFIRMAR
                            </Button>
                            <Button
                                size="small"
                                variant="contained"
                                color="default"
                                onClick={handleClose}
                                style={{ marginLeft: '10px' }}
                            >
                                CANCELAR
                            </Button>
                        </Grid>
                    </Grid>
                    <br />
                </DialogContent>
            </Dialog>
        </div>
    );
};
