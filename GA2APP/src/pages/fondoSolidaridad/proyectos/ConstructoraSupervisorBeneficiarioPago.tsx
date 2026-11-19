import {createStyles, makeStyles, Grid, MenuItem, Button, Typography, Table, TableHead, TableCell, TableRow, TableBody, IconButton } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { Delete, Edit, Visibility} from '@material-ui/icons';
import { startGetAllDepartments, startGetAllIdentificationTypes} from '../../../actions/catalog.action';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import ApiCatalog from '../../../services/api-catalog.service';
import { BeneficiarioPago, Supervisor } from '../constants/fondosolidaridadform.model.type';
import NotificationService from '../../../services/notification.service';
import AddressComponent from '../../../components/modals/address/address.component';
import { setFormDataAddres } from '../../../components/modals/address/constants/address.components.constants';
import { Address } from '../../../types/address.type';
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
        buttonAddEdit: {
            width: '300px',
            marginLeft: '.5rem',
        },
    }),
);

const ConstructoraSupervisorBeneficiarioPago = (props: Props) => {

    const classes = useStyles();
    const apiCatalog = ApiCatalog();

    const {loginState, 
           catalogState, 
           estadoProyecto,
           informacionProyecto,
           informacionSupervisores,
           informacionBeneficiarios,
           startGetAllDepartments, 
           startGetAllIdentificationTypes,
           setSupervisores,
           setBeneficiarios
          } = props;

    // Supervisor
    const [estadoBotonSupervisor, setEstadoBotonSupervisor] = useState<string>('AGREGAR SUPERVISOR');
    const [supervisorAnterior, setSupervisorAnterior] = useState<number>(0);
    const [supervisorModel, setSupervisorModel] = useState<Supervisor[]>([]);
    const [idAnteriorSupervisor, setIdAnteriorSupervisor] = useState<number>(0);

    // Beneficiario de Pago
    const [estadoBotonBeneficiario, setEstadoBotonBeneficiario] = useState<string>('AGREGAR BENEFICIARIO DE PAGO');
    const [BeneficiarioAnterior, setBeneficiarioAnterior] = useState<number>(0);
    const [beneficiarioModel, setBeneficiarioModel] = useState<BeneficiarioPago[]>([]);
    const [idAnteriorBeneficiario, setIdAnteriorBeneficiario] = useState<number>(0);
    const [entidadesBancarias,setEntidadesBancarias] = useState<any []>([]);
    const [tiposCuenta,setTiposCuenta] = useState<any []>([]);
    const [mostrarCamposCuenta,setMostrarCamposCuenta] = useState(false);
    const [mostrarDireccionCheque,setMostrarDireccionCheque] = useState(false);
    const [openDireccionBeneficiario, setOpenDireccionBeneficiario] = useState(false);

    useEffect(() => {
        startGetAllDepartments(loginState.claims);
        startGetAllIdentificationTypes(loginState.claims);
        consultarEntidadesBancarias();
        consultarTiposCuenta();
        if(informacionSupervisores.length > 0  && informacionBeneficiarios.length > 0){
            obtenerListasSupervisoresYBeneficiarios(informacionSupervisores, informacionBeneficiarios);
        }
    },[])

    const formularioSupervisor = useForm({
        defaultValues: {
            tipoDocumento: '',
            numeroDocumento: '',
            nombresApellidos: '',
            cargo: '',
            email: '',
            telefono: '',
        },
    });

    const formularioBeneficiariosPago = useForm({
        defaultValues: {
            tipoDocumento: '',
            numeroDocumento: '',
            digitoVerificacion: '',
            nombreRazonSocial: '',
            entidad: '',
            tipoCuenta: '',
            numeroCuenta: '',
            direccion: '',
            actividadEconomica: '',
        },
    });

    const obtenerListasSupervisoresYBeneficiarios = (supervisores: any[], beneficiarios: any[]) =>{

        const arraySupervisores =  supervisores.map(item => {
              if(item){
                  const supervisor: Supervisor = {
                      codigo: item.codigo,
                      tipoDocumento: item.tipoDocumentoId || item.tipoDocumento,
                      numeroDocumento: item.numeroDocumento,
                      nombresApellidos: item.nombreCompleto || item.nombresApellidos,
                      cargo: item.cargo,
                      email: item.email,
                      telefono: Number(item.telefono),
                  }
                  return supervisor;
              }
              return null;
          })
  
        if(arraySupervisores.length > 0){
            setSupervisorModel(arraySupervisores as Supervisor[]);
        }
        
        const arrayBeneficiarios =  beneficiarios.map(item => {
          if(item){
              const beneficiario: BeneficiarioPago = {
                  codigo: item.codigo,
                  tipoDocumento: 1,
                  numeroDocumento: item.numerDocumento || item.numeroDocumento,
                  digitoVerificacion: item.digitoVerificacion,
                  nombreRazonSocial: item.razonSocial || item.nombreRazonSocial,
                  entidad: item.tipoEntidaId,
                  tipoCuenta: item.tipoCuentaId || item.tipoCuenta,
                  numeroCuenta: item.numeroCuenta,
                  direccion: item.direccion,
                  actividadEconomica: item.actividadCuenta,
              }
              return beneficiario;
          }
          return null;
        })
    
        if(arrayBeneficiarios.length > 0){
            setBeneficiarioModel(arrayBeneficiarios as BeneficiarioPago[]);
        }
      }

    const handleOpenAddressBeneficiarioPago = () => {
        if((estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
            estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)){
            setOpenDireccionBeneficiario(false);
        }else{
            setOpenDireccionBeneficiario(true);
        }
    };

    const onCloseAddressBeneficiarioPago = (value: Address) => {
        if (value !== null) {
            const result = setFormDataAddres(value);
            formularioBeneficiariosPago.setValue('direccion', result);
            setOpenDireccionBeneficiario(false);
        } else {
            formularioBeneficiariosPago.setValue('direccion', '');
            setOpenDireccionBeneficiario(false);
        }
    };

    const agregarSupervisor = async (data: any) => {
        const datoEncontrado = supervisorModel.findIndex(
            item =>
                item.numeroDocumento === data.numeroDocumento &&
                item.numeroDocumento !== supervisorAnterior,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroSupervisor(data);
        } else {
            toast('El supervisor ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const agregarRegistroSupervisor = async (data: any) => {
        const supervisorNuevo: Supervisor = {
            tipoDocumento: Number(data.tipoDocumento),
            numeroDocumento: data.numeroDocumento,
            nombresApellidos: String(data.nombresApellidos).toLocaleUpperCase(),
            cargo: String(data.cargo).toLocaleUpperCase(),
            email: data.email,
            telefono: data.telefono,
        };
        let supervisores: Supervisor[] = [];
        if (estadoBotonSupervisor === 'AGREGAR SUPERVISOR') {

            setSupervisorModel(oldArray => [
                ...oldArray,
                supervisorNuevo,
            ]);
            supervisores = [...supervisorModel];
            supervisores.push(supervisorNuevo);
        } else {
            const temp = [...supervisorModel];
            const posicionActual = temp.findIndex(
                item => item.numeroDocumento === supervisorAnterior,
            );

            supervisorNuevo.numeroDocumento = temp[posicionActual].numeroDocumento;

            if (
                Number(supervisorNuevo.numeroDocumento) !==
                temp[posicionActual].numeroDocumento
            ) {
                temp[posicionActual].numeroDocumento =
                    supervisorNuevo.numeroDocumento;
            } else {
                supervisorNuevo.numeroDocumento = idAnteriorSupervisor;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, supervisorNuevo);
            supervisores = temp;
            setSupervisorModel(temp);
        }
        setSupervisores(supervisores);
        setSupervisorAnterior(0);
        resetSupervisorNuevo();
    };

    const seleccionarRegistroSupervisor = async (item: Supervisor) => {
        setSupervisorAnterior(item.numeroDocumento);
        setIdAnteriorSupervisor(item.numeroDocumento);
        formularioSupervisor.setValue('tipoDocumento', String(item.tipoDocumento));
        formularioSupervisor.setValue('numeroDocumento', String(item.numeroDocumento));
        formularioSupervisor.setValue('nombresApellidos', item.nombresApellidos);
        formularioSupervisor.setValue('cargo', String(item.cargo));
        formularioSupervisor.setValue('email', item.email);
        formularioSupervisor.setValue('telefono', String(item.telefono));
        setEstadoBotonSupervisor('EDITAR SUPERVISOR');
    };

    const eliminarRegistroSupervisor = async (numero: number) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...supervisorModel];
            temp.splice(
                temp.findIndex(item => item.numeroDocumento === numero),
                1,
            );
            setSupervisorModel(temp);
            setSupervisores(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
           resetSupervisorNuevo();
        }
    };

    const resetSupervisorNuevo = async () => {
        setEstadoBotonSupervisor('AGREGAR SUPERVISOR');
        formularioSupervisor.clearErrors();
        formularioSupervisor.setValue('tipoDocumento', '');
        formularioSupervisor.setValue('numeroDocumento', '');
        formularioSupervisor.setValue('nombresApellidos', '');
        formularioSupervisor.setValue('cargo', '');
        formularioSupervisor.setValue('email', '');
        formularioSupervisor.setValue('telefono', '');
    };

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

    const agregarBeneficiario = async (data: any) => {
        const datoEncontrado = beneficiarioModel.findIndex(
            item =>
                item.numeroDocumento === data.numeroDocumento &&
                item.numeroDocumento !== BeneficiarioAnterior,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroBeneficiario(data);
        } else {
            toast('El beneficiario ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const agregarRegistroBeneficiario = async (data: any) => {

        const beneficiarioNuevo: BeneficiarioPago = {
            tipoDocumento: Number(data.tipoDocumento),
            numeroDocumento: data.numeroDocumento,
            digitoVerificacion: data.digitoVerificacion,
            nombreRazonSocial: String(data.nombreRazonSocial).toLocaleUpperCase(),
            entidad: data.entidad,
            tipoCuenta: data.tipoCuenta,
            numeroCuenta: data.numeroCuenta,
            direccion: data.direccion,
            actividadEconomica: String(data.actividadEconomica).toLocaleUpperCase(),
        };
        let beneficiarios: BeneficiarioPago[] = [];
        if (estadoBotonBeneficiario === 'AGREGAR BENEFICIARIO DE PAGO') {

            setBeneficiarioModel(oldArray => [
                ...oldArray,
                beneficiarioNuevo,
            ]);
            beneficiarios = [...beneficiarioModel];
            beneficiarios.push(beneficiarioNuevo);
        } else {
            const temp = [...beneficiarioModel];
            const posicionActual = temp.findIndex(
                item => item.numeroDocumento === BeneficiarioAnterior,
            );

            beneficiarioNuevo.numeroDocumento = temp[posicionActual].numeroDocumento;

            if (
                Number(beneficiarioNuevo.numeroDocumento) !==
                temp[posicionActual].numeroDocumento
            ) {
                temp[posicionActual].numeroDocumento =
                beneficiarioNuevo.numeroDocumento;
            } else {
                beneficiarioNuevo.numeroDocumento = idAnteriorBeneficiario;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, beneficiarioNuevo);
            beneficiarios = temp;
            setBeneficiarioModel(temp);
        }
        setBeneficiarios(beneficiarios);
        setBeneficiarioAnterior(0);
        resetBeneficiarioNuevo();
    };

    const seleccionarRegistroBeneficiario = async (item: BeneficiarioPago) => {
        const CHEQUE = '3'
        if(String(item.tipoCuenta) === CHEQUE){
            setMostrarDireccionCheque(true);
            setMostrarCamposCuenta(false);
        }else{
            setMostrarDireccionCheque(false);
            setMostrarCamposCuenta(true);
        }
        setBeneficiarioAnterior(item.numeroDocumento);
        setIdAnteriorBeneficiario(item.numeroDocumento);
        formularioBeneficiariosPago.setValue('tipoDocumento', String(item.tipoDocumento));
        formularioBeneficiariosPago.setValue('numeroDocumento', String(item.numeroDocumento));
        formularioBeneficiariosPago.setValue('digitoVerificacion', String(item.digitoVerificacion));
        formularioBeneficiariosPago.setValue('nombreRazonSocial', String(item.nombreRazonSocial));
        formularioBeneficiariosPago.setValue('entidad', item.entidad);
        formularioBeneficiariosPago.setValue('tipoCuenta', String(item.tipoCuenta));
        formularioBeneficiariosPago.setValue('numeroCuenta', String(item.numeroCuenta));
        formularioBeneficiariosPago.setValue('actividadEconomica', String(item.actividadEconomica));
        formularioBeneficiariosPago.setValue('direccion', String(item.direccion))
        setEstadoBotonBeneficiario('EDITAR BENEFICIARIO DE PAGO');
    };

    const eliminarRegistroBeneficiario = async (numero: number) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...beneficiarioModel];
            temp.splice(
                temp.findIndex(item => item.numeroDocumento === numero),
                1,
            );
            setBeneficiarioModel(temp);
            setBeneficiarios(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
           resetBeneficiarioNuevo();
        }
    };

    const resetBeneficiarioNuevo = async () => {
        setEstadoBotonBeneficiario('AGREGAR BENEFICIARIO DE PAGO');
        formularioBeneficiariosPago.clearErrors();
        formularioBeneficiariosPago.setValue('tipoDocumento', '');
        formularioBeneficiariosPago.setValue('numeroDocumento', '');
        formularioBeneficiariosPago.setValue('digitoVerificacion', '');
        formularioBeneficiariosPago.setValue('nombreRazonSocial', '');
        formularioBeneficiariosPago.setValue('entidad', '');
        formularioBeneficiariosPago.setValue('tipoCuenta', '');
        formularioBeneficiariosPago.setValue('numeroCuenta', '');
        formularioBeneficiariosPago.setValue('actividadEconomica', '');
        formularioBeneficiariosPago.setValue('direccion', '');
    };

    const mostrarTipoIdentificacion = (numeroTipoDocumento: number) => {
        const tipoDocumento = catalogState.IdentificationType.find(item => item.id === numeroTipoDocumento);
        return tipoDocumento?.descripcion;
    }

    const mostrarEntidad = (id: string) => {
        const entidad = entidadesBancarias.find(item => item.idEntidadBancaria === id);
        return entidad?.nombreRazonSocial;
    }

    const mostrarTipoCuenta = (codigo: string) => {
        const cuenta = tiposCuenta.find(item => item.codigo === codigo);
        return cuenta?.descripcion;
    }

    return (
        <>
            <AddressComponent
                onOpen={openDireccionBeneficiario}
                onClose={onCloseAddressBeneficiarioPago}
                control={formularioBeneficiariosPago.control}
                getValues={formularioBeneficiariosPago.watch}
                tipoDireccion
                setValue={formularioBeneficiariosPago.setValue}
            />
            <CHPaper title="Información Supervisores">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="tipoDocumento"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioSupervisor.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Tipo documento *"
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
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
                            name="numeroDocumento"
                            rules={{
                                required: 'Este campo es requerido.',
                                maxLength: {
                                    value: 12,
                                    message:
                                        'La longitud máxima del campo es de 12 digito',
                                },
                            }}
                            control={formularioSupervisor.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Documento *"
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
                            name="nombresApellidos"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioSupervisor.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
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
                            name="cargo"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioSupervisor.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Cargo *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '')
                                        );
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="email"
                            control={formularioSupervisor.control}
                            rules={{
                                pattern:
                                    /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/g,
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    InputLabelProps={{ shrink: true }}
                                    label="Email *"
                                    error={!!error}
                                    helperText={
                                        error
                                            ? 'Ingrese un correo electrónico válido'
                                            : null
                                    }
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="telefono"
                            control={formularioSupervisor.control}
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
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    InputLabelProps={{ shrink: true }}
                                    label="Teléfono *"
                                    type="number"
                                    error={!!error}
                                    helperText={
                                        error
                                            ? 'El teléfono debe empezar por 3 si es celular o 6 si es fijo y tener una longitud de 10 dígitos'
                                            : null
                                    }
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
                </Grid>
                <Grid container>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        {(estadoProyecto === ESTADO_PROYECTO.EN_CREACION || estadoProyecto === 0)&& 
                         (informacionProyecto === null) && (
                           <Button
                               className={classes.buttonAddEdit}
                               variant="outlined"
                               style={{ marginRight: '10px' }}
                               onClick={formularioSupervisor.handleSubmit(agregarSupervisor)}
                           >
                               {estadoBotonSupervisor}
                           </Button>
                        )}
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetSupervisorNuevo}
                        >
                            CANCELAR
                        </Button>
                    </Grid>
                </Grid>
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Lista de Supervisores
                </Typography>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Tipo de Identificación</TableCell>
                            <TableCell>Número de Identificación</TableCell>
                            <TableCell>Nombre y Apellidos</TableCell>
                            <TableCell>Cargo</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Teléfono</TableCell>
                            <TableCell align="center">
                                Acciones
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {supervisorModel.map(
                            (option: Supervisor) => (
                                <TableRow key={option.numeroDocumento}>
                                    <TableCell>{mostrarTipoIdentificacion(option.tipoDocumento)}</TableCell>
                                    <TableCell>{option.numeroDocumento}</TableCell>
                                    <TableCell>{option.nombresApellidos}</TableCell>
                                    <TableCell>{option.cargo}</TableCell>
                                    <TableCell>{option.email}</TableCell>
                                    <TableCell>{option.telefono}</TableCell>
                                    <TableCell align="center" style={{ width: '200px' }}>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            size="small"
                                            onClick={() => {
                                                seleccionarRegistroSupervisor(
                                                    option,
                                                );
                                            }}
                                        >
                                            {
                                                ((estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                                  estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)) && (
                                                    <Visibility />
                                                )
                                            }
                                            {   (estadoProyecto === ESTADO_PROYECTO.EN_CREACION || estadoProyecto === 0) && 
                                                (informacionProyecto === null) && ( 
                                                    <Edit />
                                            )}
                                        </IconButton>

                                    {(estadoProyecto === ESTADO_PROYECTO.EN_CREACION || estadoProyecto === 0) && 
                                     (informacionProyecto === null) && (
                                        <IconButton
                                               aria-label="Eliminar"
                                               color="primary"
                                               size="small"
                                               onClick={() => {
                                                   eliminarRegistroSupervisor(
                                                       option.numeroDocumento,
                                                   );
                                               }}
                                           >
                                              <Delete />
                                        </IconButton>
                                        )}
                                    </TableCell>
                                </TableRow>
                            )
                        )}
                    </TableBody>
                </Table>
            </CHPaper>
            <CHPaper title="Información Beneficiario de Pago">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="tipoDocumento"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioBeneficiariosPago.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    label="Tipo documento *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
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
                            name="numeroDocumento"
                            rules={{
                                required: 'Este campo es requerido.',
                                maxLength: {
                                    value: 12,
                                    message:
                                        'La longitud máxima del campo es de 12 digito',
                                },
                            }}
                            control={formularioBeneficiariosPago.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Número Documento *"
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
                            name="digitoVerificacion"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioBeneficiariosPago.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Digito de Verificación *"
                                    value={value}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
                                        if(e.target.value.length < 2){
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
                            name="nombreRazonSocial"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioBeneficiariosPago.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Nombre / Razón Social *"
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
                            control={formularioBeneficiariosPago.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    label="Tipo de Cuenta *"
                                    value={ value }
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={e => {
                                        const CHEQUE = '3';
                                        if(e.target.value === CHEQUE){
                                            setMostrarCamposCuenta(false);
                                            setMostrarDireccionCheque(true);
                                            formularioBeneficiariosPago.setValue('entidad', '');
                                            formularioBeneficiariosPago.setValue('numeroCuenta', '');
                                        }else{
                                            setMostrarCamposCuenta(true);
                                            setMostrarDireccionCheque(false);
                                            formularioBeneficiariosPago.setValue('direccion', '');
                                        }
                                        onChange(e);
                                    }}
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
                    {mostrarCamposCuenta && (
                        <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                                name="entidad"
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                control={formularioBeneficiariosPago.control}
                                render={({ field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHSelect
                                        readOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                        label="Entidad *"
                                        value={value}
                                        onChange={onChange}
                                        error={!!error}
                                        helperText={
                                            error ? error.message : null
                                        }
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
                    )}
                    {mostrarCamposCuenta && (
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="numeroCuenta"
                            control={formularioBeneficiariosPago.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto) && estadoProyecto !== 0}
                                    label="Número de Cuenta *"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value
                                                .length <= 11
                                        ) {
                                            onChange(
                                                e.target.value.replace(
                                                    /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W]/g,
                                                    '',
                                                ),
                                            );
                                        }
                                        }
                                    }
                                />
                            )}
                        />
                    </Grid>
                    )}
                    {mostrarDireccionCheque && (
                         <Grid className={classes.column} item xs={12} md={3}>
                         <Controller
                             name="direccion"
                             control={formularioBeneficiariosPago.control}
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
                                     label="Dirección *"
                                     value={value}
                                     InputLabelProps={{ shrink: true }}
                                     inputProps={{
                                         style: {
                                             textTransform: 'uppercase',
                                         },
                                     }}
                                     onClick={handleOpenAddressBeneficiarioPago}
                                     onChange={onChange}
                                 />
                             )}
                         />
                     </Grid>
                    )}
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="actividadEconomica"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formularioBeneficiariosPago.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly={(estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                              estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)}
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Actividad Económica *"
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
                </Grid>
                <Grid container>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        {(estadoProyecto === ESTADO_PROYECTO.EN_CREACION || estadoProyecto === 0) && 
                         (informacionProyecto === null) && (
                             <Button
                             variant="outlined"
                             className={classes.buttonAddEdit}
                             onClick={formularioBeneficiariosPago.handleSubmit(agregarBeneficiario)}
                            >
                             {estadoBotonBeneficiario}
                            </Button>
                        )}
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetBeneficiarioNuevo}
                        >
                            CANCELAR
                        </Button>
                    </Grid>
                </Grid>
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Lista de Beneficiarios de Pago
                </Typography>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Tipo de Identificación</TableCell>
                            <TableCell>Número de Identificación</TableCell>
                            <TableCell>Digito de Verificación</TableCell>
                            <TableCell>Nombre/Razón Social</TableCell>
                            <TableCell>Tipo Cuenta</TableCell>
                            <TableCell>Entidad</TableCell>
                            <TableCell>Número Cuenta</TableCell>
                            <TableCell>Dirección</TableCell>
                            <TableCell align="center">
                                Acciones
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {beneficiarioModel.map(
                            (option: BeneficiarioPago) => (
                                <TableRow key={option.numeroDocumento}>
                                    <TableCell>{mostrarTipoIdentificacion(option.tipoDocumento)}</TableCell>
                                    <TableCell>{option.numeroDocumento}</TableCell>
                                    <TableCell>{option.digitoVerificacion}</TableCell>
                                    <TableCell>{option.nombreRazonSocial}</TableCell>
                                    <TableCell>{mostrarTipoCuenta(String(option.tipoCuenta))}</TableCell>
                                    <TableCell>{mostrarEntidad(option.entidad) || 'N/A'}</TableCell>
                                    <TableCell>{option.numeroCuenta || 'N/A'}</TableCell>
                                    <TableCell>{option.direccion || 'N/A'}</TableCell>
                                    <TableCell align="center" style={{ width: '200px' }}>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            size="small"
                                            onClick={() => {
                                                seleccionarRegistroBeneficiario(
                                                    option,
                                                );
                                            }}
                                        >
                                            {
                                                ((estadoProyecto !== ESTADO_PROYECTO.EN_CREACION && 
                                                  estadoProyecto !== 0) || (informacionProyecto && informacionProyecto?.nombreProyecto)) && (
                                                    <Visibility />
                                                )
                                            }
                                            {   (estadoProyecto === ESTADO_PROYECTO.EN_CREACION || estadoProyecto === 0) && 
                                                (informacionProyecto === null) && ( 
                                                    <Edit />
                                            )}
                                        </IconButton>
                                        {(estadoProyecto === ESTADO_PROYECTO.EN_CREACION || estadoProyecto === 0) && 
                                         (informacionProyecto === null) &&(
                                        <IconButton
                                               aria-label="Eliminar"
                                               color="primary"
                                               size="small"
                                               onClick={() => {
                                                   eliminarRegistroBeneficiario(
                                                       option.numeroDocumento,
                                                   );
                                               }}
                                           >
                                              <Delete />
                                        </IconButton>
                                        )}
                                    </TableCell>
                                </TableRow>
                            )
                        )}
                    </TableBody>
                  </Table>
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
    informacionSupervisores: any[];
    informacionBeneficiarios: any[];
    estadoProyecto: number;
    setSupervisores: (value: Supervisor[]) => void;
    setBeneficiarios: (value: BeneficiarioPago[]) => void;
    startGetAllDepartments: (claims: string) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllDepartments,
    startGetAllIdentificationTypes
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ConstructoraSupervisorBeneficiarioPago);