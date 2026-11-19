import React, { useState } from 'react';
import {
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    FormControl,
    Grid,
    TextField,
    Container,
} from '@material-ui/core';
import { connect } from 'react-redux';
import { ParametroTransaccion } from '../constants/form.model.type';
import { startCrearParametrosTransaccion } from '../../../actions/parametrization.action';
import { AppState } from '../../../store/rootReducer';
import { ParametroTransaccionDto } from '../../../types/parametrization.model.type';
import { LoginState } from '../../../reducers/login.reducer';

export const ModalInsertarParametro = (props: Props) => {
    const { loginState,  handleCloseModalInsertar, startCrearParametrosTransaccion } = props;

    const [conceptoState, setConceptoState] = useState(ParametroTransaccion);

    const handleInputChange = ({ target }: any) => {
        setConceptoState({
            ...conceptoState,
            [target.name]: target.value,
        });
    };

    const handleGuardarNuevo = async () => {
        setConceptoState({
            ...conceptoState,
            paraM_TRANS_CREADO_POR: '00000000-0000-0000-0000-000000000000',
            paraM_TRANS_MODIFICADO_POR: '00000000-0000-0000-0000-000000000000',
        });
        await startCrearParametrosTransaccion(conceptoState, loginState.claims);
        handleCloseModalInsertar();
    };

    return (
        <Container>
            <DialogTitle style={{ cursor: 'move' }} id="draggable-dialog-title">
                Insertar
            </DialogTitle>
            <DialogContent>
                <Grid container spacing={2}>
                    <Grid item xs={4}>
                        <FormControl fullWidth>
                            <TextField
                                name="paraM_TRANS_CONCEPTO"
                                value={conceptoState.paraM_TRANS_CONCEPTO}
                                type="text"
                                multiline
                                variant="outlined"
                                label="Concepto"
                                onChange={handleInputChange}
                            />
                        </FormControl>
                    </Grid>
                    <Grid item xs={4}>
                        <FormControl fullWidth>
                            <TextField
                                name="paraM_TRANS_CODIGO"
                                type="number"
                                value={conceptoState.paraM_TRANS_CODIGO}
                                label="Código"
                                variant="outlined"
                                onChange={handleInputChange}
                            />
                        </FormControl>
                    </Grid>
                    <Grid item xs={4}>
                        <FormControl fullWidth>
                            <TextField
                                name="paraM_TRANS_PROCESO"
                                value={conceptoState.paraM_TRANS_PROCESO}
                                type="text"
                                multiline
                                variant="outlined"
                                label="Proceso"
                                onChange={handleInputChange}
                            />
                        </FormControl>
                    </Grid>
                </Grid>
            </DialogContent>
            <DialogActions>
                <Button
                    autoFocus
                    onClick={handleCloseModalInsertar}
                    color="primary"
                >
                    Cerrar
                </Button>
                <Button onClick={handleGuardarNuevo} color="primary">
                    Guardar
                </Button>
            </DialogActions>
        </Container>
    );
};

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
});

const mapDispatchToProps = {
    startCrearParametrosTransaccion,
};

type Props = {
    loginState: LoginState,
    handleCloseModalInsertar: any,
    startCrearParametrosTransaccion: (parametro: ParametroTransaccionDto, props: string) => void
}

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ModalInsertarParametro);
