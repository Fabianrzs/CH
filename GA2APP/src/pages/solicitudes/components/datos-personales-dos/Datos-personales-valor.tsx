import { createStyles, Grid, makeStyles } from '@material-ui/core';
import React from 'react';
import { UseFormReturn } from 'react-hook-form';
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
    }),
);

function DatosPersonalesValor({
    responseSolicitud,
    control,
}: {
    responseSolicitud: ResponseSeveranceModel | undefined;
    control: UseFormReturn<any>;
}) {
    const classes = useStyles();
    control.setValue('valorSolicitud', responseSolicitud?.valorAhorro);
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
                            isReadOnly
                            label="Saldo Disponible"
                            value={responseSolicitud?.valorAhorro || 0}
                            currencyMask
                        />
                    </Grid>
                </Grid>
            </CHPaper>
        </>
    );
}

export default DatosPersonalesValor;
