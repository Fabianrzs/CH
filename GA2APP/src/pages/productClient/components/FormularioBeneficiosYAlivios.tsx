import { Grid, MenuItem, Typography } from "@material-ui/core";
import React from "react";
import { Controller } from "react-hook-form";
import CHSelect from "../../../components/ch-select/CHSelect";
import CHTextField from "../../../components/ch-text-field/CHTextField";

function FormularioBeneficiosYAlivios({
    formualioCartera,
    classes,
    readonlyFinal
}: {
    formualioCartera: any;
    classes: any;
    readonlyFinal:boolean
}) {
    return(
        <>
        <Grid className={classes.column} item xs={12} md={12}>
                    <Typography className={classes.titleform} variant="h5" component="h6">
                        Beneficios y Alivios
                    </Typography>
                </Grid>

                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tasaFrech"
                        control={formualioCartera.control}
                        rules={{
                            required: 'Este campo es requerido.',
                            min: {
                                value: 0,
                                message: 'No puede ser menor a 0'
                            },
                            max: {
                                value: 100,
                                message: 'No puede ser mayor a 100'
                            },
                            validate: value =>
                                (value >= 0 && value <= 100) || 'El valor debe estar entre 0 y 100',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                inputProps={{
                                    style: {
                                        textAlign: 'right',
                                    },
                                }}
                                label="Alivio en Tasa (Frech)*%"
                                value={value}
                                isReadOnly = {readonlyFinal}
                                onChange={(e) => {
                                    const newValue = e.target.value.replace('%', '');
                                    onChange(newValue);
                                }}
                                InputLabelProps={{ shrink: true }}
                                percentMask
                            />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="AlivioFrech"
                        control={formualioCartera.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                error={!!error}
                                label="Alivio en cuota (Frech)"
                                value={value}
                                readOnly = {readonlyFinal}
                                helperText={
                                    error ? error.message : null
                                }
                                onChange={onChange}
                            >
                                <MenuItem key="S" value="true">
                                    SI
                                </MenuItem>
                                <MenuItem key="N" value="false">
                                    NO
                                </MenuItem>
                            </CHSelect>
                        )}
                    />
                </Grid>
        </>
    )
}

export default FormularioBeneficiosYAlivios