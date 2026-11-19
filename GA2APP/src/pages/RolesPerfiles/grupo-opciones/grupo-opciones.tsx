import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { createStyles, makeStyles} from '@material-ui/core';
import { AppState } from '../../../store/rootReducer';

import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { startGetAllGrupoOpciones,startDeleteGrupoOpciones,addGrupoOpcion} from '../../../actions/grupoUsers.action';
import { GrupoOpcionesState } from '../../../reducers/grupo-opciones.reducer';
import TableGroupoOpciones from './table-grupo-opciones/table-grupo-opciones'
import { GrupoUsersModel } from '../../../types/grupoUsers.model';
import history from '../../../utils/history.router'; 


const useStyles = makeStyles(() =>
    createStyles({

    root: {
        flexGrow: 1,
    },

    }),
);


const AdminGrupoOpciones = (props: Props)=>{

 
    const classes = useStyles();
    const {grupoOpciones,addGrupoOpcion,startGetAllGrupoOpciones,startDeleteGrupoOpciones} = props;

    useEffect(() => {
        startGetAllGrupoOpciones();  
    }, []); 


    const setEdit=(value:GrupoUsersModel)=>{  

        addGrupoOpcion(value);  
        history.push("/ActualizarGrupoOpciones");    
    } 

   
    const setEliminar=async (value:GrupoUsersModel) =>{  
       
        
       await startDeleteGrupoOpciones(value); 
       startGetAllGrupoOpciones();    
            
    } 




  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='Administración de Grupos de Opciones'/>

    <IconTitle
         subTitle='De Grupos De Opciones'
         title='Administración'
         icon='fch-Administracion-Grupos-Usuarios-0-1'
        />



<CHPaperNew>
 
   <TableGroupoOpciones
     grupo={grupoOpciones.allgrupoOpciones }
     setEditGrupo={ setEdit}
     deleteGrupo={ setEliminar}
 
     
   />
 
</CHPaperNew>

   
   </div>
            
 );
}


interface LinkMapStateToProps {
    grupoOpciones: GrupoOpcionesState;
}

const mapStateToProps = (state: AppState) => ({
    grupoOpciones:state.grupoOpciones,
    
});

interface LinkMapDispatchToProps {
    startGetAllGrupoOpciones: () => void;
    startDeleteGrupoOpciones:(grupo:GrupoUsersModel)=>void;
    addGrupoOpcion:(grupo:GrupoUsersModel)=>void;
}

const mapDispatchToProps = {
    
    startGetAllGrupoOpciones,
    startDeleteGrupoOpciones,
    addGrupoOpcion,
  
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(AdminGrupoOpciones);