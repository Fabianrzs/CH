import React, { useEffect, useState } from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { Controller,useForm } from 'react-hook-form';
import { Button, createStyles, Grid, makeStyles} from '@material-ui/core';
import { AppState } from '../../../../store/rootReducer';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../../components/ch-paper-new/CHPaperNew';
import { startGetAllGrupoOpciones,startActualizaGrupoUsuario } from '../../../../actions/grupoUsers.action';
import CHSubContainer from '../../../../components/sub-container/sub-container';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import CHContainerControl from '../../../../components/container-control/container-control';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import TableGroupUsers from '../component/table-grupo-opciones/table-grupo-opciones'
import { GrupoUsersModel } from '../../../../types/grupoUsers.model';
import { GrupoOpcionesState } from '../../../../reducers/grupo-opciones.reducer';
import { GrupoUsuariosState } from '../../../../reducers/grupousuarios.recducer';
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


const ActualizarGrupoUsuarios = (props: Props)=>{

    const {grupoOpciones,grupoUsuario,startGetAllGrupoOpciones,startActualizaGrupoUsuario } = props;
    const [listGroup]=useState<any[]>([]);
    const classes = useStyles();

     useEffect(() => {
        startGetAllGrupoOpciones();    
    }, []); 

    const { control,handleSubmit} = useForm({
        defaultValues: {
            nombre: grupoUsuario.grupo.nombre,
            descripcion: grupoUsuario.grupo.descripcion,
            codigo: grupoUsuario.grupo.codigo,
                   
        },
    });


    if(grupoUsuario.grupo.opciones.length > 0){
        grupoUsuario.grupo.opciones.forEach((value) => {  
           if(listGroup.findIndex(x => x.id === value.id) === -1 ){
               listGroup.push(value); 
           }  

        })
    }

    const onSubmit = (data:any) => {

        if(listGroup.length > 0){  

            swal({
                text: "¿Quieres actualizar este grupo de usuario?",
                icon: "info",
                buttons: ["No","Sí"]
            }).then (async respuesta =>{
                if(respuesta){   
                    
                    data.id=grupoUsuario.grupo.id;
                    data.codigo=grupoUsuario.grupo.codigo;
                    data.opciones=listGroup;
            
                    data.estado="ACTIVO";
                    startActualizaGrupoUsuario(data);
                        
                }
            })
    
        }else{
            swal({
                text: "¿Quieres actualizar este grupo de usuarios sin asignarle grupo de opciones?",
                icon: "warning",
                buttons: ["No","Sí"]
            }).then (respuesta =>{
                if(respuesta){              
                  
                    data.opciones=[];
                    data.estado="ACTIVO" 
                    data.codigo=grupoUsuario.grupo.codigo;
                    data.id=grupoUsuario.grupo.id;

                    startActualizaGrupoUsuario(data);             
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


    const onCancel=()=>{
        history.push('/GrupoUsuarios');
    }


  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='Actualizar Grupo de Usurios'/>

    <IconTitle
         subTitle='Grupo de Usuarios'
         title='Actualizar'
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
                disabled
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
  opcions={grupoUsuario.grupo.opciones}
/>
    
 </CHPaperNew>

 <Grid className='buttonssolicitud' >
    <Button
        className={classes.buttonprincipal}
        variant="outlined"  
        onClick={handleSubmit(onSubmit)} 
    >
        ACTUALIZAR
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
 
    grupoOpciones: GrupoOpcionesState;
    grupoUsuario: GrupoUsuariosState;
}

const mapStateToProps = (state: AppState) => ({
    
    grupoOpciones:state.grupoOpciones,
    grupoUsuario:state.grupoUsuarios,
});

interface LinkMapDispatchToProps {
    startGetAllGrupoOpciones: () => void;
    startActualizaGrupoUsuario:(grupo:GrupoUsersModel)=> void;
  
   
}

const mapDispatchToProps = {
    startGetAllGrupoOpciones,
    startActualizaGrupoUsuario,
 
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ActualizarGrupoUsuarios);