import {createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import React from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import CHCheckbox from '../../../../components/ch-checkbox/CHCheckbox';
import CHPaper from '../../../../components/ch-paper/CHPaper';
import CHSelect from '../../../../components/ch-select/CHSelect';
import CHTextField from '../../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
        paragraphText: {
            textAlign: 'justify',
            lineHeight: 1.6,
            marginBottom: '16px',
            fontSize: '15px',
        },
	}),
);

const MisCuentas = (props: propsType) => {

    const classes = useStyles();
    const {formulario, clientStateData, nombreCuenta, campoSoloLecturaRadicacionAprobada, setNombreCuenta, setSaldoCuenta, setAceptaTraslado} = props;

    return(
        <CHPaper title='Mis cuentas'>
            <Grid container>
            <Grid className={classes.column} item xs={12} md={6} lg={3}>
                    <Controller
                        name="saldoCuenta"
                        control={formulario.control}
                        render={({
                            field: { onChange, value, },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                currencyMask
                                error={!!error}
                                helperText={error ? error.message : null}
                                inputProps={{
                                    style: {
                                        textAlign: 'right',
                                    },
                                }}
                                label="Saldo Cuenta"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                onChange={onChange}
                                isReadOnly
                            />
                        )}
                    />
				</Grid>
                <Grid className={classes.column} item xs={12} md={6} lg={4}>
                    <Controller
                        name="cuenta"
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                label="Tipo de Cuenta *"
                                value={value}
                                readOnly={campoSoloLecturaRadicacionAprobada}
                                onChange={e => {
                                    onChange(e);
                                    setNombreCuenta(String(e.target.value));
                                    const numeroCuentaSeleccionada = String(e.target.value).split(' - ')[0];
                                    const cuentaSeleccionada = clientStateData?.cuentas?.find((cuenta: any) => cuenta?.numeroCuentaCompleta === numeroCuentaSeleccionada);
                                    formulario.setValue('saldoCuenta',cuentaSeleccionada?.saldoDisponible);
                                    setSaldoCuenta(cuentaSeleccionada?.saldoDisponible);
                                }}
                            >
                             {clientStateData?.cuentas && clientStateData?.cuentas.length > 0 &&
                                    clientStateData?.cuentas?.map((option: any) => (
                                        <MenuItem
                                            key={option.numeroCuentaCompleta}
                                            value={`${option.numeroCuentaCompleta} - ${option.tipoCuenta}`}
                                        >
                                            {`${option.numeroCuentaCompleta} - ${option.tipoCuenta}`}
                                        </MenuItem>
                                    ))}
                            </CHSelect>
                        )}
                    />
                </Grid>
                {nombreCuenta && (
                    <Grid className={classes.column} item xs={12} md={12} lg={12}>
                        <p className={classes.paragraphText}>
                       <strong>Autorizar traslado</strong><br />
                        <br />
                        Autorizo de manera expresa y voluntaria la transferencia de los recursos registrados 
                        en la Cuenta <strong>{nombreCuenta}</strong>, 
                        según corresponda y conforme a los procedimientos establecidos por la entidad.
                        <br />
                        <br />
                        Esta autorización se realiza con pleno conocimiento de que los recursos transferidos 
                        conservarán su destinación específica y estarán sujetos a las condiciones 
                        y normativas vigentes para cada tipo de cuenta.
                        <br />
                      </p>
                    </Grid>
                 )}
                {nombreCuenta && (
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="acepta"
                            control={formulario.control}
                            render={({ field: { onChange, value }, }) => (
                                <CHCheckbox
                                    label="Acepto *"
                                    onChange={(e) => {
                                        setAceptaTraslado(e.target.checked);
                                        onChange(e.target.checked)
                                    }}
                                    checked={campoSoloLecturaRadicacionAprobada? true : value}
                                    disabled={campoSoloLecturaRadicacionAprobada}
                                />
                            )}
                        />
                    </Grid>
                )}
            </Grid>
        </CHPaper>
    )

}

type propsType = {
    formulario: UseFormReturn<any>;
    clientStateData: any;
    nombreCuenta: string;
    campoSoloLecturaRadicacionAprobada: boolean;
    setSaldoCuenta: (value: number) => void;
	setAceptaTraslado: (value: boolean) => void;
    setNombreCuenta: (value: string) => void;
}

export default MisCuentas;
