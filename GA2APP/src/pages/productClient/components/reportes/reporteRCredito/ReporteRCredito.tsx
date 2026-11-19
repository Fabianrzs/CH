import React, { useState } from 'react';
import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import { useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import moment from 'moment';
import ControllerPeriodo from '../controllers/ControllerPeriodo';
import ApiReportes from '../../../../../services/api-reportes.service';
import CHPageTitle from '../../../../../components/ch-page-title/CHPageTitle';
import { LoadingV2Component } from '../../../../../components/loading/loadingV2.component';
import DownloadBlob from '../../../../../utils/blob';


type FormData = {
    periodoReporte?: Date | null,
}

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '0',
            marginTop: '0',
        }
    }),
);


const ReporteRCredito = () => {
    const apiReportes = ApiReportes();
    const classes = useStyles();

    const [loading, setLoading] = useState(false);

    const formulario = useForm<FormData>({
        mode: 'onChange',
        defaultValues: {
            periodoReporte: new Date(),
        },
    });


    const onSubmit = async (data: any) => {

        setLoading(true);

        const request = {
            anio: data.periodoReporte?.getFullYear() ?? null,
            mes: data.periodoReporte ? data.periodoReporte.getMonth() + 1 : null,
        };

        apiReportes
            .reporteRCredito(request)
            .then(async response => {
                if (response?.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    const blob = new Blob([response], { type: response.type });
                    const periodoFormat = moment(data.periodoReporte).format("YYYY-MM");
                    const filename = `Reporte RCrédito ${periodoFormat}`;
                    DownloadBlob(blob, filename);
                }
            })
            .catch(async (e) => {
                let message;

                if(e.response) {
                    const result = JSON.parse(await e.response.data.text());
                    message = result.returnMessage;
                } else {
                    message = 'No se pudo generar el reporte';
                }
                toast(message, { type: toast.TYPE.ERROR });

            }).finally(() => setLoading(false));
       
    };

    return (
        <>
           
            <Card>
                <CHPageTitle
                    topText="Consulta Reportes"
                    subText=""
                    title1="Reporte"
                    title2="RCrédito"
                />
                
                <CardContent>
                    <Grid container justifyContent='center'>
                        <Grid item xs={12} md={3} className={classes.column}>
                            <ControllerPeriodo 
                                form={formulario} type="month" required
                            />
                        </Grid>
                    </Grid>
                    <Grid container justifyContent='center'>
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Button
                                className={classes.button}
                                variant="contained"
                                color="primary"
                                onClick={formulario.handleSubmit(onSubmit)}
                            >
                                GENERAR
                            </Button>
                        </Grid>
                    </Grid>
                </CardContent>
            </Card>
            
            <br />

            <LoadingV2Component key="loader" loading={loading} tittle='Generando reporte...' />
        </>
    );
};

export default ReporteRCredito;