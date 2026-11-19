import React, { useEffect, useState } from 'react';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { toast } from 'react-toastify';
import { Controller, useForm } from 'react-hook-form';
import moment from 'moment';


import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import { Reportes } from '../../../models/reportes-data.model';
import CHLoadingProcess from '../../../components/ch-loading-process/CHLoadingProcess';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHDatePickerV3 from '../../../components/ch-datepicker/CHDatepickerV3';
import { DatosSimulacionParaEstudioForm, DatosSimulacionParaEstudioModel } from '../../../models/credit-create-simulation-data.model';
import ApiReportes from '../../../services/api-reportes.service';
import ApiCredit from '../../../services/api-credit.service';



const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        button: {
            width: '100%',
            marginLeft: '.5rem',
            marginTop: '1.5rem',
        },
    }),
);
const GenerarExtracto = () => {
    const apiCredit = ApiCredit();
    const apiReportes = ApiReportes();

    const formulario = useForm({
        mode: 'onChange',
        defaultValues: {
            nombreReporte: '',
            identificacion: '',
            fechaReportes: moment(new Date())
                .subtract(1, 'months')
                .format('YYYY-MM'),
            fecha: new Date(),

        },
    });

    const [listaReportes, setListaReportes] = useState<Reportes[]>([]);

    const [isLoadingFile, setIsLoadingFile] = useState(false);

    const [conIdentificacion, setConIdentificacion] = useState(false);
    const [conFechaMesAnio, setConFechaMesAnio] = useState(false);
    const [datosInformacionSimulacion, setDatosInformacionSimulacion] =
        useState<DatosSimulacionParaEstudioModel>(
            DatosSimulacionParaEstudioForm,
        );


    useEffect(() => {
 
        listarRepostes();
    }, []);



    const listarRepostes = async () => {
        await apiReportes
            .generarListaReportes()
            .then(async (response: any) => {
                if (response) {
                    
                    if (response.isSuccess) {
                        debugger
                        setListaReportes(response.data.filter((option: any, index: number) => (
                            index === 4 || index ===5
                        )));
                     //   setListaReportes(response.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };

    const obtenerDatosSimulacionParaEstudio = async (request: any) => {
        await apiCredit
            .getObtenerDatosSimulacionParaEstudio(request)
            .then(async (response: any) => {
                if (response) {
                    if (response.isSuccess && response?.data !== null) {
                        debugger
                        setDatosInformacionSimulacion(response?.data);
                    }
                }
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };
    

    const onSubmit = async (item: any) => {
        debugger
        obtenerDatosSimulacionParaEstudio(String(item.identificacion))

         if (item.nombreReporte === 'Extracto (anual)') {
            const nuevaFecha = moment(item.fechaReportes).year();
            const inputFecha = moment(item.fechaReportes);
            if (nuevaFecha <= 1910) {
                return;
            }

            if (!inputFecha.isValid()) {
                return;
            }

            setIsLoadingFile(true);

            const request = {
                identificacion: item.identificacion,
                periodo: `${moment(item.fechaReportes).year()}-${
                    moment(item.fechaReportes).month() + 1
                }`,
                // mesCierre: moment(item.fechaReportes).month() + 1,
            };

            await apiReportes
                .generarExtratoAnual(request)
                .then(async response => {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                });
        } else if (item.nombreReporte === 'Extracto (mensual)') {
            setIsLoadingFile(true);
            const request = {
                identificacion: item.identificacion,
                periodo: `${moment(item.fechaReportes).year()}-${
                    moment(item.fechaReportes).month() + 1
                } `,
                // mesCierre: moment(item.fechaReportes).month() + 1,
            };
            await apiReportes
                .generarExtratoMensual(request)
                .then(async response => {
                    setIsLoadingFile(false);
                    const mimeType = response.type;
                    if (mimeType) {
                        const file = new Blob([response], { type: mimeType });
                        const fileURL = URL.createObjectURL(file);
                        window.open(fileURL, '_blank');
                    }
                })
                .catch(() => {
                    toast('Error del sistema', { type: toast.TYPE.ERROR });
                    setIsLoadingFile(false);
                });
        } 
    }
    const classes = useStyles();

    if (isLoadingFile) {
        return <CHLoadingProcess />;
    }

    return (
        <CHPrivateContainer>
            <CHPaper title="Generación de Extractos">
                <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="nombreReporte"
                            control={formulario.control}
                            rules={{
                                required: 'Este campo es requerido.',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    label="Documentos"
                                    value={value}
                                    helperText={error ? error.message : null}
                                    onChange={value => {
                                                setConIdentificacion(true);
                                                setConFechaMesAnio(true);
                                                 onChange(value);

                                    }
                                }
                                >
                                    <MenuItem key={0} value={0}>
                                        -- Seleccionar --
                                    </MenuItem>
                                    {listaReportes?.map((option: any) => (
                                        <MenuItem
                                            key={option.idReporte}
                                            value={option.nombreReporte}
                                        >
                                            {option.nombreReporte}
                                        </MenuItem>
                                    ))}
                                </CHSelect>
                            )}
                        />
                    </Grid>
                    {conIdentificacion && (
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="identificacion"
                                control={formulario.control}
                                rules={{
                                    required: 'Este campo es requerido.',
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHTextField
                                        type="number"
                                        error={!!error}
                                        label="Número Identificación *"
                                        value={value}
                                        InputLabelProps={{ shrink: true }} // sirve para que no se monte el label
                                        helperText={
                                            error ? error.message : null
                                        }
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}{' '}

                    {conFechaMesAnio && (
                        <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                name="fechaReportes"
                                control={formulario.control}
                                rules={{
                                    required: 'Es requirido',
                                    pattern: {
                                        value: /^\d{4}([/.]|[-.])(0?[1-9]|1[1-2])$/,
                                        message: 'Fecha no válida',
                                    },
                                }}
                                render={({
                                    field: { onChange, value },
                                    fieldState: { error },
                                }) => (
                                    <CHDatePickerV3
                                        views={['year', 'month']}
                                        error={!!error}
                                        label="Fecha *"
                                        invalidDateMessage="Fecha no válida"
                                        minDateMessage="La fecha no debe ser anterior a la fecha mínima"
                                        maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                                        minDate={moment('1910-12').format(
                                            'YYYY-MM',
                                        )}
                                        disableFuture
                                        maxDate={new Date()}
                                        value={value}
                                        onChange={onChange}
                                    />
                                )}
                            />
                        </Grid>
                    )}

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
            </CHPaper>
        </CHPrivateContainer>
    );
};

export default GenerarExtracto;
