import { Button, Grid, IconButton, MenuItem, Table, TableBody, TableCell, TableHead, TableRow } from '@material-ui/core';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import DeleteIcon from '@material-ui/icons/Delete';
import { Edit } from '@material-ui/icons';

import { connect } from 'react-redux';
import moment from 'moment';
import { startObtenerVendedorCredito, 
        startEliminarVendedorCredito, 
        startCrearVendedorCredito } from "../../actions/credit.actions";
import { startGetAllCities, 
        startGetAllIdentificationTypes,
        startGetAllCatalogoPorId,
        startGetAllEntidadBancaria,
        startGetAllDepartments } from '../../actions/catalog.action';
import CatalogApi from '../../services/api-catalog.service';
import CHPaper from '../ch-paper/CHPaper';
import CHSelect from '../ch-select/CHSelect';
import CHTextField from '../ch-text-field/CHTextField';

import { CITYDTO, DEPARTAMENTODTO, ENTIDADBANCARIA, PAISDTO } from '../../types/catalog.type';
import NotificationService from '../../services/notification.service';
import { AppState } from '../../store/rootReducer';

import { CreditVendedorModel } from '../../models/credit-vendedor.model';
import { Address } from '../../types/address.type';
import { setFormDataAddres } from '../modals/address/constants/address.components.constants';
import CHDatePicker from '../ch-datepicker/CHDatepicker';
import AddressComponentV2 from '../modals/address/addressV2.component'


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '20px',

            alignItems: 'flex-end',

            display: 'flex',
            minWidth: '110px',
        },

        cenButtonUno: {
            width: '30%',
            // marginLeft: '10px',
            // MozMarginEnd: '10px',
            // color: '#1706EA ',
            marginTop: '40px',
            marginBottom: '40px',
            
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
            marginBottom: '2rem',

        },


    }),
);

function InformacionBeneficiario(props: any) {

    const {
        procedureRequest,
        login,
        formularioVendedor,
        catalogState,
        CreditState,
        startObtenerVendedorCredito,  
        startEliminarVendedorCredito,
        startCrearVendedorCredito,
        startGetAllIdentificationTypes,
        startGetAllCities,
        startGetAllCatalogoPorId,
        startGetAllEntidadBancaria,
        startGetAllDepartments
    } = props

    const classes = useStyles();

    const [tipoDocumento, setTipoDocumento] = useState(0);
    const [open, setOpen] = useState(false);
    const [estadoActualizar, setEstadoActualizar] = useState(false);
    const [tipoDireccion, setTipoDireccion] = useState(false);
    const [ciudades3, setCiudades3] = useState<CITYDTO[]>([]);


    const { control, clearErrors,register, getValues, setValue, handleSubmit, reset, trigger, ...rest } = useForm<CreditVendedorModel>({
        defaultValues: {
            soc_id: '',
            tipo_Doc: 1,
            numero_Doc: '',
            nombre_Vendedor: '',
            direccion: '',
            departamento: '',
            ciudad: '',
            correo_Electronico: '',
            telefono: '',
            fecha_Entrega: new Date(),
            banco: '',
            tipo_Cuenta: '',
            cuenta: '',
            id: ''
        },
    
    });

    const formularioDireccion = useForm<any>({
        defaultValues: {
        },
    });


    useEffect(() => { 
        const request =  {
            Soc_Id: procedureRequest.requestID,
            Sls_Id: '00000000-0000-0000-0000-000000000000'
        }
        startObtenerVendedorCredito(request)
        startGetAllDepartments()
        startGetAllCities()
        startGetAllIdentificationTypes()
        startGetAllCatalogoPorId(67)
        startGetAllEntidadBancaria()
    }, []);

    useEffect(() => { 
        filtroCiudadesPorDepartamento(getValues('departamento'))
    }, [getValues('departamento')]);


    const filtroCiudadesPorDepartamento = async (idDepartment: any) => {  
        const filtroCity = catalogState.City.filter((item: any) => item.idDepartamento === Number(idDepartment)) ;
        setCiudades3(filtroCity);
    };

    const eliminarVendedor = (data: any) => async () => { 
        const confirmAlert = await  NotificationService.confirmAlert(
            '¿Está seguro?',
        );
        if (confirmAlert) {
        startEliminarVendedorCredito(data)
        }

    }

    const guardarVendedor = (data: CreditVendedorModel )  => { 
        const request = {
            soc_id: procedureRequest.requestID,
            tipo_Doc: data.tipo_Doc,
            numero_Doc: Number(data.numero_Doc),
            nombre_Vendedor: data.nombre_Vendedor,
            direccion: data.direccion,
            departamento: String(data.departamento),
            ciudad: String(data.ciudad),
            correo_Electronico: data.correo_Electronico,
            telefono: String(data.telefono),
            fecha_Entrega: data.fecha_Entrega,  
            tipo_Cuenta: Number(data.tipo_Cuenta),
            banco: data.banco,    
            cuenta: Number(data.cuenta)

        }
        startCrearVendedorCredito(request)
        resetVendedorNueva()
   }

   const actualizarVendedor = async (data: CreditVendedorModel) => {
    const confirmAlert = await NotificationService.confirmAlert(
        '¿Está seguro?',
    );
   
    if (confirmAlert) {
        await startEliminarVendedorCredito(data); 
        await startCrearVendedorCredito(data);   
        resetVendedorNueva();
        setEstadoActualizar(false);
    }
};

    const resetVendedorActualiza = (data: CreditVendedorModel) => async () => { 
        // reset({
        //     id: data.id,
        //     soc_id: data.soc_id,
        //     tipo_Doc: data.tipo_Doc,
        //     numero_Doc: data.numero_Doc,
        //     nombre_Vendedor: data.nombre_Vendedor ,
        //     direccion: data.direccion ,
        //     departamento: data.departamento ,
        //     ciudad: data.ciudad ,
        //     correo_Electronico: data.correo_Electronico ,
        //     telefono: data.telefono,
        //     fecha_Entrega:  new Date(),
        //     banco: data.banco,
        //     tipo_Cuenta: data.tipo_Cuenta  ,
        //     cuenta: data.cuenta
        //   });;

            setValue('id', data.id)
            setValue('soc_id', data.soc_id)
            setValue('tipo_Doc', data.tipo_Doc)
            setValue('numero_Doc', data.numero_Doc)
            setValue('nombre_Vendedor', data.nombre_Vendedor )
            setValue('direccion', data.direccion )
            setValue('departamento', data.departamento )
            setValue('ciudad', data.ciudad )
            setValue('correo_Electronico', data.correo_Electronico )
            setValue('telefono', data.telefono)
            setValue('fecha_Entrega',  new Date())
            setValue('banco', data.banco)
            setValue('tipo_Cuenta', data.tipo_Cuenta  )
            setValue('cuenta', data.cuenta)

            setEstadoActualizar(true)

    }

    const resetVendedorNueva = () => {
        setEstadoActualizar(false)
        clearErrors()
        reset({
            soc_id: procedureRequest.requestID,
            tipo_Doc: 1,
            numero_Doc: '',
            nombre_Vendedor: '',
            direccion: '',
            departamento: '',
            ciudad: '',
            correo_Electronico: '',
            telefono: '',
            fecha_Entrega: new Date(),
            banco: '',
            tipo_Cuenta: '',
            cuenta: '',
          });
    };

  

    const handleOpenAddress = () => {
        setOpen(true);
    };
    const onCloseAddress = (value: Address) => {
        if (value) {
            const result = setFormDataAddres(value);
            setValue('direccion', result);
        }
        setOpen(false);
    };

    const resetAddress = () => {
        formularioDireccion.reset()
    };

    return(
        <>
        
            <CHPaper title=" Datos vendedor ">

            {
            procedureRequest.activity === 'PRIMER PAGO'? null :<>
             <Grid container item xs={12}>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="tipo_Doc"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Tipo de documento vendedor *"
                                    value={value}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={e => {
                                        setTipoDocumento(
                                            Number(e.target.value),
                                        );
                                        onChange(e);
                                    }}
                                >
                                    {catalogState.IdentificationType?.map(
                                        (option: any) => (
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
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="numero_Doc"
                            control={control}
                            rules={{
                                required: {
                                    value: true,
                                    message: 'Este campo es requerido',
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
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    label="Número documento *"
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly={false}
                                    value={value}
                                    onChange={e => {
                                        if (
                                            tipoDocumento === 1 ||
                                            tipoDocumento === 2 ||
                                            tipoDocumento === 4
                                        ) {
                                            if (
                                                e.target.value.length <=
                                                12
                                            ) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        } else if (
                                            tipoDocumento === 3 ||
                                            tipoDocumento === 5 ||
                                            tipoDocumento === 6 ||
                                            tipoDocumento === 7 ||
                                            tipoDocumento === 8
                                        ) {
                                            if (
                                                e.target.value.length <=
                                                20
                                            ) {
                                                onChange(
                                                    e.target.value.replace(
                                                        /[ñÑáéíóúÁÉÍÓÚ\W]/g,
                                                        '',
                                                    ),
                                                );
                                            }
                                        } else {
                                            onChange(e);
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="nombre_Vendedor"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                                maxLength: {
                                    value: 120,
                                    message:
                                        'La longitud máxima del campo es de 120 caracteres',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    InputLabelProps={{ shrink: true }}
                                    label="Nombre o razón social *"
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    inputProps={{
                                        style: {
                                            textTransform: 'uppercase',
                                        },
                                    }}
                                    isReadOnly={false}
                                    value={value}
                                    onChange={e => {
                                        if (
                                            e.target.value.length <= 120
                                        ) {
                                            onChange(e);
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="direccion"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                InputLabelProps={{ shrink: true }}
                                    label="Dirección *"
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    isReadOnly
                                    value={value}
                                    onClick={handleOpenAddress}
                                    onChange={(onChange)}
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container item xs={12}>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="departamento"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Departamento *"
                                    value={value}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={value => {
                                        const id = value.target
                                            .value as number;

                                            filtroCiudadesPorDepartamento(id);

                                        onChange(value);
                                        register('ciudad', {required:'Este campo es requerido.',});
                                        trigger('ciudad')
                                    }}
                                >
                               
                                    {catalogState?.departments
                                        .filter((_:any, index:any) => index !== 0) 
                                        .map((option3: DEPARTAMENTODTO) => (
                                            <MenuItem key={option3.id} value={option3.id}>
                                                {option3.nombre}
                                            </MenuItem>
                                        ))}

                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="ciudad"
                            control={control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Ciudad *"
                                    value={value}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    onChange={value => {
                                        onChange(String(value.target.value));
                                    }}
                                    disabled={
                                        getValues('departamento') === null || 
                                        getValues('departamento') === undefined ||
                                        getValues('departamento') === '' 
                                    }
                                >
                                    {ciudades3.map(
                                        (item3: CITYDTO) => (
                                            <MenuItem
                                                key={item3.id}
                                                value={item3.id}
                                            >
                                                {item3.nombre}
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="correo_Electronico"
                            control={control}
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
                                    InputLabelProps={{ shrink: true }}
                                    label="Correo electrónico *"
                                    error={!!error}
                                    helperText={
                                        error
                                            ? 'Ingrese un correo electrónico válido'
                                            : null
                                    }
                                    isReadOnly={false}
                                    value={value}
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            name="telefono"
                            control={control}
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
                                    isReadOnly={false}
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

                <Grid container item xs={12}>
                    <Grid className={classes.column} item  md={3}>
                        <Controller
                            name="tipo_Cuenta"
                            control={control}
                            rules={{
                                required:'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                
                                    label="Tipo Cuenta *"
                                    error={!!error}
                                    helperText={
                                        error ? error.message : null
                                    }
                                    value={value}
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                >
                                    {catalogState.valorCatalogo?.map(
                                        (option: any) => (
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
                    <Grid className={classes.column} item  md={3}>
                        <Controller
                            name="cuenta"
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
                                helperText={
                                    error ? error.message : null
                                }   
                                    numberMask
                                    label="No. Cuenta *"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                    

                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item  md={3}>
                        <Controller
                            name="banco"
                            control={control}
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
                                    helperText={
                                        error ? error.message : null
                                    }
                                    label="Nombre banco *"
                                    value={value}
                                    
                                    onChange={value => {
                                        onChange(value);
                                    }}
                                >
                                    {catalogState.entidadBancaria?.map(
                                        (
                                            option: ENTIDADBANCARIA,
                                        ) => (
                                            <MenuItem
                                                key={
                                                    option.idEntidadBancaria
                                                }
                                                value={
                                                    option.idEntidadBancaria
                                                }
                                            >
                                                {
                                                    option.nombreRazonSocial
                                                }
                                            </MenuItem>
                                        ),
                                    )}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    <Grid item md={3} className={classes.column}>
                        <Controller
                            control={control}
                            name="fecha_Entrega"
                            rules={{ required: true }}
                            render={({
                                field: { onChange, value },
                            }) => (
                                <>
                                    <CHDatePicker
                                        label="Fecha entrega del inmueble *"
                                        value={value}
                                        invalidDateMessage="Fecha no válida"
                                        maxDateMessage="Fecha no válida"
                                        minDateMessage="Fecha no válida"
                                        placeholder="DD/MM/AAAA"
                                        disablePast
                                        minDate={new Date()}
                                        InputLabelProps={{
                                            shrink: true,
                                        }}
                                        onChange={date => {
                                            onChange(date);
                                        }}
                                    />
                                </>
                            )}
                        />
                    </Grid>
                </Grid>

                <Grid item xs={12} md={12}
                        container
                        alignItems="flex-end"
                        justifyContent="flex-end"
                        >

                        {/* {!estadoActualizar && */}
                            <Button
                            className={classes.buttonAdd}
                            type="submit"
                            variant="outlined"
                            onClick={handleSubmit(guardarVendedor,)}
                            >
                                AGREGAR VENDEDOR
                            </Button>
                        {/* // } */}
                        {/* // {estadoActualizar && */}
                            <Button
                            className={classes.buttonAdd}
                            type="submit"
                            variant="outlined"
                            onClick={handleSubmit(actualizarVendedor,)}
                            >
                            ACTUALIZAR VENDEDOR
                            </Button>
                        {/* // } */}


                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetVendedorNueva}
                        >
                            CANCELAR
                        </Button>

                </Grid>
            </>
            
        }
               

                <thead/>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableCell style = {{width:"8%"}}>Nombre o Razon Social</TableCell>
                        <TableCell style = {{width:"8%"}}>Tipo de Documento</TableCell>
                        <TableCell style = {{width:"8%"}}>No. Documento</TableCell>
                        <TableCell style = {{width:"8%"}}>Dierección</TableCell>
                        <TableCell style = {{width:"8%"}}>Departamento</TableCell>
                        <TableCell style = {{width:"8%"}}>Ciudad</TableCell>
                        <TableCell style = {{width:"8%"}}>Correo Electronico</TableCell>
                        <TableCell style = {{width:"8%"}}>Telefono</TableCell>
                        <TableCell style = {{width:"8%"}}>Tipo cuenta</TableCell>
                        <TableCell style = {{width:"8%"}}>No. cuenta</TableCell>
                        <TableCell style = {{width:"8%"}}>Banco</TableCell>
                        <TableCell style = {{width:"8%"}}>Fecha Entrega</TableCell>
                        {
                            procedureRequest.activity === 'PRIMER PAGO'?null :
                            <TableCell style = {{width:"8%"}}>Acción</TableCell>
                        }
                        

                    </TableHead>
                    <TableBody>

                        { formularioVendedor === undefined ? null : 
                        formularioVendedor?.map((dato: any) => (

                            <TableRow key={`row-${dato?.id}`}>
                                
                                <TableCell>{dato?.nombre_Vendedor === null ? '' : dato?.nombre_Vendedor }</TableCell>

                                <TableCell>
                                {(catalogState.IdentificationType?.filter((x:any) => x.id === Number(dato?.tipo_Doc)))[0]?.descripcion}
                                </TableCell>
                                
                                <TableCell>{dato?.numero_Doc}</TableCell>

                                <TableCell>{dato?.direccion}</TableCell>

                                <TableCell>
                                {(catalogState.City?.filter((x:any) => x.id === Number(dato?.ciudad)))[0]?.nombre}
                                </TableCell>
                                
                                <TableCell>
                                {(catalogState.departments?.filter((x:any) => x.id === Number(dato?.departamento)))[0]?.nombre}
                                </TableCell>
                                
                                <TableCell>{dato?.correo_Electronico}</TableCell>
                                <TableCell>{dato?.telefono}</TableCell>

                                <TableCell>
                                {(catalogState.valorCatalogo?.find((item:any) => item.codigo === String(dato?.tipo_Cuenta)))?.descripcion}
                                </TableCell>

                                <TableCell>{dato?.cuenta}</TableCell>
                                
                                <TableCell>
                                {(catalogState.entidadBancaria?.find((item:any) => item.idEntidadBancaria === dato?.banco))?.nombreRazonSocial}
                                </TableCell>


                                <TableCell>{moment(dato?.fecha_Entrega).format('DD-MM-YYYY')}</TableCell>

                                {
                                    procedureRequest.activity === 'PRIMER PAGO'? null:
                                    <TableCell align="left">
                                    <IconButton
                                        aria-label="remove"
                                        color="primary"
                                        size="medium"
                                        onClick={resetVendedorActualiza(dato)}
                                    >
                                        <Edit fontSize="inherit" />

                                    </IconButton>            
                                    <IconButton
                                        aria-label="remove"
                                        color="primary"
                                        size="medium"
                                        onClick={eliminarVendedor(dato)}
                                    >
                                        <DeleteIcon fontSize="inherit" />

                                    </IconButton>
                                    

                                </TableCell>
                                }

                                
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </CHPaper>

            <AddressComponentV2
                onOpen={open}
                onClose={onCloseAddress}
                tipoDireccion={1 === 1}
            />  
            
                 
        </>
    );

}

const mapStateToProps = (state: AppState) => ({
    login: state.login,
    CreditState: state.credit,
    formularioVendedor: state.credit.formularioVendedor,
    catalogStateCities: state.catalog.City,
    catalogState: state.catalog,

});

const mapDispatchToProps ={
    startObtenerVendedorCredito,
    startEliminarVendedorCredito,
    startCrearVendedorCredito,
    startGetAllCities,
    startGetAllIdentificationTypes,
    startGetAllCatalogoPorId,
    startGetAllEntidadBancaria,
    startGetAllDepartments

};

export default connect(mapStateToProps, mapDispatchToProps,)(InformacionBeneficiario);



