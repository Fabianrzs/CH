import React, { useEffect, useState }  from 'react';
import swal from 'sweetalert';
import { connect } from 'react-redux';
import { Controller,useForm } from 'react-hook-form';
import { Button, createStyles, Grid, makeStyles} from '@material-ui/core';
import { AppState } from '../../../../store/rootReducer';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../../components/ch-paper-new/CHPaperNew';
import { startActualizaGrupoOpciones , startGetAllOpciones} from '../../../../actions/grupoUsers.action';

import CHSubContainer from '../../../../components/sub-container/sub-container';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import CHContainerControl from '../../../../components/container-control/container-control';
import CHTextField from '../../../../components/ch-text-field/CHTextField';
import { GrupoUsersModel } from '../../../../types/grupoUsers.model';
import { GrupoOpcionesState } from '../../../../reducers/grupo-opciones.reducer';
import history from '../../../../utils/history.router';
import TableOpciones from '../table-opciones/table-opciones';
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


const ActualizaGrupoOpciones = (props: Props)=>{

    const {grupoOpcionesState,startActualizaGrupoOpciones, startGetAllOpciones} = props;
   
    const classes = useStyles();
    const [listOptions, setListOptions]=useState<any[]>([]);
    const [subOptions, setSubOptions] = useState<any[]>([]);

    const[allOpciones, setAllOpciones] = useState(grupoOpcionesState.allOpciones)


    const setOpciones = (value: GrupoOpcionesModel) => {

        if(listOptions.findIndex(x => x.id === value.id) === -1 ){
            setListOptions(x => [...x, value]); 
         }else{
            setListOptions(listOptions.filter(x => x.id !== value.id));   
         }     
    }

    const setSubOpciones = (value: any) => {
        if(subOptions.findIndex(x => x.id === value.id) === -1 ){
            setSubOptions(x => [...x, value]); 
         }else{
            setSubOptions(subOptions.filter(x => x.id !== value.id));   
         }   
    }
    

    const { control,handleSubmit} = useForm({
        defaultValues: {
            codigo: grupoOpcionesState.opcion.codigo,
            estado: grupoOpcionesState.opcion.estado,
            nombre: grupoOpcionesState.opcion.nombre,
            descripcion:grupoOpcionesState.opcion.descripcion,
                   
        },
    });

    const onCancel=()=>{
        history.push('/GrupoOpciones');
    }

    useEffect(() => {
        startGetAllOpciones();

        setAllOpciones(grupoOpcionesState.allOpciones)

        if(grupoOpcionesState.opcion.opciones.length > 0){
            grupoOpcionesState.opcion.opciones.forEach((value) => {  
               if(listOptions.findIndex(x => x.id === value.id) === -1 ){
                    setListOptions(x => [...x, value]); 
                    value.subOpciones.forEach(element => {
                        if(subOptions.findIndex(x => x.codigo === element.codigo) === -1){
                            setSubOptions(x => [...x, element]); 
                        }
                    });
                }  
    
            })
        }
    }, []);


    const onSubmit = (data:any) => {

        console.log(listOptions)
        console.log(subOptions)

        listOptions.forEach(element => {
            element.subOpciones = subOptions.filter(x => x.idOpcion === element.id)
        });

        setListOptions(listOptions)

        if(listOptions.length > 0 || subOptions.length > 0){  

            swal({
                text: "¿Quieres actualizar este grupo de opciones?",
                icon: "info",
                buttons: ["No","Sí"]
            }).then (async respuesta =>{
                if(respuesta){   
                    
                    data.id= grupoOpcionesState.opcion.id; 
                    data.codigo=grupoOpcionesState.opcion.codigo;
                    data.opciones=listOptions;    
                    data.estado="ACTIVO";

                    startActualizaGrupoOpciones(data);
                        
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
                    data.id= grupoOpcionesState.opcion.id; 
                    data.codigo=grupoOpcionesState.opcion.codigo;

                    startActualizaGrupoOpciones(data);             
                }
            })
        }
    }


  
  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='actualizar Grupo de Opciones'/>

    <IconTitle
         subTitle='Grupo de Opciones'
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
            field: { onChange, value},
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
<TableOpciones
  Allopciones={allOpciones}
  OpcionsGroup={grupoOpcionesState.opcion.opciones }
  setOpciones={setOpciones}
  setSubOpciones={setSubOpciones}
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
   
    grupoOpcionesState:GrupoOpcionesState;

}

const mapStateToProps = (state: AppState) => ({
    grupoOpcionesState:state.grupoOpciones,
    
});

interface LinkMapDispatchToProps {
  
    startActualizaGrupoOpciones :(grupo:GrupoUsersModel)=>void
    startGetAllOpciones :()=> void
   
}

const mapDispatchToProps = {
    startActualizaGrupoOpciones,
    startGetAllOpciones
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(ActualizaGrupoOpciones);