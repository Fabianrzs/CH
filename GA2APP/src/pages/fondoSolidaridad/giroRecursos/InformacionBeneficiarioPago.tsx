import {Button, createStyles,  
        Grid,  
        IconButton,  
        ListItemText,  
        makeStyles, 
        MenuItem, 
        Paper, 
        Table, 
        TableBody, 
        TableCell, 
        TableContainer,
        TableHead,
        TableRow,
        Typography} from '@material-ui/core';
import { Delete, Edit } from '@material-ui/icons';
import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import ApiCatalogo from '../../../services/api-catalog.service';
import NotificationService from '../../../services/notification.service';
import { AppState } from '../../../store/rootReducer';
import UtilService from '../../../utils/util.service';
import GenerarGuid from '../../AdministracionCuentas/components/funciones/guid.funtions';
import { TerceroApoderadoPostulacion, TerceroBeneficiarioPostulacion } from '../constants/fondosolidaridadform.model.type';

const useStyles = makeStyles(() =>
	createStyles({
		column: {
			padding: '10px',
			textAlign: 'center',
		},
		buttonAdd: {
			width: '200px',
			marginLeft: '.5rem',
		},
		columnLabel: {
			padding: '10px',
		},
        columnButtonAdd: {
			textAlign: 'right',
		},
        inline: {
			display: 'inline',
		}
	}),
);

const InformacionBeneficiarioPagoPostulacion = (props: Props) => {
    const classes = useStyles();
    const apiCatalogo = ApiCatalogo();
    const {formulario, 
           catalogState, 
           loginState,
           apoderadosAutorizados, 
           setBeneficiariosDePago,
           startGetAllIdentificationTypes} = props;
    const [tiposCuenta,setTiposCuenta] = useState<any []>([]);
    const [entidadesBancarias,setEntidadesBancarias] = useState<any []>([]);
    const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR BENEFICIARIO');
    const [terceroBeneficiarioModel, setTerceroBeneficiarioModel] = useState<TerceroBeneficiarioPostulacion[]>([]);
    const [beneficiarioAnterior, setBeneficiarioAnterior] = useState<string>('');
    const [idAnterior, setIdAnterior] = useState<string>('');

    useEffect(()=>{
        consultarTiposCuenta();
        consultarEntidadesBancarias();
        startGetAllIdentificationTypes(loginState.claims);
    },[])

    const consultarTiposCuenta = async() => {
        await apiCatalogo.getObtenerValoresCatalogosPorIdCatalogo(67)
             .then(response => {
                 if(response.data){
                     setTiposCuenta(response.data);
                 }
             }).catch(() => {
                 toast(`Error del sistema`, {type: toast.TYPE.ERROR});
             })
     }

    const consultarEntidadesBancarias = async() => {
        await apiCatalogo.getAllEntidadBancaria()
             .then(response => {
                 if(response.data){
                     setEntidadesBancarias(response.data);
                 }
             }).catch(() => {
                 toast(`Error del sistema`, {type: toast.TYPE.ERROR});
             })
    }

    const seleccionarRegistro = async (item: TerceroBeneficiarioPostulacion) => {
        setBeneficiarioAnterior(item.numeroDocumento);
        setIdAnterior(item.numeroDocumento);
        formulario.setValue('id', item.id);
        formulario.setValue('beneficiario', item.beneficiario);
        formulario.setValue('numeroDocumento', String(item.numeroDocumento));
        formulario.setValue('nombre', item.nombre);
        formulario.setValue('tipoCuenta', item.tipoCuenta);
        formulario.setValue('entidad', item.entidad);
        formulario.setValue('numeroCuenta', item.numeroCuenta);
        formulario.setValue('confirmacionNumeroCuenta', item.confirmacionNumeroCuenta);
        formulario.setValue('valorRetirar', item.valorRetirar);
        setDescEstadoBoton('EDITAR BENEFICIARIO');
    }

    const agregarTerceroBeneficiario = async (data: any) => {
        formulario.setValue('numeroDocumento', '123');
        formulario.setValue('nombre', 'nombre');
        const datoEncontrado = terceroBeneficiarioModel.findIndex(
            item =>
                item.numeroDocumento === data.numeroDocumento &&
                item.numeroDocumento !== beneficiarioAnterior,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroBeneficiario(data);
        } else {
            toast('El beneficiario ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
	}

    const agregarRegistroBeneficiario = (data: any) => {
        const guid = GenerarGuid();
        const beneficiarioNuevo: TerceroBeneficiarioPostulacion = {
            id: guid.newGuid(),
            beneficiario: data.beneficiario,
            tipoCuenta: Number(data.tipoCuenta),
            entidad: data.entidad,
            numeroCuenta: Number(data.numeroCuenta),
            confirmacionNumeroCuenta: Number(data.confirmacionNumeroCuenta),
            valorRetirar: UtilService.cleanCurrencyValue(data.valorRetirar),
            numeroDocumento: data.numeroDocumento,
            nombre: data.nombre
        }
        let beneficiarios: TerceroBeneficiarioPostulacion[] = [];
        if (descEstadoBoton === 'AGREGAR BENEFICIARIO') {

            // if(matriculasModel.length === 3){
            //     toast('No se pueden agregar más de 3 matriculas ', {
            //         type: toast.TYPE.ERROR,
            //     });
            //     return;
            // }

            setTerceroBeneficiarioModel(oldArray => [
                ...oldArray,
                beneficiarioNuevo,
            ]);
            beneficiarios = [...terceroBeneficiarioModel];
            beneficiarios.push(beneficiarioNuevo);
        } else {
            const temp = [...terceroBeneficiarioModel];
            const posicionActual = temp.findIndex(
                item => item.numeroDocumento === beneficiarioAnterior,
            );

            beneficiarioNuevo.numeroDocumento = temp[posicionActual].numeroDocumento;

            if (
                beneficiarioNuevo.numeroDocumento !==
                temp[posicionActual].numeroDocumento
            ) {
                temp[posicionActual].numeroDocumento =
                beneficiarioNuevo.numeroDocumento;
            } else {
                beneficiarioNuevo.numeroDocumento = idAnterior;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, beneficiarioNuevo);
            beneficiarios = temp;
            setTerceroBeneficiarioModel(temp);
        }
        setBeneficiariosDePago(beneficiarios);
        setBeneficiarioAnterior('');
        resetBeneficiarioNuevo();
    }

    const resetBeneficiarioNuevo = async () => {
        setDescEstadoBoton('AGREGAR BENEFICIARIO');
        formulario.reset();
    }

    const eliminarRegistro = async (numeroDocumento: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...terceroBeneficiarioModel];
            temp.splice(
                temp.findIndex(item => item.numeroDocumento === numeroDocumento),
                1,
            );
            setTerceroBeneficiarioModel(temp);
            setBeneficiariosDePago(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
            resetBeneficiarioNuevo();
        }
    }

    const mostrarTipoCuenta = (idTipoCuenta: string) => {
        const tipoCuenta = tiposCuenta.find(item => item.codigo === idTipoCuenta);
        return tipoCuenta?.descripcion;
    }

    const mostrarEntidad = (idEntidad: string) => {
        const entidad = entidadesBancarias.find(item => item.idEntidadBancaria === idEntidad);
        return entidad?.nombreRazonSocial;
    }

    const mostrarTipoIdentificacion = (numeroTipoDocumento: number) => {
        const tipoDocumento = catalogState.IdentificationType.find(item => item.id === numeroTipoDocumento);
        return tipoDocumento?.descripcion;
    }

    return(
        <>
             <CHPaper title="Información Beneficiario de Pago Nuevo">
             <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="beneficiario"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formulario.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Beneficiario *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                >
                                    {apoderadosAutorizados?.map(
                                            (option: any) => ( 
                                                <MenuItem key={option.numeroDocumento} 
                                                          value={`${option.tipoDocumentoId} - ${option.numeroDocumento} - ${option.primerNombre} ${option.segundoNombre} ${option.primerApellido} ${option.segundoApellido}`}
                                                >
                                                    <ListItemText
                                                            primary= {`${option.numeroDocumento} - ${option.primerNombre} ${option.segundoNombre} ${option.primerApellido} ${option.segundoApellido} ${option.esAutorizado? 'Autorizado' : 'Principal'}`}
                                                            secondary={
                                                                <React.Fragment
                                                                    key={`fragmento-${option.numeroDocumento}`}
                                                                >
                                                                    <Typography
                                                                        component="span"
                                                                        variant="body2"
                                                                        className={
                                                                            classes.inline
                                                                        }
                                                                        color="textPrimary"
                                                                    >
                                                                        {
                                                                            option.tipoDocumentoId
                                                                        }
                                                                    </Typography>
                                                                    {` — ${option.numeroDocumento} - ${option.primerNombre} ${option.segundoNombre} ${option.primerApellido} ${option.segundoApellido}`}
                                                                </React.Fragment>
                                                            }
                                                        />
                                                </MenuItem>
                                            ),
                                    )} 
                                </CHSelect>
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
                                        <MenuItem key={option.idEntidadBancaria} 
                                                    value={option.idEntidadBancaria}
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
                            name="confirmacionNumeroCuenta"
                            control={formulario.control}
                            rules={{
                                required:
                                    'Este campo es requerido.',
                                validate: () => formulario.watch('numeroCuenta') !== formulario.watch('confirmacionNumeroCuenta')? 'el valor debe ser igual al Número de Cuenta' : true
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    numberMask
                                    label="Confirmación No. de Cuenta *"
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
                            name="valorRetirar"
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
                                    currencyMask
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Valor Retirar *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={formulario.handleSubmit(
                                agregarTerceroBeneficiario,
                            )}
                        >
                            {descEstadoBoton}
                        </Button>
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetBeneficiarioNuevo}
                        >
                            CANCELAR
                        </Button>
                    </Grid>
                </Grid> 
                <h3>Lista de Beneficiarios</h3>
				<TableContainer component={Paper}>
					<Grid className={classes.columnButtonAdd} item xs={12} md={12}>
						<Table aria-label="simple table">
							<TableHead>
								<TableRow>
									<TableCell>Tipo de Identificación</TableCell>
									<TableCell>Número de Identificación</TableCell>
									<TableCell>Nombre / Razón Social</TableCell>
									<TableCell>Tipo de Cuenta</TableCell>
									<TableCell>Entidad</TableCell>
									<TableCell>Número de Cuenta</TableCell> 
									<TableCell>Valor a retirar</TableCell>
									<TableCell align="center">
										Acciones
									</TableCell>
								</TableRow>
							</TableHead>
							<TableBody>
								{terceroBeneficiarioModel.map(
									(option: TerceroBeneficiarioPostulacion) => (
										<TableRow key={option.id}>
											<TableCell>
												{mostrarTipoIdentificacion(Number(option.beneficiario.split('-')[0]))}
											</TableCell>
											<TableCell>
												{option.beneficiario.split('-')[1]}
											</TableCell>
                                            <TableCell>
                                                {option.beneficiario.split('-')[2]}
                                            </TableCell>
											<TableCell>
												{mostrarTipoCuenta(String(option.tipoCuenta))}
											</TableCell>
											<TableCell>
												{mostrarEntidad(option.entidad)}
											</TableCell>
											<TableCell>
												{option.numeroCuenta}
											</TableCell>
											<TableCell>
												{UtilService.formatCurrency(
													Number(
														option.valorRetirar,
													),
												)}
											</TableCell>
													<TableCell align="center">
														<IconButton
															aria-label="Editar"
															color="primary"
															onClick={() => {
																seleccionarRegistro(
																	option,
																);
															}}
														>
															<Edit />
														</IconButton>
														<IconButton
															aria-label="Eliminar"
															color="secondary"
															onClick={() => {
																eliminarRegistro(
																	option.numeroDocumento,
																);
															}}
														>
															<Delete />
														</IconButton>
													</TableCell>
										</TableRow>
									),
								)}
							</TableBody>
						</Table>
					</Grid>
				</TableContainer>
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
    formulario:  UseFormReturn<any>;
    apoderadosAutorizados: TerceroApoderadoPostulacion[];
    setBeneficiariosDePago: (value: TerceroBeneficiarioPostulacion[]) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(InformacionBeneficiarioPagoPostulacion);