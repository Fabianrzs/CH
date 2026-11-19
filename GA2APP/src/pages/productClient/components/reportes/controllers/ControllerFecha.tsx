import React from "react";
import { Controller, UseFormReturn, Validate } from "react-hook-form";
import CHDatePicker from "../../../../../components/ch-datepicker/CHDatepicker";

const ControllerFecha = (props: {form: UseFormReturn<any>, name: string, label: string, 
    disabled?: boolean, required?: boolean, validate?: Validate<any>}) => {
    
    const {form, name, label, disabled, required, validate} = props;
    
    return (
        <Controller
            name={name}
            control={form.control}
            rules={{ 
                required: {value: required ?? false, message: 'Este campo es requerido.'},
                validate
            }}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHDatePicker
                    label={label}
                    inputProps={{ readOnly: true }}
                    onChange={onChange}
                    error={!!error}
                    helperText={error ? error.message : null}
                    value={value}
                    autoOk
                    disabled={disabled ?? false}
                />
            )}
        />
    );
};

export default ControllerFecha;