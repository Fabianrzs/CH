import React, { useEffect, useState } from "react";
import moment from "moment";
import { Controller, useForm } from "react-hook-form";
import { connect } from 'react-redux';
import { Grid, makeStyles, createStyles, MenuItem } from '@material-ui/core';
import CHPaper from '../../../components/ch-paper/CHPaper';
import { CatalogState } from '../../../reducers/catalog.reducer';
import { ClienteDto, Solicitud } from '../../../types/client.model.type';
import UtilService from '../../../utils/util.service';
import { formatDate } from "../../../utils/format";
import CHDatePickerV7 from "../../../components/ch-datepicker/CHDatepickerV7";
import CHSelect from "../../../components/ch-select/CHSelect";


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        color: {
            color: '#061311'
        }
    }),
);

function Solicitudes({
    catalogState,
    clientModel,
}: {
    catalogState: CatalogState;
    clientModel: ClienteDto;
}) {
    const classes = useStyles();
    const datee = new Date("2018/01/30 23:30:14");
    const today = new Date();
    const [solicitudesFiltrados, setSolicitudes] = useState<Solicitud[]>(clientModel?.solicitudes);
    const { control, setValue, handleSubmit, reset } = useForm({
        defaultValues: {
            fecha: null,
            tipo: '',
        },
    });


    const generarLink = (numeroRadicado: string, tipoSolicitud: string) => {
        let link = '';
        if (tipoSolicitud === 'POSTULACIÓN FONDO SOLIDARIDAD') {
            link = `/Postulacion/S/${numeroRadicado}`;
        } else {
            link = `/GestionSolicitudes/${numeroRadicado}`;
        }
        return link;
    }

    const [selectedYear, setSelectedYear] = useState<number | undefined>(0);
    const [selectedTipoSolicitud, setSelectedTipoSolicitud] = useState<string>('0');

    // Lista de años (puedes generar dinámicamente si es necesario)
    const years = [2022, 2023, 2024, 2025]; // Añade los años que necesites

    // Lista de tipos de solicitudes (puedes actualizarla con los tipos necesarios)
    const tiposSolicitudes = ['Tipo 1', 'Tipo 2', 'Tipo 3'];

    useEffect(() => {
        setSolicitudes(clientModel?.solicitudes);
        if (selectedYear !== 0 && selectedYear !== undefined) {
            if (selectedTipoSolicitud !== '0') {
                const filters = clientModel?.solicitudes.filter(x => moment(x.fechaFinSolicitud).year() === selectedYear && x.tipoSolicitud === selectedTipoSolicitud.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLocaleUpperCase())
                setSolicitudes(filters);
            } else {
                const filters = clientModel?.solicitudes.filter(x => moment(x.fechaFinSolicitud).year() === selectedYear)
                setSolicitudes(filters);
            }

        } else if (selectedTipoSolicitud !== '0') {
            const filters = clientModel?.solicitudes.filter(x => x.tipoSolicitud.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLocaleUpperCase() === selectedTipoSolicitud.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLocaleUpperCase())
            setSolicitudes(filters);
        }


    }, [selectedYear, selectedTipoSolicitud, clientModel?.solicitudes]);

    const tiposSolicitudUnicos = React.useMemo(() => {
        if (!solicitudesFiltrados || solicitudesFiltrados.length === 0) {
            return [];
        }

        return Array.from(
            new Set(
                solicitudesFiltrados
                    .map(s => s.tipoSolicitud)
                    .filter(Boolean)
            )
        );
    }, [solicitudesFiltrados]);



    return (
        <CHPaper title="Solicitudes">

            <Grid container>
                <Grid className={classes.column} item xs={1} md={1}>
                    <Controller
                        name="fecha"
                        control={control}
                        render={({ field: { onChange, value }, fieldState: { error } }) => (
                            <CHDatePickerV7
                                error={!!error}
                                helperText={error ? error.message : ""}
                                label="Año"
                                onChange={(e) => {
                                    const selectedDate = e;
                                    onChange(selectedDate);
                                    setSelectedYear(selectedDate?.getFullYear())

                                }}
                                value={value}

                                minDate={new Date("1-01-1980")}
                                maxDate={today}
                            />
                        )}
                    />
                </Grid>
                {/* <Grid className={classes.column} item xs={6} md={6}>
                    <Controller
                        name="tipo"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                error={!!error}
                                label="Tipo"
                                value={value}
                                helperText={error ? error.message : null}
                                onChange={(e) => {
                                    const selectedtype = String(e.target.value);
                                    onChange(selectedtype);
                                    setSelectedTipoSolicitud(selectedtype)
                                }}
                            >
                                <MenuItem className={classes.color} value="0">SELECCIONE</MenuItem>
                                {
                                    (clientModel?.tiposSolicitud ?? []).map((element: any) => (
                                        <MenuItem className={classes.color} key={element.idTipoSolicitud} value={element.descripcionTipoSolicitud}>
                                            {element.descripcionTipoSolicitud}
                                        </MenuItem>
                                    ))

                                }
                            </CHSelect>
                        )}
                    />
                </Grid> */}

                <Grid className={classes.column} item xs={6} md={6}>
                    <Controller
                        name="tipo"
                        control={control}
                        rules={{
                            required: 'Este campo es requerido',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHSelect
                                error={!!error}
                                label="Tipo"
                                value={value}
                                helperText={error ? error.message : null}
                                onChange={(e) => {
                                    const selectedType = String(e.target.value);
                                    onChange(selectedType);
                                    setSelectedTipoSolicitud(selectedType);
                                }}
                            >
                                <MenuItem className={classes.color} value="0">
                                    SELECCIONE
                                </MenuItem>

                                {tiposSolicitudUnicos.map((tipo) => (
                                    <MenuItem
                                        className={classes.color}
                                        key={tipo}
                                        value={tipo}
                                    >
                                        {tipo}
                                    </MenuItem>
                                ))}
                            </CHSelect>
                        )}
                    />
                </Grid>

            </Grid>


            <table className="table">
                <thead>
                    <tr>
                        <th>Número solicitud</th>
                        <th>Radicado</th>
                        <th>Tipo de solicitud</th>
                        <th>Modalidad</th>
                        <th>Fecha inicio solicitud</th>
                        <th>Fecha fin solicitud</th>
                        <th>Valor</th>
                        <th>Estado</th>
                    </tr>
                </thead>

                <tbody>
                    {solicitudesFiltrados && solicitudesFiltrados.length > 0 ? (
                        solicitudesFiltrados.map((element, index) => (
                            <tr key={`table-schedule${index}`}>
                                <td className='dateReport3'>{element.numeroSolicitud}</td>
                                <td className='dateReport3'>
                                    <a href={generarLink(element.radicado, element.tipoSolicitud)}>{element.radicado}</a>
                                </td>
                                <td className='dateReport3'>{element.tipoSolicitud}</td>
                                <td className='dateReport3'>{element.modalidad !== '' ? element.modalidad : 'N/A'}</td>
                                <td className='dateReport3'>
                                    {moment(String(element.fechaInicioSolicitud).split('T')[0]).format("DD-MM-YYYY")}
                                </td>
                                <td className='dateReport3'>
                                    {moment(String(element.fechaFinSolicitud).split('T')[0]).format("DD-MM-YYYY")}
                                </td>
                                <td className='dateReport3'>{UtilService.formatCurrency(element.valorPagado)}</td>
                                <td className='dateReport3'>
                                    {element.actividad && element.actividad.includes("Trámite de pago") && element.estadoSolicitud === "APROBADA"
                                        ? "PROCESADA"
                                        : element.estadoSolicitud}
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan={8} style={{ textAlign: 'center', padding: '1rem' }}>
                                No hay solicitudes disponibles.
                            </td>
                        </tr>
                    )}
                </tbody>

            </table>
        </CHPaper>
    );
}
export default connect()(Solicitudes);
