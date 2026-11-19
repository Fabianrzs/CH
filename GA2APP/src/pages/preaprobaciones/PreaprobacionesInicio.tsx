import React, { useState } from 'react';
import {
    Button,
    createStyles,
    Grid,
    makeStyles,
    MenuItem,
} from '@material-ui/core';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';

import CHDatePicker from '../../components/ch-datepicker/CHDatepicker';
import CHPageTitle from '../../components/ch-page-title/CHPageTitle';
import CHPaper from '../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../components/ch-private-container/CHPrivateContainer';
import CHTextField from '../../components/ch-text-field/CHTextField';
import CHSelect from '../../components/ch-select/CHSelect';
import { LoadingComponent } from '../../components/loading/loading.component';

import ApiCredit from '../../services/api-credit.service';
import NotificationService from '../../services/notification.service';

import {
    ConsultaReporteTransUnions,
    ReporteTransUnionDto,
} from '../../types/reporteTransUnion.model.type';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
        },

        buttonsContainer: {
            display: 'flex',
            justifyContent: 'flex-start',
            gap: '10px',
            marginTop: '20px',
            marginBottom: '20px',
            paddingLeft: '10px',
        },

        searchButton: {
            minWidth: '130px',
            backgroundColor: '#1C3F8B',
            color: '#FFFFFF',
            fontWeight: 600,

            '&:hover': {
                backgroundColor: '#163472',
            },
        },

        clearButton: {
            minWidth: '110px',
        },

        tableContainer: {
            width: '100%',
            overflowX: 'auto',
            marginTop: '20px',
        },

        table: {
            width: '100%',
            borderCollapse: 'collapse',
            minWidth: '1100px',
            fontSize: '12px',
        },

        /*
         * COLOR DEL ENCABEZADO
         * Igual al ejemplo solicitado.
         */
        header: {
            backgroundColor: '#FEC917',
        },

        headerCell: {
            backgroundColor: '#FEC917',
            border: '1px solid #FFFFFF',
            padding: '9px 8px',
            textAlign: 'left',
            fontWeight: 400,
            color: '#4F4F4F',
            whiteSpace: 'nowrap',
            verticalAlign: 'middle',
        },

        /*
         * FILAS DE LA TABLA
         */
        row: {
            backgroundColor: '#D0D0D0',

            '&:nth-of-type(even)': {
                backgroundColor: '#E7E7E7',
            },

            '&:hover': {
                backgroundColor: '#D9D9D9',
            },
        },

        cell: {
            border: '1px solid #FFFFFF',
            padding: '8px',
            textAlign: 'center',
            color: '#555555',
            fontWeight: 400,
            verticalAlign: 'middle',
        },

        /*
         * Algunas columnas de texto largo se ven mejor
         * alineadas a la izquierda, como en el ejemplo.
         */
        textCell: {
            border: '1px solid #FFFFFF',
            padding: '8px',
            textAlign: 'left',
            color: '#555555',
            fontWeight: 400,
            verticalAlign: 'middle',
        },

        noResults: {
            textAlign: 'center',
            padding: '25px',
            fontSize: '15px',
            color: '#555555',
        },

        total: {
            marginTop: '10px',
            fontWeight: 600,
            color: '#555555',
            fontSize: '12px',
        },
    }),
);

interface FormValues {
    numeroIdentificacion: string;
    fechaDesde: Date | null;
    fechaHasta: Date | null;
    estado: string | number;
}

const defaultValues: FormValues = {
    numeroIdentificacion: '',
    fechaDesde: null,
    fechaHasta: null,
    estado: '',
};

const ConsultaReporteTransUnion = () => {
    const classes = useStyles();
    const apiCredit = ApiCredit();

    const [registros, setRegistros] = useState<ReporteTransUnionDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [isDataLoaded, setIsDataLoaded] = useState(false);

    const formularioConsulta = useForm<FormValues>({
        defaultValues,
    });

    const obtenerEstado = (estado: number) => {
        switch (Number(estado)) {
            case 1:
                return 'VIABLE';

            case 2:
                return 'PENDIENTE';

            case 3:
                return 'RECHAZO';

            default:
                return '';
        }
    };

    const formatearFechaApi = (
        fecha: Date | string | null,
        esFechaHasta: boolean,
    ) => {
        if (!fecha) {
            return null;
        }

        const date = new Date(fecha);

        if (Number.isNaN(date.getTime())) {
            return null;
        }

        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');

        if (esFechaHasta) {
            return `${year}-${month}-${day}T23:59:59`;
        }

        return `${year}-${month}-${day}T00:00:00`;
    };

    const formatearFechaTabla = (fecha: string) => {
        if (!fecha) {
            return '';
        }

        const date = new Date(fecha);

        if (Number.isNaN(date.getTime())) {
            return fecha;
        }

        return date.toLocaleDateString('es-CO', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
        });
    };

    const formatearMoneda = (valor: number) => {
        if (valor === null || valor === undefined) {
            return '$ 0';
        }

        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
            maximumFractionDigits: 0,
        }).format(valor);
    };

    const handleSearchButton = async (data: FormValues) => {
        if (
            data.fechaDesde !== null &&
            data.fechaHasta !== null &&
            new Date(data.fechaDesde) > new Date(data.fechaHasta)
        ) {
            await NotificationService.genericAlert(
                'Warning',
                'La fecha inicial no puede ser mayor a la fecha final',
                'warning',
            );

            return;
        }

        const request: ConsultaReporteTransUnions = {
            numeroIdentificacion:
                data.numeroIdentificacion &&
                data.numeroIdentificacion.trim() !== ''
                    ? data.numeroIdentificacion.trim()
                    : null,

            fechaDesde: formatearFechaApi(
                data.fechaDesde,
                false,
            ),

            fechaHasta: formatearFechaApi(
                data.fechaHasta,
                true,
            ),

            estado:
                data.estado === '' ||
                data.estado === null ||
                data.estado === undefined
                    ? null
                    : Number(data.estado),
        };

        try {
            setLoading(true);
            setIsDataLoaded(false);

            const response = await apiCredit.postObtenerReporteTransUnion(
                request,
            );

            if (
                response &&
                response.isSuccess &&
                Array.isArray(response.data)
            ) {
                setRegistros(response.data);
                setIsDataLoaded(true);

                return;
            }

            setRegistros([]);
            setIsDataLoaded(true);

            await NotificationService.genericAlert(
                'Información',
                response?.returnMessage ||
                    'No se encontraron registros.',
                'info',
            );
        } catch (error) {
            console.error(
                'Error consultando reporte TransUnion:',
                error,
            );

            setRegistros([]);
            setIsDataLoaded(true);

            toast('Error al consultar el reporte TransUnion', {
                type: toast.TYPE.ERROR,
            });
        } finally {
            setLoading(false);
        }
    };

    const handleLimpiar = () => {
        formularioConsulta.reset(defaultValues);
        setRegistros([]);
        setIsDataLoaded(false);
    };

    return (
        <CHPrivateContainer>
            <CHPageTitle
                topText="Gestión Preaprobados"
                subText=""
                title1="Consulta"
                title2="Preaprobados"
            />

            <CHPaper title="Información consulta">
                <Grid container spacing={2}>
                    {/* NÚMERO IDENTIFICACIÓN */}
                    <Grid
                        className={classes.column}
                        item
                        xs={12}
                        md={3}
                    >
                        <Controller
                            name="numeroIdentificacion"
                            control={formularioConsulta.control}
                            rules={{
                                pattern: {
                                    value: /^[0-9]*$/,
                                    message: 'Solo se permiten números',
                                },
                            }}
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                                fieldState: {
                                    error,
                                },
                            }) => (
                                <CHTextField
                                    error={!!error}
                                    helperText={
                                        error
                                            ? error.message
                                            : null
                                    }
                                    isReadOnly={false}
                                    InputLabelProps={{
                                        shrink: true,
                                    }}
                                    label="Número de Identificación"
                                    value={value}
                                    onChange={e => {
                                        const nuevoValor =
                                            e.target.value.replace(
                                                /\D/g,
                                                '',
                                            );

                                        if (nuevoValor.length <= 20) {
                                            onChange(nuevoValor);
                                        }
                                    }}
                                />
                            )}
                        />
                    </Grid>

                    {/* FECHA DESDE */}
                    <Grid
                        className={classes.column}
                        item
                        xs={12}
                        md={3}
                    >
                        <Controller
                            name="fechaDesde"
                            control={formularioConsulta.control}
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Desde"
                                    onChange={onChange}
                                    value={value}
                                    maxDate={new Date()}
                                    maxDateMessage="La fecha desde no debe ser mayor a la fecha actual"
                                    invalidDateMessage="Fecha inválida"
                                />
                            )}
                        />
                    </Grid>

                    {/* FECHA HASTA */}
                    <Grid
                        className={classes.column}
                        item
                        xs={12}
                        md={3}
                    >
                        <Controller
                            name="fechaHasta"
                            control={formularioConsulta.control}
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                            }) => (
                                <CHDatePicker
                                    label="Fecha Hasta"
                                    onChange={onChange}
                                    value={value}
                                    maxDate={new Date()}
                                    maxDateMessage="La fecha hasta no debe ser mayor a la fecha actual"
                                    invalidDateMessage="Fecha inválida"
                                />
                            )}
                        />
                    </Grid>

                    {/* ESTADO */}
                    <Grid
                        className={classes.column}
                        item
                        xs={12}
                        md={3}
                    >
                        <Controller
                            name="estado"
                            control={formularioConsulta.control}
                            render={({
                                field: {
                                    onChange,
                                    value,
                                },
                            }) => (
                                <CHSelect
                                    label="Estado"
                                    value={value}
                                    onChange={onChange}
                                >
                                    <MenuItem value="">
                                        TODOS
                                    </MenuItem>

                                    <MenuItem value={1}>
                                        VIABLE
                                    </MenuItem>

                                    <MenuItem value={2}>
                                        PENDIENTE
                                    </MenuItem>

                                    <MenuItem value={3}>
                                        RECHAZO
                                    </MenuItem>
                                </CHSelect>
                            )}
                        />
                    </Grid>
                </Grid>
            </CHPaper>

            {/* BOTONES */}
            <div className={classes.buttonsContainer}>
                <Button
                    className={classes.searchButton}
                    variant="contained"
                    onClick={formularioConsulta.handleSubmit(
                        handleSearchButton,
                    )}
                    disabled={loading}
                >
                    CONSULTAR
                </Button>

                <Button
                    className={classes.clearButton}
                    variant="outlined"
                    onClick={handleLimpiar}
                    disabled={loading}
                >
                    LIMPIAR
                </Button>
            </div>

            <LoadingComponent loading={loading} />

            {/* TABLA */}
            {isDataLoaded && (
                <>
                    {registros.length === 0 ? (
                        <div className={classes.noResults}>
                            No se encontraron resultados
                        </div>
                    ) : (
                        <>
                            <div className={classes.tableContainer}>
                                <table className={classes.table}>
                                    <thead className={classes.header}>
                                        <tr>
                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Fecha Consulta TU
                                            </th>

                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Número de Identificación
                                            </th>

                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Nombre
                                            </th>

                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Departamento
                                            </th>

                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Ciudad
                                            </th>

                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Tipo Crédito
                                            </th>

                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Valor Inmueble
                                            </th>

                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Estado
                                            </th>

                                            <th
                                                className={
                                                    classes.headerCell
                                                }
                                            >
                                                Motivo
                                            </th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        {registros.map(registro => (
                                            <tr
                                                key={registro.rtU_ID}
                                                className={classes.row}
                                            >
                                                <td
                                                    className={
                                                        classes.cell
                                                    }
                                                >
                                                    {formatearFechaTabla(
                                                        registro.rtU_FECHA_CONSULTA,
                                                    )}
                                                </td>

                                                <td
                                                    className={
                                                        classes.cell
                                                    }
                                                >
                                                    {
                                                        registro.rtU_NUMERO_IDENTIFICACION
                                                    }
                                                </td>

                                                <td
                                                    className={
                                                        classes.cell
                                                    }
                                                >
                                                    {
                                                        registro.rtU_NOMBRE
                                                    }
                                                </td>

                                                <td
                                                    className={
                                                        classes.cell
                                                    }
                                                >
                                                    {
                                                        registro.rtU_DEPARTAMENTO
                                                    }
                                                </td>

                                                <td
                                                    className={
                                                        classes.cell
                                                    }
                                                >
                                                    {
                                                        registro.rtU_CIUDAD
                                                    }
                                                </td>

                                                <td
                                                    className={
                                                        classes.cell
                                                    }
                                                >
                                                    {
                                                        registro.rtU_TIPO_CREDITO
                                                    }
                                                </td>

                                                <td
                                                    className={
                                                        classes.cell
                                                    }
                                                >
                                                    {formatearMoneda(
                                                        registro.rtU_VALOR_INMUEBLE,
                                                    )}
                                                </td>

                                                <td
                                                    className={
                                                        classes.cell
                                                    }
                                                >
                                                    {obtenerEstado(
                                                        registro.rtU_ESTADO,
                                                    )}
                                                </td>

                                                <td
                                                    className={
                                                        classes.textCell
                                                    }
                                                >
                                                    {
                                                        registro.rtU_MOTIVO
                                                    }
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>

                            <div className={classes.total}>
                                Total registros: {registros.length}
                            </div>
                        </>
                    )}
                </>
            )}
        </CHPrivateContainer>
    );
};

export default ConsultaReporteTransUnion;