import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { Button, Card, CardContent, createStyles, Grid, makeStyles } from '@material-ui/core';
import moment from 'moment';
import { Controller, useForm } from 'react-hook-form';
import MUIDataTable from 'mui-datatables';
import { toast } from 'react-toastify';
import CHContainerSubtitle from '../../../../components/container-subtittle/container-subtittle';
import PageIndicators from '../../../../components/page-navigate-indicator/page-navigate-indicator';
import IconTitle from '../../../../components/page-title-icon/page-title-icon';
import CHDatePickerV3 from '../../../../components/ch-datepicker/CHDatepickerV3';
import ApiVerificacionDerecho from '../../../../services/api-verificacion.service';
import CHLoadingProcess from '../../../../components/ch-loading-process/CHLoadingProcess';
import UtilService from '../../../../utils/util.service';
import AuthService from '../../../../services/auth.service';

const useStyles = makeStyles(() =>
    createStyles({
        buttonprincipal: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#FEC917',
        },
        buttonsecundario: {
            width: '250px',
            height: '50px',
            marginLeft: '1rem',
            background: '#43b5f1',
        },

        tablaCierre: {
            overflow: 'auto !important'

        },
    }),
);

function CierreTable({
    data, mostrarTabla, infoUsuario
}:
    {
        data: any[],
        mostrarTabla: any,
        infoUsuario: any

    }) {

    const columns = [

        {
            name: "Procesar",
            options: {
                customBodyRender: (value: any, tableMeta: any, rowData: any) => {
                    const aprobado = data[tableMeta.rowIndex].estado;
                    let buttonText = "DETALLES";

                    if(infoUsuario === "60" && aprobado === 2){
                        buttonText = "DETALLES";
                    }else if(aprobado === null || aprobado === 1 || aprobado === 2){
                        buttonText = "CONTINUAR";
                    }

                    return (
                        <Link to={{ pathname: './verificacionDerechosDetalles', state: { rowData: data[tableMeta.rowIndex] } }}>
                            <button disabled={infoUsuario === "88" && aprobado === 1}>
                                {buttonText}
                            </button>
                        </Link>
                    );
                },
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'numeroSolicitud',
            label: '# Solicitud',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
                customBodyRender: (value: any) => (value !== null ? value : 0)
            },

        },
        {
            name: 'anio',
            label: 'Año',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'mes',
            label: 'Mes',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'totalCuentasVerificadas',
            label: 'Cuentas Verificadas',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },

        },
        {
            name: 'usuarioNombre',
            label: 'UsuarioNombre',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
        {
            name: 'fechaAprobacion',
            label: 'Fecha de Aprobación',
            options: {
                customBodyRender: (value: Date) => UtilService.getFullDate(value),
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),

            },

        },
        {
            name: 'nombreEstado',
            label: 'ESTADO',
            options: {
                setCellHeaderProps: () => ({
                    className: 'filas'
                }),
            },
        },
    ];

    return (

        mostrarTabla && (
            <MUIDataTable
                title="Solicitudes Verificación de Derechos"
                data={data}
                columns={columns}
                options={{
                    selectableRows: 'none',
                    responsive: 'vertical',
                    filter: false,
                    print: false,
                    elevation: 0,
                    textLabels: {
                        body: {
                            noMatch: 'No se encontraron resultados',
                        },
                        pagination: {
                            displayRows: 'de',
                            next: 'Siguiente',
                            previous: 'Anterior',
                            rowsPerPage: 'Filas por página',
                            jumpToPage: '',
                        },
                        toolbar: {
                            downloadCsv: 'Descargar CSV',
                            search: 'Buscar',
                            viewColumns: 'Mostrar Columnas',
                        },
                        viewColumns: {
                            title: 'Mostrar Columnas',
                        },
                    },
                }}
            />
        )

    );

}

const VerificacionDerechos = () => {

    const classes = useStyles();
    const Api = ApiVerificacionDerecho();
    const user = AuthService.getUserInfo();
    const [isLocalLoading, setIsLocalLoading] = useState(false);
    const [dataCierreMes, setDataCierreMes] = useState<any[]>([]);
    const [mostrarTabla, setMostrarTabla] = useState(false);

    useEffect(() => {
        showData();
    }, [])

    const onSubmit = async (data: any) => {

        if (!data.fechaInicial) {
            data.fechaInicial = moment('1910-12').format('YYYY-MM');
        }

        if (!data.fechaFinal) {
            data.fechaFinal = moment(new Date()).format('YYYY-MM');
        }

        const request = {
            DesdeAnio: moment(data.fechaInicial).year(),
            DesdeMes: moment(data.fechaInicial).month() + 1,
            HastaAnio: moment(data.fechaFinal).year(),
            HastaMes: moment(data.fechaFinal).month() + 1,
        }

        await Api
            .ObtenerTodoVerificacionDerechoFiltroRequest(request)
            .then(async (response: any) => {

                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {
                            setDataCierreMes(response.data)
                            setMostrarTabla(true)
                            setIsLocalLoading(false);
                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTabla(false)
                        setIsLocalLoading(false);
                    }
                }
            }).catch((error: any) => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });

    };

    const showData = async () => {
        await Api
            .ObtenerTodoVerificacionDerechoRequest()
            .then(async (response: any) => {
                if (response) {
                    if (response.data !== null) {
                        if (response.isSuccess) {
                            setDataCierreMes(response.data);
                            setMostrarTabla(true)
                            setIsLocalLoading(false);
                        }
                    } else {
                        toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
                        setMostrarTabla(false)
                        setIsLocalLoading(false);
                    }

                }
            }).catch(() => {
                toast("Error del sistema", { type: toast.TYPE.ERROR });
                setIsLocalLoading(false);
            });

    }

    const formularioIPC = useForm({
        'mode': 'onChange',
        defaultValues: {
            fechaInicial: null,
            fechaFinal: moment(new Date()).add(1, 'months').format('YYYY-MM'),
        },

    });

    if (isLocalLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <div
            style={{
                display: 'table',
                tableLayout: 'fixed',
                width: '100%',

            }}>

            <PageIndicators
                topTextNivel1="Administración Cuentas"
                topTextNivel2="Verificación de Derechos"
            />
            <IconTitle
                title="Verificación"
                subTitle="de Derechos"
                icon="fch-Verifique-Docuementos-1-1"
            />
            <Grid>
                <Grid item xs={12}>
                    <Card>
                        <CardContent>
                            <Grid container justifyContent="space-around">
                                <Grid item xs={3}>
                                    <CHContainerSubtitle
                                        subTitle="Verificación de Derechos"
                                        title="Consulta"
                                        icon="fch-Procedimientos-solicitudes-0-1"
                                    />
                                </Grid>
                                <Grid item xs={3}>
                                    <br />
                                    <Controller
                                        name="fechaInicial"
                                        control={formularioIPC.control}
                                        rules={{
                                            validate: (value) => {

                                                if (!value) {
                                                    return true;
                                                }
                                                const startDateValue = moment(formularioIPC.getValues('fechaInicial'), 'YYYY/MM');
                                                const endDateValue = moment(formularioIPC.getValues('fechaFinal'), 'YYYY/MM');

                                                const isStartDateValid = startDateValue.isValid();
                                                const isEndDateValid = endDateValue.isValid();

                                                if (!isStartDateValid) {
                                                    return 'Fecha inicial no válida';
                                                }

                                                if (isEndDateValid) {
                                                    const isStartDateBeforeEndDate = startDateValue.isBefore(endDateValue);
                                                    if (!isStartDateBeforeEndDate) {
                                                        return 'Periodo inicial debe ser menor al periodo final';
                                                    }
                                                }

                                                const isAfterOrEqualMinDate = startDateValue.isSameOrAfter(moment('1910-12'), 'month');

                                                if (!isAfterOrEqualMinDate) {
                                                    return 'Fecha inicial no válida';
                                                }

                                                return true;
                                            },
                                        }}
                                        render={({ field, fieldState }) => (
                                            <CHDatePickerV3
                                                views={['year', 'month']}
                                                invalidDateMessage=""
                                                maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                                                minDateMessage='La fecha no debe ser anterior a la fecha mínima'
                                                minDate={moment('1910-12').format('YYYY-MM')}
                                                label="Periodo Inicial *"
                                                value={field.value}
                                                onChange={(date) => field.onChange(date)}
                                                onBlur={field.onBlur}
                                                error={!!fieldState.error}
                                                helperText={fieldState.error ? fieldState.error.message : null}
                                            />
                                        )}
                                    />
                                    <br />
                                    <Controller
                                        name="fechaFinal"
                                        control={formularioIPC.control}
                                        rules={{
                                            validate: (value) => {

                                                if (!value) {
                                                    return true;
                                                }


                                                const isAfterOrEqualMinDate = moment(value, 'YYYY/MM').isSameOrAfter(moment('1910-12'), 'month');

                                                if (!isAfterOrEqualMinDate) {
                                                    return 'Fecha no válida';
                                                }

                                                return true;
                                            },
                                        }}
                                        render={({ field, fieldState }) => (
                                            <CHDatePickerV3
                                                views={['year', 'month']}
                                                invalidDateMessage=""
                                                maxDateMessage="La fecha no debe ser posterior a la fecha máxima"
                                                minDateMessage='La fecha no debe ser anterior a la fecha mínima'
                                                minDate={moment('1910-12').format('YYYY-MM')}
                                                label="Periodo Final *"
                                                value={field.value}
                                                onChange={(date) => field.onChange(date)}
                                                onBlur={field.onBlur}
                                                error={!!fieldState.error}
                                                helperText={fieldState.error ? fieldState.error.message : null}

                                            />
                                        )}

                                    />
                                </Grid>
                            </Grid>
                            <br />
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <Grid className="buttonssolicitud">
                <Button
                    className={classes.buttonprincipal}
                    variant="outlined"
                    onClick={formularioIPC.handleSubmit(onSubmit)}
                >
                    Consultar
                </Button>
            </Grid>
            <br />
            <div className='MuiTableCell-stickyHeader; jss86'>
                <CierreTable
                    data={dataCierreMes}
                    mostrarTabla={mostrarTabla}
                    infoUsuario={user.GRP_ID}
                />

            </div>

        </div>

    );
};

export default VerificacionDerechos;