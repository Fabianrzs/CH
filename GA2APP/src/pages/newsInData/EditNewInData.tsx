import React, { useEffect, useState } from 'react';
import {
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Button,
    Modal,
    Fade,
    Backdrop,
    Checkbox,
    Dialog,
    Box,
    TableContainer,
    Typography,
    Table,
} from '@material-ui/core';
import { useHistory, useLocation } from 'react-router-dom';
import { connect } from 'react-redux';
import { Controller, useForm, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import swal from 'sweetalert';
import { AppState } from '../../store/rootReducer';
import history from '../../utils/history.router';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHSelect from '../../components/ch-select/CHSelect';
import { startGetAllIdentificationTypes } from '../../actions/catalog.action';
import { IDENTIFICATIONTYPEDTO, 
    CITYDTO,
    DEPARTAMENTODTO,
    EXECUTINGUNITDTO, 
} from '../../types/catalog.type';
import { JuzgadoDto } from '../../types/court.model.type';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import {
    BusquedaEmbargos,
    ConceptoEmbargosDto,
    EmbargoDto,
    EmbargoModelDto,
    BeneficiarioDto,
    TipoEmbargoDto,
    CuentaModelDto,
    TipoProcesoEmbargoDto,
    TipoRetencionDto,
} from '../../types/embargo.model.type';
import { NovedadesAfiliadoDto } from '../../types/novedad.model.type';
import { busquedaDesembargoForm, busquedaEmbargosForm } from '../embargos/constants/embargosform.model.type';
import UtilService from '../../utils/util.service';
import ApiEmbargos from '../../services/api-embargos.service';
import { ClienteDto, AdminCuentasAfiliado } from '../../types/client.model.type';
import { ClientForm } from '../curriculum/constants/form.mode.type';
import { GetClientByIdentificationTypeIdentificationNumber, GetClientByIdentificationTypeIdentificationNumberFailure, startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import { ClientState } from '../../reducers/client.reducer';
import { busquedaNovedadesForm } from '../newsManagement/constants/novedadesform.model.type';
import ApiClients from '../../services/api-clients.service';
import CHAutoComplete from '../../components/ch-auto-complete/CHAutoComplete';
import { NovedadesCronograma, UnidadEjecutoraCronogramaDTO } from '../../types/cronograma.type';
import CatalogApi from '../../services/api-catalog.service';
import ApiSchedule from '../../services/api-cronograma.service';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import AuthService from '../../services/auth.service';
import ModalEdicionNovedad from './components/ModalEdicionNovedad';
import { startGetNovedadesAfiliado } from '../../actions/novedades.action';
import { LoadingComponent } from '../../components/loading/loading.component';
import ApiNovedades from '../../services/api-novedades.service';


const useStyles = makeStyles((theme) =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        button: {
            width: '60%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        actions: {
            marginLeft: '2rem',
            width: '20%',
        },
        icon: {
            color: '#42b7ef',
            paddingRight: '10px',
        },
    }),
);

function TableNovedadEnDatos ({
    data,
    isDataLoaded,
    
}: {
    data: ClienteDto | null;
    isDataLoaded: boolean;
    
    
}) {
    const classes = useStyles();

    if (!isDataLoaded) {
        return null;
    }

    if (data == null && isDataLoaded) {
        return <div>No se encontraron resultados</div>;
    }

    const [selectedItems, setSelectedItems] = useState<number[]>([]);


    const [modalOpen, setModalOpen] = useState(false);

    const handleModalOpen = () => {
        setModalOpen(true);
    };

    const handleModalClose = () => {
        setModalOpen(false);
    };

    return (
        <table className='table'>
                <thead>
                    <tr>
                    <th style={{ textAlign: 'center' }}>
                        Tipo de Dato
                    </th>
                    <th style={{ textAlign: 'center' }}>
                        Novedad
                    </th>
                    <th style={{ textAlign: 'center' }}>
                        Estado
                    </th>
                    <th style={{ textAlign: 'center' }}>Acción</th>
                    </tr>
                </thead>
                <tbody>
                    
                    <tr key="ClientInfo">
                        <td>
                            
                            <ul>
                                <li>Tipo de Identificación</li>
                            </ul>
                            <ul>
                                <li>Número de Identificación</li>
                            </ul>
                            <ul>
                                <li>Nombres</li>
                            </ul>
                            <ul>
                                <li>Apellidos</li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>CÉDULA DE IDENTIFICACIÓN</li>
                            </ul>
                            <ul>
                                <li>
                                    {data?.identificacion}
                                     
                                </li>
                            </ul>
                            <ul>
                                <li>
                                    {`${data?.primerNombre} ${
                                        data?.segundoNombre != null ? data?.segundoNombre : ''
                                    }`}
                                </li>
                            </ul>
                            <ul>
                                <li>
                                    {`${data?.primerApellido} ${
                                        data?.segundoApellido != null
                                        ? data?.segundoApellido
                                        : ''
                                    }`}
                                </li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>Correcto</li>
                            </ul>
                            <ul>
                                <li>Correcto</li>
                            </ul>
                            <ul>
                                <li>Erroneo</li>
                            </ul>
                            <ul>
                                <li>Correcto</li>
                            </ul>
                        </td>
                        <td>
                            
                            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                                <span aria-label="column2"  onClick={handleModalOpen} style={{ marginBottom: '8px'}}>
                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                </span>
                                <span aria-label="column2"  onClick={handleModalOpen} style={{ marginBottom: '8px'}}>
                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                </span>
                                <span aria-label="column2"  onClick={handleModalOpen} style={{ marginBottom: '8px'}}>
                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                </span>
                                <span aria-label="column2"  onClick={handleModalOpen} style={{ marginBottom: '8px'}}>
                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                </span>
                            </div>
                        </td>
                    </tr>
                    
                </tbody>
        </table>  
    );
}


const editarNovedadEnDatos = (props: Props) => {

    const classes = useStyles();
    const user = AuthService.getUserInfo();
    const { startGetAllIdentificationTypes, startGetClientByIdentificationTypeIdentificationNumber, startGetNovedadesAfiliado, clientState, catalogState, loginState } = props;
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [mostrarTabla, setMostrarTabla] = useState(true);
    const [isLoading, setIsLoading] = useState(false);
    const [novedades, setNovedades] = useState<any[]>([])
    const [nombreIdentificaciones, setNombreIdentificaciones] = useState<any>('');
    const [data, setData] = useState<ClienteDto | null>(null);
    const api = ApiClients();
    
    const handleOnClickRegresar = () => {
        history.push('/NovedadesEnLosDatos')
    }

    const handleSaveNovedad = () => {
        swal({
            text: '¿Desea guardar la novedad?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(async (confirm) => {
            if (!confirm) {
                history.push('/NovedadesEnLosDatos');
                swal({
                    text: `Novedad editada y almacenada por usuario: ${user.USR_NOMBRE}`,
                    icon: 'success',
                })
            }
        })
    }

    const { control, handleSubmit, watch } = useForm<ClienteDto>({
        defaultValues: ClientForm,
    });
 
    const [selectedItems, setSelectedItems] = useState<number[]>([]);


    

    const Novedad: string[] = [
        '',

    ]

    const [modalOpen, setModalOpen] = useState(false);

    const handleModalOpen = () => {
        setModalOpen(true);
    };

    const handleModalClose = () => {
        setModalOpen(false);
    };

    const identificationType = watch('idTipoIdentificacion');
    const identification = watch('identificacion');
    const [clientData, setClientData] = useState({});
    

    useEffect(() => {
        const fetchData = async () => {
            if( identificationType !== 0 && identification !== '') {
                try {
                    const response = await api.getClientByIdentificationTypeIdentificationNumberRequest(
                        identificationType as number,
                        identification
                    );

                    const {identificacion, primerNombre, segundoNombre, primerApellido, segundoApellido } = response.data;
                        
                    setClientData({
                        identificacion, 
                        primerNombre, 
                        segundoNombre, 
                        primerApellido, 
                        segundoApellido
                    });
                } catch (error) {
                    console.log('Error al obtener los datos del cliente');
                    
                }
            }
        };
        fetchData();
    }, [identificationType, identification]);

    const handleSearchButton = async () => {
        if(identificationType !== 0 && identification !== ''){
            startGetClientByIdentificationTypeIdentificationNumber(
                '',
                identificationType as number,
                identification,
            );
            setIsDataLoaded(true);
        } else {
            toast("Los campos de identificación y tipo de identificación son obligatorios")
        }

    };


        return (
            <CHPrivateContainer>
                <LoadingComponent key="loader" loading={clientState.loading} />
                <CHPaper title="Editar novedad de datos">
                    <Grid container>
                    <table className='table'>
                <thead>
                    <tr>
                    <th style={{ textAlign: 'center' }}>
                        Tipo de Dato
                    </th>
                    <th style={{ textAlign: 'center' }}>
                        Novedad
                    </th>
                    <th style={{ textAlign: 'center' }}>
                        Estado
                    </th>
                    <th style={{ textAlign: 'center' }}>Acción</th>
                    </tr>
                </thead>
                <tbody>
                    
                    <tr key="ClientInfo">
                        <td>
                            
                            <ul>
                                <li>Tipo de Identificación</li>
                            </ul>
                            <ul>
                                <li>Número de Identificación</li>
                            </ul>
                            <ul>
                                <li>Nombres</li>
                            </ul>
                            <ul>
                                <li>Apellidos</li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>CÉDULA DE CIUDADANÍA</li>
                            </ul>
                            <ul>
                                <li>
                                    {clientState?.client.identificacion}
                                     
                                </li>
                            </ul>
                            <ul>
                                <li>
                                    {`${clientState?.client?.primerNombre} ${
                                        clientState?.client.segundoNombre != null ? clientState?.client.segundoNombre : ''
                                    }`}
                                </li>
                            </ul>
                            <ul>
                                <li>
                                    {`${clientState?.client.primerApellido} ${
                                        clientState?.client.segundoApellido != null
                                        ? clientState?.client.segundoApellido
                                        : ''
                                    }`}
                                </li>
                            </ul>
                        </td>
                        <td>
                            <ul>
                                <li>Correcto</li>
                            </ul>
                            <ul>
                                <li>Correcto</li>
                            </ul>
                            <ul>
                                <li>Erroneo</li>
                            </ul>
                            <ul>
                                <li>Correcto</li>
                            </ul>
                        </td>
                        <td>
                            {/* <span aria-label="column2" onClick={handleModalOpen}><span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} /></span> */}
                            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                                <span aria-label="column2"  onClick={handleModalOpen} style={{ marginBottom: '8px'}}>
                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                </span>
                                <span aria-label="column2"  onClick={handleModalOpen} style={{ marginBottom: '8px'}}>
                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                </span>
                                <span aria-label="column2"  onClick={handleModalOpen} style={{ marginBottom: '8px'}}>
                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                </span>
                                <span aria-label="column2"  onClick={handleModalOpen} style={{ marginBottom: '8px'}}>
                                <span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} />
                                </span>
                            </div>
                        </td>
                    </tr>
                    
                </tbody>
        </table>
                            <Grid item xs={12}>
                                <Box display="flex" justifyContent="center">
                                    {/* <Button
                                        className={classes.actions}
                                        variant="contained"
                                        color='primary'
                                        onClick={handleSubmit(handleSearchButton)}
                                    >
                                        CARGAR NOVEDADES
                                    </Button> */}
                                    <Button
                                        className={classes.actions}
                                        variant="contained"
                                        color='primary'
                                        onClick={handleSubmit(handleSaveNovedad)}
                                    >
                                        GUARDAR
                                    </Button>
                                    <Button
                                        className={classes.actions}
                                        variant="contained"
                                        color='default'
                                        onClick={handleOnClickRegresar}
                                    >
                                        REGRESAR
                                    </Button>
                                </Box>
                            </Grid>
                        

                    </Grid>
                </CHPaper>

                
                    {/* <TableNovedadEnDatos
                        data={clientState.client}
                        isDataLoaded={isDataLoaded}
                    
                    /> */}
                

                <ModalEdicionNovedad
                    data={clientState.client}
                    isDataLoaded={isDataLoaded}
                    open={modalOpen}
                    handleClose={handleModalClose}
                />

                
                
            </CHPrivateContainer>
        )
};



interface LinkMapStateToProps {
    loginState: LoginState;
    clientState: ClientState;
    catalogState: CatalogState;
    formularioEmbargos: UseFormReturn<any>;
    tiposIdentificacion: IDENTIFICATIONTYPEDTO[];
    

}

const mapStateToProps = (state: AppState) => ({
    clientState: state.client,
    catalogState: state.catalog,
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    startGetAllIdentificationTypes: (claims: string) => void;
    startGetClientByIdentificationTypeIdentificationNumber: (
        claims: string,
        idType: number,
        identification: string,
    ) => void;
    startGetNovedadesAfiliado: any
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
    startGetClientByIdentificationTypeIdentificationNumber,
    startGetNovedadesAfiliado,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(editarNovedadEnDatos);