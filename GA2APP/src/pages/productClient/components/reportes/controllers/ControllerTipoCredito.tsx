import React from "react";
import { MenuItem } from "@material-ui/core";
import { Controller, UseFormReturn } from "react-hook-form";
import CHSelect from "../../../../../components/ch-select/CHSelect";

const ControllerTipoCredito = (props: {form: UseFormReturn<any>, name?: string, required?: boolean, all?: boolean}) => {
    const {form, name, required, all} = props;

    return (
        <Controller
            name={name ?? "tipoCredito"}
            control={form.control}
            rules={{ 
                required: {value: required ?? false, message: 'Este campo es requerido.'}
            }}
            render={({
                field: { onChange, value }, fieldState: { error },
            }) => (
                <CHSelect
                    label="Tipo Crédito"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    helperText={error ? error.message : null}>
                    { (all ?? true) && <MenuItem value={0}>CONSULTA TOTAL</MenuItem> }
                    <MenuItem value={1}>LEASING HABITACIONAL</MenuItem>
                    <MenuItem value={2}>HIPOTECARIO</MenuItem>
                </CHSelect>
        )} />
    );
};

export default ControllerTipoCredito;