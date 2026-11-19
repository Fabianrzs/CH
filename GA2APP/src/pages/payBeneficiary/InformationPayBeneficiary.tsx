import React from 'react';
import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { Controller } from 'react-hook-form';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHSelect from '../../components/ch-select/CHSelect';
import { IDENTIFICATIONTYPEDTO } from '../../types/catalog.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

const InformationPayBeneficiary = (props: props) => {
    const classes = useStyles();

    const { control, tiposIdentificacion } = props;

    return (
        <>
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="radicadoWorkManager"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: 'Este campo es requerido.',
                            },
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                label="Radicado Work Manager"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tipoDocumentoId"
                        control={control}
                        render={({ field: { onChange, value } }) => (
                            <CHSelect
                                defaultValue=""
                                label="Tipo de Identificación"
                                value={value}
                                onChange={onChange}
                            >
                                {tiposIdentificacion?.map(
                                    (option: IDENTIFICATIONTYPEDTO) => (
                                        <MenuItem
                                            key={option.id}
                                            value={option.id}
                                        >
                                            {option.descripcion}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="numeroIdentificacion"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: 'Este campo es requerido',
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
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                label="No. Identificación"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="primerNombre"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: 'Este campo es requerido.',
                            },
                            pattern: /^[ A-zÀ-ú ]+$/g,
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={
                                    error
                                        ? 'El nombre solo debe contener letras'
                                        : null
                                }
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                label="Primer Nombre"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="segundoNombre"
                        control={control}
                        defaultValue=""
                        rules={{
                            pattern: /^[ A-zÀ-ú ]+$/g,
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={
                                    error
                                        ? 'El nombre solo debe contener letras'
                                        : null
                                }
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                label="Segundo Nombre"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="primerApellido"
                        control={control}
                        rules={{
                            required: {
                                value: true,
                                message: 'Este campo es requerido.',
                            },
                            pattern: /^[ A-zÀ-ú ]+$/g,
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={
                                    error
                                        ? 'El apellido solo debe contener letras'
                                        : null
                                }
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                label="Primer Apellido"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="segundoApellido"
                        control={control}
                        rules={{
                            pattern: /^[ A-zÀ-ú ]+$/g,
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={
                                    error
                                        ? 'El apellido solo debe contener letras'
                                        : null
                                }
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                label="Segundo Apellido"
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
            </Grid>
        </>
    );
};

interface LinkMapDispatchToProps {
    tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
    control: any;
}

type props = LinkMapDispatchToProps;

export default InformationPayBeneficiary;
