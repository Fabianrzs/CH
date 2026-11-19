import { Button, createStyles, 
        Grid, 
        IconButton,
        makeStyles,
        MenuItem,
        } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import { Controller, UseFormReturn } from 'react-hook-form';
import { toast } from 'react-toastify';
import {Edit } from '@material-ui/icons';
import CHAutoComplete from '../../../components/ch-auto-complete/CHAutoComplete';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHPrivateContainer from '../../../components/ch-private-container/CHPrivateContainer';
import CHSelect from '../../../components/ch-select/CHSelect';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import { ACTIVIDAD_POSTULACION, catalogPostulacionDto, 
         ESTADO_POSTULACION, 
         imputabilidad_servicio_disminucion_capacidad, 
         imputabilidad_servicio_muerte, 
         porcentajePostulacionLista, 
         TIPO_SUBMODALIDAD} from '../constants/fondoSolidaridad.constants';
import { Postulante } from '../constants/fondosolidaridadform.model.type';
import ApiFondoSolidaridad from '../../../services/api-fondoSolidaridad';
import NotificationService from '../../../services/notification.service';

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
   processButton: {
        backgroundColor: '#1C3F8B',
        color: '#fff',
        margin: '8px 0',
   },
   inline: {
    display: 'inline',
    },
    columnButtonAdd: {
        textAlign: 'right',
    },
    buttonAddEdit: {
        width: '250px',
        marginLeft: '.5rem',
    },
    buttonAdd: {
        width: '200px',
        marginLeft: '.5rem',
    },
 }),
);

function Datos({
    postulantes,
    totalPorcentajePostulacion,
    totalPorcentajeAportes,
    totalPorcentajeCesantias,
    camposCalificacionSoloLectura,
    seleccionarRegistro,
}:{
    postulantes: Postulante[];
    totalPorcentajePostulacion: number;
    totalPorcentajeAportes: number;
    totalPorcentajeCesantias: number;
    camposCalificacionSoloLectura: boolean;
    seleccionarRegistro: (beneficiario: any) => void;
}) {

    return (
         <>
             <table className="credit-table"    
             style={{
                 margin: '20px',
                 width: '95%',
             }}>
               <thead>
                   <tr>
                       <th style={{ textAlign: 'center' }}>
                           Nombres y Apellidos
                       </th>
                       <th style={{ textAlign: 'center' }}>
                           Porcentaje de Subsidio
                       </th>
                       <th style={{ textAlign: 'center' }}>
                           Porcentaje de Aportes
                       </th>
                       <th style={{ textAlign: 'center' }}>
                           Porcentaje de Cesantías
                       </th>
                       {camposCalificacionSoloLectura === false &&(
                          <th style={{ textAlign: 'center' }}>
                              Acciones
                          </th>
                       )}
                   </tr>
               </thead>
               <tbody>
                        {
                            postulantes?.map((row, index) => (
                                <tr  key={`table-${index}`}>
                                   <td>{`${row?.nombre} ${row.apellido}`}</td>
                                   <td>{row?.porcentajePostulacion || ''}</td>
                                   <td>{row?.porcentajeAportes || ''}</td>
                                   <td>{row?.porcentajeCesantias || ''}</td>
                                   {camposCalificacionSoloLectura === false &&(
                                        <td>
                                            <IconButton
                                                    aria-label="Editar"
                                                    color="primary"
                                                    size="small"
                                                    onClick={() => {
                                                        seleccionarRegistro(
                                                            row,
                                                        );
                                                    }}
                                                >
                                                    <Edit />
                                                </IconButton>
                                        </td>
                                   )}
                                </tr>
                            ))
                        }
                     <tr>
                         <td><b>TOTAL PORCENTAJE</b></td>
                         <td><b>{totalPorcentajePostulacion}%</b></td>
                         <td><b>{totalPorcentajeAportes}%</b></td>
                         <td><b>{totalPorcentajeCesantias}%</b></td>
                     </tr>
               </tbody>
           </table>
         </>
     );
 }
 

const Calificacion =  (props: props) => {

const classes = useStyles();
const apiFondoSolidaridad = ApiFondoSolidaridad();

const {datosSolicitudPostulacion,
       formularioCalificacion,
       formularioPostulacion,
       formularioEntrevista,
       postulantesControl,
       listaPostulantes,
       imputabilidadValue,
       cliente,
       postulantesTable,
       totalPorcentajePostulacion,
       totalPorcentajeAportes,
       totalPorcentajeCesantias,
       nullImputabilidadValue,
       camposCalificacionSoloLectura,
       mostrarCampoPorcentajeDisminucion,
       onChangeImputabilidad,
       setPostulantesTable,
       setTotalPorcentajePostulacion,
       setTotalPorcentajeAportes,
       consultarCalificacion,
       setTotalPorcentajeCesantias} = props;
const [informacionPostulante,setInformacionPostulante] = useState<any>(null);
const [mostrarPorcentajes,setMostrarPorcentajes] = useState(false);
const [mostrarMensaje,setMostrarMensaje] = useState(false);

useEffect(() => {
    consultarEntrevista(datosSolicitudPostulacion.idSolicitud);
    consultarCalificacion(datosSolicitudPostulacion.idSolicitud);

    if(cliente && cliente?.solicitudes && cliente?.solicitudes.length > 0){
                   
        const solicitudesPrevias = cliente?.solicitudes.filter((solicitud: any) => 
        (solicitud.tipoSolicitud === 'FONDO SOLIDARIDAD 1305' ||
        solicitud.tipoSolicitud === 'FONDO SOLIDARIDAD ACUERDO 01/2016' ||
        solicitud.tipoSolicitud === 'GIRO DE RECURSOS FONDO SOLIDARIDAD' ||
        solicitud.tipoSolicitud === 'GIRO DE RECURSOS FONDO SOLIDARIDAD ACUERDO 01/2016' ||
        solicitud.tipoSolicitud === 'GIRO RECURSOS FONDO SOLIDARIDAD 1305' ||
        solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD' ||
        solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD ACUERDO 01/2016' ||
        solicitud.tipoSolicitud === 'PERDIDA DE BENEFICIO FONDO DE SOLIDARIDAD' ||
        solicitud.tipoSolicitud === 'ADJUDICACIÓN FONDO DE SOLIDARIDAD' ||
        solicitud.tipoSolicitud === 'POSTULACIÓN FONDO SOLIDARIDAD') 
        && new Date(solicitud.fechaInicioSolicitud) < new Date(datosSolicitudPostulacion?.fechaCreacion));

        if(solicitudesPrevias && solicitudesPrevias.length === 1 && !mostrarMensaje){
            setMostrarMensaje(true);
            NotificationService.genericAlert(
                'Información',
                `El Afiliado presenta una solicitud previa de ${solicitudesPrevias?.tipoSolicitud}`,
                'warning'
            );
        }else if(solicitudesPrevias && solicitudesPrevias.length > 1 && !mostrarMensaje){
            setMostrarMensaje(true);
            const tipoSolicitudesUnicas: string[] = Array.from(
                new Set(solicitudesPrevias.map((item: any) => item.tipoSolicitud))
            );
            const descripcionSolicitudesPrevias: string = tipoSolicitudesUnicas.join('\n');
            NotificationService.genericAlert(
                'Información',
                `El Afiliado presenta solicitudes previas de: \n ${descripcionSolicitudesPrevias}`,
                'warning'
            );
        }
    }

},[datosSolicitudPostulacion,cliente])

const consultarEntrevista = async (idSolicitud: string) => {
    await apiFondoSolidaridad
        .getConsultarEntrevista(idSolicitud)
        .then(async response => {
            if(response.data){
                const array: any[] =[];
                let totalPorcentajePostulacion = 0;
                let totalPorcentajeAportes = 0;
                let totalPorcentajeCesantias = 0;
                response.data?.postulantes?.forEach((item: any) => {
                   
                    if(item.tipoIntegranteId === 'MAFP' ||
                       item.tipoIntegranteId === 'BP'){
                        if(item?.porcentajePostulacion){
                            totalPorcentajePostulacion += item?.porcentajePostulacion;
                        }
                        if(item?.porcentajeAportes){
                            totalPorcentajeAportes += item?.porcentajeAportes;
                        }
                        if(item?.porcentajeCesantias){
                            totalPorcentajeCesantias += item?.porcentajeCesantias;
                        }
                        array.push(item);
                    }
                })
                setPostulantesTable(array);
                setTotalPorcentajePostulacion(totalPorcentajePostulacion);
                setTotalPorcentajeAportes(totalPorcentajeAportes);
                setTotalPorcentajeCesantias(totalPorcentajeCesantias);
            }
        })
        .catch(() => {
            toast('Error del sistema', { type: toast.TYPE.ERROR });
        });
};

const funcionValidar = () => {
    let mensaje = '';
    if(typeof formularioCalificacion.watch('porcentajeDisminucion') === 'string' 
             && formularioCalificacion.watch('porcentajeDisminucion').includes('%')
             && (Number(formularioCalificacion.watch('porcentajeDisminucion').replace('%','')) <= 0 ||
                Number(formularioCalificacion.watch('porcentajeDisminucion').replace('%','') > 100))){
        mensaje = 'El valor debe ser mayor a 0 y menor o igual a 100';
    }else if(typeof formularioCalificacion.watch('porcentajeDisminucion') === 'number'
             &&(formularioCalificacion.watch('porcentajeDisminucion') <= 0 ||
             formularioCalificacion.watch('porcentajeDisminucion') > 100)) {
        mensaje = 'El valor debe ser mayor a 0 y menor o igual a 100';
    }
    return mensaje !== '' ? mensaje : true;
}

const seleccionarRegistro = (postulante: any) => {

    setMostrarPorcentajes(true);
    if(postulante?.porcentajePostulacion === 0){
        postulantesControl.setValue('porcentajePostulacion','');
    }else{
        postulantesControl.setValue('porcentajePostulacion',postulante?.porcentajePostulacion);
    }

    if(postulante?.porcentajeAportes === 0){
        postulantesControl.setValue('porcentajeAportes','');
    }else{
        postulantesControl.setValue('porcentajeAportes',postulante?.porcentajeAportes);
    }

    if(postulante?.porcentajeCesantias === 0){
        postulantesControl.setValue('porcentajeCesantias','');
    }else{
        postulantesControl.setValue('porcentajeCesantias',postulante?.porcentajeCesantias);
    }
    setInformacionPostulante(postulante);
}

const actualizarEntrevistaPostulantes = async(data: any) => {

    let postulantes: Postulante[] = [];
    if(listaPostulantes){

        // validar porcentaje postulación no sea mayor a 100
        let totalPostulacion = 0;
        const postulacion = Number(String(data?.porcentajePostulacion).replace('%',''));
        listaPostulantes.forEach((value, index) => {
            if(value.porcentajePostulacion){
                if(value?.numeroDocumento === informacionPostulante?.numeroDocumento){
                    totalPostulacion += postulacion;
                 }else{
                    totalPostulacion += value.porcentajePostulacion;
                 }
            }
        })

        if(totalPostulacion > 100){
            toast('El valor del porcentaje postulación supera el 100%', {
                type: toast.TYPE.ERROR,
            });
            return;
        }

        // validar porcentaje aportes no sea mayor a 100
           let totalAportes = 0;
           const aportes = Number(String(data?.porcentajeAportes).replace('%',''));
           listaPostulantes.forEach((value, index) => {
               if(value.porcentajeAportes){
                   if(value?.numeroDocumento === informacionPostulante?.numeroDocumento){
                        totalAportes += postulacion;
                    }else{
                        totalAportes += value.porcentajeAportes;
                    }
               }
           })
   
           if(totalAportes > 100){
               toast('El valor del porcentaje aportes supera el 100%', {
                   type: toast.TYPE.ERROR,
               });
               return;
           }

        // validar porcentaje aportes no sea mayor a 100
            let totalCesantias = 0;
            const cesantias = Number(String(data?.porcentajeCesantias).replace('%',''));
            listaPostulantes.forEach((value, index) => {
                if(value.porcentajeCesantias){
                    if(value?.numeroDocumento === informacionPostulante?.numeroDocumento){
                        totalCesantias += postulacion;
                     }else{
                        totalCesantias += value.porcentajeCesantias;
                     }
                }
            })
    
            if(totalCesantias > 100){
                toast('El valor del porcentaje cesantias supera el 100%', {
                    type: toast.TYPE.ERROR,
                });
                return;
            }

        // se ingresan porcentajes
        postulantes = listaPostulantes.map(item => {
            if(item.numeroDocumento === informacionPostulante?.numeroDocumento){
                const postulanteNuevo: Postulante = {
                    ...item,
                    porcentajePostulacion: postulacion,
                    porcentajeAportes: aportes,
                    porcentajeCesantias: cesantias,
                }
                return postulanteNuevo;
            }
            return item as Postulante;
        })
    }

    const request = {
        efS_ID: formularioEntrevista.watch('efS_ID'),
        efS_ID_SOLICITUD: datosSolicitudPostulacion.idSolicitud,
        efS_VIVIENDA_PROPIA: formularioEntrevista.watch('efS_VIVIENDA_PROPIA'),
        efS_TIPO_VIVIENDA: formularioEntrevista.watch('efS_TIPO_VIVIENDA'),
        efS_HABITANTE: Number(formularioEntrevista.watch('efS_HABITANTE')),
        efS_INFORMATIVO_ADMIN: formularioEntrevista.watch('efS_INFORMATIVO_ADMIN'),
        efS_CONCEPTO_PSICOSOCIAL: formularioEntrevista.watch('efS_CONCEPTO_PSICOSOCIAL'),
        efS_OBSERVACION: formularioEntrevista.watch('efS_OBSERVACION'),
        postulantes: postulantes,
    }
    await apiFondoSolidaridad
    .postActualizarEntrevista(request)
    .then(() => {
        setMostrarPorcentajes(false);
        NotificationService.genericAlert(
            'Porcentajes Actualizados con éxito',
            'Se realizó con éxito la actualización.',
        )
    })
    .catch((e) => {
            toast(`Error actualizar porcentajes en calificación ${e.message}`, { type: toast.TYPE.ERROR });
    });
    consultarEntrevista(datosSolicitudPostulacion.idSolicitud);
}

const cancelar = () => {
    setMostrarPorcentajes(false);
    postulantesControl.setValue('porcentajePostulacion','');
    postulantesControl.setValue('porcentajeAportes','');
    postulantesControl.setValue('porcentajeCesantias','');
}

const validarPorcentajePostulacion = () => {
    let mensaje = '';
    if(typeof postulantesControl.watch('porcentajePostulacion') === 'string' 
             && postulantesControl.watch('porcentajePostulacion').includes('%')
             && (Number(postulantesControl.watch('porcentajePostulacion').replace('%','')) <= 0 ||
                Number(postulantesControl.watch('porcentajePostulacion').replace('%','') > 100))){
        mensaje = 'El valor debe ser mayor a 0 y menor o igual a 100';
    }else if(typeof postulantesControl.watch('porcentajePostulacion') === 'number'
             &&(postulantesControl.watch('porcentajePostulacion') <= 0 ||
             postulantesControl.watch('porcentajePostulacion') > 100)) {
        mensaje = 'El valor debe ser mayor a 0 y menor o igual a 100';
    }
    return mensaje !== '' ? mensaje : true;
}

const validarPorcentajeAportes = () => {
    let mensaje = '';
    if(typeof postulantesControl.watch('porcentajeAportes') === 'string' 
             && postulantesControl.watch('porcentajeAportes').includes('%')
             && (Number(postulantesControl.watch('porcentajeAportes').replace('%','')) <= 0 ||
                Number(postulantesControl.watch('porcentajeAportes').replace('%','') > 100))){
        mensaje = 'El valor debe ser mayor a 0 y menor o igual a 100';
    }else if(typeof postulantesControl.watch('porcentajeAportes') === 'number'
             &&(postulantesControl.watch('porcentajeAportes') <= 0 ||
             postulantesControl.watch('porcentajeAportes') > 100)) {
        mensaje = 'El valor debe ser mayor a 0 y menor o igual a 100';
    }
    return mensaje !== '' ? mensaje : true;
}

const validarPorcentajeCesantias = () => {
    let mensaje = '';
    if(typeof postulantesControl.watch('porcentajeCesantias') === 'string' 
             && postulantesControl.watch('porcentajeCesantias').includes('%')
             && (Number(postulantesControl.watch('porcentajeCesantias').replace('%','')) <= 0 ||
                Number(postulantesControl.watch('porcentajeCesantias').replace('%','') > 100))){
        mensaje = 'El valor debe ser mayor a 0 y menor o igual a 100';
    }else if(typeof postulantesControl.watch('porcentajeCesantias') === 'number'
             &&(postulantesControl.watch('porcentajeCesantias') <= 0 ||
             postulantesControl.watch('porcentajeCesantias') > 100)) {
        mensaje = 'El valor debe ser mayor a 0 y menor o igual a 100';
    }
    return mensaje !== '' ? mensaje : true;
}

return (
   <>
       <CHPrivateContainer>
                <CHPaper title="Información Calificación">
                <Grid container>
                    <Grid className={classes.column} item xs={3} md={6}>
                          <Controller
                            name="tipoImputabilidad"
                            control={formularioCalificacion.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Sub-Modalidad (Calidad Postulación) *"
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={3} md={6}>
                        {camposCalificacionSoloLectura? (
                              <Controller
                              name="imputabilidadAlServicio"
                              control={formularioCalificacion.control}
                              rules={{
                                  required:
                                      'Este campo es requerido.',
                              }}
                              render={({ fieldState: { error } }) => (
                                  <CHAutoComplete
                                      value={
                                          nullImputabilidadValue
                                              ? null
                                              : imputabilidadValue
                                      }
                                      options={
                                         datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA || 
                                         datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA? 
                                         imputabilidad_servicio_muerte : imputabilidad_servicio_disminucion_capacidad
                                      }
                                      getOptionLabel={option =>
                                          option.descripcion
                                      }
                                      open={false}
                                      disableClearable
                                      onChange={(
                                          evento: any,
                                          imputabilidad: any,
                                      ) => {
                                          onChangeImputabilidad(
                                              evento,
                                              imputabilidad,
                                          );
                                      }}
                                      renderInput={params => (
                                          <CHTextField
                                              {...params}
                                              error={!!error}
                                              helperText={
                                                  error
                                                      ? error.message
                                                      : null
                                              }
                                              InputLabelProps={{
                                                  shrink: true,
                                              }}
                                              isReadOnly
                                              label="Imputabilidad al Servicio *"
                                          />
                                      )}
                                  />
                              )}
                            />
                        ): (
                            <Controller
                            name="imputabilidadAlServicio"
                            control={formularioCalificacion.control}
                            rules={{
                                required:
                                    'Este campo es requerido.',
                            }}
                            render={({ fieldState: { error } }) => (
                                <CHAutoComplete
                                    value={
                                        nullImputabilidadValue
                                            ? null
                                            : imputabilidadValue
                                    }
                                    options={
                                         datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA || 
                                         datosSolicitudPostulacion.idSubModalidad === TIPO_SUBMODALIDAD.BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA? 
                                         imputabilidad_servicio_muerte : imputabilidad_servicio_disminucion_capacidad
                                    }
                                    getOptionLabel={option =>
                                        option.descripcion
                                    }
                                    onChange={(
                                        evento: any,
                                        imputabilidad: any,
                                    ) => {
                                        onChangeImputabilidad(
                                            evento,
                                            imputabilidad,
                                        );
                                    }}
                                    renderInput={params => (
                                        <CHTextField
                                            {...params}
                                            error={!!error}
                                            helperText={
                                                error
                                                    ? error.message
                                                    : null
                                            }
                                            InputLabelProps={{
                                                shrink: true,
                                            }}
                                            label="Imputabilidad al Servicio *"
                                        />
                                    )}
                                />
                            )}
                          />
                        )}
                      
                         </Grid>
                    </Grid>
                    <Grid container>
                    <Grid className={classes.column} item xs={12} md={6}>
                        <Controller
                            name="porcentajePostulacion"
                            control={formularioCalificacion.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                            <CHSelect
                                readOnly={camposCalificacionSoloLectura}
                                label="Porcentaje Postulación *"
                                value={value}
                                error={!!error}
                                helperText={error ? error.message : null}
                                onChange={onChange}
                            >
                              {porcentajePostulacionLista?.map(
                                    (option: catalogPostulacionDto) => (
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
                    <Grid className={classes.column} item xs={12} md={3}>
                          <Controller
                            name="hecho"
                            control={formularioCalificacion.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Causal *"
                                        InputLabelProps={{ shrink: true }}
                                        value={value === '0'? 'DISCAPACIDAD':'FALLECIDO'}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                        isReadOnly
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                />
                            )}
                        />
                </Grid>
                { mostrarCampoPorcentajeDisminucion && (
                <Grid className={classes.column} item xs={12} md={3}>
                <Controller
                        name="porcentajeDisminucion"
                        control={formularioCalificacion.control}
                        rules={{
                            required: 'Este campo es requerido',
                            validate: funcionValidar
                        }}
                        render={({
                            field: { onChange, value },
                            fieldState: { error },
                        }) => (
                            <CHTextField
                                percentMask
                                label="Porcentaje Disminución *"
                                error={!!error}
                                helperText={error ? error.message : null}
                                isReadOnly={camposCalificacionSoloLectura}
                                InputLabelProps={{ shrink: true }}
                                value={value}
                                onChange={onChange}
                            />
                        )}
                    />
                </Grid>
              )}
              <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="disfrutePension"
                            control={formularioCalificacion.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHSelect
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    label="Disfrute de Pensión *"
                                    value={value}
                                    onChange={onChange}
                                    readOnly
                                >
                                    <MenuItem key='SI' value='SI'>
                                            SI
                                    </MenuItem>
                                    <MenuItem key='NO' value='NO'>
                                            NO
                                    </MenuItem>
                                </CHSelect>
                              
                            )}
                        />
                    </Grid>
              </Grid>
             <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="datosResolucion"
                            control={formularioCalificacion.control}
                            rules={{
                                required: 'Este campo es requerido',
                            }}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                    <CHTextField
                                        error={!!error}
                                        label="Datos Resolución de Prestaciones Sociales *"
                                        InputLabelProps={{ shrink: true }}
                                        value={value}
                                        helperText={error ? error.message : null}
                                        onChange={onChange}
                                        isReadOnly={camposCalificacionSoloLectura}
                                        inputProps={{
                                            style: {
                                                textTransform: 'uppercase',
                                            },
                                        }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="solicitudesPrevias"
                            control={formularioCalificacion.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    label="Solicitudes Previas"
                                    error={!!error}
                                    helperText={error ? error.message : null}
                                    isReadOnly
                                    InputLabelProps={{ shrink: true }}
                                    value={value}
                                    onChange={onChange}
                               />
                            )}
                        />
                    </Grid>
                  </Grid>
                  <Grid container>
                    <Grid className={classes.column} item xs={12} md={3}>
                        <Controller
                            name="afectacionesCuenta"
                            control={formularioPostulacion.control}
                            render={({
                                field: { onChange, value },
                                fieldState: { error },
                            }) => (
                                <CHTextField
                                    isReadOnly
                                    label="Afectaciones en la Cuenta"
                                    value={value}
                                    onChange={onChange}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid className={classes.column} item xs={12} md={3}>
                    {formularioPostulacion.watch('afectacionesCuenta') === 'SI' && (
                         <Controller
                         name="valorAfectaciones"
                         control={formularioPostulacion.control}
                         render={({
                             field: { onChange, value },
                             fieldState: { error },
                         }) => (
                             <CHTextField
                                 currencyMask
                                 error={!!error}
                                 helperText={error ? error.message : null}
                                 label="Valor Afectaciones"
                                 value={value}
                                 onChange={onChange}
                                 isReadOnly={!(datosSolicitudPostulacion.actividad === ACTIVIDAD_POSTULACION.CALIFICACION &&
                                               datosSolicitudPostulacion.estadoProceso === ESTADO_POSTULACION.PENDIENTE)}
                                 InputLabelProps={{ shrink: true }}
                             />
                         )}
                     />
                    )}
                    </Grid>
                  </Grid>
              <Datos
                postulantes={postulantesTable}
                totalPorcentajePostulacion={totalPorcentajePostulacion}
                totalPorcentajeAportes={totalPorcentajeAportes}
                totalPorcentajeCesantias={totalPorcentajeCesantias}
                camposCalificacionSoloLectura={camposCalificacionSoloLectura}
                seleccionarRegistro={seleccionarRegistro}
              />
              {mostrarPorcentajes && (
                 <Grid container>
                 <Grid className={classes.column} item xs={12} md={4}>
                       <Controller
                           name="porcentajePostulacion"
                           control={postulantesControl.control}
                           rules={{
                               required: 'Este campo es requerido',
                               validate: validarPorcentajePostulacion
                           }}
                           render={({
                               field: { onChange, value },
                               fieldState: { error },
                           }) => (
                               <CHTextField
                                   percentMask
                                   label="Porcentaje Subsidio *"
                                   error={!!error}
                                   helperText={error ? error.message : null}
                                   InputLabelProps={{ shrink: true }}
                                   value={value}
                                   onChange={onChange}
                              />
                           )}
                       />
                   </Grid>
                   <Grid className={classes.column} item xs={12} md={4}>
                       <Controller
                           name="porcentajeAportes"
                           control={postulantesControl.control}
                           rules={{
                               required: 'Este campo es requerido',
                               validate: validarPorcentajeAportes
                           }}
                           render={({
                               field: { onChange, value },
                               fieldState: { error },
                           }) => (
                               <CHTextField
                                   percentMask
                                   label="Porcentaje Aportes *"
                                   error={!!error}
                                   helperText={error ? error.message : null}
                                   InputLabelProps={{ shrink: true }}
                                   value={value}
                                   onChange={onChange}
                              />
                           )}
                       />
                   </Grid>
                       <Grid className={classes.column} item xs={12} md={4}>
                       <Controller
                           name="porcentajeCesantias"
                           control={postulantesControl.control}
                           rules={{
                               required: 'Este campo es requerido',
                               validate: validarPorcentajeCesantias
                           }}
                           render={({
                               field: { onChange, value },
                               fieldState: { error },
                           }) => (
                               <CHTextField
                                   percentMask
                                   label="Porcentaje Cesantías *"
                                   error={!!error}
                                   helperText={error ? error.message : null}
                                   InputLabelProps={{ shrink: true }}
                                   value={value}
                                   onChange={onChange}
                              />
                           )}
                       />
                   </Grid>
                 </Grid>
              )}
             {mostrarPorcentajes && (
                 <Grid container>
                 <Grid
                     className={classes.columnButtonAdd}
                     item
                     xs={12}
                     md={12}
                 >
                         <Button
                           className={classes.buttonAddEdit}
                           variant="outlined"
                           style={{ marginRight: '10px' }}
                           onClick={postulantesControl.handleSubmit(
                               actualizarEntrevistaPostulantes,
                           )}
                         >
                            ACTUALIZAR PORCENTAJES
                         </Button>
                         <Button
                            className={classes.buttonAdd}
                            variant="outlined"
                            onClick={cancelar}
                         >
                          CANCELAR
                        </Button>
                 </Grid>
             </Grid>
             )}
            </CHPaper>
            <CHPaper title="Observaciones">
            <Grid container>
                    <Grid className={classes.column} item xs={12} md={12}>
                        <Controller
                            name="observacion"
                            control={formularioCalificacion.control}
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
                                    label="Observaciones Adicionales *"
                                    value={value}
                                    onChange={onChange}
                                    isReadOnly={camposCalificacionSoloLectura}
                                    multiline
                                    rows={2}
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
            </CHPaper>
        </CHPrivateContainer>
   </>
 )
}

type props = {
    datosSolicitudPostulacion: any;
    formularioCalificacion: UseFormReturn<any>;
    formularioPostulacion: UseFormReturn<any>;
    formularioEntrevista: UseFormReturn<any>;
    postulantesControl: UseFormReturn<any>;
    listaPostulantes: Postulante[];
    imputabilidadValue: catalogPostulacionDto | null;
    nullImputabilidadValue: boolean;
    camposCalificacionSoloLectura: boolean;
    cliente: any;
    postulantesTable: Postulante [];
    totalPorcentajePostulacion: number;
    totalPorcentajeAportes: number;
    totalPorcentajeCesantias: number;
    mostrarCampoPorcentajeDisminucion: boolean;
    onChangeImputabilidad: (event: any, imputabilidad: catalogPostulacionDto) => void;
    setPostulantesTable: (value: Postulante[]) => void;
    setTotalPorcentajePostulacion: (value: number) => void;
    setTotalPorcentajeAportes: (value: number) => void;
    setTotalPorcentajeCesantias: (value: number) => void;
    consultarCalificacion: (idSolicitud: string) => void;
}

export default Calificacion;