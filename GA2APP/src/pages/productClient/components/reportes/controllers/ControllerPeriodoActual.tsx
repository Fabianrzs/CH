import React from "react";
import { Controller, UseFormReturn } from "react-hook-form";
import CHCheckbox from "../../../../../components/ch-checkbox/CHCheckbox";

type ControllerProps = {
    form: UseFormReturn<any>,
    name?: string,
    required?: boolean,
    onChange?: (value: boolean) => void;
}

const ControllerPeriodoActual = (props: ControllerProps) => {
    const {form, name, required, onChange: onChangeValue} = props;

    return (
        <Controller
            name={name ?? "periodoActual"}
            control={form.control}
            rules={{
                required: {value: required ?? false, message: 'Este campo es requerido.'},
            }}
            render={({
                field: { onChange, value }, fieldState: { error },
            }) => (
                <CHCheckbox
                    label="Periodo Actual"
                    onChange={(e, value) => {
                        onChange(e);
                        onChangeValue?.(value);
                    }}
                    checked={value ?? false}
                    error={!!error}
                    helperText={error ? error.message : null}
                />
            )}
        />
    );
};

export default ControllerPeriodoActual;