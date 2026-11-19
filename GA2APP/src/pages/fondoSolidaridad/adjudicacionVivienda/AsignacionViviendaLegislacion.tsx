import { createStyles, Grid, makeStyles, MenuItem } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { startGetAllIdentificationTypes, 
         startGetAllDepartments } from '../../../actions/catalog.action';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import { CITYDTO, 
         DEPARTAMENTODTO, 
         IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import ApiCatalog from '../../../services/api-catalog.service';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import { tipoInmueble } from '../constants/fondoSolidaridad.constants';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
	}),
);

const AsignacionViviendaLegislacion = (props: Props) => {
    const classes = useStyles();
    const apiCatalog = ApiCatalog();
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {loginState, 
           catalogState, 
           formulario, 
           startGetAllDepartments,
           startGetAllIdentificationTypes} = props;
    const [ciudades,setCiudades] = useState<CITYDTO[]>([]);
    const [tiposCuenta,setTiposCuenta] = useState<any []>([]);
    const [proyectos,setProyectos] = useState<any[]>([]);
    const [entidadesBancarias,setEntidadesBancarias] = useState<any []>([]);
    const [tipoVivienda, setTipoVivienda] = useState<any[]>([]);
    const [mostrarDatosAcompaniante, setMostrarDatosAcompaniante] = useState(false);

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);
        startGetAllDepartments(loginState.claims);
        consultarTiposCuenta();
        consultarEntidadesBancarias();
        consultarProyectos();
    },[])

    const consultarProyectos = async() => {
        const busqueda = {
            proyectoId: '',
            idDepartamento: '',
            idCiudad: '',
            idEstado: ''
        }
        await apiFondoSolidaridad.getConsultarProyectos(busqueda)
        .then(response => {
            if(response.data){
                if(response?.data?.estado === 'EN CREACIÓN'){
                    setProyectos(response.data);
                }
            }
        }).catch(() => {
            toast(`Error al consultar proyectos en asignación vivienda`, {type: toast.TYPE.ERROR});
        })
    }

    const obtenerCiudades = async (idDepto: number) => {
        const value = idDepto.toString();
        const response = await apiCatalog.getCityOfState(value);
        if (response.data !== null) {
            const datos = (await response.data) as CITYDTO[];
            setCiudades(datos);
        }
    };

    const consultarTiposCuenta = async() => {
        await apiCatalog.getObtenerValoresCatalogosPorIdCatalogo(67)
             .then(response => {
                 if(response.data){
                     setTiposCuenta(response.data);
                 }
             }).catch(() => {
                 toast(`Error del sistema`, {type: toast.TYPE.ERROR});
             })
     }

     const consultarEntidadesBancarias = async() => {
        await apiCatalog.getAllEntidadBancaria()
             .then(response => {
                 if(response.data){
                     setEntidadesBancarias(response.data);
                 }
             }).catch(() => {
                 toast(`Error del sistema`, {type: toast.TYPE.ERROR});
             })
    }

    return (
     <>
        <CHPaper title="Adjudicación">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="estadoAdjudicacion"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Estado Adjudicación"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={e => {
                                            if(e.target.value.length <= 15){
                                                onChange(
                                                    e.target.value.replace(
                                                        /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                        '')
                                                );
                                            }
                                        }}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="radicadoAceptacion"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Radicado de Aceptación"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="proyectoAsignar"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        error={!!error}
                                        helperText={error ? error.message : null}
                                        value={value}
                                        onChange={onChange}
                                        label="Proyecto a Asignar *"
                                    >
                                    {proyectos.map(
                                        (option: any) => (
                                           
                                        <MenuItem key={option.codigo} 
                                            value={option.codigo}
                                        >
                                        {option.nombreProyecto}
                                        </MenuItem>
                                        )
                                    )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                      <Controller
                            name="tipoInmueble"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={e => {
                                        const vivienda = tipoInmueble.find(item => item.descripcion === e.target.value);
                                        if(vivienda){
                                            setTipoVivienda(vivienda.tiposVivienda);
                                        }
                                        onChange(e);
                                    }}
                                    label= "Tipo Inmueble *"
                                >
                                   {tipoInmueble.map(
                                        (option: any) => (
                                           
                                        <MenuItem key={option.descripcion} 
                                            value={option.descripcion}
                                        >
                                        {option.descripcion}
                                        </MenuItem>
                                        )
                                    )} 
                            </CHSelect>
                        )}
                      /> 
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                         <Controller
                                name="tipoVivienda"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Tipo Vivienda *"
                                >
                                 {tipoVivienda.map(
                                        (option: any) => (
                                           
                                        <MenuItem key={option.descripcion} 
                                            value={option.descripcion}
                                        >
                                        {option.descripcion}
                                        </MenuItem>
                                        )
                                 )} 
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="claseVivienda"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Clase Vivienda *"
                                >
                    
                                <MenuItem key={0}
                                        value="APARTAMENTO"
                                >
                                    APARTAMENTO
                                </MenuItem>
                                <MenuItem key={1}
                                        value="CASA"
                                >
                                    CASA
                                </MenuItem>
                                </CHSelect>
                            )}
                        /> 
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="valorVivienda"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <>
                                    <CHTextField
                                        currencyMask
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
                                        isReadOnly={false}
                                        label="Valor Vivienda"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        inputProps={{
                                            style: {
                                                textAlign: 'right',
                                            },
                                        }}
                                        onChange={onChange}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                </Grid>
        </CHPaper>  
        <CHPaper title="Logística Para la Entrega">
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="tiempoDesplazamiento"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                label="Tiempo Desplazamiento (Horas) *"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                onChange={e => {
                                    if(e.target.value.length <= 2){
                                        onChange(
                                            e.target.value.replace(
                                                /[^A-Z\d-]/g,
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
                        name="distancia"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                label="Distancia *"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                onChange={e => {
                                    if(e.target.value.length <= 30){
                                        onChange(e);
                                    }
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="acompaniante"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={e => {
                                        if(e.target.value === 'SI'){
                                            setMostrarDatosAcompaniante(true);
                                        }else{
                                            setMostrarDatosAcompaniante(false);
                                        }
                                        onChange(e);
                                    }}
                                    label="Acompañante *"
                                >
                                    <MenuItem key='SI'
                                              value='SI'
                                    >
                                        SI
                                    </MenuItem>
                                    <MenuItem key='NO'
                                              value='NO'
                                    >
                                        NO
                                    </MenuItem>
                            </CHSelect>
                        )}
                    /> 
                </Grid>
                {mostrarDatosAcompaniante && (
                      <Grid className={classes.column} item xs={12} md={3}>
                      <Controller
                              name="tipoIdentificacionAcompaniante"
                              control={formulario.control}
                              rules={{
                                  required: 'Este campo es requerido',
                              }}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHSelect
                                      error={!!error}
                                      helperText={error ? error.message : null}
                                      value={value}
                                      onChange={onChange}
                                      label="Tipo Identificación Acompañante *"
                                  >
                                       {catalogState?.IdentificationType.map(
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
                )}
                </Grid>
                <Grid container>
                {mostrarDatosAcompaniante && (
                       <Grid className={classes.column} item xs={12} md={3}>
                       <Controller
                           name="numeroIdentificacionAcompaniante"
                           rules={{
                               required: 'Este campo es requerido.',
                           }}
                           control={formulario.control}
                           render={({
                               field: { onChange, value },
                               fieldState: { error },
                           }) => (
                               <CHTextField
                                   numberMask
                                   error={!!error}
                                   helperText={error ? error.message : null}
                                   label="No. Identificación Acompañante *"
                                   value={value}
                                   InputLabelProps={{ shrink: true }}
                                   onChange={e => {
                                       if(e.target.value.length <= 12){
                                           onChange(e);
                                       }
                                   }}
                               />
                           )}
                       />
                   </Grid>
                )}
                {mostrarDatosAcompaniante && (
                   <Grid className={classes.column} item xs={12} md={3}>
                       <Controller
                           name="nombresApellidosAcompaniante"
                           rules={{
                               required: 'Este campo es requerido.',
                           }}
                           control={formulario.control}
                           render={({
                               field: { onChange, value },
                               fieldState: { error },
                           }) => (
                               <CHTextField
                                   error={!!error}
                                   helperText={error ? error.message : null}
                                   label="Nombres y Apellidos *"
                                   value={value}
                                   InputLabelProps={{ shrink: true }}
                                   inputProps={{
                                       style: {
                                           textTransform: 'uppercase',
                                       },
                                   }}
                                   onChange={e => {
                                       if(e.target.value.length <= 40){
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
                )}
                </Grid>
        </CHPaper>    
        <CHPaper title="Entrega de la Vivienda">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={2}>
                        <Controller
                            name="fechaEntregaVivienda"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    label="Fecha de Entrega *"
                                    InputLabelProps={{ shrink: true}}
                                    inputProps={{
                                        readOnly: true
                                    }}
                                    onChange={onChange}
                                    value={value}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    autoOk
                                />
                            )}
                        />
                 </Grid>
                 <Grid className={classes.column} item xs={12} md={2}>
                    <Controller
                            name="entregada"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Entregada *"
                                >
                                    <MenuItem key='SI' 
                                              value='SI'
                                    >
                                        SI
                                    </MenuItem>
                                    <MenuItem key='NO'
                                              value='NO'
                                    >
                                        NO
                                    </MenuItem>
                            </CHSelect>
                        )}
                    /> 
                </Grid>
                <Grid className={classes.column} item xs={12} md={8}>
                        <Controller
                            name="observacionesEntrega"
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Observaciones entrega"
                                    value={value}
                                    onChange={e => {
                                        if(e.target.value.length <= 180){
                                            onChange(
                                                e.target.value.replace(
                                                    /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                    '')
                                            );
                                        }
                                    }}
                                    InputLabelProps={{ shrink: true }}
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
        </CHPaper>   
        <CHPaper title="Escrituración">
        <Grid container>
        <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                        name="departamentoId"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                error={!!error}
                                helperText={error ? error.message : null}
                                value={value}
                                onChange={e => {
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
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
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
                    name="notaria"
                    control={formulario.control}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHTextField
                            error={!!error}
                            helperText={error ? error.message : null}
                            label="Notaria"
                            value={value}
                            onChange={e => {
                                if(e.target.value.length <= 20){
                                    onChange(
                                        e.target.value.replace(
                                            /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                            '')
                                    );
                                }
                            }}
                            InputLabelProps={{ shrink: true }}
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                        />
                    )}
                />
            </Grid> 
            <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="fechaEscritura"
                    rules={{
                        required: 'Este campo es requerido.',
                    }}
                    control={formulario.control}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHDatePicker
                            label="Fecha Escritrura *"
                            InputLabelProps={{ shrink: true }}
                            inputProps={{
                                readOnly: true
                            }}
                            onChange={onChange}
                            value={value}
                            error={!!error}
                            helperText={error ? error.message : null}
                            autoOk
                        />
                    )}
                />
            </Grid>
            </Grid>
            <Grid container>
            <Grid className={classes.column} item xs={12} md={3}>
            <Controller
                    name="estadoEscritura"
                    control={formulario.control}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <>
                            <CHTextField
                                error={!!error}
                                helperText={
                                    error ? error.message : null
                                }
                                isReadOnly={false}
                                label="Estado Escritura"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                onChange={e => {
                                    if(e.target.value.length <= 15){
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '')
                                        );
                                    }
                                }}
                            />
                        </>
                    )}
                />
               </Grid>
               <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="numeroEscritura"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                numberMask
                                error={!!error}
                                helperText={error ? error.message : null}
                                label="Número Escritura *"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                onChange={e => {
                                    if(e.target.value.length <= 12){
                                        onChange(e);
                                    }
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                   <Controller
                        name="radicadoEntrega"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Radicado de Entrega *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                value={value}
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
                        name="numeroMatriculaInmobiliaria"
                        control={formulario.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Número Matrícula Inmobiliaria *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={false}
                                InputLabelProps={{ shrink: true }}
                                value={value}
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
          </Grid>
          <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fechaEntregaEscrituracion"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHDatePicker
                                label="Fecha de Entrega *"
                                InputLabelProps={{ shrink: true}}
                                inputProps={{
                                    readOnly: true
                                }}
                                onChange={onChange}
                                value={value}
                                error={!!error}
                                helperText={error ? error.message : null}
                                autoOk
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                    name="envioEscritura"
                    control={formulario.control}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHTextField
                            error={!!error}
                            helperText={error ? error.message : null}
                            label="Envío de Escritura"
                            value={value}
                            onChange={e => {
                                if(e.target.value.length <= 20){
                                    onChange(
                                        e.target.value.replace(
                                            /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                            '')
                                    );
                                }
                            }}
                            InputLabelProps={{ shrink: true }}
                            inputProps={{
                                style: {
                                    textTransform: 'uppercase',
                                },
                            }}
                        />
                    )}
                />
            </Grid> 
            <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="fechaEntregaEscritura"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHDatePicker
                                label="Fecha de Entrega Escritura *"
                                InputLabelProps={{ shrink: true}}
                                inputProps={{
                                    readOnly: true
                                }}
                                onChange={onChange}
                                value={value}
                                error={!!error}
                                helperText={error ? error.message : null}
                                autoOk
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="numeroGuia"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                numberMask
                                error={!!error}
                                helperText={error ? error.message : null}
                                label="Número de Guia *"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                onChange={e => {
                                    if(e.target.value.length <= 12){
                                        onChange(e);
                                    }
                                }}
                            />
                        )}
                    />
                </Grid>
          </Grid>
          <Grid container>
            <Grid className={classes.column} item xs={12} md={12}>
                <Controller
                    name="observacionesEscrituracion"
                    control={formulario.control}
                    render={({
                        field: { onChange, value },
                        fieldState: { error },
                    }) => (
                        <CHTextField
                            error={!!error}
                            helperText={error ? error.message : null}
                            label="Observaciones de Escrituración"
                            value={value}
                            onChange={onChange}
                            multiline
                            rows={2}
                            InputLabelProps={{ shrink: true }}
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
        </CHPaper>   
        <CHPaper title="Información del Beneficio Entregado">
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="valorCuentaIndividual"
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <>
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor Cuenta Individual *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={onChange}
                                />
                            </>
                        )}
                    />
                </Grid>   
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="valorCargoCajaHonor"
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <>
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor a Cargo de Caja Honor "
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={onChange}
                                />
                            </>
                        )}
                    />
                </Grid>  
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="valorTotalBeneficio"
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <>
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor Total del Beneficio * "
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={onChange}
                                />
                            </>
                        )}
                    />
                </Grid>  
            </Grid>    
        </CHPaper>
        <CHPaper title="Información legalización">
             <Grid container>
               <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="valorTotalLegislacion"
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <>
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor Total a Legalización * "
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={onChange}
                                />
                            </>
                        )}
                    />
                </Grid>  
            </Grid>  
        </CHPaper>
        <CHPaper title="Beneficiario de Pago">
            <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="tipoIdentificacionBeneficiario"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    value={value}
                                    onChange={onChange}
                                    label="Tipo Identificación *"
                                >
                                     {catalogState?.IdentificationType.map(
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
                        name="numeroIdentificacionBeneficiario"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                numberMask
                                error={!!error}
                                helperText={error ? error.message : null}
                                label="Número de Identificación *"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                onChange={e => {
                                    if(e.target.value.length <= 12){
                                        onChange(e);
                                    }
                                }}
                            />
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="nombreRazonSocial"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                label="Nombres y Apellidos *"
                                value={value}
                                InputLabelProps={{ shrink: true }}
                                inputProps={{
                                    style: {
                                        textTransform: 'uppercase',
                                    },
                                }}
                                onChange={e => {
                                    if(e.target.value.length <= 40){
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
                        name="tipoCuenta"
                        control={formulario.control}
                        rules={{
                            required:
                                'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                error={!!error}
                                label="Tipo de Cuenta *"
                                value={ value }
                                helperText={
                                    error ? error.message : null
                                }
                                onChange={onChange}
                            >
                            {tiposCuenta?.map(
                                (option: any) => ( 
                                    <MenuItem key={option.codigo} 
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
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="entidad"
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                error={!!error}
                                label="Entidad"
                                value={ value }
                                helperText={
                                    error ? error.message : null
                                }
                                onChange={onChange}
                            >
                            {entidadesBancarias?.map(
                                (option: any) => ( 
                                    <MenuItem key={option.codigoBanco} 
                                                value={option.codigoBanco}
                                    >
                                        {option.nombreRazonSocial}
                                    </MenuItem>
                                    ),
                            )} 
                            </CHSelect>
                        )}
                    />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroCuenta"
                            control={formulario.control}
                            rules={{
                                required:
                                    'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    label="Número de Cuenta *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                  </Grid>
                  <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="numeroComprobante"
                            control={formulario.control}
                            rules={{
                                required:
                                    'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    label="Número Comprobante *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                </Grid>
                <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="valorTotalPago"
                        control={formulario.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <>
                                <CHTextField
                                    currencyMask
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    label="Valor Total Pago * "
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    onChange={onChange}
                                />
                            </>
                        )}
                    />
                </Grid>  
            </Grid>
        </CHPaper>
        <CHPaper title="Observaciones">
          <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="observacion"
                            control={formulario.control}
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
                                    label="Observaciones Adicionales *"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={false}
                                    multiline
                                    rows={2}
                                    InputLabelProps={{ shrink: true }}
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
    formulario: UseFormReturn<any>;
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetAllDepartments: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllDepartments,
    startGetAllIdentificationTypes
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(AsignacionViviendaLegislacion);