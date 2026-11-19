import { Button, createStyles, 
         Grid, 
         IconButton, 
         makeStyles, 
         MenuItem,
         Paper,
         Table,
         TableBody,
         TableCell,
         TableContainer,
         TableHead,
         TableRow} from '@material-ui/core';
import { Delete, Edit, Search } from '@material-ui/icons';
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
import { AppState } from '../../../store/rootReducer';
import { IDENTIFICATIONTYPEDTO, VALORCATALOGO } from '../../../types/catalog.type';
import ApiCatalogo from '../../../services/api-catalog.service';
import CHCheckbox from '../../../components/ch-checkbox/CHCheckbox';
import { TerceroApoderadoPostulacion } from '../constants/fondosolidaridadform.model.type';
import GenerarGuid from '../../AdministracionCuentas/components/funciones/guid.funtions';
import NotificationService from '../../../services/notification.service';

const useStyles = makeStyles(() =>
	createStyles({
		columnButtonAdd: {
			textAlign: 'right',
		},
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
	}),
);


const InformacionApoderadoAutorizadoPostulacion = (props: Props) => {
    const classes = useStyles();
    const apiCatalogo = ApiCatalogo();
    const {formulario, 
           catalogState, 
           loginState, 
           setApoderadosAutorizados,
           startGetAllIdentificationTypes} = props;
    const [listaParentesco, setListaParentesco]= useState<VALORCATALOGO[]>([]);
    const [descEstadoBoton, setDescEstadoBoton] = useState<string>('AGREGAR APODERADO');
    const [terceroApoderadoModel, setTerceroApoderadoModel] = useState<TerceroApoderadoPostulacion[]>([]);
    const [apoderadoAnterior, setApoderadoAnterior] = useState<string>('');
    const [idAnterior, setIdAnterior] = useState<string>('');

    useEffect(() => {
      startGetAllIdentificationTypes(loginState.claims);
      consultarListaParentesco();
    },[])

    const consultarListaParentesco = async() => {
        const PARENTESCO = 3; 
        await apiCatalogo.getObtenerValoresCatalogosPorIdCatalogo(PARENTESCO)
        .then(async response => {
            if(response.data){
                setListaParentesco(response.data);
            }
        })
        .catch(() => {
			toast('Error del sistema', { type: toast.TYPE.ERROR });
		});
    } 

    const seleccionarRegistroTercero = async (item: TerceroApoderadoPostulacion) => {
        setApoderadoAnterior(item.numeroDocumento);
        setIdAnterior(item.numeroDocumento);
        formulario.setValue('id', item.id);
        formulario.setValue('tipoDocumentoId', item.tipoDocumentoId);
        formulario.setValue('numeroDocumento', String(item.numeroDocumento));
        formulario.setValue('parentesco', item.parentesco);
        formulario.setValue('esAutorizado', item.esAutorizado);
        formulario.setValue('primerNombre', item.primerNombre);
        formulario.setValue('segundoNombre', item.segundoNombre);
        formulario.setValue('primerApellido', item.primerApellido);
        formulario.setValue('segundoApellido', item.segundoApellido);
        setDescEstadoBoton('EDITAR APODERADO');
    }

    const agregarTerceroApoderado = async (data: any) => {
        const datoEncontrado = terceroApoderadoModel.findIndex(
            item =>
                item.numeroDocumento === data.numDocumento &&
                item.numeroDocumento !== apoderadoAnterior,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroApoderado(data);
        } else {
            toast('El apoderado ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
	}

    const agregarRegistroApoderado = (data: any) => {
        const guid = GenerarGuid();
        const apoderadoNuevo: TerceroApoderadoPostulacion = {
            id: guid.newGuid(),
            tipoDocumentoId: data.tipoDocumentoId,
            numeroDocumento: data.numeroDocumento,
            parentesco: data.parentesco,
            esAutorizado: data.esAutorizado,
            primerNombre: data.primerNombre.toUpperCase(),
            segundoNombre: data.segundoNombre.toUpperCase(),
            primerApellido: data.primerApellido.toUpperCase(),
            segundoApellido: data.segundoApellido.toUpperCase(),
        }
        let apoderados: TerceroApoderadoPostulacion[] = [];
        if (descEstadoBoton === 'AGREGAR APODERADO') {

            // if(matriculasModel.length === 3){
            //     toast('No se pueden agregar más de 3 matriculas ', {
            //         type: toast.TYPE.ERROR,
            //     });
            //     return;
            // }

            setTerceroApoderadoModel(oldArray => [
                ...oldArray,
                apoderadoNuevo,
            ]);
            apoderados = [...terceroApoderadoModel];
            apoderados.push(apoderadoNuevo);
        } else {
            const temp = [...terceroApoderadoModel];
            const posicionActual = temp.findIndex(
                item => item.numeroDocumento === apoderadoAnterior,
            );

            apoderadoNuevo.numeroDocumento = temp[posicionActual].numeroDocumento;

            if (
                apoderadoNuevo.numeroDocumento !==
                temp[posicionActual].numeroDocumento
            ) {
                temp[posicionActual].numeroDocumento =
                apoderadoNuevo.numeroDocumento;
            } else {
                apoderadoNuevo.numeroDocumento = idAnterior;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, apoderadoNuevo);
            apoderados = temp;
            setTerceroApoderadoModel(temp);
        }
        setApoderadosAutorizados(apoderados);
        setApoderadoAnterior('');
        resetApoderadoNuevo();
    }

    const resetApoderadoNuevo = async () => {
        setDescEstadoBoton('AGREGAR APODERADO');
        formulario.reset();
    }

    const eliminarRegistro = async (numeroDocumento: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...terceroApoderadoModel];
            temp.splice(
                temp.findIndex(item => item.numeroDocumento === numeroDocumento),
                1,
            );
            setTerceroApoderadoModel(temp);
            setApoderadosAutorizados(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
            resetApoderadoNuevo();
        }
    }

    const mostrarTipoIdentificacion = (numeroTipoDocumento: number) => {
        const tipoDocumento = catalogState.IdentificationType.find(item => item.id === numeroTipoDocumento);
        return tipoDocumento?.descripcion;
    }

    const mostrarParentesco= (idParentesco: number) => {
        const parentesco = listaParentesco.find(item => item.id === idParentesco);
        return parentesco?.descripcion;
    }

    return (
        <>
            <CHPaper title="Información del Apoderado Autorizado (Si Aplica)">
              <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="numeroDocumentoBuscar"
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
                                        label="Número de documento a buscar"
                                        value={value}
                                        InputLabelProps={{ shrink: true }}
                                        onChange={onChange}
                                    />
                                    <Button
                                        color="secondary"
                                        // onClick={() => {
                                        //     buscarMatricula(value);
                                        // }}
                                    >
                                        <Search />
                                    </Button>
                                </>
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="tipoDocumentoId"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formulario.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
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
                            control={formulario.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                  //  numberMask
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
                            name="parentesco"
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            control={formulario.control}
                            render={({ field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    label="Parentesco *"
                                    value={value}
                                    onChange={onChange}
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                >
                                    {listaParentesco.map(
                                        (option: VALORCATALOGO) => (
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
                            name="esAutorizado"
                            control={formulario.control}
                            render={({ field: { onChange, value } }) => (
                                <CHCheckbox
                                    label="Es Autorizado"
                                    onChange={onChange}
                                    checked={value}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="primerNombre"
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
                                    label="Primer Nombre *"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '').substring(0,25)
                                        
                                        );
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="segundoNombre"
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
                                    label="Segundo Nombre *"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '').substring(0,25)
                                        
                                        );
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="primerApellido"
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
                                    label="Primer Apellido *"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '').substring(0,25)
                                        
                                        );
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="segundoApellido"
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
                                    label="Segundo Apellido *"
                                    value={value}
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                    onChange={e => {
                                        onChange(
                                            e.target.value.replace(
                                                /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                '').substring(0,25)
                                        
                                        );
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
                                agregarTerceroApoderado,
                            )}
                        >
                            {descEstadoBoton}
                        </Button>

                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetApoderadoNuevo}
                        >
                           CANCELAR
                        </Button>
                    </Grid>
                </Grid> 
                <h3>Lista de Apoderados</h3>
				<TableContainer component={Paper}>
					<Grid className={classes.columnButtonAdd} item xs={12}md={12}>
						<Table aria-label="simple table">
							<TableHead>
								<TableRow>
									<TableCell>
										Tipo de Identificación
									</TableCell>
									<TableCell>
										Número de Identificación
									</TableCell>
									<TableCell>
										Nombres / Apellidos
									</TableCell>
									<TableCell>Parentesco</TableCell>
									<TableCell>Autorizado</TableCell>
									<TableCell align="center">
										Acciones
									</TableCell>
								</TableRow>
							</TableHead>
							<TableBody>
								{terceroApoderadoModel.map(
									(option: TerceroApoderadoPostulacion) => (
										<TableRow key={option.id}>
											<TableCell>
												{mostrarTipoIdentificacion(option.tipoDocumentoId)}
											</TableCell>
											<TableCell>
												{option.numeroDocumento}
											</TableCell>
											<TableCell>
												{`${option.primerNombre} ${option.segundoNombre} ${option.primerApellido} ${option.segundoApellido}`}
											</TableCell>
											<TableCell>
												{mostrarParentesco(option.parentesco)}
											</TableCell>
											<TableCell>
												{option.esAutorizado
													? 'SI'
													: 'NO'}
											</TableCell>
													<TableCell align="center">
														<IconButton
															aria-label="Editar"
															color="primary"
															onClick={() => {
																seleccionarRegistroTercero(
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
    setApoderadosAutorizados: (value: TerceroApoderadoPostulacion[]) => void;
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(InformacionApoderadoAutorizadoPostulacion);