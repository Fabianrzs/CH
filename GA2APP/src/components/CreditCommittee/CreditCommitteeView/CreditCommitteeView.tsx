import React, { useEffect, useState } from 'react';
import {
    Button,
    Grid,
    MenuItem,
    Table,
    TableCell,
    TableHead,
    TableRow,
    TableBody,
} from '@material-ui/core';
import { createStyles, makeStyles } from '@material-ui/core/styles';
import { Controller, useForm } from 'react-hook-form';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import CHPageTitle from '../../ch-page-title/CHPageTitle';
import CHPaper from '../../ch-paper/CHPaper';
import CHTextField from '../../ch-text-field/CHTextField';
import CHDatePicker from '../../ch-datepicker/CHDatepicker';
import CHSelect from '../../ch-select/CHSelect';
import { CreditCommitteeModel } from '../../../models/credit-committee.model';
import { AppState } from '../../../store/rootReducer';
import {
    startGetDetailCommittee,
    startPostCreateUpdatedCommittee,
} from '../../../actions/credit.actions';
import CHLoadingProcess from '../../ch-loading-process/CHLoadingProcess';
import { CreateUpdateCommittee } from '../../../models/requests/create-update-committee-request.model';
import UtilService from '../../../utils/util.service';
import CHTimepicker from '../../ch-timepicker/CHTimepicker';
import ApiCredit from '../../../services/api-credit.service';
import CreditCommitteeRequest from '../CreditCommitteeRequest/CreditCommitteeRequest';
import NotificationService from '../../../services/notification.service';
import AuthService from '../../../services/auth.service';
import { ModalCrearInvitado } from '../Modal/ModalCrearInvitado';


const useStyles = makeStyles(() =>
    createStyles({
        column: {
            padding: '10px',
            textAlign: 'center',
            alignItems: 'flex-end',
            justifyContent: 'center',
            display: 'flex',
        },
        color: {
            color:'#061311'
        }
    }),
);

function CreditCommitteeView({
    creditCommittee,
    isLoading,
    startGetDetailCommittee,
    listTopics,
    listMembers,
    onItemGoBack,
    currentMembers,
    currentTopics,
    startPostCreateUpdatedCommittee,
    listaSolicitudes,
    obtenerTramitesPendientes,
    mostrarFormularioComite,
    mostrarListaSolicitudes,
    mostrarBotonProcesar,
    setMostrarFormularioComite,
    setMostrarListaSolicitudes,
    setMostrarBotonProcesar
}: {
    creditCommittee?: CreditCommitteeModel;
    isLoading: boolean;
    startGetDetailCommittee: (creditCommittee?: CreditCommitteeModel) => void;
    listTopics: any[];
    listMembers: any[];
    currentTopics: any[];
    currentMembers: any[];
    onItemGoBack: () => void;
    startPostCreateUpdatedCommittee: (request: CreateUpdateCommittee) => void;
    listaSolicitudes: any;
    obtenerTramitesPendientes: () => void;
    mostrarFormularioComite: boolean;
    mostrarListaSolicitudes: boolean;
    mostrarBotonProcesar: boolean;
    setMostrarFormularioComite: (value: boolean) => void;
    setMostrarListaSolicitudes: (value: boolean) => void;
    setMostrarBotonProcesar: (value: boolean) => void;
}) {

    const apiCredit = ApiCredit();
    const array = listaSolicitudes;
    const { control, setValue, handleSubmit, reset } = useForm({
        defaultValues: {
            fecha: null,
            lugar: '',
            horaInicio: new Date(),
            horaFinalizacion: new Date(),
            desarrollo: '',
            temasAprobacion: '',
            anotacion: '',
            cargoAnalista: '',
        },
    });
    const [isLoaded, setIsLoaded] = useState(false);
    const [attendees, setAttendees] = useState<any[]>([]);
    const [topics, setTopics] = useState<any[]>([]);
    const [listaInvitados, setInListavIntados] = useState<any[]>([]);
    const [listaAsistentes, setInListaAsistentes] = useState<any[]>([]);

    const [invitados, setInvitados] = useState<any[]>([]);
    const [informacionComite, setInformacionComite] = useState<any>({});
    const [openModal,setOpenModal] = useState(false);
  

    const enabledToEdit =
        creditCommittee == null || creditCommittee?.codigoEstado !== 'A';

    const {
        control: attendeesControl,
        handleSubmit: attendeesHandle,
        setValue: attendeeValue,
       
         } = useForm({
        defaultValues: {
            attendee: '',
        },
    });

   
    const user = AuthService.getUserInfo();



    const {
        control: topicsControl,
        handleSubmit: topicHandle,
        setValue: topicValue,
        reset: reset2
    } = useForm({
        defaultValues: {
            descripcionTema: '',
            observacion: '',
          
        },
    });
    const formularioInvitado = useForm({
        defaultValues: {
            attendee: '',
           
        },
    });
    const classes = useStyles();

    useEffect(() => {
        
        if (creditCommittee != null) {
            startGetDetailCommittee(creditCommittee);
        } else {
            startGetDetailCommittee();
        }
        setInformacionComite({});

       
    }, []);

    if (!isLoaded && creditCommittee != null) {
        setIsLoaded(true);
        const fields = Object.keys(creditCommittee);
        fields.forEach(field => {
            let value: any =
                creditCommittee[field as keyof CreditCommitteeModel];
            if (field === 'horaInicio' || field === 'horaFinalizacion') {
                value = UtilService.getTimeFromString(value);
            }
            return setValue(field as any, value);
        });
    }

  




    useEffect(() => {
        
        if (attendees.length === 0) {
          const modificarAsistentes =  currentMembers.filter((item: any) => item.tipo === 'Asistente')
            setAttendees(modificarAsistentes);
        }
        if (attendees.length === 0) {
            const modificarInvitados =  currentMembers.filter((item: any) => item.tipo === 'Invitado')
              setInvitados(modificarInvitados);
          }

   
        if (topics.length === 0) {
            setTopics(currentTopics);
        }
        obtenerListadoInvitados();
    
    }, [currentMembers, currentTopics]);

    useEffect(()=> {
            obtenerListadoInvitados();
    }, [openModal])

    
     
    const addAttendeesRow = (data: any) => {
        if(data.attendee === 'SELECCIONE'){
            return;
        }
        
  const existeAttendee = attendees.some((x:any) => x.idIntegranteComite === data.attendee)
  if(existeAttendee){
    toast('El asistente ya existe... ', { type: toast.TYPE.ERROR });
    return
  }

        const member =  listMembers.find(item => item.idIntegranteComite === data.attendee);
        setAttendees(prevAttendees => [...prevAttendees, member]);
        attendeeValue('attendee','SELECCIONE');
    };

    const removeAttendeesRow = (idIntegranteComite: any) => () => {
        
        setAttendees(prevAttendees => [
            ...prevAttendees.filter(
                item => item.idIntegranteComite !== idIntegranteComite,
            ),
        ]);
    };

    const addTopicRow = (data: any) => {
        if(data.descripcionTema === 'SELECCIONE'){
            return;
        }

        const tema = listTopics.find(item => item.idTemaComite === data.descripcionTema);
        const dataAdapted = {
            ...data.descripcionTema,
            observacion: data.observacion,
            descripcionTema: tema.descripcion,
            idAsistente:data.idIntegranteComite
        };
        setTopics(prevTopics => [...prevTopics, dataAdapted]);
        topicValue('descripcionTema','SELECCIONE');
        topicValue('observacion','');
    };

    const removeTopicRow = (data: any) => () => {
        setTopics(prevTopics => [...prevTopics.filter(item => item !== data)]);
    };



  const obtenerListadoInvitados = async() => {
    await apiCredit 
    .getObtnerInvitadosComite()
    .then(async response => {
        if (response) {
            if (response.isSuccess) {
                setInListavIntados(response.data.filter((item:any) => item.tipoIntegrante === 'Invitado'))
                setInListaAsistentes(response.data.filter((item:any) => item.tipoIntegrante === 'Asistente'))
               
            }
         
        }
    
    })
    .catch(() => {
        toast('Error del sistema', { type: toast.TYPE.ERROR });
    });
  
  }

   

    const agregarInvitado = (data: any) => {
    
        if(data.attendee === 'SELECCIONE'){
            return;
        }

        const existeInvitado = invitados.some((x:any) => x.idIntegranteComite === data.attendee)
  if(existeInvitado){
    toast('El invitado ya existe... ', { type: toast.TYPE.ERROR });
    return
  }

        const dataInvitado =  listaInvitados.find(item => item.idIntegranteComite === data.attendee); 
        setInvitados(previoInvitado => [...previoInvitado, dataInvitado]);
        formularioInvitado.setValue('attendee','SELECCIONE');
     
        
    };


    const eliminarInvitado = (nombreInvitado: any) => () => {
        
        setInvitados(previoInvitado => [
            ...previoInvitado.filter(
                item => item !== nombreInvitado,
            ),
        ]);
    };

    const crearInvitado = () => {
        setOpenModal(true)
    }
   const cerrarModal = () => {
       setOpenModal(false)
   }
   
    const handleServer = (data: any) => {
        invitados.forEach(item => {
            attendees.push({'idIntegranteComite' : item.idIntegranteComite });
       });
      
          

          
           let infoCommittee;
        if (creditCommittee != null) {


            infoCommittee = {
                ...creditCommittee,
                fecha: UtilService.getAlternativeDate(data.fecha),
                lugar: data.lugar,
                horaInicio: UtilService.getTime(data.horaInicio),
                horaFinalizacion: UtilService.getTime(data.horaFinalizacion),
                desarrollo: data.desarrollo,
                temasAprobacion: data.temasAprobacion,
                anotacion: data.anotacion,
                cargoAnalista: data.cargoAnalista,
                comiteIntegrantesDto: attendees.map((value, index) => ({
                  
                    idAsistente: value.idIntegranteComite,
                    orden: index + 1,
                })),

             
            
             
            
                temasComiteCreditoDto: topics.map((value, index) => ({
                   
                    orden: index + 1,
                    observacion: value.observacion,
                })),
            };
        } else {
 
       
        
            infoCommittee = {
                creadoPor: user.USR_ID,
                fecha: UtilService.getAlternativeDate(data.fecha),
                lugar: data.lugar,
                horaInicio: UtilService.getTime(data.horaInicio),
                horaFinalizacion: UtilService.getTime(data.horaFinalizacion),
                desarrollo: data.desarrollo,
                temasAprobacion: data.temasAprobacion,
                anotacion: data.anotacion,
                cargoAnalista: data.cargoAnalista,
                comiteIntegrantesDto:attendees.map((value, index) => ({
                codigoTema: value.idTemaComite,
                    idAsistente: value.idIntegranteComite,
                    orden: index + 1,
                })),
                
                temasComiteCreditoDto: topics.map((value, index) => ({
                    codigoTema: value.idTemaComite,
                    orden: index + 1,
                    observacion: value.observacion,
                })),
            };
        }

        crearComiteCredito(infoCommittee);
       
    };

    const crearComiteCredito = async (informacionComite: any) => {
        await apiCredit
            .postCreateCommittee(informacionComite)
            .then(async response => {
                if (response) {
                    if (response.isSuccess && response.data) {
                        setInformacionComite(response.data);
                        obtenerTramitesPendientes();
                        await NotificationService.genericAlert(
                            'Guardado con éxito ',
                            'Se creó comité',
                        );
              
                        setMostrarListaSolicitudes(true);
                        setMostrarBotonProcesar(true);
                      

                    }
                 
                }
            
            })
            .catch(() => {
                toast('Error del sistema', { type: toast.TYPE.ERROR });
            });
    };



    if (isLoading) {
        return <CHLoadingProcess />;
    }

    return (
        <>
        {mostrarFormularioComite && (
            <div> 
             <CHPageTitle
             topText="Consulta Comité Crédito"
             subText=""
             title1="Comite "
             title2="Credito"
         />
         <CHPaper title="Informacion">
             <Grid container>
                 <Grid item md={3} className={classes.column}>
                     <Controller
                         name="fecha"
                         control={control}
                         rules={{
                             required: 'Este campo es requerido.',
                         }}
                         render={({
                             field: { onChange, value },
                             fieldState: { error },
                         }) => (
                             <CHDatePicker
                                 error={!!error}
                                 helperText={error ? error.message : null}
                                 label="Fecha *"
                                 onChange={onChange}
                                 value={value}
                             />
                         )}
                     />
                 </Grid>
                 <Grid item md={3} className={classes.column}>
                     <Controller
                         name="lugar"
                         control={control}
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
                                 isReadOnly={false}
                                 label="Lugar *"
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
                 <Grid item md={3} className={classes.column}>
                     <Controller
                         name="horaInicio"
                         control={control}
                         rules={{
                             required: 'Este campo es requerido.',
                         }}
                         render={({
                             field: { onChange, value },
                             fieldState: { error },
                         }) => (
                             <CHTimepicker
                                 error={!!error}
                                 helperText={error ? error.message : null}
                                 label="Hora Inicio"
                                 value={value}
                                 onChange={onChange}
                             />
                         )}
                     />
                 </Grid>
                 <Grid item md={3} className={classes.column}>
                     <Controller
                         name="horaFinalizacion"
                         control={control}
                         rules={{
                             required: 'Este campo es requerido.',
                         }}
                         render={({
                             field: { onChange, value },
                             fieldState: { error },
                         }) => (
                             <CHTimepicker
                                 error={!!error}
                                 helperText={error ? error.message : null}
                                 label="Hora Finalización"
                                 value={value}
                                 onChange={onChange}
                             />
                         )}
                     />
                 </Grid>
             </Grid>
         </CHPaper>
         <CHPaper title="Asistentes">
             <Grid container>
                 <Grid item md={4} className={classes.column}>
                     <Controller
                         name="attendee"
                         control={attendeesControl}
                         rules={{
                             required: 'Este campo es requerido',
                         }}
                         render={({
                             field: { onChange, value },
                             fieldState: { error },
                         }) => (
                             <CHSelect
                                 error={!!error}
                                 label="Asistente *"
                                 value={value}
                                 helperText={error ? error.message : null}
                                 onChange={onChange}
                             >
                                 <MenuItem className={classes.color} value="SELECCIONE">SELECCIONE</MenuItem>
                                 {listaAsistentes?.map((member, index) => (
                                     <MenuItem
                                         className={classes.color}
                                         key={`member-${index}`}
                                         value={member.idIntegranteComite}
                                     >
                                         {member.nombre}
                                 
                                     </MenuItem>
                                 ))}
                             </CHSelect>
                         )}
                     />
                 </Grid>
                 <Grid item md={4} className={classes.column}>
                     <Button 
                     onClick={attendeesHandle(addAttendeesRow)}
                     style={{ backgroundColor: '#a9a9a9'}}
                     >
                         Agregar Asistente
                     </Button>
                 </Grid>
             </Grid>
             <Table>
                 {attendees.length > 0 && (
                     <TableHead>
                         <TableRow>
                             <TableCell>Nombre</TableCell>
                             <TableCell>Cargo</TableCell>
                             <TableCell />
                         </TableRow>
                     </TableHead>
                 )}
                 <TableBody>
                     {attendees.map((attendee, index) => (
                         <TableRow key={`attende-list-${index}`}>
                             <TableCell>{attendee.nombre}</TableCell>
                             <TableCell>{attendee.cargo}</TableCell>
                             
                             <TableCell>
                                 {enabledToEdit && (
                                     <Button
                                         onClick={removeAttendeesRow(
                                             attendee.idIntegranteComite,
                                         )}
                                         style={{ backgroundColor: '#a9a9a9'}}

                                     >
                                         Eliminar
                                     </Button>
                                 )}
                             </TableCell>
                         </TableRow>
                     ))}
                 </TableBody>
             </Table>
         </CHPaper>


        

         <CHPaper title="Invitados">
             <Grid container>
             <Grid item md={4} className={classes.column}>
                     <Controller
                         name="attendee"
                         control={formularioInvitado.control}
                         rules={{
                             required: 'Este campo es requerido',
                         }}
                         render={({
                             field: { onChange, value },
                             fieldState: { error },
                         }) => (
                             <CHSelect
                                 error={!!error}
                                 label="Nombre Invitado *"
                                 value={value}
                                 helperText={error ? error.message : null}
                                 onChange={onChange}
                             >
                                 <MenuItem className={classes.color} value="SELECCIONE">SELECCIONE</MenuItem>
                                 {
                             
                             listaInvitados?.map((dataInvitado, index) => (
                                     <MenuItem
                                         className={classes.color}
                                         key={`dataInvitado-${index}`}
                                         value={dataInvitado.idIntegranteComite}
                                     >
                                         {dataInvitado.nombre}
                                 
                                     </MenuItem>
                                 ))}
                             </CHSelect>
                         )}
                     />
                 </Grid>
                 <Grid item md={4} className={classes.column}>
                     <Button 
                     onClick={formularioInvitado.handleSubmit(agregarInvitado)}
                     style={{ backgroundColor: '#a9a9a9'}}
                     >
                         Agregar Invitados
                     </Button>
                 </Grid>
                 <Grid item md={4} className={classes.column}>
                    <Button
                    onClick={crearInvitado} 
                    style={{ backgroundColor: '#a9a9a9'}}
                    >
                        Crear Invitado
                    </Button>
                 </Grid>
                 
             </Grid>
             <Table>
                 {invitados?.length > 0 && (
                     <TableHead>
                         <TableRow>
                             <TableCell>Nombre</TableCell>
                             <TableCell>Detalle</TableCell>
                             <TableCell />
                         </TableRow>
                     </TableHead>
                 )}
                 <TableBody>
                     {invitados?.map((invitado, index) => (
                         <TableRow key={`invitado-list-${index}`}>
                             <TableCell>{invitado?.nombre}</TableCell>
                             <TableCell>{invitado?.cargo}</TableCell>
                             <TableCell>
                                 {enabledToEdit && (
                                     <Button
                                      onClick={eliminarInvitado(invitado)}
                                      style={{ backgroundColor: '#a9a9a9'}}
                                      >
                                         Eliminar
                                     </Button>
                                 )}
                             </TableCell>
                         </TableRow>
                     ))}
                 </TableBody>
             </Table>
         </CHPaper>

        
         <CHPaper title="Orden del Dia">
             <Grid container>
                 <Grid item md={4} className={classes.column}>
                     <Controller
                         name="descripcionTema"
                         control={topicsControl}
                         rules={{
                             required: 'Este campo es requerido.',
                         }}
                         render={({
                             field: { onChange, value },
                             fieldState: { error },
                         }) => (
                             <CHSelect
                                 error={!!error}
                                 label="Tema *"
                                 value={value}
                                 helperText={error ? error.message : null}
                                 onChange={onChange}
                             >
                                 <MenuItem value="SELECCIONE">SELECCIONE</MenuItem>
                                 {listTopics.map((topic, index) => (
                                     <MenuItem
                                         key={`topic-${index}`}
                                         value={topic.idTemaComite}
                                     >
                                         {topic.descripcion}
                                     </MenuItem>
                                 ))}
                             </CHSelect>
                         )}
                     />
                 </Grid>
                 <Grid item md={4} className={classes.column}>
                     <Controller
                         name="observacion"
                         control={topicsControl}
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
                                 isReadOnly={false}
                                 label="Detalle *"
                                 value={value}
                                 onChange={onChange}
                                 inputProps={{
                                     style: {
                                         textTransform: 'uppercase',
                                     },
                                 }}
                             />
                         )}
                     />
                 </Grid>
                 <Grid item md={4} className={classes.column}>
                     <Button 
                     onClick={topicHandle(addTopicRow)}
                     style={{ backgroundColor: '#a9a9a9'}}
                     >
                         Agregar
                     </Button>
                 </Grid>
             </Grid>
             <Table>
                 {topics.length > 0 && (
                     <TableHead>
                         <TableRow>
                             <TableCell>Tema</TableCell>
                             <TableCell>Observación</TableCell>
                             <TableCell />
                         </TableRow>
                     </TableHead>
                 )}
                 <TableBody>
                     {topics.map((topic, index) => (
                         <TableRow key={`topic-list-${index}`}>
                             <TableCell>{topic.descripcionTema}</TableCell>
                             <TableCell>{topic.observacion}</TableCell>
                             <TableCell>
                                 {enabledToEdit && (
                                     <Button 
                                     onClick={removeTopicRow(topic)}
                                     style={{ backgroundColor: '#a9a9a9'}}
                                     >
                                         Eliminar
                                     </Button>
                                 )}
                             </TableCell>
                         </TableRow>
                     ))}
                 </TableBody>
             </Table>
         </CHPaper>
         <CHPaper title="Detalle">
             <Grid container>
                 <Grid item md={12} className={classes.column}>
                     <Controller
                         name="desarrollo"
                         control={control}
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
                                 isReadOnly={false}
                                 label="Desarrollo Reunión *"
                                 value={value}
                                 multiline
                                 maxRows={4}
                                 onChange={onChange}
                                 inputProps={{
                                     style: {
                                         textTransform: 'uppercase',
                                     },
                                 }}
                             />
                         )}
                     />
                 </Grid>
                 <Grid item md={12} className={classes.column}>
                     <Controller
                         name="temasAprobacion"
                         control={control}
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
                                 isReadOnly={false}
                                 label="Temas Aprobación *"
                                 value={value}
                                 multiline
                                 maxRows={4}
                                 onChange={onChange}
                                 inputProps={{
                                     style: {
                                         textTransform: 'uppercase',
                                     },
                                 }}
                             />
                         )}
                     />
                 </Grid>
                 <Grid item md={12} className={classes.column}>
                     <Controller
                         name="anotacion"
                         control={control}
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
                                 isReadOnly={false}
                                 label="Anotación *"
                                 value={value}
                                 multiline
                                 maxRows={4}
                                 onChange={onChange}
                                 inputProps={{
                                     style: {
                                         textTransform: 'uppercase',
                                     },
                                 }}
                             />
                         )}
                     />
                 </Grid>
                 <Grid item md={5} className={classes.column}>
                     <Controller
                         name="cargoAnalista"
                         control={control}
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
                                 isReadOnly={false}
                                 label="Cargo del Funcionario *"
                                 value={value}
                                 multiline
                                 maxRows={4}
                                 onChange={onChange}
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

                 <Grid item md={8} className={classes.column}>
                     {creditCommittee?.codigoEstado === 'C' && (
                         <Button 
                         onClick={handleSubmit(handleServer)}
                         style={{ backgroundColor: '#a9a9a9',marginLeft:"50%"}}
                         >
                            
                             Actualizar
                         </Button>
                     )}
                     {creditCommittee == null && (
                         <Button 
                         onClick={handleSubmit(handleServer)}
                         style={{ backgroundColor: '#a9a9a9',marginLeft:"50%"}}
                         >

                             Crear acta de comité
                         </Button>
                     )}

                 </Grid>
                 {/* <Grid item md={4} className={classes.column}>


                     <Button 
                     onClick={onItemGoBack}
                     style={{ backgroundColor: '#a9a9a9'}}
                     >
                        Regresar
                     </Button>
                 </Grid> */}
             </Grid>
         </CHPaper>
         <ModalCrearInvitado
                open={openModal}
                cerrarModal= {cerrarModal}
              
            />
         </div>
         
        )}
        {/* {mostrarListaSolicitudes && listaSolicitudes.length > 0 && ( */}
            <CreditCommitteeRequest 
                creditCommittee={informacionComite} 
                onItemGoBack={onItemGoBack} 
                listaSolicitudes={array} 
                obtenerTramitesPendientes={obtenerTramitesPendientes}
                setMostrarFormularioComite={setMostrarFormularioComite}
                setMostrarListaSolicitudes={setMostrarListaSolicitudes}
                setMostrarBotonProcesar={setMostrarBotonProcesar}
                mostrarFormularioComite={mostrarFormularioComite}
                mostrarListaSolicitudes={mostrarListaSolicitudes}
                mostrarBotonProcesar={mostrarBotonProcesar}
         />
         {/* )} */}
        </>
    );
}

const mapStateToProps = (state: AppState) => ({
    isLoading: state.credit.loading,
    listTopics: state.credit.listTopics,
    listMembers: state.credit.listMembers,
    currentTopics: state.credit.currentListTopics,
    currentMembers: state.credit.currentListMembers,
    error: state.credit.error,
});

const mapDispatchToProps = {
    startGetDetailCommittee,
    startPostCreateUpdatedCommittee,
};

export default connect(
    mapStateToProps,
    mapDispatchToProps,
)(CreditCommitteeView);
