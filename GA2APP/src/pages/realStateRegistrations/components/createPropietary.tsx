import React, { useEffect, useState } from 'react';
import { Grid, makeStyles, createStyles, Button, Dialog, DialogContent, DialogTitle, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { HistorialPropietariosDTO } from '../../../types/realStateRegistrations.type';
import { startAddPropietaryHistory } from '../../../actions/realStateRegistrations.action';
import { LoadingComponent } from '../../../components/loading/loading.component';
import { AppState } from '../../../store/rootReducer';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '15%',
            borderRadius: '10%',
            background: '#1C3F8B',
            color: '#ffff',
            marginBottom: '3%',
            marginLeft: '2%',
        },
        buttons: {
            color: '#fff',
            width: '15%',
            borderRadius: '10%',
            background: '#1C3F8B',
            marginBottom: '3%',
            marginLeft: '61%',
        },
        title: {
            textAlign: 'center',
        },
        table: {
            color: '#f1c3b90ff',
        }
    }),
);

function CreatePropietary({
    catalogState,
    realStateRegistrationsState,
    isOpen,
    onClose,
    startAddPropietaryHistory,
}:
    {
        realStateRegistrationsState: RealStateRegistrationsState;
        catalogState: CatalogState;
        isOpen: boolean;
        onClose: () => void;
        startAddPropietaryHistory: (data: HistorialPropietariosDTO) => void;
    }) {

    const classes = useStyles();
    const [tipoIdentificacion, setTipoIdentificacion] = useState<any>('');
    const { control, setValue, handleSubmit } = useForm<HistorialPropietariosDTO>({
        defaultValues: {
            correo: '',
            maiId: realStateRegistrationsState.informacionBasica.maiId,
            prpId: realStateRegistrationsState.informacionBasica.maiId,
            prpIdTipoIdentificacion: 0,
            prpNombreRazonSocial: '',
            prpPrimerNombre: undefined,
            prSegundoNombre: '',
            prpPrimerApellido: '',
            prpSegundoApellido: '',
            prpNumeroIdentificacion: '',
            prpTipoIdentificacion: ''
        }
    })

    useEffect(() => {
        setValue('prpPrimerNombre', "");
        setValue('prSegundoNombre', "");
        setValue('prpPrimerApellido', "");
        setValue('prpSegundoApellido', "");
        setValue('prpNombreRazonSocial', "");
        setValue('prpNumeroIdentificacion', "");
        setValue('correo', "");
        setValue('telefono', "");
    }, [realStateRegistrationsState])

    const handleClose = () => {
        onClose();
    };

    const CreatePropietaryHandleSubmit = (data: HistorialPropietariosDTO) => {

        if (data.prpIdTipoIdentificacion !== 2) {
            data.prpNombreRazonSocial = "";
            setValue('prpNombreRazonSocial', "");
        } else {
            data.prpPrimerNombre = "";
            data.prSegundoNombre = "";
            data.prpPrimerApellido = "";
            data.prpSegundoApellido = "";
            setValue('prpPrimerNombre', "");
            setValue('prSegundoNombre', "");
            setValue('prpPrimerApellido', "");
            setValue('prpSegundoApellido', "");
        }
        data.telefono = data.telefono?.replaceAll('(', '')?.replaceAll(')', '')?.replaceAll(' ', '')?.replaceAll('-', '')
        startAddPropietaryHistory(data);
        onClose();
    }

    return (
        <Dialog className={classes.table}
            fullWidth
            open={isOpen}
            maxWidth="md"
            aria-labelledby="form-dialog-title">
            <LoadingComponent key="loader" loading={realStateRegistrationsState.loading} />
            <DialogTitle className={classes.title} id="form-dialog-title">AGREGAR HISTORIAL PROPIETARIO</DialogTitle>
            <DialogContent>
                <Grid item container>
                    <Grid container md={12}>
                        <Grid className="item-containerModal" md={5}>
                            <Grid className="subcontainerModal">
                                <Controller
                                    name="prpIdTipoIdentificacion"
                                    control={control}
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHSelect
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
                                        </CHSelect>
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
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
                                            label="Número de Identificación"
                                            required
                                            numberMask
                                            onChange={onChange}
                                            value={value}
                                            helperText={error ? error.message : null}
                                            error={!!error}

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
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
                                            label="Razón Social"
                                            required
                                            onChange={onChange}
                                            value={value.toUpperCase()}
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
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Primer Nombre"
                                                required
                                                onChange={(l: any) => {
                                                    const valor = l.target.value
                                                    onChange(valor.toUpperCase())
                                                }}
                                                value={value.toUpperCase()}
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
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Segundo Nombre"
                                                onChange={(l: any) => {
                                                    const valor = l.target.value
                                                    onChange(valor.toUpperCase())
                                                }}
                                                value={value.toUpperCase()}
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
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Primer Apellido"
                                                required
                                                onChange={(l: any) => {
                                                    const valor = l.target.value
                                                    onChange(valor.toUpperCase())
                                                }}
                                                value={value.toUpperCase()}
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
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Segundo Apellido"
                                                onChange={(l: any) => {
                                                    const valor = l.target.value
                                                    onChange(valor.toUpperCase())
                                                }}
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
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
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
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
                                            error={!!error}
                                            helperText={error ? 'El celular debe comenzar por 3 y tener una longitud de 10 dígitos' : null}
                                            cellPhoneMask
                                            isReadOnly={false}
                                            label="Celular"
                                            value={value}
                                            onChange={onChange}
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
                        onClick={handleSubmit(CreatePropietaryHandleSubmit)}
                    >
                        AGREGAR
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
    startAddPropietaryHistory,
};

export default connect(mapStateToProps, mapDispatchToProps)(CreatePropietary);