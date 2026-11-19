import React, { useEffect, useState } from 'react';
import { createMuiTheme, createStyles, createTheme, Grid, makeStyles, MuiThemeProvider } from '@material-ui/core';
import { Controller, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ResponseSeveranceModel } from '../../../../models/requests/response-severance.model';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '120px',
            marginLeft: '.5rem',
        },
        column2: {
            padding: '10px',
            textAlign: 'left',
        },
        column3: {
            padding: '10px',
            textAlign: 'right',
        },        
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnLabel: {
            padding: '10px',
        },
        columnTextAdd: {
            textAlign: 'right',
            width: '200px',
        },
    }),
);

function DatosPersonalesValorCuotaInicial({
    responseSolicitud,
    control,
    valorDesembolsoCredito,
    obtenerValorDesembolsoCredito
}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    control: UseFormReturn<any>;
    valorDesembolsoCredito:any
    obtenerValorDesembolsoCredito: (numeroDocumento:string) => void;
}) {
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

    const [valorDesembolso, setValorDesembolso] = useState<string>('0');

    useEffect(() => {
        obtenerValorDesembolsoCredito(responseSolicitud?.identificacionCliente || '0');
        if(responseSolicitud?.valorRetirar == null){
            if (valorDesembolsoCredito) {
                setValorDesembolso(valorDesembolsoCredito?.ValorDesembolso || '0');
            }
        }else{
            setValorDesembolso(responseSolicitud?.valorRetirar || '0');
        }
		
	}, []);

    return (
        <>
            <CHPaper title="Información Solicitud">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Nombres"
                            value={responseSolicitud?.nombresCliente || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Apellidos"
                            value={responseSolicitud?.apellidosCliente || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            numberMask
                            isReadOnly
                            label="Número de Documento"
                            value={
                                responseSolicitud?.identificacionCliente || ''
                            }
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Tipo de Solicitud"
                            value={responseSolicitud?.descripcionProceso || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Modalidad"
                            value={
                                responseSolicitud?.descripcionModalidad || ''
                            }
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            isReadOnly
                            label="Sub-Modalidad"
                            value={
                                responseSolicitud?.descripcionSubModalidad || ''
                            }
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <CHTextField
                            name="valorSolicitud"
                            isReadOnly
                            label="Valor Cuota Inicial"
                            value={valorDesembolso || 0}
                            currencyMask
                        />
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    );
}

export default DatosPersonalesValorCuotaInicial;
