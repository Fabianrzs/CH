import React, { useState }  from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { Controller,useForm } from 'react-hook-form';
import { Button, createStyles, Grid, makeStyles} from '@material-ui/core';
import { AppState } from '../../../../store/rootReducer';
import { LoginState } from '../../../../reducers/login.reducer';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../../components/ch-paper-new/CHPaperNew';
import { startAddGrupoOpciones } from '../../../../actions/grupoUsers.action';
import CHSubContainer from '../../../../components/sub-container/sub-container';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import CHContainerControl from '../../../../components/container-control/container-control';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { GrupoUsersModel } from '../../../../types/grupoUsers.model';
import history from '../../../../utils/history.router';
import TableOpciones from '../table-opciones/table-opciones';
import { GrupoOpcionesState } from '../../../../reducers/grupo-opciones.reducer';
import { GrupoOpcionesModel } from '../../../../types/grupo-opciones.model.type';


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


const CrearGrupoOpciones = (props: Props)=>{

    const {loginState, grupoOpcionesState, startAddGrupoOpciones } = props;

    const onCancel=()=>{
        history.push('/GrupoOpciones');
    }
   
    const classes = useStyles();

    const { control,handleSubmit} = useForm({
        defaultValues: {
            codigo: '',
            estado: '',
            nombre: '',
            descripcion:'',
                   
        },
    });

    const [listOptions, setListOptions]=useState<any[]>([]);
    const [subOptions, setSubOptions] = useState<any[]>([]);

    const setOpciones = (value: GrupoOpcionesModel) => {

        if(listOptions.findIndex(x => x.id === value.id) === -1 ){
            listOptions.push(value); 
         }else{
            listOptions.splice(listOptions.findIndex(x => x.id === value.id), 1);   
         }     
    }

    const setSubOpciones = (value: any) => {
        if(subOptions.findIndex(x => x.id === value.id) === -1 ){
            setSubOptions(x => [...x, value]); 
         }else{
            setSubOptions(subOptions.filter(x => x.id !== value.id));   
         }   
    }   

    const onSubmit = (data:any) => {

            console.log(listOptions)
            console.log(subOptions)

            listOptions.forEach(element => {
                element.subOpciones = subOptions.filter(x => x.idOpcion === element.id)
            });
    
            setListOptions(listOptions)

            swal({
                text: "¿Quieres guardar grupo de opciones?",
                icon: "info",
                buttons: ["No","Sí"]
            }).then (async respuesta =>{
                if(respuesta){   
                 
                    data.estado="ACTIVO";
                    data.opciones=listOptions;    
                    startAddGrupoOpciones(data);      
                }
            })
    }

  
  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='Crear Grupo de Opciones'/>

    <IconTitle
         subTitle='Grupo de Opciones'
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

 <TableOpciones
  Allopciones={ grupoOpcionesState.allOpciones}
  OpcionsGroup={[]}
  setOpciones={setOpciones}
  setSubOpciones={setSubOpciones}
/> 

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
    grupoOpcionesState:GrupoOpcionesState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    grupoOpcionesState:state.grupoOpciones,
});

interface LinkMapDispatchToProps {
  
    startAddGrupoOpciones :(grupo:GrupoUsersModel)=>void
   
}

const mapDispatchToProps = {
    startAddGrupoOpciones 
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(CrearGrupoOpciones);