import React, { useEffect, useState } from 'react';
import { Button, TextField, Grid, Typography } from '@material-ui/core';
import { connect } from 'react-redux';
import { AppState } from '../../../store/rootReducer';
import { startUpdateTermParametrization } from '../../../actions/parametrization.action';
import AlertDialog from '../../../components/dialog/dialog.component';
import { isNumeric } from '../../../utils/regex';
import { LoginState } from '../../../reducers/login.reducer';
import { ParametrizacionState } from '../../../reducers/parametrization.reducer';
import { PlazosDto } from '../../../types/parametrization.model.type';
import { TermForm } from '../constants/form.model.type';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../../components/ch-paper/CHPaper';

const Term = (props: Props) => {
    const {parametrizationState, loginState, startUpdateTermParametrization } = props;
    const [errorInput1] = useState(false);
    const [errorInput2] = useState(false);
    const [formValues, setFormValues] = useState(TermForm);

    useEffect(() => {
        if (formValues?.id <= 0 && parametrizationState.term?.id > 0) {
            setFormValues(parametrizationState.term);
        }
    });

    const calcularPlazoMinimo = () => {
        let mesMin: number;
        mesMin = formValues.yearMin * 12;
        mesMin = parseFloat(mesMin.toFixed(2));

        setFormValues({
            ...formValues,
            monthMin: mesMin,
        });
    };

    const calcularPlazoMaximo = () => {
        let mesMax: number;
        mesMax = formValues.yearMax * 12;
        mesMax = parseFloat(mesMax.toFixed(2));

        setFormValues({
            ...formValues,
            monthMax: mesMax,
        });
    };

    const handleInputChange = ({ target }: any) => {
        if (target.value <= 100) {
            setFormValues({
                ...formValues,
                [target.name]: !Number.isNaN(target.value)
                    ? parseFloat(target.value)
                    : '',
            });
        }
    };

    const [confirmDialog, setConfirmDialog] = useState({
        isOpen: false,
        title: '',
        bodyText: '',
        onConfirm: () => {},
    });

    const handleOnClickSave = () => {
        setConfirmDialog({
            isOpen: true,
            title: 'Confirmación',
            bodyText: '¿Está seguro que quiere Actualizar los Plazos?',
            onConfirm: () => {
                updateTermParametrization();
            },
        });
    };

    const updateTermParametrization = () => {
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false,
        });

        startUpdateTermParametrization(
            formValues,
            loginState.claims,
        );
    };

    const handleonblur1 = ({ target }: any) => {
        if (!isNumeric(target.value)) {
            alert('Los caracteres son incorrectos ');
            setFormValues({
                ...formValues,
                [target.name]: '0',
            });
        } else {
            calcularPlazoMinimo();
        }
    };

    const handleonblur2 = ({ target }: any) => {
        console.log(isNumeric(target.value));
        if (!isNumeric(target.value)) {
            alert('Los caracteres son incorrectos');
            setFormValues({
                ...formValues,
                [target.name]: '0',
            });
        } else {
            calcularPlazoMaximo();
        }
    };

    return (
        <CHPrivateContainer>
            <CHPaper title="Parametrización de plazos">
                <Typography
                    variant="subtitle1"
                    align="center"
                    className="titulo1"
                >
                    Plazo{' '}
                </Typography>
                <Typography
                    variant="subtitle1"
                    align="center"
                    className="titulo2"
                >
                    Minimo
                </Typography>
                <Grid container spacing={3}>
                    <Grid item xs={6}>
                        <Typography>Años</Typography>
                        <TextField
                            name="yearMin"
                            value={formValues.yearMin}
                            inputProps={{ min: 0, max: 100 }}
                            helperText={errorInput1 && 'Fuera de límites'}
                            onChange={handleInputChange}
                            onBlur={handleonblur1}
                            type="number"
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Typography>Meses</Typography>
                        <TextField
                            name="monthMin"
                            id="mesMin"
                            type="number"
                            value={formValues.monthMin}
                            InputProps={{
                                readOnly: true,
                            }}
                            InputLabelProps={{
                                shrink: true,
                            }}
                        />
                    </Grid>
                </Grid>
                <Typography
                    variant="subtitle1"
                    align="center"
                    className="titulo1"
                >
                    Plazo{' '}
                </Typography>
                <Typography
                    variant="subtitle1"
                    align="center"
                    className="titulo2"
                >
                    Máximo
                </Typography>
                <Grid container spacing={3}>
                    <Grid item xs={6}>
                        <Typography>Años</Typography>
                        <TextField
                            name="yearMax"
                            value={formValues.yearMax}
                            inputProps={{ min: 0, max: 100 }}
                            helperText={errorInput2 && 'Fuera de límites'}
                            onChange={handleInputChange}
                            onBlur={handleonblur2}
                            type="number"
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <Typography>Meses</Typography>
                        <TextField
                            name="monthMax"
                            id="mesMax"
                            type="number"
                            value={formValues.monthMax}
                            InputProps={{
                                readOnly: true,
                            }}
                            InputLabelProps={{
                                shrink: true,
                            }}
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
                            Actualizar
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
    startUpdateTermParametrization: (
        insurance: PlazosDto,
        props: string,
    ) => void;
}

const mapDispatchToProps = {
    startUpdateTermParametrization,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Term);
