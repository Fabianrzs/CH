import { connect } from 'react-redux';
import {
    Backdrop,
    CircularProgress,
    Divider,
    makeStyles,
} from '@material-ui/core';
import React, { useEffect } from 'react';
import { AppState } from '../../store/rootReducer';
import { startGetAllRoles } from '../../actions/rol.action';
import FormRoles from './components/form.component';
import TableRoles from './components/table.component';
import { LoginState } from '../../reducers/login.reducer';

const Rol = (props: Props) => {
    const { loading, loginState, startGetAllRoles } = props;
    const useStyles = makeStyles(theme => ({
        backdrop: {
            zIndex: theme.zIndex.drawer + 1,
            color: '#fff',
        },
    }));
    const classes = useStyles();

    useEffect(() => {
        startGetAllRoles(loginState.claims);
    }, []);

    return (
        <div>
            <Backdrop className={classes.backdrop} open={loading}>
                <CircularProgress color="inherit" />
            </Backdrop>
            <h1>Roles</h1>
            <FormRoles />
            <Divider />
            <br />
            <TableRoles />
        </div>
    );
};

const mapStateToProps = (state: AppState) => ({
    rolState: state.rol,
    loginState: state.login,
});

const mapDispatchToProps = {
    startGetAllRoles,
};

type Props = {
    loginState: LoginState;
    loading: boolean;
    startGetAllRoles: (claims: string) => void;
};

export default connect(mapStateToProps, mapDispatchToProps)(Rol);
