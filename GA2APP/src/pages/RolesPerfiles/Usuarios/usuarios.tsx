import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { createStyles, makeStyles} from '@material-ui/core';
import { AppState } from '../../../store/rootReducer';
import { startLoadUsers,AddUser,startDeleteUser } from '../../../actions/user.action';
import { UserState  } from '../../../reducers/user.reducer';
import { LoginState } from '../../../reducers/login.reducer';
import PageIndicators from '../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../../components/ch-paper-new/CHPaperNew';
import TableUsers from './components/table-users/table-users';
import { User } from '../../../types/user.type';
import history from '../../../utils/history.router';
import { LoadingComponent } from '../../../components/loading/loading.component';

const useStyles = makeStyles(() =>
    createStyles({

    root: {
        flexGrow: 1,
    },

    }),
);


const AdminUsuarios = (props: Props)=>{

    const {loginState,users, startLoadUsers,AddUser,startDeleteUser } = props;
    const classes = useStyles();

    useEffect(() => {
        startLoadUsers(loginState.claims);
    }, []);

    const editarUser=(user:User)=>{
        AddUser(user);
        history.push("/ActualizarUsuario");
    };

    const eliminarUser=async (user:User)=>{
        await startDeleteUser( loginState.claims,user);
        startLoadUsers(loginState.claims);

    

    };

  return(
    <div className={classes.root}>
      <LoadingComponent loading={users.loading} />
      <PageIndicators
        topTextNivel1='Roles y permisos'
        topTextNivel2='Administración de Usuarios'/>

    <IconTitle
         subTitle='De Usuarios'
         title='Administración'
         icon='fch-Administracion-Usuarios-0-1'
        />
        
<CHPaperNew>
 
  <TableUsers
  
  users={users.users}
  setEditUser={editarUser}
  deleteUser={ eliminarUser }
  />
</CHPaperNew>

   
   </div>
            
 );
}

interface LinkMapStateToProps {
    loginState: LoginState;
    users: UserState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
        users:state.users,
});

interface LinkMapDispatchToProps {
    startLoadUsers: (props: string) => void;
    AddUser: (user:User) => void;
    startDeleteUser:(props: string, id:User)=>void
}

const mapDispatchToProps = {
    
    AddUser,
    startLoadUsers,
    startDeleteUser,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(AdminUsuarios);