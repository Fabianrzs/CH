import React, { useEffect, useState } from 'react';
import { Button, createStyles, Grid, makeStyles } from '@material-ui/core';
import moment from 'moment';
import { toast } from 'react-toastify';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../components/ch-text-field/CHTextField';
import { PreaprobadoDto } from '../../types/preaprobados.model.type';
import ApiCredit from '../../services/api-credit.service';
import UtilService from '../../utils/util.service';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '20px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
            minWidth: '110px',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
    }),
);

const InformacionPreaprobado = (props: propsType) => {
    const classes = useStyles();
    const apiCredit = ApiCredit();

    const { preapropado, onItemGoBack } = props;
    const [simulacionPersonales, setSimulacionPersonales] = useState<any>(null);
    const [simulacionCliente, setSimulacionCliente] = useState<any>(null);

    useEffect(() => {
        consultarSimulacionPersonales(preapropado.numeroDcocumento);
        consultarSimulacionCliente(preapropado.numeroDcocumento);
    }, [preapropado]);

    const consultarSimulacionPersonales = async (request: string) => {
        await apiCredit
            .getObtenerSimulacionPersonales(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response?.data !== null) {
                        setSimulacionPersonales(response?.data);
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const consultarSimulacionCliente = async (request: string) => {
        await apiCredit
            .getConsultarSimulacionCliente(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response?.data !== null) {
                        setSimulacionCliente(response?.data);
                    }
                }
            })
            .catch(e => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    return (
        <>
            {simulacionPersonales && simulacionCliente && (
                <CHPrivateContainer>
                    <CHPageTitle
                        topText=""
                        title1="Información"
                        title2="preaprobado"
                        subText=""
                    />
                    <CHPaper title="Información del solicitante">
                        <Grid container item xs={12}>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Fecha Solicitud"
                                    value={UtilService.getDate(
                                        simulacionCliente?.fechaSimulacion,
                                    )}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Fuerza"
                                    value={simulacionPersonales?.fuerza}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Categoría"
                                    value={simulacionPersonales?.categoria}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Grado"
                                    value={simulacionPersonales?.grado}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container item xs={12}>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Nombre y Apellido"
                                    value={
                                        simulacionPersonales?.nombresApellidos
                                    }
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Departamento"
                                    value={
                                        simulacionPersonales?.departamentoResidencia
                                    }
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Ciudad"
                                    value={
                                        simulacionPersonales?.ciudadResidencia
                                    }
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Dirección Residencia"
                                    value={simulacionPersonales?.direccion}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container item xs={12}>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Teléfono Fijo"
                                    value={simulacionPersonales?.telefonoFijo}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Celular"
                                    value={
                                        simulacionPersonales?.telefonoCelular
                                    }
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>

                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Correo Electrónico"
                                    value={simulacionPersonales?.eMail}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                        </Grid>
                    </CHPaper>
                    <CHPaper title="Información financiera">
                        <Grid container item xs={12}>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Valor Vivienda"
                                    value={UtilService.formatCurrency(
                                        simulacionCliente?.valorVivienda,
                                    )}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Tipo Vivienda"
                                    value={
                                        simulacionCliente?.viviendaVis === 'No'
                                            ? 'NO VIS'
                                            : 'VIS'
                                    }
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Vivienda"
                                    value={simulacionPersonales?.tipoVivienda}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Valor Préstamo"
                                    value={UtilService.formatCurrency(
                                        simulacionCliente?.valorPrestamo,
                                    )}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container item xs={12}>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Valor Cuota con Seguros"
                                    value={UtilService.formatCurrency(
                                        simulacionCliente?.valorCuota,
                                    )}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Valor Cuota sin Seguros"
                                    value={UtilService.formatCurrency(
                                        simulacionCliente?.valorCuotaSinSeg,
                                    )}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Plazo (Meses)"
                                    value={simulacionCliente?.plazo}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Años"
                                    value={
                                        (simulacionCliente?.plazo / 12) % 1 ===
                                        0
                                            ? simulacionCliente?.plazo / 12
                                            : (
                                                  simulacionCliente?.plazo / 12
                                              ).toFixed(2)
                                    }
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container item xs={12}>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    percentMask
                                    id="outlined-basic"
                                    label="Tasa E.A."
                                    isReadOnly
                                    value={simulacionCliente?.valorTasaEA * 100}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    percentMask
                                    id="outlined-basic"
                                    label="Tasa Nominal MV"
                                    value={simulacionCliente?.valorTasaMV}
                                    isReadOnly
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                            <Grid item md={3} className={classes.column}>
                                <CHTextField
                                    id="outlined-basic"
                                    label="Cuota Mod. Tradicional"
                                    value={UtilService.formatCurrency(
                                        simulacionCliente?.valorCuota,
                                    )}
                                    inputProps={{
                                        style: {
                                            textAlign: 'right',
                                        },
                                    }}
                                    InputLabelProps={{ shrink: true }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={9}
                            />
                            <Grid
                                className={classes.column}
                                item
                                xs={12}
                                md={3}
                            >
                                <Button
                                    className={classes.button}
                                    variant="contained"
                                    color="primary"
                                    onClick={onItemGoBack}
                                >
                                    REGRESAR
                                </Button>
                            </Grid>
                        </Grid>
                    </CHPaper>
                </CHPrivateContainer>
            )}
        </>
    );
};

type propsType = {
    preapropado: PreaprobadoDto;
    onItemGoBack: () => void;
};

export default InformacionPreaprobado;
