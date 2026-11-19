import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import {Button, createStyles, Grid, IconButton, makeStyles, MenuItem, Table, TableBody, TableCell, TableHead, TableRow, Typography } from '@material-ui/core';
import moment from 'moment';
import { Delete, Edit, Visibility } from '@material-ui/icons';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import { Supervision } from '../constants/fondosolidaridadform.model.type';
import NotificationService from '../../../services/notification.service';
import GenerarGuid from '../../AdministracionCuentas/components/funciones/guid.funtions';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';

const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
        },
        titleform: {
            width: '100%',
            textAlign: 'left',
            color: '#1C3F8B',
            padding: 1,
            fontWeight: 'bold',
            fontSize: '1rem',
        },
        columnButtonAdd: {
            textAlign: 'right',
        },
        buttonAdd: {
            width: '200px',
            marginLeft: '.5rem',
        },
        buttonAddEdit: {
            width: '300px',
            marginLeft: '.5rem',
        },
    }),
);

const SupervisorTecnico = (props: Props) => {
    const classes = useStyles(); 
    const apiFondoSolidaridad = ApiFondoSolidaridad();
    const {informacionProyecto, informacionSupervisores, setSupervisiones} = props;
    const [supervisionModel, setSupervisionModel] = useState<Supervision[]>([]);
    const [estadoBoton, setEstadoBoton] = useState<string>('AGREGAR SUPERVISIÓN');
    const [anterior, setAnterior] = useState<string>('');
    const [idAnterior, setIdAnterior] = useState<string>('');
    const [camposLecturaSupervicionTecnica, setCamposLecturaSupervicionTecnica] = useState(false);
    const formularioSupervisorTecnico = useForm({
        defaultValues: {
            id: '',
            personaAtendioVisita: '',
            cargo: '',
            supervisorTecnico: '',
            fechaVisita: new Date(),
            desarrolloProyecto: '',
            observacionesGenerales: '',
        },
    });

    useEffect(() => {
        consultarSupervisionTecnica(informacionProyecto?.codigo);
    },[informacionProyecto])

    const consultarSupervisionTecnica = async(idProyecto: string) => {
        await apiFondoSolidaridad.getConsultarSupervisionTecnica(idProyecto)
        .then(response => {
            if(response.data && response.data.length > 0){
                setCamposLecturaSupervicionTecnica(true);
                const listaSupervisiones = response.data.map((supervision: any) => {
                    if(supervision){
                        const supervisionNueva: Supervision = {
                            id: supervision.idSupervicionTecnica,
                            personaAtendioVisita: supervision.personaAtendioVisita,
                            cargo: supervision.cargo,
                            supervisorTecnico: supervision.supervisoTecnicoTipoDocumento,
                            nombreSupervisorTecnico: supervision.nombreSupervisorTecnico,
                            fechaVisita: supervision.fechaVisita,
                            desarrolloProyecto: supervision.desarrolloProyecto, 
                            observacionesGenerales: supervision.observacionGeneral,
                        }
                        return supervisionNueva;
                    }
                    return null;
              })
              setSupervisionModel(listaSupervisiones);
            }
        }).catch(() => {
            toast(`Error al consultar supervisiones`, {type: toast.TYPE.ERROR});
        })
    }

    const agregarRegistroSupervision = async (data: any) => {
        const guid = GenerarGuid();
        const nombreSupervisor = informacionSupervisores.find(item => item.codigo === data.supervisorTecnico);
        const supervisionNueva: Supervision = {
            id: guid.newGuid(),
            personaAtendioVisita: String(data.personaAtendioVisita).toUpperCase(),
            cargo: data.cargo,
            supervisorTecnico: data.supervisorTecnico,
            nombreSupervisorTecnico: nombreSupervisor?.nombreCompleto || '',
            fechaVisita: data.fechaVisita,
            desarrolloProyecto: data.desarrolloProyecto, 
            observacionesGenerales: data.observacionesGenerales,
        };
        let supervisiones: Supervision[] = [];
        if (estadoBoton === 'AGREGAR SUPERVISIÓN') {

            setSupervisionModel(oldArray => [
                ...oldArray,
                supervisionNueva,
            ]);
            supervisiones = [...supervisionModel];
            supervisiones.push(supervisionNueva);
        } else {
            const temp = [...supervisionModel];
            const posicionActual = temp.findIndex(
                item => item.id === anterior,
            );

            supervisionNueva.id = temp[posicionActual].id;

            if (supervisionNueva.id !== temp[posicionActual].id) {
                temp[posicionActual].id =
                supervisionNueva.id;
            } else {
                supervisionNueva.id = idAnterior;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, supervisionNueva);
            supervisiones = temp;
            setSupervisionModel(temp);
        }
        setSupervisiones(supervisiones);
        setAnterior('');
        resetSupervisiones();
    }

    const seleccionarRegistro= async (item: Supervision) => {
        if(item){
            setAnterior(item?.id || '');
            setIdAnterior(item?.id || '');
            formularioSupervisorTecnico.setValue('id', String(item.id));
            formularioSupervisorTecnico.setValue('personaAtendioVisita', item.personaAtendioVisita);
            formularioSupervisorTecnico.setValue('cargo', String(item.cargo));
            formularioSupervisorTecnico.setValue('supervisorTecnico', String(item.supervisorTecnico));
            formularioSupervisorTecnico.setValue('fechaVisita', new Date (item.fechaVisita));
            formularioSupervisorTecnico.setValue('desarrolloProyecto', item.desarrolloProyecto);
            formularioSupervisorTecnico.setValue('observacionesGenerales', item.observacionesGenerales);
            setEstadoBoton('EDITAR SUPERVISIÓN');
        }
    };

    const eliminarRegistro = async (id: string) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...supervisionModel];
            temp.splice(
                temp.findIndex(item => item.id === id),
                1,
            );
            setSupervisionModel(temp);
            setSupervisiones(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
            resetSupervisiones();
        }
    };

    const resetSupervisiones = () => {
        setEstadoBoton('AGREGAR SUPERVISIÓN');
        formularioSupervisorTecnico.clearErrors();
        formularioSupervisorTecnico.reset();
    }

    console.log('supervisiones',camposLecturaSupervicionTecnica)

    
    return(
        <CHPaper title="Información Supervisión Técnica">
            <Grid container>
             <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="personaAtendioVisita"
                        control={formularioSupervisorTecnico.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label='Persona Atendió Visita *'
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={camposLecturaSupervicionTecnica}
                                InputLabelProps={{ shrink: true }}
                                inputProps={{
                                    style: {
                                        textTransform: 'uppercase',
                                    },
                                }}
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
             </Grid>
             <Grid className={classes.column} item xs={12} md={3}>
                    <Controller
                        name="cargo"
                        control={formularioSupervisorTecnico.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                label='Cargo *'
                                error={!!error}
                                helperText={error ? error.message : null}
                                inputProps={{
                                    style: {
                                        textTransform: 'uppercase',
                                    },
                                }}
                                isReadOnly={camposLecturaSupervicionTecnica}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
             </Grid>
             <Grid className={classes.column} item xs={12} md={4}>
                    <Controller
                        name="supervisorTecnico"
                        control={formularioSupervisorTecnico.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                        <CHSelect
                            readOnly={camposLecturaSupervicionTecnica}
                            label="Supervisor Técnico *"
                            value={value}
                            onChange={onChange}
                            error={!!error}
                            helperText={
                                error ? error.message : null
                            }
                        >
                            {informacionSupervisores.map(
                                (option: any) => (
                                    <MenuItem
                                        key={option.codigo}
                                        value={option.codigo}
                                    >
                                        {option.nombreCompleto}
                                    </MenuItem>
                                ),
                            )}
                        </CHSelect>
                        )}
                    />
             </Grid>
             <Grid className={classes.column} item xs={12} md={2}>
                    <Controller
                        name="fechaVisita"
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        control={formularioSupervisorTecnico.control}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHDatePicker
                                label="Fecha Visita *"
                                InputLabelProps={{ shrink: true}}
                                onChange={onChange}
                                value={value}
                                error={!!error}
                                inputProps={{readOnly: camposLecturaSupervicionTecnica}}
                                helperText={error ? error.message : null}
                                readOnly={camposLecturaSupervicionTecnica}
                                autoOk
                            />
                        )}
                    />
                </Grid>
            </Grid>
            <Grid container>
                <Grid className={classes.column} item xs={12} md={12}>
                    <Controller
                        name="desarrolloProyecto"
                        control={formularioSupervisorTecnico.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={camposLecturaSupervicionTecnica}
                                label="Desarrollo General Proyecto *"
                                value={value}
                                onChange={e => {
                                    if(e.target.value.length <= 350){
                                        onChange(e);
                                    }
                                }}
                                multiline
                                rows={6}
                                InputLabelProps={{ shrink: true }}
                                inputProps={{
                                    style: {
                                        textTransform: 'uppercase',
                                    },
                                }}
                            />
                        )}
                    />
                </Grid>
            </Grid>
            <Grid container>
                <Grid className={classes.column} item xs={12} md={12}>
                    <Controller
                        name="observacionesGenerales"
                        control={formularioSupervisorTecnico.control}
                        rules={{
                            required: 'Este campo es requerido.',
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={camposLecturaSupervicionTecnica}
                                label="Observaciones Generales *"
                                value={value}
                                onChange={e => {
                                    if(e.target.value.length <= 350){
                                        onChange(e);
                                    }
                                }}
                                multiline
                                rows={6}
                                InputLabelProps={{ shrink: true }}
                                inputProps={{
                                    style: {
                                        textTransform: 'uppercase',
                                    },
                                }}
                            />
                        )}
                    />
                </Grid>
            </Grid>
            <Grid container>
                    <Grid
                        className={classes.columnButtonAdd}
                        item
                        xs={12}
                        md={12}
                    >
                        {!camposLecturaSupervicionTecnica && (
                            <Button
                                className={classes.buttonAddEdit}
                                variant="outlined"
                                onClick={formularioSupervisorTecnico.handleSubmit(agregarRegistroSupervision)}
                            >
                                {estadoBoton}
                            </Button>
                         )}
                        <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={resetSupervisiones}
                        >
                            CANCELAR
                        </Button>
                    </Grid>
                </Grid>
                <Typography
                    className={classes.titleform}
                    variant="h5"
                    component="h6"
                >
                    Lista de Supervisores Técnicos
                </Typography>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Fecha Visita</TableCell>
                            <TableCell>Supervisor Técnico</TableCell>
                            <TableCell>Persona que Antendió la Visita</TableCell>
                            <TableCell align="center">
                                Acciones
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {supervisionModel.map(
                            (option: Supervision) => (
                                <TableRow key={option.id}>
                                    <TableCell>{moment(option.fechaVisita).format('YYYY-MM-DD')}</TableCell>
                                    <TableCell>{option.nombreSupervisorTecnico}</TableCell>
                                    <TableCell>{option.personaAtendioVisita}</TableCell>
                                    <TableCell align="center" style={{ width: '200px' }}>
                                        <IconButton
                                            aria-label="Editar"
                                            color="primary"
                                            size="small"
                                            onClick={() => {
                                                seleccionarRegistro(
                                                    option,
                                                );
                                            }}
                                        >
                                            {
                                                camposLecturaSupervicionTecnica ? 
                                                <Visibility /> : <Edit /> 
                                                
                                            }
                                            
                                        </IconButton>
                                        {!camposLecturaSupervicionTecnica && (
                                                <IconButton
                                                    aria-label="Eliminar"
                                                    color="primary"
                                                    size="small"
                                                    onClick={() => {
                                                        eliminarRegistro(
                                                            option.id || '',
                                                        );
                                                    }}
                                                >
                                                    <Delete />
                                               </IconButton>
                                        )}
                                    </TableCell>
                                </TableRow>
                            )
                        )}
                    </TableBody>
                </Table>
        </CHPaper>
    )
}

interface Props {
    informacionProyecto: any;
    informacionSupervisores: any[];
    setSupervisiones: (value: Supervision[]) => void;
}

export default SupervisorTecnico;