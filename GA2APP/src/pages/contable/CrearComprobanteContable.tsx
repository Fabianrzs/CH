import { Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Paper, Table, TableBody, TableCell, TableContainer, TableFooter, TableHead, TablePagination, TableRow, Typography } from '@material-ui/core';
import { Alert, AlertTitle } from '@material-ui/lab';
import React, { useEffect, useMemo, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { Delete, Edit, Search, Share, ShareOutlined } from '@material-ui/icons';
import moment from 'moment';
import { CatalogState } from '../../reducers/catalog.reducer';
import { ProductState } from '../../reducers/producto.reducer';
import {startGetAllIdentificationTypes,
    startGetAllCities,
    startGetAllDepartments,
    startGetBeneficiarioCartera,
    startGetConceptosCartera,
    startGetTipoComprobante
} from '../../actions/catalog.action';
import {startGetProductoByIdentification,
        startPostCrearComprobanteCartera,
        startPostObtenerComprobanteCartera,
        startPostActualizarMovimientoComprobante
        
    } from '../../actions/producto.action';
import {startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../components/ch-select/CHSelect';
import CHTextField from '../../components/ch-text-field/CHTextField';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import { LoginState } from '../../reducers/login.reducer';
import { AppState } from '../../store/rootReducer';
import { CITYDTO, DEPARTAMENTODTO, IDENTIFICATIONTYPEDTO, MOVIMIENTOS } from '../../types/catalog.type';
import ContainerTitle from '../../components/containe-title/container-title';
import ModalEditarMovimientoCartera from './components/ModalEditarMovimiento';
import { ClientState } from '../../reducers/client.reducer';
import UtilService from '../../utils/util.service';
import NotificationService from '../../services/notification.service';
import { LoadingComponent } from '../../components/loading/loading.component';


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
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
        totales: {
            textAlign: 'right'
        },
        infoPanel: {
            display: 'grid',
            gridTemplateColumns: '1fr auto',
            gap: '0.5rem 2rem',
            marginTop: '1rem'
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
            marginBottom: '2rem',

        },
        buttonContainer: {
            display: 'flex',
            justifyContent: 'center', // Centra los botones horizontalmente
            marginTop: '1rem'
        }
      
    }),
);

const GenerarComprobanteContable = (props: Props) => {
    const classes = useStyles();
    const {
        catalogState,
        loginState,
        productState,
        clientState,
        startGetAllIdentificationTypes,
        startGetProductoByIdentification,
        startGetAllCities,
        startGetAllDepartments,
        startPostCrearComprobanteCartera,
        startPostActualizarMovimientoComprobante,
        startPostObtenerComprobanteCartera,
        startGetBeneficiarioCartera,
        startGetConceptosCartera,
        startGetTipoComprobante,
        startGetClientByIdentificationTypeIdentificationNumber,
    } = props;


    const { control,clearErrors,register, getValues, setValue, handleSubmit, reset, ...rest } = useForm({
        defaultValues: {
            numeroComprobante: '',
            fechaComprobante: new Date(),
            tipoComprobante: '',
            departamento: '',
            ciudad: '',
            concepto: '',
            beneficiario: '',
            tipoIdentificacion: '',
            identificacion: '',
            NombreBeneficiario: '',

        }
    });

    const formularioActualizarMovimiento = useForm({
        defaultValues: {
            movimiento_Comprobante_CarteraId: 0,
            comprobante_CarteraId: 0,
            cuenta_Contable: 0,
            descripcion_Cuenta: '',
            centro_Costo: 0,
            detalle: 0,
            tipo: '',
            monto: 0
        }
    })

    const { control: control2, handleSubmit: handleSubmit2 } = useForm({
        defaultValues: {
            comprobanteId: 0,
            // fechaComprobante: new Date(),
            comprobanteSevenId: 0,
        }
    });



    const [tipoDocumento, setTipoDocumento] = useState(0);
    const [ciudades3, setCiudades3] = useState<CITYDTO[]>([]);
    const [openModalAgregar, setOpenModalAgregar] = useState(false);
    const [opciones, setOpciones] = useState(0);
    const [objetcList, setObjetcList] = useState<MOVIMIENTOS[]>([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(10);
    const paginatedData = objetcList.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);

    useEffect(() => {
        startGetAllIdentificationTypes(loginState.claims);
        startGetAllDepartments()
        startGetAllCities()
        startGetBeneficiarioCartera()
        startGetConceptosCartera()
        startGetTipoComprobante()
    }, []);

    const totals = useMemo(() => {
        debugger
        const debito = objetcList
            .filter((mov) => mov.tipo === 'D')
            .reduce((sum, mov) => sum + mov.monto, 0);
        const credito = objetcList
            .filter((mov) => mov.tipo === 'C')
            .reduce((sum, mov) => sum + mov.monto, 0);
        const diferencia = debito - credito;

        return { debito, credito, diferencia };
    }, [objetcList]);

    const handleChangePage = (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const filtroCiudadesPorDepartamento = async (idDepartment: any) => {  
        const filtroCity = catalogState.City.filter((item: any) => item.idDepartamento === Number(idDepartment)) ;
        setCiudades3(filtroCity);
    };


    const handleDelete = (index: number) => {
        setObjetcList((prevObjetcList) => prevObjetcList.filter((_, i) => i !== index));
    };

    const handleEdit = (rowData: any) => {
        debugger
        setOpciones(1)
        formularioActualizarMovimiento.setValue('movimiento_Comprobante_CarteraId', rowData.movimiento_Comprobante_CarteraId)
        formularioActualizarMovimiento.setValue('comprobante_CarteraId', rowData.comprobante_CarteraId)
        formularioActualizarMovimiento.setValue('cuenta_Contable', rowData.cuenta_Contable)
        formularioActualizarMovimiento.setValue('descripcion_Cuenta', rowData.descripcion_Cuenta)
        formularioActualizarMovimiento.setValue('centro_Costo', rowData.centro_Costo)
        formularioActualizarMovimiento.setValue('detalle', rowData.detalle)
        formularioActualizarMovimiento.setValue('tipo', rowData.tipo)
        formularioActualizarMovimiento.setValue('monto', rowData.monto)
        setOpenModalAgregar(true);
    };

    const handleAgregar = () => {
        debugger
        setOpciones(2)
        formularioActualizarMovimiento.setValue('movimiento_Comprobante_CarteraId', 0)
        formularioActualizarMovimiento.setValue('comprobante_CarteraId',0 )
        formularioActualizarMovimiento.setValue('cuenta_Contable',0 )
        formularioActualizarMovimiento.setValue('descripcion_Cuenta', '')
        formularioActualizarMovimiento.setValue('centro_Costo',0)
        formularioActualizarMovimiento.setValue('detalle',0 )
        formularioActualizarMovimiento.setValue('tipo','' )
        formularioActualizarMovimiento.setValue('monto',0 )
        setOpenModalAgregar(true);
    };

    const guardarComprobante = async (data: any) => {        

        if (objetcList.length === 0) {
            NotificationService.genericAlwarning(
                'Error',
                'Debe agregar al menos un movimiento al comprobante',
                'warning'
            );
            return;
        }

        const request = {
            comprobante_Id: 0,
            comprobante_Seven_Id: 0,
            fecha_Comprobante: moment(data.fechaCoprobante).format(),
            tipo_Comprobante: data.tipoComprobante,
            departamento: data.departamento,
            ciudad: data.ciudad,
            concepto: data.concepto,
            beneficiario: data.beneficiario,
            tipo_Identificacion: data.tipoIdentificacion,
            numero_Identificacion: data.identificacion,
            lstMovimientoComprobante: objetcList   
        };

        startPostCrearComprobanteCartera(request);
    };

    const enviarComprobante = () => {
        // Lógica para manejar la edición de una fila
        console.log(`Guardar Comprobante`);
      };

    const handleCloseModalAgregar = () => {
        setOpenModalAgregar(false);
    };

    const handleActualizarMovimiento = async (data: any) => {
        debugger
        const request = {
            id: data.id,
            seguro: Number(data.seguro),
        };
        // startPostActualizarMovimientoComprobante(data);
        handleCloseModalAgregar();
    };

    const handleAgregarMovimiento = async (data: any) => {
        debugger
        const cleanedData = {
            ...data,
            monto: UtilService.cleanCurrencyValue(data.monto)
        };
        setObjetcList((prevObjetcList) => [...prevObjetcList, cleanedData]);

        console.log('objeto',objetcList)
        handleCloseModalAgregar();
    };
    const handleActualizarMovimientoPre = async (data: any) => {
        debugger
        const cleanedData = {
            ...data,
            monto: UtilService.cleanCurrencyValue(data.monto)
        };
        setObjetcList((prevObjetcList) => [...prevObjetcList, cleanedData]);

        console.log('objeto',objetcList)
        handleCloseModalAgregar();
    };

    return(
    <CHPrivateContainer>
        <PageIndicators
            topTextNivel1='Cierres Cartera'
            topTextNivel2='Contabilidad Cartera' 
        />
      <LoadingComponent key="loader" loading={clientState.loading} />
      <LoadingComponent key="loader" loading={productState.loading} />

        <IconTitle
            title='Generar '
            subTitle='Comprobante Cartera'
            icon='fch-Administracion-Usuarios-0-1'     
        />

        <CHPaper title="Datos de Comprobante">

            <ContainerTitle title="CREAR COMPROBANTE" />

            <Grid container>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="numeroComprobante"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Numero Comprobante*"
                                value={value}
                                onChange={(e) => {
                                    let newValue = e.target.value.replace(/\D/g, '');
                                    if (newValue.length > 10) {
                                        newValue = newValue.slice(0, 10);
                                    }
                                    onChange(newValue);
                                }}                                
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{shrink: true,}}
                            />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="fechaComprobante"
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        control={control}
                        render={({
                            field: { onChange, value },
                            
                        }) => (
                            <CHDatePicker
                                label="Fecha Comprobante*"
                                inputProps={{ readOnly: true }}
                                onChange={onChange}
                                value={value}
                                autoOk
                            />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="tipoComprobante"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                label="Tipo de comprobante*"
                                value={value}
                                error={!!error}
                                helperText={error ? error.message : null}
                                onChange={onChange}
                            >
                                {catalogState.tipoComprobante?.map(
                                    (option: any) => (
                                        <MenuItem
                                            key={option.tipoComprobanteId}
                                            value={option.tipoComprobanteId}
                                        >
                                            {option.tipoComprobante}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
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
                                    register('ciudad', {
                                        required:
                                            'Este campo es requerido.',
                                    });
                                }}
                            >
                                {catalogState?.departments.map(
                                    (option3: DEPARTAMENTODTO) => (
                                        <MenuItem
                                            key={option3.id}
                                            value={option3.id}
                                        >
                                            {option3.nombre}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
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

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="concepto"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                label="Concepto*"
                                value={value}
                                error={!!error}
                                helperText={error ? error.message : null}
                                onChange={onChange}
                            >
                                {catalogState.conceptosCartera?.map(
                                    (option: any) => (
                                        <MenuItem
                                            key={option.tipoConceptoId}
                                            value={option.tipoConceptoId}
                                        >
                                            {option.tipoConcepto}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        )}
                    />
                </Grid>
                
                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="beneficiario"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                label="Beneficiario*"
                                value={value}
                                error={!!error}
                                helperText={error ? error.message : null}
                                onChange={onChange}
                            >
                                {catalogState.beneficiarioCartera?.map(
                                    (option: any) => (
                                        <MenuItem
                                            key={option.tipoBeneficiarioId}
                                            value={option.tipoBeneficiarioId}
                                        >
                                            {option.tipoBeneficiario}
                                        </MenuItem>
                                    ),
                                )}
                            </CHSelect>
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="tipoIdentificacion"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                label="Tipo de Identificación *"
                                value={value}
                                error={!!error}
                                helperText={error ? error.message : null}
                                onChange={e => {
                                    setTipoDocumento(
                                        e.target.value as number,
                                    );
                                    onChange(e);
                                }}
                            >
                                {catalogState.IdentificationType?.map(
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

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="identificacion"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange,onBlur, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label="Numero Identificación*"
                                value={value}
                                onChange={(e) => {
                                    let newValue = e.target.value.replace(/\D/g, '');
                                    if (newValue.length > 10) {
                                        newValue = newValue.slice(0, 10);
                                    }
                                    onChange(newValue);

                                }} 
                                onBlur={() => {
                                    onBlur();
                                    startGetClientByIdentificationTypeIdentificationNumber('', Number(tipoDocumento), value.toString()); // Despacha la acción de Redux

                                }}                               
                                error={!!error}
                                helperText={error ? error.message : null}
                                InputLabelProps={{shrink: true,}}
                            />
                        )}
                    />
                </Grid>

                <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="NombreBeneficiario"
                        control={control}
                        // rules={{
                        //     required: 'Este campo es requerido',
                        // }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly
                                label="Nombre de beneficiario"
                                value={value}
                                onChange={onChange}
                                InputLabelProps={{ shrink: true }}
                            />
                        )}
                    />
                </Grid>
            </Grid>

            <ContainerTitle title="MOVIMIENTOS DE COMPROBANTE" />

            <Grid container>

            <Grid className="container-23" style={{ display: 'flex', justifyContent: 'flex-end', paddingLeft: '70%' }}>
                <Grid>
                    <button type="button" className="bgcian button-i blue flex" onClick={handleAgregar}>
                        <span className="ico fch-Agregar-0-1 icoMid-2 separador-marg-6" />
                        AGREGAR
                    </button>
                </Grid>
            </Grid>

                <TableContainer component={Paper}>
                    <Grid className={classes.columnButtonAdd} item xs={12} md={12}>
                        <Table aria-label="simple table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Cuenta contable</TableCell>
                                    <TableCell>Descripcion de cuenta</TableCell>
                                    <TableCell>Centro de costo</TableCell>
                                    <TableCell>Detalle</TableCell>
                                    <TableCell>Tipo (D/C)</TableCell>
                                    <TableCell>Monto</TableCell>
                                    <TableCell>Acciones</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                            {paginatedData.map((option: MOVIMIENTOS, index: number) => (
                                <TableRow key={index}>
                                    <TableCell>{option.cuenta_Contable}</TableCell>
                                    <TableCell>{option.descripcion_Cuenta}</TableCell>
                                    <TableCell>{option.centro_Costo}</TableCell>
                                    <TableCell>{option.detalle}</TableCell>
                                    <TableCell>{option.tipo}</TableCell>
                                    <TableCell>{option.monto}</TableCell>
                                    <TableCell>
                                        <IconButton color="secondary" onClick={() => handleDelete(index)}>
                                            <Delete />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                        <TableFooter>
                                <TableRow>
                                    <TablePagination
                                        rowsPerPageOptions={[5, 10, 25]}
                                        count={objetcList.length}
                                        rowsPerPage={rowsPerPage}
                                        page={page}
                                        onPageChange={handleChangePage}
                                        onRowsPerPageChange={handleChangeRowsPerPage}
                                        labelRowsPerPage="Filas por página"
                                        labelDisplayedRows={({ from, to, count }) => `${from}-${to} de ${count}`}
                                    />
                                </TableRow>
                        </TableFooter>
                        </Table>
                    </Grid>
                </TableContainer>

                <Grid container>
                    <Grid style={{ margin: '2rem 1' }} item xs={12}>
                        <Alert severity="info">
                            <AlertTitle>
                                <strong>Movimiento</strong>
                            </AlertTitle>
                            <div className={classes.infoPanel}>
                                <strong>Debito:</strong>
                                <span className={classes.totales}>{totals.debito}</span>
                                <strong>Credito:</strong>
                                <span className={classes.totales}>{totals.credito}</span>
                                <strong>Diferencia:</strong>
                                <span className={classes.totales}>{totals.diferencia}</span>
                            </div>
                        </Alert>
                    </Grid>
                </Grid>
                <Grid container className={classes.buttonContainer}>
                    <Button
                        className={classes.buttonAdd}
                        type="submit"
                        variant="outlined"
                        onClick={handleSubmit(guardarComprobante)}
                    >
                        GUARDAR COMPROBANTE
                    </Button>
                    <Button
                        className={classes.buttonAdd}
                        type="submit"
                        variant="outlined"
                        onClick={handleSubmit(enviarComprobante)}
                    >
                        ENVIAR A SEVEN
                    </Button>
                </Grid>
            </Grid>

            {/* <ModalEditarMovimientoCartera
             opciones = {opciones}
             open = { openModalAgregar }
             handleClose={handleCloseModalAgregar}
             handleAgregarMovimiento={handleAgregarMovimiento}
             handleActualizarMovimiento={handleActualizarMovimiento}
             handleActualizarMovimientoPre={handleActualizarMovimientoPre}
             formularioActualizar={formularioActualizarMovimiento}
             /> */}

        </CHPaper>

    </CHPrivateContainer>
    )

}

interface LinkMapStateToProps {
    catalogState: CatalogState;
    loginState: LoginState;
    productState: ProductState;
    clientState: ClientState;

}

const mapStateToProps = (state: AppState) => ({
    catalogState: state.catalog,
    loginState: state.login,
    productState: state.producto,
    clientState: state.client,


});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetProductoByIdentification: (identificationType: number, idNumber: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void;    startGetAllDepartments: () => void;
    startPostCrearComprobanteCartera: (data: any) => void;
    startPostActualizarMovimientoComprobante: (data: any) => void;
    startPostObtenerComprobanteCartera: (data: any) => void;
    startGetAllCities: (stateID?: string) => void;
    startGetBeneficiarioCartera: () => void;
    startGetConceptosCartera: () => void;
    startGetTipoComprobante: () => void;
    
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetProductoByIdentification,
    startGetAllDepartments,
    startPostCrearComprobanteCartera,
    startPostObtenerComprobanteCartera,
    startGetAllCities,
    startGetBeneficiarioCartera,
    startGetConceptosCartera,
    startGetTipoComprobante,
    startPostActualizarMovimientoComprobante,
    startGetClientByIdentificationTypeIdentificationNumber
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(GenerarComprobanteContable);




