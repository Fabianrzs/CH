import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Button, Grid, MenuItem } from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { useForm, Controller } from 'react-hook-form';
import moment from 'moment';
import { toast } from 'react-toastify';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHPaper from '../ch-paper/CHPaper';
import { startPostConsultCredit } from '../../actions/credit.actions';
import { AppState } from '../../store/rootReducer';
import { CreditConsultCreditModel } from '../../models/credit-consult-credit.model';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import { CreditConsultCreditRequestModel } from '../../models/requests/credit-consult-credit-request.model';
import CHTextField from '../ch-text-field/CHTextField';
import { CreditUpdateInsuranceRequestModel } from '../../models/requests/credit-update-insurance-request.model';
import AuthService from '../../services/auth.service';
import CreditApi from '../../services/api-credit.service';
import NotificationService from '../../services/notification.service';
import CHSelect from '../ch-select/CHSelect';
import { CATALOG } from '../../constants/constants';
import ApiCatalog from '../../services/api-catalog.service';
import { VALORCATALOGO } from '../../types/catalog.type';
import ConstruccionSobreLote from '../../pages/fondoSolidaridad/giroRecursos/ConstruccionSobreLote';
import afiliadoApi from '../../services/api-afiliado.service';


const creditApi = CreditApi();

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '1.5rem',
            marginTop: '.5rem',

        },
    }),
);

function RequestInformationInsuranceData({
    insurancesList,
    startPostConsultCredit,
    procedureRequest,
    creditConsultCreditData,
    isLoading,
}: {
    insurancesList: any[];
    startPostConsultCredit: (request: CreditConsultCreditRequestModel) => void;
    procedureRequest: ProcedureRequestModel;
    isLoading: boolean;
    creditConsultCreditData: CreditConsultCreditModel;

}) {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiEstrategico = afiliadoApi();


    const [isLoaded, setIsLoaded] = useState(false);
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [manejaPolizaVida, setManejaPolizaVida] = useState([]);
    const [manejaPolizaHogar, setManejaPolizaHogar] = useState([]);

    const { control, handleSubmit, setValue, watch, register, setError } = useForm({
        defaultValues: {
            percentageExtraBonus: 0,
            dealInsurance: '',
            dealInsuranceHome: '',
            insuranceID: '',
            homeInsuranceID: '',
            peso: 0,
            talla: 0,
            edad: 0,
            valorPrestamo: 0
        },
    });

    const hasDealInsurance = watch('dealInsurance') === 'N';
    const hasDealInsuranceHome = watch('dealInsuranceHome') === 'N';

    useEffect(() => {
        obtenerManejaPolizaVida();
        obtenerPesoYTalla();
        obtenerManejaPolizaHogar();
        obtenerDatosAliadosEstrategicos();
        const dataConsultCreditRequest: CreditConsultCreditRequestModel = {
            estado: '0',
            idSolicitudCredito: procedureRequest.requestID,
            identificacion: `${procedureRequest.clientIdentificationNumber}`,
        };
        startPostConsultCredit(dataConsultCreditRequest);

    }, []);



    const obtenerManejaPolizaVida = async () => {
        await apiCatalog
            .getAllCatalogoPorId(CATALOG.MANEJA_POLIZA_VIDA)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setManejaPolizaVida(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerDatosAliadosEstrategicos = async () => {
        const response = {
            soc_Id: procedureRequest?.requestID,
        }
        await apiEstrategico
            .postObtenerAliadosEstrategicos(response)
            .then(async (response: any) => {

                if (response) {
                    if (response.isSuccess) {
                        if (response.data === null) {
                            return
                        }

                        const aliados = response.data
                        setValue('peso', aliados.peso)
                        setValue('talla', aliados.talla)
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }
    const obtenerPesoYTalla = async () => {

        console.log({ procedureRequest })

        const request = procedureRequest?.requestID;
        await creditApi
            .postObtenerPesoYTalla(request)
            .then(async (response: any) => {

                if (response) {
                    if (response.isSuccess) {
                        if (response.data === null) {
                            return
                        }
                        const aliados = response.data
                        setValue('peso', aliados?.soC_PESO)
                        setValue('talla', aliados?.soC_TALLA)
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });

    }

    const obtenerManejaPolizaHogar = async () => {
        await apiCatalog
            .getAllCatalogoPorId(CATALOG.MANEJA_POLIZA_HOGAR)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setManejaPolizaHogar(response.data);
                    } else {
                        toast(`Error: ${response.returnMessage}`, {
                            type: toast.TYPE.ERROR,
                        });
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const onSubmit1 = async (data: any) => {

        if (data.peso === 0) {
            setError("peso", {
                type: "required",
                message: "Este Campo debe ser diferente a 0"
            });
            return;
        }
        if (data.talla === 0) {
            setError("talla", {
                type: "required",
                message: "Este Campo debe ser diferente a 0"
            });
            return;
        }

        const dataConsultCreditRequest: CreditConsultCreditRequestModel = {
            estado: '0',
            idSolicitudCredito: procedureRequest.requestID,
            identificacion: `${procedureRequest.clientIdentificationNumber}`,
        };
        setIsLocalLoading(true);
        const user = AuthService.getUserInfo();

        const request: CreditUpdateInsuranceRequestModel = {
            actualizadoPor: user.USR_ID,
            convenioAseguradora: data.dealInsurance,
            convenioAseguradoraHogar: data.dealInsuranceHome,
            fechaActualiza: moment().format(),
            idAseguradora: data.insuranceID.toString(),
            idAseguradoraHogar: data.homeInsuranceID.toString(),
            idSolicitudCredito: procedureRequest.requestID,
            porcentajeExtraprima: data.percentageExtraBonus,
            talla: data.talla,
            peso: data.peso,
            edad: data.edad,
        };

        try {
            await creditApi.postUpdateInsurance(request);
            setIsLocalLoading(false);
            startPostConsultCredit(dataConsultCreditRequest);

            await NotificationService.genericAlert(
                'Actualizado con éxito',
                'Se actualizó la solicitud.',
            );
        } catch (e) {
            await NotificationService.genericAlert(
                'Error en actualización',
                ` Ocurrio un error al intentar actualizar, intente de nuevo. ${e}`,
                'error',
            );

            setIsLocalLoading(false);

        }
    };


    console.log({ creditConsultCreditData })

    console.log({ insurancesList })
    if (creditConsultCreditData.requestCreditID != null && !isLoaded) {
        setIsLoaded(true);
        const fields = Object.keys(creditConsultCreditData);

        fields.forEach(field => {
            const value =
                creditConsultCreditData[
                field as keyof CreditConsultCreditModel
                ];

            const campo = field as any;

            if (value != null) {
                setValue(campo, value === '' ? '' : value);
            }

            if (value) {
                register(campo, {
                    required: 'Este campo es requerido.',
                });
            }
        });
    }


    if (isLoading || isLocalLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <>
            <CHPaper title="Simulación">
                <Grid container>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="dealInsurance"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    disabled={
                                        procedureRequest.stateActivityID === 4
                                    }
                                    label="Convenio Seguro Vida"
                                    value={value}
                                    onChange={value => {
                                        const valor = value.target.value;
                                        setValue('insuranceID', '');
                                        if (valor === 'S') {
                                            setValue(
                                                'insuranceID',
                                                '7')
                                            register('insuranceID', {
                                                required:
                                                    'Este campo es requerido.',
                                            });
                                        } else {
                                            setValue(
                                                'insuranceID',
                                                '')
                                            register('insuranceID', {
                                                required: {
                                                    value: false,
                                                    message: '',
                                                },
                                            });
                                        }
                                        onChange(value);
                                    }}
                                >
                                    {manejaPolizaVida?.map(
                                        (option: VALORCATALOGO) => (
                                            <MenuItem
                                                key={option.codigo}
                                                value={option.codigo}
                                            >
                                                {option.descripcion.toUpperCase()}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="insuranceID"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    disabled={
                                        procedureRequest.stateActivityID ===
                                        4 || !hasDealInsurance
                                    }
                                    label="Aseguradora Seguro Vida*"
                                    value={value}
                                    // readOnly
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                >
                                    <MenuItem value="">
                                        NO SELECCIONADO
                                    </MenuItem>
                                    {insurancesList.map(insurance => (
                                        <MenuItem
                                            key={`insurance-${insurance.id}`}
                                            value={insurance.id}
                                        >
                                            {insurance.razonSocial.toUpperCase()}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="dealInsuranceHome"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    disabled={
                                        procedureRequest.stateActivityID === 4
                                    }
                                    label="Convenio Seguro Hogar"
                                    value={value}
                                    onChange={value => {

                                        const valor = value.target.value;
                                        setValue('homeInsuranceID', '');
                                        if (valor === 'S') {
                                            setValue(
                                                'homeInsuranceID',
                                                '7',
                                            );
                                            register('homeInsuranceID', {
                                                required:
                                                    'Este campo es requerido.',
                                            });
                                        } else {
                                            setValue('homeInsuranceID', '')
                                            register('homeInsuranceID', {
                                                required: {
                                                    value: false,
                                                    message: '',
                                                },

                                            });
                                        }
                                        onChange(value);
                                    }}
                                >
                                    {manejaPolizaHogar?.map(
                                        (option: VALORCATALOGO) => (
                                            <MenuItem
                                                key={option.codigo}
                                                value={option.codigo}
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="homeInsuranceID"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    // readOnly
                                    helperText={error ? error.message : null}
                                    disabled={
                                        procedureRequest.stateActivityID ===
                                        4 || !hasDealInsuranceHome
                                    }
                                    label="Aseguradora Seguro Hogar *"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem value="">
                                        NO SELECCIONADO
                                    </MenuItem>
                                    {insurancesList.map(insurance => (
                                        <MenuItem
                                            key={`insurance-${insurance.id}`}
                                            value={insurance.id}
                                        >
                                            {insurance.razonSocial.toUpperCase()}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>



                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="peso"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                                pattern: {
                                    value: /^\d*\.?\d*$/,
                                    message:
                                        'Solo se aceptan caracteres numericos',
                                },
                                min: {
                                    value: 0,
                                    message:
                                        'El valor a ingresar es de 0 a 999',
                                },
                                max: {
                                    value: 999,
                                    message:
                                        'El valor no puede ser mayor a 999',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={
                                        procedureRequest.stateActivityID === 4
                                    }
                                    label="Peso (kg)*"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={e => {
                                        if (e.target.value.length <= 3) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[a-zA-ZñÑáéíóúÁÉÍÓÚ/@=+:{}#%$()*&¡!¿?[|`"~,<>^_-]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="talla"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                                pattern: {
                                    value: /^\d*\.?\d*$/,
                                    message:
                                        'Solo se aceptan caracteres numericos',
                                },
                                min: {
                                    value: 0,
                                    message:
                                        'El valor a ingresar es de 0 a 999',
                                },
                                max: {
                                    value: 999,
                                    message:
                                        'El valor no puede ser mayor a 999',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={
                                        procedureRequest.stateActivityID === 4
                                    }
                                    label="Estatura (cm)*"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={e => {
                                        if (e.target.value.length <= 3) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[a-zA-ZñÑáéíóúÁÉÍÓÚ/@=+:{}#%$()*&¡!¿?[|`"~,<>^_-]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="edad"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                                pattern: {
                                    value: /^\d*\.?\d*$/,
                                    message:
                                        'Solo se aceptan caracteres numericos',
                                },
                                min: {
                                    value: 0,
                                    message:
                                        'El valor a ingresar es de 0 a 999',
                                },
                                max: {
                                    value: 999,
                                    message:
                                        'El valor no puede ser mayor a 999',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}

                                    label="Edad (año)*"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={e => {
                                        if (e.target.value.length <= 3) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[a-zA-ZñÑáéíóúÁÉÍÓÚ/@=+:{}#%$()*&¡!¿?[|`"~,<>^_-]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    <Grid item md={4} className={classes.column}>
                        <Controller
                            name="valorPrestamo"
                            control={control}
                            render={({ field: { value } }) => (
                                <CHTextField
                                    currencyMask
                                    isReadOnly
                                    label="Valor Crédito"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    {procedureRequest.stateActivityID !== 4 && (
                        <Grid item md={12} xs={12} >
                            <Button
                                className={classes.buttonAdd}
                                color="primary"
                                variant="outlined"
                                onClick={handleSubmit(onSubmit1)}
                            // variant="contained"
                            >
                                Guardar
                            </Button>
                        </Grid>
                    )}
                </Grid>
            </CHPaper>
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    creditConsultCreditData: state.credit.creditConsultCreditData,
    insurancesList: state.credit.insurancesList,
    // isLoading: state.credit.loading,
});

const mapDispatchToProps = {
    startPostConsultCredit,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RequestInformationInsuranceData);
