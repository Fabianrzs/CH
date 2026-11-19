import React from 'react';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import Button from '@material-ui/core/Button';
import ArrowForwardIosIcon from '@material-ui/icons/ArrowForwardIos';
import history from '../../utils/history.router';
import Header from '../../components/header/Header';
import ImageValidationData from '../../assets/images/ImageValidationData.svg';

import './number_select.style.scss';

export default function Number_select() {
    const onNumberSuccess = () => {
        history.push('/validacion_otp');
    };

    return (
        <Grid container className="Centrado Sin-margen">
            <Grid item xs={12}>
                <Header />
            </Grid>
            <Grid item xs={5} className="Margen-arriba-15">
                <img
                    src={ImageValidationData}
                    alt=""
                    height="100%"
                    width="100%"
                />
            </Grid>
            <Grid item xs={7}>
                <Grid container>
                    <Grid item xs={12}>
                        <Typography
                            component="h1"
                            variant="h6"
                            className="Margen-arriba-30 Gris"
                        >
                            <ArrowForwardIosIcon style={{ fontSize: 16 }} />
                            Validación de identidad
                        </Typography>
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item xs={12}>
                        <Typography
                            component="h1"
                            variant="h5"
                            className="Margen-arriba-15 Azul"
                        >
                            Selección Número
                        </Typography>
                        <Typography
                            component="h1"
                            variant="h5"
                            className="Amarillo"
                        >
                            de Celular
                        </Typography>
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item xs={3} />
                    <Grid item xs={6}>
                        <Typography
                            component="h1"
                            variant="h6"
                            className="Margen-arriba-15 Gris"
                        >
                            Del siguiente listado selecciona el numero de tu
                            celular al cual deseas enviar el código de
                            verificación
                        </Typography>
                    </Grid>
                    <Grid item xs={3} />
                </Grid>
                <Grid container className="Margen-arriba-15">
                    <Grid item xs={12}>
                        <Grid container>
                            <Grid item xs={12}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            color="primary"
                                            name="saveAddress"
                                            value="yes"
                                        />
                                    }
                                    label="******7973"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            color="primary"
                                            name="saveAddress"
                                            value="yes"
                                        />
                                    }
                                    label="******5623"
                                />
                            </Grid>
                            <Grid item xs={12}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            color="primary"
                                            name="saveAddress"
                                            value="yes"
                                        />
                                    }
                                    label="******5623"
                                />
                            </Grid>
                            <Grid container className="Margen-arriba-30">
                                <Grid item xs={12} sm={4} />
                                <Grid item xs={12} sm={4}>
                                    <Button
                                        onClick={onNumberSuccess}
                                        type="button"
                                        fullWidth
                                        variant="contained"
                                        color="primary"
                                    >
                                        ENVIAR CÓDIGO
                                    </Button>
                                </Grid>
                                <Grid item xs={12} sm={4} />
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    );
}
