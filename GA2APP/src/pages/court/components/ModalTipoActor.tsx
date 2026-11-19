import React, { useState } from 'react';
import {
    Button,
    Grid, MenuItem,
} from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import CHSelect from '../../../components/ch-select/CHSelect';

export const ModalTipoActor = (props: any) => {
    const { formularioActor, actores, setTipoActor, setNewForm, EliminarTipoActorEmbargo, AgregarTipoActorEmbargo } = props;
    const [check, setCheck] = useState(true);


    const BackForm = async () => {
        setNewForm(false)
        formularioActor.setValue('otroTipoActorComf', '')
        formularioActor.setValue('otroTipoActor', '')

    }
    return (
        <>
            {check ? (
                <>
                    <Grid container spacing={3}>
                        <Grid item xs={10}>
                            <Controller
                                name="otroTipoActor"
                                control={formularioActor.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: "Este campo es requerido",
                                    },
                                    minLength: {
                                        value: 3,
                                        message: "La longitud mínima del campo es de 3 caracteres",
                                    },
                                    pattern: {
                                        value: /^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9.\s-]*$/,
                                        message: "Solo se admiten letras y espacios",
                                    },
                                    validate: (value) => value === formularioActor.getValues("otroTipoActorComf") || "Los valores no coinciden",
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        label="Tipo de actor*"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: "uppercase",
                                            },
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>

                    <Grid container spacing={3}>
                        <Grid item xs={10}>
                            <Controller
                                name="otroTipoActorComf"
                                control={formularioActor.control}
                                rules={{
                                    required: {
                                        value: true,
                                        message: "Este campo es requerido",
                                    },
                                    minLength: {
                                        value: 3,
                                        message: "La longitud mínima del campo es de 3 caracteres",
                                    },
                                    pattern: {
                                        value: /^[A-Za-zÁÉÍÓÚáéíóúÑñ0-9.\s-]*$/,
                                        message: "Solo se admiten letras y espacios",
                                    },
                                    validate: (value) => value === formularioActor.getValues("otroTipoActor") || "Los valores no coinciden",
                                }}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        label="Confirmar tipo actor*"
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        onChange={onChange}
                                        inputProps={{
                                            style: {
                                                textTransform: "uppercase",
                                            },
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>

                </>
            ) : (
                <Grid item xs={10}>
                    <Controller
                        name='tipoActor'
                        control={formularioActor.control}
                        rules={{
                            validate: () => formularioActor.getValues("tipoActor") !== 0,
                            required: 'Este campo es obligatorio',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                error={!!error}
                                label="Tipo de actor *"
                                value={value}
                                helperText={error ? error.message : null}
                                onChange={(event) => {
                                    const newValue = event.target.value;
                                    onChange(newValue);
                                    setTipoActor(newValue);
                                }}
                            >
                                {actores?.map((member: any) => (
                                    <MenuItem
                                        key={member.idTipoActorEmbargo}
                                        value={member.idTipoActorEmbargo}
                                    >
                                        {member.descripcionTipoActorEmbargo}
                                    </MenuItem>
                                ))}
                            </CHSelect>
                        )}
                    />
                </Grid>
            )}
            <Grid container spacing={3}>
                <Grid item xs={10}>
                    <Controller
                        name="tipoActor"
                        control={formularioActor.control}
                        render={({ field: { onChange, value } }) => (
                            <CHCheckbox
                                label="¿Eliminar Actor?"
                                onChange={(e) => {
                                    onChange(e.target.checked);
                                    setCheck(!e.target.checked);
                                }}
                                checked={value}
                            />
                        )}
                    />
                </Grid>
            </Grid>

            <Button
                size="small"
                variant="contained"
                color="primary"
                onClick={check ? formularioActor.handleSubmit(AgregarTipoActorEmbargo) : formularioActor.handleSubmit(EliminarTipoActorEmbargo)}
            >
                {check ? 'GUARDAR' : 'ELIMINAR'}
            </Button>
            <Button
                size="small"
                variant="contained"
                color="primary"
                onClick={() => BackForm()}
                style={{
                    marginLeft: '10px',
                }}
            >
                Atras
            </Button>
        </>
    );
};
