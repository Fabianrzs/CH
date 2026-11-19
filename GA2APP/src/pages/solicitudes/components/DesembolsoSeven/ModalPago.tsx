import React, { useEffect, useState } from 'react';
import {
    Button,
    Card,
    CardContent,
    createStyles,
    FormControl,
    Grid,
    InputLabel,
    makeStyles,
    MenuItem,
    Modal,
    Paper,
    Select,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Typography,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import UtilService from '../../../../utils/util.service';

import CHAutoComplete from '../../../../components/ch-auto-complete/CHAutoComplete';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { ENTIDADBANCARIA } from '../../../../types/catalog.type';
import CatalogApi from '../../../../services/api-catalog.service';
import CHSelect from '../../../../components/ch-select/CHSelect';
import ObservacionesSolicitud from '../observaciones/observaciones';


const useStyles = makeStyles(() =>
    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        fullScreenDialog: {
            width: '50%',
        },
        formRow: {
            display: 'flex',
            flexDirection: 'row',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        columnLabel: {
            padding: '10px',
        },

        inline: {
            display: 'inline',
        }
    })
);


interface Props {
    open: boolean;
    onClose: () => void;
    responseSolicitud: any;
    // onSubmitAprobar: any
    setCargar: any;
    actuliazarDatosSolicitud:(datos:any) => void;
    rechazarSolicitud:(datos:any) => void;
  
}

const ModalPago = (props: Props) => {

    const { open, onClose, responseSolicitud,  setCargar, actuliazarDatosSolicitud, rechazarSolicitud } = props;

    const catalogApi = CatalogApi()
    const [nullEntidadRegistro, setNullEntidadRegistro] = React.useState(false);
    const [entidadRegistro, setEntidadRegistro] = useState<any | null>('');
    const [listaEntidadBancaria, setListaEntidadBancaria] = useState<any[]>([]);
    const [tipoCuentas, setTipoCuentas] = useState<any[]>([]);
    const [deshabilitarAprobarModal, setDeshabilitarAprobarModal]= React.useState(false);


    const formularioPago = useForm({
        defaultValues: {
            tipoIdentificacion: '',
            numeroIdentificacion: '',
            razonSocial: '',
            tipoCuentaObj: '',
            entidadObj: '',
            numeroCuenta: '',
            confirmacionNumeroCuenta: '',
            valorPagar: '',
            observaciones: ''
        },
    });


    useEffect(() => {
        
        if(responseSolicitud?.solicitudTerceroBeneficiarioDto!== null){
            const entidadId = responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.entidadObj?.id
            const entidad = responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.entidadObj?.descripcion
            const numeroCuenta = responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.numeroCuenta
            const valorpago = responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.valorRetirarDos
            const idTipoCuenta = responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.tipoCuentaObj?.id
            const tipoCuenta = responseSolicitud?.solicitudTerceroBeneficiarioDto[0]?.tipoCuentaObj?.descripcion            

            const newValue = {
                idEntidadBancaria:entidadId,
                numeroIdentificacion: '',
                nombreRazonSocial: entidad,
                 codigoBanco: tipoCuenta,
                estado: false,
                creadoPor: '',
                fechaCreacion: '',
                modificadoPor: '',
                fechaModificacion: ''
            }

             

        setEntidadRegistro(newValue)
        formularioPago.setValue('tipoCuentaObj',`${idTipoCuenta}_${tipoCuenta}`,)
          if (responseSolicitud?.solicitudTerceroBeneficiarioDto !== null) {
              formularioPago.setValue('entidadObj', entidadId);
          
          }
  
          formularioPago.setValue('numeroCuenta', numeroCuenta)
          formularioPago.setValue('valorPagar', valorpago)
      
        }        
       
}, [open]);


  
    const verificarValorRetirarAct = (valor: any) => {
        const valorSinMask = UtilService.cleanCurrencyValue(valor)
        if (valorSinMask > responseSolicitud.valorTotal) {
            toast(`El valor maximo a retirar es de ${UtilService.formatCurrency(responseSolicitud.valorTotal)}`, { type: toast.TYPE.ERROR })
            setDeshabilitarAprobarModal(true)
        } else {
            setDeshabilitarAprobarModal(false)
        }
    }


    const ejecutarPago = (datos:any) => {
        if(datos.observaciones === ''){
            toast('Por favor agregue una observación', { type: toast.TYPE.ERROR });
        }else{
            actuliazarDatosSolicitud(datos)
            setCargar(true)
            // onSubmitAprobar()
        }
      
    };
    const rechazarModal = (datos:any) => {
        rechazarSolicitud(datos)
        setCargar(true)
        // onSubmitAprobar()
    };



    useEffect(() => {
        catalogApi
            .getAllEntidadBancaria()
            .then(async (response: any) => {
                if (response) {

                    if (response.isSuccess) {
                        
                        setListaEntidadBancaria(response.data);
                       
                    }
                }
            })
            .catch((e) => {
                toast(`Error del sistema, ${e}`, { type: toast.TYPE.ERROR });
            });
    }, [])
    useEffect(() => {
        
        const catalogoId = 67
        catalogApi
            .getAllCatalogoPorId(catalogoId)
            .then(async (response: any) => {
                
                if (response) {

                    if (response.isSuccess) {
                        
                        setTipoCuentas(response.data);
                       
                    }
                }
            })
            .catch((e) => {
                toast(`Error del sistema, ${e}`, { type: toast.TYPE.ERROR });
            });
    }, [])

    const onChangeEntidad = (_: any, newValue: ENTIDADBANCARIA) => {
        
        
        setNullEntidadRegistro(newValue === null);
        setEntidadRegistro(newValue);
      
        if (newValue !== null) {
            
            const dato = `${newValue.idEntidadBancaria}_${newValue.nombreRazonSocial}`;
             formularioPago.setValue('entidadObj', dato);
        }


    };
    const onKeyPressNoNumbers = (event: React.KeyboardEvent<HTMLInputElement>) => {
        const keyCode = event.which || event.keyCode;
        if (keyCode >= 48 && keyCode <= 57) {
            event.preventDefault();
        }
    };

    const classes = useStyles();

    return (
        <>
            <Modal
                open={open}
                onClose={onClose}
                aria-labelledby="form-dialog-title"
                style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    height: '90vh',
                }}
            >

                <Card>
                    <CardContent>
                        <Grid item>
                            <Typography
                                className={classes.titleform}
                                variant="h5"
                                component="h6"
                            >
                                VERIFICAR
                            </Typography>
                        </Grid>

                        <br />
                        <br />
                        <TableContainer component={Paper}>
                            <Grid
                                className={classes.columnButtonAdd}
                                item
                                xs={12}
                                md={12}
                            >
                                <Table aria-label="simple table">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>Tipo de Identificación</TableCell>
                                            <TableCell>Identificación</TableCell>
                                            <TableCell>Nombre / Razón Social</TableCell>
                                            <TableCell>Tipo de Cuenta</TableCell>
                                            <TableCell>Entidad Bancaria </TableCell>
                                            <TableCell>No. Cuenta</TableCell>
                                            <TableCell>Valor a retirar</TableCell>

                                        </TableRow>
                                    </TableHead>


                                    <TableBody>
                                        {responseSolicitud?.solicitudTerceroBeneficiarioDto?.map(
                                            (option: any) => (
                                                <TableRow key={option.id}>
                                                    <TableCell>{option.tipoDocObj.descripcion}</TableCell>
                                                    <TableCell>{option.numDoc}</TableCell>
                                                 
                                                    {
                                                      
                                                        option?.razonsocial === null ?
                                                            <TableCell>
                                                                {option?.razonsocial}
                                                            </TableCell> :
                                                            <TableCell>
                                                                {option?.razonsocial}
                                                            </TableCell>
                                                    }
                                                    <TableCell>
                                                        <FormControl style={{ width: '350px' }}>

                                                            <Controller
                                                                name="tipoCuentaObj"
                                                                control={formularioPago.control}
                                                                render={(
                                                                    {
                                                                        field: { onChange, value },
                                                                        fieldState: { error },
                                                                    }) => (
                                                                        <CHSelect
                                                                        label="Tipo de Cuenta *"
                                                                        value={value}
                            
                                                                        onChange={e => {
                                                                            onChange(e);
                                                                           
                                                                        }}
                            
                                                                    >
                                                                        {tipoCuentas?.map((option: any) => {
                                                                                    	const dato = `${option.idValor}_${option.descripcion}`;
                                                                                        return(
                                                                                            <MenuItem
                                                                                            key={option.codigo}
                                                                                            value={dato}
                                                                                        >
                                                                                           {option?.descripcion}
                                                                                        </MenuItem>
                                                                                        )      
                                                                                 
                                                                              
                                                                        },
                                                                        )}
                                                                    </CHSelect>
                                                                )}
                                                            />
                                                        </FormControl>
                                                        {/* {
                                                            option.tipoCuentaObj
                                                                .descripcion
                                                        } */}
                                                    </TableCell>
                                                    <TableCell>

                                                        <FormControl  style={{width:'350px'}}>

                                                            <Controller
                                                                name="entidadObj"
                                                            
                                                                control={formularioPago.control}
                                                                render={(
                                                                    {
                                                                        field: { onChange, value },
                                                                        fieldState: { error },
                                                                    }) => (
                                                                    <CHAutoComplete
                                                                        value={

                                                                            nullEntidadRegistro ? null : entidadRegistro
                                                                        }

                                                                        onChange={onChangeEntidad}  
                                                                          
                                                                        options={listaEntidadBancaria}
                                                                        getOptionLabel={option => option?.nombreRazonSocial}
                                                                        renderInput={(params) =>


                                                                            <CHTextField
                                                                                value={value}

                                                                                
                                                                              
                                                                                helperText={error ? error.message : null}
                                                                                {...params}

                                                                                onChange={onChange}
                                                                                InputLabelProps={{
                                                                                    shrink: true, // Esto coloca el label en la parte superior
                                                                                }}
                                                                                onKeyPress={onKeyPressNoNumbers}
                                                                            />

                                                                        }

                                                                    />

                                                                )}
                                                            />
                                                        </FormControl>
                                                    </TableCell>
                                                    <TableCell>
                                                    <Controller
											name="numeroCuenta"
											control={formularioPago.control}
										
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (

												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase' } }} // mayusculas
													nameMask
													label=""
												
													required
                                                    
													
													InputLabelProps={{ shrink: true }}
                                                    numberMask
													value={value}
													onChange={e => onChange(e)}
												/>

											)}
										/>
                                                    </TableCell>

                                                    <TableCell>
                                                    <Controller
											name="valorPagar"
											control={formularioPago.control}
											rules={{
												required: 'Este campo es requerido.',
											}}
											render={({
												field: { onChange, value },
												fieldState: { error },
											}) => (

												<CHTextField
													inputProps={{ style: { textTransform: 'uppercase',   textAlign: 'right' } }} // mayusculas
													nameMask
													label=""
													error={!!error}
													required
                                                    isReadOnly={false}
													helperText={
														error ? error.message : null
													}
													InputLabelProps={{ shrink: true }}
                                                    currencyMask
													value={value}
                                                    onChange={(e) => {
                                                        onChange(e);
                                                        verificarValorRetirarAct(e.target.value);
                                                    }}
                                                    // onBlur={e=> verificarValorRetirarAct(e.target.value) }
												/>

											)}
										/>
                                                    </TableCell>

                                                </TableRow>
                                            ),
                                        )}
                                    </TableBody>
                                </Table>
                            </Grid>
                        </TableContainer>
                        <br />
                        <br />
                        <ObservacionesSolicitud
                            control={formularioPago} 
							/>
                        <br />
                        <br />

                        <Grid container justify="flex-end" spacing={3}>

                            <Grid item>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="primary"
                                    disabled={deshabilitarAprobarModal}
                                    onClick={formularioPago.handleSubmit(ejecutarPago)}
                                >
                                    Aprobar
                                </Button>
                            </Grid>
                            <Grid item>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="default"
                                    onClick={formularioPago.handleSubmit(rechazarModal)}
                                >
                                    Rechazar
                                </Button>
                            </Grid>
                            <Grid item>
                                <Button
                                    size="small"
                                    variant="contained"
                                    color="default"
                                    onClick={onClose}
                                >
                                    Cancelar
                                </Button>
                            </Grid>
                        </Grid>


                    </CardContent>
                </Card>

            </Modal>
        </>
    );
};

export default ModalPago;
