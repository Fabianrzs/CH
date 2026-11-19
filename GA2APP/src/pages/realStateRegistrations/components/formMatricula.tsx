import React, { useEffect, useState } from 'react';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { Controller, useForm } from 'react-hook-form';
import { AppState } from '../../../store/rootReducer';
import CHTextFieldV2 from '../../../components/ch-text-field/CHTextFieldV2';
import CHPaperNew from '../../../components/ch-paper/CHPaperV2';
import AddressComponentV2 from '../../../components/modals/address/addressV2.component'
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import { Address } from '../../../types/address.type';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';
import history from '../../../utils/history.router';
import { AgregarMatriculaDTO } from '../../../types/realStateRegistrations.type';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import {
    CITYDTO,
    DEPARTAMENTODTO,
    IDENTIFICATIONTYPEDTO,
    OFICINASREGISTRODTO,
} from '../../../types/catalog.type';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { startAddRealStateRegistrations } from '../../../actions/realStateRegistrations.action';
import { LoadingComponent } from '../../../components/loading/loading.component';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CHAutoCompleteV2 from '../../../components/ch-auto-complete/CHAutoCompleteV2';
import {
    startGetAllCities,
    startGetAllDepartments,
    startGetRegistrationOffice
} from '../../../actions/catalog.action';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';

const useStyles = makeStyles(() =>
    createStyles({
        button: {
            width: '120px',
            borderRadius: '5%',
            backgroundColor: '#1C3F8B',
            color: '#ffffff',
            textAlign: 'right',
            marginBottom: '20px',
            marginLeft: '40%',
        },

        button3: {
            padding: '10px',
            marginRight: '10px',
            background: '#1C3F8B',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
        },
        button4: {
            padding: '10px',
            marginRight: '10px',
            background: '#1C3F8B',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
        },
        rowFlex: {
            display: 'flex',
        },
    }),
);

function formMatricula({
    catalogState,
    realStateRegistrationsState,
    startAddRealStateRegistrations,
    startGetAllDepartments,
    startGetAllCities,
    startGetRegistrationOffice,
}: {
    catalogState: CatalogState;
    realStateRegistrationsState: RealStateRegistrationsState;
    startAddRealStateRegistrations: (data: AgregarMatriculaDTO) => void;
    startGetAllDepartments: () => void;
    startGetAllCities: () => void;
    startGetRegistrationOffice: () => void;
}): any {
    const classes = useStyles();
    const [codigoRegistro, setCodigoRegistro] = useState(String);

    useEffect(() => {
        startGetAllDepartments();
        startGetAllCities();
        startGetRegistrationOffice();
    }, []);

    const [tipoIdentificacion, setTipoIdentificacion] = useState<any>('');
    const { control, setValue, watch, handleSubmit } = useForm<AgregarMatriculaDTO>({});

    const addMatricula = (data: AgregarMatriculaDTO) => {
        const cleanPhoneNumber = (number: any) => number.replace(/[()\s-]/g, '');
        data.numeroMatricula = `${codigoRegistro}-${data.numeroMatricula}`;
        data.celular = cleanPhoneNumber(data.celular)

        startAddRealStateRegistrations(data);
    };

    // Autocomplete departamentoNacimiento
    const catalogCity = [...catalogState.City];
    const [departamentoValue, setDepartamentoValue] =
        React.useState<DEPARTAMENTODTO | null>(null);
    const [ciudadValues, setCiudadValues] =
        React.useState<CITYDTO[]>(catalogCity);

    const oficinaRegistro = [...catalogState.oficinasRegistro];
    const [oficinaRegistroValues, setOficinaRegistroValues] = React.useState<OFICINASREGISTRODTO[]>(oficinaRegistro);

    const [oficinaRegistroValue, setOficinaRegistroValue] = React.useState<OFICINASREGISTRODTO | null>(null);

    // Autocomplete ciudad
    const [ciudadValue, setCiudadValue] = React.useState<CITYDTO | null>(null);
    const [open, setOpen] = useState(false);

    const handleOpenAddress = () => {
        setOpen(true);
    };

    const onCloseAddress = (value: Address) => {
        if (value) {
            const result = setFormDataAddres(value);
            setValue('direccion', result);
        }
        setOpen(false);
    };

    const onChangeDepartment = (
        event: any,
        newValue: DEPARTAMENTODTO | null,
    ) => {
        setCiudadValues(
            catalogCity.filter(x => x.idDepartamento === newValue?.codigo),
        );
        setValue('numeroMatricula', '')
        setValue('departamento', newValue?.id as number)
        setValue('codigoOficina', '')
        setDepartamentoValue(newValue);
        setCiudadValue(null);
        setOficinaRegistroValue(null);
    };

    const onChangeCity = (event: any, newValue: CITYDTO | null) => {

        setCiudadValue(newValue);
        if(newValue){
            setValue('ciudad',newValue?.id);
        }
        setOficinaRegistroValues(catalogState.oficinasRegistro.filter(x => x.codigoOficina === newValue?.codigoCirRegistral));
        if (newValue?.nombre === 'BOGOTÁ  D.C.') {

            // Show the options for BOGOTA
            const resultadoFiltrado = catalogState.oficinasRegistro.filter((objeto: OFICINASREGISTRODTO) => objeto.codigoDepartamento === 25 && (objeto.idOficina === 19 || objeto.idOficina === 20 || objeto.idOficina === 21));
            setOficinaRegistroValues(resultadoFiltrado);
        }
        else if (newValue?.nombre === 'MEDELLIN') {
            const resultadoFiltrado = catalogState.oficinasRegistro.filter((objeto: OFICINASREGISTRODTO) => objeto.codigoDepartamento === 5
                && objeto.idOficina === 96);
            const resultMedellin = catalogState.oficinasRegistro.filter((objeto: OFICINASREGISTRODTO) => objeto.codigoDepartamento === 5
                && objeto.idOficina === 97);
            resultadoFiltrado.push(resultMedellin[0])
            setOficinaRegistroValues(resultadoFiltrado);
        }
        else {
            // Show default options for other cities
            setOficinaRegistroValues(catalogState.oficinasRegistro);
            setCiudadValue(newValue);
            setOficinaRegistroValues(catalogState.oficinasRegistro.filter(x => x.codigoOficina === newValue?.codigoCirRegistral));
        }

    };

    const onChangeOficinaRegistro = (event: any, newValue: OFICINASREGISTRODTO | any) => {

        const nuevoValor = String(newValue?.codigoOficina)
        setCodigoRegistro(newValue?.codigoOficina as any);
        setValue('idOficinaRegistro', newValue?.idOficina as number)
        setValue('codigoOficina', nuevoValor)
        setOficinaRegistroValue(newValue);
    };

    return (
        <CHPrivateContainer>
            <PageIndicators
                topTextNivel1="Matrículas"
                topTextNivel2="Matrículas Inmobiliarias"
            />
            <IconTitle
                subTitle="Inmobiliarias"
                title="Agregar Matrículas"
                icon="fch-Inicio-0-1"
            />
            <LoadingComponent
                key="loader"
                loading={realStateRegistrationsState.loading}
            />
            <CHPaperNew>
                <AddressComponentV2
                    onOpen={open}
                    onClose={onCloseAddress}
                    tipoDireccion={1 === 1}
                />
                <Grid container>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className="subcontainerModal">
                            <Controller
                                name="departamento"
                                rules={{
                                    validate: () => departamentoValue === null ? "Seleccione un departamento" : true
                                }}
                                control={control}
                                render={({ fieldState: { error } }) => (
                                    <CHAutoCompleteV2
                                        value={departamentoValue}
                                        options={catalogState?.departments}
                                        getOptionLabel={option => option.nombre}
                                        defaultValue={catalogState?.departments.find(x => x.id === 0)}
                                        onChange={onChangeDepartment}
                                        renderInput={params => (
                                            <CHTextField
                                                {...params}
                                                InputLabelProps={{ shrink: true }}
                                                label="Departamento*"
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className="subcontainerModal">
                            <Controller
                                name="ciudad"
                                rules={{
                                    validate: () => ciudadValue === null ? "Seleccione una ciudad" : true
                                }}
                                control={control}
                                render={({ fieldState: { error } }) => (
                                    <CHAutoCompleteV2
                                        value={
                                            ciudadValue == null
                                                ? null
                                                : ciudadValue
                                        }
                                        options={ciudadValues}
                                        getOptionLabel={option => option.nombre}
                                        onChange={onChangeCity}
                                        renderInput={params => (
                                            <CHTextField
                                                {...params}
                                                InputLabelProps={{ shrink: true }}
                                                label="Ciudad*"
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    <Grid className={`item-containerModal ${classes.rowFlex}`} md={5}>
                        <Grid className="subcontainerModal" md={9}>
                            <Controller
                                name="idOficinaRegistro"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={control}
                                render={({ fieldState: { error } }) => (
                                    <CHAutoCompleteV2
                                        value={
                                            oficinaRegistroValue == null
                                                ? null
                                                : oficinaRegistroValue
                                        }
                                        options={oficinaRegistroValues}

                                        getOptionLabel={option => option.descripcionOficina}
                                        onChange={onChangeOficinaRegistro}
                                        renderInput={params => (
                                            <CHTextField
                                                {...params}
                                                InputLabelProps={{ shrink: true }}
                                                label="Oficinas de Registro"
                                                error={!!error}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                )}
                            />
                        </Grid>
                        <Grid className="subcontainerModal" md={3}>
                            <Controller
                                name="codigoOficina"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
                                        label="Cod Oficina"
                                        onChange={(onChange)}
                                        value={value}
                                        error={!!error}
                                        isReadOnly
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
                                name="numeroMatricula"
                                rules={{
                                    required: 'Este campo es requerido',
                                    maxLength: {
                                        value: 21,
                                        message:
                                            'La longitud máxima del campo es de 21 caracteres',
                                    },
                                    min: {
                                        value: 1,
                                        message: 'No se aceptan valores negativos'
                                    },
                                    pattern: {
                                        value: /^[0-9]+$/,
                                        message: 'No se aceptan ese tipo de valores.',
                                    }
                                }}
                                control={control}
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextFieldV2
                                        InputLabelProps={{ shrink: true }}
                                        error={!!error}
                                        onChange={(onChange)}
                                        value={value}
                                        label="Número Matrícula*"
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
                                name="direccion"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
                                        label="Dirección*"
                                        onChange={(onChange)}
                                        value={value}
                                        error={!!error}
                                        onClick={handleOpenAddress}
                                        isReadOnly
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
                                name="tipoIdentificacion"
                                rules={{
                                    validate: () => watch('tipoIdentificacion') < 1 || watch('tipoIdentificacion') == null ? 'El tipo de identificación es obligatorio' : true
                                }}
                                control={control}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        error={!!error}
                                        label="Tipo de Identificación*"
                                        value={value}
                                        helperText={
                                            error ? error.message : null
                                        }
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
                    </Grid>
                    <Grid className="item-containerModal" md={5}>
                        <Grid className="subcontainerModal">
                            <Controller
                                name="numeroIdentificacion"
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
                                defaultValue=""
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
                                        label="Número De Identificación*"
                                        onChange={(onChange)}
                                        value={value}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        numberMask
                                    />
                                )}
                            />
                        </Grid>
                    </Grid>
                    {tipoIdentificacion === 'NIT' && (
                        <Grid className="item-containerModal" md={5}>
                            <Grid className="subcontainerModal">
                                <Controller
                                    name="razonSocial"
                                    rules={{
                                        required: 'Este campo es requerido',
                                    }}
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
                                            label="Razón Social*"
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
                    )}

                    {tipoIdentificacion === 'OTHER' && (
                        <>
                            <Grid className="item-containerModal" md={5}>
                                <Grid className="subcontainerModal">
                                    <Controller
                                        name="primerNombre"
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        control={control}
                                        defaultValue=""
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Primer Nombre*"
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
                            <Grid className="item-containerModal" md={5}>
                                <Grid className="subcontainerModal">
                                    <Controller
                                        name="segundoNombre"
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
                                        name="primerApellido"
                                        rules={{
                                            required: 'Este campo es requerido',
                                        }}
                                        control={control}
                                        defaultValue=""
                                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                                            <CHTextField
                                                InputLabelProps={{ shrink: true }}
                                                label="Primer Apellido*"
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
                            <Grid className="item-containerModal" md={5}>
                                <Grid className="subcontainerModal">
                                    <Controller
                                        name="segundoApellido"
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
                                name="celular"
                                control={control}
                                defaultValue=""
                                rules={{
                                    pattern: /\(?([3-3]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
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
                    <Grid className="item-containerModal" md={12}>
                        <Grid className="subcontainerModal">
                            <Button
                                className={classes.button4}
                                onClick={() => history.push('/Matriculas')}
                            >
                                REGRESAR
                            </Button>
                            <Button
                                className={classes.button3}
                                onClick={handleSubmit(addMatricula)}
                            >
                                AGREGAR
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </CHPaperNew>
        </CHPrivateContainer>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    realStateRegistrationsState: state.realStateRegistrations,
});

const mapDispatchToProps = {
    startAddRealStateRegistrations,
    startGetAllDepartments,
    startGetAllCities,
    startGetRegistrationOffice,
};

export default connect(mapStateToProps, mapDispatchToProps)(formMatricula);