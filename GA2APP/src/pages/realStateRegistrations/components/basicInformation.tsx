import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import { AppState } from '../../../store/rootReducer';
import { RealStateRegistrationsState } from '../../../reducers/realStateRegistrations.reducer';
import { MatriculasInmobiliariasDTO, MatriculasInmobiliariasInitial } from '../../../types/realStateRegistrations.type';
import { CatalogState } from '../../../reducers/catalog.reducer';
import AuthService from '../../../services/auth.service';
import CHAutoCompleteV2 from '../../../components/ch-auto-complete/CHAutoCompleteV2';
import { CITYDTO, DEPARTAMENTODTO, OFICINASREGISTRODTO, IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import { startGetAllDepartments, startGetAllCities, startGetRegistrationOffice } from '../../../actions/catalog.action';
import AddressComponent from '../../../components/modals/address/address.component'
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import { Address } from '../../../types/address.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import ApiRegistrations from '../../../services/api-realstateregistrations.service';

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
        labelOficina: {
            marginBottom: '5px'
        },
        button3: {
            width: '120px',
            padding: '10px',
            marginRight: '10px',
            background: '#1C3F8B',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
            marginLeft: '80%',
        },
        button4: {
            padding: '10px',
            marginRight: '10px',
            background: '#1C3F8B',
            color: '#FFFFFF',
            textAlign: 'center',
            borderRadius: '5%',
        },
        processButton: {
            backgroundColor: '#42b7ef',
            color: '#fff',
            width: '12%',
            marginLeft: '69%',
        },
        processButton2: {
            backgroundColor: '#fec917',
            color: '#1c3f8b',
            width: '12%',
            marginLeft: '1%',
        },
        rowFlex: {
            display: 'flex',
        },
        colorLable: {
            color: '#009fff!important',
        }
    }),

);
const colorLable = {
    color: '#009fff!important',
}
function basicInformation({
    realStateRegistrationsState,
    catalogState,
    startGetAllDepartments,
    startGetAllCities,
    startGetRegistrationOffice,
    consultar,
}: {
    realStateRegistrationsState: RealStateRegistrationsState;
    catalogState: CatalogState;
    startGetAllDepartments: () => void;
    startGetAllCities: () => void;
    startGetRegistrationOffice: () => void;
    consultar: () => void;
}): any {

    const classes = useStyles();
    const api = ApiRegistrations();
    const [open, setOpen] = useState(false);
    const [hasLoadedFields, setHasLoadedFields] = useState(false);
    const oficinaRegistro = [...catalogState.oficinasRegistro];
    const [oficinaRegistroValues, setOficinaRegistroValues] =
        React.useState<OFICINASREGISTRODTO[]>(oficinaRegistro);
    const [oficinaRegistroValue, setOficinaRegistroValue] = React.useState<OFICINASREGISTRODTO | null>(null);
    const { control, setValue, watch, handleSubmit, getValues } = useForm<MatriculasInmobiliariasDTO>({
        defaultValues: MatriculasInmobiliariasInitial
    })

    useEffect(() => {
        startGetAllDepartments();
        startGetAllCities();
        startGetRegistrationOffice();
        const oficinaSeleccionada = oficinaRegistro.find(x => x.idOficina === getValues("idOficinaRegistro"));
        const propietario: any = realStateRegistrationsState?.informacionBasica?.propietarios.sort((a: any, b: any) => new Date(b.pmaFecCre).getTime() - new Date(a.pmaFecCre).getTime())
            .filter((propietario: any, index: any) => (index === 0));
        setValue("codigoOficina", oficinaSeleccionada?.codigoOficina || "");
        setValue("tipoIdentificacion", propietario[0]?.prpIdTipoIdentificacion || 0);
        setValue("prpNumeroIdentificacion", propietario[0]?.prpNumeroIdentificacion || "");
        const maiNumeroMatricula = realStateRegistrationsState?.informacionBasica?.maiNumeroMatricula;
        const partes = maiNumeroMatricula.split("-");
        const valorSeparado = partes.length > 1 ? partes[1].trim() : "";
        setValue("maiNumeroMatricula", valorSeparado)
    }, []);

    useEffect(() => {
        const oficinaSeleccionada = oficinaRegistro.find(x => x.idOficina === getValues("idOficinaRegistro"));
        const propietario: any = realStateRegistrationsState?.informacionBasica?.propietarios.sort((a: any, b: any) => new Date(b.pmaFecCre).getTime() - new Date(a.pmaFecCre).getTime())
            .filter((propietario: any, index: any) => (index === 0));
        setValue("codigoOficina", oficinaSeleccionada?.codigoOficina || "");
        setValue("tipoIdentificacion", propietario[0]?.prpIdTipoIdentificacion || 0);
        setValue("prpNumeroIdentificacion", propietario[0]?.prpNumeroIdentificacion || "");
        const maiNumeroMatricula = realStateRegistrationsState?.informacionBasica?.maiNumeroMatricula;
        const partes = maiNumeroMatricula.split("-");
        const valorSeparado = partes.length > 1 ? partes[1].trim() : "";
        setValue("maiNumeroMatricula", valorSeparado)
    }, [realStateRegistrationsState?.informacionBasica]);

    const nombreUserLogin = (): string => {
        const user = AuthService.getUserInfo();
        return `${user.USR_NOMBRE} ${user.USR_APELLIDO}`;
    };

    const idUserLogin = (): string => {
        const user = AuthService.getUserInfo();
        return user.USR_ID;
    };

    const updateBasicInformation = async (data: MatriculasInmobiliariasDTO) => {

        data.nombreUsuario = nombreUserLogin();
        data.idUsuario = idUserLogin();
        data.celular = data.celular?.replaceAll('(', '')?.replaceAll(')', '')?.replaceAll(' ', '')?.replaceAll('-', '');

        const propietario = realStateRegistrationsState?.informacionBasica?.propietarios.sort((a: any, b: any) => new Date(b.pmaFecCre).getTime() - new Date(a.pmaFecCre).getTime())
            .filter((propietario: any, index: any) => (index === 0));

        data.propietarios = [{ ...propietario[0], correo: data.correo, telefono: data.celular }];
        await actualizarDatosInformacionBasica(data);
        consultar();
    }

    const actualizarDatosInformacionBasica = async (data: any) => {
        await api
            .UpdateRealStateRegistrations(data)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        toast('Información actualizada');
                    }
                }
            })
            .catch((e: any) => {
                toast(`Error al actualizar información básica: ${e.message}`, { type: toast.TYPE.ERROR });
            });
    }

    if (!hasLoadedFields && realStateRegistrationsState?.informacionBasica != null) {
        setHasLoadedFields(true);
        const fields = Object.keys(realStateRegistrationsState?.informacionBasica);
        fields.forEach(field => {

            const value = realStateRegistrationsState?.informacionBasica[field as keyof MatriculasInmobiliariasDTO];
            if (field === "idOficinaRegistro") {
                setOficinaRegistroValues(catalogState.oficinasRegistro.filter(x => x.codigoDepartamento === realStateRegistrationsState?.informacionBasica?.dpdId));
                setOficinaRegistroValue(catalogState.oficinasRegistro.find(x => x.idOficina === value) as OFICINASREGISTRODTO);
                setValue(field as any, value);
            } else {
                setValue(field as any, value);
            }

            if (field === 'celular') {
                const propietario = realStateRegistrationsState?.informacionBasica?.propietarios.sort((a: any, b: any) => new Date(b.pmaFecCre).getTime() - new Date(a.pmaFecCre).getTime())
                    .filter((propietario: any, index: any) => (index === 0));
                setValue(field as any, propietario[0]?.telefono);
            }

            if (field === 'correo') {
                const propietario = realStateRegistrationsState?.informacionBasica?.propietarios.sort((a: any, b: any) => new Date(b.pmaFecCre).getTime() - new Date(a.pmaFecCre).getTime())
                    .filter((propietario: any, index: any) => (index === 0));
                setValue(field as any, propietario[0]?.correo);

            }
        });
    }

    // Autocomplete departamentoNacimiento
    const catalogCity = [...catalogState.City];
    const [departamentoValue, setDepartamentoValue] =
        React.useState<DEPARTAMENTODTO | null>(null);

    const [ciudadValues, setCiudadValues] =
        React.useState<CITYDTO[]>(catalogCity);

    const defaultDepartment = catalogState.departments?.find(
        (x: DEPARTAMENTODTO) =>
            x.id === realStateRegistrationsState?.informacionBasica.dpdId,
    );

    if (
        defaultDepartment != null &&
        departamentoValue == null &&
        defaultDepartment.id > 0
    ) {
        setDepartamentoValue(defaultDepartment);
        setCiudadValues(
            catalogCity.filter(
                x => x.idDepartamento === defaultDepartment?.codigo,
            ),
        );
    }

    // Autocomplete ciudad
    const [ciudadValue, setCiudadValue] =
        React.useState<CITYDTO | null>(null);

    const handleOpenAddress = () => {
        setOpen(true);
    };

    const onCloseAddress = (value: Address) => {
        if (!value) {
            setOpen(false);
            return;
        }
        const result = setFormDataAddres(value);
        setValue('maiDireccion', result);
        setOpen(false);
    };
    const [tipoIdentificacion, setTipoIdentificacion] = useState<any>('');
    const onChangeDepartment = (
        event: any,
        newValue: DEPARTAMENTODTO | null,
    ) => {
        setCiudadValues(catalogCity.filter(x => x.idDepartamento === newValue?.codigo));
        setDepartamentoValue(newValue);
        setValue('dpdId', newValue?.id as number)

        setOficinaRegistroValue(null);
        setOficinaRegistroValues(catalogState.oficinasRegistro.filter(x => x.codigoDepartamento === newValue?.id));
    };

    const defaultBirthCity = catalogCity?.find(
        (x: CITYDTO) =>
            x.id === realStateRegistrationsState?.informacionBasica.dcpId
    );

    if (
        defaultBirthCity != null &&
        ciudadValue == null &&
        defaultBirthCity.id > 0
    ) {
        setCiudadValue(defaultBirthCity);
    }

    const onChangeCity = (event: any, newValue: CITYDTO | null) => {

        setCiudadValue(newValue);
        setValue('dcpId', newValue?.id as number)

        setOficinaRegistroValues(catalogState.oficinasRegistro.filter(x => x.codigoOficina === newValue?.codigoCirRegistral));
        if (newValue?.nombre === 'BOGOTÁ, D.C.') {
            // Show the options for BOGOTA
            const resultadoFiltrado = catalogState.oficinasRegistro.filter((objeto: OFICINASREGISTRODTO) => objeto.codigoDepartamento === 11 && objeto.idOficina !== 235);
            setOficinaRegistroValues(resultadoFiltrado);
        }
        else if (newValue?.nombre === 'MEDELLÍN') {
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

    const onChangeOficinaRegistro = (event: any, newValue: OFICINASREGISTRODTO | null) => {

        setValue('idOficinaRegistro', newValue?.idOficina as number)
        setValue('codigoOficina', newValue?.codigoOficina as string)
        setOficinaRegistroValue(newValue);
    };

    return (
        <Grid container>
            <AddressComponent
                onOpen={open}
                onClose={onCloseAddress}
                control={control}
                getValues={watch}
                tipoDireccion={1 === 1}
                setValue={setValue}
            />
            <Grid container>
                <Grid className="item-containerModal" md={5}>
                    <Grid className="subcontainerModal">
                        <Controller
                            name="dpdId"
                            rules={{
                                validate: () => departamentoValue === null ? "Seleccione un departamento" : true
                            }}
                            control={control}
                            render={({ fieldState: { error } }) => (
                                <CHAutoCompleteV2
                                    style={colorLable}
                                    options={catalogState?.departments}
                                    getOptionLabel={option => option.nombre}
                                    value={departamentoValue}
                                    disabled
                                    defaultValue={catalogState?.departments.find(x => x.id === 0)}
                                    onChange={onChangeDepartment}
                                    renderInput={params => (
                                        <CHTextField
                                            className={classes.colorLable}
                                            {...params}
                                            label="Departamento"
                                            required
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
                            name="dcpId"
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
                                    disabled
                                    options={ciudadValues}
                                    getOptionLabel={option => option.nombre}
                                    onChange={onChangeCity}
                                    renderInput={params => (
                                        <CHTextField
                                            {...params}
                                            label="Ciudad"
                                            required

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
                                    disabled
                                    getOptionLabel={option => option.descripcionOficina}
                                    onChange={onChangeOficinaRegistro}
                                    renderInput={params => (
                                        <CHTextField
                                            {...params}
                                            label="Oficinas de Registro* "
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
                            name="maiNumeroMatricula"
                            rules={{
                                required: 'Este campo es requerido',
                                maxLength: {
                                    value: 21,
                                    message:
                                        'La longitud máxima del campo es de 21 caracteres.',
                                },
                                min: {
                                    value: 1,
                                    message: 'No se aceptan valores negativos.'
                                },
                                pattern: {
                                    value: /^[0-9]+$/,
                                    message: 'No se aceptan ese tipo de caracteres.',
                                }
                            }}

                            control={control}
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
                                    error={!!error}
                                    onChange={(onChange)}
                                    value={value}
                                    label="Número Matrícula"
                                    required
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
                            name="maiDireccion"
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            control={control}
                            defaultValue=""
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                                <CHTextField
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
                                    readOnly
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
                            name="prpNumeroIdentificacion"
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
                                    label="Número De Identificación"
                                    required
                                    onChange={(onChange)}
                                    value={value}
                                    isReadOnly
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
                                name="prpNombreRazonSocial"
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                control={control}
                                defaultValue=""
                                render={({ field: { onChange, value }, fieldState: { error } }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
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
                                        <CHTextField
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
                                    name="prpSegundoNombre"
                                    control={control}
                                    defaultValue=""
                                    render={({ field: { onChange, value }, fieldState: { error } }) => (
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
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
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
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
                                        <CHTextField
                                            InputLabelProps={{ shrink: true }}
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
                                    error={!!error}
                                    helperText={
                                        error
                                            ? 'Ingrese un correo electrónico válido'
                                            : null
                                    }
                                    label="Correo Electrónico"
                                    onChange={onChange}
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    required
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
                                    required
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid className="item-containerModal" md={5}>
                    <Grid className="subcontainerModal">
                        <>
                            <Button className={classes.button3} onClick={handleSubmit(updateBasicInformation)}>
                                ACTUALIZAR
                            </Button>
                        </>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    );
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    realStateRegistrationsState: state.realStateRegistrations,

});

const mapDispatchToProps = {
    startGetAllCities,
    startGetAllDepartments,
    startGetRegistrationOffice
}

export default connect(mapStateToProps, mapDispatchToProps)(basicInformation);
