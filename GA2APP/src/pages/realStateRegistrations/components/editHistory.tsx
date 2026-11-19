import React, { useEffect, useState } from 'react';
import { Grid, makeStyles, createStyles, Button, Dialog, DialogContent, DialogTitle, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import CHTextFieldV2 from '../../../components/ch-text-field/CHTextFieldV2';
import CHSelectV2 from '../../../components/ch-select/CHSelectV2';
import AuthService from '../../../services/auth.service';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { HistorialPropietariosDTO, HistorialPropietariosDTOsInitial } from '../../../types/realStateRegistrations.type';
import { startUpdatePropietaryHistory } from '../../../actions/realStateRegistrations.action';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { AppState } from '../../../store/rootReducer';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '15%',
            borderRadius: '10%',
            background: '#fec917',
            color: '#1c3b90',
            marginBottom: '3%',
            marginLeft: '2%',
        },
        buttons: {
            color: '#fff',
            width: '15%',
            borderRadius: '10%',
            background: '#42b7ef',
            marginBottom: '3%',
            marginLeft: '61%',
        },
        title: {
            color: '#42b7ef',
            textAlign: 'center',
        },
        table: {
            color: '#f1c3b90ff',
        }
    }),
);

function editHistory({
    catalogState,
    realStateRegistrationsState,
    isOpen,
    data,
    onClose,
    startUpdatePropietaryHistory,
    consultar,
}:
    {
        realStateRegistrationsState: RealStateRegistrationsState;
        catalogState: CatalogState;
        isOpen: boolean;
        data: HistorialPropietariosDTO;
        onClose: () => void;
        startUpdatePropietaryHistory: (data: HistorialPropietariosDTO) => void;
        consultar: () => void;
    }) {

    const classes = useStyles();
    const [tipoIdentificacion, setTipoIdentificacion] = useState<any>('');
    const [hasLoadedFieldsModal, setHasLoadedFieldsModal] = useState(false);
    const { control, setValue, handleSubmit, watch } = useForm<HistorialPropietariosDTO>({
        defaultValues: HistorialPropietariosDTOsInitial
    })

    const handleClose = () => {
        onClose();
    };

    const nombreUserLogin = (): string => {
        const user = AuthService.getUserInfo();
        return `${user.USR_NOMBRE} ${user.USR_APELLIDO}`;
    };

    useEffect(() => {
        setValue('prpIdTipoIdentificacion', Number(data.prpIdTipoIdentificacion));
        setValue('prpNumeroIdentificacion', data.prpNumeroIdentificacion);
        setValue('correo', data.correo);
        setValue('telefono', data.telefono);
        setValue('prpPrimerNombre', data.prpPrimerNombre)
        setValue('prSegundoNombre', data.prSegundoNombre)
        setValue('prpPrimerApellido', data.prpPrimerApellido)
        setValue('prpSegundoApellido', data.prpSegundoApellido)
        setValue('prpNombreRazonSocial', data.prpNombreRazonSocial);
        if (data.prpIdTipoIdentificacion !== 2) {
            setTipoIdentificacion("OTHER");

        } else {
            setTipoIdentificacion("NIT");
        }
    }, [data]);

    const EditPropietaryHandleSubmit = (data: HistorialPropietariosDTO) => {
        try {
            if (data.prpIdTipoIdentificacion !== 2) {
                data.prpNombreRazonSocial = ""
            } else {
                data.prpPrimerNombre = "";
                data.prSegundoNombre = "";
                data.prpPrimerApellido = "";
                data.prpSegundoApellido = "";
            }
            data.nombreUsuario = nombreUserLogin();
            const response: any = startUpdatePropietaryHistory(data);

            // Establecer los valores en los campos correspondientes utilizando setValue
            setValue('prpIdTipoIdentificacion', Number(response?.prpIdTipoIdentificacion));
            setValue('prpNumeroIdentificacion', response.prpNumeroIdentificacion);
            setValue('prpNombreRazonSocial', response.prpNombreRazonSocial);
            setValue('prpPrimerNombre', data.prpPrimerNombre)
            setValue('prSegundoNombre', data.prSegundoNombre)
            setValue('prpPrimerApellido', data.prpPrimerApellido)
            setValue('prpSegundoApellido', data.prpSegundoApellido)
            setValue('prpNombreRazonSocial', data.prpNombreRazonSocial);
            // Establecer los valores en otros campos según corresponda
            consultar();
            onClose();
        } catch (error) {
            console.error(error);
        }
    };

    if (!hasLoadedFieldsModal && data?.prpId != null) {
        setHasLoadedFieldsModal(true);
        const fields = Object.keys(data);
        fields.forEach(field => {
            const value = data[field as keyof HistorialPropietariosDTO];
            setValue(field as any, value);
        });
    }

    return (
        <Dialog className={classes.table}
            fullWidth
            open={isOpen}
            maxWidth="md"
            aria-labelledby="form-dialog-title">
            <LoadingComponent key="loader" loading={realStateRegistrationsState.loading} />
            <DialogTitle className={classes.title} id="form-dialog-title">EDITAR HISTORIAL PROPIETARIO</DialogTitle>
            <DialogContent>
                <Grid item container>
                    <Grid container md={12}>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className="subcontainerModal">
                                <Controller
                                    name="prpIdTipoIdentificacion"
                                    rules={{
                                        validate: () => watch('prpIdTipoIdentificacion') < 1 || watch('prpIdTipoIdentificacion') == null ? 'El tipo de identificación es obligatorio' : true
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelectV2
                                            error={!!error}
                                            label="Tipo de Identificación*"
                                            value={value}
                                            helperText={error ? error.message : null}
                                            onChange={e => {
                                                onChange(e);
                                                const { value } = e.target;
                                                switch (value) {
                                                    case 2:
                                                        setTipoIdentificacion('NIT');
                                                        break;
                                                    default:
                                                        setTipoIdentificacion('OTHER');
                                                        break;
                                                }
                                            }}
                                        >
                                            {catalogState.IdentificationType?.map(
                                                (option: IDENTIFICATIONTYPEDTO) => (
                                                    <MenuItem key={option.id} value={option.id}>
                                                        {option.descripcion}
                                                    </MenuItem>
                                                ),
                                            )}
                                        </CHSelectV2>
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className="subcontainerModal">
                                <Controller
                                    name="prpNumeroIdentificacion"
                                    rules={{
                                        required: {
                                            value: true,
                                            message: 'Este campo es requerido y debe contener solo números',
                                        },
                                        pattern: {
                                            value: /^[0-9]+$/g,
                                            message: 'Este campo es requerido y debe contener solo números'
                                        },

                                        minLength: {
                                            value: 4,
                                            message:
                                                'La longitud mínima del campo es de 4 caracteres',
                                        },
                                        maxLength: {
                                            value: 12,
                                            message:
                                                'La longitud máxima del campo es de 12 caracteres',
                                        },
                                        min: {
                                            value: 1,
                                            message: 'Número de cédula inválido'
                                        },
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error }
                                    }) => (
                                        <CHTextFieldV2
                                            label="Número de Identificación*"
                                            onChange={onChange}
                                            value={value}
                                            numberMask
                                            error={!!error}
                                            helperText={error != null ? error.message : ""}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    {tipoIdentificacion === 'NIT' && (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className="subcontainerModal">
                                <Controller
                                    name="prpNombreRazonSocial"
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextFieldV2
                                            label="Razón Social*"
                                            onChange={(onChange)}
                                            value={value}
                                            error={!!error}
                                            helperText={
                                                error ? error.message : null
                                            }
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    )}
                    {tipoIdentificacion === 'OTHER' && (
                        <>
                            <Grid className="item-containerModal" md={5}>
                                <Grid className="subcontainerModal">
                                    <Controller
                                        name="prpPrimerNombre"
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        control={control}
                                        defaultValue=""
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <CHTextFieldV2
                                                label="Primer Nombre*"
                                                onChange={(onChange)}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid className="item-containerModal" md={5}>
                                <Grid className="subcontainerModal">
                                    <Controller
                                        name="prSegundoNombre"
                                        control={control}
                                        defaultValue=""
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <CHTextFieldV2
                                                label="Segundo Nombre*"
                                                onChange={(onChange)}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid className="item-containerModal" md={5}>
                                <Grid className="subcontainerModal">
                                    <Controller
                                        name="prpPrimerApellido"
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        control={control}
                                        defaultValue=""
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <CHTextFieldV2
                                                label="Primer Apellido*"
                                                onChange={(onChange)}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                            <Grid className="item-containerModal" md={5}>
                                <Grid className="subcontainerModal">
                                    <Controller
                                        name="prpSegundoApellido"
                                        control={control}
                                        defaultValue=""
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <CHTextFieldV2
                                                label="Segundo Apellido*"
                                                onChange={(onChange)}
                                                value={value}
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                </Grid>
                            </Grid>
                        </>
                    )}
                    <Grid container md={12}>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className="subcontainerModal">
                                <Controller
                                    name="correo"
                                    rules={{
                                        pattern: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,
                                    }}
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextFieldV2
                                            error={!!error}
                                            helperText={
                                                error
                                                    ? 'Ingrese un correo electrónico válido'
                                                    : null
                                            }
                                            label="Correo Electrónico"
                                            onChange={onChange}
                                            value={value}
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className="subcontainerModal">
                                <Controller
                                    name="telefono"
                                    control={control}
                                    defaultValue=""
                                    rules={{
                                        pattern: /\(?([3-3]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                                    }}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextFieldV2
                                            error={!!error}
                                            helperText={error ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos' : null}
                                            isReadOnly={false}
                                            label="Celular"
                                            value={value}
                                            onChange={onChange}
                                            cellPhoneMask
                                        />
                                    )}
                                />
                            </Grid>
                        </Grid>
                    </Grid>
                    <Button
                        className={classes.buttons}
                        type="button"
                        onClick={handleClose}

                    >
                        REGRESAR
                    </Button>
                    <Button
                        className={classes.button}
                        type="submit"
                        onClick={handleSubmit(EditPropietaryHandleSubmit)}
                    >
                        ACTUALIZAR
                    </Button>
                </Grid>
            </DialogContent>
        </Dialog>
    );
}

const mapStateToProps = (state: AppState) => ({
    realStateRegistrationsState: state.realStateRegistrations,
});

const mapDispatchToProps = {
    startUpdatePropietaryHistory,
};

export default connect(mapStateToProps, mapDispatchToProps)(editHistory);