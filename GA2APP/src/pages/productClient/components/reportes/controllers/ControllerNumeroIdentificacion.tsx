import React from "react";
import { Controller, UseFormReturn } from "react-hook-form";
import CHTextField from "../../../../../components/ch-text-field/CHTextField";

const ControllerNumeroIdentificacion = (props: {form: UseFormReturn<any>, name?: string, required?: boolean}) => {
    const {form, name, required} = props;
    
    return (
        <Controller
            name={name ?? "numeroIdentificacion"}
            control={form.control}
            rules={{
                required: {value: required ?? false, message: 'Este campo es requerido.'},
            }}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHTextField
                    label="Identificación"
                    value={value}
                    onChange={onChange}
                    numberOnlyMask
                    error={!!error}
                    helperText={
                        error ? error.message : null
                    }
                />
            )}
        />
    );
}

export default ControllerNumeroIdentificacion;