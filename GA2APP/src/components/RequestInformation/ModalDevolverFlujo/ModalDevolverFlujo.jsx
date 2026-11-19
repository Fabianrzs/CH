import { Button, Card, CardContent, Container, createStyles, Dialog, Grid, makeStyles, MenuItem, Typography } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import moment from 'moment';
import { toast } from "react-toastify";
import { connect } from 'react-redux';
import CreditApi from "../../../services/api-credit.service";
import {
    resetCreditState,
    startPostCreditBasicData,
    startPostCreditObservationsData,
    startPostCreditProductData,
    startRequestActivityProcess,
} from '../../../actions/credit.actions';
import {
    REQUEST_ACTIVITY_PROCESS,
    REQUEST_ACTIVITY_TABS,
    REQUEST_TYPE_REQUEST,
} from '../../../constants/constants';
import CHSelect from "../../ch-select/CHSelect";
import { AppState } from '../../../store/rootReducer';
import { RequestActivityRequestModel } from "../../../models/requests/request-activity-request.model";
import NotificationService from "../../../services/notification.service";
import HelperService from "../../../utils/helper.service";


const useStyles = makeStyles(() =>

    createStyles({
        titleform: {
            width: '100%',
            textAlign: 'center',
            color: '#1C3F6B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
    }),
);

function ModalDevolverFlujo(props) {
    const {
        open,
        handleClose,
        obterFlujoModalHijo,
        procedureRequest,
        dataType,
        startRequestActivityProcess,
        observacion,
    } = props
    const apiCredito = CreditApi();

    const { control, setValue, getValues, handleSubmit } = useForm({
        defaultValues: {
            obervation: '',
            flujo: ''
        }
    })


    const [tiposActividad, setTiposActividad] = useState([])
    const [estadoActividad, setEstadoActividad] = useState(
        procedureRequest.stateActivity,
    );



    useEffect(() => {
        obtenerTiposDeactividad();
    }, [])

   
    const obtenerTiposDeactividad = () => {
        apiCredito
            .getObtenerTipoActividad()
            .then(async (response) => {
                if (response) {
                    if (response.isSuccess) {
                        
                        if (procedureRequest.flowActivityID === 6 || procedureRequest.flowActivityID === 3) {
                            const actividadPorFlujo = response.data.filter(x => x.id === 
                                5)
                            setTiposActividad(actividadPorFlujo)
                       
                            handleClose(false)
                        } else if(procedureRequest.flowActivityID === 6 ) {
                            
                            const actividadPorFlujo = response.data.filter(x => x.id === 
                                3)
                            setTiposActividad(actividadPorFlujo)
             
                        }else if(procedureRequest.flowActivityID === 8 ) {
                            
                            const actividadPorFlujo = response.data.filter(x => x.id === 
                                5)
                            setTiposActividad(actividadPorFlujo)
                        }else if(procedureRequest.flowActivityID === 18 ) {
                            
                            const actividadPorFlujo = response.data.filter(x => x.id === 
                                10)
                            setTiposActividad(actividadPorFlujo)
                            // setTiposActividad(response.data)
                        }
                        else {
                            setTiposActividad(response.data)
                        }
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    }

    const devolver = async (item) => {

        handleClose(false)

        const confirmAlert = await NotificationService.confirmAlert(
            '¿Está seguro?',
        );
        if (confirmAlert) {

            obterFlujoModalHijo(true)

            const data = {
                trS_ID: procedureRequest.trsID,
                ACT_ID: procedureRequest.flowActivityID,
                ACT_ID_RETORNO: getValues('flujo'),
                acT_OBSERVACION: observacion,
            };

            startRequestActivityProcess(data, item[0]);
            if (item[0] === REQUEST_ACTIVITY_PROCESS.BACK) {
                setEstadoActividad('VOLVER');
            } else {
                setEstadoActividad('');
            }
            setValue('observation', '');
        }
    }
    const classes = useStyles();
    return (
        <>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="form-dialog-title"
            >
                <Container maxWidth="lg">
                    <Card style={{ width: '500px' }}>
                        <CardContent>
                            <Grid item>
                                <Typography
                                    className={classes.titleform}
                                    variant="h5"
                                    component="h6"
                                >
                                    DEVOLVER ACTIVIDAD
                                </Typography>
                            </Grid>
                            <Grid container spacing={3}>

                                <Grid item md={12} >
                                    <Controller
                                        name="flujo"
                                        control={
                                            control
                                        }
                                        rules={{
                                            required: 'Este campo es requerido.',
                                        }}
                                        render={({
                                            field: { onChange, value },
                                            fieldState: { error },
                                        }) => (
                                            <CHSelect
                                                error={!!error}
                                                label="Flujo *"
                                                value={value}
                                                helperText={
                                                    error ? error.message : null
                                                }
                                                onChange={onChange}
                                            >
                                                {tiposActividad?.map(
                                                    (option) => (
                                                        <MenuItem
                                                            key={option.id}
                                                            value={option.id}
                                                        >
                                                            {option.descripcion}
                                                        </MenuItem>
                                                    ),
                                                )}
                                            </CHSelect>
                                        )}
                                    />
                                </Grid>

                            </Grid>

                            <Grid container spacing={3}>
                                <Grid item xs={12}>

                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="primary"
                                        onClick={handleSubmit(HelperService.partial(devolver,REQUEST_ACTIVITY_PROCESS.BACK,))}
                                    >
                                        Devolver
                                    </Button>

                                    <Button
                                        size="small"
                                        variant="contained"
                                        color="default"
                                        onClick={handleClose}
                                        style={{ marginLeft: "10px" }}
                                    >
                                        Cancelar
                                    </Button>
                                </Grid>
                            </Grid>
                        </CardContent>
                    </Card>
                </Container>
            </Dialog>
        </>
    )
}

const mapStateToProps = (state: AppState) => ({
    isLoadingCredit: state.credit.loading,
    creditBasicData: state.credit.creditBasicData,
    creditProductData: state.credit.creditProductData,
    creditObservationsData: state.credit.creditObservationsData,
    creditFilesData: state.credit.creditFilesData,
    error: state.credit.error,
    responseRequestActivity: state.credit.responseRequestActivity,
    informationBasicSeverance: state.severance.informationBasicSeverance,
});

const mapDispatchToProps = {
    startRequestActivityProcess,
    startPostCreditBasicData,
    startPostCreditProductData,
    startPostCreditObservationsData,
    resetCreditState,

};

export default connect(mapStateToProps, mapDispatchToProps)(ModalDevolverFlujo);