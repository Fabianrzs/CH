import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { createStyles, makeStyles,MenuItem,Grid,Theme,Button } from '@material-ui/core';
import { Controller,useForm } from 'react-hook-form';
import CHIconTitle from '../../../components/page-title-icon/page-title-icon';
import CHSubContainer from '../../../components/sub-container/sub-container'
import CHContainerSubtitle from '../../../components/container-subtittle/container-subtittle'
import CHContainerControl from '../../../components/container-control/container-control'
import CHSelect from '../../../components/ch-select/CHSelect';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { IDENTIFICATIONTYPEDTO } from '../../../types/catalog.type';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import HeaderLogin from '../../../components/header/Header';
import { AppState } from '../../../store/rootReducer';
import {
  startGetAllTypeDocuments
} from '../../../actions/catalog.action';
import './recuperacion-contrasena.scss';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { startRecuperaContrasena } from '../../../actions/user.action';
import { Recuperarcontrasena } from '../../../types/login.type';
import history from '../../../utils/history.router';




const useStyles = makeStyles((theme: Theme) =>
    createStyles({

      root: {
        flexGrow: 1,
    },
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917'
        },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#43b5f1'
        },
        processButton: {
            backgroundColor: '#1C3F8B',
            color: '#fff',
            margin: '8px 0',
        },

        boxForm: {
            textAlign: 'center',
            margin: theme.spacing(20, 0, 20, 0),
          


        },

        containerButton:{

            display: 'flex',
            flexDirection: 'row',
            alignItems: 'center',
            justifyContent: 'center',
            marginTop:' 3rem',
        }
         

    }),
);




const RecuperacionContrasena = ({

  catalogState,
  startGetAllTypeDocuments,
  startRecuperaContrasena,


}:{

  catalogState: CatalogState;
  startRecuperaContrasena:(data:Recuperarcontrasena)=> void,
  startGetAllTypeDocuments:() => void;

}) => {

    const classes = useStyles();

    const { control,handleSubmit } = useForm({
      defaultValues: {
          tipo: '',
          numero: '',
      },
  });

  useEffect(() => {
    startGetAllTypeDocuments();      
}, []);

const onCancel=()=>{
    history.push('/');
}


  const onSubmit = (data:Recuperarcontrasena) => {
    data.url=`${window.location.origin}/RestablecerContrasena/`;
    startRecuperaContrasena(data);   
  }

  return(
    <div className={classes.root}>

    <HeaderLogin />

    <Grid container spacing={4} className={ classes.boxForm }>
  
      <CHIconTitle
         subTitle='De Contraseña'
         title='Recuperación'
         icon='fch-Recuperacion-Contrasena-0-1'
        />

<CHPaperNew>
                <CHSubContainer>

                <CHContainerSubtitle
                  subTitle='Documento'
                  title='Tipo de '
                  icon='fch-Tipo-Documento-2-1'     
                />
                 
                 <CHContainerControl>

                 <Controller
                                            name="tipo"
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
                                                    label=""
                                                    value={value}
                                                    helperText={error ? error.message : null}
                                                    onChange={(value) => {
                                                        onChange(value);
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

                   
                  </CHContainerControl>    
                
                </CHSubContainer>


                <CHSubContainer>

                <CHContainerSubtitle
                  subTitle='Documento'
                  title='Número de '
                  icon='fch-Numero-Documento-2-1'     
                />
                 
                 <CHContainerControl>
                 <Controller
                        name="numero"
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
                                helperText={error ? error.message : null}
                                isReadOnly={false}
                                label=""
                                value={value}
                                onChange={onChange}
                                />
                        )}
                    />
                  </CHContainerControl>    

                  </CHSubContainer>

                  <Grid className='buttonssolicitud' >
                        <Button
                            className={classes.buttonprincipal}
                            variant="outlined"
                            onClick={handleSubmit(onSubmit)}       
                        >
                            RECUPERAR CONTRASEÑA
                        </Button>

                        <Button
                            className={classes.buttonsecundario}
                            variant="outlined"
                            onClick={onCancel}

                        >
                           CANCELAR
                        </Button>
                    </Grid>
                
                    </CHPaperNew>

               
           
                </Grid>
             
           </div>
            
          );
        }
    
    const mapStateToProps = (state: AppState) => ({
        catalogState: state.catalog,
    });

    const mapDispatchToProps = {
         startRecuperaContrasena,
        startGetAllTypeDocuments,
    }
    
    

export default connect(mapStateToProps, mapDispatchToProps)(RecuperacionContrasena);


