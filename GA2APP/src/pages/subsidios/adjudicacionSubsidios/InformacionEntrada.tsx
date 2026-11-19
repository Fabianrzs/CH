import { createStyles, Grid, makeStyles } from '@material-ui/core';
import React from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
	}),
);

const InformacionEntrada = (props: Props) => {

    const classes = useStyles();
    const {formularioInformacionEntrada} = props;

    return(
        <CHPaper title="Información Entrada">
            <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroResolucion"
                            control={formularioInformacionEntrada.control}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    isReadOnly
                                    value={value}
                                    onChange={onChange}
                                    label="No. Resolución"
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaInicioResolucion"
                            control={formularioInformacionEntrada.control}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    isReadOnly
                                    value={value}
                                    onChange={onChange}
                                    label="Fecha Inicio Resolución"
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaCierreResolucion"
                            control={formularioInformacionEntrada.control}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    isReadOnly
                                    value={value}
                                    onChange={onChange}
                                    label="Fecha Cierre Resolución"
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="estado"
                            control={formularioInformacionEntrada.control}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    isReadOnly
                                    value={value}
                                    onChange={onChange}
                                    label="Estado"
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="radicado"
                            control={formularioInformacionEntrada.control}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    isReadOnly
                                    value={value}
                                    onChange={onChange}
                                    label="No. Radicado"
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="totalAdjudicaciones"
                            control={formularioInformacionEntrada.control}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    isReadOnly
                                    value={value}
                                    onChange={onChange}
                                    label="Total Adjudicaciones"
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="montoTotal"
                            control={formularioInformacionEntrada.control}
                            render={({ field: { onChange, value } }) => (
                                <CHTextField
                                    currencyMask
                                    isReadOnly
                                    value={value}
                                    onChange={onChange}
                                    label="Monto Total"
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
            </Grid>
        </CHPaper>
    )

}

type Props = {
    formularioInformacionEntrada: UseFormReturn<any>;
}

export default InformacionEntrada;