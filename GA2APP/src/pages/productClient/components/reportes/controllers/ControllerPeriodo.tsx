import React from "react";
import { Controller, UseFormReturn, Validate } from "react-hook-form";
import moment from "moment";
import CHDatePicker from "../../../../../components/ch-datepicker/CHDatepicker";

const ControllerPeriodo = (props: {form: UseFormReturn<any>, name?: string, type?: 'year' | 'month', 
    disabled?: boolean, required?: boolean, validate?: Validate<any>}) => {
    
    const {form, name, type, disabled, required, validate} = props;
    
    return (
        <Controller
            name={name ?? "periodoReporte"}
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
                    label="Periodo"
                    inputProps={{ readOnly: true }}
                    onChange={(date, _) => 
                        onChange(moment(date).endOf('month').startOf('day').toDate())
                    }
                    error={!!error}
                    helperText={error ? error.message : null}
                    value={value}
                    autoOk
                    views={type === 'year' ? ['year'] : ['year', 'month']}
                    format={type === 'year' ? 'yyyy': 'yyyy-MMMM'}
                    placeholder={type === 'year' ? 'YYYY': 'YYYY-MM'}
                    disabled={disabled ?? false}
                />
            )}
        />
    );
};

export default ControllerPeriodo;