import {Button, 
       createStyles, 
       Grid, 
       IconButton, 
       makeStyles, 
       MenuItem, 
       Table, 
       TableBody, 
       TableCell, 
       TableHead, 
       TableRow, 
       Typography } from '@material-ui/core';
import { Delete, Edit, Visibility } from '@material-ui/icons';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { toast } from 'react-toastify';
import CHDatePicker from '../../../components/ch-datepicker/CHDatepicker';
import CHPaper from '../../../components/ch-paper/CHPaper';
import CHTextField from '../../../components/ch-text-field/CHTextField';
import NotificationService from '../../../services/notification.service';
import { Poliza } from '../constants/fondosolidaridadform.model.type';
import ApiCatalog from '../../../services/api-catalog.service';
import CHSelect from '../../../components/ch-select/CHSelect';
import { ESTADO_PROYECTO } from '../constants/fondoSolidaridad.constants';
import UtilService from '../../../utils/util.service';

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

const PolizaProyecto = (props: Props) => {

    const apiCatalog = ApiCatalog();
    const classes = useStyles();    
    const {informacionProyecto,informacionPolizas,setPoliza,estadoProyecto} = props;
    const [estadoBoton, setEstadoBoton] = useState<string>('AGREGAR PÓLIZA');
    const [anterior, setAnterior] = useState<number>(0);
    const [polizaModel, setPolizaModel] = useState<Poliza[]>([]);
    const [idAnterior, setIdAnterior] = useState<number>(0);
    const [conceptosPoliza, setConceptosPoliza] = useState<any[]>([]);

    const formularioPoliza = useForm({
        defaultValues: {
            concepto: '',
            radicado: '',
            fechaInicioPoliza: new Date(),
            fechaVigencia: new Date(),
            numeroPoliza: '',
            aseguradora: '',
            valorPoliza: '',
            porcentajePoliza: '',
            ajustador: '',
            descripcionPoliza: '',
        },
    });

    useEffect(() => {
        consultarConceptosPoliza();
        if(informacionPolizas && informacionPolizas.length > 0){
            obtenerListaPoliza(informacionPolizas);
        }
    },[])

    const obtenerListaPoliza = (polizas: any[]) =>{

        const arrayPolizas =  polizas.map(item => {
              if(item){
                  const poliza: Poliza = {
                    concepto: item.conceptoId,
                    radicado: item.radicadoPoliza,
                    fechaInicioPoliza: item.fechaInicio,
                    fechaVigencia: item.fechaVigencia,
                    numeroPoliza: item.numeroPoliza,
                    aseguradora: item.aseguradora,
                    valorPoliza: item.valorPolizas,
                    porcentajePoliza: item.porcentaje,
                    ajustador: item.ajustador,
                    descripcionPoliza: item.descripcion,
                  }
                  return poliza;
              }
              return null;
          })
  
        if(arrayPolizas.length > 0){
            setPolizaModel(arrayPolizas as Poliza[]);
        }
        
      }

    const consultarConceptosPoliza = async() => {
        const CONCEPTO_POLIZA = 96;
        await apiCatalog.getObtenerValoresCatalogosPorIdCatalogo(CONCEPTO_POLIZA)
             .then(response => {
                 if(response.data){
                    setConceptosPoliza(response.data);
                 }
             }).catch(() => {
                 toast(`Error al consultar tipos de proyectos`, {type: toast.TYPE.ERROR});
             })
     }

    const agregarPoliza = async (data: any) => {
        const datoEncontrado = polizaModel.findIndex(
            item =>
                item.numeroPoliza === data.numeroPoliza &&
                item.numeroPoliza !== anterior,
        );
        if (datoEncontrado === -1) {
            await agregarRegistroPoliza(data);
        } else {
            toast('La poliza ya existe en la lista', {
                type: toast.TYPE.ERROR,
            });
        }
    };

    const agregarRegistroPoliza = async (data: any) => {

        const polizaNueva: Poliza = {
            concepto: String(data.concepto).toLocaleUpperCase(),
            radicado: data.radicado,
            fechaInicioPoliza: moment(data.fechaInicioPoliza).format('YYYY-MM-DD'),
            fechaVigencia:  moment(data.fechaVigencia).format('YYYY-MM-DD'),
            numeroPoliza: data.numeroPoliza,
            aseguradora: String(data.aseguradora).toLocaleUpperCase(),
            valorPoliza: UtilService.cleanCurrencyValue(data.valorPoliza),
            porcentajePoliza: data.porcentajePoliza,
            ajustador: String(data.ajustador).toLocaleUpperCase(),
            descripcionPoliza: String(data.descripcionPoliza).toLocaleUpperCase(),
        };
        let polizas: Poliza[] = [];
        if (estadoBoton === 'AGREGAR PÓLIZA') {

            setPolizaModel(oldArray => [
                ...oldArray,
                polizaNueva,
            ]);
            polizas = [...polizaModel];
            polizas.push(polizaNueva);
        } else {
            const temp = [...polizaModel];
            const posicionActual = temp.findIndex(
                item => item.numeroPoliza === anterior,
            );

            polizaNueva.numeroPoliza = temp[posicionActual].numeroPoliza;

            if (
                Number(polizaNueva.numeroPoliza) !==
                temp[posicionActual].numeroPoliza
            ) {
                temp[posicionActual].numeroPoliza =
                polizaNueva.numeroPoliza;
            } else {
                polizaNueva.numeroPoliza = idAnterior;
            }
            temp.splice(posicionActual, 1);
            temp.splice(posicionActual, 0, polizaNueva);
            polizas = temp;
            setPolizaModel(temp);
        }
        setPoliza(polizas);
        setAnterior(0);
        resetPolizaNueva();
    };

    const seleccionarRegistro = async (item: Poliza) => {
        setAnterior(item.numeroPoliza);
        setIdAnterior(item.numeroPoliza);
        formularioPoliza.setValue('concepto', item.concepto);
        formularioPoliza.setValue('radicado', String(item.radicado));
        formularioPoliza.setValue('fechaInicioPoliza', new Date(item.fechaInicioPoliza));
        formularioPoliza.setValue('fechaVigencia', new Date(item.fechaVigencia));
        formularioPoliza.setValue('numeroPoliza', String(item.numeroPoliza));
        formularioPoliza.setValue('aseguradora', item.aseguradora);
        formularioPoliza.setValue('valorPoliza', String(item.valorPoliza));
        formularioPoliza.setValue('porcentajePoliza', String(item.porcentajePoliza));
        formularioPoliza.setValue('ajustador', item.ajustador);
        formularioPoliza.setValue('descripcionPoliza', item.descripcionPoliza);
        setEstadoBoton('EDITAR PÓLIZA');
    };

    const eliminarRegistro = async (numero: number) => {
        const confirmAlert = await NotificationService.confirmAlert(
            '¿Eliminando registro?',
        );
        if (confirmAlert) {
            const temp = [...polizaModel];
            temp.splice(
                temp.findIndex(item => item.numeroPoliza === numero),
                1,
            );
            setPolizaModel(temp);
            setPoliza(temp);
            await NotificationService.genericAlert(
                'Eliminado',
                'Se eliminó con éxito el registro.',
            );
            resetPolizaNueva();
        }
    };

    const resetPolizaNueva = async () => {
        setEstadoBoton('AGREGAR PÓLIZA');
        formularioPoliza.clearErrors();
        formularioPoliza.setValue('concepto', '');
        formularioPoliza.setValue('radicado', '');
        formularioPoliza.setValue('fechaInicioPoliza', new Date());
        formularioPoliza.setValue('fechaVigencia', new Date());
        formularioPoliza.setValue('numeroPoliza', '');
        formularioPoliza.setValue('aseguradora', '');
        formularioPoliza.setValue('valorPoliza', '');
        formularioPoliza.setValue('porcentajePoliza', '');
        formularioPoliza.setValue('ajustador', '');
        formularioPoliza.setValue('descripcionPoliza', '');
    };

    return (
        <CHPaper title="Información Póliza">
                      <Grid container>
                      <Grid className={classes.column} item xs={12} md={3}>
                            <Controller
                                  name="concepto"
                                  control={formularioPoliza.control}
                                  rules={{
                                      required: 'Este campo es requerido',
                                  }}
                                  render={({
                                      field: { onChange, value },
                                      fieldState: { error },
                                  }) => (
                                      <CHSelect
                                          readOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}
                                          error={!!error}
                                          helperText={error ? error.message : null}
                                          value={value}
                                          onChange={onChange}
                                          label="Concepto Póliza *"
                                      >
                                          {
                                              conceptosPoliza.map((option: any) => (
                                                  <MenuItem key={option.idValor}
                                                            value={option.idValor}
                                                  >
                                                      {option.descripcion}
                                                  </MenuItem>
                                              ))
                                          }
                                         
                                  </CHSelect>
                              )}
                          /> 
                      </Grid>
                      <Grid className={classes.column} item xs={12} md={3}>
                      <Controller
                              name="radicado"
                              control={formularioPoliza.control}
                              rules={{
                                  required: 'Este campo es requerido.',
                              }}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHTextField
                                      isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      label="Radicado Póliza *"
                                      error={!!error}
                                      helperText={error ? error.message : null}
                                      InputLabelProps={{ shrink: true }}
                                      value={value}
                                      inputProps={{
                                          style: {
                                              textTransform: 'uppercase',
                                          },
                                      }}
                                      onChange={e => {
                                          if(e.target.value.length <= 20){
                                              onChange(
                                                  e.target.value.replace(
                                                      /[#./?=!@$%^&*^)(+=_}{":;><|~` ]/g,
                                                      '',
                                                  ),
                                              );
  
                                          }
                                       
                                      }}
                                  />
                              )}
                          />
                      </Grid>
                      <Grid className={classes.column} item xs={12} md={3}>
                          <Controller
                              name="fechaInicioPoliza"
                              rules={{
                                  required: 'Este campo es requerido.',
                              }}
                              control={formularioPoliza.control}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHDatePicker
                                      label="Fecha de Inicio Póliza *"
                                      InputLabelProps={{ shrink: true}}
                                      onChange={onChange}
                                      value={value}
                                      error={!!error}
                                      inputProps={{readOnly: estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}}
                                      helperText={error ? error.message : null}
                                      readOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                               (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      disableFuture
                                      autoOk
                                  />
                              )}
                          />
                   </Grid>
                   <Grid className={classes.column} item xs={12} md={3}>
                          <Controller
                              name="fechaVigencia"
                              rules={{
                                  required: 'Este campo es requerido.',
                              }}
                              control={formularioPoliza.control}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHDatePicker
                                      label="Fecha de Vigencia *"
                                      InputLabelProps={{ shrink: true}}
                                      onChange={onChange}
                                      value={value}
                                      error={!!error}
                                      inputProps={{readOnly: estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}}
                                      helperText={error ? error.message : null}
                                      readOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                               (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      disableFuture
                                      autoOk
                                  />
                              )}
                          />
                   </Grid>
                   <Grid className={classes.column} item xs={12} md={3}>
                      <Controller
                              name="numeroPoliza"
                              control={formularioPoliza.control}
                              rules={{
                                  required: 'Este campo es requerido.',
                              }}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHTextField
                                      numberMask
                                      isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      label="Número Póliza *"
                                      error={!!error}
                                      helperText={error ? error.message : null}
                                      InputLabelProps={{ shrink: true }}
                                      value={value}
                                      onChange={onChange}
                                  />
                              )}
                          />
                      </Grid>
                      <Grid className={classes.column} item xs={12} md={3}>
                      <Controller
                              name="aseguradora"
                              control={formularioPoliza.control}
                              rules={{
                                  required: 'Este campo es requerido',
                              }}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHTextField
                                      isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      error={!!error}
                                      helperText={error ? error.message : null}
                                      label="Aseguradora *"
                                      value={value}
                                      InputLabelProps={{ shrink: true }}
                                      inputProps={{
                                          style: {
                                              textTransform: 'uppercase',
                                          },
                                      }}
                                      onChange={e => {
                                          if(e.target.value.length <= 20){
                                              onChange(
                                                  e.target.value.replace(
                                                      /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                      '')
                                              );
                                          }
                                      }}
                                  />
                              )}
                          />
                  </Grid>
                  <Grid className={classes.column} item xs={12} md={3}>
                      <Controller
                              name="valorPoliza"
                              control={formularioPoliza.control}
                              rules={{
                                  required: 'Este campo es requerido.',
                              }}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHTextField
                                      currencyMask
                                      isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      label="Valor Póliza *"
                                      error={!!error}
                                      helperText={error ? error.message : null}
                                      InputLabelProps={{ shrink: true }}
                                      inputProps={{
                                          style: {
                                              textAlign: 'right',
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
                              name="porcentajePoliza"
                              control={formularioPoliza.control}
                              rules={{
                                  required: 'Este campo es requerido.',
                                  validate: () => formularioPoliza.watch('porcentajePoliza') === '0%'? 'El porcentaje debe ser mayor a cero' : true
                              }}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHTextField
                                      percentMask
                                      isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      label="Porcentaje Póliza *"
                                      error={!!error}
                                      helperText={error ? error.message : null}
                                      InputLabelProps={{ shrink: true }}
                                      value={value}
                                      onChange={onChange}
                                  />
                              )}
                          />
                      </Grid>
                      <Grid className={classes.column} item xs={12} md={3}>
                      <Controller
                              name="ajustador"
                              control={formularioPoliza.control}
                              rules={{
                                  required: 'Este campo es requerido',
                              }}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHTextField
                                      isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      error={!!error}
                                      helperText={error ? error.message : null}
                                      label="Ajustador *"
                                      value={value}
                                      InputLabelProps={{ shrink: true }}
                                      inputProps={{
                                          style: {
                                              textTransform: 'uppercase',
                                          },
                                      }}
                                      onChange={e => {
                                          if(e.target.value.length <= 20){
                                              onChange(
                                                  e.target.value.replace(
                                                      /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                      '')
                                              );
                                          }
                                      }}
                                  />
                              )}
                          />
                  </Grid>
                  <Grid className={classes.column} item xs={12} md={9}>
                      <Controller
                              name="descripcionPoliza"
                              control={formularioPoliza.control}
                              rules={{
                                  required: 'Este campo es requerido',
                              }}
                              render={({
                                  field: { onChange, value },
                                  fieldState: { error },
                              }) => (
                                  <CHTextField
                                      isReadOnly={estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                    (informacionProyecto && informacionProyecto?.polizas !== null)}
                                      error={!!error}
                                      helperText={error ? error.message : null}
                                      label="Descipción Póliza *"
                                      value={value}
                                      InputLabelProps={{ shrink: true }}
                                      inputProps={{
                                          style: {
                                              textTransform: 'uppercase',
                                          },
                                      }}
                                      onChange={e => {
                                          if(e.target.value.length <= 90)
                                          onChange(
                                              e.target.value.replace(
                                                  /[^a-zA-ZñÑáéíóúÁÉÍÓÚ\s]/g,
                                                  '')
                                          );
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
                    {(informacionProyecto && informacionProyecto?.polizas === null) && (
                     <Button
                         className={classes.buttonAddEdit}
                         variant="outlined"
                         onClick={formularioPoliza.handleSubmit(agregarPoliza)}
                     >
                         {estadoBoton}
                     </Button>
                    )}
                     <Button
                         className={classes.buttonAdd}
                         variant="outlined"
                         onClick={resetPolizaNueva}
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
                    Lista de Pólizas
                </Typography>
                <Table aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Número de Póliza</TableCell>
                            <TableCell>Radicado Póliza</TableCell>
                            <TableCell>Aseguradora</TableCell>
                            <TableCell>Fecha Vigencia</TableCell>
                            <TableCell>Valor</TableCell>
                            <TableCell>Porcentaje</TableCell>
                            <TableCell>Ajustador</TableCell>
                            <TableCell align="center">
                                Acciones
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {polizaModel.map(
                            (option: Poliza) => (
                                <TableRow key={option.numeroPoliza}>
                                    <TableCell>{option.numeroPoliza}</TableCell>
                                    <TableCell>{option.radicado}</TableCell>
                                    <TableCell>{option.aseguradora}</TableCell>
                                    <TableCell>{moment(option.fechaVigencia).format('YYYY-MM-DD')}</TableCell>
                                    <TableCell>{UtilService.formatCurrency(option.valorPoliza)}</TableCell>
                                    <TableCell>{String(option.porcentajePoliza).includes('%') ? option.porcentajePoliza : `${option.porcentajePoliza}%`}</TableCell>
                                    <TableCell>{option.ajustador}</TableCell>
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
                                            {   (estadoProyecto !== ESTADO_PROYECTO.EN_CREACION || 
                                                (informacionProyecto && informacionProyecto?.polizas !== null)) &&(
                                                    <Visibility />
                                            )}
                                            {   (informacionProyecto && informacionProyecto?.polizas === null) && (
                                                    <Edit />
                                            )}
                                        </IconButton>
                                        {(informacionProyecto && informacionProyecto?.polizas === null) && (
                                                <IconButton
                                                    aria-label="Eliminar"
                                                    color="primary"
                                                    size="small"
                                                    onClick={() => {
                                                        eliminarRegistro(
                                                            option.numeroPoliza,
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
    informacionPolizas: any[];
    setPoliza: (value: Poliza[]) => void;
    estadoProyecto: number;
}

export default PolizaProyecto;