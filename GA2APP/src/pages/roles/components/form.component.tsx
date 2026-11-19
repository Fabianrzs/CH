import React, { useState } from 'react';
import {
    Button,
    Container,
    Grid,
    Paper,
    TextField,
    Typography,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { AppState } from '../../../store/rootReducer';
import { ROLDTO } from '../../../types/rol.type';

import { startAddRol, startGetAllRoles } from '../../../actions/rol.action';
import { LoginState } from '../../../reducers/login.reducer';

const FormRoles = (props: Props) => {
 
    const {  startAddRol, startGetAllRoles, loginState } = props;
    const [descripcion, setDescripcion] = useState<any>();

    const submitHandler = async () => {
        if (descripcion.validity.valid) {
            const rol = {
                descripcion: descripcion.value,
            } as ROLDTO;
            await startAddRol(rol, loginState.claims);
            descripcion.clear();
            await startGetAllRoles(loginState.claims);
        } else {
            console.log(descripcion.value);
        }
    };

    const changeValue = (value: string) => {
        setDescripcion(value);
    };

    return (
        <Container maxWidth="md" className="container-users">
            <Paper className="paper-users">
                <Typography component="h1" variant="h5">
                    Crear Rol
                </Typography>
                <Grid item xs={12}>
                    <form className="form-users">
                        <Grid container justify="space-between">
                            <TextField
                                key="descripcion"
                                inputRef={e => changeValue(e)}
                                margin="normal"
                                required
                                label="descripción"
                                name="descripcion"
                                autoFocus
                            />
                        </Grid>
                        <Grid>
                            <Button
                                type="button"
                                onClick={submitHandler}
                                variant="contained"
                                color="primary"
                            >
                                Crear
                            </Button>
                        </Grid>
                    </form>
                </Grid>
            </Paper>
        </Container>
    );
};

const mapStateToProps = (state: AppState) => ({
    rolState: state.rol,
    loginState: state.login,
});

const mapDispatchToProps = {
    startAddRol,
    startGetAllRoles,
};

type Props = {
    loginState: LoginState, 
    startAddRol : (rol: ROLDTO, props: string) => void ,
    startGetAllRoles: (props: string) => void,
    
}

export default connect(mapStateToProps, mapDispatchToProps)(FormRoles);
