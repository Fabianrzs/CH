import React, { useState } from 'react';
import { connect } from 'react-redux';
import {
    Button,
    Dialog,
    DialogContent,
    Grid,
    Typography,
    createStyles,
    makeStyles,
    MenuItem,
    FormControlLabel,
    Checkbox,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHSelect from '../../../components/ch-select/CHSelect';
import {
    TipoEmbargoDto,
    TipoRetencionDto,
} from '../../../types/embargo.model.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import UtilService from '../../../utils/util.service';
import history from '../../../utils/history.router'
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import { startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../../actions/client.action';
import { startGetNovedadesAfiliado } from '../../../actions/novedades.action';
import { AppState } from '../../../store/rootReducer';
import { ClientState } from '../../../reducers/client.reducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';

const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'center',
        },
        button: {
            width: '70%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
    }),
);

const ModalEdicionNovedades = (props: props) => {

    const { catalogState, clientState, loginState, startGetNovedadesAfiliado, startGetAllIdentificationTypes, startGetClientByIdentificationTypeIdentificationNumber } = props;

    const {
        handleClose,
        open,
        formularioPagos,
        tiposIdentificacion,
        tipoDocumento,
        nombreRazon,
        tipoCuenta,
        entidad,
    } = props;
    const classes = useStyles();

    const {control, handleSubmit} = useForm();
    const [documento, SetDocumento] = useState('');
    

    

    return (
        
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
                maxWidth="md"
            >
                <DialogContent style={{ width: '600px' }}>
                    <Grid item>
                        <br />
                        <Typography
                            className={classes.titleform}
                            variant="h5"
                            component="h6"
                        >
                            EDICIÓN DE NOVEDAD
                        </Typography>
                    </Grid>
                    <br />
                    <Grid container>
                        <Grid item md={10}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo de identificación"
                                    value={value}
                                    onChange={onChange}
                                >
                                    {catalogState.IdentificationType?.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="idTipoIdentificacion"
                        />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={10}>
                            <Controller
                                name='numeroDocumento'
                                control={control}
                                rules={{
                                    pattern: {
                                        value: /^[0-9]+$/g,
                                        message: 'Este campo es requerido y debe contener solo números'
                                    },
                                    required: {
                                        value: true,
                                        message: 'Este campo es requerido y debe contener solo números',
                                    },
                                    minLength: {
                                        value: 5,
                                        message:
                                            'La longitud mínima del campo es de 5 caracteres',
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
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        label="Número de Identificación *"
                                        value={value}
                                        numberMask
                                        onChange={e => {
                                            if (
                                                tipoDocumento === 1 ||
                                                tipoDocumento === 2 ||
                                                tipoDocumento === 5
                                            ) {
                                                if (e.target.value.length <= 12) {
                                                    onChange(
                                                        e.target.value.replace(
                                                            /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W]/g,
                                                            '',
                                                        ),
                                                    );
                                                }
                                            } else {
                                                onChange(e);
                                            }
                                            SetDocumento(e.target.value);
                                        }}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={10}>
                            <Controller
                                name="nombreCliente"
                                control={control}
                                rules={{
                                    required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                    },
                                    pattern: {
                                        value: /^[ A-zÀ-ú ]+$/g,
                                        message: 'Solo se permite letras',
                                    },
                                }}
                                render={({ field: { onChange, value } }) => {
                                    
                                    const nombresApellidos = value ? value.split(' ') : [];
                                    const nombres = nombresApellidos.slice(0, -2).join(' ');
                                    
                                    return (
                                    <CHTextField
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        label="Nombres"
                                        defaultValue={value}
                                        onChange={onChange}
                                        inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                        }}
                                    />
                                    );
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid item md={10}>
                            <Controller
                                name="nombreCliente"
                                control={control}
                                rules={{
                                    required: {
                                    value: true,
                                    message: 'Este campo es requerido',
                                    },
                                    pattern: {
                                        value: /^[ A-zÀ-ú ]+$/g,
                                        message: 'Solo se permite letras',
                                    },
                                }}
                                render={({ field: { onChange, value } }) => {
                                    const nombresApellidos = value ? value.split(' ') : [];
                                    const apellidos = nombresApellidos.slice(2, 4).join(' ');

                                    return (
                                    <CHTextField
                                        isReadOnly={false}
                                        InputLabelProps={{ shrink: true }}
                                        label="Apellidos"
                                        defaultValue={apellidos}
                                        onChange={onChange}
                                        inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                        }}
                                    />
                                    );
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container>
                        <Grid
                            className={classes.columnButtonAdd}
                            item
                            xs={12}
                            md={12}
                        >
                            <Button
                                className={classes.button}
                                variant="contained"
                                color="primary"
                                style={{ width: '200px' }}
                                onClick={() => {}}
                        >
                                ACTUALIZAR NOVEDAD
                            </Button>
                            <Button
                                className={classes.button}
                                variant="contained"
                                color="default"
                                style={{ width: '200px' }}
                                onClick={handleClose}
                            >
                                REGRESAR
                            </Button>

                        </Grid>
                    </Grid>
                </DialogContent>

            </Dialog>
    )
}

interface LinkMapStateToProps {
    loginState: LoginState;
    clientState: ClientState;
    catalogState: CatalogState;
}

interface LinkMapDispatchToProps {
    tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
    handleClose: any;
    open: any;
    formularioPagos: any;
    tipoDocumento: any;
    nombreRazon: any;
    tipoCuenta: any;
    entidad: any;
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void;
    startGetNovedadesAfiliado: any
}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
    loginState: state.login,
});

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetClientByIdentificationTypeIdentificationNumber,
    startGetNovedadesAfiliado,
};

type props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ModalEdicionNovedades);