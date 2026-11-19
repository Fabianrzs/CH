import { Button, Grid, TextField, Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import '../../../estiloprincipal.scss';
import { AppState } from '../../../store/rootReducer';
import { startUpdateInsuranceParametrization } from '../../../actions/parametrization.action';
import AlertDialog from '../../../components/dialog/dialog.component';
import { isNumeric } from '../../../utils/regex';
import { SeguroDto } from '../../../types/parametrization.model.type';
import { InsuranceForm } from '../constants/form.model.type';
import { LoginState } from '../../../reducers/login.reducer';
import { ParametrizacionState } from '../../../reducers/parametrization.reducer';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../../components/ch-paper/CHPaper';

const Insurance = (props: Props) => {
    const [formState, setFormState] = useState(InsuranceForm);

    const {parametrizationState, loginState, startUpdateInsuranceParametrization } = props;

    const [confirmDialog, setConfirmDialog] = useState({
        isOpen: false,
        title: '',
        bodyText: '',
        onConfirm: () => {},
    });

    useEffect(() => {
        if (
            formState?.id <= 0 &&
            parametrizationState.insurance?.id > 0
        ) {
            setFormState(parametrizationState.insurance);
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
        if (target.value <= 100) {
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
                updateInsuranceParametrization();
            },
        });
    };

    const updateInsuranceParametrization = () => {
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false,
        });

        startUpdateInsuranceParametrization(
            formState,
            loginState.claims,
        );
    };

    return (
        <CHPrivateContainer>
            <CHPaper title="Parametrización de seguros">
                <Grid container>
                    <Grid item xs={6}>
                        <Typography>Seguro de vida</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <TextField
                            type="number"
                            name="seguroVida"
                            onChange={handleInputChange}
                            onBlur={handleonblur}
                            label="%"
                            InputLabelProps={{
                                shrink: true,
                            }}
                            value={formState.seguroVida}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={3}>
                    <Grid item xs={6}>
                        <Typography>Seguros Todo Riesgo</Typography>
                    </Grid>
                    <Grid item xs={6}>
                        <TextField
                            type="number"
                            name="seguroTodoRiesgo"
                            label="%"
                            onChange={handleInputChange}
                            InputLabelProps={{
                                shrink: true,
                            }}
                            value={formState.seguroTodoRiesgo}
                            onBlur={handleonblur}
                        />
                    </Grid>
                </Grid>
                <Grid container>
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
    startUpdateInsuranceParametrization: (
        insurance: SeguroDto,
        props: string,
    ) => void;
}

const mapDispatchToProps = {
    startUpdateInsuranceParametrization,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Insurance);
