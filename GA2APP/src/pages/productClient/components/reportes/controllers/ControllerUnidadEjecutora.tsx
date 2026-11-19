import React from "react";
import { MenuItem } from "@material-ui/core";
import { Controller, UseFormReturn } from "react-hook-form";
import CHSelect from "../../../../../components/ch-select/CHSelect";
import UnidadEjecutora from "../types/UnidadEjecutora";

const ControllerUnidadEjecutora = (props: {form: UseFormReturn<any>, name?: string, unidadesEjecutoras: UnidadEjecutora[]}) => {
    const {form, name, unidadesEjecutoras} = props;
    
    return (
        <Controller
            name={name ?? "unidadEjecutora"}
            control={form.control}
            render={({
                field: { onChange, value },
                fieldState: { error },
            }) => (
                <CHSelect
                    defaultValue={0}
                    label="Unidad Ejecutora"
                    value={value}
                    onChange={onChange}
                    error={!!error}
                    helperText={error ? error.message : null}
                >
                    <MenuItem value={0}>
                        TODAS
                    </MenuItem>
                    {unidadesEjecutoras?.map((item: any) => (
                        <MenuItem key={item.id} value={item.id}>
                            {item.nombre}
                        </MenuItem>
                    ))}
                </CHSelect>
            )}
        />
    );
};

export default ControllerUnidadEjecutora;