import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { createStyles, makeStyles} from '@material-ui/core';
import { AppState } from '../../store/rootReducer';
import PageIndicators from '../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../components/page-title-icon/page-title-icon';
import CHPaperNew from '../../components/ch-paper-new/CHPaperNew';
import {startGetTablasAuditoria,startGetLogAuditoria} from '../../actions/auditoria.action';
import { ConsultaAuditoria } from '../../types/Auditoria.model.type';
import { AuditoriaState } from '../../reducers/auditoria.reducer';
import TableDetalleAuditoria from './components/tabladetalleAudiroria';
import { UserState } from '../../reducers/user.reducer';


const useStyles = makeStyles(() =>
    createStyles({

    root: {
        flexGrow: 1,
    },

    }),
);


const DetalleAuditoria = (props: Props)=>{

    const classes = useStyles();
    const {auditoriaState,startGetTablasAuditoria,startGetLogAuditoria, usersState} = props;
    useEffect(() => {
        startGetTablasAuditoria(); 
        const request = { 
            VerDetalle: true,
            Agrupador:  auditoriaState.detalle.agrupador
        } as ConsultaAuditoria;

        startGetLogAuditoria(request);

    }, []); 

 
  return(
    <div className={classes.root}>

      <PageIndicators
        topTextNivel1='Auditoria'
        topTextNivel2='Detalle Auditoria'/>

    <IconTitle
         subTitle='Auditoria'
         title='Detalle'
         icon='fch-Administracion-Grupos-Usuarios-0-1'
        />
<CHPaperNew>
 <TableDetalleAuditoria
 detalle={auditoriaState.logs.auditorias}
 usersState={usersState}
 />
</CHPaperNew>

</div>
            
 );

}


interface LinkMapStateToProps {
    auditoriaState: AuditoriaState ;
    usersState: UserState;
}

const mapStateToProps = (state: AppState) => ({
    auditoriaState:state.auditoria,
    usersState: state.users
});

interface LinkMapDispatchToProps {
    startGetTablasAuditoria: () => void;
    startGetLogAuditoria:(auditoria: ConsultaAuditoria)=>void;
  
}

const mapDispatchToProps = {  
    startGetTablasAuditoria,
    startGetLogAuditoria,  
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(DetalleAuditoria);