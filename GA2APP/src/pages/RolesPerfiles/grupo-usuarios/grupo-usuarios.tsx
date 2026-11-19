import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { createStyles, makeStyles} from '@material-ui/core';
import { AppState } from '../../../store/rootReducer';

// import { UserState  } from '../../../reducers/user.reducer';
// import { LoginState } from '../../../reducers/login.reducer';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
// import { User } from '../../../types/user.type';
import { startGetAllGrupoUsuarios,startDeleteGrupoUsuario,AddGrupo  } from '../../../actions/grupoUsers.action';
import { GrupoUsuariosState } from '../../../reducers/grupousuarios.recducer';


// import { GrupoOpcionesState } from '../../../reducers/grupo-opciones.reducer'

import TableGroupUsers from './component/table-grupo-usuarios/table-grupo-usuarios';
import { GrupoUsersModel } from '../../../types/grupoUsers.model';
import history from '../../../utils/history.router'; 

const useStyles = makeStyles(() =>
    createStyles({

    root: {
        flexGrow: 1,
    },

    }),
);


const AdminGrupoUsuarios = (props: Props)=>{

    const {grupoUsuarios,startGetAllGrupoUsuarios,startDeleteGrupoUsuario,AddGrupo} = props;
    const classes = useStyles();

    useEffect(() => {
        startGetAllGrupoUsuarios();    
    }, []);


    const setEdit=(value:GrupoUsersModel)=>{  
        AddGrupo(value); 
        history.push("/ActualizarGrupoUsuario");    
    }

    const setEliminar=async (value:GrupoUsersModel) =>{  
       
       await startDeleteGrupoUsuario(value); 
       startGetAllGrupoUsuarios();   
            
    }


  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='Administración de Grupos de Usuarios'/>

    <IconTitle
         subTitle='De Grupos De Usuarios'
         title='Administración'
         icon='fch-Administracion-Grupos-Usuarios-0-1'
        />



<CHPaperNew>
   
   <TableGroupUsers
     grupo={grupoUsuarios.allgrupo }
     setEditGrupo={ setEdit}
     deleteGrupo={ setEliminar}
     
   />

</CHPaperNew>

   
   </div>
            
 );
}

interface LinkMapStateToProps {
  
    grupoUsuarios: GrupoUsuariosState;

}

const mapStateToProps = (state: AppState) => ({
    grupoUsuarios:state.grupoUsuarios,   
});

interface LinkMapDispatchToProps {
    startGetAllGrupoUsuarios: () => void;
    startDeleteGrupoUsuario:(grupo:GrupoUsersModel)=>void;
    AddGrupo:(grupo:GrupoUsersModel)=>void; 
}

const mapDispatchToProps = {
    
   startGetAllGrupoUsuarios,
   startDeleteGrupoUsuario,
   AddGrupo 
  
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(AdminGrupoUsuarios);