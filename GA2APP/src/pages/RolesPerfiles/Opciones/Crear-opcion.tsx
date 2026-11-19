import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { Controller,useForm } from 'react-hook-form';
import { Button, createStyles, Grid, makeStyles} from '@material-ui/core';
import { AppState } from '../../../store/rootReducer';
import { LoginState } from '../../../reducers/login.reducer';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import {  startAddOpciones } from '../../../actions/grupoUsers.action';
import CHSubContainer from '../../../components/sub-container/sub-container';
import CHContainerSubtitle from '../../../components/container-subtittle/container-subtittle';
import CHContainerControl from '../../../components/container-control/container-control';
import CHTextField from '../../../components/ch-text-field/CHTextField';
// import TableGroupUsers from '../component/table-grupo-opciones/table-grupo-opciones'
import { GrupoUsersModel } from '../../../types/grupoUsers.model';
// import { GrupoOpcionesState } from '../../../reducers/grupo-opciones.reducer';
import history from '../../../utils/history.router';

const useStyles = makeStyles(() =>
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

    }),
);


const CrearOpciones = (props: Props)=>{

    const {loginState, startAddOpciones} = props;
    const classes = useStyles();

    const { control,handleSubmit} = useForm({
        defaultValues: {
            codigo: '',
            estado: '',
            nombre: '',
            descripcion:'',
                   
        },
    });


    const onCancel=()=>{
        history.push('/Opciones');
    }

    const onSubmit = (data:GrupoUsersModel) => {

            swal({
                text: "¿Quieres guardar este opcion?",
                icon: "info",
                buttons: ["No","Sí"]
            }).then (async respuesta =>{
                if(respuesta){ 
                    data.estado="1";
                    startAddOpciones(data);         
                }
            })

    }



    console.log(loginState);
  
  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='Crear Opciones'/>

    <IconTitle
         subTitle='Opciones'
         title='Crear'
         icon='fch-Administracion-Grupos-Usuarios-0-1'
    />

            <CHPaperNew>
            <CHSubContainer>
            <Grid container>
<CHContainerSubtitle
subTitle='de Opción'
title='Código '
icon='fch-Codigo-grupo-0-1'     
/>
</Grid>
<Grid container>
<Controller
        name="codigo"
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
                type="text"
                value={value}
                onChange={onChange}
                />
        )}
    />
</Grid>    

</CHSubContainer>

<CHSubContainer>
<Grid container>
<CHContainerSubtitle
subTitle='de Opción'
title='Nombre'
icon='fch-Nombre-Grupo-0-1'     
/>
</Grid>
<Grid container>
<Controller
        name="nombre"
        control={control}
        rules={{

            required: {
                value: true,
                message: 'Este campo es requerido.',
            },
            pattern: /^[ A-zÀ-ú ]+$/g
         
        }}
        render={({
            field: { onChange, value },
            fieldState: { error },
        }) => (
            <CHTextField
                error={!!error}
                helperText={error ? 'El primer nombre solo debe contener letras' : null}
                isReadOnly={false}
                label=""
                value={value}
                onChange={onChange}
                />
        )}
    />
</Grid>    

</CHSubContainer>

<CHSubContainer>
<Grid container>
<CHContainerSubtitle
subTitle='de Opción'
title='Descripción'
icon='fch-Nombre-Grupo-0-1'     
/>
</Grid>
<Grid container>
<Controller
        name="descripcion"
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
                type="textarea"
                value={value}
                onChange={onChange}
                />
        )}
    />
</Grid>   

</CHSubContainer>
     
 </CHPaperNew>
 
 <br/>
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

 

   
   </div>
            
 );
}

interface LinkMapStateToProps {
    loginState: LoginState;
  
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
});

interface LinkMapDispatchToProps {
    startAddOpciones: (opcion:GrupoUsersModel) => void;
   
   
}

const mapDispatchToProps = {
    startAddOpciones,
 
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(CrearOpciones);