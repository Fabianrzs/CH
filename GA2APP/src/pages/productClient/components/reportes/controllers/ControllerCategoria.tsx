import React from "react";
import { MenuItem } from "@material-ui/core";
import { Controller, UseFormReturn } from "react-hook-form";
import CHSelect from "../../../../../components/ch-select/CHSelect";
import Categoria from "../types/Categoria";

type ControllerProps = {
    form: UseFormReturn<any>,
    name?: string,
    categorias: Categoria[],
    required?: boolean
}

const ControllerCategoria = (props: ControllerProps) => {
    const {form, name, categorias, required} = props;
    
    return (
        <Controller
            name={name ?? "categoria"}
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
                    label="Categorías"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    helperText={error ? error.message : null}
                >
                    <MenuItem value='all'>
                        TODAS
                    </MenuItem>
                    {categorias?.map((item: any) => (
                        <MenuItem key={item.id} value={item.id}>
                            {item.descripcion}
                        </MenuItem>
                    ))}
                </CHSelect>
            )}
        />
    );
};

export default ControllerCategoria;