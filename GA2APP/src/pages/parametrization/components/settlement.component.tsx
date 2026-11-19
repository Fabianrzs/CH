import { Button, Grid, TextField, Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import '../../../estiloprincipal.scss';
import { AppState } from '../../../store/rootReducer';
import { startUpdateSettlementParametrization } from '../../../actions/parametrization.action';
import AlertDialog from '../../../components/dialog/dialog.component';
import { isNumeric } from '../../../utils/regex';
import { LiquidacionDto } from '../../../types/parametrization.model.type';
import { SettlementForm } from '../constants/form.model.type';
import { LoginState } from '../../../reducers/login.reducer';
import { ParametrizacionState } from '../../../reducers/parametrization.reducer';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../../components/ch-paper/CHPaper';

const Settlement = (props: Props) => {
    const [formState, setFormState] = useState(SettlementForm);
    const {parametrizationState, startUpdateSettlementParametrization, loginState} = props;

    const [confirmDialog, setConfirmDialog] = useState({
        isOpen: false,
        title: '',
        bodyText: '',
        onConfirm: {},
    });

    useEffect(() => {
        if (
            formState?.id <= 0 &&
            parametrizationState.settlement?.id > 0
        ) {
            setFormState(parametrizationState.settlement);
        }
    });

    const handleonblur = ({ target }: any) => {
        if (!isNumeric(target.value)) {
            alert('Los caracteres son incorrectos');
            setFormState({
                ...formState,
                [target.name]: '0',
            });
        }
    };

    const handleInputChange = ({ target }: any) => {
        if (target.value <= 31 && target.value >= 1) {
            setFormState({
                ...formState,
                [target.name]: target.value,
            });
        }
    };

    const handleOnClickSave = () => {
        setConfirmDialog({
            isOpen: true,
            title: 'Confirmación',
            bodyText: '¿Está seguro que quiere guardar esta parametrización?',
            onConfirm: () => {
                updateSettlementParametrization();
            },
        });
    };

    const updateSettlementParametrization = () => {
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false,
        });

        startUpdateSettlementParametrization(
            formState,
            loginState.claims,
        );
    };

    return (
        <CHPrivateContainer>
            <CHPaper title="Parametrización de liquidación">
                <Grid container spacing={3}>
                    <Grid item xs={6}>
                        <Typography>Fecha de Corte</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <TextField
                            type="number"
                            name="fechaCorte"
                            onChange={handleInputChange}
                            onBlur={handleonblur}
                            InputLabelProps={{
                                shrink: true,
                            }}
                            value={formState.fechaCorte}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={6}>
                        <Typography>Fecha de Pago</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <TextField
                            type="number"
                            name="fechaPago"
                            onChange={handleInputChange}
                            InputLabelProps={{
                                shrink: true,
                            }}
                            value={formState.fechaPago}
                            onBlur={handleonblur}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <Button
                            size="small"
                            variant="contained"
                            color="primary"
                            onClick={handleOnClickSave}
                        >
                            Guardar
                        </Button>
                    </Grid>
                </Grid>
            </CHPaper>
            <AlertDialog
                confirmDialog={confirmDialog}
                setConfirmDialog={setConfirmDialog}
            />
        </CHPrivateContainer>
    );
};

interface LinkMapStateToProps {
    loginState: LoginState;
    parametrizationState: ParametrizacionState;
}

const mapStateToProps = (state: AppState) => ({
    loginState: state.login,
    parametrizationState: state.parametrization,
});

interface LinkMapDispatchToProps {
    startUpdateSettlementParametrization: (
        Settlement: LiquidacionDto,
        props: string,
    ) => void;
}

const mapDispatchToProps = {
    startUpdateSettlementParametrization,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Settlement);
