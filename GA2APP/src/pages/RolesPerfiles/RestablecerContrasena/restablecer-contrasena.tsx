import React, {  useEffect} from 'react';
import { connect } from 'react-redux';
import { useParams } from 'react-router-dom';
import { Button, createStyles, Grid, makeStyles, Theme} from '@material-ui/core';
import { Controller,useForm } from 'react-hook-form';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import CHContainerSubtitle from '../../../components/container-subtittle/container-subtittle';
import CHContainerControl from '../../../components/container-control/container-control';
import CHSubContainer from '../../../components/sub-container/sub-container';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import history from '../../../utils/history.router';
import NotificationService from '../../../services/notification.service';
import { NotificationCardModel, NotificationCardModelEnum } from '../../../models/requests/notification-cards-model';
import HeaderLogin from '../../../components/header/Header';
import { startValidaTokenContrasena,startActualizaContrasena } from '../../../actions/user.action';
import { UseRecuperaContrasena } from '../../../types/user.type';

interface ParamTypes {
    token: string;
  }



const useStyles = makeStyles((theme: Theme) =>
    createStyles({

    root: {
        flexGrow: 1,
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
    boxForm: {
        textAlign: 'center',
        margin: theme.spacing(30, 0, 30, 0),

    },

    }),
);


function  RestablecerContrasena({
  
    startValidaTokenContrasena,
    startActualizaContrasena
}:{

    startValidaTokenContrasena:(data:string)=>void;
    startActualizaContrasena:(data:UseRecuperaContrasena)=>void
}){



    const classes = useStyles();

  const { token } = useParams<ParamTypes>();

  
    const onSubmit = (data:any) => {

        

        if(data.contrasena!== data.repitaContrasena){
            NotificationService.showNotification({
                message: "La contraseña de verificación no coincide.",
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

            return;
        }

        startActualizaContrasena(data);

    }

    useEffect(() => {
         startValidaTokenContrasena(token);
    }, []);

    const { control,handleSubmit } = useForm({
        defaultValues: {
            contrasena:'',
            repitaContrasena:'',
            token: token        
        },
    });

    const onCancel=()=>{
        history.push('/');
    }

   
  return(
    <div className={classes.root}>

<HeaderLogin />

<Grid className={ classes.boxForm }>



<form  onSubmit={handleSubmit(onSubmit)}>


    <IconTitle
         subTitle='de contraseña'
         title='Recuperación'
         icon='fch-Recuperacion-Contrasena-0-1'
        />

        <CHPaperNew>

             <CHSubContainer>

            <CHContainerSubtitle
            subTitle='Contraseña'
            title='Crear'
            icon='fch-Contrasena-0-1'     
            />

            <CHContainerControl>
            <Controller
                    name="contrasena"
                    control={control}
                    rules={{
                        required: 'Este campo es requerido.',
                        minLength: {
                            value: 5,
                            message:
                                'La longitud mínima del campo es de 5 caracteres',
                        },
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
                            type="password"
                            value={value}
                            onChange={onChange}
                            />
                    )}
                />
            </CHContainerControl>    

            </CHSubContainer>    
            <CHSubContainer>
            <CHContainerSubtitle
            subTitle='Contraseña'
            title='Repetir'
            icon='fch-Repetir-Contrasena-0-1'     
            />

            <CHContainerControl>
            <Controller
                    name="repitaContrasena"
                    control={control}
                    rules={{
                        required: 'Este campo es requerido.',
                        minLength: {
                            value: 5,
                            message:
                                'La longitud mínima del campo es de 5 caracteres',
                        },
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
                            type="password"
                            value={value}
                            onChange={onChange}
                            />
                    )}
                />
            </CHContainerControl>    
            </CHSubContainer>
        </CHPaperNew> 


<br/>
 
        <Grid className='buttonssolicitud' >
                <Button
                    className={classes.buttonprincipal}
                    variant="outlined"  
                    onClick={handleSubmit(onSubmit)} 
                >
                    CREAR
                </Button>

                <Button
                    className={classes.buttonsecundario}
                    variant="outlined"
                    onClick={onCancel}
                >
                    CANCELAR
                </Button>
            </Grid>
   </form>

   </Grid>
   
   </div>
            
 );
}

/*

const mapStateToProps = (state: AppState) => ({
   // loginState: state.login,
   
});  */




const mapDispatchToProps = {
    startValidaTokenContrasena,
    startActualizaContrasena,
 
}  

export default connect(null, mapDispatchToProps)(RestablecerContrasena);