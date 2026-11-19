import { connect } from 'react-redux';
import {
    makeStyles,
    createStyles,
    Grid,
    Button,
    MenuItem,
} from '@material-ui/core';
import React, {useEffect} from 'react';
import { Controller, useForm } from 'react-hook-form';
import swal from 'sweetalert';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPageTitle from '../../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { startGetAllIdentificationTypes } from '../../../actions/catalog.action';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import { AppState } from '../../../store/rootReducer';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        btn: {
            margin: '8px ',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
        button_left: {
            textAlign: 'left',
        },
        button_right: {
            textAlign: 'right',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },
    }),
);

const DisbursementCreation = (props: Props) => {

    const classes = useStyles();   
    const { catalogState, loginState, startGetAllIdentificationTypes} = props;

    useEffect(() => {
       startGetAllIdentificationTypes(loginState.claims);
    }, []);

    const { control, handleSubmit } = useForm({
        defaultValues: {
            tipoIdentificacion: '1',
            numeroIdentificacion: '',
            numeroSolicitud: '',
            valorDesembolso: '',
            fechaDesembolso: null,
            fechaCorte: null,
        },
    });   
 
    const aceptarCreacionDesembolso = (data: any) => {
        
        if(data.numeroIdentificacion === ''){
            swal({
                text: "Ingrese la identificación del afiliado",               
            })
        }
        else if(data.numeroSolicitud === ''){
            swal({
                text: "Ingrese el Número de solicitud",               
            })
        }
        else if(data.valorDesembolso === 0){
            swal({
                text: "Ingrese el Valor del Desembolso",                
            })
        }
        else if(data.fechaDesembolso == null){
            swal({
                text: "Ingrese la Fecha del Desembolso",                
            })
        }
        else if(data.fechaCorte == null){
            swal({
                text: "Ingrese la Fecha de Corte",               
            })
        }      
        else if(data.fechaCorte < data.fechaDesembolso)
        {
            swal({
                text: "La Fecha Corte no puede ser inferior a la Fecha Desembolso",                
            })
        }        
        else{
            swal({
                text: "¿Estáseguro que desea realizar la creación del desembolso?",
                icon: "warning",
                buttons: ["Sí", "No"]
            }).then (respuesta =>{
                if(!respuesta){
                    swal({
                        text: "Se ha realizado la creación del desembolso con éxito",
                        icon: "success"
                    })
                }
            })
        }
    }
    return(
        <CHPrivateContainer>
            <CHPageTitle
                    topText=""
                    subText=""
                    title1="Creación del Desembolso"
                    title2=""
            />
            <CHPaper title="Información del desembolso">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="tipoIdentificacion"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                             <CHSelect
                               defaultValue=""
                               error={!!error}
                               label="Tipo de Identificación"
                               value={value}
                               helperText={error ? error.message : null}
                               onChange={onChange}
                            
                             >
                                {catalogState.IdentificationType?.map(
                                    (option: IDENTIFICATIONTYPEDTO) => (
                                        <MenuItem key={option.id} value={option.id}>
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
                            name="numeroIdentificacion"
                            control={control}
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
                                    onChange={onChange}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="numeroSolicitud"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    label="Número Solicitud"
                                    value={value}
                                    onChange={onChange}
                                    type="number"
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="valorDesembolso"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly={false}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right'
                                        }

                                    }}
                                    label="Valor Desembolso"
                                    value={value}
                                    onChange={onChange}
                                    currencyMask
                                />
                            )}
                        />
                    </Grid>
                </Grid>
                <Grid container>               
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="fechaDesembolso"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha Desembolso"
                                    onChange={onChange}
                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                            name="fechaCorte"
                            control={control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHDatePicker
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Fecha Corte"
                                    onChange={onChange}
                                    value={value}
                                />
                            )}
                        />
                    </Grid>
                </Grid> 
                <Grid container> 
                      <Grid className={classes.column} item xs={12} md={4} />                              
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Button 
                                className={classes.button}
                                variant="contained"
                                color="primary"
                                onClick={handleSubmit(aceptarCreacionDesembolso)}                          
                            >
                                ACEPTAR
                            </Button>
                        </Grid>
                    </Grid>     
            </CHPaper>
        </CHPrivateContainer>
    );
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
    startGetAllIdentificationTypes: (claims: string) => void;
}

const mapDispatchToProps = {
    startGetAllIdentificationTypes,
};    

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(DisbursementCreation);
