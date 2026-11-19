import React from "react";
import { MenuItem } from "@material-ui/core";
import { Controller, UseFormReturn } from "react-hook-form";
import CHSelect from "../../../../../components/ch-select/CHSelect";
import Fuerza from "../types/Fuerza";

type ControllerProps = {
    form: UseFormReturn<any>,
    name?: string,
    fuerzas: Fuerza[],
    required?: boolean
}

const ControllerFuerzas = (props: ControllerProps) => {
    const {form, name, fuerzas, required} = props;
    
    return (
        <Controller
            name={name ?? "fuerza"}
            control={form.control}
            rules={{
                required: {value: required ?? false, message: 'Este campo es requerido.'},
            }}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHSelect
                    defaultValue={0}
                    label="Fuerza"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    helperText={error ? error.message : null}
                >
                    <MenuItem value='all'>
                        TODAS
                    </MenuItem>
                    {fuerzas?.map((item: any) => (
                        <MenuItem key={item.id} value={item.id}>
                            {item.descripcion}
                        </MenuItem>
                    ))}
                </CHSelect>
            )}
        />
    );
};

export default ControllerFuerzas;