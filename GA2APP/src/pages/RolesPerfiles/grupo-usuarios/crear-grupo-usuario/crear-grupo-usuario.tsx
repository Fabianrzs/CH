import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { Controller,useForm } from 'react-hook-form';
import { Button, createStyles, Grid, makeStyles} from '@material-ui/core';
import { AppState } from '../../../../store/rootReducer';
import { LoginState } from '../../../../reducers/login.reducer';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../../components/ch-paper-new/CHPaperNew';
import { startGetAllGrupoOpciones,startAddGrupoUsuarios } from '../../../../actions/grupoUsers.action';
import CHSubContainer from '../../../../components/sub-container/sub-container';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import CHContainerControl from '../../../../components/container-control/container-control';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import TableGroupUsers from '../component/table-grupo-opciones/table-grupo-opciones'
import { GrupoUsersModel } from '../../../../types/grupoUsers.model';
import { GrupoOpcionesState } from '../../../../reducers/grupo-opciones.reducer';
import history from '../../../../utils/history.router';

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


const CrearGrupoUsuarios = (props: Props)=>{

    const {loginState,grupoOpciones,startGetAllGrupoOpciones,startAddGrupoUsuarios} = props;
    const [listGroup]=useState<GrupoUsersModel[]>([]);
    const classes = useStyles();



     useEffect(() => {
        startGetAllGrupoOpciones();    
    }, []); 

    const { control,handleSubmit} = useForm({
        defaultValues: {
            codigo: '',
            estado: '',
            nombre: '',
            descripcion:'',
                   
        },
    });


    const onCancel=()=>{
        history.push('/GrupoUsuarios');
    }

    const onSubmit = (data:any) => {

        if(listGroup.length > 0){  

            swal({
                text: "¿Quieres guardar este usuario?",
                icon: "info",
                buttons: ["No","Sí"]
            }).then (async respuesta =>{
                if(respuesta){   
                    
                    data.opciones=listGroup;
                    data.estado="ACTIVO";
                    startAddGrupoUsuarios(data);      
                }
            })
    
        }else{
            swal({
                text: "¿Quieres guardar este usuario sin asignarle un grupo?",
                icon: "warning",
                buttons: ["No","Sí"]
            }).then (respuesta =>{
                if(respuesta){              
                  
                    data.opciones=[];
                    data.estado="ACTIVO";
                    startAddGrupoUsuarios(data);  
                }
            })
        }

    }

    const setGroupOpciones = (value: GrupoUsersModel) => {

        if(listGroup.findIndex(x => x.id === value.id) === -1 ){
            listGroup.push(value); 
         }else{
            listGroup.splice(listGroup.findIndex(x => x.id === value.id), 1);   
         }     
    }


    console.log(loginState);
  
  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='Crear Grupo de Usuarios'/>

    <IconTitle
         subTitle='Grupo de Usuarios'
         title='Crear'
         icon='fch-Administracion-Grupos-Usuarios-0-1'
    />

            <CHPaperNew>
            <CHSubContainer>
            <Grid container>
<CHContainerSubtitle
subTitle='de Grupo'
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
subTitle='de Grupo'
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
subTitle='de Grupo'
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
 <CHPaperNew>
<TableGroupUsers
  grupo={ grupoOpciones.allgrupoOpciones}
  setGroupOpciones={setGroupOpciones}
  opcions={[]}
  
/>
    
 </CHPaperNew>

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
    grupoOpciones: GrupoOpcionesState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    grupoOpciones:state.grupoOpciones,
});

interface LinkMapDispatchToProps {
    startGetAllGrupoOpciones: () => void;
    startAddGrupoUsuarios:(grupo:GrupoUsersModel)=>void
   
}

const mapDispatchToProps = {
    startGetAllGrupoOpciones,
    startAddGrupoUsuarios
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(CrearGrupoUsuarios);