import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    TextField,
    Typography,
} from '@material-ui/core';
import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { startUpdateRateParametrization } from '../../../actions/parametrization.action';
import '../../../estiloprincipal.scss';
import { AppState } from '../../../store/rootReducer';
import AlertDialog from '../../../components/dialog/dialog.component';
import { isNumeric } from '../../../utils/regex';
import { LoginState } from '../../../reducers/login.reducer';
import { ParametrizacionState } from '../../../reducers/parametrization.reducer';
import { TasaDto } from '../../../types/parametrization.model.type';
import { RateForm } from '../constants/form.model.type';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHPaper from '../../../components/ch-paper/CHPaper';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
    }),
);

const Rate = (props: Props) => {
    const classes = useStyles();
    const [errorInput1, setErrorInput1] = useState(false);
    const [errorInput2, setErrorInput2] = useState(false);
    const [formValues, setFormValues] = useState(RateForm);

    const {parametrizationState ,loginState, startUpdateRateParametrization } = props;

    useEffect(() => {
        if (formValues?.id <= 0 && parametrizationState.rate?.id > 0) {
            setFormValues(parametrizationState.rate);
        }
    });

    const handleErrorText = (setErrorInput: any) => {
        setErrorInput(true);
        setTimeout(() => {
            setErrorInput(false);
        }, 2000);
    };

    const handleonblur = ({ target }: any) => {
        if (!isNumeric(target.value)) {
            alert('Los caracteres son incorrectos');
            setFormValues({
                ...formValues,
                [target.name]: '0',
            });
        }
    };

    const handleInputChange = (
        { target }: any,
        tasaUsuraNM: number,
        tasaCorrienteNM: number,
    ) => {
        let taxUsuraNumber = 0;
        let taxCorrienteNumber = 0;
        let proceso: number;
        if (tasaUsuraNM === 1) {
            proceso = 1 + target.value / 100;
            taxUsuraNumber = (proceso ** (1 / 12) - 1) * 12;
            taxUsuraNumber = (taxUsuraNumber / 12) * 100;
            taxUsuraNumber = parseFloat(taxUsuraNumber.toFixed(2));
            if (target.value <= 100) {
                setFormValues({
                    ...formValues,
                    [target.name]: !Number.isNaN(target.value)
                        ? parseFloat(target.value)
                        : '',
                    tasaUsuraNM: taxUsuraNumber,
                });
            } else handleErrorText(setErrorInput1);
        } else if (tasaCorrienteNM === 1) {
            proceso = 1 + target.value / 100;
            taxCorrienteNumber = (proceso ** (1 / 12) - 1) * 12;
            taxCorrienteNumber = (taxCorrienteNumber / 12) * 100;
            taxCorrienteNumber = parseFloat(taxCorrienteNumber.toFixed(2));
            if (target.value <= 100) {
                setFormValues({
                    ...formValues,
                    [target.name]: !Number.isNaN(target.value)
                        ? parseFloat(target.value)
                        : '',
                    tasaCorrienteNM: taxCorrienteNumber,
                });
            } else handleErrorText(setErrorInput2);
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
            bodyText:
                '¿Está seguro que quiere Actualizar las Tasas de interes?',
            onConfirm: () => {
                updateRateParametrization();
            },
        });
    };

    const updateRateParametrization = () => {
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false,
        });

        startUpdateRateParametrization(
            formValues,
            loginState.claims,
        );
    };

    return (
        <CHPrivateContainer>
            <CHPaper title="Parametrización de tasas">
                <Typography
                    variant="subtitle1"
                    align="center"
                    className="titulo1"
                >
                    Tasa{' '}
                </Typography>
                <Typography
                    variant="subtitle1"
                    align="center"
                    className="titulo2"
                >
                    Usura
                </Typography>
                <Grid container spacing={3}>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Typography>E.A</Typography>
                        <TextField
                            name="tasaUsuraEA"
                            value={formValues.tasaUsuraEA}
                            inputProps={{ min: 0, max: 100 }}
                            onChange={e => handleInputChange(e, 1, 0)}
                            onBlur={handleonblur}
                            type="number"
                            helperText={errorInput1 && 'Fuera de límites'}
                            label="%"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Typography>N.M</Typography>
                        <TextField
                            name="tasaUsuraNM"
                            type="number"
                            label="%"
                            value={formValues.tasaUsuraNM}
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
                    Tasa{' '}
                </Typography>
                <Typography
                    variant="subtitle1"
                    align="center"
                    className="titulo2"
                >
                    Corriente
                </Typography>
                <Grid container spacing={3}>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Typography>E.A</Typography>
                        <TextField
                            name="tasaCorrienteEA"
                            value={formValues.tasaCorrienteEA}
                            inputProps={{ min: 0, max: 100 }}
                            onChange={e => handleInputChange(e, 0, 1)}
                            onBlur={handleonblur}
                            type="number"
                            helperText={errorInput2 && 'Fuera de límites'}
                            label="%"
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={4}>
                        <Typography>N.M</Typography>
                        <TextField
                            name="tasaCorrienteNM"
                            type="number"
                            label="%"
                            value={formValues.tasaCorrienteNM}
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
    startUpdateRateParametrization: (insurance: TasaDto, props: string) => void;
}

const mapDispatchToProps = {
    startUpdateRateParametrization,
};

type Props = LinkMapStateToProps & LinkMapDispatchToProps;

export default connect(mapStateToProps, mapDispatchToProps)(Rate);
