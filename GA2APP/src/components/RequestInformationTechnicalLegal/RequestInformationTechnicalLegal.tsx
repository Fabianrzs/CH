import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    IconButton,
    MenuItem,
    Table,
    TableBody,
    TableCell,
    TableRow,
} from '@material-ui/core';
import swal from 'sweetalert';
import { useForm, Controller } from 'react-hook-form';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { Color } from '@material-ui/lab';
import { connect } from 'react-redux';
import moment from 'moment';
import { toast } from 'react-toastify';
import { Delete, Edit } from '@material-ui/icons';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import CHSelect from '../ch-select/CHSelect';
import CHSnackBar from '../ch-snack-bar/CHSnackBar';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import { AppState } from '../../store/rootReducer';
import {
    startGetAllCities,
    startGetAllDepartments,
} from '../../actions/catalog.action';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import {
    startPostCreditCreateLegalData,
    startPostCreditLegalData,
    startPostCreditProductData,
} from '../../actions/credit.actions';
import { CreditLegalDataRequestModel } from '../../models/requests/credit-legal-data-request.model';
import AuthService from '../../services/auth.service';
import { CreditLegalDataModel } from '../../models/credit-legal-data.model';
import CHDatePicker from '../ch-datepicker/CHDatepicker';
import NotificationService from '../../services/notification.service';
import { CreditLegalCreateRequestModel } from '../../models/requests/credit-legal-create-request.model';
import UtilService from '../../utils/util.service';
import { CreditProductRequestModel, Matricula } from '../../models/requests/credit-product-request.model';
import { CreditProductModel } from '../../models/credit-product.model';
import ApiCatalog from '../../services/api-catalog.service';
import ApiCredit from '../../services/api-credit.service';
import { CATALOG } from '../../constants/constants';
import { CITYDTO, DEPARTAMENTODTO, VALORCATALOGO } from '../../types/catalog.type';
import { ModalEditarMatricula } from './ModalInformationTechnicalLegal/ModalEditarMatricula';
import CHAutoComplete from '../ch-auto-complete/CHAutoComplete';
import CHAutoCompleteV2 from '../ch-auto-complete/CHAutoCompleteV2';

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

type Inputs = {
    dateFirstAct: Date;
    propertyState: string;
    propertyCity: string;
    boundaries: string;
    parkingType: string;
    legalAge: string;
    propertyTradition: string;
    analysisUlt20Years: string;
    analysisSitLegal: string;
    analysisRegHorizontalProperty: string;
    analysisPeaceAndSave: string;
    analysisSealer: string;
    legalBusinessViability: string;
    recommendations: string;
    legalConceptEnd: string;
    legalConceptEst: string;
    propertyAddress: string;
    propertyStateID: number;
    propertyCityID: number;
    escritura?:string;
    fechaEscritura?:Date;
    notaria?:number;
    departamentoNotaria?:number;
    ciudadNotaria?:number;
    descripcionInmueble?:string;
    folioMatriculas?:number;
    oficinaInstrumentosPublicos?:string;
    
};

function RequestInformationTechnicalLegal({
    procedureRequest,
    isLoadingCatalog,
    isLoadingCredit,
    startGetAllDepartments,
    startGetAllCities,
    creditLegalData,
    startPostCreditCreateLegalData,
    startPostCreditLegalData,
    isCreatedSuccessfully,
    startPostCreditProductData,
    creditProductData,
    isLoading,
    cities,
}: {
    procedureRequest: ProcedureRequestModel;
    isLoadingCatalog: boolean;
    isLoadingCredit: boolean;
    isCreatedSuccessfully: boolean;
    creditLegalData: any;
    startGetAllDepartments: () => void;
    startGetAllCities: (stateID?: string) => void;
    startPostCreditLegalData: (data: CreditLegalDataRequestModel) => void;
    startPostCreditCreateLegalData: (
        data: CreditLegalCreateRequestModel,
    ) => void;
    startPostCreditProductData: (data: CreditProductRequestModel) => void;
    creditProductData: CreditProductModel;
    isLoading:boolean;
    cities:any
}) {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiCredit = ApiCredit();
    const isFinalizedActivity = procedureRequest.stateActivityID === 4;
    const [indexes, setIndexes] = useState<any[]>([]);
    const [message, setMessage] = useState<string>('');
    const [type, setType] = useState<Color>('success');
    const [isSubmited, setIsSubmited] = useState(false);
    const [conceptoAvaluo, setConceptoAvaluo] = useState([]);
    const [openModalEditar, setOpenModalEditar] = useState(false);
    const [tipoMatriculas, setTipoMatriculas] = useState<any[]>([]);
    const [departamentos, setDepartamentos] = useState<any[]>([]);
    const [departamentoValue, setDepartamentoValue] = React.useState<DEPARTAMENTODTO | null>(null);
    const [ciudadValue, setCiudadValue] = React.useState<CITYDTO | null>(null);
	const [ciudadValues, setCiudadValues] = React.useState<CITYDTO[]>([]);


    const handleClose = (event?: React.SyntheticEvent, reason?: string) => {
        if (reason === 'clickaway') {
            return;
        }
        setMessage('');
        setType('success');
    };

    const addRealStateRow = (index: any) => {
        setIndexes(prevIndexes => [...prevIndexes, index]);
    };

 

    const { handleSubmit, control, watch, setValue } = useForm<Inputs>({
        defaultValues: {
            dateFirstAct: new Date(),
            propertyState: '',
            propertyCity: '',
            boundaries: '',
            parkingType: '',
            legalAge: '',
            propertyTradition: '',
            analysisUlt20Years: '',
            analysisSitLegal: '',
            analysisRegHorizontalProperty: '',
            analysisPeaceAndSave: '',
            analysisSealer: '',
            legalBusinessViability: '',
            recommendations: '',
            legalConceptEnd: '',
            legalConceptEst: '',
            propertyAddress: '',
            propertyStateID: 0,
            propertyCityID: 0,
            escritura: '',
            fechaEscritura: new Date(),
            notaria:0,
            departamentoNotaria:0,
            ciudadNotaria:0,
            descripcionInmueble:'',
            folioMatriculas:0,
            oficinaInstrumentosPublicos:''
        },
    });
    const {
        handleSubmit: handleSubmit2,
        control: control2,
        reset: reset2,
        formState: { isSubmitSuccessful },
    } = useForm({
        defaultValues: {
            realStateRegistrationNumber: '',
            cadastralID: '',
            privateArea: '',
            commercialValue: null,
            cadastralValue: null,
        },
    });

    const formularioMatriculas = useForm({
        defaultValues: {
          idMatricula: '',
          numeroMatricula: '',
          fechaMatricula: new Date (),
          chip: '',
          cedulaCatastral: '',
          areaPrivadaConstruida: 0,
          areaAvaluoComercial: 0,
          areaAvaluoCatastral: 0,
          socId: '',
          tipo_Matricula: 0,
        },
    });

    useEffect(() => {
        obtenerCatalogoTipoMatricula();
    }, []);
    useEffect(() => {
        apiCatalog.getAllCities()
        .then(async response => {
            if (response) {
                if (response.isSuccess) {      
                    obtenerDepatartamentos(response.data);
                }
            }
        })
        .catch(() => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        }); 
    }, [creditLegalData.requestInformationLegalID != null]);

    useEffect(() => {
        if (isSubmitSuccessful) {
            reset2();
        }
    }, [isSubmitSuccessful]);

    useEffect(() => {
        obtenerMatriculasCreditoPorSolicitud(procedureRequest.requestID);
    }, [procedureRequest]);

    const obtenerDepatartamentos = async (dataCiudades:any) => {
		await apiCatalog.getAllStates()
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						setDepartamentos(response?.data?.filter((item: DEPARTAMENTODTO) => item.id !== 0));
                        if (creditLegalData.requestInformationLegalID != null ) {  
                            const deNota = response?.data.filter((depar: any) => depar.id === creditLegalData.departamentoNotaria)
                            setValue('departamentoNotaria', deNota[0]?.id)  
                            setDepartamentoValue(deNota[0])
                            if(dataCiudades.length !== 0){
                                const ciNota = dataCiudades.filter((ciu: any) => ciu.id === creditLegalData.ciudadNotaria && ciu.idDepartamento === creditLegalData.departamentoNotaria)
                                setCiudadValue(ciNota[0])
                                setValue('ciudadNotaria', ciNota[0]?.id)
                            }else{
                                setCiudadValue(creditLegalData.departamentoNotaria)
                                setValue('ciudadNotaria', creditLegalData.departamentoNotaria)
                            }
                       
                            const fields = Object.keys(creditLegalData);
                            fields.forEach(field =>
                                setValue(
                                    field as any,
                                    creditLegalData[field as keyof CreditLegalDataModel],
                                ),
                            );

                            const descripcionActual = creditLegalData.descripcionInmueble?.trim();
                            if (!descripcionActual) {
                                if (procedureRequest.creditType === "CRÉDITO HIPOTECARIO") {
                                    setValue("descripcionInmueble", "NO APLICA");
                                } else {
                                    setValue("descripcionInmueble", "SALACOMEDOR - ALCOBAS - BAÑOS - COCINA - PATIO DE ROPAS - PISO EN CERÁMICA - VENTANAS EN ALUMINIO - PUERTA DE ACCESO EN MADERA - COCINA INTEGRAL");
                                }
                            }
                        }
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};

    const onSubmitRow = (data: any) => {
        const err: string[] = [];
        if (UtilService.cleanCurrencyValue(data.commercialValue) === 0) {
            setMessage('El valor avalúo comercial debe ser mayor a cero');
            setType('error');
            return;
        }

        if (UtilService.cleanCurrencyValue(data.cadastralValue) === 0) {
            setMessage('El valor avalúo catastral debe ser mayor a cero');
            setType('error');
            return;
        }
        const dataMatricula = {
            realStateRegistrationNumber: data.realStateRegistrationNumber,
            cadastralID: data.cadastralID,
            privateArea: data.privateArea,
            commercialValue: UtilService.cleanCurrencyValue(
                data.commercialValue,
            ),
            cadastralValue: UtilService.cleanCurrencyValue(data.cadastralValue),
        };

        indexes.forEach(index => {
            if (index.cadastralID === dataMatricula.cadastralID) {
                err.push('Cédula Catastral no se puede repetir');
            }
            if (
                index.realStateRegistrationNumber ===
                dataMatricula.realStateRegistrationNumber
            ) {
                err.push('No. Matrícula no se puede repetir');
            }
        });

        if (err.length > 0) {
            setMessage(err.join(', '));
            setType('error');
        } else if (indexes.length > 2) {
            setMessage('No se pueden agregar más de 3 matriculas');
            setType('error');
        } else {
            addRealStateRow(dataMatricula);
        }
    };

    const obtenerCatalogoTipoMatricula = async () => {
        await apiCatalog
            .getObtenerValoresCatalogosPorIdCatalogo(90)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setTipoMatriculas(response.data);
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

    const obtenerValorTipoMatricula = (tipo_Matricula: number | undefined) => {
        const catalogoMAtricula = tipoMatriculas.find(x => x.idValor === tipo_Matricula)?.descripcion
        return catalogoMAtricula
     };
  
    const handleOpenModalEditar = (item: any) => {
        formularioMatriculas.setValue('idMatricula', item.idMatricula);
        formularioMatriculas.setValue('numeroMatricula', item.numeroMatricula);
        formularioMatriculas.setValue('fechaMatricula', item.fechaMatricula);
        formularioMatriculas.setValue('cedulaCatastral', item.cedulaCatastral);
        formularioMatriculas.setValue('chip', item.chip);
        formularioMatriculas.setValue('areaPrivadaConstruida', item.areaPrivadaConstruida);
        formularioMatriculas.setValue('areaAvaluoComercial', item.areaAvaluoComercial);
        formularioMatriculas.setValue('areaAvaluoCatastral', item.areaAvaluoCatastral);
        formularioMatriculas.setValue('socId', item.socId);
        formularioMatriculas.setValue('tipo_Matricula', item.tipo_Matricula);
        setOpenModalEditar(true);
    }

    const handleCloseModalEditar = () => {
        setOpenModalEditar(false);
    }

    const obtenerMatriculasCreditoPorSolicitud = async(socId: any) => {
        await apiCredit
            .postObtenerMatriculasCreditoPorSolicitud(
             socId
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response.data !== null) {
                        setIndexes(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleOnClickEditarMatricula = (data: any) => {
        const matricula: Matricula = {
            idMatricula: data.idMatricula,
            numeroMatricula: data.numeroMatricula,
            fechaMatricula: data.fechaMatricula,
            chip: data.chip,
            cedulaCatastral: data.cedulaCatastral,
            areaPrivadaConstruida: data.areaPrivadaConstruida,
            areaAvaluoComercial: UtilService.cleanCurrencyValue(data.areaAvaluoComercial),
            areaAvaluoCatastral: UtilService.cleanCurrencyValue(data.areaAvaluoCatastral),
            socId: data.socId,
            tipo_Matricula: data.tipo_Matricula,
        }
        ActualizarMatricula(matricula);
        handleCloseModalEditar();
    }

    const ActualizarMatricula = async(matricula: Matricula) => {
        await apiCredit
            .updateMatriculaCredito(
                matricula
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        formularioMatriculas.reset();
                        await NotificationService.genericAlert(
                            'Actualizado',
                            'Registro actualizado con éxito',
                        );
                        obtenerMatriculasCreditoPorSolicitud(procedureRequest.requestID);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const handleDeleteMatricula = (item: any) => {
        swal({
            text: '¿Está seguro que desea eliminar la matricula?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(respuesta => {
            if (!respuesta) {
                eliminarMatricula(item);
            }
        });
    }

    const eliminarMatricula = async(idMatricula: any) => {
        await apiCredit
            .deleteMatriculaCredito(
                idMatricula
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        await NotificationService.genericAlert(
                            'Eliminado',
                            'Registro eliminado con éxito',
                        );
                        obtenerMatriculasCreditoPorSolicitud(procedureRequest.requestID);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const onSubmit = (data: CreditLegalDataModel) => {
        
        if (indexes.length === 0) {
            handleSubmit2(onSubmitRow)();
            setType('error');
            setMessage('Se debe agregar al menos una información catastral.');
        } else {
            setIsSubmited(true);
            const user = AuthService.getUserInfo();
            const dataRequest: CreditLegalCreateRequestModel = {
                idSolicitudInformacionJuridica:
                creditLegalData.requestInformationLegalID,
                analisisPazySalvo: data.analysisPeaceAndSave,
                analisisRegPropHorizontal: data.analysisRegHorizontalProperty,
                analisisSitJuridica: data.analysisSitLegal,
                analisisUlt20Años: data.analysisUlt20Years,
                analisisVendedor: data.analysisSealer,
                conceptoEstJuridicio: data.legalConceptEst,
                conceptoJuridicoFin: data.legalConceptEnd,
                creadoPor: user.USR_ID,
                edadJuridica: data.legalAge,
                fechaCreacion: moment().format(),
                fechaPrimerActo: moment(data.dateFirstAct).format(),
                idSolicitudCredito: procedureRequest.requestID,
                linderos: data.boundaries,
                direccion: data.propertyAddress,
                codigoCiudad: data.propertyCityID,
                codigoDepartamento: data.propertyStateID,
                recomendaciones: data.recommendations,
                escritura: data.escritura,
                descripcionInmueble:data.descripcionInmueble,
                oficinaInstrumentosPublicos:data.oficinaInstrumentosPublicos,
                folioMatriculas:data.folioMatriculas,
                fechaEscritura: data.fechaEscritura,
                notaria:data.notaria,
                departamentoNotaria: data?.departamentoNotaria,
			    ciudadNotaria: data?.ciudadNotaria,
                solicitudesCreditoInfoJurInmueble: indexes.map(val => ({
                    idSolicitudCredito: procedureRequest.requestID,
                    idSolicitudInformacionJuridica:
                        creditLegalData.requestInformationLegalID,
                    numeroMatricula: val.numeroMatricula,
                    cedulaCatastral: val.cedulaCatastral,
                    areaPrivada: val.areaPrivadaConstruida,
                    avaluoComercial: UtilService.cleanCurrencyValue(
                        val.areaAvaluoComercial,
                    ),
                    avaluoCatastral: UtilService.cleanCurrencyValue(
                        val.areaAvaluoCatastral,
                    ),
                    creadoPor: user.USR_ID,
                    fechaCreacion: moment().format(),
                })),
                tipoParqueadero: data.parkingType,
                tradicionInmueble: data.propertyTradition,
                viabilidadJurNegocio: data.legalBusinessViability,
                tipo_Matricula: data.tipo_Matricula,
            };
            startPostCreditCreateLegalData(dataRequest);
        }
    };

    useEffect(() => {
        startGetAllDepartments();
        obtenerConceptoAvaluo();
        const dataRequest: CreditLegalDataRequestModel = {
            idSolicitudCredito: procedureRequest.requestID,
            indicador: 2,
        };
        startPostCreditLegalData(dataRequest);
    }, []);

    const obtenerConceptoAvaluo = async () => {
        await apiCatalog
            .getAllCatalogoPorId(CATALOG.CONCEPTO_AVALUO)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setConceptoAvaluo(response.data);

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

    const stateID = watch('propertyStateID');
    useEffect(() => {
        
        if (stateID !== 0) {
            startGetAllCities(stateID.toString());
        } else {
            setValue('propertyCityID', 0);
        }
    }, [stateID ]);

    useEffect(() => {
        const dataRequest: CreditProductRequestModel = {
            ind: 2,
            soC_ID: procedureRequest.requestID,
        };
        startPostCreditProductData(dataRequest);
    }, []);

    if (isLoadingCredit || isLoadingCatalog || isLoading) {
        return <CHLoadingProcess />;
    }


    if (isSubmited && isCreatedSuccessfully) {
        NotificationService.genericAlert('Guardado con éxito', '');
        setIsSubmited(false);
    }

    const obtenerCiudades = async (idDepartamento: string) => {
		await apiCatalog.getCityOfState(idDepartamento)
			.then(async response => {
				if (response) {
					if (response.isSuccess) {
						setCiudadValues(response.data);
					}
				}
			})
			.catch(() => {
				toast('Error del sistema', { type: toast.TYPE.ERROR });
			});
	};

    const onChangeDepartment = (event: any, newValue: DEPARTAMENTODTO) => {
		if (newValue !== null) {
			setDepartamentoValue(newValue)
			setCiudadValue(null)
			obtenerCiudades(String(newValue?.id))
			setValue('departamentoNotaria', newValue?.id)
			setValue('ciudadNotaria', 0)
		}
	};

    const onChangeCity = (event: any, newValue: CITYDTO | null) => {
		setCiudadValue(newValue);
		setValue('ciudadNotaria', newValue?.id as number)
	};


    return (
        <>
            <CHPaper title="Datos Estudio Legal">
                <Table>
                    <TableBody>
                            <TableRow>
                                <TableCell>No. Matricula</TableCell>
                                <TableCell>No. CHIP</TableCell>
                                <TableCell>Cédula Catastral</TableCell>
                                <TableCell>Tipo Matrícula</TableCell>
                                <TableCell>Área Privada Construida</TableCell>
                                <TableCell>Valor Avalúo Comercial</TableCell>
                                <TableCell>Valor Avalúo Catastral</TableCell>
                                {!isFinalizedActivity && ( <TableCell align='left'>Acciones</TableCell>)}
                            </TableRow>
                        {indexes.map((index, key) => (
                            <TableRow
                                key={`realState-${key}`}
                                style={{ textTransform: 'uppercase' }}
                            >
                                <TableCell>{index.numeroMatricula}</TableCell>
                                <TableCell>{index.chip}</TableCell>
                                <TableCell>{index.cedulaCatastral}</TableCell>
                                <TableCell>{obtenerValorTipoMatricula(index.tipo_Matricula)}</TableCell>
                                <TableCell>{index.areaPrivadaConstruida}</TableCell>
                                <TableCell style={{ textAlign: 'right' }}>
                                    {UtilService.formatCurrency(
                                        index.areaAvaluoComercial,
                                    )}
                                </TableCell>
                                <TableCell style={{ textAlign: 'right' }}>
                                    {UtilService.formatCurrency(
                                        index.areaAvaluoCatastral,
                                    )}
                                </TableCell>
                                {!isFinalizedActivity && (
                                <TableCell>
                                     <IconButton
                                         aria-label="Editar"
                                         color="primary"
                                         size="small"
                                         onClick={() => {
                                             handleOpenModalEditar(index);
                                         }}
                                     >
                                         <Edit />
                                     </IconButton>
                                     <IconButton
                                         aria-label="Eliminar"
                                         color="primary"
                                         size="small"
                                         onClick={() => {
                                             handleDeleteMatricula(
                                                 index.idMatricula
                                             );
                                         }}
                                     >
                                         <Delete />
                                     </IconButton>
                                 </TableCell> )}
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
                <br/>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            label="Dirección del Inmueble"
                            value={creditProductData.sopSellerAddress || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            label="Departamento del Inmueble"
                            value={creditProductData.propertyState || ''}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <CHTextField
                            isReadOnly
                            label="Ciudad del Inmueble"
                            value={creditProductData.propertyCity || ''}
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="boundaries"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Linderos de la vivienda *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="legalAge"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Edad Jurídica Aproximada *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="dateFirstAct"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    readOnly={isFinalizedActivity}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha primer acto traslaticio del dominio *"
                                    value={value}
                                    onChange={onChange}
                                    InputProps={{
                                    readOnly: isFinalizedActivity,
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            name="propertyTradition"
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Tradición del inmueble *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="analysisUlt20Years"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Análisis de los últimos 10 años de la tradición *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="analysisSitLegal"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Análisis situación jurídica actual *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="analysisRegHorizontalProperty"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Análisis del reglamento de propiedad horizontal *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="analysisPeaceAndSave"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Análisis de paz y salvos *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="analysisSealer"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Análisis del vendedor *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="legalBusinessViability"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Viabilidad jurídica del negocio *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="recommendations"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={isFinalizedActivity}
                                    label="Recomendaciones"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="legalConceptEnd"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Justificación del Concepto Estudio de Títulos *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="legalConceptEst"
                            control={control}
                            defaultValue=""
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={isFinalizedActivity}
                                    error={!!error}
                                    label="Concepto Estudio de Titulos"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {conceptoAvaluo.map(
                                        (item: VALORCATALOGO) => (
                                            <MenuItem
                                                key={item.codigo}
                                                value={item.codigo}
                                            >
                                                {`${item.descripcion} `}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="escritura"
                            control={control}
                       
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Escritura *"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={6} className={classes.column}>
                        <Controller
                            name="fechaEscritura"
                            control={control}
                          
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    readOnly={isFinalizedActivity}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Fecha Escritura"
                                    onChange={onChange}
                                    value={value}
                                    disableFuture
                                    InputLabelProps={{ shrink: true }}
                            />
                            )}
                        />
                    </Grid>

                    <Grid item xs={12} md={6} className={classes.column}>
                        <Controller
                            name="notaria"
                            control={control}
                          
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={isFinalizedActivity}
                                    error={!!error}
                                    label="Notaria"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={(e) => {
                                        const inputValue = e.target.value;
                                        if (/^\d{0,3}$/.test(inputValue)) {
                                            onChange(inputValue);
                                        }
                                    }}   
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item xs={12} md={6} className={classes.column}>
                 
                            <Controller
                                name="departamentoNotaria"
                                rules={{
                                    validate: () => departamentoValue === null ? "Seleccione un departamento" : true
                                }}
                                control={control}
                                render={({ fieldState: { error } }) => (
                                    
                                    <CHAutoComplete
                                        value={departamentoValue}
                                        options={departamentos}
                                        getOptionLabel={option => option.nombre}
                                        disabled={isFinalizedActivity}
                                        onChange={onChangeDepartment}
                                        renderInput={params => (
                                            <CHTextField
                                                {...params}
                                                InputLabelProps={{ shrink: true }}
                                                label="Departamento Notaria*"
                                                error={!!error}
                                                isReadOnly={isFinalizedActivity}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                            />
                                        )}
                                    />
                                )}
                            />
                      
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
						<Controller
							name="ciudadNotaria"
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
                                    disabled={isFinalizedActivity}
									getOptionLabel={(option: any) => option.nombre}
									onChange={onChangeCity}
									renderInput={params => (
										<CHTextField
											{...params}
											InputLabelProps={{ shrink: true }}
											label="Ciudad Notaria *"
                                            isReadOnly={isFinalizedActivity}
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

                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            name="folioMatriculas"
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Folio Matriculas*"
                                    value={value}
                                    onChange={(e) => {
                                        const { value } = e.target;
                                        onChange(Number(value));
                                    }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            name="oficinaInstrumentosPublicos"
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Oficina de Instrumentos Públicos*"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            name="descripcionInmueble"
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    label="Descripción Inmueble*"
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    
                </Grid>
                {!isFinalizedActivity && (
                    <Button
                    className={classes.buttonAdd}
                    color="primary"
                    variant="outlined"

                    onClick={handleSubmit(onSubmit)}>Guardar</Button>
                )}
            </CHPaper>
            <CHSnackBar
                message={message}
                type={type}
                isOpen={message !== ''}
                handleClose={handleClose}
            />
            <ModalEditarMatricula
                open={openModalEditar}
                handleClose={handleCloseModalEditar}
                formulario={formularioMatriculas}
                handleOnClick={handleOnClickEditarMatricula}
            />
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    isLoadingCatalog: state.catalog.loading,
    isLoadingCredit: state.credit.loading,
    states: state.catalog.departments,
    cities: state.catalog.City,
    creditLegalData: state.credit.creditLegalData,
    isCreatedSuccessfully: state.credit.createIsSuccessfully,
    creditProductData: state.credit.creditProductData,
});

const mapDispatchToProps = {
    startGetAllDepartments,
    startGetAllCities,
    startPostCreditLegalData,
    startPostCreditCreateLegalData,
    startPostCreditProductData,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RequestInformationTechnicalLegal);
