import React, { useEffect } from "react";
import { Grid, MenuItem, Typography } from "@material-ui/core";
import { Controller } from "react-hook-form";
import CHTextField from "../../../components/ch-text-field/CHTextField";
import CHSelect from "../../../components/ch-select/CHSelect";

function FormularioDatosSeguros({
    formualioCartera,
    classes,
    readonlyFinal,
    aseguradoras,
    aprobadoPasoCartera
}: {
    formualioCartera: any;
    classes: any;
    readonlyFinal: boolean
    aseguradoras: any[];
    aprobadoPasoCartera: boolean;
}) {

    const tasaSeguroVida = Number(
        formualioCartera.watch('porcentajeSeguroVida') || 0
    );

    useEffect(() => {

        const calcularExtraPrima = () => {
            const tasaVida = Number(
                formualioCartera.getValues('porcentajeSeguroVida') || 0
            );

            const extraPrima = Number(
                formualioCartera.getValues('porcentajeExtraPrimaSeguroVida') || 0
            );

            const resultado = (tasaVida * (extraPrima / 100)) + tasaVida;

            formualioCartera.setValue(
                'tasaSeguroVida',
                resultado.toFixed(7),
                {
                    shouldValidate: true,
                    shouldDirty: true
                }
            );
        };

        const subscription = formualioCartera.watch((values: any, { name }: any) => {

            if (
                name === 'porcentajeSeguroVida' ||
                name === 'porcentajeExtraPrimaSeguroVida'
            ) {
                calcularExtraPrima();
            }

            if (name === 'porcentajeSeguroVida') {
                const tasa = Number(values.porcentajeSeguroVida || 0);

                if (tasa !== 0 && !aprobadoPasoCartera) {
                    formualioCartera.setValue(
                        'aseguradoraVida',
                        ''
                    );
                }
            }
        });

        calcularExtraPrima();

        return () => subscription.unsubscribe();

    }, [formualioCartera, aprobadoPasoCartera]);

    return (
        <>
            <Grid className={classes.column} item xs={12} md={12}>
                <Typography className={classes.titleform} variant="h5" component="h6">Datos Seguros</Typography>
            </Grid>

            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tasaSeguroVida"
                        control={formualioCartera.control}
                        rules={{
                            required:
                                'Este campo es requerido.',
                            min: {
                                value: 0,
                                message: 'No puede ser menor a 0'
                            },


                        }}
                        render={({ field: { value } }) => (
                            <CHTextField
                                label="Tasa Seguro Vida con extraprima*"
                                value={value}
                                isReadOnly
                                percentMask
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="porcentajeExtraPrimaSeguroVida"
                        control={formualioCartera.control}
                        rules={{
                            required:
                                'Este campo es requerido.',
                            min: {
                                value: 0,
                                message: 'No puede ser menor a 0'
                            },


                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                InputLabelProps={{ shrink: true }}
                                helperText={
                                    error ? error.message || 'No es un número valido' : null
                                }

                                label="Extraprima(%)*"
                                value={value}
                                isReadOnly={readonlyFinal}
                                onChange={(e) => {
                                    const newValue = e.target.value.replace('%', '');
                                    onChange(newValue);
                                }}
                                percentMask
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="porcentajeSeguroVida"
                        control={formualioCartera.control}
                        rules={{
                            required:
                                'Este campo es requerido.',
                            min: {
                                value: 0,
                                message: 'No puede ser menor a 0'
                            },


                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                InputLabelProps={{ shrink: true }}
                                helperText={
                                    error ? error.message || 'No es un número valido' : null
                                }

                                label="Tasa Seguro Vida*"
                                value={value}
                                isReadOnly={readonlyFinal}
                                onChange={(e) => {
                                    const newValue = e.target.value.replace('%', '');
                                    onChange(newValue);
                                }}
                                percentMask
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="aseguradoraVida"
                        control={formualioCartera.control}
                        render={({ field }) => (
                            <CHSelect
                                label="Aseguradora"
                                value={field.value}
                                onChange={field.onChange}
                                readOnly={
                                    aprobadoPasoCartera || tasaSeguroVida !== 0
                                }
                            >
                                {aseguradoras.map((item) => (
                                    <MenuItem
                                        key={item.id}
                                        value={item.id}
                                    >
                                        {item.razonSocial}
                                    </MenuItem>
                                ))}
                            </CHSelect>
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tasaSeguroHogar"
                        control={formualioCartera.control}
                        rules={{
                            required:
                                'Este campo es requerido.',
                            min: {
                                value: 0,
                                message: 'No puede ser menor a 0'
                            },

                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                InputLabelProps={{ shrink: true }}
                                helperText={
                                    error ? error.message || 'No es un número valido' : null
                                }

                                label="Tasa Seguro Hogar*"
                                isReadOnly={readonlyFinal}
                                value={value}
                                onChange={(e) => {
                                    const newValue = e.target.value.replace('%', '');
                                    onChange(newValue);
                                }}
                                percentMask
                            />
                        )}
                    />
                </Grid>
            </Grid>
        </>
    )
}

export default FormularioDatosSeguros