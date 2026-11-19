import { connect } from 'react-redux';
import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    makeStyles,
    createStyles,
    MenuItem,
    Icon,
    // createMuiTheme,
} from '@material-ui/core';
// import { blue } from '@material-ui/core/colors';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import { Route, useHistory, useLocation } from 'react-router-dom';
import swal from 'sweetalert';
import { AppState } from '../../store/rootReducer';
import './newsInData.scss';
import { startGetAllIdentificationTypes } from '../../actions/catalog.action';
import { startGetClientByIdentificationTypeIdentificationNumber } from '../../actions/client.action';
import { startGetNovedadesAfiliado } from '../../actions/novedades.action';
import { LoadingComponent } from '../../components/loading/loading.component';
import { ClientState } from '../../reducers/client.reducer';
import { CatalogState } from '../../reducers/catalog.reducer';
import { LoginState } from '../../reducers/login.reducer';
import CHPaper from '../../components/ch-paper/CHPaper';
import { ClienteDto, AdminCuentasAfiliado } from '../../types/client.model.type';
import { NovedadesAfiliadoDto } from '../../types/novedad.model.type';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import history from '../../utils/history.router';
import CHSelect from '../../components/ch-select/CHSelect';
import { ClientForm } from '../curriculum/constants/form.mode.type';
import {
    BusquedaEmbargos,
    ConceptoEmbargosDto,
    EmbargoDto,
    EmbargoModelDto,
} from '../../types/embargo.model.type';
import ApiNovedades from '../../services/api-novedades.service';
import UtilService from '../../utils/util.service';
import CHLoadingProcess from '../../components/ch-loading-process/CHLoadingProcess';
import AuthService from '../../services/auth.service';
import ApiSchedule from '../../services/api-cronograma.service';
import CHCheckbox from '../../components/ch-checkbox/CHCheckbox';
import state from '../state/state';
import EditNewInData from './EditNewInData';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
            display: 'flex'
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
            backgroundColor: '#1C3F8B',
            color: '#fff',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0px',
        },
        accountButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 1px',
            marginLeft: '.5rem',
            width: '100px'
        },
        icon: {
            color: '#42b7ef',
            paddingRight: '10px',
        },
        alineationRight: {
            margin: '0 5rem 0 60rem',
            position: 'relative',
            display: 'flex'
        },
        actions: {
            margin: '8px 1px',
            width: '120px'
        },
        iconRed: {
            padding: '8px',
            textAlign: 'inherit',
            color: '#FF0000',
            margin: '10px'
        },
        circleRed: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            textAlign: 'center',
            marginLeft: '45%',
            width: 15,
            height: 15,
            borderRadius: '50%',
            backgroundColor: 'red',
            color: 'white',
        },


    }),
);

function NovedadAfiliado({
    data,
    isDataLoaded,
    clientState,
    onClickItem,
    onClickItemAccount,
}: {
    data: ClienteDto | null;
    isDataLoaded: boolean;
    clientState: any;
    onClickItem: (data: ClienteDto | null) => void;
    onClickItemAccount: () => void;
}) {
    const classes = useStyles();

    if (!isDataLoaded) {
        return null;
    }

    if (data == null && isDataLoaded) {
        return <div>No se encontraron resultados</div>;
    }

    

    const handleEdicion = async () => {
        swal({
            text: '¿Desea editar la novedad?',
            icon: 'warning',
            buttons: ['Sí', 'No'],
        }).then(async (confirm) => {
            if (!confirm) {
                // history.push(`/EditarNovedadDatos?idCliente=${idCliente}`);
                history.push('/EditarNovedadDatos');
                
            }
        })
    }

    
    const novedadesDatos: string[] = [
        'Homonimia por nombre o cédula',
        'Prorrateo para solución vivienda',
        'Tipo de identificación reportado por la UE',
        'Novedad enviado por UE no corresponde al reportado',
        'Fecha primer y ultimo aporte no concuerda con regimen calculado',

    ]

    

    return (
            <table className='client-table'>
                <thead>
                    <tr>
                        
                        <th style={{ textAlign: 'center' }}>Número de Identificación</th>
                        <th style={{ textAlign: 'center' }}>Nombres</th>
                        <th style={{ textAlign: 'center' }}>Apellidos</th>
                        <th style={{ textAlign: 'center' }}>Diferencia</th>
                        <th style={{ textAlign: 'center' }}>Detalle</th>
                    </tr>
                </thead>
                <tbody>
                
                        <tr key="ClientInfo">
                            
                            <td>{data?.identificacion}</td>
                            <td>
                                {`${data?.primerNombre} ${
                                data?.segundoNombre != null ? data?.segundoNombre : ''
                                }`}
                            </td>
                            <td>
                                {`${data?.primerApellido} ${
                                data?.segundoApellido != null
                                ? data?.segundoApellido
                                : ''
                                }`}
                            </td>
                            <td >
                                <Icon className={classes.circleRed}>alerta</Icon>
                            </td>

                            <td>
                                <span aria-label="column2" onClick={handleEdicion}><span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} /></span>
                                {/* <span aria-label="column2" onClick={handleEdicion(item.idCliente)}><span className={`ico fch-Hoja-Vida-1-1 icoMid-3 ${classes.icon}`} /></span> */}
                            </td>
                        </tr>
                
                </tbody>
            </table>      
    );
}



const NewsInData = (props: Props) => {
    const classes = useStyles();
    const { startGetAllIdentificationTypes, startGetClientByIdentificationTypeIdentificationNumber, startGetNovedadesAfiliado, clientState, catalogState, loginState } = props;
    const [isLoading, setIsLoading] = useState(false);
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const [mostrarTabla, setMostrarTabla] = useState(false);
    const [mostrarTablaDos, setMostrarTablaDos] = useState(false);
    const [selectedData, setSelectedData] = useState<ClienteDto | null>(null);
    const [selectedDataAccount, setSelectedDataAccount] = useState(false);
    const [nombreIdentificaciones, setNombreIdentificaciones] = useState<any>('');
    const { control, handleSubmit, watch } = useForm<ClienteDto>({
        defaultValues: ClientForm,
    });
    const identificationType = watch('idTipoIdentificacion');
    const identification = watch('identificacion');
    const [numeroIdentificacion, setNumeroIdentificacion] = useState(0);
    const api = ApiNovedades()
    const [novedades, setNovedades] = useState<any[]>([])

    const [novedadMasiva, setNovedadMasiva] =useState([]);



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

 

    const NovedadesDatos = [
        'Homonimia por nombre o cédula',
        'Prorrateo para solución vivienda',
        'Tipo de identificación reportado por la UE',
        'Novedad enviado por UE no corresponde al reportado',
        'Fecha primer y ultimo aporte no concuerda con regimen calculado',
    ]

    const campoNovedades = useForm({
        defaultValues: {
            NovedadesDatoss : '',
        }
    });

    const onItemIsSelected = (data: ClienteDto | null) => {
        setSelectedData(data);
    };

    const onClickItemAccount = () => {
        setSelectedDataAccount(true);
    };

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Novedades en los Datos"
                subText=""
                title1="Novedades en los"
                title2="Datos"
            />
            <LoadingComponent key="loader" loading={clientState.loading} />
            <CHPaper title="Novedades en los Datos">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            control={control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value }
                            }) => (
                                <CHSelect
                                    defaultValue=""
                                    label="Tipo de identificación"
                                    value={value}
                                    onChange={onChange}
                                >
                                    {catalogState.IdentificationType?.map((option: any) => (
                                        <MenuItem key={option.id} value={option.id}>
                                            {option.descripcion}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                            name="idTipoIdentificacion"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                        {
                            identificationType === 1 || identificationType === 2 ? (
                                <Controller
                                    name="identificacion"
                                    control={control}
                                    rules={{
                                        
                                        minLength: {
                                            value: 5,
                                            message:
                                                'La longitud mínima del campo es de 5 caracteres',
                                        },
                                        maxLength: {
                                            value: 10,
                                            message:
                                                'La longitud máxima del campo es de 10 caracteres',
                                        },
                                        pattern: {
                                            value: /^[0-9]+$/g,
                                            message: 'Este campo debe contener solo números'
                                        },
                                    }}
                                    defaultValue=""
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? "El número de identificación no debe contener letras" : null}
                                            label="Número de Identificación"
                                            value={value}
                                            type="number"
                                            onChange={e => {
                                                if (
                                                    numeroIdentificacion === 1 ||
                                                    numeroIdentificacion === 2 ||
                                                    numeroIdentificacion === 5
                                                ) {
                                                    if (e.target.value.length <= 10) {
                                                        onChange(
                                                            e.target.value.replace(
                                                                /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W%-+=`~]/g,
                                                                '',
                                                            ),
                                                        );
                                                    }
                                                } else if (
                                                    numeroIdentificacion === 0 ||
                                                    numeroIdentificacion === 3 ||
                                                    numeroIdentificacion === 4 ||
                                                    numeroIdentificacion === 6 ||
                                                    numeroIdentificacion === 7 ||
                                                    numeroIdentificacion === 8
                                                ) {
                                                    if (e.target.value.length <= 10) {
                                                        onChange(
                                                            e.target.value.replace(
                                                                /[ñÑáéíóúÁÉÍÓÚ\W%-+=`~]/g,
                                                                '',
                                                            ),
                                                        );
                                                    }
                                                }
                                            }}
                                        />
                                    )}
                                />
                            ) : (
                                <Controller
                                    name="identificacion"
                                    control={control}
                                    rules={{
                                        
                                        minLength: {
                                            value: 5,
                                            message:
                                                'La longitud mínima del campo es de 5 caracteres',
                                        },
                                        maxLength: {
                                            value: 10,
                                            message:
                                                'La longitud máxima del campo es de 10 caracteres',
                                        },
                                    }}
                                    defaultValue=""
                                    render={({
                                        field: { onChange, value },
                                        fieldState: { error },
                                    }) => (
                                        <CHTextField
                                            error={!!error}
                                            helperText={error ? error.message : null}
                                            isReadOnly={false}
                                            label="Número de Identificación"
                                            value={value}
                                            onChange={e => {
                                                if (
                                                    numeroIdentificacion === 1 ||
                                                    numeroIdentificacion === 2 ||
                                                    numeroIdentificacion === 5
                                                ) {
                                                    if (e.target.value.length <= 10) {
                                                        onChange(
                                                            e.target.value.replace(
                                                                /[a-zA-ZñÑáéíóúÁÉÍÓÚ\W%-+=`~]/g,
                                                                '',
                                                            ),
                                                        );
                                                    }
                                                } else if (
                                                    numeroIdentificacion === 0 ||
                                                    numeroIdentificacion === 3 ||
                                                    numeroIdentificacion === 4 ||
                                                    numeroIdentificacion === 6 ||
                                                    numeroIdentificacion === 7 ||
                                                    numeroIdentificacion === 8
                                                ) {
                                                    if (e.target.value.length <= 10) {
                                                        onChange(
                                                            e.target.value.replace(
                                                                /[ñÑáéíóúÁÉÍÓÚ\W%-+=`~]/g,
                                                                '',
                                                            ),
                                                        );
                                                    }
                                                }
                                            }}
                                        />
                                    )}
                                />
                            )
                        }
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>

                        <Controller
                            name="nombreCliente"
                            control={control}
                            rules={{
                                required: {
                                    value: false,
                                    message: 'Este campo no es requerido',
                                },
                            }}
                            render={({
                                field: { onChange, value },
                                
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    label="Nombres y Apellidos"
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
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="NovedadesDatoss"
                                control={campoNovedades.control}
                                // rules={{
                                //     required: 'Este campo es requerido',
                                // }}
                                render={({
                                    field: { onChange, value }
                                }) => (
                                    <CHSelect
                                        defaultValue=""
                                        label="Novedad en los datos"
                                        value={value}
                                        onChange={onChange}
                                    >
                                        {NovedadesDatos.map((option, index) => (

                                            <MenuItem key={index} value={option}>
                                                {option}
                                            </MenuItem>
                                        ))}
                                        
                                    </CHSelect>
                                )}
                                
                            />
                    </Grid>
                </Grid>

                    <Grid className={classes.column} item xs={8} md={3}>
                        <Button
                            variant="contained"
                            color="primary"
                            className={classes.button}
                            onClick={handleSubmit(handleSearchButton)}
                            
                        >
                            CONSULTAR

                        </Button>
                    </Grid>
                
            </CHPaper>
                
                    <NovedadAfiliado
                        data={clientState.client}
                        onClickItem={onItemIsSelected}
                        isDataLoaded={isDataLoaded}
                        onClickItemAccount={onClickItemAccount}
                    />

            
        </CHPrivateContainer>
    )
};

interface LinkMapStateToProps {
    loginState: LoginState;
    clientState: ClientState;
    catalogState: CatalogState;
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

export default connect(mapStateToProps, mapDispatchToProps)(NewsInData);