import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { createStyles, makeStyles} from '@material-ui/core';
import { AppState } from '../../../store/rootReducer';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import { startGetAllOpciones } from '../../../actions/grupoUsers.action';
import { GrupoOpcionesState } from '../../../reducers/grupo-opciones.reducer';
import TableOpcion from './table-opciones';
import { GrupoOpcionesModel } from '../../../types/grupo-opciones.model.type';

const useStyles = makeStyles(() =>
    createStyles({

    root: {
        flexGrow: 1,
    },

    }),
);


const Opciones = (props: Props)=>{

    const {grupoOpcionesState,startGetAllOpciones } = props;
    const classes = useStyles();

    useEffect(() => {
        startGetAllOpciones();
    }, []);

  const editarOpcion=(value:GrupoOpcionesModel)=>{

    console.log(value);
        // AddUser(user);
       // history.push("/ActualizarUsuario");
    };

    const eliminarOpcion=async (value:GrupoOpcionesModel)=>{

        console.log(value);
      //  await startDeleteUser( loginState.claims,user);
       // startLoadUsers(loginState.claims);

    

    }; 

  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='Administración de Opciones'/>

    <IconTitle
         subTitle='De Opciones'
         title='Administración'
         icon='fch-Administracion-Usuarios-0-1'
        />
        
<CHPaperNew>
 
  <TableOpcion
  
  allOptiions={grupoOpcionesState.allOpciones }
  setEditGrupo={ editarOpcion}
  deleteGrupo={ eliminarOpcion}

  />

</CHPaperNew>

   
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
    startGetAllOpciones :()=> void
}

const mapDispatchToProps = {  
    startGetAllOpciones
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Opciones);