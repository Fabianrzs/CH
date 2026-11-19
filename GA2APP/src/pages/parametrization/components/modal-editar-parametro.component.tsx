import React, { useEffect } from 'react';
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
import { AppState } from '../../../store/rootReducer';
import { startActualizarParametrosTransaccion } from '../../../actions/parametrization.action';
import { LoginState } from '../../../reducers/login.reducer';
import { ParametroTransaccionDto } from '../../../types/parametrization.model.type';

export const ModalEditarParametro = (props: Props) => {
    const { loginState, handleCloseModalEditar, conceptoState, setConceptoState, startActualizarParametrosTransaccion } =
        props;

    useEffect(() => {
        setConceptoState(conceptoState);
    }, [props]);

    const handleInputChange = ({ target }: any) => {
        setConceptoState({
            ...conceptoState,
            [target.name]: target.value,
        });
    };

    const handleGuardarEditado = async () => {
        await startActualizarParametrosTransaccion(conceptoState, loginState.claims);
        handleCloseModalEditar();
    };

    return (
        <Container>
            <DialogTitle style={{ cursor: 'move' }} id="draggable-dialog-title">
                Editar
            </DialogTitle>
            <DialogContent>
                <Grid container spacing={2}>
                    <Grid item xs={4}>
                        <FormControl fullWidth>
                            <TextField
                                name="paraM_TRANS_CONCEPTO"
                                value={conceptoState?.paraM_TRANS_CONCEPTO}
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
                                value={conceptoState?.paraM_TRANS_CODIGO}
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
                                value={conceptoState?.paraM_TRANS_PROCESO}
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
                    onClick={handleCloseModalEditar}
                    color="primary"
                >
                    Cerrar
                </Button>
                <Button onClick={handleGuardarEditado} color="primary">
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
    startActualizarParametrosTransaccion,
};

type Props = {
    loginState: LoginState,
    startActualizarParametrosTransaccion: (parametro: ParametroTransaccionDto, props: string) => void,
    handleCloseModalEditar: any,
    conceptoState: any,
    setConceptoState: any
}

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(ModalEditarParametro);
