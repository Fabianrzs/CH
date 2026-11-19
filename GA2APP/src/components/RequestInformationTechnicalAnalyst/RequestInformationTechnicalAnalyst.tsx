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
    TextareaAutosize,
    Typography,
} from '@material-ui/core';
import swal from 'sweetalert';
import { useForm, Controller } from 'react-hook-form';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { Color } from '@material-ui/lab';
import moment from 'moment';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { Delete, Edit } from '@material-ui/icons';
import CHPaper from '../ch-paper/CHPaper';
import CHTextField from '../ch-text-field/CHTextField';
import CHSelect from '../ch-select/CHSelect';
import CHSnackBar from '../ch-snack-bar/CHSnackBar';
import { CreditTechnicalCreateRequestModel } from '../../models/requests/credit-technical-create-request.model';
import { ProcedureRequestModel } from '../../models/procedure-request.model';
import CHLoadingProcess from '../ch-loading-process/CHLoadingProcess';
import AuthService from '../../services/auth.service';
import { AppState } from '../../store/rootReducer';
import {
    startPostCreditCreateTechnicalData,
    startPostCreditProductData,
    startPostCreditTechnicalData,
} from '../../actions/credit.actions';
import { CreditTechnicalDataRequestModel } from '../../models/requests/credit-technical-data-request.model';
import { CreditTechnicalDataModel } from '../../models/credit-technical-data.model';
import UtilService from '../../utils/util.service';
import NotificationService from '../../services/notification.service';
import { CreditProductRequestModel, Matricula} from '../../models/requests/credit-product-request.model';
import { VALORCATALOGO } from '../../types/catalog.type';
import ApiCatalog from '../../services/api-catalog.service';
import ApiCredit from '../../services/api-credit.service';
import { CATALOG } from '../../constants/constants';
import './RequestInformationTechnicalAnalyst.styles.scss';
import { ModalEditarMatricula } from './ModalInformationTechnicalAnalyst/ModalEditarMatricula';
import { ModalAgregarMatricula } from './ModalInformationTechnicalAnalyst/ModalAgregarMatricula';
import GenerarGuid from '../../pages/solicitudes/components/funciones/guid.funtions';



const useStyles = makeStyles((theme) => ({
    column: {
        padding: '10px',
        textAlign: 'center',
    },
    textarea: {
      width: '100%',
      minHeight: '100px',
      padding: '10px',
      fontSize: '20px',
      border: '1px solid #ccc',
      borderRadius: '4px',
      resize: 'vertical',
      '&:focus': {
        outline: 'visible',
       
        borderColor: "fec917",
      },
    },

    errorTextarea: {
    
    },
    title: {
        marginRight: "30px",
        fontSize: "15px",
        color: "fec917",
        textAlign: "center",
        marginTop: "20px",
    },

  }));
 

function RequestInformationTechnicalAnalyst({

    startPostCreditCreateTechnicalData,
    startPostCreditTechnicalData,
    procedureRequest,
    creditTechnicalData,
    isCreatedSuccessfully,
    creditProductData,
    startPostCreditProductData,
    isLoading,
    
}: {
   
    startPostCreditCreateTechnicalData: (
        data: CreditTechnicalCreateRequestModel,
    ) => void;
    startPostCreditTechnicalData: (data: any) => void;
    procedureRequest: ProcedureRequestModel;
    creditTechnicalData: CreditTechnicalDataModel;
    isCreatedSuccessfully: boolean;
    creditProductData: any;
    startPostCreditProductData: (data: CreditProductRequestModel) => void;
    isLoading:boolean;


    
}) {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiCredit = ApiCredit();
    const [indexes, setIndexes] = useState<any[]>([]);
    const [message, setMessage] = useState<string>('');
    const [type, setType] = useState<Color>('success');
    const [isLoaded, setIsLoaded] = useState(false);
    const [isSubmitted, setIsSubmitted] = useState(false);
    const [conceptoAvaluo, setConceptoAvaluo] = useState([]);
    const [estratos, setEstratos] = useState<any[]>([]);
    const [datosMatriculas, setDatosMatriculas] = useState<any>({});
    const [ocultarFormulario, setOcultarFormulario] = useState(false);
    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [openModalEditar, setOpenModalEditar] = useState(false);
    const [consultaTransUnion, setConsultaTransUnion] = useState<any>({});
    const [tipoMatriculas, setTipoMatriculas] = useState<any[]>([]);

    const { handleSubmit, control, setValue, getValues } = useForm<any>({
        defaultValues: {
            complianceDeclaration: '',
            commercialValue: 0,
            catastralValue: '',
            // propertySaleValue: '',
            finalConsiderations: '',
            conditionsSavings: '',
            conceptValue: '',
            stratum: '',
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
            realStateRegistrationDate: null,
            propertyChip: '',
            cadastralID: '',
        },
    });

    const formularioMatriculas = useForm({
        defaultValues: {
          idMatricula: '',
          numeroMatricula: '',
          fechaMatricula: new Date(),
          chip: '',
          cedulaCatastral: '',
          socId: '',
          tipo_Matricula: 0,
      
        },
    });

    useEffect(() => {
        obtenerConceptoAvaluo();
        consultarEstratos();
        obtenerCatalogoTipoMatricula();
      
        if (isSubmitSuccessful) {
            reset2();
        }
    }, [isSubmitSuccessful]);

    useEffect(() => {
  
        const dataRequest: CreditTechnicalDataRequestModel = {
            indicador: 2,
            solicitudCredito: procedureRequest.requestID,
        };
        startPostCreditTechnicalData(dataRequest);
      
     
    }, []);

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

  



    useEffect(() => {
        obtenerMatriculasCreditoPorSolicitud(procedureRequest.requestID);
    }, [procedureRequest]);

    useEffect(() => {
       
 
        setValue('catastralValue', creditTechnicalData?.catastralValue === undefined ? 
            '' :UtilService.formatCurrency(Number(creditTechnicalData?.catastralValue  )));

        setValue('commercialValue', creditTechnicalData?.commercialValue === undefined ? 
            '' :UtilService.formatCurrency(Number(creditTechnicalData?.commercialValue  )));
        // setValue('propertySaleValue', creditTechnicalData?.propertySaleValue === undefined ? 
        //     '' :UtilService.formatCurrency(Number(creditTechnicalData?.propertySaleValue  )));
        setValue('stratum', creditTechnicalData.stratum);
        setValue('complianceDeclaration',creditTechnicalData.complianceDeclaration);
        setValue('conceptValue', creditTechnicalData.conceptValue);
        setValue('conditionsSavings', creditTechnicalData.conditionsSavings,);
        setValue('finalConsiderations',creditTechnicalData.finalConsiderations,);
        setValue('stratum', creditTechnicalData.stratum);
    }, [creditProductData, creditTechnicalData]);




    const handleOpenModalAgregar = () => {
        if(indexes.length === 3){
            setMessage('No se pueden agregar más de 3 matriculas');
            setType('error');
        }else{
            setOpenModalAgregar(true);
            formularioMatriculas.reset();
        }
    }

    const handleCloseModalAgregar = () => setOpenModalAgregar(false);

    const handleOpenModalEditar = (item: any) => {
      
        formularioMatriculas.setValue('idMatricula', item.idMatricula);
        formularioMatriculas.setValue('numeroMatricula', item.numeroMatricula);
        formularioMatriculas.setValue('chip', item.chip);
        formularioMatriculas.setValue('cedulaCatastral', item.cedulaCatastral);
        formularioMatriculas.setValue('tipo_Matricula', item.tipo_Matricula);

        setOpenModalEditar(true);
    }

    const handleCloseModalEditar = () => setOpenModalEditar(false);

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

    const handleOnClickAgregarMatricula = (data: any) => {
        const guid = GenerarGuid();
        const matricula: Matricula = {
            idMatricula: guid.newGuid(),
            numeroMatricula: data.numeroMatricula,
            fechaMatricula: data.fechaMatricula,
            chip: data.chip,
            cedulaCatastral: data.cedulaCatastral,
            areaPrivadaConstruida: 0,
            areaAvaluoComercial: 0,
            areaAvaluoCatastral: 0,
            socId: procedureRequest.requestID,  
            tipo_Matricula: data.tipo_Matricula,    
        }
        agregarMatricula(matricula);
        handleCloseModalAgregar();
    }

    const agregarMatricula = async(matricula: Matricula) => {
        await apiCredit
            .postCrearMatriculaCredito(
                matricula
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        await NotificationService.genericAlert(
                            'Guardado',
                            'Registro guardado con éxito',
                        );
                        obtenerMatriculasCreditoPorSolicitud(procedureRequest.requestID);
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
            socId: procedureRequest.requestID,  
            tipo_Matricula: data.tipo_Matricula,

        }
        
        ActualizarMatricula(matricula);
        handleCloseModalEditar();
        formularioMatriculas.reset();
    }

    const ActualizarMatricula = async(matricula: Matricula) => {
        await apiCredit
            .updateMatriculaCredito(
                matricula
            )
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
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

    const consultarEstratos = async () => {
        await apiCatalog
            .ObtenerEstratos()
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess) {
                        setEstratos(response?.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const isFinalizedActivity = procedureRequest.stateActivityID === 4;
  

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

    const removeRealStateRow = (index: any) => () => {
        setIndexes(prevIndexes => [
            ...prevIndexes.filter(item => item !== index),
        ]);
    };




    const obtenerValorTipoMatricula = (tipo_Matricula: number | undefined) => {

        const catalogoMAtricula = tipoMatriculas.find(x => x.idValor === tipo_Matricula)?.descripcion
       
        return catalogoMAtricula


      };

    const onSubmitRow = (data: any) => {
    
        setOcultarFormulario(true);
        let matriculas: any[] = [];

        if (data.matricula3) {
            matriculas = [
                {
                    realStateRegistrationNumber: data.matricula1,
                    realStateRegistrationDate: data.fechaMatricula1,
                    propertyChip: data.chip1,
                    cadastralID: data.catastral1,
                },
                {
                    realStateRegistrationNumber: data.matricula2,
                    realStateRegistrationDate: data.fechaMatricula2,
                    propertyChip: data.chip2,
                    cadastralID: data.catastral2,
 
                },
                {
                    realStateRegistrationNumber: data.matricula3,
                    realStateRegistrationDate: data.fechaMatricula3,
                    propertyChip: data.chip3,
                    cadastralID: data.catastral3,

                },
            ];
        } else if (data.matricula2) {
            matriculas = [
                {
                    realStateRegistrationNumber: data.matricula1,
                    realStateRegistrationDate: data.fechaMatricula1,
                    propertyChip: data.chip1,
                    cadastralID: data.catastral1,

                },
                {
                    realStateRegistrationNumber: data.matricula2,
                    realStateRegistrationDate: data.fechaMatricula2,
                    propertyChip: data.chip2,
                    cadastralID: data.catastral2,

                },
            ];
        } else if (data.matricula1) {
            matriculas = [
                {
                    realStateRegistrationNumber: data.matricula1,
                    realStateRegistrationDate: data.fechaMatricula1,
                    propertyChip: data.chip1,
                    cadastralID: data.catastral1,
                    
                },
            ];
        }

        matriculas.forEach(item => {
            addRealStateRow(item);
        });

        const err: string[] = [];
        indexes.forEach(index => {

            if (index.cadastralID === data.cadastralID) {
                err.push('Cédula Catastral no se puede repetir');
            }
            if (index.propertyChip === data.propertyChip) {
                err.push('Chip no se puede repetir');
            }
            if (
                index.realStateRegistrationNumber ===
                data.realStateRegistrationNumber
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
        } else if (
            (data.realStateRegistrationDate as Date) < new Date('01-01-1910')
        ) {
            setMessage('Fecha no válida');
            setType('error');
        } else if ((data.realStateRegistrationDate as Date) > new Date()) {
            setMessage('Fecha no válida');
            setType('error');
        } else {
            setDatosMatriculas(data);
        }
    };

    const onSubmit  =  async(data: any) => {

        if (data.finalConsiderations === "" || data.finalConsiderations === undefined){
            setType('error');
            setMessage('Consideraciones Finales es un campo Requerido');
            return
        }

        if (indexes.length === 0) {
            handleSubmit2(onSubmitRow)();
            setType('error');
            setMessage('Se debe agregar al menos una información catastral.');
        } else {

            const confirmAlert = await  NotificationService.confirmAlert(
                '¿Está seguro?',
            );
            if (confirmAlert) {

                    const user = AuthService.getUserInfo();
                    const valorAvaluoCatastral = UtilService.cleanCurrencyValue(
                        getValues('catastralValue'),
                    );
                    // const valorPropiedad = UtilService.cleanCurrencyValue(
                    //     getValues('propertySaleValue'),
                    // );
                    const dataRequest: CreditTechnicalCreateRequestModel = {
                        idSolicitudInformacionTecnica:creditTechnicalData.requestTechnicalInformationID,
                        conceptoEstTecnico: data.conceptValue,
                        condicionesSalvedades: "data.conditionsSavings",
                        consideracionesFinales: data.finalConsiderations ,
                        creadoPor: user.USR_ID,
                        declaraCumplimiento: "data.complianceDeclaration",
                        fechaCreacion: moment().format(),
                        estrato: data.stratum,
                        idSolicitudCredito: procedureRequest.requestID,
                        solicitudesCreditoInfTecnicaInmueble: indexes.map(val => ({
                            idSolicitudtecnica: creditTechnicalData.requestTechnicalInformationID,
                            idSolicitudInfoTecnicaInmueble: val.requestPropertyTechnicalInformationID,
                            idSolicitudCredito: procedureRequest.requestID,
                            numeroMatricula: val.numeroMatricula,
                            fechaMatricula: moment(val.fechaMatricula,'YYYY/MM/DD',).format(),
                            numeroChip: val.chip,
                            tipo_Matricula: val.tipo_Matricula,
                            cedulaCatastral: val.cedulaCatastral,
                            creadoPor: user.USR_ID,
                            fechaCreacion: moment().format(),
                        })),
                        
                        valorAvaluoComercial: UtilService.cleanCurrencyValue( data.commercialValue),
                        // valorVentaInmueble: valorPropiedad,
                        valorAvaluoCatastral: valorAvaluoCatastral,
                    };

                    setIsSubmitted(true);
                    startPostCreditCreateTechnicalData(dataRequest);
                }
                setValue('finalConsiderations',creditTechnicalData.finalConsiderations,);
        }
    };

    if (
        creditTechnicalData.requestTechnicalInformationID != null &&
        !isLoaded
    ) {
        setIsLoaded(true);
        const fields = Object.keys(creditTechnicalData);
        fields.forEach(field =>
            setValue(
                field as any,
                creditTechnicalData[field as keyof CreditTechnicalDataModel],
            ),
        );
 
    }

    if (isLoading ) {
        return <CHLoadingProcess />;
    }

    if (isSubmitted && isCreatedSuccessfully) {
        NotificationService.genericAlert('Guardado con éxito', '');
        setIsSubmitted(false);
    }


    return (
        <>
            <CHPaper title="Datos Estudio Técnico">
                <Table>
                    <TableBody>
                        <TableRow>
                            <TableCell>No. Matricula</TableCell>
                            <TableCell>Fecha Expedición CTL</TableCell>
                            <TableCell>Chip</TableCell>
                            <TableCell>Cédula Catastral</TableCell>
                            <TableCell>Tipo Matrícula</TableCell>
                            {!isFinalizedActivity && ( <TableCell align='left'>Acciones</TableCell>)}
                        </TableRow>

                        {indexes.map((index, key) => (
                            <TableRow
                                key={`realState-${key}`}
                                style={{ textTransform: 'uppercase' }}
                            >
                             
                                <TableCell>
                                    {index?.numeroMatricula}
                                </TableCell>
                           
                                <TableCell>
                                    {UtilService.getDate( index?.fechaMatricula)}
                                </TableCell>
                                <TableCell>
                                    {index?.chip}
                                </TableCell>
                                <TableCell>
                                    {index?.cedulaCatastral}
                                </TableCell>
                                <TableCell>
                                    {obtenerValorTipoMatricula(index.tipo_Matricula)}
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
                                 </TableCell>
                                )}
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
                <br/>
                {!isFinalizedActivity
                   && (
                    <Button
                        size="small"
                        variant="contained"
                        color="primary"
                        onClick={handleOpenModalAgregar}
                    >
                        Agregar matricula
                    </Button>
                )}

                <Grid container>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            control={control}
                            name="commercialValue"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                  
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    currencyMask
                                    label="Valor Avalúo Comercial *"
                                    error={!!error}
                                    InputLabelProps={{ shrink: true }} 
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            control={control}
                            name="catastralValue"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    label="Valor Catastral *"
                                    error={!!error}
                                    InputLabelProps={{ shrink: true }} 
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    {/* <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    currencyMask
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    InputLabelProps={{ shrink: true }} 
                                    label="Valor Venta Inmueble *"
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                            name="propertySaleValue"
                        />
                    </Grid> */}
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            control={control}
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
                                    label="Estrato *"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={onChange}
                                >
                                    {estratos.map((value, index) => (
                                        <MenuItem
                                            key={`stratum-${index}`}
                                            value={value.descripcion}
                                        >
                                            {value.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="stratum"
                        />
                    </Grid>

                    <Grid className={classes.column} item xs={12} md={4}>
                        <Controller
                            name="conceptValue"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={isFinalizedActivity}
                                    defaultValue=""
                                    error={!!error}
                                    label="Concepto del avalúo *"
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

                   

                    <Grid className={classes.column} item xs={12} md={12}>
                    <Typography variant="h6" className={classes.title}>
                    Consideraciones finales para el informe de avalúo *
                    </Typography>
                        <Controller
                            control={control}
                            name="finalConsiderations"
                         
                            render={({ field: { onChange, value }, fieldState: { error } }) => (
                            <TextareaAutosize
                                className={error ? classes.errorTextarea : classes.textarea}
                              

                               
                                value={value}
                                onChange={onChange}
                                style={{ textTransform: 'uppercase',resize: 'none', width: '100%' }}
                                readOnly={isFinalizedActivity} 

                          
                            />
                            )}
                            
                        />
                    </Grid>
                    
                    {/* <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Declaración de cumplimiento *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={isFinalizedActivity}
                                    value={value}
                                    onChange={onChange}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                />
                            )}
                            name="complianceDeclaration"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="conditionsSavings"
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
                                    label="Condiciones y salvedades al avalúo *"
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
                    </Grid> */}

                    

                </Grid>
            
                {!isFinalizedActivity
                                && (
                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={handleSubmit(onSubmit)}
                                    >
                                        Guardar
                                    </Button>
                                )}

            </CHPaper>
            <CHSnackBar
                message={message}
                type={type}
                isOpen={message !== ''}
                handleClose={handleClose}
            />
            <ModalAgregarMatricula
                open={openModalAgregar}
                handleClose={handleCloseModalAgregar}
                formulario={formularioMatriculas}
                handleOnClick={handleOnClickAgregarMatricula}
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
    // isLoadingCredit: state.credit.loading,
    creditTechnicalData: state.credit.creditTechnicalData,
    isCreatedSuccessfully: state.credit.createIsSuccessfully,
    creditProductData: state.credit.creditProductData,
});

const mapDispatchToProps = {
    startPostCreditCreateTechnicalData,
    startPostCreditTechnicalData,
    startPostCreditProductData,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(RequestInformationTechnicalAnalyst);
