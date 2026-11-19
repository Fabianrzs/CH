import { createStyles, makeStyles, Grid, MenuItem } from '@material-ui/core';
import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { Controller, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import { startGetAllDepartments, startGetAllIdentificationTypes} from '../../../actions/catalog.action';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CITYDTO, DEPARTAMENTODTO, TIVIVIENDA } from '../../../types/catalog.type';
import ApiCatalog from '../../../services/api-catalog.service';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import CHSelect from '../../../components/ch-select/CHSelect';
import { AppState } from '../../../store/rootReducer';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import { Address } from '../../../types/address.type';
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import AddressComponent from '../../../components/modals/address/address.component';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { Constructora } from '../constants/fondosolidaridadform.model.type';
import { ESTADO_PROYECTO } from '../constants/fondoSolidaridad.constants';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
    }),
);

const ContratoProyecto = (props: Props) => {

    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {loginState,
           catalogState,
           informacionProyecto,
           formularioProyecto,
           ciudades,
           ciudadesLicenciaConstruccion,
           ciudadesLicenciaUrbanismo,
           estadoProyecto,
           obtenerCiudades,
           obtenerCiudadesLicenciaConstruccion,
           obtenerCiudadesLicenciaUrbanismo,
           startGetAllDepartments,
           startGetAllIdentificationTypes} = props;
  
    const [constructoras, setConstructoras] = useState<Constructora[]>([]);
    const [tiposProyectos,setTiposProyectos] = useState<any[]>([]);
    const [vivienda,setVivienda] = useState< TIVIVIENDA[]>([]);
    const [open, setOpen] = useState(false);

    useEffect(() => {
        consultarConstructoras();
        startGetAllDepartments(loginState.claims);
        startGetAllIdentificationTypes(loginState.claims);
        consultarVivienda();
        consultarTiposProyectos();
        if(formularioProyecto.getValues('estado') === ESTADO_PROYECTO.EN_CREACION){
            formularioProyecto.setValue('nombreEstado','EN CREACIÓN')
        }else if(formularioProyecto.getValues('estado') === ESTADO_PROYECTO.VIGENTE){
            formularioProyecto.setValue('nombreEstado','VIGENTE')
        }else if(formularioProyecto.getValues('estado') === ESTADO_PROYECTO.ANULADO){
            formularioProyecto.setValue('nombreEstado','ANULADO')
        }
    },[])

    const consultarConstructoras = async () => {
        await apiFondoSolidaridad.getConsultarConstructoras()
            .then((response:any) => {
                if(response.data){
                   setConstructoras(response.data);
                }
            })
            .catch( (e) => {
                toast(`Error en consultar constructoras ${e.message}`, { type: toast.TYPE.ERROR })
            })
    };

    const consultarVivienda = async () => {
        await apiCatalog.getVivienda()
            .then((response:any) => {
                if(response.data){
                    setVivienda(response.data);
                }
            })
            .catch( (e) => {
                toast(`Error en consultar constructoras ${e.message}`, { type: toast.TYPE.ERROR })
            })
    };

    const handleOpenAddress = () => {
        if((estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
            estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)){
            setOpen(false);
        }else{
            setOpen(true);
        }
    };

    const onCloseAddress = (value: Address) => {

        if (value !== null) {
            const result = setFormDataAddres(value);
            formularioProyecto.setValue('direccion', result);
            formularioProyecto.clearErrors('direccion');
            setOpen(false);
        } else {
            formularioProyecto.setValue('direccion', '');
            setOpen(false);
        }
    };

    const consultarTiposProyectos = async() => {
        const TIPO_PROYECTOS = 95;
        await apiCatalog.getObtenerValoresCatalogosPorIdCatalogo(TIPO_PROYECTOS)
             .then(response => {
                 if(response.data){
                    setTiposProyectos(response.data);
                 }
             }).catch(() => {
                 toast(`Error al consultar tipos de proyectos`, {type: toast.TYPE.ERROR});
             })
     }

    return (
        <>
         <AddressComponent
            onOpen={open}
            onClose={onCloseAddress}
            control={formularioProyecto.control}
            getValues={formularioProyecto.watch}
            tipoDireccion
            setValue={formularioProyecto.setValue}
        />
         <CHPaper title="Información Proyecto">
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="constructora"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={value}
                                        onChange={onChange}
                                        label="Constructora *"
                                    >
                                     {
                                        constructoras.map((option:Constructora) => (
                                            <MenuItem key={option.id_Constructuora} 
                                                      value={option.id_Constructuora} 
                                            >
                                                {option.nombreConstructora}
                                            </MenuItem>
                                        ))
                                    }
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreProyecto"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombre Proyecto *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {

                                        if (
                                            e.target.value.length <= 40
                                        ) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                    '')
                                            
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="direccion"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Dirección *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onClick={handleOpenAddress}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="departamentoId"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={value}
                                        onChange={e => {
                                            formularioProyecto.setValue('ciudadId','');
                                            obtenerCiudades(Number(e.target.value));
                                            onChange(e);
                                        }}
                                        label="Departamento *"
                                    >
                                    {catalogState?.departments.map(
                                        (option: DEPARTAMENTODTO) => {
                                            if(option.id !== 0){
                                                return (
                                                    <MenuItem key={option.id} 
                                                      value={option.id}
                                            >
                                                     {option.nombre}
                                                     </MenuItem>
                                                )
                                            }
                                            return null;
                                       }
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="ciudadId"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Municipio *"
                                >
                                    {ciudades?.map(
                                        (option: CITYDTO) => {   
                                            if(option.id !== 0){
                                             return (
                                                <MenuItem key={option.id} 
                                                              value={option.id}
                                                >
                                                   {option.nombre}
                                                </MenuItem>
                                              )
                                            }
                                            return null;
                                        }
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="telefono"
                                control={formularioProyecto.control}
                                rules={{
                                    pattern:
                                        /\(?([3,6]{1}[0-9]{2})\)?([ .-]?) ?([0-9]{3})-?\2([0-9]{4})/,
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        InputLabelProps={{ shrink: true }}
                                        label="Teléfono *"
                                        type="number"
                                        error={!!error}
                                        helperText={
                                            error
                                                ? 'El teléfono debe empezar por 3 si es celular o 6 si es fijo y tener una longitud de 10 dígitos'
                                                : null
                                        }
                                        isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                        value={value}
                                        onChange={e => {
                                            if (
                                                e.target.value.length <= 10
                                            ) {
                                                onChange(e);
                                            }
                                        }}
                                    />
                                )}
                            />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="valorProyecto"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        currencyMask
                                        isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                        InputLabelProps={{ shrink: true }}
                                        label="Valor Proyecto *"
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                       </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="cupos"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    label="Cupos *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="latitud"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    decimalsMask
                                    label="Latitud *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="longitud"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    decimalsMask
                                    label="Longitud *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                           <Controller
                                name="tipoProyecto"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Tipo Proyecto *"
                                >
                                    { vivienda.map((option:TIVIVIENDA) => (
                                            <MenuItem key={option.tiV_VIVIENDA_ID} 
                                                      value={option.tiV_VIVIENDA_ID} 
                                            >
                                                {option.tiV_VIVIENDA_DESCRIPCION}
                                            </MenuItem>
                                    ))} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreEstado"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Estado"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                </Grid> 
            </CHPaper>
            <CHPaper title="Información Contrato">
            <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaContrato"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioProyecto.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Contrato *"
                                    InputLabelProps={{ shrink: true}}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    inputProps={{readOnly: (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                                 estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}}
                                    disableFuture
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                               estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaVigencia"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioProyecto.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Vigencia *"
                                    InputLabelProps={{ shrink: true}}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    inputProps={{readOnly: (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}}
                                    disableFuture
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                               estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaActaInicio"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioProyecto.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Acta de Inicio *"
                                    InputLabelProps={{ shrink: true}}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    inputProps={{readOnly: (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}}
                                    helperText={error ? error.message : null}
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                               estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    disableFuture
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaEntrega"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioProyecto.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Entrega *"
                                    InputLabelProps={{ shrink: true}}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    inputProps={{readOnly: (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}}
                                    helperText={error ? error.message : null}
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                               estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    disableFuture
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="objetoContrato"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    helperText={error ? error.message : null}
                                    label="Objeto del Contrato *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={ e => {
                                        if (e.target.value.length <= 40) {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '')
                                        
                                        );
                                    }}
                                  }
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="tipoContrato"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={value}
                                        onChange={onChange}
                                        label="Tipo Contrato *"
                                    >
                                    {
                                        tiposProyectos.map((option:any) => (
                                            <MenuItem key={option.idValor} 
                                                      value={option.idValor} 
                                            >
                                                {option.descripcion}
                                            </MenuItem>
                                        ))
                                    }
                                </CHSelect>
                            )}
                        /> 
                    </Grid> 
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="numeroRegistro"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    label="Número de Registro *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
              </Grid>
            </CHPaper>
            <CHPaper title="Licencia de Construcción">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="resolucion"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    helperText={error ? error.message : null}
                                    label="Resolución *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 20){
                                            onChange(
                                                e.target.value.replace(
                                                    /[#./?=!@$%^&*^)(+=_}{":;><|~` ]/g,
                                                    '',
                                                ),
                                            );

                                        }
                                     
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="curaduria"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    helperText={error ? error.message : null}
                                    label="Curaduría *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 30){
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '')
                                        
                                        );
                                       }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="departamentoLicenciaConstruccionId"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={value}
                                        onChange={e => {
                                            formularioProyecto.setValue('ciudadLicenciaConstruccionId','');
                                            obtenerCiudadesLicenciaConstruccion(Number(e.target.value));
                                            onChange(e);
                                        }}
                                        label="Departamento Licencia Construcción *"
                                    >
                                    {catalogState?.departments.map(
                                        (option: DEPARTAMENTODTO) => {
                                            if(option.id !== 0){
                                                return (
                                                    <MenuItem key={option.id} 
                                                      value={option.id}
                                            >
                                                     {option.nombre}
                                                     </MenuItem>
                                                )
                                            }
                                            return null;
                                       }
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="ciudadLicenciaConstruccionId"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Municipio Licencia Construcción *"
                                >
                                    {ciudadesLicenciaConstruccion?.map(
                                        (option: CITYDTO) => {   
                                            if(option.id !== 0){
                                             return (
                                                <MenuItem key={option.id} 
                                                              value={option.id}
                                                >
                                                   {option.nombre}
                                                </MenuItem>
                                              )
                                            }
                                            return null;
                                        }
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaVigenciaLicenciaConstruccion"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioProyecto.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Vigencia *"
                                    InputLabelProps={{ shrink: true}}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    inputProps={{readOnly: (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}}
                                    helperText={error ? error.message : null}
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                               estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    disableFuture
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="titularLicenciaConstruccion"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Titular de la Licencia *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 30){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                    '')
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaResolucion"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioProyecto.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Resolución *"
                                    InputLabelProps={{ shrink: true}}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    inputProps={{readOnly: (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}}
                                    helperText={error ? error.message : null}
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                               estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    disableFuture
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="mismaLicencia"
                        control={formularioProyecto.control}
                        render={({ field: { onChange, value } }) => (
                            <CHCheckbox
                                label="Misma Licencia"
                                onChange={e => {
                                    if(e.target.checked){
                                        formularioProyecto.clearErrors('resolucionUrbanismo');
                                        formularioProyecto.clearErrors('curaduriaUrbanismo');
                                        formularioProyecto.clearErrors('departamentoLicenciaUrbanismoId');
                                        formularioProyecto.clearErrors('ciudadLicenciaUrbanismoId');
                                        formularioProyecto.clearErrors('fechaVigenciaUrbanismo');
                                        formularioProyecto.clearErrors('titularLicenciaUrbanismo');
                                        formularioProyecto.clearErrors('fechaResolucionUrbanismo');
                                        formularioProyecto.setValue('resolucionUrbanismo', formularioProyecto.getValues('resolucion'));
                                        formularioProyecto.setValue('curaduriaUrbanismo', formularioProyecto.getValues('curaduria'));
                                        formularioProyecto.setValue('departamentoLicenciaUrbanismoId', formularioProyecto.getValues('departamentoLicenciaConstruccionId'));
                                        obtenerCiudadesLicenciaUrbanismo(formularioProyecto.getValues('departamentoLicenciaConstruccionId'));
                                        formularioProyecto.setValue('ciudadLicenciaUrbanismoId', formularioProyecto.getValues('ciudadLicenciaConstruccionId'));
                                        formularioProyecto.setValue('fechaVigenciaUrbanismo', formularioProyecto.getValues('fechaVigenciaLicenciaConstruccion'));
                                        formularioProyecto.setValue('titularLicenciaUrbanismo', formularioProyecto.getValues('titularLicenciaConstruccion'));
                                        formularioProyecto.setValue('fechaResolucionUrbanismo', formularioProyecto.getValues('fechaResolucion'));
                                    }else{
                                        formularioProyecto.setValue('resolucionUrbanismo', '');
                                        formularioProyecto.setValue('curaduriaUrbanismo', '');
                                        formularioProyecto.setValue('departamentoLicenciaUrbanismoId', '');
                                        formularioProyecto.setValue('ciudadLicenciaUrbanismoId', '');
                                        formularioProyecto.setValue('fechaVigenciaUrbanismo', new Date());
                                        formularioProyecto.setValue('titularLicenciaUrbanismo', '');
                                        formularioProyecto.setValue('fechaResolucionUrbanismo', new Date());
                                    }
                                    onChange(e);
                                }}
                                checked={value}
                                disabled={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                            />
                        )}
                    />
                </Grid>
                </Grid>
            </CHPaper>
            <CHPaper title="Licencia de Urbanismo">
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="resolucionUrbanismo"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Resolución *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 20){
                                            onChange(
                                                e.target.value.replace(
                                                    /[#./?=!@$%^&*^)(+=_}{":;><|~` ]/g,
                                                    '',
                                                ),
                                            );

                                        }
                                     
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="curaduriaUrbanismo"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    helperText={error ? error.message : null}
                                    label="Curaduría *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 30){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                    '')
                                            
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="departamentoLicenciaUrbanismoId"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={value}
                                        onChange={e => {
                                            formularioProyecto.setValue('ciudadLicenciaUrbanismoId','');
                                            obtenerCiudadesLicenciaUrbanismo(Number(e.target.value));
                                            onChange(e);
                                        }}
                                        label="Departamento Licencia Urbanismo *"
                                    >
                                    {catalogState?.departments.map(
                                        (option: DEPARTAMENTODTO) => {
                                            if(option.id !== 0){
                                                return (
                                                    <MenuItem key={option.id} 
                                                      value={option.id}
                                            >
                                                     {option.nombre}
                                                     </MenuItem>
                                                )
                                            }
                                            return null;
                                       }
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="ciudadLicenciaUrbanismoId"
                                control={formularioProyecto.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Municipio Licencia de Urbanismo *"
                                >
                                    {ciudadesLicenciaUrbanismo?.map(
                                        (option: CITYDTO) => {   
                                            if(option.id !== 0){
                                             return (
                                                <MenuItem key={option.id} 
                                                              value={option.id}
                                                >
                                                   {option.nombre}
                                                </MenuItem>
                                              )
                                            }
                                            return null;
                                        }
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaVigenciaUrbanismo"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioProyecto.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Vigencia *"
                                    InputLabelProps={{ shrink: true}}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    inputProps={{readOnly: (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}}
                                    helperText={error ? error.message : null}
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                               estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    disableFuture
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="titularLicenciaUrbanismo"
                            control={formularioProyecto.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    helperText={error ? error.message : null}
                                    label="Titular de la Licencia *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        if(e.target.value.length <= 30){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                    '')
                                            );
                                        }
                                    }}
                                />
                            )}
                        />
                </Grid>
                 <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="fechaResolucionUrbanismo"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioProyecto.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Resolución *"
                                    InputLabelProps={{ shrink: true}}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    inputProps={{readOnly: (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}}
                                    helperText={error ? error.message : null}
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                               estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    disableFuture
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                </Grid>
            </CHPaper>
        </>
    )
}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    informacionProyecto: any;
    formularioProyecto: UseFormReturn<any>;
    ciudades: CITYDTO[]; 
    ciudadesLicenciaConstruccion: CITYDTO[]; 
    ciudadesLicenciaUrbanismo: CITYDTO[]; 
    estadoProyecto: number;
    obtenerCiudades: (idDepartamento: number) => void;
    obtenerCiudadesLicenciaConstruccion: (idDepartamento: number) => void;
    obtenerCiudadesLicenciaUrbanismo: (idDepartamento: number) => void;
    startGetAllDepartments: (claims: string) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllDepartments,
    startGetAllIdentificationTypes
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ContratoProyecto);